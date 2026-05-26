using Anglr.Compiler;
using Anglr.Parser;
using Anglr.Parser.Core;
using Anglr.Parser.SyntaxTree;
using AnglrLogLibrary;
using AnglrParserLibrary;
using Microsoft.VisualStudio.LanguageServer.Protocol;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Text.RegularExpressions;
using System.Xml.Linq;

namespace AnglrLibrary
{
    [Serializable]
    public class regstr
    {
        public regstr (string str)
        {
            this.str = str;
        }
        public string str = null;
    }

    [Serializable]
    internal class cmpreg : IComparer<(string, SymbolToken)>
    {
        public int Compare ((string, SymbolToken) x, (string, SymbolToken) y)
        {
            int xlen = x.Item1.Length;
            int ylen = y.Item1.Length;
            for (int i = 0; (i < xlen) && (i < ylen); ++i)
            {
                int diff = x.Item1 [i] - y.Item1 [i];
                if (diff == 0)
                    continue;
                return diff;
            }
            return ylen - xlen;
        }
    }

    [Serializable]
    internal class regset : SortedSet<(string, SymbolToken)>
    {
        public regset () : base (new cmpreg ())
        {
        }
    }

    public class RegexDictionary : Dictionary<SymbolToken, regstr>
    {
        public RegexDictionary () : base (new cmpsym ()) { }
    }

    internal enum ScannerActionCode
    {
        skipAction,
        terminalAction,
        eventAction,
        pushAction,
        popAction
    }

    internal class REUsageInfo
    {
        public bool terminated { get; private set; }
        public List<(ScannerActionCode action, string name)> actions { get; private set; } = new List<(ScannerActionCode action, string name)> ();
    }

    public partial class CSharpBaseGenerator : SyntaxTreeWalker
    {
        public delegate void generateParserCallback (_parser_part_ part);
        public delegate void generateLexerCallback (_lexer_part_ part);
        public delegate void generateScannerCallback (_scanner_part_ part);
        public event generateParserCallback generateParserEvent;
        public event generateLexerCallback generateLexerEvent;
        public event generateScannerCallback generateScannerEvent;

        public CSharpBaseGenerator (_anglr_file_fragment_ p__anglr_file_fragment_, string outputDir, anglrCompiler compiler)
        {
            this._anglr_file_fragment_ = p__anglr_file_fragment_;
            this.outputDir = outputDir;
            AnglrLogger = compiler?.AnglrLogger ?? new VoidAnglrLogger ();
            SymbolTable = compiler.symbolTable;

            attributeCollection = new AnglrAttributeCollection (compiler);
            attributeCollection.Traverse (p__anglr_file_fragment_);
            generalParts = attributeCollection.generalParts;
            declarationParts = attributeCollection.declarationParts;
            scannerParts = attributeCollection.scannerParts;
            lexerParts = attributeCollection.lexerParts;
            parserParts = attributeCollection.parserParts;
        }

        private void InitGenerator ()
        {
            m_errorToken = null;
            m_eofToken = null;
            m_acceptToken = null;

            m_prodlist = new prodlist ();
            m_proddict = new proddict ();
            m_cascades = new prodlist ();

            startSyntaxRule = null;
            m_startProductionNode = null;
            m_currentProductionNode = null;
            m_currentRhsProduction = null;
            m_firstRhsState = null;

            m_terminals = new tokvec ();
            m_nonterminals = new tokvec ();

            m_minTerminalNr = -1;
            m_maxTerminalNr = -1;
            m_minNonTerminalNr = -1;
            m_maxNonTerminalNr = -1;

            anonymousTokens = new regset ();
            anonymousGroup = "";
            anonymousSwitch = "";
            anonymousIndex = 0;

            tokenFlags = new bool [256];
            maxToken = 0;
            lexregindex = 0;
        }

        /// <summary>
        /// Generate application info object instance for every node of syntax tree
        /// </summary>
        /// <param name="reason">not used</param>
        /// <param name="kind">not used</param>
        /// <param name="p_node">reference of some node in syntax tree</param>
        /// <returns>always true - meanning: don't interrupt traversal of syntax tree</returns>
        internal bool Invoke_Common_Callback (SyntaxTreeCallbackReason reason, int kind, SyntaxTreeBase p_node)
        {
            if (reason != SyntaxTreeCallbackReason.TraversalPrologueCallbackReason)
                return true;

            p_node.appInfo = new AppInfo ();
            return true;
        }

        internal bool Invoke__anglr_file__Callback (SyntaxTreeCallbackReason reason, _anglr_file_.production_kind kind, _anglr_file_ p__anglr_file_)
        {
            bool result = true;
            switch (reason)
            {
                case SyntaxTreeCallbackReason.BuilderCallbackReason:
                    break;
                case SyntaxTreeCallbackReason.IterationPrologueCallbackReason:
                    break;
                case SyntaxTreeCallbackReason.TraversalPrologueCallbackReason:
                    break;
                case SyntaxTreeCallbackReason.TraversalMidTermCallbackReason:
                    break;
                case SyntaxTreeCallbackReason.TraversalEpilogueCallbackReason:
                    break;
                case SyntaxTreeCallbackReason.IterationEpilogueCallbackReason:
                {
                    // 1st step: compile all general parts in order of appearance
                    p__anglr_file_.m__anglr_file_part_list_?.Iterate (null, (node, appData) =>
                    {
                        _anglr_file_part_ p__anglr_file_part_ = node.m__anglr_file_part_;
                        if (p__anglr_file_part_.kind == (uint) _anglr_file_part_.production_kind.g__anglr_file_part__1)
                            Traverse (p__anglr_file_part_.m__general_part_);
                        return null;
                    });
                    // 2nd step: compile all declaration parts in order of appearance
                    p__anglr_file_.m__anglr_file_part_list_?.Iterate (null, (node, appData) =>
                    {
                        _anglr_file_part_ p__anglr_file_part_ = node.m__anglr_file_part_;
                        if (p__anglr_file_part_.kind == (uint) _anglr_file_part_.production_kind.g__anglr_file_part__2)
                            Traverse (p__anglr_file_part_.m__declaration_part_);
                        return null;
                    });
                    // 3rd step: compile all scanner parts in order of appearance
                    p__anglr_file_.m__anglr_file_part_list_?.Iterate (null, (node, appData) =>
                    {
                        _anglr_file_part_ p__anglr_file_part_ = node.m__anglr_file_part_;
                        if (p__anglr_file_part_.kind == (uint) _anglr_file_part_.production_kind.g__anglr_file_part__3)
                            Traverse (p__anglr_file_part_.m__scanner_part_);
                        return null;
                    });
                    // 4th step: compile all lexer parts in order of appearance
                    p__anglr_file_.m__anglr_file_part_list_?.Iterate (null, (node, appData) =>
                    {
                        _anglr_file_part_ p__anglr_file_part_ = node.m__anglr_file_part_;
                        if (p__anglr_file_part_.kind == (uint) _anglr_file_part_.production_kind.g__anglr_file_part__4)
                            Traverse (p__anglr_file_part_.m__lexer_part_);
                        return null;
                    });
                    // 5th step: compile all parser parts in order of appearance
                    p__anglr_file_.m__anglr_file_part_list_?.Iterate (null, (node, appData) =>
                    {
                        _anglr_file_part_ p__anglr_file_part_ = node.m__anglr_file_part_;
                        if (p__anglr_file_part_.kind == (uint) _anglr_file_part_.production_kind.g__anglr_file_part__5)
                            Traverse (p__anglr_file_part_.m__parser_part_);
                        return null;
                    });
                }
                break;
            }
            return result;
        }

        internal bool Invoke__terminal_definition__Callback (SyntaxTreeCallbackReason reason, _terminal_definition_.production_kind kind, _terminal_definition_ p__terminal_definition_)
        {
            bool result = true;
            switch (reason)
            {
                case SyntaxTreeCallbackReason.BuilderCallbackReason:
                    break;
                case SyntaxTreeCallbackReason.TraversalPrologueCallbackReason:
                    break;
                case SyntaxTreeCallbackReason.TraversalMidTermCallbackReason:
                    break;
                case SyntaxTreeCallbackReason.TraversalEpilogueCallbackReason:
                {
                    SyntaxTreeToken p__identifier_ = p__terminal_definition_.m__identifier_;
                    SyntaxTreeToken p__cstring_ = p__terminal_definition_.m__cstring_optional_.m__cstring_;
                    if (p__cstring_ == null)
                    {
                        SymbolToken p_SymbolToken = new SymbolToken (p__identifier_.text, (uint) AnglrClassificationType.TerminalName, null, declarationPartSymbol);
                        SymbolToken p_SymbolTokenRef = SymbolTable.find (p_SymbolToken);
                        p_SymbolToken.Dispose ();
                        if (p_SymbolTokenRef == null)
                        {
                            AnglrLogger?.ErrorLine ("Internal error: undefined terminal " + p__identifier_.text);
                            break;
                        }
                        ((AppInfo) p__terminal_definition_.appInfo) [AppInfoType.SymbolToken] = p_SymbolTokenRef;
                        p_SymbolTokenRef.AddDefInfo (p__identifier_.lineno, p__identifier_.column, p_SymbolTokenRef.name.Length);
                    }
                    else
                    {
                        SymbolToken p_SymbolToken = new SymbolToken (p__identifier_.text, (uint) AnglrClassificationType.TerminalName, null, declarationPartSymbol);
                        SymbolToken p_SymbolTokenRef = SymbolTable.find (p_SymbolToken);
                        p_SymbolToken.Dispose ();
                        if (p_SymbolTokenRef == null)
                        {
                            AnglrLogger?.ErrorLine ("Internal error: undefined terminal " + p__identifier_.text);
                            break;
                        }
                        SymbolToken p_aliasToken = new SymbolToken (p__cstring_.text, (uint) AnglrClassificationType.TerminalName, null, declarationPartSymbol, false);
                        SymbolToken p_aliasTokenRef = SymbolTable.find (p_aliasToken);
                        p_aliasToken.Dispose ();
                        if (p_aliasTokenRef == null)
                        {
                            AnglrLogger?.ErrorLine ("Internal error: undefined terminal " + p__cstring_.text);
                            break;
                        }
                        p_SymbolTokenRef.alias = p_aliasTokenRef;
                        p_aliasTokenRef.alias = p_SymbolTokenRef;
                        p_aliasTokenRef.AliasFlag = true;
                        p_SymbolTokenRef.declarator = p_aliasTokenRef.declarator = (uint) AnglrClassificationType.TerminalName;
                        ((AppInfo) p__terminal_definition_.appInfo) [AppInfoType.SymbolToken] = p_SymbolTokenRef;
                        p_SymbolTokenRef.AddDefInfo (p__identifier_.lineno, p__identifier_.column, p_SymbolTokenRef.name.Length);
                        p_aliasTokenRef.AddDefInfo (p__cstring_.lineno, p__cstring_.column, p_aliasTokenRef.name.Length);
                    }
                    break;
                }
            }
            return result;
        }

        internal bool Invoke__regex_definition__Callback (SyntaxTreeCallbackReason reason, _regex_definition_.production_kind kind, _regex_definition_ p__regex_definition_)
        {
            bool result = true;
            switch (reason)
            {
                case SyntaxTreeCallbackReason.BuilderCallbackReason:
                    break;
                case SyntaxTreeCallbackReason.TraversalPrologueCallbackReason:
                    break;
                case SyntaxTreeCallbackReason.TraversalMidTermCallbackReason:
                    break;
                case SyntaxTreeCallbackReason.TraversalEpilogueCallbackReason:
                    declarationPart.RegexAdd (p__regex_definition_);
                    break;
            }
            return result;
        }

        internal bool Invoke__anglr_syntax_rule__Callback (SyntaxTreeCallbackReason reason, _anglr_syntax_rule_.production_kind kind, _anglr_syntax_rule_ p__anglr_syntax_rule_)
        {
            bool result = true;
            switch (reason)
            {
                case SyntaxTreeCallbackReason.BuilderCallbackReason:
                    break;
                case SyntaxTreeCallbackReason.TraversalPrologueCallbackReason:
                {
                    if (kind != _anglr_syntax_rule_.production_kind.g__anglr_syntax_rule__1)
                        break;
                    SyntaxTreeToken p_identifier = p__anglr_syntax_rule_.m__identifier_;
                    SymbolToken p_SymbolToken = new SymbolToken (p_identifier.text, (uint) AnglrClassificationType.NonTerminalName, null, parserPartSymbol);
                    SymbolToken p_SymbolTokenRef = SymbolTable.insert (p_SymbolToken);
                    if (p_SymbolTokenRef != p_SymbolToken)
                    {
                        p_SymbolToken.Dispose ();
                        uint declarator = p_SymbolTokenRef.declarator;
                        if (declarator == (uint) AnglrClassificationType.TerminalName)
                            AnglrLogger?.WarnLine (p_SymbolTokenRef.name + ": definition of rule previously defined as terminal");
                    }
                    ((AppInfo) p_identifier.appInfo) [AppInfoType.SymbolToken] = p_SymbolTokenRef;
                    RhsProductionNode rhsProductionNode = null;
                    if (!m_prodlist.TryGetValue (p_SymbolTokenRef, out rhsProductionNode))
                        m_currentProductionNode = m_prodlist [p_SymbolTokenRef] = new RhsProductionNode (p_SymbolTokenRef);
                    else
                        m_currentProductionNode = rhsProductionNode;
                    p_SymbolTokenRef.declarator = (uint) AnglrClassificationType.NonTerminalName;
                    p_SymbolTokenRef.AddDefInfo (p_identifier.lineno, p_identifier.column, p_SymbolTokenRef.name.Length);

                    bool? status = (bool?) p__anglr_syntax_rule_.m__attribute_list_optional_.m__attribute_list_?.Iterate (false, (node, appData) =>
                    {
                        return ((bool) appData) || (node.m__attribute_.m__identifier_.text == "Start");
                    });

                    if ((status != null) && (status.Value))
                        startSyntaxRule = p__anglr_syntax_rule_;

                    if (startSyntaxRule == null)
                        startSyntaxRule = p__anglr_syntax_rule_;
                    if (startSyntaxRule == p__anglr_syntax_rule_)
                        m_startProductionNode = m_currentProductionNode;
                }
                break;
                case SyntaxTreeCallbackReason.TraversalMidTermCallbackReason:
                    break;
                case SyntaxTreeCallbackReason.TraversalEpilogueCallbackReason:
                    break;
            }
            return result;
        }

        internal bool Invoke__anglr_syntax_production__Callback (SyntaxTreeCallbackReason reason, _anglr_syntax_production_.production_kind kind, _anglr_syntax_production_ p__anglr_syntax_production_)
        {
            bool result = true;
            switch (reason)
            {
                case SyntaxTreeCallbackReason.BuilderCallbackReason:
                    break;
                case SyntaxTreeCallbackReason.TraversalPrologueCallbackReason:
                    (m_currentRhsProduction = new RhsProduction (m_currentProductionNode.ProductionName)).productionName =
                        SymbolTable.insert (new SymbolToken ($"{m_currentRhsProduction.productionNumber}", (uint) AnglrClassificationType.Literal, null, parserPartSymbol));
                    break;
                case SyntaxTreeCallbackReason.TraversalMidTermCallbackReason:
                    break;
                case SyntaxTreeCallbackReason.TraversalEpilogueCallbackReason:
                {
                    SyntaxTreeBase node;
                    for (node = p__anglr_syntax_production_; node.parent is _anglr_syntax_production_list_; node = node.parent)
                        ;
                    if (!(node.parent is _anglr_syntax_rule_) && !(node.parent is _anglr_file_fragment_))
                        break;
                    if (m_discardProducton)
                    {
                        m_discardProducton = false;
                        if (!(node.parent is _anglr_syntax_rule_))
                            break;
                        _anglr_syntax_rule_ rule = (_anglr_syntax_rule_) node.parent;
                        if (rule != startSyntaxRule)
                            break;
                    }
                    int nodeIndex = 0;
                    MarkerByPosition markersByPosition = m_currentRhsProduction.markersByPosition;
                    MarkerByName markersByName = m_currentRhsProduction.markersByName;
                    SymbolToken productionName = new SymbolToken ($"{m_currentRhsProduction.productionNumber}", (uint) AnglrClassificationType.Literal, null, parserPartSymbol);
                    SymbolToken productionNameRef = SymbolTable.insert (productionName);
                    if (kind == _anglr_syntax_production_.production_kind.g__anglr_syntax_production__1)
                    {
                        p__anglr_syntax_production_.m__name_list_.Iterate (null, (nameElt, appData) =>
                        {
                            _g_name_ g_name = nameElt.m__g_name_;
                            while (g_name != null)
                            {
                                _name_ p__name_ = g_name.m__name_;
                                if (p__name_ == null)
                                    break;
                                SymbolToken p_SymbolToken = (SymbolToken) ((AppInfo) p__name_.appInfo) [AppInfoType.SymbolToken];
                                if (p_SymbolToken.AliasFlag)
                                    p_SymbolToken = p_SymbolToken.alias;
                                RhsNode p_RhsNode = new RhsNode (p_SymbolToken);
                                m_currentRhsProduction.add (p_RhsNode);
                                if ((p_SymbolToken.declarator != (uint) AnglrClassificationType.NonTerminalName) && (p_SymbolToken.declarator != (uint) AnglrClassificationType.NonTerminalName))
                                    break;
                                ++nodeIndex;
                                break;
                            }
                            _marker_list_optional_ marker_Optional_ = nameElt.m__marker_list_optional_;
                            marker_Optional_.m__marker_list_?.Iterate (null, (mnode, data) =>
                            {
                                _marker_ p__marker_ = mnode.m__marker_;
                                while (p__marker_ != null)
                                {
                                    string markerName = p__marker_.m__identifier_.text;
                                    SymbolToken markerSymbol = null;
                                    if (markersByName.TryGetValue (markerName, out markerSymbol))
                                    {
                                        AnglrLogger?.WarnLine ($"Marker {markerName} redefines previously defined marker {markerSymbol.name}");
                                        break;
                                    }
                                    if (markersByPosition.TryGetValue ((nodeIndex, markerName), out markerSymbol))
                                    {
                                        AnglrLogger?.WarnLine ($"Marker {markerName} with value {nodeIndex} redefines previously defined marker {markerSymbol.name}");
                                        break;
                                    }
                                    markerSymbol = (SymbolToken) ((AppInfo) p__marker_.appInfo) [AppInfoType.SymbolToken];
                                    markersByPosition [(nodeIndex, markerName)] = markerSymbol;
                                    markersByName [markerName] = markerSymbol;
                                    break;
                                }
                                return null;
                            });
                            return null;
                        });
                    }
                    m_currentProductionNode.add (m_currentRhsProduction);
                }
                break;
            }
            return result;
        }

        internal bool Invoke__priority_specification__Callback (SyntaxTreeCallbackReason reason, _priority_specification_.production_kind kind, _priority_specification_ p__priority_specification_)
        {
            switch (reason)
            {
                case SyntaxTreeCallbackReason.TraversalPrologueCallbackReason:
                    m_currentRhsProduction.priority = int.Parse (p__priority_specification_.m__number_.text);
                    break;
                default:
                    break;
            }
            return true;
        }

        internal bool Invoke__associativity_specification__Callback (SyntaxTreeCallbackReason reason, _associativity_specification_.production_kind kind, _associativity_specification_ p__associativity_specification_)
        {
            switch (reason)
            {
                case SyntaxTreeCallbackReason.TraversalPrologueCallbackReason:
                {
                    string associativity = "";
                    switch (kind)
                    {
                        case _associativity_specification_.production_kind.g__associativity_specification__1:
                            associativity = p__associativity_specification_.m__cstring_.text;
                            break;
                        case _associativity_specification_.production_kind.g__associativity_specification__2:
                            associativity = p__associativity_specification_.m__identifier_.text;
                            break;
                    }
                    switch (associativity)
                    {
                        case "none":
                            m_currentRhsProduction.associativity = ProductionAssociativity.None;
                            break;
                        case "left":
                            m_currentRhsProduction.associativity = ProductionAssociativity.Left;
                            break;
                        case "right":
                            m_currentRhsProduction.associativity = ProductionAssociativity.Right;
                            break;
                    }
                }
                break;
                default:
                    break;
            }
            return true;
        }

        internal bool Invoke__production_name__Callback (SyntaxTreeCallbackReason reason, _production_name_.production_kind kind, _production_name_ p__production_name_)
        {
            bool result = false;
            switch (reason)
            {
                case SyntaxTreeCallbackReason.BuilderCallbackReason:
                    break;
                case SyntaxTreeCallbackReason.TraversalPrologueCallbackReason:
                {
                    SyntaxTreeBase node;
                    for (node = p__production_name_; (node != null) && !(node is _anglr_syntax_rule_); node = node.parent)
                        ;
                    SymbolToken syntaxRuleName = null;
                    _anglr_syntax_rule_ syntax_Rule_ = (_anglr_syntax_rule_) node;
                    if (syntax_Rule_ != null)
                        syntaxRuleName = (SymbolToken) ((AppInfo) syntax_Rule_.m__identifier_.appInfo) [AppInfoType.SymbolToken];
                    m_currentRhsProduction.productionName =
                        SymbolTable.insert (new SymbolToken ($"{p__production_name_.m__double_at_sign_.text}{p__production_name_.m__identifier_.text}", (uint) AnglrClassificationType.ProductionName, null, syntaxRuleName));
                }
                break;
                case SyntaxTreeCallbackReason.TraversalMidTermCallbackReason:
                    break;
                case SyntaxTreeCallbackReason.TraversalEpilogueCallbackReason:
                    break;
            }
            return result;
        }

        internal bool Invoke__name__Callback (SyntaxTreeCallbackReason reason, _name_.production_kind kind, _name_ p__name_)
        {
            bool result = true;
            switch (reason)
            {
                case SyntaxTreeCallbackReason.BuilderCallbackReason:
                    break;
                case SyntaxTreeCallbackReason.TraversalPrologueCallbackReason:
                    break;
                case SyntaxTreeCallbackReason.TraversalMidTermCallbackReason:
                    break;
                case SyntaxTreeCallbackReason.TraversalEpilogueCallbackReason:
                {
                    int lineno = -1;
                    int column = -1;
                    SymbolToken p_SymbolToken = null;
                    switch (kind)
                    {
                        case _name_.production_kind.g__name__1:
                            p_SymbolToken = new SymbolToken (p__name_.m__any_.text, (uint) AnglrClassificationType.TerminalName, null, declarationPartSymbol);
                            lineno = p__name_.m__any_.lineno;
                            column = p__name_.m__any_.column;
                            break;
                        case _name_.production_kind.g__name__2:
                            p_SymbolToken = new SymbolToken (p__name_.m__cstring_.text, (uint) AnglrClassificationType.TerminalName, null, declarationPartSymbol);
                            lineno = p__name_.m__cstring_.lineno;
                            column = p__name_.m__cstring_.column;
                            break;
                        case _name_.production_kind.g__name__3:
                        {
                            p_SymbolToken = new SymbolToken (p__name_.m__identifier_.text, (uint) AnglrClassificationType.TerminalName, null, declarationPartSymbol);
                            if (SymbolTable.find (p_SymbolToken) == null)
                                p_SymbolToken = new SymbolToken (p__name_.m__identifier_.text, (uint) AnglrClassificationType.NonTerminalName, null, parserPartSymbol);
                            lineno = p__name_.m__identifier_.lineno;
                            column = p__name_.m__identifier_.column;
                        }
                        break;
                    }
                    SymbolToken p_SymbolTokenRef = SymbolTable.insert (p_SymbolToken);
                    if (p_SymbolTokenRef != p_SymbolToken)
                        p_SymbolToken.Dispose ();
                    if ((kind == _name_.production_kind.g__name__2) && (p_SymbolTokenRef.declarator == 0))
                    {   // we got anonymous terminal
                        string text = Regex.Unescape (p__name_.m__cstring_.text.Substring (1, p__name_.m__cstring_.text.Length - 2));
                        if (text.Length == 1)   // text is something like "x" or "\x" - single character terminal
                        {
                            int terminalIndex = text [0];
                            if (terminalIndex > maxToken)
                                maxToken = terminalIndex;
                            tokenFlags [p_SymbolTokenRef.index = terminalIndex] = true;
                        }
                        p_SymbolTokenRef.correctName = string.Format ("_aonymous_{0:D}_", ++anonymousIndex);
                        AnglrLogger?.InfoRawLine ("ANONYMOUS: " + text + " = " + p_SymbolTokenRef.correctName);
                        p_SymbolTokenRef.declarator = (uint) AnglrClassificationType.TerminalName;
                        if (!anonymousTokens.Contains ((text, p_SymbolTokenRef)))
                            anonymousTokens.Add ((text, p_SymbolTokenRef));
                    }
                        ((AppInfo) p__name_.appInfo) [AppInfoType.SymbolToken] = p_SymbolTokenRef;
                    p_SymbolTokenRef.AddRefInfo (lineno, column, p_SymbolTokenRef.name.Length);
                }
                break;
            }
            return result;
        }

        internal bool Invoke__g_name__Callback (SyntaxTreeCallbackReason reason, _g_name_.production_kind kind, _g_name_ p__g_name_)
        {
            bool status = true;
            switch (reason)
            {
                case SyntaxTreeCallbackReason.BuilderCallbackReason:
                    break;
                case SyntaxTreeCallbackReason.IterationPrologueCallbackReason:
                    break;
                case SyntaxTreeCallbackReason.TraversalPrologueCallbackReason:
                    break;
                case SyntaxTreeCallbackReason.TraversalMidTermCallbackReason:
                    break;
                case SyntaxTreeCallbackReason.TraversalEpilogueCallbackReason:
                    break;
                case SyntaxTreeCallbackReason.IterationEpilogueCallbackReason:
                {
                    p__g_name_.Iterate (null, (node, appData) =>
                    {
                        NormalizeCompoundName (node);
                        return null;
                    });
                }
                break;
            }
            return status;
        }

        internal bool Invoke__marker__Callback (SyntaxTreeCallbackReason reason, _marker_.production_kind kind, _marker_ p__marker_)
        {
            bool status = true;
            switch (reason)
            {
                case SyntaxTreeCallbackReason.BuilderCallbackReason:
                    break;
                case SyntaxTreeCallbackReason.TraversalPrologueCallbackReason:
                    break;
                case SyntaxTreeCallbackReason.TraversalMidTermCallbackReason:
                    break;
                case SyntaxTreeCallbackReason.TraversalEpilogueCallbackReason:
                {
                    SyntaxTreeToken syntaxTreeToken = p__marker_.m__identifier_;
                    SymbolToken p_SymbolToken = new SymbolToken (syntaxTreeToken.text, (uint) AnglrClassificationType.MarkerName, null, m_currentRhsProduction.productionName);
                    SymbolToken p_SymbolTokenRef = SymbolTable.insert (p_SymbolToken);
                    if (p_SymbolTokenRef != p_SymbolToken)
                        p_SymbolToken.Dispose ();
                    ((AppInfo) p__marker_.appInfo) [AppInfoType.SymbolToken] = p_SymbolTokenRef;
                    p_SymbolTokenRef.AddRefInfo (syntaxTreeToken.lineno, syntaxTreeToken.column, syntaxTreeToken.text.Length);
                    p_SymbolTokenRef.AddDefInfo (syntaxTreeToken.lineno, syntaxTreeToken.column, syntaxTreeToken.text.Length);
                }
                break;
            }
            return status;
        }

        internal bool Invoke__regular_expression_usage__Callback (SyntaxTreeCallbackReason reason, _regular_expression_usage_.production_kind kind, _regular_expression_usage_ p__regular_expression_usage_)
        {
            bool status = true;
            switch (reason)
            {
                case SyntaxTreeCallbackReason.BuilderCallbackReason:
                    break;
                case SyntaxTreeCallbackReason.TraversalPrologueCallbackReason:
                    break;
                case SyntaxTreeCallbackReason.TraversalMidTermCallbackReason:
                    break;
                case SyntaxTreeCallbackReason.TraversalEpilogueCallbackReason:
                    _actions_ actions_ = p__regular_expression_usage_.m__actions_optional_.m__actions_;
                    REUsageInfo usageInfo = new REUsageInfo ();
                    if ((bool) actions_?.Iterate (true, (node, appData) =>
                    {
                        _action_ action_ = node.m__action_;
                        bool doit = (bool) appData;
                        if (!doit)
                        {
                            AnglrLogger?.WarnLine ($"skipped action: {action_.Emit (-1)}");
                            return doit;
                        }
                        switch ((_action_.production_kind) action_.kind)
                        {
                            case _action_.production_kind.g__action__1:
                            {
                                usageInfo.actions.Add ((ScannerActionCode.skipAction, null));
                                doit = false;
                            }
                            break;
                            case _action_.production_kind.g__action__2:
                            {
                                _terminal_action_ terminal_Action_ = action_.m__terminal_action_;
                                SymbolToken tokenSym = new SymbolToken (terminal_Action_.m__identifier_.text, (uint) AnglrClassificationType.TerminalName, null, declarationPartSymbol);
                                SymbolToken tokenRef = SymbolTable.find (tokenSym);
                                if (tokenRef == null)
                                {
                                    AnglrLogger?.ErrorLine ($"undefined token action symbol: {tokenSym.name}");
                                    break;
                                }
                                usageInfo.actions.Add ((ScannerActionCode.terminalAction, tokenRef.correctName));
                                doit = false;
                            }
                            break;
                            case _action_.production_kind.g__action__3:
                            {
                                _event_action_ event_Action_ = action_.m__event_action_;
                                SymbolToken tokenSym = new SymbolToken (event_Action_.m__identifier_.text, (uint) AnglrClassificationType.EventName, null, scannerPartSymbol);
                                SymbolToken tokenRef = SymbolTable.find (tokenSym);
                                if (tokenRef != null)
                                {
                                    AnglrLogger?.ErrorLine ($"redefined event action symbol: {tokenSym.name}");
                                }
                                else
                                {
                                    regexEventsCode.AppendLine ($"\t\t\tpublic event scannerCallback {tokenSym.correctName}_Event;");
                                    regexEventsExample.AppendLine ($"\t\t\t\t\tscanner.{tokenSym.correctName}_Event += (r, s) => throw new NotImplementedException (\"event {tokenSym.correctName}_Event is not implemented\");");
                                }
                                usageInfo.actions.Add ((ScannerActionCode.eventAction, tokenSym.correctName));
                                doit = false;
                            }
                            break;
                            case _action_.production_kind.g__action__4:
                            {
                                _push_action_ push_Action_ = action_.m__push_action_;
                                SymbolToken tokenSym = new SymbolToken (push_Action_.m__identifier_.text, (uint) AnglrClassificationType.ScannerPartName);
                                SymbolToken tokenRef = SymbolTable.find (tokenSym);
                                if (tokenRef == null)
                                {
                                    AnglrLogger?.ErrorLine ($"undefined push action symbol: {tokenSym.name}");
                                    break;
                                }
                                usageInfo.actions.Add ((ScannerActionCode.pushAction, tokenRef.correctName));
                            }
                            break;
                            case _action_.production_kind.g__action__5:
                            {
                                usageInfo.actions.Add ((ScannerActionCode.popAction, null));
                            }
                            break;
                        }
                        return doit;
                    }))
                    {
                        usageInfo.actions.Add ((ScannerActionCode.skipAction, null));
                        AnglrLogger?.WarnLine ($"default skip action for RE: {p__regular_expression_usage_.m__regular_expression_.Emit (-1)}");
                    }
                    {
                        regexScannerCode.AppendLine ($"\t\t\tcase {lexregindex + 1}:");
                        foreach ((ScannerActionCode action, string name) in usageInfo.actions)
                        {
                            switch (action)
                            {
                                case ScannerActionCode.skipAction:
                                    regexScannerCode.AppendLine ($"\t\t\t\tresult = 0;");
                                    regexScannerCode.AppendLine ($"\t\t\t\tbreak;");
                                    break;
                                case ScannerActionCode.terminalAction:
                                    regexScannerCode.AppendLine ($"\t\t\t\tresult = {declarationPart.declarationsClassName}.tokens.{declarationPart.tokenPrefix}{name};");
                                    regexScannerCode.AppendLine ($"\t\t\t\tbreak;");
                                    break;
                                case ScannerActionCode.eventAction:
                                    regexScannerCode.AppendLine ($"\t\t\t\tresult = {name}_Event?.Invoke (this, Scanner);");
                                    regexScannerCode.AppendLine ($"\t\t\t\tbreak;");
                                    break;
                                case ScannerActionCode.pushAction:
                                    regexScannerCode.AppendLine ($"\t\t\t\tScanner.pushScanner ({scannerParts [name].regexClassName}.Id, text);");
                                    break;
                                case ScannerActionCode.popAction:
                                    regexScannerCode.AppendLine ($"\t\t\t\tScanner.popScanner (text);");
                                    break;
                            }
                        }
                    }
                    {
                        SyntaxTreeToken p_regex = p__regular_expression_usage_.m__regular_expression_;
                        bool expanded = false;
                        string text = declarationPart.RegexResolve (p_regex, ref expanded);
                        try
                        {
                            if (lexregindex++ > 0)
                                regexMatchCode.Append ($"|");
                            regexMatchCode.Append ($"(?<g{lexregindex}>{text})");
                            scannerPart.lexRegSet.Add (text);
                            Regex regex = new Regex (text);
                            if (expanded)
                                AnglrLogger?.DebugRawLine ("Lexer regular expression, input = '" + p_regex.text + "', output = '" + text + "'");
                            else
                                AnglrLogger?.DebugRawLine ("Lexer regular expression = '" + p_regex.text + "'");
                        }
                        catch (Exception e)
                        {
                            AnglrLogger?.ErrorLine ("Error: " + e.Message);
                            if (expanded)
                                AnglrLogger?.ErrorLine ("Error in regular expression, input = '" + p_regex.text + "', output = '" + text + "'");
                            else
                                AnglrLogger?.ErrorLine ("Error in regular expression '" + p_regex.text + "'");
                        }
                    }
                    break;
            }
            return status;
        }

        internal bool Invoke__general_part__Callback (SyntaxTreeCallbackReason reason, _general_part_.production_kind kind, _general_part_ p__general_part_)
        {
            bool status = true;
            switch (reason)
            {
                case SyntaxTreeCallbackReason.BuilderCallbackReason:
                    break;
                case SyntaxTreeCallbackReason.TraversalPrologueCallbackReason:
                {
                    SyntaxTreeToken p_identifier = p__general_part_.m__identifier_;
                    try
                    {
                        generalPart = generalParts [p_identifier.text];
                        ((AppInfo) p_identifier.appInfo) [AppInfoType.SymbolToken] =
                        generalPartSymbol = generalPart.generalPartSymbol;
                    }
                    catch (Exception e)
                    {
                        AnglrLogger?.ErrorLine ("Cannot save general part info for: " + p_identifier.text +
                            "defined at line: " + p_identifier.lineno + ", column: " + p_identifier.column +
                            "; Error: " + e.Message);
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

        internal bool Invoke__declaration_part__Callback (SyntaxTreeCallbackReason reason, _declaration_part_.production_kind kind, _declaration_part_ p__declaration_part_)
        {
            bool status = true;
            switch (reason)
            {
                case SyntaxTreeCallbackReason.BuilderCallbackReason:
                    break;
                case SyntaxTreeCallbackReason.TraversalPrologueCallbackReason:
                {
                    SyntaxTreeToken p_identifier = p__declaration_part_.m__identifier_;
                    try
                    {
                        declarationPart = declarationParts [p_identifier.text];
                        ((AppInfo) p_identifier.appInfo) [AppInfoType.SymbolToken] =
                        declarationPartSymbol = declarationPart.declarationPartSymbol;
                    }
                    catch (Exception e)
                    {
                        AnglrLogger?.ErrorLine (e.Message);
                    }
                }
                break;
                case SyntaxTreeCallbackReason.TraversalMidTermCallbackReason:
                    break;
                case SyntaxTreeCallbackReason.TraversalEpilogueCallbackReason:
                {
                    try
                    {
                        StringBuilder stringBuilder = declarationPart.Generate ();
                        string fileName = outputDir + declarationPart.declarationsClassName + ".cs";
                        StreamWriter streamWriter = new StreamWriter (fileName);
                        streamWriter.WriteLine (stringBuilder);
                        streamWriter.Close ();
                    }
                    catch (Exception e)
                    {
                        AnglrLogger?.ErrorLine ($"ERROR: cannot save declarations part {declarationPart.declarationPartSymbol.name}");
                        AnglrLogger?.ErrorLine ($"\treason: {e.Message}");
                    }
                }
                break;
            }
            return status;
        }

        internal bool Invoke__scanner_part__Callback (SyntaxTreeCallbackReason reason, _scanner_part_.production_kind kind, _scanner_part_ p__scanner_part_)
        {
            bool status = true;
            switch (reason)
            {
                case SyntaxTreeCallbackReason.BuilderCallbackReason:
                    break;
                case SyntaxTreeCallbackReason.TraversalPrologueCallbackReason:
                {
                    SyntaxTreeToken p_identifier = p__scanner_part_.m__identifier_;
                    regexMatchCode.Clear ();
                    lexregindex = 0;

                    regexEventsCode.Clear ();
                    regexEventsExample.Clear ();
                    regexScannerCode.Clear ();
                    try
                    {
                        scannerPart = scannerParts [p_identifier.text];
                        ((AppInfo) p_identifier.appInfo) [AppInfoType.SymbolToken] =
                        scannerPartSymbol = scannerPart.scannerPartSymbol;
                        declarationPartSymbol = scannerPart.declarationPartSymbol;
                    }
                    catch (Exception e)
                    {
                        AnglrLogger?.ErrorLine (e.Message);
                    }
                }
                break;
                case SyntaxTreeCallbackReason.TraversalMidTermCallbackReason:
                    break;
                case SyntaxTreeCallbackReason.TraversalEpilogueCallbackReason:
                {
                    AnglrLogger?.DebugRawLine (p__scanner_part_.m__identifier_.text + " = " + regexMatchCode.ToString ());
                    scannerPart.LoadData (regexMatchCode, regexEventsCode, regexEventsExample, regexScannerCode);
                    generateScannerEvent?.Invoke (p__scanner_part_);
                }
                break;
            }
            return status;
        }

        internal bool Invoke__lexer_part__Callback (SyntaxTreeCallbackReason reason, _lexer_part_.production_kind kind, _lexer_part_ p__lexer_part_)
        {
            bool status = true;
            switch (reason)
            {
                case SyntaxTreeCallbackReason.BuilderCallbackReason:
                    break;
                case SyntaxTreeCallbackReason.TraversalPrologueCallbackReason:
                {
                    SyntaxTreeToken p_identifier = p__lexer_part_.m__identifier_;
                    try
                    {
                        lexerPart = lexerParts [p_identifier.text];
                        ((AppInfo) p_identifier.appInfo) [AppInfoType.SymbolToken] =
                        lexerPartSymbol = lexerPart.lexerPartSymbol;
                        lexerClassName = lexerPart.lexerClassName;
                        lexerNameSpace = lexerPart.lexerNameSpace;
                    }
                    catch (Exception e)
                    {
                        AnglrLogger?.ErrorLine (e.Message);
                    }
                }
                break;
                case SyntaxTreeCallbackReason.TraversalMidTermCallbackReason:
                    break;
                case SyntaxTreeCallbackReason.TraversalEpilogueCallbackReason:
                    generateLexerEvent?.Invoke (p__lexer_part_);
                    break;
            }
            return status;
        }

        internal bool Invoke__parser_part__Callback (SyntaxTreeCallbackReason reason, _parser_part_.production_kind kind, _parser_part_ p__parser_part_)
        {
            bool status = true;
            switch (reason)
            {
                case SyntaxTreeCallbackReason.BuilderCallbackReason:
                    break;
                case SyntaxTreeCallbackReason.TraversalPrologueCallbackReason:
                {
                    {
                        InitGeneralizations ();
                        InitGenerator ();
                        (m_errorToken = insertSymbol ("error", (uint) AnglrClassificationType.TerminalName, Constants.TOKEN_ERROR, false)).SymbolUsageFlag = true;
                        (m_eofToken = insertSymbol ("$eof-token", (uint) AnglrClassificationType.TerminalName, Constants.TOKEN_EOF, false)).SymbolUsageFlag = true;
                        (m_acceptToken = insertSymbol ("$accept", (uint) AnglrClassificationType.NonTerminalName, Constants.TOKEN_ACCEPT, false)).SymbolUsageFlag = true;
                    }
                    {
                        SyntaxTreeToken p_identifier = p__parser_part_.m__identifier_;
                        AnglrLogger?.InfoLine ($"Generate parser: {p_identifier.text}");
                        try
                        {
                            parserPart = parserParts [p_identifier.text];
                            parserNameSpace = parserPart.parserNameSpace;
                            parserClassName = parserPart.parserClassName;
                            scannerId = parserPart.lexerId;
                            ((AppInfo) p_identifier.appInfo) [AppInfoType.SymbolToken] =
                            parserPartSymbol = parserPart.parserPartSymbol;
                            declarationPartSymbol = parserPart.declarationPartSymbol;
                            scannerPartSymbol = parserPart.lexerPartSymbol;
                        }
                        catch (Exception e)
                        {
                            AnglrLogger?.ErrorLine (e.Message);
                        }
                    }
                }
                break;
                case SyntaxTreeCallbackReason.TraversalMidTermCallbackReason:
                    break;
                case SyntaxTreeCallbackReason.TraversalEpilogueCallbackReason:
                {
                    int count = m_prodlist.Count;
                    foreach (_anglr_syntax_rule_ syntax_Rule_ in m_generatedRuleList)
                    {
                        TraverseCommon (syntax_Rule_);
                        Traverse (syntax_Rule_);
                    }
                    if (m_startProductionNode != null)
                    {
                        RhsProduction p_RhsProduction = new RhsProduction (m_acceptToken);
                        p_RhsProduction.add (new RhsNode (m_startProductionNode.ProductionName));
                        p_RhsProduction.add (new RhsNode (m_eofToken));

                        RhsProductionNode p_RhsProductionNode = new RhsProductionNode (m_acceptToken);
                        p_RhsProductionNode.add (p_RhsProduction);

                        m_firstRhsState = new RhsState (m_startProductionNode.ProductionName, m_prodlist);
                        m_firstRhsState.add (new RhsConfiguration (p_RhsProduction, new rhsenumerator (p_RhsProduction.rhsNodes)));

                        m_prodlist [m_acceptToken] = p_RhsProductionNode;
                        m_proddict [p_RhsProduction.productionNumber] = p_RhsProduction;

                        // create terminal and non-terminal codes
                        tokenFlags [Constants.TOKEN_ERROR] = true;
                        tokenFlags [Constants.TOKEN_EOF] = true;
                        tokenFlags [Constants.TOKEN_ACCEPT] = true;
                        m_maxTerminalNr = SymbolTable.createSymbolList (m_terminals, (uint) AnglrClassificationType.TerminalName, declarationPartSymbol);
                        {
                            int index = 258;
                            m_minTerminalNr = index;
                            foreach (SymbolToken symbolToken in m_terminals)
                            {
                                if (symbolToken.context != declarationPartSymbol)
                                    continue;
                                //if (symbolToken.index > 0)
                                //    continue;
                                while ((index < 256) && tokenFlags [index])
                                    ++index;
                                symbolToken.index = index++;
                            }
                            if (index < maxToken)
                                index = maxToken;
                            m_maxTerminalNr = index;
                        }

                        m_maxNonTerminalNr = SymbolTable.createSymbolList (m_nonterminals, (uint) AnglrClassificationType.NonTerminalName, parserPartSymbol);
                        {
                            int index = m_maxTerminalNr;
                            m_minNonTerminalNr = index;
                            foreach (SymbolToken symbolToken in m_nonterminals)
                            {
                                if (symbolToken.context != parserPartSymbol)
                                    continue;
                                //if (symbolToken.index > 0)
                                //    continue;
                                while ((index < 256) && tokenFlags [index])
                                    ++index;
                                symbolToken.index = index++;
                            }
                            if (index < maxToken)
                                index = maxToken;
                            m_maxNonTerminalNr = index;
                        }
                    }

                    if (anonymousTokens.Count > 0)
                    {
                        anonymousSwitch = "\t\tpublic readonly int [] anonymousCode = new int [] { -1";
                        List<string> regSet = null;
                        ScannerPart scannerPart = null;
                        if (scannerParts.TryGetValue (scannerId, out scannerPart))
                            regSet = scannerPart.lexRegSet;
                        anonymousGroup = "";
                        int groupIndex = 0;
                        foreach ((string, SymbolToken) item in anonymousTokens)
                        {
                            AnglrLogger?.DebugRawLine ("anonymous terminal: " + item.Item1);
                            string text = Regex.Escape (item.Item1);
                            if (regSet != null)
                            {
                                bool ind = false;
                                foreach (string reg in regSet)
                                {
                                    if (reg.StartsWith (text))
                                    {
                                        ind = true;
                                        break;
                                    }
                                }
                                if (ind)
                                    continue;
                            }
                            if (groupIndex++ > 0)
                                anonymousGroup += '|';
                            anonymousGroup += "(?<g" + groupIndex + ">^" + Regex.Escape (item.Item1) + ")";
                            anonymousSwitch += ", " + item.Item2.index;
                        }
                        AnglrLogger?.DebugRawLine ("anonymous group: " + anonymousGroup);
                        anonymousSwitch += " };";
                    }

                    generateParserEvent?.Invoke (p__parser_part_);
                }
                break;
            }
            return status;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="name"></param>
        /// <param name="declarator"></param>
        /// <param name="index"></param>
        /// <param name="reportDefined"></param>
        /// <returns></returns>
        private SymbolToken insertSymbol (string name, uint declarator, int index, bool reportDefined = true)
        {
            SymbolToken p_symbolToken = new SymbolToken (name, declarator);
            SymbolToken p_symbolReference = SymbolTable.insert (p_symbolToken);
            if (p_symbolReference != p_symbolToken)
            {
                if (reportDefined)
                    AnglrLogger?.ErrorLine ("redefined symbol '" + p_symbolToken.name + "'");
            }
            p_symbolReference.index = index;
            return p_symbolReference;
        }

        public void EliminateUnusedProductions ()
        {
            prodlist prodList = new prodlist ();
            foreach (var production in m_prodlist)
            {
                SymbolToken symbolToken = production.Key;
                if (!symbolToken.SymbolUsageFlag)
                {
                    AnglrLogger.ErrorLine ($"Unused syntax rule: {production.Value.ProductionName.name}");
                    continue;
                }
                prodList [symbolToken] = production.Value;
            }
            m_prodlist = prodList;
        }

        [Serializable]
        internal enum AssemblySigningMode
        {
            UnknownSigningMode,
            NotSigning,
            DelaySigned,
            Signed
        };
        internal string lexerCode { get; private set; } = "";
        internal string regexCode { get; private set; } = "";
        internal StringBuilder regexMatchCode { get; private set; } = new StringBuilder ();
        internal StringBuilder regexEventsCode { get; private set; } = new StringBuilder ();
        internal StringBuilder regexEventsExample { get; private set; } = new StringBuilder ();
        internal StringBuilder regexScannerCode { get; private set; } = new StringBuilder ();

        // Parser attribute values
        internal string parserNameSpace { get; private set; } = "";
        internal string parserClassName { get; private set; } = "";
        internal string scannerId { get; private set; } = "";

        // Scanner attribute values
        internal string lexerNameSpace { get; private set; } = "";
        internal string lexerClassName { get; private set; } = "";

        // Regex attribute values
        internal string regexClassName { get; private set; } = "";

        internal SymbolToken m_errorToken { get; private set; } = null;
        internal SymbolToken m_eofToken { get; private set; } = null;
        internal SymbolToken m_acceptToken { get; private set; } = null;

        internal prodlist m_prodlist { get; private set; } = new prodlist ();
        internal proddict m_proddict { get; private set; } = new proddict ();
        internal prodlist m_cascades { get; private set; } = new prodlist ();

        public _anglr_syntax_rule_ startSyntaxRule { get; set; } = null;
        internal RhsProductionNode m_startProductionNode { get; private set; } = null;
        internal RhsProductionNode m_currentProductionNode { get; private set; } = null;
        internal RhsProduction m_currentRhsProduction { get; private set; } = null;
        internal RhsState m_firstRhsState { get; private set; } = null;

        internal tokvec m_terminals { get; private set; } = new tokvec ();
        internal tokvec m_nonterminals { get; private set; } = new tokvec ();

        internal int m_minTerminalNr { get; private set; } = -1;
        internal int m_maxTerminalNr { get; private set; } = -1;
        internal int m_minNonTerminalNr { get; private set; } = -1;
        internal int m_maxNonTerminalNr { get; private set; } = -1;

        internal IAnglrLogger AnglrLogger { get; private set; }
        internal SymbolTable SymbolTable { get; private set; }
        internal _anglr_file_fragment_ _anglr_file_fragment_ { get; private set; }
        internal string outputDir { get; private set; } = "";

        internal AnglrAttributeCollection attributeCollection { get; private set; } = null;
        public GeneralParts generalParts { get; private set; } = new GeneralParts ();
        internal GeneralPart generalPart { get; private set; } = null;
        public DeclarationParts declarationParts { get; private set; } = new DeclarationParts ();
        internal DeclarationsPart declarationPart { get; private set; } = null;
        public ScannerParts scannerParts { get; private set; } = new ScannerParts ();
        internal ScannerPart scannerPart { get; private set; } = null;
        public LexerParts lexerParts { get; private set; } = new LexerParts ();
        internal LexerPart lexerPart { get; private set; } = null;
        public ParserParts parserParts { get; private set; } = new ParserParts ();
        internal ParserPart parserPart { get; private set; } = null;

        private SymbolToken generalPartSymbol = null;
        private SymbolToken declarationPartSymbol = null;
        private SymbolToken scannerPartSymbol = null;
        private SymbolToken lexerPartSymbol = null;
        private SymbolToken parserPartSymbol = null;

        internal int scannerPartsCounter { get; private set; } = 0;

        internal regset anonymousTokens { get; private set; } = new regset ();
        internal string anonymousGroup { get; private set; } = "";
        internal string anonymousSwitch { get; private set; } = "";
        internal int anonymousIndex { get; private set; } = 0;

        private bool [] tokenFlags = new bool [256];
        private int maxToken = 0;
        private int lexregindex;

        private Stack<string> gnames = new Stack<string> ();
    }

    public partial class AnglrParserStatesBaseGenerator : SyntaxTreeWalker
    {
        public delegate void generateParserCallback (_parser_part_ part);
        public delegate void generateLexerCallback (_lexer_part_ part);
        public delegate void generateScannerCallback (_scanner_part_ part);
        public event generateParserCallback generateParserEvent;
        public event generateLexerCallback generateLexerEvent;
        public event generateScannerCallback generateScannerEvent;

        public AnglrParserStatesBaseGenerator (anglrCompiler compiler)
        {
            foreach (SyntaxTreeBase node in compiler.parseList)
            {
                if (node == null)
                    continue;
                _anglr_file_fragment_ = node as _anglr_file_fragment_;
            }
            AnglrLogger = compiler?.AnglrLogger ?? new VoidAnglrLogger ();
            SymbolTable = compiler.symbolTable;

            attributeCollection = new AnglrAttributeCollection (compiler);
            attributeCollection.Traverse (_anglr_file_fragment_);
            generalParts = attributeCollection.generalParts;
            declarationParts = attributeCollection.declarationParts;
            scannerParts = attributeCollection.scannerParts;
            lexerParts = attributeCollection.lexerParts;
            parserParts = attributeCollection.parserParts;
        }

        private void InitGenerator ()
        {
            m_errorToken = null;
            m_eofToken = null;
            m_acceptToken = null;

            m_prodlist = new prodlist ();
            m_proddict = new proddict ();
            m_cascades = new prodlist ();

            startSyntaxRule = null;
            m_startProductionNode = null;
            m_currentProductionNode = null;
            m_currentRhsProduction = null;
            m_firstRhsState = null;

            m_terminals = new tokvec ();
            m_nonterminals = new tokvec ();

            m_minTerminalNr = -1;
            m_maxTerminalNr = -1;
            m_minNonTerminalNr = -1;
            m_maxNonTerminalNr = -1;

            anonymousTokens = new regset ();
            anonymousGroup = "";
            anonymousSwitch = "";
            anonymousIndex = 0;

            tokenFlags = new bool [256];
            maxToken = 0;
            lexregindex = 0;
        }

        /// <summary>
        /// Generate application info object instance for every node of syntax tree
        /// </summary>
        /// <param name="reason">not used</param>
        /// <param name="kind">not used</param>
        /// <param name="p_node">reference of some node in syntax tree</param>
        /// <returns>always true - meanning: don't interrupt traversal of syntax tree</returns>
        internal bool Invoke_Common_Callback (SyntaxTreeCallbackReason reason, int kind, SyntaxTreeBase p_node)
        {
            if (reason != SyntaxTreeCallbackReason.TraversalPrologueCallbackReason)
                return true;

            p_node.appInfo = new AppInfo ();
            return true;
        }

        internal bool Invoke__anglr_file__Callback (SyntaxTreeCallbackReason reason, _anglr_file_.production_kind kind, _anglr_file_ p__anglr_file_)
        {
            bool result = true;
            switch (reason)
            {
                case SyntaxTreeCallbackReason.BuilderCallbackReason:
                    break;
                case SyntaxTreeCallbackReason.IterationPrologueCallbackReason:
                    break;
                case SyntaxTreeCallbackReason.TraversalPrologueCallbackReason:
                    break;
                case SyntaxTreeCallbackReason.TraversalMidTermCallbackReason:
                    break;
                case SyntaxTreeCallbackReason.TraversalEpilogueCallbackReason:
                    break;
                case SyntaxTreeCallbackReason.IterationEpilogueCallbackReason:
                {
                    // 1st step: compile all general parts in order of appearance
                    p__anglr_file_.m__anglr_file_part_list_?.Iterate (null, (node, appData) =>
                    {
                        _anglr_file_part_ p__anglr_file_part_ = node.m__anglr_file_part_;
                        if (p__anglr_file_part_.kind == (uint) _anglr_file_part_.production_kind.g__anglr_file_part__1)
                            Traverse (p__anglr_file_part_.m__general_part_);
                        return null;
                    });
                    // 2nd step: compile all declaration parts in order of appearance
                    p__anglr_file_.m__anglr_file_part_list_?.Iterate (null, (node, appData) =>
                    {
                        _anglr_file_part_ p__anglr_file_part_ = node.m__anglr_file_part_;
                        if (p__anglr_file_part_.kind == (uint) _anglr_file_part_.production_kind.g__anglr_file_part__2)
                            Traverse (p__anglr_file_part_.m__declaration_part_);
                        return null;
                    });
                    // 3rd step: compile all scanner parts in order of appearance
                    p__anglr_file_.m__anglr_file_part_list_?.Iterate (null, (node, appData) =>
                    {
                        _anglr_file_part_ p__anglr_file_part_ = node.m__anglr_file_part_;
                        if (p__anglr_file_part_.kind == (uint) _anglr_file_part_.production_kind.g__anglr_file_part__3)
                            Traverse (p__anglr_file_part_.m__scanner_part_);
                        return null;
                    });
                    // 4th step: compile all lexer parts in order of appearance
                    p__anglr_file_.m__anglr_file_part_list_?.Iterate (null, (node, appData) =>
                    {
                        _anglr_file_part_ p__anglr_file_part_ = node.m__anglr_file_part_;
                        if (p__anglr_file_part_.kind == (uint) _anglr_file_part_.production_kind.g__anglr_file_part__4)
                            Traverse (p__anglr_file_part_.m__lexer_part_);
                        return null;
                    });
                    // 5th step: compile all parser parts in order of appearance
                    p__anglr_file_.m__anglr_file_part_list_?.Iterate (null, (node, appData) =>
                    {
                        _anglr_file_part_ p__anglr_file_part_ = node.m__anglr_file_part_;
                        if (p__anglr_file_part_.kind == (uint) _anglr_file_part_.production_kind.g__anglr_file_part__5)
                            Traverse (p__anglr_file_part_.m__parser_part_);
                        return null;
                    });
                }
                break;
            }
            return result;
        }

        internal bool Invoke__terminal_definition__Callback (SyntaxTreeCallbackReason reason, _terminal_definition_.production_kind kind, _terminal_definition_ p__terminal_definition_)
        {
            bool result = true;
            switch (reason)
            {
                case SyntaxTreeCallbackReason.BuilderCallbackReason:
                    break;
                case SyntaxTreeCallbackReason.TraversalPrologueCallbackReason:
                    break;
                case SyntaxTreeCallbackReason.TraversalMidTermCallbackReason:
                    break;
                case SyntaxTreeCallbackReason.TraversalEpilogueCallbackReason:
                {
                    SyntaxTreeToken p__identifier_ = p__terminal_definition_.m__identifier_;
                    SyntaxTreeToken p__cstring_ = p__terminal_definition_.m__cstring_optional_.m__cstring_;
                    if (p__cstring_ == null)
                    {
                        SymbolToken p_SymbolToken = new SymbolToken (p__identifier_.text, (uint) AnglrClassificationType.TerminalName, null, declarationPartSymbol);
                        SymbolToken p_SymbolTokenRef = SymbolTable.find (p_SymbolToken);
                        p_SymbolToken.Dispose ();
                        if (p_SymbolTokenRef == null)
                        {
                            AnglrLogger?.ErrorLine ("Internal error: undefined terminal " + p__identifier_.text);
                            break;
                        }
                        ((AppInfo) p__terminal_definition_.appInfo) [AppInfoType.SymbolToken] = p_SymbolTokenRef;
                        //p_SymbolTokenRef.AddDefInfo (p__identifier_.lineno, p__identifier_.column, p_SymbolTokenRef.name.Length);
                    }
                    else
                    {
                        SymbolToken p_SymbolToken = new SymbolToken (p__identifier_.text, (uint) AnglrClassificationType.TerminalName, null, declarationPartSymbol);
                        SymbolToken p_SymbolTokenRef = SymbolTable.find (p_SymbolToken);
                        p_SymbolToken.Dispose ();
                        if (p_SymbolTokenRef == null)
                        {
                            AnglrLogger?.ErrorLine ("Internal error: undefined terminal " + p__identifier_.text);
                            break;
                        }
                        SymbolToken p_aliasToken = new SymbolToken (p__cstring_.text, (uint) AnglrClassificationType.TerminalName, null, declarationPartSymbol, false);
                        SymbolToken p_aliasTokenRef = SymbolTable.find (p_aliasToken);
                        p_aliasToken.Dispose ();
                        if (p_aliasTokenRef == null)
                        {
                            AnglrLogger?.ErrorLine ("Internal error: undefined terminal " + p__cstring_.text);
                            break;
                        }
                        p_SymbolTokenRef.alias = p_aliasTokenRef;
                        p_aliasTokenRef.alias = p_SymbolTokenRef;
                        p_aliasTokenRef.AliasFlag = true;
                        p_SymbolTokenRef.declarator = p_aliasTokenRef.declarator = (uint) AnglrClassificationType.TerminalName;
                        ((AppInfo) p__terminal_definition_.appInfo) [AppInfoType.SymbolToken] = p_SymbolTokenRef;
                        //p_SymbolTokenRef.AddDefInfo (p__identifier_.lineno, p__identifier_.column, p_SymbolTokenRef.name.Length);
                        //p_aliasTokenRef.AddDefInfo (p__cstring_.lineno, p__cstring_.column, p_aliasTokenRef.name.Length);
                    }
                    break;
                }
            }
            return result;
        }

        internal bool Invoke__regex_definition__Callback (SyntaxTreeCallbackReason reason, _regex_definition_.production_kind kind, _regex_definition_ p__regex_definition_)
        {
            bool result = true;
            switch (reason)
            {
                case SyntaxTreeCallbackReason.BuilderCallbackReason:
                    break;
                case SyntaxTreeCallbackReason.TraversalPrologueCallbackReason:
                    break;
                case SyntaxTreeCallbackReason.TraversalMidTermCallbackReason:
                    break;
                case SyntaxTreeCallbackReason.TraversalEpilogueCallbackReason:
                    declarationPart.RegexAdd (p__regex_definition_);
                    break;
            }
            return result;
        }

        internal bool Invoke__anglr_syntax_rule__Callback (SyntaxTreeCallbackReason reason, _anglr_syntax_rule_.production_kind kind, _anglr_syntax_rule_ p__anglr_syntax_rule_)
        {
            bool result = true;
            switch (reason)
            {
                case SyntaxTreeCallbackReason.BuilderCallbackReason:
                    break;
                case SyntaxTreeCallbackReason.TraversalPrologueCallbackReason:
                {
                    if (kind != _anglr_syntax_rule_.production_kind.g__anglr_syntax_rule__1)
                        break;
                    SyntaxTreeToken p_identifier = p__anglr_syntax_rule_.m__identifier_;
                    SymbolToken p_SymbolToken = new SymbolToken (p_identifier.text, (uint) AnglrClassificationType.NonTerminalName, null, parserPartSymbol);
                    SymbolToken p_SymbolTokenRef = SymbolTable.insert (p_SymbolToken);
                    if (p_SymbolTokenRef != p_SymbolToken)
                    {
                        p_SymbolToken.Dispose ();
                        uint declarator = p_SymbolTokenRef.declarator;
                        if (declarator == (uint) AnglrClassificationType.TerminalName)
                            AnglrLogger?.ErrorLine (p_SymbolTokenRef.name + ": definition of rule previously defined as terminal");
                    }
                    ((AppInfo) p_identifier.appInfo) [AppInfoType.SymbolToken] = p_SymbolTokenRef;
                    RhsProductionNode rhsProductionNode = null;
                    if (!m_prodlist.TryGetValue (p_SymbolTokenRef, out rhsProductionNode))
                        m_currentProductionNode = m_prodlist [p_SymbolTokenRef] = new RhsProductionNode (p_SymbolTokenRef);
                    else
                        m_currentProductionNode = rhsProductionNode;
                    p_SymbolTokenRef.declarator = (uint) AnglrClassificationType.NonTerminalName;
                    //p_SymbolTokenRef.AddDefInfo (p_identifier.lineno, p_identifier.column, p_SymbolTokenRef.name.Length);

                    bool? status = (bool?) p__anglr_syntax_rule_.m__attribute_list_optional_.m__attribute_list_?.Iterate (false, (node, appData) =>
                    {
                        return ((bool) appData) || (node.m__attribute_.m__identifier_.text == "Start");
                    });

                    if ((status != null) && (status.Value))
                        startSyntaxRule = p__anglr_syntax_rule_;

                    if (startSyntaxRule == null)
                        startSyntaxRule = p__anglr_syntax_rule_;
                    if (startSyntaxRule == p__anglr_syntax_rule_)
                        m_startProductionNode = m_currentProductionNode;
                }
                break;
                case SyntaxTreeCallbackReason.TraversalMidTermCallbackReason:
                    break;
                case SyntaxTreeCallbackReason.TraversalEpilogueCallbackReason:
                    break;
            }
            return result;
        }

        internal bool Invoke__anglr_syntax_production__Callback (SyntaxTreeCallbackReason reason, _anglr_syntax_production_.production_kind kind, _anglr_syntax_production_ p__anglr_syntax_production_)
        {
            bool result = true;
            switch (reason)
            {
                case SyntaxTreeCallbackReason.BuilderCallbackReason:
                    break;
                case SyntaxTreeCallbackReason.TraversalPrologueCallbackReason:
                    (m_currentRhsProduction = new RhsProduction (m_currentProductionNode.ProductionName)).productionName =
                        SymbolTable.insert (new SymbolToken ($"{m_currentRhsProduction.productionNumber}", (uint) AnglrClassificationType.Literal, null, parserPartSymbol));
                    break;
                case SyntaxTreeCallbackReason.TraversalMidTermCallbackReason:
                    break;
                case SyntaxTreeCallbackReason.TraversalEpilogueCallbackReason:
                {
                    SyntaxTreeBase node;
                    for (node = p__anglr_syntax_production_; node.parent is _anglr_syntax_production_list_; node = node.parent)
                        ;
                    if (!(node.parent is _anglr_syntax_rule_) && !(node.parent is _anglr_file_fragment_))
                        break;
                    if (m_discardProducton)
                    {
                        m_discardProducton = false;
                        if (!(node.parent is _anglr_syntax_rule_))
                            break;
                        _anglr_syntax_rule_ rule = (_anglr_syntax_rule_) node.parent;
                        if (rule != startSyntaxRule)
                            break;
                    }
                    int nodeIndex = 0;
                    MarkerByPosition markersByPosition = m_currentRhsProduction.markersByPosition;
                    MarkerByName markersByName = m_currentRhsProduction.markersByName;
                    SymbolToken productionName = new SymbolToken ($"{m_currentRhsProduction.productionNumber}", (uint) AnglrClassificationType.Literal, null, parserPartSymbol);
                    SymbolToken productionNameRef = SymbolTable.insert (productionName);
                    if (kind == _anglr_syntax_production_.production_kind.g__anglr_syntax_production__1)
                    {
                        p__anglr_syntax_production_.m__name_list_.Iterate (null, (nameElt, appData) =>
                        {
                            _g_name_ g_name = nameElt.m__g_name_;
                            while (g_name != null)
                            {
                                _name_ p__name_ = g_name.m__name_;
                                if (p__name_ == null)
                                    break;
                                SymbolToken p_SymbolToken = (SymbolToken) ((AppInfo) p__name_.appInfo) [AppInfoType.SymbolToken];
                                if (p_SymbolToken.AliasFlag)
                                    p_SymbolToken = p_SymbolToken.alias;
                                RhsNode p_RhsNode = new RhsNode (p_SymbolToken);
                                m_currentRhsProduction.add (p_RhsNode);
                                if ((p_SymbolToken.declarator != (uint) AnglrClassificationType.NonTerminalName) && (p_SymbolToken.declarator != (uint) AnglrClassificationType.NonTerminalName))
                                    break;
                                ++nodeIndex;
                                break;
                            }
                            _marker_list_optional_ marker_Optional_ = nameElt.m__marker_list_optional_;
                            marker_Optional_.m__marker_list_?.Iterate (null, (mnode, data) =>
                            {
                                _marker_ p__marker_ = mnode.m__marker_;
                                while (p__marker_ != null)
                                {
                                    string markerName = p__marker_.m__identifier_.text;
                                    SymbolToken markerSymbol = null;
                                    if (markersByName.TryGetValue (markerName, out markerSymbol))
                                    {
                                        AnglrLogger?.ErrorLine ($"Marker {markerName} redefines previously defined marker {markerSymbol.name}");
                                        break;
                                    }
                                    if (markersByPosition.TryGetValue ((nodeIndex, markerName), out markerSymbol))
                                    {
                                        AnglrLogger?.ErrorLine ($"Marker {markerName} with value {nodeIndex} redefines previously defined marker {markerSymbol.name}");
                                        break;
                                    }
                                    markerSymbol = (SymbolToken) ((AppInfo) p__marker_.appInfo) [AppInfoType.SymbolToken];
                                    markersByPosition [(nodeIndex, markerName)] = markerSymbol;
                                    markersByName [markerName] = markerSymbol;
                                    break;
                                }
                                return null;
                            });
                            return null;
                        });
                    }
                    m_currentProductionNode.add (m_currentRhsProduction);
                }
                break;
            }
            return result;
        }

        internal bool Invoke__priority_specification__Callback (SyntaxTreeCallbackReason reason, _priority_specification_.production_kind kind, _priority_specification_ p__priority_specification_)
        {
            switch (reason)
            {
                case SyntaxTreeCallbackReason.TraversalPrologueCallbackReason:
                    m_currentRhsProduction.priority = int.Parse (p__priority_specification_.m__number_.text);
                    break;
                default:
                    break;
            }
            return true;
        }

        internal bool Invoke__associativity_specification__Callback (SyntaxTreeCallbackReason reason, _associativity_specification_.production_kind kind, _associativity_specification_ p__associativity_specification_)
        {
            switch (reason)
            {
                case SyntaxTreeCallbackReason.TraversalPrologueCallbackReason:
                {
                    string associativity = "";
                    switch (kind)
                    {
                        case _associativity_specification_.production_kind.g__associativity_specification__1:
                            associativity = p__associativity_specification_.m__cstring_.text;
                            break;
                        case _associativity_specification_.production_kind.g__associativity_specification__2:
                            associativity = p__associativity_specification_.m__identifier_.text;
                            break;
                    }
                    switch (associativity)
                    {
                        case "none":
                            m_currentRhsProduction.associativity = ProductionAssociativity.None;
                            break;
                        case "left":
                            m_currentRhsProduction.associativity = ProductionAssociativity.Left;
                            break;
                        case "right":
                            m_currentRhsProduction.associativity = ProductionAssociativity.Right;
                            break;
                    }
                }
                break;
                default:
                    break;
            }
            return true;
        }

        internal bool Invoke__production_name__Callback (SyntaxTreeCallbackReason reason, _production_name_.production_kind kind, _production_name_ p__production_name_)
        {
            bool result = false;
            switch (reason)
            {
                case SyntaxTreeCallbackReason.BuilderCallbackReason:
                    break;
                case SyntaxTreeCallbackReason.TraversalPrologueCallbackReason:
                {
                    SyntaxTreeBase node;
                    for (node = p__production_name_; (node != null) && !(node is _anglr_syntax_rule_); node = node.parent)
                        ;
                    SymbolToken syntaxRuleName = null;
                    _anglr_syntax_rule_ syntax_Rule_ = (_anglr_syntax_rule_) node;
                    if (syntax_Rule_ != null)
                        syntaxRuleName = (SymbolToken) ((AppInfo) syntax_Rule_.m__identifier_.appInfo) [AppInfoType.SymbolToken];
                    m_currentRhsProduction.productionName =
                        SymbolTable.insert (new SymbolToken ($"{p__production_name_.m__double_at_sign_.text}{p__production_name_.m__identifier_.text}", (uint) AnglrClassificationType.ProductionName, null, syntaxRuleName));
                }
                break;
                case SyntaxTreeCallbackReason.TraversalMidTermCallbackReason:
                    break;
                case SyntaxTreeCallbackReason.TraversalEpilogueCallbackReason:
                    break;
            }
            return result;
        }

        internal bool Invoke__name__Callback (SyntaxTreeCallbackReason reason, _name_.production_kind kind, _name_ p__name_)
        {
            bool result = true;
            switch (reason)
            {
                case SyntaxTreeCallbackReason.BuilderCallbackReason:
                    break;
                case SyntaxTreeCallbackReason.TraversalPrologueCallbackReason:
                    break;
                case SyntaxTreeCallbackReason.TraversalMidTermCallbackReason:
                    break;
                case SyntaxTreeCallbackReason.TraversalEpilogueCallbackReason:
                {
                    int lineno = -1;
                    int column = -1;
                    SymbolToken p_SymbolToken = null;
                    switch (kind)
                    {
                        case _name_.production_kind.g__name__1:
                            p_SymbolToken = new SymbolToken (p__name_.m__any_.text, (uint) AnglrClassificationType.TerminalName, null, declarationPartSymbol);
                            lineno = p__name_.m__any_.lineno;
                            column = p__name_.m__any_.column;
                            break;
                        case _name_.production_kind.g__name__2:
                            p_SymbolToken = new SymbolToken (p__name_.m__cstring_.text, (uint) AnglrClassificationType.TerminalName, null, declarationPartSymbol);
                            lineno = p__name_.m__cstring_.lineno;
                            column = p__name_.m__cstring_.column;
                            break;
                        case _name_.production_kind.g__name__3:
                        {
                            p_SymbolToken = new SymbolToken (p__name_.m__identifier_.text, (uint) AnglrClassificationType.TerminalName, null, declarationPartSymbol);
                            if (SymbolTable.find (p_SymbolToken) == null)
                                p_SymbolToken = new SymbolToken (p__name_.m__identifier_.text, (uint) AnglrClassificationType.NonTerminalName, null, parserPartSymbol);
                            lineno = p__name_.m__identifier_.lineno;
                            column = p__name_.m__identifier_.column;
                        }
                        break;
                    }
                    SymbolToken p_SymbolTokenRef = SymbolTable.insert (p_SymbolToken);
                    if (p_SymbolTokenRef != p_SymbolToken)
                        p_SymbolToken.Dispose ();
                    if ((kind == _name_.production_kind.g__name__2) && (p_SymbolTokenRef.declarator == 0))
                    {   // we got anonymous terminal
                        string text = Regex.Unescape (p__name_.m__cstring_.text.Substring (1, p__name_.m__cstring_.text.Length - 2));
                        if (text.Length == 1)   // text is something like "x" or "\x" - single character terminal
                        {
                            int terminalIndex = text [0];
                            if (terminalIndex > maxToken)
                                maxToken = terminalIndex;
                            tokenFlags [p_SymbolTokenRef.index = terminalIndex] = true;
                        }
                        p_SymbolTokenRef.correctName = string.Format ("_aonymous_{0:D}_", ++anonymousIndex);
                        AnglrLogger?.DebugRawLine ("ANONYMOUS: " + text + " = " + p_SymbolTokenRef.correctName);
                        p_SymbolTokenRef.declarator = (uint) AnglrClassificationType.TerminalName;
                        if (!anonymousTokens.Contains ((text, p_SymbolTokenRef)))
                            anonymousTokens.Add ((text, p_SymbolTokenRef));
                    }
                        ((AppInfo) p__name_.appInfo) [AppInfoType.SymbolToken] = p_SymbolTokenRef;
                    //p_SymbolTokenRef.AddRefInfo (lineno, column, p_SymbolTokenRef.name.Length);
                }
                break;
            }
            return result;
        }

        internal bool Invoke__g_name__Callback (SyntaxTreeCallbackReason reason, _g_name_.production_kind kind, _g_name_ p__g_name_)
        {
            bool status = true;
            switch (reason)
            {
                case SyntaxTreeCallbackReason.BuilderCallbackReason:
                    break;
                case SyntaxTreeCallbackReason.IterationPrologueCallbackReason:
                    break;
                case SyntaxTreeCallbackReason.TraversalPrologueCallbackReason:
                    break;
                case SyntaxTreeCallbackReason.TraversalMidTermCallbackReason:
                    break;
                case SyntaxTreeCallbackReason.TraversalEpilogueCallbackReason:
                    break;
                case SyntaxTreeCallbackReason.IterationEpilogueCallbackReason:
                {
                    p__g_name_.Iterate (null, (node, appData) =>
                    {
                        NormalizeCompoundName (node);
                        return null;
                    });
                }
                break;
            }
            return status;
        }

        internal bool Invoke__marker__Callback (SyntaxTreeCallbackReason reason, _marker_.production_kind kind, _marker_ p__marker_)
        {
            bool status = true;
            switch (reason)
            {
                case SyntaxTreeCallbackReason.BuilderCallbackReason:
                    break;
                case SyntaxTreeCallbackReason.TraversalPrologueCallbackReason:
                    break;
                case SyntaxTreeCallbackReason.TraversalMidTermCallbackReason:
                    break;
                case SyntaxTreeCallbackReason.TraversalEpilogueCallbackReason:
                {
                    SyntaxTreeToken syntaxTreeToken = p__marker_.m__identifier_;
                    SymbolToken p_SymbolToken = new SymbolToken (syntaxTreeToken.text, (uint) AnglrClassificationType.MarkerName, null, m_currentRhsProduction.productionName);
                    SymbolToken p_SymbolTokenRef = SymbolTable.insert (p_SymbolToken);
                    if (p_SymbolTokenRef != p_SymbolToken)
                        p_SymbolToken.Dispose ();
                    ((AppInfo) p__marker_.appInfo) [AppInfoType.SymbolToken] = p_SymbolTokenRef;
                    //p_SymbolTokenRef.AddRefInfo (syntaxTreeToken.lineno, syntaxTreeToken.column, syntaxTreeToken.text.Length);
                    //p_SymbolTokenRef.AddDefInfo (syntaxTreeToken.lineno, syntaxTreeToken.column, syntaxTreeToken.text.Length);
                }
                break;
            }
            return status;
        }

        internal bool Invoke__regular_expression_usage__Callback (SyntaxTreeCallbackReason reason, _regular_expression_usage_.production_kind kind, _regular_expression_usage_ p__regular_expression_usage_)
        {
            bool status = true;
            switch (reason)
            {
                case SyntaxTreeCallbackReason.BuilderCallbackReason:
                    break;
                case SyntaxTreeCallbackReason.TraversalPrologueCallbackReason:
                    break;
                case SyntaxTreeCallbackReason.TraversalMidTermCallbackReason:
                    break;
                case SyntaxTreeCallbackReason.TraversalEpilogueCallbackReason:
                    _actions_ actions_ = p__regular_expression_usage_.m__actions_optional_.m__actions_;
                    REUsageInfo usageInfo = new REUsageInfo ();
                    if ((bool) actions_?.Iterate (true, (node, appData) =>
                    {
                        _action_ action_ = node.m__action_;
                        bool doit = (bool) appData;
                        if (!doit)
                        {
                            AnglrLogger?.ErrorLine ($"skipped action: {action_.Emit (-1)}");
                            return doit;
                        }
                        switch ((_action_.production_kind) action_.kind)
                        {
                            case _action_.production_kind.g__action__1:
                            {
                                usageInfo.actions.Add ((ScannerActionCode.skipAction, null));
                                doit = false;
                            }
                            break;
                            case _action_.production_kind.g__action__2:
                            {
                                _terminal_action_ terminal_Action_ = action_.m__terminal_action_;
                                SymbolToken tokenSym = new SymbolToken (terminal_Action_.m__identifier_.text, (uint) AnglrClassificationType.TerminalName, null, declarationPartSymbol);
                                SymbolToken tokenRef = SymbolTable.find (tokenSym);
                                if (tokenRef == null)
                                {
                                    AnglrLogger?.ErrorLine ($"undefined token action symbol: {tokenSym.name}");
                                    break;
                                }
                                usageInfo.actions.Add ((ScannerActionCode.terminalAction, tokenRef.correctName));
                                doit = false;
                            }
                            break;
                            case _action_.production_kind.g__action__3:
                            {
                                _event_action_ event_Action_ = action_.m__event_action_;
                                SymbolToken tokenSym = new SymbolToken (event_Action_.m__identifier_.text, (uint) AnglrClassificationType.EventName, null, scannerPartSymbol);
                                SymbolToken tokenRef = SymbolTable.find (tokenSym);
                                if (tokenRef != null)
                                {
                                    AnglrLogger?.ErrorLine ($"redefined event action symbol: {tokenSym.name}");
                                }
                                else
                                {
                                    regexEventsCode.AppendLine ($"\t\t\tpublic event scannerCallback {tokenSym.correctName}_Event;");
                                    regexEventsExample.AppendLine ($"\t\t\t\t\tscanner.{tokenSym.correctName}_Event += (r, s) => throw new NotImplementedException (\"event {tokenSym.correctName}_Event is not implemented\");");
                                }
                                usageInfo.actions.Add ((ScannerActionCode.eventAction, tokenSym.correctName));
                                doit = false;
                            }
                            break;
                            case _action_.production_kind.g__action__4:
                            {
                                _push_action_ push_Action_ = action_.m__push_action_;
                                SymbolToken tokenSym = new SymbolToken (push_Action_.m__identifier_.text, (uint) AnglrClassificationType.ScannerPartName);
                                SymbolToken tokenRef = SymbolTable.find (tokenSym);
                                if (tokenRef == null)
                                {
                                    AnglrLogger?.ErrorLine ($"undefined push action symbol: {tokenSym.name}");
                                    break;
                                }
                                usageInfo.actions.Add ((ScannerActionCode.pushAction, tokenRef.correctName));
                            }
                            break;
                            case _action_.production_kind.g__action__5:
                            {
                                usageInfo.actions.Add ((ScannerActionCode.popAction, null));
                            }
                            break;
                        }
                        return doit;
                    }))
                    {
                        usageInfo.actions.Add ((ScannerActionCode.skipAction, null));
                        AnglrLogger?.WarnLine ($"default skip action for RE: {p__regular_expression_usage_.m__regular_expression_.Emit (-1)}");
                    }
                    {
                        regexScannerCode.AppendLine ($"\t\t\tcase {lexregindex + 1}:");
                        foreach ((ScannerActionCode action, string name) in usageInfo.actions)
                        {
                            switch (action)
                            {
                                case ScannerActionCode.skipAction:
                                    regexScannerCode.AppendLine ($"\t\t\t\tresult = 0;");
                                    regexScannerCode.AppendLine ($"\t\t\t\tbreak;");
                                    break;
                                case ScannerActionCode.terminalAction:
                                    regexScannerCode.AppendLine ($"\t\t\t\tresult = {declarationPart.declarationsClassName}.tokens.{declarationPart.tokenPrefix}{name};");
                                    regexScannerCode.AppendLine ($"\t\t\t\tbreak;");
                                    break;
                                case ScannerActionCode.eventAction:
                                    regexScannerCode.AppendLine ($"\t\t\t\tresult = {name}_Event?.Invoke (this, Scanner);");
                                    regexScannerCode.AppendLine ($"\t\t\t\tbreak;");
                                    break;
                                case ScannerActionCode.pushAction:
                                    regexScannerCode.AppendLine ($"\t\t\t\tScanner.pushScanner ({scannerParts [name].regexClassName}.Id, text);");
                                    break;
                                case ScannerActionCode.popAction:
                                    regexScannerCode.AppendLine ($"\t\t\t\tScanner.popScanner (text);");
                                    break;
                            }
                        }
                    }
                    {
                        SyntaxTreeToken p_regex = p__regular_expression_usage_.m__regular_expression_;
                        bool expanded = false;
                        string text = declarationPart.RegexResolve (p_regex, ref expanded);
                        try
                        {
                            if (lexregindex++ > 0)
                                regexMatchCode.Append ($"|");
                            regexMatchCode.Append ($"(?<g{lexregindex}>{text})");
                            scannerPart.lexRegSet.Add (text);
                            Regex regex = new Regex (text);
                            if (expanded)
                                AnglrLogger?.DebugRawLine ("Lexer regular expression, input = '" + p_regex.text + "', output = '" + text + "'");
                            else
                                AnglrLogger?.DebugRawLine ("Lexer regular expression = '" + p_regex.text + "'");
                        }
                        catch (Exception e)
                        {
                            AnglrLogger?.ErrorLine ("Error: " + e.Message);
                            if (expanded)
                                AnglrLogger?.ErrorLine ("Error in regular expression, input = '" + p_regex.text + "', output = '" + text + "'");
                            else
                                AnglrLogger?.ErrorLine ("Error in regular expression '" + p_regex.text + "'");
                        }
                    }
                    break;
            }
            return status;
        }

        internal bool Invoke__general_part__Callback (SyntaxTreeCallbackReason reason, _general_part_.production_kind kind, _general_part_ p__general_part_)
        {
            bool status = true;
            switch (reason)
            {
                case SyntaxTreeCallbackReason.BuilderCallbackReason:
                    break;
                case SyntaxTreeCallbackReason.TraversalPrologueCallbackReason:
                {
                    SyntaxTreeToken p_identifier = p__general_part_.m__identifier_;
                    try
                    {
                        generalPart = generalParts [p_identifier.text];
                        ((AppInfo) p_identifier.appInfo) [AppInfoType.SymbolToken] =
                        generalPartSymbol = generalPart.generalPartSymbol;
                    }
                    catch (Exception e)
                    {
                        AnglrLogger?.ErrorLine ("Cannot save general part info for: " + p_identifier.text +
                            "defined at line: " + p_identifier.lineno + ", column: " + p_identifier.column +
                            "; Error: " + e.Message);
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

        internal bool Invoke__declaration_part__Callback (SyntaxTreeCallbackReason reason, _declaration_part_.production_kind kind, _declaration_part_ p__declaration_part_)
        {
            bool status = true;
            switch (reason)
            {
                case SyntaxTreeCallbackReason.BuilderCallbackReason:
                    break;
                case SyntaxTreeCallbackReason.TraversalPrologueCallbackReason:
                {
                    SyntaxTreeToken p_identifier = p__declaration_part_.m__identifier_;
                    try
                    {
                        declarationPart = declarationParts [p_identifier.text];
                        ((AppInfo) p_identifier.appInfo) [AppInfoType.SymbolToken] =
                        declarationPartSymbol = declarationPart.declarationPartSymbol;
                    }
                    catch (Exception e)
                    {
                        AnglrLogger?.ErrorLine (e.Message);
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

        internal bool Invoke__scanner_part__Callback (SyntaxTreeCallbackReason reason, _scanner_part_.production_kind kind, _scanner_part_ p__scanner_part_)
        {
            bool status = true;
            switch (reason)
            {
                case SyntaxTreeCallbackReason.BuilderCallbackReason:
                    break;
                case SyntaxTreeCallbackReason.TraversalPrologueCallbackReason:
                {
                    SyntaxTreeToken p_identifier = p__scanner_part_.m__identifier_;
                    regexMatchCode.Clear ();
                    lexregindex = 0;

                    regexEventsCode.Clear ();
                    regexEventsExample.Clear ();
                    regexScannerCode.Clear ();
                    try
                    {
                        scannerPart = scannerParts [p_identifier.text];
                        ((AppInfo) p_identifier.appInfo) [AppInfoType.SymbolToken] =
                        scannerPartSymbol = scannerPart.scannerPartSymbol;
                        declarationPartSymbol = scannerPart.declarationPartSymbol;
                    }
                    catch (Exception e)
                    {
                        AnglrLogger?.ErrorLine (e.Message);
                    }
                }
                break;
                case SyntaxTreeCallbackReason.TraversalMidTermCallbackReason:
                    break;
                case SyntaxTreeCallbackReason.TraversalEpilogueCallbackReason:
                {
                    AnglrLogger?.DebugRawLine (p__scanner_part_.m__identifier_.text + " = " + regexMatchCode.ToString ());
                    scannerPart.LoadData (regexMatchCode, regexEventsCode, regexEventsExample, regexScannerCode);
                    generateScannerEvent?.Invoke (p__scanner_part_);
                }
                break;
            }
            return status;
        }

        internal bool Invoke__lexer_part__Callback (SyntaxTreeCallbackReason reason, _lexer_part_.production_kind kind, _lexer_part_ p__lexer_part_)
        {
            bool status = true;
            switch (reason)
            {
                case SyntaxTreeCallbackReason.BuilderCallbackReason:
                    break;
                case SyntaxTreeCallbackReason.TraversalPrologueCallbackReason:
                {
                    SyntaxTreeToken p_identifier = p__lexer_part_.m__identifier_;
                    try
                    {
                        lexerPart = lexerParts [p_identifier.text];
                        ((AppInfo) p_identifier.appInfo) [AppInfoType.SymbolToken] =
                        lexerPartSymbol = lexerPart.lexerPartSymbol;
                        lexerClassName = lexerPart.lexerClassName;
                        lexerNameSpace = lexerPart.lexerNameSpace;
                    }
                    catch (Exception e)
                    {
                        AnglrLogger?.ErrorLine (e.Message);
                    }
                }
                break;
                case SyntaxTreeCallbackReason.TraversalMidTermCallbackReason:
                    break;
                case SyntaxTreeCallbackReason.TraversalEpilogueCallbackReason:
                    generateLexerEvent?.Invoke (p__lexer_part_);
                    break;
            }
            return status;
        }

        internal bool Invoke__parser_part__Callback (SyntaxTreeCallbackReason reason, _parser_part_.production_kind kind, _parser_part_ p__parser_part_)
        {
            bool status = true;
            switch (reason)
            {
                case SyntaxTreeCallbackReason.BuilderCallbackReason:
                    break;
                case SyntaxTreeCallbackReason.TraversalPrologueCallbackReason:
                {
                    {
                        InitGeneralizations ();
                        InitGenerator ();
                        m_errorToken = insertSymbol ("error", (uint) AnglrClassificationType.TerminalName, Constants.TOKEN_ERROR, false);
                        m_eofToken = insertSymbol ("$eof-token", (uint) AnglrClassificationType.TerminalName, Constants.TOKEN_EOF, false);
                        m_acceptToken = insertSymbol ("$accept", (uint) AnglrClassificationType.NonTerminalName, Constants.TOKEN_ACCEPT, false);
                    }
                    {
                        SyntaxTreeToken p_identifier = p__parser_part_.m__identifier_;
                        AnglrLogger?.InfoLine ($"Generate parser: {p_identifier.text}");
                        try
                        {
                            parserPart = parserParts [p_identifier.text];
                            parserNameSpace = parserPart.parserNameSpace;
                            parserClassName = parserPart.parserClassName;
                            scannerId = parserPart.lexerId;
                            ((AppInfo) p_identifier.appInfo) [AppInfoType.SymbolToken] =
                            parserPartSymbol = parserPart.parserPartSymbol;
                            declarationPartSymbol = parserPart.declarationPartSymbol;
                            scannerPartSymbol = parserPart.lexerPartSymbol;
                        }
                        catch (Exception e)
                        {
                            AnglrLogger?.ErrorLine (e.Message);
                        }
                    }
                }
                break;
                case SyntaxTreeCallbackReason.TraversalMidTermCallbackReason:
                    break;
                case SyntaxTreeCallbackReason.TraversalEpilogueCallbackReason:
                {
                    int count = m_prodlist.Count;
                    foreach (_anglr_syntax_rule_ syntax_Rule_ in m_generatedRuleList)
                    {
                        TraverseCommon (syntax_Rule_);
                        Traverse (syntax_Rule_);
                    }
                    if (m_startProductionNode != null)
                    {
                        RhsProduction p_RhsProduction = new RhsProduction (m_acceptToken);
                        p_RhsProduction.add (new RhsNode (m_startProductionNode.ProductionName));
                        p_RhsProduction.add (new RhsNode (m_eofToken));

                        RhsProductionNode p_RhsProductionNode = new RhsProductionNode (m_acceptToken);
                        p_RhsProductionNode.add (p_RhsProduction);

                        m_firstRhsState = new RhsState (m_startProductionNode.ProductionName, m_prodlist);
                        m_firstRhsState.add (new RhsConfiguration (p_RhsProduction, new rhsenumerator (p_RhsProduction.rhsNodes)));

                        m_prodlist [m_acceptToken] = p_RhsProductionNode;
                        m_proddict [p_RhsProduction.productionNumber] = p_RhsProduction;

                        // create terminal and non-terminal codes
                        tokenFlags [Constants.TOKEN_ERROR] = true;
                        tokenFlags [Constants.TOKEN_EOF] = true;
                        tokenFlags [Constants.TOKEN_ACCEPT] = true;
                        m_maxTerminalNr = SymbolTable.createSymbolList (m_terminals, (uint) AnglrClassificationType.TerminalName, declarationPartSymbol);
                        {
                            int index = 258;
                            m_minTerminalNr = index;
                            foreach (SymbolToken symbolToken in m_terminals)
                            {
                                if (symbolToken.context != declarationPartSymbol)
                                    continue;
                                //if (symbolToken.index > 0)
                                //    continue;
                                while ((index < 256) && tokenFlags [index])
                                    ++index;
                                symbolToken.index = index++;
                            }
                            if (index < maxToken)
                                index = maxToken;
                            m_maxTerminalNr = index;
                        }

                        m_maxNonTerminalNr = SymbolTable.createSymbolList (m_nonterminals, (uint) AnglrClassificationType.NonTerminalName, parserPartSymbol);
                        {
                            int index = m_maxTerminalNr;
                            m_minNonTerminalNr = index;
                            foreach (SymbolToken symbolToken in m_nonterminals)
                            {
                                if (symbolToken.context != parserPartSymbol)
                                    continue;
                                //if (symbolToken.index > 0)
                                //    continue;
                                while ((index < 256) && tokenFlags [index])
                                    ++index;
                                symbolToken.index = index++;
                            }
                            if (index < maxToken)
                                index = maxToken;
                            m_maxNonTerminalNr = index;
                        }
                    }

                    if (anonymousTokens.Count > 0)
                    {
                        anonymousSwitch = "\t\tpublic readonly int [] anonymousCode = new int [] { -1";
                        List<string> regSet = null;
                        ScannerPart scannerPart = null;
                        if (scannerParts.TryGetValue (scannerId, out scannerPart))
                            regSet = scannerPart.lexRegSet;
                        anonymousGroup = "";
                        int groupIndex = 0;
                        foreach ((string, SymbolToken) item in anonymousTokens)
                        {
                            AnglrLogger?.WarnLine ("anonymous terminal: " + item.Item1);
                            string text = Regex.Escape (item.Item1);
                            if (regSet != null)
                            {
                                bool ind = false;
                                foreach (string reg in regSet)
                                {
                                    if (reg.StartsWith (text))
                                    {
                                        ind = true;
                                        break;
                                    }
                                }
                                if (ind)
                                    continue;
                            }
                            if (groupIndex++ > 0)
                                anonymousGroup += '|';
                            anonymousGroup += "(?<g" + groupIndex + ">^" + Regex.Escape (item.Item1) + ")";
                            anonymousSwitch += ", " + item.Item2.index;
                        }
                        AnglrLogger?.WarnLine ("anonymous group: " + anonymousGroup);
                        anonymousSwitch += " };";
                    }

                    generateParserEvent?.Invoke (p__parser_part_);
                }
                break;
            }
            return status;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="name"></param>
        /// <param name="declarator"></param>
        /// <param name="index"></param>
        /// <param name="reportDefined"></param>
        /// <returns></returns>
        private SymbolToken insertSymbol (string name, uint declarator, int index, bool reportDefined = true)
        {
            SymbolToken p_symbolToken = new SymbolToken (name, declarator);
            SymbolToken p_symbolReference = SymbolTable.insert (p_symbolToken);
            if (p_symbolReference != p_symbolToken)
            {
                if (reportDefined)
                    AnglrLogger?.WarnLine ("redefined symbol '" + p_symbolToken.name + "'");
            }
            p_symbolReference.index = index;
            return p_symbolReference;
        }

        public void InvokeProductionsIterator (Func<SymbolToken, RhsProductionNode, int> func)
        {
            foreach (SymbolToken symbol in m_prodlist.Keys)
            {
                func (symbol, m_prodlist [symbol]);
            }
        }

        public RhsProductionNode GetProductionNode (string name)
        {
            RhsProductionNode prod = null;
            m_prodlist.TryGetValue (new SymbolToken (name), out prod);
            return prod;
        }

        [Serializable]
        internal enum AssemblySigningMode
        {
            UnknownSigningMode,
            NotSigning,
            DelaySigned,
            Signed
        };
        internal string lexerCode { get; private set; } = "";
        internal string regexCode { get; private set; } = "";
        internal StringBuilder regexMatchCode { get; private set; } = new StringBuilder ();
        internal StringBuilder regexEventsCode { get; private set; } = new StringBuilder ();
        internal StringBuilder regexEventsExample { get; private set; } = new StringBuilder ();
        internal StringBuilder regexScannerCode { get; private set; } = new StringBuilder ();

        // Parser attribute values
        internal string parserNameSpace { get; private set; } = "";
        internal string parserClassName { get; private set; } = "";
        internal string scannerId { get; private set; } = "";

        // Scanner attribute values
        internal string lexerNameSpace { get; private set; } = "";
        internal string lexerClassName { get; private set; } = "";

        // Regex attribute values
        internal string regexClassName { get; private set; } = "";

        internal SymbolToken m_errorToken { get; private set; } = null;
        internal SymbolToken m_eofToken { get; private set; } = null;
        internal SymbolToken m_acceptToken { get; private set; } = null;

        internal prodlist m_prodlist { get; private set; } = new prodlist ();
        internal proddict m_proddict { get; private set; } = new proddict ();
        internal prodlist m_cascades { get; private set; } = new prodlist ();

        public _anglr_syntax_rule_ startSyntaxRule { get; set; } = null;
        internal RhsProductionNode m_startProductionNode { get; private set; } = null;
        internal RhsProductionNode m_currentProductionNode { get; private set; } = null;
        internal RhsProduction m_currentRhsProduction { get; private set; } = null;
        internal RhsState m_firstRhsState { get; private set; } = null;

        internal tokvec m_terminals { get; private set; } = new tokvec ();
        internal tokvec m_nonterminals { get; private set; } = new tokvec ();

        internal int m_minTerminalNr { get; private set; } = -1;
        internal int m_maxTerminalNr { get; private set; } = -1;
        internal int m_minNonTerminalNr { get; private set; } = -1;
        internal int m_maxNonTerminalNr { get; private set; } = -1;

        internal IAnglrLogger AnglrLogger { get; private set; }
        internal SymbolTable SymbolTable { get; private set; }
        internal _anglr_file_fragment_ _anglr_file_fragment_ { get; private set; } = null;
        internal string outputDir { get; private set; } = "";

        internal AnglrAttributeCollection attributeCollection { get; private set; } = null;
        public GeneralParts generalParts { get; private set; } = new GeneralParts ();
        internal GeneralPart generalPart { get; private set; } = null;
        public DeclarationParts declarationParts { get; private set; } = new DeclarationParts ();
        internal DeclarationsPart declarationPart { get; private set; } = null;
        public ScannerParts scannerParts { get; private set; } = new ScannerParts ();
        internal ScannerPart scannerPart { get; private set; } = null;
        public LexerParts lexerParts { get; private set; } = new LexerParts ();
        internal LexerPart lexerPart { get; private set; } = null;
        public ParserParts parserParts { get; private set; } = new ParserParts ();
        internal ParserPart parserPart { get; private set; } = null;

        private SymbolToken generalPartSymbol = null;
        private SymbolToken declarationPartSymbol = null;
        private SymbolToken scannerPartSymbol = null;
        private SymbolToken lexerPartSymbol = null;
        private SymbolToken parserPartSymbol = null;

        internal int scannerPartsCounter { get; private set; } = 0;

        internal regset anonymousTokens { get; private set; } = new regset ();
        internal string anonymousGroup { get; private set; } = "";
        internal string anonymousSwitch { get; private set; } = "";
        internal int anonymousIndex { get; private set; } = 0;

        private bool [] tokenFlags = new bool [256];
        private int maxToken = 0;
        private int lexregindex;

        private Stack<string> gnames = new Stack<string> ();
    }
}
