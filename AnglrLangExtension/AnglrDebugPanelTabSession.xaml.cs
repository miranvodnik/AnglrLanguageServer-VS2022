using Anglr.Parser;
using Anglr.Parser.Core;
using Anglr.Parser.SyntaxTree;
using AnglrBreakPointDBLibrary;
using AnglrDebuggerBridge;
using AnglrDebuggerJsonRpcMessages;
using AnglrJsonRpcMethods;
using AnglrLibrary;
using AnglrLogLibrary;
using Microsoft.VisualStudio.GraphModel.CodeSchema;
using Microsoft.VisualStudio.LanguageServer.Protocol;
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
using System.Diagnostics.Metrics;
using System.IO;
using System.IO.Pipelines;
using System.Linq;
using System.Reflection;
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
    public class AnglrPDASetElementComparer : IComparer<AnglrGetParserStateTransitionPointData>
    {
        public int Compare (AnglrGetParserStateTransitionPointData x, AnglrGetParserStateTransitionPointData y)
        {
            if (x.Production.ProductionNumber != y.Production.ProductionNumber)
                return x.Production.ProductionNumber - y.Production.ProductionNumber;
            return x.Position - y.Position;
        }
    }

    public class AnglrPDASet : SortedSet<AnglrGetParserStateTransitionPointData>
    {
        public AnglrPDASet () : base (new AnglrPDASetElementComparer ()) { }
        public AnglrPDASet (IEnumerable<AnglrGetParserStateTransitionPointData> anglrPDASetElements) :
            base (anglrPDASetElements, new AnglrPDASetElementComparer ())
        { }
    }

    public class AnglrPDAStateItemComparer : IEqualityComparer<AnglrGetParserStateItemResult>
    {
        public bool Equals (AnglrGetParserStateItemResult x, AnglrGetParserStateItemResult y) => x.StateNumber == y.StateNumber;

        public int GetHashCode (AnglrGetParserStateItemResult obj) => obj.StateNumber;
    }

    public class AnglrPDAViableSet : Dictionary<AnglrGetParserStateItemResult, AnglrPDASet>
    {
        public AnglrPDAViableSet () : base (new AnglrPDAStateItemComparer ()) { }
    }

    public class AnglrLRStackViewSet : Dictionary<int, AnglrDebuggerStackView> { }

    /// <summary>
    /// Interaction logic for AnglrDebugPanelTabSession.xaml
    /// </summary>
    public partial class AnglrDebugPanelTabSession : UserControl, IAnglrClientSideDebugger
    {
        public int MagicNumber { get; private set; }
        public AnglrLangDictionaryItem DictionaryItem { get; private set; }
        public JsonRpc Rpc { get; set; }
        public ObservableCollection<AnglrDebuggerStackView> LRStackViewCollection { get; set; }

        public IAnglrLogger Logger { get; private set; }

        private IAnglrLangService anglrLangService;
        private string fileName;
        private AnglrDebuggerClientBridge anglrDebuggerServerBridge;

        private AnglrLRStackViewSet lRStackViewSet;
        private bool showDebuggerText;

        public AnglrDebugPanelTabSession (IAnglrLangService anglrLangService)
        {
            InitializeComponent ();

            LRStackViewCollection = new ObservableCollection<AnglrDebuggerStackView> ();
            lRStackViewSet = new AnglrLRStackViewSet ();

            this.anglrLangService = anglrLangService;
            Logger = anglrLangService?.AnglrLogger ?? new VoidAnglrLogger ();
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
                AnglrBreakPointDBChunk chunk = null;
                if (!AnglrBreakPointDB.Get (MagicNumber, out chunk))
                    chunk = new AnglrBreakPointDBChunk ();
                chunk.Changed = false;
                if ((DictionaryItem = AnglrLangDictionary.GetItem (MagicNumber)) == null)
                    Logger?.WarnLine ($"Anglr file mismatch. Please load anglr file used to create debugged process");
                Logger.InfoLine ($"connect request: magic nr. = {MagicNumber}, db chunk = {JsonConvert.SerializeObject (chunk)}");

                return new AnglrDebuggerConnectResponse ()
                {
                    SequenceNr = connectMessageRequest.SequenceNr,
                    Valid = (DictionaryItem != null),
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
            AnglrDebuggerDbgBreakPointHitRequest dbgBreakPointHitRequest = e as AnglrDebuggerDbgBreakPointHitRequest;
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
                    SequenceNr = dbgBreakPointHitRequest.SequenceNr
                }
            ).Result;
            if (getPDASnapshotResponse == null)
            {
                Logger?.InfoLine ($"break-point hit (null snapshot)");
                return;
            }

            JsonSerializerSettings settings = new JsonSerializerSettings
            {
                TypeNameHandling = TypeNameHandling.Objects,
                MaxDepth = null
            };
            Logger?.InfoLine ($"break-point hit ({dbgBreakPointHitRequest.SequenceNr})");
            foreach (var stack in getPDASnapshotResponse.PDAStackSet)
            {
                Logger?.InfoLine ($"stack ({stack.PDAStackId})");
                Logger?.InfoLine ($"state\tcode\tname\t\tvalue");
                foreach (var cell in stack.PDAStackCells)
                {
                    object anglrFileFragment = JsonConvert.DeserializeObject (cell.Tree, settings);
                    Logger?.InfoLine ($"\t{cell.State}\t{cell.Code}\t\t{cell.Name}\t\t{cell.Value}\t\t{cell.Tree}");
                }
                AnglrPDAViableSet pdaSetList = AnalyzePDAStack (stack);
                foreach (var pda in pdaSetList)
                    DisplayAnglrPDASet (pda.Key, pda.Value);
            }
        }

        private AnglrPDASet LoadAnglrPDASet (AnglrGetParserStateItemResult anglrGetParserStateItemResult, int token)
        {
            AnglrPDASet anglrPDASet = new AnglrPDASet ();
            List<AnglrGetParserStateTransitionPointData> elementList = new List<AnglrGetParserStateTransitionPointData> ();
            int step = 0;
            int changes;
            while (true)
            {
                changes = 0;
                foreach (var coreData in anglrGetParserStateItemResult.CoreSet)
                {
                    var production = coreData.TransitionPoint.Production;
                    var position = coreData.TransitionPoint.Position;
                    Logger?.DebugLine ($"analyze core production {production.ProductionNumber}");
                    if (production.RhsNodeSet.Length <= position)
                    {
                        Logger?.DebugLine ($"\tlength of core production {production.ProductionNumber} <= {position}");
                        continue;
                    }
                    var rhsNode = production.RhsNodeSet [position];
                    if (rhsNode.Id != token)
                    {
                        Logger?.DebugLine ($"\tnode id of core production {production.ProductionNumber} != {token}");
                        continue;
                    }
                    AnglrGetParserStateTransitionPointData element = new AnglrGetParserStateTransitionPointData ()
                    {
                        Production = production,
                        Position = position
                    };
                    if (anglrPDASet.Contains (element))
                    {
                        Logger?.DebugLine ($"\tset contains core production {production.ProductionNumber}");
                        continue;
                    }
                    Logger?.DebugLine ($"\tadd core production {production.ProductionNumber}");
                    anglrPDASet.Add (element);
                    elementList.Add (element);
                    ++changes;
                }
                foreach (var closureData in anglrGetParserStateItemResult.ClosureSet)
                {
                    foreach (var productionInfo in closureData.ProductionNode.ProductionSet)
                    {
                        Logger?.DebugLine ($"analyze closure production {productionInfo.ProductionNumber}");
                        if (productionInfo.RhsNodeSet.Length <= 0)
                        {
                            Logger?.DebugLine ($"\tlength of closure production {productionInfo.ProductionNumber} <= {0}");
                            continue;
                        }
                        var rhsNode = productionInfo.RhsNodeSet [0];
                        if (rhsNode.Id != token)
                        {
                            Logger?.DebugLine ($"\tnode id of closure production {productionInfo.ProductionNumber} != {token}");
                            continue;
                        }
                        AnglrGetParserStateTransitionPointData element = new AnglrGetParserStateTransitionPointData ()
                        {
                            Production = productionInfo,
                            Position = 0
                        };
                        if (anglrPDASet.Contains (element))
                        {
                            Logger?.DebugLine ($"\tset contains closure production {productionInfo.ProductionNumber}");
                            continue;
                        }
                        Logger?.DebugLine ($"\tadd closure production {productionInfo.ProductionNumber}");
                        anglrPDASet.Add (element);
                        elementList.Add (element);
                        ++changes;
                    }
                }
                if (changes == 0)
                    break;
                while (step < elementList.Count)
                {
                    AnglrGetParserStateProductionData productionData = elementList [step].Production;
                    if (elementList [step++].Position > 0)
                        continue;
                    if (productionData.ProductionName.Id == token)
                        continue;
                    token = productionData.ProductionName.Id;
                    Logger?.DebugLine ($"\tselect new token ({token}, {productionData.ProductionName.Name}");
                    break;
                }
            }
            return anglrPDASet;
        }

        public void DisplayAnglrPDASet (AnglrGetParserStateItemResult pdaState, AnglrPDASet anglrPDASet)
        {
            Logger?.InfoLine ($"PDA CELL STATE {pdaState.StateNumber}");
            foreach (var element in anglrPDASet)
            {
                Logger?.InfoLine<AnglrGetParserStateTransitionPointData>
                (
                    (data) =>
                    {
                        StringBuilder sb = new StringBuilder ();
                        AnglrGetParserStateProductionData productionData = data.Production;
                        int index = 0;
                        int nodePosition = data.Position;
                        int productionNumber = productionData.ProductionNumber;
                        string productionName = productionData.ProductionName.Name;
                        sb.Append ($"{productionNumber} {productionName} :");
                        foreach (var node in productionData.RhsNodeSet)
                        {
                            if (index++ == nodePosition)
                                sb.Append ($" .");
                            sb.Append ($" {node.Name}");
                        }
                        return sb.ToString ();
                    },
                    element
                );
            }
        }

        private AnglrPDAViableSet AnalyzePDAStack (AnglrDebuggerGetPDAStack stack)
        {
            int counter = 0;
            int token = 0;

            AnglrPDAViableSet list = new AnglrPDAViableSet ();
            try
            {
                AnglrDrawingDictionary dictionary = DictionaryItem?.Drawings;
                if (dictionary == null)
                {
                    Logger?.ErrorLine ($"dictionary = null");
                    return list;
                }

                foreach (var cell in stack.PDAStackCells.Reverse ())
                {
                    AnglrGetParserStateItemResult pdaStateItem = anglrLangService?.InvokeGetParserState (new AnglrGetParserStateItemParams ()
                    {
                        TextDocument = new TextDocumentIdentifier ()
                        {
                            Uri = null
                        },
                        MagicNr = MagicNumber,
                        StateNr = cell.State
                    });
                    if (pdaStateItem == null)
                        continue;
                    AnglrPDASet anglrPDASet = LoadAnglrPDASet (pdaStateItem, token);
                    list [pdaStateItem] = anglrPDASet;
                    token = cell.Code;
                    counter += anglrPDASet.Count;
                }
                Logger?.InfoLine ($"generated {counter} visuals");
            }
            catch (Exception e)
            {
                Logger?.ErrorLine (e, $"cannot analyze PDA stack");
            }
            return list;
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
