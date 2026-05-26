using System;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using Anglr.Parser;
using Anglr.Parser.Core;
using Anglr.Parser.SyntaxTree;
using AnglrLibrary;

namespace AnglrHtmlColorizerLibrary
{
    internal class AnglrColorizerSymbol
    {
        public string name { get; private set; }
        public AnglrColorizerSymbol context { get; private set; }

        public AnglrColorizerSymbol (string name, AnglrColorizerSymbol context)
        {
            this.name = name;
            this.context = context;
        }
    }

    internal class AnglrColorizerSymbolComparer : IComparer<AnglrColorizerSymbol>
    {
        public int Compare (AnglrColorizerSymbol x, AnglrColorizerSymbol y)
        {
            int ret;
            if (x == null)
                if (y != null)
                    return -1;
                else
                    return 0;
            else if (y == null)
                return 1;
            if ((ret = x.name.CompareTo (y.name)) != 0)
                return ret;
            return Compare (x.context, y.context);
        }
    }

    internal class AnglrColorizerSymbolTable : SortedSet<AnglrColorizerSymbol>
    {
        public AnglrColorizerSymbolTable () : base (new AnglrColorizerSymbolComparer ()) { }
    }

    internal class AnglrHtmlColorizer : SyntaxTreeWalker
    {
        private bool newline = false;
        private int lineno = -1;
        private int column = -1;
        private AnglrColorizerSymbolTable anglrColorizerSymbols = new AnglrColorizerSymbolTable ();
        private AnglrColorizerSymbol parserSymbol = null;
        private int step = 0;

        private string normalizeName (string name) => name.Replace ('<', '-').Replace ('>', '-').Replace (' ', '-');

        public AnglrHtmlColorizer ()
        {
            Common_Event += AnglrHtmlColorizer_Common_Event;
            _anglr_file_fragment__Event += AnglrHtmlColorizer__anglr_file_fragment__Event;

            _attribute__Event += AnglrHtmlColorizer__attribute__Event;
            _name_value_pair__Event += AnglrHtmlColorizer__name_value_pair__Event;

            _general_part__Event += AnglrHtmlColorizer__general_part__Event;

            _declaration_part__Event += AnglrHtmlColorizer__declaration_part__Event;
            _single_terminal_definition__Event += AnglrHtmlColorizer__single_terminal_definition__Event;
            _single_regex_definition__Event += AnglrHtmlColorizer__single_regex_definition__Event;
            _block_of_terminal_definitions__Event += AnglrHtmlColorizer__block_of_terminal_definitions__Event;
            _block_of_regex_definitions__Event += AnglrHtmlColorizer__block_of_regex_definitions__Event;
            _terminal_definition__Event += AnglrHtmlColorizer__terminal_definition__Event;
            _regex_definition__Event += AnglrHtmlColorizer__regex_definition__Event;

            _scanner_part__Event += AnglrHtmlColorizer__scanner_part__Event;
            _regular_expression_usage__Event += AnglrHtmlColorizer__regular_expression_usage__Event;
            _skip_action__Event += AnglrHtmlColorizer__skip_action__Event;
            _terminal_action__Event += AnglrHtmlColorizer__terminal_action__Event;
            _event_action__Event += AnglrHtmlColorizer__event_action__Event;
            _push_action__Event += AnglrHtmlColorizer__push_action__Event;
            _pop_action__Event += AnglrHtmlColorizer__pop_action__Event;

            _lexer_part__Event += AnglrHtmlColorizer__lexer_part__Event;

            _parser_part__Event += AnglrHtmlColorizer__parser_part__Event;
            _anglr_syntax_rule_list__Event += AnglrHtmlColorizer__anglr_syntax_rule_list__Event;
            _anglr_syntax_rule__Event += AnglrHtmlColorizer__anglr_syntax_rule__Event;
            _anglr_syntax_production_list_name__Event += AnglrHtmlColorizer__anglr_syntax_production_list_name__Event;
            _anglr_syntax_production_list__Event += AnglrHtmlColorizer__anglr_syntax_production_list__Event;
            _anglr_syntax_production__Event += AnglrHtmlColorizer__anglr_syntax_production__Event;
            _priority_specification__Event += AnglrHtmlColorizer__priority_specification__Event;
            _associativity_specification__Event += AnglrHtmlColorizer__associativity_specification__Event;
            _production_name__Event += AnglrHtmlColorizer__production_name__Event;
            _marker__Event += AnglrHtmlColorizer__marker__Event;
            _g_name__Event += AnglrHtmlColorizer__g_name__Event;
            _name__Event += AnglrHtmlColorizer__name__Event;
            _cardinality__Event += AnglrHtmlColorizer__cardinality__Event;
            _delimiter__Event += AnglrHtmlColorizer__delimiter__Event;
        }

        private bool AnglrHtmlColorizer_Common_Event (SyntaxTreeCallbackReason reason, int kind, SyntaxTreeBase p_node)
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
                    int tokenlineno = token.lineno;
                    int tokenColumn = token.column;
                    string tokenText = token.text;
                    int tokenLength = tokenText.Length;
                    while (tokenlineno > lineno)
                    {
                        Console.WriteLine ();
                        ++lineno;
                        column = 0;
                        newline = true;
                    }
                    while (column < tokenColumn)
                    {
                        if (newline)
                            Console.Write (WebUtility.HtmlEncode ("    "));
                        else
                            Console.Write (WebUtility.HtmlEncode (" "));
                        ++column;
                    }
                    Console.Write ((string) token.appInfo);
                    column += tokenLength;
                    newline = false;
                }
                break;
                case SyntaxTreeCallbackReason.TraversalMidTermCallbackReason:
                    break;
                case SyntaxTreeCallbackReason.TraversalEpilogueCallbackReason:
                    break;
            }
            return true;
        }

        private bool AnglrHtmlColorizer__anglr_file_fragment__Event (SyntaxTreeCallbackReason reason, _anglr_file_fragment_.production_kind kind, _anglr_file_fragment_ p__anglr_file_fragment_)
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
                    ++step;
                    break;
            }
            return true;
        }

        private bool AnglrHtmlColorizer__attribute__Event (SyntaxTreeCallbackReason reason, _attribute_.production_kind kind, _attribute_ p__attribute_)
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
                    if (step <= 0)
                        break;
                    p__attribute_.m__left_square_bracket_.appInfo = $"<span class=\"attribute-brace\">{WebUtility.HtmlEncode (p__attribute_.m__left_square_bracket_.text)}</span>";
                    p__attribute_.m__identifier_.appInfo = $"<span class=\"attribute-name\" id=\"{normalizeName (p__attribute_.m__identifier_.text)}\">{WebUtility.HtmlEncode (p__attribute_.m__identifier_.text)}</span>";
                    p__attribute_.m__right_square_bracket_.appInfo = $"<span class=\"attribute-brace\">{WebUtility.HtmlEncode (p__attribute_.m__right_square_bracket_.text)}</span>";
                }
                break;
            }
            return true;
        }

        private bool AnglrHtmlColorizer__name_value_pair__Event (SyntaxTreeCallbackReason reason, _name_value_pair_.production_kind kind, _name_value_pair_ p__name_value_pair_)
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
                    if (step <= 0)
                        break;
                    p__name_value_pair_.m__identifier_.appInfo = $"<span class=\"attribute-val-name\" id=\"{normalizeName (p__name_value_pair_.m__identifier_.text)}\">{WebUtility.HtmlEncode (p__name_value_pair_.m__identifier_.text)}</span>";
                    p__name_value_pair_.m__equals_sign_.appInfo = $"<span class=\"def-separator\">{WebUtility.HtmlEncode (p__name_value_pair_.m__equals_sign_.text)}</span>";
                    p__name_value_pair_.m__cstring_.appInfo = $"<span class=\"attribute-val-value\">{WebUtility.HtmlEncode (p__name_value_pair_.m__cstring_.text)}</span>";
                }
                break;
            }
            return true;
        }

        private bool AnglrHtmlColorizer__general_part__Event (SyntaxTreeCallbackReason reason, _general_part_.production_kind kind, _general_part_ p__general_part_)
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
                    if (step <= 0)
                        break;
                    p__general_part_.m__general_.appInfo = $"<span class=\"part-token\">{WebUtility.HtmlEncode (p__general_part_.m__general_.text)}</span>";
                    p__general_part_.m__identifier_.appInfo = $"<span class=\"part-name\" id=\"{normalizeName (p__general_part_.m__identifier_.text)}\">{WebUtility.HtmlEncode (p__general_part_.m__identifier_.text)}</span>";
                    p__general_part_.m__left_part_bracket_.appInfo = $"<span class=\"part-brace\">{WebUtility.HtmlEncode (p__general_part_.m__left_part_bracket_.text)}</span>";
                    p__general_part_.m__right_part_bracket_.appInfo = $"<span class=\"part-brace\">{WebUtility.HtmlEncode (p__general_part_.m__right_part_bracket_.text)}</span>";
                }
                break;
            }
            return true;
        }

        private bool AnglrHtmlColorizer__declaration_part__Event (SyntaxTreeCallbackReason reason, _declaration_part_.production_kind kind, _declaration_part_ p__declaration_part_)
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
                    if (step <= 0)
                        break;
                    p__declaration_part_.m__declarations_.appInfo = $"<span class=\"part-token\">{WebUtility.HtmlEncode (p__declaration_part_.m__declarations_.text)}</span>";
                    p__declaration_part_.m__identifier_.appInfo = $"<span class=\"part-name\" id=\"{normalizeName (p__declaration_part_.m__identifier_.text)}\">{WebUtility.HtmlEncode (p__declaration_part_.m__identifier_.text)}</span>";
                    p__declaration_part_.m__left_part_bracket_.appInfo = $"<span class=\"part-brace\">{WebUtility.HtmlEncode (p__declaration_part_.m__left_part_bracket_.text)}</span>";
                    p__declaration_part_.m__right_part_bracket_.appInfo = $"<span class=\"part-brace\">{WebUtility.HtmlEncode (p__declaration_part_.m__right_part_bracket_.text)}</span>";
                }
                break;
            }
            return true;
        }

        private bool AnglrHtmlColorizer__single_terminal_definition__Event (SyntaxTreeCallbackReason reason, _single_terminal_definition_.production_kind kind, _single_terminal_definition_ p__single_terminal_definition_)
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
                    if (step <= 0)
                        break;
                    p__single_terminal_definition_.m__terminal_.appInfo = $"<span class=\"reserved-word\">{WebUtility.HtmlEncode (p__single_terminal_definition_.m__terminal_.text)}</span>";
                }
                break;
            }
            return true;
        }

        private bool AnglrHtmlColorizer__single_regex_definition__Event (SyntaxTreeCallbackReason reason, _single_regex_definition_.production_kind kind, _single_regex_definition_ p__single_regex_definition_)
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
                    if (step <= 0)
                        break;
                    p__single_regex_definition_.m__regex_.appInfo = $"<span class=\"reserved-word\">{WebUtility.HtmlEncode (p__single_regex_definition_.m__regex_.text)}</span>";
                }
                break;
            }
            return true;
        }

        private bool AnglrHtmlColorizer__block_of_terminal_definitions__Event (SyntaxTreeCallbackReason reason, _block_of_terminal_definitions_.production_kind kind, _block_of_terminal_definitions_ p__block_of_terminal_definitions_)
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
                    if (step <= 0)
                        break;
                    p__block_of_terminal_definitions_.m__terminal_.appInfo = $"<span class=\"reserved-word\">{WebUtility.HtmlEncode (p__block_of_terminal_definitions_.m__terminal_.text)}</span>";
                    p__block_of_terminal_definitions_.m__left_curly_bracket_.appInfo = $"<span class=\"def-separator\">{WebUtility.HtmlEncode (p__block_of_terminal_definitions_.m__left_curly_bracket_.text)}</span>";
                    p__block_of_terminal_definitions_.m__right_curly_bracket_.appInfo = $"<span class=\"def-separator\">{WebUtility.HtmlEncode (p__block_of_terminal_definitions_.m__right_curly_bracket_.text)}</span>";
                }
                break;
            }
            return true;
        }

        private bool AnglrHtmlColorizer__block_of_regex_definitions__Event (SyntaxTreeCallbackReason reason, _block_of_regex_definitions_.production_kind kind, _block_of_regex_definitions_ p__block_of_regex_definitions_)
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
                    if (step <= 0)
                        break;
                    p__block_of_regex_definitions_.m__regex_.appInfo = $"<span class=\"reserved-word\">{WebUtility.HtmlEncode (p__block_of_regex_definitions_.m__regex_.text)}</span>";
                    p__block_of_regex_definitions_.m__left_curly_bracket_.appInfo = $"<span class=\"def-separator\">{WebUtility.HtmlEncode (p__block_of_regex_definitions_.m__left_curly_bracket_.text)}</span>";
                    p__block_of_regex_definitions_.m__right_curly_bracket_.appInfo = $"<span class=\"def-separator\">{WebUtility.HtmlEncode (p__block_of_regex_definitions_.m__right_curly_bracket_.text)}</span>";
                }
                break;
            }
            return true;
        }

        private bool AnglrHtmlColorizer__terminal_definition__Event (SyntaxTreeCallbackReason reason, _terminal_definition_.production_kind kind, _terminal_definition_ p__terminal_definition_)
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
                    if (step <= 0)
                        break;
                    p__terminal_definition_.m__identifier_.appInfo = $"<span class=\"token-name\" id=\"{normalizeName (p__terminal_definition_.m__identifier_.text)}\">{WebUtility.HtmlEncode (p__terminal_definition_.m__identifier_.text)}</span>";
                    _cstring_optional_ cstring_Optional_ = p__terminal_definition_.m__cstring_optional_;
                    switch ((_cstring_optional_.production_kind) cstring_Optional_.kind)
                    {
                        case _cstring_optional_.production_kind.g__cstring_optional__1:
                            break;
                        case _cstring_optional_.production_kind.g__cstring_optional__2:
                        {
                            cstring_Optional_.m__cstring_.appInfo = $"<span class=\"token-value\" id=\"{normalizeName (p__terminal_definition_.m__identifier_.text)}\">{WebUtility.HtmlEncode (cstring_Optional_.m__cstring_.text)}</span>";
                        }
                        break;
                    }
                }
                break;
            }
            return true;
        }

        private bool AnglrHtmlColorizer__regex_definition__Event (SyntaxTreeCallbackReason reason, _regex_definition_.production_kind kind, _regex_definition_ p__regex_definition_)
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
                    if (step <= 0)
                        break;
                    p__regex_definition_.m__identifier_.appInfo = $"<span class=\"regex-name\" id=\"{normalizeName (p__regex_definition_.m__identifier_.text)}\">{WebUtility.HtmlEncode (p__regex_definition_.m__identifier_.text)}</span>";
                    p__regex_definition_.m__regular_expression_.appInfo = $"<span class=\"regex-def\">{WebUtility.HtmlEncode (p__regex_definition_.m__regular_expression_.text)}</span>";
                }
                break;
            }
            return true;
        }

        private bool AnglrHtmlColorizer__scanner_part__Event (SyntaxTreeCallbackReason reason, _scanner_part_.production_kind kind, _scanner_part_ p__scanner_part_)
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
                    if (step <= 0)
                        break;
                    p__scanner_part_.m__scanner_.appInfo = $"<span class=\"part-token\">{WebUtility.HtmlEncode (p__scanner_part_.m__scanner_.text)}</span>";
                    p__scanner_part_.m__identifier_.appInfo = $"<span class=\"part-name\" id=\"{normalizeName (p__scanner_part_.m__identifier_.text)}\">{WebUtility.HtmlEncode (p__scanner_part_.m__identifier_.text)}</span>";
                    p__scanner_part_.m__left_part_bracket_.appInfo = $"<span class=\"part-brace\">{WebUtility.HtmlEncode (p__scanner_part_.m__left_part_bracket_.text)}</span>";
                    p__scanner_part_.m__right_part_bracket_.appInfo = $"<span class=\"part-brace\">{WebUtility.HtmlEncode (p__scanner_part_.m__right_part_bracket_.text)}</span>";
                }
                break;
            }
            return true;
        }

        private bool AnglrHtmlColorizer__regular_expression_usage__Event (SyntaxTreeCallbackReason reason, _regular_expression_usage_.production_kind kind, _regular_expression_usage_ p__regular_expression_usage_)
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
                    if (step <= 0)
                        break;
                    p__regular_expression_usage_.m__regular_expression_.appInfo = $"<span class=\"regex-ref\">{WebUtility.HtmlEncode (p__regular_expression_usage_.m__regular_expression_.text)}</span>";
                }
                break;
            }
            return true;
        }

        private bool AnglrHtmlColorizer__skip_action__Event (SyntaxTreeCallbackReason reason, _skip_action_.production_kind kind, _skip_action_ p__skip_action_)
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
                    if (step <= 0)
                        break;
                    p__skip_action_.m__skip_.appInfo = $"<span class=\"action-token\">{WebUtility.HtmlEncode (p__skip_action_.m__skip_.text)}</span>";
                }
                break;
            }
            return true;
        }

        private bool AnglrHtmlColorizer__terminal_action__Event (SyntaxTreeCallbackReason reason, _terminal_action_.production_kind kind, _terminal_action_ p__terminal_action_)
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
                    if (step <= 0)
                        break;
                    p__terminal_action_.m__ttoken_.appInfo = $"<span class=\"action-token\">{WebUtility.HtmlEncode (p__terminal_action_.m__ttoken_.text)}</span>";
                    p__terminal_action_.m__identifier_.appInfo = $"<a href=\"#{normalizeName (p__terminal_action_.m__identifier_.text)}\"><span class=\"action-identifier\">{WebUtility.HtmlEncode (p__terminal_action_.m__identifier_.text)}</span></a>";
                }
                break;
            }
            return true;
        }

        private bool AnglrHtmlColorizer__event_action__Event (SyntaxTreeCallbackReason reason, _event_action_.production_kind kind, _event_action_ p__event_action_)
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
                    if (step <= 0)
                        break;
                    p__event_action_.m__event_.appInfo = $"<span class=\"action-token\">{WebUtility.HtmlEncode (p__event_action_.m__event_.text)}</span>";
                    p__event_action_.m__identifier_.appInfo = $"<span class=\"action-identifier\" id=\"{normalizeName (p__event_action_.m__identifier_.text)}\">{WebUtility.HtmlEncode (p__event_action_.m__identifier_.text)}</span>";
                }
                break;
            }
            return true;
        }

        private bool AnglrHtmlColorizer__push_action__Event (SyntaxTreeCallbackReason reason, _push_action_.production_kind kind, _push_action_ p__push_action_)
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
                    if (step <= 0)
                        break;
                    p__push_action_.m__push_.appInfo = $"<span class=\"action-token\">{WebUtility.HtmlEncode (p__push_action_.m__push_.text)}</span>";
                    p__push_action_.m__identifier_.appInfo = $"<a href=\"#{normalizeName (p__push_action_.m__identifier_.text)}\"><span class=\"action-identifier\">{WebUtility.HtmlEncode (p__push_action_.m__identifier_.text)}</span></a>";
                }
                break;
            }
            return true;
        }

        private bool AnglrHtmlColorizer__pop_action__Event (SyntaxTreeCallbackReason reason, _pop_action_.production_kind kind, _pop_action_ p__pop_action_)
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
                    if (step <= 0)
                        break;
                    p__pop_action_.m__pop_.appInfo = $"<span class=\"action-token\">{WebUtility.HtmlEncode (p__pop_action_.m__pop_.text)}</span>";
                }
                break;
            }
            return true;
        }

        private bool AnglrHtmlColorizer__lexer_part__Event (SyntaxTreeCallbackReason reason, _lexer_part_.production_kind kind, _lexer_part_ p__lexer_part_)
        {
            switch (reason)
            {
                case SyntaxTreeCallbackReason.BuilderCallbackReason:
                    break;
                case SyntaxTreeCallbackReason.TraversalPrologueCallbackReason:
                {
                    if (step > 0)
                        break;
                    AnglrColorizerSymbol colorizerSymbol = new AnglrColorizerSymbol (p__lexer_part_.m__identifier_.text, null);
                    anglrColorizerSymbols.Add (colorizerSymbol);
                }
                break;
                case SyntaxTreeCallbackReason.TraversalMidTermCallbackReason:
                    break;
                case SyntaxTreeCallbackReason.TraversalEpilogueCallbackReason:
                {
                    if (step <= 0)
                        break;
                    p__lexer_part_.m__lexer_.appInfo = $"<span class=\"part-token\">{WebUtility.HtmlEncode (p__lexer_part_.m__lexer_.text)}</span>";
                    p__lexer_part_.m__identifier_.appInfo = $"<span class=\"part-name\" id=\"{normalizeName (p__lexer_part_.m__identifier_.text)}\">{WebUtility.HtmlEncode (p__lexer_part_.m__identifier_.text)}</span>";
                    p__lexer_part_.m__left_part_bracket_.appInfo = $"<span class=\"part-brace\">{WebUtility.HtmlEncode (p__lexer_part_.m__left_part_bracket_.text)}</span>";
                    p__lexer_part_.m__right_part_bracket_.appInfo = $"<span class=\"part-brace\">{WebUtility.HtmlEncode (p__lexer_part_.m__right_part_bracket_.text)}</span>";
                }
                break;
            }
            return true;
        }

        private bool AnglrHtmlColorizer__parser_part__Event (SyntaxTreeCallbackReason reason, _parser_part_.production_kind kind, _parser_part_ p__parser_part_)
        {
            switch (reason)
            {
                case SyntaxTreeCallbackReason.BuilderCallbackReason:
                    break;
                case SyntaxTreeCallbackReason.TraversalPrologueCallbackReason:
                {
                    if (step > 0)
                        break;
                    parserSymbol = new AnglrColorizerSymbol (p__parser_part_.m__identifier_.text, null);
                    anglrColorizerSymbols.Add (parserSymbol);
                }
                break;
                case SyntaxTreeCallbackReason.TraversalMidTermCallbackReason:
                    break;
                case SyntaxTreeCallbackReason.TraversalEpilogueCallbackReason:
                {
                    if (step <= 0)
                        break;
                    p__parser_part_.m__parser_.appInfo = $"<span class=\"part-token\">{WebUtility.HtmlEncode (p__parser_part_.m__parser_.text)}</span>";
                    p__parser_part_.m__identifier_.appInfo = $"<span class=\"part-name\" id=\"{normalizeName (p__parser_part_.m__identifier_.text)}\">{WebUtility.HtmlEncode (p__parser_part_.m__identifier_.text)}</span>";
                    p__parser_part_.m__left_part_bracket_.appInfo = $"<span class=\"part-brace\">{WebUtility.HtmlEncode (p__parser_part_.m__left_part_bracket_.text)}</span>";
                    p__parser_part_.m__right_part_bracket_.appInfo = $"<span class=\"part-brace\">{WebUtility.HtmlEncode (p__parser_part_.m__right_part_bracket_.text)}</span>";
                }
                break;
            }
            return true;
        }

        private bool AnglrHtmlColorizer__anglr_syntax_rule_list__Event (SyntaxTreeCallbackReason reason, _anglr_syntax_rule_list_.production_kind kind, _anglr_syntax_rule_list_ p__anglr_syntax_rule_list_)
        {
            switch (reason)
            {
                case SyntaxTreeCallbackReason.BuilderCallbackReason:
                    break;
                case SyntaxTreeCallbackReason.IterationPrologueCallbackReason:
                {
                    _ = p__anglr_syntax_rule_list_.Iterate (null, (node, appData) =>
                    {
                        _anglr_syntax_rule_ rule = node.m__anglr_syntax_rule_;
                        switch ((_anglr_syntax_rule_.production_kind) rule.kind)
                        {
                            case _anglr_syntax_rule_.production_kind.g__anglr_syntax_rule__1:
                            {
                                AnglrColorizerSymbol symbol = new AnglrColorizerSymbol (rule.m__identifier_.text, parserSymbol);
                                if (anglrColorizerSymbols.Contains (symbol))
                                    return null;
                                anglrColorizerSymbols.Add (symbol);
                            }
                            break;
                            case _anglr_syntax_rule_.production_kind.g__anglr_syntax_rule__2:
                            {
                                _anglr_syntax_rule_list_ q__anglr_syntax_rule_list_ = rule.m__anglr_syntax_rule_list_optional_.m__anglr_syntax_rule_list_;
                                if (q__anglr_syntax_rule_list_ != null)
                                    Traverse (q__anglr_syntax_rule_list_);
                            }
                            break;
                        }
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
            return true;
        }

        private bool AnglrHtmlColorizer__anglr_syntax_rule__Event (SyntaxTreeCallbackReason reason, _anglr_syntax_rule_.production_kind kind, _anglr_syntax_rule_ p__anglr_syntax_rule_)
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
                    if (step <= 0)
                        break;
                    switch (kind)
                    {
                        case _anglr_syntax_rule_.production_kind.g__anglr_syntax_rule__1:
                        {
                            p__anglr_syntax_rule_.m__identifier_.appInfo = $"<span class=\"rule-name\" id=\"{normalizeName (p__anglr_syntax_rule_.m__identifier_.text)}\">{WebUtility.HtmlEncode (p__anglr_syntax_rule_.m__identifier_.text)}</span>";
                            p__anglr_syntax_rule_.m__colon_.appInfo = $"<span class=\"def-separator\">{WebUtility.HtmlEncode (p__anglr_syntax_rule_.m__colon_.text)}</span>";
                            p__anglr_syntax_rule_.m__semicolon_.appInfo = $"<span class=\"def-separator\">{WebUtility.HtmlEncode (p__anglr_syntax_rule_.m__semicolon_.text)}</span>";
                        }
                        break;
                        case _anglr_syntax_rule_.production_kind.g__anglr_syntax_rule__2:
                        {
                            p__anglr_syntax_rule_.m__identifier_.appInfo = $"<span class=\"group-name\" id=\"{normalizeName (p__anglr_syntax_rule_.m__identifier_.text)}\">{WebUtility.HtmlEncode (p__anglr_syntax_rule_.m__identifier_.text)}</span>";
                            p__anglr_syntax_rule_.m__left_curly_bracket_.appInfo = $"<span class=\"def-separator\">{WebUtility.HtmlEncode (p__anglr_syntax_rule_.m__left_curly_bracket_.text)}</span>";
                            p__anglr_syntax_rule_.m__right_curly_bracket_.appInfo = $"<span class=\"def-separator\">{WebUtility.HtmlEncode (p__anglr_syntax_rule_.m__right_curly_bracket_.text)}</span>";
                        }
                        break;
                    }
                }
                break;
            }
            return true;
        }

        private bool AnglrHtmlColorizer__anglr_syntax_production_list_name__Event (SyntaxTreeCallbackReason reason, _anglr_syntax_production_list_name_.production_kind kind, _anglr_syntax_production_list_name_ p__anglr_syntax_production_list_name_)
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
                    AnglrColorizerSymbol symbol = new AnglrColorizerSymbol (p__anglr_syntax_production_list_name_.m__identifier_.text, parserSymbol);
                    if (!anglrColorizerSymbols.Contains (symbol))
                        anglrColorizerSymbols.Add (symbol);
                    if (step <= 0)
                        break;
                    p__anglr_syntax_production_list_name_.m__colon_.appInfo = $"<span class=\"def-separator\">{WebUtility.HtmlEncode (p__anglr_syntax_production_list_name_.m__colon_.text)}</span>";
                    p__anglr_syntax_production_list_name_.m__identifier_.appInfo = $"<span class=\"rule-name\" id=\"{normalizeName (p__anglr_syntax_production_list_name_.m__identifier_.text)}\">{WebUtility.HtmlEncode (p__anglr_syntax_production_list_name_.m__identifier_.text)}</span>";
                    p__anglr_syntax_production_list_name_.m__colon__1.appInfo = $"<span class=\"def-separator\">{WebUtility.HtmlEncode (p__anglr_syntax_production_list_name_.m__colon__1.text)}</span>";
                }
                break;
            }
            return true;
        }

        private bool AnglrHtmlColorizer__anglr_syntax_production_list__Event (SyntaxTreeCallbackReason reason, _anglr_syntax_production_list_.production_kind kind, _anglr_syntax_production_list_ p__anglr_syntax_production_list_)
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
                    if (step <= 0)
                        break;
                    switch (kind)
                    {
                        case _anglr_syntax_production_list_.production_kind.g__anglr_syntax_production_list__1:
                            break;
                        case _anglr_syntax_production_list_.production_kind.g__anglr_syntax_production_list__2:
                        {
                            p__anglr_syntax_production_list_.m__vertical_bar_.appInfo = $"<span class=\"def-separator\">{WebUtility.HtmlEncode (p__anglr_syntax_production_list_.m__vertical_bar_.text)}</span>";
                        }
                        break;
                    }
                }
                break;
            }
            return true;
        }

        private bool AnglrHtmlColorizer__anglr_syntax_production__Event (SyntaxTreeCallbackReason reason, _anglr_syntax_production_.production_kind kind, _anglr_syntax_production_ p__anglr_syntax_production_)
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
                    if (step <= 0)
                        break;
                    switch (kind)
                    {
                        case _anglr_syntax_production_.production_kind.g__anglr_syntax_production__1:
                            break;
                        case _anglr_syntax_production_.production_kind.g__anglr_syntax_production__2:
                        {
                            p__anglr_syntax_production_.m__empty_.appInfo = $"<span class=\"reserved-word\">{WebUtility.HtmlEncode (p__anglr_syntax_production_.m__empty_.text)}</span>";
                        }
                        break;
                    }
                }
                break;
            }
            return true;
        }

        private bool AnglrHtmlColorizer__priority_specification__Event (SyntaxTreeCallbackReason reason, _priority_specification_.production_kind kind, _priority_specification_ p__priority_specification_)
        {
            if ((reason != SyntaxTreeCallbackReason.TraversalEpilogueCallbackReason) || (step <= 0))
                return true;
            p__priority_specification_.m__priority_.appInfo = $"<span class=\"reserved-word\">{WebUtility.HtmlEncode (p__priority_specification_.m__priority_.text)}</span>";
            p__priority_specification_.m__number_.appInfo = $"<span class=\"reserved-word\">{WebUtility.HtmlEncode (p__priority_specification_.m__number_.text)}</span>";
            return true;
        }

        private bool AnglrHtmlColorizer__associativity_specification__Event (SyntaxTreeCallbackReason reason, _associativity_specification_.production_kind kind, _associativity_specification_ p__associativity_specification_)
        {
            if ((reason != SyntaxTreeCallbackReason.TraversalEpilogueCallbackReason) || (step <= 0))
                return true;
            p__associativity_specification_.m__associativity_.appInfo= $"<span class=\"reserved-word\">{WebUtility.HtmlEncode (p__associativity_specification_.m__associativity_.text)}</span>";
            switch (kind)
            {
                case _associativity_specification_.production_kind.g__associativity_specification__1:
                case _associativity_specification_.production_kind.g__associativity_specification__2:
                    p__associativity_specification_.m__cstring_.appInfo = $"<span class=\"string-name\">{WebUtility.HtmlEncode (p__associativity_specification_.m__cstring_.text)}</span>";
                    break;
                case _associativity_specification_.production_kind.g__associativity_specification__3:
                case _associativity_specification_.production_kind.g__associativity_specification__4:
                    p__associativity_specification_.m__identifier_.appInfo = $"<span class=\"identifier-name\">{WebUtility.HtmlEncode (p__associativity_specification_.m__identifier_.text)}</span>";
                    break;
            }
            return true;
        }

        private bool AnglrHtmlColorizer__production_name__Event (SyntaxTreeCallbackReason reason, _production_name_.production_kind kind, _production_name_ p__production_name_)
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
                    if (step <= 0)
                        break;
                    p__production_name_.m__double_at_sign_.appInfo = $"<span class=\"production-name\">{WebUtility.HtmlEncode (p__production_name_.m__double_at_sign_.text)}</span>";
                    p__production_name_.m__identifier_.appInfo = $"<span class=\"production-name\" id=\"{normalizeName (p__production_name_.m__identifier_.text)}\">{WebUtility.HtmlEncode (p__production_name_.m__identifier_.text)}</span>";
                }
                break;
            }
            return true;
        }

        private bool AnglrHtmlColorizer__g_name__Event (SyntaxTreeCallbackReason reason, _g_name_.production_kind kind, _g_name_ p__g_name_)
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
                    if (step <= 0)
                        break;
                    switch (kind)
                    {
                        case _g_name_.production_kind.g__g_name__1:
                            break;
                        case _g_name_.production_kind.g__g_name__2:
                        {
                            p__g_name_.m__left_bracket_.appInfo = $"<span class=\"def-separator\">{WebUtility.HtmlEncode (p__g_name_.m__left_bracket_.text)}</span>";
                            p__g_name_.m__right_bracket_.appInfo = $"<span class=\"def-separator\">{WebUtility.HtmlEncode (p__g_name_.m__right_bracket_.text)}</span>";
                        }
                        break;
                        case _g_name_.production_kind.g__g_name__3:
                            break;
                    }
                }
                break;
            }
            return true;
        }

        private bool AnglrHtmlColorizer__marker__Event (SyntaxTreeCallbackReason reason, _marker_.production_kind kind, _marker_ p__marker_)
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
                    if (step <= 0)
                        break;
                    p__marker_.m__at_sign_.appInfo = $"<span class=\"marker-name\">{WebUtility.HtmlEncode (p__marker_.m__at_sign_.text)}</span>";
                    p__marker_.m__identifier_.appInfo = $"<span class=\"marker-name\" id=\"{normalizeName (p__marker_.m__identifier_.text)}\">{WebUtility.HtmlEncode (p__marker_.m__identifier_.text)}</span>";
                }
                break;
            }
            return true;
        }

        private bool AnglrHtmlColorizer__name__Event (SyntaxTreeCallbackReason reason, _name_.production_kind kind, _name_ p__name_)
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
                    if (step <= 0)
                        break;
                    switch (kind)
                    {
                        case _name_.production_kind.g__name__1:
                        {
                            p__name_.m__any_.appInfo = $"<span class=\"any-name\">{WebUtility.HtmlEncode (p__name_.m__any_.text)}</span>";
                        }
                        break;
                        case _name_.production_kind.g__name__2:
                        {
                            p__name_.m__cstring_.appInfo = $"<span class=\"string-name\">{WebUtility.HtmlEncode (p__name_.m__cstring_.text)}</span>";
                        }
                        break;
                        case _name_.production_kind.g__name__3:
                        {
                            AnglrColorizerSymbol symbol = new AnglrColorizerSymbol (p__name_.m__identifier_.text, parserSymbol);
                            if (anglrColorizerSymbols.Contains (symbol))
                                p__name_.m__identifier_.appInfo = $"<a href=\"#{normalizeName(p__name_.m__identifier_.text)}\"><span class=\"rule-ref\">{WebUtility.HtmlEncode (p__name_.m__identifier_.text)}</span></a>";
                            else
                                p__name_.m__identifier_.appInfo = $"<a href=\"#{normalizeName (p__name_.m__identifier_.text)}\"><span class=\"identifier-name\">{WebUtility.HtmlEncode (p__name_.m__identifier_.text)}</span></a>";
                        }
                        break;
                    }
                }
                break;
            }
            return true;
        }

        private bool AnglrHtmlColorizer__cardinality__Event (SyntaxTreeCallbackReason reason, _cardinality_.production_kind kind, _cardinality_ p__cardinality_)
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
                    if (step <= 0)
                        break;
                    switch (kind)
                    {
                        case _cardinality_.production_kind.g__cardinality__1:
                        {
                            p__cardinality_.m__question_mark_.appInfo = $"<span class=\"reserved-word\">{WebUtility.HtmlEncode (p__cardinality_.m__question_mark_.text)}</span>";
                        }
                        break;
                        case _cardinality_.production_kind.g__cardinality__2:
                        {
                            p__cardinality_.m__plus_sign_.appInfo = $"<span class=\"reserved-word\">{WebUtility.HtmlEncode (p__cardinality_.m__plus_sign_.text)}</span>";
                        }
                        break;
                        case _cardinality_.production_kind.g__cardinality__3:
                        {
                            p__cardinality_.m__minus_sign_.appInfo = $"<span class=\"reserved-word\">{WebUtility.HtmlEncode (p__cardinality_.m__minus_sign_.text)}</span>";
                        }
                        break;
                        case _cardinality_.production_kind.g__cardinality__4:
                        {
                            p__cardinality_.m__asterisk_.appInfo = $"<span class=\"reserved-word\">{WebUtility.HtmlEncode (p__cardinality_.m__asterisk_.text)}</span>";
                        }
                        break;
                        case _cardinality_.production_kind.g__cardinality__5:
                        {
                            p__cardinality_.m__slash_.appInfo = $"<span class=\"reserved-word\">{WebUtility.HtmlEncode (p__cardinality_.m__slash_.text)}</span>";
                        }
                        break;
                        case _cardinality_.production_kind.g__cardinality__6:
                        {
                            p__cardinality_.m__inv_plus_sign_.appInfo = $"<span class=\"reserved-word\">{WebUtility.HtmlEncode (p__cardinality_.m__inv_plus_sign_.text)}</span>";
                        }
                        break;
                        case _cardinality_.production_kind.g__cardinality__7:
                        {
                            p__cardinality_.m__inv_minus_sign_.appInfo = $"<span class=\"reserved-word\">{WebUtility.HtmlEncode (p__cardinality_.m__inv_minus_sign_.text)}</span>";
                        }
                        break;
                        case _cardinality_.production_kind.g__cardinality__8:
                        {
                            p__cardinality_.m__inv_asterisk_.appInfo = $"<span class=\"reserved-word\">{WebUtility.HtmlEncode (p__cardinality_.m__inv_asterisk_.text)}</span>";
                        }
                        break;
                        case _cardinality_.production_kind.g__cardinality__9:
                        {
                            p__cardinality_.m__inv_slash_.appInfo = $"<span class=\"reserved-word\">{WebUtility.HtmlEncode (p__cardinality_.m__inv_slash_.text)}</span>";
                        }
                        break;
                        case _cardinality_.production_kind.g__cardinality__10:
                        {
                            p__cardinality_.m__left_curly_bracket_.appInfo = $"<span class=\"reserved-word\">{WebUtility.HtmlEncode (p__cardinality_.m__left_curly_bracket_.text)}</span>";
                            {
                                _number_optional_ _Number_ = p__cardinality_.m__number_optional_;
                                switch ((_number_optional_.production_kind) _Number_.kind)
                                {
                                    case _number_optional_.production_kind.g__number_optional__1:
                                        break;
                                    case _number_optional_.production_kind.g__number_optional__2:
                                    {
                                        p__cardinality_.m__number_optional_.m__number_.appInfo = $"<span class=\"identifier-name\">{WebUtility.HtmlEncode (p__cardinality_.m__number_optional_.m__number_.text)}</span>";
                                    }
                                    break;
                                }
                            }
                            p__cardinality_.m__comma_.appInfo = $"<span class=\"def-separator\">{WebUtility.HtmlEncode (p__cardinality_.m__comma_.text)}</span>";
                            {
                                _number_optional_ _Number_ = p__cardinality_.m__number_optional__1;
                                switch ((_number_optional_.production_kind) _Number_.kind)
                                {
                                    case _number_optional_.production_kind.g__number_optional__1:
                                        break;
                                    case _number_optional_.production_kind.g__number_optional__2:
                                    {
                                        p__cardinality_.m__number_optional__1.m__number_.appInfo = $"<span class=\"identifier-name\">{WebUtility.HtmlEncode (p__cardinality_.m__number_optional__1.m__number_.text)}</span>";
                                    }
                                    break;
                                }
                            }
                            p__cardinality_.m__right_curly_bracket_.appInfo = $"<span class=\"def-separator\">{WebUtility.HtmlEncode (p__cardinality_.m__right_curly_bracket_.text)}</span>";
                        }
                        break;
                    }
                }
                break;
            }
            return true;
        }

        private bool AnglrHtmlColorizer__delimiter__Event (SyntaxTreeCallbackReason reason, _delimiter_.production_kind kind, _delimiter_ p__delimiter_)
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
                    if (step <= 0)
                        break;
                    p__delimiter_.m__left_square_bracket_.appInfo = $"<span class=\"def-separator\">{WebUtility.HtmlEncode (p__delimiter_.m__left_square_bracket_.text)}</span>";
                    p__delimiter_.m__right_square_bracket_.appInfo = $"<span class=\"def-separator\">{WebUtility.HtmlEncode (p__delimiter_.m__right_square_bracket_.text)}</span>";
                }
                break;
            }
            return true;
        }
    }
}
