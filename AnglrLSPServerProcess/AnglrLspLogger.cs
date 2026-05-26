using AnglrJsonRpcMethods;
using AnglrLogLibrary;
using Microsoft.VisualStudio.LanguageServer.Protocol;
using StreamJsonRpc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AnglrLSPServerProcess
{
    public class AnglrLspLogger : AnglrLoggerBase
    {
        public JsonRpc JsonRpcLink { get; set; } = null;
        public AnglrLspLogger (JsonRpc JsonRpcLink)
        {
            this.JsonRpcLink = JsonRpcLink;
        }
        public void Log (MessageType msgType, string message, uint flags = 0)
        {
            AnglrLogLevel level = AnglrLogLevel.None;
            switch (msgType)
            {
                case MessageType.Error:
                    level = AnglrLogLevel.Error;
                    break;
                case MessageType.Warning:
                    level = AnglrLogLevel.Warn;
                    break;
                case MessageType.Info:
                    level = AnglrLogLevel.Info;
                    break;
                case MessageType.Log:
                    level = AnglrLogLevel.Debug;
                    break;
            }
            base.Log (level, message, flags);
        }
        protected override void Write (AnglrLogLevel level, string message, uint flags = 0)
        {
            if (JsonRpcLink == null)
                return;
            AnglrLspLogMessageNotification logMessageParams = new AnglrLspLogMessageNotification ()
            {
                LogLevel = (int) level,
                Message = message,
                Flags = flags
            };
            _ = JsonRpcLink.NotifyAsync (AnglrMethods.AnglrLspLogMessageName, logMessageParams);
        }
        protected override void WriteLine (AnglrLogLevel level, string message, uint flags = 0) => Write (level, message, flags);
    }
}
