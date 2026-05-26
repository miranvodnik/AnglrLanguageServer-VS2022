using AnglrDebuggerBridge;
using AnglrDebuggerJsonRpcMessages;
using Newtonsoft.Json.Linq;
using StreamJsonRpc;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Diagnostics;
using System.Diagnostics.SymbolStore;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace anglr_cs
{

    class Program
    {
        static async Task Main (string [] args) => await AnglrCompilerTestProgram.MainTask (args);
    }
}
