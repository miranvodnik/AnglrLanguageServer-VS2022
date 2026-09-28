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
using System.Globalization;
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
using System.Windows.Media.Media3D;
using System.Windows.Navigation;
using System.Windows.Shapes;
using static System.Windows.Forms.AxHost;

namespace AnglrLangExtension
{
    public class AnglrStateItemResultList : List<AnglrGetParserStateItemResult> { }
    public class AnglrStateItemResultListComparer : IComparer<AnglrStateItemResultList>
    {
        public int Compare (AnglrStateItemResultList x, AnglrStateItemResultList y)
        {
            if (x.Count != y.Count)
                return x.Count - y.Count;
            AnglrStateItemResultList.Enumerator xenum = x.GetEnumerator ();
            AnglrStateItemResultList.Enumerator yenum = y.GetEnumerator ();
            while (xenum.MoveNext () && yenum.MoveNext ())
            {
                int diff = xenum.Current.StateNumber - yenum.Current.StateNumber;
                if (diff != 0)
                    return diff;
            }
            return 0;
        }
    }
    public class AnglrPDAStateTransitionInfo : AnglrGetParserStateTransitionPointData
    {
        IAnglrLogger Logger { get; set; }
        public List<AnglrPDAStateTransitionInfo> Children { get; private set; }
        public AnglrPDAStateTransitionInfo Parent { get; set; }
        public AnglrStateItemResultList PDAStates { get; private set; }
        public AnglrPDAStateTransitionInfo (IAnglrLogger logger)
        {
            Logger = logger ?? new VoidAnglrLogger ();
            Children = new List<AnglrPDAStateTransitionInfo> ();
            PDAStates = new AnglrStateItemResultList ();
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
        }
        public string Display ()
        {
            StringBuilder sb = new StringBuilder ();
            int index = 0;
            int productionNumber = Production.ProductionNumber;
            AnglrGetParserStateSymbolTokenData productionName = Production.ProductionName;
            sb.Append ($"{productionNumber} {productionName.Name} ({productionName.Id}):");
            foreach (var node in Production.RhsNodeSet)
            {
                if (index++ <= Position)
                    sb.Append ($" .");
                sb.Append ($" {node.Name} ({node.Id})");
            }
            return sb.ToString ();
        }
    }

    public class AnglrPDASetElementComparer : IComparer<AnglrGetParserStateTransitionPointData>
    {
        public int Compare (AnglrGetParserStateTransitionPointData x, AnglrGetParserStateTransitionPointData y)
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
                    new AnglrPDAStateTransitionInfo (Logger)
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
                        new AnglrPDAStateTransitionInfo (Logger)
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
                    AnglrPDAStateTransitionInfo coreTransition = new AnglrPDAStateTransitionInfo (Logger)
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
                    if (appData == null)
                        for (var element = transition; element.Parent != null; element = element.Parent)
                            indent += "    ";

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
                    if (transition.Children.Count > 1)
                    {
                        sb.AppendLine ();
                        sb.Append ($"{indent}    {transition.Children.Count} conflicts");
                    }
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

    public enum AnglrPDADrawingType
    {
        None,
        SyntaxRuleName,
        ConstantSymbol,
        TerminalSymbol,
        NonTerminalSymbol,
        PDATransition,
        PDAState,
        PDASet
    }

    public abstract class AnglrPDABaseDrawing : DrawingVisual
    {
        //
        // common properties
        //

        public static bool Debug { get; set; } = true;
        public static int IdCounter;
        public static CultureInfo CultureInfo { get; set; } = CultureInfo.InvariantCulture;
        public static FlowDirection FlowDirection { get; set; } = FlowDirection.LeftToRight;
        public static string TypefaceName { get; set; } = "Consolas";
        public static int FontSize { get; set; } = 14;
        public static Brush Brush { get; set; } = Brushes.Black;
        public static Pen Pen { get; set; } = new Pen (Brush, 0.5);
        public static Brush TerminalSymbolBackground { get; set; } = Brushes.LightGreen;
        public static Brush ConstantSymbolBackground { get; set; } = Brushes.LightGray;
        public static Brush NonTerminalSymbolBackground { get; set; } = Brushes.LightBlue;
        public static Brush SyntaxRuleBackground { get; set; } = Brushes.Blue;
        public static Brush SyntaxGroupBackground { get; set; } = Brushes.Orange;
        public static int Margin { get; set; } = 4;
        public static int RectangleRadius { get; set; } = 6;
        public static int ConnectorRadius { get; set; } = 6;
        public static int ConnectorLength { get; set; } = 20;

        //
        // object properties
        //

        public Point ConnectorPoint { get; set; }
        public double Width { get; set; }
        public double Height { get; set; }
        public Vector Position { get; set; }
        public Size Size => new Size (Width, Height);
        public Rect Bounds => new Rect ((Point) Position, Size);
        public AnglrPDADrawingType DrawingType { get; }
        public AnglrPDAStateTransitionInfo TransitionInfo { get; }
        public double Opacity { get; protected set; }
        public AnglrPDABaseDrawing (AnglrPDAStateTransitionInfo transitionInfo, AnglrPDADrawingType drawingType)
        {
            TransitionInfo = transitionInfo;
            DrawingType = drawingType;
            Opacity = 1.0;
        }
        public abstract void Draw ();
        public abstract void Display ();

    }

    public class AnglrPDASyntaxRuleNameDrawing : AnglrPDABaseDrawing
    {
        public string Name { get; }
        public int SyntaxRuleNr { get; }
        public int ProductionNr { get; }
        public int StateNr { get; }
        public AnglrPDASyntaxRuleNameDrawing (AnglrPDAStateTransitionInfo transitionInfo) : base (transitionInfo, AnglrPDADrawingType.SyntaxRuleName)
        {
            AnglrGetParserStateProductionData productionData = transitionInfo.Production;
            AnglrGetParserStateSymbolTokenData ruleName = productionData.ProductionName;
            Name = ruleName.Name;
            SyntaxRuleNr = ruleName.Id;
            ProductionNr = productionData.ProductionNumber;
            AnglrGetParserStateItemResult stateInfo = transitionInfo.PDAStates [0];
            StateNr = (stateInfo != null) ? stateInfo.StateNumber : -1;
        }
        public override void Draw ()
        {
            using (var dc = RenderOpen ())
            {
                FormattedText text = new FormattedText
                (
                    Name,
                    CultureInfo,
                    FlowDirection,
                    new Typeface (TypefaceName),
                    FontSize,
                    Brush,
                    0.5
                );
                double width = text.Width;
                double height = text.Height;

                dc.DrawRoundedRectangle (NonTerminalSymbolBackground, Pen, new Rect (0, 0, Width = width + 4 * Margin, Height = height + 4 * Margin), 2 * Margin, 2 * Margin);
                dc.DrawRoundedRectangle (NonTerminalSymbolBackground, Pen, new Rect (Margin, Margin, width + 2 * Margin, height + 2 * Margin), Margin, Margin);
                dc.DrawText (text, new Point (2 * Margin, 2 * Margin));
                ConnectorPoint = new Point (0, Height / 2.0);
            }
            Drawing.Freeze ();
        }
        public override void Display ()
        {
            throw new NotImplementedException ();
        }

    }

    public class AnglrPDATerminalSymbolDrawing : AnglrPDABaseDrawing
    {
        public AnglrGetParserStateItemResult StateInfo { get; }
        public AnglrGetParserStateSymbolTokenData SymbolName { get; }
        public string Name { get; }
        public int TokenCode { get; }
        public int StateNr { get; }
        public AnglrPDATerminalSymbolDrawing
        (
            AnglrPDAStateTransitionInfo transitionInfo,
            AnglrGetParserStateItemResult stateInfo,
            AnglrGetParserStateSymbolTokenData symbolName
        ) : base (transitionInfo, AnglrPDADrawingType.TerminalSymbol)
        {
            StateInfo = stateInfo;
            SymbolName = symbolName;
            Name = symbolName.Name;
            TokenCode = symbolName.Id;
            if ((StateNr = (StateInfo != null) ? StateInfo.StateNumber : -1) < 0)
                Opacity = 0.5;
        }

        public override void Draw ()
        {
            string info =
                (StateNr >= 0) ?
                $"{StateNr} : {Name}" :
                $"{Name}";
            using (var dc = RenderOpen ())
            {
                FormattedText text = new FormattedText
                (
                    info,
                    CultureInfo,
                    FlowDirection,
                    new Typeface (TypefaceName),
                    FontSize,
                    Brush,
                    0.5
                );
                double width = text.Width;
                double height = text.Height;

                dc.DrawRectangle ((StateNr >= 0) ? TerminalSymbolBackground : Brushes.Transparent, Pen, new Rect (0, 0, Width = width + 2 * Margin, Height = height + 2 * Margin));
                dc.DrawText (text, new Point (Margin, Margin));
                ConnectorPoint = new Point (0, Height / 2.0);
            }
            Drawing.Freeze ();
        }
        public override void Display ()
        {
            throw new NotImplementedException ();
        }

    }

    public class AnglrPDAConstantSymbolDrawing : AnglrPDABaseDrawing
    {
        public AnglrGetParserStateItemResult StateInfo { get; }
        public AnglrGetParserStateSymbolTokenData SymbolName { get; }
        public string Name { get; }
        public int TokenCode { get; }
        public int StateNr { get; }
        public AnglrPDAConstantSymbolDrawing
        (
            AnglrPDAStateTransitionInfo transitionInfo,
            AnglrGetParserStateItemResult stateInfo,
            AnglrGetParserStateSymbolTokenData symbolName
        ) : base (transitionInfo, AnglrPDADrawingType.ConstantSymbol)
        {
            StateInfo = stateInfo;
            SymbolName = symbolName;
            Name = symbolName.Synonym;
            TokenCode = symbolName.Id;
            if ((StateNr = (StateInfo != null) ? StateInfo.StateNumber : -1) < 0)
                Opacity = 0.5;
        }

        public override void Draw ()
        {
            string info =
                (StateNr >= 0) ?
                $"{StateNr} : {Name}" :
                $"{Name}";
            using (var dc = RenderOpen ())
            {
                FormattedText text = new FormattedText
                (
                    info,
                    CultureInfo,
                    FlowDirection,
                    new Typeface (TypefaceName),
                    FontSize,
                    Brush,
                    0.5
                );
                double width = text.Width;
                double height = text.Height;

                dc.DrawRectangle ((StateNr >= 0) ? ConstantSymbolBackground : Brushes.Transparent, Pen, new Rect (0, 0, Width = width + 2 * Margin, Height = height + 2 * Margin));
                dc.DrawText (text, new Point (Margin, Margin));
                ConnectorPoint = new Point (0, Height / 2.0);
            }
            Drawing.Freeze ();
        }
        public override void Display ()
        {
            throw new NotImplementedException ();
        }

    }

    public class AnglrPDANonTerminalSymbolDrawing : AnglrPDABaseDrawing
    {
        public AnglrGetParserStateItemResult StateInfo { get; }
        public AnglrGetParserStateSymbolTokenData SymbolName { get; }
        public string Name { get; }
        public int SyntaxRuleNr { get; }
        public int StateNr { get; }
        public AnglrPDANonTerminalSymbolDrawing
        (
            AnglrPDAStateTransitionInfo transitionInfo,
            AnglrGetParserStateItemResult stateInfo,
            AnglrGetParserStateSymbolTokenData symbolName
        ) : base (transitionInfo, AnglrPDADrawingType.NonTerminalSymbol)
        {
            StateInfo = stateInfo;
            SymbolName = symbolName;
            Name = symbolName.Name;
            SyntaxRuleNr = symbolName.Id;
            if ((StateNr = (StateInfo != null) ? StateInfo.StateNumber : -1) < 0)
                Opacity = 0.5;
        }

        public override void Draw ()
        {
            using (var dc = RenderOpen ())
            {
                string info =
                    (StateNr >= 0) ?
                    $"{StateNr} : {Name}" :
                    $"{Name}";
                FormattedText text = new FormattedText
                (
                    info,
                    CultureInfo,
                    FlowDirection,
                    new Typeface (TypefaceName),
                    FontSize,
                    Brush,
                    0.5
                );
                double width = text.Width;
                double height = text.Height;

                dc.DrawRoundedRectangle ((StateNr >= 0) ? NonTerminalSymbolBackground : Brushes.Transparent, Pen, new Rect (0, 0, Width = width + 2 * Margin, Height = height + 2 * Margin), Margin, Margin);
                dc.DrawText (text, new Point (Margin, Margin));
                ConnectorPoint = new Point (0, Height / 2.0);
            }
            Drawing.Freeze ();
        }
        public override void Display ()
        {
            throw new NotImplementedException ();
        }

    }

    public class AnglrPDATransitionInfoDrawing : AnglrPDABaseDrawing
    {
        public AnglrPDASyntaxRuleNameDrawing RuleName { get; }
        public List<AnglrPDABaseDrawing> ProductionParts { get; }
        public AnglrPDATransitionInfoDrawing (AnglrPDAStateTransitionInfo transitionInfo) : base (transitionInfo, AnglrPDADrawingType.PDATransition)
        {
            ProductionParts = new List<AnglrPDABaseDrawing> ();
            RuleName = new AnglrPDASyntaxRuleNameDrawing (transitionInfo);
            AnglrPDABaseDrawing drawing = null;
            int index = 0;
            int count = transitionInfo.PDAStates.Count;
            foreach (var rhsNode in transitionInfo.Production.RhsNodeSet)
            {
                AnglrGetParserStateItemResult stateInfo = (index < count) ? transitionInfo.PDAStates [index++] : null;
                if (rhsNode.Declarator == 18)
                {
                    if ((rhsNode.Synonym != null) && (rhsNode.Synonym.Length > 0))
                        drawing = new AnglrPDAConstantSymbolDrawing (transitionInfo, stateInfo, rhsNode);
                    else
                        drawing = new AnglrPDATerminalSymbolDrawing (transitionInfo, stateInfo, rhsNode);
                }
                else
                    drawing = new AnglrPDANonTerminalSymbolDrawing (transitionInfo, stateInfo, rhsNode);
                ProductionParts.Add (drawing);
            }
        }

        public override void Draw ()
        {
            RuleName.Draw ();
            double drawingHeight = 0;
            foreach (var drawing in ProductionParts)
            {
                drawing.Draw ();
                drawingHeight = Math.Max (drawingHeight, drawing.Height);
            }
            double height = RuleName.Height + drawingHeight + 4 * Margin;
            double width = 4 * Margin;
            using (var dc = RenderOpen ())
            {
                dc.PushTransform (new TranslateTransform (0, 2 * Margin));
                dc.DrawDrawing (RuleName.Drawing);
                dc.PushTransform (new TranslateTransform (0, RuleName.Height + 2 * Margin));
                foreach (var drawing in ProductionParts)
                {
                    dc.PushTransform (new TranslateTransform (width, (drawingHeight - drawing.Height) / 2));
                    dc.PushOpacity (Opacity);
                    dc.DrawDrawing (drawing.Drawing);
                    dc.Pop ();
                    dc.Pop ();
                    width += drawing.Width + 4 * Margin;
                }
                dc.Pop ();
                dc.Pop ();
            }
            Drawing.Freeze ();

            Width = Math.Max (RuleName.Width + 2 * Margin, width);
            Height = height;
        }
        public override void Display ()
        {
            throw new NotImplementedException ();
        }

    }

    public class AnglrPDAStateDrawing : AnglrPDABaseDrawing
    {
        public AnglrStateItemResultList StateItemResults { get; }
        public List<AnglrPDATransitionInfoDrawing> TransitionInfoDrawings { get; }

        public AnglrPDAStateDrawing (AnglrStateItemResultList stateItemResults) : base (null, AnglrPDADrawingType.PDAState)
        {
            StateItemResults = stateItemResults;
            TransitionInfoDrawings = new List<AnglrPDATransitionInfoDrawing> ();
        }
        public void Add (AnglrPDATransitionInfoDrawing transitionInfoDrawing) => TransitionInfoDrawings.Add (transitionInfoDrawing);
        public override void Draw ()
        {
            string states = (StateItemResults.Count > 1) ? "states" : "state";
            foreach (var stateItem in StateItemResults)
                states += $" {stateItem.StateNumber}";
            FormattedText text = new FormattedText
            (
                states,
                CultureInfo,
                FlowDirection,
                new Typeface (TypefaceName),
                FontSize,
                Brush,
                0.5
            );
            double width = text.Width;
            double height = text.Height;
            foreach (var transition in TransitionInfoDrawings)
                transition.Draw ();
            using (var dc = RenderOpen ())
            {
                dc.DrawText (text, new Point (Margin, Margin));
                width += Margin;
                height += 2 * Margin;
                foreach (var transition in TransitionInfoDrawings)
                {
                    dc.PushTransform (new TranslateTransform (0, height));
                    dc.DrawDrawing (transition.Drawing);
                    dc.Pop ();
                    height += transition.Height + 2 * Margin;
                    width=Math.Max (width, transition.Width);
                }
                dc.PushTransform (new TranslateTransform (0, Margin));
                dc.DrawRoundedRectangle (Brushes.Transparent, Pen, new Rect (0, 0, width, height - Margin), 2 * Margin, 2 * Margin);
                dc.Pop ();
            }
            Width = width;
            Height= height;
        }
        public override void Display ()
        {
            throw new NotImplementedException ();
        }

    }

    public class AnglrPDASetDrawing : AnglrPDABaseDrawing
    {
        public List<AnglrPDAStateDrawing> PDAStatesDrawings { get; }
        public AnglrPDASetDrawing (AnglrPDASet pdaSet) : base (null, AnglrPDADrawingType.PDASet)
        {
            List<AnglrPDATransitionInfoDrawing> PDASet = new List<AnglrPDATransitionInfoDrawing> ();
            foreach (var element in pdaSet)
                if (element.Parent == null)
                    element.Traverse
                    (
                        (info, data) => PDASet.Add (new AnglrPDATransitionInfoDrawing (info)),
                        null
                    );
            PDAStatesDrawings = new List<AnglrPDAStateDrawing> ();
            AnglrStateItemResultList stateItemResults = new AnglrStateItemResultList ();
            AnglrStateItemResultListComparer comparer = new AnglrStateItemResultListComparer ();
            AnglrPDAStateDrawing stateSet = null;
            foreach (var drawing in PDASet)
            {
                if (comparer.Compare (drawing.TransitionInfo.PDAStates, stateItemResults) != 0)
                    PDAStatesDrawings.Add (stateSet = new AnglrPDAStateDrawing (stateItemResults = drawing.TransitionInfo.PDAStates));
                stateSet.Add (drawing);
            }
        }

        public override void Draw ()
        {
            foreach (var drawing in PDAStatesDrawings)
                drawing.Draw ();

            double height = 0;
            double width = 0;
            using (var dc = RenderOpen ())
            {
                foreach (var drawing in PDAStatesDrawings)
                {
                    dc.PushTransform (new TranslateTransform (0, height));
                    dc.DrawDrawing (drawing.Drawing);
                    dc.Pop ();
                    width = Math.Max (width, drawing.Width);
                    height += drawing.Height + 2 * Margin;
                }
            }
            Drawing.Freeze ();

            Width = width;
            Height = height;
        }
        public override void Display ()
        {
            throw new NotImplementedException ();
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
                    AnglrPDASet pdaSet = null;
                    if (true)
                    {
                        foreach (var pda in pdaReducedList)
                        {
                            pda?.Display ("PDA CELL STATE");
                            pdaSet = pda;
                            break;
                        }
                    }
                    if (pdaSet != null)
                        Dispatcher.Invoke (() =>
                        {
                            pdaStateViewer.Clear ();
                            AnglrPDASetDrawing setDrawing = new AnglrPDASetDrawing (pdaSet);
                            setDrawing.Draw ();
                            pdaStateViewer.AddVisual (setDrawing);
                            pdaStateViewer.Width = setDrawing.Width;
                            pdaStateViewer.Height = setDrawing.Height;
                            Logger?.InfoLine ($"PDA STATE VIEWER: (W = {setDrawing.Width}, H = {setDrawing.Height})");
                        });
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
