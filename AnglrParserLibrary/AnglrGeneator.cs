using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using Anglr.Parser;

namespace AnglrLibrary
{
	[Serializable]
	public partial class AnglrGenerator : SyntaxTreeWalker
	{
		public AnglrGenerator (_anglr_source_ p__anglr_source_)
		{
			m_anglrFile = p__anglr_source_.m__anglr_file_;

			Common_Event += Invoke_Common_Callback;
			Error_Event += Invoke_Error_Callback;
			_anglr_file_part_list__Event += Invoke__anglr_file_part_list__Callback;
			_general_part__Event += Invoke__general_part__Callback;
			_declaration_part__Event += Invoke__declaration_part__Callback;
			_scanner_part__Event += Invoke__scanner_part__Callback;
			_regular_expression_list__Event += Invoke__regular_expression_list__Callback;
			_parser_part__Event += Invoke__parser_part__Callback;
			_attribute__Event += Invoke__attribute__Callback;
			_name_value_pair__Event += Invoke__name_value_pair__Callback;
			_anglr_definition_list__Event += Invoke__anglr_definition_list__Callback;
			_anglr_definition__Event += Invoke__anglr_definition__Callback;
			_token_string__Event += Invoke__token_string__Callback;
			_token_definition__Event += Invoke__token_definition__Callback;
			_anglr_syntax_rule_list__Event += Invoke__anglr_syntax_rule_list__Callback;
			_anglr_syntax_rule__Event += Invoke__anglr_syntax_rule__Callback;
			_anglr_syntax_production_list_name__Event += Invoke__anglr_syntax_production_list_name__Callback;
			_anglr_syntax_production__Event += Invoke__anglr_syntax_production__Callback;
			_name__Event += Invoke__name__Callback;

			TraverseCommon (m_anglrFile);
			// Console.Error.WriteLine ("Collect symbols");
			Traverse (m_anglrFile);
		}

		_anglr_file_ m_anglrFile;
		SymbolTable m_symtab = new SymbolTable ();
	}
}
