using AnglrBreakPointDBLibrary;
using AnglrDebuggerBridge;
using AnglrDebuggerJsonRpcMessages;
using AnglrJsonRpcMethods;
using AnglrLogLibrary;
using Microsoft.VisualStudio.Shell;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using StreamJsonRpc;
using StreamJsonRpc.Protocol;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.ComponentModel;
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

namespace AnglrLangExtension
{
    public class AnglrLRStackViewSet : Dictionary<int, AnglrDebuggerStackView> { }

    /// <summary>
    /// Interaction logic for AnglrDebugPanelTabSession.xaml
    /// </summary>
    public partial class AnglrDebugPanelTabSession : UserControl, IAnglrClientSideDebugger
    {
        public int MagicNumber { get; private set; }
        public JsonRpc Rpc { get; set; }
        public ObservableCollection<AnglrDebuggerStackView> LRStackViewCollection { get; set; }

        public IAnglrLogger Logger { get; private set; }

        private IAnglrLangService anglrLangService;
        private string fileName;
        private AnglrDebuggerClientBridge anglrDebuggerServerBridge;

        private AnglrLRStackViewSet lRStackViewSet;
        private (AnglrLangItem, AnglrStateItem, AnglrGetParserSyntaxRulesResult, AnglrDrawingDictionary) anglrInfo;
        private bool showDebuggerText;

        public AnglrDebugPanelTabSession (IAnglrLangService anglrLangService)
        {
            InitializeComponent ();

            LRStackViewCollection = new ObservableCollection<AnglrDebuggerStackView> ();
            lRStackViewSet = new AnglrLRStackViewSet ();

            this.anglrLangService = anglrLangService;
            Logger = anglrLangService?.AnglrLogger ?? new VoidAnglrLogger ();
            anglrInfo = default;
            showDebuggerText = true;

            Logger?.InfoLine ($"AnglrDebugPanelSession created ");
        }

        public async Task InvokeRpcSessionAsync (int count, Stream pipe, CancellationToken token)
        {
            Logger?.InfoLine ($"<AnglrDebuggerClientBridge>: rpc channel {count} trying to attach");
            Rpc = JsonRpc.Attach (pipe, pipe, new AnglrClientSideDebuggerJsonRpcMessagesHandler (this));
            Logger?.InfoLine ($"<AnglrDebuggerClientBridge>: rpc channel {count} created");
            Rpc.Disconnected += Rpc_Disconnected;
            await Rpc.Completion;
            Logger?.InfoLine ($"<AnglrDebuggerClientBridge>: rpc channel {count} completed");
            Rpc.Dispose ();
            Logger?.InfoLine ($"<AnglrDebuggerClientBridge>: rpc channel {count} disposed");
            Rpc = null;
        }

        private void Rpc_Disconnected (object sender, JsonRpcDisconnectedEventArgs e)
        {
            Logger?.InfoLine ($"<AnglrDebuggerClientBridge>: RPC disconnected, reason: {e.Reason}");
        }

        public void LogMessageHandler (object sender, EventArgs e)
        {
            try
            {
                AnglrDebuggerLogRequest logMessageRequest = e as AnglrDebuggerLogRequest;
                if (logMessageRequest == null)
                    return;

                Logger?.Log ((AnglrLogLevel) logMessageRequest.LogLevel, logMessageRequest.Message);
            }
            catch (Exception ex)
            {
            }
        }

        public AnglrDebuggerConnectResponse ConnectMessageHandler (object sender, EventArgs e)
        {
            try
            {
                AnglrDebuggerConnectRequest connectMessageRequest = e as AnglrDebuggerConnectRequest;
                if (connectMessageRequest == null)
                {
                    Logger?.DebugLine ($"connect (null request)");
                    return null;
                }
                Logger?.InfoLine ($"connect ({connectMessageRequest.SequenceNr})");
                object [] info = connectMessageRequest.Info;
                if (info != null)
                {
                    foreach (object item in info)
                        if (item != null)
                            Logger?.DebugLine ($"connect info: {item as string}");
                }

                MagicNumber = connectMessageRequest.MagicNumber.HasValue ? connectMessageRequest.MagicNumber.Value : -1;
                anglrInfo = AnglrLangDictionary.GetItem (MagicNumber);
                AnglrBreakPointDBChunk chunk = null;
                if (!AnglrBreakPointDB.Get (MagicNumber, out chunk))
                    chunk = new AnglrBreakPointDBChunk ();
                chunk.Changed = false;
                Logger.InfoLine ($"connect request: magic nr. = {MagicNumber}, db chunk = {JsonConvert.SerializeObject (chunk)}");

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

        public void SyntaxErrorMessageHandler (object sender, EventArgs e)
        {
            AnglrDebuggerSyntaxErrorRequest syntaxErrorMessageRequest = e as AnglrDebuggerSyntaxErrorRequest;
            if (syntaxErrorMessageRequest == null)
                return;
            Logger?.InfoLine ($"syntax error ({syntaxErrorMessageRequest.SequenceNr})");
        }

        public void ShiftStepMessageHandler (object sender, EventArgs e)
        {
            AnglrDebuggerShiftStepRequest shiftStepMessageRequest = e as AnglrDebuggerShiftStepRequest;
            if (shiftStepMessageRequest == null)
                return;
            Logger?.InfoLine ($"shift ({shiftStepMessageRequest.SequenceNr})");
        }

        public void ReduceStepMessageHandler (object sender, EventArgs e)
        {
            AnglrDebuggerReduceStepRequest reduceStepMessageRequest = e as AnglrDebuggerReduceStepRequest;
            if (reduceStepMessageRequest == null)
                return;
            Logger?.InfoLine ($"reduce ({reduceStepMessageRequest.SequenceNr})");
        }

        public void SplitStepMessageHandler (object sender, EventArgs e)
        {
            AnglrDebuggerSplitStepRequest splitStepMessageRequest = e as AnglrDebuggerSplitStepRequest;
            if (splitStepMessageRequest == null)
                return;
            Logger?.InfoLine ($"split ({splitStepMessageRequest.OldStackNr})");
        }

        public void LoopStepMessageHandler (object sender, EventArgs e)
        {
            AnglrDebuggerLoopStepRequest loopStepMessageRequest = e as AnglrDebuggerLoopStepRequest;
            if (loopStepMessageRequest == null)
                return;
            Logger?.InfoLine ($"loop ({loopStepMessageRequest.SequenceNr})");
        }

        public void JoinMessageHandler (object sender, EventArgs e)
        {
            AnglrDebuggerJoinRequest joinMessageRequest = e as AnglrDebuggerJoinRequest;
            if (joinMessageRequest == null)
                return;
            Logger?.InfoLine ($"join ({joinMessageRequest.SequenceNr})");
        }

        public void FinalStepMessageHandler (object sender, EventArgs e)
        {
            AnglrDebuggerFinalStepRequest finalStepMessageRequest = e as AnglrDebuggerFinalStepRequest;
            if (finalStepMessageRequest == null)
                return;
            Logger?.InfoLine ($"final ({finalStepMessageRequest.SequenceNr})");
        }

        public void StopParserMessageHandler (object sender, EventArgs e)
        {
            AnglrDebuggerStopParserRequest stopParserMessageRequest = e as AnglrDebuggerStopParserRequest;
            if (stopParserMessageRequest == null)
                return;
            Logger?.InfoLine ($"stop ({stopParserMessageRequest.SequenceNr})");
        }

        public void DbgBreakPointHitMessageHandler (object sender, EventArgs e)
        {
            AnglrDebuggerDbgBreakPointHitRequest dbgBreakPointHitRequest =e as AnglrDebuggerDbgBreakPointHitRequest;
            if (dbgBreakPointHitRequest == null)
            {
                Logger?.InfoLine ($"break-point hit (null request)");
                return;
            }

            AnglrDebuggerGetPDASnapshotResponse getPDASnapshotResponse =
            Rpc.InvokeAsync<AnglrDebuggerGetPDASnapshotResponse>
            (
                AnglrDebuggerJsonRpcMessageNames.GetPDASnapshotMessageName,
                new AnglrDebuggerGetPDASnapshotRequest ()
                {
                    SequenceNr=dbgBreakPointHitRequest.SequenceNr
                }
            ).Result;
            if (getPDASnapshotResponse == null)
            {
                Logger?.InfoLine ($"break-point hit (null snapshot)");
                return;
            }

            Logger?.InfoLine ($"break-point hit ({dbgBreakPointHitRequest.SequenceNr})");
            foreach (var stack in getPDASnapshotResponse.PDAStackSet)
            {
                Logger?.InfoLine ($"stack ({stack.PDAStackId})");
                foreach (var cell in stack.PDAStackCells)
                    Logger?.InfoLine ($"\t{cell.Id} {cell.State} {cell.Name}");
            }
        }

        async Task InvokeRpcSessionAsync (int count, Stream pipe, CancellationToken token, object msgFormatter)
        {
            Rpc = JsonRpc.Attach (pipe, pipe, msgFormatter);
            Logger?.InfoLine ($"<AnglrDebuggerClientBridge>: rpc channel {count} created");
            Rpc.Disconnected += Rpc_Disconnected;
            await Rpc.Completion;
            Logger?.InfoLine ($"<AnglrDebuggerClientBridge>: rpc channel {count} completed");
            Rpc.Dispose ();
            Logger?.InfoLine ($"<AnglrDebuggerClientBridge>: rpc channel {count} disposed");
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
