using Anglr.Parser;
using Anglr.Parser.Core;
using Anglr.Parser.SyntaxTree;
using AnglrBreakPointDBLibrary;
using AnglrDebuggerBridge;
using AnglrDebuggerJsonRpcMessages;
using AnglrJsonRpcMethods;
using AnglrLibrary;
using AnglrLogLibrary;
using EnvDTE;
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
    public class AnglrPDAStateTransitionInfo : AnglrGetParserStateTransitionPointData
    {
        public List<AnglrPDAStateTransitionInfo> Children { get; private set; }
        public AnglrPDAStateTransitionInfo Parent { get; set; }
        public List<AnglrGetParserStateItemResult> PDAStates { get; private set; }
        public AnglrPDAStateTransitionInfo ()
        {
            Children = new List<AnglrPDAStateTransitionInfo> ();
            PDAStates = new List<AnglrGetParserStateItemResult> ();
        }
        public void Add (AnglrPDAStateTransitionInfo child)
        {
            foreach (var element in Children)
            {
                if ((element.Position == child.Position) && (element.Production.ProductionNumber == child.Production.ProductionNumber))
                    return;
            }
            Children.Add (child);
        }
        public void Traverse (Action<AnglrPDAStateTransitionInfo, object> f, object appData)
        {
            f (this, appData);
            foreach (var element in Children)
                element.Traverse (f, appData);
            //if (element.Parent == this)
            //        element.Traverse (f, appData);
            //    else
            //        f (element, " -> ");
        }
    }

    public class AnglrPDASetElementComparer : IComparer<AnglrPDAStateTransitionInfo>
    {
        public int Compare (AnglrPDAStateTransitionInfo x, AnglrPDAStateTransitionInfo y)
        {
            if (x.Production.ProductionNumber != y.Production.ProductionNumber)
                return x.Production.ProductionNumber - y.Production.ProductionNumber;
            return x.Position - y.Position;
        }
    }

    public class AnglrPDASet : SortedSet<AnglrPDAStateTransitionInfo>
    {
        public IAnglrLogger Logger { get; }
        public AnglrGetParserStateItemResult AnglrPDAState { get; }
        public AnglrPDASet (AnglrGetParserStateItemResult anglrPDAState, IAnglrLogger logger = null) : base (new AnglrPDASetElementComparer ())
        {
            AnglrPDAState = anglrPDAState;
            Logger = logger ?? new VoidAnglrLogger ();
        }
        public void Load (bool full)
        {
            foreach (var coreData in AnglrPDAState.CoreSet)
            {
                var production = coreData.TransitionPoint.Production;
                var position = coreData.TransitionPoint.Position;
                Logger?.DebugLine ($"\tadd core production {production.ProductionNumber}");
                Add
                (
                    new AnglrPDAStateTransitionInfo ()
                    {
                        Production = production,
                        Position = position
                    }
                );
            }
            if (!full)
                return;
            foreach (var closureData in AnglrPDAState.ClosureSet)
            {
                foreach (var productionInfo in closureData.ProductionNode.ProductionSet)
                {
                    Logger?.DebugLine ($"\tadd closure production {productionInfo.ProductionNumber}");
                    Add
                    (
                        new AnglrPDAStateTransitionInfo ()
                        {
                            Production = productionInfo,
                            Position = 0
                        }
                    );
                }
            }
        }

        public AnglrPDASet Reduce (AnglrPDASet ctrlCell)
        {
            if (ctrlCell == null)
            {
                foreach (var transition in this)
                    transition.PDAStates.Add (AnglrPDAState);
                return null;
            }
            AnglrPDASet reducedSet = new AnglrPDASet (AnglrPDAState, Logger);
            try
            {
                foreach (var transition in ctrlCell)
                {
                    if (transition.Position <= 0)
                        continue;
                    AnglrPDAStateTransitionInfo coreTransition = new AnglrPDAStateTransitionInfo ()
                    {
                        Production = transition.Production,
                        Position = transition.Position - 1
                    };
                    if (!reducedSet.Add (coreTransition))
                        continue;
                }
                int changes;
                while (true)
                {
                    changes = 0;
                    List<AnglrPDAStateTransitionInfo> elementList = new List<AnglrPDAStateTransitionInfo> ();
                    foreach (var closureTransition in reducedSet)
                    {
                        if (closureTransition.Position > 0)
                            continue;
                        int id = closureTransition.Production.ProductionName.Id;
                        foreach (var transition in this)
                        {
                            AnglrGetParserStateSymbolTokenData [] nodeSet = transition.Production.RhsNodeSet;
                            if ((nodeSet.Length > transition.Position) && (nodeSet [transition.Position].Id == id))
                            {
                                if (transition.Position == 0)
                                {
                                    AnglrGetParserStateProductionData coreProduction = transition.Production;
                                    if (coreProduction.ProductionName.Id == coreProduction.RhsNodeSet [0].Id)
                                        continue;
                                }
                                elementList.Add (transition);
                                transition.Add (closureTransition);
                            }
                        }
                    }
                    foreach (var transition in elementList)
                        if (reducedSet.Add (transition))
                            ++changes;
                    if (changes <= 0)
                        break;
                }
                foreach (var transition in reducedSet)
                    transition.PDAStates.Add (AnglrPDAState);
            }
            catch (Exception ex)
            {
                int depth = 0;
                Logger?.ErrorLine ($"Reduce failed: cell = {AnglrPDAState.StateNumber}, ctrl cell = {ctrlCell.AnglrPDAState.StateNumber}");
                while (ex != null)
                {
                    Logger?.ErrorLine ($"exception (depth = {depth++}):");
                    Logger?.ErrorLine (ex.Message);
                    Logger?.ErrorLine (ex.StackTrace);
                    ex = ex.InnerException;
                }
            }
            return reducedSet;
        }
        public void DisplayTransition (AnglrPDAStateTransitionInfo transition, object appData) => Logger?.InfoLine<AnglrPDAStateTransitionInfo>
            (
                (data) =>
                {
                    string indent = appData as string ?? "";
                    //if (appData == null)
                    //    for (var element = transition; element.Parent != null; element = element.Parent)
                    //        indent += "    ";

                    StringBuilder sb = new StringBuilder ();
                    AnglrGetParserStateProductionData productionData = data.Production;
                    int index = 0;
                    int nodePosition = data.Position;
                    int productionNumber = productionData.ProductionNumber;
                    string productionName = productionData.ProductionName.Name;
                    sb.Append ($"{indent}{productionNumber} {productionName} ({productionData.ProductionName.Id}):");
                    foreach (var node in productionData.RhsNodeSet)
                    {
                        if (index++ <= nodePosition)
                            sb.Append ($" .");
                        sb.Append ($" {node.Name} ({node.Id})");
                    }
                    sb.AppendLine ();
                    sb.Append ($"{indent}    states:");
                    foreach (var state in transition.PDAStates)
                        sb.Append ($" {state.StateNumber}");
                    return sb.ToString ();
                },
                transition
            );
        public void Display (string comment)
        {
            try
            {
                Logger?.InfoLine ($"{comment} {AnglrPDAState.StateNumber}");
                foreach (var element in this)
                    if (element.Parent == null)
                        element.Traverse (DisplayTransition, null);
            }
            catch (Exception ex)
            {
                Logger?.ErrorLine (ex, $"*** DISPLAY ERROR ***");
            }
        }
    }

    public class AnglrPDAViableSet : List<AnglrPDASet>
    {
        public IAnglrLogger Logger { get; }
        public AnglrPDAViableSet (IAnglrLogger logger = null)
        {
            Logger = logger ?? new VoidAnglrLogger ();
        }
        public AnglrPDAViableSet ReducePDASnapshot ()
        {
            AnglrPDAViableSet viableSet = new AnglrPDAViableSet (Logger);
            AnglrPDASet ctrlCell = null;
            foreach (var cell in ToArray ().Reverse ())
            {
                AnglrPDASet anglrPdaSet = cell.Reduce (ctrlCell);
                viableSet.Add (ctrlCell = anglrPdaSet ?? cell);
            }
            foreach (var pdaSet in viableSet.ToArray ().Reverse ())
                foreach (var element in pdaSet)
                    foreach (var child in element.Children)
                        child.Parent = child.Parent ?? element;
            ctrlCell = null;
            foreach (var cell in viableSet)
            {
                if (ctrlCell != null)
                {
                    foreach (var element in ctrlCell)
                    {
                        if (element.Parent != null)
                            continue;
                        int position = element.Position;
                        if (position <= 0)
                            continue;
                        int productionNumber = element.Production.ProductionNumber;
                        foreach (var cellElt in cell)
                        {
                            if (cellElt.Position + 1 != position)
                                continue;
                            if (cellElt.Production.ProductionNumber != productionNumber)
                                continue;
                            foreach (var child in element.Children)
                            {
                                child.Parent = cellElt;
                                cellElt.Children.Add (child);
                            }
                            foreach (var state in element.PDAStates)
                                cellElt.PDAStates.Add (state);
                        }
                    }
                }
                ctrlCell = cell;
            }
            viableSet.Reverse ();
            return viableSet;
        }
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
            try
            {
                Logger?.InfoLine ($"break-point hit ({dbgBreakPointHitRequest.SequenceNr})");
                foreach (var stack in getPDASnapshotResponse.PDAStackSet)
                {
                    Logger?.InfoLine ($"stack ({stack.PDAStackId})");
                    Logger?.InfoLine ($"state\tcode\tname\t\tvalue");
                    foreach (var cell in stack.PDAStackCells)
                    {
                        Logger?.InfoLine ($"\t{cell.State}\t{cell.Code}\t\t{cell.Name}\t\t{cell.Value}\t\t{cell.Tree}");
                        if (false)
                        {
                            try
                            {
                                object anglrFileFragment = JsonConvert.DeserializeObject (cell.Tree, settings);
                            }
                            catch (Exception ex)
                            {
                                if (false)
                                    Logger?.ErrorLine (ex, $"deserialization failed:");
                            }
                        }
                    }
                    AnglrPDAViableSet pdaSetList = AnalyzePDAStack (stack);
                    if (false)
                    {
                        foreach (var pda in pdaSetList)
                            pda?.Display ("PDA ORIGINAL CELL STATE");
                    }
                    AnglrPDAViableSet pdaReducedList = pdaSetList.ReducePDASnapshot ();
                    if (true)
                    {
                        foreach (var pda in pdaReducedList)
                        {
                            pda?.Display ("PDA CELL STATE");
                            break;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Logger?.ErrorLine (ex, $"break-point handler exception");
            }
        }

        private AnglrPDAViableSet AnalyzePDAStack (AnglrDebuggerGetPDAStack stack)
        {
            int counter = 0;

            AnglrPDAViableSet list = new AnglrPDAViableSet (Logger);
            try
            {
                AnglrDrawingDictionary dictionary = DictionaryItem?.Drawings;
                if (dictionary == null)
                {
                    Logger?.ErrorLine ($"dictionary = null");
                    return list;
                }

                bool full = false;
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
                    AnglrPDASet anglrPDASet = new AnglrPDASet (pdaStateItem, Logger);
                    anglrPDASet.Load (full);
                    list.Add (anglrPDASet);
                    counter += anglrPDASet.Count;
                    full = true;
                }
                Logger?.InfoLine ($"generated {counter} visuals");
                list.Reverse ();
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
