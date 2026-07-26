using Anglr.Parser;
using Anglr.Parser.Core;
using System;
using System.Collections.Generic;
using System.Text;

namespace AnglrParserLibrary
{
    internal class AnglrVisualizer : SyntaxTreeWalker
    {
        public AnglrVisualizer ()
        {
            _parser_part__Event += AnglrVisualizer__parser_part__Event;
            _anglr_syntax_rule_list__Event += AnglrVisualizer__anglr_syntax_rule_list__Event;
            _anglr_syntax_rule__Event += AnglrVisualizer__anglr_syntax_rule__Event;
            _anglr_nested_rule__Event += AnglrVisualizer__anglr_nested_rule__Event;
            _anglr_syntax_production_list_name__Event += AnglrVisualizer__anglr_syntax_production_list_name__Event;
            _anglr_syntax_production_list__Event += AnglrVisualizer__anglr_syntax_production_list__Event;
            _anglr_syntax_production__Event += AnglrVisualizer__anglr_syntax_production__Event;
            _production_name__Event += AnglrVisualizer__production_name__Event;
            _priority_assoc_specification__Event += AnglrVisualizer__priority_assoc_specification__Event;
            _priority_specification__Event += AnglrVisualizer__priority_specification__Event;
            _associativity_specification__Event += AnglrVisualizer__associativity_specification__Event;
            _name_list__Event += AnglrVisualizer__name_list__Event;
            _marker_list__Event += AnglrVisualizer__marker_list__Event;
            _marker__Event += AnglrVisualizer__marker__Event;
            _g_name__Event += AnglrVisualizer__g_name__Event;
            _name__Event += AnglrVisualizer__name__Event;
            _cardinality_delimiter__Event += AnglrVisualizer__cardinality_delimiter__Event;
            _cardinality__Event += AnglrVisualizer__cardinality__Event;
            _delimiter__Event += AnglrVisualizer__delimiter__Event;
        }

        private bool AnglrVisualizer__parser_part__Event (SyntaxTreeCallbackReason reason, _parser_part_.production_kind kind, _parser_part_ p__parser_part_)
        {
            switch (reason)
            {
                case SyntaxTreeCallbackReason.TraversalPrologueCallbackReason:
                    break;
                case SyntaxTreeCallbackReason.TraversalEpilogueCallbackReason:
                    break;
            }
            return true;
        }

        private bool AnglrVisualizer__anglr_syntax_rule_list__Event (SyntaxTreeCallbackReason reason, _anglr_syntax_rule_list_.production_kind kind, _anglr_syntax_rule_list_ p__anglr_syntax_rule_list_)
        {
            switch (reason)
            {
                case SyntaxTreeCallbackReason.TraversalPrologueCallbackReason:
                    break;
                case SyntaxTreeCallbackReason.TraversalEpilogueCallbackReason:
                    break;
            }
            return true;
        }

        private bool AnglrVisualizer__anglr_syntax_rule__Event (SyntaxTreeCallbackReason reason, _anglr_syntax_rule_.production_kind kind, _anglr_syntax_rule_ p__anglr_syntax_rule_)
        {
            switch (reason)
            {
                case SyntaxTreeCallbackReason.TraversalPrologueCallbackReason:
                    break;
                case SyntaxTreeCallbackReason.TraversalEpilogueCallbackReason:
                    break;
            }
            return true;
        }

        private bool AnglrVisualizer__anglr_nested_rule__Event (SyntaxTreeCallbackReason reason, _anglr_nested_rule_.production_kind kind, _anglr_nested_rule_ p__anglr_nested_rule_)
        {
            switch (reason)
            {
                case SyntaxTreeCallbackReason.TraversalPrologueCallbackReason:
                    break;
                case SyntaxTreeCallbackReason.TraversalEpilogueCallbackReason:
                    break;
            }
            return true;
        }

        private bool AnglrVisualizer__anglr_syntax_production_list_name__Event (SyntaxTreeCallbackReason reason, _anglr_syntax_production_list_name_.production_kind kind, _anglr_syntax_production_list_name_ p__anglr_syntax_production_list_name_)
        {
            switch (reason)
            {
                case SyntaxTreeCallbackReason.TraversalPrologueCallbackReason:
                    break;
                case SyntaxTreeCallbackReason.TraversalEpilogueCallbackReason:
                    break;
            }
            return true;
        }

        private bool AnglrVisualizer__anglr_syntax_production_list__Event (SyntaxTreeCallbackReason reason, _anglr_syntax_production_list_.production_kind kind, _anglr_syntax_production_list_ p__anglr_syntax_production_list_)
        {
            switch (reason)
            {
                case SyntaxTreeCallbackReason.TraversalPrologueCallbackReason:
                    break;
                case SyntaxTreeCallbackReason.TraversalEpilogueCallbackReason:
                    break;
            }
            return true;
        }

        private bool AnglrVisualizer__anglr_syntax_production__Event (SyntaxTreeCallbackReason reason, _anglr_syntax_production_.production_kind kind, _anglr_syntax_production_ p__anglr_syntax_production_)
        {
            switch (reason)
            {
                case SyntaxTreeCallbackReason.TraversalPrologueCallbackReason:
                    break;
                case SyntaxTreeCallbackReason.TraversalEpilogueCallbackReason:
                    break;
            }
            return true;
        }

        private bool AnglrVisualizer__production_name__Event (SyntaxTreeCallbackReason reason, _production_name_.production_kind kind, _production_name_ p__production_name_)
        {
            switch (reason)
            {
                case SyntaxTreeCallbackReason.TraversalPrologueCallbackReason:
                    break;
                case SyntaxTreeCallbackReason.TraversalEpilogueCallbackReason:
                    break;
            }
            return true;
        }

        private bool AnglrVisualizer__priority_assoc_specification__Event (SyntaxTreeCallbackReason reason, _priority_assoc_specification_.production_kind kind, _priority_assoc_specification_ p__priority_assoc_specification_)
        {
            switch (reason)
            {
                case SyntaxTreeCallbackReason.TraversalPrologueCallbackReason:
                    break;
                case SyntaxTreeCallbackReason.TraversalEpilogueCallbackReason:
                    break;
            }
            return true;
        }

        private bool AnglrVisualizer__priority_specification__Event (SyntaxTreeCallbackReason reason, _priority_specification_.production_kind kind, _priority_specification_ p__priority_specification_)
        {
            switch (reason)
            {
                case SyntaxTreeCallbackReason.TraversalPrologueCallbackReason:
                    break;
                case SyntaxTreeCallbackReason.TraversalEpilogueCallbackReason:
                    break;
            }
            return true;
        }

        private bool AnglrVisualizer__associativity_specification__Event (SyntaxTreeCallbackReason reason, _associativity_specification_.production_kind kind, _associativity_specification_ p__associativity_specification_)
        {
            switch (reason)
            {
                case SyntaxTreeCallbackReason.TraversalPrologueCallbackReason:
                    break;
                case SyntaxTreeCallbackReason.TraversalEpilogueCallbackReason:
                    break;
            }
            return true;
        }

        private bool AnglrVisualizer__name_list__Event (SyntaxTreeCallbackReason reason, _name_list_.production_kind kind, _name_list_ p__name_list_)
        {
            switch (reason)
            {
                case SyntaxTreeCallbackReason.TraversalPrologueCallbackReason:
                    break;
                case SyntaxTreeCallbackReason.TraversalEpilogueCallbackReason:
                    break;
            }
            return true;
        }

        private bool AnglrVisualizer__marker_list__Event (SyntaxTreeCallbackReason reason, _marker_list_.production_kind kind, _marker_list_ p__marker_list_)
        {
            switch (reason)
            {
                case SyntaxTreeCallbackReason.TraversalPrologueCallbackReason:
                    break;
                case SyntaxTreeCallbackReason.TraversalEpilogueCallbackReason:
                    break;
            }
            return true;
        }

        private bool AnglrVisualizer__marker__Event (SyntaxTreeCallbackReason reason, _marker_.production_kind kind, _marker_ p__marker_)
        {
            switch (reason)
            {
                case SyntaxTreeCallbackReason.TraversalPrologueCallbackReason:
                    break;
                case SyntaxTreeCallbackReason.TraversalEpilogueCallbackReason:
                    break;
            }
            return true;
        }

        private bool AnglrVisualizer__g_name__Event (SyntaxTreeCallbackReason reason, _g_name_.production_kind kind, _g_name_ p__g_name_)
        {
            switch (reason)
            {
                case SyntaxTreeCallbackReason.TraversalPrologueCallbackReason:
                    break;
                case SyntaxTreeCallbackReason.TraversalEpilogueCallbackReason:
                    break;
            }
            return true;
        }

        private bool AnglrVisualizer__name__Event (SyntaxTreeCallbackReason reason, _name_.production_kind kind, _name_ p__name_)
        {
            switch (reason)
            {
                case SyntaxTreeCallbackReason.TraversalPrologueCallbackReason:
                    break;
                case SyntaxTreeCallbackReason.TraversalEpilogueCallbackReason:
                    break;
            }
            return true;
        }

        private bool AnglrVisualizer__cardinality_delimiter__Event (SyntaxTreeCallbackReason reason, _cardinality_delimiter_.production_kind kind, _cardinality_delimiter_ p__cardinality_delimiter_)
        {
            switch (reason)
            {
                case SyntaxTreeCallbackReason.TraversalPrologueCallbackReason:
                    break;
                case SyntaxTreeCallbackReason.TraversalEpilogueCallbackReason:
                    break;
            }
            return true;
        }

        private bool AnglrVisualizer__cardinality__Event (SyntaxTreeCallbackReason reason, _cardinality_.production_kind kind, _cardinality_ p__cardinality_)
        {
            switch (reason)
            {
                case SyntaxTreeCallbackReason.TraversalPrologueCallbackReason:
                    break;
                case SyntaxTreeCallbackReason.TraversalEpilogueCallbackReason:
                    break;
            }
            return true;
        }

        private bool AnglrVisualizer__delimiter__Event (SyntaxTreeCallbackReason reason, _delimiter_.production_kind kind, _delimiter_ p__delimiter_)
        {
            switch (reason)
            {
                case SyntaxTreeCallbackReason.TraversalPrologueCallbackReason:
                    break;
                case SyntaxTreeCallbackReason.TraversalEpilogueCallbackReason:
                    break;
            }
            return true;
        }
    }
}
