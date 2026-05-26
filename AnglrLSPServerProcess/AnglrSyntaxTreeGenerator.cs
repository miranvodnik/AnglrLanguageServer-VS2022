using System;
using System.Collections.Generic;
using Anglr.Parser;
using Anglr.Parser.Core;
using Anglr.Parser.SyntaxTree;
using AnglrLibrary;
using System.Net;
using System.IO;
using Anglr.Declarations;
using System.Xml.Linq;

namespace AnglrLSPServerProcess
{

    internal static class FragmentIdMapping
    {
        private static Dictionary<ProductionID, int> fragmentValues = new Dictionary<ProductionID, int> ()
        {
            { ProductionID.__anglr_file_fragment__ID, -1 },
            { ProductionID.__attribute_list__ID, AnglrDeclarations.tokens._attribute_list_terminal_ },
            { ProductionID.__attribute__ID, AnglrDeclarations.tokens._attribute_terminal_ },
            { ProductionID.__name_value_list__ID, AnglrDeclarations.tokens._name_value_list_terminal_ },
            { ProductionID.__name_value_pair__ID, AnglrDeclarations.tokens._name_value_pair_terminal_ },
            { ProductionID.__anglr_file__ID, AnglrDeclarations.tokens._anglr_file_terminal_ },
            { ProductionID.__anglr_file_part_list__ID, AnglrDeclarations.tokens._anglr_file_part_list_terminal_ },
            { ProductionID.__anglr_file_part__ID, AnglrDeclarations.tokens._anglr_file_part_terminal_ },
            { ProductionID.__general_part__ID, AnglrDeclarations.tokens._general_part_terminal_ },
            { ProductionID.__declaration_part__ID, AnglrDeclarations.tokens._declaration_part_terminal_ },
            { ProductionID.__anglr_definition_list__ID, AnglrDeclarations.tokens._anglr_definition_list_terminal_ },
            { ProductionID.__anglr_definition_with_attribute__ID, AnglrDeclarations.tokens._anglr_definition_with_attribute_list_terminal_ },
            { ProductionID.__anglr_definition__ID, AnglrDeclarations.tokens._anglr_definition_terminal_ },
            { ProductionID.__single_terminal_definition__ID, AnglrDeclarations.tokens._single_terminal_definition_terminal_ },
            { ProductionID.__single_regex_definition__ID, AnglrDeclarations.tokens._single_regex_definition_terminal_ },
            { ProductionID.__block_of_terminal_definitions__ID, AnglrDeclarations.tokens._block_of_terminal_definitions_terminal_ },
            { ProductionID.__block_of_regex_definitions__ID, AnglrDeclarations.tokens._block_of_regex_definitions_terminal_ },
            { ProductionID.__terminal_definition__ID, AnglrDeclarations.tokens._terminal_definition_terminal_ },
            { ProductionID.__regex_definition__ID, AnglrDeclarations.tokens._regex_definition_terminal_ },
            { ProductionID.__block_terminal_definitions__ID, AnglrDeclarations.tokens._block_terminal_definitions_terminal_ },
            { ProductionID.__block_terminal_definition__ID, AnglrDeclarations.tokens._block_terminal_definition_terminal_ },
            { ProductionID.__block_regex_definitions__ID, AnglrDeclarations.tokens._block_regex_definitions_terminal_ },
            { ProductionID.__block_regex_definition__ID, AnglrDeclarations.tokens._block_regex_definition_terminal_ },
            { ProductionID.__scanner_part__ID, AnglrDeclarations.tokens._scanner_part_terminal_ },
            { ProductionID.__regular_expression_list__ID, AnglrDeclarations.tokens._regular_expression_list_terminal_ },
            { ProductionID.__regular_expression_usage__ID, AnglrDeclarations.tokens._regular_expression_usage_terminal_ },
            { ProductionID.__actions__ID, AnglrDeclarations.tokens._actions_terminal_ },
            { ProductionID.__action__ID, AnglrDeclarations.tokens._action_terminal_ },
            { ProductionID.__skip_action__ID, AnglrDeclarations.tokens._skip_action_terminal_ },
            { ProductionID.__terminal_action__ID, AnglrDeclarations.tokens._terminal_action_terminal_ },
            { ProductionID.__event_action__ID, AnglrDeclarations.tokens._event_action_terminal_ },
            { ProductionID.__push_action__ID, AnglrDeclarations.tokens._push_action_terminal_ },
            { ProductionID.__pop_action__ID, AnglrDeclarations.tokens._pop_action_terminal_ },
            { ProductionID.__lexer_part__ID, AnglrDeclarations.tokens._lexer_part_terminal_ },
            { ProductionID.__parser_part__ID, AnglrDeclarations.tokens._parser_part_terminal_ },
            { ProductionID.__anglr_syntax_rule_list__ID, AnglrDeclarations.tokens._anglr_syntax_rule_list_terminal_ },
            { ProductionID.__anglr_syntax_rule__ID, AnglrDeclarations.tokens._anglr_syntax_rule_terminal_ },
            { ProductionID.__anglr_nested_rule__ID, AnglrDeclarations.tokens._anglr_nested_rule_terminal_ },
            { ProductionID.__anglr_syntax_production_list_name__ID, AnglrDeclarations.tokens._anglr_syntax_production_list_name_terminal_ },
            { ProductionID.__anglr_syntax_production_list__ID, AnglrDeclarations.tokens._anglr_syntax_production_list_terminal_ },
            { ProductionID.__anglr_syntax_production__ID, AnglrDeclarations.tokens._anglr_syntax_production_terminal_ },
            { ProductionID.__production_name__ID, AnglrDeclarations.tokens._production_name_terminal_ },
            { ProductionID.__priority_assoc_specification__ID, AnglrDeclarations.tokens._priority_assoc_specification_terminal_ },
            { ProductionID.__priority_specification__ID, AnglrDeclarations.tokens._priority_specification_terminal_ },
            { ProductionID.__associativity_specification__ID, AnglrDeclarations.tokens._associativity_specification_terminal_ },
            { ProductionID.__name_list__ID, AnglrDeclarations.tokens._name_list_terminal_ },
            { ProductionID.__marker_list__ID, AnglrDeclarations.tokens._marker_list_terminal_ },
            { ProductionID.__marker__ID, AnglrDeclarations.tokens._marker_terminal_ },
            { ProductionID.__g_name__ID, AnglrDeclarations.tokens._g_name_terminal_ },
            { ProductionID.__name__ID, AnglrDeclarations.tokens._name_terminal_ },
            { ProductionID.__cardinality_delimiter__ID, AnglrDeclarations.tokens._cardinality_delimiter_terminal_ },
            { ProductionID.__cardinality__ID, AnglrDeclarations.tokens._cardinality_terminal_ },
            { ProductionID.__delimiter__ID, AnglrDeclarations.tokens._delimiter_terminal_ },
        };
        public static int GetFragmentId (ProductionID id) => fragmentValues [id];
    }

    internal class CmpIndex : IComparer<AnglrStackChildren>
    {
        public int Compare (AnglrStackChildren x, AnglrStackChildren y)
        {
            int cmp;
            AnglrStackChildren.Enumerator xenum = x.GetEnumerator ();
            AnglrStackChildren.Enumerator yenum = y.GetEnumerator ();
            while (xenum.MoveNext () && yenum.MoveNext ())
            {
                cmp = (int) xenum.Current.node.id - (int) yenum.Current.node.id;
                if (cmp != 0)
                    return cmp;
                cmp = xenum.Current.index - yenum.Current.index;
                if (cmp != 0)
                    return cmp;
            }
            cmp = x.Count - y.Count;
            if (cmp != 0)
                return cmp;
            return 0;
        }
    }

    internal abstract class AnglrSyntaxTreeNode
    {
        public SyntaxTreeBase node { get; private set; } = null;
        public _anglr_file_fragment_ fragment { get; private set; } = null;
        public int index { get; private set; } = 0;
        public AnglrSyntaxTreeNode (SyntaxTreeBase node, _anglr_file_fragment_ fragment, int index)
        {
            this.node = node;
            this.fragment = fragment;
            this.index = index;
        }
        public void Add (AnglrSyntaxTreeNode node) => ((AnglrNodeChildren) (((AppInfo) (this.node.appInfo)) [AppInfoType.Children])).Add (node);
        public abstract void Prologue (StreamWriter writer, object obj);
        public abstract void Body (StreamWriter writer, object obj);
        public abstract void Epilogue (StreamWriter writer, object obj);
        public abstract string Name { get; }
        public abstract int NodeCategory { get; }
        public abstract int NodeSubCategory { get; }
        public int Count { get { return ((AnglrNodeChildren) ((AppInfo) node.appInfo) [AppInfoType.Children]).Count; } }
    }

    internal class anglr_file_fragment_STN : AnglrSyntaxTreeNode
    {
        public anglr_file_fragment_STN (SyntaxTreeBase node, int index) : base (node, null, index)
        {
        }

        public override string Name => "";

        public override int NodeCategory => (int) node.id;

        public override int NodeSubCategory => 0;

        public override void Body (StreamWriter writer, object obj)
        {
        }

        public override void Epilogue (StreamWriter writer, object obj)
        {
        }

        public override void Prologue (StreamWriter writer, object obj)
        {
        }
    }

    internal class attribute_list_STN : AnglrSyntaxTreeNode
    {
        public attribute_list_STN (SyntaxTreeBase node, int index) : base (node, new _anglr_file_fragment_ (new SyntaxTreeToken ((int) AnglrDeclarations.tokens._attribute_list_terminal_, -1, -1, ""), (_attribute_list_) node), index)
        {
        }

        public override string Name => "attributes";

        public override int NodeCategory => (int) node.id;

        public override int NodeSubCategory => 0;

        public override void Body (StreamWriter writer, object obj)
        {
        }

        public override void Epilogue (StreamWriter writer, object obj)
        {
            writer.WriteLine ($"{(string) obj}}}");
        }

        public override void Prologue (StreamWriter writer, object obj)
        {
            writer.WriteLine ($"{(string) obj}attributes");
            writer.WriteLine ($"{(string) obj}{{");
        }
    }

    internal class attribute_STN : AnglrSyntaxTreeNode
    {
        public attribute_STN (SyntaxTreeBase node, int index) : base (node, new _anglr_file_fragment_ (new SyntaxTreeToken ((int) AnglrDeclarations.tokens._attribute_terminal_, -1, -1, ""), (_attribute_) node), index)
        {
        }

        public override string Name => ((_attribute_) node).m__identifier_.text;

        public override int NodeCategory => (int) node.id;

        public override int NodeSubCategory => 0;

        public override void Body (StreamWriter writer, object obj)
        {
        }

        public override void Epilogue (StreamWriter writer, object obj)
        {
            writer.WriteLine ($"{(string) obj}}}");
        }

        public override void Prologue (StreamWriter writer, object obj)
        {
            _attribute_ _Attribute_ = (_attribute_) node;
            writer.WriteLine ($"{(string) obj}{_Attribute_.m__identifier_.text}");
            writer.WriteLine ($"{(string) obj}{{");
        }
    }

    internal class name_value_list_STN : AnglrSyntaxTreeNode
    {
        public name_value_list_STN (SyntaxTreeBase node, int index) : base (node, new _anglr_file_fragment_ (new SyntaxTreeToken ((int) AnglrDeclarations.tokens._name_value_list_terminal_, -1, -1, ""), (_name_value_list_) node), index)
        {
        }

        public override string Name => throw new NotImplementedException ();

        public override int NodeCategory => throw new NotImplementedException ();

        public override int NodeSubCategory => throw new NotImplementedException ();

        public override void Body (StreamWriter writer, object obj)
        {
            throw new NotImplementedException ();
        }

        public override void Epilogue (StreamWriter writer, object obj)
        {
            throw new NotImplementedException ();
        }

        public override void Prologue (StreamWriter writer, object obj)
        {
            throw new NotImplementedException ();
        }
    }

    internal class name_value_pair_STN : AnglrSyntaxTreeNode
    {
        public name_value_pair_STN (SyntaxTreeBase node, int index) : base (node, new _anglr_file_fragment_ (new SyntaxTreeToken ((int) AnglrDeclarations.tokens._name_value_pair_terminal_, -1, -1, ""), (_name_value_pair_) node), index)
        {
        }

        public override string Name => $"{((_name_value_pair_) node).m__identifier_.text}={((_name_value_pair_) node).m__cstring_.text}";

        public override int NodeCategory => (int) node.id;

        public override int NodeSubCategory => 0;

        public override void Body (StreamWriter writer, object obj)
        {
        }

        public override void Epilogue (StreamWriter writer, object obj)
        {
        }

        public override void Prologue (StreamWriter writer, object obj)
        {
            _name_value_pair_ _Name_Value_Pair_ = (_name_value_pair_) node;
            writer.WriteLine ($"{(string) obj}{_Name_Value_Pair_.m__identifier_.text} = {_Name_Value_Pair_.m__cstring_.text}");
        }
    }

    internal class anglr_file_STN : AnglrSyntaxTreeNode
    {
        public anglr_file_STN (SyntaxTreeBase node, int index) : base (node, new _anglr_file_fragment_ (new SyntaxTreeToken ((int) AnglrDeclarations.tokens._anglr_file_terminal_, -1, -1, ""), (_anglr_file_) node), index)
        {
        }

        public override string Name => "anglr file";

        public override int NodeCategory => (int) node.id;

        public override int NodeSubCategory => 0;

        public override void Body (StreamWriter writer, object obj)
        {
        }

        public override void Epilogue (StreamWriter writer, object obj)
        {
        }

        public override void Prologue (StreamWriter writer, object obj)
        {
        }
    }

    internal class anglr_file_part_list_STN : AnglrSyntaxTreeNode
    {
        public anglr_file_part_list_STN (SyntaxTreeBase node, int index) : base (node, new _anglr_file_fragment_ (new SyntaxTreeToken ((int) AnglrDeclarations.tokens._anglr_file_part_list_terminal_, -1, -1, ""), (_anglr_file_part_list_) node), index)
        {
        }

        public override string Name => throw new NotImplementedException ();

        public override int NodeCategory => throw new NotImplementedException ();

        public override int NodeSubCategory => throw new NotImplementedException ();

        public override void Body (StreamWriter writer, object obj)
        {
            throw new NotImplementedException ();
        }

        public override void Epilogue (StreamWriter writer, object obj)
        {
            throw new NotImplementedException ();
        }

        public override void Prologue (StreamWriter writer, object obj)
        {
            throw new NotImplementedException ();
        }
    }

    internal class anglr_file_part_STN : AnglrSyntaxTreeNode
    {
        public anglr_file_part_STN (SyntaxTreeBase node, int index) : base (node, new _anglr_file_fragment_ (new SyntaxTreeToken ((int) AnglrDeclarations.tokens._anglr_file_part_terminal_, -1, -1, ""), (_anglr_file_part_) node), index)
        {
        }

        public override string Name => throw new NotImplementedException ();

        public override int NodeCategory => throw new NotImplementedException ();

        public override int NodeSubCategory => throw new NotImplementedException ();

        public override void Body (StreamWriter writer, object obj)
        {
            throw new NotImplementedException ();
        }

        public override void Epilogue (StreamWriter writer, object obj)
        {
            throw new NotImplementedException ();
        }

        public override void Prologue (StreamWriter writer, object obj)
        {
            throw new NotImplementedException ();
        }
    }

    internal class general_part_STN : AnglrSyntaxTreeNode
    {
        public general_part_STN (SyntaxTreeBase node, int index) : base (node, new _anglr_file_fragment_ (new SyntaxTreeToken ((int) AnglrDeclarations.tokens._general_part_terminal_, -1, -1, ""), (_general_part_) node), index)
        {
        }

        public override string Name => ((_general_part_) node).m__identifier_.text;

        public override int NodeCategory => (int) node.id;

        public override int NodeSubCategory => 0;

        public override void Body (StreamWriter writer, object obj)
        {
        }

        public override void Epilogue (StreamWriter writer, object obj)
        {
            writer.WriteLine ($"{(string) obj}}}");
        }

        public override void Prologue (StreamWriter writer, object obj)
        {
            _general_part_ _General_Part_ = (_general_part_) node;
            writer.WriteLine ($"{(string) obj}{_General_Part_.m__identifier_.text}");
            writer.WriteLine ($"{(string) obj}{{");
        }
    }

    internal class declaration_part_STN : AnglrSyntaxTreeNode
    {
        public declaration_part_STN (SyntaxTreeBase node, int index) : base (node, new _anglr_file_fragment_ (new SyntaxTreeToken ((int) AnglrDeclarations.tokens._declaration_part_terminal_, -1, -1, ""), (_declaration_part_) node), index)
        {
        }

        public override string Name => ((_declaration_part_) node).m__identifier_.text;

        public override int NodeCategory => (int) node.id;

        public override int NodeSubCategory => 0;

        public override void Body (StreamWriter writer, object obj)
        {
        }

        public override void Epilogue (StreamWriter writer, object obj)
        {
            writer.WriteLine ($"{(string) obj}}}");
        }

        public override void Prologue (StreamWriter writer, object obj)
        {
            _declaration_part_ _Declaration_Part_ = (_declaration_part_) node;
            writer.WriteLine ($"{(string) obj}{_Declaration_Part_.m__identifier_.text}");
            writer.WriteLine ($"{(string) obj}{{");
        }
    }

    internal class anglr_definition_list_STN : AnglrSyntaxTreeNode
    {
        public anglr_definition_list_STN (SyntaxTreeBase node, int index) : base (node, new _anglr_file_fragment_ (new SyntaxTreeToken ((int) AnglrDeclarations.tokens._anglr_definition_list_terminal_, -1, -1, ""), (_anglr_definition_list_) node), index)
        {
        }

        public override string Name => throw new NotImplementedException ();

        public override int NodeCategory => throw new NotImplementedException ();

        public override int NodeSubCategory => throw new NotImplementedException ();

        public override void Body (StreamWriter writer, object obj)
        {
            throw new NotImplementedException ();
        }

        public override void Epilogue (StreamWriter writer, object obj)
        {
            throw new NotImplementedException ();
        }

        public override void Prologue (StreamWriter writer, object obj)
        {
            throw new NotImplementedException ();
        }
    }

    internal class anglr_definition_with_attribute_STN : AnglrSyntaxTreeNode
    {
        public anglr_definition_with_attribute_STN (SyntaxTreeBase node, int index, string name) : base (node, new _anglr_file_fragment_ (new SyntaxTreeToken ((int) AnglrDeclarations.tokens._anglr_definition_with_attribute_list_terminal_, -1, -1, ""), (_anglr_definition_with_attribute_) node), index)
        {
            this.name = name;
        }

        private string name;
        public override string Name => name;

        public override int NodeCategory => (int) node.id;

        public override int NodeSubCategory => 0;

        public override void Body (StreamWriter writer, object obj)
        {
        }

        public override void Epilogue (StreamWriter writer, object obj)
        {
        }

        public override void Prologue (StreamWriter writer, object obj)
        {
        }
    }

    internal class anglr_definition_STN : AnglrSyntaxTreeNode
    {
        public anglr_definition_STN (SyntaxTreeBase node, int index) : base (node, new _anglr_file_fragment_ (new SyntaxTreeToken ((int) AnglrDeclarations.tokens._anglr_definition_terminal_, -1, -1, ""), (_anglr_definition_) node), index)
        {
        }

        public override string Name => throw new NotImplementedException ();

        public override int NodeCategory => throw new NotImplementedException ();

        public override int NodeSubCategory => throw new NotImplementedException ();

        public override void Body (StreamWriter writer, object obj)
        {
            throw new NotImplementedException ();
        }

        public override void Epilogue (StreamWriter writer, object obj)
        {
            throw new NotImplementedException ();
        }

        public override void Prologue (StreamWriter writer, object obj)
        {
            throw new NotImplementedException ();
        }
    }

    internal class single_terminal_definition_STN : AnglrSyntaxTreeNode
    {
        public single_terminal_definition_STN (SyntaxTreeBase node, int index) : base (node, new _anglr_file_fragment_ (new SyntaxTreeToken ((int) AnglrDeclarations.tokens._single_terminal_definition_terminal_, -1, -1, ""), (_single_terminal_definition_) node), index)
        {
        }

        public override string Name => "single terminal";

        public override int NodeCategory => (int) node.id;

        public override int NodeSubCategory => 0;

        public override void Body (StreamWriter writer, object obj)
        {
        }

        public override void Epilogue (StreamWriter writer, object obj)
        {
        }

        public override void Prologue (StreamWriter writer, object obj)
        {
        }
    }

    internal class single_regex_definition_STN : AnglrSyntaxTreeNode
    {
        public single_regex_definition_STN (SyntaxTreeBase node, int index) : base (node, new _anglr_file_fragment_ (new SyntaxTreeToken ((int) AnglrDeclarations.tokens._single_regex_definition_terminal_, -1, -1, ""), (_single_regex_definition_) node), index)
        {
        }

        public override string Name => "single regex";

        public override int NodeCategory => (int) node.id;

        public override int NodeSubCategory => 0;

        public override void Body (StreamWriter writer, object obj)
        {
        }

        public override void Epilogue (StreamWriter writer, object obj)
        {
        }

        public override void Prologue (StreamWriter writer, object obj)
        {
        }
    }

    internal class block_of_terminal_definitions_STN : AnglrSyntaxTreeNode
    {
        public block_of_terminal_definitions_STN (SyntaxTreeBase node, int index) : base (node, new _anglr_file_fragment_ (new SyntaxTreeToken ((int) AnglrDeclarations.tokens._block_of_terminal_definitions_terminal_, -1, -1, ""), (_block_of_terminal_definitions_) node), index)
        {
        }

        public override string Name => "block of terminals";

        public override int NodeCategory => (int) node.id;

        public override int NodeSubCategory => 0;

        public override void Body (StreamWriter writer, object obj)
        {
        }

        public override void Epilogue (StreamWriter writer, object obj)
        {
            writer.WriteLine ($"{(string) obj}}}");
        }

        public override void Prologue (StreamWriter writer, object obj)
        {
            writer.WriteLine ($"{(string) obj}block of terminals");
            writer.WriteLine ($"{(string) obj}{{");
        }
    }

    internal class block_of_regex_definitions_STN : AnglrSyntaxTreeNode
    {
        public block_of_regex_definitions_STN (SyntaxTreeBase node, int index) : base (node, new _anglr_file_fragment_ (new SyntaxTreeToken ((int) AnglrDeclarations.tokens._block_of_regex_definitions_terminal_, -1, -1, ""), (_block_of_regex_definitions_) node), index)
        {
        }

        public override string Name => "block of regular expressions";

        public override int NodeCategory => (int) node.id;

        public override int NodeSubCategory => 0;

        public override void Body (StreamWriter writer, object obj)
        {
        }

        public override void Epilogue (StreamWriter writer, object obj)
        {
            writer.WriteLine ($"{(string) obj}}}");
        }

        public override void Prologue (StreamWriter writer, object obj)
        {
            writer.WriteLine ($"{(string) obj}block of regular expressions");
            writer.WriteLine ($"{(string) obj}{{");
        }
    }

    internal class terminal_definition_STN : AnglrSyntaxTreeNode
    {
        public terminal_definition_STN (SyntaxTreeBase node, int index) : base (node, new _anglr_file_fragment_ (new SyntaxTreeToken ((int) AnglrDeclarations.tokens._terminal_definition_terminal_, -1, -1, ""), (_terminal_definition_) node), index)
        {
        }

        public override string Name => ((_terminal_definition_) node).m__identifier_.text;

        public override int NodeCategory => (int) node.id;

        public override int NodeSubCategory => 0;

        public override void Body (StreamWriter writer, object obj)
        {
        }

        public override void Epilogue (StreamWriter writer, object obj)
        {
        }

        public override void Prologue (StreamWriter writer, object obj)
        {
            _terminal_definition_ _Terminal_Definition_ = (_terminal_definition_) node;
            writer.WriteLine ($"{(string) obj}{_Terminal_Definition_.m__identifier_.text}");
        }
    }

    internal class regex_definition_STN : AnglrSyntaxTreeNode
    {
        public regex_definition_STN (SyntaxTreeBase node, int index) : base (node, new _anglr_file_fragment_ (new SyntaxTreeToken ((int) AnglrDeclarations.tokens._regex_definition_terminal_, -1, -1, ""), (_regex_definition_) node), index)
        {
        }

        public override string Name => ((_regex_definition_) node).m__identifier_.text;

        public override int NodeCategory => (int) node.id;

        public override int NodeSubCategory => 0;

        public override void Body (StreamWriter writer, object obj)
        {
        }

        public override void Epilogue (StreamWriter writer, object obj)
        {
        }

        public override void Prologue (StreamWriter writer, object obj)
        {
            _regex_definition_ _Regex_Definition_ = (_regex_definition_) node;
            writer.WriteLine ($"{(string) obj}{_Regex_Definition_.m__identifier_.text}");
        }
    }

    internal class block_terminal_definitions_STN : AnglrSyntaxTreeNode
    {
        public block_terminal_definitions_STN (SyntaxTreeBase node, int index) : base (node, new _anglr_file_fragment_ (new SyntaxTreeToken ((int) AnglrDeclarations.tokens._block_terminal_definitions_terminal_, -1, -1, ""), (_block_terminal_definitions_) node), index)
        {
        }

        public override string Name => throw new NotImplementedException ();

        public override int NodeCategory => throw new NotImplementedException ();

        public override int NodeSubCategory => throw new NotImplementedException ();

        public override void Body (StreamWriter writer, object obj)
        {
            throw new NotImplementedException ();
        }

        public override void Epilogue (StreamWriter writer, object obj)
        {
            throw new NotImplementedException ();
        }

        public override void Prologue (StreamWriter writer, object obj)
        {
            throw new NotImplementedException ();
        }
    }

    internal class block_terminal_definition_STN : AnglrSyntaxTreeNode
    {
        public block_terminal_definition_STN (SyntaxTreeBase node, int index) : base (node, new _anglr_file_fragment_ (new SyntaxTreeToken ((int) AnglrDeclarations.tokens._block_terminal_definition_terminal_, -1, -1, ""), (_block_terminal_definition_) node), index)
        {
        }

        public override string Name => ((_block_terminal_definition_) node).m__terminal_definition_.m__identifier_.text;

        public override int NodeCategory => (int) node.id;

        public override int NodeSubCategory => 0;

        public override void Body (StreamWriter writer, object obj)
        {
            throw new NotImplementedException ();
        }

        public override void Epilogue (StreamWriter writer, object obj)
        {
            throw new NotImplementedException ();
        }

        public override void Prologue (StreamWriter writer, object obj)
        {
            throw new NotImplementedException ();
        }
    }

    internal class block_regex_definitions_STN : AnglrSyntaxTreeNode
    {
        public block_regex_definitions_STN (SyntaxTreeBase node, int index) : base (node, new _anglr_file_fragment_ (new SyntaxTreeToken ((int) AnglrDeclarations.tokens._block_regex_definitions_terminal_, -1, -1, ""), (_block_regex_definitions_) node), index)
        {
        }

        public override string Name => throw new NotImplementedException ();

        public override int NodeCategory => throw new NotImplementedException ();

        public override int NodeSubCategory => throw new NotImplementedException ();

        public override void Body (StreamWriter writer, object obj)
        {
            throw new NotImplementedException ();
        }

        public override void Epilogue (StreamWriter writer, object obj)
        {
            throw new NotImplementedException ();
        }

        public override void Prologue (StreamWriter writer, object obj)
        {
            throw new NotImplementedException ();
        }
    }

    internal class block_regex_definition_STN : AnglrSyntaxTreeNode
    {
        public block_regex_definition_STN (SyntaxTreeBase node, int index) : base (node, new _anglr_file_fragment_ (new SyntaxTreeToken ((int) AnglrDeclarations.tokens._block_regex_definition_terminal_, -1, -1, ""), (_block_regex_definition_) node), index)
        {
        }

        public override string Name => ((_block_regex_definition_) node).m__regex_definition_.m__identifier_.text;

        public override int NodeCategory => (int) node.id;

        public override int NodeSubCategory => 0;

        public override void Body (StreamWriter writer, object obj)
        {
            throw new NotImplementedException ();
        }

        public override void Epilogue (StreamWriter writer, object obj)
        {
            throw new NotImplementedException ();
        }

        public override void Prologue (StreamWriter writer, object obj)
        {
            throw new NotImplementedException ();
        }
    }

    internal class scanner_part_STN : AnglrSyntaxTreeNode
    {
        public scanner_part_STN (SyntaxTreeBase node, int index) : base (node, new _anglr_file_fragment_ (new SyntaxTreeToken ((int) AnglrDeclarations.tokens._scanner_part_terminal_, -1, -1, ""), (_scanner_part_) node), index)
        {
        }

        public override string Name => ((_scanner_part_) node).m__identifier_.text;

        public override int NodeCategory => (int) node.id;

        public override int NodeSubCategory => 0;

        public override void Body (StreamWriter writer, object obj)
        {
        }

        public override void Epilogue (StreamWriter writer, object obj)
        {
            writer.WriteLine ($"{(string) obj}}}");
        }

        public override void Prologue (StreamWriter writer, object obj)
        {
            _scanner_part_ _Scanner_Part_ = (_scanner_part_) node;
            writer.WriteLine ($"{(string) obj}{_Scanner_Part_.m__identifier_.text}");
            writer.WriteLine ($"{(string) obj}{{");
        }
    }

    internal class regular_expression_list_STN : AnglrSyntaxTreeNode
    {
        public regular_expression_list_STN (SyntaxTreeBase node, int index) : base (node, new _anglr_file_fragment_ (new SyntaxTreeToken ((int) AnglrDeclarations.tokens._regular_expression_list_terminal_, -1, -1, ""), (_regular_expression_list_) node), index)
        {
        }

        public override string Name => throw new NotImplementedException ();

        public override int NodeCategory => throw new NotImplementedException ();

        public override int NodeSubCategory => throw new NotImplementedException ();

        public override void Body (StreamWriter writer, object obj)
        {
            throw new NotImplementedException ();
        }

        public override void Epilogue (StreamWriter writer, object obj)
        {
            throw new NotImplementedException ();
        }

        public override void Prologue (StreamWriter writer, object obj)
        {
            throw new NotImplementedException ();
        }
    }

    internal class regular_expression_usage_STN : AnglrSyntaxTreeNode
    {
        public regular_expression_usage_STN (SyntaxTreeBase node, int index) : base (node, new _anglr_file_fragment_ (new SyntaxTreeToken ((int) AnglrDeclarations.tokens._regular_expression_usage_terminal_, -1, -1, ""), (_regular_expression_usage_) node), index)
        {
        }

        public override string Name => ((_regular_expression_usage_) node).m__regular_expression_.text;

        public override int NodeCategory => (int) node.id;

        public override int NodeSubCategory => 0;

        public override void Body (StreamWriter writer, object obj)
        {
        }

        public override void Epilogue (StreamWriter writer, object obj)
        {
        }

        public override void Prologue (StreamWriter writer, object obj)
        {
        }
    }

    internal class actions_STN : AnglrSyntaxTreeNode
    {
        public actions_STN (SyntaxTreeBase node, int index) : base (node, new _anglr_file_fragment_ (new SyntaxTreeToken ((int) AnglrDeclarations.tokens._actions_terminal_, -1, -1, ""), (_actions_) node), index)
        {
        }

        public override string Name => throw new NotImplementedException ();

        public override int NodeCategory => throw new NotImplementedException ();

        public override int NodeSubCategory => throw new NotImplementedException ();

        public override void Body (StreamWriter writer, object obj)
        {
            throw new NotImplementedException ();
        }

        public override void Epilogue (StreamWriter writer, object obj)
        {
            throw new NotImplementedException ();
        }

        public override void Prologue (StreamWriter writer, object obj)
        {
            throw new NotImplementedException ();
        }
    }

    internal class action_STN : AnglrSyntaxTreeNode
    {
        public action_STN (SyntaxTreeBase node, int index) : base (node, new _anglr_file_fragment_ (new SyntaxTreeToken ((int) AnglrDeclarations.tokens._action_terminal_, -1, -1, ""), (_action_) node), index)
        {
        }

        public override string Name => throw new NotImplementedException ();

        public override int NodeCategory => throw new NotImplementedException ();

        public override int NodeSubCategory => throw new NotImplementedException ();

        public override void Body (StreamWriter writer, object obj)
        {
            throw new NotImplementedException ();
        }

        public override void Epilogue (StreamWriter writer, object obj)
        {
            throw new NotImplementedException ();
        }

        public override void Prologue (StreamWriter writer, object obj)
        {
            throw new NotImplementedException ();
        }
    }

    internal class skip_action_STN : AnglrSyntaxTreeNode
    {
        public skip_action_STN (SyntaxTreeBase node, int index) : base (node, new _anglr_file_fragment_ (new SyntaxTreeToken ((int) AnglrDeclarations.tokens._skip_action_terminal_, -1, -1, ""), (_skip_action_) node), index)
        {
        }

        public override string Name => ((_skip_action_) node).m__skip_.text;

        public override int NodeCategory => (int) node.id;

        public override int NodeSubCategory => 0;

        public override void Body (StreamWriter writer, object obj)
        {
        }

        public override void Epilogue (StreamWriter writer, object obj)
        {
        }

        public override void Prologue (StreamWriter writer, object obj)
        {
        }
    }

    internal class terminal_action_STN : AnglrSyntaxTreeNode
    {
        public terminal_action_STN (SyntaxTreeBase node, int index) : base (node, new _anglr_file_fragment_ (new SyntaxTreeToken ((int) AnglrDeclarations.tokens._terminal_action_terminal_, -1, -1, ""), (_terminal_action_) node), index)
        {
        }

        public override string Name => $"{((_terminal_action_) node).m__ttoken_.text} {((_terminal_action_) node).m__identifier_.text}";

        public override int NodeCategory => (int) node.id;

        public override int NodeSubCategory => 0;

        public override void Body (StreamWriter writer, object obj)
        {
        }

        public override void Epilogue (StreamWriter writer, object obj)
        {
        }

        public override void Prologue (StreamWriter writer, object obj)
        {
        }
    }

    internal class event_action_STN : AnglrSyntaxTreeNode
    {
        public event_action_STN (SyntaxTreeBase node, int index) : base (node, new _anglr_file_fragment_ (new SyntaxTreeToken ((int) AnglrDeclarations.tokens._event_action_terminal_, -1, -1, ""), (_event_action_) node), index)
        {
        }

        public override string Name => $"{((_event_action_) node).m__event_.text} {((_event_action_) node).m__identifier_.text}";

        public override int NodeCategory => (int) node.id;

        public override int NodeSubCategory => 0;

        public override void Body (StreamWriter writer, object obj)
        {
        }

        public override void Epilogue (StreamWriter writer, object obj)
        {
        }

        public override void Prologue (StreamWriter writer, object obj)
        {
        }
    }

    internal class push_action_STN : AnglrSyntaxTreeNode
    {
        public push_action_STN (SyntaxTreeBase node, int index) : base (node, new _anglr_file_fragment_ (new SyntaxTreeToken ((int) AnglrDeclarations.tokens._push_action_terminal_, -1, -1, ""), (_push_action_) node), index)
        {
        }

        public override string Name => $"{((_push_action_) node).m__push_.text} {((_push_action_) node).m__identifier_.text}";

        public override int NodeCategory => (int) node.id;

        public override int NodeSubCategory => 0;

        public override void Body (StreamWriter writer, object obj)
        {
        }

        public override void Epilogue (StreamWriter writer, object obj)
        {
        }

        public override void Prologue (StreamWriter writer, object obj)
        {
        }
    }

    internal class pop_action_STN : AnglrSyntaxTreeNode
    {
        public pop_action_STN (SyntaxTreeBase node, int index) : base (node, new _anglr_file_fragment_ (new SyntaxTreeToken ((int) AnglrDeclarations.tokens._pop_action_terminal_, -1, -1, ""), (_pop_action_) node), index)
        {
        }

        public override string Name => ((_pop_action_) node).m__pop_.text;

        public override int NodeCategory => (int) node.id;

        public override int NodeSubCategory => 0;

        public override void Body (StreamWriter writer, object obj)
        {
        }

        public override void Epilogue (StreamWriter writer, object obj)
        {
        }

        public override void Prologue (StreamWriter writer, object obj)
        {
        }
    }

    internal class lexer_part_STN : AnglrSyntaxTreeNode
    {
        public lexer_part_STN (SyntaxTreeBase node, int index) : base (node, new _anglr_file_fragment_ (new SyntaxTreeToken ((int) AnglrDeclarations.tokens._lexer_part_terminal_, -1, -1, ""), (_lexer_part_) node), index)
        {
        }

        public override string Name => ((_lexer_part_) node).m__identifier_.text;

        public override int NodeCategory => (int) node.id;

        public override int NodeSubCategory => 0;

        public override void Body (StreamWriter writer, object obj)
        {
        }

        public override void Epilogue (StreamWriter writer, object obj)
        {
            writer.WriteLine ($"{(string) obj}}}");
        }

        public override void Prologue (StreamWriter writer, object obj)
        {
            _lexer_part_ _Lexer_Part_ = (_lexer_part_) node;
            writer.WriteLine ($"{(string) obj}{_Lexer_Part_.m__identifier_.text}");
            writer.WriteLine ($"{(string) obj}{{");
        }
    }

    internal class parser_part_STN : AnglrSyntaxTreeNode
    {
        public parser_part_STN (SyntaxTreeBase node, int index) : base (node, new _anglr_file_fragment_ (new SyntaxTreeToken ((int) AnglrDeclarations.tokens._parser_part_terminal_, -1, -1, ""), (_parser_part_) node), index)
        {
        }

        public override string Name => ((_parser_part_) node).m__identifier_.text;

        public override int NodeCategory => (int) node.id;

        public override int NodeSubCategory => 0;

        public override void Body (StreamWriter writer, object obj)
        {
        }

        public override void Epilogue (StreamWriter writer, object obj)
        {
            writer.WriteLine ($"{(string) obj}}}");
        }

        public override void Prologue (StreamWriter writer, object obj)
        {
            _parser_part_ _Parser_Part_ = (_parser_part_) node;
            writer.WriteLine ($"{(string) obj}{_Parser_Part_.m__identifier_.text}");
            writer.WriteLine ($"{(string) obj}{{");
        }
    }

    internal class anglr_syntax_rule_list_STN : AnglrSyntaxTreeNode
    {
        public anglr_syntax_rule_list_STN (SyntaxTreeBase node, int index) : base (node, new _anglr_file_fragment_ (new SyntaxTreeToken ((int) AnglrDeclarations.tokens._anglr_syntax_rule_list_terminal_, -1, -1, ""), (_anglr_syntax_rule_list_) node), index)
        {
        }

        public override string Name => throw new NotImplementedException ();

        public override int NodeCategory => throw new NotImplementedException ();

        public override int NodeSubCategory => throw new NotImplementedException ();

        public override void Body (StreamWriter writer, object obj)
        {
            throw new NotImplementedException ();
        }

        public override void Epilogue (StreamWriter writer, object obj)
        {
            throw new NotImplementedException ();
        }

        public override void Prologue (StreamWriter writer, object obj)
        {
            throw new NotImplementedException ();
        }
    }

    internal class anglr_syntax_rule_STN : AnglrSyntaxTreeNode
    {
        public anglr_syntax_rule_STN (SyntaxTreeBase node, int index) : base (node, new _anglr_file_fragment_ (new SyntaxTreeToken ((int) AnglrDeclarations.tokens._anglr_syntax_rule_terminal_, -1, -1, ""), (_anglr_syntax_rule_) node), index)
        {
        }

        public override string Name => ((_anglr_syntax_rule_) node).m__identifier_.text;

        public override int NodeCategory => (int) node.id;

        public override int NodeSubCategory => (int) ((_anglr_syntax_rule_) node).kind - 1;

        public override void Body (StreamWriter writer, object obj)
        {
        }

        public override void Epilogue (StreamWriter writer, object obj)
        {
            _anglr_syntax_rule_ _Anglr_Syntax_Rule_ = (_anglr_syntax_rule_) node;
            if (_Anglr_Syntax_Rule_.kind == (uint) _anglr_syntax_rule_.production_kind.g__anglr_syntax_rule__2)
                writer.WriteLine ($"{(string) obj}}}");
        }

        public override void Prologue (StreamWriter writer, object obj)
        {
            _anglr_syntax_rule_ _Anglr_Syntax_Rule_ = (_anglr_syntax_rule_) node;
            writer.WriteLine ($"{(string) obj}{_Anglr_Syntax_Rule_.m__identifier_.text}");
            if (_Anglr_Syntax_Rule_.kind == (uint) _anglr_syntax_rule_.production_kind.g__anglr_syntax_rule__2)
                writer.WriteLine ($"{(string) obj}{{");
        }
    }

    internal class anglr_nested_rule_STN : AnglrSyntaxTreeNode
    {
        public anglr_nested_rule_STN (SyntaxTreeBase node, int index) : base (node, new _anglr_file_fragment_ (new SyntaxTreeToken ((int) AnglrDeclarations.tokens._anglr_nested_rule_terminal_, -1, -1, ""), (_anglr_nested_rule_) node), index)
        {
        }

        public override string Name
        {
            get
            {
                _anglr_nested_rule_ _Anglr_Nested_Rule_ = (_anglr_nested_rule_) node;
                _anglr_syntax_production_list_name_optional_ _Anglr_Syntax_Production_List_Name_Optional_ = _Anglr_Nested_Rule_.m__anglr_syntax_production_list_name_optional_;
                return (_Anglr_Syntax_Production_List_Name_Optional_.m__anglr_syntax_production_list_name_ != null) ? _Anglr_Syntax_Production_List_Name_Optional_.m__anglr_syntax_production_list_name_.m__identifier_.text : "nested rule";
            }
        }

        public override int NodeCategory => (int) node.id;

        public override int NodeSubCategory => 0;

        public override void Body (StreamWriter writer, object obj)
        {
        }

        public override void Epilogue (StreamWriter writer, object obj)
        {
        }

        public override void Prologue (StreamWriter writer, object obj)
        {
        }
    }

    internal class anglr_syntax_production_list_name_STN : AnglrSyntaxTreeNode
    {
        public anglr_syntax_production_list_name_STN (SyntaxTreeBase node, int index) : base (node, new _anglr_file_fragment_ (new SyntaxTreeToken ((int) AnglrDeclarations.tokens._anglr_syntax_production_list_name_terminal_, -1, -1, ""), (_anglr_syntax_production_list_name_) node), index)
        {
        }

        public override string Name => throw new NotImplementedException ();

        public override int NodeCategory => throw new NotImplementedException ();

        public override int NodeSubCategory => throw new NotImplementedException ();

        public override void Body (StreamWriter writer, object obj)
        {
            throw new NotImplementedException ();
        }

        public override void Epilogue (StreamWriter writer, object obj)
        {
            throw new NotImplementedException ();
        }

        public override void Prologue (StreamWriter writer, object obj)
        {
            throw new NotImplementedException ();
        }
    }

    internal class anglr_syntax_production_list_STN : AnglrSyntaxTreeNode
    {
        public anglr_syntax_production_list_STN (SyntaxTreeBase node, int index) : base (node, new _anglr_file_fragment_ (new SyntaxTreeToken ((int) AnglrDeclarations.tokens._anglr_syntax_production_list_terminal_, -1, -1, ""), (_anglr_syntax_production_list_) node), index)
        {
        }

        public override string Name => throw new NotImplementedException ();

        public override int NodeCategory => throw new NotImplementedException ();

        public override int NodeSubCategory => throw new NotImplementedException ();

        public override void Body (StreamWriter writer, object obj)
        {
            throw new NotImplementedException ();
        }

        public override void Epilogue (StreamWriter writer, object obj)
        {
            throw new NotImplementedException ();
        }

        public override void Prologue (StreamWriter writer, object obj)
        {
            throw new NotImplementedException ();
        }
    }

    internal class anglr_syntax_production_STN : AnglrSyntaxTreeNode
    {
        public anglr_syntax_production_STN (SyntaxTreeBase node, int index) : base (node, new _anglr_file_fragment_ (new SyntaxTreeToken ((int) AnglrDeclarations.tokens._anglr_syntax_production_terminal_, -1, -1, ""), (_anglr_syntax_production_) node), index)
        {
        }

        public override string Name => throw new NotImplementedException ();

        public override int NodeCategory => throw new NotImplementedException ();

        public override int NodeSubCategory => throw new NotImplementedException ();

        public override void Body (StreamWriter writer, object obj)
        {
            throw new NotImplementedException ();
        }

        public override void Epilogue (StreamWriter writer, object obj)
        {
            throw new NotImplementedException ();
        }

        public override void Prologue (StreamWriter writer, object obj)
        {
            throw new NotImplementedException ();
        }
    }

    internal class production_name_STN : AnglrSyntaxTreeNode
    {
        public production_name_STN (SyntaxTreeBase node, int index) : base (node, new _anglr_file_fragment_ (new SyntaxTreeToken ((int) AnglrDeclarations.tokens._production_name_terminal_, -1, -1, ""), (_production_name_) node), index)
        {
        }

        public override string Name => throw new NotImplementedException ();

        public override int NodeCategory => throw new NotImplementedException ();

        public override int NodeSubCategory => throw new NotImplementedException ();

        public override void Body (StreamWriter writer, object obj)
        {
            throw new NotImplementedException ();
        }

        public override void Epilogue (StreamWriter writer, object obj)
        {
            throw new NotImplementedException ();
        }

        public override void Prologue (StreamWriter writer, object obj)
        {
            throw new NotImplementedException ();
        }
    }

    internal class priority_assoc_specification_STN : AnglrSyntaxTreeNode
    {
        public priority_assoc_specification_STN (SyntaxTreeBase node, int index) : base (node, new _anglr_file_fragment_ (new SyntaxTreeToken ((int) AnglrDeclarations.tokens._priority_assoc_specification_terminal_, -1, -1, ""), (_priority_assoc_specification_) node), index)
        {
        }

        public override string Name => throw new NotImplementedException ();

        public override int NodeCategory => throw new NotImplementedException ();

        public override int NodeSubCategory => throw new NotImplementedException ();

        public override void Body (StreamWriter writer, object obj)
        {
            throw new NotImplementedException ();
        }

        public override void Epilogue (StreamWriter writer, object obj)
        {
            throw new NotImplementedException ();
        }

        public override void Prologue (StreamWriter writer, object obj)
        {
            throw new NotImplementedException ();
        }
    }

    internal class associativity_specification_STN : AnglrSyntaxTreeNode
    {
        public associativity_specification_STN (SyntaxTreeBase node, int index) : base (node, new _anglr_file_fragment_ (new SyntaxTreeToken ((int) AnglrDeclarations.tokens._associativity_specification_terminal_, -1, -1, ""), (_associativity_specification_) node), index)
        {
        }

        public override string Name => throw new NotImplementedException ();

        public override int NodeCategory => throw new NotImplementedException ();

        public override int NodeSubCategory => throw new NotImplementedException ();

        public override void Body (StreamWriter writer, object obj)
        {
            throw new NotImplementedException ();
        }

        public override void Epilogue (StreamWriter writer, object obj)
        {
            throw new NotImplementedException ();
        }

        public override void Prologue (StreamWriter writer, object obj)
        {
            throw new NotImplementedException ();
        }
    }

    internal class priority_specification_STN : AnglrSyntaxTreeNode
    {
        public priority_specification_STN (SyntaxTreeBase node, int index) : base (node, new _anglr_file_fragment_ (new SyntaxTreeToken ((int) AnglrDeclarations.tokens._priority_specification_terminal_, -1, -1, ""), (_priority_specification_) node), index)
        {
        }

        public override string Name => throw new NotImplementedException ();

        public override int NodeCategory => throw new NotImplementedException ();

        public override int NodeSubCategory => throw new NotImplementedException ();

        public override void Body (StreamWriter writer, object obj)
        {
            throw new NotImplementedException ();
        }

        public override void Epilogue (StreamWriter writer, object obj)
        {
            throw new NotImplementedException ();
        }

        public override void Prologue (StreamWriter writer, object obj)
        {
            throw new NotImplementedException ();
        }
    }

    internal class name_list_STN : AnglrSyntaxTreeNode
    {
        public name_list_STN (SyntaxTreeBase node, int index) : base (node, new _anglr_file_fragment_ (new SyntaxTreeToken ((int) AnglrDeclarations.tokens._name_list_terminal_, -1, -1, ""), (_name_list_) node), index)
        {
        }

        public override string Name => throw new NotImplementedException ();

        public override int NodeCategory => throw new NotImplementedException ();

        public override int NodeSubCategory => throw new NotImplementedException ();

        public override void Body (StreamWriter writer, object obj)
        {
            throw new NotImplementedException ();
        }

        public override void Epilogue (StreamWriter writer, object obj)
        {
            throw new NotImplementedException ();
        }

        public override void Prologue (StreamWriter writer, object obj)
        {
            throw new NotImplementedException ();
        }
    }

    internal class marker_list_STN : AnglrSyntaxTreeNode
    {
        public marker_list_STN (SyntaxTreeBase node, int index) : base (node, new _anglr_file_fragment_ (new SyntaxTreeToken ((int) AnglrDeclarations.tokens._marker_list_terminal_, -1, -1, ""), (_marker_list_) node), index)
        {
        }

        public override string Name => throw new NotImplementedException ();

        public override int NodeCategory => throw new NotImplementedException ();

        public override int NodeSubCategory => throw new NotImplementedException ();

        public override void Body (StreamWriter writer, object obj)
        {
            throw new NotImplementedException ();
        }

        public override void Epilogue (StreamWriter writer, object obj)
        {
            throw new NotImplementedException ();
        }

        public override void Prologue (StreamWriter writer, object obj)
        {
            throw new NotImplementedException ();
        }
    }

    internal class marker_STN : AnglrSyntaxTreeNode
    {
        public marker_STN (SyntaxTreeBase node, int index) : base (node, new _anglr_file_fragment_ (new SyntaxTreeToken ((int) AnglrDeclarations.tokens._marker_terminal_, -1, -1, ""), (_marker_) node), index)
        {
        }

        public override string Name => throw new NotImplementedException ();

        public override int NodeCategory => throw new NotImplementedException ();

        public override int NodeSubCategory => throw new NotImplementedException ();

        public override void Body (StreamWriter writer, object obj)
        {
            throw new NotImplementedException ();
        }

        public override void Epilogue (StreamWriter writer, object obj)
        {
            throw new NotImplementedException ();
        }

        public override void Prologue (StreamWriter writer, object obj)
        {
            throw new NotImplementedException ();
        }
    }

    internal class g_name_STN : AnglrSyntaxTreeNode
    {
        public g_name_STN (SyntaxTreeBase node, int index) : base (node, new _anglr_file_fragment_ (new SyntaxTreeToken ((int) AnglrDeclarations.tokens._g_name_terminal_, -1, -1, ""), (_g_name_) node), index)
        {
        }

        public override string Name => throw new NotImplementedException ();

        public override int NodeCategory => throw new NotImplementedException ();

        public override int NodeSubCategory => throw new NotImplementedException ();

        public override void Body (StreamWriter writer, object obj)
        {
            throw new NotImplementedException ();
        }

        public override void Epilogue (StreamWriter writer, object obj)
        {
            throw new NotImplementedException ();
        }

        public override void Prologue (StreamWriter writer, object obj)
        {
            throw new NotImplementedException ();
        }
    }

    internal class name_STN : AnglrSyntaxTreeNode
    {
        public name_STN (SyntaxTreeBase node, int index) : base (node, new _anglr_file_fragment_ (new SyntaxTreeToken ((int) AnglrDeclarations.tokens._name_terminal_, -1, -1, ""), (_name_) node), index)
        {
        }

        public override string Name => throw new NotImplementedException ();

        public override int NodeCategory => throw new NotImplementedException ();

        public override int NodeSubCategory => throw new NotImplementedException ();

        public override void Body (StreamWriter writer, object obj)
        {
            throw new NotImplementedException ();
        }

        public override void Epilogue (StreamWriter writer, object obj)
        {
            throw new NotImplementedException ();
        }

        public override void Prologue (StreamWriter writer, object obj)
        {
            throw new NotImplementedException ();
        }
    }

    internal class cardinality_delimiter_STN : AnglrSyntaxTreeNode
    {
        public cardinality_delimiter_STN (SyntaxTreeBase node, int index) : base (node, new _anglr_file_fragment_ (new SyntaxTreeToken ((int) AnglrDeclarations.tokens._cardinality_delimiter_terminal_, -1, -1, ""), (_cardinality_delimiter_) node), index)
        {
        }

        public override string Name => throw new NotImplementedException ();

        public override int NodeCategory => throw new NotImplementedException ();

        public override int NodeSubCategory => throw new NotImplementedException ();

        public override void Body (StreamWriter writer, object obj)
        {
            throw new NotImplementedException ();
        }

        public override void Epilogue (StreamWriter writer, object obj)
        {
            throw new NotImplementedException ();
        }

        public override void Prologue (StreamWriter writer, object obj)
        {
            throw new NotImplementedException ();
        }
    }

    internal class cardinality_STN : AnglrSyntaxTreeNode
    {
        public cardinality_STN (SyntaxTreeBase node, int index) : base (node, new _anglr_file_fragment_ (new SyntaxTreeToken ((int) AnglrDeclarations.tokens._cardinality_terminal_, -1, -1, ""), (_cardinality_) node), index)
        {
        }

        public override string Name => throw new NotImplementedException ();

        public override int NodeCategory => throw new NotImplementedException ();

        public override int NodeSubCategory => throw new NotImplementedException ();

        public override void Body (StreamWriter writer, object obj)
        {
            throw new NotImplementedException ();
        }

        public override void Epilogue (StreamWriter writer, object obj)
        {
            throw new NotImplementedException ();
        }

        public override void Prologue (StreamWriter writer, object obj)
        {
            throw new NotImplementedException ();
        }
    }

    internal class delimiter_STN : AnglrSyntaxTreeNode
    {
        public delimiter_STN (SyntaxTreeBase node, int index) : base (node, new _anglr_file_fragment_ (new SyntaxTreeToken ((int) AnglrDeclarations.tokens._delimiter_terminal_, -1, -1, ""), (_delimiter_) node), index)
        {
        }

        public override string Name => throw new NotImplementedException ();

        public override int NodeCategory => throw new NotImplementedException ();

        public override int NodeSubCategory => throw new NotImplementedException ();

        public override void Body (StreamWriter writer, object obj)
        {
            throw new NotImplementedException ();
        }

        public override void Epilogue (StreamWriter writer, object obj)
        {
            throw new NotImplementedException ();
        }

        public override void Prologue (StreamWriter writer, object obj)
        {
            throw new NotImplementedException ();
        }
    }

    internal class NameIndex : SortedDictionary<AnglrStackChildren, AnglrSyntaxTreeNode>
    {
        public NameIndex () : base (new CmpIndex ()) { }
    }

    internal class AnglrNodeChildren : List<AnglrSyntaxTreeNode> { }
    internal class AnglrStackChildren : Stack<AnglrSyntaxTreeNode>
    {
        public AnglrStackChildren ()
        {
        }

        public AnglrStackChildren (int capacity) : base (capacity)
        {
        }

        public AnglrStackChildren (IEnumerable<AnglrSyntaxTreeNode> collection) : base (collection)
        {
        }
    }

    internal static class AglrSyntaxTreeExtensions
    {
        public static void CreateChildrenCollection (this SyntaxTreeBase node)
        {
            ((AppInfo) (node.appInfo = new AppInfo ())) [AppInfoType.Children] = new AnglrNodeChildren ();
        }
    }

    internal class AnglrSyntaxTreeGenerator : SyntaxTreeWalker
    {

        private AnglrLSPTarget anglrLSPTarget;
        private NameIndex names = new NameIndex ();
        private AnglrStackChildren nodes = new AnglrStackChildren ();

        public AnglrSyntaxTreeNode SyntaxTree { get; private set; } = null;

        internal AnglrSyntaxTreeGenerator (AnglrLSPTarget anglrLSPTarget)
        {
            this.anglrLSPTarget = anglrLSPTarget;
            Common_Event += AnglrSyntaxTreeGenerator_Common_Event;
            _anglr_file_fragment__Event += AnglrSyntaxTreeGenerator__anglr_file_fragment__Event;
            _attribute_list__Event += AnglrSyntaxTreeGenerator__attribute_list__Event;
            _attribute__Event += AnglrSyntaxTreeGenerator__attribute__Event;
            _name_value_pair__Event += AnglrSyntaxTreeGenerator__name_value_pair__Event;
            _anglr_file__Event += AnglrSyntaxTreeGenerator__anglr_file__Event;
            _general_part__Event += AnglrSyntaxTreeGenerator__general_part__Event;
            _declaration_part__Event += AnglrSyntaxTreeGenerator__declaration_part__Event;
            _anglr_definition_with_attribute__Event += AnglrSyntaxTreeGenerator__anglr_definition_with_attribute__Event;
            _single_terminal_definition__Event += AnglrSyntaxTreeGenerator__single_terminal_definition__Event;
            _single_regex_definition__Event += AnglrSyntaxTreeGenerator__single_regex_definition__Event;
            _block_of_terminal_definitions__Event += AnglrSyntaxTreeGenerator__block_of_terminal_definitions__Event;
            _block_of_regex_definitions__Event += AnglrSyntaxTreeGenerator__block_of_regex_definitions__Event;
            _terminal_definition__Event += AnglrSyntaxTreeGenerator__terminal_definition__Event;
            _regex_definition__Event += AnglrSyntaxTreeGenerator__regex_definition__Event;
            _block_terminal_definition__Event += AnglrSyntaxTreeGenerator__block_terminal_definition__Event;
            _block_regex_definition__Event += AnglrSyntaxTreeGenerator__block_regex_definition__Event;
            _scanner_part__Event += AnglrSyntaxTreeGenerator__scanner_part__Event;
            _regular_expression_usage__Event += AnglrSyntaxTreeGenerator__regular_expression_usage__Event;
            _skip_action__Event += AnglrSyntaxTreeGenerator__skip_action__Event;
            _terminal_action__Event += AnglrSyntaxTreeGenerator__terminal_action__Event;
            _event_action__Event += AnglrSyntaxTreeGenerator__event_action__Event;
            _push_action__Event += AnglrSyntaxTreeGenerator__push_action__Event;
            _pop_action__Event += AnglrSyntaxTreeGenerator__pop_action__Event;
            _lexer_part__Event += AnglrSyntaxTreeGenerator__lexer_part__Event;
            _parser_part__Event += AnglrSyntaxTreeGenerator__parser_part__Event;
            _anglr_syntax_rule__Event += AnglrSyntaxTreeGenerator__anglr_syntax_rule__Event;
            _anglr_nested_rule__Event += AnglrSyntaxTreeGenerator__anglr_nested_rule__Event;
        }

        private bool AnglrSyntaxTreeGenerator_Common_Event (SyntaxTreeCallbackReason reason, int kind, SyntaxTreeBase p_node)
        {
            switch (reason)
            {
            case SyntaxTreeCallbackReason.BuilderCallbackReason:
                break;
            case SyntaxTreeCallbackReason.TraversalPrologueCallbackReason:
                if (p_node is SyntaxTreeToken)
                    break;
                p_node.CreateChildrenCollection ();
                break;
            case SyntaxTreeCallbackReason.TraversalMidTermCallbackReason:
                break;
            case SyntaxTreeCallbackReason.TraversalEpilogueCallbackReason:
                break;
            }
            return true;
        }

        private bool AnglrSyntaxTreeGenerator__anglr_file_fragment__Event (SyntaxTreeCallbackReason reason, _anglr_file_fragment_.production_kind kind, _anglr_file_fragment_ p__anglr_file_fragment_)
        {
            bool status = true;
            switch (reason)
            {
            case SyntaxTreeCallbackReason.BuilderCallbackReason:
                break;
            case SyntaxTreeCallbackReason.TraversalPrologueCallbackReason:
                AnglrSyntaxTreeNode node = new anglr_file_fragment_STN (p__anglr_file_fragment_, 1);
                nodes.Push (node);
                break;
            case SyntaxTreeCallbackReason.TraversalMidTermCallbackReason:
                break;
            case SyntaxTreeCallbackReason.TraversalEpilogueCallbackReason:
                SyntaxTree = nodes.Pop ();
                break;
            }
            return status;
        }

        private bool AnglrSyntaxTreeGenerator__attribute_list__Event (SyntaxTreeCallbackReason reason, _attribute_list_.production_kind kind, _attribute_list_ p__attribute_list_)
        {
            bool status = true;
            switch (reason)
            {
            case SyntaxTreeCallbackReason.BuilderCallbackReason:
                break;
            case SyntaxTreeCallbackReason.IterationPrologueCallbackReason:
                AnglrSyntaxTreeNode parentNode = nodes.Peek ();
                AnglrSyntaxTreeNode node = new attribute_list_STN (p__attribute_list_, parentNode.Count + 1);
                parentNode.Add (node);
                nodes.Push (node);
                break;
            case SyntaxTreeCallbackReason.TraversalPrologueCallbackReason:
                break;
            case SyntaxTreeCallbackReason.TraversalMidTermCallbackReason:
                break;
            case SyntaxTreeCallbackReason.TraversalEpilogueCallbackReason:
                break;
            case SyntaxTreeCallbackReason.IterationEpilogueCallbackReason:
                nodes.Pop ();
                break;
            }
            return status;
        }

        private bool AnglrSyntaxTreeGenerator__attribute__Event (SyntaxTreeCallbackReason reason, _attribute_.production_kind kind, _attribute_ p__attribute_)
        {
            bool status = true;
            switch (reason)
            {
            case SyntaxTreeCallbackReason.BuilderCallbackReason:
                break;
            case SyntaxTreeCallbackReason.TraversalPrologueCallbackReason:
                AnglrSyntaxTreeNode parentNode = nodes.Peek ();
                AnglrSyntaxTreeNode node = new attribute_STN (p__attribute_, parentNode.Count + 1);
                parentNode.Add (node);
                nodes.Push (node);
                break;
            case SyntaxTreeCallbackReason.TraversalMidTermCallbackReason:
                break;
            case SyntaxTreeCallbackReason.TraversalEpilogueCallbackReason:
                nodes.Pop ();
                break;
            }
            return status;
        }

        private bool AnglrSyntaxTreeGenerator__name_value_pair__Event (SyntaxTreeCallbackReason reason, _name_value_pair_.production_kind kind, _name_value_pair_ p__name_value_pair_)
        {
            bool status = true;
            switch (reason)
            {
            case SyntaxTreeCallbackReason.BuilderCallbackReason:
                break;
            case SyntaxTreeCallbackReason.TraversalPrologueCallbackReason:
                AnglrSyntaxTreeNode parentNode = nodes.Peek ();
                AnglrSyntaxTreeNode node = new name_value_pair_STN (p__name_value_pair_, parentNode.Count + 1);
                parentNode.Add (node);
                nodes.Push (node);
                break;
            case SyntaxTreeCallbackReason.TraversalMidTermCallbackReason:
                break;
            case SyntaxTreeCallbackReason.TraversalEpilogueCallbackReason:
                nodes.Pop ();
                break;
            }
            return status;
        }

        private bool AnglrSyntaxTreeGenerator__anglr_file__Event (SyntaxTreeCallbackReason reason, _anglr_file_.production_kind kind, _anglr_file_ p__anglr_file_)
        {
            bool status = true;
            switch (reason)
            {
            case SyntaxTreeCallbackReason.BuilderCallbackReason:
                break;
            case SyntaxTreeCallbackReason.TraversalPrologueCallbackReason:
                AnglrSyntaxTreeNode parentNode = nodes.Peek ();
                AnglrSyntaxTreeNode node = new anglr_file_STN (p__anglr_file_, parentNode.Count + 1);
                parentNode.Add (node);
                nodes.Push (node);
                break;
            case SyntaxTreeCallbackReason.TraversalMidTermCallbackReason:
                break;
            case SyntaxTreeCallbackReason.TraversalEpilogueCallbackReason:
                nodes.Pop ();
                break;
            }
            return status;
        }

        private bool AnglrSyntaxTreeGenerator__general_part__Event (SyntaxTreeCallbackReason reason, _general_part_.production_kind kind, _general_part_ p__general_part_)
        {
            bool status = true;
            switch (reason)
            {
            case SyntaxTreeCallbackReason.BuilderCallbackReason:
                break;
            case SyntaxTreeCallbackReason.TraversalPrologueCallbackReason:
                AnglrSyntaxTreeNode parentNode = nodes.Peek ();
                AnglrSyntaxTreeNode node = new general_part_STN (p__general_part_, parentNode.Count + 1);
                parentNode.Add (node);
                nodes.Push (node);
                break;
            case SyntaxTreeCallbackReason.TraversalMidTermCallbackReason:
                break;
            case SyntaxTreeCallbackReason.TraversalEpilogueCallbackReason:
                nodes.Pop ();
                break;
            }
            return status;
        }

        private bool AnglrSyntaxTreeGenerator__declaration_part__Event (SyntaxTreeCallbackReason reason, _declaration_part_.production_kind kind, _declaration_part_ p__declaration_part_)
        {
            bool status = true;
            switch (reason)
            {
            case SyntaxTreeCallbackReason.BuilderCallbackReason:
                break;
            case SyntaxTreeCallbackReason.TraversalPrologueCallbackReason:
                AnglrSyntaxTreeNode parentNode = nodes.Peek ();
                AnglrSyntaxTreeNode node = new declaration_part_STN (p__declaration_part_, parentNode.Count + 1);
                parentNode.Add (node);
                nodes.Push (node);
                break;
            case SyntaxTreeCallbackReason.TraversalMidTermCallbackReason:
                break;
            case SyntaxTreeCallbackReason.TraversalEpilogueCallbackReason:
                nodes.Pop ();
                break;
            }
            return status;
        }

        private bool AnglrSyntaxTreeGenerator__anglr_definition_with_attribute__Event (SyntaxTreeCallbackReason reason, _anglr_definition_with_attribute_.production_kind kind, _anglr_definition_with_attribute_ p__anglr_definition_with_attribute_)
        {
            bool status = true;
            switch (reason)
            {
            case SyntaxTreeCallbackReason.BuilderCallbackReason:
                break;
            case SyntaxTreeCallbackReason.TraversalPrologueCallbackReason:
            {
                string name = "";
                _anglr_definition_ p__anglr_definition_ = p__anglr_definition_with_attribute_.m__anglr_definition_;
                switch ((_anglr_definition_.production_kind) p__anglr_definition_.kind)
                {
                case _anglr_definition_.production_kind.g__anglr_definition__1:
                    name = "single terminal definition";
                    break;
                case _anglr_definition_.production_kind.g__anglr_definition__2:
                    name = "single regex definition";
                    break;
                case _anglr_definition_.production_kind.g__anglr_definition__3:
                    name = "block of terminal definitions";
                    break;
                case _anglr_definition_.production_kind.g__anglr_definition__4:
                    name = "block of regex definitions";
                    break;
                }
                AnglrSyntaxTreeNode parentNode = nodes.Peek ();
                AnglrSyntaxTreeNode node = new anglr_definition_with_attribute_STN (p__anglr_definition_with_attribute_, parentNode.Count + 1, name);
                parentNode.Add (node);
                nodes.Push (node);
            }
            break;
            case SyntaxTreeCallbackReason.TraversalMidTermCallbackReason:
                break;
            case SyntaxTreeCallbackReason.TraversalEpilogueCallbackReason:
                nodes.Pop ();
                break;
            }
            return status;
        }

        private bool AnglrSyntaxTreeGenerator__single_terminal_definition__Event (SyntaxTreeCallbackReason reason, _single_terminal_definition_.production_kind kind, _single_terminal_definition_ p__single_terminal_definition_)
        {
            bool status = true;
            switch (reason)
            {
            case SyntaxTreeCallbackReason.BuilderCallbackReason:
                break;
            case SyntaxTreeCallbackReason.TraversalPrologueCallbackReason:
                AnglrSyntaxTreeNode parentNode = nodes.Peek ();
                //AnglrSyntaxTreeNode node = new single_terminal_definition_STN (p__single_terminal_definition_.parent.parent, parentNode.Count + 1);
                AnglrSyntaxTreeNode node = new single_terminal_definition_STN (p__single_terminal_definition_, parentNode.Count + 1);
                parentNode.Add (node);
                nodes.Push (node);
                break;
            case SyntaxTreeCallbackReason.TraversalMidTermCallbackReason:
                break;
            case SyntaxTreeCallbackReason.TraversalEpilogueCallbackReason:
                nodes.Pop ();
                break;
            }
            return status;
        }

        private bool AnglrSyntaxTreeGenerator__single_regex_definition__Event (SyntaxTreeCallbackReason reason, _single_regex_definition_.production_kind kind, _single_regex_definition_ p__single_regex_definition_)
        {
            bool status = true;
            switch (reason)
            {
            case SyntaxTreeCallbackReason.BuilderCallbackReason:
                break;
            case SyntaxTreeCallbackReason.TraversalPrologueCallbackReason:
                AnglrSyntaxTreeNode parentNode = nodes.Peek ();
                //AnglrSyntaxTreeNode node = new single_regex_definition_STN (p__single_regex_definition_.parent.parent, parentNode.Count + 1);
                AnglrSyntaxTreeNode node = new single_regex_definition_STN (p__single_regex_definition_, parentNode.Count + 1);
                parentNode.Add (node);
                nodes.Push (node);
                break;
            case SyntaxTreeCallbackReason.TraversalMidTermCallbackReason:
                break;
            case SyntaxTreeCallbackReason.TraversalEpilogueCallbackReason:
                nodes.Pop ();
                break;
            }
            return status;
        }

        private bool AnglrSyntaxTreeGenerator__block_of_terminal_definitions__Event (SyntaxTreeCallbackReason reason, _block_of_terminal_definitions_.production_kind kind, _block_of_terminal_definitions_ p__block_of_terminal_definitions_)
        {
            bool status = true;
            switch (reason)
            {
            case SyntaxTreeCallbackReason.BuilderCallbackReason:
                break;
            case SyntaxTreeCallbackReason.TraversalPrologueCallbackReason:
                AnglrSyntaxTreeNode parentNode = nodes.Peek ();
                //AnglrSyntaxTreeNode node = new block_of_terminal_definitions_STN (p__block_of_terminal_definitions_.parent.parent, parentNode.Count + 1);
                AnglrSyntaxTreeNode node = new block_of_terminal_definitions_STN (p__block_of_terminal_definitions_, parentNode.Count + 1);
                parentNode.Add (node);
                nodes.Push (node);
                break;
            case SyntaxTreeCallbackReason.TraversalMidTermCallbackReason:
                break;
            case SyntaxTreeCallbackReason.TraversalEpilogueCallbackReason:
                nodes.Pop ();
                break;
            }
            return status;
        }

        private bool AnglrSyntaxTreeGenerator__block_of_regex_definitions__Event (SyntaxTreeCallbackReason reason, _block_of_regex_definitions_.production_kind kind, _block_of_regex_definitions_ p__block_of_regex_definitions_)
        {
            bool status = true;
            switch (reason)
            {
            case SyntaxTreeCallbackReason.BuilderCallbackReason:
                break;
            case SyntaxTreeCallbackReason.TraversalPrologueCallbackReason:
                AnglrSyntaxTreeNode parentNode = nodes.Peek ();
                //AnglrSyntaxTreeNode node = new block_of_regex_definitions_STN (p__block_of_regex_definitions_.parent.parente, parentNode.Count + 1);
                AnglrSyntaxTreeNode node = new block_of_regex_definitions_STN (p__block_of_regex_definitions_, parentNode.Count + 1);
                parentNode.Add (node);
                nodes.Push (node);
                break;
            case SyntaxTreeCallbackReason.TraversalMidTermCallbackReason:
                break;
            case SyntaxTreeCallbackReason.TraversalEpilogueCallbackReason:
                nodes.Pop ();
                break;
            }
            return status;
        }

        private bool AnglrSyntaxTreeGenerator__terminal_definition__Event (SyntaxTreeCallbackReason reason, _terminal_definition_.production_kind kind, _terminal_definition_ p__terminal_definition_)
        {
            bool status = true;
            switch (reason)
            {
            case SyntaxTreeCallbackReason.BuilderCallbackReason:
                break;
            case SyntaxTreeCallbackReason.TraversalPrologueCallbackReason:
                AnglrSyntaxTreeNode parentNode = nodes.Peek ();
                AnglrSyntaxTreeNode node = new terminal_definition_STN (p__terminal_definition_, parentNode.Count + 1);
                parentNode.Add (node);
                nodes.Push (node);
                break;
            case SyntaxTreeCallbackReason.TraversalMidTermCallbackReason:
                break;
            case SyntaxTreeCallbackReason.TraversalEpilogueCallbackReason:
                nodes.Pop ();
                break;
            }
            return status;
        }

        private bool AnglrSyntaxTreeGenerator__regex_definition__Event (SyntaxTreeCallbackReason reason, _regex_definition_.production_kind kind, _regex_definition_ p__regex_definition_)
        {
            bool status = true;
            switch (reason)
            {
            case SyntaxTreeCallbackReason.BuilderCallbackReason:
                break;
            case SyntaxTreeCallbackReason.TraversalPrologueCallbackReason:
                AnglrSyntaxTreeNode parentNode = nodes.Peek ();
                AnglrSyntaxTreeNode node = new regex_definition_STN (p__regex_definition_, parentNode.Count + 1);
                parentNode.Add (node);
                nodes.Push (node);
                break;
            case SyntaxTreeCallbackReason.TraversalMidTermCallbackReason:
                break;
            case SyntaxTreeCallbackReason.TraversalEpilogueCallbackReason:
                nodes.Pop ();
                break;
            }
            return status;
        }

        private bool AnglrSyntaxTreeGenerator__block_terminal_definition__Event (SyntaxTreeCallbackReason reason, _block_terminal_definition_.production_kind kind, _block_terminal_definition_ p__block_terminal_definition_)
        {
            switch (reason)
            {
            case SyntaxTreeCallbackReason.BuilderCallbackReason:
                break;
            case SyntaxTreeCallbackReason.TraversalPrologueCallbackReason:
                AnglrSyntaxTreeNode parentNode = nodes.Peek ();
                AnglrSyntaxTreeNode node = new block_terminal_definition_STN (p__block_terminal_definition_, parentNode.Count + 1);
                parentNode.Add (node);
                nodes.Push (node);
                break;
            case SyntaxTreeCallbackReason.TraversalMidTermCallbackReason:
                break;
            case SyntaxTreeCallbackReason.TraversalEpilogueCallbackReason:
                nodes.Pop ();
                break;
            }
            return true;
        }

        private bool AnglrSyntaxTreeGenerator__block_regex_definition__Event (SyntaxTreeCallbackReason reason, _block_regex_definition_.production_kind kind, _block_regex_definition_ p__block_regex_definition_)
        {
            switch (reason)
            {
            case SyntaxTreeCallbackReason.BuilderCallbackReason:
                break;
            case SyntaxTreeCallbackReason.TraversalPrologueCallbackReason:
                AnglrSyntaxTreeNode parentNode = nodes.Peek ();
                AnglrSyntaxTreeNode node = new block_regex_definition_STN (p__block_regex_definition_, parentNode.Count + 1);
                parentNode.Add (node);
                nodes.Push (node);
                break;
            case SyntaxTreeCallbackReason.TraversalMidTermCallbackReason:
                break;
            case SyntaxTreeCallbackReason.TraversalEpilogueCallbackReason:
                nodes.Pop ();
                break;
            }
            return true;
        }

        private bool AnglrSyntaxTreeGenerator__scanner_part__Event (SyntaxTreeCallbackReason reason, _scanner_part_.production_kind kind, _scanner_part_ p__scanner_part_)
        {
            bool status = true;
            switch (reason)
            {
            case SyntaxTreeCallbackReason.BuilderCallbackReason:
                break;
            case SyntaxTreeCallbackReason.TraversalPrologueCallbackReason:
                AnglrSyntaxTreeNode parentNode = nodes.Peek ();
                AnglrSyntaxTreeNode node = new scanner_part_STN (p__scanner_part_, parentNode.Count + 1);
                parentNode.Add (node);
                nodes.Push (node);
                break;
            case SyntaxTreeCallbackReason.TraversalMidTermCallbackReason:
                break;
            case SyntaxTreeCallbackReason.TraversalEpilogueCallbackReason:
                nodes.Pop ();
                break;
            }
            return status;
        }

        private bool AnglrSyntaxTreeGenerator__regular_expression_usage__Event (SyntaxTreeCallbackReason reason, _regular_expression_usage_.production_kind kind, _regular_expression_usage_ p__regular_expression_usage_)
        {
            bool status = true;
            switch (reason)
            {
            case SyntaxTreeCallbackReason.BuilderCallbackReason:
                break;
            case SyntaxTreeCallbackReason.TraversalPrologueCallbackReason:
                AnglrSyntaxTreeNode parentNode = nodes.Peek ();
                AnglrSyntaxTreeNode node = new regular_expression_usage_STN (p__regular_expression_usage_, parentNode.Count + 1);
                parentNode.Add (node);
                nodes.Push (node);
                break;
            case SyntaxTreeCallbackReason.TraversalMidTermCallbackReason:
                break;
            case SyntaxTreeCallbackReason.TraversalEpilogueCallbackReason:
                nodes.Pop ();
                break;
            }
            return status;
        }

        private bool AnglrSyntaxTreeGenerator__skip_action__Event (SyntaxTreeCallbackReason reason, _skip_action_.production_kind kind, _skip_action_ p__skip_action_)
        {
            bool status = true;
            switch (reason)
            {
            case SyntaxTreeCallbackReason.BuilderCallbackReason:
                break;
            case SyntaxTreeCallbackReason.TraversalPrologueCallbackReason:
                AnglrSyntaxTreeNode parentNode = nodes.Peek ();
                AnglrSyntaxTreeNode node = new skip_action_STN (p__skip_action_, parentNode.Count + 1);
                parentNode.Add (node);
                nodes.Push (node);
                break;
            case SyntaxTreeCallbackReason.TraversalMidTermCallbackReason:
                break;
            case SyntaxTreeCallbackReason.TraversalEpilogueCallbackReason:
                nodes.Pop ();
                break;
            }
            return status;
        }

        private bool AnglrSyntaxTreeGenerator__terminal_action__Event (SyntaxTreeCallbackReason reason, _terminal_action_.production_kind kind, _terminal_action_ p__terminal_action_)
        {
            bool status = true;
            switch (reason)
            {
            case SyntaxTreeCallbackReason.BuilderCallbackReason:
                break;
            case SyntaxTreeCallbackReason.TraversalPrologueCallbackReason:
                AnglrSyntaxTreeNode parentNode = nodes.Peek ();
                AnglrSyntaxTreeNode node = new terminal_action_STN (p__terminal_action_, parentNode.Count + 1);
                parentNode.Add (node);
                nodes.Push (node);
                break;
            case SyntaxTreeCallbackReason.TraversalMidTermCallbackReason:
                break;
            case SyntaxTreeCallbackReason.TraversalEpilogueCallbackReason:
                nodes.Pop ();
                break;
            }
            return status;
        }

        private bool AnglrSyntaxTreeGenerator__event_action__Event (SyntaxTreeCallbackReason reason, _event_action_.production_kind kind, _event_action_ p__event_action_)
        {
            bool status = true;
            switch (reason)
            {
            case SyntaxTreeCallbackReason.BuilderCallbackReason:
                break;
            case SyntaxTreeCallbackReason.TraversalPrologueCallbackReason:
                AnglrSyntaxTreeNode parentNode = nodes.Peek ();
                AnglrSyntaxTreeNode node = new event_action_STN (p__event_action_, parentNode.Count + 1);
                parentNode.Add (node);
                nodes.Push (node);
                break;
            case SyntaxTreeCallbackReason.TraversalMidTermCallbackReason:
                break;
            case SyntaxTreeCallbackReason.TraversalEpilogueCallbackReason:
                nodes.Pop ();
                break;
            }
            return status;
        }

        private bool AnglrSyntaxTreeGenerator__push_action__Event (SyntaxTreeCallbackReason reason, _push_action_.production_kind kind, _push_action_ p__push_action_)
        {
            bool status = true;
            switch (reason)
            {
            case SyntaxTreeCallbackReason.BuilderCallbackReason:
                break;
            case SyntaxTreeCallbackReason.TraversalPrologueCallbackReason:
                AnglrSyntaxTreeNode parentNode = nodes.Peek ();
                AnglrSyntaxTreeNode node = new push_action_STN (p__push_action_, parentNode.Count + 1);
                parentNode.Add (node);
                nodes.Push (node);
                break;
            case SyntaxTreeCallbackReason.TraversalMidTermCallbackReason:
                break;
            case SyntaxTreeCallbackReason.TraversalEpilogueCallbackReason:
                nodes.Pop ();
                break;
            }
            return status;
        }

        private bool AnglrSyntaxTreeGenerator__pop_action__Event (SyntaxTreeCallbackReason reason, _pop_action_.production_kind kind, _pop_action_ p__pop_action_)
        {
            bool status = true;
            switch (reason)
            {
            case SyntaxTreeCallbackReason.BuilderCallbackReason:
                break;
            case SyntaxTreeCallbackReason.TraversalPrologueCallbackReason:
                AnglrSyntaxTreeNode parentNode = nodes.Peek ();
                AnglrSyntaxTreeNode node = new pop_action_STN (p__pop_action_, parentNode.Count + 1);
                parentNode.Add (node);
                nodes.Push (node);
                break;
            case SyntaxTreeCallbackReason.TraversalMidTermCallbackReason:
                break;
            case SyntaxTreeCallbackReason.TraversalEpilogueCallbackReason:
                nodes.Pop ();
                break;
            }
            return status;
        }

        private bool AnglrSyntaxTreeGenerator__lexer_part__Event (SyntaxTreeCallbackReason reason, _lexer_part_.production_kind kind, _lexer_part_ p__lexer_part_)
        {
            bool status = true;
            switch (reason)
            {
            case SyntaxTreeCallbackReason.BuilderCallbackReason:
                break;
            case SyntaxTreeCallbackReason.TraversalPrologueCallbackReason:
                AnglrSyntaxTreeNode parentNode = nodes.Peek ();
                AnglrSyntaxTreeNode node = new lexer_part_STN (p__lexer_part_, parentNode.Count + 1);
                parentNode.Add (node);
                nodes.Push (node);
                break;
            case SyntaxTreeCallbackReason.TraversalMidTermCallbackReason:
                break;
            case SyntaxTreeCallbackReason.TraversalEpilogueCallbackReason:
                nodes.Pop ();
                break;
            }
            return status;
        }

        private bool AnglrSyntaxTreeGenerator__parser_part__Event (SyntaxTreeCallbackReason reason, _parser_part_.production_kind kind, _parser_part_ p__parser_part_)
        {
            bool status = true;
            switch (reason)
            {
            case SyntaxTreeCallbackReason.BuilderCallbackReason:
                break;
            case SyntaxTreeCallbackReason.TraversalPrologueCallbackReason:
                AnglrSyntaxTreeNode parentNode = nodes.Peek ();
                AnglrSyntaxTreeNode node = new parser_part_STN (p__parser_part_, parentNode.Count + 1);
                parentNode.Add (node);
                nodes.Push (node);
                break;
            case SyntaxTreeCallbackReason.TraversalMidTermCallbackReason:
                break;
            case SyntaxTreeCallbackReason.TraversalEpilogueCallbackReason:
                nodes.Pop ();
                break;
            }
            return status;
        }

        private bool AnglrSyntaxTreeGenerator__anglr_syntax_rule__Event (SyntaxTreeCallbackReason reason, _anglr_syntax_rule_.production_kind kind, _anglr_syntax_rule_ p__anglr_syntax_rule_)
        {
            bool status = true;
            switch (reason)
            {
            case SyntaxTreeCallbackReason.BuilderCallbackReason:
                break;
            case SyntaxTreeCallbackReason.TraversalPrologueCallbackReason:
                AnglrSyntaxTreeNode parentNode = nodes.Peek ();
                AnglrSyntaxTreeNode node = new anglr_syntax_rule_STN (p__anglr_syntax_rule_, parentNode.Count + 1);
                parentNode.Add (node);
                nodes.Push (node);
                break;
            case SyntaxTreeCallbackReason.TraversalMidTermCallbackReason:
                break;
            case SyntaxTreeCallbackReason.TraversalEpilogueCallbackReason:
                nodes.Pop ();
                break;
            }
            return status;
        }

        private bool AnglrSyntaxTreeGenerator__anglr_nested_rule__Event (SyntaxTreeCallbackReason reason, _anglr_nested_rule_.production_kind kind, _anglr_nested_rule_ p__anglr_nested_rule_)
        {
            bool status = true;
            switch (reason)
            {
            case SyntaxTreeCallbackReason.BuilderCallbackReason:
                break;
            case SyntaxTreeCallbackReason.TraversalPrologueCallbackReason:
                AnglrSyntaxTreeNode parentNode = nodes.Peek ();
                AnglrSyntaxTreeNode node = new anglr_nested_rule_STN (p__anglr_nested_rule_, parentNode.Count + 1);
                parentNode.Add (node);
                nodes.Push (node);
                break;
            case SyntaxTreeCallbackReason.TraversalMidTermCallbackReason:
                break;
            case SyntaxTreeCallbackReason.TraversalEpilogueCallbackReason:
                nodes.Pop ();
                break;
            }
            return status;
        }

        public void GenerateNames (AnglrSyntaxTreeNode node)
        {
            _GenerateNames (new AnglrStackChildren (), node);
        }

        private void _GenerateNames (AnglrStackChildren nameStack, AnglrSyntaxTreeNode node)
        {
            nameStack.Push (node);
            {
                try
                {
                    names [new AnglrStackChildren (nameStack)] = node;
                }
                catch (Exception ex)
                {
                    Console.WriteLine (ex.Message);
                }
            }
            foreach (var child in (AnglrNodeChildren) ((AppInfo) node.node.appInfo) [AppInfoType.Children])
                _GenerateNames (nameStack, child);
            nameStack.Pop ();
        }

        public void GenerateSyntaxTree (_anglr_file_fragment_ p_node)
        {
            TraverseCommon (p_node);
            Traverse (p_node);
        }

        public AnglrSyntaxTreeNode FindNode (string path)
        {
            path = path.Trim (new char [] { '.' });
            return _FindNode (this.SyntaxTree, path.Split (new char [] { '.' }), 0);
        }

        public AnglrSyntaxTreeNode _FindNode (AnglrSyntaxTreeNode node, string [] path, int index)
        {
            if (index >= path.Length)
                return node;
            string [] pair = path [index].Split (new char [] { '-' });
            foreach (var item in (AnglrNodeChildren) ((AppInfo) node.node.appInfo) [AppInfoType.Children])
            {
                if (item.node.id != uint.Parse (pair [0]))
                    continue;
                if (item.index != int.Parse (pair [1]))
                    continue;
                return _FindNode (item, path, index + 1);
            }
            return null;
        }

        public AnglrNodeChildren FindChildren (string path)
        {
            path = path.Trim (new char [] { '.' });
            if ((path == null) || (path.Length == 0))
                return _FindChildren (SyntaxTree, new string [] { }, 0);
            return _FindChildren (SyntaxTree, path.Split (new char [] { '.' }), 0);
        }

        public AnglrNodeChildren _FindChildren (AnglrSyntaxTreeNode node, string [] path, int index)
        {
            if (index >= path.Length)
                return (AnglrNodeChildren) ((AppInfo) node.node.appInfo) [AppInfoType.Children];
            string [] pair = path [index].Split (new char [] { '-' });
            if (node.node.appInfo == null)
                return null;
            foreach (var item in (AnglrNodeChildren) ((AppInfo) node.node.appInfo) [AppInfoType.Children])
            {
                if (item.node.id != uint.Parse (pair [0]))
                    continue;
                if (item.index != int.Parse (pair [1]))
                    continue;
                return _FindChildren (item, path, index + 1);
            }
            return null;
        }
    }

    internal class AnglrTokenNavigationException : Exception
    {

    }

    internal class AnglrTokenNavigatorInfo : SyntaxTreeWalker
    {
        public int LineNo { get; private set; } = -1;
        public int Column { get; private set; } = -1;

        private _anglr_file_fragment_ fragment = null;

        internal AnglrTokenNavigatorInfo ()
        {
            Common_Event += AnglrTokenNavigatorInfo_Common_Event;
            //_attribute_list__Event += (reason, kind, node) => { return true; };
        }

        private bool AnglrTokenNavigatorInfo_Common_Event (SyntaxTreeCallbackReason reason, int kind, SyntaxTreeBase p_node)
        {
            switch (reason)
            {
            case SyntaxTreeCallbackReason.BuilderCallbackReason:
                break;
            case SyntaxTreeCallbackReason.TraversalPrologueCallbackReason:
            {
                if ((fragment.kind != (uint) _anglr_file_fragment_.production_kind.g__anglr_file_fragment__1) && (p_node is _attribute_list_))
                    return false;
                if (!(p_node is SyntaxTreeToken))
                    break;
                SyntaxTreeToken token = p_node as SyntaxTreeToken;
                if (((LineNo = token.lineno) < 0) || ((Column = token.column) < 0))
                    break;
                throw new AnglrTokenNavigationException ();
            }
            break;
            case SyntaxTreeCallbackReason.TraversalMidTermCallbackReason:
                break;
            case SyntaxTreeCallbackReason.TraversalEpilogueCallbackReason:
                break;
            }
            return true;
        }

        public void GetNavigationInfo (_anglr_file_fragment_ fragment)
        {
            LineNo = -1;
            Column = -1;
            this.fragment = fragment;
            try
            {
                TraverseCommon (fragment);
            }
            catch (AnglrTokenNavigationException)
            {

            }
        }
    }
}
