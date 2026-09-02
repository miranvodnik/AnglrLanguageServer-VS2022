using Anglr.Declarations;
using Anglr.ScannerLib;
using Anglr.Lexer;
using Anglr.Parser;
using Anglr.Parser.Core;
using Anglr.Parser.SyntaxTree;
using AnglrLibrary;
using AnglrLogLibrary;
using System;
using System.Collections.Generic;
using System.IO;
using System.Runtime.CompilerServices;

namespace Anglr.Compiler
{
    public class AnglrNonSyntaxContent
    {
        public AnglrNonSyntaxContent (int lineNr, int columnNr)
        {
            LineNr = lineNr;
            ColumnNr = columnNr;
        }

        public void Add (string content) => Content += content;
        public string Content { get; private set; } = default;
        public int LineNr { get; private set; } = -1;
        public int ColumnNr { get; private set; } = -1;
    }
    internal class CmpNSCKey : IComparer<(int, int)>
    {
        public int Compare ((int, int) x, (int, int) y)
        {
            int diff = x.Item1 - y.Item1;
            if (diff != 0)
                return diff;
            return x.Item2 - y.Item2;
        }
    }

    public class AnglrNSCInfo : List<AnglrNonSyntaxContent> { }

    public class AnglrNSCSet : SortedDictionary<(int, int), AnglrNSCInfo>
    {
        public AnglrNSCSet () : base (new CmpNSCKey ()) { }
    }

    public class anglrCompiler : AnglrParser
    {
        public IAnglrLogger logger { get; private set; }
        int startTerminal = -1;
        AnglrLexer lexer = null;
        public int lineCounter { get; private set; } = -1;
        public string sourceFileName { get; private set; } = default;
        public SymbolTable symbolTable { get; private set; } = new SymbolTable ();
        public _anglr_syntax_rule_ startSyntaxRule { get; private set; } = default;
        private AnglrNonSyntaxContent CommentString { get; set; } = default;
        private AnglrNonSyntaxContent LineCommentString { get; set; } = default;
        private AnglrNonSyntaxContent AttributeString { get; set; } = default;
        private AnglrNSCInfo AnglrNonSyntaxContents { get; set; } = new AnglrNSCInfo ();
        private AnglrNSCSet anglrNSCSet { get; set; } = new AnglrNSCSet ();

        private SymbolToken contextSymbol = null;
        private SymbolToken generalPartSymbol = null;
        private SymbolToken declarationPartSymbol = null;
        private SymbolToken scannerPartSymbol = null;
        private SymbolToken parserPartSymbol = null;
        private SymbolToken syntaxRuleSymbol = null;
        private SymbolToken attributeSymbol = null;

        private bool buildEvent = false;
        public static bool createPrecedenceGrammar { get; set; } = false;
        public static bool createIterators { get; set; } = false;

        public AnglrNSCInfo FindNSCInfo ((int lineno, int column) key)
        {
            AnglrNSCInfo val;
            return anglrNSCSet.TryGetValue (key, out val) ? val : default;
        }

        public anglrCompiler (string fragmentName = null, IAnglrLogger logger = null) : base (fragmentName, logger)
        {
            this.logger = logger ?? new VoidAnglrLogger ();
            sourceFileName = "";
            InitParser ();
        }

        private void InitParser ()
        {
            //  builder action
            _anglr_file__Event += Invoke__anglr_file__Callback;

            //  symbol table collectors
            _attribute__Event += Invoke__attribute__Callback;
            _name_value_pair__Event += Invoke__name_value_pair__Callback;
            _general_part__Event += Invoke__general_part__Callback;
            _declaration_part__Event += Invoke__declaration_part__Callback;
            _terminal_definition__Event += Invoke__terminal_definition__Callback;
            _regex_definition__Event += Invoke__regex_definition__Callback;
            _scanner_part__Event += Invoke__scanner_part__Callback;
            _lexer_part__Event += Invoke__lexer_part__Callback;
            _parser_part__Event += Invoke__parser_part__Callback;
            _anglr_syntax_rule__Event += Invoke__anglr_syntax_rule__Callback;
            _anglr_syntax_production_list_name__Event += Invoke__anglr_syntax_production_list_name__Callback;
            _production_name__Event += Invoke__production_name__Callback;
            _marker__Event += Invoke__marker__Callback;

            //  iterators for recursive rules
            //_attribute_list__Event += Invoke__attribute_list__Callback;
            //_name_value_list__Event += Invoke__name_value_list__Callback;
            //_anglr_definition_list__Event += Invoke__anglr_definition_list__Callback;
            //_block_terminal_definitions__Event += Invoke__block_terminal_definitions__Callback;
            //_block_regex_definitions__Event += Invoke__block_regex_definitions__Callback;
            //_regular_expression_list__Event += Invoke__regular_expression_list__Callback;
            //_actions__Event += Invoke__actions__Callback;
            //_anglr_syntax_rule_list__Event += Invoke__anglr_syntax_rule_list__Callback;
            //_anglr_syntax_production_list__Event += Invoke__anglr_syntax_production_list__Callback;
            //_name_list__Event += Invoke__name_list__Callback;
            //_marker_list__Event += Invoke__marker_list__Callback;
        }

        public int Parse (string fileName, uint startToken, object [] info = null)
        {
            try
            {
                this.startTerminal = (int) startToken;
                sourceFileName = fileName;
                lexer = new AnglrLexer (new StreamReader (sourceFileName), info);

                lexer.scannerEnterEvent += Scanner_scannerEnterEvent;
                lexer.scannerLeaveEvent += Scanner_scannerLeaveEvent;
                lexer.scannerPushEvent += Scanner_scannerPushEvent;
                lexer.scannerPopEvent += Scanner_scannerPopEvent;
                lexer.scannerTokenEvent += Scanner_scannerTokenEvent;
                return parse (lexer);
            }
            catch (Exception e)
            {
                int i = 0;
                while (e != null)
                {
                    logger?.ErrorRawLine ($"exception ({i}): {e.Message}");
                    logger?.ErrorRawLine ($"exception ({i}): stack trace: {e.StackTrace}");
                    e = e.InnerException;
                    ++i;
                }
                return -1;
            }
            finally
            {
                lineCounter = (lexer != null) ? lexer.lineno : 0;
            }
        }

        public int ParseString (string str, uint startToken, object [] info = null)
        {
            try
            {
                this.startTerminal = (int) startToken;
                lexer = new AnglrLexer (str, info);
                lexer.scannerEnterEvent += Scanner_scannerEnterEvent;
                lexer.scannerLeaveEvent += Scanner_scannerLeaveEvent;
                lexer.scannerPushEvent += Scanner_scannerPushEvent;
                lexer.scannerPopEvent += Scanner_scannerPopEvent;
                lexer.scannerTokenEvent += Scanner_scannerTokenEvent;
                return parse (lexer);
            }
            catch (Exception e)
            {
                logger?.ErrorRawLine (e.Message);
                return -1;
            }
            finally
            {
                lineCounter = lexer.lineno;
            }
        }

        public int ParseStringList (string [] lines, uint startToken, object [] info = null)
        {
            try
            {
                this.startTerminal = (int) startToken;
                lexer = new AnglrLexer (lines, info);
                lexer.scannerEnterEvent += Scanner_scannerEnterEvent;
                lexer.scannerLeaveEvent += Scanner_scannerLeaveEvent;
                lexer.scannerPushEvent += Scanner_scannerPushEvent;
                lexer.scannerPopEvent += Scanner_scannerPopEvent;
                lexer.scannerTokenEvent += Scanner_scannerTokenEvent;
                return parse (lexer);
            }
            catch (Exception e)
            {
                logger?.ErrorRawLine (e.Message);
                return -1;
            }
            finally
            {
                lineCounter = lexer.lineno;
            }
        }

        private void PrepareFragmentParse (int terminal)
        {
            switch (terminal)
            {
                case AnglrDeclarations.tokens._anglr_file_terminal_:
                    break;
                case AnglrDeclarations.tokens._anglr_file_part_list_terminal_:
                    break;
                case AnglrDeclarations.tokens._anglr_file_part_terminal_:
                    break;
                case AnglrDeclarations.tokens._general_part_terminal_:
                    break;
                case AnglrDeclarations.tokens._declaration_part_terminal_:
                    break;
                case AnglrDeclarations.tokens._scanner_part_terminal_:
                    break;
                case AnglrDeclarations.tokens._regular_expression_list_terminal_:
                    break;
                case AnglrDeclarations.tokens._regular_expression_usage_terminal_:
                    lexer.pushScanner (ScannerPartScanner.Id, "");
                    break;
                case AnglrDeclarations.tokens._actions_terminal_:
                    break;
                case AnglrDeclarations.tokens._action_terminal_:
                    break;
                case AnglrDeclarations.tokens._skip_action_terminal_:
                    lexer.pushScanner (ScannerPartScanner.Id, "");
                    break;
                case AnglrDeclarations.tokens._terminal_action_terminal_:
                    lexer.pushScanner (ScannerPartScanner.Id, "");
                    break;
                case AnglrDeclarations.tokens._event_action_terminal_:
                    lexer.pushScanner (ScannerPartScanner.Id, "");
                    break;
                case AnglrDeclarations.tokens._push_action_terminal_:
                    lexer.pushScanner (ScannerPartScanner.Id, "");
                    break;
                case AnglrDeclarations.tokens._pop_action_terminal_:
                    lexer.pushScanner (ScannerPartScanner.Id, "");
                    break;
                case AnglrDeclarations.tokens._lexer_part_terminal_:
                    break;
                case AnglrDeclarations.tokens._parser_part_terminal_:
                    break;
                case AnglrDeclarations.tokens._attribute_list_terminal_:
                    break;
                case AnglrDeclarations.tokens._attribute_terminal_:
                    break;
                case AnglrDeclarations.tokens._name_value_list_terminal_:
                    break;
                case AnglrDeclarations.tokens._name_value_pair_terminal_:
                    break;
                case AnglrDeclarations.tokens._anglr_definition_list_terminal_:
                    break;
                case AnglrDeclarations.tokens._anglr_definition_with_attribute_list_terminal_:
                    break;
                case AnglrDeclarations.tokens._anglr_definition_terminal_:
                    break;
                case AnglrDeclarations.tokens._single_terminal_definition_terminal_:
                    break;
                case AnglrDeclarations.tokens._single_regex_definition_terminal_:
                    break;
                case AnglrDeclarations.tokens._block_of_terminal_definitions_terminal_:
                    break;
                case AnglrDeclarations.tokens._block_of_regex_definitions_terminal_:
                    break;
                case AnglrDeclarations.tokens._terminal_definition_terminal_:
                    break;
                case AnglrDeclarations.tokens._regex_definition_terminal_:
                    lexer.pushScanner (RegexBlockScanner.Id, "");
                    break;
                case AnglrDeclarations.tokens._block_terminal_definitions_terminal_:
                    break;
                case AnglrDeclarations.tokens._block_terminal_definition_terminal_:
                    break;
                case AnglrDeclarations.tokens._block_regex_definitions_terminal_:
                    break;
                case AnglrDeclarations.tokens._block_regex_definition_terminal_:
                    break;
                case AnglrDeclarations.tokens._anglr_syntax_rule_list_terminal_:
                    break;
                case AnglrDeclarations.tokens._anglr_syntax_rule_terminal_:
                    break;
                case AnglrDeclarations.tokens._anglr_syntax_production_list_terminal_:
                    break;
                case AnglrDeclarations.tokens._anglr_syntax_production_terminal_:
                    break;
                case AnglrDeclarations.tokens._priority_assoc_specification_terminal_:
                    break;
                case AnglrDeclarations.tokens._priority_specification_terminal_:
                    break;
                case AnglrDeclarations.tokens._associativity_specification_terminal_:
                    break;
                case AnglrDeclarations.tokens._anglr_nested_rule_terminal_:
                    break;
                case AnglrDeclarations.tokens._anglr_syntax_production_list_name_terminal_:
                    break;
                case AnglrDeclarations.tokens._name_list_terminal_:
                    break;
                case AnglrDeclarations.tokens._production_name_terminal_:
                    break;
                case AnglrDeclarations.tokens._marker_list_terminal_:
                    break;
                case AnglrDeclarations.tokens._marker_terminal_:
                    break;
                case AnglrDeclarations.tokens._g_name_terminal_:
                    break;
                case AnglrDeclarations.tokens._name_terminal_:
                    break;
                case AnglrDeclarations.tokens._cardinality_delimiter_terminal_:
                    break;
                case AnglrDeclarations.tokens._cardinality_terminal_:
                    break;
                case AnglrDeclarations.tokens._delimiter_terminal_:
                    break;
            }
        }

        private int Scanner_scannerEnterEvent ()
        {
            int terminal = (ProductionID <= 0) ? startTerminal : -1;
            PrepareFragmentParse (terminal);
            startTerminal = -1;
            return terminal;
        }

        private void Scanner_scannerLeaveEvent (int terminal)
        {
            //logger.LogDebug($"(line: {scanner.lineno,5:d}, column: {scanner.column,5:d}, scanner: {scanner.regexIndex,5:d}, case: {scanner.scase,5:d}, terminal: {terminal,5:d}): {scanner.text}");
        }

        private void Scanner_scannerPushEvent (int oldCtx, int newCtx, string text)
        {
            switch (newCtx)
            {
                case AnglrLexer.comment_scanner:
                    CommentString = new AnglrNonSyntaxContent (lexer.lineno, lexer.column);
                    break;
                case AnglrLexer.line_comment_scanner:
                    LineCommentString = new AnglrNonSyntaxContent (lexer.lineno, lexer.column);
                    break;
                case AnglrLexer.attribute_scanner:
                    AttributeString = new AnglrNonSyntaxContent (lexer.lineno, lexer.column);
                    break;
            }
        }

        private void Scanner_scannerPopEvent (int oldCtx, int newCtx, string text)
        {
            switch (oldCtx)
            {
                case AnglrLexer.comment_scanner:
                    CommentString.Add (text);
                    AnglrNonSyntaxContents.Add (CommentString);
                    break;
                case AnglrLexer.line_comment_scanner:
                    LineCommentString.Add (text);
                    AnglrNonSyntaxContents.Add (LineCommentString);
                    break;
                case AnglrLexer.attribute_scanner:
                    AttributeString.Add (text);
                    AnglrNonSyntaxContents.Add (AttributeString);
                    break;
            }
        }

        private void Scanner_scannerTokenEvent (int ctx, int token, string text)
        {
            switch (ctx)
            {
                case AnglrLexer.comment_scanner:
                    CommentString.Add (text);
                    break;
                case AnglrLexer.line_comment_scanner:
                    LineCommentString.Add (text);
                    break;
                case AnglrLexer.attribute_scanner:
                    AttributeString.Add (text);
                    break;
                default:
                    if (token <= 0)
                        break;
                    if (AnglrNonSyntaxContents.Count == 0)
                        break;
                    anglrNSCSet [(lexer.lineno, lexer.column)] = AnglrNonSyntaxContents;
                    AnglrNonSyntaxContents = new AnglrNSCInfo ();
                    break;
            }
        }

        private bool Invoke__anglr_file__Callback (SyntaxTreeCallbackReason reason, _anglr_file_.production_kind kind, _anglr_file_ p__anglr_file_)
        {
            if (reason == SyntaxTreeCallbackReason.BuilderCallbackReason)
            {
                buildEvent = true;
                Traverse (p__anglr_file_.m__anglr_file_part_list_);
            }
            return false;
        }

        private bool Invoke__attribute__Callback (SyntaxTreeCallbackReason reason, _attribute_.production_kind kind, _attribute_ p__attribute_)
        {
            switch (reason)
            {
                case SyntaxTreeCallbackReason.BuilderCallbackReason:
                    return buildEvent;
                    break;
                case SyntaxTreeCallbackReason.TraversalPrologueCallbackReason:
                {
                    SyntaxTreeToken syntaxTreeToken = p__attribute_.m__identifier_;
                    SymbolToken symbolToken = new SymbolToken (syntaxTreeToken.text, (uint) AnglrClassificationType.AttributeName, contextSymbol);
                    SymbolToken symbolTokenRef = symbolTable.insert (symbolToken);
                    if (symbolTokenRef != symbolToken)
                    {
                        symbolToken.tag = symbolTokenRef.tag;
                        symbolTokenRef.tag = symbolToken;
                    }
                    attributeSymbol = symbolTokenRef;
                }
                break;
                case SyntaxTreeCallbackReason.TraversalMidTermCallbackReason:
                    break;
                case SyntaxTreeCallbackReason.TraversalEpilogueCallbackReason:
                    break;
            }
            return true;
        }

        private bool Invoke__name_value_pair__Callback (SyntaxTreeCallbackReason reason, _name_value_pair_.production_kind kind, _name_value_pair_ p__name_value_pair_)
        {
            switch (reason)
            {
                case SyntaxTreeCallbackReason.BuilderCallbackReason:
                    return buildEvent;
                    break;
                case SyntaxTreeCallbackReason.TraversalPrologueCallbackReason:
                {
                    SyntaxTreeToken syntaxTreeToken = p__name_value_pair_.m__identifier_;
                    SymbolToken symbolToken = new SymbolToken (syntaxTreeToken.text, (uint) AnglrClassificationType.PropertyName, attributeSymbol);
                    SymbolToken symbolTokenRef = symbolTable.insert (symbolToken);
                    if (symbolTokenRef != symbolToken)
                    {
                        symbolToken.tag = symbolTokenRef.tag;
                        symbolTokenRef.tag = symbolToken;
                    }
                }
                break;
                case SyntaxTreeCallbackReason.TraversalMidTermCallbackReason:
                    break;
                case SyntaxTreeCallbackReason.TraversalEpilogueCallbackReason:
                    break;
            }
            return true;
        }

        private bool Invoke__general_part__Callback (SyntaxTreeCallbackReason reason, _general_part_.production_kind kind, _general_part_ p__general_part_)
        {
            bool status = true;
            switch (reason)
            {
                case SyntaxTreeCallbackReason.BuilderCallbackReason:
                    return buildEvent;
                    break;
                case SyntaxTreeCallbackReason.TraversalPrologueCallbackReason:
                {
                    SyntaxTreeToken syntaxTreeToken = p__general_part_.m__identifier_;
                    SymbolToken symbolToken = new SymbolToken (syntaxTreeToken.text, (uint) AnglrClassificationType.GeneralPartName);
                    SymbolToken symbolTokenRef = symbolTable.insert (symbolToken);
                    if (symbolTokenRef != symbolToken)
                    {
                        logger?.ErrorRawLine ($"Error, at ({syntaxTreeToken.lineno}, {syntaxTreeToken.column}): redefinition of %general part name {syntaxTreeToken.text}");
                        symbolToken.Dispose ();
                    }
                    contextSymbol = generalPartSymbol = symbolTokenRef;
                }
                break;
                case SyntaxTreeCallbackReason.TraversalMidTermCallbackReason:
                    break;
                case SyntaxTreeCallbackReason.TraversalEpilogueCallbackReason:
                    break;
            }
            return status;
        }

        private bool Invoke__declaration_part__Callback (SyntaxTreeCallbackReason reason, _declaration_part_.production_kind kind, _declaration_part_ p__declaration_part_)
        {
            bool status = true;
            switch (reason)
            {
                case SyntaxTreeCallbackReason.BuilderCallbackReason:
                    return buildEvent;
                    break;
                case SyntaxTreeCallbackReason.TraversalPrologueCallbackReason:
                {
                    SyntaxTreeToken syntaxTreeToken = p__declaration_part_.m__identifier_;
                    SymbolToken symbolToken = new SymbolToken (syntaxTreeToken.text, (uint) AnglrClassificationType.DeclarationsPartName);
                    SymbolToken symbolTokenRef = symbolTable.insert (symbolToken);
                    if (symbolTokenRef != symbolToken)
                    {
                        logger?.ErrorRawLine ($"Error, at ({syntaxTreeToken.lineno}, {syntaxTreeToken.column}): redefinition of %declaration part name {syntaxTreeToken.text}");
                        symbolToken.Dispose ();
                    }
                    contextSymbol = declarationPartSymbol = symbolTokenRef;
                }
                break;
                case SyntaxTreeCallbackReason.TraversalMidTermCallbackReason:
                    break;
                case SyntaxTreeCallbackReason.TraversalEpilogueCallbackReason:
                    break;
            }
            return status;
        }

        private bool Invoke__scanner_part__Callback (SyntaxTreeCallbackReason reason, _scanner_part_.production_kind kind, _scanner_part_ p__scanner_part_)
        {
            bool status = true;
            switch (reason)
            {
                case SyntaxTreeCallbackReason.BuilderCallbackReason:
                    return buildEvent;
                    break;
                case SyntaxTreeCallbackReason.TraversalPrologueCallbackReason:
                {
                    SyntaxTreeToken syntaxTreeToken = p__scanner_part_.m__identifier_;
                    SymbolToken symbolToken = new SymbolToken (syntaxTreeToken.text, (uint) AnglrClassificationType.ScannerPartName);
                    SymbolToken symbolTokenRef = symbolTable.insert (symbolToken);
                    if (symbolTokenRef != symbolToken)
                    {
                        logger?.ErrorRawLine ($"Error, at ({syntaxTreeToken.lineno}, {syntaxTreeToken.column}): redefinition of %scanner part name {syntaxTreeToken.text}");
                        symbolToken.Dispose ();
                    }
                    contextSymbol = scannerPartSymbol = symbolTokenRef;
                }
                break;
                case SyntaxTreeCallbackReason.TraversalMidTermCallbackReason:
                    break;
                case SyntaxTreeCallbackReason.TraversalEpilogueCallbackReason:
                    break;
            }
            return status;
        }

        private bool Invoke__lexer_part__Callback (SyntaxTreeCallbackReason reason, _lexer_part_.production_kind kind, _lexer_part_ p__lexer_part_)
        {
            bool status = true;
            switch (reason)
            {
                case SyntaxTreeCallbackReason.BuilderCallbackReason:
                    return buildEvent;
                    break;
                case SyntaxTreeCallbackReason.TraversalPrologueCallbackReason:
                {
                    SyntaxTreeToken syntaxTreeToken = p__lexer_part_.m__identifier_;
                    SymbolToken symbolToken = new SymbolToken (syntaxTreeToken.text, (uint) AnglrClassificationType.LexerPartName);
                    SymbolToken symbolTokenRef = symbolTable.insert (symbolToken);
                    if (symbolTokenRef != symbolToken)
                    {
                        logger?.ErrorRawLine ($"Error, at ({syntaxTreeToken.lineno}, {syntaxTreeToken.column}): redefinition of %lexer part name {syntaxTreeToken.text}");
                        symbolToken.Dispose ();
                    }
                    contextSymbol = parserPartSymbol = symbolTokenRef;
                }
                break;
                case SyntaxTreeCallbackReason.TraversalMidTermCallbackReason:
                    break;
                case SyntaxTreeCallbackReason.TraversalEpilogueCallbackReason:
                    break;
            }
            return status;
        }

        private bool Invoke__parser_part__Callback (SyntaxTreeCallbackReason reason, _parser_part_.production_kind kind, _parser_part_ p__parser_part_)
        {
            bool status = true;
            switch (reason)
            {
                case SyntaxTreeCallbackReason.BuilderCallbackReason:
                    return buildEvent;
                    break;
                case SyntaxTreeCallbackReason.TraversalPrologueCallbackReason:
                {
                    SyntaxTreeToken syntaxTreeToken = p__parser_part_.m__identifier_;
                    SymbolToken symbolToken = new SymbolToken (syntaxTreeToken.text, (uint) AnglrClassificationType.ParserPartName);
                    SymbolToken symbolTokenRef = symbolTable.insert (symbolToken);
                    if (symbolTokenRef != symbolToken)
                    {
                        logger?.ErrorRawLine ($"Error, at ({syntaxTreeToken.lineno}, {syntaxTreeToken.column}): redefinition of %parser part name {syntaxTreeToken.text}");
                        symbolToken.Dispose ();
                    }
                    contextSymbol = parserPartSymbol = symbolTokenRef;
                }
                break;
                case SyntaxTreeCallbackReason.TraversalMidTermCallbackReason:
                    break;
                case SyntaxTreeCallbackReason.TraversalEpilogueCallbackReason:
                    break;
            }
            return status;
        }

        private bool Invoke__terminal_definition__Callback (SyntaxTreeCallbackReason reason, _terminal_definition_.production_kind kind, _terminal_definition_ p__terminal_definition_)
        {
            bool status = true;
            switch (reason)
            {
                case SyntaxTreeCallbackReason.BuilderCallbackReason:
                    return buildEvent;
                    break;
                case SyntaxTreeCallbackReason.TraversalPrologueCallbackReason:
                {
                    SyntaxTreeToken syntaxTreeToken = p__terminal_definition_.m__identifier_;
                    SyntaxTreeToken p__cstring_ = p__terminal_definition_.m__cstring_optional_.m__cstring_;
                    SymbolToken symbolToken = new SymbolToken (syntaxTreeToken.text, (uint) AnglrClassificationType.TerminalName, null, declarationPartSymbol);
                    SymbolToken symbolTokenRef = symbolTable.insert (symbolToken);
                    if (symbolTokenRef != symbolToken)
                    {
                        logger?.ErrorRawLine ($"Error, at ({syntaxTreeToken.lineno}, {syntaxTreeToken.column}): redefinition of terminal name {syntaxTreeToken.text}");
                        symbolToken.Dispose ();
                    }
                    if (p__cstring_ != null)
                    {
                        SymbolToken aliasToken = new SymbolToken (p__cstring_.text, (uint) AnglrClassificationType.TerminalName, null, declarationPartSymbol, false);
                        SymbolToken aliasTokenRef = symbolTable.insert (aliasToken);
                        if (aliasTokenRef != aliasToken)
                        {
                            logger?.ErrorRawLine ($"Error, at ({p__cstring_.lineno}, {p__cstring_.column}): redefinition of terminal string {p__cstring_.text}");
                            aliasToken.Dispose ();
                        }
                        symbolTokenRef.alias = aliasTokenRef;
                        aliasTokenRef.alias = symbolTokenRef;
                        aliasTokenRef.AliasFlag = true;
                    }
                }
                break;
                case SyntaxTreeCallbackReason.TraversalMidTermCallbackReason:
                    break;
                case SyntaxTreeCallbackReason.TraversalEpilogueCallbackReason:
                    break;
            }
            return status;
        }

        private bool Invoke__regex_definition__Callback (SyntaxTreeCallbackReason reason, _regex_definition_.production_kind kind, _regex_definition_ p__regex_definition_)
        {
            bool status = true;
            switch (reason)
            {
                case SyntaxTreeCallbackReason.BuilderCallbackReason:
                    return buildEvent;
                    break;
                case SyntaxTreeCallbackReason.TraversalPrologueCallbackReason:
                    break;
                case SyntaxTreeCallbackReason.TraversalMidTermCallbackReason:
                    break;
                case SyntaxTreeCallbackReason.TraversalEpilogueCallbackReason:
                {
                    SyntaxTreeToken syntaxTreeToken = p__regex_definition_.m__identifier_;
                    SymbolToken symbolToken = new SymbolToken (syntaxTreeToken.text, (uint) AnglrClassificationType.RegexName, null, declarationPartSymbol);
                    SymbolToken symbolTokenRef = symbolTable.insert (symbolToken);
                    if (symbolTokenRef != symbolToken)
                    {
                        logger?.ErrorRawLine ($"Error, at ({syntaxTreeToken.lineno}, {syntaxTreeToken.column}): redefinition of %regex symbol name {syntaxTreeToken.text}");
                        symbolToken.Dispose ();
                    }
                }
                break;
            }
            return status;
        }

        private bool Invoke__anglr_syntax_rule__Callback (SyntaxTreeCallbackReason reason, _anglr_syntax_rule_.production_kind kind, _anglr_syntax_rule_ p__anglr_syntax_rule_)
        {
            bool status = true;
            switch (reason)
            {
                case SyntaxTreeCallbackReason.BuilderCallbackReason:
                {
                    if (startSyntaxRule != null)
                        break;
                    _attribute_list_optional_ attribute_List_Optional_ = p__anglr_syntax_rule_.m__attribute_list_optional_;
                    _attribute_list_ attribute_List_ = attribute_List_Optional_.m__attribute_list_;
                    if (attribute_List_ == null)
                        break;
                    if ((bool) attribute_List_.Iterate (false, (node, appData) => (bool) appData || (node.m__attribute_.m__identifier_.text == "Start")))
                        startSyntaxRule = p__anglr_syntax_rule_;
                    return buildEvent;
                }
                break;
                case SyntaxTreeCallbackReason.TraversalPrologueCallbackReason:
                {
                    switch (kind)
                    {
                        case _anglr_syntax_rule_.production_kind.g__anglr_syntax_rule__1:
                        {
                            SyntaxTreeToken syntaxTreeToken = p__anglr_syntax_rule_.m__identifier_;
                            SymbolToken symbolToken = new SymbolToken (syntaxTreeToken.text, (uint) AnglrClassificationType.NonTerminalName, null, parserPartSymbol);
                            SymbolToken symbolTokenRef = symbolTable.insert (symbolToken);
                            if (symbolTokenRef != symbolToken)
                            {
                                symbolToken.tag = symbolTokenRef.tag;
                                symbolTokenRef.tag = symbolToken;
                                logger?.ErrorRawLine ($"Warning, at ({syntaxTreeToken.lineno}, {syntaxTreeToken.column}): redefinition of syntax rule name {syntaxTreeToken.text}");
                            }
                            contextSymbol = syntaxRuleSymbol = symbolTokenRef;
                            _attribute_list_optional_ attribute_List_Optional_ = p__anglr_syntax_rule_.m__attribute_list_optional_;
                            _attribute_list_ attribute_List_ = attribute_List_Optional_.m__attribute_list_;
                            if (attribute_List_ == null)
                                break;
                            if ((bool) attribute_List_.Iterate (false, (node, appData) => (bool) appData || (node.m__attribute_.m__identifier_.text == "Iterator")))
                                symbolTokenRef.IteratorAttributeFlag = true;
                        }
                        break;
                        case _anglr_syntax_rule_.production_kind.g__anglr_syntax_rule__2:
                        {
                            SyntaxTreeToken syntaxTreeToken = p__anglr_syntax_rule_.m__identifier_;
                            SymbolToken symbolToken = new SymbolToken (syntaxTreeToken.text, (uint) AnglrClassificationType.GroupName, null, parserPartSymbol);
                            SymbolToken symbolTokenRef = symbolTable.insert (symbolToken);
                            if (symbolTokenRef != symbolToken)
                            {
                                symbolToken.tag = symbolTokenRef.tag;
                                symbolTokenRef.tag = symbolToken;
                                logger?.ErrorRawLine ($"Warning, at ({syntaxTreeToken.lineno}, {syntaxTreeToken.column}): redefinition of syntax group name {syntaxTreeToken.text}");
                            }
                        }
                        break;
                    }
                }
                break;
                case SyntaxTreeCallbackReason.TraversalMidTermCallbackReason:
                    break;
                case SyntaxTreeCallbackReason.TraversalEpilogueCallbackReason:
                    break;
            }
            return status;
        }

        private bool Invoke__anglr_syntax_production_list_name__Callback (SyntaxTreeCallbackReason reason, _anglr_syntax_production_list_name_.production_kind kind, _anglr_syntax_production_list_name_ p__anglr_syntax_production_list_name_)
        {
            bool status = true;
            switch (reason)
            {
                case SyntaxTreeCallbackReason.BuilderCallbackReason:
                    return buildEvent;
                    break;
                case SyntaxTreeCallbackReason.TraversalPrologueCallbackReason:
                {
                    SyntaxTreeToken syntaxTreeToken = p__anglr_syntax_production_list_name_.m__identifier_;
                    SymbolToken symbolToken = new SymbolToken (syntaxTreeToken.text, (uint) AnglrClassificationType.NonTerminalName, null, parserPartSymbol);
                    SymbolToken symbolTokenRef = symbolTable.insert (symbolToken);
                    if (symbolTokenRef != symbolToken)
                    {
                        logger?.ErrorRawLine ($"Error, at ({syntaxTreeToken.lineno}, {syntaxTreeToken.column}): redefinition of nested rule name {syntaxTreeToken.text}");
                        symbolToken.Dispose ();
                    }
                }
                break;
                case SyntaxTreeCallbackReason.TraversalMidTermCallbackReason:
                    break;
                case SyntaxTreeCallbackReason.TraversalEpilogueCallbackReason:
                    break;
            }
            return status;
        }

        private bool Invoke__production_name__Callback (SyntaxTreeCallbackReason reason, _production_name_.production_kind kind, _production_name_ p__production_name_)
        {
            switch (reason)
            {
                case SyntaxTreeCallbackReason.BuilderCallbackReason:
                    return buildEvent;
                    break;
                case SyntaxTreeCallbackReason.TraversalPrologueCallbackReason:
                {
                    SyntaxTreeToken syntaxTreeToken = p__production_name_.m__identifier_;
                    SymbolToken symbolToken = new SymbolToken (syntaxTreeToken.text, (uint) AnglrClassificationType.ProductionName, null, syntaxRuleSymbol);
                    SymbolToken symbolTokenRef = symbolTable.insert (symbolToken);
                    if (symbolTokenRef != symbolToken)
                    {
                        logger?.ErrorRawLine ($"Error, at ({syntaxTreeToken.lineno}, {syntaxTreeToken.column}): redefinition of production name {syntaxTreeToken.text}");
                        symbolToken.Dispose ();
                    }
                }
                break;
                case SyntaxTreeCallbackReason.TraversalMidTermCallbackReason:
                    break;
                case SyntaxTreeCallbackReason.TraversalEpilogueCallbackReason:
                    break;
            }
            return true;
        }

        private bool Invoke__marker__Callback (SyntaxTreeCallbackReason reason, _marker_.production_kind kind, _marker_ p__marker_)
        {
            switch (reason)
            {
                case SyntaxTreeCallbackReason.BuilderCallbackReason:
                    return buildEvent;
                    break;
                case SyntaxTreeCallbackReason.TraversalPrologueCallbackReason:
                {
                    SyntaxTreeToken syntaxTreeToken = p__marker_.m__identifier_;
                    SymbolToken symbolToken = new SymbolToken (syntaxTreeToken.text, (uint) AnglrClassificationType.MarkerName, null, syntaxRuleSymbol);
                    SymbolToken symbolTokenRef = symbolTable.insert (symbolToken);
                    if (symbolTokenRef != symbolToken)
                    {
                        logger?.ErrorRawLine ($"Error, at ({syntaxTreeToken.lineno}, {syntaxTreeToken.column}): redefinition of marker name {syntaxTreeToken.text}");
                        symbolToken.Dispose ();
                    }
                }
                break;
                case SyntaxTreeCallbackReason.TraversalMidTermCallbackReason:
                    break;
                case SyntaxTreeCallbackReason.TraversalEpilogueCallbackReason:
                    break;
            }
            return true;
        }
    }
}
