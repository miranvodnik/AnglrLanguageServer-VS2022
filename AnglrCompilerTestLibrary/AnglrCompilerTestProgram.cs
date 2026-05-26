using AnglrDebuggerBridge;
using AnglrDebuggerJsonRpcMessages;
using AnglrLogLibrary;
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
using System.Threading;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace anglr_cs
{
    public class AnglrDebuggerStackElement
    {
        public int Id { get; set; }
        public bool IsTerminal { get; set; }
        public int State { get; set; }
        public string Name { get; set; }
        public string Value { get; set; }
        public AnglrDebuggerStackElement () { }
        public AnglrDebuggerStackElement (AnglrDebuggerStackElement item)
        {
            Id = item.Id;
            IsTerminal = item.IsTerminal;
            State = item.State;
            Name = item.Name;
            Value = item.Value;
        }
    }

    public class ObservableStack<T> : List<T>, INotifyCollectionChanged, INotifyPropertyChanged
    {
        public ObservableStack () : base () { }

        public ObservableStack (IEnumerable<T> collection) : base (collection) { }

        public ObservableStack (int capacity) : base (capacity) { }

        public void Push (T item)
        {
            Add (item);
            OnCollectionChanged (NotifyCollectionChangedAction.Add, item, Count - 1);
            OnPropertyChanged (nameof (Count));
        }

        public T Pop (int count = 1)
        {
            T item = default;
            while (count-- > 0)
            {
                item = Peek ();
                RemoveAt (Count - 1);
                OnCollectionChanged (NotifyCollectionChangedAction.Remove, item, Count);
                OnPropertyChanged (nameof (Count));
            }
            return item;
        }

        public new T Peek () => this [Count - 1];

        public new void Clear ()
        {
            base.Clear ();
            OnCollectionChanged (NotifyCollectionChangedAction.Reset);
            OnPropertyChanged (nameof (Count));
        }

        public event NotifyCollectionChangedEventHandler CollectionChanged;

        protected virtual void OnCollectionChanged (NotifyCollectionChangedAction action, T item = default, int index = 0)
        {
            CollectionChanged?.Invoke (this, (item == null) && (action != NotifyCollectionChangedAction.Reset)
                ? new NotifyCollectionChangedEventArgs (action)
                : new NotifyCollectionChangedEventArgs (action, item, index));
        }

        public event PropertyChangedEventHandler PropertyChanged;

        protected virtual void OnPropertyChanged (string propertyName)
        {
            PropertyChanged?.Invoke (this, new PropertyChangedEventArgs (propertyName));
        }
    }

    public class AnglrDebuggerStackInfo : ObservableStack<AnglrDebuggerStackElement>
    {
        public AnglrDebuggerStackInfo (AnglrDebuggerStackInfo stackInfo)
        {
            foreach (var item in stackInfo)
                base.Push (new AnglrDebuggerStackElement (item));
        }
        public AnglrDebuggerStackInfo (AnglrDebuggerStackElement item) => base.Push (item);
    }

    public partial class AnglrDebuggerStackView
    {
        public AnglrDebuggerStackInfo ParserStack { get; private set; }
        public int StackNr { get; set; }

        public AnglrDebuggerStackView ()
        {
            ParserStack = null;
        }

        public void Log (AnglrLogLevel logLevel, string message, string indent = null) => Console.WriteLine (message);

        public void InitParserStack (AnglrDebuggerStackInfo parserStack, int stackNr)
        {
            StackNr = stackNr;
            if (parserStack == null)
            {
                ParserStack = new AnglrDebuggerStackInfo
                (
                    new AnglrDebuggerStackElement ()
                    {
                        Id = 0,
                        IsTerminal = false,
                        State = 0,
                        Name = "",
                        Value = ""
                    }
                );
            }
            else
            {
                ParserStack = new AnglrDebuggerStackInfo (parserStack);
            }
            //ParserStack.CollectionChanged += AnglrDebuggerStackView_CollectionChanged;
        }

        public string ReducedValue { get; set; }

        private void AnglrDebuggerStackView_CollectionChanged (object sender, NotifyCollectionChangedEventArgs e)
        {
            NotifyCollectionChangedAction action = e.Action;
            if (action != NotifyCollectionChangedAction.Remove)
                return;

            if ((e.OldItems == null) || (e.OldItems.Count == 0))
                return;

            AnglrDebuggerStackElement element = e.OldItems [0] as AnglrDebuggerStackElement;
            if (element == null)
                return;

            Log (AnglrLogLevel.Debug, $"AnglrDebuggerStackView_CollectionChanged, value = {element.Value}");
            string val = element.Value + " " + ReducedValue;
            ReducedValue = val.Trim ();
        }
    }

    public class AnglrLRStackViewSet : Dictionary<int, AnglrDebuggerStackView> { }

    public class DbgProcessRpc : IAnglrServerSideDebugger
    {
        public ObservableCollection<AnglrDebuggerStackView> LRStackViewCollection { get; set; }
        private AnglrLRStackViewSet lRStackViewSet;

        public JsonRpc Rpc { get; set; }

        public IAnglrLogger Logger {  get; private set; }

        public DbgProcessRpc ()
        {
            Logger = new ConsoleAnglrLogger ();
            LRStackViewCollection = new ObservableCollection<AnglrDebuggerStackView> ();
            lRStackViewSet = new AnglrLRStackViewSet ();
        }

        public void Log (string message, string indent = null)
        {
            Console.WriteLine (message);
        }

        public void Log (AnglrLogLevel logLevel, string message, string indent = null)
        {
        }

        public AnglrDebuggerLogResponse LogMessageHandler (object sender, EventArgs e)
        {
            AnglrDebuggerLogRequest logMessageRequest = e as AnglrDebuggerLogRequest;
            if (logMessageRequest == null)
                return new AnglrDebuggerLogResponse () { SequenceNr = -1 };

            return new AnglrDebuggerLogResponse () { SequenceNr = logMessageRequest.SequenceNr };
        }

        public AnglrDebuggerConnectResponse ConnectMessageHandler (object sender, EventArgs e)
        {
            AnglrDebuggerConnectRequest connectMessageRequest = e as AnglrDebuggerConnectRequest;
            if (connectMessageRequest == null)
                return new AnglrDebuggerConnectResponse () { SequenceNr = -1, Valid = false };

            try
            {
                lRStackViewSet.Clear ();
                LRStackViewCollection.Clear ();
                AnglrDebuggerStackView lrStack = new AnglrDebuggerStackView ();
                lrStack.InitParserStack (null, connectMessageRequest.StackNr);
                lRStackViewSet [connectMessageRequest.StackNr] = lrStack;
                LRStackViewCollection.Add (lrStack);
            }
            catch (Exception ex)
            {
                Logger?.ErrorLine ($"ConnectMessageInvoker exception: {ex.Message}");
                Logger?.ErrorLine ($"\t{ex.StackTrace}");
            }
            return new AnglrDebuggerConnectResponse () { SequenceNr = connectMessageRequest.SequenceNr, Valid = true };
        }

        public AnglrDebuggerSyntaxErrorResponse SyntaxErrorMessageHandler (object sender, EventArgs e)
        {
            AnglrDebuggerSyntaxErrorRequest syntaxErrorMessageRequest = e as AnglrDebuggerSyntaxErrorRequest;
            if (syntaxErrorMessageRequest == null)
                return new AnglrDebuggerSyntaxErrorResponse () { SequenceNr = -1 };
            try
            {
                LRStackViewCollection.Remove (lRStackViewSet [syntaxErrorMessageRequest.StackNr]);
                lRStackViewSet.Remove (syntaxErrorMessageRequest.StackNr);
            }
            catch (Exception ex)
            {
                Logger?.ErrorLine ($"SyntaxErrorMessageHandler exception: {ex.Message}");
                Logger?.ErrorLine ($"\t{ex.StackTrace}");
            }
            return new AnglrDebuggerSyntaxErrorResponse () { SequenceNr = syntaxErrorMessageRequest.SequenceNr };
        }

        public AnglrDebuggerShiftStepResponse ShiftStepMessageHandler (object sender, EventArgs e)
        {
            AnglrDebuggerShiftStepRequest shiftStepMessageRequest = e as AnglrDebuggerShiftStepRequest;
            if (shiftStepMessageRequest == null)
                return new AnglrDebuggerShiftStepResponse () { SequenceNr = -1 };
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
                Logger?.ErrorLine ($"ShiftStepMessageInvoker exception: {ex.Message}");
                Logger?.ErrorLine ($"\t{ex.StackTrace}");
            }
            return new AnglrDebuggerShiftStepResponse () { SequenceNr = shiftStepMessageRequest.SequenceNr };
        }

        public AnglrDebuggerReduceStepResponse ReduceStepMessageHandler (object sender, EventArgs e)
        {
            AnglrDebuggerReduceStepRequest reduceStepMessageRequest = e as AnglrDebuggerReduceStepRequest;
            if (reduceStepMessageRequest == null)
                return new AnglrDebuggerReduceStepResponse () { SequenceNr = -1 };
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
                Logger?.ErrorLine ($"exception:");
                Logger?.ErrorLine ($"ReduceStepMessageInvoker exception: {ex.Message}");
                Logger?.ErrorLine ($"\t{ex.StackTrace}");
            }
            return new AnglrDebuggerReduceStepResponse () { SequenceNr = reduceStepMessageRequest.SequenceNr };
        }

        public AnglrDebuggerSplitStepResponse SplitStepMessageHandler (object sender, EventArgs e)
        {
            AnglrDebuggerSplitStepRequest splitStepMessageRequest = e as AnglrDebuggerSplitStepRequest;
            if (splitStepMessageRequest == null)
                return new AnglrDebuggerSplitStepResponse () { SequenceNr = -1 };
            try
            {
                if (splitStepMessageRequest.Begin)
                {
                    AnglrDebuggerStackView anglrDebuggerStackView = new AnglrDebuggerStackView ();
                    anglrDebuggerStackView.InitParserStack (lRStackViewSet [splitStepMessageRequest.OldStackNr].ParserStack, splitStepMessageRequest.NewStackNr);
                    LRStackViewCollection.Add (lRStackViewSet [splitStepMessageRequest.NewStackNr] = anglrDebuggerStackView);
                }
                else
                {
                    LRStackViewCollection.Remove (lRStackViewSet [splitStepMessageRequest.OldStackNr]);
                    lRStackViewSet.Remove (splitStepMessageRequest.OldStackNr);
                }
            }
            catch (Exception ex)
            {
                Logger?.ErrorLine ($"SplitStepMessageInvoker exception: {ex.Message}");
                Logger?.ErrorLine ($"\t{ex.StackTrace}");
            }
            return new AnglrDebuggerSplitStepResponse () { SequenceNr = splitStepMessageRequest.SequenceNr };
        }

        public AnglrDebuggerLoopStepResponse LoopStepMessageHandler (object sender, EventArgs e)
        {
            AnglrDebuggerLoopStepRequest loopStepMessageRequest = e as AnglrDebuggerLoopStepRequest;
            if (loopStepMessageRequest == null)
                return new AnglrDebuggerLoopStepResponse () { SequenceNr = -1 };
            try
            {
                LRStackViewCollection.Remove (lRStackViewSet [loopStepMessageRequest.StackNr]);
                lRStackViewSet.Remove (loopStepMessageRequest.StackNr);
            }
            catch (Exception ex)
            {
                Logger?.ErrorLine ($"LoopStepMessageInvoker exception: {ex.Message}");
                Logger?.ErrorLine ($"\t{ex.StackTrace}");
            }
            return new AnglrDebuggerLoopStepResponse () { SequenceNr = loopStepMessageRequest.SequenceNr };
        }

        public AnglrDebuggerJoinResponse JoinMessageHandler (object sender, EventArgs e)
        {
            AnglrDebuggerJoinRequest joinMessageRequest = e as AnglrDebuggerJoinRequest;
            if (joinMessageRequest == null)
                return new AnglrDebuggerJoinResponse () { SequenceNr = -1 };
            try
            {
                LRStackViewCollection.Remove (lRStackViewSet [joinMessageRequest.StackNr]);
                lRStackViewSet.Remove (joinMessageRequest.StackNr);
            }
            catch (Exception ex)
            {
                Logger?.ErrorLine ($"JoinMessageInvoker exception: {ex.Message}");
                Logger?.ErrorLine ($"\t{ex.StackTrace}");
            }
            return new AnglrDebuggerJoinResponse () { SequenceNr = joinMessageRequest.SequenceNr };
        }

        public AnglrDebuggerFinalStepResponse FinalStepMessageHandler (object sender, EventArgs e)
        {
            AnglrDebuggerFinalStepRequest finalStepRequest = e as AnglrDebuggerFinalStepRequest;
            if ((finalStepRequest == null))
                return new AnglrDebuggerFinalStepResponse () { SequenceNr = -1 };
            return new AnglrDebuggerFinalStepResponse () { SequenceNr = finalStepRequest.SequenceNr };
        }

        public AnglrDebuggerStopParserResponse StopParserMessageHandler (object sender, EventArgs e)
        {
            AnglrDebuggerStopParserRequest stopParserRequest = e as AnglrDebuggerStopParserRequest;
            if (stopParserRequest == null)
                return new AnglrDebuggerStopParserResponse () { SequenceNr = -1 };
            return new AnglrDebuggerStopParserResponse () { SequenceNr = stopParserRequest.SequenceNr };
        }

        [JsonRpcMethod (AnglrDebuggerJsonRpcMessageNames.ConnectMessageName)]
        public AnglrDebuggerConnectResponse HandleConnectMessage (JToken connectMessage) =>
            ConnectMessageHandler (this, connectMessage.ToObject<AnglrDebuggerConnectRequest> ());

        [JsonRpcMethod (AnglrDebuggerJsonRpcMessageNames.SyntaxErrorMessageName)]
        public AnglrDebuggerSyntaxErrorResponse HandleSyntaxErrorMessage (JToken syntaxErrorMessage) =>
            SyntaxErrorMessageHandler (this, syntaxErrorMessage.ToObject<AnglrDebuggerSyntaxErrorRequest> ());

        [JsonRpcMethod (AnglrDebuggerJsonRpcMessageNames.ShiftStepMessageName)]
        public AnglrDebuggerShiftStepResponse HandleShiftStepMessage (JToken shiftStepMessage) =>
            ShiftStepMessageHandler (this, shiftStepMessage.ToObject<AnglrDebuggerShiftStepRequest> ());

        [JsonRpcMethod (AnglrDebuggerJsonRpcMessageNames.ReduceStepMessageName)]
        public AnglrDebuggerReduceStepResponse HandleReduceStepMessage (JToken reduceStepMessage) =>
            ReduceStepMessageHandler (this, reduceStepMessage.ToObject<AnglrDebuggerReduceStepRequest> ());

        [JsonRpcMethod (AnglrDebuggerJsonRpcMessageNames.SplitStepMessageName)]
        public AnglrDebuggerSplitStepResponse HandleSplitStepMessage (JToken splitStepMessage) =>
            SplitStepMessageHandler (this, splitStepMessage.ToObject<AnglrDebuggerSplitStepRequest> ());

        [JsonRpcMethod (AnglrDebuggerJsonRpcMessageNames.LoopStepMessageName)]
        public AnglrDebuggerLoopStepResponse HandleLoopStepMessage (JToken loopStepMessage) =>
            LoopStepMessageHandler (this, loopStepMessage.ToObject<AnglrDebuggerLoopStepRequest> ());

        [JsonRpcMethod (AnglrDebuggerJsonRpcMessageNames.JoinMessageName)]
        public AnglrDebuggerJoinResponse HandleJoinMessage (JToken joinMessage) =>
            JoinMessageHandler (this, joinMessage.ToObject<AnglrDebuggerJoinRequest> ());

        [JsonRpcMethod (AnglrDebuggerJsonRpcMessageNames.FinalStepMessageName)]
        public AnglrDebuggerFinalStepResponse HandleFinalStepMessage (JToken finalStepMessage) =>
            FinalStepMessageHandler (this, finalStepMessage.ToObject<AnglrDebuggerFinalStepRequest> ());

        [JsonRpcMethod (AnglrDebuggerJsonRpcMessageNames.StopParserMessageName)]
        public AnglrDebuggerStopParserResponse HandleStopParserMessage (JToken stopParserMessage) =>
            StopParserMessageHandler (this, stopParserMessage.ToObject<AnglrDebuggerStopParserRequest> ());
    }

    public class DbgProcessCtrl : IAnglrServerSideDebuggerInvoker
    {
        public JsonRpc Rpc { get; set; }

        public IAnglrLogger Logger { get; private set; } = new ConsoleAnglrLogger ();

        private void Rpc_Disconnected (object sender, JsonRpcDisconnectedEventArgs e)
        {
            Logger?.InfoLine ($"<AnglrDebuggerServerBridge>: RPC disconnected, reason: {e.Reason}");
        }

        public void InvokeRpcSession (int counter, Stream pipe, CancellationToken token)
        {
            _ = Task.Run (() =>
            {
                Logger?.InfoLine ($"<AnglrDebuggerServerBridge>: rpc channel {counter} trying to attach");
                DbgProcessRpc dbgProcessRpc = new DbgProcessRpc ();
                Rpc = dbgProcessRpc.Rpc = JsonRpc.Attach (pipe, pipe, dbgProcessRpc);
                Logger?.InfoLine ($"<AnglrDebuggerServerBridge>: rpc channel {counter} created");
                Rpc.Disconnected += Rpc_Disconnected;
                Rpc.Completion.ConfigureAwait (false).GetAwaiter ().GetResult ();
                Logger?.InfoLine ($"<AnglrDebuggerServerBridge>: rpc channel {counter} completed");
                Rpc.Dispose ();
                Logger?.InfoLine ($"<AnglrDebuggerServerBridge>: rpc channel {counter} disposed");
                Rpc = null;
            });
        }
    }

    public class AnglrCompilerTestProgram
    {
        public static AnglrDebuggerServerBridge anglrDebuggerServerBridge = null;
        public static async Task MainTask (string [] args)
        {
            if (args.Length < 3)
            {
                Console.WriteLine ("Usage: <program path> <program args> <program working directory> <output redirection file> <error redirection file>");
                return;
            }
            anglrDebuggerServerBridge = new AnglrDebuggerServerBridge (args [0], args [1], args [2], (args.Length > 3) ? args [3] : null, (args.Length > 4) ? args [4] : null);
            await Task.Run (() => anglrDebuggerServerBridge.DebuggedProcessCtrlAsync (new DbgProcessCtrl ()));
            Console.WriteLine ("Press return key to continue");
            await Console.In.ReadLineAsync ();
        }
    }
}
