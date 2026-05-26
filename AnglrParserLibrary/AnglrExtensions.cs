using Anglr.Parser;

namespace AnglrLibrary
{
    public static class AnglrExtensions
    {
        #region Custom Iterators

        public delegate object CustomIteratorDelegate (SyntaxTreeBase node, object appData);

        public static object CustomIterate (this _anglr_file_part_list_ topNode, object appData, CustomIteratorDelegate customIteratorDelegate)
        {
            _anglr_file_part_list_ leafNode;
            for (leafNode = topNode; topNode != null; topNode = topNode.m__anglr_file_part_list_)
                leafNode = topNode;
            for (SyntaxTreeBase currentNode = leafNode; (currentNode != null) && (currentNode is _anglr_file_part_list_); currentNode = currentNode.parent)
                appData = customIteratorDelegate (((_anglr_file_part_list_) currentNode).m__anglr_file_part_, appData);
            return appData;
        }

        public static object CustomIterate (this _regular_expression_list_ topNode, object appData, CustomIteratorDelegate customIteratorDelegate)
        {
            _regular_expression_list_ leafNode;
            for (leafNode = topNode; topNode != null; topNode = topNode.m__regular_expression_list_)
                leafNode = topNode;
            for (SyntaxTreeBase currentNode = leafNode; (currentNode != null) && (currentNode is _regular_expression_list_); currentNode = currentNode.parent)
                appData = customIteratorDelegate (((_regular_expression_list_) currentNode).m__regular_expression_usage_, appData);
            return appData;
        }

        public static object CustomIterate (this _actions_ topNode, object appData, CustomIteratorDelegate customIteratorDelegate)
        {
            _actions_ leafNode;
            for (leafNode = topNode; topNode != null; topNode = topNode.m__actions_)
                leafNode = topNode;
            for (SyntaxTreeBase currentNode = leafNode; (currentNode != null) && (currentNode is _actions_); currentNode = currentNode.parent)
                appData = customIteratorDelegate (((_actions_) currentNode).m__action_, appData);
            return appData;
        }

        public static object CustomIterate (this _attribute_list_ topNode, object appData, CustomIteratorDelegate customIteratorDelegate)
        {
            _attribute_list_ leafNode;
            for (leafNode = topNode; topNode != null; topNode = topNode.m__attribute_list_)
                leafNode = topNode;
            for (SyntaxTreeBase currentNode = leafNode; (currentNode != null) && (currentNode is _attribute_list_); currentNode = currentNode.parent)
                appData = customIteratorDelegate (((_attribute_list_) currentNode).m__attribute_, appData);
            return appData;
        }

        public static object CustomIterate (this _name_value_list_ topNode, object appData, CustomIteratorDelegate customIteratorDelegate)
        {
            _name_value_list_ leafNode;
            for (leafNode = topNode; topNode != null; topNode = topNode.m__name_value_list_)
                leafNode = topNode;
            for (SyntaxTreeBase currentNode = leafNode; (currentNode != null) && (currentNode is _name_value_list_); currentNode = currentNode.parent)
                appData = customIteratorDelegate (((_name_value_list_) currentNode).m__name_value_pair_, appData);
            return appData;
        }

        public static object CustomIterate (this _anglr_definition_list_ topNode, object appData, CustomIteratorDelegate customIteratorDelegate)
        {
            _anglr_definition_list_ leafNode;
            for (leafNode = topNode; topNode != null; topNode = topNode.m__anglr_definition_list_)
                leafNode = topNode;
            for (SyntaxTreeBase currentNode = leafNode; (currentNode != null) && (currentNode is _anglr_definition_list_); currentNode = currentNode.parent)
                appData = customIteratorDelegate (((_anglr_definition_list_) currentNode).m__anglr_definition_with_attribute_.m__anglr_definition_, appData);
            return appData;
        }

        public static object CustomIterate (this _block_terminal_definitions_ topNode, object appData, CustomIteratorDelegate customIteratorDelegate)
        {
            _block_terminal_definitions_ leafNode;
            for (leafNode = topNode; topNode != null; topNode = topNode.m__block_terminal_definitions_)
                leafNode = topNode;
            for (SyntaxTreeBase currentNode = leafNode; (currentNode != null) && (currentNode is _block_terminal_definitions_); currentNode = currentNode.parent)
                appData = customIteratorDelegate (((_block_terminal_definitions_) currentNode).m__block_terminal_definition_, appData);
            return appData;
        }

        public static object CustomIterate (this _block_regex_definitions_ topNode, object appData, CustomIteratorDelegate customIteratorDelegate)
        {
            _block_regex_definitions_ leafNode;
            for (leafNode = topNode; topNode != null; topNode = topNode.m__block_regex_definitions_)
                leafNode = topNode;
            for (SyntaxTreeBase currentNode = leafNode; (currentNode != null) && (currentNode is _block_regex_definitions_); currentNode = currentNode.parent)
                appData = customIteratorDelegate (((_block_regex_definitions_) currentNode).m__block_regex_definition_, appData);
            return appData;
        }

        public static object CustomIterate (this _anglr_syntax_rule_list_ topNode, object appData, CustomIteratorDelegate customIteratorDelegate)
        {
            _anglr_syntax_rule_list_ leafNode;
            for (leafNode = topNode; topNode != null; topNode = topNode.m__anglr_syntax_rule_list_)
                leafNode = topNode;
            for (SyntaxTreeBase currentNode = leafNode; (currentNode != null) && (currentNode is _anglr_syntax_rule_list_); currentNode = currentNode.parent)
                appData = customIteratorDelegate (((_anglr_syntax_rule_list_) currentNode).m__anglr_syntax_rule_, appData);
            return appData;
        }

        public static object CustomIterate (this _anglr_syntax_production_list_ topNode, object appData, CustomIteratorDelegate customIteratorDelegate)
        {
            _anglr_syntax_production_list_ leafNode;
            for (leafNode = topNode; topNode != null; topNode = topNode.m__anglr_syntax_production_list_)
                leafNode = topNode;
            for (SyntaxTreeBase currentNode = leafNode; (currentNode != null) && (currentNode is _anglr_syntax_production_list_); currentNode = currentNode.parent)
                appData = customIteratorDelegate (((_anglr_syntax_production_list_) currentNode).m__anglr_syntax_production_, appData);
            return appData;
        }

        public static object CustomIterate (this _marker_list_ topNode, object appData, CustomIteratorDelegate customIteratorDelegate)
        {
            _marker_list_ leafNode;
            for (leafNode = topNode; topNode != null; topNode = topNode.m__marker_list_)
                leafNode = topNode;
            for (SyntaxTreeBase currentNode = leafNode; (currentNode != null) && (currentNode is _marker_list_); currentNode = currentNode.parent)
                appData = customIteratorDelegate (((_marker_list_) currentNode).m__marker_, appData);
            return appData;
        }

        public static object CustomIterate (this _name_list_ topNode, object appData, CustomIteratorDelegate customIteratorDelegate)
        {
            _name_list_ leafNode;
            for (leafNode = topNode; topNode != null; topNode = topNode.m__name_list_)
                leafNode = topNode;
            for (SyntaxTreeBase currentNode = leafNode; (currentNode != null) && (currentNode is _name_list_); currentNode = currentNode.parent)
                appData = customIteratorDelegate (((_name_list_) currentNode).m__marker_list_optional_, currentNode);
            return appData;
        }

        #endregion

        #region Iterators for Recursive Nodes

        public static void Iterate (this _attribute_list_ topNode, SyntaxTreeWalker walker)
        {
            topNode.CustomIterate (null, (node, appData) =>
            {
                walker.Traverse ((_attribute_) node);
                return null;
            });
        }

        public static void Iterate (this _name_value_list_ topNode, SyntaxTreeWalker walker)
        {
            topNode.CustomIterate (null, (node, appData) =>
            {
                walker.Traverse ((_name_value_pair_) node);
                return null;
            });
        }

        public static void Iterate (this _anglr_file_part_list_ topNode, SyntaxTreeWalker walker)
        {
            topNode.CustomIterate (null, (node, appData) =>
            {
                walker.Traverse ((_anglr_file_part_) node);
                return null;
            });
        }

        public static void Iterate (this _anglr_definition_list_ topNode, SyntaxTreeWalker walker)
        {
            topNode.CustomIterate (null, (node, appData) =>
            {
                walker.Traverse ((_anglr_definition_) node);
                return null;
            });
        }

        public static void Iterate (this _block_terminal_definitions_ topNode, SyntaxTreeWalker walker)
        {
            topNode.CustomIterate (null, (node, appData) =>
            {
                walker.Traverse ((_block_terminal_definition_) node);
                return null;
            });
        }

        public static void Iterate (this _block_regex_definitions_ topNode, SyntaxTreeWalker walker)
        {
            topNode.CustomIterate (null, (node, appData) =>
            {
                walker.Traverse ((_block_regex_definition_) node);
                return null;
            });
        }

        public static void Iterate (this _regular_expression_list_ topNode, SyntaxTreeWalker walker)
        {
            topNode.CustomIterate (null, (node, appData) =>
            {
                walker.Traverse ((_regular_expression_usage_) node);
                return null;
            });
        }

        public static void Iterate (this _actions_ topNode, SyntaxTreeWalker walker)
        {
            topNode.CustomIterate (null, (node, appData) =>
            {
                walker.Traverse ((_action_) node);
                return null;
            });
        }

        public static void Iterate (this _anglr_syntax_rule_list_ topNode, SyntaxTreeWalker walker)
        {
            topNode.CustomIterate (null, (node, appData) =>
            {
                walker.Traverse ((_anglr_syntax_rule_) node);
                return null;
            });
        }

        public static void Iterate (this _anglr_syntax_production_list_ topNode, SyntaxTreeWalker walker)
        {
            topNode.CustomIterate (null, (node, appData) =>
            {
                walker.Traverse ((_anglr_syntax_production_) node);
                return null;
            });
        }

        public static void Iterate (this _name_list_ topNode, SyntaxTreeWalker walker)
        {
            topNode.CustomIterate (null, (node, appData) =>
            {
                _g_name_ g_name = ((_name_list_) appData).m__g_name_;
                if (g_name != null)
                    walker.Traverse (g_name);
                walker.Traverse ((_marker_list_optional_) node);
                return null;
            });
        }

        public static void Iterate (this _marker_list_ topNode, SyntaxTreeWalker walker)
        {
            topNode.CustomIterate (null, (node, appData) =>
            {
                walker.Traverse ((_marker_) node);
                return null;
            });
        }

        #endregion
    }
}
