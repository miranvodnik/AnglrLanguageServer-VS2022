using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using System.Collections.Concurrent;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using Anglr.Parser;
using Anglr.Parser.Core;
using Anglr.Parser.SyntaxTree;
using Anglr.Compiler;
using AnglrParserLibrary;
using System.ComponentModel;
using static Anglr.Declarations.AnglrDeclarations;
using AnglrLogLibrary;

namespace AnglrLibrary
{
    /// <summary>
    /// classification types for tokens in ANGLR files
    /// </summary>
    public enum AnglrClassificationType
    {
        Undefined,
        ReservedWord,
        GeneralPartName,
        DeclarationsPartName,
        ScannerPartName,
        LexerPartName,
        ParserPartName,
        PartBracket,
        Bracket,
        RegularExpression,
        AttributeName,
        PropertyName,
        OperatorSignature,
        PropertyValue,
        NonTerminalName,
        NonTerminalNameDef,
        NonTerminalNameRef,
        RegexName,
        TerminalName,
        TerminalNameDef,
        TerminalNameRef,
        ProductionName,
        MarkerName,
        Literal,
        StringLiteral,
        NumericalLiteral,
        Code,
        GroupName,
        AttributeList,
        ScannerAction,
        EventName
    }

    /// <summary>
    /// span comparator: it defines relation &lt; on set of spans. span s1
    /// is 'less' than span s2 if it appears before span s2. If spans
    /// intersect they are considered equal
    /// </summary>
    internal class symSpanCmp : IComparer<(int line, int column, int length)>
    {
        public int Compare ((int line, int column, int length) x, (int line, int column, int length) y)
        {
            int diff = x.line - y.line;
            if (diff != 0)
                return diff;
            if ((x.length == 0) && (y.length == 0))
                return x.column - y.column;
            if (x.length == 0)
                return (x.column < y.column) ? -1 : (y.column + y.length < x.column) ? 1 : 0;
            if (y.length == 0)
                return (x.column + x.length < y.column) ? -1 : (y.column < x.column) ? 1 : 0;
            return (x.column + x.length <= y.column) ? -1 : (y.column + y.length <= x.column) ? 1 : 0;
        }
    }

    public class SymSpanInfo
    {
        public SymSpanInfo (SyntaxTreeToken token, AnglrClassificationType classificationType, SyntaxTreeBase syntax = null)
        {
            this.token = token;
            this.classificationType = classificationType;
            this.syntax = syntax;
        }
        public SyntaxTreeToken token { get; private set; }
        public AnglrClassificationType classificationType { get; private set; }
        public SyntaxTreeBase syntax { get; private set; }
    }

    /// <summary>
    /// set of pairs (SyntaxTreeToken, AnglrClassificationType) accessed with
    /// keys in the form of span tuples (int, int, int) which are linearly
    /// ordered with relation symSpanCmp. Pairs represent tokens and their
    /// classification types in given ANGLR file
    /// </summary>
    internal class SymSpanDictionary : SortedDictionary<(int, int, int), SymSpanInfo>
    {
        public SymSpanDictionary () : base (new symSpanCmp ()) { }
    }

    /// <summary>
    /// unordered list of pairs (SyntaxTreeToken, AnglrClassificationType), typically
    /// representing all tokens and their classification types in single line
    /// of ANGLR source file
    /// </summary>
    public class SymSpanList : List<SymSpanInfo> { }

    /// <summary>
    /// class which builds token sets in such form that they are easyly used
    /// for different VisualStudio editor operations like: hover, symbol references,
    /// token classification, intellisense
    /// </summary>
    public class AnglrSpanGenerator : SyntaxTreeWalker
    {
        public anglrCompiler Compiler {  get; private set; }
        public SymbolTable SymbolTable { get; private set; }
        public IAnglrLogger AnglrLogger { get; private set; }

        internal SymSpanDictionary symbolSpans = new SymSpanDictionary ();
        internal SymSpanList [] symSpanLists { get; set; }
        private SymbolToken startSymbolToken = null;
        private SymbolToken firstNonTerminalToken = null;
        private Regex regparts = new Regex (@"{([a-zA-Z_][a-zA-Z0-9_.-]*|<[a-zA-Z0-9_ .-]+>)}", RegexOptions.None);
        private int productionCounter = 0;
        private uint attributeListCounter = 0;
        private Stack<SymbolToken> symbolTokens = new Stack<SymbolToken> ();

        private SymbolToken generalPartSymbol = null;
        private SymbolToken declarationsPartSymbol = null;
        private SymbolToken scannerPartSymbol = null;
        private SymbolToken lexerPartSymbol = null;
        private SymbolToken parserPartSymbol = null;

        /// <summary>
        /// constructor initializes event listeners which are fired
        /// when traversing ANGLR syntax tree
        /// </summary>
        /// <param name="lineCount"></param>
        public AnglrSpanGenerator (anglrCompiler compiler)
        {
            Compiler = compiler;
            SymbolTable = Compiler?.symbolTable;
            AnglrLogger = Compiler?.AnglrLogger ?? new VoidAnglrLogger ();

            symbolTokens.Push (null);
            try
            {
                symSpanLists = new SymSpanList [compiler.lineCounter + 1];
            }
            catch
            {
                symSpanLists = null;
            }

            //
            // event registration templates
            //
            Common_Event += Invoke_Common_Callback;
            _anglr_file__Event += Invoke__anglr_file__Callback;
            _anglr_file_part_list__Event += Invoke__anglr_file_part_list__Callback;
            _general_part__Event += Invoke__general_part__Callback;
            _declaration_part__Event += Invoke__declaration_part__Callback;
            _scanner_part__Event += Invoke__scanner_part__Callback;
            _regular_expression_usage__Event += Invoke__regular_expression_usage__Callback;
            _skip_action__Event += Invoke__skip_action__Callback;
            _terminal_action__Event += Invoke__terminal_action__Callback;
            _event_action__Event += Invoke__event_action__Callback;
            _push_action__Event += Invoke__push_action__Callback;
            _pop_action__Event += Invoke__pop_action__Callback;
            _lexer_part__Event += Invoke__lexer_part__Callback;
            _parser_part__Event += Invoke__parser_part__Callback;
            _attribute_list__Event += Invoke__attribute_list__Callback;
            _attribute__Event += Invoke__attribute__Callback;
            _name_value_pair__Event += Invoke__name_value_pair__Callback;
            _single_terminal_definition__Event += Invoke__single_terminal_definition__Callback;
            _single_regex_definition__Event += Invoke__single_regex_definition__Callback;
            _block_of_terminal_definitions__Event += Invoke__block_of_terminal_definitions__Callback;
            _block_of_regex_definitions__Event += Invoke__block_of_regex_definitions__Callback;
            _terminal_definition__Event += Invoke__terminal_definition__Callback;
            _regex_definition__Event += Invoke__regex_definition__Callback;
            _anglr_syntax_rule__Event += Invoke__anglr_syntax_rule__Callback;
            _anglr_syntax_production_list__Event += Invoke__anglr_syntax_production_list__Callback;
            _anglr_syntax_production__Event += Invoke__anglr_syntax_production__Callback;
            _priority_specification__Event += Invoke__priority_specification__Callback;
            _associativity_specification__Event += Invoke__associativity_specification__Callback;
            _production_name__Event += Invoke__production_name__Callback;
            _g_name__Event += Invoke__g_name__Callback;
            _marker__Event += Invoke__marker__Callback;
            _name__Event += Invoke__name__Callback;
            _cardinality__Event += Invoke__cardinality__Callback;
            _delimiter__Event += Invoke__delimiter__Callback;
            _cstring_optional__Event += Invoke__cstring_optional__Callback;
            _anglr_syntax_production_list_name__Event += Invoke__anglr_syntax_production_list_name__Callback;
            _number_optional__Event += Invoke__number_optional__Callback;
        }

        /// <summary>
        /// save token and its classification type in such way that it
        /// is easily accessible by position 
        /// </summary>
        /// <param name="syntaxTreeToken"></param> token reference in syntax tree
        /// <param name="anglrClassificationType"></param> classification type of token
        private void addSymSpan (SyntaxTreeToken syntaxTreeToken, AnglrClassificationType anglrClassificationType, SyntaxTreeBase displayTreeToken = null)
        {
            SymSpanInfo symSpanInfo = symbolSpans [(syntaxTreeToken.lineno, syntaxTreeToken.column, syntaxTreeToken.text.Length)] = new SymSpanInfo (syntaxTreeToken, anglrClassificationType, displayTreeToken);
            try
            {
                SymSpanList spanList = symSpanLists [syntaxTreeToken.lineno];
                if (spanList == null)
                    spanList = symSpanLists [syntaxTreeToken.lineno] = new SymSpanList ();
                spanList.Add (symSpanInfo);
            }
            catch
            {

            }
        }

        /// <summary>
        /// find token and its classification type on a given
        /// position in text file
        /// </summary>
        /// <param name="tokenPositionDescriptor"></param> token position in the form of span
        /// <returns></returns>
        public SymSpanInfo findSymSpan ((int lineno, int column, int length) tokenPositionDescriptor)
        {
            SymSpanInfo tokenDes = new SymSpanInfo (null, AnglrClassificationType.Undefined, null);
            _ = symbolSpans.TryGetValue (tokenPositionDescriptor, out tokenDes);
            return tokenDes;
        }

        /// <summary>
        /// find all tokens and their classification types in a given span
        /// </summary>
        /// <param name="tokenRange"></param> span of given length and position
        /// <param name="tokenList"></param> reference to list of tokens and their
        /// classification types
        public void findLineSymSpans ((int lineno, int column, int length) tokenRange, ref List<SymSpanInfo> tokenList)
        {
            SymSpanInfo tokenDes = new SymSpanInfo (null, AnglrClassificationType.Undefined, null);
            _ = symbolSpans.TryGetValue (tokenRange, out tokenDes);
            if (tokenDes.token == null)
                return;
            tokenList.Add (tokenDes);
            SyntaxTreeToken token = tokenDes.token;
            if (token.column > tokenRange.column)
                findLineSymSpans ((tokenRange.lineno, tokenRange.column, token.column - tokenRange.column), ref tokenList);
            if (token.column + token.text.Length < tokenRange.column + tokenRange.length)
                findLineSymSpans ((tokenRange.lineno, token.column + token.text.Length, tokenRange.column + tokenRange.length - (token.column + token.text.Length)), ref tokenList);
        }

        /// <summary>
        /// find all tokens and their classification types for given line
        /// </summary>
        /// <param name="lineno"></param>
        /// <returns></returns>
        public List<SymSpanInfo> findLineSymSpans (int lineno)
        {
            try
            {
                return symSpanLists [lineno];
            }
            catch
            {
                return null;
            }
        }

        /// <summary>
        /// disply all tokens and their classification types in file using specified text writer
        /// </summary>
        /// <param name="writer"></param> text writer
        public void displaySymSpans (TextWriter writer)
        {
            foreach (SymSpanInfo symSpanInfo in symbolSpans.Values)
                writer.WriteLine ($"({symSpanInfo.token.lineno}, {symSpanInfo.token.column}): {symSpanInfo.token.text}");
        }

        /// <summary>
        /// display an unordered list of tokens and their classification types
        /// </summary>
        /// <param name="writer"></param> text writer
        /// <param name="spanList"></param> list of tokens and classification types
        public void displaySymSpanList (TextWriter writer, SymSpanList spanList)
        {
            try
            {
                foreach (SymSpanInfo symSpanInfo in spanList)
                    writer.WriteLine ($"({symSpanInfo.token.lineno}, {symSpanInfo.token.column}): {symSpanInfo.token.text}");
            }
            catch (Exception e)
            {
                writer.WriteLine (e.Message);
            }
        }

        /// <summary>
        /// save token symbol and all references into symbol table
        /// </summary>
        /// <param name="syntaxTreeToken"></param> token representing symbol to be saved
        /// <param name="anglrClassificationType"></param> classification type used as symbol type
        /// <param name="definition"></param> definition or reference?
        /// <returns></returns>
        private SymbolToken addToSymbolTable (SyntaxTreeToken syntaxTreeToken, SymbolToken contextSymbol, AnglrClassificationType anglrClassificationType, bool definition)
        {
            SymbolToken symbolToken = new SymbolToken (syntaxTreeToken.text, (uint) anglrClassificationType, null, contextSymbol, true, syntaxTreeToken.lineno, syntaxTreeToken.column);
            SymbolToken symbolTokenRef = SymbolTable.insert (symbolToken);
            if (symbolTokenRef != symbolToken)
            {
                if (symbolTokenRef.declarator != (uint) AnglrClassificationType.Undefined)
                {
                    if ((symbolToken.declarator != (uint) AnglrClassificationType.Undefined) && (symbolToken.declarator != symbolTokenRef.declarator))
                        AnglrLogger?.ErrorLine ($"declaration conflict for symbol {symbolTokenRef.name}, previous declarator = {symbolTokenRef.declarator}, attempted declarator = {symbolToken.declarator}");
                }
                else
                {
                    if (definition)
                        symbolTokenRef.declarator = symbolToken.declarator;
                }
                symbolToken.Dispose ();
            }
            if (definition)
                symbolTokenRef.AddDefInfo (syntaxTreeToken.lineno, syntaxTreeToken.column, syntaxTreeToken.text.Length);
            else
                symbolTokenRef.AddRefInfo (syntaxTreeToken.lineno, syntaxTreeToken.column, syntaxTreeToken.text.Length);
            if (syntaxTreeToken.appInfo == null)
                syntaxTreeToken.appInfo = new AppInfo ();
            ((AppInfo) syntaxTreeToken.appInfo) [AppInfoType.SymbolToken] = symbolTokenRef;
            return symbolTokenRef;
        }

        /// <summary>
        /// display all undefined symbols in ANGLR file
        /// </summary>
        private void displayUndefinedSymbols ()
        {
            for (symtab.Enumerator enumerator = SymbolTable.enumerator; enumerator.MoveNext ();)
            {
                KeyValuePair<SymbolToken, SymbolToken> keyValuePair = enumerator.Current;
                SymbolToken symbolToken = keyValuePair.Value;
                if (symbolToken.declarator == (uint) AnglrClassificationType.Undefined)
                    AnglrLogger?.ErrorLine ($"undefined symbol {symbolToken.name}");
            }
        }

        /// <summary>
        /// prepare syntax tree: create application info in every node of the tree
        /// </summary>
        /// <param name="reason"></param>
        /// <param name="kind"></param>
        /// <param name="p_node"></param>
        /// <returns></returns>
        private bool Invoke_Common_Callback (SyntaxTreeCallbackReason reason, int kind, SyntaxTreeBase p_node)
        {
            if (reason == SyntaxTreeCallbackReason.TraversalPrologueCallbackReason)
                p_node.appInfo = new AppInfo ();
            return true;
        }

        /// <summary>
        /// at the end of syntax tree traversal do following things:
        /// - check if grammar is empty
        /// - check if start rule is specified
        /// - if start rule is not specified let first syntax rule
        ///   become the starting rule if such rule exists
        /// - display all undefined symbols
        /// </summary>
        /// <param name="reason"></param>
        /// <param name="kind"></param>
        /// <param name="p__anglr_file_"></param>
        /// <returns></returns>
        private bool Invoke__anglr_file__Callback (SyntaxTreeCallbackReason reason, _anglr_file_.production_kind kind, _anglr_file_ p__anglr_file_)
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
                    if (firstNonTerminalToken == null)
                        AnglrLogger?.WarnLine ("Warning, no grammar rule specified");
                    if (startSymbolToken == null)
                    {
                        AnglrLogger?.WarnLine ("Warning, no starting grammar rule specified");
                        if (firstNonTerminalToken != null)
                            AnglrLogger?.WarnLine ($", first grammar rule {firstNonTerminalToken.name} specified in source file taken as starting grammar rule");
                    }
                    displayUndefinedSymbols ();
                    SymbolTable.print (AnglrLogger);
                }
                break;
            }
            return status;
        }

        /// <summary>
        /// customize traversal of &lt;anglr file part list> syntax rule. Comments
        /// explaining customisation logic are intermixed with source code.
        /// </summary>
        /// <param name="reason"></param>
        /// <param name="kind"></param>
        /// <param name="p__anglr_file_part_list_"></param>
        /// <returns></returns>
        private bool Invoke__anglr_file_part_list__Callback (SyntaxTreeCallbackReason reason, _anglr_file_part_list_.production_kind kind, _anglr_file_part_list_ p__anglr_file_part_list_)
        {
            bool result = false;    // custom logic for compilation of different source file parts
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
                p__anglr_file_part_list_.Iterate (null, (node, appData) =>
                    {
                        _anglr_file_part_ p__anglr_file_part_ = node.m__anglr_file_part_;
                        if (p__anglr_file_part_.kind == (uint) _anglr_file_part_.production_kind.g__anglr_file_part__1)
                            Traverse (p__anglr_file_part_.m__general_part_);
                        return null;
                    });
                    // 2nd step: compile all declaration parts in order of appearance
                    p__anglr_file_part_list_.Iterate (null, (node, appData) =>
                    {
                        _anglr_file_part_ p__anglr_file_part_ = node.m__anglr_file_part_;
                        if (p__anglr_file_part_.kind == (uint) _anglr_file_part_.production_kind.g__anglr_file_part__2)
                            Traverse (p__anglr_file_part_.m__declaration_part_);
                        return null;
                    });
                    // 3rd step: compile all scanner parts in order of appearance
                    p__anglr_file_part_list_.Iterate (null, (node, appData) =>
                    {
                        _anglr_file_part_ p__anglr_file_part_ = node.m__anglr_file_part_;
                        if (p__anglr_file_part_.kind == (uint) _anglr_file_part_.production_kind.g__anglr_file_part__3)
                            Traverse (p__anglr_file_part_.m__scanner_part_);
                        return null;
                    });
                    // 4th step: compile all lexer parts in order of appearance
                    p__anglr_file_part_list_.Iterate (null, (node, appData) =>
                    {
                        _anglr_file_part_ p__anglr_file_part_ = node.m__anglr_file_part_;
                        if (p__anglr_file_part_.kind == (uint) _anglr_file_part_.production_kind.g__anglr_file_part__4)
                            Traverse (p__anglr_file_part_.m__lexer_part_);
                        return null;
                    });
                    // 5th step: compile all parser parts in order of appearance
                    p__anglr_file_part_list_.Iterate (null, (node, appData) =>
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

        /// <summary>
        /// following methods are event handlers for different syntax tree traversal evets
        /// All of then fall in two categories:
        /// - tokenizers: they collect tokens and define their classification types for
        ///   given syntax rule. But not for all tokens. It collects only those tokens
        ///   which are represented by terminals in given syntax rules. Other tokens
        ///   are collected by other tokenizers - event handlers (methods) associated
        ///   with syntax rules for nonterminal symbols apearing in given syntax rule
        /// - customization methods: they are designed to change deep recursion calls
        ///   into iterative loops
        /// All of them have the following parameters
        /// </summary>
        /// <param name="reason"></param> event handlers should be called at the beginning
        /// of the syntax rule, in the middle between terminal and noterminal symbols, or
        /// at the end of syntax rule. Tokenizers and customizations are invoked at the
        /// beginning of syntax tree. Customisations must be invoked at the beginning
        /// of syntax tree since they will have no effect if they would be invoked later.
        /// <param name="kind"></param> an integer identifying production of syntax rule.
        /// Different productions should be tokenized differently.
        /// <param name="node reference"></param> reference to some node within syntax
        /// tree. Type of that node is associated with particular syntax rule, called
        /// given syntax tree
        /// <returns>
        /// Return code is different for tokenizers and for customizers
        /// - tokenizer must return true to enable tokenizing parts of given syntax rule
        ///   represented by non-terminal symbols
        /// - customizers must return false to prevent deep recursions and to prevent
        ///   multiple tokenisation of same code, since customisers implement its own
        ///   logic of tokenisation of syntax rules associated with non-terminal symbols
        ///   of given syntax rule
        /// </returns>

        /// <summary>
        /// tokenizer for &lt;general part> syntax rule
        /// </summary>
        /// <param name="reason"></param>
        /// <param name="kind"></param>
        /// <param name="p__general_part_"></param>
        /// <returns></returns>
        private bool Invoke__general_part__Callback (SyntaxTreeCallbackReason reason, _general_part_.production_kind kind, _general_part_ p__general_part_)
        {
            bool status = true;
            switch (reason)
            {
                case SyntaxTreeCallbackReason.BuilderCallbackReason:
                    break;
                case SyntaxTreeCallbackReason.TraversalPrologueCallbackReason:
                {
                    addSymSpan (p__general_part_.m__general_, AnglrClassificationType.ReservedWord);
                    addSymSpan (p__general_part_.m__identifier_, AnglrClassificationType.GeneralPartName, p__general_part_);
                    addSymSpan (p__general_part_.m__left_part_bracket_, AnglrClassificationType.PartBracket);
                    addSymSpan (p__general_part_.m__left_part_bracket_, AnglrClassificationType.PartBracket);
                    generalPartSymbol = addToSymbolTable (p__general_part_.m__identifier_, null, AnglrClassificationType.GeneralPartName, true);
                }
                break;
                case SyntaxTreeCallbackReason.TraversalMidTermCallbackReason:
                    break;
                case SyntaxTreeCallbackReason.TraversalEpilogueCallbackReason:
                    break;
            }
            return status;
        }

        /// <summary>
        /// tokenizer of &lt;declaration part> syntax rule
        /// </summary>
        /// <param name="reason"></param>
        /// <param name="kind"></param>
        /// <param name="p__declaration_part_"></param>
        /// <returns></returns>
        private bool Invoke__declaration_part__Callback (SyntaxTreeCallbackReason reason, _declaration_part_.production_kind kind, _declaration_part_ p__declaration_part_)
        {
            bool status = true;
            switch (reason)
            {
                case SyntaxTreeCallbackReason.BuilderCallbackReason:
                    break;
                case SyntaxTreeCallbackReason.TraversalPrologueCallbackReason:
                {
                    addSymSpan (p__declaration_part_.m__declarations_, AnglrClassificationType.ReservedWord);
                    addSymSpan (p__declaration_part_.m__identifier_, AnglrClassificationType.DeclarationsPartName, p__declaration_part_);
                    addSymSpan (p__declaration_part_.m__left_part_bracket_, AnglrClassificationType.PartBracket);
                    addSymSpan (p__declaration_part_.m__right_part_bracket_, AnglrClassificationType.PartBracket);
                    declarationsPartSymbol = addToSymbolTable (p__declaration_part_.m__identifier_, null, AnglrClassificationType.DeclarationsPartName, true);
                }
                break;
                case SyntaxTreeCallbackReason.TraversalMidTermCallbackReason:
                    break;
                case SyntaxTreeCallbackReason.TraversalEpilogueCallbackReason:
                    break;
            }
            return status;
        }

        /// <summary>
        /// tokenizer for &lt;scanner part> syntax rule
        /// </summary>
        /// <param name="reason"></param>
        /// <param name="kind"></param>
        /// <param name="p__scanner_part_"></param>
        /// <returns></returns>
        private bool Invoke__scanner_part__Callback (SyntaxTreeCallbackReason reason, _scanner_part_.production_kind kind, _scanner_part_ p__scanner_part_)
        {
            bool status = true;
            switch (reason)
            {
                case SyntaxTreeCallbackReason.BuilderCallbackReason:
                    break;
                case SyntaxTreeCallbackReason.TraversalPrologueCallbackReason:
                {
                    addSymSpan (p__scanner_part_.m__scanner_, AnglrClassificationType.ReservedWord);
                    addSymSpan (p__scanner_part_.m__identifier_, AnglrClassificationType.ScannerPartName, p__scanner_part_);
                    addSymSpan (p__scanner_part_.m__left_part_bracket_, AnglrClassificationType.PartBracket);
                    addSymSpan (p__scanner_part_.m__right_part_bracket_, AnglrClassificationType.PartBracket);
                    scannerPartSymbol = addToSymbolTable (p__scanner_part_.m__identifier_, null, AnglrClassificationType.ScannerPartName, true);
                }
                break;
                case SyntaxTreeCallbackReason.TraversalMidTermCallbackReason:
                    break;
                case SyntaxTreeCallbackReason.TraversalEpilogueCallbackReason:
                    break;
            }
            return status;
        }

        private bool Invoke__regular_expression_usage__Callback (SyntaxTreeCallbackReason reason, _regular_expression_usage_.production_kind kind, _regular_expression_usage_ p__regular_expression_usage_)
        {
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
                    SyntaxTreeToken p_regex = p__regular_expression_usage_.m__regular_expression_;
                    foreach (Match match in regparts.Matches (p_regex.text))
                    {
                        string partname = match.Value.Substring (1, match.Value.Length - 2);
                        SyntaxTreeToken syntaxTreeToken = new SyntaxTreeToken (p_regex.token, p_regex.lineno, p_regex.column + match.Index + 1, partname);
                        _ = addToSymbolTable (syntaxTreeToken, declarationsPartSymbol, AnglrClassificationType.RegexName, false);
                        addSymSpan (syntaxTreeToken, AnglrClassificationType.RegexName, p_regex);
                    }
                    addSymSpan (p__regular_expression_usage_.m__regular_expression_, AnglrClassificationType.RegularExpression);
                }
                break;
            }
            return true;
        }

        private bool Invoke__skip_action__Callback (SyntaxTreeCallbackReason reason, _skip_action_.production_kind kind, _skip_action_ p__skip_action_)
        {
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
                    addSymSpan (p__skip_action_.m__skip_, AnglrClassificationType.ScannerAction);
                }
                break;
            }
            return true;
        }

        private bool Invoke__terminal_action__Callback (SyntaxTreeCallbackReason reason, _terminal_action_.production_kind kind, _terminal_action_ p__terminal_action_)
        {
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
                    addSymSpan (p__terminal_action_.m__ttoken_, AnglrClassificationType.ScannerAction);
                    addSymSpan (p__terminal_action_.m__identifier_, AnglrClassificationType.TerminalNameRef);
                    _ = addToSymbolTable (p__terminal_action_.m__identifier_, declarationsPartSymbol, AnglrClassificationType.TerminalName, false);
                }
                break;
            }
            return true;
        }

        private bool Invoke__event_action__Callback (SyntaxTreeCallbackReason reason, _event_action_.production_kind kind, _event_action_ p__event_action_)
        {
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
                    addSymSpan (p__event_action_.m__event_, AnglrClassificationType.ScannerAction);
                    addSymSpan (p__event_action_.m__identifier_, AnglrClassificationType.EventName);
                    _ = addToSymbolTable (p__event_action_.m__identifier_, null, AnglrClassificationType.EventName, true);
                }
                break;
            }
            return true;
        }

        private bool Invoke__push_action__Callback (SyntaxTreeCallbackReason reason, _push_action_.production_kind kind, _push_action_ p__push_action_)
        {
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
                    addSymSpan (p__push_action_.m__push_, AnglrClassificationType.ScannerAction);
                    addSymSpan (p__push_action_.m__identifier_, AnglrClassificationType.ScannerPartName);
                    _ = addToSymbolTable (p__push_action_.m__identifier_, null, AnglrClassificationType.ScannerPartName, false);
                }
                break;
            }
            return true;
        }

        private bool Invoke__pop_action__Callback (SyntaxTreeCallbackReason reason, _pop_action_.production_kind kind, _pop_action_ p__pop_action_)
        {
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
                    addSymSpan (p__pop_action_.m__pop_, AnglrClassificationType.ScannerAction);
                }
                break;
            }
            return true;
        }

        private bool Invoke__lexer_part__Callback (SyntaxTreeCallbackReason reason, _lexer_part_.production_kind kind, _lexer_part_ p__lexer_part_)
        {
            bool status = true;
            switch (reason)
            {
                case SyntaxTreeCallbackReason.BuilderCallbackReason:
                    break;
                case SyntaxTreeCallbackReason.TraversalPrologueCallbackReason:
                {
                    addSymSpan (p__lexer_part_.m__lexer_, AnglrClassificationType.ReservedWord);
                    addSymSpan (p__lexer_part_.m__identifier_, AnglrClassificationType.LexerPartName, p__lexer_part_);
                    addSymSpan (p__lexer_part_.m__left_part_bracket_, AnglrClassificationType.PartBracket);
                    addSymSpan (p__lexer_part_.m__right_part_bracket_, AnglrClassificationType.PartBracket);
                    lexerPartSymbol = addToSymbolTable (p__lexer_part_.m__identifier_, null, AnglrClassificationType.LexerPartName, true);
                }
                break;
                case SyntaxTreeCallbackReason.TraversalMidTermCallbackReason:
                    break;
                case SyntaxTreeCallbackReason.TraversalEpilogueCallbackReason:
                    break;
            }
            return status;
        }

        /// <summary>
        /// tokenizer for &lt;parser part> syntax rule
        /// </summary>
        /// <param name="reason"></param>
        /// <param name="kind"></param>
        /// <param name="p__parser_part_"></param>
        /// <returns></returns>
        private bool Invoke__parser_part__Callback (SyntaxTreeCallbackReason reason, _parser_part_.production_kind kind, _parser_part_ p__parser_part_)
        {
            bool status = true;
            switch (reason)
            {
                case SyntaxTreeCallbackReason.BuilderCallbackReason:
                    break;
                case SyntaxTreeCallbackReason.TraversalPrologueCallbackReason:
                {
                    addSymSpan (p__parser_part_.m__parser_, AnglrClassificationType.ReservedWord);
                    addSymSpan (p__parser_part_.m__identifier_, AnglrClassificationType.ParserPartName, p__parser_part_);
                    addSymSpan (p__parser_part_.m__left_part_bracket_, AnglrClassificationType.PartBracket);
                    addSymSpan (p__parser_part_.m__right_part_bracket_, AnglrClassificationType.PartBracket);
                    parserPartSymbol = addToSymbolTable (p__parser_part_.m__identifier_, null, AnglrClassificationType.ParserPartName, true);
                }
                break;
                case SyntaxTreeCallbackReason.TraversalMidTermCallbackReason:
                    break;
                case SyntaxTreeCallbackReason.TraversalEpilogueCallbackReason:
                    break;
            }
            return status;
        }

        /// <summary>
        /// recursion customization for &lt;attribute list> syntax rule
        /// </summary>
        /// <param name="reason"></param>
        /// <param name="kind"></param>
        /// <param name="p__attribute_list_"></param>
        /// <returns></returns>
        private bool Invoke__attribute_list__Callback (SyntaxTreeCallbackReason reason, _attribute_list_.production_kind kind, _attribute_list_ p__attribute_list_)
        {
            bool status = true;
            switch (reason)
            {
                case SyntaxTreeCallbackReason.BuilderCallbackReason:
                    break;
                case SyntaxTreeCallbackReason.TraversalPrologueCallbackReason:
                {
                    symbolTokens.Push (addToSymbolTable (new SyntaxTreeToken (0, -1, -1, $"$AL_{++attributeListCounter}"), null, AnglrClassificationType.AttributeList, true));
                }
                break;
                case SyntaxTreeCallbackReason.TraversalMidTermCallbackReason:
                    break;
                case SyntaxTreeCallbackReason.TraversalEpilogueCallbackReason:
                    symbolTokens.Pop ();
                    break;
            }
            return status;
        }

        /// <summary>
        /// tokenizer for &lt;attribute> syntax rule
        /// </summary>
        /// <param name="reason"></param>
        /// <param name="kind"></param>
        /// <param name="p__parser_part_"></param>
        /// <returns></returns>
        private bool Invoke__attribute__Callback (SyntaxTreeCallbackReason reason, _attribute_.production_kind kind, _attribute_ p__attribute_)
        {
            bool status = true;
            switch (reason)
            {
                case SyntaxTreeCallbackReason.BuilderCallbackReason:
                    break;
                case SyntaxTreeCallbackReason.TraversalPrologueCallbackReason:
                {
                    addSymSpan (p__attribute_.m__left_square_bracket_, AnglrClassificationType.Bracket);
                    addSymSpan (p__attribute_.m__identifier_, AnglrClassificationType.AttributeName, p__attribute_);
                    addSymSpan (p__attribute_.m__right_square_bracket_, AnglrClassificationType.Bracket);
                    SymbolToken symbol = symbolTokens.Peek ();
                    symbolTokens.Push (addToSymbolTable (p__attribute_.m__identifier_, symbol, AnglrClassificationType.AttributeName, true));
                }
                break;
                case SyntaxTreeCallbackReason.TraversalMidTermCallbackReason:
                    break;
                case SyntaxTreeCallbackReason.TraversalEpilogueCallbackReason:
                    symbolTokens.Pop ();
                    break;
            }
            return status;
        }

        /// <summary>
        /// tokenizer for &lt;name value pair> syntax rule
        /// </summary>
        /// <param name="reason"></param>
        /// <param name="kind"></param>
        /// <param name="p__parser_part_"></param>
        /// <returns></returns>
        private bool Invoke__name_value_pair__Callback (SyntaxTreeCallbackReason reason, _name_value_pair_.production_kind kind, _name_value_pair_ p__name_value_pair_)
        {
            bool status = true;
            switch (reason)
            {
                case SyntaxTreeCallbackReason.BuilderCallbackReason:
                    break;
                case SyntaxTreeCallbackReason.TraversalPrologueCallbackReason:
                {
                    addSymSpan (p__name_value_pair_.m__identifier_, AnglrClassificationType.PropertyName, p__name_value_pair_);
                    addSymSpan (p__name_value_pair_.m__equals_sign_, AnglrClassificationType.OperatorSignature);
                    addSymSpan (p__name_value_pair_.m__cstring_, AnglrClassificationType.PropertyValue);
                    _ = addToSymbolTable (p__name_value_pair_.m__identifier_, symbolTokens.Peek (), AnglrClassificationType.PropertyName, true);
                }
                break;
                case SyntaxTreeCallbackReason.TraversalMidTermCallbackReason:
                    break;
                case SyntaxTreeCallbackReason.TraversalEpilogueCallbackReason:
                    break;
            }
            return status;
        }

        private bool Invoke__single_terminal_definition__Callback (SyntaxTreeCallbackReason reason, _single_terminal_definition_.production_kind kind, _single_terminal_definition_ p__single_terminal_definition_)
        {
            bool status = true;
            switch (reason)
            {
                case SyntaxTreeCallbackReason.BuilderCallbackReason:
                    break;
                case SyntaxTreeCallbackReason.TraversalPrologueCallbackReason:
                    addSymSpan (p__single_terminal_definition_.m__terminal_, AnglrClassificationType.ReservedWord);
                    break;
                case SyntaxTreeCallbackReason.TraversalMidTermCallbackReason:
                    break;
                case SyntaxTreeCallbackReason.TraversalEpilogueCallbackReason:
                    break;
            }
            return status;
        }

        private bool Invoke__single_regex_definition__Callback (SyntaxTreeCallbackReason reason, _single_regex_definition_.production_kind kind, _single_regex_definition_ p__single_regex_definition_)
        {
            bool status = true;
            switch (reason)
            {
                case SyntaxTreeCallbackReason.BuilderCallbackReason:
                    break;
                case SyntaxTreeCallbackReason.TraversalPrologueCallbackReason:
                    addSymSpan (p__single_regex_definition_.m__regex_, AnglrClassificationType.ReservedWord);
                    break;
                case SyntaxTreeCallbackReason.TraversalMidTermCallbackReason:
                    break;
                case SyntaxTreeCallbackReason.TraversalEpilogueCallbackReason:
                    break;
            }
            return status;
        }

        private bool Invoke__block_of_terminal_definitions__Callback (SyntaxTreeCallbackReason reason, _block_of_terminal_definitions_.production_kind kind, _block_of_terminal_definitions_ p__block_of_terminal_definitions_)
        {
            bool status = true;
            switch (reason)
            {
                case SyntaxTreeCallbackReason.BuilderCallbackReason:
                    break;
                case SyntaxTreeCallbackReason.TraversalPrologueCallbackReason:
                    addSymSpan (p__block_of_terminal_definitions_.m__terminal_, AnglrClassificationType.ReservedWord);
                    addSymSpan (p__block_of_terminal_definitions_.m__left_curly_bracket_, AnglrClassificationType.Bracket);
                    addSymSpan (p__block_of_terminal_definitions_.m__right_curly_bracket_, AnglrClassificationType.Bracket);
                    break;
                case SyntaxTreeCallbackReason.TraversalMidTermCallbackReason:
                    break;
                case SyntaxTreeCallbackReason.TraversalEpilogueCallbackReason:
                    break;
            }
            return status;
        }

        private bool Invoke__block_of_regex_definitions__Callback (SyntaxTreeCallbackReason reason, _block_of_regex_definitions_.production_kind kind, _block_of_regex_definitions_ p__block_of_regex_definitions_)
        {
            bool status = true;
            switch (reason)
            {
                case SyntaxTreeCallbackReason.BuilderCallbackReason:
                    break;
                case SyntaxTreeCallbackReason.TraversalPrologueCallbackReason:
                    addSymSpan (p__block_of_regex_definitions_.m__regex_, AnglrClassificationType.ReservedWord);
                    addSymSpan (p__block_of_regex_definitions_.m__left_curly_bracket_, AnglrClassificationType.Bracket);
                    addSymSpan (p__block_of_regex_definitions_.m__right_curly_bracket_, AnglrClassificationType.Bracket);
                    break;
                case SyntaxTreeCallbackReason.TraversalMidTermCallbackReason:
                    break;
                case SyntaxTreeCallbackReason.TraversalEpilogueCallbackReason:
                    break;
            }
            return status;
        }

        /// <summary>
        /// tokenizer for &lt;token definition> syntax rule
        /// </summary>
        /// <param name="reason"></param>
        /// <param name="kind"></param>
        /// <param name="p__parser_part_"></param>
        /// <returns></returns>
        private bool Invoke__terminal_definition__Callback (SyntaxTreeCallbackReason reason, _terminal_definition_.production_kind kind, _terminal_definition_ p__terminal_definition_)
        {
            bool status = true;
            switch (reason)
            {
                case SyntaxTreeCallbackReason.BuilderCallbackReason:
                    break;
                case SyntaxTreeCallbackReason.TraversalPrologueCallbackReason:
                {
                    addSymSpan (p__terminal_definition_.m__identifier_, AnglrClassificationType.TerminalNameDef/*, p__terminal_definition_*/);
                    _ = addToSymbolTable (p__terminal_definition_.m__identifier_, declarationsPartSymbol, AnglrClassificationType.TerminalName, true);
                    SyntaxTreeToken syntaxTreeToken = p__terminal_definition_.m__cstring_optional_.m__cstring_;
                    if (syntaxTreeToken == null)
                        break;
                    addSymSpan (syntaxTreeToken, AnglrClassificationType.TerminalNameDef/*, p__token_definition_*/);
                    _ = addToSymbolTable (syntaxTreeToken, declarationsPartSymbol, AnglrClassificationType.TerminalName, true);
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
                    break;
                case SyntaxTreeCallbackReason.TraversalPrologueCallbackReason:
                {
                    SyntaxTreeToken p_regex = p__regex_definition_.m__regular_expression_;
                    foreach (Match match in regparts.Matches (p_regex.text))
                    {
                        string partname = match.Value.Substring (1, match.Value.Length - 2);
                        SyntaxTreeToken syntaxTreeToken = new SyntaxTreeToken (p_regex.token, p_regex.lineno, p_regex.column + match.Index + 1, partname);
                        _ = addToSymbolTable (syntaxTreeToken, declarationsPartSymbol, AnglrClassificationType.RegexName, false);
                        addSymSpan (syntaxTreeToken, AnglrClassificationType.RegexName/*, p_regex*/);
                    }
                    addSymSpan (p__regex_definition_.m__identifier_, AnglrClassificationType.RegexName/*, p__anglr_definition_*/);
                    addSymSpan (p__regex_definition_.m__regular_expression_, AnglrClassificationType.RegularExpression);
                    _ = addToSymbolTable (p__regex_definition_.m__identifier_, declarationsPartSymbol, AnglrClassificationType.RegexName, true);
                }
                break;
                case SyntaxTreeCallbackReason.TraversalMidTermCallbackReason:
                    break;
                case SyntaxTreeCallbackReason.TraversalEpilogueCallbackReason:
                    break;
            }
            return status;
        }

        /// <summary>
        /// tokenizer for &lt;anglr syntax rule> syntax rule
        /// </summary>
        /// <param name="reason"></param>
        /// <param name="kind"></param>
        /// <param name="p__parser_part_"></param>
        /// <returns></returns>
        private bool Invoke__anglr_syntax_rule__Callback (SyntaxTreeCallbackReason reason, _anglr_syntax_rule_.production_kind kind, _anglr_syntax_rule_ p__anglr_syntax_rule_)
        {
            bool status = true;
            switch (reason)
            {
                case SyntaxTreeCallbackReason.BuilderCallbackReason:
                    break;
                case SyntaxTreeCallbackReason.TraversalPrologueCallbackReason:
                {
                    switch (kind)
                    {
                        case _anglr_syntax_rule_.production_kind.g__anglr_syntax_rule__1:
                        {
                            addSymSpan (p__anglr_syntax_rule_.m__identifier_, AnglrClassificationType.NonTerminalNameDef/*, p__anglr_syntax_rule_*/);
                            addSymSpan (p__anglr_syntax_rule_.m__colon_, AnglrClassificationType.OperatorSignature);
                            addSymSpan (p__anglr_syntax_rule_.m__semicolon_, AnglrClassificationType.OperatorSignature);
                            SymbolToken symbolToken = addToSymbolTable (p__anglr_syntax_rule_.m__identifier_, parserPartSymbol, AnglrClassificationType.NonTerminalName, true);
                            if (firstNonTerminalToken == null)
                                firstNonTerminalToken = symbolToken;
                            _attribute_list_optional_ attribute_List_Optional_ = p__anglr_syntax_rule_.m__attribute_list_optional_;
                            if (attribute_List_Optional_.kind != (uint) _attribute_list_optional_.production_kind.g__attribute_list_optional__2)
                                break;
                            if
                                (
                                    (attribute_List_Optional_.m__attribute_list_ == null) ||
                                    !(bool) attribute_List_Optional_.m__attribute_list_.Iterate (false, (node, appData) => { return (bool) appData || (node.m__attribute_.m__identifier_.text == "Start"); })
                                )
                                break;
                            if (startSymbolToken != null)
                                AnglrLogger?.ErrorLine ($"Error, start symbol redefined: previous {startSymbolToken.name} , attempted {symbolToken.name}");
                            else
                                startSymbolToken = symbolToken;
                        }
                        break;
                        case _anglr_syntax_rule_.production_kind.g__anglr_syntax_rule__2:
                        {
                            addSymSpan (p__anglr_syntax_rule_.m__identifier_, AnglrClassificationType.GroupName/*, p__anglr_syntax_rule_*/);
                            addSymSpan (p__anglr_syntax_rule_.m__left_curly_bracket_, AnglrClassificationType.OperatorSignature);
                            addSymSpan (p__anglr_syntax_rule_.m__right_curly_bracket_, AnglrClassificationType.OperatorSignature);
                            SymbolToken symbolToken = addToSymbolTable (p__anglr_syntax_rule_.m__identifier_, parserPartSymbol, AnglrClassificationType.GroupName, true);
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

        /// <summary>
        /// recursion customisation for &lt;anglr syntax production list> syntax rule
        /// </summary>
        /// <param name="reason"></param>
        /// <param name="kind"></param>
        /// <param name="p__parser_part_"></param>
        /// <returns></returns>
        private bool Invoke__anglr_syntax_production_list__Callback (SyntaxTreeCallbackReason reason, _anglr_syntax_production_list_.production_kind kind, _anglr_syntax_production_list_ p__anglr_syntax_production_list_)
        {
            bool status = false;
            switch (reason)
            {
                case SyntaxTreeCallbackReason.BuilderCallbackReason:
                    break;
                case SyntaxTreeCallbackReason.IterationPrologueCallbackReason:
                {
                p__anglr_syntax_production_list_.Iterate (null, (node, appData) =>
                    {
                        if ((_anglr_syntax_production_list_.production_kind) node.kind == _anglr_syntax_production_list_.production_kind.g__anglr_syntax_production_list__2)
                            addSymSpan (node.m__vertical_bar_, AnglrClassificationType.OperatorSignature);
                        Traverse (node.m__anglr_syntax_production_);
                        return null;
                    });
                }
                break;
                case SyntaxTreeCallbackReason.TraversalPrologueCallbackReason:
                    break;
                case SyntaxTreeCallbackReason.TraversalMidTermCallbackReason:
                    break;
                case SyntaxTreeCallbackReason.TraversalEpilogueCallbackReason:
                    break;
                case SyntaxTreeCallbackReason.IterationEpilogueCallbackReason:
                    break;
            }
            return status;
        }

        /// <summary>
        /// tokenizer for &lt;anglr syntax production> syntax rule
        /// </summary>
        /// <param name="reason"></param>
        /// <param name="kind"></param>
        /// <param name="p__parser_part_"></param>
        /// <returns></returns>
        private bool Invoke__anglr_syntax_production__Callback (SyntaxTreeCallbackReason reason, _anglr_syntax_production_.production_kind kind, _anglr_syntax_production_ p__anglr_syntax_production_)
        {
            bool status = true;
            switch (reason)
            {
                case SyntaxTreeCallbackReason.BuilderCallbackReason:
                    break;
                case SyntaxTreeCallbackReason.TraversalPrologueCallbackReason:
                {
                    symbolTokens.Push (addToSymbolTable (new SyntaxTreeToken (0, -1, -1, $"$PN_{++productionCounter}"), parserPartSymbol, AnglrClassificationType.ProductionName, true));
                    if (kind == _anglr_syntax_production_.production_kind.g__anglr_syntax_production__2)
                    {
                        addSymSpan (p__anglr_syntax_production_.m__empty_, AnglrClassificationType.ReservedWord);
                    }
                }
                break;
                case SyntaxTreeCallbackReason.TraversalMidTermCallbackReason:
                    break;
                case SyntaxTreeCallbackReason.TraversalEpilogueCallbackReason:
                    symbolTokens.Pop ();
                    break;
            }
            return status;
        }

        private bool Invoke__priority_specification__Callback (SyntaxTreeCallbackReason reason, _priority_specification_.production_kind kind, _priority_specification_ p__priority_specification_)
        {
            if (reason != SyntaxTreeCallbackReason.TraversalPrologueCallbackReason)
                return true;
            addSymSpan (p__priority_specification_.m__priority_, AnglrClassificationType.ReservedWord);
            addSymSpan (p__priority_specification_.m__number_, AnglrClassificationType.NumericalLiteral);
            return true;
        }

        private bool Invoke__associativity_specification__Callback (SyntaxTreeCallbackReason reason, _associativity_specification_.production_kind kind, _associativity_specification_ p__associativity_specification_)
        {
            if (reason != SyntaxTreeCallbackReason.TraversalPrologueCallbackReason)
                return true;
            addSymSpan (p__associativity_specification_.m__associativity_, AnglrClassificationType.ReservedWord);
            switch (kind)
            {
                case _associativity_specification_.production_kind.g__associativity_specification__1:
                case _associativity_specification_.production_kind.g__associativity_specification__2:
                    addSymSpan (p__associativity_specification_.m__cstring_, AnglrClassificationType.StringLiteral);
                    break;
                case _associativity_specification_.production_kind.g__associativity_specification__3:
                case _associativity_specification_.production_kind.g__associativity_specification__4:
                    addSymSpan (p__associativity_specification_.m__identifier_, AnglrClassificationType.Literal);
                    break;
            }
            return true;
        }

        /// <summary>
        /// tokenizer for &lt;production name> syntax rule
        /// </summary>
        /// <param name="reason"></param>
        /// <param name="kind"></param>
        /// <param name="p__parser_part_"></param>
        /// <returns></returns>
        private bool Invoke__production_name__Callback (SyntaxTreeCallbackReason reason, _production_name_.production_kind kind, _production_name_ p__production_name_)
        {
            bool status = true;
            switch (reason)
            {
                case SyntaxTreeCallbackReason.BuilderCallbackReason:
                    break;
                case SyntaxTreeCallbackReason.TraversalPrologueCallbackReason:
                    addSymSpan (p__production_name_.m__double_at_sign_, AnglrClassificationType.OperatorSignature);
                    addSymSpan (p__production_name_.m__identifier_, AnglrClassificationType.ProductionName/*, p__production_name_*/);
                    SyntaxTreeBase node;
                    for (node = p__production_name_; (node != null) && !(node is _anglr_syntax_rule_); node = node.parent)
                        ;
                    _anglr_syntax_rule_ syntax_Rule_ = (_anglr_syntax_rule_) node;
                    SymbolToken syntaxRuleName = null;
                    try
                    {
                        if (syntax_Rule_ != null)
                            syntaxRuleName = (SymbolToken) ((AppInfo) syntax_Rule_.m__identifier_.appInfo) [AppInfoType.SymbolToken];
                    }
                    catch (Exception e)
                    {
                    }
                    symbolTokens.Push (addToSymbolTable (p__production_name_.m__identifier_, syntaxRuleName, AnglrClassificationType.ProductionName, true));
                    break;
                case SyntaxTreeCallbackReason.TraversalMidTermCallbackReason:
                    break;
                case SyntaxTreeCallbackReason.TraversalEpilogueCallbackReason:
                    symbolTokens.Pop ();
                    break;
            }
            return status;
        }

        /// <summary>
        /// tokenizer for &lt;g name> syntax rule
        /// </summary>
        /// <param name="reason"></param>
        /// <param name="kind"></param>
        /// <param name="p__parser_part_"></param>
        /// <returns></returns>
        private bool Invoke__g_name__Callback (SyntaxTreeCallbackReason reason, _g_name_.production_kind kind, _g_name_ p__g_name_)
        {
            bool status = true;
            switch (reason)
            {
                case SyntaxTreeCallbackReason.BuilderCallbackReason:
                    break;
                case SyntaxTreeCallbackReason.TraversalPrologueCallbackReason:
                {
                    if (kind == _g_name_.production_kind.g__g_name__2)
                    {
                        addSymSpan (p__g_name_.m__left_bracket_, AnglrClassificationType.Bracket);
                        addSymSpan (p__g_name_.m__right_bracket_, AnglrClassificationType.Bracket);
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

        /// <summary>
        /// tokenizer for &lt;marker> syntax rule
        /// </summary>
        /// <param name="reason"></param>
        /// <param name="kind"></param>
        /// <param name="p__parser_part_"></param>
        /// <returns></returns>
        private bool Invoke__marker__Callback (SyntaxTreeCallbackReason reason, _marker_.production_kind kind, _marker_ p__marker_)
        {
            bool status = true;
            switch (reason)
            {
                case SyntaxTreeCallbackReason.BuilderCallbackReason:
                    break;
                case SyntaxTreeCallbackReason.TraversalPrologueCallbackReason:
                {
                    addSymSpan (p__marker_.m__at_sign_, AnglrClassificationType.OperatorSignature);
                    addSymSpan (p__marker_.m__identifier_, AnglrClassificationType.MarkerName/*, p__marker_*/);
                    _ = addToSymbolTable (p__marker_.m__identifier_, symbolTokens.Peek (), AnglrClassificationType.MarkerName, true);
                }
                break;
                case SyntaxTreeCallbackReason.TraversalMidTermCallbackReason:
                    break;
                case SyntaxTreeCallbackReason.TraversalEpilogueCallbackReason:
                    break;
            }
            return status;
        }

        /// <summary>
        /// tokenizer for &lt;name> syntax rule
        /// </summary>
        /// <param name="reason"></param>
        /// <param name="kind"></param>
        /// <param name="p__parser_part_"></param>
        /// <returns></returns>
        private bool Invoke__name__Callback (SyntaxTreeCallbackReason reason, _name_.production_kind kind, _name_ p__name_)
        {
            bool status = true;
            switch (reason)
            {
                case SyntaxTreeCallbackReason.BuilderCallbackReason:
                    break;
                case SyntaxTreeCallbackReason.TraversalPrologueCallbackReason:
                {
                    switch (kind)
                    {
                        case _name_.production_kind.g__name__1:
                            addSymSpan (p__name_.m__any_, AnglrClassificationType.Literal);
                            break;
                        case _name_.production_kind.g__name__2:
                        {
                            SyntaxTreeToken syntaxTreeToken = p__name_.m__cstring_;
                            SymbolToken symbolToken = new SymbolToken (syntaxTreeToken.text, (uint) AnglrClassificationType.TerminalName, null, declarationsPartSymbol);
                            SymbolToken symbolTokenRef = SymbolTable.find (symbolToken);
                            if (symbolTokenRef != null)
                            {
                                symbolTokenRef.AddRefInfo (syntaxTreeToken.lineno, syntaxTreeToken.column, syntaxTreeToken.text.Length);
                                addSymSpan (p__name_.m__cstring_, AnglrClassificationType.TerminalNameRef/*, p__name_*/);
                                ((AppInfo) syntaxTreeToken.appInfo) [AppInfoType.SymbolToken] = symbolTokenRef;
                            }
                            else
                            {
                                _ = addToSymbolTable (syntaxTreeToken, parserPartSymbol, AnglrClassificationType.Undefined, false);
                                addSymSpan (p__name_.m__cstring_, AnglrClassificationType.Undefined/*, p__name_*/);
                            }
                        }
                        break;
                        case _name_.production_kind.g__name__3:
                        {
                            SyntaxTreeToken syntaxTreeToken = p__name_.m__identifier_;
                            SymbolToken symbolToken = new SymbolToken (syntaxTreeToken.text, (uint) AnglrClassificationType.NonTerminalName, null, parserPartSymbol);
                            SymbolToken symbolTokenRef = SymbolTable.find (symbolToken);
                            if (symbolTokenRef != null)
                            {
                                symbolTokenRef.AddRefInfo (syntaxTreeToken.lineno, syntaxTreeToken.column, syntaxTreeToken.text.Length);
                                addSymSpan (p__name_.m__identifier_, AnglrClassificationType.NonTerminalNameRef);
                                ((AppInfo) syntaxTreeToken.appInfo) [AppInfoType.SymbolToken] = symbolTokenRef;
                            }
                            else
                            {
                                symbolToken = new SymbolToken (syntaxTreeToken.text, (uint) AnglrClassificationType.TerminalName, null, declarationsPartSymbol);
                                symbolTokenRef = SymbolTable.find (symbolToken);
                                if (symbolTokenRef != null)
                                {
                                    symbolTokenRef.AddRefInfo (syntaxTreeToken.lineno, syntaxTreeToken.column, syntaxTreeToken.text.Length);
                                    addSymSpan (p__name_.m__identifier_, AnglrClassificationType.TerminalNameRef/*, p__name_*/);
                                    ((AppInfo) syntaxTreeToken.appInfo) [AppInfoType.SymbolToken] = symbolTokenRef;
                                }
                                else
                                {
                                    _ = addToSymbolTable (syntaxTreeToken, parserPartSymbol, AnglrClassificationType.Undefined, false);
                                    addSymSpan (p__name_.m__identifier_, AnglrClassificationType.Undefined/*, p__name_*/);
                                }
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

        /// <summary>
        /// tokenizer for &lt;cardinality> syntax rule
        /// </summary>
        /// <param name="reason"></param>
        /// <param name="kind"></param>
        /// <param name="p__parser_part_"></param>
        /// <returns></returns>
        private bool Invoke__cardinality__Callback (SyntaxTreeCallbackReason reason, _cardinality_.production_kind kind, _cardinality_ p__cardinality_)
        {
            bool status = true;
            switch (reason)
            {
                case SyntaxTreeCallbackReason.BuilderCallbackReason:
                    break;
                case SyntaxTreeCallbackReason.TraversalPrologueCallbackReason:
                {
                    switch (kind)
                    {
                        case _cardinality_.production_kind.g__cardinality__1:
                            addSymSpan (p__cardinality_.m__question_mark_, AnglrClassificationType.OperatorSignature);
                            break;
                        case _cardinality_.production_kind.g__cardinality__2:
                            addSymSpan (p__cardinality_.m__plus_sign_, AnglrClassificationType.OperatorSignature);
                            break;
                        case _cardinality_.production_kind.g__cardinality__3:
                            addSymSpan (p__cardinality_.m__minus_sign_, AnglrClassificationType.OperatorSignature);
                            break;
                        case _cardinality_.production_kind.g__cardinality__4:
                            addSymSpan (p__cardinality_.m__asterisk_, AnglrClassificationType.OperatorSignature);
                            break;
                        case _cardinality_.production_kind.g__cardinality__5:
                            addSymSpan (p__cardinality_.m__slash_, AnglrClassificationType.OperatorSignature);
                            break;
                        case _cardinality_.production_kind.g__cardinality__6:
                            addSymSpan (p__cardinality_.m__inv_plus_sign_, AnglrClassificationType.OperatorSignature);
                            break;
                        case _cardinality_.production_kind.g__cardinality__7:
                            addSymSpan (p__cardinality_.m__inv_minus_sign_, AnglrClassificationType.OperatorSignature);
                            break;
                        case _cardinality_.production_kind.g__cardinality__8:
                            addSymSpan (p__cardinality_.m__inv_asterisk_, AnglrClassificationType.OperatorSignature);
                            break;
                        case _cardinality_.production_kind.g__cardinality__9:
                            addSymSpan (p__cardinality_.m__inv_slash_, AnglrClassificationType.OperatorSignature);
                            break;
                        case _cardinality_.production_kind.g__cardinality__10:
                            addSymSpan (p__cardinality_.m__left_curly_bracket_, AnglrClassificationType.Bracket);
                            addSymSpan (p__cardinality_.m__comma_, AnglrClassificationType.OperatorSignature);
                            addSymSpan (p__cardinality_.m__right_curly_bracket_, AnglrClassificationType.Bracket);
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

        /// <summary>
        /// tokenizer for &lt;delimiter> syntax rule
        /// </summary>
        /// <param name="reason"></param>
        /// <param name="kind"></param>
        /// <param name="p__parser_part_"></param>
        /// <returns></returns>
        private bool Invoke__delimiter__Callback (SyntaxTreeCallbackReason reason, _delimiter_.production_kind kind, _delimiter_ p__delimiter_)
        {
            bool status = true;
            switch (reason)
            {
                case SyntaxTreeCallbackReason.BuilderCallbackReason:
                    break;
                case SyntaxTreeCallbackReason.TraversalPrologueCallbackReason:
                {
                    addSymSpan (p__delimiter_.m__left_square_bracket_, AnglrClassificationType.Bracket);
                    addSymSpan (p__delimiter_.m__right_square_bracket_, AnglrClassificationType.Bracket);
                }
                break;
                case SyntaxTreeCallbackReason.TraversalMidTermCallbackReason:
                    break;
                case SyntaxTreeCallbackReason.TraversalEpilogueCallbackReason:
                    break;
            }
            return status;
        }

        /// <summary>
        /// tokenizer for &lt;cstring optional> syntax rule
        /// </summary>
        /// <param name="reason"></param>
        /// <param name="kind"></param>
        /// <param name="p__parser_part_"></param>
        /// <returns></returns>
        private bool Invoke__cstring_optional__Callback (SyntaxTreeCallbackReason reason, _cstring_optional_.production_kind kind, _cstring_optional_ p__cstring_optional_)
        {
            bool status = true;
            switch (reason)
            {
                case SyntaxTreeCallbackReason.BuilderCallbackReason:
                    break;
                case SyntaxTreeCallbackReason.TraversalPrologueCallbackReason:
                {
                    if (kind == _cstring_optional_.production_kind.g__cstring_optional__2)
                        addSymSpan (p__cstring_optional_.m__cstring_, AnglrClassificationType.StringLiteral);
                }
                break;
                case SyntaxTreeCallbackReason.TraversalMidTermCallbackReason:
                    break;
                case SyntaxTreeCallbackReason.TraversalEpilogueCallbackReason:
                    break;
            }
            return status;
        }

        /// <summary>
        /// tokenizer for &lt;anglr syntax production list name> syntax rule
        /// </summary>
        /// <param name="reason"></param>
        /// <param name="kind"></param>
        /// <param name="p__parser_part_"></param>
        /// <returns></returns>
        private bool Invoke__anglr_syntax_production_list_name__Callback (SyntaxTreeCallbackReason reason, _anglr_syntax_production_list_name_.production_kind kind, _anglr_syntax_production_list_name_ p__anglr_syntax_production_list_name_)
        {
            bool status = true;
            switch (reason)
            {
                case SyntaxTreeCallbackReason.BuilderCallbackReason:
                    break;
                case SyntaxTreeCallbackReason.TraversalPrologueCallbackReason:
                {
                    addSymSpan (p__anglr_syntax_production_list_name_.m__colon_, AnglrClassificationType.OperatorSignature);
                    addSymSpan (p__anglr_syntax_production_list_name_.m__identifier_, AnglrClassificationType.NonTerminalNameDef/*, p__anglr_syntax_production_list_name_*/);
                    addSymSpan (p__anglr_syntax_production_list_name_.m__colon__1, AnglrClassificationType.OperatorSignature);
                    _ = addToSymbolTable (p__anglr_syntax_production_list_name_.m__identifier_, parserPartSymbol, AnglrClassificationType.NonTerminalName, true);
                }
                break;
                case SyntaxTreeCallbackReason.TraversalMidTermCallbackReason:
                    break;
                case SyntaxTreeCallbackReason.TraversalEpilogueCallbackReason:
                    break;
            }
            return status;
        }

        /// <summary>
        /// tokenizer for &lt;number optional> syntax rule
        /// </summary>
        /// <param name="reason"></param>
        /// <param name="kind"></param>
        /// <param name="p__parser_part_"></param>
        /// <returns></returns>
        private bool Invoke__number_optional__Callback (SyntaxTreeCallbackReason reason, _number_optional_.production_kind kind, _number_optional_ p__number_optional_)
        {
            bool status = true;
            switch (reason)
            {
                case SyntaxTreeCallbackReason.BuilderCallbackReason:
                    break;
                case SyntaxTreeCallbackReason.TraversalPrologueCallbackReason:
                {
                    if (kind == _number_optional_.production_kind.g__number_optional__2)
                        addSymSpan (p__number_optional_.m__number_, AnglrClassificationType.Literal);
                }
                break;
                case SyntaxTreeCallbackReason.TraversalMidTermCallbackReason:
                    break;
                case SyntaxTreeCallbackReason.TraversalEpilogueCallbackReason:
                    break;
            }
            return status;
        }
    }
}
