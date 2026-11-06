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
using Microsoft.VisualStudio.Package;
using Microsoft.VisualStudio.Shell;
using Microsoft.VisualStudio.Text;
using Microsoft.VisualStudio.Text.Classification;
using Microsoft.VisualStudio.Text.Editor;
using Microsoft.VisualStudio.Text.Formatting;
using Microsoft.VisualStudio.Text.Outlining;
using Microsoft.VisualStudio.Text.Projection;
using Microsoft.VisualStudio.Text.Tagging;
using Microsoft.VisualStudio.TextManager.Interop;
using Microsoft.VisualStudio.Threading;
using Microsoft.VisualStudio.Utilities;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using Newtonsoft.Json.Linq;
using StreamJsonRpc;
using StreamJsonRpc.Protocol;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.ComponentModel;
using System.ComponentModel.Composition;
using System.Diagnostics;
using System.Diagnostics.Metrics;
using System.Globalization;
using System.IO;
using System.IO.Pipelines;
using System.Linq;
using System.Reflection;
using System.Reflection.Emit;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Forms;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Media.Media3D;
using System.Windows.Navigation;
using System.Windows.Shapes;
using static System.Windows.Forms.AxHost;

using Color = System.Windows.Media.Color;
using UserControl = System.Windows.Controls.UserControl;

namespace AnglrLangExtension
{
    public class TabItemsSet : Dictionary<int, TabItem> { }
    public class AdornmentItems : Dictionary<int, UIElement> { }

    [Export (typeof (IWpfTextViewMarginProvider))]
    [Name (PdaOutliningMargin.MarginName)]
    [MarginContainer (PredefinedMarginNames.Left)]
    [Order (After = PredefinedMarginNames.Glyph)]
    [ContentType ("anglr-dbg")]
    [TextViewRole (PredefinedTextViewRoles.Document)]
    internal sealed class PdaOutliningMarginProvider : IWpfTextViewMarginProvider
    {
        internal IAnglrLangService anglrLangService = ThreadHelper.JoinableTaskFactory.Run (() => AnglrLangExtensionPackage.Instance.GetServiceAsync (typeof (SAnglrLangService))) as IAnglrLangService;
        public IWpfTextViewMargin CreateMargin (
            IWpfTextViewHost host,
            IWpfTextViewMargin marginContainer)
        {
            return new PdaOutliningMargin (host.TextView, anglrLangService);
        }
    }

    internal sealed class PDAMarginControl : Canvas
    {
        private VisualCollection _visualCollection;
        public PDAMarginControl ()
        {
            _visualCollection = new VisualCollection (this);
        }
        protected override int VisualChildrenCount
        {
            get => _visualCollection.Count;
        }

        protected override Visual GetVisualChild (int index) => _visualCollection [index];
        public void AddVisual (Visual visual) => _visualCollection.Add (visual);
        public void DeleteVisual (Visual visual) => _visualCollection.Remove (visual);
        public void Clear () => _visualCollection.Clear ();
    }

    internal sealed class PdaOutliningMargin : IWpfTextViewMargin
    {
        private VisualCollection visuals;
        public const string MarginName = "PdaOutliningMargin";
        internal IAnglrLangService AnglrLangService { get; }
        internal IAnglrLogger Logger { get; }
        internal AnglrDebugPanelTabSession AnglrDebugPanel { get; }
        internal PDAMarginControl MarginControl { get; }
        internal IWpfTextView SourceView { get; }
        private readonly IAdornmentLayer layer;
        private AnglrDebuggerGetPDASnapshotResponse PDASnapshotResponse { get; set; }
        private readonly Brush brush;
        private readonly Pen pen;

        public PdaOutliningMargin (IWpfTextView view, IAnglrLangService anglrLangService)
        {
            AnglrLangService = anglrLangService;
            Logger = AnglrLangService?.AnglrLogger;
            SourceView = view;
            layer = SourceView.GetAdornmentLayer ("PdaCellAdornment");

            brush = new SolidColorBrush (Color.FromArgb (0x20, 0x00, 0x00, 0xff));
            brush.Freeze ();

            var penBrush = new SolidColorBrush (Colors.Red);
            penBrush.Freeze ();
            pen = new Pen (penBrush, 0.5);
            pen.Freeze ();

            MarginControl = new PDAMarginControl
            {
                Width = 100
            };

            SourceView.LayoutChanged += OnLayoutChanged;
            SourceView.Properties.TryGetProperty<AnglrDebugPanelTabSession> ("AnglrDebugPanelTabSession", out var anglrDebugPanelTabSession);
            if ((AnglrDebugPanel = anglrDebugPanelTabSession) != null)
            {
                Logger?.InfoLine ($"created PDA outlining margin, granted access to AnglrDebugPanelTabSession instance");
                AnglrDebugPanel.SnapshotChanged += AnglrDebugPanel_SnapshotChanged;
            }
            else
                Logger?.InfoLine ($"created PDA outlining margin, cannot access AnglrDebugPanelTabSession instance");
        }

        private void AnglrDebugPanel_SnapshotChanged (object sender, EventArgs e)
        {
            Logger?.InfoLine ($"PDA outlining margin: snapshot changed");
            PDASnapshotResponse = e as AnglrDebuggerGetPDASnapshotResponse;
            Redraw (SourceView.TextBuffer.CurrentSnapshot);
        }

        public IEnumerable<AnglrSelection> GetSpans (ITextSnapshot snapshot)
        {
            Logger?.InfoLine ("PDA stack outlining tagger: get tags");
            foreach (var stack in PDASnapshotResponse?.PDAStackSet)
            {
                foreach (var cell in stack.PDAStackCells)
                    yield return new AnglrSelection (cell.StartSpan, cell.EndSpan);
                yield break;
            }
        }

        public FrameworkElement VisualElement => MarginControl;

        public double MarginSize => MarginControl.Width;

        public bool Enabled => true;

        public ITextViewMargin GetTextViewMargin (string marginName)
            => marginName == MarginName ? this : null;

        public void Dispose ()
        {
            SourceView.LayoutChanged -= OnLayoutChanged;
        }

        private void OnLayoutChanged (object sender, TextViewLayoutChangedEventArgs e)
        {
            Logger?.InfoLine ($"PDA outlining margin: layout changed");
            Redraw (e.NewSnapshot);
        }

        private void Redraw (ITextSnapshot snapshot)
        {
            MarginControl.Clear ();
            layer.RemoveAllAdornments ();
            Logger?.InfoLine ($"*** CREATE VISUALS ***");
            GeneralTransform transform = SourceView.VisualElement.TransformToVisual (MarginControl);
            IWpfTextViewLineCollection textViewLines = SourceView.TextViewLines;
            Geometry viewGeometry = textViewLines.GetLineMarkerGeometry (textViewLines.FormattedSpan);
            Rect viewBounds = new Rect ();
            if (viewGeometry != null)
            {
                viewBounds = transform.TransformBounds (viewGeometry.Bounds);
                Logger?.InfoLine ($"view old bounds: {viewGeometry.Bounds}");
                Logger?.InfoLine ($"view new bounds: {viewBounds}");
                //Canvas.SetTop (MarginControl, viewBounds.Top);
                //Canvas.SetBottom (MarginControl, viewBounds.Bottom);
            }
            else
            {
                Logger?.InfoLine ($"cannot access viewport geometry");
                return;
            }
                snapshot = SourceView.TextBuffer.CurrentSnapshot;
            foreach (var stack in PDASnapshotResponse?.PDAStackSet)
            {
                foreach (var cell in stack.PDAStackCells)
                {
                    (int lineno, int column) StartSpan = cell.StartSpan;
                    (int lineno, int column) EndSpan = cell.EndSpan;
                    Logger?.InfoLine ($"\tspan: {StartSpan} - {EndSpan}");
                    ITextSnapshotLine startSnapshotLine = snapshot.GetLineFromLineNumber (StartSpan.lineno);
                    ITextSnapshotLine endSnapshotLine = snapshot.GetLineFromLineNumber (EndSpan.lineno);
                    SnapshotPoint startSnapshotPoint = startSnapshotLine.Start + StartSpan.column;
                    SnapshotPoint endSnapshotPoint = endSnapshotLine.Start + EndSpan.column;
                    SnapshotSpan snapshotSpan = new SnapshotSpan (startSnapshotPoint, endSnapshotPoint);
                    Geometry geometry = textViewLines.GetLineMarkerGeometry (snapshotSpan);
                    if (geometry == null)
                        continue;

                    Rect bounds = transform.TransformBounds (geometry.Bounds);
                    Logger?.InfoLine ($"\told bounds: {geometry.Bounds}");
                    Logger?.InfoLine ($"\tnew bounds: {bounds}");

                    var icon = new PDAStateIcon (this, cell, geometry);

                    Canvas.SetLeft (icon, 0.0);
                    Canvas.SetTop (icon, geometry.Bounds.Top);

                    icon.DrawingVisual.Offset = new Vector (0.0, bounds.Top - viewBounds.Top);
                    MarginControl.AddVisual (icon.DrawingVisual);

                    layer.AddAdornment (AdornmentPositioningBehavior.TextRelative, snapshotSpan, null, icon, null);
                }
            }
        }
    }

    internal class PDAStateIcon : Image
    {
        private Brush IconBrush { get; }
        private Pen IconPen { get; }
        public double IconMargin { get; }
        public PdaOutliningMargin Margin { get; }
        public AnglrDebuggerGetPDAStackCell PDAStackCell { get; }
        public Geometry Geometry { get; }
        public DrawingVisual DrawingVisual { get; private set; }
        internal IAnglrLogger Logger { get; }
        public PDAStateIcon (PdaOutliningMargin margin, AnglrDebuggerGetPDAStackCell cell, Geometry geometry)
        {
            Margin = margin;
            PDAStackCell = cell;
            Geometry = geometry;
            Logger = margin?.Logger;

            IconBrush = new SolidColorBrush (Color.FromArgb (0x20, 0x00, 0x00, 0xff));
            IconBrush.Freeze ();

            var penBrush = new SolidColorBrush (Colors.Red);
            penBrush.Freeze ();
            IconPen = new Pen (penBrush, 0.5);
            IconPen.Freeze ();

            IconMargin = 3.0;

            Source = CreateMarginIcon ();
            CreateBubbleIcon ();

            MouseLeftButtonDown += PDAStateIcon_MouseLeftButtonDown;
            MouseRightButtonDown += PDAStateIcon_MouseRightButtonDown;
            MouseEnter += PDAStateIcon_MouseEnter;
            MouseLeave += PDAStateIcon_MouseLeave;
        }

        private void PDAStateIcon_MouseLeave (object sender, System.Windows.Input.MouseEventArgs e)
        {
        }

        private void PDAStateIcon_MouseEnter (object sender, System.Windows.Input.MouseEventArgs e)
        {
        }

        private void PDAStateIcon_MouseLeftButtonDown (object sender, MouseButtonEventArgs e)
        {
            ITextSnapshot snapshot = Margin.SourceView.TextBuffer.CurrentSnapshot;
            for (int i = PDAStackCell.StartSpan.lineno; i <= PDAStackCell.EndSpan.lineno; ++i)
            {
                ITextSnapshotLine line = snapshot.GetLineFromLineNumber (i);
            }
        }

        private void PDAStateIcon_MouseRightButtonDown (object sender, MouseButtonEventArgs e)
        {
            Logger?.InfoLine ($"right mouse down: state = {PDAStackCell.State}");
        }

        private DrawingImage CreateMarginIcon ()
        {
            Rect bounds = Geometry.Bounds;
            bounds.X = 0;
            bounds.Width = 10;
            RectangleGeometry rectangleGeometry = new RectangleGeometry (bounds, 3.0, 3.0);
            var drawing = new GeometryDrawing (IconBrush, IconPen, rectangleGeometry);
            drawing.Freeze ();

            var drawingImage = new DrawingImage (drawing);
            drawingImage.Freeze ();

            return drawingImage;
        }

        public DrawingImage CreateBubbleIcon ()
        {
            string info = $"{PDAStackCell.State}";
            DrawingVisual = new DrawingVisual ();
            using (var dc = DrawingVisual.RenderOpen ())
            {
                FormattedText text = new FormattedText
                (
                    info,
                    CultureInfo.InvariantCulture,
                    System.Windows.FlowDirection.LeftToRight,
                    new Typeface ("Consolas"),
                    14,
                    Brushes.Red,
                    0.5
                );
                double width = text.Width;
                double height = text.Height;

                dc.DrawRoundedRectangle
                    (
                        Brushes.LightBlue,
                        IconPen,
                        new Rect (0, 0, width + 2 * IconMargin, height + 2 * IconMargin),
                        2 * IconMargin,
                        2 * IconMargin
                    );
                dc.DrawText (text, new Point (IconMargin, IconMargin));
            }
            DrawingVisual.Drawing.Freeze ();
            var drawingImage = new DrawingImage (DrawingVisual.Drawing);
            drawingImage.Freeze ();
            return drawingImage;
        }
    }

    /// <summary>
    /// Interaction logic for AnglrDebugPanelTabSession.xaml
    /// </summary>
    public partial class AnglrDebugPanelTabSession : UserControl, IAnglrClientSideDebugger
    {
        public int MagicNumber { get; private set; }
        public AnglrLangDictionaryItem DictionaryItem { get; private set; }
        public JsonRpc Rpc { get; set; }

        public IAnglrLogger Logger { get; private set; }

        private IAnglrLangService anglrLangService;

        private SemaphoreSlim Semaphore { get; }
        private TabItemsSet tabItems;
        private AdornmentItems adornmentItems;
        private ITextBuffer TextBuffer { get; }
        private IWpfTextView WpfTextView { get; }
        private IWpfTextViewHost WpfTextViewHost { get; }
        private UIElement UIElement { get; }
        private bool AutoScroll { get; }

        private readonly IAdornmentLayer layer;
        private readonly Brush brush;
        private readonly Pen pen;
        public event EventHandler SnapshotChanged;

        public AnglrDebugPanelTabSession (IAnglrLangService anglrLangService)
        {
            InitializeComponent ();

            this.anglrLangService = anglrLangService;
            Logger = anglrLangService?.AnglrLogger ?? new VoidAnglrLogger ();

            Logger?.InfoLine ($"AnglrDebugPanelSession created ");

            Semaphore = new SemaphoreSlim (1);
            tabItems = new TabItemsSet ();
            adornmentItems = new AdornmentItems ();

            var anglrDbgContentType = anglrLangService.ContentTypeRegistryService.GetContentType ("anglr-dbg");

            if (anglrDbgContentType != null)
                Logger?.InfoLine ($"anglr-dbg content type loaded");
            else
                Logger?.InfoLine ($"anglr-dbg content type not loaded");

            if (anglrDbgContentType != null)
                TextBuffer = anglrLangService?.TextBufferFactoryService?.CreateTextBuffer (anglrDbgContentType);
            else
                TextBuffer = anglrLangService?.TextBufferFactoryService?.CreateTextBuffer ();

            TextBuffer.Properties.AddProperty ("AnglrDebugPanelTabSession", this);

            WpfTextView = anglrLangService?.TextEditorFactoryService?.CreateTextView (TextBuffer);
            WpfTextView.Properties.AddProperty ("AnglrDebugPanelTabSession", this);
            WpfTextView.LayoutChanged += WpfTextView_LayoutChanged;
            WpfTextViewHost = anglrLangService?.TextEditorFactoryService?.CreateTextViewHost (WpfTextView, true);
            UIElement = WpfTextViewHost.HostControl;
            pdaSnapshotText.Children.Add (UIElement);

            WpfTextView.Options.SetOptionValue (DefaultTextViewOptions.ViewProhibitUserInputId, false);
            WpfTextView.Options.SetOptionValue (DefaultTextViewHostOptions.LineNumberMarginId, true);
            WpfTextView.Options.SetOptionValue (DefaultTextViewHostOptions.OutliningMarginId, false);

            this.layer = WpfTextView.GetAdornmentLayer ("PdaCellAdornment");

            // Create the pen and brush to color the box behind the a's
            this.brush = new SolidColorBrush (Color.FromArgb (0x20, 0x00, 0x00, 0xff));
            this.brush.Freeze ();

            var penBrush = new SolidColorBrush (Colors.Red);
            penBrush.Freeze ();
            this.pen = new Pen (penBrush, 0.5);
            this.pen.Freeze ();

            AutoScroll = true;
        }

        private void WpfTextView_LayoutChanged (object sender, TextViewLayoutChangedEventArgs e)
        {
            ITextSnapshot snapshot = TextBuffer.CurrentSnapshot;
            SnapshotSpan entireBuffer = new SnapshotSpan (snapshot, 0, snapshot.Length);

            Logger?.InfoLine ($"layout changed, adorments count = {adornmentItems.Count}");
            foreach (var cell in adornmentItems)
            {
                Logger?.InfoLine ($"layout changed: add adornment nr.: {cell.Key}, size: {cell.Value.RenderSize}");
                layer.AddAdornment (AdornmentPositioningBehavior.TextRelative, entireBuffer, null, cell.Value, null);
            }
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
            continueButton.IsEnabled = true;
        }

        private void Rpc_Disconnected (object sender, JsonRpcDisconnectedEventArgs e)
        {
            Logger?.InfoLine ($"<AnglrDebuggerClientBridge>: RPC disconnected, reason: {e.Reason}");
        }

        public async Task LogMessageHandler (object sender, EventArgs e)
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

        public async Task<AnglrDebuggerConnectResponse> ConnectMessageHandler (object sender, EventArgs e)
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

        public async Task SyntaxErrorMessageHandler (object sender, EventArgs e)
        {
            AnglrDebuggerSyntaxErrorRequest syntaxErrorMessageRequest = e as AnglrDebuggerSyntaxErrorRequest;
            if (syntaxErrorMessageRequest == null)
                return;
            Logger?.InfoLine ($"syntax error ({syntaxErrorMessageRequest.SequenceNr})");
        }

        public async Task ShiftStepMessageHandler (object sender, EventArgs e)
        {
            AnglrDebuggerShiftStepRequest shiftStepMessageRequest = e as AnglrDebuggerShiftStepRequest;
            if (shiftStepMessageRequest == null)
                return;
            Logger?.InfoLine ($"shift ({shiftStepMessageRequest.SequenceNr})");
        }

        public async Task ReduceStepMessageHandler (object sender, EventArgs e)
        {
            AnglrDebuggerReduceStepRequest reduceStepMessageRequest = e as AnglrDebuggerReduceStepRequest;
            if (reduceStepMessageRequest == null)
                return;
            Logger?.InfoLine ($"reduce ({reduceStepMessageRequest.SequenceNr})");
        }

        public async Task SplitStepMessageHandler (object sender, EventArgs e)
        {
            AnglrDebuggerSplitStepRequest splitStepMessageRequest = e as AnglrDebuggerSplitStepRequest;
            if (splitStepMessageRequest == null)
                return;
            Logger?.InfoLine ($"split ({splitStepMessageRequest.OldStackNr})");
        }

        public async Task LoopStepMessageHandler (object sender, EventArgs e)
        {
            AnglrDebuggerLoopStepRequest loopStepMessageRequest = e as AnglrDebuggerLoopStepRequest;
            if (loopStepMessageRequest == null)
                return;
            Logger?.InfoLine ($"loop ({loopStepMessageRequest.SequenceNr})");
        }

        public async Task JoinMessageHandler (object sender, EventArgs e)
        {
            AnglrDebuggerJoinRequest joinMessageRequest = e as AnglrDebuggerJoinRequest;
            if (joinMessageRequest == null)
                return;
            Logger?.InfoLine ($"join ({joinMessageRequest.SequenceNr})");
        }

        public async Task FinalStepMessageHandler (object sender, EventArgs e)
        {
            AnglrDebuggerFinalStepRequest finalStepMessageRequest = e as AnglrDebuggerFinalStepRequest;
            if (finalStepMessageRequest == null)
                return;
            Logger?.InfoLine ($"final ({finalStepMessageRequest.SequenceNr})");
        }

        public async Task StopParserMessageHandler (object sender, EventArgs e)
        {
            AnglrDebuggerStopParserRequest stopParserMessageRequest = e as AnglrDebuggerStopParserRequest;
            if (stopParserMessageRequest == null)
                return;
            Logger?.InfoLine ($"stop ({stopParserMessageRequest.SequenceNr})");
        }

        public async Task DbgBreakPointHitMessageHandler (object sender, EventArgs e)
        {
            AnglrDebuggerDbgBreakPointHitRequest dbgBreakPointHitRequest = e as AnglrDebuggerDbgBreakPointHitRequest;
            if (dbgBreakPointHitRequest == null)
            {
                Logger?.InfoLine ($"break-point hit (null request)");
                return;
            }

            AnglrDebuggerGetPDASnapshotResponse getPDASnapshotResponse =
            await Rpc.InvokeAsync<AnglrDebuggerGetPDASnapshotResponse>
            (
                AnglrDebuggerJsonRpcMessageNames.GetPDASnapshotMessageName,
                new AnglrDebuggerGetPDASnapshotRequest ()
                {
                    SequenceNr = dbgBreakPointHitRequest.SequenceNr
                }
            );
            if (getPDASnapshotResponse == null)
            {
                Logger?.InfoLine ($"break-point hit (null snapshot)");
                return;
            }

            try
            {
                await Semaphore.WaitAsync ();
                Logger?.InfoLine ($"break-point hit ({dbgBreakPointHitRequest.SequenceNr})");
                Logger?.DebugLine ($"*** START OF DELTA SOURCE TEXT POSITION = {getPDASnapshotResponse.StartSpan} ***\n");
                Logger?.DebugLine ($"{getPDASnapshotResponse.DeltaSourceText}");
                Logger?.DebugLine ($"\n*** END   OF DELTA SOURCE TEXT POSITION = {getPDASnapshotResponse.EndSpan} ***");

                await ThreadHelper.JoinableTaskFactory.SwitchToMainThreadAsync ();

                adornmentItems.Clear ();

                AppendText (getPDASnapshotResponse.DeltaSourceText);
                SnapshotChanged?.Invoke (this, getPDASnapshotResponse);

                TabItemsSet newTabItems = new TabItemsSet ();
                foreach (var stack in getPDASnapshotResponse.PDAStackSet)
                {
                    if (stack == null)
                        continue;
                    Logger?.DebugLine ($"stack ({stack.PDAStackId})");
                    Logger?.DebugLine ($"state\tcode\tname\t\tvalue");
                    foreach (var cell in stack.PDAStackCells)
                    {
                        Logger?.DebugLine ($"\t{cell.State}\t{cell.Code}\t\t{cell.Name}\t\t{cell.Value}");
                        Logger?.DebugLine ($"\t\t{new AnglrSelection (cell.StartSpan, cell.EndSpan)}");
                    }
                    AnglrPDAViableSet pdaSetList = await AnalyzePDAStack (stack);
                    AnglrPDAViableSet pdaReducedList = pdaSetList.ReducePDASnapshot ();
                    AnglrPDASet pdaSet = null;
                    if (true)
                    {
                        foreach (var pda in pdaReducedList)
                        {
                            //pda?.Display ("PDA CELL STATE");
                            pdaSet = pda;
                            break;
                        }
                    }
                    if (pdaSet != null)
                    {
                        await ThreadHelper.JoinableTaskFactory.SwitchToMainThreadAsync ();
                        AnglrPDASnapshotViewerTab viewerTab = null;
                        if (!tabItems.TryGetValue (stack.PDAStackId, out var tabItem))
                        {
                            tabItem = new TabItem ();
                            viewerTab = new AnglrPDASnapshotViewerTab (Logger, stack, this);
                            tabItem.Content = viewerTab;
                            tabItem.Tag = stack.PDAStackId;
                            tabItem.Header = $"PDA {stack.PDAStackId}";
                            pdaSnapshotTabs.Items.Add (tabItem);
                        }
                        else
                        {
                            viewerTab = tabItem.Content as AnglrPDASnapshotViewerTab;
                            viewerTab.Refresh (stack, false);
                        }
                        newTabItems [stack.PDAStackId] = tabItem;

                        AnglrPDASnapshotDrawing pdaShapshotDrawing = new AnglrPDASnapshotDrawing (Logger, pdaSet);
                        pdaShapshotDrawing.Draw ();
                        pdaShapshotDrawing.Offset = new Vector (0, 0);
                        pdaShapshotDrawing.ComputeVisualBounds ();

                        AnglrParserPartDrawingCanvas drawingCanvas = viewerTab.pdaSnapshotVisual;
                        drawingCanvas.Logger = Logger;
                        drawingCanvas.Clear ();
                        drawingCanvas.AddVisual (pdaShapshotDrawing);
                        drawingCanvas.Width = pdaShapshotDrawing.Width;
                        drawingCanvas.Height = pdaShapshotDrawing.Height;
                        drawingCanvas.InvalidateVisual ();

                        CreateVisuals (stack);
                    }
                }

                await ThreadHelper.JoinableTaskFactory.SwitchToMainThreadAsync ();
                foreach (var key in tabItems.Keys)
                    if (!newTabItems.TryGetValue (key, out var item))
                    {
                        Logger?.DebugLine ($"remove tab item {key}");
                        pdaSnapshotTabs.Items.Remove (tabItems [key]);
                    }

                tabItems = newTabItems;
                Logger?.DebugLine<TabItemsSet>
                (
                    (data) =>
                    {
                        string message = $"tab items:";
                        foreach (var item in data)
                            message += $" {item.Key}";
                        return message;
                    },
                    tabItems
                );
            }
            catch (Exception ex)
            {
                Logger?.ErrorLine (ex, $"break-point handler exception");
            }
            finally
            {
                Semaphore.Release ();
                continueButton.IsEnabled = true;
            }
        }

        private void AppendText (string text)
        {
            using (var edit = TextBuffer.CreateEdit ())
            {
                edit.Insert (
                    TextBuffer.CurrentSnapshot.Length,
                    text ?? "");

                edit.Apply ();
            }

            if (AutoScroll)
            {
                var point = new SnapshotPoint (TextBuffer.CurrentSnapshot, TextBuffer.CurrentSnapshot.Length);
                WpfTextView.Caret.MoveTo (point);
                WpfTextView.DisplayTextLineContainingBufferPosition (point, 0.0, ViewRelativePosition.Bottom);
            }
        }
        private async Task<AnglrPDAViableSet> AnalyzePDAStack (AnglrDebuggerGetPDAStack stack)
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
                    AnglrGetParserStateItemResult pdaStateItem = await anglrLangService?.InvokeGetParserState (new AnglrGetParserStateItemParams ()
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
                Logger?.DebugLine ($"generated {counter} visuals");
                list.Reverse ();
            }
            catch (Exception e)
            {
                Logger?.ErrorLine (e, $"cannot analyze PDA stack");
            }
            return list;
        }

        private void CreateVisuals (AnglrDebuggerGetPDAStack pdaStack)
        {
            return;
            layer.RemoveAllAdornments ();
            IWpfTextViewLineCollection textViewLines = WpfTextView.TextViewLines;
            Logger?.InfoLine ($"*** CREATE VISUALS ***");
            foreach (var cell in pdaStack.PDAStackCells)
            {
                (int lineno, int column) StartSpan = cell.StartSpan;
                (int lineno, int column) EndSpan = cell.EndSpan;
                Logger?.InfoLine ($"\tspan: {StartSpan} - {EndSpan}");
                ITextSnapshot snapshot = TextBuffer.CurrentSnapshot;
                ITextSnapshotLine startSnapshotLine = snapshot.GetLineFromLineNumber (StartSpan.lineno);
                ITextSnapshotLine endSnapshotLine = snapshot.GetLineFromLineNumber (EndSpan.lineno);
                SnapshotPoint startSnapshotPoint = startSnapshotLine.Start + StartSpan.column;
                SnapshotPoint endSnapshotPoint = endSnapshotLine.Start + EndSpan.column;
                SnapshotSpan snapshotSpan = new SnapshotSpan (startSnapshotPoint, endSnapshotPoint);
                Geometry geometry = textViewLines.GetLineMarkerGeometry (snapshotSpan);
                if (geometry == null)
                    continue;
                Rect bounds = geometry.Bounds;
                Logger?.InfoLine ($"\tgeometry bounds: ({bounds.Left}, {bounds.Top}, {bounds.Width}, {bounds.Height})");
                var drawing = new GeometryDrawing (this.brush, this.pen, geometry);
                drawing.Freeze ();

                var drawingImage = new DrawingImage (drawing);
                drawingImage.Freeze ();

                var image = new Image
                {
                    Source = drawingImage,
                };

                // Align the image with the top of the bounds of the text geometry
                Canvas.SetLeft (image, geometry.Bounds.Left);
                Canvas.SetTop (image, geometry.Bounds.Top);

                layer.AddAdornment (AdornmentPositioningBehavior.TextRelative, snapshotSpan, null, image, null);
            }
        }

        public void AddAdornment (AnglrDebuggerGetPDAStackCell cell)
        {
            return;
            RemoveAdornment (cell);
            IWpfTextViewLineCollection textViewLines = WpfTextView.TextViewLines;
            (int lineno, int column) StartSpan = cell.StartSpan;
            (int lineno, int column) EndSpan = cell.EndSpan;
            ITextSnapshot snapshot = TextBuffer.CurrentSnapshot;
            ITextSnapshotLine startSnapshotLine = snapshot.GetLineFromLineNumber (StartSpan.lineno);
            ITextSnapshotLine endSnapshotLine = snapshot.GetLineFromLineNumber (EndSpan.lineno);
            SnapshotPoint startSnapshotPoint = startSnapshotLine.Start + StartSpan.column;
            SnapshotPoint endSnapshotPoint = endSnapshotLine.Start + EndSpan.column;
            SnapshotSpan snapshotSpan = new SnapshotSpan (startSnapshotPoint, endSnapshotPoint);
            Geometry geometry = textViewLines.GetLineMarkerGeometry (snapshotSpan);
            if (geometry == null)
                return;
            var drawing = new GeometryDrawing (this.brush, this.pen, geometry);
            drawing.Freeze ();

            var drawingImage = new DrawingImage (drawing);
            drawingImage.Freeze ();

            var image = new Image
            {
                Source = drawingImage,
            };

            // Align the image with the top of the bounds of the text geometry
            Canvas.SetLeft (image, geometry.Bounds.Left);
            Canvas.SetTop (image, geometry.Bounds.Top);

            layer.AddAdornment (AdornmentPositioningBehavior.TextRelative, snapshotSpan, null, image, null);
            adornmentItems [cell.State] = image;
        }

        public void RemoveAdornment (AnglrDebuggerGetPDAStackCell cell)
        {
            return;
            if (adornmentItems.TryGetValue (cell.State, out var adornment))
            {
                layer.RemoveAdornment (adornment);
                adornmentItems.Remove (cell.State);
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
                    continueButton.IsEnabled = false;
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
