//
//	This file was generated with ANGLR compiler
//
using System;

using Anglr.Parser.Core;
using Anglr.Parser.SyntaxTree;
using Anglr.Parser.Walker;
namespace Anglr.Parser
{
	//
	// class associated with syntax rule <anglr file fragment>
	//

	public class	_anglr_file_fragment_ : SyntaxTreeBase
	{
		#region enumerated production(s) of syntax rule <anglr file fragment>
		public enum production_kind : ushort	// enumerated production(s) of syntax rule <anglr file fragment>
		{
			g__anglr_file_fragment__1 = 1,	// <attribute list terminal> <attribute list>

			g__anglr_file_fragment__2 = 2,	// <attribute terminal> <attribute>

			g__anglr_file_fragment__3 = 3,	// <name value list terminal> <name value list>

			g__anglr_file_fragment__4 = 4,	// <name value pair terminal> <name value pair>

			g__anglr_file_fragment__5 = 5,	// <anglr file terminal> <anglr file>

			g__anglr_file_fragment__6 = 6,	// <anglr file part list terminal> <anglr file part list>

			g__anglr_file_fragment__7 = 7,	// <anglr file part terminal> <anglr file part>

			g__anglr_file_fragment__8 = 8,	// <general part terminal> <general part>

			g__anglr_file_fragment__9 = 9,	// <declaration part terminal> <declaration part>

			g__anglr_file_fragment__10 = 10,	// <anglr definition list terminal> <anglr definition list>

			g__anglr_file_fragment__11 = 11,	// <anglr definition with attribute list terminal> <anglr definition with attribute>

			g__anglr_file_fragment__12 = 12,	// <anglr definition terminal> <anglr definition>

			g__anglr_file_fragment__13 = 13,	// <single terminal definition terminal> <single terminal definition>

			g__anglr_file_fragment__14 = 14,	// <single regex definition terminal> <single regex definition>

			g__anglr_file_fragment__15 = 15,	// <block of terminal definitions terminal> <block of terminal definitions>

			g__anglr_file_fragment__16 = 16,	// <block of regex definitions terminal> <block of regex definitions>

			g__anglr_file_fragment__17 = 17,	// <terminal definition terminal> <terminal definition>

			g__anglr_file_fragment__18 = 18,	// <regex definition terminal> <regex definition>

			g__anglr_file_fragment__19 = 19,	// <block terminal definitions terminal> <block terminal definitions>

			g__anglr_file_fragment__20 = 20,	// <block terminal definition terminal> <block terminal definition>

			g__anglr_file_fragment__21 = 21,	// <block regex definitions terminal> <block regex definitions>

			g__anglr_file_fragment__22 = 22,	// <block regex definition terminal> <block regex definition>

			g__anglr_file_fragment__23 = 23,	// <scanner part terminal> <scanner part>

			g__anglr_file_fragment__24 = 24,	// <regular expression list terminal> <regular expression list>

			g__anglr_file_fragment__25 = 25,	// <regular expression usage terminal> <regular expression usage>

			g__anglr_file_fragment__26 = 26,	// <actions terminal> <actions>

			g__anglr_file_fragment__27 = 27,	// <action terminal> <action>

			g__anglr_file_fragment__28 = 28,	// <skip action terminal> <skip action>

			g__anglr_file_fragment__29 = 29,	// <terminal action terminal> <terminal action>

			g__anglr_file_fragment__30 = 30,	// <event action terminal> <event action>

			g__anglr_file_fragment__31 = 31,	// <push action terminal> <push action>

			g__anglr_file_fragment__32 = 32,	// <pop action terminal> <pop action>

			g__anglr_file_fragment__33 = 33,	// <lexer part terminal> <lexer part>

			g__anglr_file_fragment__34 = 34,	// <parser part terminal> <parser part>

			g__anglr_file_fragment__35 = 35,	// <anglr syntax rule list terminal> <anglr syntax rule list>

			g__anglr_file_fragment__36 = 36,	// <anglr syntax rule terminal> <anglr syntax rule>

			g__anglr_file_fragment__37 = 37,	// <anglr nested rule terminal> <anglr nested rule>

			g__anglr_file_fragment__38 = 38,	// <anglr syntax production list name terminal> <anglr syntax production list name>

			g__anglr_file_fragment__39 = 39,	// <anglr syntax production list terminal> <anglr syntax production list>

			g__anglr_file_fragment__40 = 40,	// <anglr syntax production terminal> <anglr syntax production>

			g__anglr_file_fragment__41 = 41,	// <priority assoc specification terminal> <priority assoc specification>

			g__anglr_file_fragment__42 = 42,	// <priority specification terminal> <priority specification>

			g__anglr_file_fragment__43 = 43,	// <associativity specification terminal> <associativity specification>

			g__anglr_file_fragment__44 = 44,	// <production name terminal> <production name>

			g__anglr_file_fragment__45 = 45,	// <name list terminal> <name list>

			g__anglr_file_fragment__46 = 46,	// <marker list terminal> <marker list>

			g__anglr_file_fragment__47 = 47,	// <marker terminal> <marker>

			g__anglr_file_fragment__48 = 48,	// <g name terminal> <g name>

			g__anglr_file_fragment__49 = 49,	// <name terminal> <name>

			g__anglr_file_fragment__50 = 50,	// <cardinality delimiter terminal> <cardinality delimiter>

			g__anglr_file_fragment__51 = 51,	// <cardinality terminal> <cardinality>

			g__anglr_file_fragment__52 = 52,	// <delimiter terminal> <delimiter>

			g__anglr_file_fragment__53 = 53,	// <attribute list optional terminal> <attribute list optional>

			g__anglr_file_fragment__54 = 54,	// <name value list optional terminal> <name value list optional>

			g__anglr_file_fragment__55 = 55,	// <anglr file part list optional terminal> <anglr file part list optional>

			g__anglr_file_fragment__56 = 56,	// <anglr definition list optional terminal> <anglr definition list optional>

			g__anglr_file_fragment__57 = 57,	// <block terminal definitions optional terminal> <block terminal definitions optional>

			g__anglr_file_fragment__58 = 58,	// <block regex definitions optional terminal> <block regex definitions optional>

			g__anglr_file_fragment__59 = 59,	// <regular expression list optional terminal> <regular expression list optional>

			g__anglr_file_fragment__60 = 60,	// <actions optional terminal> <actions optional>

			g__anglr_file_fragment__61 = 61,	// <anglr syntax rule list optional terminal> <anglr syntax rule list optional>

			g__anglr_file_fragment__62 = 62,	// <anglr syntax production list name optional terminal> <anglr syntax production list name optional>

			g__anglr_file_fragment__63 = 63,	// <production name optional terminal> <production name optional>

			g__anglr_file_fragment__64 = 64,	// <priority assoc specification optional terminal> <priority assoc specification optional>

			g__anglr_file_fragment__65 = 65,	// <marker list optional terminal> <marker list optional>

			g__anglr_file_fragment__66 = 66,	// <delimiter optional terminal> <delimiter optional>

			g__anglr_file_fragment__67 = 67,	// <cstring optional terminal> <cstring optional>

			g__anglr_file_fragment__68 = 68,	// <number optional terminal> <number optional>

		};
		#endregion
		#region production markers associated with the syntax rule <anglr file fragment>

		// markers associated with production: <anglr file fragment> -> <attribute list terminal> <attribute list>

		public enum production_marker_1 : ushort
		{
			m__attribute_list_,
			final
		};

		// markers associated with production: <anglr file fragment> -> <attribute terminal> <attribute>

		public enum production_marker_2 : ushort
		{
			m__attribute_,
			final
		};

		// markers associated with production: <anglr file fragment> -> <name value list terminal> <name value list>

		public enum production_marker_3 : ushort
		{
			m__name_value_list_,
			final
		};

		// markers associated with production: <anglr file fragment> -> <name value pair terminal> <name value pair>

		public enum production_marker_4 : ushort
		{
			m__name_value_pair_,
			final
		};

		// markers associated with production: <anglr file fragment> -> <anglr file terminal> <anglr file>

		public enum production_marker_5 : ushort
		{
			m__anglr_file_,
			final
		};

		// markers associated with production: <anglr file fragment> -> <anglr file part list terminal> <anglr file part list>

		public enum production_marker_6 : ushort
		{
			m__anglr_file_part_list_,
			final
		};

		// markers associated with production: <anglr file fragment> -> <anglr file part terminal> <anglr file part>

		public enum production_marker_7 : ushort
		{
			m__anglr_file_part_,
			final
		};

		// markers associated with production: <anglr file fragment> -> <general part terminal> <general part>

		public enum production_marker_8 : ushort
		{
			m__general_part_,
			final
		};

		// markers associated with production: <anglr file fragment> -> <declaration part terminal> <declaration part>

		public enum production_marker_9 : ushort
		{
			m__declaration_part_,
			final
		};

		// markers associated with production: <anglr file fragment> -> <anglr definition list terminal> <anglr definition list>

		public enum production_marker_10 : ushort
		{
			m__anglr_definition_list_,
			final
		};

		// markers associated with production: <anglr file fragment> -> <anglr definition with attribute list terminal> <anglr definition with attribute>

		public enum production_marker_11 : ushort
		{
			m__anglr_definition_with_attribute_,
			final
		};

		// markers associated with production: <anglr file fragment> -> <anglr definition terminal> <anglr definition>

		public enum production_marker_12 : ushort
		{
			m__anglr_definition_,
			final
		};

		// markers associated with production: <anglr file fragment> -> <single terminal definition terminal> <single terminal definition>

		public enum production_marker_13 : ushort
		{
			m__single_terminal_definition_,
			final
		};

		// markers associated with production: <anglr file fragment> -> <single regex definition terminal> <single regex definition>

		public enum production_marker_14 : ushort
		{
			m__single_regex_definition_,
			final
		};

		// markers associated with production: <anglr file fragment> -> <block of terminal definitions terminal> <block of terminal definitions>

		public enum production_marker_15 : ushort
		{
			m__block_of_terminal_definitions_,
			final
		};

		// markers associated with production: <anglr file fragment> -> <block of regex definitions terminal> <block of regex definitions>

		public enum production_marker_16 : ushort
		{
			m__block_of_regex_definitions_,
			final
		};

		// markers associated with production: <anglr file fragment> -> <terminal definition terminal> <terminal definition>

		public enum production_marker_17 : ushort
		{
			m__terminal_definition_,
			final
		};

		// markers associated with production: <anglr file fragment> -> <regex definition terminal> <regex definition>

		public enum production_marker_18 : ushort
		{
			m__regex_definition_,
			final
		};

		// markers associated with production: <anglr file fragment> -> <block terminal definitions terminal> <block terminal definitions>

		public enum production_marker_19 : ushort
		{
			m__block_terminal_definitions_,
			final
		};

		// markers associated with production: <anglr file fragment> -> <block terminal definition terminal> <block terminal definition>

		public enum production_marker_20 : ushort
		{
			m__block_terminal_definition_,
			final
		};

		// markers associated with production: <anglr file fragment> -> <block regex definitions terminal> <block regex definitions>

		public enum production_marker_21 : ushort
		{
			m__block_regex_definitions_,
			final
		};

		// markers associated with production: <anglr file fragment> -> <block regex definition terminal> <block regex definition>

		public enum production_marker_22 : ushort
		{
			m__block_regex_definition_,
			final
		};

		// markers associated with production: <anglr file fragment> -> <scanner part terminal> <scanner part>

		public enum production_marker_23 : ushort
		{
			m__scanner_part_,
			final
		};

		// markers associated with production: <anglr file fragment> -> <regular expression list terminal> <regular expression list>

		public enum production_marker_24 : ushort
		{
			m__regular_expression_list_,
			final
		};

		// markers associated with production: <anglr file fragment> -> <regular expression usage terminal> <regular expression usage>

		public enum production_marker_25 : ushort
		{
			m__regular_expression_usage_,
			final
		};

		// markers associated with production: <anglr file fragment> -> <actions terminal> <actions>

		public enum production_marker_26 : ushort
		{
			m__actions_,
			final
		};

		// markers associated with production: <anglr file fragment> -> <action terminal> <action>

		public enum production_marker_27 : ushort
		{
			m__action_,
			final
		};

		// markers associated with production: <anglr file fragment> -> <skip action terminal> <skip action>

		public enum production_marker_28 : ushort
		{
			m__skip_action_,
			final
		};

		// markers associated with production: <anglr file fragment> -> <terminal action terminal> <terminal action>

		public enum production_marker_29 : ushort
		{
			m__terminal_action_,
			final
		};

		// markers associated with production: <anglr file fragment> -> <event action terminal> <event action>

		public enum production_marker_30 : ushort
		{
			m__event_action_,
			final
		};

		// markers associated with production: <anglr file fragment> -> <push action terminal> <push action>

		public enum production_marker_31 : ushort
		{
			m__push_action_,
			final
		};

		// markers associated with production: <anglr file fragment> -> <pop action terminal> <pop action>

		public enum production_marker_32 : ushort
		{
			m__pop_action_,
			final
		};

		// markers associated with production: <anglr file fragment> -> <lexer part terminal> <lexer part>

		public enum production_marker_33 : ushort
		{
			m__lexer_part_,
			final
		};

		// markers associated with production: <anglr file fragment> -> <parser part terminal> <parser part>

		public enum production_marker_34 : ushort
		{
			m__parser_part_,
			final
		};

		// markers associated with production: <anglr file fragment> -> <anglr syntax rule list terminal> <anglr syntax rule list>

		public enum production_marker_35 : ushort
		{
			m__anglr_syntax_rule_list_,
			final
		};

		// markers associated with production: <anglr file fragment> -> <anglr syntax rule terminal> <anglr syntax rule>

		public enum production_marker_36 : ushort
		{
			m__anglr_syntax_rule_,
			final
		};

		// markers associated with production: <anglr file fragment> -> <anglr nested rule terminal> <anglr nested rule>

		public enum production_marker_37 : ushort
		{
			m__anglr_nested_rule_,
			final
		};

		// markers associated with production: <anglr file fragment> -> <anglr syntax production list name terminal> <anglr syntax production list name>

		public enum production_marker_38 : ushort
		{
			m__anglr_syntax_production_list_name_,
			final
		};

		// markers associated with production: <anglr file fragment> -> <anglr syntax production list terminal> <anglr syntax production list>

		public enum production_marker_39 : ushort
		{
			m__anglr_syntax_production_list_,
			final
		};

		// markers associated with production: <anglr file fragment> -> <anglr syntax production terminal> <anglr syntax production>

		public enum production_marker_40 : ushort
		{
			m__anglr_syntax_production_,
			final
		};

		// markers associated with production: <anglr file fragment> -> <priority assoc specification terminal> <priority assoc specification>

		public enum production_marker_41 : ushort
		{
			m__priority_assoc_specification_,
			final
		};

		// markers associated with production: <anglr file fragment> -> <priority specification terminal> <priority specification>

		public enum production_marker_42 : ushort
		{
			m__priority_specification_,
			final
		};

		// markers associated with production: <anglr file fragment> -> <associativity specification terminal> <associativity specification>

		public enum production_marker_43 : ushort
		{
			m__associativity_specification_,
			final
		};

		// markers associated with production: <anglr file fragment> -> <production name terminal> <production name>

		public enum production_marker_44 : ushort
		{
			m__production_name_,
			final
		};

		// markers associated with production: <anglr file fragment> -> <name list terminal> <name list>

		public enum production_marker_45 : ushort
		{
			m__name_list_,
			final
		};

		// markers associated with production: <anglr file fragment> -> <marker list terminal> <marker list>

		public enum production_marker_46 : ushort
		{
			m__marker_list_,
			final
		};

		// markers associated with production: <anglr file fragment> -> <marker terminal> <marker>

		public enum production_marker_47 : ushort
		{
			m__marker_,
			final
		};

		// markers associated with production: <anglr file fragment> -> <g name terminal> <g name>

		public enum production_marker_48 : ushort
		{
			m__g_name_,
			final
		};

		// markers associated with production: <anglr file fragment> -> <name terminal> <name>

		public enum production_marker_49 : ushort
		{
			m__name_,
			final
		};

		// markers associated with production: <anglr file fragment> -> <cardinality delimiter terminal> <cardinality delimiter>

		public enum production_marker_50 : ushort
		{
			m__cardinality_delimiter_,
			final
		};

		// markers associated with production: <anglr file fragment> -> <cardinality terminal> <cardinality>

		public enum production_marker_51 : ushort
		{
			m__cardinality_,
			final
		};

		// markers associated with production: <anglr file fragment> -> <delimiter terminal> <delimiter>

		public enum production_marker_52 : ushort
		{
			m__delimiter_,
			final
		};

		// markers associated with production: <anglr file fragment> -> <attribute list optional terminal> <attribute list optional>

		public enum production_marker_53 : ushort
		{
			m__attribute_list_optional_,
			final
		};

		// markers associated with production: <anglr file fragment> -> <name value list optional terminal> <name value list optional>

		public enum production_marker_54 : ushort
		{
			m__name_value_list_optional_,
			final
		};

		// markers associated with production: <anglr file fragment> -> <anglr file part list optional terminal> <anglr file part list optional>

		public enum production_marker_55 : ushort
		{
			m__anglr_file_part_list_optional_,
			final
		};

		// markers associated with production: <anglr file fragment> -> <anglr definition list optional terminal> <anglr definition list optional>

		public enum production_marker_56 : ushort
		{
			m__anglr_definition_list_optional_,
			final
		};

		// markers associated with production: <anglr file fragment> -> <block terminal definitions optional terminal> <block terminal definitions optional>

		public enum production_marker_57 : ushort
		{
			m__block_terminal_definitions_optional_,
			final
		};

		// markers associated with production: <anglr file fragment> -> <block regex definitions optional terminal> <block regex definitions optional>

		public enum production_marker_58 : ushort
		{
			m__block_regex_definitions_optional_,
			final
		};

		// markers associated with production: <anglr file fragment> -> <regular expression list optional terminal> <regular expression list optional>

		public enum production_marker_59 : ushort
		{
			m__regular_expression_list_optional_,
			final
		};

		// markers associated with production: <anglr file fragment> -> <actions optional terminal> <actions optional>

		public enum production_marker_60 : ushort
		{
			m__actions_optional_,
			final
		};

		// markers associated with production: <anglr file fragment> -> <anglr syntax rule list optional terminal> <anglr syntax rule list optional>

		public enum production_marker_61 : ushort
		{
			m__anglr_syntax_rule_list_optional_,
			final
		};

		// markers associated with production: <anglr file fragment> -> <anglr syntax production list name optional terminal> <anglr syntax production list name optional>

		public enum production_marker_62 : ushort
		{
			m__anglr_syntax_production_list_name_optional_,
			final
		};

		// markers associated with production: <anglr file fragment> -> <production name optional terminal> <production name optional>

		public enum production_marker_63 : ushort
		{
			m__production_name_optional_,
			final
		};

		// markers associated with production: <anglr file fragment> -> <priority assoc specification optional terminal> <priority assoc specification optional>

		public enum production_marker_64 : ushort
		{
			m__priority_assoc_specification_optional_,
			final
		};

		// markers associated with production: <anglr file fragment> -> <marker list optional terminal> <marker list optional>

		public enum production_marker_65 : ushort
		{
			m__marker_list_optional_,
			final
		};

		// markers associated with production: <anglr file fragment> -> <delimiter optional terminal> <delimiter optional>

		public enum production_marker_66 : ushort
		{
			m__delimiter_optional_,
			final
		};

		// markers associated with production: <anglr file fragment> -> <cstring optional terminal> <cstring optional>

		public enum production_marker_67 : ushort
		{
			m__cstring_optional_,
			final
		};

		// markers associated with production: <anglr file fragment> -> <number optional terminal> <number optional>

		public enum production_marker_68 : ushort
		{
			m__number_optional_,
			final
		};
		#endregion
		#region Constructors and destructors associated with the syntax rule <anglr file fragment>

		// Constructor declaration(s) associated with production(s) of syntax rule <anglr file fragment>

		//
		// Constructor associated with the following production(s)
		// <anglr file fragment> -> <attribute list terminal> <attribute list>

		//

		public _anglr_file_fragment_ (SyntaxTreeToken p_token, _attribute_list_ p__attribute_list_) : base ((uint) ProductionID.__anglr_file_fragment__ID, (uint) production_kind.g__anglr_file_fragment__1)
		{
			++g_counter;
			_init ();
			children = (SyntaxTreeBase []) Array.CreateInstance (typeof (SyntaxTreeBase), 2);
			children[0] = m__attribute_list_terminal_ = p_token;
			children[1] = m__attribute_list_ = p__attribute_list_;
		}

		//
		// Constructor associated with the following production(s)
		// <anglr file fragment> -> <attribute terminal> <attribute>

		//

		public _anglr_file_fragment_ (SyntaxTreeToken p_token, _attribute_ p__attribute_) : base ((uint) ProductionID.__anglr_file_fragment__ID, (uint) production_kind.g__anglr_file_fragment__2)
		{
			++g_counter;
			_init ();
			children = (SyntaxTreeBase []) Array.CreateInstance (typeof (SyntaxTreeBase), 2);
			children[0] = m__attribute_terminal_ = p_token;
			children[1] = m__attribute_ = p__attribute_;
		}

		//
		// Constructor associated with the following production(s)
		// <anglr file fragment> -> <name value list terminal> <name value list>

		//

		public _anglr_file_fragment_ (SyntaxTreeToken p_token, _name_value_list_ p__name_value_list_) : base ((uint) ProductionID.__anglr_file_fragment__ID, (uint) production_kind.g__anglr_file_fragment__3)
		{
			++g_counter;
			_init ();
			children = (SyntaxTreeBase []) Array.CreateInstance (typeof (SyntaxTreeBase), 2);
			children[0] = m__name_value_list_terminal_ = p_token;
			children[1] = m__name_value_list_ = p__name_value_list_;
		}

		//
		// Constructor associated with the following production(s)
		// <anglr file fragment> -> <name value pair terminal> <name value pair>

		//

		public _anglr_file_fragment_ (SyntaxTreeToken p_token, _name_value_pair_ p__name_value_pair_) : base ((uint) ProductionID.__anglr_file_fragment__ID, (uint) production_kind.g__anglr_file_fragment__4)
		{
			++g_counter;
			_init ();
			children = (SyntaxTreeBase []) Array.CreateInstance (typeof (SyntaxTreeBase), 2);
			children[0] = m__name_value_pair_terminal_ = p_token;
			children[1] = m__name_value_pair_ = p__name_value_pair_;
		}

		//
		// Constructor associated with the following production(s)
		// <anglr file fragment> -> <anglr file terminal> <anglr file>

		//

		public _anglr_file_fragment_ (SyntaxTreeToken p_token, _anglr_file_ p__anglr_file_) : base ((uint) ProductionID.__anglr_file_fragment__ID, (uint) production_kind.g__anglr_file_fragment__5)
		{
			++g_counter;
			_init ();
			children = (SyntaxTreeBase []) Array.CreateInstance (typeof (SyntaxTreeBase), 2);
			children[0] = m__anglr_file_terminal_ = p_token;
			children[1] = m__anglr_file_ = p__anglr_file_;
		}

		//
		// Constructor associated with the following production(s)
		// <anglr file fragment> -> <anglr file part list terminal> <anglr file part list>

		//

		public _anglr_file_fragment_ (SyntaxTreeToken p_token, _anglr_file_part_list_ p__anglr_file_part_list_) : base ((uint) ProductionID.__anglr_file_fragment__ID, (uint) production_kind.g__anglr_file_fragment__6)
		{
			++g_counter;
			_init ();
			children = (SyntaxTreeBase []) Array.CreateInstance (typeof (SyntaxTreeBase), 2);
			children[0] = m__anglr_file_part_list_terminal_ = p_token;
			children[1] = m__anglr_file_part_list_ = p__anglr_file_part_list_;
		}

		//
		// Constructor associated with the following production(s)
		// <anglr file fragment> -> <anglr file part terminal> <anglr file part>

		//

		public _anglr_file_fragment_ (SyntaxTreeToken p_token, _anglr_file_part_ p__anglr_file_part_) : base ((uint) ProductionID.__anglr_file_fragment__ID, (uint) production_kind.g__anglr_file_fragment__7)
		{
			++g_counter;
			_init ();
			children = (SyntaxTreeBase []) Array.CreateInstance (typeof (SyntaxTreeBase), 2);
			children[0] = m__anglr_file_part_terminal_ = p_token;
			children[1] = m__anglr_file_part_ = p__anglr_file_part_;
		}

		//
		// Constructor associated with the following production(s)
		// <anglr file fragment> -> <general part terminal> <general part>

		//

		public _anglr_file_fragment_ (SyntaxTreeToken p_token, _general_part_ p__general_part_) : base ((uint) ProductionID.__anglr_file_fragment__ID, (uint) production_kind.g__anglr_file_fragment__8)
		{
			++g_counter;
			_init ();
			children = (SyntaxTreeBase []) Array.CreateInstance (typeof (SyntaxTreeBase), 2);
			children[0] = m__general_part_terminal_ = p_token;
			children[1] = m__general_part_ = p__general_part_;
		}

		//
		// Constructor associated with the following production(s)
		// <anglr file fragment> -> <declaration part terminal> <declaration part>

		//

		public _anglr_file_fragment_ (SyntaxTreeToken p_token, _declaration_part_ p__declaration_part_) : base ((uint) ProductionID.__anglr_file_fragment__ID, (uint) production_kind.g__anglr_file_fragment__9)
		{
			++g_counter;
			_init ();
			children = (SyntaxTreeBase []) Array.CreateInstance (typeof (SyntaxTreeBase), 2);
			children[0] = m__declaration_part_terminal_ = p_token;
			children[1] = m__declaration_part_ = p__declaration_part_;
		}

		//
		// Constructor associated with the following production(s)
		// <anglr file fragment> -> <anglr definition list terminal> <anglr definition list>

		//

		public _anglr_file_fragment_ (SyntaxTreeToken p_token, _anglr_definition_list_ p__anglr_definition_list_) : base ((uint) ProductionID.__anglr_file_fragment__ID, (uint) production_kind.g__anglr_file_fragment__10)
		{
			++g_counter;
			_init ();
			children = (SyntaxTreeBase []) Array.CreateInstance (typeof (SyntaxTreeBase), 2);
			children[0] = m__anglr_definition_list_terminal_ = p_token;
			children[1] = m__anglr_definition_list_ = p__anglr_definition_list_;
		}

		//
		// Constructor associated with the following production(s)
		// <anglr file fragment> -> <anglr definition with attribute list terminal> <anglr definition with attribute>

		//

		public _anglr_file_fragment_ (SyntaxTreeToken p_token, _anglr_definition_with_attribute_ p__anglr_definition_with_attribute_) : base ((uint) ProductionID.__anglr_file_fragment__ID, (uint) production_kind.g__anglr_file_fragment__11)
		{
			++g_counter;
			_init ();
			children = (SyntaxTreeBase []) Array.CreateInstance (typeof (SyntaxTreeBase), 2);
			children[0] = m__anglr_definition_with_attribute_list_terminal_ = p_token;
			children[1] = m__anglr_definition_with_attribute_ = p__anglr_definition_with_attribute_;
		}

		//
		// Constructor associated with the following production(s)
		// <anglr file fragment> -> <anglr definition terminal> <anglr definition>

		//

		public _anglr_file_fragment_ (SyntaxTreeToken p_token, _anglr_definition_ p__anglr_definition_) : base ((uint) ProductionID.__anglr_file_fragment__ID, (uint) production_kind.g__anglr_file_fragment__12)
		{
			++g_counter;
			_init ();
			children = (SyntaxTreeBase []) Array.CreateInstance (typeof (SyntaxTreeBase), 2);
			children[0] = m__anglr_definition_terminal_ = p_token;
			children[1] = m__anglr_definition_ = p__anglr_definition_;
		}

		//
		// Constructor associated with the following production(s)
		// <anglr file fragment> -> <single terminal definition terminal> <single terminal definition>

		//

		public _anglr_file_fragment_ (SyntaxTreeToken p_token, _single_terminal_definition_ p__single_terminal_definition_) : base ((uint) ProductionID.__anglr_file_fragment__ID, (uint) production_kind.g__anglr_file_fragment__13)
		{
			++g_counter;
			_init ();
			children = (SyntaxTreeBase []) Array.CreateInstance (typeof (SyntaxTreeBase), 2);
			children[0] = m__single_terminal_definition_terminal_ = p_token;
			children[1] = m__single_terminal_definition_ = p__single_terminal_definition_;
		}

		//
		// Constructor associated with the following production(s)
		// <anglr file fragment> -> <single regex definition terminal> <single regex definition>

		//

		public _anglr_file_fragment_ (SyntaxTreeToken p_token, _single_regex_definition_ p__single_regex_definition_) : base ((uint) ProductionID.__anglr_file_fragment__ID, (uint) production_kind.g__anglr_file_fragment__14)
		{
			++g_counter;
			_init ();
			children = (SyntaxTreeBase []) Array.CreateInstance (typeof (SyntaxTreeBase), 2);
			children[0] = m__single_regex_definition_terminal_ = p_token;
			children[1] = m__single_regex_definition_ = p__single_regex_definition_;
		}

		//
		// Constructor associated with the following production(s)
		// <anglr file fragment> -> <block of terminal definitions terminal> <block of terminal definitions>

		//

		public _anglr_file_fragment_ (SyntaxTreeToken p_token, _block_of_terminal_definitions_ p__block_of_terminal_definitions_) : base ((uint) ProductionID.__anglr_file_fragment__ID, (uint) production_kind.g__anglr_file_fragment__15)
		{
			++g_counter;
			_init ();
			children = (SyntaxTreeBase []) Array.CreateInstance (typeof (SyntaxTreeBase), 2);
			children[0] = m__block_of_terminal_definitions_terminal_ = p_token;
			children[1] = m__block_of_terminal_definitions_ = p__block_of_terminal_definitions_;
		}

		//
		// Constructor associated with the following production(s)
		// <anglr file fragment> -> <block of regex definitions terminal> <block of regex definitions>

		//

		public _anglr_file_fragment_ (SyntaxTreeToken p_token, _block_of_regex_definitions_ p__block_of_regex_definitions_) : base ((uint) ProductionID.__anglr_file_fragment__ID, (uint) production_kind.g__anglr_file_fragment__16)
		{
			++g_counter;
			_init ();
			children = (SyntaxTreeBase []) Array.CreateInstance (typeof (SyntaxTreeBase), 2);
			children[0] = m__block_of_regex_definitions_terminal_ = p_token;
			children[1] = m__block_of_regex_definitions_ = p__block_of_regex_definitions_;
		}

		//
		// Constructor associated with the following production(s)
		// <anglr file fragment> -> <terminal definition terminal> <terminal definition>

		//

		public _anglr_file_fragment_ (SyntaxTreeToken p_token, _terminal_definition_ p__terminal_definition_) : base ((uint) ProductionID.__anglr_file_fragment__ID, (uint) production_kind.g__anglr_file_fragment__17)
		{
			++g_counter;
			_init ();
			children = (SyntaxTreeBase []) Array.CreateInstance (typeof (SyntaxTreeBase), 2);
			children[0] = m__terminal_definition_terminal_ = p_token;
			children[1] = m__terminal_definition_ = p__terminal_definition_;
		}

		//
		// Constructor associated with the following production(s)
		// <anglr file fragment> -> <regex definition terminal> <regex definition>

		//

		public _anglr_file_fragment_ (SyntaxTreeToken p_token, _regex_definition_ p__regex_definition_) : base ((uint) ProductionID.__anglr_file_fragment__ID, (uint) production_kind.g__anglr_file_fragment__18)
		{
			++g_counter;
			_init ();
			children = (SyntaxTreeBase []) Array.CreateInstance (typeof (SyntaxTreeBase), 2);
			children[0] = m__regex_definition_terminal_ = p_token;
			children[1] = m__regex_definition_ = p__regex_definition_;
		}

		//
		// Constructor associated with the following production(s)
		// <anglr file fragment> -> <block terminal definitions terminal> <block terminal definitions>

		//

		public _anglr_file_fragment_ (SyntaxTreeToken p_token, _block_terminal_definitions_ p__block_terminal_definitions_) : base ((uint) ProductionID.__anglr_file_fragment__ID, (uint) production_kind.g__anglr_file_fragment__19)
		{
			++g_counter;
			_init ();
			children = (SyntaxTreeBase []) Array.CreateInstance (typeof (SyntaxTreeBase), 2);
			children[0] = m__block_terminal_definitions_terminal_ = p_token;
			children[1] = m__block_terminal_definitions_ = p__block_terminal_definitions_;
		}

		//
		// Constructor associated with the following production(s)
		// <anglr file fragment> -> <block terminal definition terminal> <block terminal definition>

		//

		public _anglr_file_fragment_ (SyntaxTreeToken p_token, _block_terminal_definition_ p__block_terminal_definition_) : base ((uint) ProductionID.__anglr_file_fragment__ID, (uint) production_kind.g__anglr_file_fragment__20)
		{
			++g_counter;
			_init ();
			children = (SyntaxTreeBase []) Array.CreateInstance (typeof (SyntaxTreeBase), 2);
			children[0] = m__block_terminal_definition_terminal_ = p_token;
			children[1] = m__block_terminal_definition_ = p__block_terminal_definition_;
		}

		//
		// Constructor associated with the following production(s)
		// <anglr file fragment> -> <block regex definitions terminal> <block regex definitions>

		//

		public _anglr_file_fragment_ (SyntaxTreeToken p_token, _block_regex_definitions_ p__block_regex_definitions_) : base ((uint) ProductionID.__anglr_file_fragment__ID, (uint) production_kind.g__anglr_file_fragment__21)
		{
			++g_counter;
			_init ();
			children = (SyntaxTreeBase []) Array.CreateInstance (typeof (SyntaxTreeBase), 2);
			children[0] = m__block_regex_definitions_terminal_ = p_token;
			children[1] = m__block_regex_definitions_ = p__block_regex_definitions_;
		}

		//
		// Constructor associated with the following production(s)
		// <anglr file fragment> -> <block regex definition terminal> <block regex definition>

		//

		public _anglr_file_fragment_ (SyntaxTreeToken p_token, _block_regex_definition_ p__block_regex_definition_) : base ((uint) ProductionID.__anglr_file_fragment__ID, (uint) production_kind.g__anglr_file_fragment__22)
		{
			++g_counter;
			_init ();
			children = (SyntaxTreeBase []) Array.CreateInstance (typeof (SyntaxTreeBase), 2);
			children[0] = m__block_regex_definition_terminal_ = p_token;
			children[1] = m__block_regex_definition_ = p__block_regex_definition_;
		}

		//
		// Constructor associated with the following production(s)
		// <anglr file fragment> -> <scanner part terminal> <scanner part>

		//

		public _anglr_file_fragment_ (SyntaxTreeToken p_token, _scanner_part_ p__scanner_part_) : base ((uint) ProductionID.__anglr_file_fragment__ID, (uint) production_kind.g__anglr_file_fragment__23)
		{
			++g_counter;
			_init ();
			children = (SyntaxTreeBase []) Array.CreateInstance (typeof (SyntaxTreeBase), 2);
			children[0] = m__scanner_part_terminal_ = p_token;
			children[1] = m__scanner_part_ = p__scanner_part_;
		}

		//
		// Constructor associated with the following production(s)
		// <anglr file fragment> -> <regular expression list terminal> <regular expression list>

		//

		public _anglr_file_fragment_ (SyntaxTreeToken p_token, _regular_expression_list_ p__regular_expression_list_) : base ((uint) ProductionID.__anglr_file_fragment__ID, (uint) production_kind.g__anglr_file_fragment__24)
		{
			++g_counter;
			_init ();
			children = (SyntaxTreeBase []) Array.CreateInstance (typeof (SyntaxTreeBase), 2);
			children[0] = m__regular_expression_list_terminal_ = p_token;
			children[1] = m__regular_expression_list_ = p__regular_expression_list_;
		}

		//
		// Constructor associated with the following production(s)
		// <anglr file fragment> -> <regular expression usage terminal> <regular expression usage>

		//

		public _anglr_file_fragment_ (SyntaxTreeToken p_token, _regular_expression_usage_ p__regular_expression_usage_) : base ((uint) ProductionID.__anglr_file_fragment__ID, (uint) production_kind.g__anglr_file_fragment__25)
		{
			++g_counter;
			_init ();
			children = (SyntaxTreeBase []) Array.CreateInstance (typeof (SyntaxTreeBase), 2);
			children[0] = m__regular_expression_usage_terminal_ = p_token;
			children[1] = m__regular_expression_usage_ = p__regular_expression_usage_;
		}

		//
		// Constructor associated with the following production(s)
		// <anglr file fragment> -> <actions terminal> <actions>

		//

		public _anglr_file_fragment_ (SyntaxTreeToken p_token, _actions_ p__actions_) : base ((uint) ProductionID.__anglr_file_fragment__ID, (uint) production_kind.g__anglr_file_fragment__26)
		{
			++g_counter;
			_init ();
			children = (SyntaxTreeBase []) Array.CreateInstance (typeof (SyntaxTreeBase), 2);
			children[0] = m__actions_terminal_ = p_token;
			children[1] = m__actions_ = p__actions_;
		}

		//
		// Constructor associated with the following production(s)
		// <anglr file fragment> -> <action terminal> <action>

		//

		public _anglr_file_fragment_ (SyntaxTreeToken p_token, _action_ p__action_) : base ((uint) ProductionID.__anglr_file_fragment__ID, (uint) production_kind.g__anglr_file_fragment__27)
		{
			++g_counter;
			_init ();
			children = (SyntaxTreeBase []) Array.CreateInstance (typeof (SyntaxTreeBase), 2);
			children[0] = m__action_terminal_ = p_token;
			children[1] = m__action_ = p__action_;
		}

		//
		// Constructor associated with the following production(s)
		// <anglr file fragment> -> <skip action terminal> <skip action>

		//

		public _anglr_file_fragment_ (SyntaxTreeToken p_token, _skip_action_ p__skip_action_) : base ((uint) ProductionID.__anglr_file_fragment__ID, (uint) production_kind.g__anglr_file_fragment__28)
		{
			++g_counter;
			_init ();
			children = (SyntaxTreeBase []) Array.CreateInstance (typeof (SyntaxTreeBase), 2);
			children[0] = m__skip_action_terminal_ = p_token;
			children[1] = m__skip_action_ = p__skip_action_;
		}

		//
		// Constructor associated with the following production(s)
		// <anglr file fragment> -> <terminal action terminal> <terminal action>

		//

		public _anglr_file_fragment_ (SyntaxTreeToken p_token, _terminal_action_ p__terminal_action_) : base ((uint) ProductionID.__anglr_file_fragment__ID, (uint) production_kind.g__anglr_file_fragment__29)
		{
			++g_counter;
			_init ();
			children = (SyntaxTreeBase []) Array.CreateInstance (typeof (SyntaxTreeBase), 2);
			children[0] = m__terminal_action_terminal_ = p_token;
			children[1] = m__terminal_action_ = p__terminal_action_;
		}

		//
		// Constructor associated with the following production(s)
		// <anglr file fragment> -> <event action terminal> <event action>

		//

		public _anglr_file_fragment_ (SyntaxTreeToken p_token, _event_action_ p__event_action_) : base ((uint) ProductionID.__anglr_file_fragment__ID, (uint) production_kind.g__anglr_file_fragment__30)
		{
			++g_counter;
			_init ();
			children = (SyntaxTreeBase []) Array.CreateInstance (typeof (SyntaxTreeBase), 2);
			children[0] = m__event_action_terminal_ = p_token;
			children[1] = m__event_action_ = p__event_action_;
		}

		//
		// Constructor associated with the following production(s)
		// <anglr file fragment> -> <push action terminal> <push action>

		//

		public _anglr_file_fragment_ (SyntaxTreeToken p_token, _push_action_ p__push_action_) : base ((uint) ProductionID.__anglr_file_fragment__ID, (uint) production_kind.g__anglr_file_fragment__31)
		{
			++g_counter;
			_init ();
			children = (SyntaxTreeBase []) Array.CreateInstance (typeof (SyntaxTreeBase), 2);
			children[0] = m__push_action_terminal_ = p_token;
			children[1] = m__push_action_ = p__push_action_;
		}

		//
		// Constructor associated with the following production(s)
		// <anglr file fragment> -> <pop action terminal> <pop action>

		//

		public _anglr_file_fragment_ (SyntaxTreeToken p_token, _pop_action_ p__pop_action_) : base ((uint) ProductionID.__anglr_file_fragment__ID, (uint) production_kind.g__anglr_file_fragment__32)
		{
			++g_counter;
			_init ();
			children = (SyntaxTreeBase []) Array.CreateInstance (typeof (SyntaxTreeBase), 2);
			children[0] = m__pop_action_terminal_ = p_token;
			children[1] = m__pop_action_ = p__pop_action_;
		}

		//
		// Constructor associated with the following production(s)
		// <anglr file fragment> -> <lexer part terminal> <lexer part>

		//

		public _anglr_file_fragment_ (SyntaxTreeToken p_token, _lexer_part_ p__lexer_part_) : base ((uint) ProductionID.__anglr_file_fragment__ID, (uint) production_kind.g__anglr_file_fragment__33)
		{
			++g_counter;
			_init ();
			children = (SyntaxTreeBase []) Array.CreateInstance (typeof (SyntaxTreeBase), 2);
			children[0] = m__lexer_part_terminal_ = p_token;
			children[1] = m__lexer_part_ = p__lexer_part_;
		}

		//
		// Constructor associated with the following production(s)
		// <anglr file fragment> -> <parser part terminal> <parser part>

		//

		public _anglr_file_fragment_ (SyntaxTreeToken p_token, _parser_part_ p__parser_part_) : base ((uint) ProductionID.__anglr_file_fragment__ID, (uint) production_kind.g__anglr_file_fragment__34)
		{
			++g_counter;
			_init ();
			children = (SyntaxTreeBase []) Array.CreateInstance (typeof (SyntaxTreeBase), 2);
			children[0] = m__parser_part_terminal_ = p_token;
			children[1] = m__parser_part_ = p__parser_part_;
		}

		//
		// Constructor associated with the following production(s)
		// <anglr file fragment> -> <anglr syntax rule list terminal> <anglr syntax rule list>

		//

		public _anglr_file_fragment_ (SyntaxTreeToken p_token, _anglr_syntax_rule_list_ p__anglr_syntax_rule_list_) : base ((uint) ProductionID.__anglr_file_fragment__ID, (uint) production_kind.g__anglr_file_fragment__35)
		{
			++g_counter;
			_init ();
			children = (SyntaxTreeBase []) Array.CreateInstance (typeof (SyntaxTreeBase), 2);
			children[0] = m__anglr_syntax_rule_list_terminal_ = p_token;
			children[1] = m__anglr_syntax_rule_list_ = p__anglr_syntax_rule_list_;
		}

		//
		// Constructor associated with the following production(s)
		// <anglr file fragment> -> <anglr syntax rule terminal> <anglr syntax rule>

		//

		public _anglr_file_fragment_ (SyntaxTreeToken p_token, _anglr_syntax_rule_ p__anglr_syntax_rule_) : base ((uint) ProductionID.__anglr_file_fragment__ID, (uint) production_kind.g__anglr_file_fragment__36)
		{
			++g_counter;
			_init ();
			children = (SyntaxTreeBase []) Array.CreateInstance (typeof (SyntaxTreeBase), 2);
			children[0] = m__anglr_syntax_rule_terminal_ = p_token;
			children[1] = m__anglr_syntax_rule_ = p__anglr_syntax_rule_;
		}

		//
		// Constructor associated with the following production(s)
		// <anglr file fragment> -> <anglr nested rule terminal> <anglr nested rule>

		//

		public _anglr_file_fragment_ (SyntaxTreeToken p_token, _anglr_nested_rule_ p__anglr_nested_rule_) : base ((uint) ProductionID.__anglr_file_fragment__ID, (uint) production_kind.g__anglr_file_fragment__37)
		{
			++g_counter;
			_init ();
			children = (SyntaxTreeBase []) Array.CreateInstance (typeof (SyntaxTreeBase), 2);
			children[0] = m__anglr_nested_rule_terminal_ = p_token;
			children[1] = m__anglr_nested_rule_ = p__anglr_nested_rule_;
		}

		//
		// Constructor associated with the following production(s)
		// <anglr file fragment> -> <anglr syntax production list name terminal> <anglr syntax production list name>

		//

		public _anglr_file_fragment_ (SyntaxTreeToken p_token, _anglr_syntax_production_list_name_ p__anglr_syntax_production_list_name_) : base ((uint) ProductionID.__anglr_file_fragment__ID, (uint) production_kind.g__anglr_file_fragment__38)
		{
			++g_counter;
			_init ();
			children = (SyntaxTreeBase []) Array.CreateInstance (typeof (SyntaxTreeBase), 2);
			children[0] = m__anglr_syntax_production_list_name_terminal_ = p_token;
			children[1] = m__anglr_syntax_production_list_name_ = p__anglr_syntax_production_list_name_;
		}

		//
		// Constructor associated with the following production(s)
		// <anglr file fragment> -> <anglr syntax production list terminal> <anglr syntax production list>

		//

		public _anglr_file_fragment_ (SyntaxTreeToken p_token, _anglr_syntax_production_list_ p__anglr_syntax_production_list_) : base ((uint) ProductionID.__anglr_file_fragment__ID, (uint) production_kind.g__anglr_file_fragment__39)
		{
			++g_counter;
			_init ();
			children = (SyntaxTreeBase []) Array.CreateInstance (typeof (SyntaxTreeBase), 2);
			children[0] = m__anglr_syntax_production_list_terminal_ = p_token;
			children[1] = m__anglr_syntax_production_list_ = p__anglr_syntax_production_list_;
		}

		//
		// Constructor associated with the following production(s)
		// <anglr file fragment> -> <anglr syntax production terminal> <anglr syntax production>

		//

		public _anglr_file_fragment_ (SyntaxTreeToken p_token, _anglr_syntax_production_ p__anglr_syntax_production_) : base ((uint) ProductionID.__anglr_file_fragment__ID, (uint) production_kind.g__anglr_file_fragment__40)
		{
			++g_counter;
			_init ();
			children = (SyntaxTreeBase []) Array.CreateInstance (typeof (SyntaxTreeBase), 2);
			children[0] = m__anglr_syntax_production_terminal_ = p_token;
			children[1] = m__anglr_syntax_production_ = p__anglr_syntax_production_;
		}

		//
		// Constructor associated with the following production(s)
		// <anglr file fragment> -> <priority assoc specification terminal> <priority assoc specification>

		//

		public _anglr_file_fragment_ (SyntaxTreeToken p_token, _priority_assoc_specification_ p__priority_assoc_specification_) : base ((uint) ProductionID.__anglr_file_fragment__ID, (uint) production_kind.g__anglr_file_fragment__41)
		{
			++g_counter;
			_init ();
			children = (SyntaxTreeBase []) Array.CreateInstance (typeof (SyntaxTreeBase), 2);
			children[0] = m__priority_assoc_specification_terminal_ = p_token;
			children[1] = m__priority_assoc_specification_ = p__priority_assoc_specification_;
		}

		//
		// Constructor associated with the following production(s)
		// <anglr file fragment> -> <priority specification terminal> <priority specification>

		//

		public _anglr_file_fragment_ (SyntaxTreeToken p_token, _priority_specification_ p__priority_specification_) : base ((uint) ProductionID.__anglr_file_fragment__ID, (uint) production_kind.g__anglr_file_fragment__42)
		{
			++g_counter;
			_init ();
			children = (SyntaxTreeBase []) Array.CreateInstance (typeof (SyntaxTreeBase), 2);
			children[0] = m__priority_specification_terminal_ = p_token;
			children[1] = m__priority_specification_ = p__priority_specification_;
		}

		//
		// Constructor associated with the following production(s)
		// <anglr file fragment> -> <associativity specification terminal> <associativity specification>

		//

		public _anglr_file_fragment_ (SyntaxTreeToken p_token, _associativity_specification_ p__associativity_specification_) : base ((uint) ProductionID.__anglr_file_fragment__ID, (uint) production_kind.g__anglr_file_fragment__43)
		{
			++g_counter;
			_init ();
			children = (SyntaxTreeBase []) Array.CreateInstance (typeof (SyntaxTreeBase), 2);
			children[0] = m__associativity_specification_terminal_ = p_token;
			children[1] = m__associativity_specification_ = p__associativity_specification_;
		}

		//
		// Constructor associated with the following production(s)
		// <anglr file fragment> -> <production name terminal> <production name>

		//

		public _anglr_file_fragment_ (SyntaxTreeToken p_token, _production_name_ p__production_name_) : base ((uint) ProductionID.__anglr_file_fragment__ID, (uint) production_kind.g__anglr_file_fragment__44)
		{
			++g_counter;
			_init ();
			children = (SyntaxTreeBase []) Array.CreateInstance (typeof (SyntaxTreeBase), 2);
			children[0] = m__production_name_terminal_ = p_token;
			children[1] = m__production_name_ = p__production_name_;
		}

		//
		// Constructor associated with the following production(s)
		// <anglr file fragment> -> <name list terminal> <name list>

		//

		public _anglr_file_fragment_ (SyntaxTreeToken p_token, _name_list_ p__name_list_) : base ((uint) ProductionID.__anglr_file_fragment__ID, (uint) production_kind.g__anglr_file_fragment__45)
		{
			++g_counter;
			_init ();
			children = (SyntaxTreeBase []) Array.CreateInstance (typeof (SyntaxTreeBase), 2);
			children[0] = m__name_list_terminal_ = p_token;
			children[1] = m__name_list_ = p__name_list_;
		}

		//
		// Constructor associated with the following production(s)
		// <anglr file fragment> -> <marker list terminal> <marker list>

		//

		public _anglr_file_fragment_ (SyntaxTreeToken p_token, _marker_list_ p__marker_list_) : base ((uint) ProductionID.__anglr_file_fragment__ID, (uint) production_kind.g__anglr_file_fragment__46)
		{
			++g_counter;
			_init ();
			children = (SyntaxTreeBase []) Array.CreateInstance (typeof (SyntaxTreeBase), 2);
			children[0] = m__marker_list_terminal_ = p_token;
			children[1] = m__marker_list_ = p__marker_list_;
		}

		//
		// Constructor associated with the following production(s)
		// <anglr file fragment> -> <marker terminal> <marker>

		//

		public _anglr_file_fragment_ (SyntaxTreeToken p_token, _marker_ p__marker_) : base ((uint) ProductionID.__anglr_file_fragment__ID, (uint) production_kind.g__anglr_file_fragment__47)
		{
			++g_counter;
			_init ();
			children = (SyntaxTreeBase []) Array.CreateInstance (typeof (SyntaxTreeBase), 2);
			children[0] = m__marker_terminal_ = p_token;
			children[1] = m__marker_ = p__marker_;
		}

		//
		// Constructor associated with the following production(s)
		// <anglr file fragment> -> <g name terminal> <g name>

		//

		public _anglr_file_fragment_ (SyntaxTreeToken p_token, _g_name_ p__g_name_) : base ((uint) ProductionID.__anglr_file_fragment__ID, (uint) production_kind.g__anglr_file_fragment__48)
		{
			++g_counter;
			_init ();
			children = (SyntaxTreeBase []) Array.CreateInstance (typeof (SyntaxTreeBase), 2);
			children[0] = m__g_name_terminal_ = p_token;
			children[1] = m__g_name_ = p__g_name_;
		}

		//
		// Constructor associated with the following production(s)
		// <anglr file fragment> -> <name terminal> <name>

		//

		public _anglr_file_fragment_ (SyntaxTreeToken p_token, _name_ p__name_) : base ((uint) ProductionID.__anglr_file_fragment__ID, (uint) production_kind.g__anglr_file_fragment__49)
		{
			++g_counter;
			_init ();
			children = (SyntaxTreeBase []) Array.CreateInstance (typeof (SyntaxTreeBase), 2);
			children[0] = m__name_terminal_ = p_token;
			children[1] = m__name_ = p__name_;
		}

		//
		// Constructor associated with the following production(s)
		// <anglr file fragment> -> <cardinality delimiter terminal> <cardinality delimiter>

		//

		public _anglr_file_fragment_ (SyntaxTreeToken p_token, _cardinality_delimiter_ p__cardinality_delimiter_) : base ((uint) ProductionID.__anglr_file_fragment__ID, (uint) production_kind.g__anglr_file_fragment__50)
		{
			++g_counter;
			_init ();
			children = (SyntaxTreeBase []) Array.CreateInstance (typeof (SyntaxTreeBase), 2);
			children[0] = m__cardinality_delimiter_terminal_ = p_token;
			children[1] = m__cardinality_delimiter_ = p__cardinality_delimiter_;
		}

		//
		// Constructor associated with the following production(s)
		// <anglr file fragment> -> <cardinality terminal> <cardinality>

		//

		public _anglr_file_fragment_ (SyntaxTreeToken p_token, _cardinality_ p__cardinality_) : base ((uint) ProductionID.__anglr_file_fragment__ID, (uint) production_kind.g__anglr_file_fragment__51)
		{
			++g_counter;
			_init ();
			children = (SyntaxTreeBase []) Array.CreateInstance (typeof (SyntaxTreeBase), 2);
			children[0] = m__cardinality_terminal_ = p_token;
			children[1] = m__cardinality_ = p__cardinality_;
		}

		//
		// Constructor associated with the following production(s)
		// <anglr file fragment> -> <delimiter terminal> <delimiter>

		//

		public _anglr_file_fragment_ (SyntaxTreeToken p_token, _delimiter_ p__delimiter_) : base ((uint) ProductionID.__anglr_file_fragment__ID, (uint) production_kind.g__anglr_file_fragment__52)
		{
			++g_counter;
			_init ();
			children = (SyntaxTreeBase []) Array.CreateInstance (typeof (SyntaxTreeBase), 2);
			children[0] = m__delimiter_terminal_ = p_token;
			children[1] = m__delimiter_ = p__delimiter_;
		}

		//
		// Constructor associated with the following production(s)
		// <anglr file fragment> -> <attribute list optional terminal> <attribute list optional>

		//

		public _anglr_file_fragment_ (SyntaxTreeToken p_token, _attribute_list_optional_ p__attribute_list_optional_) : base ((uint) ProductionID.__anglr_file_fragment__ID, (uint) production_kind.g__anglr_file_fragment__53)
		{
			++g_counter;
			_init ();
			children = (SyntaxTreeBase []) Array.CreateInstance (typeof (SyntaxTreeBase), 2);
			children[0] = m__attribute_list_optional_terminal_ = p_token;
			children[1] = m__attribute_list_optional_ = p__attribute_list_optional_;
		}

		//
		// Constructor associated with the following production(s)
		// <anglr file fragment> -> <name value list optional terminal> <name value list optional>

		//

		public _anglr_file_fragment_ (SyntaxTreeToken p_token, _name_value_list_optional_ p__name_value_list_optional_) : base ((uint) ProductionID.__anglr_file_fragment__ID, (uint) production_kind.g__anglr_file_fragment__54)
		{
			++g_counter;
			_init ();
			children = (SyntaxTreeBase []) Array.CreateInstance (typeof (SyntaxTreeBase), 2);
			children[0] = m__name_value_list_optional_terminal_ = p_token;
			children[1] = m__name_value_list_optional_ = p__name_value_list_optional_;
		}

		//
		// Constructor associated with the following production(s)
		// <anglr file fragment> -> <anglr file part list optional terminal> <anglr file part list optional>

		//

		public _anglr_file_fragment_ (SyntaxTreeToken p_token, _anglr_file_part_list_optional_ p__anglr_file_part_list_optional_) : base ((uint) ProductionID.__anglr_file_fragment__ID, (uint) production_kind.g__anglr_file_fragment__55)
		{
			++g_counter;
			_init ();
			children = (SyntaxTreeBase []) Array.CreateInstance (typeof (SyntaxTreeBase), 2);
			children[0] = m__anglr_file_part_list_optional_terminal_ = p_token;
			children[1] = m__anglr_file_part_list_optional_ = p__anglr_file_part_list_optional_;
		}

		//
		// Constructor associated with the following production(s)
		// <anglr file fragment> -> <anglr definition list optional terminal> <anglr definition list optional>

		//

		public _anglr_file_fragment_ (SyntaxTreeToken p_token, _anglr_definition_list_optional_ p__anglr_definition_list_optional_) : base ((uint) ProductionID.__anglr_file_fragment__ID, (uint) production_kind.g__anglr_file_fragment__56)
		{
			++g_counter;
			_init ();
			children = (SyntaxTreeBase []) Array.CreateInstance (typeof (SyntaxTreeBase), 2);
			children[0] = m__anglr_definition_list_optional_terminal_ = p_token;
			children[1] = m__anglr_definition_list_optional_ = p__anglr_definition_list_optional_;
		}

		//
		// Constructor associated with the following production(s)
		// <anglr file fragment> -> <block terminal definitions optional terminal> <block terminal definitions optional>

		//

		public _anglr_file_fragment_ (SyntaxTreeToken p_token, _block_terminal_definitions_optional_ p__block_terminal_definitions_optional_) : base ((uint) ProductionID.__anglr_file_fragment__ID, (uint) production_kind.g__anglr_file_fragment__57)
		{
			++g_counter;
			_init ();
			children = (SyntaxTreeBase []) Array.CreateInstance (typeof (SyntaxTreeBase), 2);
			children[0] = m__block_terminal_definitions_optional_terminal_ = p_token;
			children[1] = m__block_terminal_definitions_optional_ = p__block_terminal_definitions_optional_;
		}

		//
		// Constructor associated with the following production(s)
		// <anglr file fragment> -> <block regex definitions optional terminal> <block regex definitions optional>

		//

		public _anglr_file_fragment_ (SyntaxTreeToken p_token, _block_regex_definitions_optional_ p__block_regex_definitions_optional_) : base ((uint) ProductionID.__anglr_file_fragment__ID, (uint) production_kind.g__anglr_file_fragment__58)
		{
			++g_counter;
			_init ();
			children = (SyntaxTreeBase []) Array.CreateInstance (typeof (SyntaxTreeBase), 2);
			children[0] = m__block_regex_definitions_optional_terminal_ = p_token;
			children[1] = m__block_regex_definitions_optional_ = p__block_regex_definitions_optional_;
		}

		//
		// Constructor associated with the following production(s)
		// <anglr file fragment> -> <regular expression list optional terminal> <regular expression list optional>

		//

		public _anglr_file_fragment_ (SyntaxTreeToken p_token, _regular_expression_list_optional_ p__regular_expression_list_optional_) : base ((uint) ProductionID.__anglr_file_fragment__ID, (uint) production_kind.g__anglr_file_fragment__59)
		{
			++g_counter;
			_init ();
			children = (SyntaxTreeBase []) Array.CreateInstance (typeof (SyntaxTreeBase), 2);
			children[0] = m__regular_expression_list_optional_terminal_ = p_token;
			children[1] = m__regular_expression_list_optional_ = p__regular_expression_list_optional_;
		}

		//
		// Constructor associated with the following production(s)
		// <anglr file fragment> -> <actions optional terminal> <actions optional>

		//

		public _anglr_file_fragment_ (SyntaxTreeToken p_token, _actions_optional_ p__actions_optional_) : base ((uint) ProductionID.__anglr_file_fragment__ID, (uint) production_kind.g__anglr_file_fragment__60)
		{
			++g_counter;
			_init ();
			children = (SyntaxTreeBase []) Array.CreateInstance (typeof (SyntaxTreeBase), 2);
			children[0] = m__actions_optional_terminal_ = p_token;
			children[1] = m__actions_optional_ = p__actions_optional_;
		}

		//
		// Constructor associated with the following production(s)
		// <anglr file fragment> -> <anglr syntax rule list optional terminal> <anglr syntax rule list optional>

		//

		public _anglr_file_fragment_ (SyntaxTreeToken p_token, _anglr_syntax_rule_list_optional_ p__anglr_syntax_rule_list_optional_) : base ((uint) ProductionID.__anglr_file_fragment__ID, (uint) production_kind.g__anglr_file_fragment__61)
		{
			++g_counter;
			_init ();
			children = (SyntaxTreeBase []) Array.CreateInstance (typeof (SyntaxTreeBase), 2);
			children[0] = m__anglr_syntax_rule_list_optional_terminal_ = p_token;
			children[1] = m__anglr_syntax_rule_list_optional_ = p__anglr_syntax_rule_list_optional_;
		}

		//
		// Constructor associated with the following production(s)
		// <anglr file fragment> -> <anglr syntax production list name optional terminal> <anglr syntax production list name optional>

		//

		public _anglr_file_fragment_ (SyntaxTreeToken p_token, _anglr_syntax_production_list_name_optional_ p__anglr_syntax_production_list_name_optional_) : base ((uint) ProductionID.__anglr_file_fragment__ID, (uint) production_kind.g__anglr_file_fragment__62)
		{
			++g_counter;
			_init ();
			children = (SyntaxTreeBase []) Array.CreateInstance (typeof (SyntaxTreeBase), 2);
			children[0] = m__anglr_syntax_production_list_name_optional_terminal_ = p_token;
			children[1] = m__anglr_syntax_production_list_name_optional_ = p__anglr_syntax_production_list_name_optional_;
		}

		//
		// Constructor associated with the following production(s)
		// <anglr file fragment> -> <production name optional terminal> <production name optional>

		//

		public _anglr_file_fragment_ (SyntaxTreeToken p_token, _production_name_optional_ p__production_name_optional_) : base ((uint) ProductionID.__anglr_file_fragment__ID, (uint) production_kind.g__anglr_file_fragment__63)
		{
			++g_counter;
			_init ();
			children = (SyntaxTreeBase []) Array.CreateInstance (typeof (SyntaxTreeBase), 2);
			children[0] = m__production_name_optional_terminal_ = p_token;
			children[1] = m__production_name_optional_ = p__production_name_optional_;
		}

		//
		// Constructor associated with the following production(s)
		// <anglr file fragment> -> <priority assoc specification optional terminal> <priority assoc specification optional>

		//

		public _anglr_file_fragment_ (SyntaxTreeToken p_token, _priority_assoc_specification_optional_ p__priority_assoc_specification_optional_) : base ((uint) ProductionID.__anglr_file_fragment__ID, (uint) production_kind.g__anglr_file_fragment__64)
		{
			++g_counter;
			_init ();
			children = (SyntaxTreeBase []) Array.CreateInstance (typeof (SyntaxTreeBase), 2);
			children[0] = m__priority_assoc_specification_optional_terminal_ = p_token;
			children[1] = m__priority_assoc_specification_optional_ = p__priority_assoc_specification_optional_;
		}

		//
		// Constructor associated with the following production(s)
		// <anglr file fragment> -> <marker list optional terminal> <marker list optional>

		//

		public _anglr_file_fragment_ (SyntaxTreeToken p_token, _marker_list_optional_ p__marker_list_optional_) : base ((uint) ProductionID.__anglr_file_fragment__ID, (uint) production_kind.g__anglr_file_fragment__65)
		{
			++g_counter;
			_init ();
			children = (SyntaxTreeBase []) Array.CreateInstance (typeof (SyntaxTreeBase), 2);
			children[0] = m__marker_list_optional_terminal_ = p_token;
			children[1] = m__marker_list_optional_ = p__marker_list_optional_;
		}

		//
		// Constructor associated with the following production(s)
		// <anglr file fragment> -> <delimiter optional terminal> <delimiter optional>

		//

		public _anglr_file_fragment_ (SyntaxTreeToken p_token, _delimiter_optional_ p__delimiter_optional_) : base ((uint) ProductionID.__anglr_file_fragment__ID, (uint) production_kind.g__anglr_file_fragment__66)
		{
			++g_counter;
			_init ();
			children = (SyntaxTreeBase []) Array.CreateInstance (typeof (SyntaxTreeBase), 2);
			children[0] = m__delimiter_optional_terminal_ = p_token;
			children[1] = m__delimiter_optional_ = p__delimiter_optional_;
		}

		//
		// Constructor associated with the following production(s)
		// <anglr file fragment> -> <cstring optional terminal> <cstring optional>

		//

		public _anglr_file_fragment_ (SyntaxTreeToken p_token, _cstring_optional_ p__cstring_optional_) : base ((uint) ProductionID.__anglr_file_fragment__ID, (uint) production_kind.g__anglr_file_fragment__67)
		{
			++g_counter;
			_init ();
			children = (SyntaxTreeBase []) Array.CreateInstance (typeof (SyntaxTreeBase), 2);
			children[0] = m__cstring_optional_terminal_ = p_token;
			children[1] = m__cstring_optional_ = p__cstring_optional_;
		}

		//
		// Constructor associated with the following production(s)
		// <anglr file fragment> -> <number optional terminal> <number optional>

		//

		public _anglr_file_fragment_ (SyntaxTreeToken p_token, _number_optional_ p__number_optional_) : base ((uint) ProductionID.__anglr_file_fragment__ID, (uint) production_kind.g__anglr_file_fragment__68)
		{
			++g_counter;
			_init ();
			children = (SyntaxTreeBase []) Array.CreateInstance (typeof (SyntaxTreeBase), 2);
			children[0] = m__number_optional_terminal_ = p_token;
			children[1] = m__number_optional_ = p__number_optional_;
		}

		// Copy constructor

		public _anglr_file_fragment_ (_anglr_file_fragment_ p__anglr_file_fragment_) : base (p__anglr_file_fragment_.id, p__anglr_file_fragment_.kind)
		{
			++g_counter;
			_init ();
			switch ((production_kind) this.kind)
			{
			case production_kind.g__anglr_file_fragment__1:
				children = (SyntaxTreeBase []) Array.CreateInstance (typeof (SyntaxTreeBase), 2);
				children[0] = m__attribute_list_terminal_ = p__anglr_file_fragment_.m__attribute_list_terminal_;
				children[1] = m__attribute_list_ = p__anglr_file_fragment_.m__attribute_list_;
				break;
			case production_kind.g__anglr_file_fragment__2:
				children = (SyntaxTreeBase []) Array.CreateInstance (typeof (SyntaxTreeBase), 2);
				children[0] = m__attribute_terminal_ = p__anglr_file_fragment_.m__attribute_terminal_;
				children[1] = m__attribute_ = p__anglr_file_fragment_.m__attribute_;
				break;
			case production_kind.g__anglr_file_fragment__3:
				children = (SyntaxTreeBase []) Array.CreateInstance (typeof (SyntaxTreeBase), 2);
				children[0] = m__name_value_list_terminal_ = p__anglr_file_fragment_.m__name_value_list_terminal_;
				children[1] = m__name_value_list_ = p__anglr_file_fragment_.m__name_value_list_;
				break;
			case production_kind.g__anglr_file_fragment__4:
				children = (SyntaxTreeBase []) Array.CreateInstance (typeof (SyntaxTreeBase), 2);
				children[0] = m__name_value_pair_terminal_ = p__anglr_file_fragment_.m__name_value_pair_terminal_;
				children[1] = m__name_value_pair_ = p__anglr_file_fragment_.m__name_value_pair_;
				break;
			case production_kind.g__anglr_file_fragment__5:
				children = (SyntaxTreeBase []) Array.CreateInstance (typeof (SyntaxTreeBase), 2);
				children[0] = m__anglr_file_terminal_ = p__anglr_file_fragment_.m__anglr_file_terminal_;
				children[1] = m__anglr_file_ = p__anglr_file_fragment_.m__anglr_file_;
				break;
			case production_kind.g__anglr_file_fragment__6:
				children = (SyntaxTreeBase []) Array.CreateInstance (typeof (SyntaxTreeBase), 2);
				children[0] = m__anglr_file_part_list_terminal_ = p__anglr_file_fragment_.m__anglr_file_part_list_terminal_;
				children[1] = m__anglr_file_part_list_ = p__anglr_file_fragment_.m__anglr_file_part_list_;
				break;
			case production_kind.g__anglr_file_fragment__7:
				children = (SyntaxTreeBase []) Array.CreateInstance (typeof (SyntaxTreeBase), 2);
				children[0] = m__anglr_file_part_terminal_ = p__anglr_file_fragment_.m__anglr_file_part_terminal_;
				children[1] = m__anglr_file_part_ = p__anglr_file_fragment_.m__anglr_file_part_;
				break;
			case production_kind.g__anglr_file_fragment__8:
				children = (SyntaxTreeBase []) Array.CreateInstance (typeof (SyntaxTreeBase), 2);
				children[0] = m__general_part_terminal_ = p__anglr_file_fragment_.m__general_part_terminal_;
				children[1] = m__general_part_ = p__anglr_file_fragment_.m__general_part_;
				break;
			case production_kind.g__anglr_file_fragment__9:
				children = (SyntaxTreeBase []) Array.CreateInstance (typeof (SyntaxTreeBase), 2);
				children[0] = m__declaration_part_terminal_ = p__anglr_file_fragment_.m__declaration_part_terminal_;
				children[1] = m__declaration_part_ = p__anglr_file_fragment_.m__declaration_part_;
				break;
			case production_kind.g__anglr_file_fragment__10:
				children = (SyntaxTreeBase []) Array.CreateInstance (typeof (SyntaxTreeBase), 2);
				children[0] = m__anglr_definition_list_terminal_ = p__anglr_file_fragment_.m__anglr_definition_list_terminal_;
				children[1] = m__anglr_definition_list_ = p__anglr_file_fragment_.m__anglr_definition_list_;
				break;
			case production_kind.g__anglr_file_fragment__11:
				children = (SyntaxTreeBase []) Array.CreateInstance (typeof (SyntaxTreeBase), 2);
				children[0] = m__anglr_definition_with_attribute_list_terminal_ = p__anglr_file_fragment_.m__anglr_definition_with_attribute_list_terminal_;
				children[1] = m__anglr_definition_with_attribute_ = p__anglr_file_fragment_.m__anglr_definition_with_attribute_;
				break;
			case production_kind.g__anglr_file_fragment__12:
				children = (SyntaxTreeBase []) Array.CreateInstance (typeof (SyntaxTreeBase), 2);
				children[0] = m__anglr_definition_terminal_ = p__anglr_file_fragment_.m__anglr_definition_terminal_;
				children[1] = m__anglr_definition_ = p__anglr_file_fragment_.m__anglr_definition_;
				break;
			case production_kind.g__anglr_file_fragment__13:
				children = (SyntaxTreeBase []) Array.CreateInstance (typeof (SyntaxTreeBase), 2);
				children[0] = m__single_terminal_definition_terminal_ = p__anglr_file_fragment_.m__single_terminal_definition_terminal_;
				children[1] = m__single_terminal_definition_ = p__anglr_file_fragment_.m__single_terminal_definition_;
				break;
			case production_kind.g__anglr_file_fragment__14:
				children = (SyntaxTreeBase []) Array.CreateInstance (typeof (SyntaxTreeBase), 2);
				children[0] = m__single_regex_definition_terminal_ = p__anglr_file_fragment_.m__single_regex_definition_terminal_;
				children[1] = m__single_regex_definition_ = p__anglr_file_fragment_.m__single_regex_definition_;
				break;
			case production_kind.g__anglr_file_fragment__15:
				children = (SyntaxTreeBase []) Array.CreateInstance (typeof (SyntaxTreeBase), 2);
				children[0] = m__block_of_terminal_definitions_terminal_ = p__anglr_file_fragment_.m__block_of_terminal_definitions_terminal_;
				children[1] = m__block_of_terminal_definitions_ = p__anglr_file_fragment_.m__block_of_terminal_definitions_;
				break;
			case production_kind.g__anglr_file_fragment__16:
				children = (SyntaxTreeBase []) Array.CreateInstance (typeof (SyntaxTreeBase), 2);
				children[0] = m__block_of_regex_definitions_terminal_ = p__anglr_file_fragment_.m__block_of_regex_definitions_terminal_;
				children[1] = m__block_of_regex_definitions_ = p__anglr_file_fragment_.m__block_of_regex_definitions_;
				break;
			case production_kind.g__anglr_file_fragment__17:
				children = (SyntaxTreeBase []) Array.CreateInstance (typeof (SyntaxTreeBase), 2);
				children[0] = m__terminal_definition_terminal_ = p__anglr_file_fragment_.m__terminal_definition_terminal_;
				children[1] = m__terminal_definition_ = p__anglr_file_fragment_.m__terminal_definition_;
				break;
			case production_kind.g__anglr_file_fragment__18:
				children = (SyntaxTreeBase []) Array.CreateInstance (typeof (SyntaxTreeBase), 2);
				children[0] = m__regex_definition_terminal_ = p__anglr_file_fragment_.m__regex_definition_terminal_;
				children[1] = m__regex_definition_ = p__anglr_file_fragment_.m__regex_definition_;
				break;
			case production_kind.g__anglr_file_fragment__19:
				children = (SyntaxTreeBase []) Array.CreateInstance (typeof (SyntaxTreeBase), 2);
				children[0] = m__block_terminal_definitions_terminal_ = p__anglr_file_fragment_.m__block_terminal_definitions_terminal_;
				children[1] = m__block_terminal_definitions_ = p__anglr_file_fragment_.m__block_terminal_definitions_;
				break;
			case production_kind.g__anglr_file_fragment__20:
				children = (SyntaxTreeBase []) Array.CreateInstance (typeof (SyntaxTreeBase), 2);
				children[0] = m__block_terminal_definition_terminal_ = p__anglr_file_fragment_.m__block_terminal_definition_terminal_;
				children[1] = m__block_terminal_definition_ = p__anglr_file_fragment_.m__block_terminal_definition_;
				break;
			case production_kind.g__anglr_file_fragment__21:
				children = (SyntaxTreeBase []) Array.CreateInstance (typeof (SyntaxTreeBase), 2);
				children[0] = m__block_regex_definitions_terminal_ = p__anglr_file_fragment_.m__block_regex_definitions_terminal_;
				children[1] = m__block_regex_definitions_ = p__anglr_file_fragment_.m__block_regex_definitions_;
				break;
			case production_kind.g__anglr_file_fragment__22:
				children = (SyntaxTreeBase []) Array.CreateInstance (typeof (SyntaxTreeBase), 2);
				children[0] = m__block_regex_definition_terminal_ = p__anglr_file_fragment_.m__block_regex_definition_terminal_;
				children[1] = m__block_regex_definition_ = p__anglr_file_fragment_.m__block_regex_definition_;
				break;
			case production_kind.g__anglr_file_fragment__23:
				children = (SyntaxTreeBase []) Array.CreateInstance (typeof (SyntaxTreeBase), 2);
				children[0] = m__scanner_part_terminal_ = p__anglr_file_fragment_.m__scanner_part_terminal_;
				children[1] = m__scanner_part_ = p__anglr_file_fragment_.m__scanner_part_;
				break;
			case production_kind.g__anglr_file_fragment__24:
				children = (SyntaxTreeBase []) Array.CreateInstance (typeof (SyntaxTreeBase), 2);
				children[0] = m__regular_expression_list_terminal_ = p__anglr_file_fragment_.m__regular_expression_list_terminal_;
				children[1] = m__regular_expression_list_ = p__anglr_file_fragment_.m__regular_expression_list_;
				break;
			case production_kind.g__anglr_file_fragment__25:
				children = (SyntaxTreeBase []) Array.CreateInstance (typeof (SyntaxTreeBase), 2);
				children[0] = m__regular_expression_usage_terminal_ = p__anglr_file_fragment_.m__regular_expression_usage_terminal_;
				children[1] = m__regular_expression_usage_ = p__anglr_file_fragment_.m__regular_expression_usage_;
				break;
			case production_kind.g__anglr_file_fragment__26:
				children = (SyntaxTreeBase []) Array.CreateInstance (typeof (SyntaxTreeBase), 2);
				children[0] = m__actions_terminal_ = p__anglr_file_fragment_.m__actions_terminal_;
				children[1] = m__actions_ = p__anglr_file_fragment_.m__actions_;
				break;
			case production_kind.g__anglr_file_fragment__27:
				children = (SyntaxTreeBase []) Array.CreateInstance (typeof (SyntaxTreeBase), 2);
				children[0] = m__action_terminal_ = p__anglr_file_fragment_.m__action_terminal_;
				children[1] = m__action_ = p__anglr_file_fragment_.m__action_;
				break;
			case production_kind.g__anglr_file_fragment__28:
				children = (SyntaxTreeBase []) Array.CreateInstance (typeof (SyntaxTreeBase), 2);
				children[0] = m__skip_action_terminal_ = p__anglr_file_fragment_.m__skip_action_terminal_;
				children[1] = m__skip_action_ = p__anglr_file_fragment_.m__skip_action_;
				break;
			case production_kind.g__anglr_file_fragment__29:
				children = (SyntaxTreeBase []) Array.CreateInstance (typeof (SyntaxTreeBase), 2);
				children[0] = m__terminal_action_terminal_ = p__anglr_file_fragment_.m__terminal_action_terminal_;
				children[1] = m__terminal_action_ = p__anglr_file_fragment_.m__terminal_action_;
				break;
			case production_kind.g__anglr_file_fragment__30:
				children = (SyntaxTreeBase []) Array.CreateInstance (typeof (SyntaxTreeBase), 2);
				children[0] = m__event_action_terminal_ = p__anglr_file_fragment_.m__event_action_terminal_;
				children[1] = m__event_action_ = p__anglr_file_fragment_.m__event_action_;
				break;
			case production_kind.g__anglr_file_fragment__31:
				children = (SyntaxTreeBase []) Array.CreateInstance (typeof (SyntaxTreeBase), 2);
				children[0] = m__push_action_terminal_ = p__anglr_file_fragment_.m__push_action_terminal_;
				children[1] = m__push_action_ = p__anglr_file_fragment_.m__push_action_;
				break;
			case production_kind.g__anglr_file_fragment__32:
				children = (SyntaxTreeBase []) Array.CreateInstance (typeof (SyntaxTreeBase), 2);
				children[0] = m__pop_action_terminal_ = p__anglr_file_fragment_.m__pop_action_terminal_;
				children[1] = m__pop_action_ = p__anglr_file_fragment_.m__pop_action_;
				break;
			case production_kind.g__anglr_file_fragment__33:
				children = (SyntaxTreeBase []) Array.CreateInstance (typeof (SyntaxTreeBase), 2);
				children[0] = m__lexer_part_terminal_ = p__anglr_file_fragment_.m__lexer_part_terminal_;
				children[1] = m__lexer_part_ = p__anglr_file_fragment_.m__lexer_part_;
				break;
			case production_kind.g__anglr_file_fragment__34:
				children = (SyntaxTreeBase []) Array.CreateInstance (typeof (SyntaxTreeBase), 2);
				children[0] = m__parser_part_terminal_ = p__anglr_file_fragment_.m__parser_part_terminal_;
				children[1] = m__parser_part_ = p__anglr_file_fragment_.m__parser_part_;
				break;
			case production_kind.g__anglr_file_fragment__35:
				children = (SyntaxTreeBase []) Array.CreateInstance (typeof (SyntaxTreeBase), 2);
				children[0] = m__anglr_syntax_rule_list_terminal_ = p__anglr_file_fragment_.m__anglr_syntax_rule_list_terminal_;
				children[1] = m__anglr_syntax_rule_list_ = p__anglr_file_fragment_.m__anglr_syntax_rule_list_;
				break;
			case production_kind.g__anglr_file_fragment__36:
				children = (SyntaxTreeBase []) Array.CreateInstance (typeof (SyntaxTreeBase), 2);
				children[0] = m__anglr_syntax_rule_terminal_ = p__anglr_file_fragment_.m__anglr_syntax_rule_terminal_;
				children[1] = m__anglr_syntax_rule_ = p__anglr_file_fragment_.m__anglr_syntax_rule_;
				break;
			case production_kind.g__anglr_file_fragment__37:
				children = (SyntaxTreeBase []) Array.CreateInstance (typeof (SyntaxTreeBase), 2);
				children[0] = m__anglr_nested_rule_terminal_ = p__anglr_file_fragment_.m__anglr_nested_rule_terminal_;
				children[1] = m__anglr_nested_rule_ = p__anglr_file_fragment_.m__anglr_nested_rule_;
				break;
			case production_kind.g__anglr_file_fragment__38:
				children = (SyntaxTreeBase []) Array.CreateInstance (typeof (SyntaxTreeBase), 2);
				children[0] = m__anglr_syntax_production_list_name_terminal_ = p__anglr_file_fragment_.m__anglr_syntax_production_list_name_terminal_;
				children[1] = m__anglr_syntax_production_list_name_ = p__anglr_file_fragment_.m__anglr_syntax_production_list_name_;
				break;
			case production_kind.g__anglr_file_fragment__39:
				children = (SyntaxTreeBase []) Array.CreateInstance (typeof (SyntaxTreeBase), 2);
				children[0] = m__anglr_syntax_production_list_terminal_ = p__anglr_file_fragment_.m__anglr_syntax_production_list_terminal_;
				children[1] = m__anglr_syntax_production_list_ = p__anglr_file_fragment_.m__anglr_syntax_production_list_;
				break;
			case production_kind.g__anglr_file_fragment__40:
				children = (SyntaxTreeBase []) Array.CreateInstance (typeof (SyntaxTreeBase), 2);
				children[0] = m__anglr_syntax_production_terminal_ = p__anglr_file_fragment_.m__anglr_syntax_production_terminal_;
				children[1] = m__anglr_syntax_production_ = p__anglr_file_fragment_.m__anglr_syntax_production_;
				break;
			case production_kind.g__anglr_file_fragment__41:
				children = (SyntaxTreeBase []) Array.CreateInstance (typeof (SyntaxTreeBase), 2);
				children[0] = m__priority_assoc_specification_terminal_ = p__anglr_file_fragment_.m__priority_assoc_specification_terminal_;
				children[1] = m__priority_assoc_specification_ = p__anglr_file_fragment_.m__priority_assoc_specification_;
				break;
			case production_kind.g__anglr_file_fragment__42:
				children = (SyntaxTreeBase []) Array.CreateInstance (typeof (SyntaxTreeBase), 2);
				children[0] = m__priority_specification_terminal_ = p__anglr_file_fragment_.m__priority_specification_terminal_;
				children[1] = m__priority_specification_ = p__anglr_file_fragment_.m__priority_specification_;
				break;
			case production_kind.g__anglr_file_fragment__43:
				children = (SyntaxTreeBase []) Array.CreateInstance (typeof (SyntaxTreeBase), 2);
				children[0] = m__associativity_specification_terminal_ = p__anglr_file_fragment_.m__associativity_specification_terminal_;
				children[1] = m__associativity_specification_ = p__anglr_file_fragment_.m__associativity_specification_;
				break;
			case production_kind.g__anglr_file_fragment__44:
				children = (SyntaxTreeBase []) Array.CreateInstance (typeof (SyntaxTreeBase), 2);
				children[0] = m__production_name_terminal_ = p__anglr_file_fragment_.m__production_name_terminal_;
				children[1] = m__production_name_ = p__anglr_file_fragment_.m__production_name_;
				break;
			case production_kind.g__anglr_file_fragment__45:
				children = (SyntaxTreeBase []) Array.CreateInstance (typeof (SyntaxTreeBase), 2);
				children[0] = m__name_list_terminal_ = p__anglr_file_fragment_.m__name_list_terminal_;
				children[1] = m__name_list_ = p__anglr_file_fragment_.m__name_list_;
				break;
			case production_kind.g__anglr_file_fragment__46:
				children = (SyntaxTreeBase []) Array.CreateInstance (typeof (SyntaxTreeBase), 2);
				children[0] = m__marker_list_terminal_ = p__anglr_file_fragment_.m__marker_list_terminal_;
				children[1] = m__marker_list_ = p__anglr_file_fragment_.m__marker_list_;
				break;
			case production_kind.g__anglr_file_fragment__47:
				children = (SyntaxTreeBase []) Array.CreateInstance (typeof (SyntaxTreeBase), 2);
				children[0] = m__marker_terminal_ = p__anglr_file_fragment_.m__marker_terminal_;
				children[1] = m__marker_ = p__anglr_file_fragment_.m__marker_;
				break;
			case production_kind.g__anglr_file_fragment__48:
				children = (SyntaxTreeBase []) Array.CreateInstance (typeof (SyntaxTreeBase), 2);
				children[0] = m__g_name_terminal_ = p__anglr_file_fragment_.m__g_name_terminal_;
				children[1] = m__g_name_ = p__anglr_file_fragment_.m__g_name_;
				break;
			case production_kind.g__anglr_file_fragment__49:
				children = (SyntaxTreeBase []) Array.CreateInstance (typeof (SyntaxTreeBase), 2);
				children[0] = m__name_terminal_ = p__anglr_file_fragment_.m__name_terminal_;
				children[1] = m__name_ = p__anglr_file_fragment_.m__name_;
				break;
			case production_kind.g__anglr_file_fragment__50:
				children = (SyntaxTreeBase []) Array.CreateInstance (typeof (SyntaxTreeBase), 2);
				children[0] = m__cardinality_delimiter_terminal_ = p__anglr_file_fragment_.m__cardinality_delimiter_terminal_;
				children[1] = m__cardinality_delimiter_ = p__anglr_file_fragment_.m__cardinality_delimiter_;
				break;
			case production_kind.g__anglr_file_fragment__51:
				children = (SyntaxTreeBase []) Array.CreateInstance (typeof (SyntaxTreeBase), 2);
				children[0] = m__cardinality_terminal_ = p__anglr_file_fragment_.m__cardinality_terminal_;
				children[1] = m__cardinality_ = p__anglr_file_fragment_.m__cardinality_;
				break;
			case production_kind.g__anglr_file_fragment__52:
				children = (SyntaxTreeBase []) Array.CreateInstance (typeof (SyntaxTreeBase), 2);
				children[0] = m__delimiter_terminal_ = p__anglr_file_fragment_.m__delimiter_terminal_;
				children[1] = m__delimiter_ = p__anglr_file_fragment_.m__delimiter_;
				break;
			case production_kind.g__anglr_file_fragment__53:
				children = (SyntaxTreeBase []) Array.CreateInstance (typeof (SyntaxTreeBase), 2);
				children[0] = m__attribute_list_optional_terminal_ = p__anglr_file_fragment_.m__attribute_list_optional_terminal_;
				children[1] = m__attribute_list_optional_ = p__anglr_file_fragment_.m__attribute_list_optional_;
				break;
			case production_kind.g__anglr_file_fragment__54:
				children = (SyntaxTreeBase []) Array.CreateInstance (typeof (SyntaxTreeBase), 2);
				children[0] = m__name_value_list_optional_terminal_ = p__anglr_file_fragment_.m__name_value_list_optional_terminal_;
				children[1] = m__name_value_list_optional_ = p__anglr_file_fragment_.m__name_value_list_optional_;
				break;
			case production_kind.g__anglr_file_fragment__55:
				children = (SyntaxTreeBase []) Array.CreateInstance (typeof (SyntaxTreeBase), 2);
				children[0] = m__anglr_file_part_list_optional_terminal_ = p__anglr_file_fragment_.m__anglr_file_part_list_optional_terminal_;
				children[1] = m__anglr_file_part_list_optional_ = p__anglr_file_fragment_.m__anglr_file_part_list_optional_;
				break;
			case production_kind.g__anglr_file_fragment__56:
				children = (SyntaxTreeBase []) Array.CreateInstance (typeof (SyntaxTreeBase), 2);
				children[0] = m__anglr_definition_list_optional_terminal_ = p__anglr_file_fragment_.m__anglr_definition_list_optional_terminal_;
				children[1] = m__anglr_definition_list_optional_ = p__anglr_file_fragment_.m__anglr_definition_list_optional_;
				break;
			case production_kind.g__anglr_file_fragment__57:
				children = (SyntaxTreeBase []) Array.CreateInstance (typeof (SyntaxTreeBase), 2);
				children[0] = m__block_terminal_definitions_optional_terminal_ = p__anglr_file_fragment_.m__block_terminal_definitions_optional_terminal_;
				children[1] = m__block_terminal_definitions_optional_ = p__anglr_file_fragment_.m__block_terminal_definitions_optional_;
				break;
			case production_kind.g__anglr_file_fragment__58:
				children = (SyntaxTreeBase []) Array.CreateInstance (typeof (SyntaxTreeBase), 2);
				children[0] = m__block_regex_definitions_optional_terminal_ = p__anglr_file_fragment_.m__block_regex_definitions_optional_terminal_;
				children[1] = m__block_regex_definitions_optional_ = p__anglr_file_fragment_.m__block_regex_definitions_optional_;
				break;
			case production_kind.g__anglr_file_fragment__59:
				children = (SyntaxTreeBase []) Array.CreateInstance (typeof (SyntaxTreeBase), 2);
				children[0] = m__regular_expression_list_optional_terminal_ = p__anglr_file_fragment_.m__regular_expression_list_optional_terminal_;
				children[1] = m__regular_expression_list_optional_ = p__anglr_file_fragment_.m__regular_expression_list_optional_;
				break;
			case production_kind.g__anglr_file_fragment__60:
				children = (SyntaxTreeBase []) Array.CreateInstance (typeof (SyntaxTreeBase), 2);
				children[0] = m__actions_optional_terminal_ = p__anglr_file_fragment_.m__actions_optional_terminal_;
				children[1] = m__actions_optional_ = p__anglr_file_fragment_.m__actions_optional_;
				break;
			case production_kind.g__anglr_file_fragment__61:
				children = (SyntaxTreeBase []) Array.CreateInstance (typeof (SyntaxTreeBase), 2);
				children[0] = m__anglr_syntax_rule_list_optional_terminal_ = p__anglr_file_fragment_.m__anglr_syntax_rule_list_optional_terminal_;
				children[1] = m__anglr_syntax_rule_list_optional_ = p__anglr_file_fragment_.m__anglr_syntax_rule_list_optional_;
				break;
			case production_kind.g__anglr_file_fragment__62:
				children = (SyntaxTreeBase []) Array.CreateInstance (typeof (SyntaxTreeBase), 2);
				children[0] = m__anglr_syntax_production_list_name_optional_terminal_ = p__anglr_file_fragment_.m__anglr_syntax_production_list_name_optional_terminal_;
				children[1] = m__anglr_syntax_production_list_name_optional_ = p__anglr_file_fragment_.m__anglr_syntax_production_list_name_optional_;
				break;
			case production_kind.g__anglr_file_fragment__63:
				children = (SyntaxTreeBase []) Array.CreateInstance (typeof (SyntaxTreeBase), 2);
				children[0] = m__production_name_optional_terminal_ = p__anglr_file_fragment_.m__production_name_optional_terminal_;
				children[1] = m__production_name_optional_ = p__anglr_file_fragment_.m__production_name_optional_;
				break;
			case production_kind.g__anglr_file_fragment__64:
				children = (SyntaxTreeBase []) Array.CreateInstance (typeof (SyntaxTreeBase), 2);
				children[0] = m__priority_assoc_specification_optional_terminal_ = p__anglr_file_fragment_.m__priority_assoc_specification_optional_terminal_;
				children[1] = m__priority_assoc_specification_optional_ = p__anglr_file_fragment_.m__priority_assoc_specification_optional_;
				break;
			case production_kind.g__anglr_file_fragment__65:
				children = (SyntaxTreeBase []) Array.CreateInstance (typeof (SyntaxTreeBase), 2);
				children[0] = m__marker_list_optional_terminal_ = p__anglr_file_fragment_.m__marker_list_optional_terminal_;
				children[1] = m__marker_list_optional_ = p__anglr_file_fragment_.m__marker_list_optional_;
				break;
			case production_kind.g__anglr_file_fragment__66:
				children = (SyntaxTreeBase []) Array.CreateInstance (typeof (SyntaxTreeBase), 2);
				children[0] = m__delimiter_optional_terminal_ = p__anglr_file_fragment_.m__delimiter_optional_terminal_;
				children[1] = m__delimiter_optional_ = p__anglr_file_fragment_.m__delimiter_optional_;
				break;
			case production_kind.g__anglr_file_fragment__67:
				children = (SyntaxTreeBase []) Array.CreateInstance (typeof (SyntaxTreeBase), 2);
				children[0] = m__cstring_optional_terminal_ = p__anglr_file_fragment_.m__cstring_optional_terminal_;
				children[1] = m__cstring_optional_ = p__anglr_file_fragment_.m__cstring_optional_;
				break;
			case production_kind.g__anglr_file_fragment__68:
				children = (SyntaxTreeBase []) Array.CreateInstance (typeof (SyntaxTreeBase), 2);
				children[0] = m__number_optional_terminal_ = p__anglr_file_fragment_.m__number_optional_terminal_;
				children[1] = m__number_optional_ = p__anglr_file_fragment_.m__number_optional_;
				break;
			default:
				string[] args = new string[] { "_anglr_file_fragment_" };
				throw new SyntaxTreeError (SyntaxTreeError.InvalidKindError, args);
			}
		}

		// Clone node - wrapper of copy constructor

		public override SyntaxTreeBase Clone () { return (SyntaxTreeBase) new _anglr_file_fragment_ (this); }

		// Destructor

		public new void Dispose ()
		{
			base.Dispose ();
			--g_counter;

			m__attribute_list_terminal_?.Dispose ();
			m__attribute_list_?.Dispose ();
			m__attribute_terminal_?.Dispose ();
			m__attribute_?.Dispose ();
			m__name_value_list_terminal_?.Dispose ();
			m__name_value_list_?.Dispose ();
			m__name_value_pair_terminal_?.Dispose ();
			m__name_value_pair_?.Dispose ();
			m__anglr_file_terminal_?.Dispose ();
			m__anglr_file_?.Dispose ();
			m__anglr_file_part_list_terminal_?.Dispose ();
			m__anglr_file_part_list_?.Dispose ();
			m__anglr_file_part_terminal_?.Dispose ();
			m__anglr_file_part_?.Dispose ();
			m__general_part_terminal_?.Dispose ();
			m__general_part_?.Dispose ();
			m__declaration_part_terminal_?.Dispose ();
			m__declaration_part_?.Dispose ();
			m__anglr_definition_list_terminal_?.Dispose ();
			m__anglr_definition_list_?.Dispose ();
			m__anglr_definition_with_attribute_list_terminal_?.Dispose ();
			m__anglr_definition_with_attribute_?.Dispose ();
			m__anglr_definition_terminal_?.Dispose ();
			m__anglr_definition_?.Dispose ();
			m__single_terminal_definition_terminal_?.Dispose ();
			m__single_terminal_definition_?.Dispose ();
			m__single_regex_definition_terminal_?.Dispose ();
			m__single_regex_definition_?.Dispose ();
			m__block_of_terminal_definitions_terminal_?.Dispose ();
			m__block_of_terminal_definitions_?.Dispose ();
			m__block_of_regex_definitions_terminal_?.Dispose ();
			m__block_of_regex_definitions_?.Dispose ();
			m__terminal_definition_terminal_?.Dispose ();
			m__terminal_definition_?.Dispose ();
			m__regex_definition_terminal_?.Dispose ();
			m__regex_definition_?.Dispose ();
			m__block_terminal_definitions_terminal_?.Dispose ();
			m__block_terminal_definitions_?.Dispose ();
			m__block_terminal_definition_terminal_?.Dispose ();
			m__block_terminal_definition_?.Dispose ();
			m__block_regex_definitions_terminal_?.Dispose ();
			m__block_regex_definitions_?.Dispose ();
			m__block_regex_definition_terminal_?.Dispose ();
			m__block_regex_definition_?.Dispose ();
			m__scanner_part_terminal_?.Dispose ();
			m__scanner_part_?.Dispose ();
			m__regular_expression_list_terminal_?.Dispose ();
			m__regular_expression_list_?.Dispose ();
			m__regular_expression_usage_terminal_?.Dispose ();
			m__regular_expression_usage_?.Dispose ();
			m__actions_terminal_?.Dispose ();
			m__actions_?.Dispose ();
			m__action_terminal_?.Dispose ();
			m__action_?.Dispose ();
			m__skip_action_terminal_?.Dispose ();
			m__skip_action_?.Dispose ();
			m__terminal_action_terminal_?.Dispose ();
			m__terminal_action_?.Dispose ();
			m__event_action_terminal_?.Dispose ();
			m__event_action_?.Dispose ();
			m__push_action_terminal_?.Dispose ();
			m__push_action_?.Dispose ();
			m__pop_action_terminal_?.Dispose ();
			m__pop_action_?.Dispose ();
			m__lexer_part_terminal_?.Dispose ();
			m__lexer_part_?.Dispose ();
			m__parser_part_terminal_?.Dispose ();
			m__parser_part_?.Dispose ();
			m__anglr_syntax_rule_list_terminal_?.Dispose ();
			m__anglr_syntax_rule_list_?.Dispose ();
			m__anglr_syntax_rule_terminal_?.Dispose ();
			m__anglr_syntax_rule_?.Dispose ();
			m__anglr_nested_rule_terminal_?.Dispose ();
			m__anglr_nested_rule_?.Dispose ();
			m__anglr_syntax_production_list_name_terminal_?.Dispose ();
			m__anglr_syntax_production_list_name_?.Dispose ();
			m__anglr_syntax_production_list_terminal_?.Dispose ();
			m__anglr_syntax_production_list_?.Dispose ();
			m__anglr_syntax_production_terminal_?.Dispose ();
			m__anglr_syntax_production_?.Dispose ();
			m__priority_assoc_specification_terminal_?.Dispose ();
			m__priority_assoc_specification_?.Dispose ();
			m__priority_specification_terminal_?.Dispose ();
			m__priority_specification_?.Dispose ();
			m__associativity_specification_terminal_?.Dispose ();
			m__associativity_specification_?.Dispose ();
			m__production_name_terminal_?.Dispose ();
			m__production_name_?.Dispose ();
			m__name_list_terminal_?.Dispose ();
			m__name_list_?.Dispose ();
			m__marker_list_terminal_?.Dispose ();
			m__marker_list_?.Dispose ();
			m__marker_terminal_?.Dispose ();
			m__marker_?.Dispose ();
			m__g_name_terminal_?.Dispose ();
			m__g_name_?.Dispose ();
			m__name_terminal_?.Dispose ();
			m__name_?.Dispose ();
			m__cardinality_delimiter_terminal_?.Dispose ();
			m__cardinality_delimiter_?.Dispose ();
			m__cardinality_terminal_?.Dispose ();
			m__cardinality_?.Dispose ();
			m__delimiter_terminal_?.Dispose ();
			m__delimiter_?.Dispose ();
			m__attribute_list_optional_terminal_?.Dispose ();
			m__attribute_list_optional_?.Dispose ();
			m__name_value_list_optional_terminal_?.Dispose ();
			m__name_value_list_optional_?.Dispose ();
			m__anglr_file_part_list_optional_terminal_?.Dispose ();
			m__anglr_file_part_list_optional_?.Dispose ();
			m__anglr_definition_list_optional_terminal_?.Dispose ();
			m__anglr_definition_list_optional_?.Dispose ();
			m__block_terminal_definitions_optional_terminal_?.Dispose ();
			m__block_terminal_definitions_optional_?.Dispose ();
			m__block_regex_definitions_optional_terminal_?.Dispose ();
			m__block_regex_definitions_optional_?.Dispose ();
			m__regular_expression_list_optional_terminal_?.Dispose ();
			m__regular_expression_list_optional_?.Dispose ();
			m__actions_optional_terminal_?.Dispose ();
			m__actions_optional_?.Dispose ();
			m__anglr_syntax_rule_list_optional_terminal_?.Dispose ();
			m__anglr_syntax_rule_list_optional_?.Dispose ();
			m__anglr_syntax_production_list_name_optional_terminal_?.Dispose ();
			m__anglr_syntax_production_list_name_optional_?.Dispose ();
			m__production_name_optional_terminal_?.Dispose ();
			m__production_name_optional_?.Dispose ();
			m__priority_assoc_specification_optional_terminal_?.Dispose ();
			m__priority_assoc_specification_optional_?.Dispose ();
			m__marker_list_optional_terminal_?.Dispose ();
			m__marker_list_optional_?.Dispose ();
			m__delimiter_optional_terminal_?.Dispose ();
			m__delimiter_optional_?.Dispose ();
			m__cstring_optional_terminal_?.Dispose ();
			m__cstring_optional_?.Dispose ();
			m__number_optional_terminal_?.Dispose ();
			m__number_optional_?.Dispose ();

			_init ();
		}
		#endregion
		#region Content changing and inclusion checking methods associated with the syntax rule <anglr file fragment>

		// Content changing function(s) associated with production(s) of syntax rule <anglr file fragment>

		//
		// Content changing function associated with following production(s)
		// <anglr file fragment> -> <attribute list terminal> <attribute list>

		//

		public void change(SyntaxTreeToken p_token, _attribute_list_ p__attribute_list_)
		{
			_init ();
			this.kind = (uint) production_kind.g__anglr_file_fragment__1;
			children = (SyntaxTreeBase []) Array.CreateInstance (typeof (SyntaxTreeBase), 2);
			children [0] = m__attribute_list_terminal_ = p_token;
			children [1] = m__attribute_list_ = p__attribute_list_;
			reparent (parent);
		}

		//
		// Content changing function associated with following production(s)
		// <anglr file fragment> -> <attribute terminal> <attribute>

		//

		public void change(SyntaxTreeToken p_token, _attribute_ p__attribute_)
		{
			_init ();
			this.kind = (uint) production_kind.g__anglr_file_fragment__2;
			children = (SyntaxTreeBase []) Array.CreateInstance (typeof (SyntaxTreeBase), 2);
			children [0] = m__attribute_terminal_ = p_token;
			children [1] = m__attribute_ = p__attribute_;
			reparent (parent);
		}

		//
		// Content changing function associated with following production(s)
		// <anglr file fragment> -> <name value list terminal> <name value list>

		//

		public void change(SyntaxTreeToken p_token, _name_value_list_ p__name_value_list_)
		{
			_init ();
			this.kind = (uint) production_kind.g__anglr_file_fragment__3;
			children = (SyntaxTreeBase []) Array.CreateInstance (typeof (SyntaxTreeBase), 2);
			children [0] = m__name_value_list_terminal_ = p_token;
			children [1] = m__name_value_list_ = p__name_value_list_;
			reparent (parent);
		}

		//
		// Content changing function associated with following production(s)
		// <anglr file fragment> -> <name value pair terminal> <name value pair>

		//

		public void change(SyntaxTreeToken p_token, _name_value_pair_ p__name_value_pair_)
		{
			_init ();
			this.kind = (uint) production_kind.g__anglr_file_fragment__4;
			children = (SyntaxTreeBase []) Array.CreateInstance (typeof (SyntaxTreeBase), 2);
			children [0] = m__name_value_pair_terminal_ = p_token;
			children [1] = m__name_value_pair_ = p__name_value_pair_;
			reparent (parent);
		}

		//
		// Content changing function associated with following production(s)
		// <anglr file fragment> -> <anglr file terminal> <anglr file>

		//

		public void change(SyntaxTreeToken p_token, _anglr_file_ p__anglr_file_)
		{
			_init ();
			this.kind = (uint) production_kind.g__anglr_file_fragment__5;
			children = (SyntaxTreeBase []) Array.CreateInstance (typeof (SyntaxTreeBase), 2);
			children [0] = m__anglr_file_terminal_ = p_token;
			children [1] = m__anglr_file_ = p__anglr_file_;
			reparent (parent);
		}

		//
		// Content changing function associated with following production(s)
		// <anglr file fragment> -> <anglr file part list terminal> <anglr file part list>

		//

		public void change(SyntaxTreeToken p_token, _anglr_file_part_list_ p__anglr_file_part_list_)
		{
			_init ();
			this.kind = (uint) production_kind.g__anglr_file_fragment__6;
			children = (SyntaxTreeBase []) Array.CreateInstance (typeof (SyntaxTreeBase), 2);
			children [0] = m__anglr_file_part_list_terminal_ = p_token;
			children [1] = m__anglr_file_part_list_ = p__anglr_file_part_list_;
			reparent (parent);
		}

		//
		// Content changing function associated with following production(s)
		// <anglr file fragment> -> <anglr file part terminal> <anglr file part>

		//

		public void change(SyntaxTreeToken p_token, _anglr_file_part_ p__anglr_file_part_)
		{
			_init ();
			this.kind = (uint) production_kind.g__anglr_file_fragment__7;
			children = (SyntaxTreeBase []) Array.CreateInstance (typeof (SyntaxTreeBase), 2);
			children [0] = m__anglr_file_part_terminal_ = p_token;
			children [1] = m__anglr_file_part_ = p__anglr_file_part_;
			reparent (parent);
		}

		//
		// Content changing function associated with following production(s)
		// <anglr file fragment> -> <general part terminal> <general part>

		//

		public void change(SyntaxTreeToken p_token, _general_part_ p__general_part_)
		{
			_init ();
			this.kind = (uint) production_kind.g__anglr_file_fragment__8;
			children = (SyntaxTreeBase []) Array.CreateInstance (typeof (SyntaxTreeBase), 2);
			children [0] = m__general_part_terminal_ = p_token;
			children [1] = m__general_part_ = p__general_part_;
			reparent (parent);
		}

		//
		// Content changing function associated with following production(s)
		// <anglr file fragment> -> <declaration part terminal> <declaration part>

		//

		public void change(SyntaxTreeToken p_token, _declaration_part_ p__declaration_part_)
		{
			_init ();
			this.kind = (uint) production_kind.g__anglr_file_fragment__9;
			children = (SyntaxTreeBase []) Array.CreateInstance (typeof (SyntaxTreeBase), 2);
			children [0] = m__declaration_part_terminal_ = p_token;
			children [1] = m__declaration_part_ = p__declaration_part_;
			reparent (parent);
		}

		//
		// Content changing function associated with following production(s)
		// <anglr file fragment> -> <anglr definition list terminal> <anglr definition list>

		//

		public void change(SyntaxTreeToken p_token, _anglr_definition_list_ p__anglr_definition_list_)
		{
			_init ();
			this.kind = (uint) production_kind.g__anglr_file_fragment__10;
			children = (SyntaxTreeBase []) Array.CreateInstance (typeof (SyntaxTreeBase), 2);
			children [0] = m__anglr_definition_list_terminal_ = p_token;
			children [1] = m__anglr_definition_list_ = p__anglr_definition_list_;
			reparent (parent);
		}

		//
		// Content changing function associated with following production(s)
		// <anglr file fragment> -> <anglr definition with attribute list terminal> <anglr definition with attribute>

		//

		public void change(SyntaxTreeToken p_token, _anglr_definition_with_attribute_ p__anglr_definition_with_attribute_)
		{
			_init ();
			this.kind = (uint) production_kind.g__anglr_file_fragment__11;
			children = (SyntaxTreeBase []) Array.CreateInstance (typeof (SyntaxTreeBase), 2);
			children [0] = m__anglr_definition_with_attribute_list_terminal_ = p_token;
			children [1] = m__anglr_definition_with_attribute_ = p__anglr_definition_with_attribute_;
			reparent (parent);
		}

		//
		// Content changing function associated with following production(s)
		// <anglr file fragment> -> <anglr definition terminal> <anglr definition>

		//

		public void change(SyntaxTreeToken p_token, _anglr_definition_ p__anglr_definition_)
		{
			_init ();
			this.kind = (uint) production_kind.g__anglr_file_fragment__12;
			children = (SyntaxTreeBase []) Array.CreateInstance (typeof (SyntaxTreeBase), 2);
			children [0] = m__anglr_definition_terminal_ = p_token;
			children [1] = m__anglr_definition_ = p__anglr_definition_;
			reparent (parent);
		}

		//
		// Content changing function associated with following production(s)
		// <anglr file fragment> -> <single terminal definition terminal> <single terminal definition>

		//

		public void change(SyntaxTreeToken p_token, _single_terminal_definition_ p__single_terminal_definition_)
		{
			_init ();
			this.kind = (uint) production_kind.g__anglr_file_fragment__13;
			children = (SyntaxTreeBase []) Array.CreateInstance (typeof (SyntaxTreeBase), 2);
			children [0] = m__single_terminal_definition_terminal_ = p_token;
			children [1] = m__single_terminal_definition_ = p__single_terminal_definition_;
			reparent (parent);
		}

		//
		// Content changing function associated with following production(s)
		// <anglr file fragment> -> <single regex definition terminal> <single regex definition>

		//

		public void change(SyntaxTreeToken p_token, _single_regex_definition_ p__single_regex_definition_)
		{
			_init ();
			this.kind = (uint) production_kind.g__anglr_file_fragment__14;
			children = (SyntaxTreeBase []) Array.CreateInstance (typeof (SyntaxTreeBase), 2);
			children [0] = m__single_regex_definition_terminal_ = p_token;
			children [1] = m__single_regex_definition_ = p__single_regex_definition_;
			reparent (parent);
		}

		//
		// Content changing function associated with following production(s)
		// <anglr file fragment> -> <block of terminal definitions terminal> <block of terminal definitions>

		//

		public void change(SyntaxTreeToken p_token, _block_of_terminal_definitions_ p__block_of_terminal_definitions_)
		{
			_init ();
			this.kind = (uint) production_kind.g__anglr_file_fragment__15;
			children = (SyntaxTreeBase []) Array.CreateInstance (typeof (SyntaxTreeBase), 2);
			children [0] = m__block_of_terminal_definitions_terminal_ = p_token;
			children [1] = m__block_of_terminal_definitions_ = p__block_of_terminal_definitions_;
			reparent (parent);
		}

		//
		// Content changing function associated with following production(s)
		// <anglr file fragment> -> <block of regex definitions terminal> <block of regex definitions>

		//

		public void change(SyntaxTreeToken p_token, _block_of_regex_definitions_ p__block_of_regex_definitions_)
		{
			_init ();
			this.kind = (uint) production_kind.g__anglr_file_fragment__16;
			children = (SyntaxTreeBase []) Array.CreateInstance (typeof (SyntaxTreeBase), 2);
			children [0] = m__block_of_regex_definitions_terminal_ = p_token;
			children [1] = m__block_of_regex_definitions_ = p__block_of_regex_definitions_;
			reparent (parent);
		}

		//
		// Content changing function associated with following production(s)
		// <anglr file fragment> -> <terminal definition terminal> <terminal definition>

		//

		public void change(SyntaxTreeToken p_token, _terminal_definition_ p__terminal_definition_)
		{
			_init ();
			this.kind = (uint) production_kind.g__anglr_file_fragment__17;
			children = (SyntaxTreeBase []) Array.CreateInstance (typeof (SyntaxTreeBase), 2);
			children [0] = m__terminal_definition_terminal_ = p_token;
			children [1] = m__terminal_definition_ = p__terminal_definition_;
			reparent (parent);
		}

		//
		// Content changing function associated with following production(s)
		// <anglr file fragment> -> <regex definition terminal> <regex definition>

		//

		public void change(SyntaxTreeToken p_token, _regex_definition_ p__regex_definition_)
		{
			_init ();
			this.kind = (uint) production_kind.g__anglr_file_fragment__18;
			children = (SyntaxTreeBase []) Array.CreateInstance (typeof (SyntaxTreeBase), 2);
			children [0] = m__regex_definition_terminal_ = p_token;
			children [1] = m__regex_definition_ = p__regex_definition_;
			reparent (parent);
		}

		//
		// Content changing function associated with following production(s)
		// <anglr file fragment> -> <block terminal definitions terminal> <block terminal definitions>

		//

		public void change(SyntaxTreeToken p_token, _block_terminal_definitions_ p__block_terminal_definitions_)
		{
			_init ();
			this.kind = (uint) production_kind.g__anglr_file_fragment__19;
			children = (SyntaxTreeBase []) Array.CreateInstance (typeof (SyntaxTreeBase), 2);
			children [0] = m__block_terminal_definitions_terminal_ = p_token;
			children [1] = m__block_terminal_definitions_ = p__block_terminal_definitions_;
			reparent (parent);
		}

		//
		// Content changing function associated with following production(s)
		// <anglr file fragment> -> <block terminal definition terminal> <block terminal definition>

		//

		public void change(SyntaxTreeToken p_token, _block_terminal_definition_ p__block_terminal_definition_)
		{
			_init ();
			this.kind = (uint) production_kind.g__anglr_file_fragment__20;
			children = (SyntaxTreeBase []) Array.CreateInstance (typeof (SyntaxTreeBase), 2);
			children [0] = m__block_terminal_definition_terminal_ = p_token;
			children [1] = m__block_terminal_definition_ = p__block_terminal_definition_;
			reparent (parent);
		}

		//
		// Content changing function associated with following production(s)
		// <anglr file fragment> -> <block regex definitions terminal> <block regex definitions>

		//

		public void change(SyntaxTreeToken p_token, _block_regex_definitions_ p__block_regex_definitions_)
		{
			_init ();
			this.kind = (uint) production_kind.g__anglr_file_fragment__21;
			children = (SyntaxTreeBase []) Array.CreateInstance (typeof (SyntaxTreeBase), 2);
			children [0] = m__block_regex_definitions_terminal_ = p_token;
			children [1] = m__block_regex_definitions_ = p__block_regex_definitions_;
			reparent (parent);
		}

		//
		// Content changing function associated with following production(s)
		// <anglr file fragment> -> <block regex definition terminal> <block regex definition>

		//

		public void change(SyntaxTreeToken p_token, _block_regex_definition_ p__block_regex_definition_)
		{
			_init ();
			this.kind = (uint) production_kind.g__anglr_file_fragment__22;
			children = (SyntaxTreeBase []) Array.CreateInstance (typeof (SyntaxTreeBase), 2);
			children [0] = m__block_regex_definition_terminal_ = p_token;
			children [1] = m__block_regex_definition_ = p__block_regex_definition_;
			reparent (parent);
		}

		//
		// Content changing function associated with following production(s)
		// <anglr file fragment> -> <scanner part terminal> <scanner part>

		//

		public void change(SyntaxTreeToken p_token, _scanner_part_ p__scanner_part_)
		{
			_init ();
			this.kind = (uint) production_kind.g__anglr_file_fragment__23;
			children = (SyntaxTreeBase []) Array.CreateInstance (typeof (SyntaxTreeBase), 2);
			children [0] = m__scanner_part_terminal_ = p_token;
			children [1] = m__scanner_part_ = p__scanner_part_;
			reparent (parent);
		}

		//
		// Content changing function associated with following production(s)
		// <anglr file fragment> -> <regular expression list terminal> <regular expression list>

		//

		public void change(SyntaxTreeToken p_token, _regular_expression_list_ p__regular_expression_list_)
		{
			_init ();
			this.kind = (uint) production_kind.g__anglr_file_fragment__24;
			children = (SyntaxTreeBase []) Array.CreateInstance (typeof (SyntaxTreeBase), 2);
			children [0] = m__regular_expression_list_terminal_ = p_token;
			children [1] = m__regular_expression_list_ = p__regular_expression_list_;
			reparent (parent);
		}

		//
		// Content changing function associated with following production(s)
		// <anglr file fragment> -> <regular expression usage terminal> <regular expression usage>

		//

		public void change(SyntaxTreeToken p_token, _regular_expression_usage_ p__regular_expression_usage_)
		{
			_init ();
			this.kind = (uint) production_kind.g__anglr_file_fragment__25;
			children = (SyntaxTreeBase []) Array.CreateInstance (typeof (SyntaxTreeBase), 2);
			children [0] = m__regular_expression_usage_terminal_ = p_token;
			children [1] = m__regular_expression_usage_ = p__regular_expression_usage_;
			reparent (parent);
		}

		//
		// Content changing function associated with following production(s)
		// <anglr file fragment> -> <actions terminal> <actions>

		//

		public void change(SyntaxTreeToken p_token, _actions_ p__actions_)
		{
			_init ();
			this.kind = (uint) production_kind.g__anglr_file_fragment__26;
			children = (SyntaxTreeBase []) Array.CreateInstance (typeof (SyntaxTreeBase), 2);
			children [0] = m__actions_terminal_ = p_token;
			children [1] = m__actions_ = p__actions_;
			reparent (parent);
		}

		//
		// Content changing function associated with following production(s)
		// <anglr file fragment> -> <action terminal> <action>

		//

		public void change(SyntaxTreeToken p_token, _action_ p__action_)
		{
			_init ();
			this.kind = (uint) production_kind.g__anglr_file_fragment__27;
			children = (SyntaxTreeBase []) Array.CreateInstance (typeof (SyntaxTreeBase), 2);
			children [0] = m__action_terminal_ = p_token;
			children [1] = m__action_ = p__action_;
			reparent (parent);
		}

		//
		// Content changing function associated with following production(s)
		// <anglr file fragment> -> <skip action terminal> <skip action>

		//

		public void change(SyntaxTreeToken p_token, _skip_action_ p__skip_action_)
		{
			_init ();
			this.kind = (uint) production_kind.g__anglr_file_fragment__28;
			children = (SyntaxTreeBase []) Array.CreateInstance (typeof (SyntaxTreeBase), 2);
			children [0] = m__skip_action_terminal_ = p_token;
			children [1] = m__skip_action_ = p__skip_action_;
			reparent (parent);
		}

		//
		// Content changing function associated with following production(s)
		// <anglr file fragment> -> <terminal action terminal> <terminal action>

		//

		public void change(SyntaxTreeToken p_token, _terminal_action_ p__terminal_action_)
		{
			_init ();
			this.kind = (uint) production_kind.g__anglr_file_fragment__29;
			children = (SyntaxTreeBase []) Array.CreateInstance (typeof (SyntaxTreeBase), 2);
			children [0] = m__terminal_action_terminal_ = p_token;
			children [1] = m__terminal_action_ = p__terminal_action_;
			reparent (parent);
		}

		//
		// Content changing function associated with following production(s)
		// <anglr file fragment> -> <event action terminal> <event action>

		//

		public void change(SyntaxTreeToken p_token, _event_action_ p__event_action_)
		{
			_init ();
			this.kind = (uint) production_kind.g__anglr_file_fragment__30;
			children = (SyntaxTreeBase []) Array.CreateInstance (typeof (SyntaxTreeBase), 2);
			children [0] = m__event_action_terminal_ = p_token;
			children [1] = m__event_action_ = p__event_action_;
			reparent (parent);
		}

		//
		// Content changing function associated with following production(s)
		// <anglr file fragment> -> <push action terminal> <push action>

		//

		public void change(SyntaxTreeToken p_token, _push_action_ p__push_action_)
		{
			_init ();
			this.kind = (uint) production_kind.g__anglr_file_fragment__31;
			children = (SyntaxTreeBase []) Array.CreateInstance (typeof (SyntaxTreeBase), 2);
			children [0] = m__push_action_terminal_ = p_token;
			children [1] = m__push_action_ = p__push_action_;
			reparent (parent);
		}

		//
		// Content changing function associated with following production(s)
		// <anglr file fragment> -> <pop action terminal> <pop action>

		//

		public void change(SyntaxTreeToken p_token, _pop_action_ p__pop_action_)
		{
			_init ();
			this.kind = (uint) production_kind.g__anglr_file_fragment__32;
			children = (SyntaxTreeBase []) Array.CreateInstance (typeof (SyntaxTreeBase), 2);
			children [0] = m__pop_action_terminal_ = p_token;
			children [1] = m__pop_action_ = p__pop_action_;
			reparent (parent);
		}

		//
		// Content changing function associated with following production(s)
		// <anglr file fragment> -> <lexer part terminal> <lexer part>

		//

		public void change(SyntaxTreeToken p_token, _lexer_part_ p__lexer_part_)
		{
			_init ();
			this.kind = (uint) production_kind.g__anglr_file_fragment__33;
			children = (SyntaxTreeBase []) Array.CreateInstance (typeof (SyntaxTreeBase), 2);
			children [0] = m__lexer_part_terminal_ = p_token;
			children [1] = m__lexer_part_ = p__lexer_part_;
			reparent (parent);
		}

		//
		// Content changing function associated with following production(s)
		// <anglr file fragment> -> <parser part terminal> <parser part>

		//

		public void change(SyntaxTreeToken p_token, _parser_part_ p__parser_part_)
		{
			_init ();
			this.kind = (uint) production_kind.g__anglr_file_fragment__34;
			children = (SyntaxTreeBase []) Array.CreateInstance (typeof (SyntaxTreeBase), 2);
			children [0] = m__parser_part_terminal_ = p_token;
			children [1] = m__parser_part_ = p__parser_part_;
			reparent (parent);
		}

		//
		// Content changing function associated with following production(s)
		// <anglr file fragment> -> <anglr syntax rule list terminal> <anglr syntax rule list>

		//

		public void change(SyntaxTreeToken p_token, _anglr_syntax_rule_list_ p__anglr_syntax_rule_list_)
		{
			_init ();
			this.kind = (uint) production_kind.g__anglr_file_fragment__35;
			children = (SyntaxTreeBase []) Array.CreateInstance (typeof (SyntaxTreeBase), 2);
			children [0] = m__anglr_syntax_rule_list_terminal_ = p_token;
			children [1] = m__anglr_syntax_rule_list_ = p__anglr_syntax_rule_list_;
			reparent (parent);
		}

		//
		// Content changing function associated with following production(s)
		// <anglr file fragment> -> <anglr syntax rule terminal> <anglr syntax rule>

		//

		public void change(SyntaxTreeToken p_token, _anglr_syntax_rule_ p__anglr_syntax_rule_)
		{
			_init ();
			this.kind = (uint) production_kind.g__anglr_file_fragment__36;
			children = (SyntaxTreeBase []) Array.CreateInstance (typeof (SyntaxTreeBase), 2);
			children [0] = m__anglr_syntax_rule_terminal_ = p_token;
			children [1] = m__anglr_syntax_rule_ = p__anglr_syntax_rule_;
			reparent (parent);
		}

		//
		// Content changing function associated with following production(s)
		// <anglr file fragment> -> <anglr nested rule terminal> <anglr nested rule>

		//

		public void change(SyntaxTreeToken p_token, _anglr_nested_rule_ p__anglr_nested_rule_)
		{
			_init ();
			this.kind = (uint) production_kind.g__anglr_file_fragment__37;
			children = (SyntaxTreeBase []) Array.CreateInstance (typeof (SyntaxTreeBase), 2);
			children [0] = m__anglr_nested_rule_terminal_ = p_token;
			children [1] = m__anglr_nested_rule_ = p__anglr_nested_rule_;
			reparent (parent);
		}

		//
		// Content changing function associated with following production(s)
		// <anglr file fragment> -> <anglr syntax production list name terminal> <anglr syntax production list name>

		//

		public void change(SyntaxTreeToken p_token, _anglr_syntax_production_list_name_ p__anglr_syntax_production_list_name_)
		{
			_init ();
			this.kind = (uint) production_kind.g__anglr_file_fragment__38;
			children = (SyntaxTreeBase []) Array.CreateInstance (typeof (SyntaxTreeBase), 2);
			children [0] = m__anglr_syntax_production_list_name_terminal_ = p_token;
			children [1] = m__anglr_syntax_production_list_name_ = p__anglr_syntax_production_list_name_;
			reparent (parent);
		}

		//
		// Content changing function associated with following production(s)
		// <anglr file fragment> -> <anglr syntax production list terminal> <anglr syntax production list>

		//

		public void change(SyntaxTreeToken p_token, _anglr_syntax_production_list_ p__anglr_syntax_production_list_)
		{
			_init ();
			this.kind = (uint) production_kind.g__anglr_file_fragment__39;
			children = (SyntaxTreeBase []) Array.CreateInstance (typeof (SyntaxTreeBase), 2);
			children [0] = m__anglr_syntax_production_list_terminal_ = p_token;
			children [1] = m__anglr_syntax_production_list_ = p__anglr_syntax_production_list_;
			reparent (parent);
		}

		//
		// Content changing function associated with following production(s)
		// <anglr file fragment> -> <anglr syntax production terminal> <anglr syntax production>

		//

		public void change(SyntaxTreeToken p_token, _anglr_syntax_production_ p__anglr_syntax_production_)
		{
			_init ();
			this.kind = (uint) production_kind.g__anglr_file_fragment__40;
			children = (SyntaxTreeBase []) Array.CreateInstance (typeof (SyntaxTreeBase), 2);
			children [0] = m__anglr_syntax_production_terminal_ = p_token;
			children [1] = m__anglr_syntax_production_ = p__anglr_syntax_production_;
			reparent (parent);
		}

		//
		// Content changing function associated with following production(s)
		// <anglr file fragment> -> <priority assoc specification terminal> <priority assoc specification>

		//

		public void change(SyntaxTreeToken p_token, _priority_assoc_specification_ p__priority_assoc_specification_)
		{
			_init ();
			this.kind = (uint) production_kind.g__anglr_file_fragment__41;
			children = (SyntaxTreeBase []) Array.CreateInstance (typeof (SyntaxTreeBase), 2);
			children [0] = m__priority_assoc_specification_terminal_ = p_token;
			children [1] = m__priority_assoc_specification_ = p__priority_assoc_specification_;
			reparent (parent);
		}

		//
		// Content changing function associated with following production(s)
		// <anglr file fragment> -> <priority specification terminal> <priority specification>

		//

		public void change(SyntaxTreeToken p_token, _priority_specification_ p__priority_specification_)
		{
			_init ();
			this.kind = (uint) production_kind.g__anglr_file_fragment__42;
			children = (SyntaxTreeBase []) Array.CreateInstance (typeof (SyntaxTreeBase), 2);
			children [0] = m__priority_specification_terminal_ = p_token;
			children [1] = m__priority_specification_ = p__priority_specification_;
			reparent (parent);
		}

		//
		// Content changing function associated with following production(s)
		// <anglr file fragment> -> <associativity specification terminal> <associativity specification>

		//

		public void change(SyntaxTreeToken p_token, _associativity_specification_ p__associativity_specification_)
		{
			_init ();
			this.kind = (uint) production_kind.g__anglr_file_fragment__43;
			children = (SyntaxTreeBase []) Array.CreateInstance (typeof (SyntaxTreeBase), 2);
			children [0] = m__associativity_specification_terminal_ = p_token;
			children [1] = m__associativity_specification_ = p__associativity_specification_;
			reparent (parent);
		}

		//
		// Content changing function associated with following production(s)
		// <anglr file fragment> -> <production name terminal> <production name>

		//

		public void change(SyntaxTreeToken p_token, _production_name_ p__production_name_)
		{
			_init ();
			this.kind = (uint) production_kind.g__anglr_file_fragment__44;
			children = (SyntaxTreeBase []) Array.CreateInstance (typeof (SyntaxTreeBase), 2);
			children [0] = m__production_name_terminal_ = p_token;
			children [1] = m__production_name_ = p__production_name_;
			reparent (parent);
		}

		//
		// Content changing function associated with following production(s)
		// <anglr file fragment> -> <name list terminal> <name list>

		//

		public void change(SyntaxTreeToken p_token, _name_list_ p__name_list_)
		{
			_init ();
			this.kind = (uint) production_kind.g__anglr_file_fragment__45;
			children = (SyntaxTreeBase []) Array.CreateInstance (typeof (SyntaxTreeBase), 2);
			children [0] = m__name_list_terminal_ = p_token;
			children [1] = m__name_list_ = p__name_list_;
			reparent (parent);
		}

		//
		// Content changing function associated with following production(s)
		// <anglr file fragment> -> <marker list terminal> <marker list>

		//

		public void change(SyntaxTreeToken p_token, _marker_list_ p__marker_list_)
		{
			_init ();
			this.kind = (uint) production_kind.g__anglr_file_fragment__46;
			children = (SyntaxTreeBase []) Array.CreateInstance (typeof (SyntaxTreeBase), 2);
			children [0] = m__marker_list_terminal_ = p_token;
			children [1] = m__marker_list_ = p__marker_list_;
			reparent (parent);
		}

		//
		// Content changing function associated with following production(s)
		// <anglr file fragment> -> <marker terminal> <marker>

		//

		public void change(SyntaxTreeToken p_token, _marker_ p__marker_)
		{
			_init ();
			this.kind = (uint) production_kind.g__anglr_file_fragment__47;
			children = (SyntaxTreeBase []) Array.CreateInstance (typeof (SyntaxTreeBase), 2);
			children [0] = m__marker_terminal_ = p_token;
			children [1] = m__marker_ = p__marker_;
			reparent (parent);
		}

		//
		// Content changing function associated with following production(s)
		// <anglr file fragment> -> <g name terminal> <g name>

		//

		public void change(SyntaxTreeToken p_token, _g_name_ p__g_name_)
		{
			_init ();
			this.kind = (uint) production_kind.g__anglr_file_fragment__48;
			children = (SyntaxTreeBase []) Array.CreateInstance (typeof (SyntaxTreeBase), 2);
			children [0] = m__g_name_terminal_ = p_token;
			children [1] = m__g_name_ = p__g_name_;
			reparent (parent);
		}

		//
		// Content changing function associated with following production(s)
		// <anglr file fragment> -> <name terminal> <name>

		//

		public void change(SyntaxTreeToken p_token, _name_ p__name_)
		{
			_init ();
			this.kind = (uint) production_kind.g__anglr_file_fragment__49;
			children = (SyntaxTreeBase []) Array.CreateInstance (typeof (SyntaxTreeBase), 2);
			children [0] = m__name_terminal_ = p_token;
			children [1] = m__name_ = p__name_;
			reparent (parent);
		}

		//
		// Content changing function associated with following production(s)
		// <anglr file fragment> -> <cardinality delimiter terminal> <cardinality delimiter>

		//

		public void change(SyntaxTreeToken p_token, _cardinality_delimiter_ p__cardinality_delimiter_)
		{
			_init ();
			this.kind = (uint) production_kind.g__anglr_file_fragment__50;
			children = (SyntaxTreeBase []) Array.CreateInstance (typeof (SyntaxTreeBase), 2);
			children [0] = m__cardinality_delimiter_terminal_ = p_token;
			children [1] = m__cardinality_delimiter_ = p__cardinality_delimiter_;
			reparent (parent);
		}

		//
		// Content changing function associated with following production(s)
		// <anglr file fragment> -> <cardinality terminal> <cardinality>

		//

		public void change(SyntaxTreeToken p_token, _cardinality_ p__cardinality_)
		{
			_init ();
			this.kind = (uint) production_kind.g__anglr_file_fragment__51;
			children = (SyntaxTreeBase []) Array.CreateInstance (typeof (SyntaxTreeBase), 2);
			children [0] = m__cardinality_terminal_ = p_token;
			children [1] = m__cardinality_ = p__cardinality_;
			reparent (parent);
		}

		//
		// Content changing function associated with following production(s)
		// <anglr file fragment> -> <delimiter terminal> <delimiter>

		//

		public void change(SyntaxTreeToken p_token, _delimiter_ p__delimiter_)
		{
			_init ();
			this.kind = (uint) production_kind.g__anglr_file_fragment__52;
			children = (SyntaxTreeBase []) Array.CreateInstance (typeof (SyntaxTreeBase), 2);
			children [0] = m__delimiter_terminal_ = p_token;
			children [1] = m__delimiter_ = p__delimiter_;
			reparent (parent);
		}

		//
		// Content changing function associated with following production(s)
		// <anglr file fragment> -> <attribute list optional terminal> <attribute list optional>

		//

		public void change(SyntaxTreeToken p_token, _attribute_list_optional_ p__attribute_list_optional_)
		{
			_init ();
			this.kind = (uint) production_kind.g__anglr_file_fragment__53;
			children = (SyntaxTreeBase []) Array.CreateInstance (typeof (SyntaxTreeBase), 2);
			children [0] = m__attribute_list_optional_terminal_ = p_token;
			children [1] = m__attribute_list_optional_ = p__attribute_list_optional_;
			reparent (parent);
		}

		//
		// Content changing function associated with following production(s)
		// <anglr file fragment> -> <name value list optional terminal> <name value list optional>

		//

		public void change(SyntaxTreeToken p_token, _name_value_list_optional_ p__name_value_list_optional_)
		{
			_init ();
			this.kind = (uint) production_kind.g__anglr_file_fragment__54;
			children = (SyntaxTreeBase []) Array.CreateInstance (typeof (SyntaxTreeBase), 2);
			children [0] = m__name_value_list_optional_terminal_ = p_token;
			children [1] = m__name_value_list_optional_ = p__name_value_list_optional_;
			reparent (parent);
		}

		//
		// Content changing function associated with following production(s)
		// <anglr file fragment> -> <anglr file part list optional terminal> <anglr file part list optional>

		//

		public void change(SyntaxTreeToken p_token, _anglr_file_part_list_optional_ p__anglr_file_part_list_optional_)
		{
			_init ();
			this.kind = (uint) production_kind.g__anglr_file_fragment__55;
			children = (SyntaxTreeBase []) Array.CreateInstance (typeof (SyntaxTreeBase), 2);
			children [0] = m__anglr_file_part_list_optional_terminal_ = p_token;
			children [1] = m__anglr_file_part_list_optional_ = p__anglr_file_part_list_optional_;
			reparent (parent);
		}

		//
		// Content changing function associated with following production(s)
		// <anglr file fragment> -> <anglr definition list optional terminal> <anglr definition list optional>

		//

		public void change(SyntaxTreeToken p_token, _anglr_definition_list_optional_ p__anglr_definition_list_optional_)
		{
			_init ();
			this.kind = (uint) production_kind.g__anglr_file_fragment__56;
			children = (SyntaxTreeBase []) Array.CreateInstance (typeof (SyntaxTreeBase), 2);
			children [0] = m__anglr_definition_list_optional_terminal_ = p_token;
			children [1] = m__anglr_definition_list_optional_ = p__anglr_definition_list_optional_;
			reparent (parent);
		}

		//
		// Content changing function associated with following production(s)
		// <anglr file fragment> -> <block terminal definitions optional terminal> <block terminal definitions optional>

		//

		public void change(SyntaxTreeToken p_token, _block_terminal_definitions_optional_ p__block_terminal_definitions_optional_)
		{
			_init ();
			this.kind = (uint) production_kind.g__anglr_file_fragment__57;
			children = (SyntaxTreeBase []) Array.CreateInstance (typeof (SyntaxTreeBase), 2);
			children [0] = m__block_terminal_definitions_optional_terminal_ = p_token;
			children [1] = m__block_terminal_definitions_optional_ = p__block_terminal_definitions_optional_;
			reparent (parent);
		}

		//
		// Content changing function associated with following production(s)
		// <anglr file fragment> -> <block regex definitions optional terminal> <block regex definitions optional>

		//

		public void change(SyntaxTreeToken p_token, _block_regex_definitions_optional_ p__block_regex_definitions_optional_)
		{
			_init ();
			this.kind = (uint) production_kind.g__anglr_file_fragment__58;
			children = (SyntaxTreeBase []) Array.CreateInstance (typeof (SyntaxTreeBase), 2);
			children [0] = m__block_regex_definitions_optional_terminal_ = p_token;
			children [1] = m__block_regex_definitions_optional_ = p__block_regex_definitions_optional_;
			reparent (parent);
		}

		//
		// Content changing function associated with following production(s)
		// <anglr file fragment> -> <regular expression list optional terminal> <regular expression list optional>

		//

		public void change(SyntaxTreeToken p_token, _regular_expression_list_optional_ p__regular_expression_list_optional_)
		{
			_init ();
			this.kind = (uint) production_kind.g__anglr_file_fragment__59;
			children = (SyntaxTreeBase []) Array.CreateInstance (typeof (SyntaxTreeBase), 2);
			children [0] = m__regular_expression_list_optional_terminal_ = p_token;
			children [1] = m__regular_expression_list_optional_ = p__regular_expression_list_optional_;
			reparent (parent);
		}

		//
		// Content changing function associated with following production(s)
		// <anglr file fragment> -> <actions optional terminal> <actions optional>

		//

		public void change(SyntaxTreeToken p_token, _actions_optional_ p__actions_optional_)
		{
			_init ();
			this.kind = (uint) production_kind.g__anglr_file_fragment__60;
			children = (SyntaxTreeBase []) Array.CreateInstance (typeof (SyntaxTreeBase), 2);
			children [0] = m__actions_optional_terminal_ = p_token;
			children [1] = m__actions_optional_ = p__actions_optional_;
			reparent (parent);
		}

		//
		// Content changing function associated with following production(s)
		// <anglr file fragment> -> <anglr syntax rule list optional terminal> <anglr syntax rule list optional>

		//

		public void change(SyntaxTreeToken p_token, _anglr_syntax_rule_list_optional_ p__anglr_syntax_rule_list_optional_)
		{
			_init ();
			this.kind = (uint) production_kind.g__anglr_file_fragment__61;
			children = (SyntaxTreeBase []) Array.CreateInstance (typeof (SyntaxTreeBase), 2);
			children [0] = m__anglr_syntax_rule_list_optional_terminal_ = p_token;
			children [1] = m__anglr_syntax_rule_list_optional_ = p__anglr_syntax_rule_list_optional_;
			reparent (parent);
		}

		//
		// Content changing function associated with following production(s)
		// <anglr file fragment> -> <anglr syntax production list name optional terminal> <anglr syntax production list name optional>

		//

		public void change(SyntaxTreeToken p_token, _anglr_syntax_production_list_name_optional_ p__anglr_syntax_production_list_name_optional_)
		{
			_init ();
			this.kind = (uint) production_kind.g__anglr_file_fragment__62;
			children = (SyntaxTreeBase []) Array.CreateInstance (typeof (SyntaxTreeBase), 2);
			children [0] = m__anglr_syntax_production_list_name_optional_terminal_ = p_token;
			children [1] = m__anglr_syntax_production_list_name_optional_ = p__anglr_syntax_production_list_name_optional_;
			reparent (parent);
		}

		//
		// Content changing function associated with following production(s)
		// <anglr file fragment> -> <production name optional terminal> <production name optional>

		//

		public void change(SyntaxTreeToken p_token, _production_name_optional_ p__production_name_optional_)
		{
			_init ();
			this.kind = (uint) production_kind.g__anglr_file_fragment__63;
			children = (SyntaxTreeBase []) Array.CreateInstance (typeof (SyntaxTreeBase), 2);
			children [0] = m__production_name_optional_terminal_ = p_token;
			children [1] = m__production_name_optional_ = p__production_name_optional_;
			reparent (parent);
		}

		//
		// Content changing function associated with following production(s)
		// <anglr file fragment> -> <priority assoc specification optional terminal> <priority assoc specification optional>

		//

		public void change(SyntaxTreeToken p_token, _priority_assoc_specification_optional_ p__priority_assoc_specification_optional_)
		{
			_init ();
			this.kind = (uint) production_kind.g__anglr_file_fragment__64;
			children = (SyntaxTreeBase []) Array.CreateInstance (typeof (SyntaxTreeBase), 2);
			children [0] = m__priority_assoc_specification_optional_terminal_ = p_token;
			children [1] = m__priority_assoc_specification_optional_ = p__priority_assoc_specification_optional_;
			reparent (parent);
		}

		//
		// Content changing function associated with following production(s)
		// <anglr file fragment> -> <marker list optional terminal> <marker list optional>

		//

		public void change(SyntaxTreeToken p_token, _marker_list_optional_ p__marker_list_optional_)
		{
			_init ();
			this.kind = (uint) production_kind.g__anglr_file_fragment__65;
			children = (SyntaxTreeBase []) Array.CreateInstance (typeof (SyntaxTreeBase), 2);
			children [0] = m__marker_list_optional_terminal_ = p_token;
			children [1] = m__marker_list_optional_ = p__marker_list_optional_;
			reparent (parent);
		}

		//
		// Content changing function associated with following production(s)
		// <anglr file fragment> -> <delimiter optional terminal> <delimiter optional>

		//

		public void change(SyntaxTreeToken p_token, _delimiter_optional_ p__delimiter_optional_)
		{
			_init ();
			this.kind = (uint) production_kind.g__anglr_file_fragment__66;
			children = (SyntaxTreeBase []) Array.CreateInstance (typeof (SyntaxTreeBase), 2);
			children [0] = m__delimiter_optional_terminal_ = p_token;
			children [1] = m__delimiter_optional_ = p__delimiter_optional_;
			reparent (parent);
		}

		//
		// Content changing function associated with following production(s)
		// <anglr file fragment> -> <cstring optional terminal> <cstring optional>

		//

		public void change(SyntaxTreeToken p_token, _cstring_optional_ p__cstring_optional_)
		{
			_init ();
			this.kind = (uint) production_kind.g__anglr_file_fragment__67;
			children = (SyntaxTreeBase []) Array.CreateInstance (typeof (SyntaxTreeBase), 2);
			children [0] = m__cstring_optional_terminal_ = p_token;
			children [1] = m__cstring_optional_ = p__cstring_optional_;
			reparent (parent);
		}

		//
		// Content changing function associated with following production(s)
		// <anglr file fragment> -> <number optional terminal> <number optional>

		//

		public void change(SyntaxTreeToken p_token, _number_optional_ p__number_optional_)
		{
			_init ();
			this.kind = (uint) production_kind.g__anglr_file_fragment__68;
			children = (SyntaxTreeBase []) Array.CreateInstance (typeof (SyntaxTreeBase), 2);
			children [0] = m__number_optional_terminal_ = p_token;
			children [1] = m__number_optional_ = p__number_optional_;
			reparent (parent);
		}

		//
		// check if node 'element' is included within syntax tree associated with syntax rule <anglr file fragment>
		//

		public new bool checkInclusion (SyntaxTreeBase element)
		{
			return
				(element == this) ||
				(m__attribute_list_terminal_ != null) && m__attribute_list_terminal_.checkInclusion (element) ||
				(m__attribute_list_ != null) && m__attribute_list_.checkInclusion (element) ||
				(m__attribute_terminal_ != null) && m__attribute_terminal_.checkInclusion (element) ||
				(m__attribute_ != null) && m__attribute_.checkInclusion (element) ||
				(m__name_value_list_terminal_ != null) && m__name_value_list_terminal_.checkInclusion (element) ||
				(m__name_value_list_ != null) && m__name_value_list_.checkInclusion (element) ||
				(m__name_value_pair_terminal_ != null) && m__name_value_pair_terminal_.checkInclusion (element) ||
				(m__name_value_pair_ != null) && m__name_value_pair_.checkInclusion (element) ||
				(m__anglr_file_terminal_ != null) && m__anglr_file_terminal_.checkInclusion (element) ||
				(m__anglr_file_ != null) && m__anglr_file_.checkInclusion (element) ||
				(m__anglr_file_part_list_terminal_ != null) && m__anglr_file_part_list_terminal_.checkInclusion (element) ||
				(m__anglr_file_part_list_ != null) && m__anglr_file_part_list_.checkInclusion (element) ||
				(m__anglr_file_part_terminal_ != null) && m__anglr_file_part_terminal_.checkInclusion (element) ||
				(m__anglr_file_part_ != null) && m__anglr_file_part_.checkInclusion (element) ||
				(m__general_part_terminal_ != null) && m__general_part_terminal_.checkInclusion (element) ||
				(m__general_part_ != null) && m__general_part_.checkInclusion (element) ||
				(m__declaration_part_terminal_ != null) && m__declaration_part_terminal_.checkInclusion (element) ||
				(m__declaration_part_ != null) && m__declaration_part_.checkInclusion (element) ||
				(m__anglr_definition_list_terminal_ != null) && m__anglr_definition_list_terminal_.checkInclusion (element) ||
				(m__anglr_definition_list_ != null) && m__anglr_definition_list_.checkInclusion (element) ||
				(m__anglr_definition_with_attribute_list_terminal_ != null) && m__anglr_definition_with_attribute_list_terminal_.checkInclusion (element) ||
				(m__anglr_definition_with_attribute_ != null) && m__anglr_definition_with_attribute_.checkInclusion (element) ||
				(m__anglr_definition_terminal_ != null) && m__anglr_definition_terminal_.checkInclusion (element) ||
				(m__anglr_definition_ != null) && m__anglr_definition_.checkInclusion (element) ||
				(m__single_terminal_definition_terminal_ != null) && m__single_terminal_definition_terminal_.checkInclusion (element) ||
				(m__single_terminal_definition_ != null) && m__single_terminal_definition_.checkInclusion (element) ||
				(m__single_regex_definition_terminal_ != null) && m__single_regex_definition_terminal_.checkInclusion (element) ||
				(m__single_regex_definition_ != null) && m__single_regex_definition_.checkInclusion (element) ||
				(m__block_of_terminal_definitions_terminal_ != null) && m__block_of_terminal_definitions_terminal_.checkInclusion (element) ||
				(m__block_of_terminal_definitions_ != null) && m__block_of_terminal_definitions_.checkInclusion (element) ||
				(m__block_of_regex_definitions_terminal_ != null) && m__block_of_regex_definitions_terminal_.checkInclusion (element) ||
				(m__block_of_regex_definitions_ != null) && m__block_of_regex_definitions_.checkInclusion (element) ||
				(m__terminal_definition_terminal_ != null) && m__terminal_definition_terminal_.checkInclusion (element) ||
				(m__terminal_definition_ != null) && m__terminal_definition_.checkInclusion (element) ||
				(m__regex_definition_terminal_ != null) && m__regex_definition_terminal_.checkInclusion (element) ||
				(m__regex_definition_ != null) && m__regex_definition_.checkInclusion (element) ||
				(m__block_terminal_definitions_terminal_ != null) && m__block_terminal_definitions_terminal_.checkInclusion (element) ||
				(m__block_terminal_definitions_ != null) && m__block_terminal_definitions_.checkInclusion (element) ||
				(m__block_terminal_definition_terminal_ != null) && m__block_terminal_definition_terminal_.checkInclusion (element) ||
				(m__block_terminal_definition_ != null) && m__block_terminal_definition_.checkInclusion (element) ||
				(m__block_regex_definitions_terminal_ != null) && m__block_regex_definitions_terminal_.checkInclusion (element) ||
				(m__block_regex_definitions_ != null) && m__block_regex_definitions_.checkInclusion (element) ||
				(m__block_regex_definition_terminal_ != null) && m__block_regex_definition_terminal_.checkInclusion (element) ||
				(m__block_regex_definition_ != null) && m__block_regex_definition_.checkInclusion (element) ||
				(m__scanner_part_terminal_ != null) && m__scanner_part_terminal_.checkInclusion (element) ||
				(m__scanner_part_ != null) && m__scanner_part_.checkInclusion (element) ||
				(m__regular_expression_list_terminal_ != null) && m__regular_expression_list_terminal_.checkInclusion (element) ||
				(m__regular_expression_list_ != null) && m__regular_expression_list_.checkInclusion (element) ||
				(m__regular_expression_usage_terminal_ != null) && m__regular_expression_usage_terminal_.checkInclusion (element) ||
				(m__regular_expression_usage_ != null) && m__regular_expression_usage_.checkInclusion (element) ||
				(m__actions_terminal_ != null) && m__actions_terminal_.checkInclusion (element) ||
				(m__actions_ != null) && m__actions_.checkInclusion (element) ||
				(m__action_terminal_ != null) && m__action_terminal_.checkInclusion (element) ||
				(m__action_ != null) && m__action_.checkInclusion (element) ||
				(m__skip_action_terminal_ != null) && m__skip_action_terminal_.checkInclusion (element) ||
				(m__skip_action_ != null) && m__skip_action_.checkInclusion (element) ||
				(m__terminal_action_terminal_ != null) && m__terminal_action_terminal_.checkInclusion (element) ||
				(m__terminal_action_ != null) && m__terminal_action_.checkInclusion (element) ||
				(m__event_action_terminal_ != null) && m__event_action_terminal_.checkInclusion (element) ||
				(m__event_action_ != null) && m__event_action_.checkInclusion (element) ||
				(m__push_action_terminal_ != null) && m__push_action_terminal_.checkInclusion (element) ||
				(m__push_action_ != null) && m__push_action_.checkInclusion (element) ||
				(m__pop_action_terminal_ != null) && m__pop_action_terminal_.checkInclusion (element) ||
				(m__pop_action_ != null) && m__pop_action_.checkInclusion (element) ||
				(m__lexer_part_terminal_ != null) && m__lexer_part_terminal_.checkInclusion (element) ||
				(m__lexer_part_ != null) && m__lexer_part_.checkInclusion (element) ||
				(m__parser_part_terminal_ != null) && m__parser_part_terminal_.checkInclusion (element) ||
				(m__parser_part_ != null) && m__parser_part_.checkInclusion (element) ||
				(m__anglr_syntax_rule_list_terminal_ != null) && m__anglr_syntax_rule_list_terminal_.checkInclusion (element) ||
				(m__anglr_syntax_rule_list_ != null) && m__anglr_syntax_rule_list_.checkInclusion (element) ||
				(m__anglr_syntax_rule_terminal_ != null) && m__anglr_syntax_rule_terminal_.checkInclusion (element) ||
				(m__anglr_syntax_rule_ != null) && m__anglr_syntax_rule_.checkInclusion (element) ||
				(m__anglr_nested_rule_terminal_ != null) && m__anglr_nested_rule_terminal_.checkInclusion (element) ||
				(m__anglr_nested_rule_ != null) && m__anglr_nested_rule_.checkInclusion (element) ||
				(m__anglr_syntax_production_list_name_terminal_ != null) && m__anglr_syntax_production_list_name_terminal_.checkInclusion (element) ||
				(m__anglr_syntax_production_list_name_ != null) && m__anglr_syntax_production_list_name_.checkInclusion (element) ||
				(m__anglr_syntax_production_list_terminal_ != null) && m__anglr_syntax_production_list_terminal_.checkInclusion (element) ||
				(m__anglr_syntax_production_list_ != null) && m__anglr_syntax_production_list_.checkInclusion (element) ||
				(m__anglr_syntax_production_terminal_ != null) && m__anglr_syntax_production_terminal_.checkInclusion (element) ||
				(m__anglr_syntax_production_ != null) && m__anglr_syntax_production_.checkInclusion (element) ||
				(m__priority_assoc_specification_terminal_ != null) && m__priority_assoc_specification_terminal_.checkInclusion (element) ||
				(m__priority_assoc_specification_ != null) && m__priority_assoc_specification_.checkInclusion (element) ||
				(m__priority_specification_terminal_ != null) && m__priority_specification_terminal_.checkInclusion (element) ||
				(m__priority_specification_ != null) && m__priority_specification_.checkInclusion (element) ||
				(m__associativity_specification_terminal_ != null) && m__associativity_specification_terminal_.checkInclusion (element) ||
				(m__associativity_specification_ != null) && m__associativity_specification_.checkInclusion (element) ||
				(m__production_name_terminal_ != null) && m__production_name_terminal_.checkInclusion (element) ||
				(m__production_name_ != null) && m__production_name_.checkInclusion (element) ||
				(m__name_list_terminal_ != null) && m__name_list_terminal_.checkInclusion (element) ||
				(m__name_list_ != null) && m__name_list_.checkInclusion (element) ||
				(m__marker_list_terminal_ != null) && m__marker_list_terminal_.checkInclusion (element) ||
				(m__marker_list_ != null) && m__marker_list_.checkInclusion (element) ||
				(m__marker_terminal_ != null) && m__marker_terminal_.checkInclusion (element) ||
				(m__marker_ != null) && m__marker_.checkInclusion (element) ||
				(m__g_name_terminal_ != null) && m__g_name_terminal_.checkInclusion (element) ||
				(m__g_name_ != null) && m__g_name_.checkInclusion (element) ||
				(m__name_terminal_ != null) && m__name_terminal_.checkInclusion (element) ||
				(m__name_ != null) && m__name_.checkInclusion (element) ||
				(m__cardinality_delimiter_terminal_ != null) && m__cardinality_delimiter_terminal_.checkInclusion (element) ||
				(m__cardinality_delimiter_ != null) && m__cardinality_delimiter_.checkInclusion (element) ||
				(m__cardinality_terminal_ != null) && m__cardinality_terminal_.checkInclusion (element) ||
				(m__cardinality_ != null) && m__cardinality_.checkInclusion (element) ||
				(m__delimiter_terminal_ != null) && m__delimiter_terminal_.checkInclusion (element) ||
				(m__delimiter_ != null) && m__delimiter_.checkInclusion (element) ||
				(m__attribute_list_optional_terminal_ != null) && m__attribute_list_optional_terminal_.checkInclusion (element) ||
				(m__attribute_list_optional_ != null) && m__attribute_list_optional_.checkInclusion (element) ||
				(m__name_value_list_optional_terminal_ != null) && m__name_value_list_optional_terminal_.checkInclusion (element) ||
				(m__name_value_list_optional_ != null) && m__name_value_list_optional_.checkInclusion (element) ||
				(m__anglr_file_part_list_optional_terminal_ != null) && m__anglr_file_part_list_optional_terminal_.checkInclusion (element) ||
				(m__anglr_file_part_list_optional_ != null) && m__anglr_file_part_list_optional_.checkInclusion (element) ||
				(m__anglr_definition_list_optional_terminal_ != null) && m__anglr_definition_list_optional_terminal_.checkInclusion (element) ||
				(m__anglr_definition_list_optional_ != null) && m__anglr_definition_list_optional_.checkInclusion (element) ||
				(m__block_terminal_definitions_optional_terminal_ != null) && m__block_terminal_definitions_optional_terminal_.checkInclusion (element) ||
				(m__block_terminal_definitions_optional_ != null) && m__block_terminal_definitions_optional_.checkInclusion (element) ||
				(m__block_regex_definitions_optional_terminal_ != null) && m__block_regex_definitions_optional_terminal_.checkInclusion (element) ||
				(m__block_regex_definitions_optional_ != null) && m__block_regex_definitions_optional_.checkInclusion (element) ||
				(m__regular_expression_list_optional_terminal_ != null) && m__regular_expression_list_optional_terminal_.checkInclusion (element) ||
				(m__regular_expression_list_optional_ != null) && m__regular_expression_list_optional_.checkInclusion (element) ||
				(m__actions_optional_terminal_ != null) && m__actions_optional_terminal_.checkInclusion (element) ||
				(m__actions_optional_ != null) && m__actions_optional_.checkInclusion (element) ||
				(m__anglr_syntax_rule_list_optional_terminal_ != null) && m__anglr_syntax_rule_list_optional_terminal_.checkInclusion (element) ||
				(m__anglr_syntax_rule_list_optional_ != null) && m__anglr_syntax_rule_list_optional_.checkInclusion (element) ||
				(m__anglr_syntax_production_list_name_optional_terminal_ != null) && m__anglr_syntax_production_list_name_optional_terminal_.checkInclusion (element) ||
				(m__anglr_syntax_production_list_name_optional_ != null) && m__anglr_syntax_production_list_name_optional_.checkInclusion (element) ||
				(m__production_name_optional_terminal_ != null) && m__production_name_optional_terminal_.checkInclusion (element) ||
				(m__production_name_optional_ != null) && m__production_name_optional_.checkInclusion (element) ||
				(m__priority_assoc_specification_optional_terminal_ != null) && m__priority_assoc_specification_optional_terminal_.checkInclusion (element) ||
				(m__priority_assoc_specification_optional_ != null) && m__priority_assoc_specification_optional_.checkInclusion (element) ||
				(m__marker_list_optional_terminal_ != null) && m__marker_list_optional_terminal_.checkInclusion (element) ||
				(m__marker_list_optional_ != null) && m__marker_list_optional_.checkInclusion (element) ||
				(m__delimiter_optional_terminal_ != null) && m__delimiter_optional_terminal_.checkInclusion (element) ||
				(m__delimiter_optional_ != null) && m__delimiter_optional_.checkInclusion (element) ||
				(m__cstring_optional_terminal_ != null) && m__cstring_optional_terminal_.checkInclusion (element) ||
				(m__cstring_optional_ != null) && m__cstring_optional_.checkInclusion (element) ||
				(m__number_optional_terminal_ != null) && m__number_optional_terminal_.checkInclusion (element) ||
				(m__number_optional_ != null) && m__number_optional_.checkInclusion (element) ||
				false;
		}
		#endregion
		#region Syntax tree emit methods associated with the syntax rule <anglr file fragment>

		//
		// emit production tree node associated with any production of syntax rule <anglr file fragment>
		//

		public override string Emit (int depth)
		{
			string s = "";
			if (depth != 0)
			switch ((production_kind) kind)
			{
				case production_kind.g__anglr_file_fragment__1:
					// emit syntax tree node associated with production
					// <anglr file fragment>: <attribute list terminal> <attribute list>

					s += m__attribute_list_terminal_.Emit (depth - 1);
					s += " ";
					s += m__attribute_list_.Emit (depth - 1);
				break;
				case production_kind.g__anglr_file_fragment__2:
					// emit syntax tree node associated with production
					// <anglr file fragment>: <attribute terminal> <attribute>

					s += m__attribute_terminal_.Emit (depth - 1);
					s += " ";
					s += m__attribute_.Emit (depth - 1);
				break;
				case production_kind.g__anglr_file_fragment__3:
					// emit syntax tree node associated with production
					// <anglr file fragment>: <name value list terminal> <name value list>

					s += m__name_value_list_terminal_.Emit (depth - 1);
					s += " ";
					s += m__name_value_list_.Emit (depth - 1);
				break;
				case production_kind.g__anglr_file_fragment__4:
					// emit syntax tree node associated with production
					// <anglr file fragment>: <name value pair terminal> <name value pair>

					s += m__name_value_pair_terminal_.Emit (depth - 1);
					s += " ";
					s += m__name_value_pair_.Emit (depth - 1);
				break;
				case production_kind.g__anglr_file_fragment__5:
					// emit syntax tree node associated with production
					// <anglr file fragment>: <anglr file terminal> <anglr file>

					s += m__anglr_file_terminal_.Emit (depth - 1);
					s += " ";
					s += m__anglr_file_.Emit (depth - 1);
				break;
				case production_kind.g__anglr_file_fragment__6:
					// emit syntax tree node associated with production
					// <anglr file fragment>: <anglr file part list terminal> <anglr file part list>

					s += m__anglr_file_part_list_terminal_.Emit (depth - 1);
					s += " ";
					s += m__anglr_file_part_list_.Emit (depth - 1);
				break;
				case production_kind.g__anglr_file_fragment__7:
					// emit syntax tree node associated with production
					// <anglr file fragment>: <anglr file part terminal> <anglr file part>

					s += m__anglr_file_part_terminal_.Emit (depth - 1);
					s += " ";
					s += m__anglr_file_part_.Emit (depth - 1);
				break;
				case production_kind.g__anglr_file_fragment__8:
					// emit syntax tree node associated with production
					// <anglr file fragment>: <general part terminal> <general part>

					s += m__general_part_terminal_.Emit (depth - 1);
					s += " ";
					s += m__general_part_.Emit (depth - 1);
				break;
				case production_kind.g__anglr_file_fragment__9:
					// emit syntax tree node associated with production
					// <anglr file fragment>: <declaration part terminal> <declaration part>

					s += m__declaration_part_terminal_.Emit (depth - 1);
					s += " ";
					s += m__declaration_part_.Emit (depth - 1);
				break;
				case production_kind.g__anglr_file_fragment__10:
					// emit syntax tree node associated with production
					// <anglr file fragment>: <anglr definition list terminal> <anglr definition list>

					s += m__anglr_definition_list_terminal_.Emit (depth - 1);
					s += " ";
					s += m__anglr_definition_list_.Emit (depth - 1);
				break;
				case production_kind.g__anglr_file_fragment__11:
					// emit syntax tree node associated with production
					// <anglr file fragment>: <anglr definition with attribute list terminal> <anglr definition with attribute>

					s += m__anglr_definition_with_attribute_list_terminal_.Emit (depth - 1);
					s += " ";
					s += m__anglr_definition_with_attribute_.Emit (depth - 1);
				break;
				case production_kind.g__anglr_file_fragment__12:
					// emit syntax tree node associated with production
					// <anglr file fragment>: <anglr definition terminal> <anglr definition>

					s += m__anglr_definition_terminal_.Emit (depth - 1);
					s += " ";
					s += m__anglr_definition_.Emit (depth - 1);
				break;
				case production_kind.g__anglr_file_fragment__13:
					// emit syntax tree node associated with production
					// <anglr file fragment>: <single terminal definition terminal> <single terminal definition>

					s += m__single_terminal_definition_terminal_.Emit (depth - 1);
					s += " ";
					s += m__single_terminal_definition_.Emit (depth - 1);
				break;
				case production_kind.g__anglr_file_fragment__14:
					// emit syntax tree node associated with production
					// <anglr file fragment>: <single regex definition terminal> <single regex definition>

					s += m__single_regex_definition_terminal_.Emit (depth - 1);
					s += " ";
					s += m__single_regex_definition_.Emit (depth - 1);
				break;
				case production_kind.g__anglr_file_fragment__15:
					// emit syntax tree node associated with production
					// <anglr file fragment>: <block of terminal definitions terminal> <block of terminal definitions>

					s += m__block_of_terminal_definitions_terminal_.Emit (depth - 1);
					s += " ";
					s += m__block_of_terminal_definitions_.Emit (depth - 1);
				break;
				case production_kind.g__anglr_file_fragment__16:
					// emit syntax tree node associated with production
					// <anglr file fragment>: <block of regex definitions terminal> <block of regex definitions>

					s += m__block_of_regex_definitions_terminal_.Emit (depth - 1);
					s += " ";
					s += m__block_of_regex_definitions_.Emit (depth - 1);
				break;
				case production_kind.g__anglr_file_fragment__17:
					// emit syntax tree node associated with production
					// <anglr file fragment>: <terminal definition terminal> <terminal definition>

					s += m__terminal_definition_terminal_.Emit (depth - 1);
					s += " ";
					s += m__terminal_definition_.Emit (depth - 1);
				break;
				case production_kind.g__anglr_file_fragment__18:
					// emit syntax tree node associated with production
					// <anglr file fragment>: <regex definition terminal> <regex definition>

					s += m__regex_definition_terminal_.Emit (depth - 1);
					s += " ";
					s += m__regex_definition_.Emit (depth - 1);
				break;
				case production_kind.g__anglr_file_fragment__19:
					// emit syntax tree node associated with production
					// <anglr file fragment>: <block terminal definitions terminal> <block terminal definitions>

					s += m__block_terminal_definitions_terminal_.Emit (depth - 1);
					s += " ";
					s += m__block_terminal_definitions_.Emit (depth - 1);
				break;
				case production_kind.g__anglr_file_fragment__20:
					// emit syntax tree node associated with production
					// <anglr file fragment>: <block terminal definition terminal> <block terminal definition>

					s += m__block_terminal_definition_terminal_.Emit (depth - 1);
					s += " ";
					s += m__block_terminal_definition_.Emit (depth - 1);
				break;
				case production_kind.g__anglr_file_fragment__21:
					// emit syntax tree node associated with production
					// <anglr file fragment>: <block regex definitions terminal> <block regex definitions>

					s += m__block_regex_definitions_terminal_.Emit (depth - 1);
					s += " ";
					s += m__block_regex_definitions_.Emit (depth - 1);
				break;
				case production_kind.g__anglr_file_fragment__22:
					// emit syntax tree node associated with production
					// <anglr file fragment>: <block regex definition terminal> <block regex definition>

					s += m__block_regex_definition_terminal_.Emit (depth - 1);
					s += " ";
					s += m__block_regex_definition_.Emit (depth - 1);
				break;
				case production_kind.g__anglr_file_fragment__23:
					// emit syntax tree node associated with production
					// <anglr file fragment>: <scanner part terminal> <scanner part>

					s += m__scanner_part_terminal_.Emit (depth - 1);
					s += " ";
					s += m__scanner_part_.Emit (depth - 1);
				break;
				case production_kind.g__anglr_file_fragment__24:
					// emit syntax tree node associated with production
					// <anglr file fragment>: <regular expression list terminal> <regular expression list>

					s += m__regular_expression_list_terminal_.Emit (depth - 1);
					s += " ";
					s += m__regular_expression_list_.Emit (depth - 1);
				break;
				case production_kind.g__anglr_file_fragment__25:
					// emit syntax tree node associated with production
					// <anglr file fragment>: <regular expression usage terminal> <regular expression usage>

					s += m__regular_expression_usage_terminal_.Emit (depth - 1);
					s += " ";
					s += m__regular_expression_usage_.Emit (depth - 1);
				break;
				case production_kind.g__anglr_file_fragment__26:
					// emit syntax tree node associated with production
					// <anglr file fragment>: <actions terminal> <actions>

					s += m__actions_terminal_.Emit (depth - 1);
					s += " ";
					s += m__actions_.Emit (depth - 1);
				break;
				case production_kind.g__anglr_file_fragment__27:
					// emit syntax tree node associated with production
					// <anglr file fragment>: <action terminal> <action>

					s += m__action_terminal_.Emit (depth - 1);
					s += " ";
					s += m__action_.Emit (depth - 1);
				break;
				case production_kind.g__anglr_file_fragment__28:
					// emit syntax tree node associated with production
					// <anglr file fragment>: <skip action terminal> <skip action>

					s += m__skip_action_terminal_.Emit (depth - 1);
					s += " ";
					s += m__skip_action_.Emit (depth - 1);
				break;
				case production_kind.g__anglr_file_fragment__29:
					// emit syntax tree node associated with production
					// <anglr file fragment>: <terminal action terminal> <terminal action>

					s += m__terminal_action_terminal_.Emit (depth - 1);
					s += " ";
					s += m__terminal_action_.Emit (depth - 1);
				break;
				case production_kind.g__anglr_file_fragment__30:
					// emit syntax tree node associated with production
					// <anglr file fragment>: <event action terminal> <event action>

					s += m__event_action_terminal_.Emit (depth - 1);
					s += " ";
					s += m__event_action_.Emit (depth - 1);
				break;
				case production_kind.g__anglr_file_fragment__31:
					// emit syntax tree node associated with production
					// <anglr file fragment>: <push action terminal> <push action>

					s += m__push_action_terminal_.Emit (depth - 1);
					s += " ";
					s += m__push_action_.Emit (depth - 1);
				break;
				case production_kind.g__anglr_file_fragment__32:
					// emit syntax tree node associated with production
					// <anglr file fragment>: <pop action terminal> <pop action>

					s += m__pop_action_terminal_.Emit (depth - 1);
					s += " ";
					s += m__pop_action_.Emit (depth - 1);
				break;
				case production_kind.g__anglr_file_fragment__33:
					// emit syntax tree node associated with production
					// <anglr file fragment>: <lexer part terminal> <lexer part>

					s += m__lexer_part_terminal_.Emit (depth - 1);
					s += " ";
					s += m__lexer_part_.Emit (depth - 1);
				break;
				case production_kind.g__anglr_file_fragment__34:
					// emit syntax tree node associated with production
					// <anglr file fragment>: <parser part terminal> <parser part>

					s += m__parser_part_terminal_.Emit (depth - 1);
					s += " ";
					s += m__parser_part_.Emit (depth - 1);
				break;
				case production_kind.g__anglr_file_fragment__35:
					// emit syntax tree node associated with production
					// <anglr file fragment>: <anglr syntax rule list terminal> <anglr syntax rule list>

					s += m__anglr_syntax_rule_list_terminal_.Emit (depth - 1);
					s += " ";
					s += m__anglr_syntax_rule_list_.Emit (depth - 1);
				break;
				case production_kind.g__anglr_file_fragment__36:
					// emit syntax tree node associated with production
					// <anglr file fragment>: <anglr syntax rule terminal> <anglr syntax rule>

					s += m__anglr_syntax_rule_terminal_.Emit (depth - 1);
					s += " ";
					s += m__anglr_syntax_rule_.Emit (depth - 1);
				break;
				case production_kind.g__anglr_file_fragment__37:
					// emit syntax tree node associated with production
					// <anglr file fragment>: <anglr nested rule terminal> <anglr nested rule>

					s += m__anglr_nested_rule_terminal_.Emit (depth - 1);
					s += " ";
					s += m__anglr_nested_rule_.Emit (depth - 1);
				break;
				case production_kind.g__anglr_file_fragment__38:
					// emit syntax tree node associated with production
					// <anglr file fragment>: <anglr syntax production list name terminal> <anglr syntax production list name>

					s += m__anglr_syntax_production_list_name_terminal_.Emit (depth - 1);
					s += " ";
					s += m__anglr_syntax_production_list_name_.Emit (depth - 1);
				break;
				case production_kind.g__anglr_file_fragment__39:
					// emit syntax tree node associated with production
					// <anglr file fragment>: <anglr syntax production list terminal> <anglr syntax production list>

					s += m__anglr_syntax_production_list_terminal_.Emit (depth - 1);
					s += " ";
					s += m__anglr_syntax_production_list_.Emit (depth - 1);
				break;
				case production_kind.g__anglr_file_fragment__40:
					// emit syntax tree node associated with production
					// <anglr file fragment>: <anglr syntax production terminal> <anglr syntax production>

					s += m__anglr_syntax_production_terminal_.Emit (depth - 1);
					s += " ";
					s += m__anglr_syntax_production_.Emit (depth - 1);
				break;
				case production_kind.g__anglr_file_fragment__41:
					// emit syntax tree node associated with production
					// <anglr file fragment>: <priority assoc specification terminal> <priority assoc specification>

					s += m__priority_assoc_specification_terminal_.Emit (depth - 1);
					s += " ";
					s += m__priority_assoc_specification_.Emit (depth - 1);
				break;
				case production_kind.g__anglr_file_fragment__42:
					// emit syntax tree node associated with production
					// <anglr file fragment>: <priority specification terminal> <priority specification>

					s += m__priority_specification_terminal_.Emit (depth - 1);
					s += " ";
					s += m__priority_specification_.Emit (depth - 1);
				break;
				case production_kind.g__anglr_file_fragment__43:
					// emit syntax tree node associated with production
					// <anglr file fragment>: <associativity specification terminal> <associativity specification>

					s += m__associativity_specification_terminal_.Emit (depth - 1);
					s += " ";
					s += m__associativity_specification_.Emit (depth - 1);
				break;
				case production_kind.g__anglr_file_fragment__44:
					// emit syntax tree node associated with production
					// <anglr file fragment>: <production name terminal> <production name>

					s += m__production_name_terminal_.Emit (depth - 1);
					s += " ";
					s += m__production_name_.Emit (depth - 1);
				break;
				case production_kind.g__anglr_file_fragment__45:
					// emit syntax tree node associated with production
					// <anglr file fragment>: <name list terminal> <name list>

					s += m__name_list_terminal_.Emit (depth - 1);
					s += " ";
					s += m__name_list_.Emit (depth - 1);
				break;
				case production_kind.g__anglr_file_fragment__46:
					// emit syntax tree node associated with production
					// <anglr file fragment>: <marker list terminal> <marker list>

					s += m__marker_list_terminal_.Emit (depth - 1);
					s += " ";
					s += m__marker_list_.Emit (depth - 1);
				break;
				case production_kind.g__anglr_file_fragment__47:
					// emit syntax tree node associated with production
					// <anglr file fragment>: <marker terminal> <marker>

					s += m__marker_terminal_.Emit (depth - 1);
					s += " ";
					s += m__marker_.Emit (depth - 1);
				break;
				case production_kind.g__anglr_file_fragment__48:
					// emit syntax tree node associated with production
					// <anglr file fragment>: <g name terminal> <g name>

					s += m__g_name_terminal_.Emit (depth - 1);
					s += " ";
					s += m__g_name_.Emit (depth - 1);
				break;
				case production_kind.g__anglr_file_fragment__49:
					// emit syntax tree node associated with production
					// <anglr file fragment>: <name terminal> <name>

					s += m__name_terminal_.Emit (depth - 1);
					s += " ";
					s += m__name_.Emit (depth - 1);
				break;
				case production_kind.g__anglr_file_fragment__50:
					// emit syntax tree node associated with production
					// <anglr file fragment>: <cardinality delimiter terminal> <cardinality delimiter>

					s += m__cardinality_delimiter_terminal_.Emit (depth - 1);
					s += " ";
					s += m__cardinality_delimiter_.Emit (depth - 1);
				break;
				case production_kind.g__anglr_file_fragment__51:
					// emit syntax tree node associated with production
					// <anglr file fragment>: <cardinality terminal> <cardinality>

					s += m__cardinality_terminal_.Emit (depth - 1);
					s += " ";
					s += m__cardinality_.Emit (depth - 1);
				break;
				case production_kind.g__anglr_file_fragment__52:
					// emit syntax tree node associated with production
					// <anglr file fragment>: <delimiter terminal> <delimiter>

					s += m__delimiter_terminal_.Emit (depth - 1);
					s += " ";
					s += m__delimiter_.Emit (depth - 1);
				break;
				case production_kind.g__anglr_file_fragment__53:
					// emit syntax tree node associated with production
					// <anglr file fragment>: <attribute list optional terminal> <attribute list optional>

					s += m__attribute_list_optional_terminal_.Emit (depth - 1);
					s += " ";
					s += m__attribute_list_optional_.Emit (depth - 1);
				break;
				case production_kind.g__anglr_file_fragment__54:
					// emit syntax tree node associated with production
					// <anglr file fragment>: <name value list optional terminal> <name value list optional>

					s += m__name_value_list_optional_terminal_.Emit (depth - 1);
					s += " ";
					s += m__name_value_list_optional_.Emit (depth - 1);
				break;
				case production_kind.g__anglr_file_fragment__55:
					// emit syntax tree node associated with production
					// <anglr file fragment>: <anglr file part list optional terminal> <anglr file part list optional>

					s += m__anglr_file_part_list_optional_terminal_.Emit (depth - 1);
					s += " ";
					s += m__anglr_file_part_list_optional_.Emit (depth - 1);
				break;
				case production_kind.g__anglr_file_fragment__56:
					// emit syntax tree node associated with production
					// <anglr file fragment>: <anglr definition list optional terminal> <anglr definition list optional>

					s += m__anglr_definition_list_optional_terminal_.Emit (depth - 1);
					s += " ";
					s += m__anglr_definition_list_optional_.Emit (depth - 1);
				break;
				case production_kind.g__anglr_file_fragment__57:
					// emit syntax tree node associated with production
					// <anglr file fragment>: <block terminal definitions optional terminal> <block terminal definitions optional>

					s += m__block_terminal_definitions_optional_terminal_.Emit (depth - 1);
					s += " ";
					s += m__block_terminal_definitions_optional_.Emit (depth - 1);
				break;
				case production_kind.g__anglr_file_fragment__58:
					// emit syntax tree node associated with production
					// <anglr file fragment>: <block regex definitions optional terminal> <block regex definitions optional>

					s += m__block_regex_definitions_optional_terminal_.Emit (depth - 1);
					s += " ";
					s += m__block_regex_definitions_optional_.Emit (depth - 1);
				break;
				case production_kind.g__anglr_file_fragment__59:
					// emit syntax tree node associated with production
					// <anglr file fragment>: <regular expression list optional terminal> <regular expression list optional>

					s += m__regular_expression_list_optional_terminal_.Emit (depth - 1);
					s += " ";
					s += m__regular_expression_list_optional_.Emit (depth - 1);
				break;
				case production_kind.g__anglr_file_fragment__60:
					// emit syntax tree node associated with production
					// <anglr file fragment>: <actions optional terminal> <actions optional>

					s += m__actions_optional_terminal_.Emit (depth - 1);
					s += " ";
					s += m__actions_optional_.Emit (depth - 1);
				break;
				case production_kind.g__anglr_file_fragment__61:
					// emit syntax tree node associated with production
					// <anglr file fragment>: <anglr syntax rule list optional terminal> <anglr syntax rule list optional>

					s += m__anglr_syntax_rule_list_optional_terminal_.Emit (depth - 1);
					s += " ";
					s += m__anglr_syntax_rule_list_optional_.Emit (depth - 1);
				break;
				case production_kind.g__anglr_file_fragment__62:
					// emit syntax tree node associated with production
					// <anglr file fragment>: <anglr syntax production list name optional terminal> <anglr syntax production list name optional>

					s += m__anglr_syntax_production_list_name_optional_terminal_.Emit (depth - 1);
					s += " ";
					s += m__anglr_syntax_production_list_name_optional_.Emit (depth - 1);
				break;
				case production_kind.g__anglr_file_fragment__63:
					// emit syntax tree node associated with production
					// <anglr file fragment>: <production name optional terminal> <production name optional>

					s += m__production_name_optional_terminal_.Emit (depth - 1);
					s += " ";
					s += m__production_name_optional_.Emit (depth - 1);
				break;
				case production_kind.g__anglr_file_fragment__64:
					// emit syntax tree node associated with production
					// <anglr file fragment>: <priority assoc specification optional terminal> <priority assoc specification optional>

					s += m__priority_assoc_specification_optional_terminal_.Emit (depth - 1);
					s += " ";
					s += m__priority_assoc_specification_optional_.Emit (depth - 1);
				break;
				case production_kind.g__anglr_file_fragment__65:
					// emit syntax tree node associated with production
					// <anglr file fragment>: <marker list optional terminal> <marker list optional>

					s += m__marker_list_optional_terminal_.Emit (depth - 1);
					s += " ";
					s += m__marker_list_optional_.Emit (depth - 1);
				break;
				case production_kind.g__anglr_file_fragment__66:
					// emit syntax tree node associated with production
					// <anglr file fragment>: <delimiter optional terminal> <delimiter optional>

					s += m__delimiter_optional_terminal_.Emit (depth - 1);
					s += " ";
					s += m__delimiter_optional_.Emit (depth - 1);
				break;
				case production_kind.g__anglr_file_fragment__67:
					// emit syntax tree node associated with production
					// <anglr file fragment>: <cstring optional terminal> <cstring optional>

					s += m__cstring_optional_terminal_.Emit (depth - 1);
					s += " ";
					s += m__cstring_optional_.Emit (depth - 1);
				break;
				case production_kind.g__anglr_file_fragment__68:
					// emit syntax tree node associated with production
					// <anglr file fragment>: <number optional terminal> <number optional>

					s += m__number_optional_terminal_.Emit (depth - 1);
					s += " ";
					s += m__number_optional_.Emit (depth - 1);
				break;
			}
			return s.Trim ();
		}

		//
		// emit production tree node associated with any production of syntax rule <anglr file fragment>
		//

		public override string EmitProductionTree (int depth)
		{
			string s = "_anglr_file_fragment_ (";
			if (depth != 0)
				switch ((production_kind) kind)
				{
				case production_kind.g__anglr_file_fragment__1:
					// emit syntax tree node associated with production
					// <anglr file fragment>: <attribute list terminal> <attribute list>

					s += "_attribute_list_terminal_";
					s += ' ';
					s += m__attribute_list_.EmitProductionTree (depth - 1);
				break;
				case production_kind.g__anglr_file_fragment__2:
					// emit syntax tree node associated with production
					// <anglr file fragment>: <attribute terminal> <attribute>

					s += "_attribute_terminal_";
					s += ' ';
					s += m__attribute_.EmitProductionTree (depth - 1);
				break;
				case production_kind.g__anglr_file_fragment__3:
					// emit syntax tree node associated with production
					// <anglr file fragment>: <name value list terminal> <name value list>

					s += "_name_value_list_terminal_";
					s += ' ';
					s += m__name_value_list_.EmitProductionTree (depth - 1);
				break;
				case production_kind.g__anglr_file_fragment__4:
					// emit syntax tree node associated with production
					// <anglr file fragment>: <name value pair terminal> <name value pair>

					s += "_name_value_pair_terminal_";
					s += ' ';
					s += m__name_value_pair_.EmitProductionTree (depth - 1);
				break;
				case production_kind.g__anglr_file_fragment__5:
					// emit syntax tree node associated with production
					// <anglr file fragment>: <anglr file terminal> <anglr file>

					s += "_anglr_file_terminal_";
					s += ' ';
					s += m__anglr_file_.EmitProductionTree (depth - 1);
				break;
				case production_kind.g__anglr_file_fragment__6:
					// emit syntax tree node associated with production
					// <anglr file fragment>: <anglr file part list terminal> <anglr file part list>

					s += "_anglr_file_part_list_terminal_";
					s += ' ';
					s += m__anglr_file_part_list_.EmitProductionTree (depth - 1);
				break;
				case production_kind.g__anglr_file_fragment__7:
					// emit syntax tree node associated with production
					// <anglr file fragment>: <anglr file part terminal> <anglr file part>

					s += "_anglr_file_part_terminal_";
					s += ' ';
					s += m__anglr_file_part_.EmitProductionTree (depth - 1);
				break;
				case production_kind.g__anglr_file_fragment__8:
					// emit syntax tree node associated with production
					// <anglr file fragment>: <general part terminal> <general part>

					s += "_general_part_terminal_";
					s += ' ';
					s += m__general_part_.EmitProductionTree (depth - 1);
				break;
				case production_kind.g__anglr_file_fragment__9:
					// emit syntax tree node associated with production
					// <anglr file fragment>: <declaration part terminal> <declaration part>

					s += "_declaration_part_terminal_";
					s += ' ';
					s += m__declaration_part_.EmitProductionTree (depth - 1);
				break;
				case production_kind.g__anglr_file_fragment__10:
					// emit syntax tree node associated with production
					// <anglr file fragment>: <anglr definition list terminal> <anglr definition list>

					s += "_anglr_definition_list_terminal_";
					s += ' ';
					s += m__anglr_definition_list_.EmitProductionTree (depth - 1);
				break;
				case production_kind.g__anglr_file_fragment__11:
					// emit syntax tree node associated with production
					// <anglr file fragment>: <anglr definition with attribute list terminal> <anglr definition with attribute>

					s += "_anglr_definition_with_attribute_list_terminal_";
					s += ' ';
					s += m__anglr_definition_with_attribute_.EmitProductionTree (depth - 1);
				break;
				case production_kind.g__anglr_file_fragment__12:
					// emit syntax tree node associated with production
					// <anglr file fragment>: <anglr definition terminal> <anglr definition>

					s += "_anglr_definition_terminal_";
					s += ' ';
					s += m__anglr_definition_.EmitProductionTree (depth - 1);
				break;
				case production_kind.g__anglr_file_fragment__13:
					// emit syntax tree node associated with production
					// <anglr file fragment>: <single terminal definition terminal> <single terminal definition>

					s += "_single_terminal_definition_terminal_";
					s += ' ';
					s += m__single_terminal_definition_.EmitProductionTree (depth - 1);
				break;
				case production_kind.g__anglr_file_fragment__14:
					// emit syntax tree node associated with production
					// <anglr file fragment>: <single regex definition terminal> <single regex definition>

					s += "_single_regex_definition_terminal_";
					s += ' ';
					s += m__single_regex_definition_.EmitProductionTree (depth - 1);
				break;
				case production_kind.g__anglr_file_fragment__15:
					// emit syntax tree node associated with production
					// <anglr file fragment>: <block of terminal definitions terminal> <block of terminal definitions>

					s += "_block_of_terminal_definitions_terminal_";
					s += ' ';
					s += m__block_of_terminal_definitions_.EmitProductionTree (depth - 1);
				break;
				case production_kind.g__anglr_file_fragment__16:
					// emit syntax tree node associated with production
					// <anglr file fragment>: <block of regex definitions terminal> <block of regex definitions>

					s += "_block_of_regex_definitions_terminal_";
					s += ' ';
					s += m__block_of_regex_definitions_.EmitProductionTree (depth - 1);
				break;
				case production_kind.g__anglr_file_fragment__17:
					// emit syntax tree node associated with production
					// <anglr file fragment>: <terminal definition terminal> <terminal definition>

					s += "_terminal_definition_terminal_";
					s += ' ';
					s += m__terminal_definition_.EmitProductionTree (depth - 1);
				break;
				case production_kind.g__anglr_file_fragment__18:
					// emit syntax tree node associated with production
					// <anglr file fragment>: <regex definition terminal> <regex definition>

					s += "_regex_definition_terminal_";
					s += ' ';
					s += m__regex_definition_.EmitProductionTree (depth - 1);
				break;
				case production_kind.g__anglr_file_fragment__19:
					// emit syntax tree node associated with production
					// <anglr file fragment>: <block terminal definitions terminal> <block terminal definitions>

					s += "_block_terminal_definitions_terminal_";
					s += ' ';
					s += m__block_terminal_definitions_.EmitProductionTree (depth - 1);
				break;
				case production_kind.g__anglr_file_fragment__20:
					// emit syntax tree node associated with production
					// <anglr file fragment>: <block terminal definition terminal> <block terminal definition>

					s += "_block_terminal_definition_terminal_";
					s += ' ';
					s += m__block_terminal_definition_.EmitProductionTree (depth - 1);
				break;
				case production_kind.g__anglr_file_fragment__21:
					// emit syntax tree node associated with production
					// <anglr file fragment>: <block regex definitions terminal> <block regex definitions>

					s += "_block_regex_definitions_terminal_";
					s += ' ';
					s += m__block_regex_definitions_.EmitProductionTree (depth - 1);
				break;
				case production_kind.g__anglr_file_fragment__22:
					// emit syntax tree node associated with production
					// <anglr file fragment>: <block regex definition terminal> <block regex definition>

					s += "_block_regex_definition_terminal_";
					s += ' ';
					s += m__block_regex_definition_.EmitProductionTree (depth - 1);
				break;
				case production_kind.g__anglr_file_fragment__23:
					// emit syntax tree node associated with production
					// <anglr file fragment>: <scanner part terminal> <scanner part>

					s += "_scanner_part_terminal_";
					s += ' ';
					s += m__scanner_part_.EmitProductionTree (depth - 1);
				break;
				case production_kind.g__anglr_file_fragment__24:
					// emit syntax tree node associated with production
					// <anglr file fragment>: <regular expression list terminal> <regular expression list>

					s += "_regular_expression_list_terminal_";
					s += ' ';
					s += m__regular_expression_list_.EmitProductionTree (depth - 1);
				break;
				case production_kind.g__anglr_file_fragment__25:
					// emit syntax tree node associated with production
					// <anglr file fragment>: <regular expression usage terminal> <regular expression usage>

					s += "_regular_expression_usage_terminal_";
					s += ' ';
					s += m__regular_expression_usage_.EmitProductionTree (depth - 1);
				break;
				case production_kind.g__anglr_file_fragment__26:
					// emit syntax tree node associated with production
					// <anglr file fragment>: <actions terminal> <actions>

					s += "_actions_terminal_";
					s += ' ';
					s += m__actions_.EmitProductionTree (depth - 1);
				break;
				case production_kind.g__anglr_file_fragment__27:
					// emit syntax tree node associated with production
					// <anglr file fragment>: <action terminal> <action>

					s += "_action_terminal_";
					s += ' ';
					s += m__action_.EmitProductionTree (depth - 1);
				break;
				case production_kind.g__anglr_file_fragment__28:
					// emit syntax tree node associated with production
					// <anglr file fragment>: <skip action terminal> <skip action>

					s += "_skip_action_terminal_";
					s += ' ';
					s += m__skip_action_.EmitProductionTree (depth - 1);
				break;
				case production_kind.g__anglr_file_fragment__29:
					// emit syntax tree node associated with production
					// <anglr file fragment>: <terminal action terminal> <terminal action>

					s += "_terminal_action_terminal_";
					s += ' ';
					s += m__terminal_action_.EmitProductionTree (depth - 1);
				break;
				case production_kind.g__anglr_file_fragment__30:
					// emit syntax tree node associated with production
					// <anglr file fragment>: <event action terminal> <event action>

					s += "_event_action_terminal_";
					s += ' ';
					s += m__event_action_.EmitProductionTree (depth - 1);
				break;
				case production_kind.g__anglr_file_fragment__31:
					// emit syntax tree node associated with production
					// <anglr file fragment>: <push action terminal> <push action>

					s += "_push_action_terminal_";
					s += ' ';
					s += m__push_action_.EmitProductionTree (depth - 1);
				break;
				case production_kind.g__anglr_file_fragment__32:
					// emit syntax tree node associated with production
					// <anglr file fragment>: <pop action terminal> <pop action>

					s += "_pop_action_terminal_";
					s += ' ';
					s += m__pop_action_.EmitProductionTree (depth - 1);
				break;
				case production_kind.g__anglr_file_fragment__33:
					// emit syntax tree node associated with production
					// <anglr file fragment>: <lexer part terminal> <lexer part>

					s += "_lexer_part_terminal_";
					s += ' ';
					s += m__lexer_part_.EmitProductionTree (depth - 1);
				break;
				case production_kind.g__anglr_file_fragment__34:
					// emit syntax tree node associated with production
					// <anglr file fragment>: <parser part terminal> <parser part>

					s += "_parser_part_terminal_";
					s += ' ';
					s += m__parser_part_.EmitProductionTree (depth - 1);
				break;
				case production_kind.g__anglr_file_fragment__35:
					// emit syntax tree node associated with production
					// <anglr file fragment>: <anglr syntax rule list terminal> <anglr syntax rule list>

					s += "_anglr_syntax_rule_list_terminal_";
					s += ' ';
					s += m__anglr_syntax_rule_list_.EmitProductionTree (depth - 1);
				break;
				case production_kind.g__anglr_file_fragment__36:
					// emit syntax tree node associated with production
					// <anglr file fragment>: <anglr syntax rule terminal> <anglr syntax rule>

					s += "_anglr_syntax_rule_terminal_";
					s += ' ';
					s += m__anglr_syntax_rule_.EmitProductionTree (depth - 1);
				break;
				case production_kind.g__anglr_file_fragment__37:
					// emit syntax tree node associated with production
					// <anglr file fragment>: <anglr nested rule terminal> <anglr nested rule>

					s += "_anglr_nested_rule_terminal_";
					s += ' ';
					s += m__anglr_nested_rule_.EmitProductionTree (depth - 1);
				break;
				case production_kind.g__anglr_file_fragment__38:
					// emit syntax tree node associated with production
					// <anglr file fragment>: <anglr syntax production list name terminal> <anglr syntax production list name>

					s += "_anglr_syntax_production_list_name_terminal_";
					s += ' ';
					s += m__anglr_syntax_production_list_name_.EmitProductionTree (depth - 1);
				break;
				case production_kind.g__anglr_file_fragment__39:
					// emit syntax tree node associated with production
					// <anglr file fragment>: <anglr syntax production list terminal> <anglr syntax production list>

					s += "_anglr_syntax_production_list_terminal_";
					s += ' ';
					s += m__anglr_syntax_production_list_.EmitProductionTree (depth - 1);
				break;
				case production_kind.g__anglr_file_fragment__40:
					// emit syntax tree node associated with production
					// <anglr file fragment>: <anglr syntax production terminal> <anglr syntax production>

					s += "_anglr_syntax_production_terminal_";
					s += ' ';
					s += m__anglr_syntax_production_.EmitProductionTree (depth - 1);
				break;
				case production_kind.g__anglr_file_fragment__41:
					// emit syntax tree node associated with production
					// <anglr file fragment>: <priority assoc specification terminal> <priority assoc specification>

					s += "_priority_assoc_specification_terminal_";
					s += ' ';
					s += m__priority_assoc_specification_.EmitProductionTree (depth - 1);
				break;
				case production_kind.g__anglr_file_fragment__42:
					// emit syntax tree node associated with production
					// <anglr file fragment>: <priority specification terminal> <priority specification>

					s += "_priority_specification_terminal_";
					s += ' ';
					s += m__priority_specification_.EmitProductionTree (depth - 1);
				break;
				case production_kind.g__anglr_file_fragment__43:
					// emit syntax tree node associated with production
					// <anglr file fragment>: <associativity specification terminal> <associativity specification>

					s += "_associativity_specification_terminal_";
					s += ' ';
					s += m__associativity_specification_.EmitProductionTree (depth - 1);
				break;
				case production_kind.g__anglr_file_fragment__44:
					// emit syntax tree node associated with production
					// <anglr file fragment>: <production name terminal> <production name>

					s += "_production_name_terminal_";
					s += ' ';
					s += m__production_name_.EmitProductionTree (depth - 1);
				break;
				case production_kind.g__anglr_file_fragment__45:
					// emit syntax tree node associated with production
					// <anglr file fragment>: <name list terminal> <name list>

					s += "_name_list_terminal_";
					s += ' ';
					s += m__name_list_.EmitProductionTree (depth - 1);
				break;
				case production_kind.g__anglr_file_fragment__46:
					// emit syntax tree node associated with production
					// <anglr file fragment>: <marker list terminal> <marker list>

					s += "_marker_list_terminal_";
					s += ' ';
					s += m__marker_list_.EmitProductionTree (depth - 1);
				break;
				case production_kind.g__anglr_file_fragment__47:
					// emit syntax tree node associated with production
					// <anglr file fragment>: <marker terminal> <marker>

					s += "_marker_terminal_";
					s += ' ';
					s += m__marker_.EmitProductionTree (depth - 1);
				break;
				case production_kind.g__anglr_file_fragment__48:
					// emit syntax tree node associated with production
					// <anglr file fragment>: <g name terminal> <g name>

					s += "_g_name_terminal_";
					s += ' ';
					s += m__g_name_.EmitProductionTree (depth - 1);
				break;
				case production_kind.g__anglr_file_fragment__49:
					// emit syntax tree node associated with production
					// <anglr file fragment>: <name terminal> <name>

					s += "_name_terminal_";
					s += ' ';
					s += m__name_.EmitProductionTree (depth - 1);
				break;
				case production_kind.g__anglr_file_fragment__50:
					// emit syntax tree node associated with production
					// <anglr file fragment>: <cardinality delimiter terminal> <cardinality delimiter>

					s += "_cardinality_delimiter_terminal_";
					s += ' ';
					s += m__cardinality_delimiter_.EmitProductionTree (depth - 1);
				break;
				case production_kind.g__anglr_file_fragment__51:
					// emit syntax tree node associated with production
					// <anglr file fragment>: <cardinality terminal> <cardinality>

					s += "_cardinality_terminal_";
					s += ' ';
					s += m__cardinality_.EmitProductionTree (depth - 1);
				break;
				case production_kind.g__anglr_file_fragment__52:
					// emit syntax tree node associated with production
					// <anglr file fragment>: <delimiter terminal> <delimiter>

					s += "_delimiter_terminal_";
					s += ' ';
					s += m__delimiter_.EmitProductionTree (depth - 1);
				break;
				case production_kind.g__anglr_file_fragment__53:
					// emit syntax tree node associated with production
					// <anglr file fragment>: <attribute list optional terminal> <attribute list optional>

					s += "_attribute_list_optional_terminal_";
					s += ' ';
					s += m__attribute_list_optional_.EmitProductionTree (depth - 1);
				break;
				case production_kind.g__anglr_file_fragment__54:
					// emit syntax tree node associated with production
					// <anglr file fragment>: <name value list optional terminal> <name value list optional>

					s += "_name_value_list_optional_terminal_";
					s += ' ';
					s += m__name_value_list_optional_.EmitProductionTree (depth - 1);
				break;
				case production_kind.g__anglr_file_fragment__55:
					// emit syntax tree node associated with production
					// <anglr file fragment>: <anglr file part list optional terminal> <anglr file part list optional>

					s += "_anglr_file_part_list_optional_terminal_";
					s += ' ';
					s += m__anglr_file_part_list_optional_.EmitProductionTree (depth - 1);
				break;
				case production_kind.g__anglr_file_fragment__56:
					// emit syntax tree node associated with production
					// <anglr file fragment>: <anglr definition list optional terminal> <anglr definition list optional>

					s += "_anglr_definition_list_optional_terminal_";
					s += ' ';
					s += m__anglr_definition_list_optional_.EmitProductionTree (depth - 1);
				break;
				case production_kind.g__anglr_file_fragment__57:
					// emit syntax tree node associated with production
					// <anglr file fragment>: <block terminal definitions optional terminal> <block terminal definitions optional>

					s += "_block_terminal_definitions_optional_terminal_";
					s += ' ';
					s += m__block_terminal_definitions_optional_.EmitProductionTree (depth - 1);
				break;
				case production_kind.g__anglr_file_fragment__58:
					// emit syntax tree node associated with production
					// <anglr file fragment>: <block regex definitions optional terminal> <block regex definitions optional>

					s += "_block_regex_definitions_optional_terminal_";
					s += ' ';
					s += m__block_regex_definitions_optional_.EmitProductionTree (depth - 1);
				break;
				case production_kind.g__anglr_file_fragment__59:
					// emit syntax tree node associated with production
					// <anglr file fragment>: <regular expression list optional terminal> <regular expression list optional>

					s += "_regular_expression_list_optional_terminal_";
					s += ' ';
					s += m__regular_expression_list_optional_.EmitProductionTree (depth - 1);
				break;
				case production_kind.g__anglr_file_fragment__60:
					// emit syntax tree node associated with production
					// <anglr file fragment>: <actions optional terminal> <actions optional>

					s += "_actions_optional_terminal_";
					s += ' ';
					s += m__actions_optional_.EmitProductionTree (depth - 1);
				break;
				case production_kind.g__anglr_file_fragment__61:
					// emit syntax tree node associated with production
					// <anglr file fragment>: <anglr syntax rule list optional terminal> <anglr syntax rule list optional>

					s += "_anglr_syntax_rule_list_optional_terminal_";
					s += ' ';
					s += m__anglr_syntax_rule_list_optional_.EmitProductionTree (depth - 1);
				break;
				case production_kind.g__anglr_file_fragment__62:
					// emit syntax tree node associated with production
					// <anglr file fragment>: <anglr syntax production list name optional terminal> <anglr syntax production list name optional>

					s += "_anglr_syntax_production_list_name_optional_terminal_";
					s += ' ';
					s += m__anglr_syntax_production_list_name_optional_.EmitProductionTree (depth - 1);
				break;
				case production_kind.g__anglr_file_fragment__63:
					// emit syntax tree node associated with production
					// <anglr file fragment>: <production name optional terminal> <production name optional>

					s += "_production_name_optional_terminal_";
					s += ' ';
					s += m__production_name_optional_.EmitProductionTree (depth - 1);
				break;
				case production_kind.g__anglr_file_fragment__64:
					// emit syntax tree node associated with production
					// <anglr file fragment>: <priority assoc specification optional terminal> <priority assoc specification optional>

					s += "_priority_assoc_specification_optional_terminal_";
					s += ' ';
					s += m__priority_assoc_specification_optional_.EmitProductionTree (depth - 1);
				break;
				case production_kind.g__anglr_file_fragment__65:
					// emit syntax tree node associated with production
					// <anglr file fragment>: <marker list optional terminal> <marker list optional>

					s += "_marker_list_optional_terminal_";
					s += ' ';
					s += m__marker_list_optional_.EmitProductionTree (depth - 1);
				break;
				case production_kind.g__anglr_file_fragment__66:
					// emit syntax tree node associated with production
					// <anglr file fragment>: <delimiter optional terminal> <delimiter optional>

					s += "_delimiter_optional_terminal_";
					s += ' ';
					s += m__delimiter_optional_.EmitProductionTree (depth - 1);
				break;
				case production_kind.g__anglr_file_fragment__67:
					// emit syntax tree node associated with production
					// <anglr file fragment>: <cstring optional terminal> <cstring optional>

					s += "_cstring_optional_terminal_";
					s += ' ';
					s += m__cstring_optional_.EmitProductionTree (depth - 1);
				break;
				case production_kind.g__anglr_file_fragment__68:
					// emit syntax tree node associated with production
					// <anglr file fragment>: <number optional terminal> <number optional>

					s += "_number_optional_terminal_";
					s += ' ';
					s += m__number_optional_.EmitProductionTree (depth - 1);
				break;
			}
			s += ")";
			return s;
		}
		#endregion
		#region Syntax tree traversal,  reperrent, init and iterator methods associated with the syntax rule <anglr file fragment>

		//
		// traverse sub-tree rooted in this node using selected syntax tree walker
		//

		public override void InvokeTraverse (SyntaxTreeWalkerCore syntaxTreeWalker) => ((SyntaxTreeWalker) syntaxTreeWalker).Traverse (this);
		public override void InvokeTraverseCommon (SyntaxTreeWalkerCore syntaxTreeWalker) => ((SyntaxTreeWalker) syntaxTreeWalker).TraverseCommon (this);

		// reparent sub-tree

		public override void reparent (SyntaxTreeBase parent)
		{
			this.parent = parent;

			m__attribute_list_terminal_?.reparent (this);
			m__attribute_list_?.reparent (this);
			m__attribute_terminal_?.reparent (this);
			m__attribute_?.reparent (this);
			m__name_value_list_terminal_?.reparent (this);
			m__name_value_list_?.reparent (this);
			m__name_value_pair_terminal_?.reparent (this);
			m__name_value_pair_?.reparent (this);
			m__anglr_file_terminal_?.reparent (this);
			m__anglr_file_?.reparent (this);
			m__anglr_file_part_list_terminal_?.reparent (this);
			m__anglr_file_part_list_?.reparent (this);
			m__anglr_file_part_terminal_?.reparent (this);
			m__anglr_file_part_?.reparent (this);
			m__general_part_terminal_?.reparent (this);
			m__general_part_?.reparent (this);
			m__declaration_part_terminal_?.reparent (this);
			m__declaration_part_?.reparent (this);
			m__anglr_definition_list_terminal_?.reparent (this);
			m__anglr_definition_list_?.reparent (this);
			m__anglr_definition_with_attribute_list_terminal_?.reparent (this);
			m__anglr_definition_with_attribute_?.reparent (this);
			m__anglr_definition_terminal_?.reparent (this);
			m__anglr_definition_?.reparent (this);
			m__single_terminal_definition_terminal_?.reparent (this);
			m__single_terminal_definition_?.reparent (this);
			m__single_regex_definition_terminal_?.reparent (this);
			m__single_regex_definition_?.reparent (this);
			m__block_of_terminal_definitions_terminal_?.reparent (this);
			m__block_of_terminal_definitions_?.reparent (this);
			m__block_of_regex_definitions_terminal_?.reparent (this);
			m__block_of_regex_definitions_?.reparent (this);
			m__terminal_definition_terminal_?.reparent (this);
			m__terminal_definition_?.reparent (this);
			m__regex_definition_terminal_?.reparent (this);
			m__regex_definition_?.reparent (this);
			m__block_terminal_definitions_terminal_?.reparent (this);
			m__block_terminal_definitions_?.reparent (this);
			m__block_terminal_definition_terminal_?.reparent (this);
			m__block_terminal_definition_?.reparent (this);
			m__block_regex_definitions_terminal_?.reparent (this);
			m__block_regex_definitions_?.reparent (this);
			m__block_regex_definition_terminal_?.reparent (this);
			m__block_regex_definition_?.reparent (this);
			m__scanner_part_terminal_?.reparent (this);
			m__scanner_part_?.reparent (this);
			m__regular_expression_list_terminal_?.reparent (this);
			m__regular_expression_list_?.reparent (this);
			m__regular_expression_usage_terminal_?.reparent (this);
			m__regular_expression_usage_?.reparent (this);
			m__actions_terminal_?.reparent (this);
			m__actions_?.reparent (this);
			m__action_terminal_?.reparent (this);
			m__action_?.reparent (this);
			m__skip_action_terminal_?.reparent (this);
			m__skip_action_?.reparent (this);
			m__terminal_action_terminal_?.reparent (this);
			m__terminal_action_?.reparent (this);
			m__event_action_terminal_?.reparent (this);
			m__event_action_?.reparent (this);
			m__push_action_terminal_?.reparent (this);
			m__push_action_?.reparent (this);
			m__pop_action_terminal_?.reparent (this);
			m__pop_action_?.reparent (this);
			m__lexer_part_terminal_?.reparent (this);
			m__lexer_part_?.reparent (this);
			m__parser_part_terminal_?.reparent (this);
			m__parser_part_?.reparent (this);
			m__anglr_syntax_rule_list_terminal_?.reparent (this);
			m__anglr_syntax_rule_list_?.reparent (this);
			m__anglr_syntax_rule_terminal_?.reparent (this);
			m__anglr_syntax_rule_?.reparent (this);
			m__anglr_nested_rule_terminal_?.reparent (this);
			m__anglr_nested_rule_?.reparent (this);
			m__anglr_syntax_production_list_name_terminal_?.reparent (this);
			m__anglr_syntax_production_list_name_?.reparent (this);
			m__anglr_syntax_production_list_terminal_?.reparent (this);
			m__anglr_syntax_production_list_?.reparent (this);
			m__anglr_syntax_production_terminal_?.reparent (this);
			m__anglr_syntax_production_?.reparent (this);
			m__priority_assoc_specification_terminal_?.reparent (this);
			m__priority_assoc_specification_?.reparent (this);
			m__priority_specification_terminal_?.reparent (this);
			m__priority_specification_?.reparent (this);
			m__associativity_specification_terminal_?.reparent (this);
			m__associativity_specification_?.reparent (this);
			m__production_name_terminal_?.reparent (this);
			m__production_name_?.reparent (this);
			m__name_list_terminal_?.reparent (this);
			m__name_list_?.reparent (this);
			m__marker_list_terminal_?.reparent (this);
			m__marker_list_?.reparent (this);
			m__marker_terminal_?.reparent (this);
			m__marker_?.reparent (this);
			m__g_name_terminal_?.reparent (this);
			m__g_name_?.reparent (this);
			m__name_terminal_?.reparent (this);
			m__name_?.reparent (this);
			m__cardinality_delimiter_terminal_?.reparent (this);
			m__cardinality_delimiter_?.reparent (this);
			m__cardinality_terminal_?.reparent (this);
			m__cardinality_?.reparent (this);
			m__delimiter_terminal_?.reparent (this);
			m__delimiter_?.reparent (this);
			m__attribute_list_optional_terminal_?.reparent (this);
			m__attribute_list_optional_?.reparent (this);
			m__name_value_list_optional_terminal_?.reparent (this);
			m__name_value_list_optional_?.reparent (this);
			m__anglr_file_part_list_optional_terminal_?.reparent (this);
			m__anglr_file_part_list_optional_?.reparent (this);
			m__anglr_definition_list_optional_terminal_?.reparent (this);
			m__anglr_definition_list_optional_?.reparent (this);
			m__block_terminal_definitions_optional_terminal_?.reparent (this);
			m__block_terminal_definitions_optional_?.reparent (this);
			m__block_regex_definitions_optional_terminal_?.reparent (this);
			m__block_regex_definitions_optional_?.reparent (this);
			m__regular_expression_list_optional_terminal_?.reparent (this);
			m__regular_expression_list_optional_?.reparent (this);
			m__actions_optional_terminal_?.reparent (this);
			m__actions_optional_?.reparent (this);
			m__anglr_syntax_rule_list_optional_terminal_?.reparent (this);
			m__anglr_syntax_rule_list_optional_?.reparent (this);
			m__anglr_syntax_production_list_name_optional_terminal_?.reparent (this);
			m__anglr_syntax_production_list_name_optional_?.reparent (this);
			m__production_name_optional_terminal_?.reparent (this);
			m__production_name_optional_?.reparent (this);
			m__priority_assoc_specification_optional_terminal_?.reparent (this);
			m__priority_assoc_specification_optional_?.reparent (this);
			m__marker_list_optional_terminal_?.reparent (this);
			m__marker_list_optional_?.reparent (this);
			m__delimiter_optional_terminal_?.reparent (this);
			m__delimiter_optional_?.reparent (this);
			m__cstring_optional_terminal_?.reparent (this);
			m__cstring_optional_?.reparent (this);
			m__number_optional_terminal_?.reparent (this);
			m__number_optional_?.reparent (this);
		}

		//
		// initialize object associated with syntax rule <anglr file fragment>
		//

		public void _init ()
		{
			m__attribute_list_terminal_ = null;
			m__attribute_list_ = null;
			m__attribute_terminal_ = null;
			m__attribute_ = null;
			m__name_value_list_terminal_ = null;
			m__name_value_list_ = null;
			m__name_value_pair_terminal_ = null;
			m__name_value_pair_ = null;
			m__anglr_file_terminal_ = null;
			m__anglr_file_ = null;
			m__anglr_file_part_list_terminal_ = null;
			m__anglr_file_part_list_ = null;
			m__anglr_file_part_terminal_ = null;
			m__anglr_file_part_ = null;
			m__general_part_terminal_ = null;
			m__general_part_ = null;
			m__declaration_part_terminal_ = null;
			m__declaration_part_ = null;
			m__anglr_definition_list_terminal_ = null;
			m__anglr_definition_list_ = null;
			m__anglr_definition_with_attribute_list_terminal_ = null;
			m__anglr_definition_with_attribute_ = null;
			m__anglr_definition_terminal_ = null;
			m__anglr_definition_ = null;
			m__single_terminal_definition_terminal_ = null;
			m__single_terminal_definition_ = null;
			m__single_regex_definition_terminal_ = null;
			m__single_regex_definition_ = null;
			m__block_of_terminal_definitions_terminal_ = null;
			m__block_of_terminal_definitions_ = null;
			m__block_of_regex_definitions_terminal_ = null;
			m__block_of_regex_definitions_ = null;
			m__terminal_definition_terminal_ = null;
			m__terminal_definition_ = null;
			m__regex_definition_terminal_ = null;
			m__regex_definition_ = null;
			m__block_terminal_definitions_terminal_ = null;
			m__block_terminal_definitions_ = null;
			m__block_terminal_definition_terminal_ = null;
			m__block_terminal_definition_ = null;
			m__block_regex_definitions_terminal_ = null;
			m__block_regex_definitions_ = null;
			m__block_regex_definition_terminal_ = null;
			m__block_regex_definition_ = null;
			m__scanner_part_terminal_ = null;
			m__scanner_part_ = null;
			m__regular_expression_list_terminal_ = null;
			m__regular_expression_list_ = null;
			m__regular_expression_usage_terminal_ = null;
			m__regular_expression_usage_ = null;
			m__actions_terminal_ = null;
			m__actions_ = null;
			m__action_terminal_ = null;
			m__action_ = null;
			m__skip_action_terminal_ = null;
			m__skip_action_ = null;
			m__terminal_action_terminal_ = null;
			m__terminal_action_ = null;
			m__event_action_terminal_ = null;
			m__event_action_ = null;
			m__push_action_terminal_ = null;
			m__push_action_ = null;
			m__pop_action_terminal_ = null;
			m__pop_action_ = null;
			m__lexer_part_terminal_ = null;
			m__lexer_part_ = null;
			m__parser_part_terminal_ = null;
			m__parser_part_ = null;
			m__anglr_syntax_rule_list_terminal_ = null;
			m__anglr_syntax_rule_list_ = null;
			m__anglr_syntax_rule_terminal_ = null;
			m__anglr_syntax_rule_ = null;
			m__anglr_nested_rule_terminal_ = null;
			m__anglr_nested_rule_ = null;
			m__anglr_syntax_production_list_name_terminal_ = null;
			m__anglr_syntax_production_list_name_ = null;
			m__anglr_syntax_production_list_terminal_ = null;
			m__anglr_syntax_production_list_ = null;
			m__anglr_syntax_production_terminal_ = null;
			m__anglr_syntax_production_ = null;
			m__priority_assoc_specification_terminal_ = null;
			m__priority_assoc_specification_ = null;
			m__priority_specification_terminal_ = null;
			m__priority_specification_ = null;
			m__associativity_specification_terminal_ = null;
			m__associativity_specification_ = null;
			m__production_name_terminal_ = null;
			m__production_name_ = null;
			m__name_list_terminal_ = null;
			m__name_list_ = null;
			m__marker_list_terminal_ = null;
			m__marker_list_ = null;
			m__marker_terminal_ = null;
			m__marker_ = null;
			m__g_name_terminal_ = null;
			m__g_name_ = null;
			m__name_terminal_ = null;
			m__name_ = null;
			m__cardinality_delimiter_terminal_ = null;
			m__cardinality_delimiter_ = null;
			m__cardinality_terminal_ = null;
			m__cardinality_ = null;
			m__delimiter_terminal_ = null;
			m__delimiter_ = null;
			m__attribute_list_optional_terminal_ = null;
			m__attribute_list_optional_ = null;
			m__name_value_list_optional_terminal_ = null;
			m__name_value_list_optional_ = null;
			m__anglr_file_part_list_optional_terminal_ = null;
			m__anglr_file_part_list_optional_ = null;
			m__anglr_definition_list_optional_terminal_ = null;
			m__anglr_definition_list_optional_ = null;
			m__block_terminal_definitions_optional_terminal_ = null;
			m__block_terminal_definitions_optional_ = null;
			m__block_regex_definitions_optional_terminal_ = null;
			m__block_regex_definitions_optional_ = null;
			m__regular_expression_list_optional_terminal_ = null;
			m__regular_expression_list_optional_ = null;
			m__actions_optional_terminal_ = null;
			m__actions_optional_ = null;
			m__anglr_syntax_rule_list_optional_terminal_ = null;
			m__anglr_syntax_rule_list_optional_ = null;
			m__anglr_syntax_production_list_name_optional_terminal_ = null;
			m__anglr_syntax_production_list_name_optional_ = null;
			m__production_name_optional_terminal_ = null;
			m__production_name_optional_ = null;
			m__priority_assoc_specification_optional_terminal_ = null;
			m__priority_assoc_specification_optional_ = null;
			m__marker_list_optional_terminal_ = null;
			m__marker_list_optional_ = null;
			m__delimiter_optional_terminal_ = null;
			m__delimiter_optional_ = null;
			m__cstring_optional_terminal_ = null;
			m__cstring_optional_ = null;
			m__number_optional_terminal_ = null;
			m__number_optional_ = null;
		}
		#endregion
		#region Fields and properties associated with the syntax rule <anglr file fragment>

		// counter of all nodes associated with syntax rule <anglr file fragment>
		public static int g_counter;

		// objects associated with terminal and non-terminal symbols within production(s) of syntax rule <anglr file fragment>
		public SyntaxTreeToken m__attribute_list_terminal_ { get; private set; }
		public _attribute_list_ m__attribute_list_ { get; private set; }
		public SyntaxTreeToken m__attribute_terminal_ { get; private set; }
		public _attribute_ m__attribute_ { get; private set; }
		public SyntaxTreeToken m__name_value_list_terminal_ { get; private set; }
		public _name_value_list_ m__name_value_list_ { get; private set; }
		public SyntaxTreeToken m__name_value_pair_terminal_ { get; private set; }
		public _name_value_pair_ m__name_value_pair_ { get; private set; }
		public SyntaxTreeToken m__anglr_file_terminal_ { get; private set; }
		public _anglr_file_ m__anglr_file_ { get; private set; }
		public SyntaxTreeToken m__anglr_file_part_list_terminal_ { get; private set; }
		public _anglr_file_part_list_ m__anglr_file_part_list_ { get; private set; }
		public SyntaxTreeToken m__anglr_file_part_terminal_ { get; private set; }
		public _anglr_file_part_ m__anglr_file_part_ { get; private set; }
		public SyntaxTreeToken m__general_part_terminal_ { get; private set; }
		public _general_part_ m__general_part_ { get; private set; }
		public SyntaxTreeToken m__declaration_part_terminal_ { get; private set; }
		public _declaration_part_ m__declaration_part_ { get; private set; }
		public SyntaxTreeToken m__anglr_definition_list_terminal_ { get; private set; }
		public _anglr_definition_list_ m__anglr_definition_list_ { get; private set; }
		public SyntaxTreeToken m__anglr_definition_with_attribute_list_terminal_ { get; private set; }
		public _anglr_definition_with_attribute_ m__anglr_definition_with_attribute_ { get; private set; }
		public SyntaxTreeToken m__anglr_definition_terminal_ { get; private set; }
		public _anglr_definition_ m__anglr_definition_ { get; private set; }
		public SyntaxTreeToken m__single_terminal_definition_terminal_ { get; private set; }
		public _single_terminal_definition_ m__single_terminal_definition_ { get; private set; }
		public SyntaxTreeToken m__single_regex_definition_terminal_ { get; private set; }
		public _single_regex_definition_ m__single_regex_definition_ { get; private set; }
		public SyntaxTreeToken m__block_of_terminal_definitions_terminal_ { get; private set; }
		public _block_of_terminal_definitions_ m__block_of_terminal_definitions_ { get; private set; }
		public SyntaxTreeToken m__block_of_regex_definitions_terminal_ { get; private set; }
		public _block_of_regex_definitions_ m__block_of_regex_definitions_ { get; private set; }
		public SyntaxTreeToken m__terminal_definition_terminal_ { get; private set; }
		public _terminal_definition_ m__terminal_definition_ { get; private set; }
		public SyntaxTreeToken m__regex_definition_terminal_ { get; private set; }
		public _regex_definition_ m__regex_definition_ { get; private set; }
		public SyntaxTreeToken m__block_terminal_definitions_terminal_ { get; private set; }
		public _block_terminal_definitions_ m__block_terminal_definitions_ { get; private set; }
		public SyntaxTreeToken m__block_terminal_definition_terminal_ { get; private set; }
		public _block_terminal_definition_ m__block_terminal_definition_ { get; private set; }
		public SyntaxTreeToken m__block_regex_definitions_terminal_ { get; private set; }
		public _block_regex_definitions_ m__block_regex_definitions_ { get; private set; }
		public SyntaxTreeToken m__block_regex_definition_terminal_ { get; private set; }
		public _block_regex_definition_ m__block_regex_definition_ { get; private set; }
		public SyntaxTreeToken m__scanner_part_terminal_ { get; private set; }
		public _scanner_part_ m__scanner_part_ { get; private set; }
		public SyntaxTreeToken m__regular_expression_list_terminal_ { get; private set; }
		public _regular_expression_list_ m__regular_expression_list_ { get; private set; }
		public SyntaxTreeToken m__regular_expression_usage_terminal_ { get; private set; }
		public _regular_expression_usage_ m__regular_expression_usage_ { get; private set; }
		public SyntaxTreeToken m__actions_terminal_ { get; private set; }
		public _actions_ m__actions_ { get; private set; }
		public SyntaxTreeToken m__action_terminal_ { get; private set; }
		public _action_ m__action_ { get; private set; }
		public SyntaxTreeToken m__skip_action_terminal_ { get; private set; }
		public _skip_action_ m__skip_action_ { get; private set; }
		public SyntaxTreeToken m__terminal_action_terminal_ { get; private set; }
		public _terminal_action_ m__terminal_action_ { get; private set; }
		public SyntaxTreeToken m__event_action_terminal_ { get; private set; }
		public _event_action_ m__event_action_ { get; private set; }
		public SyntaxTreeToken m__push_action_terminal_ { get; private set; }
		public _push_action_ m__push_action_ { get; private set; }
		public SyntaxTreeToken m__pop_action_terminal_ { get; private set; }
		public _pop_action_ m__pop_action_ { get; private set; }
		public SyntaxTreeToken m__lexer_part_terminal_ { get; private set; }
		public _lexer_part_ m__lexer_part_ { get; private set; }
		public SyntaxTreeToken m__parser_part_terminal_ { get; private set; }
		public _parser_part_ m__parser_part_ { get; private set; }
		public SyntaxTreeToken m__anglr_syntax_rule_list_terminal_ { get; private set; }
		public _anglr_syntax_rule_list_ m__anglr_syntax_rule_list_ { get; private set; }
		public SyntaxTreeToken m__anglr_syntax_rule_terminal_ { get; private set; }
		public _anglr_syntax_rule_ m__anglr_syntax_rule_ { get; private set; }
		public SyntaxTreeToken m__anglr_nested_rule_terminal_ { get; private set; }
		public _anglr_nested_rule_ m__anglr_nested_rule_ { get; private set; }
		public SyntaxTreeToken m__anglr_syntax_production_list_name_terminal_ { get; private set; }
		public _anglr_syntax_production_list_name_ m__anglr_syntax_production_list_name_ { get; private set; }
		public SyntaxTreeToken m__anglr_syntax_production_list_terminal_ { get; private set; }
		public _anglr_syntax_production_list_ m__anglr_syntax_production_list_ { get; private set; }
		public SyntaxTreeToken m__anglr_syntax_production_terminal_ { get; private set; }
		public _anglr_syntax_production_ m__anglr_syntax_production_ { get; private set; }
		public SyntaxTreeToken m__priority_assoc_specification_terminal_ { get; private set; }
		public _priority_assoc_specification_ m__priority_assoc_specification_ { get; private set; }
		public SyntaxTreeToken m__priority_specification_terminal_ { get; private set; }
		public _priority_specification_ m__priority_specification_ { get; private set; }
		public SyntaxTreeToken m__associativity_specification_terminal_ { get; private set; }
		public _associativity_specification_ m__associativity_specification_ { get; private set; }
		public SyntaxTreeToken m__production_name_terminal_ { get; private set; }
		public _production_name_ m__production_name_ { get; private set; }
		public SyntaxTreeToken m__name_list_terminal_ { get; private set; }
		public _name_list_ m__name_list_ { get; private set; }
		public SyntaxTreeToken m__marker_list_terminal_ { get; private set; }
		public _marker_list_ m__marker_list_ { get; private set; }
		public SyntaxTreeToken m__marker_terminal_ { get; private set; }
		public _marker_ m__marker_ { get; private set; }
		public SyntaxTreeToken m__g_name_terminal_ { get; private set; }
		public _g_name_ m__g_name_ { get; private set; }
		public SyntaxTreeToken m__name_terminal_ { get; private set; }
		public _name_ m__name_ { get; private set; }
		public SyntaxTreeToken m__cardinality_delimiter_terminal_ { get; private set; }
		public _cardinality_delimiter_ m__cardinality_delimiter_ { get; private set; }
		public SyntaxTreeToken m__cardinality_terminal_ { get; private set; }
		public _cardinality_ m__cardinality_ { get; private set; }
		public SyntaxTreeToken m__delimiter_terminal_ { get; private set; }
		public _delimiter_ m__delimiter_ { get; private set; }
		public SyntaxTreeToken m__attribute_list_optional_terminal_ { get; private set; }
		public _attribute_list_optional_ m__attribute_list_optional_ { get; private set; }
		public SyntaxTreeToken m__name_value_list_optional_terminal_ { get; private set; }
		public _name_value_list_optional_ m__name_value_list_optional_ { get; private set; }
		public SyntaxTreeToken m__anglr_file_part_list_optional_terminal_ { get; private set; }
		public _anglr_file_part_list_optional_ m__anglr_file_part_list_optional_ { get; private set; }
		public SyntaxTreeToken m__anglr_definition_list_optional_terminal_ { get; private set; }
		public _anglr_definition_list_optional_ m__anglr_definition_list_optional_ { get; private set; }
		public SyntaxTreeToken m__block_terminal_definitions_optional_terminal_ { get; private set; }
		public _block_terminal_definitions_optional_ m__block_terminal_definitions_optional_ { get; private set; }
		public SyntaxTreeToken m__block_regex_definitions_optional_terminal_ { get; private set; }
		public _block_regex_definitions_optional_ m__block_regex_definitions_optional_ { get; private set; }
		public SyntaxTreeToken m__regular_expression_list_optional_terminal_ { get; private set; }
		public _regular_expression_list_optional_ m__regular_expression_list_optional_ { get; private set; }
		public SyntaxTreeToken m__actions_optional_terminal_ { get; private set; }
		public _actions_optional_ m__actions_optional_ { get; private set; }
		public SyntaxTreeToken m__anglr_syntax_rule_list_optional_terminal_ { get; private set; }
		public _anglr_syntax_rule_list_optional_ m__anglr_syntax_rule_list_optional_ { get; private set; }
		public SyntaxTreeToken m__anglr_syntax_production_list_name_optional_terminal_ { get; private set; }
		public _anglr_syntax_production_list_name_optional_ m__anglr_syntax_production_list_name_optional_ { get; private set; }
		public SyntaxTreeToken m__production_name_optional_terminal_ { get; private set; }
		public _production_name_optional_ m__production_name_optional_ { get; private set; }
		public SyntaxTreeToken m__priority_assoc_specification_optional_terminal_ { get; private set; }
		public _priority_assoc_specification_optional_ m__priority_assoc_specification_optional_ { get; private set; }
		public SyntaxTreeToken m__marker_list_optional_terminal_ { get; private set; }
		public _marker_list_optional_ m__marker_list_optional_ { get; private set; }
		public SyntaxTreeToken m__delimiter_optional_terminal_ { get; private set; }
		public _delimiter_optional_ m__delimiter_optional_ { get; private set; }
		public SyntaxTreeToken m__cstring_optional_terminal_ { get; private set; }
		public _cstring_optional_ m__cstring_optional_ { get; private set; }
		public SyntaxTreeToken m__number_optional_terminal_ { get; private set; }
		public _number_optional_ m__number_optional_ { get; private set; }
		#endregion

	};

	public partial class SyntaxTreeWalker
	{
		#region delegates and events associated with the syntax rule <anglr file fragment>

		// delegate function (callback) prototype associated with syntax rule <anglr file fragment>
		public delegate bool _anglr_file_fragment__Callback (SyntaxTreeCallbackReason reason, _anglr_file_fragment_.production_kind kind, _anglr_file_fragment_ p__anglr_file_fragment_);

		// event associated with syntax rule <anglr file fragment>
		public event _anglr_file_fragment__Callback _anglr_file_fragment__Event;

		// event trigger associated with syntax rule <anglr file fragment>
		public bool Raise__anglr_file_fragment__Event (SyntaxTreeCallbackReason reason, _anglr_file_fragment_.production_kind kind, _anglr_file_fragment_ p__anglr_file_fragment_)
		{
			bool? status = _anglr_file_fragment__Event?.Invoke (reason, kind, p__anglr_file_fragment_);
			return (status == null) || status.Value;
		}
		#endregion
		#region syntax tree traversal methods associated with the syntax rule <anglr file fragment>
		//
		// traverse syntax tree node associated with any production of syntax rule <anglr file fragment>
		//

		public void Traverse (_anglr_file_fragment_ p__anglr_file_fragment_)
		{
			if (p__anglr_file_fragment_.isLocked())
				return;
			p__anglr_file_fragment_.dolock();
			_anglr_file_fragment_.production_kind kind = (_anglr_file_fragment_.production_kind) p__anglr_file_fragment_.kind;
			p__anglr_file_fragment_.turn_reset ();
			if (Raise__anglr_file_fragment__Event (SyntaxTreeCallbackReason.TraversalPrologueCallbackReason, kind, p__anglr_file_fragment_))
			switch (kind)
			{
				case _anglr_file_fragment_.production_kind.g__anglr_file_fragment__1:
					// traverse syntax tree node associated with production
					// <anglr file fragment>: <attribute list terminal> <attribute list>

					if (Raise__anglr_file_fragment__Event (SyntaxTreeCallbackReason.TraversalMidTermCallbackReason, kind, p__anglr_file_fragment_))
						Traverse (p__anglr_file_fragment_.m__attribute_list_);
					p__anglr_file_fragment_.turn_inc ();
					Raise__anglr_file_fragment__Event (SyntaxTreeCallbackReason.TraversalMidTermCallbackReason, kind, p__anglr_file_fragment_);
				break;
				case _anglr_file_fragment_.production_kind.g__anglr_file_fragment__2:
					// traverse syntax tree node associated with production
					// <anglr file fragment>: <attribute terminal> <attribute>

					if (Raise__anglr_file_fragment__Event (SyntaxTreeCallbackReason.TraversalMidTermCallbackReason, kind, p__anglr_file_fragment_))
						Traverse (p__anglr_file_fragment_.m__attribute_);
					p__anglr_file_fragment_.turn_inc ();
					Raise__anglr_file_fragment__Event (SyntaxTreeCallbackReason.TraversalMidTermCallbackReason, kind, p__anglr_file_fragment_);
				break;
				case _anglr_file_fragment_.production_kind.g__anglr_file_fragment__3:
					// traverse syntax tree node associated with production
					// <anglr file fragment>: <name value list terminal> <name value list>

					if (Raise__anglr_file_fragment__Event (SyntaxTreeCallbackReason.TraversalMidTermCallbackReason, kind, p__anglr_file_fragment_))
						Traverse (p__anglr_file_fragment_.m__name_value_list_);
					p__anglr_file_fragment_.turn_inc ();
					Raise__anglr_file_fragment__Event (SyntaxTreeCallbackReason.TraversalMidTermCallbackReason, kind, p__anglr_file_fragment_);
				break;
				case _anglr_file_fragment_.production_kind.g__anglr_file_fragment__4:
					// traverse syntax tree node associated with production
					// <anglr file fragment>: <name value pair terminal> <name value pair>

					if (Raise__anglr_file_fragment__Event (SyntaxTreeCallbackReason.TraversalMidTermCallbackReason, kind, p__anglr_file_fragment_))
						Traverse (p__anglr_file_fragment_.m__name_value_pair_);
					p__anglr_file_fragment_.turn_inc ();
					Raise__anglr_file_fragment__Event (SyntaxTreeCallbackReason.TraversalMidTermCallbackReason, kind, p__anglr_file_fragment_);
				break;
				case _anglr_file_fragment_.production_kind.g__anglr_file_fragment__5:
					// traverse syntax tree node associated with production
					// <anglr file fragment>: <anglr file terminal> <anglr file>

					if (Raise__anglr_file_fragment__Event (SyntaxTreeCallbackReason.TraversalMidTermCallbackReason, kind, p__anglr_file_fragment_))
						Traverse (p__anglr_file_fragment_.m__anglr_file_);
					p__anglr_file_fragment_.turn_inc ();
					Raise__anglr_file_fragment__Event (SyntaxTreeCallbackReason.TraversalMidTermCallbackReason, kind, p__anglr_file_fragment_);
				break;
				case _anglr_file_fragment_.production_kind.g__anglr_file_fragment__6:
					// traverse syntax tree node associated with production
					// <anglr file fragment>: <anglr file part list terminal> <anglr file part list>

					if (Raise__anglr_file_fragment__Event (SyntaxTreeCallbackReason.TraversalMidTermCallbackReason, kind, p__anglr_file_fragment_))
						Traverse (p__anglr_file_fragment_.m__anglr_file_part_list_);
					p__anglr_file_fragment_.turn_inc ();
					Raise__anglr_file_fragment__Event (SyntaxTreeCallbackReason.TraversalMidTermCallbackReason, kind, p__anglr_file_fragment_);
				break;
				case _anglr_file_fragment_.production_kind.g__anglr_file_fragment__7:
					// traverse syntax tree node associated with production
					// <anglr file fragment>: <anglr file part terminal> <anglr file part>

					if (Raise__anglr_file_fragment__Event (SyntaxTreeCallbackReason.TraversalMidTermCallbackReason, kind, p__anglr_file_fragment_))
						Traverse (p__anglr_file_fragment_.m__anglr_file_part_);
					p__anglr_file_fragment_.turn_inc ();
					Raise__anglr_file_fragment__Event (SyntaxTreeCallbackReason.TraversalMidTermCallbackReason, kind, p__anglr_file_fragment_);
				break;
				case _anglr_file_fragment_.production_kind.g__anglr_file_fragment__8:
					// traverse syntax tree node associated with production
					// <anglr file fragment>: <general part terminal> <general part>

					if (Raise__anglr_file_fragment__Event (SyntaxTreeCallbackReason.TraversalMidTermCallbackReason, kind, p__anglr_file_fragment_))
						Traverse (p__anglr_file_fragment_.m__general_part_);
					p__anglr_file_fragment_.turn_inc ();
					Raise__anglr_file_fragment__Event (SyntaxTreeCallbackReason.TraversalMidTermCallbackReason, kind, p__anglr_file_fragment_);
				break;
				case _anglr_file_fragment_.production_kind.g__anglr_file_fragment__9:
					// traverse syntax tree node associated with production
					// <anglr file fragment>: <declaration part terminal> <declaration part>

					if (Raise__anglr_file_fragment__Event (SyntaxTreeCallbackReason.TraversalMidTermCallbackReason, kind, p__anglr_file_fragment_))
						Traverse (p__anglr_file_fragment_.m__declaration_part_);
					p__anglr_file_fragment_.turn_inc ();
					Raise__anglr_file_fragment__Event (SyntaxTreeCallbackReason.TraversalMidTermCallbackReason, kind, p__anglr_file_fragment_);
				break;
				case _anglr_file_fragment_.production_kind.g__anglr_file_fragment__10:
					// traverse syntax tree node associated with production
					// <anglr file fragment>: <anglr definition list terminal> <anglr definition list>

					if (Raise__anglr_file_fragment__Event (SyntaxTreeCallbackReason.TraversalMidTermCallbackReason, kind, p__anglr_file_fragment_))
						Traverse (p__anglr_file_fragment_.m__anglr_definition_list_);
					p__anglr_file_fragment_.turn_inc ();
					Raise__anglr_file_fragment__Event (SyntaxTreeCallbackReason.TraversalMidTermCallbackReason, kind, p__anglr_file_fragment_);
				break;
				case _anglr_file_fragment_.production_kind.g__anglr_file_fragment__11:
					// traverse syntax tree node associated with production
					// <anglr file fragment>: <anglr definition with attribute list terminal> <anglr definition with attribute>

					if (Raise__anglr_file_fragment__Event (SyntaxTreeCallbackReason.TraversalMidTermCallbackReason, kind, p__anglr_file_fragment_))
						Traverse (p__anglr_file_fragment_.m__anglr_definition_with_attribute_);
					p__anglr_file_fragment_.turn_inc ();
					Raise__anglr_file_fragment__Event (SyntaxTreeCallbackReason.TraversalMidTermCallbackReason, kind, p__anglr_file_fragment_);
				break;
				case _anglr_file_fragment_.production_kind.g__anglr_file_fragment__12:
					// traverse syntax tree node associated with production
					// <anglr file fragment>: <anglr definition terminal> <anglr definition>

					if (Raise__anglr_file_fragment__Event (SyntaxTreeCallbackReason.TraversalMidTermCallbackReason, kind, p__anglr_file_fragment_))
						Traverse (p__anglr_file_fragment_.m__anglr_definition_);
					p__anglr_file_fragment_.turn_inc ();
					Raise__anglr_file_fragment__Event (SyntaxTreeCallbackReason.TraversalMidTermCallbackReason, kind, p__anglr_file_fragment_);
				break;
				case _anglr_file_fragment_.production_kind.g__anglr_file_fragment__13:
					// traverse syntax tree node associated with production
					// <anglr file fragment>: <single terminal definition terminal> <single terminal definition>

					if (Raise__anglr_file_fragment__Event (SyntaxTreeCallbackReason.TraversalMidTermCallbackReason, kind, p__anglr_file_fragment_))
						Traverse (p__anglr_file_fragment_.m__single_terminal_definition_);
					p__anglr_file_fragment_.turn_inc ();
					Raise__anglr_file_fragment__Event (SyntaxTreeCallbackReason.TraversalMidTermCallbackReason, kind, p__anglr_file_fragment_);
				break;
				case _anglr_file_fragment_.production_kind.g__anglr_file_fragment__14:
					// traverse syntax tree node associated with production
					// <anglr file fragment>: <single regex definition terminal> <single regex definition>

					if (Raise__anglr_file_fragment__Event (SyntaxTreeCallbackReason.TraversalMidTermCallbackReason, kind, p__anglr_file_fragment_))
						Traverse (p__anglr_file_fragment_.m__single_regex_definition_);
					p__anglr_file_fragment_.turn_inc ();
					Raise__anglr_file_fragment__Event (SyntaxTreeCallbackReason.TraversalMidTermCallbackReason, kind, p__anglr_file_fragment_);
				break;
				case _anglr_file_fragment_.production_kind.g__anglr_file_fragment__15:
					// traverse syntax tree node associated with production
					// <anglr file fragment>: <block of terminal definitions terminal> <block of terminal definitions>

					if (Raise__anglr_file_fragment__Event (SyntaxTreeCallbackReason.TraversalMidTermCallbackReason, kind, p__anglr_file_fragment_))
						Traverse (p__anglr_file_fragment_.m__block_of_terminal_definitions_);
					p__anglr_file_fragment_.turn_inc ();
					Raise__anglr_file_fragment__Event (SyntaxTreeCallbackReason.TraversalMidTermCallbackReason, kind, p__anglr_file_fragment_);
				break;
				case _anglr_file_fragment_.production_kind.g__anglr_file_fragment__16:
					// traverse syntax tree node associated with production
					// <anglr file fragment>: <block of regex definitions terminal> <block of regex definitions>

					if (Raise__anglr_file_fragment__Event (SyntaxTreeCallbackReason.TraversalMidTermCallbackReason, kind, p__anglr_file_fragment_))
						Traverse (p__anglr_file_fragment_.m__block_of_regex_definitions_);
					p__anglr_file_fragment_.turn_inc ();
					Raise__anglr_file_fragment__Event (SyntaxTreeCallbackReason.TraversalMidTermCallbackReason, kind, p__anglr_file_fragment_);
				break;
				case _anglr_file_fragment_.production_kind.g__anglr_file_fragment__17:
					// traverse syntax tree node associated with production
					// <anglr file fragment>: <terminal definition terminal> <terminal definition>

					if (Raise__anglr_file_fragment__Event (SyntaxTreeCallbackReason.TraversalMidTermCallbackReason, kind, p__anglr_file_fragment_))
						Traverse (p__anglr_file_fragment_.m__terminal_definition_);
					p__anglr_file_fragment_.turn_inc ();
					Raise__anglr_file_fragment__Event (SyntaxTreeCallbackReason.TraversalMidTermCallbackReason, kind, p__anglr_file_fragment_);
				break;
				case _anglr_file_fragment_.production_kind.g__anglr_file_fragment__18:
					// traverse syntax tree node associated with production
					// <anglr file fragment>: <regex definition terminal> <regex definition>

					if (Raise__anglr_file_fragment__Event (SyntaxTreeCallbackReason.TraversalMidTermCallbackReason, kind, p__anglr_file_fragment_))
						Traverse (p__anglr_file_fragment_.m__regex_definition_);
					p__anglr_file_fragment_.turn_inc ();
					Raise__anglr_file_fragment__Event (SyntaxTreeCallbackReason.TraversalMidTermCallbackReason, kind, p__anglr_file_fragment_);
				break;
				case _anglr_file_fragment_.production_kind.g__anglr_file_fragment__19:
					// traverse syntax tree node associated with production
					// <anglr file fragment>: <block terminal definitions terminal> <block terminal definitions>

					if (Raise__anglr_file_fragment__Event (SyntaxTreeCallbackReason.TraversalMidTermCallbackReason, kind, p__anglr_file_fragment_))
						Traverse (p__anglr_file_fragment_.m__block_terminal_definitions_);
					p__anglr_file_fragment_.turn_inc ();
					Raise__anglr_file_fragment__Event (SyntaxTreeCallbackReason.TraversalMidTermCallbackReason, kind, p__anglr_file_fragment_);
				break;
				case _anglr_file_fragment_.production_kind.g__anglr_file_fragment__20:
					// traverse syntax tree node associated with production
					// <anglr file fragment>: <block terminal definition terminal> <block terminal definition>

					if (Raise__anglr_file_fragment__Event (SyntaxTreeCallbackReason.TraversalMidTermCallbackReason, kind, p__anglr_file_fragment_))
						Traverse (p__anglr_file_fragment_.m__block_terminal_definition_);
					p__anglr_file_fragment_.turn_inc ();
					Raise__anglr_file_fragment__Event (SyntaxTreeCallbackReason.TraversalMidTermCallbackReason, kind, p__anglr_file_fragment_);
				break;
				case _anglr_file_fragment_.production_kind.g__anglr_file_fragment__21:
					// traverse syntax tree node associated with production
					// <anglr file fragment>: <block regex definitions terminal> <block regex definitions>

					if (Raise__anglr_file_fragment__Event (SyntaxTreeCallbackReason.TraversalMidTermCallbackReason, kind, p__anglr_file_fragment_))
						Traverse (p__anglr_file_fragment_.m__block_regex_definitions_);
					p__anglr_file_fragment_.turn_inc ();
					Raise__anglr_file_fragment__Event (SyntaxTreeCallbackReason.TraversalMidTermCallbackReason, kind, p__anglr_file_fragment_);
				break;
				case _anglr_file_fragment_.production_kind.g__anglr_file_fragment__22:
					// traverse syntax tree node associated with production
					// <anglr file fragment>: <block regex definition terminal> <block regex definition>

					if (Raise__anglr_file_fragment__Event (SyntaxTreeCallbackReason.TraversalMidTermCallbackReason, kind, p__anglr_file_fragment_))
						Traverse (p__anglr_file_fragment_.m__block_regex_definition_);
					p__anglr_file_fragment_.turn_inc ();
					Raise__anglr_file_fragment__Event (SyntaxTreeCallbackReason.TraversalMidTermCallbackReason, kind, p__anglr_file_fragment_);
				break;
				case _anglr_file_fragment_.production_kind.g__anglr_file_fragment__23:
					// traverse syntax tree node associated with production
					// <anglr file fragment>: <scanner part terminal> <scanner part>

					if (Raise__anglr_file_fragment__Event (SyntaxTreeCallbackReason.TraversalMidTermCallbackReason, kind, p__anglr_file_fragment_))
						Traverse (p__anglr_file_fragment_.m__scanner_part_);
					p__anglr_file_fragment_.turn_inc ();
					Raise__anglr_file_fragment__Event (SyntaxTreeCallbackReason.TraversalMidTermCallbackReason, kind, p__anglr_file_fragment_);
				break;
				case _anglr_file_fragment_.production_kind.g__anglr_file_fragment__24:
					// traverse syntax tree node associated with production
					// <anglr file fragment>: <regular expression list terminal> <regular expression list>

					if (Raise__anglr_file_fragment__Event (SyntaxTreeCallbackReason.TraversalMidTermCallbackReason, kind, p__anglr_file_fragment_))
						Traverse (p__anglr_file_fragment_.m__regular_expression_list_);
					p__anglr_file_fragment_.turn_inc ();
					Raise__anglr_file_fragment__Event (SyntaxTreeCallbackReason.TraversalMidTermCallbackReason, kind, p__anglr_file_fragment_);
				break;
				case _anglr_file_fragment_.production_kind.g__anglr_file_fragment__25:
					// traverse syntax tree node associated with production
					// <anglr file fragment>: <regular expression usage terminal> <regular expression usage>

					if (Raise__anglr_file_fragment__Event (SyntaxTreeCallbackReason.TraversalMidTermCallbackReason, kind, p__anglr_file_fragment_))
						Traverse (p__anglr_file_fragment_.m__regular_expression_usage_);
					p__anglr_file_fragment_.turn_inc ();
					Raise__anglr_file_fragment__Event (SyntaxTreeCallbackReason.TraversalMidTermCallbackReason, kind, p__anglr_file_fragment_);
				break;
				case _anglr_file_fragment_.production_kind.g__anglr_file_fragment__26:
					// traverse syntax tree node associated with production
					// <anglr file fragment>: <actions terminal> <actions>

					if (Raise__anglr_file_fragment__Event (SyntaxTreeCallbackReason.TraversalMidTermCallbackReason, kind, p__anglr_file_fragment_))
						Traverse (p__anglr_file_fragment_.m__actions_);
					p__anglr_file_fragment_.turn_inc ();
					Raise__anglr_file_fragment__Event (SyntaxTreeCallbackReason.TraversalMidTermCallbackReason, kind, p__anglr_file_fragment_);
				break;
				case _anglr_file_fragment_.production_kind.g__anglr_file_fragment__27:
					// traverse syntax tree node associated with production
					// <anglr file fragment>: <action terminal> <action>

					if (Raise__anglr_file_fragment__Event (SyntaxTreeCallbackReason.TraversalMidTermCallbackReason, kind, p__anglr_file_fragment_))
						Traverse (p__anglr_file_fragment_.m__action_);
					p__anglr_file_fragment_.turn_inc ();
					Raise__anglr_file_fragment__Event (SyntaxTreeCallbackReason.TraversalMidTermCallbackReason, kind, p__anglr_file_fragment_);
				break;
				case _anglr_file_fragment_.production_kind.g__anglr_file_fragment__28:
					// traverse syntax tree node associated with production
					// <anglr file fragment>: <skip action terminal> <skip action>

					if (Raise__anglr_file_fragment__Event (SyntaxTreeCallbackReason.TraversalMidTermCallbackReason, kind, p__anglr_file_fragment_))
						Traverse (p__anglr_file_fragment_.m__skip_action_);
					p__anglr_file_fragment_.turn_inc ();
					Raise__anglr_file_fragment__Event (SyntaxTreeCallbackReason.TraversalMidTermCallbackReason, kind, p__anglr_file_fragment_);
				break;
				case _anglr_file_fragment_.production_kind.g__anglr_file_fragment__29:
					// traverse syntax tree node associated with production
					// <anglr file fragment>: <terminal action terminal> <terminal action>

					if (Raise__anglr_file_fragment__Event (SyntaxTreeCallbackReason.TraversalMidTermCallbackReason, kind, p__anglr_file_fragment_))
						Traverse (p__anglr_file_fragment_.m__terminal_action_);
					p__anglr_file_fragment_.turn_inc ();
					Raise__anglr_file_fragment__Event (SyntaxTreeCallbackReason.TraversalMidTermCallbackReason, kind, p__anglr_file_fragment_);
				break;
				case _anglr_file_fragment_.production_kind.g__anglr_file_fragment__30:
					// traverse syntax tree node associated with production
					// <anglr file fragment>: <event action terminal> <event action>

					if (Raise__anglr_file_fragment__Event (SyntaxTreeCallbackReason.TraversalMidTermCallbackReason, kind, p__anglr_file_fragment_))
						Traverse (p__anglr_file_fragment_.m__event_action_);
					p__anglr_file_fragment_.turn_inc ();
					Raise__anglr_file_fragment__Event (SyntaxTreeCallbackReason.TraversalMidTermCallbackReason, kind, p__anglr_file_fragment_);
				break;
				case _anglr_file_fragment_.production_kind.g__anglr_file_fragment__31:
					// traverse syntax tree node associated with production
					// <anglr file fragment>: <push action terminal> <push action>

					if (Raise__anglr_file_fragment__Event (SyntaxTreeCallbackReason.TraversalMidTermCallbackReason, kind, p__anglr_file_fragment_))
						Traverse (p__anglr_file_fragment_.m__push_action_);
					p__anglr_file_fragment_.turn_inc ();
					Raise__anglr_file_fragment__Event (SyntaxTreeCallbackReason.TraversalMidTermCallbackReason, kind, p__anglr_file_fragment_);
				break;
				case _anglr_file_fragment_.production_kind.g__anglr_file_fragment__32:
					// traverse syntax tree node associated with production
					// <anglr file fragment>: <pop action terminal> <pop action>

					if (Raise__anglr_file_fragment__Event (SyntaxTreeCallbackReason.TraversalMidTermCallbackReason, kind, p__anglr_file_fragment_))
						Traverse (p__anglr_file_fragment_.m__pop_action_);
					p__anglr_file_fragment_.turn_inc ();
					Raise__anglr_file_fragment__Event (SyntaxTreeCallbackReason.TraversalMidTermCallbackReason, kind, p__anglr_file_fragment_);
				break;
				case _anglr_file_fragment_.production_kind.g__anglr_file_fragment__33:
					// traverse syntax tree node associated with production
					// <anglr file fragment>: <lexer part terminal> <lexer part>

					if (Raise__anglr_file_fragment__Event (SyntaxTreeCallbackReason.TraversalMidTermCallbackReason, kind, p__anglr_file_fragment_))
						Traverse (p__anglr_file_fragment_.m__lexer_part_);
					p__anglr_file_fragment_.turn_inc ();
					Raise__anglr_file_fragment__Event (SyntaxTreeCallbackReason.TraversalMidTermCallbackReason, kind, p__anglr_file_fragment_);
				break;
				case _anglr_file_fragment_.production_kind.g__anglr_file_fragment__34:
					// traverse syntax tree node associated with production
					// <anglr file fragment>: <parser part terminal> <parser part>

					if (Raise__anglr_file_fragment__Event (SyntaxTreeCallbackReason.TraversalMidTermCallbackReason, kind, p__anglr_file_fragment_))
						Traverse (p__anglr_file_fragment_.m__parser_part_);
					p__anglr_file_fragment_.turn_inc ();
					Raise__anglr_file_fragment__Event (SyntaxTreeCallbackReason.TraversalMidTermCallbackReason, kind, p__anglr_file_fragment_);
				break;
				case _anglr_file_fragment_.production_kind.g__anglr_file_fragment__35:
					// traverse syntax tree node associated with production
					// <anglr file fragment>: <anglr syntax rule list terminal> <anglr syntax rule list>

					if (Raise__anglr_file_fragment__Event (SyntaxTreeCallbackReason.TraversalMidTermCallbackReason, kind, p__anglr_file_fragment_))
						Traverse (p__anglr_file_fragment_.m__anglr_syntax_rule_list_);
					p__anglr_file_fragment_.turn_inc ();
					Raise__anglr_file_fragment__Event (SyntaxTreeCallbackReason.TraversalMidTermCallbackReason, kind, p__anglr_file_fragment_);
				break;
				case _anglr_file_fragment_.production_kind.g__anglr_file_fragment__36:
					// traverse syntax tree node associated with production
					// <anglr file fragment>: <anglr syntax rule terminal> <anglr syntax rule>

					if (Raise__anglr_file_fragment__Event (SyntaxTreeCallbackReason.TraversalMidTermCallbackReason, kind, p__anglr_file_fragment_))
						Traverse (p__anglr_file_fragment_.m__anglr_syntax_rule_);
					p__anglr_file_fragment_.turn_inc ();
					Raise__anglr_file_fragment__Event (SyntaxTreeCallbackReason.TraversalMidTermCallbackReason, kind, p__anglr_file_fragment_);
				break;
				case _anglr_file_fragment_.production_kind.g__anglr_file_fragment__37:
					// traverse syntax tree node associated with production
					// <anglr file fragment>: <anglr nested rule terminal> <anglr nested rule>

					if (Raise__anglr_file_fragment__Event (SyntaxTreeCallbackReason.TraversalMidTermCallbackReason, kind, p__anglr_file_fragment_))
						Traverse (p__anglr_file_fragment_.m__anglr_nested_rule_);
					p__anglr_file_fragment_.turn_inc ();
					Raise__anglr_file_fragment__Event (SyntaxTreeCallbackReason.TraversalMidTermCallbackReason, kind, p__anglr_file_fragment_);
				break;
				case _anglr_file_fragment_.production_kind.g__anglr_file_fragment__38:
					// traverse syntax tree node associated with production
					// <anglr file fragment>: <anglr syntax production list name terminal> <anglr syntax production list name>

					if (Raise__anglr_file_fragment__Event (SyntaxTreeCallbackReason.TraversalMidTermCallbackReason, kind, p__anglr_file_fragment_))
						Traverse (p__anglr_file_fragment_.m__anglr_syntax_production_list_name_);
					p__anglr_file_fragment_.turn_inc ();
					Raise__anglr_file_fragment__Event (SyntaxTreeCallbackReason.TraversalMidTermCallbackReason, kind, p__anglr_file_fragment_);
				break;
				case _anglr_file_fragment_.production_kind.g__anglr_file_fragment__39:
					// traverse syntax tree node associated with production
					// <anglr file fragment>: <anglr syntax production list terminal> <anglr syntax production list>

					if (Raise__anglr_file_fragment__Event (SyntaxTreeCallbackReason.TraversalMidTermCallbackReason, kind, p__anglr_file_fragment_))
						Traverse (p__anglr_file_fragment_.m__anglr_syntax_production_list_);
					p__anglr_file_fragment_.turn_inc ();
					Raise__anglr_file_fragment__Event (SyntaxTreeCallbackReason.TraversalMidTermCallbackReason, kind, p__anglr_file_fragment_);
				break;
				case _anglr_file_fragment_.production_kind.g__anglr_file_fragment__40:
					// traverse syntax tree node associated with production
					// <anglr file fragment>: <anglr syntax production terminal> <anglr syntax production>

					if (Raise__anglr_file_fragment__Event (SyntaxTreeCallbackReason.TraversalMidTermCallbackReason, kind, p__anglr_file_fragment_))
						Traverse (p__anglr_file_fragment_.m__anglr_syntax_production_);
					p__anglr_file_fragment_.turn_inc ();
					Raise__anglr_file_fragment__Event (SyntaxTreeCallbackReason.TraversalMidTermCallbackReason, kind, p__anglr_file_fragment_);
				break;
				case _anglr_file_fragment_.production_kind.g__anglr_file_fragment__41:
					// traverse syntax tree node associated with production
					// <anglr file fragment>: <priority assoc specification terminal> <priority assoc specification>

					if (Raise__anglr_file_fragment__Event (SyntaxTreeCallbackReason.TraversalMidTermCallbackReason, kind, p__anglr_file_fragment_))
						Traverse (p__anglr_file_fragment_.m__priority_assoc_specification_);
					p__anglr_file_fragment_.turn_inc ();
					Raise__anglr_file_fragment__Event (SyntaxTreeCallbackReason.TraversalMidTermCallbackReason, kind, p__anglr_file_fragment_);
				break;
				case _anglr_file_fragment_.production_kind.g__anglr_file_fragment__42:
					// traverse syntax tree node associated with production
					// <anglr file fragment>: <priority specification terminal> <priority specification>

					if (Raise__anglr_file_fragment__Event (SyntaxTreeCallbackReason.TraversalMidTermCallbackReason, kind, p__anglr_file_fragment_))
						Traverse (p__anglr_file_fragment_.m__priority_specification_);
					p__anglr_file_fragment_.turn_inc ();
					Raise__anglr_file_fragment__Event (SyntaxTreeCallbackReason.TraversalMidTermCallbackReason, kind, p__anglr_file_fragment_);
				break;
				case _anglr_file_fragment_.production_kind.g__anglr_file_fragment__43:
					// traverse syntax tree node associated with production
					// <anglr file fragment>: <associativity specification terminal> <associativity specification>

					if (Raise__anglr_file_fragment__Event (SyntaxTreeCallbackReason.TraversalMidTermCallbackReason, kind, p__anglr_file_fragment_))
						Traverse (p__anglr_file_fragment_.m__associativity_specification_);
					p__anglr_file_fragment_.turn_inc ();
					Raise__anglr_file_fragment__Event (SyntaxTreeCallbackReason.TraversalMidTermCallbackReason, kind, p__anglr_file_fragment_);
				break;
				case _anglr_file_fragment_.production_kind.g__anglr_file_fragment__44:
					// traverse syntax tree node associated with production
					// <anglr file fragment>: <production name terminal> <production name>

					if (Raise__anglr_file_fragment__Event (SyntaxTreeCallbackReason.TraversalMidTermCallbackReason, kind, p__anglr_file_fragment_))
						Traverse (p__anglr_file_fragment_.m__production_name_);
					p__anglr_file_fragment_.turn_inc ();
					Raise__anglr_file_fragment__Event (SyntaxTreeCallbackReason.TraversalMidTermCallbackReason, kind, p__anglr_file_fragment_);
				break;
				case _anglr_file_fragment_.production_kind.g__anglr_file_fragment__45:
					// traverse syntax tree node associated with production
					// <anglr file fragment>: <name list terminal> <name list>

					if (Raise__anglr_file_fragment__Event (SyntaxTreeCallbackReason.TraversalMidTermCallbackReason, kind, p__anglr_file_fragment_))
						Traverse (p__anglr_file_fragment_.m__name_list_);
					p__anglr_file_fragment_.turn_inc ();
					Raise__anglr_file_fragment__Event (SyntaxTreeCallbackReason.TraversalMidTermCallbackReason, kind, p__anglr_file_fragment_);
				break;
				case _anglr_file_fragment_.production_kind.g__anglr_file_fragment__46:
					// traverse syntax tree node associated with production
					// <anglr file fragment>: <marker list terminal> <marker list>

					if (Raise__anglr_file_fragment__Event (SyntaxTreeCallbackReason.TraversalMidTermCallbackReason, kind, p__anglr_file_fragment_))
						Traverse (p__anglr_file_fragment_.m__marker_list_);
					p__anglr_file_fragment_.turn_inc ();
					Raise__anglr_file_fragment__Event (SyntaxTreeCallbackReason.TraversalMidTermCallbackReason, kind, p__anglr_file_fragment_);
				break;
				case _anglr_file_fragment_.production_kind.g__anglr_file_fragment__47:
					// traverse syntax tree node associated with production
					// <anglr file fragment>: <marker terminal> <marker>

					if (Raise__anglr_file_fragment__Event (SyntaxTreeCallbackReason.TraversalMidTermCallbackReason, kind, p__anglr_file_fragment_))
						Traverse (p__anglr_file_fragment_.m__marker_);
					p__anglr_file_fragment_.turn_inc ();
					Raise__anglr_file_fragment__Event (SyntaxTreeCallbackReason.TraversalMidTermCallbackReason, kind, p__anglr_file_fragment_);
				break;
				case _anglr_file_fragment_.production_kind.g__anglr_file_fragment__48:
					// traverse syntax tree node associated with production
					// <anglr file fragment>: <g name terminal> <g name>

					if (Raise__anglr_file_fragment__Event (SyntaxTreeCallbackReason.TraversalMidTermCallbackReason, kind, p__anglr_file_fragment_))
						Traverse (p__anglr_file_fragment_.m__g_name_);
					p__anglr_file_fragment_.turn_inc ();
					Raise__anglr_file_fragment__Event (SyntaxTreeCallbackReason.TraversalMidTermCallbackReason, kind, p__anglr_file_fragment_);
				break;
				case _anglr_file_fragment_.production_kind.g__anglr_file_fragment__49:
					// traverse syntax tree node associated with production
					// <anglr file fragment>: <name terminal> <name>

					if (Raise__anglr_file_fragment__Event (SyntaxTreeCallbackReason.TraversalMidTermCallbackReason, kind, p__anglr_file_fragment_))
						Traverse (p__anglr_file_fragment_.m__name_);
					p__anglr_file_fragment_.turn_inc ();
					Raise__anglr_file_fragment__Event (SyntaxTreeCallbackReason.TraversalMidTermCallbackReason, kind, p__anglr_file_fragment_);
				break;
				case _anglr_file_fragment_.production_kind.g__anglr_file_fragment__50:
					// traverse syntax tree node associated with production
					// <anglr file fragment>: <cardinality delimiter terminal> <cardinality delimiter>

					if (Raise__anglr_file_fragment__Event (SyntaxTreeCallbackReason.TraversalMidTermCallbackReason, kind, p__anglr_file_fragment_))
						Traverse (p__anglr_file_fragment_.m__cardinality_delimiter_);
					p__anglr_file_fragment_.turn_inc ();
					Raise__anglr_file_fragment__Event (SyntaxTreeCallbackReason.TraversalMidTermCallbackReason, kind, p__anglr_file_fragment_);
				break;
				case _anglr_file_fragment_.production_kind.g__anglr_file_fragment__51:
					// traverse syntax tree node associated with production
					// <anglr file fragment>: <cardinality terminal> <cardinality>

					if (Raise__anglr_file_fragment__Event (SyntaxTreeCallbackReason.TraversalMidTermCallbackReason, kind, p__anglr_file_fragment_))
						Traverse (p__anglr_file_fragment_.m__cardinality_);
					p__anglr_file_fragment_.turn_inc ();
					Raise__anglr_file_fragment__Event (SyntaxTreeCallbackReason.TraversalMidTermCallbackReason, kind, p__anglr_file_fragment_);
				break;
				case _anglr_file_fragment_.production_kind.g__anglr_file_fragment__52:
					// traverse syntax tree node associated with production
					// <anglr file fragment>: <delimiter terminal> <delimiter>

					if (Raise__anglr_file_fragment__Event (SyntaxTreeCallbackReason.TraversalMidTermCallbackReason, kind, p__anglr_file_fragment_))
						Traverse (p__anglr_file_fragment_.m__delimiter_);
					p__anglr_file_fragment_.turn_inc ();
					Raise__anglr_file_fragment__Event (SyntaxTreeCallbackReason.TraversalMidTermCallbackReason, kind, p__anglr_file_fragment_);
				break;
				case _anglr_file_fragment_.production_kind.g__anglr_file_fragment__53:
					// traverse syntax tree node associated with production
					// <anglr file fragment>: <attribute list optional terminal> <attribute list optional>

					if (Raise__anglr_file_fragment__Event (SyntaxTreeCallbackReason.TraversalMidTermCallbackReason, kind, p__anglr_file_fragment_))
						Traverse (p__anglr_file_fragment_.m__attribute_list_optional_);
					p__anglr_file_fragment_.turn_inc ();
					Raise__anglr_file_fragment__Event (SyntaxTreeCallbackReason.TraversalMidTermCallbackReason, kind, p__anglr_file_fragment_);
				break;
				case _anglr_file_fragment_.production_kind.g__anglr_file_fragment__54:
					// traverse syntax tree node associated with production
					// <anglr file fragment>: <name value list optional terminal> <name value list optional>

					if (Raise__anglr_file_fragment__Event (SyntaxTreeCallbackReason.TraversalMidTermCallbackReason, kind, p__anglr_file_fragment_))
						Traverse (p__anglr_file_fragment_.m__name_value_list_optional_);
					p__anglr_file_fragment_.turn_inc ();
					Raise__anglr_file_fragment__Event (SyntaxTreeCallbackReason.TraversalMidTermCallbackReason, kind, p__anglr_file_fragment_);
				break;
				case _anglr_file_fragment_.production_kind.g__anglr_file_fragment__55:
					// traverse syntax tree node associated with production
					// <anglr file fragment>: <anglr file part list optional terminal> <anglr file part list optional>

					if (Raise__anglr_file_fragment__Event (SyntaxTreeCallbackReason.TraversalMidTermCallbackReason, kind, p__anglr_file_fragment_))
						Traverse (p__anglr_file_fragment_.m__anglr_file_part_list_optional_);
					p__anglr_file_fragment_.turn_inc ();
					Raise__anglr_file_fragment__Event (SyntaxTreeCallbackReason.TraversalMidTermCallbackReason, kind, p__anglr_file_fragment_);
				break;
				case _anglr_file_fragment_.production_kind.g__anglr_file_fragment__56:
					// traverse syntax tree node associated with production
					// <anglr file fragment>: <anglr definition list optional terminal> <anglr definition list optional>

					if (Raise__anglr_file_fragment__Event (SyntaxTreeCallbackReason.TraversalMidTermCallbackReason, kind, p__anglr_file_fragment_))
						Traverse (p__anglr_file_fragment_.m__anglr_definition_list_optional_);
					p__anglr_file_fragment_.turn_inc ();
					Raise__anglr_file_fragment__Event (SyntaxTreeCallbackReason.TraversalMidTermCallbackReason, kind, p__anglr_file_fragment_);
				break;
				case _anglr_file_fragment_.production_kind.g__anglr_file_fragment__57:
					// traverse syntax tree node associated with production
					// <anglr file fragment>: <block terminal definitions optional terminal> <block terminal definitions optional>

					if (Raise__anglr_file_fragment__Event (SyntaxTreeCallbackReason.TraversalMidTermCallbackReason, kind, p__anglr_file_fragment_))
						Traverse (p__anglr_file_fragment_.m__block_terminal_definitions_optional_);
					p__anglr_file_fragment_.turn_inc ();
					Raise__anglr_file_fragment__Event (SyntaxTreeCallbackReason.TraversalMidTermCallbackReason, kind, p__anglr_file_fragment_);
				break;
				case _anglr_file_fragment_.production_kind.g__anglr_file_fragment__58:
					// traverse syntax tree node associated with production
					// <anglr file fragment>: <block regex definitions optional terminal> <block regex definitions optional>

					if (Raise__anglr_file_fragment__Event (SyntaxTreeCallbackReason.TraversalMidTermCallbackReason, kind, p__anglr_file_fragment_))
						Traverse (p__anglr_file_fragment_.m__block_regex_definitions_optional_);
					p__anglr_file_fragment_.turn_inc ();
					Raise__anglr_file_fragment__Event (SyntaxTreeCallbackReason.TraversalMidTermCallbackReason, kind, p__anglr_file_fragment_);
				break;
				case _anglr_file_fragment_.production_kind.g__anglr_file_fragment__59:
					// traverse syntax tree node associated with production
					// <anglr file fragment>: <regular expression list optional terminal> <regular expression list optional>

					if (Raise__anglr_file_fragment__Event (SyntaxTreeCallbackReason.TraversalMidTermCallbackReason, kind, p__anglr_file_fragment_))
						Traverse (p__anglr_file_fragment_.m__regular_expression_list_optional_);
					p__anglr_file_fragment_.turn_inc ();
					Raise__anglr_file_fragment__Event (SyntaxTreeCallbackReason.TraversalMidTermCallbackReason, kind, p__anglr_file_fragment_);
				break;
				case _anglr_file_fragment_.production_kind.g__anglr_file_fragment__60:
					// traverse syntax tree node associated with production
					// <anglr file fragment>: <actions optional terminal> <actions optional>

					if (Raise__anglr_file_fragment__Event (SyntaxTreeCallbackReason.TraversalMidTermCallbackReason, kind, p__anglr_file_fragment_))
						Traverse (p__anglr_file_fragment_.m__actions_optional_);
					p__anglr_file_fragment_.turn_inc ();
					Raise__anglr_file_fragment__Event (SyntaxTreeCallbackReason.TraversalMidTermCallbackReason, kind, p__anglr_file_fragment_);
				break;
				case _anglr_file_fragment_.production_kind.g__anglr_file_fragment__61:
					// traverse syntax tree node associated with production
					// <anglr file fragment>: <anglr syntax rule list optional terminal> <anglr syntax rule list optional>

					if (Raise__anglr_file_fragment__Event (SyntaxTreeCallbackReason.TraversalMidTermCallbackReason, kind, p__anglr_file_fragment_))
						Traverse (p__anglr_file_fragment_.m__anglr_syntax_rule_list_optional_);
					p__anglr_file_fragment_.turn_inc ();
					Raise__anglr_file_fragment__Event (SyntaxTreeCallbackReason.TraversalMidTermCallbackReason, kind, p__anglr_file_fragment_);
				break;
				case _anglr_file_fragment_.production_kind.g__anglr_file_fragment__62:
					// traverse syntax tree node associated with production
					// <anglr file fragment>: <anglr syntax production list name optional terminal> <anglr syntax production list name optional>

					if (Raise__anglr_file_fragment__Event (SyntaxTreeCallbackReason.TraversalMidTermCallbackReason, kind, p__anglr_file_fragment_))
						Traverse (p__anglr_file_fragment_.m__anglr_syntax_production_list_name_optional_);
					p__anglr_file_fragment_.turn_inc ();
					Raise__anglr_file_fragment__Event (SyntaxTreeCallbackReason.TraversalMidTermCallbackReason, kind, p__anglr_file_fragment_);
				break;
				case _anglr_file_fragment_.production_kind.g__anglr_file_fragment__63:
					// traverse syntax tree node associated with production
					// <anglr file fragment>: <production name optional terminal> <production name optional>

					if (Raise__anglr_file_fragment__Event (SyntaxTreeCallbackReason.TraversalMidTermCallbackReason, kind, p__anglr_file_fragment_))
						Traverse (p__anglr_file_fragment_.m__production_name_optional_);
					p__anglr_file_fragment_.turn_inc ();
					Raise__anglr_file_fragment__Event (SyntaxTreeCallbackReason.TraversalMidTermCallbackReason, kind, p__anglr_file_fragment_);
				break;
				case _anglr_file_fragment_.production_kind.g__anglr_file_fragment__64:
					// traverse syntax tree node associated with production
					// <anglr file fragment>: <priority assoc specification optional terminal> <priority assoc specification optional>

					if (Raise__anglr_file_fragment__Event (SyntaxTreeCallbackReason.TraversalMidTermCallbackReason, kind, p__anglr_file_fragment_))
						Traverse (p__anglr_file_fragment_.m__priority_assoc_specification_optional_);
					p__anglr_file_fragment_.turn_inc ();
					Raise__anglr_file_fragment__Event (SyntaxTreeCallbackReason.TraversalMidTermCallbackReason, kind, p__anglr_file_fragment_);
				break;
				case _anglr_file_fragment_.production_kind.g__anglr_file_fragment__65:
					// traverse syntax tree node associated with production
					// <anglr file fragment>: <marker list optional terminal> <marker list optional>

					if (Raise__anglr_file_fragment__Event (SyntaxTreeCallbackReason.TraversalMidTermCallbackReason, kind, p__anglr_file_fragment_))
						Traverse (p__anglr_file_fragment_.m__marker_list_optional_);
					p__anglr_file_fragment_.turn_inc ();
					Raise__anglr_file_fragment__Event (SyntaxTreeCallbackReason.TraversalMidTermCallbackReason, kind, p__anglr_file_fragment_);
				break;
				case _anglr_file_fragment_.production_kind.g__anglr_file_fragment__66:
					// traverse syntax tree node associated with production
					// <anglr file fragment>: <delimiter optional terminal> <delimiter optional>

					if (Raise__anglr_file_fragment__Event (SyntaxTreeCallbackReason.TraversalMidTermCallbackReason, kind, p__anglr_file_fragment_))
						Traverse (p__anglr_file_fragment_.m__delimiter_optional_);
					p__anglr_file_fragment_.turn_inc ();
					Raise__anglr_file_fragment__Event (SyntaxTreeCallbackReason.TraversalMidTermCallbackReason, kind, p__anglr_file_fragment_);
				break;
				case _anglr_file_fragment_.production_kind.g__anglr_file_fragment__67:
					// traverse syntax tree node associated with production
					// <anglr file fragment>: <cstring optional terminal> <cstring optional>

					if (Raise__anglr_file_fragment__Event (SyntaxTreeCallbackReason.TraversalMidTermCallbackReason, kind, p__anglr_file_fragment_))
						Traverse (p__anglr_file_fragment_.m__cstring_optional_);
					p__anglr_file_fragment_.turn_inc ();
					Raise__anglr_file_fragment__Event (SyntaxTreeCallbackReason.TraversalMidTermCallbackReason, kind, p__anglr_file_fragment_);
				break;
				case _anglr_file_fragment_.production_kind.g__anglr_file_fragment__68:
					// traverse syntax tree node associated with production
					// <anglr file fragment>: <number optional terminal> <number optional>

					if (Raise__anglr_file_fragment__Event (SyntaxTreeCallbackReason.TraversalMidTermCallbackReason, kind, p__anglr_file_fragment_))
						Traverse (p__anglr_file_fragment_.m__number_optional_);
					p__anglr_file_fragment_.turn_inc ();
					Raise__anglr_file_fragment__Event (SyntaxTreeCallbackReason.TraversalMidTermCallbackReason, kind, p__anglr_file_fragment_);
				break;
			}
			Raise__anglr_file_fragment__Event (SyntaxTreeCallbackReason.TraversalEpilogueCallbackReason, kind, p__anglr_file_fragment_);
			p__anglr_file_fragment_.unlock();
		}

		//
		// traverse syntax tree node associated with any production of syntax rule <anglr file fragment>
		//

		public void TraverseCommon (_anglr_file_fragment_ p__anglr_file_fragment_)
		{
			_anglr_file_fragment_.production_kind kind = (_anglr_file_fragment_.production_kind) p__anglr_file_fragment_.kind;
			if (Raise_Common_Event (SyntaxTreeCallbackReason.TraversalPrologueCallbackReason, (int) kind, p__anglr_file_fragment_))
			switch (kind)
			{
				case _anglr_file_fragment_.production_kind.g__anglr_file_fragment__1:
					// traverse syntax tree node associated with production
					// <anglr file fragment>: <attribute list terminal> <attribute list>

						TraverseCommon (p__anglr_file_fragment_.m__attribute_list_terminal_);
						TraverseCommon (p__anglr_file_fragment_.m__attribute_list_);
				break;
				case _anglr_file_fragment_.production_kind.g__anglr_file_fragment__2:
					// traverse syntax tree node associated with production
					// <anglr file fragment>: <attribute terminal> <attribute>

						TraverseCommon (p__anglr_file_fragment_.m__attribute_terminal_);
						TraverseCommon (p__anglr_file_fragment_.m__attribute_);
				break;
				case _anglr_file_fragment_.production_kind.g__anglr_file_fragment__3:
					// traverse syntax tree node associated with production
					// <anglr file fragment>: <name value list terminal> <name value list>

						TraverseCommon (p__anglr_file_fragment_.m__name_value_list_terminal_);
						TraverseCommon (p__anglr_file_fragment_.m__name_value_list_);
				break;
				case _anglr_file_fragment_.production_kind.g__anglr_file_fragment__4:
					// traverse syntax tree node associated with production
					// <anglr file fragment>: <name value pair terminal> <name value pair>

						TraverseCommon (p__anglr_file_fragment_.m__name_value_pair_terminal_);
						TraverseCommon (p__anglr_file_fragment_.m__name_value_pair_);
				break;
				case _anglr_file_fragment_.production_kind.g__anglr_file_fragment__5:
					// traverse syntax tree node associated with production
					// <anglr file fragment>: <anglr file terminal> <anglr file>

						TraverseCommon (p__anglr_file_fragment_.m__anglr_file_terminal_);
						TraverseCommon (p__anglr_file_fragment_.m__anglr_file_);
				break;
				case _anglr_file_fragment_.production_kind.g__anglr_file_fragment__6:
					// traverse syntax tree node associated with production
					// <anglr file fragment>: <anglr file part list terminal> <anglr file part list>

						TraverseCommon (p__anglr_file_fragment_.m__anglr_file_part_list_terminal_);
						TraverseCommon (p__anglr_file_fragment_.m__anglr_file_part_list_);
				break;
				case _anglr_file_fragment_.production_kind.g__anglr_file_fragment__7:
					// traverse syntax tree node associated with production
					// <anglr file fragment>: <anglr file part terminal> <anglr file part>

						TraverseCommon (p__anglr_file_fragment_.m__anglr_file_part_terminal_);
						TraverseCommon (p__anglr_file_fragment_.m__anglr_file_part_);
				break;
				case _anglr_file_fragment_.production_kind.g__anglr_file_fragment__8:
					// traverse syntax tree node associated with production
					// <anglr file fragment>: <general part terminal> <general part>

						TraverseCommon (p__anglr_file_fragment_.m__general_part_terminal_);
						TraverseCommon (p__anglr_file_fragment_.m__general_part_);
				break;
				case _anglr_file_fragment_.production_kind.g__anglr_file_fragment__9:
					// traverse syntax tree node associated with production
					// <anglr file fragment>: <declaration part terminal> <declaration part>

						TraverseCommon (p__anglr_file_fragment_.m__declaration_part_terminal_);
						TraverseCommon (p__anglr_file_fragment_.m__declaration_part_);
				break;
				case _anglr_file_fragment_.production_kind.g__anglr_file_fragment__10:
					// traverse syntax tree node associated with production
					// <anglr file fragment>: <anglr definition list terminal> <anglr definition list>

						TraverseCommon (p__anglr_file_fragment_.m__anglr_definition_list_terminal_);
						TraverseCommon (p__anglr_file_fragment_.m__anglr_definition_list_);
				break;
				case _anglr_file_fragment_.production_kind.g__anglr_file_fragment__11:
					// traverse syntax tree node associated with production
					// <anglr file fragment>: <anglr definition with attribute list terminal> <anglr definition with attribute>

						TraverseCommon (p__anglr_file_fragment_.m__anglr_definition_with_attribute_list_terminal_);
						TraverseCommon (p__anglr_file_fragment_.m__anglr_definition_with_attribute_);
				break;
				case _anglr_file_fragment_.production_kind.g__anglr_file_fragment__12:
					// traverse syntax tree node associated with production
					// <anglr file fragment>: <anglr definition terminal> <anglr definition>

						TraverseCommon (p__anglr_file_fragment_.m__anglr_definition_terminal_);
						TraverseCommon (p__anglr_file_fragment_.m__anglr_definition_);
				break;
				case _anglr_file_fragment_.production_kind.g__anglr_file_fragment__13:
					// traverse syntax tree node associated with production
					// <anglr file fragment>: <single terminal definition terminal> <single terminal definition>

						TraverseCommon (p__anglr_file_fragment_.m__single_terminal_definition_terminal_);
						TraverseCommon (p__anglr_file_fragment_.m__single_terminal_definition_);
				break;
				case _anglr_file_fragment_.production_kind.g__anglr_file_fragment__14:
					// traverse syntax tree node associated with production
					// <anglr file fragment>: <single regex definition terminal> <single regex definition>

						TraverseCommon (p__anglr_file_fragment_.m__single_regex_definition_terminal_);
						TraverseCommon (p__anglr_file_fragment_.m__single_regex_definition_);
				break;
				case _anglr_file_fragment_.production_kind.g__anglr_file_fragment__15:
					// traverse syntax tree node associated with production
					// <anglr file fragment>: <block of terminal definitions terminal> <block of terminal definitions>

						TraverseCommon (p__anglr_file_fragment_.m__block_of_terminal_definitions_terminal_);
						TraverseCommon (p__anglr_file_fragment_.m__block_of_terminal_definitions_);
				break;
				case _anglr_file_fragment_.production_kind.g__anglr_file_fragment__16:
					// traverse syntax tree node associated with production
					// <anglr file fragment>: <block of regex definitions terminal> <block of regex definitions>

						TraverseCommon (p__anglr_file_fragment_.m__block_of_regex_definitions_terminal_);
						TraverseCommon (p__anglr_file_fragment_.m__block_of_regex_definitions_);
				break;
				case _anglr_file_fragment_.production_kind.g__anglr_file_fragment__17:
					// traverse syntax tree node associated with production
					// <anglr file fragment>: <terminal definition terminal> <terminal definition>

						TraverseCommon (p__anglr_file_fragment_.m__terminal_definition_terminal_);
						TraverseCommon (p__anglr_file_fragment_.m__terminal_definition_);
				break;
				case _anglr_file_fragment_.production_kind.g__anglr_file_fragment__18:
					// traverse syntax tree node associated with production
					// <anglr file fragment>: <regex definition terminal> <regex definition>

						TraverseCommon (p__anglr_file_fragment_.m__regex_definition_terminal_);
						TraverseCommon (p__anglr_file_fragment_.m__regex_definition_);
				break;
				case _anglr_file_fragment_.production_kind.g__anglr_file_fragment__19:
					// traverse syntax tree node associated with production
					// <anglr file fragment>: <block terminal definitions terminal> <block terminal definitions>

						TraverseCommon (p__anglr_file_fragment_.m__block_terminal_definitions_terminal_);
						TraverseCommon (p__anglr_file_fragment_.m__block_terminal_definitions_);
				break;
				case _anglr_file_fragment_.production_kind.g__anglr_file_fragment__20:
					// traverse syntax tree node associated with production
					// <anglr file fragment>: <block terminal definition terminal> <block terminal definition>

						TraverseCommon (p__anglr_file_fragment_.m__block_terminal_definition_terminal_);
						TraverseCommon (p__anglr_file_fragment_.m__block_terminal_definition_);
				break;
				case _anglr_file_fragment_.production_kind.g__anglr_file_fragment__21:
					// traverse syntax tree node associated with production
					// <anglr file fragment>: <block regex definitions terminal> <block regex definitions>

						TraverseCommon (p__anglr_file_fragment_.m__block_regex_definitions_terminal_);
						TraverseCommon (p__anglr_file_fragment_.m__block_regex_definitions_);
				break;
				case _anglr_file_fragment_.production_kind.g__anglr_file_fragment__22:
					// traverse syntax tree node associated with production
					// <anglr file fragment>: <block regex definition terminal> <block regex definition>

						TraverseCommon (p__anglr_file_fragment_.m__block_regex_definition_terminal_);
						TraverseCommon (p__anglr_file_fragment_.m__block_regex_definition_);
				break;
				case _anglr_file_fragment_.production_kind.g__anglr_file_fragment__23:
					// traverse syntax tree node associated with production
					// <anglr file fragment>: <scanner part terminal> <scanner part>

						TraverseCommon (p__anglr_file_fragment_.m__scanner_part_terminal_);
						TraverseCommon (p__anglr_file_fragment_.m__scanner_part_);
				break;
				case _anglr_file_fragment_.production_kind.g__anglr_file_fragment__24:
					// traverse syntax tree node associated with production
					// <anglr file fragment>: <regular expression list terminal> <regular expression list>

						TraverseCommon (p__anglr_file_fragment_.m__regular_expression_list_terminal_);
						TraverseCommon (p__anglr_file_fragment_.m__regular_expression_list_);
				break;
				case _anglr_file_fragment_.production_kind.g__anglr_file_fragment__25:
					// traverse syntax tree node associated with production
					// <anglr file fragment>: <regular expression usage terminal> <regular expression usage>

						TraverseCommon (p__anglr_file_fragment_.m__regular_expression_usage_terminal_);
						TraverseCommon (p__anglr_file_fragment_.m__regular_expression_usage_);
				break;
				case _anglr_file_fragment_.production_kind.g__anglr_file_fragment__26:
					// traverse syntax tree node associated with production
					// <anglr file fragment>: <actions terminal> <actions>

						TraverseCommon (p__anglr_file_fragment_.m__actions_terminal_);
						TraverseCommon (p__anglr_file_fragment_.m__actions_);
				break;
				case _anglr_file_fragment_.production_kind.g__anglr_file_fragment__27:
					// traverse syntax tree node associated with production
					// <anglr file fragment>: <action terminal> <action>

						TraverseCommon (p__anglr_file_fragment_.m__action_terminal_);
						TraverseCommon (p__anglr_file_fragment_.m__action_);
				break;
				case _anglr_file_fragment_.production_kind.g__anglr_file_fragment__28:
					// traverse syntax tree node associated with production
					// <anglr file fragment>: <skip action terminal> <skip action>

						TraverseCommon (p__anglr_file_fragment_.m__skip_action_terminal_);
						TraverseCommon (p__anglr_file_fragment_.m__skip_action_);
				break;
				case _anglr_file_fragment_.production_kind.g__anglr_file_fragment__29:
					// traverse syntax tree node associated with production
					// <anglr file fragment>: <terminal action terminal> <terminal action>

						TraverseCommon (p__anglr_file_fragment_.m__terminal_action_terminal_);
						TraverseCommon (p__anglr_file_fragment_.m__terminal_action_);
				break;
				case _anglr_file_fragment_.production_kind.g__anglr_file_fragment__30:
					// traverse syntax tree node associated with production
					// <anglr file fragment>: <event action terminal> <event action>

						TraverseCommon (p__anglr_file_fragment_.m__event_action_terminal_);
						TraverseCommon (p__anglr_file_fragment_.m__event_action_);
				break;
				case _anglr_file_fragment_.production_kind.g__anglr_file_fragment__31:
					// traverse syntax tree node associated with production
					// <anglr file fragment>: <push action terminal> <push action>

						TraverseCommon (p__anglr_file_fragment_.m__push_action_terminal_);
						TraverseCommon (p__anglr_file_fragment_.m__push_action_);
				break;
				case _anglr_file_fragment_.production_kind.g__anglr_file_fragment__32:
					// traverse syntax tree node associated with production
					// <anglr file fragment>: <pop action terminal> <pop action>

						TraverseCommon (p__anglr_file_fragment_.m__pop_action_terminal_);
						TraverseCommon (p__anglr_file_fragment_.m__pop_action_);
				break;
				case _anglr_file_fragment_.production_kind.g__anglr_file_fragment__33:
					// traverse syntax tree node associated with production
					// <anglr file fragment>: <lexer part terminal> <lexer part>

						TraverseCommon (p__anglr_file_fragment_.m__lexer_part_terminal_);
						TraverseCommon (p__anglr_file_fragment_.m__lexer_part_);
				break;
				case _anglr_file_fragment_.production_kind.g__anglr_file_fragment__34:
					// traverse syntax tree node associated with production
					// <anglr file fragment>: <parser part terminal> <parser part>

						TraverseCommon (p__anglr_file_fragment_.m__parser_part_terminal_);
						TraverseCommon (p__anglr_file_fragment_.m__parser_part_);
				break;
				case _anglr_file_fragment_.production_kind.g__anglr_file_fragment__35:
					// traverse syntax tree node associated with production
					// <anglr file fragment>: <anglr syntax rule list terminal> <anglr syntax rule list>

						TraverseCommon (p__anglr_file_fragment_.m__anglr_syntax_rule_list_terminal_);
						TraverseCommon (p__anglr_file_fragment_.m__anglr_syntax_rule_list_);
				break;
				case _anglr_file_fragment_.production_kind.g__anglr_file_fragment__36:
					// traverse syntax tree node associated with production
					// <anglr file fragment>: <anglr syntax rule terminal> <anglr syntax rule>

						TraverseCommon (p__anglr_file_fragment_.m__anglr_syntax_rule_terminal_);
						TraverseCommon (p__anglr_file_fragment_.m__anglr_syntax_rule_);
				break;
				case _anglr_file_fragment_.production_kind.g__anglr_file_fragment__37:
					// traverse syntax tree node associated with production
					// <anglr file fragment>: <anglr nested rule terminal> <anglr nested rule>

						TraverseCommon (p__anglr_file_fragment_.m__anglr_nested_rule_terminal_);
						TraverseCommon (p__anglr_file_fragment_.m__anglr_nested_rule_);
				break;
				case _anglr_file_fragment_.production_kind.g__anglr_file_fragment__38:
					// traverse syntax tree node associated with production
					// <anglr file fragment>: <anglr syntax production list name terminal> <anglr syntax production list name>

						TraverseCommon (p__anglr_file_fragment_.m__anglr_syntax_production_list_name_terminal_);
						TraverseCommon (p__anglr_file_fragment_.m__anglr_syntax_production_list_name_);
				break;
				case _anglr_file_fragment_.production_kind.g__anglr_file_fragment__39:
					// traverse syntax tree node associated with production
					// <anglr file fragment>: <anglr syntax production list terminal> <anglr syntax production list>

						TraverseCommon (p__anglr_file_fragment_.m__anglr_syntax_production_list_terminal_);
						TraverseCommon (p__anglr_file_fragment_.m__anglr_syntax_production_list_);
				break;
				case _anglr_file_fragment_.production_kind.g__anglr_file_fragment__40:
					// traverse syntax tree node associated with production
					// <anglr file fragment>: <anglr syntax production terminal> <anglr syntax production>

						TraverseCommon (p__anglr_file_fragment_.m__anglr_syntax_production_terminal_);
						TraverseCommon (p__anglr_file_fragment_.m__anglr_syntax_production_);
				break;
				case _anglr_file_fragment_.production_kind.g__anglr_file_fragment__41:
					// traverse syntax tree node associated with production
					// <anglr file fragment>: <priority assoc specification terminal> <priority assoc specification>

						TraverseCommon (p__anglr_file_fragment_.m__priority_assoc_specification_terminal_);
						TraverseCommon (p__anglr_file_fragment_.m__priority_assoc_specification_);
				break;
				case _anglr_file_fragment_.production_kind.g__anglr_file_fragment__42:
					// traverse syntax tree node associated with production
					// <anglr file fragment>: <priority specification terminal> <priority specification>

						TraverseCommon (p__anglr_file_fragment_.m__priority_specification_terminal_);
						TraverseCommon (p__anglr_file_fragment_.m__priority_specification_);
				break;
				case _anglr_file_fragment_.production_kind.g__anglr_file_fragment__43:
					// traverse syntax tree node associated with production
					// <anglr file fragment>: <associativity specification terminal> <associativity specification>

						TraverseCommon (p__anglr_file_fragment_.m__associativity_specification_terminal_);
						TraverseCommon (p__anglr_file_fragment_.m__associativity_specification_);
				break;
				case _anglr_file_fragment_.production_kind.g__anglr_file_fragment__44:
					// traverse syntax tree node associated with production
					// <anglr file fragment>: <production name terminal> <production name>

						TraverseCommon (p__anglr_file_fragment_.m__production_name_terminal_);
						TraverseCommon (p__anglr_file_fragment_.m__production_name_);
				break;
				case _anglr_file_fragment_.production_kind.g__anglr_file_fragment__45:
					// traverse syntax tree node associated with production
					// <anglr file fragment>: <name list terminal> <name list>

						TraverseCommon (p__anglr_file_fragment_.m__name_list_terminal_);
						TraverseCommon (p__anglr_file_fragment_.m__name_list_);
				break;
				case _anglr_file_fragment_.production_kind.g__anglr_file_fragment__46:
					// traverse syntax tree node associated with production
					// <anglr file fragment>: <marker list terminal> <marker list>

						TraverseCommon (p__anglr_file_fragment_.m__marker_list_terminal_);
						TraverseCommon (p__anglr_file_fragment_.m__marker_list_);
				break;
				case _anglr_file_fragment_.production_kind.g__anglr_file_fragment__47:
					// traverse syntax tree node associated with production
					// <anglr file fragment>: <marker terminal> <marker>

						TraverseCommon (p__anglr_file_fragment_.m__marker_terminal_);
						TraverseCommon (p__anglr_file_fragment_.m__marker_);
				break;
				case _anglr_file_fragment_.production_kind.g__anglr_file_fragment__48:
					// traverse syntax tree node associated with production
					// <anglr file fragment>: <g name terminal> <g name>

						TraverseCommon (p__anglr_file_fragment_.m__g_name_terminal_);
						TraverseCommon (p__anglr_file_fragment_.m__g_name_);
				break;
				case _anglr_file_fragment_.production_kind.g__anglr_file_fragment__49:
					// traverse syntax tree node associated with production
					// <anglr file fragment>: <name terminal> <name>

						TraverseCommon (p__anglr_file_fragment_.m__name_terminal_);
						TraverseCommon (p__anglr_file_fragment_.m__name_);
				break;
				case _anglr_file_fragment_.production_kind.g__anglr_file_fragment__50:
					// traverse syntax tree node associated with production
					// <anglr file fragment>: <cardinality delimiter terminal> <cardinality delimiter>

						TraverseCommon (p__anglr_file_fragment_.m__cardinality_delimiter_terminal_);
						TraverseCommon (p__anglr_file_fragment_.m__cardinality_delimiter_);
				break;
				case _anglr_file_fragment_.production_kind.g__anglr_file_fragment__51:
					// traverse syntax tree node associated with production
					// <anglr file fragment>: <cardinality terminal> <cardinality>

						TraverseCommon (p__anglr_file_fragment_.m__cardinality_terminal_);
						TraverseCommon (p__anglr_file_fragment_.m__cardinality_);
				break;
				case _anglr_file_fragment_.production_kind.g__anglr_file_fragment__52:
					// traverse syntax tree node associated with production
					// <anglr file fragment>: <delimiter terminal> <delimiter>

						TraverseCommon (p__anglr_file_fragment_.m__delimiter_terminal_);
						TraverseCommon (p__anglr_file_fragment_.m__delimiter_);
				break;
				case _anglr_file_fragment_.production_kind.g__anglr_file_fragment__53:
					// traverse syntax tree node associated with production
					// <anglr file fragment>: <attribute list optional terminal> <attribute list optional>

						TraverseCommon (p__anglr_file_fragment_.m__attribute_list_optional_terminal_);
						TraverseCommon (p__anglr_file_fragment_.m__attribute_list_optional_);
				break;
				case _anglr_file_fragment_.production_kind.g__anglr_file_fragment__54:
					// traverse syntax tree node associated with production
					// <anglr file fragment>: <name value list optional terminal> <name value list optional>

						TraverseCommon (p__anglr_file_fragment_.m__name_value_list_optional_terminal_);
						TraverseCommon (p__anglr_file_fragment_.m__name_value_list_optional_);
				break;
				case _anglr_file_fragment_.production_kind.g__anglr_file_fragment__55:
					// traverse syntax tree node associated with production
					// <anglr file fragment>: <anglr file part list optional terminal> <anglr file part list optional>

						TraverseCommon (p__anglr_file_fragment_.m__anglr_file_part_list_optional_terminal_);
						TraverseCommon (p__anglr_file_fragment_.m__anglr_file_part_list_optional_);
				break;
				case _anglr_file_fragment_.production_kind.g__anglr_file_fragment__56:
					// traverse syntax tree node associated with production
					// <anglr file fragment>: <anglr definition list optional terminal> <anglr definition list optional>

						TraverseCommon (p__anglr_file_fragment_.m__anglr_definition_list_optional_terminal_);
						TraverseCommon (p__anglr_file_fragment_.m__anglr_definition_list_optional_);
				break;
				case _anglr_file_fragment_.production_kind.g__anglr_file_fragment__57:
					// traverse syntax tree node associated with production
					// <anglr file fragment>: <block terminal definitions optional terminal> <block terminal definitions optional>

						TraverseCommon (p__anglr_file_fragment_.m__block_terminal_definitions_optional_terminal_);
						TraverseCommon (p__anglr_file_fragment_.m__block_terminal_definitions_optional_);
				break;
				case _anglr_file_fragment_.production_kind.g__anglr_file_fragment__58:
					// traverse syntax tree node associated with production
					// <anglr file fragment>: <block regex definitions optional terminal> <block regex definitions optional>

						TraverseCommon (p__anglr_file_fragment_.m__block_regex_definitions_optional_terminal_);
						TraverseCommon (p__anglr_file_fragment_.m__block_regex_definitions_optional_);
				break;
				case _anglr_file_fragment_.production_kind.g__anglr_file_fragment__59:
					// traverse syntax tree node associated with production
					// <anglr file fragment>: <regular expression list optional terminal> <regular expression list optional>

						TraverseCommon (p__anglr_file_fragment_.m__regular_expression_list_optional_terminal_);
						TraverseCommon (p__anglr_file_fragment_.m__regular_expression_list_optional_);
				break;
				case _anglr_file_fragment_.production_kind.g__anglr_file_fragment__60:
					// traverse syntax tree node associated with production
					// <anglr file fragment>: <actions optional terminal> <actions optional>

						TraverseCommon (p__anglr_file_fragment_.m__actions_optional_terminal_);
						TraverseCommon (p__anglr_file_fragment_.m__actions_optional_);
				break;
				case _anglr_file_fragment_.production_kind.g__anglr_file_fragment__61:
					// traverse syntax tree node associated with production
					// <anglr file fragment>: <anglr syntax rule list optional terminal> <anglr syntax rule list optional>

						TraverseCommon (p__anglr_file_fragment_.m__anglr_syntax_rule_list_optional_terminal_);
						TraverseCommon (p__anglr_file_fragment_.m__anglr_syntax_rule_list_optional_);
				break;
				case _anglr_file_fragment_.production_kind.g__anglr_file_fragment__62:
					// traverse syntax tree node associated with production
					// <anglr file fragment>: <anglr syntax production list name optional terminal> <anglr syntax production list name optional>

						TraverseCommon (p__anglr_file_fragment_.m__anglr_syntax_production_list_name_optional_terminal_);
						TraverseCommon (p__anglr_file_fragment_.m__anglr_syntax_production_list_name_optional_);
				break;
				case _anglr_file_fragment_.production_kind.g__anglr_file_fragment__63:
					// traverse syntax tree node associated with production
					// <anglr file fragment>: <production name optional terminal> <production name optional>

						TraverseCommon (p__anglr_file_fragment_.m__production_name_optional_terminal_);
						TraverseCommon (p__anglr_file_fragment_.m__production_name_optional_);
				break;
				case _anglr_file_fragment_.production_kind.g__anglr_file_fragment__64:
					// traverse syntax tree node associated with production
					// <anglr file fragment>: <priority assoc specification optional terminal> <priority assoc specification optional>

						TraverseCommon (p__anglr_file_fragment_.m__priority_assoc_specification_optional_terminal_);
						TraverseCommon (p__anglr_file_fragment_.m__priority_assoc_specification_optional_);
				break;
				case _anglr_file_fragment_.production_kind.g__anglr_file_fragment__65:
					// traverse syntax tree node associated with production
					// <anglr file fragment>: <marker list optional terminal> <marker list optional>

						TraverseCommon (p__anglr_file_fragment_.m__marker_list_optional_terminal_);
						TraverseCommon (p__anglr_file_fragment_.m__marker_list_optional_);
				break;
				case _anglr_file_fragment_.production_kind.g__anglr_file_fragment__66:
					// traverse syntax tree node associated with production
					// <anglr file fragment>: <delimiter optional terminal> <delimiter optional>

						TraverseCommon (p__anglr_file_fragment_.m__delimiter_optional_terminal_);
						TraverseCommon (p__anglr_file_fragment_.m__delimiter_optional_);
				break;
				case _anglr_file_fragment_.production_kind.g__anglr_file_fragment__67:
					// traverse syntax tree node associated with production
					// <anglr file fragment>: <cstring optional terminal> <cstring optional>

						TraverseCommon (p__anglr_file_fragment_.m__cstring_optional_terminal_);
						TraverseCommon (p__anglr_file_fragment_.m__cstring_optional_);
				break;
				case _anglr_file_fragment_.production_kind.g__anglr_file_fragment__68:
					// traverse syntax tree node associated with production
					// <anglr file fragment>: <number optional terminal> <number optional>

						TraverseCommon (p__anglr_file_fragment_.m__number_optional_terminal_);
						TraverseCommon (p__anglr_file_fragment_.m__number_optional_);
				break;
			}
			Raise_Common_Event (SyntaxTreeCallbackReason.TraversalEpilogueCallbackReason, (int) kind, p__anglr_file_fragment_);
		}
	};
		#endregion

	public partial class SyntaxTreeBuilder : SyntaxTreeWalker
	{
		#region syntax tree builder methods associated with the syntax rule <anglr file fragment>

		//
		// create syntax tree node associated with production
		// <anglr file fragment>: <attribute list terminal> <attribute list>

		//

		public _anglr_file_fragment_ _anglr_file_fragment__1 (SyntaxTreeToken p_token, _attribute_list_ p__attribute_list_)
		{
			_anglr_file_fragment_ p__anglr_file_fragment__ref = new _anglr_file_fragment_(p_token, p__attribute_list_);
			p_token.parent = p__anglr_file_fragment__ref;
			p__attribute_list_.parent = p__anglr_file_fragment__ref;
			Raise__anglr_file_fragment__Event (SyntaxTreeCallbackReason.BuilderCallbackReason, _anglr_file_fragment_.production_kind.g__anglr_file_fragment__1, p__anglr_file_fragment__ref);
			return p__anglr_file_fragment__ref;
		}

		//
		// create syntax tree node associated with production
		// <anglr file fragment>: <attribute terminal> <attribute>

		//

		public _anglr_file_fragment_ _anglr_file_fragment__2 (SyntaxTreeToken p_token, _attribute_ p__attribute_)
		{
			_anglr_file_fragment_ p__anglr_file_fragment__ref = new _anglr_file_fragment_(p_token, p__attribute_);
			p_token.parent = p__anglr_file_fragment__ref;
			p__attribute_.parent = p__anglr_file_fragment__ref;
			Raise__anglr_file_fragment__Event (SyntaxTreeCallbackReason.BuilderCallbackReason, _anglr_file_fragment_.production_kind.g__anglr_file_fragment__2, p__anglr_file_fragment__ref);
			return p__anglr_file_fragment__ref;
		}

		//
		// create syntax tree node associated with production
		// <anglr file fragment>: <name value list terminal> <name value list>

		//

		public _anglr_file_fragment_ _anglr_file_fragment__3 (SyntaxTreeToken p_token, _name_value_list_ p__name_value_list_)
		{
			_anglr_file_fragment_ p__anglr_file_fragment__ref = new _anglr_file_fragment_(p_token, p__name_value_list_);
			p_token.parent = p__anglr_file_fragment__ref;
			p__name_value_list_.parent = p__anglr_file_fragment__ref;
			Raise__anglr_file_fragment__Event (SyntaxTreeCallbackReason.BuilderCallbackReason, _anglr_file_fragment_.production_kind.g__anglr_file_fragment__3, p__anglr_file_fragment__ref);
			return p__anglr_file_fragment__ref;
		}

		//
		// create syntax tree node associated with production
		// <anglr file fragment>: <name value pair terminal> <name value pair>

		//

		public _anglr_file_fragment_ _anglr_file_fragment__4 (SyntaxTreeToken p_token, _name_value_pair_ p__name_value_pair_)
		{
			_anglr_file_fragment_ p__anglr_file_fragment__ref = new _anglr_file_fragment_(p_token, p__name_value_pair_);
			p_token.parent = p__anglr_file_fragment__ref;
			p__name_value_pair_.parent = p__anglr_file_fragment__ref;
			Raise__anglr_file_fragment__Event (SyntaxTreeCallbackReason.BuilderCallbackReason, _anglr_file_fragment_.production_kind.g__anglr_file_fragment__4, p__anglr_file_fragment__ref);
			return p__anglr_file_fragment__ref;
		}

		//
		// create syntax tree node associated with production
		// <anglr file fragment>: <anglr file terminal> <anglr file>

		//

		public _anglr_file_fragment_ _anglr_file_fragment__5 (SyntaxTreeToken p_token, _anglr_file_ p__anglr_file_)
		{
			_anglr_file_fragment_ p__anglr_file_fragment__ref = new _anglr_file_fragment_(p_token, p__anglr_file_);
			p_token.parent = p__anglr_file_fragment__ref;
			p__anglr_file_.parent = p__anglr_file_fragment__ref;
			Raise__anglr_file_fragment__Event (SyntaxTreeCallbackReason.BuilderCallbackReason, _anglr_file_fragment_.production_kind.g__anglr_file_fragment__5, p__anglr_file_fragment__ref);
			return p__anglr_file_fragment__ref;
		}

		//
		// create syntax tree node associated with production
		// <anglr file fragment>: <anglr file part list terminal> <anglr file part list>

		//

		public _anglr_file_fragment_ _anglr_file_fragment__6 (SyntaxTreeToken p_token, _anglr_file_part_list_ p__anglr_file_part_list_)
		{
			_anglr_file_fragment_ p__anglr_file_fragment__ref = new _anglr_file_fragment_(p_token, p__anglr_file_part_list_);
			p_token.parent = p__anglr_file_fragment__ref;
			p__anglr_file_part_list_.parent = p__anglr_file_fragment__ref;
			Raise__anglr_file_fragment__Event (SyntaxTreeCallbackReason.BuilderCallbackReason, _anglr_file_fragment_.production_kind.g__anglr_file_fragment__6, p__anglr_file_fragment__ref);
			return p__anglr_file_fragment__ref;
		}

		//
		// create syntax tree node associated with production
		// <anglr file fragment>: <anglr file part terminal> <anglr file part>

		//

		public _anglr_file_fragment_ _anglr_file_fragment__7 (SyntaxTreeToken p_token, _anglr_file_part_ p__anglr_file_part_)
		{
			_anglr_file_fragment_ p__anglr_file_fragment__ref = new _anglr_file_fragment_(p_token, p__anglr_file_part_);
			p_token.parent = p__anglr_file_fragment__ref;
			p__anglr_file_part_.parent = p__anglr_file_fragment__ref;
			Raise__anglr_file_fragment__Event (SyntaxTreeCallbackReason.BuilderCallbackReason, _anglr_file_fragment_.production_kind.g__anglr_file_fragment__7, p__anglr_file_fragment__ref);
			return p__anglr_file_fragment__ref;
		}

		//
		// create syntax tree node associated with production
		// <anglr file fragment>: <general part terminal> <general part>

		//

		public _anglr_file_fragment_ _anglr_file_fragment__8 (SyntaxTreeToken p_token, _general_part_ p__general_part_)
		{
			_anglr_file_fragment_ p__anglr_file_fragment__ref = new _anglr_file_fragment_(p_token, p__general_part_);
			p_token.parent = p__anglr_file_fragment__ref;
			p__general_part_.parent = p__anglr_file_fragment__ref;
			Raise__anglr_file_fragment__Event (SyntaxTreeCallbackReason.BuilderCallbackReason, _anglr_file_fragment_.production_kind.g__anglr_file_fragment__8, p__anglr_file_fragment__ref);
			return p__anglr_file_fragment__ref;
		}

		//
		// create syntax tree node associated with production
		// <anglr file fragment>: <declaration part terminal> <declaration part>

		//

		public _anglr_file_fragment_ _anglr_file_fragment__9 (SyntaxTreeToken p_token, _declaration_part_ p__declaration_part_)
		{
			_anglr_file_fragment_ p__anglr_file_fragment__ref = new _anglr_file_fragment_(p_token, p__declaration_part_);
			p_token.parent = p__anglr_file_fragment__ref;
			p__declaration_part_.parent = p__anglr_file_fragment__ref;
			Raise__anglr_file_fragment__Event (SyntaxTreeCallbackReason.BuilderCallbackReason, _anglr_file_fragment_.production_kind.g__anglr_file_fragment__9, p__anglr_file_fragment__ref);
			return p__anglr_file_fragment__ref;
		}

		//
		// create syntax tree node associated with production
		// <anglr file fragment>: <anglr definition list terminal> <anglr definition list>

		//

		public _anglr_file_fragment_ _anglr_file_fragment__10 (SyntaxTreeToken p_token, _anglr_definition_list_ p__anglr_definition_list_)
		{
			_anglr_file_fragment_ p__anglr_file_fragment__ref = new _anglr_file_fragment_(p_token, p__anglr_definition_list_);
			p_token.parent = p__anglr_file_fragment__ref;
			p__anglr_definition_list_.parent = p__anglr_file_fragment__ref;
			Raise__anglr_file_fragment__Event (SyntaxTreeCallbackReason.BuilderCallbackReason, _anglr_file_fragment_.production_kind.g__anglr_file_fragment__10, p__anglr_file_fragment__ref);
			return p__anglr_file_fragment__ref;
		}

		//
		// create syntax tree node associated with production
		// <anglr file fragment>: <anglr definition with attribute list terminal> <anglr definition with attribute>

		//

		public _anglr_file_fragment_ _anglr_file_fragment__11 (SyntaxTreeToken p_token, _anglr_definition_with_attribute_ p__anglr_definition_with_attribute_)
		{
			_anglr_file_fragment_ p__anglr_file_fragment__ref = new _anglr_file_fragment_(p_token, p__anglr_definition_with_attribute_);
			p_token.parent = p__anglr_file_fragment__ref;
			p__anglr_definition_with_attribute_.parent = p__anglr_file_fragment__ref;
			Raise__anglr_file_fragment__Event (SyntaxTreeCallbackReason.BuilderCallbackReason, _anglr_file_fragment_.production_kind.g__anglr_file_fragment__11, p__anglr_file_fragment__ref);
			return p__anglr_file_fragment__ref;
		}

		//
		// create syntax tree node associated with production
		// <anglr file fragment>: <anglr definition terminal> <anglr definition>

		//

		public _anglr_file_fragment_ _anglr_file_fragment__12 (SyntaxTreeToken p_token, _anglr_definition_ p__anglr_definition_)
		{
			_anglr_file_fragment_ p__anglr_file_fragment__ref = new _anglr_file_fragment_(p_token, p__anglr_definition_);
			p_token.parent = p__anglr_file_fragment__ref;
			p__anglr_definition_.parent = p__anglr_file_fragment__ref;
			Raise__anglr_file_fragment__Event (SyntaxTreeCallbackReason.BuilderCallbackReason, _anglr_file_fragment_.production_kind.g__anglr_file_fragment__12, p__anglr_file_fragment__ref);
			return p__anglr_file_fragment__ref;
		}

		//
		// create syntax tree node associated with production
		// <anglr file fragment>: <single terminal definition terminal> <single terminal definition>

		//

		public _anglr_file_fragment_ _anglr_file_fragment__13 (SyntaxTreeToken p_token, _single_terminal_definition_ p__single_terminal_definition_)
		{
			_anglr_file_fragment_ p__anglr_file_fragment__ref = new _anglr_file_fragment_(p_token, p__single_terminal_definition_);
			p_token.parent = p__anglr_file_fragment__ref;
			p__single_terminal_definition_.parent = p__anglr_file_fragment__ref;
			Raise__anglr_file_fragment__Event (SyntaxTreeCallbackReason.BuilderCallbackReason, _anglr_file_fragment_.production_kind.g__anglr_file_fragment__13, p__anglr_file_fragment__ref);
			return p__anglr_file_fragment__ref;
		}

		//
		// create syntax tree node associated with production
		// <anglr file fragment>: <single regex definition terminal> <single regex definition>

		//

		public _anglr_file_fragment_ _anglr_file_fragment__14 (SyntaxTreeToken p_token, _single_regex_definition_ p__single_regex_definition_)
		{
			_anglr_file_fragment_ p__anglr_file_fragment__ref = new _anglr_file_fragment_(p_token, p__single_regex_definition_);
			p_token.parent = p__anglr_file_fragment__ref;
			p__single_regex_definition_.parent = p__anglr_file_fragment__ref;
			Raise__anglr_file_fragment__Event (SyntaxTreeCallbackReason.BuilderCallbackReason, _anglr_file_fragment_.production_kind.g__anglr_file_fragment__14, p__anglr_file_fragment__ref);
			return p__anglr_file_fragment__ref;
		}

		//
		// create syntax tree node associated with production
		// <anglr file fragment>: <block of terminal definitions terminal> <block of terminal definitions>

		//

		public _anglr_file_fragment_ _anglr_file_fragment__15 (SyntaxTreeToken p_token, _block_of_terminal_definitions_ p__block_of_terminal_definitions_)
		{
			_anglr_file_fragment_ p__anglr_file_fragment__ref = new _anglr_file_fragment_(p_token, p__block_of_terminal_definitions_);
			p_token.parent = p__anglr_file_fragment__ref;
			p__block_of_terminal_definitions_.parent = p__anglr_file_fragment__ref;
			Raise__anglr_file_fragment__Event (SyntaxTreeCallbackReason.BuilderCallbackReason, _anglr_file_fragment_.production_kind.g__anglr_file_fragment__15, p__anglr_file_fragment__ref);
			return p__anglr_file_fragment__ref;
		}

		//
		// create syntax tree node associated with production
		// <anglr file fragment>: <block of regex definitions terminal> <block of regex definitions>

		//

		public _anglr_file_fragment_ _anglr_file_fragment__16 (SyntaxTreeToken p_token, _block_of_regex_definitions_ p__block_of_regex_definitions_)
		{
			_anglr_file_fragment_ p__anglr_file_fragment__ref = new _anglr_file_fragment_(p_token, p__block_of_regex_definitions_);
			p_token.parent = p__anglr_file_fragment__ref;
			p__block_of_regex_definitions_.parent = p__anglr_file_fragment__ref;
			Raise__anglr_file_fragment__Event (SyntaxTreeCallbackReason.BuilderCallbackReason, _anglr_file_fragment_.production_kind.g__anglr_file_fragment__16, p__anglr_file_fragment__ref);
			return p__anglr_file_fragment__ref;
		}

		//
		// create syntax tree node associated with production
		// <anglr file fragment>: <terminal definition terminal> <terminal definition>

		//

		public _anglr_file_fragment_ _anglr_file_fragment__17 (SyntaxTreeToken p_token, _terminal_definition_ p__terminal_definition_)
		{
			_anglr_file_fragment_ p__anglr_file_fragment__ref = new _anglr_file_fragment_(p_token, p__terminal_definition_);
			p_token.parent = p__anglr_file_fragment__ref;
			p__terminal_definition_.parent = p__anglr_file_fragment__ref;
			Raise__anglr_file_fragment__Event (SyntaxTreeCallbackReason.BuilderCallbackReason, _anglr_file_fragment_.production_kind.g__anglr_file_fragment__17, p__anglr_file_fragment__ref);
			return p__anglr_file_fragment__ref;
		}

		//
		// create syntax tree node associated with production
		// <anglr file fragment>: <regex definition terminal> <regex definition>

		//

		public _anglr_file_fragment_ _anglr_file_fragment__18 (SyntaxTreeToken p_token, _regex_definition_ p__regex_definition_)
		{
			_anglr_file_fragment_ p__anglr_file_fragment__ref = new _anglr_file_fragment_(p_token, p__regex_definition_);
			p_token.parent = p__anglr_file_fragment__ref;
			p__regex_definition_.parent = p__anglr_file_fragment__ref;
			Raise__anglr_file_fragment__Event (SyntaxTreeCallbackReason.BuilderCallbackReason, _anglr_file_fragment_.production_kind.g__anglr_file_fragment__18, p__anglr_file_fragment__ref);
			return p__anglr_file_fragment__ref;
		}

		//
		// create syntax tree node associated with production
		// <anglr file fragment>: <block terminal definitions terminal> <block terminal definitions>

		//

		public _anglr_file_fragment_ _anglr_file_fragment__19 (SyntaxTreeToken p_token, _block_terminal_definitions_ p__block_terminal_definitions_)
		{
			_anglr_file_fragment_ p__anglr_file_fragment__ref = new _anglr_file_fragment_(p_token, p__block_terminal_definitions_);
			p_token.parent = p__anglr_file_fragment__ref;
			p__block_terminal_definitions_.parent = p__anglr_file_fragment__ref;
			Raise__anglr_file_fragment__Event (SyntaxTreeCallbackReason.BuilderCallbackReason, _anglr_file_fragment_.production_kind.g__anglr_file_fragment__19, p__anglr_file_fragment__ref);
			return p__anglr_file_fragment__ref;
		}

		//
		// create syntax tree node associated with production
		// <anglr file fragment>: <block terminal definition terminal> <block terminal definition>

		//

		public _anglr_file_fragment_ _anglr_file_fragment__20 (SyntaxTreeToken p_token, _block_terminal_definition_ p__block_terminal_definition_)
		{
			_anglr_file_fragment_ p__anglr_file_fragment__ref = new _anglr_file_fragment_(p_token, p__block_terminal_definition_);
			p_token.parent = p__anglr_file_fragment__ref;
			p__block_terminal_definition_.parent = p__anglr_file_fragment__ref;
			Raise__anglr_file_fragment__Event (SyntaxTreeCallbackReason.BuilderCallbackReason, _anglr_file_fragment_.production_kind.g__anglr_file_fragment__20, p__anglr_file_fragment__ref);
			return p__anglr_file_fragment__ref;
		}

		//
		// create syntax tree node associated with production
		// <anglr file fragment>: <block regex definitions terminal> <block regex definitions>

		//

		public _anglr_file_fragment_ _anglr_file_fragment__21 (SyntaxTreeToken p_token, _block_regex_definitions_ p__block_regex_definitions_)
		{
			_anglr_file_fragment_ p__anglr_file_fragment__ref = new _anglr_file_fragment_(p_token, p__block_regex_definitions_);
			p_token.parent = p__anglr_file_fragment__ref;
			p__block_regex_definitions_.parent = p__anglr_file_fragment__ref;
			Raise__anglr_file_fragment__Event (SyntaxTreeCallbackReason.BuilderCallbackReason, _anglr_file_fragment_.production_kind.g__anglr_file_fragment__21, p__anglr_file_fragment__ref);
			return p__anglr_file_fragment__ref;
		}

		//
		// create syntax tree node associated with production
		// <anglr file fragment>: <block regex definition terminal> <block regex definition>

		//

		public _anglr_file_fragment_ _anglr_file_fragment__22 (SyntaxTreeToken p_token, _block_regex_definition_ p__block_regex_definition_)
		{
			_anglr_file_fragment_ p__anglr_file_fragment__ref = new _anglr_file_fragment_(p_token, p__block_regex_definition_);
			p_token.parent = p__anglr_file_fragment__ref;
			p__block_regex_definition_.parent = p__anglr_file_fragment__ref;
			Raise__anglr_file_fragment__Event (SyntaxTreeCallbackReason.BuilderCallbackReason, _anglr_file_fragment_.production_kind.g__anglr_file_fragment__22, p__anglr_file_fragment__ref);
			return p__anglr_file_fragment__ref;
		}

		//
		// create syntax tree node associated with production
		// <anglr file fragment>: <scanner part terminal> <scanner part>

		//

		public _anglr_file_fragment_ _anglr_file_fragment__23 (SyntaxTreeToken p_token, _scanner_part_ p__scanner_part_)
		{
			_anglr_file_fragment_ p__anglr_file_fragment__ref = new _anglr_file_fragment_(p_token, p__scanner_part_);
			p_token.parent = p__anglr_file_fragment__ref;
			p__scanner_part_.parent = p__anglr_file_fragment__ref;
			Raise__anglr_file_fragment__Event (SyntaxTreeCallbackReason.BuilderCallbackReason, _anglr_file_fragment_.production_kind.g__anglr_file_fragment__23, p__anglr_file_fragment__ref);
			return p__anglr_file_fragment__ref;
		}

		//
		// create syntax tree node associated with production
		// <anglr file fragment>: <regular expression list terminal> <regular expression list>

		//

		public _anglr_file_fragment_ _anglr_file_fragment__24 (SyntaxTreeToken p_token, _regular_expression_list_ p__regular_expression_list_)
		{
			_anglr_file_fragment_ p__anglr_file_fragment__ref = new _anglr_file_fragment_(p_token, p__regular_expression_list_);
			p_token.parent = p__anglr_file_fragment__ref;
			p__regular_expression_list_.parent = p__anglr_file_fragment__ref;
			Raise__anglr_file_fragment__Event (SyntaxTreeCallbackReason.BuilderCallbackReason, _anglr_file_fragment_.production_kind.g__anglr_file_fragment__24, p__anglr_file_fragment__ref);
			return p__anglr_file_fragment__ref;
		}

		//
		// create syntax tree node associated with production
		// <anglr file fragment>: <regular expression usage terminal> <regular expression usage>

		//

		public _anglr_file_fragment_ _anglr_file_fragment__25 (SyntaxTreeToken p_token, _regular_expression_usage_ p__regular_expression_usage_)
		{
			_anglr_file_fragment_ p__anglr_file_fragment__ref = new _anglr_file_fragment_(p_token, p__regular_expression_usage_);
			p_token.parent = p__anglr_file_fragment__ref;
			p__regular_expression_usage_.parent = p__anglr_file_fragment__ref;
			Raise__anglr_file_fragment__Event (SyntaxTreeCallbackReason.BuilderCallbackReason, _anglr_file_fragment_.production_kind.g__anglr_file_fragment__25, p__anglr_file_fragment__ref);
			return p__anglr_file_fragment__ref;
		}

		//
		// create syntax tree node associated with production
		// <anglr file fragment>: <actions terminal> <actions>

		//

		public _anglr_file_fragment_ _anglr_file_fragment__26 (SyntaxTreeToken p_token, _actions_ p__actions_)
		{
			_anglr_file_fragment_ p__anglr_file_fragment__ref = new _anglr_file_fragment_(p_token, p__actions_);
			p_token.parent = p__anglr_file_fragment__ref;
			p__actions_.parent = p__anglr_file_fragment__ref;
			Raise__anglr_file_fragment__Event (SyntaxTreeCallbackReason.BuilderCallbackReason, _anglr_file_fragment_.production_kind.g__anglr_file_fragment__26, p__anglr_file_fragment__ref);
			return p__anglr_file_fragment__ref;
		}

		//
		// create syntax tree node associated with production
		// <anglr file fragment>: <action terminal> <action>

		//

		public _anglr_file_fragment_ _anglr_file_fragment__27 (SyntaxTreeToken p_token, _action_ p__action_)
		{
			_anglr_file_fragment_ p__anglr_file_fragment__ref = new _anglr_file_fragment_(p_token, p__action_);
			p_token.parent = p__anglr_file_fragment__ref;
			p__action_.parent = p__anglr_file_fragment__ref;
			Raise__anglr_file_fragment__Event (SyntaxTreeCallbackReason.BuilderCallbackReason, _anglr_file_fragment_.production_kind.g__anglr_file_fragment__27, p__anglr_file_fragment__ref);
			return p__anglr_file_fragment__ref;
		}

		//
		// create syntax tree node associated with production
		// <anglr file fragment>: <skip action terminal> <skip action>

		//

		public _anglr_file_fragment_ _anglr_file_fragment__28 (SyntaxTreeToken p_token, _skip_action_ p__skip_action_)
		{
			_anglr_file_fragment_ p__anglr_file_fragment__ref = new _anglr_file_fragment_(p_token, p__skip_action_);
			p_token.parent = p__anglr_file_fragment__ref;
			p__skip_action_.parent = p__anglr_file_fragment__ref;
			Raise__anglr_file_fragment__Event (SyntaxTreeCallbackReason.BuilderCallbackReason, _anglr_file_fragment_.production_kind.g__anglr_file_fragment__28, p__anglr_file_fragment__ref);
			return p__anglr_file_fragment__ref;
		}

		//
		// create syntax tree node associated with production
		// <anglr file fragment>: <terminal action terminal> <terminal action>

		//

		public _anglr_file_fragment_ _anglr_file_fragment__29 (SyntaxTreeToken p_token, _terminal_action_ p__terminal_action_)
		{
			_anglr_file_fragment_ p__anglr_file_fragment__ref = new _anglr_file_fragment_(p_token, p__terminal_action_);
			p_token.parent = p__anglr_file_fragment__ref;
			p__terminal_action_.parent = p__anglr_file_fragment__ref;
			Raise__anglr_file_fragment__Event (SyntaxTreeCallbackReason.BuilderCallbackReason, _anglr_file_fragment_.production_kind.g__anglr_file_fragment__29, p__anglr_file_fragment__ref);
			return p__anglr_file_fragment__ref;
		}

		//
		// create syntax tree node associated with production
		// <anglr file fragment>: <event action terminal> <event action>

		//

		public _anglr_file_fragment_ _anglr_file_fragment__30 (SyntaxTreeToken p_token, _event_action_ p__event_action_)
		{
			_anglr_file_fragment_ p__anglr_file_fragment__ref = new _anglr_file_fragment_(p_token, p__event_action_);
			p_token.parent = p__anglr_file_fragment__ref;
			p__event_action_.parent = p__anglr_file_fragment__ref;
			Raise__anglr_file_fragment__Event (SyntaxTreeCallbackReason.BuilderCallbackReason, _anglr_file_fragment_.production_kind.g__anglr_file_fragment__30, p__anglr_file_fragment__ref);
			return p__anglr_file_fragment__ref;
		}

		//
		// create syntax tree node associated with production
		// <anglr file fragment>: <push action terminal> <push action>

		//

		public _anglr_file_fragment_ _anglr_file_fragment__31 (SyntaxTreeToken p_token, _push_action_ p__push_action_)
		{
			_anglr_file_fragment_ p__anglr_file_fragment__ref = new _anglr_file_fragment_(p_token, p__push_action_);
			p_token.parent = p__anglr_file_fragment__ref;
			p__push_action_.parent = p__anglr_file_fragment__ref;
			Raise__anglr_file_fragment__Event (SyntaxTreeCallbackReason.BuilderCallbackReason, _anglr_file_fragment_.production_kind.g__anglr_file_fragment__31, p__anglr_file_fragment__ref);
			return p__anglr_file_fragment__ref;
		}

		//
		// create syntax tree node associated with production
		// <anglr file fragment>: <pop action terminal> <pop action>

		//

		public _anglr_file_fragment_ _anglr_file_fragment__32 (SyntaxTreeToken p_token, _pop_action_ p__pop_action_)
		{
			_anglr_file_fragment_ p__anglr_file_fragment__ref = new _anglr_file_fragment_(p_token, p__pop_action_);
			p_token.parent = p__anglr_file_fragment__ref;
			p__pop_action_.parent = p__anglr_file_fragment__ref;
			Raise__anglr_file_fragment__Event (SyntaxTreeCallbackReason.BuilderCallbackReason, _anglr_file_fragment_.production_kind.g__anglr_file_fragment__32, p__anglr_file_fragment__ref);
			return p__anglr_file_fragment__ref;
		}

		//
		// create syntax tree node associated with production
		// <anglr file fragment>: <lexer part terminal> <lexer part>

		//

		public _anglr_file_fragment_ _anglr_file_fragment__33 (SyntaxTreeToken p_token, _lexer_part_ p__lexer_part_)
		{
			_anglr_file_fragment_ p__anglr_file_fragment__ref = new _anglr_file_fragment_(p_token, p__lexer_part_);
			p_token.parent = p__anglr_file_fragment__ref;
			p__lexer_part_.parent = p__anglr_file_fragment__ref;
			Raise__anglr_file_fragment__Event (SyntaxTreeCallbackReason.BuilderCallbackReason, _anglr_file_fragment_.production_kind.g__anglr_file_fragment__33, p__anglr_file_fragment__ref);
			return p__anglr_file_fragment__ref;
		}

		//
		// create syntax tree node associated with production
		// <anglr file fragment>: <parser part terminal> <parser part>

		//

		public _anglr_file_fragment_ _anglr_file_fragment__34 (SyntaxTreeToken p_token, _parser_part_ p__parser_part_)
		{
			_anglr_file_fragment_ p__anglr_file_fragment__ref = new _anglr_file_fragment_(p_token, p__parser_part_);
			p_token.parent = p__anglr_file_fragment__ref;
			p__parser_part_.parent = p__anglr_file_fragment__ref;
			Raise__anglr_file_fragment__Event (SyntaxTreeCallbackReason.BuilderCallbackReason, _anglr_file_fragment_.production_kind.g__anglr_file_fragment__34, p__anglr_file_fragment__ref);
			return p__anglr_file_fragment__ref;
		}

		//
		// create syntax tree node associated with production
		// <anglr file fragment>: <anglr syntax rule list terminal> <anglr syntax rule list>

		//

		public _anglr_file_fragment_ _anglr_file_fragment__35 (SyntaxTreeToken p_token, _anglr_syntax_rule_list_ p__anglr_syntax_rule_list_)
		{
			_anglr_file_fragment_ p__anglr_file_fragment__ref = new _anglr_file_fragment_(p_token, p__anglr_syntax_rule_list_);
			p_token.parent = p__anglr_file_fragment__ref;
			p__anglr_syntax_rule_list_.parent = p__anglr_file_fragment__ref;
			Raise__anglr_file_fragment__Event (SyntaxTreeCallbackReason.BuilderCallbackReason, _anglr_file_fragment_.production_kind.g__anglr_file_fragment__35, p__anglr_file_fragment__ref);
			return p__anglr_file_fragment__ref;
		}

		//
		// create syntax tree node associated with production
		// <anglr file fragment>: <anglr syntax rule terminal> <anglr syntax rule>

		//

		public _anglr_file_fragment_ _anglr_file_fragment__36 (SyntaxTreeToken p_token, _anglr_syntax_rule_ p__anglr_syntax_rule_)
		{
			_anglr_file_fragment_ p__anglr_file_fragment__ref = new _anglr_file_fragment_(p_token, p__anglr_syntax_rule_);
			p_token.parent = p__anglr_file_fragment__ref;
			p__anglr_syntax_rule_.parent = p__anglr_file_fragment__ref;
			Raise__anglr_file_fragment__Event (SyntaxTreeCallbackReason.BuilderCallbackReason, _anglr_file_fragment_.production_kind.g__anglr_file_fragment__36, p__anglr_file_fragment__ref);
			return p__anglr_file_fragment__ref;
		}

		//
		// create syntax tree node associated with production
		// <anglr file fragment>: <anglr nested rule terminal> <anglr nested rule>

		//

		public _anglr_file_fragment_ _anglr_file_fragment__37 (SyntaxTreeToken p_token, _anglr_nested_rule_ p__anglr_nested_rule_)
		{
			_anglr_file_fragment_ p__anglr_file_fragment__ref = new _anglr_file_fragment_(p_token, p__anglr_nested_rule_);
			p_token.parent = p__anglr_file_fragment__ref;
			p__anglr_nested_rule_.parent = p__anglr_file_fragment__ref;
			Raise__anglr_file_fragment__Event (SyntaxTreeCallbackReason.BuilderCallbackReason, _anglr_file_fragment_.production_kind.g__anglr_file_fragment__37, p__anglr_file_fragment__ref);
			return p__anglr_file_fragment__ref;
		}

		//
		// create syntax tree node associated with production
		// <anglr file fragment>: <anglr syntax production list name terminal> <anglr syntax production list name>

		//

		public _anglr_file_fragment_ _anglr_file_fragment__38 (SyntaxTreeToken p_token, _anglr_syntax_production_list_name_ p__anglr_syntax_production_list_name_)
		{
			_anglr_file_fragment_ p__anglr_file_fragment__ref = new _anglr_file_fragment_(p_token, p__anglr_syntax_production_list_name_);
			p_token.parent = p__anglr_file_fragment__ref;
			p__anglr_syntax_production_list_name_.parent = p__anglr_file_fragment__ref;
			Raise__anglr_file_fragment__Event (SyntaxTreeCallbackReason.BuilderCallbackReason, _anglr_file_fragment_.production_kind.g__anglr_file_fragment__38, p__anglr_file_fragment__ref);
			return p__anglr_file_fragment__ref;
		}

		//
		// create syntax tree node associated with production
		// <anglr file fragment>: <anglr syntax production list terminal> <anglr syntax production list>

		//

		public _anglr_file_fragment_ _anglr_file_fragment__39 (SyntaxTreeToken p_token, _anglr_syntax_production_list_ p__anglr_syntax_production_list_)
		{
			_anglr_file_fragment_ p__anglr_file_fragment__ref = new _anglr_file_fragment_(p_token, p__anglr_syntax_production_list_);
			p_token.parent = p__anglr_file_fragment__ref;
			p__anglr_syntax_production_list_.parent = p__anglr_file_fragment__ref;
			Raise__anglr_file_fragment__Event (SyntaxTreeCallbackReason.BuilderCallbackReason, _anglr_file_fragment_.production_kind.g__anglr_file_fragment__39, p__anglr_file_fragment__ref);
			return p__anglr_file_fragment__ref;
		}

		//
		// create syntax tree node associated with production
		// <anglr file fragment>: <anglr syntax production terminal> <anglr syntax production>

		//

		public _anglr_file_fragment_ _anglr_file_fragment__40 (SyntaxTreeToken p_token, _anglr_syntax_production_ p__anglr_syntax_production_)
		{
			_anglr_file_fragment_ p__anglr_file_fragment__ref = new _anglr_file_fragment_(p_token, p__anglr_syntax_production_);
			p_token.parent = p__anglr_file_fragment__ref;
			p__anglr_syntax_production_.parent = p__anglr_file_fragment__ref;
			Raise__anglr_file_fragment__Event (SyntaxTreeCallbackReason.BuilderCallbackReason, _anglr_file_fragment_.production_kind.g__anglr_file_fragment__40, p__anglr_file_fragment__ref);
			return p__anglr_file_fragment__ref;
		}

		//
		// create syntax tree node associated with production
		// <anglr file fragment>: <priority assoc specification terminal> <priority assoc specification>

		//

		public _anglr_file_fragment_ _anglr_file_fragment__41 (SyntaxTreeToken p_token, _priority_assoc_specification_ p__priority_assoc_specification_)
		{
			_anglr_file_fragment_ p__anglr_file_fragment__ref = new _anglr_file_fragment_(p_token, p__priority_assoc_specification_);
			p_token.parent = p__anglr_file_fragment__ref;
			p__priority_assoc_specification_.parent = p__anglr_file_fragment__ref;
			Raise__anglr_file_fragment__Event (SyntaxTreeCallbackReason.BuilderCallbackReason, _anglr_file_fragment_.production_kind.g__anglr_file_fragment__41, p__anglr_file_fragment__ref);
			return p__anglr_file_fragment__ref;
		}

		//
		// create syntax tree node associated with production
		// <anglr file fragment>: <priority specification terminal> <priority specification>

		//

		public _anglr_file_fragment_ _anglr_file_fragment__42 (SyntaxTreeToken p_token, _priority_specification_ p__priority_specification_)
		{
			_anglr_file_fragment_ p__anglr_file_fragment__ref = new _anglr_file_fragment_(p_token, p__priority_specification_);
			p_token.parent = p__anglr_file_fragment__ref;
			p__priority_specification_.parent = p__anglr_file_fragment__ref;
			Raise__anglr_file_fragment__Event (SyntaxTreeCallbackReason.BuilderCallbackReason, _anglr_file_fragment_.production_kind.g__anglr_file_fragment__42, p__anglr_file_fragment__ref);
			return p__anglr_file_fragment__ref;
		}

		//
		// create syntax tree node associated with production
		// <anglr file fragment>: <associativity specification terminal> <associativity specification>

		//

		public _anglr_file_fragment_ _anglr_file_fragment__43 (SyntaxTreeToken p_token, _associativity_specification_ p__associativity_specification_)
		{
			_anglr_file_fragment_ p__anglr_file_fragment__ref = new _anglr_file_fragment_(p_token, p__associativity_specification_);
			p_token.parent = p__anglr_file_fragment__ref;
			p__associativity_specification_.parent = p__anglr_file_fragment__ref;
			Raise__anglr_file_fragment__Event (SyntaxTreeCallbackReason.BuilderCallbackReason, _anglr_file_fragment_.production_kind.g__anglr_file_fragment__43, p__anglr_file_fragment__ref);
			return p__anglr_file_fragment__ref;
		}

		//
		// create syntax tree node associated with production
		// <anglr file fragment>: <production name terminal> <production name>

		//

		public _anglr_file_fragment_ _anglr_file_fragment__44 (SyntaxTreeToken p_token, _production_name_ p__production_name_)
		{
			_anglr_file_fragment_ p__anglr_file_fragment__ref = new _anglr_file_fragment_(p_token, p__production_name_);
			p_token.parent = p__anglr_file_fragment__ref;
			p__production_name_.parent = p__anglr_file_fragment__ref;
			Raise__anglr_file_fragment__Event (SyntaxTreeCallbackReason.BuilderCallbackReason, _anglr_file_fragment_.production_kind.g__anglr_file_fragment__44, p__anglr_file_fragment__ref);
			return p__anglr_file_fragment__ref;
		}

		//
		// create syntax tree node associated with production
		// <anglr file fragment>: <name list terminal> <name list>

		//

		public _anglr_file_fragment_ _anglr_file_fragment__45 (SyntaxTreeToken p_token, _name_list_ p__name_list_)
		{
			_anglr_file_fragment_ p__anglr_file_fragment__ref = new _anglr_file_fragment_(p_token, p__name_list_);
			p_token.parent = p__anglr_file_fragment__ref;
			p__name_list_.parent = p__anglr_file_fragment__ref;
			Raise__anglr_file_fragment__Event (SyntaxTreeCallbackReason.BuilderCallbackReason, _anglr_file_fragment_.production_kind.g__anglr_file_fragment__45, p__anglr_file_fragment__ref);
			return p__anglr_file_fragment__ref;
		}

		//
		// create syntax tree node associated with production
		// <anglr file fragment>: <marker list terminal> <marker list>

		//

		public _anglr_file_fragment_ _anglr_file_fragment__46 (SyntaxTreeToken p_token, _marker_list_ p__marker_list_)
		{
			_anglr_file_fragment_ p__anglr_file_fragment__ref = new _anglr_file_fragment_(p_token, p__marker_list_);
			p_token.parent = p__anglr_file_fragment__ref;
			p__marker_list_.parent = p__anglr_file_fragment__ref;
			Raise__anglr_file_fragment__Event (SyntaxTreeCallbackReason.BuilderCallbackReason, _anglr_file_fragment_.production_kind.g__anglr_file_fragment__46, p__anglr_file_fragment__ref);
			return p__anglr_file_fragment__ref;
		}

		//
		// create syntax tree node associated with production
		// <anglr file fragment>: <marker terminal> <marker>

		//

		public _anglr_file_fragment_ _anglr_file_fragment__47 (SyntaxTreeToken p_token, _marker_ p__marker_)
		{
			_anglr_file_fragment_ p__anglr_file_fragment__ref = new _anglr_file_fragment_(p_token, p__marker_);
			p_token.parent = p__anglr_file_fragment__ref;
			p__marker_.parent = p__anglr_file_fragment__ref;
			Raise__anglr_file_fragment__Event (SyntaxTreeCallbackReason.BuilderCallbackReason, _anglr_file_fragment_.production_kind.g__anglr_file_fragment__47, p__anglr_file_fragment__ref);
			return p__anglr_file_fragment__ref;
		}

		//
		// create syntax tree node associated with production
		// <anglr file fragment>: <g name terminal> <g name>

		//

		public _anglr_file_fragment_ _anglr_file_fragment__48 (SyntaxTreeToken p_token, _g_name_ p__g_name_)
		{
			_anglr_file_fragment_ p__anglr_file_fragment__ref = new _anglr_file_fragment_(p_token, p__g_name_);
			p_token.parent = p__anglr_file_fragment__ref;
			p__g_name_.parent = p__anglr_file_fragment__ref;
			Raise__anglr_file_fragment__Event (SyntaxTreeCallbackReason.BuilderCallbackReason, _anglr_file_fragment_.production_kind.g__anglr_file_fragment__48, p__anglr_file_fragment__ref);
			return p__anglr_file_fragment__ref;
		}

		//
		// create syntax tree node associated with production
		// <anglr file fragment>: <name terminal> <name>

		//

		public _anglr_file_fragment_ _anglr_file_fragment__49 (SyntaxTreeToken p_token, _name_ p__name_)
		{
			_anglr_file_fragment_ p__anglr_file_fragment__ref = new _anglr_file_fragment_(p_token, p__name_);
			p_token.parent = p__anglr_file_fragment__ref;
			p__name_.parent = p__anglr_file_fragment__ref;
			Raise__anglr_file_fragment__Event (SyntaxTreeCallbackReason.BuilderCallbackReason, _anglr_file_fragment_.production_kind.g__anglr_file_fragment__49, p__anglr_file_fragment__ref);
			return p__anglr_file_fragment__ref;
		}

		//
		// create syntax tree node associated with production
		// <anglr file fragment>: <cardinality delimiter terminal> <cardinality delimiter>

		//

		public _anglr_file_fragment_ _anglr_file_fragment__50 (SyntaxTreeToken p_token, _cardinality_delimiter_ p__cardinality_delimiter_)
		{
			_anglr_file_fragment_ p__anglr_file_fragment__ref = new _anglr_file_fragment_(p_token, p__cardinality_delimiter_);
			p_token.parent = p__anglr_file_fragment__ref;
			p__cardinality_delimiter_.parent = p__anglr_file_fragment__ref;
			Raise__anglr_file_fragment__Event (SyntaxTreeCallbackReason.BuilderCallbackReason, _anglr_file_fragment_.production_kind.g__anglr_file_fragment__50, p__anglr_file_fragment__ref);
			return p__anglr_file_fragment__ref;
		}

		//
		// create syntax tree node associated with production
		// <anglr file fragment>: <cardinality terminal> <cardinality>

		//

		public _anglr_file_fragment_ _anglr_file_fragment__51 (SyntaxTreeToken p_token, _cardinality_ p__cardinality_)
		{
			_anglr_file_fragment_ p__anglr_file_fragment__ref = new _anglr_file_fragment_(p_token, p__cardinality_);
			p_token.parent = p__anglr_file_fragment__ref;
			p__cardinality_.parent = p__anglr_file_fragment__ref;
			Raise__anglr_file_fragment__Event (SyntaxTreeCallbackReason.BuilderCallbackReason, _anglr_file_fragment_.production_kind.g__anglr_file_fragment__51, p__anglr_file_fragment__ref);
			return p__anglr_file_fragment__ref;
		}

		//
		// create syntax tree node associated with production
		// <anglr file fragment>: <delimiter terminal> <delimiter>

		//

		public _anglr_file_fragment_ _anglr_file_fragment__52 (SyntaxTreeToken p_token, _delimiter_ p__delimiter_)
		{
			_anglr_file_fragment_ p__anglr_file_fragment__ref = new _anglr_file_fragment_(p_token, p__delimiter_);
			p_token.parent = p__anglr_file_fragment__ref;
			p__delimiter_.parent = p__anglr_file_fragment__ref;
			Raise__anglr_file_fragment__Event (SyntaxTreeCallbackReason.BuilderCallbackReason, _anglr_file_fragment_.production_kind.g__anglr_file_fragment__52, p__anglr_file_fragment__ref);
			return p__anglr_file_fragment__ref;
		}

		//
		// create syntax tree node associated with production
		// <anglr file fragment>: <attribute list optional terminal> <attribute list optional>

		//

		public _anglr_file_fragment_ _anglr_file_fragment__53 (SyntaxTreeToken p_token, _attribute_list_optional_ p__attribute_list_optional_)
		{
			_anglr_file_fragment_ p__anglr_file_fragment__ref = new _anglr_file_fragment_(p_token, p__attribute_list_optional_);
			p_token.parent = p__anglr_file_fragment__ref;
			p__attribute_list_optional_.parent = p__anglr_file_fragment__ref;
			Raise__anglr_file_fragment__Event (SyntaxTreeCallbackReason.BuilderCallbackReason, _anglr_file_fragment_.production_kind.g__anglr_file_fragment__53, p__anglr_file_fragment__ref);
			return p__anglr_file_fragment__ref;
		}

		//
		// create syntax tree node associated with production
		// <anglr file fragment>: <name value list optional terminal> <name value list optional>

		//

		public _anglr_file_fragment_ _anglr_file_fragment__54 (SyntaxTreeToken p_token, _name_value_list_optional_ p__name_value_list_optional_)
		{
			_anglr_file_fragment_ p__anglr_file_fragment__ref = new _anglr_file_fragment_(p_token, p__name_value_list_optional_);
			p_token.parent = p__anglr_file_fragment__ref;
			p__name_value_list_optional_.parent = p__anglr_file_fragment__ref;
			Raise__anglr_file_fragment__Event (SyntaxTreeCallbackReason.BuilderCallbackReason, _anglr_file_fragment_.production_kind.g__anglr_file_fragment__54, p__anglr_file_fragment__ref);
			return p__anglr_file_fragment__ref;
		}

		//
		// create syntax tree node associated with production
		// <anglr file fragment>: <anglr file part list optional terminal> <anglr file part list optional>

		//

		public _anglr_file_fragment_ _anglr_file_fragment__55 (SyntaxTreeToken p_token, _anglr_file_part_list_optional_ p__anglr_file_part_list_optional_)
		{
			_anglr_file_fragment_ p__anglr_file_fragment__ref = new _anglr_file_fragment_(p_token, p__anglr_file_part_list_optional_);
			p_token.parent = p__anglr_file_fragment__ref;
			p__anglr_file_part_list_optional_.parent = p__anglr_file_fragment__ref;
			Raise__anglr_file_fragment__Event (SyntaxTreeCallbackReason.BuilderCallbackReason, _anglr_file_fragment_.production_kind.g__anglr_file_fragment__55, p__anglr_file_fragment__ref);
			return p__anglr_file_fragment__ref;
		}

		//
		// create syntax tree node associated with production
		// <anglr file fragment>: <anglr definition list optional terminal> <anglr definition list optional>

		//

		public _anglr_file_fragment_ _anglr_file_fragment__56 (SyntaxTreeToken p_token, _anglr_definition_list_optional_ p__anglr_definition_list_optional_)
		{
			_anglr_file_fragment_ p__anglr_file_fragment__ref = new _anglr_file_fragment_(p_token, p__anglr_definition_list_optional_);
			p_token.parent = p__anglr_file_fragment__ref;
			p__anglr_definition_list_optional_.parent = p__anglr_file_fragment__ref;
			Raise__anglr_file_fragment__Event (SyntaxTreeCallbackReason.BuilderCallbackReason, _anglr_file_fragment_.production_kind.g__anglr_file_fragment__56, p__anglr_file_fragment__ref);
			return p__anglr_file_fragment__ref;
		}

		//
		// create syntax tree node associated with production
		// <anglr file fragment>: <block terminal definitions optional terminal> <block terminal definitions optional>

		//

		public _anglr_file_fragment_ _anglr_file_fragment__57 (SyntaxTreeToken p_token, _block_terminal_definitions_optional_ p__block_terminal_definitions_optional_)
		{
			_anglr_file_fragment_ p__anglr_file_fragment__ref = new _anglr_file_fragment_(p_token, p__block_terminal_definitions_optional_);
			p_token.parent = p__anglr_file_fragment__ref;
			p__block_terminal_definitions_optional_.parent = p__anglr_file_fragment__ref;
			Raise__anglr_file_fragment__Event (SyntaxTreeCallbackReason.BuilderCallbackReason, _anglr_file_fragment_.production_kind.g__anglr_file_fragment__57, p__anglr_file_fragment__ref);
			return p__anglr_file_fragment__ref;
		}

		//
		// create syntax tree node associated with production
		// <anglr file fragment>: <block regex definitions optional terminal> <block regex definitions optional>

		//

		public _anglr_file_fragment_ _anglr_file_fragment__58 (SyntaxTreeToken p_token, _block_regex_definitions_optional_ p__block_regex_definitions_optional_)
		{
			_anglr_file_fragment_ p__anglr_file_fragment__ref = new _anglr_file_fragment_(p_token, p__block_regex_definitions_optional_);
			p_token.parent = p__anglr_file_fragment__ref;
			p__block_regex_definitions_optional_.parent = p__anglr_file_fragment__ref;
			Raise__anglr_file_fragment__Event (SyntaxTreeCallbackReason.BuilderCallbackReason, _anglr_file_fragment_.production_kind.g__anglr_file_fragment__58, p__anglr_file_fragment__ref);
			return p__anglr_file_fragment__ref;
		}

		//
		// create syntax tree node associated with production
		// <anglr file fragment>: <regular expression list optional terminal> <regular expression list optional>

		//

		public _anglr_file_fragment_ _anglr_file_fragment__59 (SyntaxTreeToken p_token, _regular_expression_list_optional_ p__regular_expression_list_optional_)
		{
			_anglr_file_fragment_ p__anglr_file_fragment__ref = new _anglr_file_fragment_(p_token, p__regular_expression_list_optional_);
			p_token.parent = p__anglr_file_fragment__ref;
			p__regular_expression_list_optional_.parent = p__anglr_file_fragment__ref;
			Raise__anglr_file_fragment__Event (SyntaxTreeCallbackReason.BuilderCallbackReason, _anglr_file_fragment_.production_kind.g__anglr_file_fragment__59, p__anglr_file_fragment__ref);
			return p__anglr_file_fragment__ref;
		}

		//
		// create syntax tree node associated with production
		// <anglr file fragment>: <actions optional terminal> <actions optional>

		//

		public _anglr_file_fragment_ _anglr_file_fragment__60 (SyntaxTreeToken p_token, _actions_optional_ p__actions_optional_)
		{
			_anglr_file_fragment_ p__anglr_file_fragment__ref = new _anglr_file_fragment_(p_token, p__actions_optional_);
			p_token.parent = p__anglr_file_fragment__ref;
			p__actions_optional_.parent = p__anglr_file_fragment__ref;
			Raise__anglr_file_fragment__Event (SyntaxTreeCallbackReason.BuilderCallbackReason, _anglr_file_fragment_.production_kind.g__anglr_file_fragment__60, p__anglr_file_fragment__ref);
			return p__anglr_file_fragment__ref;
		}

		//
		// create syntax tree node associated with production
		// <anglr file fragment>: <anglr syntax rule list optional terminal> <anglr syntax rule list optional>

		//

		public _anglr_file_fragment_ _anglr_file_fragment__61 (SyntaxTreeToken p_token, _anglr_syntax_rule_list_optional_ p__anglr_syntax_rule_list_optional_)
		{
			_anglr_file_fragment_ p__anglr_file_fragment__ref = new _anglr_file_fragment_(p_token, p__anglr_syntax_rule_list_optional_);
			p_token.parent = p__anglr_file_fragment__ref;
			p__anglr_syntax_rule_list_optional_.parent = p__anglr_file_fragment__ref;
			Raise__anglr_file_fragment__Event (SyntaxTreeCallbackReason.BuilderCallbackReason, _anglr_file_fragment_.production_kind.g__anglr_file_fragment__61, p__anglr_file_fragment__ref);
			return p__anglr_file_fragment__ref;
		}

		//
		// create syntax tree node associated with production
		// <anglr file fragment>: <anglr syntax production list name optional terminal> <anglr syntax production list name optional>

		//

		public _anglr_file_fragment_ _anglr_file_fragment__62 (SyntaxTreeToken p_token, _anglr_syntax_production_list_name_optional_ p__anglr_syntax_production_list_name_optional_)
		{
			_anglr_file_fragment_ p__anglr_file_fragment__ref = new _anglr_file_fragment_(p_token, p__anglr_syntax_production_list_name_optional_);
			p_token.parent = p__anglr_file_fragment__ref;
			p__anglr_syntax_production_list_name_optional_.parent = p__anglr_file_fragment__ref;
			Raise__anglr_file_fragment__Event (SyntaxTreeCallbackReason.BuilderCallbackReason, _anglr_file_fragment_.production_kind.g__anglr_file_fragment__62, p__anglr_file_fragment__ref);
			return p__anglr_file_fragment__ref;
		}

		//
		// create syntax tree node associated with production
		// <anglr file fragment>: <production name optional terminal> <production name optional>

		//

		public _anglr_file_fragment_ _anglr_file_fragment__63 (SyntaxTreeToken p_token, _production_name_optional_ p__production_name_optional_)
		{
			_anglr_file_fragment_ p__anglr_file_fragment__ref = new _anglr_file_fragment_(p_token, p__production_name_optional_);
			p_token.parent = p__anglr_file_fragment__ref;
			p__production_name_optional_.parent = p__anglr_file_fragment__ref;
			Raise__anglr_file_fragment__Event (SyntaxTreeCallbackReason.BuilderCallbackReason, _anglr_file_fragment_.production_kind.g__anglr_file_fragment__63, p__anglr_file_fragment__ref);
			return p__anglr_file_fragment__ref;
		}

		//
		// create syntax tree node associated with production
		// <anglr file fragment>: <priority assoc specification optional terminal> <priority assoc specification optional>

		//

		public _anglr_file_fragment_ _anglr_file_fragment__64 (SyntaxTreeToken p_token, _priority_assoc_specification_optional_ p__priority_assoc_specification_optional_)
		{
			_anglr_file_fragment_ p__anglr_file_fragment__ref = new _anglr_file_fragment_(p_token, p__priority_assoc_specification_optional_);
			p_token.parent = p__anglr_file_fragment__ref;
			p__priority_assoc_specification_optional_.parent = p__anglr_file_fragment__ref;
			Raise__anglr_file_fragment__Event (SyntaxTreeCallbackReason.BuilderCallbackReason, _anglr_file_fragment_.production_kind.g__anglr_file_fragment__64, p__anglr_file_fragment__ref);
			return p__anglr_file_fragment__ref;
		}

		//
		// create syntax tree node associated with production
		// <anglr file fragment>: <marker list optional terminal> <marker list optional>

		//

		public _anglr_file_fragment_ _anglr_file_fragment__65 (SyntaxTreeToken p_token, _marker_list_optional_ p__marker_list_optional_)
		{
			_anglr_file_fragment_ p__anglr_file_fragment__ref = new _anglr_file_fragment_(p_token, p__marker_list_optional_);
			p_token.parent = p__anglr_file_fragment__ref;
			p__marker_list_optional_.parent = p__anglr_file_fragment__ref;
			Raise__anglr_file_fragment__Event (SyntaxTreeCallbackReason.BuilderCallbackReason, _anglr_file_fragment_.production_kind.g__anglr_file_fragment__65, p__anglr_file_fragment__ref);
			return p__anglr_file_fragment__ref;
		}

		//
		// create syntax tree node associated with production
		// <anglr file fragment>: <delimiter optional terminal> <delimiter optional>

		//

		public _anglr_file_fragment_ _anglr_file_fragment__66 (SyntaxTreeToken p_token, _delimiter_optional_ p__delimiter_optional_)
		{
			_anglr_file_fragment_ p__anglr_file_fragment__ref = new _anglr_file_fragment_(p_token, p__delimiter_optional_);
			p_token.parent = p__anglr_file_fragment__ref;
			p__delimiter_optional_.parent = p__anglr_file_fragment__ref;
			Raise__anglr_file_fragment__Event (SyntaxTreeCallbackReason.BuilderCallbackReason, _anglr_file_fragment_.production_kind.g__anglr_file_fragment__66, p__anglr_file_fragment__ref);
			return p__anglr_file_fragment__ref;
		}

		//
		// create syntax tree node associated with production
		// <anglr file fragment>: <cstring optional terminal> <cstring optional>

		//

		public _anglr_file_fragment_ _anglr_file_fragment__67 (SyntaxTreeToken p_token, _cstring_optional_ p__cstring_optional_)
		{
			_anglr_file_fragment_ p__anglr_file_fragment__ref = new _anglr_file_fragment_(p_token, p__cstring_optional_);
			p_token.parent = p__anglr_file_fragment__ref;
			p__cstring_optional_.parent = p__anglr_file_fragment__ref;
			Raise__anglr_file_fragment__Event (SyntaxTreeCallbackReason.BuilderCallbackReason, _anglr_file_fragment_.production_kind.g__anglr_file_fragment__67, p__anglr_file_fragment__ref);
			return p__anglr_file_fragment__ref;
		}

		//
		// create syntax tree node associated with production
		// <anglr file fragment>: <number optional terminal> <number optional>

		//

		public _anglr_file_fragment_ _anglr_file_fragment__68 (SyntaxTreeToken p_token, _number_optional_ p__number_optional_)
		{
			_anglr_file_fragment_ p__anglr_file_fragment__ref = new _anglr_file_fragment_(p_token, p__number_optional_);
			p_token.parent = p__anglr_file_fragment__ref;
			p__number_optional_.parent = p__anglr_file_fragment__ref;
			Raise__anglr_file_fragment__Event (SyntaxTreeCallbackReason.BuilderCallbackReason, _anglr_file_fragment_.production_kind.g__anglr_file_fragment__68, p__anglr_file_fragment__ref);
			return p__anglr_file_fragment__ref;
		}
		#endregion
	};
}
