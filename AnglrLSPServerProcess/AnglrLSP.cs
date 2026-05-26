using Microsoft.VisualStudio.LanguageServer.Protocol;
using Newtonsoft.Json.Linq;
using StreamJsonRpc;
using System;
using System.Diagnostics;
using System.Collections.Generic;
using System.IO;
using System.IO.Pipes;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using System.ComponentModel;

namespace AnglrLSPServerProcess
{
    public class AnglrLSP
    {
        public AnglrLspLogger Logger { get; private set; }
        public JsonRpc JsonRpcLink { get; private set; }
        private ManualResetEvent disconnectEvent;
        private Stream reader;
        private Stream writer;

        public AnglrLSP (Stream writer, Stream reader)
        {
            this.writer = writer;
            this.reader = reader;
        }

        public void Log (MessageType msgType, string message)
        {
            Logger?.Log (msgType, message);
        }

        private void OnRpcDisconnected (object sender, JsonRpcDisconnectedEventArgs e)
        {
            Log (MessageType.Log, "disconnected");
            disconnectEvent.Set ();
        }

        public void Run ()
        {
            disconnectEvent = new ManualResetEvent (false);

            try
            {
                JsonRpcLink = JsonRpc.Attach (writer, reader, new AnglrLSPTarget (this));
                JsonRpcLink.Disconnected += OnRpcDisconnected;
                Logger = new AnglrLspLogger (JsonRpcLink);
                Log (MessageType.Log, "Anglr LSP server started");
                disconnectEvent.WaitOne ();
                Log (MessageType.Log, "Anglr LSP server stopped");
            }
            catch (Exception e)
            {
                Log (MessageType.Error, e.Message);
            }
        }

        public void Notify (string methodName, object argument)
        {
            JsonRpcLink.NotifyAsync (methodName, argument);
        }
    }
}
