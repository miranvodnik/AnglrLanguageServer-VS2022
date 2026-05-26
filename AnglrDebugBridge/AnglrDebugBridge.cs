using Anglr.Parser.Core;
using Anglr.Parser.SyntaxTree;
using AnglrDebuggerJsonRpcMessages;
using AnglrLogLibrary;
using AnglrBreakPointDBLibrary;
using Microsoft.VisualStudio.Threading;
using StreamJsonRpc;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.IO;
using System.IO.Pipes;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace AnglrDebuggerBridge
{
    public class AnglrDebuggerClientBridge : IAnglrClientSideDebugger
    {
        public AnglrLogLevel LogLevel { get; private set; }
        public ParserInterface ParserInterface { get; private set; }
        public NamedPipeClientStream AnglrDebuggerClientStream { get; private set; }
        public JsonRpc Rpc { get; set; }
        public AnglrClientSideDebuggerJsonRpcMessagesHandler AnglrDebugger { get; private set; }
        private AutoResetEvent ParserStepEvent { get; set; }
        public AnglrBreakPointDBChunk AnglrBreakPointDBChunk { get; private set; }
        public AnglrBitField AnglrProdBitField { get; private set; }
        public AnglrBitField AnglrShiftBitField { get; private set; }
        public AnglrBitField AnglrGotoBitField { get; private set; }
        private object LockingObject { get; set; }
        private bool SingleStepBreakPoint { get; set; }
        private bool BreakBreakPoint { get; set; }
        private int SequenceCount;
#if !ANGLR_DBG_SYNC_RPC
        private int MessageCount { get; set; }
        private ManualResetEventSlim MessageCountEvent { get; set; }
        private bool MessageCountFlag { get; set; }
#endif
        public AnglrDebuggerClientBridge (ParserInterface parserInterface)
        {
            LogLevel = AnglrLogLevel.Info;
            ParserInterface = parserInterface;
            AnglrDebugger = new AnglrClientSideDebuggerJsonRpcMessagesHandler (this);

            ParserStepEvent = new AutoResetEvent (false);

            LockingObject = new object ();
            SingleStepBreakPoint = false;
            BreakBreakPoint = false;

            AnglrBreakPointDBChunk = new AnglrBreakPointDBChunk ();
            AnglrBreakPointDBChunk.AnglrShiftBPEvent += AnglrBreakPointDBChunk_AnglrShiftBPEvent;
            AnglrBreakPointDBChunk.AnglrGotoBPEvent += AnglrBreakPointDBChunk_AnglrGotoBPEvent;
            AnglrBreakPointDBChunk.AnglrReduceBPEvent += AnglrBreakPointDBChunk_AnglrReduceBPEvent;

            AnglrProdBitField = new AnglrBitField ((uint) ParserInterface.productionLengths ().Length);

            SequenceCount = 0;
#if !ANGLR_DBG_SYNC_RPC
            MessageCount = 0;
            MessageCountFlag = false;
            MessageCountEvent = new ManualResetEventSlim (false);
#endif

            Process process = Process.GetCurrentProcess ();
            AnglrDebuggerClientStream = new NamedPipeClientStream (".", $"anglr-debugger-{process.Id}", PipeDirection.InOut, PipeOptions.Asynchronous);
            Log (AnglrLogLevel.Info, $"<AnglrDebuggerClientBridge>: client pipe 'anglr-debugger-{process.Id}' created");

            try
            {
                AnglrDebuggerClientStream.Connect (1000);
                if (AnglrDebuggerClientStream.IsConnected)
                {
                    Log (AnglrLogLevel.Info, $"<AnglrDebuggerClientBridge>: client connected");
                    Rpc = JsonRpc.Attach (AnglrDebuggerClientStream, AnglrDebuggerClientStream, AnglrDebugger);
                    Log (AnglrLogLevel.Info, $"<AnglrDebuggerClientBridge>: client rpc created");
                    Rpc.Disconnected += Rpc_Disconnected;
                }
            }
            catch (Exception e)
            {
                LogException (e);
            }

            ParserInterface.StartParserEvent += ParserInterface_StartParserEvent_Async;
            ParserInterface.SyntaxErrorEvent += ParserInterface_SyntaxErrorEvent_Async;
            ParserInterface.ShiftStepEvent += ParserInterface_ShiftStepEvent_Async;
            ParserInterface.ReduceStepEvent += ParserInterface_ReduceStepEvent_Async;
            ParserInterface.SplitStepEvent += ParserInterface_SplitStepEvent_Async;
            ParserInterface.LoopStepEvent += ParserInterface_LoopStepEvent_Async;
            ParserInterface.JoinEvent += ParserInterface_JoinEvent_Async;
            ParserInterface.FinalStepEvent += ParserInterface_FinalStepEvent_Async;
            ParserInterface.StopParserEvent += ParserInterface_StopParserEvent_Async;
        }

        private void AnglrBreakPointDBChunk_AnglrShiftBPEvent (AnglrShiftBP bp, bool removed)
        {
        }

        private void AnglrBreakPointDBChunk_AnglrGotoBPEvent (AnglrGotoBP bp, bool removed)
        {
        }

        private void AnglrBreakPointDBChunk_AnglrReduceBPEvent (AnglrReduceBP bp, bool removed)
        {
            Log (AnglrLogLevel.Info, $"change syntax rule break-point: production = {bp.ProductionNumber}, set = {removed}");
            if (removed)
                AnglrProdBitField.Clear ((uint) bp.ProductionNumber);
            else
                AnglrProdBitField.Set ((uint) bp.ProductionNumber);
        }

        public void Dispose ()
        {
            ParserInterface.StartParserEvent -= ParserInterface_StartParserEvent_Async;
            ParserInterface.SyntaxErrorEvent -= ParserInterface_SyntaxErrorEvent_Async;
            ParserInterface.ShiftStepEvent -= ParserInterface_ShiftStepEvent_Async;
            ParserInterface.ReduceStepEvent -= ParserInterface_ReduceStepEvent_Async;
            ParserInterface.SplitStepEvent -= ParserInterface_SplitStepEvent_Async;
            ParserInterface.LoopStepEvent -= ParserInterface_LoopStepEvent_Async;
            ParserInterface.JoinEvent -= ParserInterface_JoinEvent_Async;
            ParserInterface.FinalStepEvent -= ParserInterface_FinalStepEvent_Async;
            ParserInterface.StopParserEvent -= ParserInterface_StopParserEvent_Async;

            if (Rpc != null)
            {
                Log (AnglrLogLevel.Info, $"<AnglrDebuggerClientBridge>: RPC disposed");
#if !ANGLR_DBG_SYNC_RPC
                int count = 0;
                lock (LockingObject)
                {
                    count = MessageCount;
                    MessageCountFlag = true;
                }
                if (count != 0)
                    MessageCountEvent.Wait ();
#endif
                Rpc.Dispose ();
                Rpc = null;
            }
        }

#if !ANGLR_DBG_SYNC_RPC
        private void IncMessageCount ()
        {
            lock (LockingObject)
            {
                ++MessageCount;
            }
        }

        private void DecMessageCount(int? sequenceNr)
        {
            lock (LockingObject)
            {
                if ((--MessageCount <= 0) && MessageCountFlag)
                    MessageCountEvent.Set ();
                Log (AnglrLogLevel.Info, $"sequence nr = {sequenceNr}");
            }
        }
#endif

        private void Rpc_Disconnected (object sender, JsonRpcDisconnectedEventArgs e)
        {
            ParserStepEvent?.Set ();
            SingleStepBreakPoint = false;
            BreakBreakPoint = false;

            Log (AnglrLogLevel.Info, $"<AnglrDebuggerClientBridge>: RPC disconnected, reason: {e.Reason}");
        }

        public void Log (AnglrLogLevel logLevel, string message)
        {
            if (logLevel < LogLevel)
                return;
            if (Rpc == null)
                return;
            try
            {
                _ = Rpc.InvokeAsync<AnglrDebuggerLogResponse>
                (
                    AnglrDebuggerJsonRpcMessageNames.LogMessageName,
                    new AnglrDebuggerLogRequest ()
                    {
                        SequenceNr = ++SequenceCount,
                        LogLevel = (int) logLevel,
                        Message = message
                    }
                );
            }
            catch (Exception ex)
            {
            }
        }

        private void LogException (Exception exception)
        {
            StackTrace stackTrace = new StackTrace ();
            if (stackTrace.FrameCount < 2)
                return;
            StackFrame frame = stackTrace.GetFrame (1);
            MethodBase method = frame.GetMethod ();
            int depth = 0;
            Log (AnglrLogLevel.Error, $"{method.DeclaringType.Name}.{method.Name} exception:");
            while (exception != null)
            {
                Log (AnglrLogLevel.Error, $"\t{depth}: {exception.Message}");
                Log (AnglrLogLevel.Error, $"\t{depth}: {exception.StackTrace}");
                exception = exception.InnerException;
                depth++;
            }
        }

        private bool CheckBreakPoint ()
        {
            lock (LockingObject)
            {
                bool check = SingleStepBreakPoint || BreakBreakPoint;
                SingleStepBreakPoint = BreakBreakPoint = false;
                return check;
            }
        }

        //
        // The ANGLR_DBG_SYNC_RPC switch is intended for handling RPC messages.
        //
        // When enabled, all RPC messages are exchanged synchronously. This
        // means that the next message is sent only after a response to the
        // previous one has been received. This method is easier, especially
        // if the answers are connected to each other. However, it is slower.
        //
        // When disabled, all RPC messages are exchanged asynchronously. This
        // means that the next message is sent only after a response to the
        // previous one has been received. This means that messages are sent
        // regardless of when (or if at all) responses are received. This
        // method is faster and particularly suitable when the responses are
        // not interrelated. However, if they are interrelated, problems may
        // arise because the order of responses is not necessarily the same
        // as the order of messages. In this case, it is necessary to
        // implement additional logic that can resolve the order of responses.
        // With this method, it is necessary to be careful about releasing the
        // RPC channel. Before releasing it, it is necessary to wait for all
        // delayed responses to arrive. See the Dispose() method.
        //

#if ANGLR_DBG_SYNC_RPC

        //
        // The methods in this block are executed synchronously
        // with respect to the JsonRpc object through which
        // RPC messages are exchanged.
        //

        private void ParserInterface_StartParserEvent_Async (int stackNr, object [] info = null)
        {
            if (!AnglrDebuggerClientStream.IsConnected)
                return;

            try
            {
                //ParserStepEvent.WaitOne ();

                Log (AnglrLogLevel.Info, $"<AnglrDebuggerClientBridge>: client connection request task created");
                AnglrDebuggerConnectResponse anglrDebuggerConnectResponse = Rpc.InvokeAsync<AnglrDebuggerConnectResponse>
                (
                    AnglrDebuggerJsonRpcMessageNames.ConnectMessageName,
                    new AnglrDebuggerConnectRequest ()
                    {
                        SequenceNr = ++SequenceCount,
                        StackNr = stackNr,
                        MagicNumber = ParserInterface.magicNumber (),
                        Info = info
                    }
                ).Result;
                Log (AnglrLogLevel.Info, $"<AnglrDebuggerClientBridge>: client connection response received");
                if (anglrDebuggerConnectResponse == null)
                {
                    Log (AnglrLogLevel.Info, $"<AnglrDebuggerClientBridge>: client connection response invalid, dispose client side pipe");
                    AnglrDebuggerClientStream.Dispose ();
                    return;
                }
                if (anglrDebuggerConnectResponse.BreakPointDB == null)
                    return;
                Log (AnglrLogLevel.Info, $"BreakPointDB = {anglrDebuggerConnectResponse.BreakPointDB}");
                AnglrBreakPointDBChunk?.ApplyChanges (JsonConvert.DeserializeObject<AnglrBreakPointDBChunk> (anglrDebuggerConnectResponse.BreakPointDB));
            }
            catch (Exception e)
            {
                LogException (e);
            }
        }

        private void ParserInterface_SyntaxErrorEvent_Async (int stackNr, int state)
        {
            if (!AnglrDebuggerClientStream.IsConnected)
                return;

            try
            {
                if (CheckBreakPoint ())
                    ParserStepEvent.WaitOne ();

                Log (AnglrLogLevel.Trace, $"<AnglrDebuggerClientBridge>: client syntax error request task created");
                _ = Rpc.InvokeAsync<AnglrDebuggerSyntaxErrorResponse>
                (
                    AnglrDebuggerJsonRpcMessageNames.SyntaxErrorMessageName,
                    new AnglrDebuggerSyntaxErrorRequest ()
                    {
                        SequenceNr = ++SequenceCount,
                        StackNr = stackNr,
                        State = state
                    }
                );
                Log (AnglrLogLevel.Trace, $"<AnglrDebuggerClientBridge>: client syntax error response received");
            }
            catch (Exception e)
            {
                LogException (e);
            }
        }

        private void ParserInterface_ShiftStepEvent_Async (int stackNr, int state, int tokenValue, string tokenName, string tokenText, bool conflict)
        {
            if (!AnglrDebuggerClientStream.IsConnected)
                return;

            try
            {
                if (CheckBreakPoint ())
                    ParserStepEvent.WaitOne ();

                Log (AnglrLogLevel.Trace, $"<AnglrDebuggerClientBridge>: client shift step request task created");
                _ = Rpc.InvokeAsync<AnglrDebuggerShiftStepResponse>
                (
                    AnglrDebuggerJsonRpcMessageNames.ShiftStepMessageName,
                    new AnglrDebuggerShiftStepRequest ()
                    {
                        SequenceNr = ++SequenceCount,
                        StackNr = stackNr,
                        State = state,
                        TokenValue = tokenValue,
                        TokenName = tokenName,
                        TokenText = tokenText,
                        Conflict = conflict
                    }
                );
                Log (AnglrLogLevel.Trace, $"<AnglrDebuggerClientBridge>: client shift step response received");
            }
            catch (Exception e)
            {
                LogException (e);
            }
        }

        private void ParserInterface_ReduceStepEvent_Async (int stackNr, int prodNr, int ruleNr, string ruleName, int prodLen, int fallingState, int bottomState, int risingState, Anglr.Parser.SyntaxTree.SyntaxTreeBase currentValue, bool conflict)
        {
            if (!AnglrDebuggerClientStream.IsConnected)
                return;

            try
            {
                if (CheckBreakPoint ())
                    ParserStepEvent.WaitOne ();

                Log (AnglrLogLevel.Trace, $"<AnglrDebuggerClientBridge>: client reduce step request task created");
                _ = Rpc.InvokeAsync<AnglrDebuggerReduceStepResponse>
                (
                    AnglrDebuggerJsonRpcMessageNames.ReduceStepMessageName,
                    new AnglrDebuggerReduceStepRequest ()
                    {
                        SequenceNr = ++SequenceCount,
                        StackNr = stackNr,
                        ProdNr = prodNr,
                        RuleNr = ruleNr,
                        RuleName = ruleName,
                        ProdLen = prodLen,
                        FallingState = fallingState,
                        BottomState = bottomState,
                        RisingState = risingState,
                        Conflict = conflict
                    }
                );
                Log (AnglrLogLevel.Trace, $"<AnglrDebuggerClientBridge>: client reduce step response received");
            }
            catch (Exception e)
            {
                LogException (e);
            }
            if (AnglrProdBitField.Check ((uint) prodNr) == 0)
                return;
            BreakBreakPoint = true;
            Log (AnglrLogLevel.Info, $"syntax rule break-point");
            Log (AnglrLogLevel.Info, $"\tsyntax rule name:  {ruleName}");
            Log (AnglrLogLevel.Info, $"\tproduction number: {prodNr}");
            Log (AnglrLogLevel.Info, $"\tsyntax rule nr.:   {ruleNr}");
            Log (AnglrLogLevel.Info, $"\tstate transitions: {fallingState} --> {bottomState} --> {risingState}");
            Log (AnglrLogLevel.Info, $"\tsyntax rule text:  {currentValue.Emit (-1)}");
        }

        private void ParserInterface_SplitStepEvent_Async (int oldStackNr, int newStackNr, bool begin)
        {
            if (!AnglrDebuggerClientStream.IsConnected)
                return;

            try
            {
                if (CheckBreakPoint ())
                    ParserStepEvent.WaitOne ();

                Log (AnglrLogLevel.Trace, $"<AnglrDebuggerClientBridge>: client split step request task created");
                _ = Rpc.InvokeAsync<AnglrDebuggerSplitStepResponse>
                (
                    AnglrDebuggerJsonRpcMessageNames.SplitStepMessageName,
                    new AnglrDebuggerSplitStepRequest ()
                    {
                        SequenceNr = ++SequenceCount,
                        OldStackNr = oldStackNr,
                        NewStackNr = newStackNr,
                        Begin = begin
                    }
                );
                Log (AnglrLogLevel.Trace, $"<AnglrDebuggerClientBridge>: client split step response received");
            }
            catch (Exception e)
            {
                LogException (e);
            }
        }

        private void ParserInterface_LoopStepEvent_Async (int stackNr, int state)
        {
            if (!AnglrDebuggerClientStream.IsConnected)
                return;

            try
            {
                if (CheckBreakPoint ())
                    ParserStepEvent.WaitOne ();

                Log (AnglrLogLevel.Trace, $"<AnglrDebuggerClientBridge>: client loop step request task created");
                _ = Rpc.InvokeAsync<AnglrDebuggerLoopStepResponse>
                (
                    AnglrDebuggerJsonRpcMessageNames.LoopStepMessageName,
                    new AnglrDebuggerLoopStepRequest ()
                    {
                        SequenceNr = ++SequenceCount,
                        StackNr = stackNr,
                        State = state
                    }
                );
                Log (AnglrLogLevel.Trace, $"<AnglrDebuggerClientBridge>: client loop step response received");
            }
            catch (Exception e)
            {
                LogException (e);
            }
        }

        private void ParserInterface_JoinEvent_Async (int stackNr, int joinNr)
        {
            if (!AnglrDebuggerClientStream.IsConnected)
                return;

            try
            {
                if (CheckBreakPoint ())
                    ParserStepEvent.WaitOne ();

                Log (AnglrLogLevel.Trace, $"<AnglrDebuggerClientBridge>: client join request task created");
                _ = Rpc.InvokeAsync<AnglrDebuggerJoinResponse>
                (
                    AnglrDebuggerJsonRpcMessageNames.JoinMessageName,
                    new AnglrDebuggerJoinRequest ()
                    {
                        SequenceNr = ++SequenceCount,
                        StackNr = stackNr,
                        JoinNr = joinNr
                    }
                );
                Log (AnglrLogLevel.Trace, $"<AnglrDebuggerClientBridge>: client join response received");
            }
            catch (Exception e)
            {
                LogException (e);
            }
        }

        private void ParserInterface_FinalStepEvent_Async (int stackNr)
        {
            if (!AnglrDebuggerClientStream.IsConnected)
                return;

            try
            {
                if (CheckBreakPoint ())
                    ParserStepEvent.WaitOne ();

                Log (AnglrLogLevel.Trace, $"<AnglrDebuggerClientBridge>: client final step request task created");
                _ = Rpc.InvokeAsync<AnglrDebuggerFinalStepResponse>
                (
                    AnglrDebuggerJsonRpcMessageNames.FinalStepMessageName,
                    new AnglrDebuggerFinalStepRequest ()
                    {
                        SequenceNr = ++SequenceCount,
                        StackNr = stackNr
                    }
                );
                Log (AnglrLogLevel.Trace, $"<AnglrDebuggerClientBridge>: client final step response received");
            }
            catch (Exception e)
            {
                LogException (e);
            }
        }

        private void ParserInterface_StopParserEvent_Async ()
        {
            if (!AnglrDebuggerClientStream.IsConnected)
                return;

            try
            {
                if (CheckBreakPoint ())
                    ParserStepEvent.WaitOne ();

                Log (AnglrLogLevel.Info, $"<AnglrDebuggerClientBridge>: client stop parser request task created");
                _ = Rpc.InvokeAsync<AnglrDebuggerStopParserResponse>
                (
                    AnglrDebuggerJsonRpcMessageNames.StopParserMessageName,
                    new AnglrDebuggerStopParserRequest ()
                    {
                        SequenceNr = ++SequenceCount
                    }
                ).Result;
                Log (AnglrLogLevel.Info, $"<AnglrDebuggerClientBridge>: client stop parser response received");
            }
            catch (Exception e)
            {
                LogException (e);
            }
        }
#else

        //
        // The methods in this block are executed asynchronously
        // with respect to the JsonRpc object through which
        // RPC messages are exchanged. Every method is in fact
        // asynchronous task invoking message exchange. The methods
        // in both blocks are similar. The key difference in this
        // block is that it is necessary to count RPC messages.
        //

        private async void ParserInterface_StartParserEvent_Async (int stackNr)
        {
            try
            {
                ParserStepEvent.WaitOne ();

                if (AnglrDebuggerClientStream.IsConnected)
                {
                    Log (AnglrLogLevel.Info, $"<AnglrDebuggerClientBridge>: client connection request task created");
                    IncMessageCount ();
                    AnglrDebuggerConnectResponse anglrDebuggerConnectResponse = await Rpc.InvokeAsync<AnglrDebuggerConnectResponse>
                    (
                        AnglrDebuggerJsonRpcMessageNames.ConnectMessageName,
                        new AnglrDebuggerConnectRequest ()
                        {
                            SequenceNr = Interlocked.Increment(ref SequenceCount),
                            StackNr = stackNr,
                            MagicNumber = ParserInterface.magicNumber ()
                        }
                    );
                    DecMessageCount (anglrDebuggerConnectResponse?.SequenceNr);
                    if (anglrDebuggerConnectResponse.Valid)
                        return;
                    AnglrDebuggerClientStream.Dispose ();
                }
            }
            catch (Exception e)
            {
                LogException (e);
            }
        }

        private async void ParserInterface_SyntaxErrorEvent_Async (int stackNr, int state)
        {
            try
            {
                if (CheckBreakPoint ())
                    ParserStepEvent.WaitOne ();

                if (AnglrDebuggerClientStream.IsConnected)
                {
                    Log (AnglrLogLevel.Trace, $"<AnglrDebuggerClientBridge>: client syntax error request task created");
                    IncMessageCount ();
                    AnglrDebuggerSyntaxErrorResponse anglrDebuggerSyntaxErrorResponse = await Rpc.InvokeAsync<AnglrDebuggerSyntaxErrorResponse>
                    (
                        AnglrDebuggerJsonRpcMessageNames.SyntaxErrorMessageName,
                        new AnglrDebuggerSyntaxErrorRequest ()
                        {
                            SequenceNr = Interlocked.Increment (ref SequenceCount),
                            StackNr = stackNr,
                            State = state
                        }
                    );
                    DecMessageCount (anglrDebuggerSyntaxErrorResponse?.SequenceNr);
                    if (anglrDebuggerSyntaxErrorResponse == null)
                        return;
                    Log (AnglrLogLevel.Trace, $"<AnglrDebuggerClientBridge>: client syntax error response received");
                }
            }
            catch (Exception e)
            {
                LogException (e);
            }
        }

        private async void ParserInterface_ShiftStepEvent_Async (int stackNr, int state, int tokenValue, string tokenText, bool conflict)
        {
            try
            {
                if (CheckBreakPoint ())
                    ParserStepEvent.WaitOne ();

                if (AnglrDebuggerClientStream.IsConnected)
                {
                    Log (AnglrLogLevel.Trace, $"<AnglrDebuggerClientBridge>: client shift step request task created");
                    IncMessageCount ();
                    AnglrDebuggerShiftStepResponse anglrDebuggerShiftStepResponse = await Rpc.InvokeAsync<AnglrDebuggerShiftStepResponse>
                    (
                        AnglrDebuggerJsonRpcMessageNames.ShiftStepMessageName,
                        new AnglrDebuggerShiftStepRequest ()
                        {
                            SequenceNr = Interlocked.Increment (ref SequenceCount),
                            StackNr = stackNr,
                            State = state,
                            TokenValue = tokenValue,
                            TokenText = tokenText,
                            Conflict = conflict
                        }
                    );
                    DecMessageCount (anglrDebuggerShiftStepResponse?.SequenceNr);
                    if (anglrDebuggerShiftStepResponse == null)
                        return;
                    Log (AnglrLogLevel.Trace, $"<AnglrDebuggerClientBridge>: client shift step response received");
                }
            }
            catch (Exception e)
            {
                LogException (e);
            }
        }

        private async void ParserInterface_ReduceStepEvent_Async (int stackNr, int prodNr, int ruleNr, int prodLen, int fallingState, int bottomState, int risingState, Anglr.Parser.SyntaxTree.SyntaxTreeBase currentValue, bool conflict)
        {
            try
            {
                if (CheckBreakPoint ())
                    ParserStepEvent.WaitOne ();

                if (AnglrDebuggerClientStream.IsConnected)
                {
                    Log (AnglrLogLevel.Trace, $"<AnglrDebuggerClientBridge>: client reduce step request task created");
                    IncMessageCount ();
                    AnglrDebuggerReduceStepResponse anglrDebuggerReduceStepResponse = await Rpc.InvokeAsync<AnglrDebuggerReduceStepResponse>
                    (
                        AnglrDebuggerJsonRpcMessageNames.ReduceStepMessageName,
                        new AnglrDebuggerReduceStepRequest ()
                        {
                            SequenceNr = Interlocked.Increment (ref SequenceCount),
                            StackNr = stackNr,
                            ProdNr = prodNr,
                            RuleNr = ruleNr,
                            ProdLen = prodLen,
                            FallingState = fallingState,
                            BottomState = bottomState,
                            RisingState = risingState,
                            Conflict = conflict
                        }
                    );
                    DecMessageCount (anglrDebuggerReduceStepResponse?.SequenceNr);
                    if (anglrDebuggerReduceStepResponse == null)
                        return;
                    Log (AnglrLogLevel.Trace, $"<AnglrDebuggerClientBridge>: client reduce step response received");
                }
            }
            catch (Exception e)
            {
                LogException (e);
            }
        }

        private async void ParserInterface_SplitStepEvent_Async (int stackNr, int state, bool begin)
        {
            try
            {
                if (CheckBreakPoint ())
                    ParserStepEvent.WaitOne ();

                if (AnglrDebuggerClientStream.IsConnected)
                {
                    Log (AnglrLogLevel.Trace, $"<AnglrDebuggerClientBridge>: client split step request task created");
                    IncMessageCount ();
                    AnglrDebuggerSplitStepResponse anglrDebuggerSplitStepResponse = await Rpc.InvokeAsync<AnglrDebuggerSplitStepResponse>
                    (
                        AnglrDebuggerJsonRpcMessageNames.SplitStepMessageName,
                        new AnglrDebuggerSplitStepRequest ()
                        {
                            SequenceNr = Interlocked.Increment (ref SequenceCount),
                            StackNr = stackNr,
                            State = state,
                            Begin = begin
                        }
                    );
                    DecMessageCount (anglrDebuggerSplitStepResponse?.SequenceNr);
                    if (anglrDebuggerSplitStepResponse == null)
                        return;
                    Log (AnglrLogLevel.Trace, $"<AnglrDebuggerClientBridge>: client split step response received");
                }
            }
            catch (Exception e)
            {
                LogException (e);
            }
        }

        private async void ParserInterface_LoopStepEvent_Async (int stackNr, int state)
        {
            try
            {
                if (CheckBreakPoint ())
                    ParserStepEvent.WaitOne ();

                if (AnglrDebuggerClientStream.IsConnected)
                {
                    Log (AnglrLogLevel.Trace, $"<AnglrDebuggerClientBridge>: client loop step request task created");
                    IncMessageCount ();
                    AnglrDebuggerLoopStepResponse anglrDebuggerLoopStepResponse = await Rpc.InvokeAsync<AnglrDebuggerLoopStepResponse>
                    (
                        AnglrDebuggerJsonRpcMessageNames.LoopStepMessageName,
                        new AnglrDebuggerLoopStepRequest ()
                        {
                            SequenceNr = Interlocked.Increment (ref SequenceCount),
                            StackNr = stackNr,
                            State = state
                        }
                    );
                    DecMessageCount (anglrDebuggerLoopStepResponse?.SequenceNr);
                    if (anglrDebuggerLoopStepResponse == null)
                        return;
                    Log (AnglrLogLevel.Trace, $"<AnglrDebuggerClientBridge>: client loop step response received");
                }
            }
            catch (Exception e)
            {
                LogException (e);
            }
        }

        private async void ParserInterface_JoinEvent_Async (int stackNr, int joinNr)
        {
            try
            {
                if (CheckBreakPoint ())
                    ParserStepEvent.WaitOne ();

                if (AnglrDebuggerClientStream.IsConnected)
                {
                    Log (AnglrLogLevel.Trace, $"<AnglrDebuggerClientBridge>: client join request task created");
                    IncMessageCount ();
                    AnglrDebuggerJoinResponse anglrDebuggerJoinResponse = await Rpc.InvokeAsync<AnglrDebuggerJoinResponse>
                    (
                        AnglrDebuggerJsonRpcMessageNames.JoinMessageName,
                        new AnglrDebuggerJoinRequest ()
                        {
                            SequenceNr = Interlocked.Increment (ref SequenceCount),
                            StackNr = stackNr,
                            JoinNr = joinNr
                        }
                    );
                    DecMessageCount (anglrDebuggerJoinResponse?.SequenceNr);
                    if (anglrDebuggerJoinResponse == null)
                        return;
                    Log (AnglrLogLevel.Trace, $"<AnglrDebuggerClientBridge>: client join response received");
                }
            }
            catch (Exception e)
            {
                LogException (e);
            }
        }

        private async void ParserInterface_FinalStepEvent_Async (int stackNr)
        {
            try
            {
                if (CheckBreakPoint ())
                    ParserStepEvent.WaitOne ();

                if (AnglrDebuggerClientStream.IsConnected)
                {
                    Log (AnglrLogLevel.Trace, $"<AnglrDebuggerClientBridge>: client final step request task created");
                    IncMessageCount ();
                    AnglrDebuggerFinalStepResponse anglrDebuggerFinalStepResponse = await Rpc.InvokeAsync<AnglrDebuggerFinalStepResponse>
                    (
                        AnglrDebuggerJsonRpcMessageNames.FinalStepMessageName,
                        new AnglrDebuggerFinalStepRequest ()
                        {
                            SequenceNr = Interlocked.Increment (ref SequenceCount),
                            StackNr = stackNr
                        }
                    );
                    DecMessageCount (anglrDebuggerFinalStepResponse?.SequenceNr);
                    if (anglrDebuggerFinalStepResponse == null)
                        return;
                    Log (AnglrLogLevel.Trace, $"<AnglrDebuggerClientBridge>: client final step response received");
                }
            }
            catch (Exception e)
            {
                LogException (e);
            }
        }

        private async void ParserInterface_StopParserEvent_Async ()
        {
            try
            {
                if (CheckBreakPoint ())
                    ParserStepEvent.WaitOne ();

                if (AnglrDebuggerClientStream.IsConnected)
                {
                    Log (AnglrLogLevel.Info, $"<AnglrDebuggerClientBridge>: client stop parser request task created");
                    IncMessageCount ();
                    AnglrDebuggerStopParserResponse anglrDebuggerStopParserResponse = await Rpc.InvokeAsync<AnglrDebuggerStopParserResponse>
                    (
                        AnglrDebuggerJsonRpcMessageNames.StopParserMessageName,
                        new AnglrDebuggerStopParserRequest ()
                        {
                            SequenceNr = Interlocked.Increment (ref SequenceCount)
                        }
                    );
                    DecMessageCount (anglrDebuggerStopParserResponse?.SequenceNr);
                    if (anglrDebuggerStopParserResponse == null)
                        return;
                    Log (AnglrLogLevel.Info, $"<AnglrDebuggerClientBridge>: client stop parser response received");
                }
            }
            catch (Exception e)
            {
                LogException (e);
            }
        }
#endif

        public void DbgSingleStepMessageHandler (object sender, EventArgs e)
        {
            AnglrDebuggerDbgSingleStepRequest dbgSingleStepMessageRequest = e as AnglrDebuggerDbgSingleStepRequest;
            lock (LockingObject)
            {
                try
                {
                    if (dbgSingleStepMessageRequest.BreakPointDB != null)
                    {
                        Log (AnglrLogLevel.Info, $"BreakPointDB = {dbgSingleStepMessageRequest.BreakPointDB}");
                        AnglrBreakPointDBChunk?.ApplyChanges (JsonConvert.DeserializeObject<AnglrBreakPointDBChunk> (dbgSingleStepMessageRequest.BreakPointDB));
                    }
                    SingleStepBreakPoint = true;
                    ParserStepEvent.Set ();
                }
                catch (Exception ex)
                {
                    LogException (ex);
                }
            }
        }

        public void DbgBreakMessageHandler (object sender, EventArgs e)
        {
            AnglrDebuggerDbgBreakRequest dbgBreakMessageRequest = e as AnglrDebuggerDbgBreakRequest;
            lock (LockingObject)
            {
                BreakBreakPoint = true;
                ParserStepEvent.Reset ();
            }
        }

        public void DbgContinueMessageHandler (object sender, EventArgs e)
        {
            AnglrDebuggerDbgContinueRequest dbgContinueMessageRequest = e as AnglrDebuggerDbgContinueRequest;
            lock (LockingObject)
            {
                try
                {
                    if (dbgContinueMessageRequest.BreakPointDB != null)
                    {
                        Log (AnglrLogLevel.Info, $"BreakPointDB = {dbgContinueMessageRequest.BreakPointDB}");
                        AnglrBreakPointDBChunk?.ApplyChanges (JsonConvert.DeserializeObject<AnglrBreakPointDBChunk> (dbgContinueMessageRequest.BreakPointDB));
                    }
                    ParserStepEvent.Set ();
                }
                catch (Exception ex)
                {
                    LogException (ex);
                }
            }
        }

        public void DbgAddBreakPointMessageHandler (object sender, EventArgs e)
        {
            AnglrDebuggerDbgAddBreakPointRequest dbgAddBreakPointMessageRequest = e as AnglrDebuggerDbgAddBreakPointRequest;
            lock (LockingObject)
            {
            }
        }

        public void DbgDeleteBreakPointMessageHandler (object sender, EventArgs e)
        {
            AnglrDebuggerDbgDeleteBreakPointRequest dbgDeleteBreakPointMessageRequest = e as AnglrDebuggerDbgDeleteBreakPointRequest;
            lock (LockingObject)
            {
            }
        }
    }

    /// <summary>
    /// This class implements the infrastructure needed for sequential or
    /// parallel execution of parser debugging tasks initiated by external
    /// process using JsonRpc communication.
    /// </summary>
    public class AnglrDebuggerServerBridge
    {
        [Obfuscation(Exclude = true)]
        public AnglrLogLevel LogLevel { get; set; }

        [Obfuscation (Exclude = true)]
        public string FileName { get; set; }
        [Obfuscation (Exclude = true)]
        public string Arguments { get; set; }
        [Obfuscation (Exclude = true)]
        public string WorkingDirectory { get; set; }
        [Obfuscation (Exclude = true)]
        public string OutFileName { get; set; }
        [Obfuscation (Exclude = true)]
        public string ErrFileName { get; set; }
        [Obfuscation (Exclude = true)]
        public bool IsRunning { get; set; }

        private Process process;

        /// <summary>
        /// default constructor used by save/restore methods
        /// </summary>
        public AnglrDebuggerServerBridge ()
        {
            LogLevel = AnglrLogLevel.Info;
            FileName = "";
            Arguments = "";
            WorkingDirectory = "";
            OutFileName = "";
            ErrFileName = "";
            IsRunning = false;
            process = null;
        }

        /// <summary>
        /// constructor used by debugger activation
        /// </summary>
        /// <param name="fileName">
        /// executable file path
        /// </param>
        /// <param name="arguments">
        /// string containing executable file arguments
        /// </param>
        /// <param name="workingDirectory">
        /// working directory of executable file
        /// </param>
        /// <param name="outFileName">
        /// output file path
        /// </param>
        /// <param name="errFileName">
        /// error file path
        /// </param>
        public AnglrDebuggerServerBridge (string fileName, string arguments, string workingDirectory, string outFileName = null, string errFileName = null)
        {
            LogLevel = AnglrLogLevel.Trace;
            FileName = fileName;
            Arguments = arguments;
            WorkingDirectory = workingDirectory;
            OutFileName = outFileName;
            ErrFileName = errFileName;
            IsRunning = false;
            process = null;
        }

        /// <summary>
        /// This method stop debugged process by killing it. It is used at the end of
        /// application to store basic debugging info into persisting storage of
        /// user's AppData: C:\Users\$(user-name)\AppData\Local\.anglr
        /// </summary>
        public void Reset ()
        {
            process?.Kill ();
            IsRunning = false;
        }

        /// <summary>
        /// Display exception message using user interface's Log functionality
        /// </summary>
        /// <param name="userInterface">
        /// reference to user interface object
        /// </param>
        /// <param name="exception">
        /// reference to reported exception
        /// </param>
        private void LogException (IAnglrServerSideDebuggerInvoker userInterface, Exception exception)
        {
            StackTrace stackTrace = new StackTrace ();
            if (stackTrace.FrameCount < 2)
                return;
            StackFrame frame = stackTrace.GetFrame (1);
            MethodBase method = frame.GetMethod ();
            int depth = 0;
            userInterface?.Logger?.ErrorLine ($"{method.DeclaringType.Name}.{method.Name} exception:");
            while (exception != null)
            {
                userInterface?.Logger?.ErrorLine ($"\t{depth}: {exception.Message}");
                userInterface?.Logger?.ErrorLine ($"\t{depth}: {exception.StackTrace}");
                exception = exception.InnerException;
                depth++;
            }
        }

        /// <summary>
        /// This method represents task which does the following things:
        /// <list type="bullet">
        /// <item>
        /// <term>
        /// create process which will be debugged
        /// </term>
        /// <description>
        /// it initializes ProcessStartInfo object with executable file name,
        /// arguments used by this executable
        /// and initial working directory
        /// </description>
        /// </item>
        /// <item>
        /// <term>
        /// start process
        /// </term>
        /// <description>
        /// using ProcessStartInfo object, created in the previous step,
        /// it starts process by using object's Start() method
        /// </description>
        /// </item>
        /// <item>
        /// <term>
        /// start process controlling task
        /// </term>
        /// <description>
        /// then it is started special task, which establishes communication
        /// link with process being debugged, in fact with every instance of
        /// Anglr parser object created by debugged process. Debugged process
        /// should be built in such way that this is possible, otherwise the
        /// debugged process will run unnoticed.
        /// </description>
        /// </item>
        /// <item>
        /// <term>
        /// dispose terminated project
        /// </term>
        /// <description>
        /// when debugged process stop running, this task cleans all its resources
        /// and terminates its job.
        /// </description>
        /// </item>
        /// </list>
        /// </summary>
        /// <param name="userInterface">
        /// reference to user interface object, which implements interface
        /// IAnglrServerSideDebuggerInvoker
        /// </param>
        /// <returns></returns>
        public async Task DebuggedProcessCtrlAsync (IAnglrServerSideDebuggerInvoker userInterface)
        {
            using (process = new Process ())
            {
                ProcessStartInfo info = new ProcessStartInfo ();
                info.FileName = FileName;
                info.Arguments = Arguments;
                info.WorkingDirectory = WorkingDirectory;
                info.RedirectStandardOutput = (OutFileName != null);
                info.RedirectStandardError = (ErrFileName != null);
                info.UseShellExecute = false;
                info.CreateNoWindow = true;

                process.StartInfo = info;

                if (process.Start ())
                {
                    // prepare cancellation token
                    CancellationTokenSource cancellationToken = new CancellationTokenSource ();

                    // prepare redirection sources for standard output and standard error
                    Task outTask = (OutFileName != null) ? process.StandardOutput.BaseStream.CopyToAsync (new FileStream (OutFileName, FileMode.OpenOrCreate)) : null;
                    Task errTask = (ErrFileName != null) ? process.StandardError.BaseStream.CopyToAsync (new FileStream (ErrFileName, FileMode.OpenOrCreate)) : null;
                    List<Task> tasks = new List<Task> ();
                    if (outTask != null)
                        tasks.Add (outTask);
                    if (errTask != null)
                        tasks.Add (errTask);

                    IsRunning = true;
                    userInterface?.Logger?.InfoLine ($"<AnglrDebuggerServerBridge>: process'{FileName}' started");

                    // manage all syntax tree debuggers in a detached task
                    _ = DebuggedPipeCtrlAsync (userInterface, cancellationToken.Token, process.Id);

                    // await draining tasks for all established redirections
                    userInterface?.Logger?.DebugLine ($"<AnglrDebuggerServerBridge>: wait for draining tasks");
                    await Task.WhenAll (tasks);
                    outTask?.Dispose ();
                    errTask?.Dispose ();

                    // await process termination
                    userInterface?.Logger?.InfoLine ($"<AnglrDebuggerServerBridge>: process'{FileName}' wait until terminated");
                    await process.WaitForExitAsync (cancellationToken.Token);
                    process.Dispose ();
                    userInterface?.Logger?.InfoLine ($"<AnglrDebuggerServerBridge>: process'{FileName}' terminated");

                    // cancel  all debugger tasks which are eventually stil running
                    cancellationToken.Cancel ();

                    IsRunning = false;
                }
                else
                    userInterface?.Logger?.WarnLine ($"<AnglrDebuggerServerBridge>: Program {info.FileName} not started");
            }
        }

        private int pipeCount = 0;

        private async Task DebuggedPipeCtrlAsync (IAnglrServerSideDebuggerInvoker userInterface, CancellationToken cancellationToken, int pid)
        {
            while (true)
            {
                ++pipeCount;
                // for every syntax tree being debugged create named pipe server stream distinguished by process id of process being debugged
                NamedPipeServerStream AnglrDebuggerServerStream = new NamedPipeServerStream ($"anglr-debugger-{pid}", PipeDirection.InOut, NamedPipeServerStream.MaxAllowedServerInstances, PipeTransmissionMode.Message, PipeOptions.Asynchronous);
                userInterface?.Logger?.InfoLine ($"<AnglrDebuggerServerBridge>: server pipe {pipeCount} 'anglr-debugger-{pid}' created");

                try
                {
                    // wait for connection request. For every parsing task being executed by debugged process there should be one connection request
                    await AnglrDebuggerServerStream.WaitForConnectionAsync (cancellationToken);
                    userInterface?.Logger?.InfoLine ($"<AnglrDebuggerServerBridge>: client pipe {pipeCount} connected");

                    // debug parsing with detached task
                    userInterface.InvokeRpcSession (pipeCount, AnglrDebuggerServerStream, cancellationToken);
                }
                catch (Exception e)
                {
                    LogException (userInterface, e);
                    break;
                }
            }
        }
    }

    public class AnglrDebuggerServerBridgeSet : ObservableCollection<AnglrDebuggerServerBridge> { }
}
