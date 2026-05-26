using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Runtime.InteropServices;

namespace AnglrWin32Imports
{
	public static class AnglrWin32Imports
	{
		[DllImport ("AnglrWin32Exports", CallingConvention = CallingConvention.Cdecl)]
		public static extern int GetParentProcessId (int processId);
		[DllImport ("AnglrWin32Exports", CallingConvention = CallingConvention.Cdecl)]
		public static extern int CreateRegexClass (string[] regexTextArray, int regexTextArraySize);
	}
}
