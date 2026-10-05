using Anglr.Parser;
using Anglr.Parser.Core;
using Anglr.Parser.SyntaxTree;
using AnglrJsonRpcMethods;
using AnglrLibrary;
using AnglrLogLibrary;
using AnglrParserLibrary;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Diagnostics.SymbolStore;
using System.Drawing.Drawing2D;
using System.Globalization;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Media3D;

namespace AnglrLangExtension
{
    public class AnglrVisualPosition : Stack<Vector>
    {
        public Vector Position { get; private set; }
        public AnglrVisualPosition () => Push (Position = new Vector (0, 0));
        public Vector PushPosition (Vector vector)
        {
            Push (vector);
            Position += vector;
            return Position;
        }
        public Vector PushPosition (double x, double y) => PushPosition (new Vector (x, y));
        public Vector PopPosition ()
        {
            if (Count <= 1)
                return Position;
            Vector vector = Pop ();
            Position -= vector;
            return Position;
        }
    }

    public class HitList : Stack<AnglrRawDrawingVisual> { }

    public class AnglrDrawingVisual : DrawingVisual
    {
        //
        // common properties
        //

        public static bool Debug { get; set; } = true;
        public static CultureInfo CultureInfo { get; set; } = CultureInfo.InvariantCulture;
        public static FlowDirection FlowDirection { get; set; } = FlowDirection.LeftToRight;
        public static string TypefaceName { get; set; } = "Consolas";
        public static int FontSize { get; set; } = 14;
        public static Brush Brush { get; set; } = Brushes.Black;
        public static Pen Pen { get; set; } = new Pen (Brush, 1);
        public static Brush TerminalSymbolBackground { get; set; } = Brushes.LightGreen;
        public static Brush ConstantSymbolBackground { get; set; } = Brushes.LightGray;
        public static Brush NonTerminalSymbolBackground { get; set; } = Brushes.LightBlue;
        public static int Margin { get; set; } = 2;
        public static int RectangleRadius { get; set; } = 6;
        public static int ConnectorLength { get; set; } = 20;
        public static double OpacityValue { get; set; } = 0.3;

        //
        // object properties
        //

        public int Index { get; protected set; }
    }

    public abstract class AnglrRawDrawingVisual : AnglrDrawingVisual
    {
        //
        // common properties
        //

        public static int IdCounter;
        public static Brush SyntaxRuleBackground { get; set; } = Brushes.Blue;
        public static Brush SyntaxGroupBackground { get; set; } = Brushes.Orange;
        public static int ConnectorRadius { get; set; } = 6;

        //
        // object properties
        //

        public int Id { get; private set; }
        public IAnglrLogger Logger { get; set; }
        public double ConnectorOffset { get; set; }
        public double Width { get; set; }
        public double Height { get; set; }
        public Vector Position { get; set; }
        public Size Size => new Size (Width, Height);
        public Rect Bounds => new Rect ((Point) Position, Size);
        public AnglrRawDrawingVisual AnglrVisualParent
        {
            get => _AnglrVisualParent;
            set => (_AnglrVisualParent = value).AnglrVisualChildren.Add (this);
        }
        public List<AnglrRawDrawingVisual> AnglrVisualChildren { get; } = new List<AnglrRawDrawingVisual> ();
        private AnglrRawDrawingVisual _AnglrVisualParent = null;

        public AnglrRawDrawingVisual (IAnglrLogger logger)
        {
            Id = ++IdCounter;
            Logger = logger ?? new VoidAnglrLogger ();
            Position = new Vector (0, 0);
        }

        public void ComputeVisualBounds ()
        {
            _ComputeVisualBounds (new AnglrVisualPosition ());
        }

        private void _ComputeVisualBounds (AnglrVisualPosition visualPosition)
        {
            Position = visualPosition.PushPosition (Position);
            foreach (var child in AnglrVisualChildren)
            {
                child._ComputeVisualBounds (visualPosition);
            }
            visualPosition.PopPosition ();
        }

        public HitList HitTest (object sender, MouseEventArgs e, Point point, AnglrMouseEventKind mouseEventKind)
        {
            try
            {
                HitList hitVisuals = new HitList ();
                _HitTest (hitVisuals, point);
                foreach (var visual in hitVisuals)
                {
                    switch (mouseEventKind)
                    {
                        case AnglrMouseEventKind.None:
                            break;
                        case AnglrMouseEventKind.MouseDown:
                            (visual as IAnglrEventHandler)?.OnMouseDown (sender, e as MouseButtonEventArgs, point, Logger);
                            break;
                        case AnglrMouseEventKind.MouseEnter:
                            (visual as IAnglrEventHandler)?.OnMouseEnter (sender, e, point, Logger);
                            break;
                        case AnglrMouseEventKind.MouseLeave:
                            (visual as IAnglrEventHandler)?.OnMouseLeave (sender, e, point, Logger);
                            break;
                        case AnglrMouseEventKind.MouseLeftButttonDown:
                            (visual as IAnglrEventHandler)?.OnMouseLeftButtonDown (sender, e as MouseButtonEventArgs, point, Logger);
                            break;
                        case AnglrMouseEventKind.MouseLeftButttonUp:
                            (visual as IAnglrEventHandler)?.OnMouseLeftButtonUp (sender, e as MouseButtonEventArgs, point, Logger);
                            break;
                        case AnglrMouseEventKind.MouseMove:
                            (visual as IAnglrEventHandler)?.OnMouseMove (sender, e, point, Logger);
                            break;
                        case AnglrMouseEventKind.MouseRightButttonDown:
                            (visual as IAnglrEventHandler)?.OnMouseRightButtonDown (sender, e as MouseButtonEventArgs, point, Logger);
                            break;
                        case AnglrMouseEventKind.MouseRightButttonUp:
                            (visual as IAnglrEventHandler)?.OnMouseRightButtonUp (sender, e as MouseButtonEventArgs, point, Logger);
                            break;
                        case AnglrMouseEventKind.MouseUp:
                            (visual as IAnglrEventHandler)?.OnMouseUp (sender, e as MouseButtonEventArgs, point, Logger);
                            break;
                        case AnglrMouseEventKind.MouseWheel:
                            (visual as IAnglrEventHandler)?.OnMouseWheel (sender, e as MouseWheelEventArgs, point, Logger);
                            break;
                    }
                }
                return hitVisuals;
            }
            catch (Exception ex)
            {
                Logger?.ErrorLine (ex, "Hit test failed");
                return null;
            }
        }

        public bool _HitTest (HitList hitVisuals, Point point)
        {
            if (!Bounds.Contains (point))
                return false;
            hitVisuals.Push (this);
            foreach (var child in AnglrVisualChildren)
            {
                if (child._HitTest (hitVisuals, point))
                    break;
            }
            return true;
        }

        public abstract void Draw ();
        public abstract void Display ();
    }

    public interface IAnglrVisualCloneable
    {
        AnglrDrawingVisual Clone ();
    }

    public interface IAnglrEventHandler
    {
        void OnMouseDown (object sender, MouseButtonEventArgs e, Point point, IAnglrLogger logger);
        void OnMouseEnter (object sender, MouseEventArgs e, Point point, IAnglrLogger logger);
        void OnMouseLeave (object sender, MouseEventArgs e, Point point, IAnglrLogger logger);
        void OnMouseLeftButtonDown (object sender, MouseButtonEventArgs e, Point point, IAnglrLogger logger);
        void OnMouseLeftButtonUp (object sender, MouseButtonEventArgs e, Point point, IAnglrLogger logger);
        void OnMouseMove (object sender, MouseEventArgs e, Point point, IAnglrLogger logger);
        void OnMouseRightButtonDown (object sender, MouseButtonEventArgs e, Point point, IAnglrLogger logger);
        void OnMouseRightButtonUp (object sender, MouseButtonEventArgs e, Point point, IAnglrLogger logger);
        void OnMouseUp (object sender, MouseButtonEventArgs e, Point point, IAnglrLogger logger);
        void OnMouseWheel (object sender, MouseWheelEventArgs e, Point point, IAnglrLogger logger);
    }

    public class AnglrDrawingDictionary : Dictionary<int, AnglrContainerSymbolVisual> { }

    public enum AnglrMouseEventKind
    {
        None = 0,
        MouseDown,
        MouseEnter,
        MouseLeave,
        MouseLeftButttonDown,
        MouseLeftButttonUp,
        MouseMove,
        MouseRightButttonDown,
        MouseRightButttonUp,
        MouseUp,
        MouseWheel
    }

    public class AnglrSyntaxProductionNameVisual : AnglrDrawingVisual, IAnglrVisualCloneable, IAnglrEventHandler
    {
        public void Draw (string name)
        {
            Index = -1;
            using (var dc = RenderOpen ())
            {
                FormattedText text = new FormattedText
                (
                    name,
                    CultureInfo,
                    FlowDirection,
                    new Typeface (TypefaceName),
                    FontSize,
                    Brush,
                    1.0
                );
                double width = text.Width;
                double height = text.Height;

                dc.DrawRoundedRectangle (NonTerminalSymbolBackground, Pen, new Rect (0, 0, width + 4 * Margin, height + 4 * Margin), RectangleRadius, RectangleRadius);
                dc.DrawRoundedRectangle (NonTerminalSymbolBackground, Pen, new Rect (Margin, Margin, width + 2 * Margin, height + 2 * Margin), RectangleRadius, RectangleRadius);
                dc.DrawText (text, new Point (2 * Margin, 2 * Margin));
            }
            Drawing.Freeze ();
        }

        public AnglrDrawingVisual Clone ()
        {
            AnglrSyntaxProductionNameVisual cloned = new AnglrSyntaxProductionNameVisual ();
            cloned.Index = Index;
            if (Transform != null)
                cloned.Transform = Transform.Clone ();
            cloned.Offset = Offset;
            using (var dc = cloned.RenderOpen ())
            {
                dc.DrawDrawing (Drawing.Clone ());
            }
            cloned.Drawing.Freeze ();
            return cloned;
        }

        public void OnMouseDown (object sender, MouseButtonEventArgs e, Point point, IAnglrLogger logger) { if (Debug) logger?.DebugLine ($"mouse down in terminal symbol nr. {Index} at ({point})"); }
        public void OnMouseEnter (object sender, MouseEventArgs e, Point point, IAnglrLogger logger) { }
        public void OnMouseLeave (object sender, MouseEventArgs e, Point point, IAnglrLogger logger) { }
        public void OnMouseLeftButtonDown (object sender, MouseButtonEventArgs e, Point point, IAnglrLogger logger) { }
        public void OnMouseLeftButtonUp (object sender, MouseButtonEventArgs e, Point point, IAnglrLogger logger) { }
        public void OnMouseMove (object sender, MouseEventArgs e, Point point, IAnglrLogger logger) { }
        public void OnMouseRightButtonDown (object sender, MouseButtonEventArgs e, Point point, IAnglrLogger logger) { }
        public void OnMouseRightButtonUp (object sender, MouseButtonEventArgs e, Point point, IAnglrLogger logger) { }
        public void OnMouseUp (object sender, MouseButtonEventArgs e, Point point, IAnglrLogger logger) { }
        public void OnMouseWheel (object sender, MouseWheelEventArgs e, Point point, IAnglrLogger logger) { }
    }

    public class AnglrTerminalSymbolVisual : AnglrDrawingVisual, IAnglrVisualCloneable, IAnglrEventHandler
    {
        public void Draw (string name, int index)
        {
            Index = index;
            using (var dc = RenderOpen ())
            {
                FormattedText text = new FormattedText
                (
                    name,
                    CultureInfo,
                    FlowDirection,
                    new Typeface (TypefaceName),
                    FontSize,
                    Brush,
                    1.0
                );
                double width = text.Width;
                double height = text.Height;

                dc.DrawLine (Pen, new Point (0, (height + 2 * Margin) / 2), new Point (ConnectorLength, (height + 2 * Margin) / 2));
                dc.PushTransform (new TranslateTransform (ConnectorLength, 0));
                dc.DrawRectangle (TerminalSymbolBackground, Pen, new Rect (0, 0, width + 2 * Margin, height + 2 * Margin));
                dc.DrawText (text, new Point (Margin, Margin));
                dc.Pop ();
            }
            Drawing.Freeze ();
        }

        public AnglrDrawingVisual Clone ()
        {
            AnglrTerminalSymbolVisual cloned = new AnglrTerminalSymbolVisual ();
            cloned.Index = Index;
            if (Transform != null)
                cloned.Transform = Transform.Clone ();
            cloned.Offset = Offset;
            using (var dc = cloned.RenderOpen ())
            {
                dc.DrawDrawing (Drawing.Clone ());
            }
            cloned.Drawing.Freeze ();
            return cloned;
        }

        public void OnMouseDown (object sender, MouseButtonEventArgs e, Point point, IAnglrLogger logger) { if (Debug) logger?.DebugLine ($"mouse down in terminal symbol nr. {Index} at ({point})"); }
        public void OnMouseEnter (object sender, MouseEventArgs e, Point point, IAnglrLogger logger) { }
        public void OnMouseLeave (object sender, MouseEventArgs e, Point point, IAnglrLogger logger) { }
        public void OnMouseLeftButtonDown (object sender, MouseButtonEventArgs e, Point point, IAnglrLogger logger) { }
        public void OnMouseLeftButtonUp (object sender, MouseButtonEventArgs e, Point point, IAnglrLogger logger) { }
        public void OnMouseMove (object sender, MouseEventArgs e, Point point, IAnglrLogger logger) { }
        public void OnMouseRightButtonDown (object sender, MouseButtonEventArgs e, Point point, IAnglrLogger logger) { }
        public void OnMouseRightButtonUp (object sender, MouseButtonEventArgs e, Point point, IAnglrLogger logger) { }
        public void OnMouseUp (object sender, MouseButtonEventArgs e, Point point, IAnglrLogger logger) { }
        public void OnMouseWheel (object sender, MouseWheelEventArgs e, Point point, IAnglrLogger logger) { }
    }

    public class AnglrConstantSymbolVisual : AnglrDrawingVisual, IAnglrVisualCloneable, IAnglrEventHandler
    {
        public void Draw (string name, int index)
        {
            Index = index;
            using (var dc = RenderOpen ())
            {
                FormattedText text = new FormattedText
                (
                    name,
                    CultureInfo,
                    FlowDirection,
                    new Typeface (TypefaceName),
                    FontSize,
                    Brush,
                    1.0
                );
                double width = text.Width;
                double height = text.Height;

                dc.DrawLine (Pen, new Point (0, (height + 2 * Margin) / 2), new Point (ConnectorLength, (height + 2 * Margin) / 2));
                dc.PushTransform (new TranslateTransform (ConnectorLength, 0));
                dc.DrawRectangle (ConstantSymbolBackground, Pen, new Rect (0, 0, width + 2 * Margin, height + 2 * Margin));
                dc.DrawText (text, new Point (Margin, Margin));
                dc.Pop ();
            }
            Drawing.Freeze ();
        }

        public AnglrDrawingVisual Clone ()
        {
            AnglrConstantSymbolVisual cloned = new AnglrConstantSymbolVisual ();
            cloned.Index = Index;
            if (Transform != null)
                cloned.Transform = Transform.Clone ();
            cloned.Offset = Offset;
            using (var dc = cloned.RenderOpen ())
            {
                dc.DrawDrawing (Drawing.Clone ());
            }
            cloned.Drawing.Freeze ();
            return cloned;
        }

        public void OnMouseDown (object sender, MouseButtonEventArgs e, Point point, IAnglrLogger logger) { if (Debug) logger?.DebugLine ($"mouse down in constant symbol nr. {Index} at ({point})"); }
        public void OnMouseEnter (object sender, MouseEventArgs e, Point point, IAnglrLogger logger) { }
        public void OnMouseLeave (object sender, MouseEventArgs e, Point point, IAnglrLogger logger) { }
        public void OnMouseLeftButtonDown (object sender, MouseButtonEventArgs e, Point point, IAnglrLogger logger) { }
        public void OnMouseLeftButtonUp (object sender, MouseButtonEventArgs e, Point point, IAnglrLogger logger) { }
        public void OnMouseMove (object sender, MouseEventArgs e, Point point, IAnglrLogger logger) { }
        public void OnMouseRightButtonDown (object sender, MouseButtonEventArgs e, Point point, IAnglrLogger logger) { }
        public void OnMouseRightButtonUp (object sender, MouseButtonEventArgs e, Point point, IAnglrLogger logger) { }
        public void OnMouseUp (object sender, MouseButtonEventArgs e, Point point, IAnglrLogger logger) { }
        public void OnMouseWheel (object sender, MouseWheelEventArgs e, Point point, IAnglrLogger logger) { }
    }

    public class AnglrNonTerminalSymbolVisual : AnglrDrawingVisual, IAnglrVisualCloneable, IAnglrEventHandler
    {
        public void Draw (string name, int index)
        {
            Index = index;
            using (var dc = RenderOpen ())
            {
                FormattedText text = new FormattedText
                (
                    name,
                    CultureInfo,
                    FlowDirection,
                    new Typeface (TypefaceName),
                    FontSize,
                    Brush,
                    1.0
                );
                double width = text.Width;
                double height = text.Height;

                dc.DrawLine (Pen, new Point (0, (height + 2 * Margin) / 2), new Point (ConnectorLength, (height + 2 * Margin) / 2));
                dc.PushTransform (new TranslateTransform (ConnectorLength, 0));
                dc.DrawRoundedRectangle (NonTerminalSymbolBackground, Pen, new Rect (0, 0, width + 2 * Margin, height + 2 * Margin), RectangleRadius, RectangleRadius);
                dc.DrawText (text, new Point (Margin, Margin));
                dc.Pop ();
            }
            Drawing.Freeze ();
        }

        public AnglrDrawingVisual Clone ()
        {
            AnglrNonTerminalSymbolVisual cloned = new AnglrNonTerminalSymbolVisual ();
            cloned.Index = Index;
            if (Transform != null)
                cloned.Transform = Transform.Clone ();
            cloned.Offset = Offset;
            using (var dc = cloned.RenderOpen ())
            {
                dc.DrawDrawing (Drawing.Clone ());
            }
            cloned.Drawing.Freeze ();
            return cloned;
        }

        public void OnMouseDown (object sender, MouseButtonEventArgs e, Point point, IAnglrLogger logger) { if (Debug) logger?.DebugLine ($"mouse down in non-terminal symbol nr. {Index} at ({point})"); }
        public void OnMouseEnter (object sender, MouseEventArgs e, Point point, IAnglrLogger logger) { }
        public void OnMouseLeave (object sender, MouseEventArgs e, Point point, IAnglrLogger logger) { }
        public void OnMouseLeftButtonDown (object sender, MouseButtonEventArgs e, Point point, IAnglrLogger logger) { }
        public void OnMouseLeftButtonUp (object sender, MouseButtonEventArgs e, Point point, IAnglrLogger logger) { }
        public void OnMouseMove (object sender, MouseEventArgs e, Point point, IAnglrLogger logger) { }
        public void OnMouseRightButtonDown (object sender, MouseButtonEventArgs e, Point point, IAnglrLogger logger) { }
        public void OnMouseRightButtonUp (object sender, MouseButtonEventArgs e, Point point, IAnglrLogger logger) { }
        public void OnMouseUp (object sender, MouseButtonEventArgs e, Point point, IAnglrLogger logger) { }
        public void OnMouseWheel (object sender, MouseWheelEventArgs e, Point point, IAnglrLogger logger) { }
    }

    public class AnglrContainerSymbolVisual : AnglrDrawingVisual, IAnglrVisualCloneable, IAnglrEventHandler
    {
        public AnglrDrawingVisual Clone ()
        {
            AnglrContainerSymbolVisual cloned = new AnglrContainerSymbolVisual ();
            cloned.Index = Index;
            if (Transform != null)
                cloned.Transform = Transform.Clone ();
            cloned.Offset = Offset;
            foreach (var child in Children)
            {
                if (!(child is IAnglrVisualCloneable))
                    continue;
                cloned.Children.Add ((child as IAnglrVisualCloneable)?.Clone ());
            }
            return cloned;
        }

        public void OnMouseDown (object sender, MouseButtonEventArgs e, Point point, IAnglrLogger logger) { logger?.DebugLine ($"mouse down in container nr. {Index} at ({point})"); }
        public void OnMouseEnter (object sender, MouseEventArgs e, Point point, IAnglrLogger logger) { }
        public void OnMouseLeave (object sender, MouseEventArgs e, Point point, IAnglrLogger logger) { }
        public void OnMouseLeftButtonDown (object sender, MouseButtonEventArgs e, Point point, IAnglrLogger logger) { }
        public void OnMouseLeftButtonUp (object sender, MouseButtonEventArgs e, Point point, IAnglrLogger logger) { }
        public void OnMouseMove (object sender, MouseEventArgs e, Point point, IAnglrLogger logger) { }
        public void OnMouseRightButtonDown (object sender, MouseButtonEventArgs e, Point point, IAnglrLogger logger) { }
        public void OnMouseRightButtonUp (object sender, MouseButtonEventArgs e, Point point, IAnglrLogger logger) { }
        public void OnMouseUp (object sender, MouseButtonEventArgs e, Point point, IAnglrLogger logger) { }
        public void OnMouseWheel (object sender, MouseWheelEventArgs e, Point point, IAnglrLogger logger) { }
    }

    public class AnglrRawTerminalSymbolVisual : AnglrRawDrawingVisual, IAnglrEventHandler
    {
        public SimpleSymbolToken SymbolToken { get; private set; }
        public AnglrRawTerminalSymbolVisual (IAnglrLogger logger, SimpleSymbolToken symbolToken) : base (logger)
        {
            SymbolToken = symbolToken;
        }
        public AnglrRawTerminalSymbolVisual (AnglrRawTerminalSymbolVisual terminalSymbol) : base (terminalSymbol.Logger)
        {
            SymbolToken = terminalSymbol.SymbolToken;
        }

        public override void Draw ()
        {
            using (var dc = RenderOpen ())
            {
                FormattedText text = new FormattedText
                (
                    SymbolToken.Name,
                    CultureInfo,
                    FlowDirection,
                    new Typeface (TypefaceName),
                    FontSize,
                    Brush,
                    1.0
                );
                double width = text.Width;
                double height = text.Height;

                dc.DrawRectangle (TerminalSymbolBackground, Pen, new Rect (0, 0, Width = width + 2 * Margin, Height = height + 2 * Margin));
                dc.DrawText (text, new Point (Margin, Margin));
                ConnectorOffset = Height / 2.0;
            }
            Drawing.Freeze ();
        }

        public override void Display ()
        {
            if (Debug)
                Logger?.InfoLine ($"terminal symbol, id = {Id}, position = {Position}, size = {Size}, name = {SymbolToken.Name}");
        }

        public void OnMouseDown (object sender, MouseButtonEventArgs e, Point point, IAnglrLogger logger)
        {
            if (Debug)
                Logger?.InfoLine ($"OnMouseDown (terminal symbol, id = {Id}, value =  {SymbolToken.Name})");
        }

        public void OnMouseEnter (object sender, MouseEventArgs e, Point point, IAnglrLogger logger)
        {
            if (Debug)
                Logger?.InfoLine ($"OnMouseEnter (terminal symbol, id = {Id}, value =  {SymbolToken.Name})");
        }

        public void OnMouseLeave (object sender, MouseEventArgs e, Point point, IAnglrLogger logger)
        {
            if (Debug)
                Logger?.InfoLine ($"OnMouseLeave (terminal symbol, id = {Id}, value =  {SymbolToken.Name})");
        }

        public void OnMouseLeftButtonDown (object sender, MouseButtonEventArgs e, Point point, IAnglrLogger logger)
        {
            if (Debug)
                Logger?.InfoLine ($"OnMouseLeftButtonDown (terminal symbol, id = {Id}, value =  {SymbolToken.Name})");
        }

        public void OnMouseLeftButtonUp (object sender, MouseButtonEventArgs e, Point point, IAnglrLogger logger)
        {
            if (Debug)
                Logger?.InfoLine ($"OnMouseLeftButtonUp (terminal symbol, id = {Id}, value =  {SymbolToken.Name})");
        }

        public void OnMouseMove (object sender, MouseEventArgs e, Point point, IAnglrLogger logger)
        {
            if (Debug)
                Logger?.InfoLine ($"OnMouseMove (terminal symbol, id = {Id}, value =  {SymbolToken.Name})");
        }

        public void OnMouseRightButtonDown (object sender, MouseButtonEventArgs e, Point point, IAnglrLogger logger)
        {
            if (Debug)
                Logger?.InfoLine ($"OnMouseRightButtonDown (terminal symbol, id = {Id}, value =  {SymbolToken.Name})");
        }

        public void OnMouseRightButtonUp (object sender, MouseButtonEventArgs e, Point point, IAnglrLogger logger)
        {
            if (Debug)
                Logger?.InfoLine ($"OnMouseRightButtonUp (terminal symbol, id = {Id}, value =  {SymbolToken.Name})");
        }

        public void OnMouseUp (object sender, MouseButtonEventArgs e, Point point, IAnglrLogger logger)
        {
            if (Debug)
                Logger?.InfoLine ($"OnMouseUp (terminal symbol, id = {Id}, value =  {SymbolToken.Name})");
        }
        public void OnMouseWheel (object sender, MouseWheelEventArgs e, Point point, IAnglrLogger logger)
        {
            if (Debug)
                Logger?.InfoLine ($"OnMouseWheel (terminal symbol, id = {Id}, value =  {SymbolToken.Name})");
        }
    }

    public class AnglrRawConstantSymbolVisual : AnglrRawDrawingVisual, IAnglrEventHandler
    {
        public SimpleSymbolToken SymbolToken { get; private set; }
        public AnglrRawConstantSymbolVisual (IAnglrLogger logger, SimpleSymbolToken symbolToken) : base (logger)
        {
            SymbolToken = symbolToken;
        }
        public AnglrRawConstantSymbolVisual (AnglrRawConstantSymbolVisual terminalSymbol) : base (terminalSymbol.Logger)
        {
            SymbolToken = terminalSymbol.SymbolToken;
        }

        public override void Draw ()
        {
            using (var dc = RenderOpen ())
            {
                FormattedText text = new FormattedText
                (
                    SymbolToken.Name,
                    CultureInfo,
                    FlowDirection,
                    new Typeface (TypefaceName),
                    FontSize,
                    Brush,
                    1.0
                );
                double width = text.Width;
                double height = text.Height;

                dc.DrawRectangle (ConstantSymbolBackground, Pen, new Rect (0, 0, Width = width + 2 * Margin, Height = height + 2 * Margin));
                dc.DrawText (text, new Point (Margin, Margin));
                ConnectorOffset = Height / 2.0;
            }
            Drawing.Freeze ();
        }

        public override void Display ()
        {
            if (Debug)
                Logger?.InfoLine ($"constant symbol, id = {Id}, position = {Position}, size = {Size}, name = {SymbolToken.Name}");
        }

        public void OnMouseDown (object sender, MouseButtonEventArgs e, Point point, IAnglrLogger logger)
        {
            if (Debug)
                Logger?.InfoLine ($"OnMouseDown (constant symbol, id = {Id}, value =  {SymbolToken.Name})");
        }

        public void OnMouseEnter (object sender, MouseEventArgs e, Point point, IAnglrLogger logger)
        {
            if (Debug)
                Logger?.InfoLine ($"OnMouseEnter (constant symbol, id = {Id}, value =  {SymbolToken.Name})");
        }

        public void OnMouseLeave (object sender, MouseEventArgs e, Point point, IAnglrLogger logger)
        {
            if (Debug)
                Logger?.InfoLine ($"OnMouseLeave (constant symbol, id = {Id}, value =  {SymbolToken.Name})");
        }

        public void OnMouseLeftButtonDown (object sender, MouseButtonEventArgs e, Point point, IAnglrLogger logger)
        {
            if (Debug)
                Logger?.InfoLine ($"OnMouseLeftButtonDown (constant symbol, id = {Id}, value =  {SymbolToken.Name})");
        }

        public void OnMouseLeftButtonUp (object sender, MouseButtonEventArgs e, Point point, IAnglrLogger logger)
        {
            if (Debug)
                Logger?.InfoLine ($"OnMouseLeftButtonUp (constant symbol, id = {Id}, value =  {SymbolToken.Name})");
        }

        public void OnMouseMove (object sender, MouseEventArgs e, Point point, IAnglrLogger logger)
        {
            if (Debug)
                Logger?.InfoLine ($"OnMouseMove (constant symbol, id = {Id}, value =  {SymbolToken.Name})");
        }

        public void OnMouseRightButtonDown (object sender, MouseButtonEventArgs e, Point point, IAnglrLogger logger)
        {
            if (Debug)
                Logger?.InfoLine ($"OnMouseRightButtonDown (constant symbol, id = {Id}, value =  {SymbolToken.Name})");
        }

        public void OnMouseRightButtonUp (object sender, MouseButtonEventArgs e, Point point, IAnglrLogger logger)
        {
            if (Debug)
                Logger?.InfoLine ($"OnMouseRightButtonUp (constant symbol, id = {Id}, value =  {SymbolToken.Name})");
        }

        public void OnMouseUp (object sender, MouseButtonEventArgs e, Point point, IAnglrLogger logger)
        {
            if (Debug)
                Logger?.InfoLine ($"OnMouseUp (constant symbol, id = {Id}, value =  {SymbolToken.Name})");
        }
        public void OnMouseWheel (object sender, MouseWheelEventArgs e, Point point, IAnglrLogger logger)
        {
            if (Debug)
                Logger?.InfoLine ($"OnMouseWheel (constant symbol, id = {Id}, value =  {SymbolToken.Name})");
        }
    }

    public class AnglrRawNonTerminalSymbolVisual : AnglrRawDrawingVisual, IAnglrEventHandler
    {
        public SimpleSymbolToken SymbolToken { get; private set; }
        public AnglrRawNonTerminalSymbolVisual (IAnglrLogger logger, SimpleSymbolToken symbolToken) : base (logger)
        {
            SymbolToken = symbolToken;
        }
        public AnglrRawNonTerminalSymbolVisual (AnglrRawNonTerminalSymbolVisual terminalSymbol) : base (terminalSymbol.Logger)
        {
            SymbolToken = terminalSymbol.SymbolToken;
        }

        public override void Draw ()
        {
            using (var dc = RenderOpen ())
            {
                FormattedText text = new FormattedText
                (
                    SymbolToken.Name,
                    CultureInfo,
                    FlowDirection,
                    new Typeface (TypefaceName),
                    FontSize,
                    Brush,
                    1.0
                );
                double width = text.Width;
                double height = text.Height;

                dc.DrawRoundedRectangle (NonTerminalSymbolBackground, Pen, new Rect (0, 0, Width = width + 2 * Margin, Height = height + 2 * Margin), RectangleRadius, RectangleRadius);
                dc.DrawText (text, new Point (Margin, Margin));
                ConnectorOffset = Height / 2.0;
            }
            Drawing.Freeze ();
        }

        public override void Display ()
        {
            if (Debug)
                Logger?.InfoLine ($"non-terminal symbol, id = {Id}, position = {Position}, size = {Size}, name = {SymbolToken.Name}");
        }

        public void OnMouseDown (object sender, MouseButtonEventArgs e, Point point, IAnglrLogger logger)
        {
            if (Debug)
                Logger?.InfoLine ($"OnMouseDown (non-terminal symbol, id = {Id}, value =  {SymbolToken.Name})");
        }

        public void OnMouseEnter (object sender, MouseEventArgs e, Point point, IAnglrLogger logger)
        {
            if (Debug)
                Logger?.InfoLine ($"OnMouseEnter (non-terminal symbol, id = {Id}, value =  {SymbolToken.Name})");
        }

        public void OnMouseLeave (object sender, MouseEventArgs e, Point point, IAnglrLogger logger)
        {
            if (Debug)
                Logger?.InfoLine ($"OnMouseLeave (non-terminal symbol, id = {Id}, value =  {SymbolToken.Name})");
        }

        public void OnMouseLeftButtonDown (object sender, MouseButtonEventArgs e, Point point, IAnglrLogger logger)
        {
            if (Debug)
                Logger?.InfoLine ($"OnMouseLeftButtonDown (non-terminal symbol, id = {Id}, value =  {SymbolToken.Name})");
        }

        public void OnMouseLeftButtonUp (object sender, MouseButtonEventArgs e, Point point, IAnglrLogger logger)
        {
            if (Debug)
                Logger?.InfoLine ($"OnMouseLeftButtonUp (non-terminal symbol, id = {Id}, value =  {SymbolToken.Name})");
        }

        public void OnMouseMove (object sender, MouseEventArgs e, Point point, IAnglrLogger logger)
        {
            if (Debug)
                Logger?.InfoLine ($"OnMouseMove (non-terminal symbol, id = {Id}, value =  {SymbolToken.Name})");
        }

        public void OnMouseRightButtonDown (object sender, MouseButtonEventArgs e, Point point, IAnglrLogger logger)
        {
            if (Debug)
                Logger?.InfoLine ($"OnMouseRightButtonDown (non-terminal symbol, id = {Id}, value =  {SymbolToken.Name})");
        }

        public void OnMouseRightButtonUp (object sender, MouseButtonEventArgs e, Point point, IAnglrLogger logger)
        {
            if (Debug)
                Logger?.InfoLine ($"OnMouseRightButtonUp (non-terminal symbol, id = {Id}, value =  {SymbolToken.Name})");
        }

        public void OnMouseUp (object sender, MouseButtonEventArgs e, Point point, IAnglrLogger logger)
        {
            if (Debug)
                Logger?.InfoLine ($"OnMouseUp (non-terminal symbol, id = {Id}, value =  {SymbolToken.Name})");
        }
        public void OnMouseWheel (object sender, MouseWheelEventArgs e, Point point, IAnglrLogger logger)
        {
            if (Debug)
                Logger?.InfoLine ($"OnMousWheel (non-terminal symbol, id = {Id}, value =  {SymbolToken.Name})");
        }
    }

    public class AnglrSyntaxRuleNameVisual : AnglrRawDrawingVisual, IAnglrEventHandler
    {
        public SyntaxTreeToken SymbolToken { get; private set; }
        public AnglrSyntaxRuleNameVisual (IAnglrLogger logger, SyntaxTreeToken symbolToken) : base (logger)
        {
            SymbolToken = symbolToken;
        }

        public override void Draw ()
        {
            using (var dc = RenderOpen ())
            {
                FormattedText text = new FormattedText
                (
                    SymbolToken.text,
                    CultureInfo,
                    FlowDirection,
                    new Typeface (TypefaceName),
                    FontSize,
                    Brush,
                    1.0
                );
                double width = text.Width;
                double height = text.Height;

                dc.DrawRoundedRectangle (NonTerminalSymbolBackground, Pen, new Rect (0, 0, Width = width + 4 * Margin, Height = height + 4 * Margin), RectangleRadius, RectangleRadius);
                dc.DrawRoundedRectangle (NonTerminalSymbolBackground, Pen, new Rect (Margin, Margin, width + 2 * Margin, height + 2 * Margin), RectangleRadius, RectangleRadius);
                dc.DrawText (text, new Point (2 * Margin, 2 * Margin));
                ConnectorOffset = Height / 2.0;
            }
            Drawing.Freeze ();
        }

        public override void Display ()
        {
            if (Debug)
                Logger?.InfoLine ($"syntax rule name, id = {Id}, position = {Position}, size = {Size}, name = {SymbolToken.text}");
        }

        public void OnMouseDown (object sender, MouseButtonEventArgs e, Point point, IAnglrLogger logger)
        {
            if (Debug)
                Logger?.InfoLine ($"OnMouseDown (syntax rule name, id = {Id}, value =  {SymbolToken.text})");
        }

        public void OnMouseEnter (object sender, MouseEventArgs e, Point point, IAnglrLogger logger)
        {
            if (Debug)
                Logger?.InfoLine ($"OnMouseEnter (syntax rule name, id = {Id}, value =  {SymbolToken.text})");
        }

        public void OnMouseLeave (object sender, MouseEventArgs e, Point point, IAnglrLogger logger)
        {
            if (Debug)
                Logger?.InfoLine ($"OnMouseLeave (syntax rule name, id = {Id}, value =  {SymbolToken.text})");
        }

        public void OnMouseLeftButtonDown (object sender, MouseButtonEventArgs e, Point point, IAnglrLogger logger)
        {
            if (Debug)
                Logger?.InfoLine ($"OnMouseLeftButtonDown (syntax rule name, id = {Id}, value =  {SymbolToken.text})");
        }

        public void OnMouseLeftButtonUp (object sender, MouseButtonEventArgs e, Point point, IAnglrLogger logger)
        {
            if (Debug)
                Logger?.InfoLine ($"OnMouseLeftButtonUp (syntax rule name, id = {Id}, value =  {SymbolToken.text})");
        }

        public void OnMouseMove (object sender, MouseEventArgs e, Point point, IAnglrLogger logger)
        {
            if (Debug)
                Logger?.InfoLine ($"OnMouseMove (syntax rule name, id = {Id}, value =  {SymbolToken.text})");
        }

        public void OnMouseRightButtonDown (object sender, MouseButtonEventArgs e, Point point, IAnglrLogger logger)
        {
            if (Debug)
                Logger?.InfoLine ($"OnMouseRightButtonDown (syntax rule name, id = {Id}, value =  {SymbolToken.text})");
        }

        public void OnMouseRightButtonUp (object sender, MouseButtonEventArgs e, Point point, IAnglrLogger logger)
        {
            if (Debug)
                Logger?.InfoLine ($"OnMouseRightButtonUp (syntax rule name, id = {Id}, value =  {SymbolToken.text})");
        }

        public void OnMouseUp (object sender, MouseButtonEventArgs e, Point point, IAnglrLogger logger)
        {
            if (Debug)
                Logger?.InfoLine ($"OnMouseUp (syntax rule name, id = {Id}, value =  {SymbolToken.text})");
        }
        public void OnMouseWheel (object sender, MouseWheelEventArgs e, Point point, IAnglrLogger logger)
        {
            if (Debug)
                Logger?.InfoLine ($"OnMouseWheel (syntax rule name, id = {Id}, value =  {SymbolToken.text})");
        }
    }

    public class AnglrSyntaxGroupNameVisual : AnglrRawDrawingVisual, IAnglrEventHandler
    {
        public SyntaxTreeToken SymbolToken { get; private set; }
        public AnglrSyntaxGroupNameVisual (IAnglrLogger logger, SyntaxTreeToken symbolToken) : base (logger)
        {
            SymbolToken = symbolToken;
        }

        public override void Draw ()
        {
            using (var dc = RenderOpen ())
            {
                FormattedText text = new FormattedText
                (
                    SymbolToken.text,
                    CultureInfo,
                    FlowDirection,
                    new Typeface (TypefaceName),
                    FontSize,
                    Brush,
                    1.0
                );
                double width = text.Width;
                double height = text.Height;

                dc.DrawRectangle (TerminalSymbolBackground, Pen, new Rect (0, 0, Width = width + 4 * Margin, Height = height + 4 * Margin));
                dc.DrawRectangle (TerminalSymbolBackground, Pen, new Rect (Margin, Margin, width + 2 * Margin, height + 2 * Margin));
                dc.DrawText (text, new Point (2 * Margin, 2 * Margin));
                ConnectorOffset = Height / 2.0;
            }
            Drawing.Freeze ();
        }

        public override void Display ()
        {
            if (Debug)
                Logger?.InfoLine ($"syntax group name, id = {Id}, position = {Position}, size = {Size}, name = {SymbolToken.text}");
        }

        public void OnMouseDown (object sender, MouseButtonEventArgs e, Point point, IAnglrLogger logger)
        {
            if (Debug)
                Logger?.InfoLine ($"OnMouseDown (syntax group name, id = {Id}, value =  {SymbolToken.text})");
        }

        public void OnMouseEnter (object sender, MouseEventArgs e, Point point, IAnglrLogger logger)
        {
            if (Debug)
                Logger?.InfoLine ($"OnMouseEnter (syntax group name, id = {Id}, value =  {SymbolToken.text})");
        }

        public void OnMouseLeave (object sender, MouseEventArgs e, Point point, IAnglrLogger logger)
        {
            if (Debug)
                Logger?.InfoLine ($"OnMouseLeave (syntax group name, id = {Id}, value =  {SymbolToken.text})");
        }

        public void OnMouseLeftButtonDown (object sender, MouseButtonEventArgs e, Point point, IAnglrLogger logger)
        {
            if (Debug)
                Logger?.InfoLine ($"OnMouseLeftButtonDown (syntax group name, id = {Id}, value =  {SymbolToken.text})");
        }

        public void OnMouseLeftButtonUp (object sender, MouseButtonEventArgs e, Point point, IAnglrLogger logger)
        {
            if (Debug)
                Logger?.InfoLine ($"OnMouseLeftButtonUp (syntax group name, id = {Id}, value =  {SymbolToken.text})");
        }

        public void OnMouseMove (object sender, MouseEventArgs e, Point point, IAnglrLogger logger)
        {
            if (Debug)
                Logger?.InfoLine ($"OnMouseMove (syntax group name, id = {Id}, value =  {SymbolToken.text})");
        }

        public void OnMouseRightButtonDown (object sender, MouseButtonEventArgs e, Point point, IAnglrLogger logger)
        {
            if (Debug)
                Logger?.InfoLine ($"OnMouseRightButtonDown (syntax group name, id = {Id}, value =  {SymbolToken.text})");
        }

        public void OnMouseRightButtonUp (object sender, MouseButtonEventArgs e, Point point, IAnglrLogger logger)
        {
            if (Debug)
                Logger?.InfoLine ($"OnMouseRightButtonUp (syntax group name, id = {Id}, value =  {SymbolToken.text})");
        }

        public void OnMouseUp (object sender, MouseButtonEventArgs e, Point point, IAnglrLogger logger)
        {
            if (Debug)
                Logger?.InfoLine ($"OnMouseUp (syntax group name, id = {Id}, value =  {SymbolToken.text})");
        }
        public void OnMouseWheel (object sender, MouseWheelEventArgs e, Point point, IAnglrLogger logger)
        {
            if (Debug)
                Logger?.InfoLine ($"OnMouseWheel (syntax group name, id = {Id}, value =  {SymbolToken.text})");
        }
    }

    public class AnglrGeneralizedNameVisual : AnglrRawDrawingVisual, IAnglrEventHandler
    {
        public AnglrRawDrawingVisual GnameVisual { get; private set; }
        public _cardinality_ Cardinality { get; private set; }
        public AnglrRawDrawingVisual DelimiterVisual { get; private set; }
        public _cardinality_delimiter_ CardinalityDelimiter { get; private set; }

        public AnglrGeneralizedNameVisual (IAnglrLogger logger, _g_name_ name) : base (logger)
        {
            if (((AppInfo) name.m__g_name_.appInfo).TryGetValue (AppInfoType.Visual, out var gnameVisual))
                GnameVisual = gnameVisual as AnglrRawDrawingVisual;
            CardinalityDelimiter = name.m__cardinality_delimiter_;
            Cardinality = CardinalityDelimiter.m__cardinality_;
            _delimiter_ delimiter = CardinalityDelimiter.m__delimiter_optional_.m__delimiter_;
            if ((delimiter != null) && ((AppInfo) delimiter.m__anglr_nested_rule_.appInfo).TryGetValue (AppInfoType.Visual, out var visualObject))
                DelimiterVisual = visualObject as AnglrRawDrawingVisual;
        }

        public override void Draw ()
        {
            switch ((_cardinality_.production_kind) Cardinality.kind)
            {
                case _cardinality_.production_kind.g__cardinality__1:
                    DrawOptional ();
                    break;
                case _cardinality_.production_kind.g__cardinality__2:
                    DrawRepeat ();
                    break;
                case _cardinality_.production_kind.g__cardinality__3:
                    DrawRepeat ();
                    break;
                case _cardinality_.production_kind.g__cardinality__4:
                    DrawOptionalRepeat ();
                    break;
                case _cardinality_.production_kind.g__cardinality__5:
                    DrawOptionalRepeat ();
                    break;
                case _cardinality_.production_kind.g__cardinality__6:
                    DrawRepeat ();
                    break;
                case _cardinality_.production_kind.g__cardinality__7:
                    DrawRepeat ();
                    break;
                case _cardinality_.production_kind.g__cardinality__8:
                    DrawOptionalRepeat ();
                    break;
                case _cardinality_.production_kind.g__cardinality__9:
                    DrawOptionalRepeat ();
                    break;
                case _cardinality_.production_kind.g__cardinality__10:
                {
                    int lowLimit = -1;
                    int highLimit = -1;
                    SyntaxTreeToken lowNr = Cardinality.m__number_optional_.m__number_;
                    SyntaxTreeToken highNr = Cardinality.m__number_optional__1.m__number_;
                    if (lowNr != null)
                        lowLimit = int.Parse (lowNr.text);
                    if (highNr != null)
                        highLimit = int.Parse (highNr.text);
                    if ((highLimit < lowLimit) && (highLimit > 0))
                        highLimit = lowLimit;
                    if (lowLimit <= 0)
                    {
                        switch (highLimit)
                        {
                            case -1:
                                DrawOptionalRepeat ();
                                break;
                            case 0:
                                break;
                            case 1:
                                DrawOptional ();
                                break;
                            default:
                                DrawRepeat ();
                                break;
                        }
                    }
                    else if (lowLimit == 1)
                    {
                        if (highLimit == 1)
                            DrawInternal ();
                        else
                            DrawRepeat ();
                    }
                    else
                        DrawRepeat ();
                }
                break;
            }
        }

        private void DrawOptional ()
        {
            AnglrVisualPosition position = new AnglrVisualPosition ();
            double x, y;
            ConnectorOffset = 2 * Margin + GnameVisual.ConnectorOffset;
            Width = 4 * Margin + GnameVisual.Width;
            Height = 2 * Margin + GnameVisual.Height;

            using (var dc = RenderOpen ())
            {
                dc.DrawLine (Pen, new Point (2 * Margin, ConnectorOffset), new Point (0, ConnectorOffset));
                dc.DrawLine (Pen, new Point (0, ConnectorOffset), new Point (0, 0));
                dc.DrawLine (Pen, new Point (0, 0), new Point (Width, 0));
                dc.DrawLine (Pen, new Point (Width, 0), new Point (Width, ConnectorOffset));
                dc.DrawLine (Pen, new Point (Width, ConnectorOffset), new Point (Width - 2 * Margin, ConnectorOffset));
                dc.PushTransform (new TranslateTransform (x = 2 * Margin, y = 2 * Margin));
                GnameVisual.Position += position.PushPosition (x, y);
                dc.DrawDrawing (GnameVisual.Drawing);
                GnameVisual.AnglrVisualParent = this;
                dc.Pop ();
                position.PopPosition ();
            }
        }

        private void DrawRepeat ()
        {
            AnglrVisualPosition position = new AnglrVisualPosition ();
            double x, y;
            ConnectorOffset = GnameVisual.ConnectorOffset;
            if (DelimiterVisual != null)
            {
                double gwidth = 2 * Margin;
                double dwidth = 2 * Margin;
                double gheight = ConnectorOffset;
                double dheight = GnameVisual.Height + 2 * Margin + DelimiterVisual.ConnectorOffset;

                if (DelimiterVisual.Width > GnameVisual.Width)
                {
                    Width = 4 * Margin + DelimiterVisual.Width;
                    gwidth = (Width - GnameVisual.Width) / 2;
                }
                else
                {
                    Width = 4 * Margin + GnameVisual.Width;
                    dwidth = (Width - DelimiterVisual.Width) / 2;
                }
                Height = 2 * Margin + GnameVisual.Height + DelimiterVisual.Height;

                using (var dc = RenderOpen ())
                {
                    dc.DrawLine (Pen, new Point (gwidth, gheight), new Point (0, gheight));
                    dc.DrawLine (Pen, new Point (0, gheight), new Point (0, dheight));
                    dc.DrawLine (Pen, new Point (0, dheight), new Point (dwidth, dheight));
                    dc.DrawLine (Pen, new Point (Width - gwidth, gheight), new Point (Width, gheight));
                    dc.DrawLine (Pen, new Point (Width, gheight), new Point (Width, dheight));
                    dc.DrawLine (Pen, new Point (Width, dheight), new Point (Width - dwidth, dheight));
                    dc.PushTransform (new TranslateTransform (x = gwidth, y = 0));
                    GnameVisual.Position += position.PushPosition (x, y);
                    dc.DrawDrawing (GnameVisual.Drawing);
                    GnameVisual.AnglrVisualParent = this;
                    dc.Pop ();
                    position.PopPosition ();
                    dc.PushTransform (new TranslateTransform (x = dwidth, y = GnameVisual.Height + 2 * Margin));
                    DelimiterVisual.Position += position.PushPosition (x, y);
                    dc.DrawDrawing (DelimiterVisual.Drawing);
                    DelimiterVisual.AnglrVisualParent = this;
                    dc.Pop ();
                    position.PopPosition ();
                }
            }
            else
            {
                Width = 4 * Margin + GnameVisual.Width;
                Height = 2 * Margin + GnameVisual.Height;
                using (var dc = RenderOpen ())
                {
                    dc.DrawLine (Pen, new Point (2 * Margin, ConnectorOffset), new Point (0, ConnectorOffset));
                    dc.DrawLine (Pen, new Point (0, ConnectorOffset), new Point (0, Height));
                    dc.DrawLine (Pen, new Point (0, Height), new Point (Width, Height));
                    dc.DrawLine (Pen, new Point (Width, Height), new Point (Width, ConnectorOffset));
                    dc.DrawLine (Pen, new Point (Width, ConnectorOffset), new Point (Width - 2 * Margin, ConnectorOffset));
                    dc.PushTransform (new TranslateTransform (x = 2 * Margin, y = 0));
                    GnameVisual.Position += position.PushPosition (x, y);
                    dc.DrawDrawing (GnameVisual.Drawing);
                    GnameVisual.AnglrVisualParent = this;
                    dc.Pop ();
                    position.PopPosition ();
                }
            }
        }

        private void DrawOptionalRepeat ()
        {
            AnglrVisualPosition position = new AnglrVisualPosition ();
            double x, y;

            ConnectorOffset = 2 * Margin + GnameVisual.ConnectorOffset;
            if (DelimiterVisual != null)
            {
                double gwidth = 4 * Margin;
                double dwidth = 4 * Margin;
                double gheight = ConnectorOffset;
                double dheight = GnameVisual.Height + 4 * Margin + DelimiterVisual.ConnectorOffset;

                if (DelimiterVisual.Width > GnameVisual.Width)
                {
                    Width = 8 * Margin + DelimiterVisual.Width;
                    gwidth = (Width - GnameVisual.Width) / 2;
                }
                else
                {
                    Width = 8 * Margin + GnameVisual.Width;
                    dwidth = (Width - DelimiterVisual.Width) / 2;
                }
                Height = 4 * Margin + GnameVisual.Height + DelimiterVisual.Height;

                using (var dc = RenderOpen ())
                {
                    dc.DrawLine (Pen, new Point (gwidth, gheight), new Point (0, gheight));
                    dc.DrawLine (Pen, new Point (0, gheight), new Point (0, 0));
                    dc.DrawLine (Pen, new Point (0, 0), new Point (Width, 0));
                    dc.DrawLine (Pen, new Point (Width, 0), new Point (Width, gheight));
                    dc.DrawLine (Pen, new Point (Width, gheight), new Point (Width - gwidth, gheight));

                    dc.DrawLine (Pen, new Point (2 * Margin, gheight), new Point (2 * Margin, dheight));
                    dc.DrawLine (Pen, new Point (2 * Margin, dheight), new Point (dwidth, dheight));
                    dc.DrawLine (Pen, new Point (Width - 2 * Margin, gheight), new Point (Width - 2 * Margin, dheight));
                    dc.DrawLine (Pen, new Point (Width - 2 * Margin, dheight), new Point (Width - dwidth, dheight));
                    dc.PushTransform (new TranslateTransform (x = gwidth, y = 2 * Margin));
                    GnameVisual.Position += position.PushPosition (x, y);
                    dc.DrawDrawing (GnameVisual.Drawing);
                    GnameVisual.AnglrVisualParent = this;
                    dc.Pop ();
                    position.PopPosition ();
                    dc.PushTransform (new TranslateTransform (x = dwidth, y = GnameVisual.Height + 4 * Margin));
                    DelimiterVisual.Position += position.PushPosition (x, y);
                    dc.DrawDrawing (DelimiterVisual.Drawing);
                    DelimiterVisual.AnglrVisualParent = this;
                    dc.Pop ();
                    position.PopPosition ();
                }
            }
            else
            {
                double gwidth = 4 * Margin;
                double dwidth = 4 * Margin;
                double gheight = ConnectorOffset;
                double dheight = GnameVisual.Height + 4 * Margin;

                Width = 8 * Margin + GnameVisual.Width;
                Height = 4 * Margin + GnameVisual.Height;
                using (var dc = RenderOpen ())
                {
                    dc.DrawLine (Pen, new Point (gwidth, gheight), new Point (0, gheight));
                    dc.DrawLine (Pen, new Point (0, gheight), new Point (0, 0));
                    dc.DrawLine (Pen, new Point (0, 0), new Point (Width, 0));
                    dc.DrawLine (Pen, new Point (Width, 0), new Point (Width, gheight));
                    dc.DrawLine (Pen, new Point (Width, gheight), new Point (Width - gwidth, gheight));

                    dc.DrawLine (Pen, new Point (2 * Margin, ConnectorOffset), new Point (2 * Margin, Height));
                    dc.DrawLine (Pen, new Point (2 * Margin, Height), new Point (Width - 2 * Margin, Height));
                    dc.DrawLine (Pen, new Point (Width - 2 * Margin, Height), new Point (Width - 2 * Margin, ConnectorOffset));
                    dc.PushTransform (new TranslateTransform (x = 4 * Margin, y = 2 * Margin));
                    GnameVisual.Position += position.PushPosition (x, y);
                    dc.DrawDrawing (GnameVisual.Drawing);
                    GnameVisual.AnglrVisualParent = this;
                    dc.Pop ();
                    position.PopPosition ();
                }
            }
        }

        private void DrawInternal ()
        {
            AnglrVisualPosition position = new AnglrVisualPosition ();
            double x, y;

            ConnectorOffset = GnameVisual.ConnectorOffset;
            Width = GnameVisual.Width;
            Height = GnameVisual.Height;
            using (var dc = RenderOpen ())
            {
                dc.DrawDrawing (GnameVisual.Drawing);
                GnameVisual.AnglrVisualParent = this;
            }
        }

        public override void Display ()
        {
            if (Debug)
                Logger?.InfoLine ($"g-name, id = {Id}, position = {Position}, size = {Size}, value = {CardinalityDelimiter.parent.Emit (-1).Substring (0, 100)}");
        }

        public void OnMouseDown (object sender, MouseButtonEventArgs e, Point point, IAnglrLogger logger)
        {
            if (Debug)
                Logger?.InfoLine ($"OnMouseDown (g-name, id = {Id}, value =  {CardinalityDelimiter.parent.Emit (-1)})");
        }

        public void OnMouseEnter (object sender, MouseEventArgs e, Point point, IAnglrLogger logger)
        {
            if (Debug)
                Logger?.InfoLine ($"OnMouseEnter (g-name, id = {Id}, value =  {CardinalityDelimiter.parent.Emit (-1)})");
        }

        public void OnMouseLeave (object sender, MouseEventArgs e, Point point, IAnglrLogger logger)
        {
            if (Debug)
                Logger?.InfoLine ($"OnMouseLeave (g-name, id = {Id}, value =  {CardinalityDelimiter.parent.Emit (-1)})");
        }

        public void OnMouseLeftButtonDown (object sender, MouseButtonEventArgs e, Point point, IAnglrLogger logger)
        {
            if (Debug)
                Logger?.InfoLine ($"OnMouseLeftButtonDown (g-name, id = {Id}, value =  {CardinalityDelimiter.parent.Emit (-1)})");
        }

        public void OnMouseLeftButtonUp (object sender, MouseButtonEventArgs e, Point point, IAnglrLogger logger)
        {
            if (Debug)
                Logger?.InfoLine ($"OnMouseLeftButtonUp (g-name, id = {Id}, value =  {CardinalityDelimiter.parent.Emit (-1)})");
        }

        public void OnMouseMove (object sender, MouseEventArgs e, Point point, IAnglrLogger logger)
        {
            if (Debug)
                Logger?.InfoLine ($"OnMouseMove (g-name, id = {Id}, value =  {CardinalityDelimiter.parent.Emit (-1)})");
        }

        public void OnMouseRightButtonDown (object sender, MouseButtonEventArgs e, Point point, IAnglrLogger logger)
        {
            if (Debug)
                Logger?.InfoLine ($"OnMouseRightButtonDown (g-name, id = {Id}, value =  {CardinalityDelimiter.parent.Emit (-1)})");
        }

        public void OnMouseRightButtonUp (object sender, MouseButtonEventArgs e, Point point, IAnglrLogger logger)
        {
            if (Debug)
                Logger?.InfoLine ($"OnMouseRightButtonUp (g-name, id = {Id}, value =  {CardinalityDelimiter.parent.Emit (-1)})");
        }

        public void OnMouseUp (object sender, MouseButtonEventArgs e, Point point, IAnglrLogger logger)
        {
            if (Debug)
                Logger?.InfoLine ($"OnMouseUp (g-name, id = {Id}, value =  {CardinalityDelimiter.parent.Emit (-1)})");
        }
        public void OnMouseWheel (object sender, MouseWheelEventArgs e, Point point, IAnglrLogger logger)
        {
            if (Debug)
                Logger?.InfoLine ($"OnMouseWheel (g-name, id = {Id}, value =  {CardinalityDelimiter.parent.Emit (-1)})");
        }
    }

    public class AnglrStateNrVisual : AnglrRawDrawingVisual, IAnglrEventHandler
    {
        public int StateNr { get; private set; }
        public AnglrStateNrVisual (IAnglrLogger logger, int stateNr) : base (logger)
        {
            StateNr = stateNr;
        }

        public override void Draw ()
        {
            using (var dc = RenderOpen ())
            {
                FormattedText text = new FormattedText
                (
                    StateNr.ToString (),
                    CultureInfo,
                    FlowDirection,
                    new Typeface (TypefaceName),
                    FontSize,
                    Brush,
                    1.0
                );
                double width = text.Width;
                double height = text.Height;

                Width = width + 2 * Margin;
                Height = height + 2 * Margin;

                dc.DrawGeometry
                (
                    Brush,
                    Pen,
                    new PathGeometry
                    (
                        new List<PathFigure> ()
                        {
                            new PathFigure
                            (
                                new Point (0, Height / 2.0),
                                new List<LineSegment> ()
                                {
                                    new LineSegment() { Point = new Point (Margin, 0) },
                                    new LineSegment() { Point = new Point (Margin + width, 0) },
                                    new LineSegment() { Point = new Point (Width, Height / 2.0) },
                                    new LineSegment() { Point = new Point (Margin + width, Height) },
                                    new LineSegment() { Point = new Point (Margin, Height) },
                                    new LineSegment() { Point = new Point (0, Height / 2.0) },
                                },
                                true
                            )
                        }
                    )
                );
                dc.DrawText (text, new Point (Margin, Margin));
                ConnectorOffset = Height / 2.0;
            }
            Drawing.Freeze ();
        }
        public override void Display ()
        {
            if (Debug)
                Logger?.InfoLine ($"state number, id = {Id}, position = {Position}, size = {Size}, value = {StateNr}");
        }

        public void OnMouseDown (object sender, MouseButtonEventArgs e, Point point, IAnglrLogger logger)
        {
            if (Debug)
                Logger?.InfoLine ($"OnMouseDown (state number, id = {Id}, value =  {StateNr})");
        }

        public void OnMouseEnter (object sender, MouseEventArgs e, Point point, IAnglrLogger logger)
        {
            if (Debug)
                Logger?.InfoLine ($"OnMouseEnter (state number, id = {Id}, value =  {StateNr})");
        }

        public void OnMouseLeave (object sender, MouseEventArgs e, Point point, IAnglrLogger logger)
        {
            if (Debug)
                Logger?.InfoLine ($"OnMouseLeave (state number, id = {Id}, value =  {StateNr})");
        }

        public void OnMouseLeftButtonDown (object sender, MouseButtonEventArgs e, Point point, IAnglrLogger logger)
        {
            if (Debug)
                Logger?.InfoLine ($"OnMouseLeftButtonDown (state number, id = {Id}, value =  {StateNr})");
        }

        public void OnMouseLeftButtonUp (object sender, MouseButtonEventArgs e, Point point, IAnglrLogger logger)
        {
            if (Debug)
                Logger?.InfoLine ($"OnMouseLeftButtonUp (state number, id = {Id}, value =  {StateNr})");
        }

        public void OnMouseMove (object sender, MouseEventArgs e, Point point, IAnglrLogger logger)
        {
            if (Debug)
                Logger?.InfoLine ($"OnMouseMove (state number, id = {Id}, value =  {StateNr})");
        }

        public void OnMouseRightButtonDown (object sender, MouseButtonEventArgs e, Point point, IAnglrLogger logger)
        {
            if (Debug)
                Logger?.InfoLine ($"OnMouseRightButtonDown (state number, id = {Id}, value =  {StateNr})");
        }

        public void OnMouseRightButtonUp (object sender, MouseButtonEventArgs e, Point point, IAnglrLogger logger)
        {
            if (Debug)
                Logger?.InfoLine ($"OnMouseRightButtonUp (state number, id = {Id}, value =  {StateNr})");
        }

        public void OnMouseUp (object sender, MouseButtonEventArgs e, Point point, IAnglrLogger logger)
        {
            if (Debug)
                Logger?.InfoLine ($"OnMouseUp (state number, id = {Id}, value =  {StateNr})");
        }
        public void OnMouseWheel (object sender, MouseWheelEventArgs e, Point point, IAnglrLogger logger)
        {
            if (Debug)
                Logger?.InfoLine ($"OnMouseWheel (state number, id = {Id}, value =  {StateNr})");
        }
    }

    public class AnglrNameListVisual : AnglrRawDrawingVisual, IAnglrEventHandler
    {
        public _name_list_ NameList { get; private set; }
        public AnglrNameListVisual (IAnglrLogger logger, _name_list_ nameList) : base (logger)
        {
            NameList = nameList;
            NameList.Iterate
            (
                0,
                (list, appData) =>
                {
                    int counter = (int) appData;
                    _g_name_ name = list.m__g_name_;
                    if ((name == null) || (name.appInfo == null) || !((AppInfo) name.appInfo).TryGetValue (AppInfoType.Visual, out var visual))
                        return counter;
                    AnglrRawDrawingVisual drawingVisual = visual as AnglrRawDrawingVisual;
                    if (drawingVisual == null)
                        return counter;
                    if (ConnectorOffset < drawingVisual.ConnectorOffset)
                        ConnectorOffset = drawingVisual.ConnectorOffset;
                    return counter + 1;
                }
            );
            NameList.Iterate
            (
                0,
                (list, appData) =>
                {
                    int counter = (int) appData;
                    _g_name_ name = list.m__g_name_;
                    if ((name == null) || (name.appInfo == null) || !((AppInfo) name.appInfo).TryGetValue (AppInfoType.Visual, out var visual))
                        return counter;
                    AnglrRawDrawingVisual drawingVisual = visual as AnglrRawDrawingVisual;
                    if (drawingVisual == null)
                        return counter;
                    double diff = drawingVisual.Height - drawingVisual.ConnectorOffset;
                    if (diff > Height)
                        Height = diff;
                    Width += drawingVisual.Width + 2 * Margin;
                    return counter + 1;
                }
            );
            Height += ConnectorOffset;
            Width -= 2 * Margin;
        }

        public override void Draw ()
        {
            AnglrVisualPosition position = new AnglrVisualPosition ();
            double x, y;

            double width = 0;
            using (var dc = RenderOpen ())
            {
                NameList.Iterate
                (
                    0,
                    (list, appData) =>
                    {
                        int counter = (int) appData;
                        _g_name_ name = list.m__g_name_;
                        if ((name == null) || (name.appInfo == null) || !((AppInfo) name.appInfo).TryGetValue (AppInfoType.Visual, out var visual))
                            return counter;
                        AnglrRawDrawingVisual drawingVisual = visual as AnglrRawDrawingVisual;
                        if (drawingVisual == null)
                            return counter;
                        dc.PushTransform (new TranslateTransform (x = width, y = 0));
                        position.PushPosition (x, y);
                        if (counter > 0)
                        {
                            dc.DrawLine (Pen, new Point (0, ConnectorOffset), new Point (2 * Margin, ConnectorOffset));
                            dc.PushTransform (new TranslateTransform (x = 2 * Margin, y = 0));
                            position.PushPosition (x, y);
                        }
                        dc.PushTransform (new TranslateTransform (x = 0, y = ConnectorOffset - drawingVisual.ConnectorOffset));
                        drawingVisual.Position += position.PushPosition (x, y);
                        dc.DrawDrawing (drawingVisual.Drawing);
                        drawingVisual.AnglrVisualParent = this;
                        dc.Pop ();
                        position.PopPosition ();
                        width += drawingVisual.Width;
                        if (counter > 0)
                        {
                            width += 2 * Margin;
                            dc.Pop ();
                            position.PopPosition ();
                        }
                        dc.Pop ();
                        position.PopPosition ();
                        return counter + 1;
                    }
                );
            }
        }

        public override void Display ()
        {
            if (Debug)
                Logger?.InfoLine ($"name list, id = {Id}, position = {Position}, size = {Size}, value = {NameList.Emit (-1)}");
        }

        public void OnMouseDown (object sender, MouseButtonEventArgs e, Point point, IAnglrLogger logger)
        {
            if (Debug)
                Logger?.InfoLine ($"OnMouseDown (name list, id = {Id}, value =  {NameList.Emit (-1)})");
        }

        public void OnMouseEnter (object sender, MouseEventArgs e, Point point, IAnglrLogger logger)
        {
            if (Debug)
                Logger?.InfoLine ($"OnMouseEnter (name list, id = {Id}, value =  {NameList.Emit (-1)})");
        }

        public void OnMouseLeave (object sender, MouseEventArgs e, Point point, IAnglrLogger logger)
        {
            if (Debug)
                Logger?.InfoLine ($"OnMouseLeave (name list, id = {Id}, value =  {NameList.Emit (-1)})");
        }

        public void OnMouseLeftButtonDown (object sender, MouseButtonEventArgs e, Point point, IAnglrLogger logger)
        {
            if (Debug)
                Logger?.InfoLine ($"OnMouseLeftButtonDown (name list, id = {Id}, value =  {NameList.Emit (-1)})");
        }

        public void OnMouseLeftButtonUp (object sender, MouseButtonEventArgs e, Point point, IAnglrLogger logger)
        {
            if (Debug)
                Logger?.InfoLine ($"OnMouseLeftButtonUp (name list, id = {Id}, value =  {NameList.Emit (-1)})");
        }

        public void OnMouseMove (object sender, MouseEventArgs e, Point point, IAnglrLogger logger)
        {
            if (Debug)
                Logger?.InfoLine ($"OnMouseMove (name list, id = {Id}, value =  {NameList.Emit (-1)})");
        }

        public void OnMouseRightButtonDown (object sender, MouseButtonEventArgs e, Point point, IAnglrLogger logger)
        {
            if (Debug)
                Logger?.InfoLine ($"OnMouseRightButtonDown (name list, id = {Id}, value =  {NameList.Emit (-1)})");
        }

        public void OnMouseRightButtonUp (object sender, MouseButtonEventArgs e, Point point, IAnglrLogger logger)
        {
            if (Debug)
                Logger?.InfoLine ($"OnMouseRightButtonUp (name list, id = {Id}, value =  {NameList.Emit (-1)})");
        }

        public void OnMouseUp (object sender, MouseButtonEventArgs e, Point point, IAnglrLogger logger)
        {
            if (Debug)
                Logger?.InfoLine ($"OnMouseUp (name list, id = {Id}, value =  {NameList.Emit (-1)})");
        }
        public void OnMouseWheel (object sender, MouseWheelEventArgs e, Point point, IAnglrLogger logger)
        {
            if (Debug)
                Logger?.InfoLine ($"OnMouseWheel (name list, id = {Id}, value =  {NameList.Emit (-1)})");
        }
    }

    public class AnglrNestedRuleVisual : AnglrRawDrawingVisual, IAnglrEventHandler
    {
        public _anglr_nested_rule_ NestedRule { get; private set; }
        public AnglrRawDrawingVisual SyntaxRuleNameVisual { get; private set; }
        public AnglrNestedRuleVisual (IAnglrLogger logger, _anglr_nested_rule_ nestedRule) : base (logger)
        {
            NestedRule = nestedRule;
            _anglr_syntax_production_list_name_optional_ list_Name_Optional_ = NestedRule.m__anglr_syntax_production_list_name_optional_;
            if ((_anglr_syntax_production_list_name_optional_.production_kind) list_Name_Optional_.kind == _anglr_syntax_production_list_name_optional_.production_kind.g__anglr_syntax_production_list_name_optional__2)
            {
                AppInfo appInfo = list_Name_Optional_.m__anglr_syntax_production_list_name_.appInfo as AppInfo;
                if ((appInfo != null) && appInfo.TryGetValue (AppInfoType.Visual, out var visual))
                    SyntaxRuleNameVisual = visual as AnglrRawDrawingVisual;
            }
            NestedRule.m__anglr_syntax_production_list_.Iterate
            (
                0,
                (node, data) =>
                {
                    int counter = (int) data;
                    _anglr_syntax_production_ production = node.m__anglr_syntax_production_;
                    if ((production == null) || (production.appInfo == null) || !((AppInfo) production.appInfo).TryGetValue (AppInfoType.Visual, out var visual))
                        return counter;
                    AnglrRawDrawingVisual drawingVisual = visual as AnglrRawDrawingVisual;
                    if (drawingVisual == null)
                        return counter;
                    if (Width < drawingVisual.Width)
                        Width = drawingVisual.Width;
                    Height += drawingVisual.Height;
                    if ((counter == 0) && (SyntaxRuleNameVisual == null))
                        ConnectorOffset = drawingVisual.ConnectorOffset;
                    Height += 2 * Margin;
                    return counter + 1;
                }
            );
            if (SyntaxRuleNameVisual != null)
            {
                Height += SyntaxRuleNameVisual.Height + 2 * Margin;
                ConnectorOffset = SyntaxRuleNameVisual.Height / 2;
                Width += 8 * Margin;
                Width = Math.Max (Width, SyntaxRuleNameVisual.Width + 2 * Margin);
            }
            else
                Width += 4 * Margin;
        }
        public override void Draw ()
        {
            AnglrVisualPosition position = new AnglrVisualPosition ();
            double x, y;

            double height = 0;
            double connectionOffset = 0;
            using (var dc = RenderOpen ())
            {
                if (SyntaxRuleNameVisual != null)
                {
                    dc.DrawDrawing (SyntaxRuleNameVisual.Drawing);
                    SyntaxRuleNameVisual.AnglrVisualParent = this;
                    dc.DrawLine (Pen, new Point (SyntaxRuleNameVisual.Width, ConnectorOffset), new Point (Width, ConnectorOffset));
                    dc.PushTransform (new TranslateTransform (x = 8 * Margin, y = SyntaxRuleNameVisual.Height + 2 * Margin));
                    position.PushPosition (x, y);
                }
                NestedRule.m__anglr_syntax_production_list_.Iterate
                (
                    0,
                    (node, data) =>
                    {
                        int counter = (int) data;
                        _anglr_syntax_production_ production = node.m__anglr_syntax_production_;
                        if ((production == null) || (production.appInfo == null) || !((AppInfo) production.appInfo).TryGetValue (AppInfoType.Visual, out var visual))
                            return counter;
                        AnglrRawDrawingVisual drawingVisual = visual as AnglrRawDrawingVisual;
                        if (drawingVisual == null)
                            return counter;
                        connectionOffset = height + drawingVisual.ConnectorOffset;
                        dc.DrawLine (Pen, new Point (0, height + drawingVisual.ConnectorOffset), new Point (2 * Margin, height + drawingVisual.ConnectorOffset));
                        dc.PushTransform (new TranslateTransform (x = 2 * Margin, y = height));
                        drawingVisual.Position += position.PushPosition (x, y);
                        dc.DrawDrawing (drawingVisual.Drawing);
                        drawingVisual.AnglrVisualParent = this;
                        dc.Pop ();
                        position.PopPosition ();
                        if (SyntaxRuleNameVisual == null)
                            dc.DrawLine (Pen, new Point (drawingVisual.Width + 2 * Margin, height + drawingVisual.ConnectorOffset), new Point (Width, height + drawingVisual.ConnectorOffset));
                        height += drawingVisual.Height + 2 * Margin;
                        return counter + 1;
                    }
                );
                if (SyntaxRuleNameVisual != null)
                {
                    dc.DrawLine (Pen, new Point (0, -2 * Margin), new Point (0, connectionOffset));
                    dc.Pop ();
                    position.PopPosition ();
                }
                else
                {
                    dc.DrawLine (Pen, new Point (0, ConnectorOffset), new Point (0, connectionOffset));
                    dc.DrawLine (Pen, new Point (Width, ConnectorOffset), new Point (Width, connectionOffset));
                }
            }
        }

        public override void Display ()
        {
            if (Debug)
                Logger?.InfoLine ($"nested syntax rule, id = {Id}, position = {Position}, size = {Size}, value = {NestedRule.Emit (-1)}");
        }

        public void OnMouseDown (object sender, MouseButtonEventArgs e, Point point, IAnglrLogger logger)
        {
            if (Debug)
                Logger?.InfoLine ($"OnMouseDown (nested syntax rule, id = {Id}, value =  {NestedRule.Emit (-1)})");
        }

        public void OnMouseEnter (object sender, MouseEventArgs e, Point point, IAnglrLogger logger)
        {
            if (Debug)
                Logger?.InfoLine ($"OnMouseEnter (nested syntax rule, id = {Id}, value =  {NestedRule.Emit (-1)})");
        }

        public void OnMouseLeave (object sender, MouseEventArgs e, Point point, IAnglrLogger logger)
        {
            if (Debug)
                Logger?.InfoLine ($"OnMouseLeave (nested syntax rule, id = {Id}, value =  {NestedRule.Emit (-1)})");
        }

        public void OnMouseLeftButtonDown (object sender, MouseButtonEventArgs e, Point point, IAnglrLogger logger)
        {
            if (Debug)
                Logger?.InfoLine ($"OnMouseLeftButtonDown (nested syntax rule, id = {Id}, value =  {NestedRule.Emit (-1)})");
        }

        public void OnMouseLeftButtonUp (object sender, MouseButtonEventArgs e, Point point, IAnglrLogger logger)
        {
            if (Debug)
                Logger?.InfoLine ($"OnMouseLeftButtonUp (nested syntax rule, id = {Id}, value =  {NestedRule.Emit (-1)})");
        }

        public void OnMouseMove (object sender, MouseEventArgs e, Point point, IAnglrLogger logger)
        {
            if (Debug)
                Logger?.InfoLine ($"OnMouseMove (nested syntax rule, id = {Id}, value =  {NestedRule.Emit (-1)})");
        }

        public void OnMouseRightButtonDown (object sender, MouseButtonEventArgs e, Point point, IAnglrLogger logger)
        {
            if (Debug)
                Logger?.InfoLine ($"OnMouseRightButtonDown (nested syntax rule, id = {Id}, value =  {NestedRule.Emit (-1)})");
        }

        public void OnMouseRightButtonUp (object sender, MouseButtonEventArgs e, Point point, IAnglrLogger logger)
        {
            if (Debug)
                Logger?.InfoLine ($"OnMouseRightButtonUp (nested syntax rule, id = {Id}, value =  {NestedRule.Emit (-1)})");
        }

        public void OnMouseUp (object sender, MouseButtonEventArgs e, Point point, IAnglrLogger logger)
        {
            if (Debug)
                Logger?.InfoLine ($"OnMouseUp (nested syntax rule, id = {Id}, value =  {NestedRule.Emit (-1)})");
        }
        public void OnMouseWheel (object sender, MouseWheelEventArgs e, Point point, IAnglrLogger logger)
        {
            if (Debug)
                Logger?.InfoLine ($"OnMouseWheel (nested syntax rule, id = {Id}, value =  {NestedRule.Emit (-1)})");
        }
    }

    public class AnglrSyntaxRuleVisual : AnglrRawDrawingVisual, IAnglrEventHandler
    {
        public _anglr_syntax_rule_ SyntaxRule { get; private set; }
        public AnglrRawDrawingVisual SyntaxRuleNameVisual { get; private set; }
        public AnglrSyntaxRuleVisual (IAnglrLogger logger, _anglr_syntax_rule_ syntaxRule) : base (logger)
        {
            SyntaxRule = syntaxRule;
            SyntaxTreeToken ruleName = SyntaxRule.m__identifier_;
            if ((ruleName != null) && (ruleName.appInfo != null) && ((AppInfo) ruleName.appInfo).TryGetValue (AppInfoType.Visual, out var nameVisual))
            {
                SyntaxRuleNameVisual = nameVisual as AnglrRawDrawingVisual;
                if (SyntaxRuleNameVisual != null)
                    Height += SyntaxRuleNameVisual.Height + 2 * Margin;
            }
            SyntaxRule.m__anglr_syntax_production_list_.Iterate
            (
                0,
                (node, data) =>
                {
                    int counter = (int) data;
                    _anglr_syntax_production_ production = node.m__anglr_syntax_production_;
                    if ((production == null) || (production.appInfo == null) || !((AppInfo) production.appInfo).TryGetValue (AppInfoType.Visual, out var visual))
                        return counter;
                    AnglrRawDrawingVisual drawingVisual = visual as AnglrRawDrawingVisual;
                    if (drawingVisual == null)
                        return counter;
                    if (Width < drawingVisual.Width)
                        Width = drawingVisual.Width;
                    Height += drawingVisual.Height;
                    if (counter == 0)
                        ConnectorOffset = drawingVisual.ConnectorOffset;
                    Height += 2 * Margin;
                    return counter + 1;
                }
            );
            Width += 10 * Margin;
        }

        public override void Draw ()
        {
            AnglrVisualPosition position = new AnglrVisualPosition ();
            double x, y;

            double height = 0;
            double connectionOffset = 0;
            using (var dc = RenderOpen ())
            {
                if (SyntaxRuleNameVisual != null)
                {
                    dc.DrawDrawing (SyntaxRuleNameVisual.Drawing);
                    SyntaxRuleNameVisual.AnglrVisualParent = this;
                    dc.PushTransform (new TranslateTransform (x = 8 * Margin, y = SyntaxRuleNameVisual.Height + 2 * Margin));
                    position.PushPosition (x, y);
                }
                else
                {
                    dc.PushTransform (new TranslateTransform (x = 8 * Margin, y = 0));
                    position.PushPosition (x, y);
                }
                SyntaxRule.m__anglr_syntax_production_list_.Iterate
                (
                    0,
                    (node, data) =>
                    {
                        int counter = (int) data;
                        _anglr_syntax_production_ production = node.m__anglr_syntax_production_;
                        if ((production == null) || (production.appInfo == null) || !((AppInfo) production.appInfo).TryGetValue (AppInfoType.Visual, out var visual))
                            return counter;
                        AnglrRawDrawingVisual drawingVisual = visual as AnglrRawDrawingVisual;
                        if (drawingVisual == null)
                            return counter;
                        connectionOffset = height + drawingVisual.ConnectorOffset;
                        dc.DrawLine (Pen, new Point (0, height + drawingVisual.ConnectorOffset), new Point (2 * Margin, height + drawingVisual.ConnectorOffset));
                        dc.PushTransform (new TranslateTransform (x = 2 * Margin, y = height));
                        drawingVisual.Position += position.PushPosition (x, y);
                        dc.DrawDrawing (drawingVisual.Drawing);
                        drawingVisual.AnglrVisualParent = this;
                        dc.Pop ();
                        position.PopPosition ();
                        height += drawingVisual.Height + 2 * Margin;
                        return counter + 1;
                    }
                );
                if (SyntaxRuleNameVisual != null)
                    dc.DrawLine (Pen, new Point (0, -2 * Margin), new Point (0, connectionOffset));
                else
                    dc.DrawLine (Pen, new Point (0, ConnectorOffset), new Point (0, connectionOffset));
                dc.Pop ();
                position.PopPosition ();
            }
        }

        public override void Display ()
        {
            if (Debug)
                Logger?.InfoLine ($"syntax rule, id = {Id}, position = {Position}, size = {Size}, value =  {SyntaxRule.Emit (-1)}");
        }

        public void OnMouseDown (object sender, MouseButtonEventArgs e, Point point, IAnglrLogger logger)
        {
            if (Debug)
                Logger?.InfoLine ($"OnMouseDown (syntax rule, id = {Id}, value =  {SyntaxRule.Emit (-1)})");
        }

        public void OnMouseEnter (object sender, MouseEventArgs e, Point point, IAnglrLogger logger)
        {
            if (Debug)
                Logger?.InfoLine ($"OnMouseEnter (syntax rule, id = {Id}, value =  {SyntaxRule.Emit (-1)})");
        }

        public void OnMouseLeave (object sender, MouseEventArgs e, Point point, IAnglrLogger logger)
        {
            if (Debug)
                Logger?.InfoLine ($"OnMouseLeave (syntax rule, id = {Id}, value =  {SyntaxRule.Emit (-1)})");
        }

        public void OnMouseLeftButtonDown (object sender, MouseButtonEventArgs e, Point point, IAnglrLogger logger)
        {
            if (Debug)
                Logger?.InfoLine ($"OnMouseLeftButtonDown (syntax rule, id = {Id}, value =  {SyntaxRule.Emit (-1)})");
        }

        public void OnMouseLeftButtonUp (object sender, MouseButtonEventArgs e, Point point, IAnglrLogger logger)
        {
            if (Debug)
                Logger?.InfoLine ($"OnMouseLeftButtonUp (syntax rule, id = {Id}, value =  {SyntaxRule.Emit (-1)})");
        }

        public void OnMouseMove (object sender, MouseEventArgs e, Point point, IAnglrLogger logger)
        {
            if (Debug)
                Logger?.InfoLine ($"OnMouseMove (syntax rule, id = {Id}, value =  {SyntaxRule.Emit (-1)})");
        }

        public void OnMouseRightButtonDown (object sender, MouseButtonEventArgs e, Point point, IAnglrLogger logger)
        {
            if (Debug)
                Logger?.InfoLine ($"OnMouseRightButtonDown (syntax rule, id = {Id}, value =  {SyntaxRule.Emit (-1)})");
        }

        public void OnMouseRightButtonUp (object sender, MouseButtonEventArgs e, Point point, IAnglrLogger logger)
        {
            if (Debug)
                Logger?.InfoLine ($"OnMouseRightButtonUp (syntax rule, id = {Id}, value =  {SyntaxRule.Emit (-1)})");
        }

        public void OnMouseUp (object sender, MouseButtonEventArgs e, Point point, IAnglrLogger logger)
        {
            if (Debug)
                Logger?.InfoLine ($"OnMouseUp (syntax rule, id = {Id}, value =  {SyntaxRule.Emit (-1)})");
        }
        public void OnMouseWheel (object sender, MouseWheelEventArgs e, Point point, IAnglrLogger logger)
        {
            if (Debug)
                Logger?.InfoLine ($"OnMouseWheel (syntax rule, id = {Id}, value =  {SyntaxRule.Emit (-1)})");
        }
    }

    public class AnglrSyntaxGroupVisual : AnglrRawDrawingVisual, IAnglrEventHandler
    {
        public _anglr_syntax_rule_ SyntaxGroup { get; private set; }
        public AnglrRawDrawingVisual SyntaxGroupNameVisual { get; private set; }
        public AnglrSyntaxGroupVisual (IAnglrLogger logger, _anglr_syntax_rule_ syntaxGroup) : base (logger)
        {
            SyntaxGroup = syntaxGroup;
            SyntaxTreeToken groupName = SyntaxGroup.m__identifier_;
            if ((groupName != null) && (groupName.appInfo != null) && ((AppInfo) groupName.appInfo).TryGetValue (AppInfoType.Visual, out var nameVisual))
            {
                SyntaxGroupNameVisual = nameVisual as AnglrRawDrawingVisual;
                if (SyntaxGroupNameVisual != null)
                    Height += SyntaxGroupNameVisual.Height + 8 * Margin;
            }
            if ((_anglr_syntax_rule_list_optional_.production_kind) SyntaxGroup.m__anglr_syntax_rule_list_optional_.kind == _anglr_syntax_rule_list_optional_.production_kind.g__anglr_syntax_rule_list_optional__2)
            {
                SyntaxGroup.m__anglr_syntax_rule_list_optional_.m__anglr_syntax_rule_list_.Iterate
                (
                    0,
                    (node, data) =>
                    {
                        int counter = (int) data;
                        _anglr_syntax_rule_ rule = node.m__anglr_syntax_rule_;
                        if ((rule == null) || (rule.appInfo == null) || !((AppInfo) rule.appInfo).TryGetValue (AppInfoType.Visual, out var visual))
                            return counter;
                        AnglrRawDrawingVisual drawingVisual = visual as AnglrRawDrawingVisual;
                        if (drawingVisual == null)
                            return counter;
                        if (Width < drawingVisual.Width)
                            Width = drawingVisual.Width;
                        Height += drawingVisual.Height;
                        if (counter == 0)
                            ConnectorOffset = drawingVisual.ConnectorOffset;
                        Height += 2 * Margin;
                        return counter + 1;
                    }
                );
            }
        }
        public override void Draw ()
        {
            AnglrVisualPosition position = new AnglrVisualPosition ();
            double x, y;

            double height = 0;
            double connectionOffset = 0;
            using (var dc = RenderOpen ())
            {
                if ((_anglr_syntax_rule_list_optional_.production_kind) SyntaxGroup.m__anglr_syntax_rule_list_optional_.kind == _anglr_syntax_rule_list_optional_.production_kind.g__anglr_syntax_rule_list_optional__2)
                {
                    if (SyntaxGroupNameVisual != null)
                    {
                        dc.DrawDrawing (SyntaxGroupNameVisual.Drawing);
                        SyntaxGroupNameVisual.AnglrVisualParent = this;
                        dc.PushTransform (new TranslateTransform (x = 8 * Margin, y = SyntaxGroupNameVisual.Height + 2 * Margin));
                        position.PushPosition (x, y);
                    }
                    else
                    {
                        dc.PushTransform (new TranslateTransform (x = 8 * Margin, y = 0));
                        position.PushPosition (x, y);
                    }
                    dc.PushTransform (new TranslateTransform (x = 0, y = 2 * Margin));
                    position.PushPosition (x, y);
                    dc.DrawLine (Pen, new Point (0, 0), new Point (Width, 0));
                    dc.PushTransform (new TranslateTransform (x = 0, y = 2 * Margin));
                    position.PushPosition (x, y);
                    SyntaxGroup.m__anglr_syntax_rule_list_optional_.m__anglr_syntax_rule_list_.Iterate
                    (
                        0,
                        (node, data) =>
                        {
                            int counter = (int) data;
                            _anglr_syntax_rule_ rule = node.m__anglr_syntax_rule_;
                            if ((rule == null) || (rule.appInfo == null) || !((AppInfo) rule.appInfo).TryGetValue (AppInfoType.Visual, out var visual))
                                return counter;
                            AnglrRawDrawingVisual drawingVisual = visual as AnglrRawDrawingVisual;
                            if (drawingVisual == null)
                                return counter;
                            connectionOffset = height + drawingVisual.ConnectorOffset;
                            dc.PushTransform (new TranslateTransform (x = 0, y = height));
                            drawingVisual.Position += position.PushPosition (x, y);
                            dc.DrawDrawing (drawingVisual.Drawing);
                            drawingVisual.AnglrVisualParent = this;
                            dc.Pop ();
                            position.PopPosition ();
                            height += drawingVisual.Height + 2 * Margin;
                            return counter + 1;
                        }
                    );
                    dc.PushTransform (new TranslateTransform (x = 0, y = 2 * Margin + height));
                    position.PushPosition (x, y);
                    dc.DrawLine (Pen, new Point (0, 0), new Point (Width, 0));
                    dc.Pop ();
                    position.PopPosition ();
                    dc.Pop ();
                    position.PopPosition ();
                    dc.Pop ();
                    position.PopPosition ();
                    dc.Pop ();
                    position.PopPosition ();
                }
            }
        }

        public override void Display ()
        {
            if (Debug)
                Logger?.InfoLine ($"syntax group, id = {Id}, position = {Position}, size = {Size}, value = {SyntaxGroup.Emit (-1)}");
        }

        public void OnMouseDown (object sender, MouseButtonEventArgs e, Point point, IAnglrLogger logger)
        {
            if (Debug)
                Logger?.InfoLine ($"OnMouseDown (syntax group, id = {Id}, value =  {SyntaxGroup.Emit (-1)})");
        }

        public void OnMouseEnter (object sender, MouseEventArgs e, Point point, IAnglrLogger logger)
        {
            if (Debug)
                Logger?.InfoLine ($"OnMouseEnter (syntax group, id = {Id}, value =  {SyntaxGroup.Emit (-1)})");
        }

        public void OnMouseLeave (object sender, MouseEventArgs e, Point point, IAnglrLogger logger)
        {
            if (Debug)
                Logger?.InfoLine ($"OnMouseLeave (syntax group, id = {Id}, value =  {SyntaxGroup.Emit (-1)})");
        }

        public void OnMouseLeftButtonDown (object sender, MouseButtonEventArgs e, Point point, IAnglrLogger logger)
        {
            if (Debug)
                Logger?.InfoLine ($"OnMouseLeftButtonDown (syntax group, id = {Id}, value =  {SyntaxGroup.Emit (-1)})");
        }

        public void OnMouseLeftButtonUp (object sender, MouseButtonEventArgs e, Point point, IAnglrLogger logger)
        {
            if (Debug)
                Logger?.InfoLine ($"OnMouseLeftButtonUp (syntax group, id = {Id}, value =  {SyntaxGroup.Emit (-1)})");
        }

        public void OnMouseMove (object sender, MouseEventArgs e, Point point, IAnglrLogger logger)
        {
            if (Debug)
                Logger?.InfoLine ($"OnMouseMove (syntax group, id = {Id}, value =  {SyntaxGroup.Emit (-1)})");
        }

        public void OnMouseRightButtonDown (object sender, MouseButtonEventArgs e, Point point, IAnglrLogger logger)
        {
            if (Debug)
                Logger?.InfoLine ($"OnMouseRightButtonDown (syntax group, id = {Id}, value =  {SyntaxGroup.Emit (-1)})");
        }

        public void OnMouseRightButtonUp (object sender, MouseButtonEventArgs e, Point point, IAnglrLogger logger)
        {
            if (Debug)
                Logger?.InfoLine ($"OnMouseRightButtonUp (syntax group, id = {Id}, value =  {SyntaxGroup.Emit (-1)})");
        }

        public void OnMouseUp (object sender, MouseButtonEventArgs e, Point point, IAnglrLogger logger)
        {
            if (Debug)
                Logger?.InfoLine ($"OnMouseUp (syntax group, id = {Id}, value =  {SyntaxGroup.Emit (-1)})");
        }
        public void OnMouseWheel (object sender, MouseWheelEventArgs e, Point point, IAnglrLogger logger)
        {
            if (Debug)
                Logger?.InfoLine ($"OnMouseWheel (syntax group, id = {Id}, value =  {SyntaxGroup.Emit (-1)})");
        }
    }

    public class AnglrParserPartVisual : AnglrRawDrawingVisual, IAnglrEventHandler
    {
        public _parser_part_ ParserPart { get; private set; }
        public AnglrParserPartVisual (IAnglrLogger logger, _parser_part_ parserPart) : base (logger)
        {
            ParserPart = parserPart;
            if ((_anglr_syntax_rule_list_optional_.production_kind) ParserPart.m__anglr_syntax_rule_list_optional_.kind == _anglr_syntax_rule_list_optional_.production_kind.g__anglr_syntax_rule_list_optional__2)
            {
                ParserPart.m__anglr_syntax_rule_list_optional_.m__anglr_syntax_rule_list_.Iterate
                (
                    0,
                    (node, data) =>
                    {
                        int counter = (int) data;
                        _anglr_syntax_rule_ rule = node.m__anglr_syntax_rule_;
                        if ((rule == null) || (rule.appInfo == null) || !((AppInfo) rule.appInfo).TryGetValue (AppInfoType.Visual, out var visual))
                            return counter;
                        AnglrRawDrawingVisual drawingVisual = visual as AnglrRawDrawingVisual;
                        if (drawingVisual == null)
                            return counter;
                        if (Width < drawingVisual.Width)
                            Width = drawingVisual.Width;
                        Height += drawingVisual.Height + 2 * Margin;
                        if (counter == 0)
                            ConnectorOffset = drawingVisual.ConnectorOffset;
                        return counter + 1;
                    }
                );
            }
        }

        public override void Draw ()
        {
            AnglrVisualPosition position = new AnglrVisualPosition ();
            double x, y;

            double height = 0;
            using (var dc = RenderOpen ())
            {
                if ((_anglr_syntax_rule_list_optional_.production_kind) ParserPart.m__anglr_syntax_rule_list_optional_.kind == _anglr_syntax_rule_list_optional_.production_kind.g__anglr_syntax_rule_list_optional__2)
                {
                    ParserPart.m__anglr_syntax_rule_list_optional_.m__anglr_syntax_rule_list_.Iterate
                    (
                        0,
                        (node, data) =>
                        {
                            int counter = (int) data;
                            _anglr_syntax_rule_ rule = node.m__anglr_syntax_rule_;
                            if ((rule == null) || (rule.appInfo == null) || !((AppInfo) rule.appInfo).TryGetValue (AppInfoType.Visual, out var visual))
                                return counter;
                            AnglrRawDrawingVisual drawingVisual = visual as AnglrRawDrawingVisual;
                            if (drawingVisual == null)
                                return counter;
                            dc.PushTransform (new TranslateTransform (x = 0, y = height));
                            drawingVisual.Position += position.PushPosition (x, y);
                            dc.DrawDrawing (drawingVisual.Drawing);
                            drawingVisual.AnglrVisualParent = this;
                            dc.Pop ();
                            position.PopPosition ();
                            height += drawingVisual.Height + 2 * Margin;
                            return counter + 1;
                        }
                    );
                }
            }
        }

        public override void Display ()
        {
            if (Debug)
                Logger?.InfoLine ($"parser part, id = {Id}, position = {Position}, size = {Size}, value =  {ParserPart.Emit (-1)}");
        }

        public void OnMouseDown (object sender, MouseButtonEventArgs e, Point point, IAnglrLogger logger)
        {
            if (Debug)
                Logger?.InfoLine ($"OnMouseDown (parser part, id = {Id}, value =  {ParserPart.Emit (-1)})");
        }

        public void OnMouseEnter (object sender, MouseEventArgs e, Point point, IAnglrLogger logger)
        {
            if (Debug)
                Logger?.InfoLine ($"OnMouseEnter (parser part, id = {Id}, value =  {ParserPart.Emit (-1)})");
        }

        public void OnMouseLeave (object sender, MouseEventArgs e, Point point, IAnglrLogger logger)
        {
            if (Debug)
                Logger?.InfoLine ($"OnMouseLeave (parser part, id = {Id}, value =  {ParserPart.Emit (-1)})");
        }

        public void OnMouseLeftButtonDown (object sender, MouseButtonEventArgs e, Point point, IAnglrLogger logger)
        {
            if (Debug)
                Logger?.InfoLine ($"OnMouseLeftButtonDown (parser part, id = {Id}, value =  {ParserPart.Emit (-1)})");
        }

        public void OnMouseLeftButtonUp (object sender, MouseButtonEventArgs e, Point point, IAnglrLogger logger)
        {
            if (Debug)
                Logger?.InfoLine ($"OnMouseLeftButtonUp (parser part, id = {Id}, value =  {ParserPart.Emit (-1)})");
        }

        public void OnMouseMove (object sender, MouseEventArgs e, Point point, IAnglrLogger logger)
        {
            if (Debug)
                Logger?.InfoLine ($"OnMouseMove (parser part, id = {Id}, value =  {ParserPart.Emit (-1)})");
        }

        public void OnMouseRightButtonDown (object sender, MouseButtonEventArgs e, Point point, IAnglrLogger logger)
        {
            if (Debug)
                Logger?.InfoLine ($"OnMouseRightButtonDown (parser part, id = {Id}, value =  {ParserPart.Emit (-1)})");
        }

        public void OnMouseRightButtonUp (object sender, MouseButtonEventArgs e, Point point, IAnglrLogger logger)
        {
            if (Debug)
                Logger?.InfoLine ($"OnMouseRightButtonUp (parser part, id = {Id}, value =  {ParserPart.Emit (-1)})");
        }

        public void OnMouseUp (object sender, MouseButtonEventArgs e, Point point, IAnglrLogger logger)
        {
            if (Debug)
                Logger?.InfoLine ($"OnMouseUp (parser part, id = {Id}, value =  {ParserPart.Emit (-1)})");
        }
        public void OnMouseWheel (object sender, MouseWheelEventArgs e, Point point, IAnglrLogger logger)
        {
            if (Debug)
                Logger?.InfoLine ($"OnMouseWheel (parser part, id = {Id}, value =  {ParserPart.Emit (-1)})");
        }
    }

    public class AnglrFilePartListVisual : AnglrRawDrawingVisual, IAnglrEventHandler
    {
        public _anglr_file_part_list_ FilePartList { get; private set; }
        public AnglrFilePartListVisual (IAnglrLogger logger, _anglr_file_part_list_ filePartList) : base (logger)
        {
            FilePartList = filePartList;
            FilePartList.Iterate
            (
                0,
                (node, data) =>
                {
                    int counter = (int) data;
                    _anglr_file_part_ part = node.m__anglr_file_part_;
                    if ((_anglr_file_part_.production_kind) part.kind != _anglr_file_part_.production_kind.g__anglr_file_part__5)
                        return counter;
                    _parser_part_ parserPart = part.m__parser_part_;
                    if ((parserPart == null) || (parserPart.appInfo == null) || !((AppInfo) parserPart.appInfo).TryGetValue (AppInfoType.Visual, out var visual))
                        return counter;
                    AnglrRawDrawingVisual drawingVisual = visual as AnglrRawDrawingVisual;
                    if (drawingVisual == null)
                        return counter;
                    if (Width < drawingVisual.Width)
                        Width = drawingVisual.Width;
                    Height += drawingVisual.Height + 10 * Margin;
                    if (counter == 0)
                        ConnectorOffset = drawingVisual.ConnectorOffset;
                    return counter + 1;
                }
            );
        }

        public override void Draw ()
        {
            AnglrVisualPosition position = new AnglrVisualPosition ();
            double x, y;

            double height = 0;
            using (var dc = RenderOpen ())
            {
                FilePartList.Iterate
                (
                    0,
                    (node, data) =>
                    {
                        int counter = (int) data;
                        _anglr_file_part_ part = node.m__anglr_file_part_;
                        if ((_anglr_file_part_.production_kind) part.kind != _anglr_file_part_.production_kind.g__anglr_file_part__5)
                            return counter;
                        _parser_part_ parserPart = part.m__parser_part_;
                        if ((parserPart == null) || (parserPart.appInfo == null) || !((AppInfo) parserPart.appInfo).TryGetValue (AppInfoType.Visual, out var visual))
                            return counter;
                        AnglrRawDrawingVisual drawingVisual = visual as AnglrRawDrawingVisual;
                        if (drawingVisual == null)
                            return counter;
                        dc.PushTransform (new TranslateTransform (x = 0, y = height));
                        drawingVisual.Position += position.PushPosition (x, y);
                        dc.DrawDrawing (drawingVisual.Drawing);
                        drawingVisual.AnglrVisualParent = this;
                        dc.Pop ();
                        position.PopPosition ();
                        height += drawingVisual.Height + 10 * Margin;
                        return counter + 1;
                    }
                );
            }
            Drawing.Freeze ();
        }

        public override void Display ()
        {
            if (Debug)
                Logger?.InfoLine ($"file part list, id = {Id}, position = {Position}, size = {Size}, value =  {FilePartList.Emit (-1)}");
        }

        public void OnMouseDown (object sender, MouseButtonEventArgs e, Point point, IAnglrLogger logger)
        {
            if (Debug)
                Logger?.InfoLine ($"OnMouseDown (file part list, id = {Id}, value =  {FilePartList.Emit (-1)})");
        }

        public void OnMouseEnter (object sender, MouseEventArgs e, Point point, IAnglrLogger logger)
        {
            if (Debug)
                Logger?.InfoLine ($"OnMouseEner (file part list, id = {Id}, value =  {FilePartList.Emit (-1)})");
        }

        public void OnMouseLeave (object sender, MouseEventArgs e, Point point, IAnglrLogger logger)
        {
            if (Debug)
                Logger?.InfoLine ($"OnMouseLeave (file part list, id = {Id}, value =  {FilePartList.Emit (-1)})");
        }

        public void OnMouseLeftButtonDown (object sender, MouseButtonEventArgs e, Point point, IAnglrLogger logger)
        {
            if (Debug)
                Logger?.InfoLine ($"OnMouseLeftButtonDown (file part list, id = {Id}, value =  {FilePartList.Emit (-1)})");
        }

        public void OnMouseLeftButtonUp (object sender, MouseButtonEventArgs e, Point point, IAnglrLogger logger)
        {
            if (Debug)
                Logger?.InfoLine ($"OnMouseLeftButtonUp (file part list, id = {Id}, value =  {FilePartList.Emit (-1)})");
        }

        public void OnMouseMove (object sender, MouseEventArgs e, Point point, IAnglrLogger logger)
        {
            if (Debug)
                Logger?.InfoLine ($"OnMouseMove (file part list, id = {Id}, value =  {FilePartList.Emit (-1)})");
        }

        public void OnMouseRightButtonDown (object sender, MouseButtonEventArgs e, Point point, IAnglrLogger logger)
        {
            if (Debug)
                Logger?.InfoLine ($"OnMouseRightButtonDown (file part list, id = {Id}, value =  {FilePartList.Emit (-1)})");
        }

        public void OnMouseRightButtonUp (object sender, MouseButtonEventArgs e, Point point, IAnglrLogger logger)
        {
            if (Debug)
                Logger?.InfoLine ($"OnMouseRightButtonUp (file part list, id = {Id}, value =  {FilePartList.Emit (-1)})");
        }

        public void OnMouseUp (object sender, MouseButtonEventArgs e, Point point, IAnglrLogger logger)
        {
            if (Debug)
                Logger?.InfoLine ($"OnMouseUp (file part list, id = {Id}, value =  {FilePartList.Emit (-1)})");
        }
        public void OnMouseWheel (object sender, MouseWheelEventArgs e, Point point, IAnglrLogger logger)
        {
            if (Debug)
                Logger?.InfoLine ($"OnMousewheel (file part list, id = {Id}, value =  {FilePartList.Emit (-1)})");
        }
    }

    public static class AnglrSyntaxRuleDrawingBuilder
    {
        public static AnglrDrawingVisual DrawProductionNameSymbol (string name)
        {
            AnglrSyntaxProductionNameVisual visual = new AnglrSyntaxProductionNameVisual ();
            visual.Draw (name);
            return visual;
        }

        public static AnglrDrawingVisual DrawTerminalSymbol (string name, int index)
        {
            AnglrTerminalSymbolVisual visual = new AnglrTerminalSymbolVisual ();
            visual.Draw (name, index);
            return visual;
        }

        public static AnglrDrawingVisual DrawConstantSymbol (string name, int index)
        {
            AnglrConstantSymbolVisual visual = new AnglrConstantSymbolVisual ();
            visual.Draw (name, index);
            return visual;
        }

        public static AnglrDrawingVisual DrawNonTerminalSymbol (string name, int index)
        {
            AnglrNonTerminalSymbolVisual visual = new AnglrNonTerminalSymbolVisual ();
            visual.Draw (name, index);
            return visual;
        }

        public static AnglrRawDrawingVisual DrawRawTerminalSymbol (IAnglrLogger logger, SimpleSymbolToken symbolToken)
        {
            AnglrRawTerminalSymbolVisual visual = new AnglrRawTerminalSymbolVisual (logger, symbolToken);
            visual.Draw ();
            return visual;
        }

        public static AnglrRawDrawingVisual DrawRawConstantSymbol (IAnglrLogger logger, SimpleSymbolToken symbolToken)
        {
            AnglrRawConstantSymbolVisual visual = new AnglrRawConstantSymbolVisual (logger, symbolToken);
            visual.Draw ();
            return visual;
        }

        public static AnglrRawDrawingVisual DrawRawNonTerminalSymbol (IAnglrLogger logger, SimpleSymbolToken symbolToken)
        {
            AnglrRawNonTerminalSymbolVisual visual = new AnglrRawNonTerminalSymbolVisual (logger, symbolToken);
            visual.Draw ();
            return visual;
        }

        public static AnglrRawDrawingVisual DrawGeneralizedSymbol (IAnglrLogger logger, _g_name_ name)
        {
            AnglrGeneralizedNameVisual visual = new AnglrGeneralizedNameVisual (logger, name);
            visual.Draw ();
            return visual;
        }

        public static AnglrRawDrawingVisual DrawNameList (IAnglrLogger logger, _name_list_ nameList)
        {
            AnglrNameListVisual visual = new AnglrNameListVisual (logger, nameList);
            visual.Draw ();
            return visual;
        }

        public static AnglrRawDrawingVisual DrawNestedRule (IAnglrLogger logger, _anglr_nested_rule_ nestedRule)
        {
            AnglrNestedRuleVisual visual = new AnglrNestedRuleVisual (logger, nestedRule);
            visual.Draw ();
            return visual;
        }

        public static AnglrRawDrawingVisual DrawSyntaxRuleName (IAnglrLogger logger, SyntaxTreeToken ruleName)
        {
            AnglrSyntaxRuleNameVisual visual = new AnglrSyntaxRuleNameVisual (logger, ruleName);
            visual.Draw ();
            return visual;
        }

        public static AnglrRawDrawingVisual DrawSyntaxRule (IAnglrLogger logger, _anglr_syntax_rule_ syntaxRule)
        {
            AnglrSyntaxRuleVisual visual = new AnglrSyntaxRuleVisual (logger, syntaxRule);
            visual.Draw ();
            return visual;
        }

        public static AnglrRawDrawingVisual DrawSyntaxGroupName (IAnglrLogger logger, SyntaxTreeToken groupName)
        {
            AnglrSyntaxGroupNameVisual visual = new AnglrSyntaxGroupNameVisual (logger, groupName);
            visual.Draw ();
            return visual;
        }

        public static AnglrRawDrawingVisual DrawSyntaxGroup (IAnglrLogger logger, _anglr_syntax_rule_ syntaxRule)
        {
            AnglrSyntaxGroupVisual visual = new AnglrSyntaxGroupVisual (logger, syntaxRule);
            visual.Draw ();
            return visual;
        }

        public static AnglrRawDrawingVisual DrawParserPart (IAnglrLogger logger, _parser_part_ parserPart)
        {
            AnglrParserPartVisual visual = new AnglrParserPartVisual (logger, parserPart);
            visual.Draw ();
            return visual;
        }

        public static AnglrRawDrawingVisual DrawAnglrFilePartList (IAnglrLogger logger, _anglr_file_part_list_ filePartList)
        {
            AnglrFilePartListVisual visual = new AnglrFilePartListVisual (logger, filePartList);
            visual.Draw ();
            return visual;
        }

        public static AnglrDrawingDictionary BuildCanonicalSyntaxRulesDrawings (AnglrGetParserSyntaxRulesResult syntaxRulesResult, IAnglrLogger logger)
        {
            AnglrDrawingDictionary SyntaxRuleVisuals = new AnglrDrawingDictionary ();
            foreach (var syntaxRule in syntaxRulesResult.SyntaxRuleList)
            {
                var syntaxRuleName = syntaxRule.SyntaxRuleName;
                var name = syntaxRuleName.Name;
                var id = syntaxRuleName.Id;
                foreach (var production in syntaxRule.Productions)
                {
                    int index = 0;
                    double horizontalOffset = 0.0;
                    AnglrContainerSymbolVisual container = new AnglrContainerSymbolVisual ();
                    var prodNr = production.ProductionNumber;
                    using (var dc = container.RenderOpen ())
                    {
                        AnglrDrawingVisual drawingVisual = DrawProductionNameSymbol (name);
                        container.Children.Add (drawingVisual);
                        horizontalOffset += drawingVisual.Drawing.Bounds.Width;
                        if (production.RhsNodeSet.Length == 0)
                        {
                            AnglrDrawingVisual drawing = DrawConstantSymbol ("%empty", index++);
                            drawing.Offset = new Vector (horizontalOffset, 0);
                            container.Children.Add (drawing);
                            horizontalOffset += drawing.Drawing.Bounds.Width;
                        }
                        else
                        {
                            foreach (var rhsNode in production.RhsNodeSet)
                            {
                                AnglrDrawingVisual drawing = null;
                                if (rhsNode.Declarator == 18)
                                {
                                    if ((rhsNode.Synonym != null) && (rhsNode.Synonym.Length > 0))
                                        drawing = DrawConstantSymbol (rhsNode.Synonym, index++);
                                    else
                                        drawing = DrawTerminalSymbol (rhsNode.Name, index++);
                                }
                                else
                                    drawing = DrawNonTerminalSymbol (rhsNode.Name, index++);
                                drawing.Offset = new Vector (horizontalOffset, 0);
                                container.Children.Add (drawing);
                                horizontalOffset += drawing.Drawing.Bounds.Width;
                            }
                        }
                    }
                    SyntaxRuleVisuals [production.ProductionNumber] = container;
                }
            }
            return SyntaxRuleVisuals;
        }

        public static AnglrRawDrawingVisual BuildSyntaxRulesVisual (AnglrGetSyntaxTreeResult syntaxTreeResult, IAnglrLogger logger)
        {
            try
            {
                logger?.DebugLine ($"syntax tree = {syntaxTreeResult?.SyntaxTree}");
                JsonSerializerSettings settings = new JsonSerializerSettings
                {
                    TypeNameHandling = TypeNameHandling.Objects,
                    MaxDepth = null
                };
                _anglr_file_fragment_ anglrFileFragment = JsonConvert.DeserializeObject<_anglr_file_fragment_> (syntaxTreeResult?.SyntaxTree, settings);
                if (anglrFileFragment != null)
                {
                    logger?.DebugLine ($"traverse anglr fragment");
                    anglrFileFragment.reparent (null);
                    if (false)
                    {
                        AnglrMagicNrGenerator magicNrGenerator = new AnglrMagicNrGenerator (anglrFileFragment);
                        int magicNr = magicNrGenerator.ComputeMagicNr ();
                        string fragmentText = magicNrGenerator.CreateText ();
                        logger?.InfoLine ($"anglr visualizer: magic nr = {magicNr}");
                        logger?.InfoLine ($"anglr visualizer: text = {fragmentText}");
                    }
                    AnglrVisualizer anglrVisualizer = new AnglrVisualizer (logger);
                    anglrVisualizer.Traverse (anglrFileFragment);
                    _anglr_file_ anglrFile = anglrFileFragment.m__anglr_file_;
                    if ((anglrFile == null) || (anglrFile.appInfo == null) || !((AppInfo) anglrFile.appInfo).TryGetValue (AppInfoType.Visual, out var visual))
                        return null;
                    (visual as AnglrRawDrawingVisual)?.ComputeVisualBounds ();
                    return visual as AnglrRawDrawingVisual;
                }
                else
                {
                    logger?.ErrorLine ($"null anglr fragment conversion");
                    return null;
                }
            }
            catch (Exception e)
            {
                logger?.ErrorLine (e, $"visualizer failure");
                return null;
            }
        }
    }

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

    public abstract class AnglrPDABaseDrawing : AnglrRawDrawingVisual
    {
        //
        // common properties
        //

        public static Brush PDAStateHeadBackground { get; set; } = Brushes.Orange;
        public static Brush PDAStateBodyBackground { get; set; } = Brushes.LightGray;

        //
        // object properties
        //

        public Point ConnectorPoint { get; set; }
        public AnglrPDADrawingType DrawingType { get; }
        public AnglrPDAStateTransitionInfo TransitionInfo { get; }
        public AnglrPDABaseDrawing (IAnglrLogger logger, AnglrPDAStateTransitionInfo transitionInfo, AnglrPDADrawingType drawingType) : base (logger)
        {
            TransitionInfo = transitionInfo;
            DrawingType = drawingType;
        }

    }

    public class AnglrPDASyntaxRuleNameDrawing : AnglrPDABaseDrawing, IAnglrEventHandler
    {
        public string Name { get; }
        public int SyntaxRuleNr { get; }
        public int ProductionNr { get; }
        public int StateNr { get; }
        public AnglrPDASyntaxRuleNameDrawing (IAnglrLogger logger, AnglrPDAStateTransitionInfo transitionInfo) : base (logger, transitionInfo, AnglrPDADrawingType.SyntaxRuleName)
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

                dc.PushOpacity (OpacityValue);
                dc.DrawRoundedRectangle (NonTerminalSymbolBackground, Pen, new Rect (0, 0, Width = width + 4 * Margin, Height = height + 4 * Margin), 2 * Margin, 2 * Margin);
                dc.DrawRoundedRectangle (NonTerminalSymbolBackground, Pen, new Rect (Margin, Margin, width + 2 * Margin, height + 2 * Margin), Margin, Margin);
                dc.Pop ();
                dc.DrawText (text, new Point (2 * Margin, 2 * Margin));
                ConnectorPoint = new Point (0, Height / 2.0);
            }
            Drawing.Freeze ();
        }
        public override void Display ()
        {
            throw new NotImplementedException ();
        }


        public void OnMouseDown (object sender, MouseButtonEventArgs e, Point point, IAnglrLogger logger)
        {
            if (Debug)
                Logger?.InfoLine ($"OnMouseDown (pda syntax rule name, id = {Id}, Name = {Name}, syntax rule nr. = {SyntaxRuleNr}, production nr. = {ProductionNr}, state nr. = {StateNr})");
        }

        public void OnMouseEnter (object sender, MouseEventArgs e, Point point, IAnglrLogger logger)
        {
            if (Debug)
                Logger?.InfoLine ($"OnMouseEner (pda syntax rule name, id = {Id}, Name = {Name}, syntax rule nr. = {SyntaxRuleNr}, production nr. = {ProductionNr}, state nr. = {StateNr})");
        }

        public void OnMouseLeave (object sender, MouseEventArgs e, Point point, IAnglrLogger logger)
        {
            if (Debug)
                Logger?.InfoLine ($"OnMouseLeave (pda syntax rule name, id = {Id}, Name = {Name}, syntax rule nr. = {SyntaxRuleNr}, production nr. = {ProductionNr}, state nr. = {StateNr})");
        }

        public void OnMouseLeftButtonDown (object sender, MouseButtonEventArgs e, Point point, IAnglrLogger logger)
        {
            if (Debug)
                Logger?.InfoLine ($"OnMouseLeftButtonDown (pda syntax rule name, id = {Id}, Name = {Name}, syntax rule nr. = {SyntaxRuleNr}, production nr. = {ProductionNr}, state nr. = {StateNr})");
        }

        public void OnMouseLeftButtonUp (object sender, MouseButtonEventArgs e, Point point, IAnglrLogger logger)
        {
            if (Debug)
                Logger?.InfoLine ($"OnMouseLeftButtonUp (pda syntax rule name, id = {Id}, Name = {Name}, syntax rule nr. = {SyntaxRuleNr}, production nr. = {ProductionNr}, state nr. = {StateNr})");
        }

        public void OnMouseMove (object sender, MouseEventArgs e, Point point, IAnglrLogger logger)
        {
            if (Debug)
                Logger?.InfoLine ($"OnMouseMove (pda syntax rule name, id = {Id}, Name = {Name}, syntax rule nr. = {SyntaxRuleNr}, production nr. = {ProductionNr}, state nr. = {StateNr})");
        }

        public void OnMouseRightButtonDown (object sender, MouseButtonEventArgs e, Point point, IAnglrLogger logger)
        {
            if (Debug)
                Logger?.InfoLine ($"OnMouseRightButtonDown (pda syntax rule name, id = {Id}, Name = {Name}, syntax rule nr. = {SyntaxRuleNr}, production nr. = {ProductionNr}, state nr. = {StateNr})");
        }

        public void OnMouseRightButtonUp (object sender, MouseButtonEventArgs e, Point point, IAnglrLogger logger)
        {
            if (Debug)
                Logger?.InfoLine ($"OnMouseRightButtonUp (pda syntax rule name, id = {Id}, Name = {Name}, syntax rule nr. = {SyntaxRuleNr}, production nr. = {ProductionNr}, state nr. = {StateNr})");
        }

        public void OnMouseUp (object sender, MouseButtonEventArgs e, Point point, IAnglrLogger logger)
        {
            if (Debug)
                Logger?.InfoLine ($"OnMouseUp (pda syntax rule name, id = {Id}, Name = {Name}, syntax rule nr. = {SyntaxRuleNr}, production nr. = {ProductionNr}, state nr. = {StateNr})");
        }
        public void OnMouseWheel (object sender, MouseWheelEventArgs e, Point point, IAnglrLogger logger)
        {
            if (Debug)
                Logger?.InfoLine ($"OnMousewheel (pda syntax rule name, id = {Id}, Name = {Name}, syntax rule nr. = {SyntaxRuleNr}, production nr. = {ProductionNr}, state nr. = {StateNr})");
        }
    }

    public class AnglrPDATerminalSymbolDrawing : AnglrPDABaseDrawing, IAnglrEventHandler
    {
        public AnglrGetParserStateItemResult StateInfo { get; }
        public AnglrGetParserStateSymbolTokenData SymbolName { get; }
        public string Name { get; }
        public int TokenCode { get; }
        public int StateNr { get; }
        public AnglrPDATerminalSymbolDrawing
        (
            IAnglrLogger logger,
            AnglrPDAStateTransitionInfo transitionInfo,
            AnglrGetParserStateItemResult stateInfo,
            AnglrGetParserStateSymbolTokenData symbolName
        ) : base (logger, transitionInfo, AnglrPDADrawingType.TerminalSymbol)
        {
            StateInfo = stateInfo;
            SymbolName = symbolName;
            Name = symbolName.Name;
            TokenCode = symbolName.Id;
            StateNr = (StateInfo != null) ? StateInfo.StateNumber : -1;
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

                dc.PushOpacity (OpacityValue);
                dc.DrawRectangle ((StateNr >= 0) ? TerminalSymbolBackground : Brushes.Transparent, Pen, new Rect (0, 0, Width = width + 2 * Margin, Height = height + 2 * Margin));
                dc.Pop ();
                dc.DrawText (text, new Point (Margin, Margin));
                ConnectorPoint = new Point (0, Height / 2.0);
            }
            Drawing.Freeze ();
        }
        public override void Display ()
        {
            throw new NotImplementedException ();
        }

        public void OnMouseDown (object sender, MouseButtonEventArgs e, Point point, IAnglrLogger logger)
        {
            if (Debug)
                Logger?.InfoLine ($"OnMouseDown (pda terminal symbol, id = {Id}, Name = {Name}, token = {TokenCode}, state nr. = {StateNr})");
        }

        public void OnMouseEnter (object sender, MouseEventArgs e, Point point, IAnglrLogger logger)
        {
            if (Debug)
                Logger?.InfoLine ($"OnMouseEner (pda terminal symbol, id = {Id}, Name = {Name}, token = {TokenCode}, state nr. = {StateNr})");
        }

        public void OnMouseLeave (object sender, MouseEventArgs e, Point point, IAnglrLogger logger)
        {
            if (Debug)
                Logger?.InfoLine ($"OnMouseLeave (pda terminal symbol, id = {Id}, Name = {Name}, token = {TokenCode}, state nr. = {StateNr})");
        }

        public void OnMouseLeftButtonDown (object sender, MouseButtonEventArgs e, Point point, IAnglrLogger logger)
        {
            if (Debug)
                Logger?.InfoLine ($"OnMouseLeftButtonDown (pda terminal symbol, id = {Id}, Name = {Name}, token = {TokenCode}, state nr. = {StateNr})");
        }

        public void OnMouseLeftButtonUp (object sender, MouseButtonEventArgs e, Point point, IAnglrLogger logger)
        {
            if (Debug)
                Logger?.InfoLine ($"OnMouseLeftButtonUp (pda terminal symbol, id = {Id}, Name = {Name}, token = {TokenCode}, state nr. = {StateNr})");
        }

        public void OnMouseMove (object sender, MouseEventArgs e, Point point, IAnglrLogger logger)
        {
            if (Debug)
                Logger?.InfoLine ($"OnMouseMove (pda terminal symbol, id = {Id}, Name = {Name}, token = {TokenCode}, state nr. = {StateNr})");
        }

        public void OnMouseRightButtonDown (object sender, MouseButtonEventArgs e, Point point, IAnglrLogger logger)
        {
            if (Debug)
                Logger?.InfoLine ($"OnMouseRightButtonDown (pda terminal symbol, id = {Id}, Name = {Name}, token = {TokenCode}, state nr. = {StateNr})");
        }

        public void OnMouseRightButtonUp (object sender, MouseButtonEventArgs e, Point point, IAnglrLogger logger)
        {
            if (Debug)
                Logger?.InfoLine ($"OnMouseRightButtonUp (pda terminal symbol, id = {Id}, Name = {Name}, token = {TokenCode}, state nr. = {StateNr})");
        }

        public void OnMouseUp (object sender, MouseButtonEventArgs e, Point point, IAnglrLogger logger)
        {
            if (Debug)
                Logger?.InfoLine ($"OnMouseUp (pda terminal symbol, id = {Id}, Name = {Name}, token = {TokenCode}, state nr. = {StateNr})");
        }
        public void OnMouseWheel (object sender, MouseWheelEventArgs e, Point point, IAnglrLogger logger)
        {
            if (Debug)
                Logger?.InfoLine ($"OnMousewheel (pda terminal symbol, id = {Id}, Name = {Name}, token = {TokenCode}, state nr. = {StateNr})");
        }

    }

    public class AnglrPDAConstantSymbolDrawing : AnglrPDABaseDrawing, IAnglrEventHandler
    {
        public AnglrGetParserStateItemResult StateInfo { get; }
        public AnglrGetParserStateSymbolTokenData SymbolName { get; }
        public string Name { get; }
        public int TokenCode { get; }
        public int StateNr { get; }
        public AnglrPDAConstantSymbolDrawing
        (
            IAnglrLogger logger,
            AnglrPDAStateTransitionInfo transitionInfo,
            AnglrGetParserStateItemResult stateInfo,
            AnglrGetParserStateSymbolTokenData symbolName
        ) : base (logger, transitionInfo, AnglrPDADrawingType.ConstantSymbol)
        {
            StateInfo = stateInfo;
            SymbolName = symbolName;
            Name = symbolName.Synonym;
            TokenCode = symbolName.Id;
            StateNr = (StateInfo != null) ? StateInfo.StateNumber : -1;
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

                dc.PushOpacity (OpacityValue);
                dc.DrawRectangle ((StateNr >= 0) ? ConstantSymbolBackground : Brushes.Transparent, Pen, new Rect (0, 0, Width = width + 2 * Margin, Height = height + 2 * Margin));
                dc.Pop ();
                dc.DrawText (text, new Point (Margin, Margin));
                ConnectorPoint = new Point (0, Height / 2.0);
            }
            Drawing.Freeze ();
        }
        public override void Display ()
        {
            throw new NotImplementedException ();
        }

        public void OnMouseDown (object sender, MouseButtonEventArgs e, Point point, IAnglrLogger logger)
        {
            if (Debug)
                Logger?.InfoLine ($"OnMouseDown (pda constant symbol, id = {Id}, Value = {Name})");
        }

        public void OnMouseEnter (object sender, MouseEventArgs e, Point point, IAnglrLogger logger)
        {
            if (Debug)
                Logger?.InfoLine ($"OnMouseEner (pda constant symbol, id = {Id}, Value = {Name})");
        }

        public void OnMouseLeave (object sender, MouseEventArgs e, Point point, IAnglrLogger logger)
        {
            if (Debug)
                Logger?.InfoLine ($"OnMouseLeave (pda constant symbol, id = {Id}, Value = {Name})");
        }

        public void OnMouseLeftButtonDown (object sender, MouseButtonEventArgs e, Point point, IAnglrLogger logger)
        {
            if (Debug)
                Logger?.InfoLine ($"OnMouseLeftButtonDown (pda constant symbol, id = {Id}, Value = {Name})");
        }

        public void OnMouseLeftButtonUp (object sender, MouseButtonEventArgs e, Point point, IAnglrLogger logger)
        {
            if (Debug)
                Logger?.InfoLine ($"OnMouseLeftButtonUp (pda constant symbol, id = {Id}, Value = {Name})");
        }

        public void OnMouseMove (object sender, MouseEventArgs e, Point point, IAnglrLogger logger)
        {
            if (Debug)
                Logger?.InfoLine ($"OnMouseMove (pda constant symbol, id = {Id}, Value = {Name})");
        }

        public void OnMouseRightButtonDown (object sender, MouseButtonEventArgs e, Point point, IAnglrLogger logger)
        {
            if (Debug)
                Logger?.InfoLine ($"OnMouseRightButtonDown (pda constant symbol, id = {Id}, Value = {Name})");
        }

        public void OnMouseRightButtonUp (object sender, MouseButtonEventArgs e, Point point, IAnglrLogger logger)
        {
            if (Debug)
                Logger?.InfoLine ($"OnMouseRightButtonUp (pda constant symbol, id = {Id}, Value = {Name})");
        }

        public void OnMouseUp (object sender, MouseButtonEventArgs e, Point point, IAnglrLogger logger)
        {
            if (Debug)
                Logger?.InfoLine ($"OnMouseUp (pda constant symbol, id = {Id}, Value = {Name})");
        }
        public void OnMouseWheel (object sender, MouseWheelEventArgs e, Point point, IAnglrLogger logger)
        {
            if (Debug)
                Logger?.InfoLine ($"OnMousewheel (pda constant symbol, id = {Id}, Value = {Name})");
        }

    }

    public class AnglrPDANonTerminalSymbolDrawing : AnglrPDABaseDrawing, IAnglrEventHandler
    {
        public AnglrGetParserStateItemResult StateInfo { get; }
        public AnglrGetParserStateSymbolTokenData SymbolName { get; }
        public string Name { get; }
        public int SyntaxRuleNr { get; }
        public int StateNr { get; }
        public AnglrPDANonTerminalSymbolDrawing
        (
            IAnglrLogger logger,
            AnglrPDAStateTransitionInfo transitionInfo,
            AnglrGetParserStateItemResult stateInfo,
            AnglrGetParserStateSymbolTokenData symbolName
        ) : base (logger, transitionInfo, AnglrPDADrawingType.NonTerminalSymbol)
        {
            StateInfo = stateInfo;
            SymbolName = symbolName;
            Name = symbolName.Name;
            SyntaxRuleNr = symbolName.Id;
            StateNr = (StateInfo != null) ? StateInfo.StateNumber : -1;
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

                dc.PushOpacity (OpacityValue);
                dc.DrawRoundedRectangle ((StateNr >= 0) ? NonTerminalSymbolBackground : Brushes.Transparent, Pen, new Rect (0, 0, Width = width + 2 * Margin, Height = height + 2 * Margin), Margin, Margin);
                dc.Pop ();
                dc.DrawText (text, new Point (Margin, Margin));
                ConnectorPoint = new Point (0, Height / 2.0);
            }
            Drawing.Freeze ();
        }
        public override void Display ()
        {
            throw new NotImplementedException ();
        }

        public void OnMouseDown (object sender, MouseButtonEventArgs e, Point point, IAnglrLogger logger)
        {
            if (Debug)
                Logger?.InfoLine ($"OnMouseDown (pda non terminal symbol, id = {Id}, Name = {Name}, syntax rule nr. = {SyntaxRuleNr}, state nr. = {StateNr})");
        }

        public void OnMouseEnter (object sender, MouseEventArgs e, Point point, IAnglrLogger logger)
        {
            if (Debug)
                Logger?.InfoLine ($"OnMouseEner (pda non terminal symbol, id = {Id}, Name = {Name}, syntax rule nr. = {SyntaxRuleNr}, state nr. = {StateNr})");
        }

        public void OnMouseLeave (object sender, MouseEventArgs e, Point point, IAnglrLogger logger)
        {
            if (Debug)
                Logger?.InfoLine ($"OnMouseLeave (pda non terminal symbol, id = {Id}, Name = {Name}, syntax rule nr. = {SyntaxRuleNr}, state nr. = {StateNr})");
        }

        public void OnMouseLeftButtonDown (object sender, MouseButtonEventArgs e, Point point, IAnglrLogger logger)
        {
            if (Debug)
                Logger?.InfoLine ($"OnMouseLeftButtonDown (pda non terminal symbol, id = {Id}, Name = {Name}, syntax rule nr. = {SyntaxRuleNr}, state nr. = {StateNr})");
        }

        public void OnMouseLeftButtonUp (object sender, MouseButtonEventArgs e, Point point, IAnglrLogger logger)
        {
            if (Debug)
                Logger?.InfoLine ($"OnMouseLeftButtonUp (pda non terminal symbol, id = {Id}, Name = {Name}, syntax rule nr. = {SyntaxRuleNr}, state nr. = {StateNr})");
        }

        public void OnMouseMove (object sender, MouseEventArgs e, Point point, IAnglrLogger logger)
        {
            if (Debug)
                Logger?.InfoLine ($"OnMouseMove (pda non terminal symbol, id = {Id}, Name = {Name}, syntax rule nr. = {SyntaxRuleNr}, state nr. = {StateNr})");
        }

        public void OnMouseRightButtonDown (object sender, MouseButtonEventArgs e, Point point, IAnglrLogger logger)
        {
            if (Debug)
                Logger?.InfoLine ($"OnMouseRightButtonDown (pda non terminal symbol, id = {Id}, Name = {Name}, syntax rule nr. = {SyntaxRuleNr}, state nr. = {StateNr})");
        }

        public void OnMouseRightButtonUp (object sender, MouseButtonEventArgs e, Point point, IAnglrLogger logger)
        {
            if (Debug)
                Logger?.InfoLine ($"OnMouseRightButtonUp (pda non terminal symbol, id = {Id}, Name = {Name}, syntax rule nr. = {SyntaxRuleNr}, state nr. = {StateNr})");
        }

        public void OnMouseUp (object sender, MouseButtonEventArgs e, Point point, IAnglrLogger logger)
        {
            if (Debug)
                Logger?.InfoLine ($"OnMouseUp (pda non terminal symbol, id = {Id}, Name = {Name}, syntax rule nr. = {SyntaxRuleNr}, state nr. = {StateNr})");
        }
        public void OnMouseWheel (object sender, MouseWheelEventArgs e, Point point, IAnglrLogger logger)
        {
            if (Debug)
                Logger?.InfoLine ($"OnMousewheel (pda non terminal symbol, id = {Id}, Name = {Name}, syntax rule nr. = {SyntaxRuleNr}, state nr. = {StateNr})");
        }

    }

    public class AnglrPDATransitionInfoDrawing : AnglrPDABaseDrawing, IAnglrEventHandler
    {
        public AnglrPDASyntaxRuleNameDrawing RuleName { get; }
        public List<AnglrPDABaseDrawing> ProductionParts { get; }
        public AnglrPDATransitionInfoDrawing (IAnglrLogger logger, AnglrPDAStateTransitionInfo transitionInfo) : base (logger, transitionInfo, AnglrPDADrawingType.PDATransition)
        {
            ProductionParts = new List<AnglrPDABaseDrawing> ();
            RuleName = new AnglrPDASyntaxRuleNameDrawing (logger, transitionInfo);
            AnglrPDABaseDrawing drawing = null;
            int index = 0;
            int count = transitionInfo.PDAStates.Count;
            foreach (var rhsNode in transitionInfo.Production.RhsNodeSet)
            {
                AnglrGetParserStateItemResult stateInfo = (index < count) ? transitionInfo.PDAStates [index++] : null;
                if (rhsNode.Declarator == 18)
                {
                    if ((rhsNode.Synonym != null) && (rhsNode.Synonym.Length > 0))
                        drawing = new AnglrPDAConstantSymbolDrawing (logger, transitionInfo, stateInfo, rhsNode);
                    else
                        drawing = new AnglrPDATerminalSymbolDrawing (logger, transitionInfo, stateInfo, rhsNode);
                }
                else
                    drawing = new AnglrPDANonTerminalSymbolDrawing (logger, transitionInfo, stateInfo, rhsNode);
                ProductionParts.Add (drawing);
            }
        }

        public override void Draw ()
        {
            AnglrVisualPosition position = new AnglrVisualPosition ();
            double x, y;

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
                dc.PushTransform (new TranslateTransform (x = 0, y = 2 * Margin));
                RuleName.Position += position.PushPosition (x, y);
                dc.DrawDrawing (RuleName.Drawing);
                RuleName.AnglrVisualParent = this;
                dc.PushTransform (new TranslateTransform (x = 0, y = RuleName.Height + 2 * Margin));
                position.PushPosition (x, y);
                foreach (var drawing in ProductionParts)
                {
                    dc.PushTransform (new TranslateTransform (x = width, y = (drawingHeight - drawing.Height) / 2));
                    drawing.Position += position.PushPosition (x, y);
                    dc.DrawDrawing (drawing.Drawing);
                    drawing.AnglrVisualParent = this;
                    dc.Pop ();
                    position.PopPosition ();
                    width += drawing.Width + 4 * Margin;
                }
                dc.Pop ();
                position.PopPosition ();
                dc.Pop ();
                position.PopPosition ();
            }
            Drawing.Freeze ();

            Width = Math.Max (RuleName.Width + 2 * Margin, width);
            Height = height;
        }
        public override void Display ()
        {
            throw new NotImplementedException ();
        }

        public void OnMouseDown (object sender, MouseButtonEventArgs e, Point point, IAnglrLogger logger)
        {
            if (Debug)
                Logger?.InfoLine ($"OnMouseDown (pda transition info, id = {Id})");
        }

        public void OnMouseEnter (object sender, MouseEventArgs e, Point point, IAnglrLogger logger)
        {
            if (Debug)
                Logger?.InfoLine ($"OnMouseEner (pda transition info, id = {Id})");
        }

        public void OnMouseLeave (object sender, MouseEventArgs e, Point point, IAnglrLogger logger)
        {
            if (Debug)
                Logger?.InfoLine ($"OnMouseLeave (pda transition info, id = {Id})");
        }

        public void OnMouseLeftButtonDown (object sender, MouseButtonEventArgs e, Point point, IAnglrLogger logger)
        {
            if (Debug)
                Logger?.InfoLine ($"OnMouseLeftButtonDown (pda transition info, id = {Id})");
        }

        public void OnMouseLeftButtonUp (object sender, MouseButtonEventArgs e, Point point, IAnglrLogger logger)
        {
            if (Debug)
                Logger?.InfoLine ($"OnMouseLeftButtonUp (pda transition info, id = {Id})");
        }

        public void OnMouseMove (object sender, MouseEventArgs e, Point point, IAnglrLogger logger)
        {
            if (Debug)
                Logger?.InfoLine ($"OnMouseMove (pda transition info, id = {Id})");
        }

        public void OnMouseRightButtonDown (object sender, MouseButtonEventArgs e, Point point, IAnglrLogger logger)
        {
            if (Debug)
                Logger?.InfoLine ($"OnMouseRightButtonDown (pda transition info, id = {Id})");
        }

        public void OnMouseRightButtonUp (object sender, MouseButtonEventArgs e, Point point, IAnglrLogger logger)
        {
            if (Debug)
                Logger?.InfoLine ($"OnMouseRightButtonUp (pda transition info, id = {Id})");
        }

        public void OnMouseUp (object sender, MouseButtonEventArgs e, Point point, IAnglrLogger logger)
        {
            if (Debug)
                Logger?.InfoLine ($"OnMouseUp (pda transition info, id = {Id})");
        }
        public void OnMouseWheel (object sender, MouseWheelEventArgs e, Point point, IAnglrLogger logger)
        {
            if (Debug)
                Logger?.InfoLine ($"OnMousewheel (pda transition info, id = {Id})");
        }

    }

    public class AnglrPDAStateDrawing : AnglrPDABaseDrawing, IAnglrEventHandler
    {
        public AnglrStateItemResultList StateItemResults { get; }
        public List<AnglrPDATransitionInfoDrawing> TransitionInfoDrawings { get; }

        public AnglrPDAStateDrawing (IAnglrLogger logger, AnglrStateItemResultList stateItemResults) : base (logger, null, AnglrPDADrawingType.PDAState)
        {
            StateItemResults = stateItemResults;
            TransitionInfoDrawings = new List<AnglrPDATransitionInfoDrawing> ();
        }
        public void Add (AnglrPDATransitionInfoDrawing transitionInfoDrawing) => TransitionInfoDrawings.Add (transitionInfoDrawing);
        public override void Draw ()
        {
            AnglrVisualPosition position = new AnglrVisualPosition ();
            double x, y;

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
            width += Margin;
            height += 2 * Margin;
            foreach (var transition in TransitionInfoDrawings)
            {
                transition.Draw ();
                height += transition.Height + 2 * Margin;
                width = Math.Max (width, transition.Width);
            }
            using (var dc = RenderOpen ())
            {
                dc.PushTransform (new TranslateTransform (0, Margin));
                dc.PushOpacity (OpacityValue);
                dc.DrawRoundedRectangle (PDAStateHeadBackground, Pen, new Rect (0, 0, width, text.Height + 4 * Margin), 2 * Margin, 2 * Margin);
                dc.PushOpacity (OpacityValue);
                dc.DrawRoundedRectangle (PDAStateBodyBackground, Pen, new Rect (0, text.Height + 2 * Margin, width, height - text.Height - 3 * Margin), 2 * Margin, 2 * Margin);
                dc.Pop ();
                dc.Pop ();
                dc.DrawText (text, new Point (Margin, Margin));
                dc.Pop ();

                width = text.Width + Margin;
                height = text.Height + 2 * Margin;
                foreach (var transition in TransitionInfoDrawings)
                {
                    dc.PushTransform (new TranslateTransform (x = 0, y = height));
                    transition.Position += position.PushPosition (x, y);
                    dc.DrawDrawing (transition.Drawing);
                    transition.AnglrVisualParent = this;
                    dc.Pop ();
                    position.PopPosition ();
                    height += transition.Height + 2 * Margin;
                    width = Math.Max (width, transition.Width);
                }
                if (false)
                {
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

        public void OnMouseDown (object sender, MouseButtonEventArgs e, Point point, IAnglrLogger logger)
        {
            if (Debug)
                Logger?.InfoLine ($"OnMouseDown (pda state, id = {Id})");
        }

        public void OnMouseEnter (object sender, MouseEventArgs e, Point point, IAnglrLogger logger)
        {
            if (Debug)
                Logger?.InfoLine ($"OnMouseEner (pda state, id = {Id})");
        }

        public void OnMouseLeave (object sender, MouseEventArgs e, Point point, IAnglrLogger logger)
        {
            if (Debug)
                Logger?.InfoLine ($"OnMouseLeave (pda state, id = {Id})");
        }

        public void OnMouseLeftButtonDown (object sender, MouseButtonEventArgs e, Point point, IAnglrLogger logger)
        {
            if (Debug)
                Logger?.InfoLine ($"OnMouseLeftButtonDown (pda state, id = {Id})");
        }

        public void OnMouseLeftButtonUp (object sender, MouseButtonEventArgs e, Point point, IAnglrLogger logger)
        {
            if (Debug)
                Logger?.InfoLine ($"OnMouseLeftButtonUp (pda state, id = {Id})");
        }

        public void OnMouseMove (object sender, MouseEventArgs e, Point point, IAnglrLogger logger)
        {
            if (Debug)
                Logger?.InfoLine ($"OnMouseMove (pda state, id = {Id})");
        }

        public void OnMouseRightButtonDown (object sender, MouseButtonEventArgs e, Point point, IAnglrLogger logger)
        {
            if (Debug)
                Logger?.InfoLine ($"OnMouseRightButtonDown (pda state, id = {Id})");
        }

        public void OnMouseRightButtonUp (object sender, MouseButtonEventArgs e, Point point, IAnglrLogger logger)
        {
            if (Debug)
                Logger?.InfoLine ($"OnMouseRightButtonUp (pda state, id = {Id})");
        }

        public void OnMouseUp (object sender, MouseButtonEventArgs e, Point point, IAnglrLogger logger)
        {
            if (Debug)
                Logger?.InfoLine ($"OnMouseUp (pda state, id = {Id})");
        }
        public void OnMouseWheel (object sender, MouseWheelEventArgs e, Point point, IAnglrLogger logger)
        {
            if (Debug)
                Logger?.InfoLine ($"OnMousewheel (pda state, id = {Id})");
        }

    }

    public class AnglrPDASnapshotDrawing : AnglrPDABaseDrawing, IAnglrEventHandler
    {
        public List<AnglrPDAStateDrawing> PDAStatesDrawings { get; }
        public AnglrPDASnapshotDrawing (IAnglrLogger logger, AnglrPDASet pdaSet) : base (logger, null, AnglrPDADrawingType.PDASet)
        {
            List<AnglrPDATransitionInfoDrawing> PDASet = new List<AnglrPDATransitionInfoDrawing> ();
            foreach (var element in pdaSet)
                if (element.Parent == null)
                    element.Traverse
                    (
                        (info, data) => PDASet.Add (new AnglrPDATransitionInfoDrawing (logger, info)),
                        null
                    );
            PDAStatesDrawings = new List<AnglrPDAStateDrawing> ();
            AnglrStateItemResultList stateItemResults = new AnglrStateItemResultList ();
            AnglrStateItemResultListComparer comparer = new AnglrStateItemResultListComparer ();
            AnglrPDAStateDrawing stateSet = null;
            foreach (var drawing in PDASet)
            {
                if (comparer.Compare (drawing.TransitionInfo.PDAStates, stateItemResults) != 0)
                    PDAStatesDrawings.Add (stateSet = new AnglrPDAStateDrawing (logger, stateItemResults = drawing.TransitionInfo.PDAStates));
                stateSet.Add (drawing);
            }
        }

        public override void Draw ()
        {
            AnglrVisualPosition position = new AnglrVisualPosition ();
            double x, y;

            foreach (var drawing in PDAStatesDrawings)
                drawing.Draw ();

            double height = 0;
            double width = 0;
            using (var dc = RenderOpen ())
            {
                foreach (var drawing in PDAStatesDrawings)
                {
                    dc.PushTransform (new TranslateTransform (x = 0, y = height));
                    drawing.Position += position.PushPosition (x, y);
                    dc.DrawDrawing (drawing.Drawing);
                    dc.Pop ();
                    position.PopPosition ();
                    drawing.AnglrVisualParent = this;
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

        public void OnMouseDown (object sender, MouseButtonEventArgs e, Point point, IAnglrLogger logger)
        {
            if (Debug)
                Logger?.InfoLine ($"OnMouseDown (pda snapshot, id = {Id})");
        }

        public void OnMouseEnter (object sender, MouseEventArgs e, Point point, IAnglrLogger logger)
        {
            if (Debug)
                Logger?.InfoLine ($"OnMouseEner (pda snapshot, id = {Id})");
        }

        public void OnMouseLeave (object sender, MouseEventArgs e, Point point, IAnglrLogger logger)
        {
            if (Debug)
                Logger?.InfoLine ($"OnMouseLeave (pda snapshot, id = {Id})");
        }

        public void OnMouseLeftButtonDown (object sender, MouseButtonEventArgs e, Point point, IAnglrLogger logger)
        {
            if (Debug)
                Logger?.InfoLine ($"OnMouseLeftButtonDown (pda snapshot, id = {Id})");
        }

        public void OnMouseLeftButtonUp (object sender, MouseButtonEventArgs e, Point point, IAnglrLogger logger)
        {
            if (Debug)
                Logger?.InfoLine ($"OnMouseLeftButtonUp (pda snapshot, id = {Id})");
        }

        public void OnMouseMove (object sender, MouseEventArgs e, Point point, IAnglrLogger logger)
        {
            if (Debug)
                Logger?.InfoLine ($"OnMouseMove (pda snapshot, id = {Id})");
        }

        public void OnMouseRightButtonDown (object sender, MouseButtonEventArgs e, Point point, IAnglrLogger logger)
        {
            if (Debug)
                Logger?.InfoLine ($"OnMouseRightButtonDown (pda snapshot, id = {Id})");
        }

        public void OnMouseRightButtonUp (object sender, MouseButtonEventArgs e, Point point, IAnglrLogger logger)
        {
            if (Debug)
                Logger?.InfoLine ($"OnMouseRightButtonUp (pda snapshot, id = {Id})");
        }

        public void OnMouseUp (object sender, MouseButtonEventArgs e, Point point, IAnglrLogger logger)
        {
            if (Debug)
                Logger?.InfoLine ($"OnMouseUp (pda snapshot, id = {Id})");
        }
        public void OnMouseWheel (object sender, MouseWheelEventArgs e, Point point, IAnglrLogger logger)
        {
            if (Debug)
                Logger?.InfoLine ($"OnMousewheel (pda snapshot, id = {Id})");
        }

    }

    public class AnglrVisualizer : SyntaxTreeWalker
    {
        _anglr_file_fragment_ Fragment { get; set; }
        public IAnglrLogger AnglrLogger { get; private set; }
        public AnglrVisualizer (IAnglrLogger logger)
        {
            AnglrLogger = logger ?? new VoidAnglrLogger ();
            _anglr_file_fragment__Event += AnglrVisualizer__anglr_file_fragment__Event;
            _anglr_file__Event += AnglrVisualizer__anglr_file__Event;
            _parser_part__Event += AnglrVisualizer__parser_part__Event;
            _anglr_syntax_rule__Event += AnglrVisualizer__anglr_syntax_rule__Event;
            _anglr_nested_rule__Event += AnglrVisualizer__anglr_nested_rule__Event;
            _anglr_syntax_production_list_name__Event += AnglrVisualizer__anglr_syntax_production_list_name__Event;
            _anglr_syntax_production__Event += AnglrVisualizer__anglr_syntax_production__Event;
            _g_name__Event += AnglrVisualizer__g_name__Event;
            _name__Event += AnglrVisualizer__name__Event;
        }

        private bool AnglrVisualizer__anglr_file_fragment__Event (SyntaxTreeCallbackReason reason, _anglr_file_fragment_.production_kind kind, _anglr_file_fragment_ p__anglr_file_fragment_)
        {
            switch (reason)
            {
                case SyntaxTreeCallbackReason.TraversalPrologueCallbackReason:
                    Fragment = p__anglr_file_fragment_;
                    break;
                case SyntaxTreeCallbackReason.TraversalEpilogueCallbackReason:
                    break;
            }
            return true;
        }

        private bool AnglrVisualizer__anglr_file__Event (SyntaxTreeCallbackReason reason, _anglr_file_.production_kind kind, _anglr_file_ p__anglr_file_)
        {
            switch (reason)
            {
                case SyntaxTreeCallbackReason.TraversalPrologueCallbackReason:
                    break;
                case SyntaxTreeCallbackReason.TraversalEpilogueCallbackReason:
                {
                    try
                    {
                        _anglr_file_part_list_ filePartList = p__anglr_file_.m__anglr_file_part_list_;
                        if (filePartList == null)
                            break;
                        ((AppInfo) p__anglr_file_.appInfo) [AppInfoType.Visual] =
                            AnglrSyntaxRuleDrawingBuilder.DrawAnglrFilePartList (AnglrLogger, filePartList);
                    }
                    catch (Exception e)
                    {
                        AnglrLogger?.ErrorLine (e, $"Visualization of <anglr file> node {p__anglr_file_.Emit (-1)} failed");
                    }
                }
                break;
            }
            return true;
        }

        private bool AnglrVisualizer__parser_part__Event (SyntaxTreeCallbackReason reason, _parser_part_.production_kind kind, _parser_part_ p__parser_part_)
        {
            switch (reason)
            {
                case SyntaxTreeCallbackReason.TraversalPrologueCallbackReason:
                    break;
                case SyntaxTreeCallbackReason.TraversalEpilogueCallbackReason:
                    try
                    {
                        ((AppInfo) p__parser_part_.appInfo) [AppInfoType.Visual] =
                            AnglrSyntaxRuleDrawingBuilder.DrawParserPart (AnglrLogger, p__parser_part_);
                    }
                    catch (Exception e)
                    {
                        AnglrLogger?.ErrorLine (e, $"Visualization of <anglr syntax rule> node {p__parser_part_.Emit (-1)} failed");
                    }
                    break;
            }
            return true;
        }

        private bool AnglrVisualizer__anglr_syntax_rule__Event (SyntaxTreeCallbackReason reason, _anglr_syntax_rule_.production_kind kind, _anglr_syntax_rule_ p__anglr_syntax_rule_)
        {
            switch (reason)
            {
                case SyntaxTreeCallbackReason.TraversalPrologueCallbackReason:
                    break;
                case SyntaxTreeCallbackReason.TraversalEpilogueCallbackReason:
                {
                    try
                    {
                        switch (kind)
                        {
                            case _anglr_syntax_rule_.production_kind.g__anglr_syntax_rule__1:
                            {
                                ((AppInfo) p__anglr_syntax_rule_.m__identifier_.appInfo) [AppInfoType.Visual] =
                                    AnglrSyntaxRuleDrawingBuilder.DrawSyntaxRuleName (AnglrLogger, p__anglr_syntax_rule_.m__identifier_);
                                ((AppInfo) p__anglr_syntax_rule_.appInfo) [AppInfoType.Visual] =
                                    AnglrSyntaxRuleDrawingBuilder.DrawSyntaxRule (AnglrLogger, p__anglr_syntax_rule_);
                            }
                            break;
                            case _anglr_syntax_rule_.production_kind.g__anglr_syntax_rule__2:
                            {
                                ((AppInfo) p__anglr_syntax_rule_.m__identifier_.appInfo) [AppInfoType.Visual] =
                                    AnglrSyntaxRuleDrawingBuilder.DrawSyntaxGroupName (AnglrLogger, p__anglr_syntax_rule_.m__identifier_);
                                ((AppInfo) p__anglr_syntax_rule_.appInfo) [AppInfoType.Visual] =
                                    AnglrSyntaxRuleDrawingBuilder.DrawSyntaxGroup (AnglrLogger, p__anglr_syntax_rule_);
                            }
                            break;
                        }
                    }
                    catch (Exception e)
                    {
                        AnglrLogger?.ErrorLine (e, $"Visualization of <anglr syntax rule> node {p__anglr_syntax_rule_.Emit (-1)} failed");
                    }
                }
                break;
            }
            return true;
        }

        private bool AnglrVisualizer__anglr_nested_rule__Event (SyntaxTreeCallbackReason reason, _anglr_nested_rule_.production_kind kind, _anglr_nested_rule_ p__anglr_nested_rule_)
        {
            switch (reason)
            {
                case SyntaxTreeCallbackReason.TraversalPrologueCallbackReason:
                    break;
                case SyntaxTreeCallbackReason.TraversalEpilogueCallbackReason:
                    try
                    {
                        ((AppInfo) p__anglr_nested_rule_.appInfo) [AppInfoType.Visual] =
                            AnglrSyntaxRuleDrawingBuilder.DrawNestedRule (AnglrLogger, p__anglr_nested_rule_);
                    }
                    catch (Exception e)
                    {
                        AnglrLogger?.ErrorLine (e, $"Visualization of <anglr nested rule> node {p__anglr_nested_rule_.Emit (-1)} failed");
                    }
                    break;
            }
            return true;
        }

        private bool AnglrVisualizer__anglr_syntax_production_list_name__Event (SyntaxTreeCallbackReason reason, _anglr_syntax_production_list_name_.production_kind kind, _anglr_syntax_production_list_name_ p__anglr_syntax_production_list_name_)
        {
            switch (reason)
            {
                case SyntaxTreeCallbackReason.TraversalPrologueCallbackReason:
                    break;
                case SyntaxTreeCallbackReason.TraversalEpilogueCallbackReason:
                    ((AppInfo) p__anglr_syntax_production_list_name_.appInfo) [AppInfoType.Visual] =
                        AnglrSyntaxRuleDrawingBuilder.DrawSyntaxRuleName (AnglrLogger, p__anglr_syntax_production_list_name_.m__identifier_);
                    break;

            }
            return true;
        }

        private bool AnglrVisualizer__anglr_syntax_production__Event (SyntaxTreeCallbackReason reason, _anglr_syntax_production_.production_kind kind, _anglr_syntax_production_ p__anglr_syntax_production_)
        {
            switch (reason)
            {
                case SyntaxTreeCallbackReason.TraversalPrologueCallbackReason:
                    break;
                case SyntaxTreeCallbackReason.TraversalEpilogueCallbackReason:
                {
                    try
                    {
                        switch (kind)
                        {
                            case _anglr_syntax_production_.production_kind.g__anglr_syntax_production__1:
                            {
                                ((AppInfo) p__anglr_syntax_production_.appInfo) [AppInfoType.Visual] =
                                    AnglrSyntaxRuleDrawingBuilder.DrawNameList (AnglrLogger, p__anglr_syntax_production_.m__name_list_);
                            }
                            break;
                            case _anglr_syntax_production_.production_kind.g__anglr_syntax_production__2:
                            {
                                if (!((AppInfo) p__anglr_syntax_production_.m__empty_.appInfo).TryGetValue (AppInfoType.Visual, out var visual))
                                    break;
                                ((AppInfo) p__anglr_syntax_production_.appInfo) [AppInfoType.Visual] = visual;
                            }
                            break;
                        }
                    }
                    catch (Exception e)
                    {
                        AnglrLogger?.ErrorLine (e, $"Visualization of <anglr syntax production> node {p__anglr_syntax_production_.Emit (-1)} failed");
                    }
                }
                break;
            }
            return true;
        }

        private bool AnglrVisualizer__g_name__Event (SyntaxTreeCallbackReason reason, _g_name_.production_kind kind, _g_name_ p__g_name_)
        {
            switch (reason)
            {
                case SyntaxTreeCallbackReason.TraversalPrologueCallbackReason:
                    break;
                case SyntaxTreeCallbackReason.TraversalEpilogueCallbackReason:
                    try
                    {
                        switch (kind)
                        {
                            case _g_name_.production_kind.g__g_name__1:
                            {
                                if (!((AppInfo) p__g_name_.m__name_.appInfo).TryGetValue (AppInfoType.Visual, out var visual))
                                    break;
                                ((AppInfo) p__g_name_.appInfo) [AppInfoType.Visual] = visual;
                            }
                            break;
                            case _g_name_.production_kind.g__g_name__2:
                            {
                                if (!((AppInfo) p__g_name_.m__anglr_nested_rule_.appInfo).TryGetValue (AppInfoType.Visual, out var visual))
                                    break;
                                ((AppInfo) p__g_name_.appInfo) [AppInfoType.Visual] = visual;
                            }
                            break;
                            case _g_name_.production_kind.g__g_name__3:
                            {
                                if (!((AppInfo) p__g_name_.m__g_name_.appInfo).TryGetValue (AppInfoType.Visual, out var gnameVisual))
                                    break;
                                ((AppInfo) p__g_name_.appInfo) [AppInfoType.Visual] =
                                    AnglrSyntaxRuleDrawingBuilder.DrawGeneralizedSymbol (AnglrLogger, p__g_name_);
                            }
                            break;
                        }
                    }
                    catch (Exception e)
                    {
                        AnglrLogger?.ErrorLine (e, $"Visualization of <g name> {p__g_name_.Emit (-1)} failed");
                    }
                    break;
            }
            return true;
        }

        private bool AnglrVisualizer__name__Event (SyntaxTreeCallbackReason reason, _name_.production_kind kind, _name_ p__name_)
        {
            switch (reason)
            {
                case SyntaxTreeCallbackReason.TraversalPrologueCallbackReason:
                    break;
                case SyntaxTreeCallbackReason.TraversalEpilogueCallbackReason:
                    try
                    {
                        AnglrRawDrawingVisual visual = null;
                        switch (kind)
                        {
                            case _name_.production_kind.g__name__1:
                                break;
                            case _name_.production_kind.g__name__2:
                            {
                                SyntaxTreeToken token = p__name_.m__cstring_;

                                AppInfo appInfo = token.appInfo as AppInfo;
                                if ((appInfo != null) && appInfo.TryGetValue (AppInfoType.SimpleSymbolToken, out var simpleSymbolToken))
                                {
                                    SimpleSymbolToken p_SymbolToken = simpleSymbolToken as SimpleSymbolToken;
                                    if (p_SymbolToken != null)
                                    {
                                        AnglrLogger?.DebugLine ($"constant symbol name = {p_SymbolToken.Name}");
                                        visual = AnglrSyntaxRuleDrawingBuilder.DrawRawConstantSymbol (AnglrLogger, p_SymbolToken);
                                    }
                                    else
                                        AnglrLogger?.WarnLine ($"symbol info for {token.text} is null");
                                }
                                else
                                    AnglrLogger?.WarnLine ($"appInfo for {token.text} is null");
                            }
                            break;
                            case _name_.production_kind.g__name__3:
                            {
                                SyntaxTreeToken identifier = p__name_.m__identifier_;

                                AppInfo appInfo = identifier.appInfo as AppInfo;
                                if ((appInfo != null) && appInfo.TryGetValue (AppInfoType.SimpleSymbolToken, out var simpleSymbolToken))
                                {
                                    SimpleSymbolToken p_SymbolToken = simpleSymbolToken as SimpleSymbolToken;
                                    if (p_SymbolToken != null)
                                    {
                                        if (p_SymbolToken.Declarator != (uint) AnglrClassificationType.NonTerminalName)
                                        {
                                            AnglrLogger?.DebugLine ($"terminal symbol name = {p_SymbolToken.Name}");
                                            visual = AnglrSyntaxRuleDrawingBuilder.DrawRawTerminalSymbol (AnglrLogger, p_SymbolToken);
                                        }
                                        else
                                        {
                                            AnglrLogger?.DebugLine ($"non-terminal symbol name = {p_SymbolToken.Name}");
                                            visual = AnglrSyntaxRuleDrawingBuilder.DrawRawNonTerminalSymbol (AnglrLogger, p_SymbolToken);
                                        }
                                    }
                                    else
                                        AnglrLogger?.WarnLine ($"symbol info for {identifier.text} is null");
                                }
                                else
                                    AnglrLogger?.WarnLine ($"appInfo for {identifier.text} is null");
                            }
                            break;
                        }
                        if (visual == null)
                            break;
                        ((AppInfo) p__name_.appInfo) [AppInfoType.Visual] = visual;
                    }
                    catch (Exception e)
                    {
                        AnglrLogger?.ErrorLine (e, $"Visualization of <name> node {p__name_.Emit (-1)} failed");
                    }
                    break;
            }
            return true;
        }
    }
}
