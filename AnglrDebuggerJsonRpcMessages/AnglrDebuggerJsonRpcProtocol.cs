using AnglrLogLibrary;
using Newtonsoft.Json.Linq;
using StreamJsonRpc;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Runtime.Serialization;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace AnglrDebuggerJsonRpcMessages
{
    [Obfuscation (Exclude = true)]
    [DataContract]
    public class AnglrDebuggerLogRequest : EventArgs
    {
        [DataMember (Name = "sequenceNr")] public int SequenceNr { get; set; }
        [DataMember (Name = "logLevel")] public int LogLevel { get; set; }
        [DataMember (Name = "message")] public string Message{ get; set; }
    }

    [Obfuscation (Exclude = true)]
    [DataContract]
    public class AnglrDebuggerLogResponse : EventArgs
    {
        [DataMember (Name = "sequenceNr")] public int SequenceNr { get; set; }
    }

    [Obfuscation (Exclude = true)]
    [DataContract]
    public class AnglrDebuggerConnectRequest : EventArgs
    {
        [DataMember (Name = "sequenceNr")] public int SequenceNr { get; set; }
        [DataMember (Name = "stackNr")] public int StackNr { get; set; }
        [DataMember (Name = "magicNumber")] public int? MagicNumber { get; set; }
        [DataMember (Name = "info")] public object [] Info { get; set; }
    }

    [Obfuscation (Exclude = true)]
    [DataContract]
    public class AnglrDebuggerConnectResponse : EventArgs
    {
        [DataMember (Name = "sequenceNr")] public int SequenceNr { get; set; }
        [DataMember (Name = "valid")] public bool Valid { get; set; }
        [DataMember (Name = "breakPointDB")] public string BreakPointDB { get; set; }
    }

    [Obfuscation (Exclude = true)]
    [DataContract]
    public class AnglrDebuggerSyntaxErrorRequest : EventArgs
    {
        [DataMember (Name = "sequenceNr")] public int SequenceNr { get; set; }
        [DataMember (Name = "stackNr")] public int StackNr { get; set; }
        [DataMember (Name = "state")] public int State { get; set; }
    }

    [Obfuscation (Exclude = true)]
    [DataContract]
    public class AnglrDebuggerSyntaxErrorResponse : EventArgs
    {
        [DataMember (Name = "sequenceNr")] public int SequenceNr { get; set; }
    }

    [Obfuscation (Exclude = true)]
    [DataContract]
    public class AnglrDebuggerShiftStepRequest : EventArgs
    {
        [DataMember (Name = "sequenceNr")] public int SequenceNr { get; set; }
        [DataMember (Name = "stackNr")] public int StackNr { get; set; }
        [DataMember (Name = "state")] public int State { get; set; }
        [DataMember (Name = "tokenValue")] public int TokenValue { get; set; }
        [DataMember (Name = "tokenName")] public string TokenName { get; set; }
        [DataMember (Name = "tokenText")] public string TokenText { get; set; }
        [DataMember (Name = "conflict")] public bool Conflict { get; set; }
    }

    [Obfuscation (Exclude = true)]
    [DataContract]
    public class AnglrDebuggerShiftStepResponse : EventArgs
    {
        [DataMember (Name = "sequenceNr")] public int SequenceNr { get; set; }
    }

    [Obfuscation (Exclude = true)]
    [DataContract]
    public class AnglrDebuggerReduceStepRequest : EventArgs
    {
        [DataMember (Name = "sequenceNr")] public int SequenceNr { get; set; }
        [DataMember (Name = "stackNr")] public int StackNr { get; set; }
        [DataMember (Name = "prodNr")] public int ProdNr { get; set; }
        [DataMember (Name = "ruleNr")] public int RuleNr { get; set; }
        [DataMember (Name = "ruleName")] public string RuleName { get; set; }
        [DataMember (Name = "prodLen")] public int ProdLen { get; set; }
        [DataMember (Name = "fallingState")] public int FallingState { get; set; }
        [DataMember (Name = "bottomState")] public int BottomState { get; set; }
        [DataMember (Name = "risingState")] public int RisingState { get; set; }
        [DataMember (Name = "conflict")] public bool Conflict { get; set; }
    }

    [Obfuscation (Exclude = true)]
    [DataContract]
    public class AnglrDebuggerReduceStepResponse : EventArgs
    {
        [DataMember (Name = "sequenceNr")] public int SequenceNr { get; set; }
    }

    [Obfuscation (Exclude = true)]
    [DataContract]
    public class AnglrDebuggerSplitStepRequest : EventArgs
    {
        [DataMember (Name = "sequenceNr")] public int SequenceNr { get; set; }
        [DataMember (Name = "oldStackNr")] public int OldStackNr { get; set; }
        [DataMember (Name = "newStackNr")] public int NewStackNr { get; set; }
        [DataMember (Name = "begin")] public bool Begin { get; set; }
    }

    [Obfuscation (Exclude = true)]
    [DataContract]
    public class AnglrDebuggerSplitStepResponse : EventArgs
    {
        [DataMember (Name = "sequenceNr")] public int SequenceNr { get; set; }
    }

    [Obfuscation (Exclude = true)]
    [DataContract]
    public class AnglrDebuggerLoopStepRequest : EventArgs
    {
        [DataMember (Name = "sequenceNr")] public int SequenceNr { get; set; }
        [DataMember (Name = "stackNr")] public int StackNr { get; set; }
        [DataMember (Name = "state")] public int State { get; set; }
    }

    [Obfuscation (Exclude = true)]
    [DataContract]
    public class AnglrDebuggerLoopStepResponse : EventArgs
    {
        [DataMember (Name = "sequenceNr")] public int SequenceNr { get; set; }
    }

    [Obfuscation (Exclude = true)]
    [DataContract]
    public class AnglrDebuggerJoinRequest : EventArgs
    {
        [DataMember (Name = "sequenceNr")] public int SequenceNr { get; set; }
        [DataMember (Name = "stackNr")] public int StackNr { get; set; }
        [DataMember (Name = "joinNr")] public int JoinNr { get; set; }
    }

    [Obfuscation (Exclude = true)]
    [DataContract]
    public class AnglrDebuggerJoinResponse : EventArgs
    {
        [DataMember (Name = "sequenceNr")] public int SequenceNr { get; set; }
    }

    [Obfuscation (Exclude = true)]
    [DataContract]
    public class AnglrDebuggerFinalStepRequest : EventArgs
    {
        [DataMember (Name = "sequenceNr")] public int SequenceNr { get; set; }
        [DataMember (Name = "stackNr")] public int StackNr { get; set; }
    }

    [Obfuscation (Exclude = true)]
    [DataContract]
    public class AnglrDebuggerFinalStepResponse : EventArgs
    {
        [DataMember (Name = "sequenceNr")] public int SequenceNr { get; set; }
    }

    [Obfuscation (Exclude = true)]
    [DataContract]
    public class AnglrDebuggerStopParserRequest : EventArgs
    {
        [DataMember (Name = "sequenceNr")] public int SequenceNr { get; set; }
    }

    [Obfuscation (Exclude = true)]
    [DataContract]
    public class AnglrDebuggerStopParserResponse : EventArgs
    {
        [DataMember (Name = "sequenceNr")] public int SequenceNr { get; set; }
    }

    [Obfuscation (Exclude = true)]
    [DataContract]
    public class AnglrDebuggerDbgSingleStepRequest : EventArgs
    {
        [DataMember (Name = "sequenceNr")] public int SequenceNr { get; set; }
        [DataMember (Name = "breakPointDB")] public string BreakPointDB { get; set; }
    }

    [Obfuscation (Exclude = true)]
    [DataContract]
    public class AnglrDebuggerDbgSingleStepResponse : EventArgs
    {
        [DataMember (Name = "sequenceNr")] public int SequenceNr { get; set; }
    }

    [Obfuscation (Exclude = true)]
    [DataContract]
    public class AnglrDebuggerDbgContinueRequest : EventArgs
    {
        [DataMember (Name = "sequenceNr")] public int SequenceNr { get; set; }
        [DataMember (Name = "breakPointDB")] public string BreakPointDB { get; set; }
    }

    [Obfuscation (Exclude = true)]
    [DataContract]
    public class AnglrDebuggerDbgContinueResponse : EventArgs
    {
        [DataMember (Name = "sequenceNr")] public int SequenceNr { get; set; }
    }

    [Obfuscation (Exclude = true)]
    [DataContract]
    public class AnglrDebuggerDbgBreakRequest : EventArgs
    {
        [DataMember (Name = "sequenceNr")] public int SequenceNr { get; set; }
    }

    [Obfuscation (Exclude = true)]
    [DataContract]
    public class AnglrDebuggerDbgBreakResponse : EventArgs
    {
        [DataMember (Name = "sequenceNr")] public int SequenceNr { get; set; }
    }

    [Obfuscation (Exclude = true)]
    [DataContract]
    public class AnglrDebuggerDbgAddBreakPointRequest : EventArgs
    {
        [DataMember (Name = "sequenceNr")] public int SequenceNr { get; set; }
    }

    [Obfuscation (Exclude = true)]
    [DataContract]
    public class AnglrDebuggerDbgAddBreakPointResponse : EventArgs
    {
        [DataMember (Name = "sequenceNr")] public int SequenceNr { get; set; }
    }

    [Obfuscation (Exclude = true)]
    [DataContract]
    public class AnglrDebuggerDbgDeleteBreakPointRequest : EventArgs
    {
        [DataMember (Name = "sequenceNr")] public int SequenceNr { get; set; }
    }

    [Obfuscation (Exclude = true)]
    [DataContract]
    public class AnglrDebuggerDbgDeleteBreakPointResponse : EventArgs
    {
        [DataMember (Name = "sequenceNr")] public int SequenceNr { get; set; }
    }

    [Obfuscation (Exclude = true)]
    public static class AnglrDebuggerJsonRpcMessageNames
    {
        public const string LogMessageName = "LogMessage";
        public const string ConnectMessageName = "ConnectMessage";
        public const string SyntaxErrorMessageName = "SyntaxErrorMessage";
        public const string ShiftStepMessageName = "ShiftStepMessage";
        public const string ReduceStepMessageName = "ReduceStepMessage";
        public const string SplitStepMessageName = "SplitStepMessage";
        public const string LoopStepMessageName = "LoopStepMessage";
        public const string JoinMessageName = "JoinMessage";
        public const string FinalStepMessageName = "FinalStepMessage";
        public const string StopParserMessageName = "StopParserMessage";

        public const string DbgSingleStepMessageName = "DbgSingleStepMessage";
        public const string DbgContinueMessageName = "DbgContinueMessage";
        public const string DbgBreakMessageName = "DbgBreakMessage";
        public const string DbgAddBreakPointMessageName = "DbgAddBreakPointMessage";
        public const string DbgDeleteBreakPointMessageName = "DbgDeleteBreakPointMessage";
    }

    [Obfuscation (Exclude = true)]
    public interface IAnglrClientSideDebugger
    {
        JsonRpc Rpc { get; set; }
        void DbgSingleStepMessageHandler (object sender, EventArgs e);
        void DbgContinueMessageHandler (object sender, EventArgs e);
        void DbgBreakMessageHandler (object sender, EventArgs e);
        void DbgAddBreakPointMessageHandler (object sender, EventArgs e);
        void DbgDeleteBreakPointMessageHandler (object sender, EventArgs e);
    }

    [Obfuscation (Exclude = true)]
    public interface IAnglrServerSideDebuggerInvoker
    {
        void InvokeRpcSession (int counter, Stream pipe, CancellationToken token);
        IAnglrLogger Logger { get; }
    }

    [Obfuscation (Exclude = true)]
    public interface IAnglrServerSideDebugger
    {
        JsonRpc Rpc { get; set; }
        AnglrDebuggerLogResponse LogMessageHandler (object sender, EventArgs e);
        AnglrDebuggerConnectResponse ConnectMessageHandler (object sender, EventArgs e);
        AnglrDebuggerSyntaxErrorResponse SyntaxErrorMessageHandler (object sender, EventArgs e);
        AnglrDebuggerShiftStepResponse ShiftStepMessageHandler (object sender, EventArgs e);
        AnglrDebuggerReduceStepResponse ReduceStepMessageHandler (object sender, EventArgs e);
        AnglrDebuggerSplitStepResponse SplitStepMessageHandler (object sender, EventArgs e);
        AnglrDebuggerLoopStepResponse LoopStepMessageHandler (object sender, EventArgs e);
        AnglrDebuggerJoinResponse JoinMessageHandler (object sender, EventArgs e);
        AnglrDebuggerFinalStepResponse FinalStepMessageHandler (object sender, EventArgs e);
        AnglrDebuggerStopParserResponse StopParserMessageHandler (object sender, EventArgs e);
        IAnglrLogger Logger { get; }
    }

    [Obfuscation (Exclude = true)]
    public class AnglrClientSideDebuggerJsonRpcMessagesHandler
    {
        public IAnglrClientSideDebugger Debugger { get; private set; }

        public AnglrClientSideDebuggerJsonRpcMessagesHandler (IAnglrClientSideDebugger anglrDebugger)
        {
            Debugger = anglrDebugger;
        }

        [JsonRpcMethod (AnglrDebuggerJsonRpcMessageNames.DbgSingleStepMessageName)]
        public void HandleDbgSingleStepMessage (JToken dbgSingleStepMessage)
        {
            AnglrDebuggerDbgSingleStepRequest dbgSingleStepMessageRequest = dbgSingleStepMessage.ToObject<AnglrDebuggerDbgSingleStepRequest> ();
            _ = Task.Run (() => Debugger?.DbgSingleStepMessageHandler (this, dbgSingleStepMessageRequest));
        }

        [JsonRpcMethod (AnglrDebuggerJsonRpcMessageNames.DbgBreakMessageName)]
        public void HandleDbgBreakMessage (JToken dbgBreakMessage)
        {
            AnglrDebuggerDbgBreakRequest dbgBreakMessageRequest = dbgBreakMessage.ToObject<AnglrDebuggerDbgBreakRequest> ();
            _ = Task.Run (() => Debugger?.DbgBreakMessageHandler (this, dbgBreakMessageRequest));
        }

        [JsonRpcMethod (AnglrDebuggerJsonRpcMessageNames.DbgContinueMessageName)]
        public void HandleDbgContinueMessage (JToken dbgContinueMessage)
        {
            AnglrDebuggerDbgContinueRequest dbgContinueMessageRequest = dbgContinueMessage.ToObject<AnglrDebuggerDbgContinueRequest> ();
            _ = Task.Run (() => Debugger?.DbgContinueMessageHandler (this, dbgContinueMessageRequest));
        }

        [JsonRpcMethod (AnglrDebuggerJsonRpcMessageNames.DbgAddBreakPointMessageName)]
        public void HandleDbgAddBreakPointMessage (JToken dbgAddBreakPointMessage)
        {
            AnglrDebuggerDbgAddBreakPointRequest dbgAddBreakPointMessageRequest = dbgAddBreakPointMessage.ToObject<AnglrDebuggerDbgAddBreakPointRequest> ();
            _ = Task.Run (() => Debugger?.DbgAddBreakPointMessageHandler (this, dbgAddBreakPointMessageRequest));
        }

        [JsonRpcMethod (AnglrDebuggerJsonRpcMessageNames.DbgDeleteBreakPointMessageName)]
        public void HandleDbgDeleteBreakPointMessage (JToken dbgDeleteBreakPointMessage)
        {
            AnglrDebuggerDbgDeleteBreakPointRequest dbgDeleteBreakPointMessageRequest = dbgDeleteBreakPointMessage.ToObject<AnglrDebuggerDbgDeleteBreakPointRequest> ();
            _ = Task.Run (() => Debugger?.DbgDeleteBreakPointMessageHandler (this, dbgDeleteBreakPointMessageRequest));
        }
    }

    [Obfuscation (Exclude = true)]
    public class AnglrServerSideDebuggerJsonRpcMessagesHandler
    {
        public IAnglrServerSideDebugger Debugger { get; private set; }

        public AnglrServerSideDebuggerJsonRpcMessagesHandler (IAnglrServerSideDebugger anglrDebugger)
        {
            Debugger = anglrDebugger;
        }

        [JsonRpcMethod (AnglrDebuggerJsonRpcMessageNames.LogMessageName)]
        public AnglrDebuggerLogResponse HandleLogMessage (JToken logMessage) =>
            Debugger?.LogMessageHandler (this, logMessage.ToObject<AnglrDebuggerLogRequest> ());

        [JsonRpcMethod (AnglrDebuggerJsonRpcMessageNames.ConnectMessageName)]
        public AnglrDebuggerConnectResponse HandleConnectMessage (JToken connectMessage) =>
            Debugger?.ConnectMessageHandler (this, connectMessage.ToObject<AnglrDebuggerConnectRequest> ());

        [JsonRpcMethod (AnglrDebuggerJsonRpcMessageNames.SyntaxErrorMessageName)]
        public AnglrDebuggerSyntaxErrorResponse HandleSyntaxErrorMessage (JToken syntaxErrorMessage)=>
            Debugger?.SyntaxErrorMessageHandler (this, syntaxErrorMessage.ToObject<AnglrDebuggerSyntaxErrorRequest> ());

        [JsonRpcMethod (AnglrDebuggerJsonRpcMessageNames.ShiftStepMessageName)]
        public AnglrDebuggerShiftStepResponse HandleShiftStepMessage (JToken shiftStepMessage)=>
            Debugger?.ShiftStepMessageHandler (this, shiftStepMessage.ToObject<AnglrDebuggerShiftStepRequest> ());

        [JsonRpcMethod (AnglrDebuggerJsonRpcMessageNames.ReduceStepMessageName)]
        public AnglrDebuggerReduceStepResponse HandleReduceStepMessage (JToken reduceStepMessage)=>
            Debugger?.ReduceStepMessageHandler (this, reduceStepMessage.ToObject<AnglrDebuggerReduceStepRequest> ());

        [JsonRpcMethod (AnglrDebuggerJsonRpcMessageNames.SplitStepMessageName)]
        public AnglrDebuggerSplitStepResponse HandleSplitStepMessage (JToken splitStepMessage) =>
            Debugger?.SplitStepMessageHandler (this, splitStepMessage.ToObject<AnglrDebuggerSplitStepRequest> ());

        [JsonRpcMethod (AnglrDebuggerJsonRpcMessageNames.LoopStepMessageName)]
        public AnglrDebuggerLoopStepResponse HandleLoopStepMessage (JToken loopStepMessage)=>
            Debugger?.LoopStepMessageHandler (this, loopStepMessage.ToObject<AnglrDebuggerLoopStepRequest> ());

        [JsonRpcMethod (AnglrDebuggerJsonRpcMessageNames.JoinMessageName)]
        public AnglrDebuggerJoinResponse HandleJoinMessage (JToken joinMessage)=>
            Debugger?.JoinMessageHandler (this, joinMessage.ToObject<AnglrDebuggerJoinRequest> ());

        [JsonRpcMethod (AnglrDebuggerJsonRpcMessageNames.FinalStepMessageName)]
        public AnglrDebuggerFinalStepResponse HandleFinalStepMessage (JToken finalStepMessage) =>
            Debugger?.FinalStepMessageHandler (this, finalStepMessage.ToObject<AnglrDebuggerFinalStepRequest> ());

        [JsonRpcMethod (AnglrDebuggerJsonRpcMessageNames.StopParserMessageName)]
        public AnglrDebuggerStopParserResponse HandleStopParserMessage (JToken stopParserMessage)=>
            Debugger?.StopParserMessageHandler (this, stopParserMessage.ToObject<AnglrDebuggerStopParserRequest> ());
    }
}
