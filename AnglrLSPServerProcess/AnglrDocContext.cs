using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Threading;
using Microsoft.VisualStudio.LanguageServer.Protocol;
using Anglr.Parser;
using Anglr.Parser.Core;
using Anglr.Parser.SyntaxTree;
using Anglr.Compiler;
using AnglrLibrary;
using AnglrJsonRpcMethods;
using Anglr.Declarations;
using System.Data.Common;
using Newtonsoft.Json;
using System.Xml.Linq;
using System.Security.Cryptography;
using System.Net;
using System.IO;
using AnglrParserLibrary;
using Newtonsoft.Json.Linq;
using AnglrLogLibrary;

namespace AnglrLSPServerProcess
{
    using static Anglr.Declarations.AnglrDeclarations;
    using Range = Microsoft.VisualStudio.LanguageServer.Protocol.Range;

    internal class textChanges : Dictionary<string, TextEdit []>
    {
    }

    internal class editlist : Queue<TextDocumentContentChangeEvent>
    {

    }

    internal class CmpHierarchyItem : IComparer<AnglrGetGetHierarchyItemData>
    {
        public int Compare (AnglrGetGetHierarchyItemData x, AnglrGetGetHierarchyItemData y)
        {
            return x.ItemName.CompareTo (y.ItemName);
        }
    }

    internal class sortedHierarchyItems : SortedSet<AnglrGetGetHierarchyItemData>
    {
        public sortedHierarchyItems () : base (new CmpHierarchyItem ()) { }
    }

    internal class AnglrDocContext : SyntaxTreeWalker, IDisposable
    {
        public IAnglrLogger Logger { get; private set; }
        public AnglrLSPTarget AnglrLSPTarget { get; private set; } = null;
        public string fileName { get; private set; }
        public Uri uri { get; private set; }
        public string [] lines { get; private set; }

        private anglrCompiler anglrCompiler = null;
        private AnglrHtmlColorizer anglrHtmlColorizer = null;
        private AnglrSyntaxTreeGenerator anglrSyntaxTreeGenerator = null;
        private AnglrSpanGenerator anglrSpanGenerator = null;
        private AnglrReferencesGenerator anglrReferencesGenerator = null;
        private AnglrParserStatesGenerator anglrParserStatesGenerator = null;
        private AnglrProdToTextEmiter anglrProdToTextEmiter = new AnglrProdToTextEmiter ();
        private AnglrTokenNavigatorInfo anglrTokenNavigatorInfo = new AnglrTokenNavigatorInfo ();

        private object parserLock = null;

        private parselist syntaxTrees = null;
        private _anglr_file_ anglr_file_tree = null;

        private editlist editChanges = null;
        private Timer editTimer = null;

        private List<Diagnostic> diagnostics = null;

        public AnglrDocContext (DidOpenTextDocumentParams didOpenTextDocumentParams, AnglrLSPTarget anglrLSPTarget)
        {
            AnglrLSPTarget = anglrLSPTarget;
            Logger = (IAnglrLogger) AnglrLSPTarget?.AnglrLspDriver?.Logger ?? new VoidAnglrLogger ();
            uri = didOpenTextDocumentParams.TextDocument.Uri;
            fileName = didOpenTextDocumentParams.TextDocument.Uri.AbsolutePath;
            lines = didOpenTextDocumentParams.TextDocument.Text.Replace ("\r", "").Split (new char [] { '\n' });
            Error_Event += Invoke_Error_Callback;

            parserLock = new object ();
            editChanges = new editlist ();
            editTimer = new Timer (InvokeEditChanges);

            Common_Event += AnglrDocContext_Common_Event;
        }

        private bool AnglrDocContext_Common_Event (SyntaxTreeCallbackReason reason, int kind, SyntaxTreeBase p_node)
        {
            switch (reason)
            {
                case SyntaxTreeCallbackReason.BuilderCallbackReason:
                    break;
                case SyntaxTreeCallbackReason.TraversalPrologueCallbackReason:
                {
                    if (!(p_node is SyntaxTreeToken))
                        break;
                    SyntaxTreeToken token = (SyntaxTreeToken) p_node;
                    AnglrNSCInfo info = anglrCompiler.FindNSCInfo ((token.lineno, token.column));
                    if (info == null)
                        break;
                    ((AppInfo) token.appInfo) [AppInfoType.NSCInfo] = info;
                }
                break;
                case SyntaxTreeCallbackReason.TraversalMidTermCallbackReason:
                    break;
                case SyntaxTreeCallbackReason.TraversalEpilogueCallbackReason:
                    break;
            }
            return true;
        }

        private bool Invoke_Error_Callback (int lineno, int column, int token, string tokenString)
        {
            Diagnostic diagnostic = new Diagnostic ();
            diagnostic.Code = "E001";
            diagnostic.Message = "Syntax Error";
            diagnostic.Severity = DiagnosticSeverity.Error;
            diagnostic.Source = fileName;
            Position start = new Position ();
            start.Line = lineno;
            start.Character = column;
            Position end = new Position ();
            end.Line = lineno;
            end.Character = column + tokenString.Length;
            Range range = new Range ();
            range.Start = start;
            range.End = end;
            diagnostic.Range = range;
            diagnostics.Add (diagnostic);
            Logger?.ErrorLine ($"SYNTAX ERROR: line = {lineno}, column = {column}, token = {token}, text = {tokenString}");
            return false;
        }

        private void PublishDiagnostics ()
        {
            Diagnostic [] diagnostics = this.diagnostics.ToArray ();
            //this.diagnostics = null;
            //int index = 0;
            //foreach (Diagnostic diagnostic in this.diagnostics)
            //    diagnostics [index++] = diagnostic;
            PublishDiagnosticParams publishDiagnosticParams = new PublishDiagnosticParams ();
            publishDiagnosticParams.Diagnostics = diagnostics;
            publishDiagnosticParams.Uri = uri;
            Logger?.DebugLine ($"PublishDiagnostics: {JsonConvert.SerializeObject (publishDiagnosticParams)}");
            AnglrLSPTarget.Notify (Methods.TextDocumentPublishDiagnosticsName, publishDiagnosticParams);
        }

        private void Parse (string text, object [] info)
        {
            AnglrParser.debug = false;
            AnglrParser.createParseTree = true;
            AnglrParser.loopDetection = true;
            anglrCompiler.createPrecedenceGrammar = true;
            anglrCompiler.createIterators = true;
            diagnostics = new List<Diagnostic> ();
            try
            {
                anglrCompiler = new anglrCompiler (null, Logger);
                anglrCompiler.Error_Event += Invoke_Error_Callback;
                if (anglrCompiler.ParseString (text, AnglrDeclarations.tokens._anglr_file_terminal_, info) != 0)
                {
                    Logger?.ErrorLine ($"parse errors in '{fileName}'");
                }
                else
                {
                    Logger?.InfoLine ($"parse OK '{fileName}'");
                    anglrSpanGenerator = new AnglrSpanGenerator (anglrCompiler);
                    anglrReferencesGenerator = new AnglrReferencesGenerator (anglrCompiler);
                    anglrSyntaxTreeGenerator = new AnglrSyntaxTreeGenerator (AnglrLSPTarget);
                    anglrHtmlColorizer = new AnglrHtmlColorizer (AnglrLSPTarget);
                    anglrParserStatesGenerator = new AnglrParserStatesGenerator (anglrCompiler);
                    foreach (SyntaxTreeBase node in (syntaxTrees = anglrCompiler.parseList))
                    {
                        //AnglrGenerator generator = new AnglrGenerator ((_anglr_file_fragment_) node);
                        anglrSpanGenerator.TraverseCommon ((_anglr_file_fragment_) node);
                        anglrReferencesGenerator.TraverseCommon ((_anglr_file_fragment_) node);
                        anglrSpanGenerator.Traverse ((_anglr_file_fragment_) node);
                        anglrSyntaxTreeGenerator.GenerateSyntaxTree ((_anglr_file_fragment_) node);
                        //anglrSyntaxTreeGenerator.GenerateNames (anglrSyntaxTreeGenerator.SyntaxTree);
                        anglrHtmlColorizer.Traverse ((_anglr_file_fragment_) node);    // 1st step
                        anglrHtmlColorizer.Traverse ((_anglr_file_fragment_) node);    // 2nd step
                        anglrParserStatesGenerator.GenerateParserStates ();
                        TraverseCommon ((_anglr_file_fragment_) node);
                    }
                }
            }
            catch (Exception pe)
            {
                Logger?.ErrorLine ($"parser exception: {pe.Message}");
                Logger?.ErrorLine ($"     stack trace:");
                Logger?.ErrorLine ($"{pe.StackTrace}");
            }
            LogSymTab (AnglrLSPTarget);
            PublishDiagnostics ();
        }

        private void Parse (string [] lines, object [] info = null)
        {
            AnglrParser.debug = false;
            AnglrParser.createParseTree = true;
            AnglrParser.loopDetection = true;
            anglrCompiler.createPrecedenceGrammar = true;
            anglrCompiler.createIterators = true;
            diagnostics = new List<Diagnostic> ();
            try
            {
                anglrCompiler = new anglrCompiler (null, Logger);
                anglrCompiler.Error_Event += Invoke_Error_Callback;
                if (anglrCompiler.ParseStringList (lines, AnglrDeclarations.tokens._anglr_file_terminal_, info) != 0)
                {
                    Logger?.ErrorLine ($"parse errors in '{fileName}'");
                }
                else
                {
                    Logger?.InfoLine ($"parse OK '{fileName}'");
                    anglrSpanGenerator = new AnglrSpanGenerator (anglrCompiler);
                    anglrReferencesGenerator = new AnglrReferencesGenerator (anglrCompiler);
                    anglrSyntaxTreeGenerator = new AnglrSyntaxTreeGenerator (AnglrLSPTarget);
                    anglrHtmlColorizer = new AnglrHtmlColorizer (AnglrLSPTarget);
                    anglrParserStatesGenerator = new AnglrParserStatesGenerator (anglrCompiler);
                    foreach (SyntaxTreeBase node in (syntaxTrees = anglrCompiler.parseList))
                    {
                        anglrSpanGenerator.TraverseCommon ((_anglr_file_fragment_) node);
                        anglrReferencesGenerator.TraverseCommon ((_anglr_file_fragment_) node);
                        anglrSpanGenerator.Traverse ((_anglr_file_fragment_) node);
                        anglrSyntaxTreeGenerator.GenerateSyntaxTree ((_anglr_file_fragment_) node);
                        //anglrSyntaxTreeGenerator.GenerateNames (anglrSyntaxTreeGenerator.SyntaxTree);
                        anglrHtmlColorizer.Traverse ((_anglr_file_fragment_) node);    // 1st step
                        anglrHtmlColorizer.Traverse ((_anglr_file_fragment_) node);    // 2nd step
                        anglrParserStatesGenerator.GenerateParserStates ();
                        TraverseCommon ((_anglr_file_fragment_) node);
                    }
                }
            }
            catch (Exception pe)
            {
                Logger?.ErrorLine ($"parser exception: {pe.Message}");
                Logger?.ErrorLine ($"     stack trace:");
                Logger?.ErrorLine ($"{pe.StackTrace}");
            }
            LogSymTab (AnglrLSPTarget);
            PublishDiagnostics ();
        }

        private void LogSymTab (AnglrLSPTarget anglrLSPTarget)
        {
            if (!AnglrParser.debug)
                return;
        }

        public void TextDocumentDidOpen (AnglrLSPTarget anglrLSPTarget, DidOpenTextDocumentParams didOpenTextDocumentParams)
        {
            Logger?.DebugLine ($"current thread ID = {Thread.CurrentThread.ManagedThreadId}");
            Logger?.DebugLine ($"languageId = {didOpenTextDocumentParams.TextDocument.LanguageId}");
            Logger?.DebugLine ($"document   = {didOpenTextDocumentParams.TextDocument.Uri.AbsolutePath}");
            string fileName = didOpenTextDocumentParams.TextDocument.Uri.AbsolutePath;
            lock (parserLock)
            {
                Parse (lines);
            }
        }

        public void TextDocumentDidClose (AnglrLSPTarget anglrLSPTarget, DidCloseTextDocumentParams didCloseTextDocumentParams)
        {
            lock (parserLock)
            {
                diagnostics.Clear ();
                PublishDiagnostics ();
            }
        }

        public void TextDocumentDidChange (AnglrLSPTarget anglrLSPTarget, DidChangeTextDocumentParams didChangeTextDocumentParams)
        {
            Logger?.DebugLine ($"current thread ID = {Thread.CurrentThread.ManagedThreadId}");
            TextDocumentContentChangeEvent [] changeEvents = didChangeTextDocumentParams.ContentChanges;
            lock (parserLock)
            {
                editTimer.Change (1000, Timeout.Infinite);
                foreach (TextDocumentContentChangeEvent changeEvent in changeEvents)
                {
                    editChanges.Enqueue (changeEvent);
                    continue;
                    Range range = changeEvent.Range;
                    int? rangeLength = changeEvent.RangeLength;
                    string text = changeEvent.Text;
                    Position start = range.Start;
                    Position end = range.End;
                    Logger?.DebugLine ($"change event:");
                    Logger?.DebugLine ($"\ttext: {text}");
                    Logger?.DebugLine ($"\tstart ({start.Line}, {start.Character}), end ({end.Line}, {end.Character})");
                }
            }
        }

        public void TextDocumentDidSave (AnglrLSPTarget anglrLSPTarget, DidSaveTextDocumentParams didSaveTextDocumentParams)
        {
            Logger?.DebugLine ($"current thread ID = {Thread.CurrentThread.ManagedThreadId}");
        }

        public void TextDocumentDidCloe (AnglrLSPTarget anglrLSPTarget, DidCloseTextDocumentParams didCloseTextDocumentParams)
        {
            Logger?.DebugLine ($"current thread ID = {Thread.CurrentThread.ManagedThreadId}");
        }

        public SymbolInformation [] TextDocumentDocumentSymbol (AnglrLSPTarget anglrLSPTarget, DocumentSymbolParams symbolParams)
        {
            Logger?.DebugLine ($"current thread ID = {Thread.CurrentThread.ManagedThreadId}");
            List<SymbolInformation> symInfoList = new List<SymbolInformation> ();
            //foreach (KeyValuePair<(int lineno, int column, int length, bool reference), (SymbolToken symbol, object obj)> keyval in symtab)
            //{
            //	symInfoList.Add
            //	(
            //		new SymbolInformation ()
            //		{
            //			ContainerName = null,
            //			Kind = SymbolKind.String,
            //			Location = new Location ()
            //			{
            //				Range = new Range ()
            //				{
            //					Start = new Position ()
            //					{
            //						Character = keyval.Key.column,
            //						Line = keyval.Key.lineno
            //					},
            //					End = new Position ()
            //					{
            //						Character = keyval.Key.column + keyval.Key.length,
            //						Line = keyval.Key.lineno
            //					}
            //				}
            //			},
            //			Name = keyval.Value.symbol.name
            //		}
            //	);
            //}
            Logger?.DebugLine ($"Text Document Symbol Count = {symInfoList.Count}");
            return symInfoList.ToArray ();
        }

        public Location [] TextDocumentReferences (AnglrLSPTarget anglrLSPTarget, ReferenceParams referenceParams)
        {
            Logger?.DebugLine ($"current thread ID = {Thread.CurrentThread.ManagedThreadId}");
            Position position = referenceParams.Position;
            Logger?.DebugLine ($"symbol references at: ({position.Line}, {position.Character})");
            lock (parserLock)
            {
                SymbolToken symbol = null;
                SymSpanInfo tokenDes = anglrSpanGenerator.findSymSpan ((position.Line + 1, position.Character, 0));
                try
                {
                    symbol = (SymbolToken) ((AppInfo) tokenDes.token.appInfo) [AppInfoType.SymbolToken];
                }
                catch
                {
                    Logger?.DebugLine ($"cannot locate symbol at position: {position.Line}, {position.Character}");
                    return null;
                }
                Logger?.DebugLine ($"symbol references for: {symbol.name}");

                List<Location> locations = new List<Location> ();

                for (int i = 0; i < 2; ++i)
                {
                    if ((i == 0) && (referenceParams.Context.IncludeDeclaration))
                    {
                        reflist definitions = symbol.m_deflist;
                        foreach (SymbolReference reference in definitions)
                        {
                            Location location = new Location ();
                            Position start = new Position (reference.lineno, reference.column);
                            Position end = new Position (reference.lineno, reference.column + symbol.name.Length);
                            Range range = new Range ();
                            range.Start = start;
                            range.End = end;
                            location.Range = range;
                            location.Uri = referenceParams.TextDocument.Uri;
                            locations.Add (location);
                            //anglrLSPTarget.Log ("\tdefinition: (" + reference.lineno + ", " + reference.column + ")");
                        }
                    }

                    reflist references = symbol.m_reflist;
                    foreach (SymbolReference reference in references)
                    {
                        Location location = new Location ();
                        Position start = new Position (reference.lineno, reference.column);
                        Position end = new Position (reference.lineno, reference.column + symbol.name.Length);
                        Range range = new Range ();
                        range.Start = start;
                        range.End = end;
                        location.Range = range;
                        location.Uri = referenceParams.TextDocument.Uri;
                        locations.Add (location);
                        //anglrLSPTarget.Log ("\treference:  (" + reference.lineno + ", " + reference.column + ")");
                    }
                    if ((symbol = symbol.alias) == null)
                        break;
                }

                return locations.ToArray ();
            }
        }

        public Location [] TextDocumentReferences (AnglrLSPTarget anglrLSPTarget, ReferencesParamsClass referenceParams)
        {
            Logger?.DebugLine ($"current thread ID = {Thread.CurrentThread.ManagedThreadId}");
            Position position = referenceParams.position;
            Logger?.DebugLine ($"symbol references at: ({position.Line}, {position.Character})");
            lock (parserLock)
            {
                SymbolToken symbol = null;
                SymSpanInfo tokenDes = anglrSpanGenerator.findSymSpan ((position.Line + 1, position.Character, 0));
                try
                {
                    symbol = (SymbolToken) ((AppInfo) tokenDes.token.appInfo) [AppInfoType.SymbolToken];
                }
                catch
                {
                    Logger?.DebugLine ($"cannot locate symbol at position: {position.Line}, {position.Character}");
                    return null;
                }
                Logger?.DebugLine ($"symbol references for: {symbol.name}");

                List<Location> locations = new List<Location> ();

                Uri uri = referenceParams.textDocument.uri;
                for (int i = 0; i < 2; ++i)
                {
                    if ((i == 0) && (referenceParams.context.IncludeDeclaration))
                    {
                        reflist definitions = symbol.m_deflist;
                        foreach (SymbolReference reference in definitions)
                        {
                            Location location = new Location ();
                            Position start = new Position (reference.lineno, reference.column);
                            Position end = new Position (reference.lineno, reference.column + symbol.name.Length);
                            Range range = new Range ();
                            range.Start = start;
                            range.End = end;
                            location.Range = range;
                            location.Uri = uri;
                            locations.Add (location);
                            //anglrLSPTarget.Log ("\tdefinition: (" + reference.lineno + ", " + reference.column + ")");
                        }
                    }

                    reflist references = symbol.m_reflist;
                    foreach (SymbolReference reference in references)
                    {
                        Location location = new Location ();
                        Position start = new Position (reference.lineno, reference.column);
                        Position end = new Position (reference.lineno, reference.column + symbol.name.Length);
                        Range range = new Range ();
                        range.Start = start;
                        range.End = end;
                        location.Range = range;
                        location.Uri = uri;
                        locations.Add (location);
                        //anglrLSPTarget.Log ("\treference:  (" + reference.lineno + ", " + reference.column + ")");
                    }
                    if ((symbol = symbol.alias) == null)
                        break;
                }
                return locations.ToArray ();
            }
        }

        public object TextDocumentDefinition (AnglrLSPTarget anglrLSPTarget, TextDocumentPositionParams textDocumentPositionParams)
        {
            Logger?.DebugLine ($"current thread ID = {Thread.CurrentThread.ManagedThreadId}");
            Position position = textDocumentPositionParams.Position;
            Logger?.DebugLine ($"symbol definitions at: ({position.Line}, {position.Character})");
            lock (parserLock)
            {
                SymbolToken symbol = null;
                SymSpanInfo tokenDes = anglrSpanGenerator.findSymSpan ((position.Line + 1, position.Character, 0));
                try
                {
                    symbol = (SymbolToken) ((AppInfo) tokenDes.token.appInfo) [AppInfoType.SymbolToken];
                }
                catch
                {
                    Logger?.DebugLine ($"cannot locate symbol at position: {position.Line}, {position.Character}");
                    return null;
                }
                Logger?.DebugLine ($"symbol definitions for: {symbol.name}");

                List<Location> locations = new List<Location> ();

                for (int i = 0; i < 2; ++i)
                {
                    reflist definitions = symbol.m_deflist;
                    foreach (SymbolReference reference in definitions)
                    {
                        Location location = new Location ();
                        Position start = new Position (reference.lineno, reference.column);
                        Position end = new Position (reference.lineno, reference.column + symbol.name.Length);
                        Range range = new Range ();
                        range.Start = start;
                        range.End = end;
                        location.Range = range;
                        location.Uri = textDocumentPositionParams.TextDocument.Uri;
                        locations.Add (location);
                        //anglrLSPTarget.Log ("\tdefinition: (" + reference.lineno + ", " + reference.column + ")");
                    }
                    //if ((symbol = symbol.alias) == null)
                    break;
                }

                return locations.ToArray ();
            }
        }

        public DocumentHighlight [] TextDocumentDocumentHighlight (AnglrLSPTarget anglrLSPTarget, TextDocumentPositionParams textDocumentPositionParams)
        {
            Logger?.DebugLine ($"current thread ID = {Thread.CurrentThread.ManagedThreadId}");
            Position position = textDocumentPositionParams.Position;
            Logger?.DebugLine ($"symbol highlight at: ({position.Line}, {position.Character})");
            lock (parserLock)
            {
                SymbolToken symbol = null;
                SymSpanInfo tokenDes = anglrSpanGenerator.findSymSpan ((position.Line + 1, position.Character, 0));
                try
                {
                    symbol = (SymbolToken) ((AppInfo) tokenDes.token.appInfo) [AppInfoType.SymbolToken];
                }
                catch
                {
                    Logger?.DebugLine ($"cannot locate symbol at position: {position.Line}, {position.Character}");
                    return null;
                }
                Logger?.DebugLine ($"symbol highlight for: {symbol.name}");

                List<DocumentHighlight> highlights = new List<DocumentHighlight> ();

                for (int i = 0; i < 2; ++i)
                {
                    reflist definitions = symbol.m_deflist;
                    foreach (SymbolReference reference in definitions)
                    {
                        DocumentHighlight highlight = new DocumentHighlight ();
                        highlight.Kind = DocumentHighlightKind.Text;
                        Range range = new Range ();
                        Position start = new Position (reference.lineno, reference.column);
                        Position end = new Position (reference.lineno, reference.column + symbol.name.Length);
                        range.Start = start;
                        range.End = end;
                        highlight.Range = range;
                        highlights.Add (highlight);
                        //anglrLSPTarget.Log ("\thighlight: (" + highlight.Range.Start.Line + ", " + highlight.Range.Start.Character + "), (" + highlight.Range.End.Line + ", " + highlight.Range.End.Character + ")");
                    }

                    reflist references = symbol.m_reflist;
                    foreach (SymbolReference reference in references)
                    {
                        DocumentHighlight highlight = new DocumentHighlight ();
                        highlight.Kind = DocumentHighlightKind.Text;
                        Range range = new Range ();
                        Position start = new Position (reference.lineno, reference.column);
                        Position end = new Position (reference.lineno, reference.column + symbol.name.Length);
                        range.Start = start;
                        range.End = end;
                        highlight.Range = range;
                        highlights.Add (highlight);
                        //anglrLSPTarget.Log ("\thighlight: (" + highlight.Range.Start.Line + ", " + highlight.Range.Start.Character + "), (" + highlight.Range.End.Line + ", " + highlight.Range.End.Character + ")");
                    }
                    if ((symbol = symbol.alias) == null)
                        break;
                }
                return highlights.ToArray ();
            }
        }

        public async Task<Hover> TextDocumentHoverAsync (AnglrLSPTarget anglrLSPTarget, TextDocumentPositionParams textDocumentPositionParams)
        {
            return await Task.Run (() =>
            {
                Logger?.DebugLine ($"current thread ID = {Thread.CurrentThread.ManagedThreadId}");
                Position position = textDocumentPositionParams.Position;
                Logger?.DebugLine ("symbol definitions at: ({position.Line}, {position.Character})");
                lock (parserLock)
                {
                    SymbolToken symbol = null;
                    SymSpanInfo tokenDes = anglrSpanGenerator.findSymSpan ((position.Line + 1, position.Character, 0));
                    try
                    {
                        symbol = (SymbolToken) ((AppInfo) tokenDes.token.appInfo) [AppInfoType.SymbolToken];
                    }
                    catch
                    {
                        Logger?.DebugLine ($"cannot locate symbol at position: {position.Line}, {position.Character}");
                        return null;
                    }

                    Hover hover = new Hover ();
                    hover.Range = null;
                    MarkedString markedString = new MarkedString ();
                    markedString.Language = "anglr";
                    markedString.Value = "";

                    bool multiLine = false;
                    foreach (SymbolReference symbolReference in symbol.m_deflist)
                    {
                        Logger?.DebugLine ($"hover symbol definition at ({symbolReference.lineno}, {symbolReference.column})");

                        SymSpanInfo refDes = anglrSpanGenerator.findSymSpan ((symbolReference.lineno + 1, symbolReference.column, symbolReference.length));

                        SyntaxTreeBase node;
                        node = refDes.syntax;
                        Logger?.DebugLine ($"hover syntax node = {refDes.token.Emit (-1)}");
                        if (node == null)
                            for
                            (
                                node = (SyntaxTreeBase) refDes.token;

                                !((node == null) ||
                                (node is _parser_part_) ||
                                (node is _scanner_part_) ||
                                (node is _general_part_) ||
                                (node is _declaration_part_) ||
                                (node is _anglr_syntax_rule_) ||
                                (node is _anglr_nested_rule_) ||
                                (node is _anglr_definition_) ||
                                (node is _attribute_) ||
                                (node is _marker_) ||
                                (node is _production_name_) ||
                                (node is _name_value_pair_));

                                node = node.parent
                            )
                                ;

                        if (node == null)
                        {
                            Logger?.WarnLine ($"hover symbol node conflict: {symbol.name}");
                            continue;
                        }
                        Logger?.DebugLine ($"hover node = {node.Emit (-1)}");
                        if (multiLine)
                            markedString.Value += "\n";
                        try
                        {
                            markedString.Value += anglrProdToTextEmiter.GetNodeString (node);
                        }
                        catch (Exception e)
                        {
                            Logger?.ErrorLine ($"hover error:\nmessage = {e.Message},\nstack = {e.StackTrace}");
                        }
                        multiLine = true;
                    }
                    hover.Contents = new SumType<string, MarkedString> (markedString);

                    return hover;
                }
            });
        }

        public WorkspaceEdit TextDocumentRename (AnglrLSPTarget anglrLSPTarget, RenameParams renameParams)
        {
            Logger?.DebugLine ($"current thread ID = {Thread.CurrentThread.ManagedThreadId}");
            Position position = renameParams.Position;
            Logger?.DebugLine ($"symbol rename at: ({position.Line}, {position.Character})");

            lock (parserLock)
            {
                SymbolToken symbol = null;
                SymSpanInfo tokenDes = anglrSpanGenerator.findSymSpan ((position.Line + 1, position.Character, 0));
                try
                {
                    symbol = (SymbolToken) ((AppInfo) tokenDes.token.appInfo) [AppInfoType.SymbolToken];
                }
                catch
                {
                    Logger?.DebugLine ($"cannot locate symbol at position: {position.Line}, {position.Character}");
                    return null;
                }
                reflist references = symbol.m_reflist;
                reflist definition = symbol.m_deflist;

                WorkspaceEdit workspaceEdit = new WorkspaceEdit ();
                TextEdit [] textEdits = new TextEdit [references.Count + definition.Count];
                workspaceEdit.Changes = null;
                TextDocumentEdit [] textDocumentEdits = new TextDocumentEdit [1];
                workspaceEdit.DocumentChanges = textDocumentEdits;
                TextDocumentEdit textDocumentEdit = new TextDocumentEdit ();
                textDocumentEdit.Edits = textEdits;
                textDocumentEdit.TextDocument = new OptionalVersionedTextDocumentIdentifier ();
                textDocumentEdit.TextDocument.Uri = renameParams.TextDocument.Uri;
                textDocumentEdit.TextDocument.Version = null;
                //workspaceEdit.DocumentChanges [0] = textDocumentEdit;

                int count = 0;
                foreach (SymbolReference reference in definition)
                {
                    TextEdit textEdit = new TextEdit ();
                    textEdits [count++] = textEdit;
                    textEdit.NewText = renameParams.NewName;
                    Range range = new Range ();
                    Position start = new Position (reference.lineno, reference.column);
                    Position end = new Position (reference.lineno, reference.column + reference.length);
                    range.Start = start;
                    range.End = end;
                    textEdit.Range = range;
                    //anglrLSPTarget.Log ("\trename: (" + textEdit.Range.Start.Line + ", " + textEdit.Range.Start.Character + "), (" + textEdit.Range.End.Line + ", " + textEdit.Range.End.Character + ")");
                }
                foreach (SymbolReference reference in references)
                {
                    TextEdit textEdit = new TextEdit ();
                    textEdits [count++] = textEdit;
                    textEdit.NewText = renameParams.NewName;
                    Range range = new Range ();
                    Position start = new Position (reference.lineno, reference.column);
                    Position end = new Position (reference.lineno, reference.column + reference.length);
                    range.Start = start;
                    range.End = end;
                    textEdit.Range = range;
                    //anglrLSPTarget.Log ("\trename: (" + textEdit.Range.Start.Line + ", " + textEdit.Range.Start.Character + "), (" + textEdit.Range.End.Line + ", " + textEdit.Range.End.Character + ")");
                }

                return workspaceEdit;
            }
        }

        public async Task<AnglrGetClassificationSpansResult> AnglrGetClassificationSpansAsync (AnglrLSPTarget anglrLSPTarget, AnglrGetClassificationSpansParams anglrGetClassificationSpansParams)
        {
            return await Task.Run (() =>
            {
                AnglrGetClassificationSpansResult anglrGetClassificationSpansResult = new AnglrGetClassificationSpansResult ()
                {
                    Position = anglrGetClassificationSpansParams.Position,
                    ClassificationSpanInfo = new List<(int column, int line, int classification)> ()
                };
                try
                {
                    List<SymSpanInfo> list = anglrSpanGenerator.findLineSymSpans (anglrGetClassificationSpansParams.Position.Line + 1);
                    foreach (SymSpanInfo element in list)
                    {
                        anglrGetClassificationSpansResult.ClassificationSpanInfo.Add ((element.token.column, element.token.text.Length, (int) element.classificationType));
                    }
                }
                catch
                {

                }
                Logger?.DebugLine ($"AnglrGetClassificationSpansResult = {JsonConvert.SerializeObject (anglrGetClassificationSpansResult)}");
                return anglrGetClassificationSpansResult;
            });
        }

        public AnglrGetGetHierarchyItemResult AnglrGetGetHierarchyItem (AnglrLSPTarget anglrLSPTarget, AnglrGetGetHierarchyItemParams anglrGetGetHierarchyItemParams)
        {
            try
            {
                AnglrGetGetHierarchyItemResult result = new AnglrGetGetHierarchyItemResult ();
                AnglrSyntaxTreeNode syntaxTreeNode = anglrSyntaxTreeGenerator.FindNode (anglrGetGetHierarchyItemParams.ItemId);
                foreach (var node in syntaxTrees)
                {
                    result.NodeCategory = syntaxTreeNode.NodeCategory;
                    result.NodeSubCategory = syntaxTreeNode.NodeSubCategory;
                    result.NodeName = syntaxTreeNode.Name;
                    result.HtmlText = anglrHtmlColorizer.GenerateHtmlText ((_anglr_file_fragment_) node, syntaxTreeNode.node);
                }
                AnglrNodeChildren syntaxTreeNodes = anglrSyntaxTreeGenerator.FindChildren (anglrGetGetHierarchyItemParams.ItemId);
                result.Items = new AnglrGetGetHierarchyItemData [syntaxTreeNodes.Count];
                int index = 0;
                foreach (var child in syntaxTreeNodes)
                {
                    AnglrGetGetHierarchyItemData anglrGetGetHierarchyItemData = new AnglrGetGetHierarchyItemData ();
                    anglrGetGetHierarchyItemData.ItemId = $"{anglrGetGetHierarchyItemParams.ItemId}.{child.node.id}-{child.index}";
                    anglrGetGetHierarchyItemData.ItemName = child.Name;
                    anglrGetGetHierarchyItemData.Cookie = 0;
                    anglrGetGetHierarchyItemData.Specie = child.node.id;
                    result.Items [index++] = anglrGetGetHierarchyItemData;
                }
                Logger?.DebugLine ($"SYNTAX TREE NODE = {syntaxTreeNode.node.Emit (-1)}");
                Logger?.DebugLine ($"AnglrGetGetHierarchyItemResult = {JsonConvert.SerializeObject (result)}");
                return result;
            }
            catch (Exception e)
            {
                Logger?.DebugLine ($"Anglr Get Hierarchy Item failed: {e.Message}\n{e.StackTrace}");
                return null;
            }
        }

        public AnglrGetDictionaryItemResult AnglrGetDictionaryItem (AnglrLSPTarget anglrLSPTarget, AnglrGetDictionaryItemParams anglrGetDictionaryItemParams)
        {
            try
            {
                AnglrGetDictionaryItemResult result = new AnglrGetDictionaryItemResult ();
                AnglrSyntaxTreeNode syntaxTreeNode = anglrSyntaxTreeGenerator.FindNode (anglrGetDictionaryItemParams.ItemId);
                foreach (var node in syntaxTrees)
                {
                    result.NodeCategory = syntaxTreeNode.NodeCategory;
                    result.NodeSubCategory = syntaxTreeNode.NodeSubCategory;
                    result.NodeName = syntaxTreeNode.Name;
                    result.HtmlText = anglrHtmlColorizer.GenerateHtmlText ((_anglr_file_fragment_) node, syntaxTreeNode.node);
                }
                AnglrNodeChildren syntaxTreeNodes = anglrSyntaxTreeGenerator.FindChildren (anglrGetDictionaryItemParams.ItemId);
                result.Items = new AnglrGetDictionaryItemData [syntaxTreeNodes.Count];
                int index = 0;
                foreach (var child in syntaxTreeNodes)
                {
                    AnglrGetDictionaryItemData anglrGetDictionaryItemData = new AnglrGetDictionaryItemData ();
                    anglrGetDictionaryItemData.ItemId = $"{anglrGetDictionaryItemParams.ItemId}.{child.node.id}-{child.index}";
                    anglrGetDictionaryItemData.ItemName = child.Name;
                    anglrGetDictionaryItemData.Cookie = 0;
                    anglrGetDictionaryItemData.Specie = child.node.id;
                    result.Items [index++] = anglrGetDictionaryItemData;
                }
                Logger?.DebugLine ($"SYNTAX TREE NODE = {syntaxTreeNode.node.Emit (-1)}");
                Logger?.DebugLine ($"AnglrGetDictionaryItemResult = {JsonConvert.SerializeObject (result)}");
                return result;
            }
            catch (Exception e)
            {
                Logger?.ErrorLine ($"Anglr Get Dictionary Item failed: {e.Message}\n{e.StackTrace}");
                return null;
            }
        }

        public AnglrGetParserStateItemResult AnglrGetParserStateItem (AnglrLSPTarget anglrLSPTarget, AnglrGetParserStateItemParams anglrGetParserStateItemParams)
        {
            try
            {
                return CreateAnglrGetParserStateItemResult (anglrParserStatesGenerator, anglrGetParserStateItemParams.StateNr);
            }
            catch (Exception e)
            {
                Logger?.ErrorLine (e, $"Anglr Get Parser State Item failed");
                return null;
            }
        }

        public AnglrGetParserStatesInfoResult AnglrGetParserStatesInfo (AnglrLSPTarget anglrLSPTarget, AnglrGetParserStatesInfoParams anglrGetParserStatesInfoParams)
        {
            try
            {
                AnglrGetParserStateItemResultSet resultSet = new AnglrGetParserStateItemResultSet ();
                CreateResultSet (resultSet, anglrParserStatesGenerator, 0);
                return new AnglrGetParserStatesInfoResult () { ParserStatesSet = resultSet };
            }
            catch (Exception e)
            {
                Logger?.ErrorLine ($"Anglr Get Parser States Info failed: {e.Message}\n{e.StackTrace}");
                return null;
            }
        }

        public AnglrGetParserStateLinkResult AnglrGetParserStateLink (AnglrLSPTarget anglrLSPTarget, AnglrGetParserStateLinkParams anglrGetParserStateLinkParams)
        {
            try
            {
                return CreateAnglrGetParserStateLinkResult (anglrParserStatesGenerator, anglrGetParserStateLinkParams.StateNr);
            }
            catch (Exception e)
            {
                Logger?.ErrorLine ($"Anglr Get Parser State Link failed: {e.Message}\n{e.StackTrace}");
                return null;
            }
        }

        public AnglrGetParserMagicNumberResult AnglrGetParserMagicNumber (AnglrLSPTarget anglrLSPTarget, AnglrGetParserMagicNumberParams anglrGetParserMagicNumberParams)
        {
            return new AnglrGetParserMagicNumberResult ()
            {
                MagicNumber = anglrParserStatesGenerator?.magicNr
            };
        }

        public AnglrGetParserSyntaxRuleResult AnglrGetParserSyntaxRule (AnglrLSPTarget anglrLSPTarget, AnglrGetParserSyntaxRuleParams anglrGetParserSyntaxRuleParams)
        {
            try
            {
                return CreateAnglrGetParserSyntaxRuleResult (anglrParserStatesGenerator, anglrGetParserSyntaxRuleParams.RuleName);
            }
            catch (Exception e)
            {
                Logger?.ErrorLine ($"Anglr Get Parser Syntax Rule failed: {e.Message}\n{e.StackTrace}");
                return null;
            }
        }

        public AnglrGetParserSyntaxRulesResult AnglrGetParserSyntaxRules (AnglrLSPTarget anglrLSPTarget, AnglrGetParserSyntaxRulesParams anglrGetParserSyntaxRulesParams)
        {
            try
            {
                return CreateAnglrGetParserSyntaxRulesResult (anglrParserStatesGenerator);
            }
            catch (Exception e)
            {
                Logger?.ErrorLine ($"Anglr Get Parser Syntax Rules failed: {e.Message}\n{e.StackTrace}");
                return null;
            }
        }

        public AnglrGetItemNavigationInfoResponse AnglrGetItemNavigationInfo (AnglrLSPTarget anglrLSPTarget, AnglrGetItemNavigationInfoRequest anglrGetItemNavigationInfoRequest)
        {
            try
            {
                AnglrGetItemNavigationInfoResponse result = new AnglrGetItemNavigationInfoResponse ()
                {
                    ItemColumn = -1,
                    ItemLineno = -1
                };
                AnglrSyntaxTreeNode syntaxTreeNode = anglrSyntaxTreeGenerator.FindNode (anglrGetItemNavigationInfoRequest.ItemId);
                anglrTokenNavigatorInfo.GetNavigationInfo (syntaxTreeNode.fragment);
                result.ItemLineno = anglrTokenNavigatorInfo.LineNo;
                result.ItemColumn = anglrTokenNavigatorInfo.Column;
                return result;
            }
            catch (Exception e)
            {
                Logger?.ErrorLine ($"Anglr Get Item Navigation Info failed: {e.Message}\n{e.StackTrace}");
                return null;
            }
        }

        public AnglrGetCompileFragmentResponse AnglrGetCompileFragment (AnglrLSPTarget anglrLSPTarget, AnglrGetCompileFragmentRequest anglrGetCompileFragmentRequest)
        {
            try
            {
                AnglrGetCompileFragmentResponse result = new AnglrGetCompileFragmentResponse ()
                {
                    Result = -1,
                    Fragment = ""
                };
                AnglrSyntaxTreeNode syntaxTreeNode = anglrSyntaxTreeGenerator.FindNode (anglrGetCompileFragmentRequest.ItemId);
                anglrCompiler compiler = new anglrCompiler (null, Logger);
                compiler.Error_Event += (lineno, column, token, tokenString) =>
                {
                    anglrLSPTarget.LogError ($"FRAGMENT SYNTAX ERROR: line = {lineno}, col = {column}, token = {token}, string = {tokenString}");
                    return false;
                };
                string text = anglrHtmlColorizer.GeneratePlainText (syntaxTreeNode.node).ToString ();
                string [] src = text.Split (new char [] { '\n', '\r' });
                result.Fragment = text;
                if ((result.Result = compiler.ParseStringList (src, (uint) FragmentIdMapping.GetFragmentId ((ProductionID) syntaxTreeNode.node.id))) < 0)
                {
                    anglrLSPTarget.LogError ($"Anglr Get Compile Fragment failed: PID = {syntaxTreeNode.node.id}\nFID = {FragmentIdMapping.GetFragmentId ((ProductionID) syntaxTreeNode.node.id)}\nfragment =");
                    foreach (string s in src)
                    {
                        anglrLSPTarget.LogError (s);
                    }
                }
                return result;
            }
            catch (Exception e)
            {
                anglrLSPTarget.LogError ($"Anglr Get Item Navigation Info failed: {e.Message}\n{e.StackTrace}");
                return null;
            }
        }

        private void InsertText (TextDocumentContentChangeEvent changeEvent)
        {
        }

        private void DeleteText (TextDocumentContentChangeEvent changeEvent)
        {
        }

        private void ChangeText (TextDocumentContentChangeEvent changeEvent)
        {
        }

        private void InvokeEditChanges (object state)
        {
            Logger?.DebugLine ($"Invoke Edit Changes, current thread ID = {Thread.CurrentThread.ManagedThreadId}");
            editlist changes = null;
            lock (parserLock)
            {
                changes = editChanges;
                editChanges = new editlist ();
                foreach (TextDocumentContentChangeEvent changeEvent in changes)
                {
                    Range range = changeEvent.Range;
                    Position start = range.Start;
                    Position end = range.End;
                    try
                    {
                        if (changeEvent.RangeLength == 0)
                            InsertText (changeEvent);
                        else if (changeEvent.Text.Length == 0)
                            DeleteText (changeEvent);
                        else
                            ChangeText (changeEvent);
                        string startLine = lines [start.Line];
                        string endLine = lines [end.Line];
                        string startPiece = startLine.Substring (0, start.Character);
                        string endPiece = endLine.Substring (end.Character);
                        string replacement = startPiece + changeEvent.Text + endPiece;
                        string [] replacements = replacement.Replace ("\r", "").Split (new char [] { '\n' });
                        if ((replacements.Length == 1) && (start.Line == end.Line))
                        {
                            lines [start.Line] = replacement;
                            continue;
                        }
                        string [] newLines = new string [lines.Length + replacements.Length - (end.Line - start.Line)];
                        int index = 0, linecount = 0;
                        for (index = 0; index < start.Line; ++index)
                            newLines [linecount++] = lines [index];
                        foreach (string repl in replacements)
                            newLines [linecount++] = repl;
                        for (index = end.Line + 1; index < lines.Length; ++index)
                            newLines [linecount++] = lines [index];
                        lines = newLines;
                    }
                    catch (Exception e)
                    {
                        Logger?.ErrorLine ($"INVOKE CHANGES EXCEPTION, LC={lines.Length}, S=({start.Line}, {start.Character}, E=({end.Line}, {end.Character}): {e.Message}");
                        Logger?.ErrorLine ($"{e.Message}");
                        Logger?.ErrorLine ($"{e.StackTrace}");
                    }
                }
                Logger?.DebugLine ("before parse text");
                Parse (lines);
                Logger?.DebugLine ("after parse text");
            }
        }

        public void CreateResultSet (AnglrGetParserStateItemResultSet resultSet, AnglrParserStatesGenerator anglrParserStatesGenerator, int stateNr)
        {
            if (resultSet.Keys.Contains (stateNr))
                return;
            AnglrGetParserStateItemResult anglrGetParserStateItem = CreateAnglrGetParserStateItemResult (anglrParserStatesGenerator, stateNr);
            resultSet [stateNr] = anglrGetParserStateItem;
            foreach (AnglrGetParserStateTransitionData state in anglrGetParserStateItem.ShiftSet)
                CreateResultSet (resultSet, anglrParserStatesGenerator, state.State);
            foreach (AnglrGetParserStateTransitionData state in anglrGetParserStateItem.GotoSet)
                CreateResultSet (resultSet, anglrParserStatesGenerator, state.State);
        }

        private AnglrGetParserStateItemResult CreateAnglrGetParserStateItemResult (AnglrParserStatesGenerator anglrParserStatesGenerator, int stateNr)
        {
            RhsState rhsState = anglrParserStatesGenerator.GetRhsState (stateNr);
            AnglrGetParserStateItemResult result = new AnglrGetParserStateItemResult ();

            result.StateNumber = rhsState.m_stateNumber;

            SymbolToken symbolToken = rhsState.m_SymbolToken;
            result.SymbolToken = new AnglrGetParserStateSymbolTokenData ()
            {
                Declarator = symbolToken.declarator,
                Name = symbolToken.name,
                Synonym = (symbolToken.alias != null) ? symbolToken.alias.name : null
            };

            rhscfgset coreSet = rhsState.m_core;
            result.CoreSet = new AnglrGetParserStateCoreData [coreSet.Count];
            {
                int index = 0;
                foreach (RhsConfiguration rhsConfiguration in coreSet.Values)
                {
                    RhsProduction rhsProduction = rhsConfiguration.rhsProduction;
                    SymbolToken productionName = rhsProduction.symbolToken;
                    rhslist rhsNodes = rhsProduction.rhsNodes;
                    AnglrGetParserStateCoreData anglrGetParserStateCoreData = result.CoreSet [index++] = new AnglrGetParserStateCoreData ();
                    AnglrGetParserStateProductionData anglrGetParserStateProductionData = anglrGetParserStateCoreData.Production = new AnglrGetParserStateProductionData ();
                    {
                        anglrGetParserStateProductionData.ProductionNumber = rhsProduction.productionNumber;
                        anglrGetParserStateProductionData.ProductionName = productionName.name;
                        anglrGetParserStateProductionData.RhsNodeSet = new AnglrGetParserStateSymbolTokenData [rhsNodes.Count];
                        {
                            int rhsNodeIndex = 0;
                            foreach (RhsNode rhsNode in rhsNodes)
                            {
                                anglrGetParserStateProductionData.RhsNodeSet [rhsNodeIndex++] = new AnglrGetParserStateSymbolTokenData ()
                                {
                                    Declarator = rhsNode.symbolToken.declarator,
                                    Name = rhsNode.symbolToken.name,
                                    Synonym = (rhsNode.symbolToken.alias != null) ? rhsNode.symbolToken.alias.name : null,
                                    Id = rhsNode.symbolToken.index
                                };
                            }
                        }
                    }
                    anglrGetParserStateCoreData.Position = rhsConfiguration.rhsIterator.position;
                    tokset tokenSet = rhsConfiguration.getFollowSet.m_tokset;
                    anglrGetParserStateCoreData.FollowSet = new string [tokenSet.Count];
                    {
                        int folloowSetIndex = 0;
                        foreach (SymbolToken token in tokenSet.Values)
                            anglrGetParserStateCoreData.FollowSet [folloowSetIndex++] = token.name;
                    }
                }
            }

            closurelist closureSet = rhsState.m_closure;
            result.ClosureSet = new AnglrGetParserStateClosureData [closureSet.Count];
            {
                int index = 0;
                foreach (KeyValuePair<SymbolToken, RhsClosureElt> p in closureSet)
                {
                    SymbolToken token = p.Key;
                    RhsClosureElt rhsClosureElt = p.Value;
                    AnglrGetParserStateClosureData anglrGetParserStateClosureData = result.ClosureSet [index++] = new AnglrGetParserStateClosureData ();
                    AnglrGetParserStateProductionNodeData anglrGetParserStateProductionNodeData = anglrGetParserStateClosureData.ProductionNode = new AnglrGetParserStateProductionNodeData ();
                    anglrGetParserStateProductionNodeData.ProductionName = token.name;
                    RhsProductionNode productionNode = rhsClosureElt.rhsProductionNode;
                    productions rhsProductions = productionNode.Productions;
                    anglrGetParserStateProductionNodeData.ProductionSet = new AnglrGetParserStateProductionData [rhsProductions.Count];
                    {
                        int productionIndex = 0;
                        foreach (RhsProduction production in rhsProductions)
                        {
                            rhslist rhsNodes = production.rhsNodes;
                            AnglrGetParserStateProductionData anglrGetParserStateProductionData = anglrGetParserStateProductionNodeData.ProductionSet [productionIndex++] = new AnglrGetParserStateProductionData ();
                            {
                                anglrGetParserStateProductionData.ProductionNumber = production.productionNumber;
                                anglrGetParserStateProductionData.ProductionName = production.symbolToken.name;
                                anglrGetParserStateProductionData.RhsNodeSet = new AnglrGetParserStateSymbolTokenData [rhsNodes.Count];
                                {
                                    int rhsNodeIndex = 0;
                                    foreach (RhsNode rhsNode in rhsNodes)
                                    {
                                        anglrGetParserStateProductionData.RhsNodeSet [rhsNodeIndex++] = new AnglrGetParserStateSymbolTokenData ()
                                        {
                                            Declarator = rhsNode.symbolToken.declarator,
                                            Name = rhsNode.symbolToken.name,
                                            Synonym = (rhsNode.symbolToken.alias != null) ? rhsNode.symbolToken.alias.name : null,
                                            Id = rhsNode.symbolToken.index
                                        };
                                    }
                                }
                            }
                        }
                    }
                    tokset tokenSet = rhsClosureElt.getFollowSet.m_tokset;
                    anglrGetParserStateClosureData.FollowSet = new string [tokenSet.Count];
                    {
                        int folloowSetIndex = 0;
                        foreach (SymbolToken symbol in tokenSet.Values)
                            anglrGetParserStateClosureData.FollowSet [folloowSetIndex++] = symbol.name;
                    }
                }
            }

            statearray shiftSet = rhsState.ShiftArray;
            result.ShiftSet = new AnglrGetParserStateTransitionData [shiftSet.Count];
            {
                int index = 0;
                foreach (RhsState state in shiftSet)
                {
                    result.ShiftSet [index++] = new AnglrGetParserStateTransitionData ()
                    {
                        State = state.m_stateNumber,
                        Conflicts = state.ConflictFlags,
                        Token = (state.m_SymbolToken.alias != null) ? state.m_SymbolToken.alias.name : state.m_SymbolToken.name
                    };
                }
            }

            statearray gotoSet = rhsState.GotoArray;
            result.GotoSet = new AnglrGetParserStateTransitionData [gotoSet.Count];
            {
                int index = 0;
                foreach (RhsState state in gotoSet)
                {
                    result.GotoSet [index++] = new AnglrGetParserStateTransitionData ()
                    {
                        State = state.m_stateNumber,
                        Conflicts = state.ConflictFlags,
                        Token = state.m_SymbolToken.name
                    };
                }
            }

            prodset reductions = rhsState.m_reductions;
            result.ReductionsSet = new AnglrGetParserStateReductionsData [reductions.Count];
            {
                int index = 0;
                foreach (KeyValuePair<int, RhsConfiguration> p in reductions)
                {
                    int productionNumber = p.Key;
                    RhsConfiguration rhsConfiguration = p.Value;
                    RhsProduction rhsProduction = rhsConfiguration.rhsProduction;
                    rhslist rhsNodes = rhsProduction.rhsNodes;
                    AnglrGetParserStateReductionsData anglrGetParserStateReductionsData = result.ReductionsSet [index++] = new AnglrGetParserStateReductionsData ();
                    anglrGetParserStateReductionsData.Production = new AnglrGetParserStateProductionData ();
                    AnglrGetParserStateProductionData anglrGetParserStateProductionData = anglrGetParserStateReductionsData.Production = new AnglrGetParserStateProductionData ();
                    {
                        anglrGetParserStateProductionData.ProductionNumber = rhsProduction.productionNumber;
                        anglrGetParserStateProductionData.ProductionName = rhsProduction.symbolToken.name;
                        anglrGetParserStateProductionData.RhsNodeSet = new AnglrGetParserStateSymbolTokenData [rhsNodes.Count];
                        {
                            int rhsNodeIndex = 0;
                            foreach (RhsNode rhsNode in rhsNodes)
                            {
                                anglrGetParserStateProductionData.RhsNodeSet [rhsNodeIndex++] = new AnglrGetParserStateSymbolTokenData ()
                                {
                                    Declarator = rhsNode.symbolToken.declarator,
                                    Name = rhsNode.symbolToken.name,
                                    Synonym = (rhsNode.symbolToken.alias != null) ? rhsNode.symbolToken.alias.name : null,
                                    Id = rhsNode.symbolToken.index
                                };
                            }
                        }
                    }
                    tokset tokens = rhsConfiguration.getFollowSet.m_tokset;
                    anglrGetParserStateReductionsData.FollowSet = new string [tokens.Count];
                    {
                        int folloowSetIndex = 0;
                        foreach (SymbolToken symbol in tokens.Values)
                            anglrGetParserStateReductionsData.FollowSet [folloowSetIndex++] = (symbol.alias != null) ? symbol.alias.name : symbol.name;
                    }
                }
            }

            Logger?.DebugLine ($"AnglrGetParserStateItemResult = {JsonConvert.SerializeObject (result)}");
            return result;
        }

        private AnglrGetParserStateLinkResult CreateAnglrGetParserStateLinkResult (AnglrParserStatesGenerator anglrParserStatesGenerator, int stateNr)
        {
            Stack<int> states = new Stack<int> ();
            for (RhsState rhsState = anglrParserStatesGenerator.GetRhsState (stateNr); rhsState != null; rhsState = rhsState.RhsStateTreeLink)
                states.Push (rhsState.m_stateNumber);
            return new AnglrGetParserStateLinkResult ()
            {
                StateLinks = states.ToArray ()
            };
        }

        private AnglrGetParserSyntaxRuleResult CreateAnglrGetParserSyntaxRuleResult (AnglrParserStatesGenerator anglrParserStatesGenerator, string ruleName)
        {
            AnglrGetParserSyntaxRuleData anglrGetParserSyntaxRuleData = new AnglrGetParserSyntaxRuleData ();
            anglrParserStatesGenerator.InvokeProductionsIterator
            (
                (symbol, node) =>
                {
                    if (symbol.name != ruleName)
                        return 0;
                    anglrGetParserSyntaxRuleData = new AnglrGetParserSyntaxRuleData ()
                    {
                        SyntaxRuleName = new AnglrGetParserStateSymbolTokenData ()
                        {
                            Declarator = symbol.declarator,
                            Name = symbol.name,
                            Synonym = (symbol.alias != null) ? symbol.alias.name : null,
                            Id = symbol.index,
                        },
                        Productions = new AnglrGetParserStateProductionData [node.Productions.Count]
                    };
                    int productionCount = 0;
                    foreach (var production in node.Productions)
                    {
                        AnglrGetParserStateProductionData anglrGetParserStateProductionData = new AnglrGetParserStateProductionData ()
                        {
                            ProductionName = (production.productionName != null) ? production.productionName.name : null,
                            ProductionNumber = production.productionNumber,
                            RhsNodeSet = new AnglrGetParserStateSymbolTokenData [production.rhsNodes.Count]
                        };
                        int rhsCount = 0;
                        foreach (var rhsNode in production.rhsNodes)
                        {
                            AnglrGetParserStateSymbolTokenData anglrGetParserStateSymbolTokenData = new AnglrGetParserStateSymbolTokenData ()
                            {
                                Declarator = rhsNode.symbolToken.declarator,
                                Name = rhsNode.symbolToken.name,
                                Id = rhsNode.symbolToken.index,
                                Synonym = (rhsNode.symbolToken.alias != null) ? rhsNode.symbolToken.alias.name : null,
                            };
                            anglrGetParserStateProductionData.RhsNodeSet [rhsCount++] = anglrGetParserStateSymbolTokenData;
                        }
                        anglrGetParserSyntaxRuleData.Productions [productionCount++] = anglrGetParserStateProductionData;
                    }
                    return 0;
                }
            );
            return new AnglrGetParserSyntaxRuleResult ()
            {
                SyntaxRule = anglrGetParserSyntaxRuleData
            };
        }

        private AnglrGetParserSyntaxRulesResult CreateAnglrGetParserSyntaxRulesResult (AnglrParserStatesGenerator anglrParserStatesGenerator)
        {
            List<AnglrGetParserSyntaxRuleData> anglrGetParserSyntaxRuleDatas = new List<AnglrGetParserSyntaxRuleData> ();
            anglrParserStatesGenerator.InvokeProductionsIterator
            (
                (symbol, node) =>
                {
                    AnglrGetParserSyntaxRuleData anglrGetParserSyntaxRuleData = new AnglrGetParserSyntaxRuleData ()
                    {
                        SyntaxRuleName = new AnglrGetParserStateSymbolTokenData ()
                        {
                            Declarator = symbol.declarator,
                            Name = symbol.name,
                            Synonym = (symbol.alias != null) ? symbol.alias.name : null,
                            Id = symbol.index,
                        },
                        Productions = new AnglrGetParserStateProductionData [node.Productions.Count]
                    };
                    int productionCount = 0;
                    foreach (var production in node.Productions)
                    {
                        AnglrGetParserStateProductionData anglrGetParserStateProductionData = new AnglrGetParserStateProductionData ()
                        {
                            ProductionName = (production.productionName != null) ? production.productionName.name : null,
                            ProductionNumber = production.productionNumber,
                            RhsNodeSet = new AnglrGetParserStateSymbolTokenData [production.rhsNodes.Count]
                        };
                        int rhsCount = 0;
                        foreach (var rhsNode in production.rhsNodes)
                        {
                            AnglrGetParserStateSymbolTokenData anglrGetParserStateSymbolTokenData = new AnglrGetParserStateSymbolTokenData ()
                            {
                                Declarator = rhsNode.symbolToken.declarator,
                                Name = rhsNode.symbolToken.name,
                                Id = rhsNode.symbolToken.index,
                                Synonym = (rhsNode.symbolToken.alias != null) ? rhsNode.symbolToken.alias.name : null,
                            };
                            anglrGetParserStateProductionData.RhsNodeSet [rhsCount++] = anglrGetParserStateSymbolTokenData;
                        }
                        anglrGetParserSyntaxRuleData.Productions [productionCount++] = anglrGetParserStateProductionData;
                    }
                    anglrGetParserSyntaxRuleDatas.Add (anglrGetParserSyntaxRuleData);
                    return 0;
                }
            );
            return new AnglrGetParserSyntaxRulesResult ()
            {
                SyntaxRuleList = anglrGetParserSyntaxRuleDatas.ToArray ()
            };
        }

        private void CheckStateConflicts (AnglrParserStatesGenerator anglrParserStatesGenerator)
        {
            AnglrGetParserSyntaxRulesResult anglrGetParserSyntaxRulesResult = CreateAnglrGetParserSyntaxRulesResult (anglrParserStatesGenerator);
            Dictionary<int, AnglrGetParserSyntaxRuleData> keyValuePairs = anglrGetParserSyntaxRulesResult.SyntaxRuleList.ToDictionary (x=>x.SyntaxRuleName.Id);
            this.anglrParserStatesGenerator.InvokeStatesIterator
            (
                (state) =>
                {
                    if (state.ConflictFlags == 0)
                        return 0;
                    foreach (var reduction in state.m_reductions)
                    {
                        int prodNr = reduction.Key;
                        RhsConfiguration rhsConfiguration = reduction.Value;
                        RhsProduction rhsProduction = rhsConfiguration.rhsProduction;
                        rhsenumerator rhsenumerator = rhsConfiguration.rhsIterator;
                        TokenSet tokenSet = rhsConfiguration.getFollowSet;
                    }
                    return 0;
                }
            );
        }

        public void Dispose ()
        {
            Logger?.DebugLine ($"dispose '{fileName}'");
            foreach (SyntaxTreeBase node in syntaxTrees)
            {
                ((_anglr_file_fragment_) node).Dispose ();
            }
        }
    }

    internal class AnglrDocDictionary : SortedDictionary<string, AnglrDocContext>
    {
        public AnglrDocDictionary ()
        {

        }
        public void Add (AnglrDocContext docCtx)
        {
            lock (lockKey)
            {
                base [docCtx.fileName] = docCtx;
            }
        }
        public AnglrDocContext Find (string fileName)
        {
            lock (lockKey)
            {
                AnglrDocContext docCtx = null;
                return base.TryGetValue (fileName, out docCtx) ? docCtx : null;
            }
        }
        public int Remove (string fileName)
        {
            lock (lockKey)
            {
                base [fileName].Dispose ();
                base.Remove (fileName);
                return base.Count;
            }
        }
        object lockKey = new object ();
    }
}
