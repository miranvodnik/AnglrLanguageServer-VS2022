using Anglr.Parser;

namespace AnglrLibrary
{
    public class AnglrIterator : SyntaxTreeWalker
    {
        public AnglrIterator()
        {
            _anglr_file_part_list__Event += (SyntaxTreeCallbackReason reason, _anglr_file_part_list_.production_kind kind, _anglr_file_part_list_ p__anglr_file_part_list_) =>
            {
                if (reason == SyntaxTreeCallbackReason.TraversalPrologueCallbackReason)
                    p__anglr_file_part_list_.Iterate (this);
                return false;
            };
            _regular_expression_list__Event += (SyntaxTreeCallbackReason reason, _regular_expression_list_.production_kind kind, _regular_expression_list_ p__regular_expression_list_) =>
            {
                if (reason == SyntaxTreeCallbackReason.TraversalPrologueCallbackReason)
                    p__regular_expression_list_.Iterate (this);
                return false;
            };
            _attribute_list__Event += (SyntaxTreeCallbackReason reason, _attribute_list_.production_kind kind, _attribute_list_ p__attribute_list_) =>
            {
                if (reason == SyntaxTreeCallbackReason.TraversalPrologueCallbackReason)
                    p__attribute_list_.Iterate (this);
                return false;
            };
            _name_value_list__Event += (SyntaxTreeCallbackReason reason, _name_value_list_.production_kind kind, _name_value_list_ p__name_value_list_) =>
            {
                if (reason == SyntaxTreeCallbackReason.TraversalPrologueCallbackReason)
                    p__name_value_list_.Iterate (this);
                return false;
            };
            _anglr_definition_list__Event += (SyntaxTreeCallbackReason reason, _anglr_definition_list_.production_kind kind, _anglr_definition_list_ p__anglr_definition_list_) =>
            {
                if (reason == SyntaxTreeCallbackReason.TraversalPrologueCallbackReason)
                    p__anglr_definition_list_.Iterate (this);
                return false;
            };
            _token_string__Event += (SyntaxTreeCallbackReason reason, _token_string_.production_kind kind, _token_string_ p__token_string_) =>
            {
                if (reason == SyntaxTreeCallbackReason.TraversalPrologueCallbackReason)
                    p__token_string_.Iterate (this);
                return false;
            };
            _anglr_syntax_rule_list__Event += (SyntaxTreeCallbackReason reason, _anglr_syntax_rule_list_.production_kind kind, _anglr_syntax_rule_list_ p__anglr_syntax_rule_list_) =>
            {
                if (reason == SyntaxTreeCallbackReason.TraversalPrologueCallbackReason)
                    p__anglr_syntax_rule_list_.Iterate (this);
                return false;
            };
            _anglr_syntax_production_list__Event += (SyntaxTreeCallbackReason reason, _anglr_syntax_production_list_.production_kind kind, _anglr_syntax_production_list_ p__anglr_syntax_production_list_) =>
            {
                if (reason == SyntaxTreeCallbackReason.TraversalPrologueCallbackReason)
                    p__anglr_syntax_production_list_.Iterate (this);
                return false;
            };
            _name_list__Event += (SyntaxTreeCallbackReason reason, _name_list_.production_kind kind, _name_list_ p__name_list_) =>
            {
                if (reason == SyntaxTreeCallbackReason.TraversalPrologueCallbackReason)
                    p__name_list_.Iterate (this);
                return false;
            };
        }
    }
}
