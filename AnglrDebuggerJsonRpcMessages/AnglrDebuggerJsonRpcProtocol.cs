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
    public class AnglrDebuggerBaseMessage : EventArgs
    {
        [DataMember (Name = "sequenceNr")] public int SequenceNr { get; set; }
    }

    [Obfuscation (Exclude = true)]
    [DataContract]
    public class AnglrDebuggerLogRequest : AnglrDebuggerBaseMessage
    {
        [DataMember (Name = "logLevel")] public int LogLevel { get; set; }
        [DataMember (Name = "message")] public string Message{ get; set; }
    }

    [Obfuscation (Exclude = true)]
    [DataContract]
    public class AnglrDebuggerConnectRequest : AnglrDebuggerBaseMessage
    {
        [DataMember (Name = "stackNr")] public int StackNr { get; set; }
        [DataMember (Name = "magicNumber")] public int? MagicNumber { get; set; }
        [DataMember (Name = "info")] public object [] Info { get; set; }
    }

    [Obfuscation (Exclude = true)]
    [DataContract]
    public class AnglrDebuggerConnectResponse : AnglrDebuggerBaseMessage
    {
        [DataMember (Name = "valid")] public bool Valid { get; set; }
        [DataMember (Name = "breakPointDB")] public string BreakPointDB { get; set; }
    }

    [Obfuscation (Exclude = true)]
    [DataContract]
    public class AnglrDebuggerSyntaxErrorRequest : AnglrDebuggerBaseMessage
    {
        [DataMember (Name = "stackNr")] public int StackNr { get; set; }
        [DataMember (Name = "state")] public int State { get; set; }
    }

    [Obfuscation (Exclude = true)]
    [DataContract]
    public class AnglrDebuggerShiftStepRequest : AnglrDebuggerBaseMessage
    {
        [DataMember (Name = "stackNr")] public int StackNr { get; set; }
        [DataMember (Name = "state")] public int State { get; set; }
        [DataMember (Name = "tokenValue")] public int TokenValue { get; set; }
        [DataMember (Name = "tokenName")] public string TokenName { get; set; }
        [DataMember (Name = "tokenText")] public string TokenText { get; set; }
        [DataMember (Name = "conflict")] public bool Conflict { get; set; }
    }

    [Obfuscation (Exclude = true)]
    [DataContract]
    public class AnglrDebuggerReduceStepRequest : AnglrDebuggerBaseMessage
    {
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
    public class AnglrDebuggerSplitStepRequest : AnglrDebuggerBaseMessage
    {
        [DataMember (Name = "oldStackNr")] public int OldStackNr { get; set; }
        [DataMember (Name = "newStackNr")] public int NewStackNr { get; set; }
        [DataMember (Name = "begin")] public bool Begin { get; set; }
    }

    [Obfuscation (Exclude = true)]
    [DataContract]
    public class AnglrDebuggerLoopStepRequest : AnglrDebuggerBaseMessage
    {
        [DataMember (Name = "stackNr")] public int StackNr { get; set; }
        [DataMember (Name = "state")] public int State { get; set; }
    }

    [Obfuscation (Exclude = true)]
    [DataContract]
    public class AnglrDebuggerJoinRequest : AnglrDebuggerBaseMessage
    {
        [DataMember (Name = "stackNr")] public int StackNr { get; set; }
        [DataMember (Name = "joinNr")] public int JoinNr { get; set; }
    }

    [Obfuscation (Exclude = true)]
    [DataContract]
    public class AnglrDebuggerFinalStepRequest : AnglrDebuggerBaseMessage
    {
        [DataMember (Name = "stackNr")] public int StackNr { get; set; }
    }

    [Obfuscation (Exclude = true)]
    [DataContract]
    public class AnglrDebuggerStopParserRequest : AnglrDebuggerBaseMessage
    {
    }

    [Obfuscation (Exclude = true)]
    [DataContract]
    public class AnglrDebuggerDbgSingleStepRequest : AnglrDebuggerBaseMessage
    {
        [DataMember (Name = "breakPointDB")] public string BreakPointDB { get; set; }
    }

    [Obfuscation (Exclude = true)]
    [DataContract]
    public class AnglrDebuggerDbgContinueRequest : AnglrDebuggerBaseMessage
    {
        [DataMember (Name = "breakPointDB")] public string BreakPointDB { get; set; }
    }

    [Obfuscation (Exclude = true)]
    [DataContract]
    public class AnglrDebuggerDbgBreakRequest : AnglrDebuggerBaseMessage
    {
    }

    [Obfuscation (Exclude = true)]
    [DataContract]
    public class AnglrDebuggerDbgBreakPointHitRequest : AnglrDebuggerBaseMessage
    {
    }

    [Obfuscation (Exclude = true)]
    [DataContract]
    public class AnglrDebuggerDbgAddBreakPointRequest : AnglrDebuggerBaseMessage
    {
    }

    [Obfuscation (Exclude = true)]
    [DataContract]
    public class AnglrDebuggerDbgDeleteBreakPointRequest : AnglrDebuggerBaseMessage
    {
    }

    [Obfuscation (Exclude = true)]
    [DataContract]
    public class AnglrDebuggerGetPDASnapshotRequest : AnglrDebuggerBaseMessage
    {
    }

    [Obfuscation (Exclude = true)]
    [DataContract]
    public class AnglrDebuggerGetPDASnapshotResponse : AnglrDebuggerBaseMessage
    {
        [DataMember (Name ="pdaStackSet")] public AnglrDebuggerGetPDAStack [] PDAStackSet { get; set; }
    }

    [Obfuscation (Exclude = true)]
    [DataContract]
    public class AnglrDebuggerGetPDAStack
    {
        [DataMember (Name = "pdaStackId")] public int PDAStackId { get; set; }
        [DataMember (Name = "pdaStack")] public AnglrDebuggerGetPDAStackCell [] PDAStackCells { get; set; }
    }

    [Obfuscation (Exclude = true)]
    [DataContract]
    public class AnglrDebuggerGetPDAStackCell
    {
        [DataMember (Name = "isTerminal")] public bool IsTerminal { get; set; }
        [DataMember (Name = "id")] public int Id { get; set; }
        [DataMember (Name = "state")] public int State { get; set; }
        [DataMember (Name = "name")] public string Name { get; set; }
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
        public const string DbgBreakPointHitMessageName = "DbgBreakPointHitMessage";
        public const string DbgAddBreakPointMessageName = "DbgAddBreakPointMessage";
        public const string DbgDeleteBreakPointMessageName = "DbgDeleteBreakPointMessage";

        public const string GetPDASnapshotMessageName = "GetPDASnapshotMessage";
    }

    [Obfuscation (Exclude = true)]
    public interface IAnglrServerSideDebugger
    {
        JsonRpc Rpc { get; set; }
        void DbgSingleStepMessageHandler (object sender, EventArgs e);
        void DbgContinueMessageHandler (object sender, EventArgs e);
        void DbgBreakMessageHandler (object sender, EventArgs e);
        void DbgAddBreakPointMessageHandler (object sender, EventArgs e);
        void DbgDeleteBreakPointMessageHandler (object sender, EventArgs e);
        AnglrDebuggerGetPDASnapshotResponse GetPDASnapshotMessageHandler (object sender, EventArgs e);
    }

    [Obfuscation (Exclude = true)]
    public interface IAnglrClientSideDebuggerInvoker
    {
        void InvokeRpcSession (int counter, Stream pipe, CancellationToken token);
        IAnglrLogger Logger { get; }
    }

    [Obfuscation (Exclude = true)]
    public interface IAnglrClientSideDebugger
    {
        JsonRpc Rpc { get; set; }
        void LogMessageHandler (object sender, EventArgs e);
        AnglrDebuggerConnectResponse ConnectMessageHandler (object sender, EventArgs e);
        void SyntaxErrorMessageHandler (object sender, EventArgs e);
        void ShiftStepMessageHandler (object sender, EventArgs e);
        void ReduceStepMessageHandler (object sender, EventArgs e);
        void SplitStepMessageHandler (object sender, EventArgs e);
        void LoopStepMessageHandler (object sender, EventArgs e);
        void JoinMessageHandler (object sender, EventArgs e);
        void FinalStepMessageHandler (object sender, EventArgs e);
        void StopParserMessageHandler (object sender, EventArgs e);
        void DbgBreakPointHitMessageHandler (object sender, EventArgs e);

        IAnglrLogger Logger { get; }
    }

    [Obfuscation (Exclude = true)]
    public class AnglrServerSideDebuggerJsonRpcMessagesHandler
    {
        public IAnglrServerSideDebugger Debugger { get; private set; }

        public AnglrServerSideDebuggerJsonRpcMessagesHandler (IAnglrServerSideDebugger anglrDebugger)
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

        [JsonRpcMethod (AnglrDebuggerJsonRpcMessageNames.GetPDASnapshotMessageName)]
        public AnglrDebuggerGetPDASnapshotResponse HandleGetPDASnapshotMessage (JToken getPDASnapshotMessage)
        {
            AnglrDebuggerGetPDASnapshotRequest getPDASnapshotRequest = getPDASnapshotMessage.ToObject<AnglrDebuggerGetPDASnapshotRequest> ();
            return Task.Run (() => Debugger?.GetPDASnapshotMessageHandler (this, getPDASnapshotRequest)).Result;
        }
    }

    [Obfuscation (Exclude = true)]
    public class AnglrClientSideDebuggerJsonRpcMessagesHandler
    {
        public IAnglrClientSideDebugger Debugger { get; private set; }

        public AnglrClientSideDebuggerJsonRpcMessagesHandler (IAnglrClientSideDebugger anglrDebugger)
        {
            Debugger = anglrDebugger;
        }

        [JsonRpcMethod (AnglrDebuggerJsonRpcMessageNames.LogMessageName)]
        public void HandleLogMessage (JToken logMessage) =>
            Debugger?.LogMessageHandler (this, logMessage.ToObject<AnglrDebuggerLogRequest> ());

        [JsonRpcMethod (AnglrDebuggerJsonRpcMessageNames.ConnectMessageName)]
        public AnglrDebuggerConnectResponse HandleConnectMessage (JToken connectMessage) =>
            Debugger?.ConnectMessageHandler (this, connectMessage.ToObject<AnglrDebuggerConnectRequest> ());

        [JsonRpcMethod (AnglrDebuggerJsonRpcMessageNames.SyntaxErrorMessageName)]
        public void HandleSyntaxErrorMessage (JToken syntaxErrorMessage)=>
            Debugger?.SyntaxErrorMessageHandler (this, syntaxErrorMessage.ToObject<AnglrDebuggerSyntaxErrorRequest> ());

        [JsonRpcMethod (AnglrDebuggerJsonRpcMessageNames.ShiftStepMessageName)]
        public void HandleShiftStepMessage (JToken shiftStepMessage)=>
            Debugger?.ShiftStepMessageHandler (this, shiftStepMessage.ToObject<AnglrDebuggerShiftStepRequest> ());

        [JsonRpcMethod (AnglrDebuggerJsonRpcMessageNames.ReduceStepMessageName)]
        public void HandleReduceStepMessage (JToken reduceStepMessage)=>
            Debugger?.ReduceStepMessageHandler (this, reduceStepMessage.ToObject<AnglrDebuggerReduceStepRequest> ());

        [JsonRpcMethod (AnglrDebuggerJsonRpcMessageNames.SplitStepMessageName)]
        public void HandleSplitStepMessage (JToken splitStepMessage) =>
            Debugger?.SplitStepMessageHandler (this, splitStepMessage.ToObject<AnglrDebuggerSplitStepRequest> ());

        [JsonRpcMethod (AnglrDebuggerJsonRpcMessageNames.LoopStepMessageName)]
        public void HandleLoopStepMessage (JToken loopStepMessage)=>
            Debugger?.LoopStepMessageHandler (this, loopStepMessage.ToObject<AnglrDebuggerLoopStepRequest> ());

        [JsonRpcMethod (AnglrDebuggerJsonRpcMessageNames.JoinMessageName)]
        public void HandleJoinMessage (JToken joinMessage)=>
            Debugger?.JoinMessageHandler (this, joinMessage.ToObject<AnglrDebuggerJoinRequest> ());

        [JsonRpcMethod (AnglrDebuggerJsonRpcMessageNames.FinalStepMessageName)]
        public void HandleFinalStepMessage (JToken finalStepMessage) =>
            Debugger?.FinalStepMessageHandler (this, finalStepMessage.ToObject<AnglrDebuggerFinalStepRequest> ());

        [JsonRpcMethod (AnglrDebuggerJsonRpcMessageNames.StopParserMessageName)]
        public void HandleStopParserMessage (JToken stopParserMessage)=>
            Debugger?.StopParserMessageHandler (this, stopParserMessage.ToObject<AnglrDebuggerStopParserRequest> ());

        [JsonRpcMethod (AnglrDebuggerJsonRpcMessageNames.DbgBreakPointHitMessageName)]
        public void HandleDbgBreakPointHitMessage (JToken dbgBreakPointHitMessage) =>
            Debugger?.DbgBreakPointHitMessageHandler (this, dbgBreakPointHitMessage.ToObject<AnglrDebuggerDbgBreakPointHitRequest> ());
    }
}
