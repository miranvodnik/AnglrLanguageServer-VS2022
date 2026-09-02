using Anglr.Compiler;
using Anglr.Declarations;
using Anglr.Parser;
using Anglr.Parser.SyntaxTree;
using AnglrLibrary;
using AnglrLogLibrary;
using System;
using System.Collections.Generic;
using System.IO;

namespace AnglrHtmlColorizerLibrary
{
    public class AnglrHtmlColorizerProgram
    {
        static Dictionary<string, int> fragmentTypes = new Dictionary<string, int>
        {
            { "anglr-file", AnglrDeclarations.tokens._anglr_file_terminal_ },
            { "anglr-file-part-list", AnglrDeclarations.tokens._anglr_file_part_list_terminal_ },
            { "anglr-file-part", AnglrDeclarations.tokens._anglr_file_part_terminal_ },
            { "general-part", AnglrDeclarations.tokens._general_part_terminal_ },
            { "declaration-part", AnglrDeclarations.tokens._declaration_part_terminal_ },
            { "scanner-part", AnglrDeclarations.tokens._scanner_part_terminal_ },
            { "regular-expression-list", AnglrDeclarations.tokens._regular_expression_list_terminal_ },
            { "regular-expression-usage", AnglrDeclarations.tokens._regular_expression_usage_terminal_ },
            { "actions", AnglrDeclarations.tokens._actions_terminal_ },
            { "action", AnglrDeclarations.tokens._action_terminal_ },
            { "skip-action", AnglrDeclarations.tokens._skip_action_terminal_ },
            { "terminal-action", AnglrDeclarations.tokens._terminal_action_terminal_ },
            { "event-action", AnglrDeclarations.tokens._event_action_terminal_ },
            { "push-action", AnglrDeclarations.tokens._push_action_terminal_ },
            { "pop-action", AnglrDeclarations.tokens._pop_action_terminal_ },
            { "lexer-part", AnglrDeclarations.tokens._lexer_part_terminal_ },
            { "parser-part", AnglrDeclarations.tokens._parser_part_terminal_ },
            { "attribute-list", AnglrDeclarations.tokens._attribute_list_terminal_ },
            { "attribute", AnglrDeclarations.tokens._attribute_terminal_ },
            { "name-value-list", AnglrDeclarations.tokens._name_value_list_terminal_ },
            { "name-value-pair", AnglrDeclarations.tokens._name_value_pair_terminal_ },
            { "anglr-definition-list", AnglrDeclarations.tokens._anglr_definition_list_terminal_ },
            { "anglr-definition-with-attribute-list", AnglrDeclarations.tokens._anglr_definition_with_attribute_list_terminal_ },
            { "anglr-definition", AnglrDeclarations.tokens._anglr_definition_terminal_ },
            { "single-definition", AnglrDeclarations.tokens._single_terminal_definition_terminal_ },
            { "single-regex-definition", AnglrDeclarations.tokens._single_regex_definition_terminal_ },
            { "block-of-definitions", AnglrDeclarations.tokens._block_of_terminal_definitions_terminal_ },
            { "block-of-regex-definitions", AnglrDeclarations.tokens._block_of_regex_definitions_terminal_ },
            { "terminal-definition", AnglrDeclarations.tokens._terminal_definition_terminal_ },
            { "regex-definition", AnglrDeclarations.tokens._regex_definition_terminal_ },
            { "block-definitions", AnglrDeclarations.tokens._block_terminal_definitions_terminal_ },
            { "block-definition", AnglrDeclarations.tokens._block_terminal_definition_terminal_ },
            { "block-regex-definitions", AnglrDeclarations.tokens._block_regex_definitions_terminal_ },
            { "block-regex-definition", AnglrDeclarations.tokens._block_regex_definition_terminal_ },
            { "anglr-syntax-rule-list", AnglrDeclarations.tokens._anglr_syntax_rule_list_terminal_ },
            { "anglr-syntax-rule", AnglrDeclarations.tokens._anglr_syntax_rule_terminal_ },
            { "anglr-syntax-production-list", AnglrDeclarations.tokens._anglr_syntax_production_list_terminal_ },
            { "anglr-syntax-production", AnglrDeclarations.tokens._anglr_syntax_production_terminal_ },
            { "priority-assoc-specification", AnglrDeclarations.tokens._priority_assoc_specification_terminal_ },
            { "priority-specification", AnglrDeclarations.tokens._priority_specification_terminal_ },
            { "associativity-specification", AnglrDeclarations.tokens._associativity_specification_terminal_ },
            { "anglr-nested-rule", AnglrDeclarations.tokens._anglr_nested_rule_terminal_ },
            { "anglr-syntax-production-list-name", AnglrDeclarations.tokens._anglr_syntax_production_list_name_terminal_ },
            { "name-list", AnglrDeclarations.tokens._name_list_terminal_ },
            { "production-name", AnglrDeclarations.tokens._production_name_terminal_ },
            { "marker-list", AnglrDeclarations.tokens._marker_list_terminal_ },
            { "marker", AnglrDeclarations.tokens._marker_terminal_ },
            { "g-name", AnglrDeclarations.tokens._g_name_terminal_ },
            { "name", AnglrDeclarations.tokens._name_terminal_ },
            { "cardinality-delimiter", AnglrDeclarations.tokens._cardinality_delimiter_terminal_ },
            { "cardinality", AnglrDeclarations.tokens._cardinality_terminal_ },
            { "delimiter", AnglrDeclarations.tokens._delimiter_terminal_ },
            { "attribute-list-optional", AnglrDeclarations.tokens._attribute_list_optional_terminal_ },
            { "name-value-list-optional", AnglrDeclarations.tokens._name_value_list_optional_terminal_ },
            { "anglr-file-part-list-optional-terminal", AnglrDeclarations.tokens._anglr_file_part_list_optional_terminal_ },
            { "anglr-definition-list-optional", AnglrDeclarations.tokens._anglr_definition_list_optional_terminal_ },
            { "block-terminal-definitions-optional", AnglrDeclarations.tokens._block_terminal_definitions_optional_terminal_ },
            { "block-regex-definitions-optional", AnglrDeclarations.tokens._block_regex_definitions_optional_terminal_ },
            { "regular-expression-list-optional", AnglrDeclarations.tokens._regular_expression_list_optional_terminal_ },
            { "actions-optional", AnglrDeclarations.tokens._actions_optional_terminal_ },
            { "anglr-syntax-rule-list-optional", AnglrDeclarations.tokens._anglr_syntax_rule_list_optional_terminal_ },
            { "anglr-syntax-production-list-name-optional", AnglrDeclarations.tokens._anglr_syntax_production_list_name_optional_terminal_ },
            { "production-name-optional", AnglrDeclarations.tokens._production_name_optional_terminal_ },
            { "priority-assoc-specification-optional", AnglrDeclarations.tokens._priority_assoc_specification_optional_terminal_ },
            { "marker-list-optional", AnglrDeclarations.tokens._marker_list_optional_terminal_ },
            { "delimiter-optional", AnglrDeclarations.tokens._delimiter_optional_terminal_ },
            { "cstring-optional", AnglrDeclarations.tokens._cstring_optional_terminal_ },
            { "number-optional", AnglrDeclarations.tokens._number_optional_terminal_ }
        };

        public static void MainTask (string [] args)
        {
            IAnglrLogger Logger = new ConsoleAnglrLogger ();

            if (args.Length < 1)
            {
                Logger.ErrorLine ("usage: colorizer ( [-t <fragment type>] <file>.anglr ) ...");
                return;
            }

            anglrCompiler.loopDetection = true;
            anglrCompiler.createParseTree = true;

            int fragmentType = AnglrDeclarations.tokens._anglr_file_terminal_;
            bool changeFragmentType = false;

            foreach (string arg in args)
            {
                if (arg == "-t")
                {
                    changeFragmentType = true;
                    continue;
                }
                if (changeFragmentType)
                {
                    try
                    {
                        fragmentType = fragmentTypes [arg];
                    }
                    catch (Exception)
                    {
                        fragmentType = AnglrDeclarations.tokens._anglr_file_terminal_;
                        Logger.ErrorLine ($"ERROR: Invalid fragment type {arg}");
                    }
                    changeFragmentType = false;
                    continue;
                }
                anglrCompiler compiler = new anglrCompiler (null, Logger);
                if (compiler.Parse (arg, (uint) fragmentType, new object [] { Path.GetFileNameWithoutExtension (arg), "File Name", arg }) != 0)
                {
                    fragmentType = AnglrDeclarations.tokens._anglr_file_terminal_;
                    continue;
                }
                AnglrHtmlColorizer anglrHtmlColorizer = new AnglrHtmlColorizer ();
                foreach (SyntaxTreeBase node in compiler.parseList)
                {
                    anglrHtmlColorizer.Traverse ((_anglr_file_fragment_) node);    // 1st step
                    anglrHtmlColorizer.Traverse ((_anglr_file_fragment_) node);    // 2nd step
                    anglrHtmlColorizer.TraverseCommon ((_anglr_file_fragment_) node);
                }
                fragmentType = AnglrDeclarations.tokens._anglr_file_terminal_;
            }
        }
    }
}
