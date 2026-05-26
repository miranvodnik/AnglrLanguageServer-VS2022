using System;
using System.Diagnostics;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.IO.Pipes;
using System.Threading;
using System.Threading.Tasks;
using Anglr.Compiler;
using Anglr.Parser;
using Anglr.Declarations;

namespace AnglrLSPServerProcess
{
	class Program
	{
		static void Main (string[] args)
		{
			Process currentProcess = Process.GetCurrentProcess();

			try
			{
				string anglrPipeName = $"anglr-lsp-pipe-{currentProcess.Id}";
				NamedPipeClientStream anglrPipe  = new NamedPipeClientStream (".", anglrPipeName, PipeDirection.InOut, PipeOptions.Asynchronous, System.Security.Principal.TokenImpersonationLevel.Impersonation);

				anglrPipe.Connect ();

				AnglrLSP anglrLSP = new AnglrLSP (anglrPipe, anglrPipe);

				anglrLSP.Run ();

			}
			catch (Exception e)
			{
			}
		}
	}
}
