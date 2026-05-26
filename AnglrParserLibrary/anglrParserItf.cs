using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Runtime.InteropServices;

namespace anglrParser_ns
{
	class anglrParserItf
	{
		[DllImport ("AnglrLexerLibrary", EntryPoint = "createAnglrCompilerWrapper", CallingConvention = CallingConvention.Cdecl)]
		public static extern IntPtr createAnglrCompilerWrapper (IntPtr asnCompilerObj);

		[DllImport ("AnglrLexerLibrary", EntryPoint = "init_scanner", CallingConvention = CallingConvention.Cdecl)]
		public static extern int init_scanner (IntPtr p_asnCompilerWrapper, string fileName, uint startTerminal);

		[DllImport ("AnglrLexerLibrary", EntryPoint = "init_string_scanner", CallingConvention = CallingConvention.Cdecl)]
		public static extern int init_string_scanner (IntPtr p_asnCompilerWrapper, string text, uint startTerminal);

		[DllImport ("AnglrLexerLibrary", EntryPoint = "init_string_list_scanner", CallingConvention = CallingConvention.Cdecl)]
		public static extern int init_string_list_scanner (IntPtr p_asnCompilerWrapper, int count, [In, Out] string [] lines, uint startTerminal);

		[DllImport ("AnglrLexerLibrary", EntryPoint = "destroy_scanner", CallingConvention = CallingConvention.Cdecl)]
		public static extern int destroy_scanner (IntPtr p_asnCompilerWrapper);

		[DllImport ("AnglrLexerLibrary", EntryPoint = "get_scanner", CallingConvention = CallingConvention.Cdecl)]
		public static extern IntPtr get_scanner (IntPtr p_asnCompilerWrapper);

		[DllImport ("AnglrLexerLibrary", EntryPoint = "invoke_scanner", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
		//[return: MarshalAs(UnmanagedType.HString)]
		public static extern IntPtr invoke_scanner (IntPtr p_asnCompilerWrapper, ref int terminal, ref int lineno, ref int column);

	}
}
