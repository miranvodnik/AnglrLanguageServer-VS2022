using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Anglr.Parser;
using Anglr.Parser.Core;
using Anglr.Parser.SyntaxTree;

namespace AnglrLibrary
{
    public class AnglrProdToTextEmiter : SyntaxTreeWalker
    {
        private bool hoverProperty = true;
        private bool inGeneralPart = false;
        private int inNestedRule = 0;
        private Stack<string> emitStack = new Stack<string> ();
        private SyntaxTreeBase rootNode = null;

        public int Count
        {
            get
            {
                try
                {
                    return emitStack.Count;
                }
                catch
                {
                    return -1;
                }
            }
        }

        public string EmitedText
        {
            get
            {
                try
                {
                    return emitStack.Peek ();
                }
                catch
                {
                    return "";
                }
            }
        }

        public string [] EmitedArray
        {
            get
            {
                try
                {
                    return emitStack.ToArray ();
                }
                catch
                {
                    return null;
                }
            }
        }

        public AnglrProdToTextEmiter ()
        {
            //  anglr file fragments
            _anglr_file_fragment__Event += Invoke__anglr_file_fragment__Callback;

            //  anglr file attributes
            _attribute_list_optional__Event += Invoke__attribute_list_optional__Callback;
            _attribute_list__Event += Invoke__attribute_list__Callback;
            _attribute__Event += Invoke__attribute__Callback;
            _name_value_list_optional__Event += Invoke__name_value_list_optional__Callback;
            _name_value_list__Event += Invoke__name_value_list__Callback;
            _name_value_pair__Event += Invoke__name_value_pair__Callback;

            //  anglr file parts
            _anglr_file__Event += Invoke__anglr_file__Callback;
            _anglr_file_part_list__Event += Invoke__anglr_file_part_list__Callback;

            //  general part of anglr file
            _general_part__Event += Invoke__general_part__Callback;

            //  declaration part of anglr file
            _declaration_part__Event += Invoke__declaration_part__Callback;
            _anglr_definition_list_optional__Event += Invoke__anglr_definition_list_optional__Callback;
            _anglr_definition_list__Event += Invoke__anglr_definition_list__Callback;
            _anglr_definition_with_attribute__Event += Invoke__anglr_definition_with_attribute__Callback;
            _single_terminal_definition__Event += Invoke__single_terminal_definition__Callback;
            _single_regex_definition__Event += Invoke__single_regex_definition__Callback;
            _block_of_terminal_definitions__Event += Invoke__block_of_terminal_definitions__Callback;
            _block_of_regex_definitions__Event += Invoke__block_of_regex_definitions__Callback;
            _terminal_definition__Event += Invoke__terminal_definition__Callback;
            _cstring_optional__Event += Invoke__cstring_optional__Callback;
            _regex_definition__Event += Invoke__regex_definition__Callback;
            _block_terminal_definitions_optional__Event += Invoke__block_terminal_definitions_optional__Callback;
            _block_terminal_definitions__Event += Invoke__block_terminal_definitions__Callbacl;
            _block_terminal_definition__Event += Invoke__block_terminal_definition__Callback;
            _block_regex_definitions_optional__Event += Invoke__block_regex_definitions_optional__Callback;
            _block_regex_definitions__Event += Invoke__block_regex_definitions__Callback;
            _block_regex_definition__Event += Invoke__block_regex_definition__Callback;

            //  scanner part of anglr file
            _scanner_part__Event += Invoke__scanner_part__Callback;
            _regular_expression_list_optional__Event += Invoke__regular_expression_list_optional__Callback;
            _regular_expression_list__Event += Invoke__regular_expression_list__Callback;
            _regular_expression_usage__Event += Invoke__regular_expression_usage__Callback;
            _actions_optional__Event += Invoke__actions_optional__Callback;
            _actions__Event += Invoke__actions__Callback;
            _skip_action__Event += Invoke__skip_action__Callback;
            _terminal_action__Event += Invoke__terminal_action__Callback;
            _event_action__Event += Invoke__event_action__Callback;
            _push_action__Event += Invoke__push_action__Callback;
            _pop_action__Event += Invoke__pop_action__Callback;

            //  lexer part of anglr file
            _lexer_part__Event += Invoke__lexer_part__Callback;

            //  parser part of anglr file
            _parser_part__Event += Invoke__parser_part__Callback;
            _anglr_syntax_rule_list_optional__Event += Invoke__anglr_syntax_rule_list_optional__Callback;
            _anglr_syntax_rule_list__Event += Invoke__anglr_syntax_rule_list__Callback;
            _anglr_syntax_rule__Event += Invoke__anglr_syntax_rule__Callback;
            _anglr_nested_rule__Event += Invoke__anglr_nested_rule__Callback;
            _anglr_syntax_production_list_name_optional__Event += Invoke__anglr_syntax_production_list_name_optional__Callback;
            _anglr_syntax_production_list_name__Event += Invoke__anglr_syntax_production_list_name__Callback;
            _anglr_syntax_production_list__Event += Invoke__anglr_syntax_production_list__Callback;
            _anglr_syntax_production__Event += Invoke__anglr_syntax_production__Callback;
            _priority_assoc_specification_optional__Event += Invoke__priority_assoc_specification_optional__Callback;
            _priority_assoc_specification__Event += Invoke__priority_assoc_specification__Callback;
            _priority_specification__Event += Invoke__priority_specification__Callback;
            _associativity_specification__Event += Invoke__associativity_specification__Callback;
            _production_name_optional__Event += Invoke__production_name_optional__Callback;
            _production_name__Event += Invoke__production_name__Callback;
            _name_list__Event += Invoke__name_list__Callback;
            _marker_list_optional__Event += Invoke__marker_list_optional__Callback;
            _marker_list__Event += Invoke__marker_list__Callback;
            _marker__Event += Invoke__marker__Callback;
            _g_name__Event += Invoke__g_name__Callback;
            _name__Event += Invoke__name__Callback;
            _cardinality_delimiter__Event += Invoke__cardinality_delimiter__Callback;
            _cardinality__Event += Invoke__cardinality__Callback;
            _number_optional__Event += Invoke__number_optional__Callback;
            _delimiter_optional__Event += Invoke__delimiter_optional__Callback;
            _delimiter__Event += Invoke__delimiter__Callback;
        }

        public string concatenate (string [] strings)
        {
            string result = "";
            foreach (string s in strings)
            {
                if (s.Length > 0)
                    if (result.Length > 0)
                    result += ' ' + s;
                else result += s;
            }
            return result;
        }

        public string GetNodeString (SyntaxTreeBase node)
        {
            rootNode = node;
            emitStack.Clear ();
            node.InvokeTraverse (this);
            return EmitedText;
        }

        private bool Invoke__anglr_file_fragment__Callback (SyntaxTreeCallbackReason reason, _anglr_file_fragment_.production_kind kind, _anglr_file_fragment_ p__anglr_file_fragment_)
        {
            bool status = true;
            switch (reason)
            {
                case SyntaxTreeCallbackReason.BuilderCallbackReason:
                    break;
                case SyntaxTreeCallbackReason.TraversalPrologueCallbackReason:
                    inGeneralPart = false;
                    inNestedRule = 0;
                    break;
                case SyntaxTreeCallbackReason.TraversalMidTermCallbackReason:
                    break;
                case SyntaxTreeCallbackReason.TraversalEpilogueCallbackReason:
                    break;
            }
            return status;
        }

        private bool Invoke__attribute_list_optional__Callback (SyntaxTreeCallbackReason reason, _attribute_list_optional_.production_kind kind, _attribute_list_optional_ p__attribute_list_optional_)
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
                    if (kind != _attribute_list_optional_.production_kind.g__attribute_list_optional__2)
                        emitStack.Push ("");
                }
                break;
            }
            return status;
        }

        private bool Invoke__attribute_list__Callback (SyntaxTreeCallbackReason reason, _attribute_list_.production_kind kind, _attribute_list_ p__attribute_list_)
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
                    if (kind != _attribute_list_.production_kind.g__attribute_list__2)
                        break;
                    string s__attribute_ = emitStack.Pop ();
                    string s__attribute_list_ = emitStack.Pop ();
                    emitStack.Push ($"{s__attribute_list_}\n{s__attribute_}");
                }
                break;
            }
            return status;
        }

        private bool Invoke__attribute__Callback (SyntaxTreeCallbackReason reason, _attribute_.production_kind kind, _attribute_ p__attribute_)
        {
            bool status = true;
            switch (reason)
            {
                case SyntaxTreeCallbackReason.BuilderCallbackReason:
                    break;
                case SyntaxTreeCallbackReason.TraversalPrologueCallbackReason:
                    hoverProperty = true;
                    break;
                case SyntaxTreeCallbackReason.TraversalMidTermCallbackReason:
                    break;
                case SyntaxTreeCallbackReason.TraversalEpilogueCallbackReason:
                {
                    string s__name_value_list_optional_ = emitStack.Pop ();
                    if (hoverProperty)
                    {
                        if (inGeneralPart)
                            emitStack.Push ($"\t{p__attribute_.m__left_square_bracket_.text} {p__attribute_.m__identifier_.text} {s__name_value_list_optional_} {p__attribute_.m__right_square_bracket_.text}");
                        else
                            emitStack.Push ($"{p__attribute_.m__left_square_bracket_.text} {p__attribute_.m__identifier_.text} {s__name_value_list_optional_} {p__attribute_.m__right_square_bracket_.text}");
                    }
                    else
                        emitStack.Push ("");
                }
                break;
            }
            return status;
        }

        private bool Invoke__name_value_list_optional__Callback (SyntaxTreeCallbackReason reason, _name_value_list_optional_.production_kind kind, _name_value_list_optional_ p__name_value_list_optional_)
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
                    if (kind != _name_value_list_optional_.production_kind.g__name_value_list_optional__2)
                        emitStack.Push ("");
                }
                break;
            }
            return status;
        }

        private bool Invoke__name_value_list__Callback (SyntaxTreeCallbackReason reason, _name_value_list_.production_kind kind, _name_value_list_ p__name_value_list_)
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
                    if (kind != _name_value_list_.production_kind.g__name_value_list__2)
                        break;
                    string s__name_value_pair_ = emitStack.Pop ();
                    string s__name_value_list_ = emitStack.Pop ();
                    emitStack.Push ($"{s__name_value_list_}\n\t{s__name_value_pair_}");
                }
                break;
            }
            return status;
        }

        private bool Invoke__name_value_pair__Callback (SyntaxTreeCallbackReason reason, _name_value_pair_.production_kind kind, _name_value_pair_ p__name_value_pair_)
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
                    emitStack.Push ($"{p__name_value_pair_.m__identifier_.text}{p__name_value_pair_.m__equals_sign_.text}{p__name_value_pair_.m__cstring_.text}");
                    if (string.Compare (p__name_value_pair_.m__identifier_.text, "Hover", true) != 0)
                        break;
                    string text = p__name_value_pair_.m__cstring_.text;
                    int length = text.Length;
                    if (string.Compare (text.Substring (1, length - 2), "false", true) != 0)
                        break;
                    hoverProperty = false;
                    break;
            }
            return status;
        }

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
                    if (kind != _anglr_file_.production_kind.g__anglr_file__2)
                        emitStack.Push ("");
                    break;
            }
            return status;
        }

        private bool Invoke__anglr_file_part_list__Callback (SyntaxTreeCallbackReason reason, _anglr_file_part_list_.production_kind kind, _anglr_file_part_list_ p__anglr_file_part_list_)
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
                    if (kind != _anglr_file_part_list_.production_kind.g__anglr_file_part_list__2)
                        break;
                    string s1 = emitStack.Pop ();
                    string s2 = emitStack.Pop ();
                    emitStack.Push ($"{s2}\n{s1}");
                }
                break;
            }
            return status;
        }

        private bool Invoke__general_part__Callback (SyntaxTreeCallbackReason reason, _general_part_.production_kind kind, _general_part_ p__general_part_)
        {
            bool status = true;
            switch (reason)
            {
                case SyntaxTreeCallbackReason.BuilderCallbackReason:
                    break;
                case SyntaxTreeCallbackReason.TraversalPrologueCallbackReason:
                    inGeneralPart = true;
                    break;
                case SyntaxTreeCallbackReason.TraversalMidTermCallbackReason:
                    break;
                case SyntaxTreeCallbackReason.TraversalEpilogueCallbackReason:
                {
                    string s_attribute_list_optional_inner = emitStack.Pop ();
                    string s_attribute_list_optional_outer = emitStack.Pop ();
                    if (s_attribute_list_optional_outer.Length > 0)
                    {
                        if (s_attribute_list_optional_inner.Length > 0)
                            emitStack.Push ($"{s_attribute_list_optional_outer}\n{p__general_part_.m__general_.text} {p__general_part_.m__identifier_.text}\n{p__general_part_.m__left_part_bracket_.text}\n{s_attribute_list_optional_inner}\n{p__general_part_.m__right_part_bracket_.text}\n");
                        else
                            emitStack.Push ($"{s_attribute_list_optional_outer}\n{p__general_part_.m__general_.text} {p__general_part_.m__identifier_.text}\n{p__general_part_.m__left_part_bracket_.text}\n{p__general_part_.m__right_part_bracket_.text}\n");
                    }
                    else
                    {
                        if (s_attribute_list_optional_inner.Length > 0)
                            emitStack.Push ($"{p__general_part_.m__general_.text} {p__general_part_.m__identifier_.text}\n{p__general_part_.m__left_part_bracket_.text}\n{s_attribute_list_optional_inner}\n{p__general_part_.m__right_part_bracket_.text}\n");
                        else
                            emitStack.Push ($"{p__general_part_.m__general_.text} {p__general_part_.m__identifier_.text}\n{p__general_part_.m__left_part_bracket_.text}\n{p__general_part_.m__right_part_bracket_.text}\n");
                    }
                    inGeneralPart = false;
                }
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
                    break;
                case SyntaxTreeCallbackReason.TraversalPrologueCallbackReason:
                    break;
                case SyntaxTreeCallbackReason.TraversalMidTermCallbackReason:
                    break;
                case SyntaxTreeCallbackReason.TraversalEpilogueCallbackReason:
                {
                    string s__anglr_definition_list_optional_ = emitStack.Pop ();
                    string s_attribute_list_optional_ = emitStack.Pop ();
                    if (s_attribute_list_optional_.Length > 0)
                        emitStack.Push ($"{s_attribute_list_optional_}\n{p__declaration_part_.m__declarations_.text} {p__declaration_part_.m__identifier_.text}\n{p__declaration_part_.m__left_part_bracket_.text}\n{s__anglr_definition_list_optional_}\n{p__declaration_part_.m__right_part_bracket_.text}\n");
                    else
                        emitStack.Push ($"{p__declaration_part_.m__declarations_.text} {p__declaration_part_.m__identifier_.text}\n{p__declaration_part_.m__left_part_bracket_.text}\n{s__anglr_definition_list_optional_}\n{p__declaration_part_.m__right_part_bracket_.text}\n");
                }
                break;
            }
            return status;
        }

        private bool Invoke__anglr_definition_list_optional__Callback (SyntaxTreeCallbackReason reason, _anglr_definition_list_optional_.production_kind kind, _anglr_definition_list_optional_ p__anglr_definition_list_optional_)
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
                    if (kind != _anglr_definition_list_optional_.production_kind.g__anglr_definition_list_optional__2)
                        emitStack.Push ("");
                }
                break;
            }
            return status;
        }

        private bool Invoke__anglr_definition_list__Callback (SyntaxTreeCallbackReason reason, _anglr_definition_list_.production_kind kind, _anglr_definition_list_ p__anglr_definition_list_)
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
                    if (kind != _anglr_definition_list_.production_kind.g__anglr_definition_list__2)
                        break;
                    string m__anglr_definition_ = emitStack.Pop ();
                    string m__anglr_definition_list_ = emitStack.Pop ();
                    emitStack.Push ($"{m__anglr_definition_list_}\n{m__anglr_definition_}");
                }
                break;
            }
            return status;
        }

        private bool Invoke__anglr_definition_with_attribute__Callback (SyntaxTreeCallbackReason reason, _anglr_definition_with_attribute_.production_kind kind, _anglr_definition_with_attribute_ p__anglr_definition_with_attribute_)
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
                    string m__anglr_definition_ = emitStack.Pop ();
                    string m__attribute_list_optional = emitStack.Pop ();
                    if (m__attribute_list_optional.Length > 0)
                        emitStack.Push ($"{m__attribute_list_optional}\n{m__anglr_definition_}");
                    else
                        emitStack.Push ($"{m__anglr_definition_}");
                }
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
                    break;
                case SyntaxTreeCallbackReason.TraversalMidTermCallbackReason:
                    break;
                case SyntaxTreeCallbackReason.TraversalEpilogueCallbackReason:
                {
                    string m__terminal_definition_ = emitStack.Pop ();
                    emitStack.Push ($"\t{p__single_terminal_definition_.m__terminal_.text} {m__terminal_definition_}");
                }
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
                    break;
                case SyntaxTreeCallbackReason.TraversalMidTermCallbackReason:
                    break;
                case SyntaxTreeCallbackReason.TraversalEpilogueCallbackReason:
                {
                    string m__regex_definition_ = emitStack.Pop ();
                    emitStack.Push ($"\t{p__single_regex_definition_.m__regex_.text} {m__regex_definition_}");
                }
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
                    break;
                case SyntaxTreeCallbackReason.TraversalMidTermCallbackReason:
                    break;
                case SyntaxTreeCallbackReason.TraversalEpilogueCallbackReason:
                {
                    string m__block_terminal_definition_ = emitStack.Pop ();
                    emitStack.Push ($"\t{p__block_of_terminal_definitions_.m__terminal_.text}\n\t{p__block_of_terminal_definitions_.m__left_curly_bracket_.text}\n\t\t{m__block_terminal_definition_}\n\t{p__block_of_terminal_definitions_.m__right_curly_bracket_.text}");
                }
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
                    break;
                case SyntaxTreeCallbackReason.TraversalMidTermCallbackReason:
                    break;
                case SyntaxTreeCallbackReason.TraversalEpilogueCallbackReason:
                {
                    string m__block_regex_definition_ = emitStack.Pop ();
                    emitStack.Push ($"\t{p__block_of_regex_definitions_.m__regex_.text}\n\t{p__block_of_regex_definitions_.m__left_curly_bracket_.text}\n\t\t{m__block_regex_definition_}\n\t{p__block_of_regex_definitions_.m__right_curly_bracket_.text}");
                }
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
                    break;
                case SyntaxTreeCallbackReason.TraversalPrologueCallbackReason:
                    break;
                case SyntaxTreeCallbackReason.TraversalMidTermCallbackReason:
                    break;
                case SyntaxTreeCallbackReason.TraversalEpilogueCallbackReason:
                {
                    string m__cstring_optional_ = emitStack.Pop ();
                    if (m__cstring_optional_.Length > 0)
                        emitStack.Push ($"{p__terminal_definition_.m__identifier_.text} {m__cstring_optional_}");
                    else
                        emitStack.Push ($"{p__terminal_definition_.m__identifier_.text}");
                }
                break;
            }
            return status;
        }

        private bool Invoke__cstring_optional__Callback (SyntaxTreeCallbackReason reason, _cstring_optional_.production_kind kind, _cstring_optional_ p__cstring_optional_)
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
                    if (kind != _cstring_optional_.production_kind.g__cstring_optional__2)
                        emitStack.Push ("");
                    else
                        emitStack.Push ($"{p__cstring_optional_.m__cstring_.text}");
                }
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
                    break;
                case SyntaxTreeCallbackReason.TraversalMidTermCallbackReason:
                    break;
                case SyntaxTreeCallbackReason.TraversalEpilogueCallbackReason:
                    emitStack.Push ($"{p__regex_definition_.m__identifier_.text} {p__regex_definition_.m__regular_expression_.text}");
                    break;
            }
            return status;
        }

        private bool Invoke__block_terminal_definitions_optional__Callback (SyntaxTreeCallbackReason reason, _block_terminal_definitions_optional_.production_kind kind, _block_terminal_definitions_optional_ p__block_terminal_definitions_optional_)
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
                    if (kind != _block_terminal_definitions_optional_.production_kind.g__block_terminal_definitions_optional__2)
                        emitStack.Push ("");
                }
                break;
            }
            return status;
        }

        private bool Invoke__block_terminal_definitions__Callbacl (SyntaxTreeCallbackReason reason, _block_terminal_definitions_.production_kind kind, _block_terminal_definitions_ p__block_terminal_definitions_)
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
                    if (kind != _block_terminal_definitions_.production_kind.g__block_terminal_definitions__2)
                        break;
                    string  m__block_terminal_definition_ = emitStack.Pop ();
                    string m__block_terminal_definitions_ = emitStack.Pop ();
                    emitStack.Push ($"{m__block_terminal_definitions_}\n\t\t{m__block_terminal_definition_}");
                }
                    break;
            }
            return status;
        }

        private bool Invoke__block_terminal_definition__Callback (SyntaxTreeCallbackReason reason, _block_terminal_definition_.production_kind kind, _block_terminal_definition_ p__block_terminal_definition_)
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
                    string m__terminal_definition_ = emitStack.Pop ();
                    string m__attribute_list_optional_ = emitStack.Pop ();
                    if (m__attribute_list_optional_.Length > 0)
                        emitStack.Push ($"\t{m__attribute_list_optional_}\n\t{m__terminal_definition_}");
                    else
                        emitStack.Push ($"\t{m__terminal_definition_}");
                }
                break;
            }
            return status;
        }

        private bool Invoke__block_regex_definitions_optional__Callback (SyntaxTreeCallbackReason reason, _block_regex_definitions_optional_.production_kind kind, _block_regex_definitions_optional_ p__block_regex_definitions_optional_)
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
                    if (kind != _block_regex_definitions_optional_.production_kind.g__block_regex_definitions_optional__2)
                        emitStack.Push ("");
                }
                    break;
            }
            return status;
        }

        private bool Invoke__block_regex_definitions__Callback (SyntaxTreeCallbackReason reason, _block_regex_definitions_.production_kind kind, _block_regex_definitions_ p__block_regex_definitions_)
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
                    if (kind != _block_regex_definitions_.production_kind.g__block_regex_definitions__2)
                        break;
                    string m__block_regex_definition_ = emitStack.Pop ();
                    string m__block_regex_definitions_ = emitStack.Pop ();
                    emitStack.Push ($"{m__block_regex_definitions_}\n\t\t{m__block_regex_definition_}");
                }
                    break;
            }
            return status;
        }

        private bool Invoke__block_regex_definition__Callback (SyntaxTreeCallbackReason reason, _block_regex_definition_.production_kind kind, _block_regex_definition_ p__block_regex_definition_)
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
                    string m__regex_definition_ = emitStack.Pop ();
                    string m__attribute_list_optional_ = emitStack.Pop ();
                    if (m__attribute_list_optional_.Length > 0)
                        emitStack.Push ($"{m__attribute_list_optional_}\n{m__regex_definition_}");
                    else
                        emitStack.Push ($"{m__regex_definition_}");
                }
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
                    break;
                case SyntaxTreeCallbackReason.TraversalPrologueCallbackReason:
                    break;
                case SyntaxTreeCallbackReason.TraversalMidTermCallbackReason:
                    break;
                case SyntaxTreeCallbackReason.TraversalEpilogueCallbackReason:
                {
                    string s__regular_expression_list_optional_ = emitStack.Pop ();
                    string s__attribute_list_optional_ = emitStack.Pop ();
                    if (s__attribute_list_optional_.Length > 0)
                        emitStack.Push ($"{s__attribute_list_optional_}\n{p__scanner_part_.m__scanner_.text} {p__scanner_part_.m__identifier_.text}\n{p__scanner_part_.m__left_part_bracket_.text}\n{s__regular_expression_list_optional_}\n{p__scanner_part_.m__right_part_bracket_.text}\n");
                    else
                        emitStack.Push ($"{p__scanner_part_.m__scanner_.text} {p__scanner_part_.m__identifier_.text}\n{p__scanner_part_.m__left_part_bracket_.text}\n{s__regular_expression_list_optional_}\n{p__scanner_part_.m__right_part_bracket_.text}\n");
                }
                break;
            }
            return status;
        }

        private bool Invoke__regular_expression_list_optional__Callback (SyntaxTreeCallbackReason reason, _regular_expression_list_optional_.production_kind kind, _regular_expression_list_optional_ p__regular_expression_list_optional_)
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
                    if (kind != _regular_expression_list_optional_.production_kind.g__regular_expression_list_optional__2)
                        emitStack.Push ("");
                }
                break;
            }
            return status;
        }

        private bool Invoke__regular_expression_list__Callback (SyntaxTreeCallbackReason reason, _regular_expression_list_.production_kind kind, _regular_expression_list_ p__regular_expression_list_)
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
                    if (kind != _regular_expression_list_.production_kind.g__regular_expression_list__2)
                        break;
                    string s__regular_expression_usage_ = emitStack.Pop ();
                    string s__regular_expression_list_ = emitStack.Pop ();
                    emitStack.Push ($"{s__regular_expression_list_}\n{s__regular_expression_usage_}");
                }
                break;
            }
            return status;
        }

        private bool Invoke__regular_expression_usage__Callback (SyntaxTreeCallbackReason reason, _regular_expression_usage_.production_kind kind, _regular_expression_usage_ p__regular_expression_usage_)
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
                    string m__actions_optional_ = emitStack.Pop ();
                    if (m__actions_optional_.Length > 0)
                        emitStack.Push ($"{p__regular_expression_usage_.m__regular_expression_.text}\n\t{m__actions_optional_}");
                    else
                        emitStack.Push ($"{p__regular_expression_usage_.m__regular_expression_.text}");
                }
                break;
            }
            return status;
        }

        private bool Invoke__actions_optional__Callback (SyntaxTreeCallbackReason reason, _actions_optional_.production_kind kind, _actions_optional_ p__actions_optional_)
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
                    if (kind != _actions_optional_.production_kind.g__actions_optional__2)
                        emitStack.Push ("");
                }
                    break;
            }
            return status;
        }

        private bool Invoke__actions__Callback (SyntaxTreeCallbackReason reason, _actions_.production_kind kind, _actions_ p__actions_)
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
                    if (kind != _actions_.production_kind.g__actions__2)
                        break;
                    string m__action_ = emitStack.Pop ();
                    string m__actions_ = emitStack.Pop ();
                    emitStack.Push ($"{m__actions_}\n\t{m__action_}");
                }
                    break;
            }
            return status;
        }

        private bool Invoke__skip_action__Callback (SyntaxTreeCallbackReason reason, _skip_action_.production_kind kind, _skip_action_ p__skip_action_)
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
                    emitStack.Push ($"\t{p__skip_action_.m__skip_.text}");
                }
                    break;
            }
            return status;
        }

        private bool Invoke__terminal_action__Callback (SyntaxTreeCallbackReason reason, _terminal_action_.production_kind kind, _terminal_action_ p__terminal_action_)
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
                    emitStack.Push ($"\t{p__terminal_action_.m__ttoken_.text} {p__terminal_action_.m__identifier_.text}");
                }
                    break;
            }
            return status;
        }

        private bool Invoke__event_action__Callback (SyntaxTreeCallbackReason reason, _event_action_.production_kind kind, _event_action_ p__event_action_)
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
                    emitStack.Push ($"\t{p__event_action_.m__event_.text} {p__event_action_.m__identifier_.text}");
                }
                    break;
            }
            return status;
        }

        private bool Invoke__push_action__Callback (SyntaxTreeCallbackReason reason, _push_action_.production_kind kind, _push_action_ p__push_action_)
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
                    emitStack.Push ($"\t{p__push_action_.m__push_.text} {p__push_action_.m__identifier_.text}");
                }
                    break;
            }
            return status;
        }

        private bool Invoke__pop_action__Callback (SyntaxTreeCallbackReason reason, _pop_action_.production_kind kind, _pop_action_ p__pop_action_)
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
                    emitStack.Push ($"\t{p__pop_action_.m__pop_.text}");
                }
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
                    break;
                case SyntaxTreeCallbackReason.TraversalPrologueCallbackReason:
                    break;
                case SyntaxTreeCallbackReason.TraversalMidTermCallbackReason:
                    break;
                case SyntaxTreeCallbackReason.TraversalEpilogueCallbackReason:
                {
                    string s_inner_attribute_list_optional_ = emitStack.Pop ();
                    string s__attribute_list_optional_ = emitStack.Pop ();
                    if (s__attribute_list_optional_.Length > 0)
                        emitStack.Push ($"{s__attribute_list_optional_}\n{p__lexer_part_.m__lexer_.text} {p__lexer_part_.m__identifier_.text}\n{p__lexer_part_.m__left_part_bracket_.text}\n{s_inner_attribute_list_optional_}\n{p__lexer_part_.m__right_part_bracket_.text}\n");
                    else
                        emitStack.Push ($"{p__lexer_part_.m__lexer_.text} {p__lexer_part_.m__identifier_.text}\n{p__lexer_part_.m__left_part_bracket_.text}\n{s_inner_attribute_list_optional_}\n{p__lexer_part_.m__right_part_bracket_.text}\n");
                }
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
                    break;
                case SyntaxTreeCallbackReason.TraversalPrologueCallbackReason:
                    break;
                case SyntaxTreeCallbackReason.TraversalMidTermCallbackReason:
                    break;
                case SyntaxTreeCallbackReason.TraversalEpilogueCallbackReason:
                {
                    string s__anglr_syntax_rule_list_optional_ = emitStack.Pop ();
                    string s__attribute_list_optional_ = emitStack.Pop ();
                    if (s__attribute_list_optional_.Length > 0)
                        emitStack.Push ($"{s__attribute_list_optional_}\n{p__parser_part_.m__parser_.text} {p__parser_part_.m__identifier_.text}\n{p__parser_part_.m__left_part_bracket_.text}\n{s__anglr_syntax_rule_list_optional_}\n{p__parser_part_.m__right_part_bracket_.text}\n");
                    else
                        emitStack.Push ($"{p__parser_part_.m__parser_.text} {p__parser_part_.m__identifier_.text}\n{p__parser_part_.m__left_part_bracket_.text}\n{s__anglr_syntax_rule_list_optional_}\n{p__parser_part_.m__right_part_bracket_.text}\n");
                }
                break;
            }
            return status;
        }

        private bool Invoke__anglr_syntax_rule_list_optional__Callback (SyntaxTreeCallbackReason reason, _anglr_syntax_rule_list_optional_.production_kind kind, _anglr_syntax_rule_list_optional_ p__anglr_syntax_rule_list_optional_)
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
                    if (kind != _anglr_syntax_rule_list_optional_.production_kind.g__anglr_syntax_rule_list_optional__2)
                        emitStack.Push ("");
                }
                break;
            }
            return status;
        }

        private bool Invoke__anglr_syntax_rule_list__Callback (SyntaxTreeCallbackReason reason, _anglr_syntax_rule_list_.production_kind kind, _anglr_syntax_rule_list_ p__anglr_syntax_rule_list_)
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
                    if (kind != _anglr_syntax_rule_list_.production_kind.g__anglr_syntax_rule_list__2)
                        break;
                    string m__anglr_syntax_rule_ = emitStack.Pop ();
                    string m__anglr_syntax_rule_list_ = emitStack.Pop ();
                    emitStack.Push ($"{m__anglr_syntax_rule_list_}\n\n{m__anglr_syntax_rule_}");
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
                    break;
                case SyntaxTreeCallbackReason.TraversalPrologueCallbackReason:
                    ++inNestedRule;
                    break;
                case SyntaxTreeCallbackReason.TraversalMidTermCallbackReason:
                    break;
                case SyntaxTreeCallbackReason.TraversalEpilogueCallbackReason:
                {
                    switch (kind)
                    {
                        case _anglr_syntax_rule_.production_kind.g__anglr_syntax_rule__1:
                        {
                            string m__anglr_syntax_production_list_ = emitStack.Pop ();
                            string m__attribute_list_optional_ = emitStack.Pop ();
                            if (m__attribute_list_optional_.Length > 0)
                                emitStack.Push ($"{m__attribute_list_optional_}\n{p__anglr_syntax_rule_.m__identifier_.text}\n\t{p__anglr_syntax_rule_.m__colon_.text}\t{m__anglr_syntax_production_list_}\n\t{p__anglr_syntax_rule_.m__semicolon_.text}");
                            else
                                emitStack.Push ($"{p__anglr_syntax_rule_.m__identifier_.text}\n\t{p__anglr_syntax_rule_.m__colon_.text}\t{m__anglr_syntax_production_list_}\n\t{p__anglr_syntax_rule_.m__semicolon_.text}");
                        }
                        break;
                        case _anglr_syntax_rule_.production_kind.g__anglr_syntax_rule__2:
                        {
                            string m__anglr_syntax_rule_list_ = emitStack.Pop ();
                            string m__attribute_list_optional_ = emitStack.Pop ();
                            if (m__attribute_list_optional_.Length > 0)
                                emitStack.Push ($"{m__attribute_list_optional_}\n{p__anglr_syntax_rule_.m__identifier_.text}\n{p__anglr_syntax_rule_.m__left_curly_bracket_.text}\n{m__anglr_syntax_rule_list_}\n{p__anglr_syntax_rule_.m__right_curly_bracket_.text}");
                            else
                                emitStack.Push ($"{p__anglr_syntax_rule_.m__identifier_.text}\n{p__anglr_syntax_rule_.m__left_curly_bracket_.text}\n{m__anglr_syntax_rule_list_}\n{p__anglr_syntax_rule_.m__right_curly_bracket_.text}");
                        }
                        break;
                    }
                    --inNestedRule;
                }
                break;
            }
            return status;
        }

        private bool Invoke__anglr_nested_rule__Callback (SyntaxTreeCallbackReason reason, _anglr_nested_rule_.production_kind kind, _anglr_nested_rule_ p__anglr_nested_rule_)
        {
            bool status = true;
            switch (reason)
            {
                case SyntaxTreeCallbackReason.BuilderCallbackReason:
                    break;
                case SyntaxTreeCallbackReason.TraversalPrologueCallbackReason:
                    ++inNestedRule;
                    break;
                case SyntaxTreeCallbackReason.TraversalMidTermCallbackReason:
                    break;
                case SyntaxTreeCallbackReason.TraversalEpilogueCallbackReason:
                {
                    string m__anglr_syntax_production_list_ = emitStack.Pop ();
                    string m__anglr_syntax_production_list_name_optional_ = emitStack.Pop ();
                    if (inNestedRule > 1)
                    {
                        if (m__anglr_syntax_production_list_name_optional_.Length > 0)
                            emitStack.Push ($"{m__anglr_syntax_production_list_name_optional_}");
                        else
                            emitStack.Push ($"{m__anglr_syntax_production_list_}");
                    }
                    else
                    {
                        if (m__anglr_syntax_production_list_name_optional_.Length > 0)
                            emitStack.Push ($"{m__anglr_syntax_production_list_name_optional_}\n\t:\t{m__anglr_syntax_production_list_}\n\t;");
                        else
                            emitStack.Push ($"{m__anglr_syntax_production_list_}");
                    }
                    --inNestedRule;
                }
                break;
            }
            return status;
        }

        private bool Invoke__anglr_syntax_production_list_name_optional__Callback (SyntaxTreeCallbackReason reason, _anglr_syntax_production_list_name_optional_.production_kind kind, _anglr_syntax_production_list_name_optional_ p__anglr_syntax_production_list_name_optional_)
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
                    if (kind != _anglr_syntax_production_list_name_optional_.production_kind.g__anglr_syntax_production_list_name_optional__2)
                        emitStack.Push ("");
                }
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
                    break;
                case SyntaxTreeCallbackReason.TraversalPrologueCallbackReason:
                    break;
                case SyntaxTreeCallbackReason.TraversalMidTermCallbackReason:
                    break;
                case SyntaxTreeCallbackReason.TraversalEpilogueCallbackReason:
                {
                    emitStack.Push ($"{p__anglr_syntax_production_list_name_.m__identifier_.text}");
                }
                break;
            }
            return status;
        }

        private bool Invoke__anglr_syntax_production_list__Callback (SyntaxTreeCallbackReason reason, _anglr_syntax_production_list_.production_kind kind, _anglr_syntax_production_list_ p__anglr_syntax_production_list_)
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
                    if (kind != _anglr_syntax_production_list_.production_kind.g__anglr_syntax_production_list__2)
                        break;
                    string m__anglr_syntax_production_ = emitStack.Pop ();
                    string m__anglr_syntax_production_list_ = emitStack.Pop ();
                    emitStack.Push ($"{m__anglr_syntax_production_list_}\n\t{p__anglr_syntax_production_list_.m__vertical_bar_.text}\t{m__anglr_syntax_production_}");
                }
                break;
            }
            return status;
        }

        private bool Invoke__anglr_syntax_production__Callback (SyntaxTreeCallbackReason reason, _anglr_syntax_production_.production_kind kind, _anglr_syntax_production_ p__anglr_syntax_production_)
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
                    if (kind != _anglr_syntax_production_.production_kind.g__anglr_syntax_production__2)
                    {
                        string prioAssocSpec = emitStack.Pop ();
                        string productionString = emitStack.Pop ();
                        string productionName = emitStack.Pop ();
                        if (productionName.Length > 0)
                            productionName += ' ';
                        productionName += productionString;
                        if (prioAssocSpec.Length > 0)
                            productionName += $" {prioAssocSpec}";
                        emitStack.Push (productionName);
                    }
                    else
                        emitStack.Push ("%empty");
                }
                break;
            }
            return status;
        }

        private bool Invoke__priority_assoc_specification_optional__Callback (SyntaxTreeCallbackReason reason, _priority_assoc_specification_optional_.production_kind kind, _priority_assoc_specification_optional_ p__priority_assoc_specification_optional_)
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
                    if (_priority_assoc_specification_optional_.production_kind.g__priority_assoc_specification_optional__1 == kind)
                        emitStack.Push ("");
                }
                break;
            }
            return status;
        }

        private bool Invoke__priority_assoc_specification__Callback (SyntaxTreeCallbackReason reason, _priority_assoc_specification_.production_kind kind, _priority_assoc_specification_ p__priority_assoc_specification_)
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
                    switch (kind)
                    {
                        case _priority_assoc_specification_.production_kind.g__priority_assoc_specification__1:
                        {
                            string assocSpec = emitStack.Pop ();
                            string priorSpec = emitStack.Pop ();
                            emitStack.Push ($"{priorSpec} {assocSpec}");
                        }
                        break;
                        case _priority_assoc_specification_.production_kind.g__priority_assoc_specification__2:
                        {
                            string priorSpec = emitStack.Pop ();
                            string assocSpec = emitStack.Pop ();
                            emitStack.Push ($"{assocSpec} {priorSpec}");
                        }
                        break;
                        case _priority_assoc_specification_.production_kind.g__priority_assoc_specification__3:
                        {
                            string priorSpec = emitStack.Pop ();
                            emitStack.Push ($"{priorSpec}");
                        }
                        break;
                        case _priority_assoc_specification_.production_kind.g__priority_assoc_specification__4:
                        {
                            string assocSpec = emitStack.Pop ();
                            emitStack.Push ($"{assocSpec}");
                        }
                        break;
                        case _priority_assoc_specification_.production_kind.g__priority_assoc_specification__5:
                        {
                            string assocSpec = emitStack.Pop ();
                            string priorSpec = emitStack.Pop ();
                            emitStack.Push ($"{priorSpec} , {assocSpec}");
                        }
                        break;
                        case _priority_assoc_specification_.production_kind.g__priority_assoc_specification__6:
                        {
                            string priorSpec = emitStack.Pop ();
                            string assocSpec = emitStack.Pop ();
                            emitStack.Push ($"{assocSpec} , {priorSpec}");
                        }
                        break;
                    }
                }
                break;
            }
            return status;
        }

        private bool Invoke__priority_specification__Callback (SyntaxTreeCallbackReason reason, _priority_specification_.production_kind kind, _priority_specification_ p__priority_specification_)
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
                    switch (kind)
                    {
                        case _priority_specification_.production_kind.g__priority_specification__1:
                            emitStack.Push ($"{p__priority_specification_.m__priority_.text} {p__priority_specification_.m__number_.text}");
                            break;
                        case _priority_specification_.production_kind.g__priority_specification__2:
                            emitStack.Push ($"{p__priority_specification_.m__priority_.text} = {p__priority_specification_.m__number_.text}");
                            break;
                    }
                }
                break;
            }
            return status;
        }

        private bool Invoke__associativity_specification__Callback (SyntaxTreeCallbackReason reason, _associativity_specification_.production_kind kind, _associativity_specification_ p__associativity_specification_)
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
                    switch (kind)
                    {
                        case _associativity_specification_.production_kind.g__associativity_specification__1:
                            emitStack.Push ($"{p__associativity_specification_.m__associativity_.text} {p__associativity_specification_.m__cstring_.text}");
                            break;
                        case _associativity_specification_.production_kind.g__associativity_specification__2:
                            emitStack.Push ($"{p__associativity_specification_.m__associativity_.text} = {p__associativity_specification_.m__cstring_.text}");
                            break;
                        case _associativity_specification_.production_kind.g__associativity_specification__3:
                            emitStack.Push ($"{p__associativity_specification_.m__associativity_.text} {p__associativity_specification_.m__identifier_.text}");
                            break;
                        case _associativity_specification_.production_kind.g__associativity_specification__4:
                            emitStack.Push ($"{p__associativity_specification_.m__associativity_.text} = {p__associativity_specification_.m__identifier_.text}");
                            break;
                    }
                }
                break;
            }
            return status;
        }

        private bool Invoke__production_name_optional__Callback (SyntaxTreeCallbackReason reason, _production_name_optional_.production_kind kind, _production_name_optional_ p__production_name_optional_)
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
                    if (kind != _production_name_optional_.production_kind.g__production_name_optional__2)
                        emitStack.Push ($"");
                    break;
            }
            return status;
        }

        private bool Invoke__production_name__Callback (SyntaxTreeCallbackReason reason, _production_name_.production_kind kind, _production_name_ p__production_name_)
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
                    if (p__production_name_ == rootNode)
                        emitStack.Push ($"production name = {p__production_name_.m__identifier_.text}");
                    else
                        emitStack.Push ($"{p__production_name_.m__double_at_sign_.text}{p__production_name_.m__identifier_.text}");
                    break;
            }
            return status;
        }

        private bool Invoke__name_list__Callback (SyntaxTreeCallbackReason reason, _name_list_.production_kind kind, _name_list_ p__name_list_)
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
                    switch ((_name_list_.production_kind) kind)
                    {
                        case _name_list_.production_kind.g__name_list__1:
                            break;
                        case _name_list_.production_kind.g__name_list__2:
                        {
                            string m__marker_list_optional_ = emitStack.Pop ();
                            string m__g_name_ = emitStack.Pop ();
                            string m__name_list_ = emitStack.Pop ();
                            if(m__marker_list_optional_.Length > 0)
                                emitStack.Push ($"{m__name_list_} {m__g_name_} {m__marker_list_optional_}");
                            else
                                emitStack.Push ($"{m__name_list_} {m__g_name_}");
                        }
                        break;
                    }
                }
                break;
            }
            return status;
        }

        private bool Invoke__marker_list_optional__Callback (SyntaxTreeCallbackReason reason, _marker_list_optional_.production_kind kind, _marker_list_optional_ p__marker_list_optional_)
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
                    if (kind != _marker_list_optional_.production_kind.g__marker_list_optional__2)
                        emitStack.Push ("");
                }
                    break;
            }
            return status;
        }

        private bool Invoke__marker_list__Callback (SyntaxTreeCallbackReason reason, _marker_list_.production_kind kind, _marker_list_ p__marker_list_)
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
                    if (kind != _marker_list_.production_kind.g__marker_list__2)
                        break;
                    string m__marker_ = emitStack.Pop ();
                    string m__marker_list_ = emitStack.Pop ();
                    emitStack.Push ($"{m__marker_list_} {m__marker_})");
                }
                    break;
            }
            return status;
        }

        private bool Invoke__marker__Callback (SyntaxTreeCallbackReason reason, _marker_.production_kind kind, _marker_ p__marker_)
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
                    if (p__marker_ == rootNode)
                        emitStack.Push ($"production marker; name = {p__marker_.m__identifier_.text}, value = 0");
                    else
                        emitStack.Push ($"{p__marker_.m__at_sign_.text}{p__marker_.m__identifier_.text}");
                }
                break;
            }
            return status;
        }

        private bool Invoke__g_name__Callback (SyntaxTreeCallbackReason reason, _g_name_.production_kind kind, _g_name_ p__g_name_)
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
                    switch (kind)
                    {
                        case _g_name_.production_kind.g__g_name__1:
                            break;
                        case _g_name_.production_kind.g__g_name__2:
                        {
                            string m__anglr_nested_rule_ = emitStack.Pop ();
                            emitStack.Push ($"{m__anglr_nested_rule_}");
                        }
                        break;
                        case _g_name_.production_kind.g__g_name__3:
                        {
                            string m__cardinality_delimiter_ = emitStack.Pop ();
                            string m__g_name_ = emitStack.Pop ();
                            if (m__g_name_.Length > 0)
                                emitStack.Push ($"{m__g_name_} {m__cardinality_delimiter_}");
                            else
                                emitStack.Push (m__cardinality_delimiter_);
                        }
                        break;
                    }
                }
                break;
            }
            return status;
        }

        private bool Invoke__name__Callback (SyntaxTreeCallbackReason reason, _name_.production_kind kind, _name_ p__name_)
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
                    switch (kind)
                    {
                        case _name_.production_kind.g__name__1:
                            emitStack.Push (p__name_.m__any_.text);
                            break;
                        case _name_.production_kind.g__name__2:
                            emitStack.Push (p__name_.m__cstring_.text);
                            break;
                        case _name_.production_kind.g__name__3:
                            emitStack.Push (p__name_.m__identifier_.text);
                            break;
                    }
                }
                break;
            }
            return status;
        }

        private bool Invoke__cardinality_delimiter__Callback (SyntaxTreeCallbackReason reason, _cardinality_delimiter_.production_kind kind, _cardinality_delimiter_ p__cardinality_delimiter_)
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
                    string delimiter = emitStack.Pop ();
                    string cardinality = emitStack.Pop ();
                    if (delimiter.Length > 0)
                        cardinality += $" {delimiter}";
                    emitStack.Push (cardinality);
                }
                break;
            }
            return status;
        }

        private bool Invoke__cardinality__Callback (SyntaxTreeCallbackReason reason, _cardinality_.production_kind kind, _cardinality_ p__cardinality_)
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
                    switch (kind)
                    {
                        case _cardinality_.production_kind.g__cardinality__1:
                            emitStack.Push (p__cardinality_.m__question_mark_.text);
                            break;
                        case _cardinality_.production_kind.g__cardinality__2:
                            emitStack.Push (p__cardinality_.m__plus_sign_.text);
                            break;
                        case _cardinality_.production_kind.g__cardinality__3:
                            emitStack.Push (p__cardinality_.m__minus_sign_.text);
                            break;
                        case _cardinality_.production_kind.g__cardinality__4:
                            emitStack.Push (p__cardinality_.m__asterisk_.text);
                            break;
                        case _cardinality_.production_kind.g__cardinality__5:
                            emitStack.Push (p__cardinality_.m__slash_.text);
                            break;
                        case _cardinality_.production_kind.g__cardinality__6:
                            emitStack.Push (p__cardinality_.m__inv_plus_sign_.text);
                            break;
                        case _cardinality_.production_kind.g__cardinality__7:
                            emitStack.Push (p__cardinality_.m__inv_minus_sign_.text);
                            break;
                        case _cardinality_.production_kind.g__cardinality__8:
                            emitStack.Push (p__cardinality_.m__inv_asterisk_.text);
                            break;
                        case _cardinality_.production_kind.g__cardinality__9:
                            emitStack.Push (p__cardinality_.m__inv_slash_.text);
                            break;
                        case _cardinality_.production_kind.g__cardinality__10:
                        {
                            string m_nrmax = emitStack.Pop ();
                            string m_nrmin = emitStack.Pop ();
                            emitStack.Push ($"{p__cardinality_.m__left_curly_bracket_.text}{m_nrmin}{p__cardinality_.m__comma_.text}{m_nrmax}{p__cardinality_.m__right_curly_bracket_.text}");
                        }
                        break;
                    }
                }
                break;
            }
            return status;
        }

        private bool Invoke__number_optional__Callback (SyntaxTreeCallbackReason reason, _number_optional_.production_kind kind, _number_optional_ p__number_optional_)
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
                    if (kind != _number_optional_.production_kind.g__number_optional__2)
                        emitStack.Push ("");
                    else
                        emitStack.Push ($"{p__number_optional_.m__number_.text}");
                }
                break;
            }
            return status;
        }

        private bool Invoke__delimiter_optional__Callback (SyntaxTreeCallbackReason reason, _delimiter_optional_.production_kind kind, _delimiter_optional_ p__delimiter_optional_)
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
                    if (kind != _delimiter_optional_.production_kind.g__delimiter_optional__2)
                        emitStack.Push ("");
                }
                break;
            }
            return status;
        }

        private bool Invoke__delimiter__Callback (SyntaxTreeCallbackReason reason, _delimiter_.production_kind kind, _delimiter_ p__delimiter_)
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
                    string m__anglr_nested_rule_ = emitStack.Pop ();
                    emitStack.Push ($"{p__delimiter_.m__left_square_bracket_.text} {m__anglr_nested_rule_} {p__delimiter_.m__right_square_bracket_.text}");
                }
                break;
            }
            return status;
        }
    }

}
