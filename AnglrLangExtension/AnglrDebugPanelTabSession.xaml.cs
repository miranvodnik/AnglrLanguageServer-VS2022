using AnglrDebuggerBridge;
using AnglrDebuggerJsonRpcMessages;
using AnglrLogLibrary;
using AnglrJsonRpcMethods;
using AnglrBreakPointDBLibrary;
using StreamJsonRpc;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.IO.Pipelines;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using System.ComponentModel;

namespace AnglrLangExtension
{
    public class AnglrLRStackViewSet : Dictionary<int, AnglrDebuggerStackView> { }

    public class UpdateMessageQueue : List<(Action<object, EventArgs>, object, EventArgs)>
    {
        public AnglrDebugPanelTabSession anglrDebugPanelTab;
        public UpdateMessageQueue (AnglrDebugPanelTabSession anglrDebugPanelTab)
        {
            this.anglrDebugPanelTab = anglrDebugPanelTab;
        }

        public void Display ()
        {
            foreach (var item in this)
            {
                item.Item1 (item.Item2, item.Item3);
            }
        }
    }

    /// <summary>
    /// Interaction logic for AnglrDebugPanelTabSession.xaml
    /// </summary>
    public partial class AnglrDebugPanelTabSession : UserControl, IAnglrServerSideDebugger
    {
        public int MagicNumber { get; private set; }
        public JsonRpc Rpc { get; set; }
        public ObservableCollection<AnglrDebuggerStackView> LRStackViewCollection { get; set; }

        public IAnglrLogger Logger { get; private set; }

        private IAnglrLangService anglrLangService;
        private string fileName;
        private AnglrDebuggerServerBridge anglrDebuggerServerBridge;

        private AnglrLRStackViewSet lRStackViewSet;
        private (AnglrLangItem, AnglrStateItem, AnglrGetParserSyntaxRulesResult) anglrInfo;
        private bool showDebuggerText;

        private UpdateMessageQueue updateMessageQueue;
        private AutoResetEvent updateViewEvent;
        private object updateQueueLock;
        private CancellationTokenSource updateCancellationTokenSource;
        private CancellationToken updateCancellationToken;

        public AnglrDebugPanelTabSession (IAnglrLangService anglrLangService)
        {
            InitializeComponent ();

            this.DataContext = this;

            LRStackViewCollection = new ObservableCollection<AnglrDebuggerStackView> ();
            lRStackViewSet = new AnglrLRStackViewSet ();

            this.anglrLangService = anglrLangService;
            Logger = anglrLangService?.AnglrLogger ?? new VoidAnglrLogger ();
            anglrInfo = default;
            showDebuggerText = true;

            updateMessageQueue = new UpdateMessageQueue (this);
            updateViewEvent = new AutoResetEvent (false);
            updateQueueLock = new object ();
            updateCancellationTokenSource = new CancellationTokenSource ();
            updateCancellationToken = updateCancellationTokenSource.Token;

            _ = Task.Factory.StartNew
            (
                () =>
                {
                    Logger?.InfoLine ($"AnglrDebugPanelSession update view task started");
                    while (true)
                    {
                        if (updateCancellationToken.IsCancellationRequested)
                            break;
                        updateViewEvent.WaitOne ();
                        UpdateMessageQueue mq = null;
                        lock (updateQueueLock)
                        {
                            mq = updateMessageQueue;
                            updateMessageQueue = new UpdateMessageQueue (this);
                        }
                        Dispatcher.Invoke (mq.Display);
                    }
                    Logger?.InfoLine ($"AnglrDebugPanelSession update view task stopped");
                },
                updateCancellationToken,
                TaskCreationOptions.LongRunning,
                TaskScheduler.Default
            );

            Logger?.InfoLine ($"AnglrDebugPanelSession created ");
        }

        public async Task InvokeRpcSessionAsync (int count, Stream pipe, CancellationToken token)
        {
            Logger?.InfoLine ($"<AnglrDebuggerServerBridge>: rpc channel {count} trying to attach");
            Rpc = JsonRpc.Attach (pipe, pipe, new AnglrServerSideDebuggerJsonRpcMessagesHandler (this));
            Logger?.InfoLine ($"<AnglrDebuggerServerBridge>: rpc channel {count} created");
            Rpc.Disconnected += Rpc_Disconnected;
            await Rpc.Completion;
            Logger?.InfoLine ($"<AnglrDebuggerServerBridge>: rpc channel {count} completed");
            Rpc.Dispose ();
            Logger?.InfoLine ($"<AnglrDebuggerServerBridge>: rpc channel {count} disposed");
            Rpc = null;
        }

        private void Rpc_Disconnected (object sender, JsonRpcDisconnectedEventArgs e)
        {
            Logger?.InfoLine ($"<AnglrDebuggerServerBridge>: RPC disconnected, reason: {e.Reason}");
        }

        public async Task UpdateDebugView (Action<object, EventArgs> dlg, object sender, EventArgs e)
        {
            lock (updateQueueLock)
            {
                updateMessageQueue.Add ((dlg, sender, e));
                updateViewEvent.Set ();
            }
        }

        public AnglrDebuggerLogResponse LogMessageHandler (object sender, EventArgs e)
        {
            try
            {
                AnglrDebuggerLogRequest logMessageRequest = e as AnglrDebuggerLogRequest;
                if (logMessageRequest == null)
                    return null;

                Logger?.Log ((AnglrLogLevel) logMessageRequest.LogLevel, logMessageRequest.Message);
                return new AnglrDebuggerLogResponse ()
                {
                    SequenceNr = logMessageRequest.SequenceNr
                };
            }
            catch (Exception ex)
            {
                return null;
            }
        }

        public AnglrDebuggerConnectResponse ConnectMessageHandler (object sender, EventArgs e)
        {
            try
            {
                UpdateDebugView (ConnectMessageInvoker, sender, e);
                AnglrDebuggerConnectRequest connectMessageRequest = e as AnglrDebuggerConnectRequest;
                if (connectMessageRequest == null)
                {
                    Logger?.DebugLine ($"connect (null request)");
                    return null;
                }

                MagicNumber = connectMessageRequest.MagicNumber.HasValue ? connectMessageRequest.MagicNumber.Value : -1;
                anglrInfo = AnglrLangDictionary.GetItem (MagicNumber);
                AnglrBreakPointDBChunk chunk = null;
                if (!AnglrBreakPointDB.Get (MagicNumber, out chunk))
                    chunk = new AnglrBreakPointDBChunk ();
                chunk.Changed = false;
                Logger.InfoRawLine ($"connect request: magic nr. = {MagicNumber}, db chunk = {JsonConvert.SerializeObject (chunk)}");

                return new AnglrDebuggerConnectResponse ()
                {
                    SequenceNr = connectMessageRequest.SequenceNr,
                    Valid = (MagicNumber != -1),
                    BreakPointDB = JsonConvert.SerializeObject (chunk)
                };
            }
            catch (Exception ex)
            {
                Logger?.ErrorLine (ex, $"ConnectMessageHandler exception");
                return null;
            }
        }

        public void ConnectMessageInvoker (object sender, EventArgs e)
        {
            AnglrDebuggerConnectRequest connectMessageRequest = e as AnglrDebuggerConnectRequest;
            if (connectMessageRequest == null)
                return;
            if (showDebuggerText)
                debuggerList.Text = $"connect ({connectMessageRequest.StackNr})";
            Logger?.DebugLine ($"connect ({connectMessageRequest.StackNr})");
            object [] info = connectMessageRequest.Info;
            if (info != null)
            {
                foreach (object item in info)
                    if (item != null)
                        Logger?.DebugLine ($"connect info: {item as string}");
            }

            try
            {
                lRStackViewSet.Clear ();
                if (showDebuggerText)
                    LRStackViewCollection?.Clear ();
                AnglrDebuggerStackView lrStack = new AnglrDebuggerStackView ();
                lrStack.AnglrLangService = anglrLangService;
                lrStack.InitParserStack (null, connectMessageRequest.StackNr);
                lRStackViewSet [connectMessageRequest.StackNr] = lrStack;
                if (showDebuggerText)
                    LRStackViewCollection?.Add (lrStack);
            }
            catch (Exception ex)
            {
                Logger?.ErrorLine (ex, $"ConnectMessageInvoker exception");
            }
        }

        public AnglrDebuggerSyntaxErrorResponse SyntaxErrorMessageHandler (object sender, EventArgs e)
        {
            UpdateDebugView (SyntaxErrorMessageInvoker, sender, e);
            AnglrDebuggerSyntaxErrorRequest syntaxErrorMessageRequest = e as AnglrDebuggerSyntaxErrorRequest;
            if (syntaxErrorMessageRequest == null)
                return null;
            return new AnglrDebuggerSyntaxErrorResponse ()
            {
                SequenceNr = syntaxErrorMessageRequest.SequenceNr
            };
        }

        public void SyntaxErrorMessageInvoker (object sender, EventArgs e)
        {
            AnglrDebuggerSyntaxErrorRequest syntaxErrorMessageRequest = e as AnglrDebuggerSyntaxErrorRequest;
            if (syntaxErrorMessageRequest == null)
                return;
            if (showDebuggerText)
                debuggerList.Text = $"syntax error ({syntaxErrorMessageRequest.StackNr}, {syntaxErrorMessageRequest.State})";
            try
            {
                if (showDebuggerText)
                    LRStackViewCollection?.Remove (lRStackViewSet [syntaxErrorMessageRequest.StackNr]);
                lRStackViewSet.Remove (syntaxErrorMessageRequest.StackNr);
            }
            catch (Exception ex)
            {
                Logger?.ErrorLine (ex, $"SyntaxErrorMessageHandler exception");
            }
        }

        public AnglrDebuggerShiftStepResponse ShiftStepMessageHandler (object sender, EventArgs e)
        {
            UpdateDebugView (ShiftStepMessageInvoker, sender, e);
            AnglrDebuggerShiftStepRequest shiftStepMessageRequest = e as AnglrDebuggerShiftStepRequest;
            if (shiftStepMessageRequest == null)
                return null;
            return new AnglrDebuggerShiftStepResponse ()
            {
                SequenceNr = shiftStepMessageRequest.SequenceNr
            };
        }

        public void ShiftStepMessageInvoker (object sender, EventArgs e)
        {
            AnglrDebuggerShiftStepRequest shiftStepMessageRequest = e as AnglrDebuggerShiftStepRequest;
            if (shiftStepMessageRequest == null)
                return;
            if (showDebuggerText)
                debuggerList.Text = $"shift ({shiftStepMessageRequest.StackNr}, {shiftStepMessageRequest.State}, {shiftStepMessageRequest.TokenValue}, {shiftStepMessageRequest.TokenText})";
            try
            {
                lRStackViewSet [shiftStepMessageRequest.StackNr].ParserStack.Push
                (
                    new AnglrDebuggerStackElement ()
                    {
                        Id = shiftStepMessageRequest.TokenValue,
                        IsTerminal = true,
                        State = shiftStepMessageRequest.State,
                        Name = shiftStepMessageRequest.TokenName,
                        Value = shiftStepMessageRequest.TokenText
                    }
                );
            }
            catch (Exception ex)
            {
                Logger?.ErrorLine (ex, $"ShiftStepMessageInvoker exception");
            }
        }

        public AnglrDebuggerReduceStepResponse ReduceStepMessageHandler (object sender, EventArgs e)
        {
            UpdateDebugView (ReduceStepMessageInvoker, sender, e);
            AnglrDebuggerReduceStepRequest reduceStepMessageRequest = e as AnglrDebuggerReduceStepRequest;
            if (reduceStepMessageRequest == null)
                return null;
            return new AnglrDebuggerReduceStepResponse ()
            {
                SequenceNr = reduceStepMessageRequest.SequenceNr
            };
        }

        public void ReduceStepMessageInvoker (object sender, EventArgs e)
        {
            AnglrDebuggerReduceStepRequest reduceStepMessageRequest = e as AnglrDebuggerReduceStepRequest;
            if (reduceStepMessageRequest == null)
                return;
            if (showDebuggerText)
                debuggerList.Text = $"reduce ({reduceStepMessageRequest.StackNr}, {lRStackViewSet [reduceStepMessageRequest.StackNr].ParserStack.Count}, {reduceStepMessageRequest.ProdNr}, {reduceStepMessageRequest.RuleNr}, {reduceStepMessageRequest.ProdLen}, {reduceStepMessageRequest.FallingState}, {reduceStepMessageRequest.BottomState}, {reduceStepMessageRequest.RisingState})";
            try
            {
                lRStackViewSet [reduceStepMessageRequest.StackNr].ReducedValue = "";
                lRStackViewSet [reduceStepMessageRequest.StackNr].ParserStack.Pop (reduceStepMessageRequest.ProdLen);
                lRStackViewSet [reduceStepMessageRequest.StackNr].ParserStack.Push
                (
                    new AnglrDebuggerStackElement ()
                    {
                        Id = reduceStepMessageRequest.RuleNr,
                        IsTerminal = false,
                        State = reduceStepMessageRequest.RisingState,
                        Name = reduceStepMessageRequest.RuleName,
                        Value = lRStackViewSet [reduceStepMessageRequest.StackNr].ReducedValue
                    }
                );
            }
            catch (Exception ex)
            {
                Logger?.ErrorLine ($"exception: {debuggerList.Text}");
                Logger?.ErrorLine (ex, $"ReduceStepMessageInvoker exception");
            }
        }

        public AnglrDebuggerSplitStepResponse SplitStepMessageHandler (object sender, EventArgs e)
        {
            UpdateDebugView (SplitStepMessageInvoker, sender, e);
            AnglrDebuggerSplitStepRequest splitStepMessageRequest = e as AnglrDebuggerSplitStepRequest;
            if (splitStepMessageRequest == null)
                return null;
            return new AnglrDebuggerSplitStepResponse ()
            {
                SequenceNr = splitStepMessageRequest.SequenceNr
            };
        }

        public void SplitStepMessageInvoker (object sender, EventArgs e)
        {
            AnglrDebuggerSplitStepRequest splitStepMessageRequest = e as AnglrDebuggerSplitStepRequest;
            if (splitStepMessageRequest == null)
                return;
            if (showDebuggerText)
                debuggerList.Text = $"split ({splitStepMessageRequest.OldStackNr}, {splitStepMessageRequest.NewStackNr})";
            try
            {
                if (splitStepMessageRequest.Begin)
                {
                    AnglrDebuggerStackView anglrDebuggerStackView = new AnglrDebuggerStackView ();
                    anglrDebuggerStackView.AnglrLangService = anglrLangService;
                    anglrDebuggerStackView.InitParserStack (lRStackViewSet [splitStepMessageRequest.OldStackNr].ParserStack, splitStepMessageRequest.NewStackNr);
                    lRStackViewSet [splitStepMessageRequest.NewStackNr] = anglrDebuggerStackView;
                    if (showDebuggerText)
                        LRStackViewCollection?.Add (anglrDebuggerStackView);
                }
                else
                {
                    if (showDebuggerText)
                        LRStackViewCollection?.Remove (lRStackViewSet [splitStepMessageRequest.OldStackNr]);
                    lRStackViewSet.Remove (splitStepMessageRequest.OldStackNr);
                }
            }
            catch (Exception ex)
            {
                Logger?.ErrorLine (ex, $"SplitStepMessageInvoker exception");
            }
        }

        public AnglrDebuggerLoopStepResponse LoopStepMessageHandler (object sender, EventArgs e)
        {
            UpdateDebugView (LoopStepMessageInvoker, sender, e);
            AnglrDebuggerLoopStepRequest loopStepMessageRequest = e as AnglrDebuggerLoopStepRequest;
            if (loopStepMessageRequest == null)
                return null;
            return new AnglrDebuggerLoopStepResponse ()
            {
                SequenceNr = loopStepMessageRequest.SequenceNr
            };
        }

        public void LoopStepMessageInvoker (object sender, EventArgs e)
        {
            AnglrDebuggerLoopStepRequest loopStepMessageRequest = e as AnglrDebuggerLoopStepRequest;
            if (loopStepMessageRequest == null)
                return;
            if (showDebuggerText)
                debuggerList.Text = $"loop ({loopStepMessageRequest.StackNr}, {loopStepMessageRequest.State})";
            try
            {
                if (showDebuggerText)
                    LRStackViewCollection?.Remove (lRStackViewSet [loopStepMessageRequest.StackNr]);
                lRStackViewSet.Remove (loopStepMessageRequest.StackNr);
            }
            catch (Exception ex)
            {
                Logger?.ErrorLine (ex, $"LoopStepMessageInvoker exception");
            }
        }

        public AnglrDebuggerJoinResponse JoinMessageHandler (object sender, EventArgs e)
        {
            UpdateDebugView (JoinMessageInvoker, sender, e);
            AnglrDebuggerJoinRequest joinMessageRequest = e as AnglrDebuggerJoinRequest;
            if (joinMessageRequest == null)
                return null;
            return new AnglrDebuggerJoinResponse ()
            {
                SequenceNr = joinMessageRequest.SequenceNr
            };
        }

        public void JoinMessageInvoker (object sender, EventArgs e)
        {
            AnglrDebuggerJoinRequest joinMessageRequest = e as AnglrDebuggerJoinRequest;
            if (joinMessageRequest == null)
                return;

            if (showDebuggerText)
                debuggerList.Text = $"join ({joinMessageRequest.StackNr}, {joinMessageRequest.JoinNr})";
            try
            {
                if (showDebuggerText)
                    LRStackViewCollection?.Remove (lRStackViewSet [joinMessageRequest.StackNr]);
                lRStackViewSet.Remove (joinMessageRequest.StackNr);
            }
            catch (Exception ex)
            {
                Logger?.ErrorLine (ex, $"JoinMessageInvoker exception");
            }
        }

        public AnglrDebuggerFinalStepResponse FinalStepMessageHandler (object sender, EventArgs e)
        {
            UpdateDebugView (FinalStepMessageInvoker, sender, e);
            AnglrDebuggerFinalStepRequest finalStepMessageRequest = e as AnglrDebuggerFinalStepRequest;
            if (finalStepMessageRequest == null)
                return null;
            return new AnglrDebuggerFinalStepResponse ()
            {
                SequenceNr = finalStepMessageRequest.SequenceNr
            };
        }

        public void FinalStepMessageInvoker (object sender, EventArgs e)
        {
            try
            {
                AnglrDebuggerFinalStepRequest finalStepMessageRequest = e as AnglrDebuggerFinalStepRequest;
                if (finalStepMessageRequest == null)
                    return;
                if (showDebuggerText)
                    debuggerList.Text = $"final ({finalStepMessageRequest.StackNr})";
            }
            finally
            {
                updateCancellationTokenSource.Cancel ();
            }
        }

        public AnglrDebuggerStopParserResponse StopParserMessageHandler (object sender, EventArgs e)
        {
            UpdateDebugView (StopParserMessageInvoker, sender, e);
            AnglrDebuggerStopParserRequest stopParserMessageRequest = e as AnglrDebuggerStopParserRequest;
            if (stopParserMessageRequest == null)
                return null;
            return new AnglrDebuggerStopParserResponse ()
            {
                SequenceNr = stopParserMessageRequest.SequenceNr
            };
        }

        public void StopParserMessageInvoker (object sender, EventArgs e)
        {
            AnglrDebuggerStopParserRequest stopParserMessageRequest = e as AnglrDebuggerStopParserRequest;
            if (stopParserMessageRequest == null)
                return;
            if (showDebuggerText)
                debuggerList.Text = $"stop ()";
            foreach (KeyValuePair<int, AnglrDebuggerStackView> l in lRStackViewSet)
            {
                Logger?.DebugLine ($"stop ({l.Key}, {l.Value.ParserStack.Count})");
            }
        }

        async Task InvokeRpcSessionAsync (int count, Stream pipe, CancellationToken token, object msgFormatter)
        {
            Rpc = JsonRpc.Attach (pipe, pipe, msgFormatter);
            Logger?.InfoLine ($"<AnglrDebuggerServerBridge>: rpc channel {count} created");
            Rpc.Disconnected += Rpc_Disconnected;
            await Rpc.Completion;
            Logger?.InfoLine ($"<AnglrDebuggerServerBridge>: rpc channel {count} completed");
            Rpc.Dispose ();
            Logger?.InfoLine ($"<AnglrDebuggerServerBridge>: rpc channel {count} disposed");
            Rpc = null;
        }

        private void breakButton_Click (object sender, RoutedEventArgs e)
        {
            if (Rpc != null)
                try
                {
                    Logger?.DebugLine ($"Break Button activated");
                    _ = Rpc.NotifyAsync
                    (
                        AnglrDebuggerJsonRpcMessageNames.DbgBreakMessageName,
                        new AnglrDebuggerDbgBreakRequest ()
                    );
                }
                catch (Exception ex)
                {
                    Logger?.ErrorLine (ex, $"Break Button exception");
                }
            else
                Logger?.ErrorLine ($"Break Button: no RPC");
        }

        private void continueButton_Click (object sender, RoutedEventArgs e)
        {
            if (Rpc != null)
                try
                {
                    Logger?.DebugLine ($"Continue Button activated");
                    AnglrBreakPointDBChunk chunk = null;
                    if (!AnglrBreakPointDB.Get (MagicNumber, out chunk))
                        chunk = new AnglrBreakPointDBChunk ()
                        {
                            Changed = true
                        };
                    _ = Rpc.InvokeAsync
                    (
                        AnglrDebuggerJsonRpcMessageNames.DbgContinueMessageName,
                        new AnglrDebuggerDbgContinueRequest ()
                        {
                            SequenceNr = 0,
                            BreakPointDB = chunk.Changed ? JsonConvert.SerializeObject (chunk) : null
                        }
                    );
                    chunk.Changed = false;
                }
                catch (Exception ex)
                {
                    Logger?.ErrorLine (ex, $"Continue Button exception");
                }
            else
                Logger?.ErrorLine ($"Continue Button: no RPC");
        }

        private void singleStepButton_Click (object sender, RoutedEventArgs e)
        {
            if (Rpc != null)
                try
                {
                    Logger?.DebugLine ($"Single Step Button activated");
                    AnglrBreakPointDBChunk chunk = null;
                    if (!AnglrBreakPointDB.Get (MagicNumber, out chunk))
                        chunk = new AnglrBreakPointDBChunk ()
                        {
                            Changed = true
                        };
                    _ = Rpc.InvokeAsync
                    (
                        AnglrDebuggerJsonRpcMessageNames.DbgSingleStepMessageName,
                        new AnglrDebuggerDbgSingleStepRequest ()
                        {
                            SequenceNr = 0,
                            BreakPointDB = chunk.Changed ? JsonConvert.SerializeObject (chunk) : null
                        }
                    );
                    chunk.Changed = false;
                }
                catch (Exception ex)
                {
                    Logger?.ErrorLine (ex, $"Single Step exception");
                }
            else
                Logger?.ErrorLine ($"Single Step Button: no RPC");
        }
    }
}
