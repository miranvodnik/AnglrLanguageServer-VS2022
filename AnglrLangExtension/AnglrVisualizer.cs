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

    public class HitList : Dictionary<int, AnglrRawDrawingVisual> { }

    public class AnglrDrawingVisual : DrawingVisual
    {
        //
        // common properties
        //

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

        //
        // object properties
        //

        public int Index { get; protected set; }
    }

    public abstract class AnglrRawDrawingVisual : DrawingVisual
    {
        //
        // common properties
        //

        public static int IdCounter;
        public static CultureInfo CultureInfo { get; set; } = CultureInfo.InvariantCulture;
        public static FlowDirection FlowDirection { get; set; } = FlowDirection.LeftToRight;
        public static string TypefaceName { get; set; } = "Consolas";
        public static int FontSize { get; set; } = 14;
        public static Brush Brush { get; set; } = Brushes.Black;
        public static Pen Pen { get; set; } = new Pen (Brush, 1);
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

        public int Id { get; private set; }
        public AnglrVisualizer AnglrVisualizer { get; set; }
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

        public AnglrRawDrawingVisual (AnglrVisualizer anglrVisualizer)
        {
            Id = ++IdCounter;
            AnglrVisualizer = anglrVisualizer;
            Position = new Vector (0, 0);
        }

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

        public void OnMouseDown (object sender, MouseButtonEventArgs e, Point point, IAnglrLogger logger) { logger?.InfoLine ($"mouse down in terminal symbol nr. {Index} at ({point})"); }
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

        public void OnMouseDown (object sender, MouseButtonEventArgs e, Point point, IAnglrLogger logger) { logger?.InfoLine ($"mouse down in constant symbol nr. {Index} at ({point})"); }
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

        public void OnMouseDown (object sender, MouseButtonEventArgs e, Point point, IAnglrLogger logger) { logger?.InfoLine ($"mouse down in non-terminal symbol nr. {Index} at ({point})"); }
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
        public AnglrRawTerminalSymbolVisual (AnglrVisualizer anglrVisualizer, SimpleSymbolToken symbolToken) : base (anglrVisualizer)
        {
            SymbolToken = symbolToken;
        }
        public AnglrRawTerminalSymbolVisual (AnglrRawTerminalSymbolVisual terminalSymbol) : base (terminalSymbol.AnglrVisualizer)
        {
            SymbolToken = terminalSymbol.SymbolToken;
        }

        public void Draw ()
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
            AnglrVisualizer?.AnglrLogger?.InfoLine ($"terminal symbol, id = {Id}, position = {Position}, size = {Size}, name = {SymbolToken.Name}");
        }

        public void OnMouseDown (object sender, MouseButtonEventArgs e, Point point, IAnglrLogger logger)
        {
            AnglrVisualizer?.AnglrLogger?.InfoLine ($"OnMouseDown (terminal symbol, id = {Id}, value =  {SymbolToken.Name})");
        }

        public void OnMouseEnter (object sender, MouseEventArgs e, Point point, IAnglrLogger logger)
        {
            AnglrVisualizer?.AnglrLogger?.InfoLine ($"OnMouseEnter (terminal symbol, id = {Id}, value =  {SymbolToken.Name})");
        }

        public void OnMouseLeave (object sender, MouseEventArgs e, Point point, IAnglrLogger logger)
        {
            AnglrVisualizer?.AnglrLogger?.InfoLine ($"OnMouseLeave (terminal symbol, id = {Id}, value =  {SymbolToken.Name})");
        }

        public void OnMouseLeftButtonDown (object sender, MouseButtonEventArgs e, Point point, IAnglrLogger logger)
        {
            AnglrVisualizer?.AnglrLogger?.InfoLine ($"OnMouseLeftButtonDown (terminal symbol, id = {Id}, value =  {SymbolToken.Name})");
        }

        public void OnMouseLeftButtonUp (object sender, MouseButtonEventArgs e, Point point, IAnglrLogger logger)
        {
            AnglrVisualizer?.AnglrLogger?.InfoLine ($"OnMouseLeftButtonUp (terminal symbol, id = {Id}, value =  {SymbolToken.Name})");
        }

        public void OnMouseMove (object sender, MouseEventArgs e, Point point, IAnglrLogger logger)
        {
            AnglrVisualizer?.AnglrLogger?.InfoLine ($"OnMouseMove (terminal symbol, id = {Id}, value =  {SymbolToken.Name})");
        }

        public void OnMouseRightButtonDown (object sender, MouseButtonEventArgs e, Point point, IAnglrLogger logger)
        {
            AnglrVisualizer?.AnglrLogger?.InfoLine ($"OnMouseRightButtonDown (terminal symbol, id = {Id}, value =  {SymbolToken.Name})");
        }

        public void OnMouseRightButtonUp (object sender, MouseButtonEventArgs e, Point point, IAnglrLogger logger)
        {
            AnglrVisualizer?.AnglrLogger?.InfoLine ($"OnMouseRightButtonUp (terminal symbol, id = {Id}, value =  {SymbolToken.Name})");
        }

        public void OnMouseUp (object sender, MouseButtonEventArgs e, Point point, IAnglrLogger logger)
        {
            AnglrVisualizer?.AnglrLogger?.InfoLine ($"OnMouseUp (terminal symbol, id = {Id}, value =  {SymbolToken.Name})");
        }
        public void OnMouseWheel (object sender, MouseWheelEventArgs e, Point point, IAnglrLogger logger) { }
    }

    public class AnglrRawConstantSymbolVisual : AnglrRawDrawingVisual, IAnglrEventHandler
    {
        public SimpleSymbolToken SymbolToken { get; private set; }
        public AnglrRawConstantSymbolVisual (AnglrVisualizer anglrVisualizer, SimpleSymbolToken symbolToken) : base (anglrVisualizer)
        {
            SymbolToken = symbolToken;
        }
        public AnglrRawConstantSymbolVisual (AnglrRawConstantSymbolVisual terminalSymbol) : base (terminalSymbol.AnglrVisualizer)
        {
            SymbolToken = terminalSymbol.SymbolToken;
        }

        public void Draw ()
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
            AnglrVisualizer?.AnglrLogger?.InfoLine ($"constant symbol, id = {Id}, position = {Position}, size = {Size}, name = {SymbolToken.Name}");
        }

        public void OnMouseDown (object sender, MouseButtonEventArgs e, Point point, IAnglrLogger logger)
        {
            AnglrVisualizer?.AnglrLogger?.InfoLine ($"OnMouseDown (constant symbol, id = {Id}, value =  {SymbolToken.Name})");
        }

        public void OnMouseEnter (object sender, MouseEventArgs e, Point point, IAnglrLogger logger)
        {
            AnglrVisualizer?.AnglrLogger?.InfoLine ($"OnMouseEnter (constant symbol, id = {Id}, value =  {SymbolToken.Name})");
        }

        public void OnMouseLeave (object sender, MouseEventArgs e, Point point, IAnglrLogger logger)
        {
            AnglrVisualizer?.AnglrLogger?.InfoLine ($"OnMouseLeave (constant symbol, id = {Id}, value =  {SymbolToken.Name})");
        }

        public void OnMouseLeftButtonDown (object sender, MouseButtonEventArgs e, Point point, IAnglrLogger logger)
        {
            AnglrVisualizer?.AnglrLogger?.InfoLine ($"OnMouseLeftButtonDown (constant symbol, id = {Id}, value =  {SymbolToken.Name})");
        }

        public void OnMouseLeftButtonUp (object sender, MouseButtonEventArgs e, Point point, IAnglrLogger logger)
        {
            AnglrVisualizer?.AnglrLogger?.InfoLine ($"OnMouseLeftButtonUp (constant symbol, id = {Id}, value =  {SymbolToken.Name})");
        }

        public void OnMouseMove (object sender, MouseEventArgs e, Point point, IAnglrLogger logger)
        {
            AnglrVisualizer?.AnglrLogger?.InfoLine ($"OnMouseMove (constant symbol, id = {Id}, value =  {SymbolToken.Name})");
        }

        public void OnMouseRightButtonDown (object sender, MouseButtonEventArgs e, Point point, IAnglrLogger logger)
        {
            AnglrVisualizer?.AnglrLogger?.InfoLine ($"OnMouseRightButtonDown (constant symbol, id = {Id}, value =  {SymbolToken.Name})");
        }

        public void OnMouseRightButtonUp (object sender, MouseButtonEventArgs e, Point point, IAnglrLogger logger)
        {
            AnglrVisualizer?.AnglrLogger?.InfoLine ($"OnMouseRightButtonUp (constant symbol, id = {Id}, value =  {SymbolToken.Name})");
        }

        public void OnMouseUp (object sender, MouseButtonEventArgs e, Point point, IAnglrLogger logger)
        {
            AnglrVisualizer?.AnglrLogger?.InfoLine ($"OnMouseUp (constant symbol, id = {Id}, value =  {SymbolToken.Name})");
        }
        public void OnMouseWheel (object sender, MouseWheelEventArgs e, Point point, IAnglrLogger logger) { }
    }

    public class AnglrRawNonTerminalSymbolVisual : AnglrRawDrawingVisual, IAnglrEventHandler
    {
        public SimpleSymbolToken SymbolToken { get; private set; }
        public AnglrRawNonTerminalSymbolVisual (AnglrVisualizer anglrVisualizer, SimpleSymbolToken symbolToken) : base (anglrVisualizer)
        {
            SymbolToken = symbolToken;
        }
        public AnglrRawNonTerminalSymbolVisual (AnglrRawNonTerminalSymbolVisual terminalSymbol) : base (terminalSymbol.AnglrVisualizer)
        {
            SymbolToken = terminalSymbol.SymbolToken;
        }

        public void Draw ()
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
            AnglrVisualizer?.AnglrLogger?.InfoLine ($"non-terminal symbol, id = {Id}, position = {Position}, size = {Size}, name = {SymbolToken.Name}");
        }

        public void OnMouseDown (object sender, MouseButtonEventArgs e, Point point, IAnglrLogger logger)
        {
            AnglrVisualizer?.AnglrLogger?.InfoLine ($"OnMouseDown (non-terminal symbol, id = {Id}, value =  {SymbolToken.Name})");
        }

        public void OnMouseEnter (object sender, MouseEventArgs e, Point point, IAnglrLogger logger)
        {
            AnglrVisualizer?.AnglrLogger?.InfoLine ($"OnMouseEnter (non-terminal symbol, id = {Id}, value =  {SymbolToken.Name})");
        }

        public void OnMouseLeave (object sender, MouseEventArgs e, Point point, IAnglrLogger logger)
        {
            AnglrVisualizer?.AnglrLogger?.InfoLine ($"OnMouseLeave (non-terminal symbol, id = {Id}, value =  {SymbolToken.Name})");
        }

        public void OnMouseLeftButtonDown (object sender, MouseButtonEventArgs e, Point point, IAnglrLogger logger)
        {
            AnglrVisualizer?.AnglrLogger?.InfoLine ($"OnMouseLeftButtonDown (non-terminal symbol, id = {Id}, value =  {SymbolToken.Name})");
        }

        public void OnMouseLeftButtonUp (object sender, MouseButtonEventArgs e, Point point, IAnglrLogger logger)
        {
            AnglrVisualizer?.AnglrLogger?.InfoLine ($"OnMouseLeftButtonUp (non-terminal symbol, id = {Id}, value =  {SymbolToken.Name})");
        }

        public void OnMouseMove (object sender, MouseEventArgs e, Point point, IAnglrLogger logger)
        {
            AnglrVisualizer?.AnglrLogger?.InfoLine ($"OnMouseMove (non-terminal symbol, id = {Id}, value =  {SymbolToken.Name})");
        }

        public void OnMouseRightButtonDown (object sender, MouseButtonEventArgs e, Point point, IAnglrLogger logger)
        {
            AnglrVisualizer?.AnglrLogger?.InfoLine ($"OnMouseRightButtonDown (non-terminal symbol, id = {Id}, value =  {SymbolToken.Name})");
        }

        public void OnMouseRightButtonUp (object sender, MouseButtonEventArgs e, Point point, IAnglrLogger logger)
        {
            AnglrVisualizer?.AnglrLogger?.InfoLine ($"OnMouseRightButtonUp (non-terminal symbol, id = {Id}, value =  {SymbolToken.Name})");
        }

        public void OnMouseUp (object sender, MouseButtonEventArgs e, Point point, IAnglrLogger logger)
        {
            AnglrVisualizer?.AnglrLogger?.InfoLine ($"OnMouseUp (non-terminal symbol, id = {Id}, value =  {SymbolToken.Name})");
        }
        public void OnMouseWheel (object sender, MouseWheelEventArgs e, Point point, IAnglrLogger logger) { }
    }

    public class AnglrSyntaxRuleNameVisual : AnglrRawDrawingVisual, IAnglrEventHandler
    {
        public SyntaxTreeToken SymbolToken { get; private set; }
        public AnglrSyntaxRuleNameVisual (AnglrVisualizer anglrVisualizer, SyntaxTreeToken symbolToken) : base (anglrVisualizer)
        {
            SymbolToken = symbolToken;
        }

        public void Draw ()
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
            AnglrVisualizer?.AnglrLogger?.InfoLine ($"syntax rule name, id = {Id}, position = {Position}, size = {Size}, name = {SymbolToken.text}");
        }

        public void OnMouseDown (object sender, MouseButtonEventArgs e, Point point, IAnglrLogger logger)
        {
            AnglrVisualizer?.AnglrLogger?.InfoLine ($"OnMouseDown (syntax rule name, id = {Id}, value =  {SymbolToken.text})");
        }

        public void OnMouseEnter (object sender, MouseEventArgs e, Point point, IAnglrLogger logger)
        {
            AnglrVisualizer?.AnglrLogger?.InfoLine ($"OnMouseEnter (syntax rule name, id = {Id}, value =  {SymbolToken.text})");
        }

        public void OnMouseLeave (object sender, MouseEventArgs e, Point point, IAnglrLogger logger)
        {
            AnglrVisualizer?.AnglrLogger?.InfoLine ($"OnMouseLeave (syntax rule name, id = {Id}, value =  {SymbolToken.text})");
        }

        public void OnMouseLeftButtonDown (object sender, MouseButtonEventArgs e, Point point, IAnglrLogger logger)
        {
            AnglrVisualizer?.AnglrLogger?.InfoLine ($"OnMouseLeftButtonDown (syntax rule name, id = {Id}, value =  {SymbolToken.text})");
        }

        public void OnMouseLeftButtonUp (object sender, MouseButtonEventArgs e, Point point, IAnglrLogger logger)
        {
            AnglrVisualizer?.AnglrLogger?.InfoLine ($"OnMouseLeftButtonUp (syntax rule name, id = {Id}, value =  {SymbolToken.text})");
        }

        public void OnMouseMove (object sender, MouseEventArgs e, Point point, IAnglrLogger logger)
        {
            AnglrVisualizer?.AnglrLogger?.InfoLine ($"OnMouseMove (syntax rule name, id = {Id}, value =  {SymbolToken.text})");
        }

        public void OnMouseRightButtonDown (object sender, MouseButtonEventArgs e, Point point, IAnglrLogger logger)
        {
            AnglrVisualizer?.AnglrLogger?.InfoLine ($"OnMouseRightButtonDown (syntax rule name, id = {Id}, value =  {SymbolToken.text})");
        }

        public void OnMouseRightButtonUp (object sender, MouseButtonEventArgs e, Point point, IAnglrLogger logger)
        {
            AnglrVisualizer?.AnglrLogger?.InfoLine ($"OnMouseRightButtonUp (syntax rule name, id = {Id}, value =  {SymbolToken.text})");
        }

        public void OnMouseUp (object sender, MouseButtonEventArgs e, Point point, IAnglrLogger logger)
        {
            AnglrVisualizer?.AnglrLogger?.InfoLine ($"OnMouseUp (syntax rule name, id = {Id}, value =  {SymbolToken.text})");
        }
        public void OnMouseWheel (object sender, MouseWheelEventArgs e, Point point, IAnglrLogger logger) { }
    }

    public class AnglrSyntaxGroupNameVisual : AnglrRawDrawingVisual, IAnglrEventHandler
    {
        public SyntaxTreeToken SymbolToken { get; private set; }
        public AnglrSyntaxGroupNameVisual (AnglrVisualizer anglrVisualizer, SyntaxTreeToken symbolToken) : base (anglrVisualizer)
        {
            SymbolToken = symbolToken;
        }

        public void Draw ()
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
            AnglrVisualizer?.AnglrLogger?.InfoLine ($"syntax group name, id = {Id}, position = {Position}, size = {Size}, name = {SymbolToken.text}");
        }

        public void OnMouseDown (object sender, MouseButtonEventArgs e, Point point, IAnglrLogger logger)
        {
            AnglrVisualizer?.AnglrLogger?.InfoLine ($"OnMouseDown (syntax group name, id = {Id}, value =  {SymbolToken.text})");
        }

        public void OnMouseEnter (object sender, MouseEventArgs e, Point point, IAnglrLogger logger)
        {
            AnglrVisualizer?.AnglrLogger?.InfoLine ($"OnMouseEnter (syntax group name, id = {Id}, value =  {SymbolToken.text})");
        }

        public void OnMouseLeave (object sender, MouseEventArgs e, Point point, IAnglrLogger logger)
        {
            AnglrVisualizer?.AnglrLogger?.InfoLine ($"OnMouseLeave (syntax group name, id = {Id}, value =  {SymbolToken.text})");
        }

        public void OnMouseLeftButtonDown (object sender, MouseButtonEventArgs e, Point point, IAnglrLogger logger)
        {
            AnglrVisualizer?.AnglrLogger?.InfoLine ($"OnMouseLeftButtonDown (syntax group name, id = {Id}, value =  {SymbolToken.text})");
        }

        public void OnMouseLeftButtonUp (object sender, MouseButtonEventArgs e, Point point, IAnglrLogger logger)
        {
            AnglrVisualizer?.AnglrLogger?.InfoLine ($"OnMouseLeftButtonUp (syntax group name, id = {Id}, value =  {SymbolToken.text})");
        }

        public void OnMouseMove (object sender, MouseEventArgs e, Point point, IAnglrLogger logger)
        {
            AnglrVisualizer?.AnglrLogger?.InfoLine ($"OnMouseMove (syntax group name, id = {Id}, value =  {SymbolToken.text})");
        }

        public void OnMouseRightButtonDown (object sender, MouseButtonEventArgs e, Point point, IAnglrLogger logger)
        {
            AnglrVisualizer?.AnglrLogger?.InfoLine ($"OnMouseRightButtonDown (syntax group name, id = {Id}, value =  {SymbolToken.text})");
        }

        public void OnMouseRightButtonUp (object sender, MouseButtonEventArgs e, Point point, IAnglrLogger logger)
        {
            AnglrVisualizer?.AnglrLogger?.InfoLine ($"OnMouseRightButtonUp (syntax group name, id = {Id}, value =  {SymbolToken.text})");
        }

        public void OnMouseUp (object sender, MouseButtonEventArgs e, Point point, IAnglrLogger logger)
        {
            AnglrVisualizer?.AnglrLogger?.InfoLine ($"OnMouseUp (syntax group name, id = {Id}, value =  {SymbolToken.text})");
        }
        public void OnMouseWheel (object sender, MouseWheelEventArgs e, Point point, IAnglrLogger logger) { }
    }

    public class AnglrGeneralizedNameVisual : AnglrRawDrawingVisual, IAnglrEventHandler
    {
        public AnglrRawDrawingVisual GnameVisual { get; private set; }
        public _cardinality_ Cardinality { get; private set; }
        public AnglrRawDrawingVisual DelimiterVisual { get; private set; }
        public _cardinality_delimiter_ CardinalityDelimiter { get; private set; }

        public AnglrGeneralizedNameVisual (AnglrVisualizer anglrVisualizer, _g_name_ name) : base (anglrVisualizer)
        {
            if (((AppInfo) name.m__g_name_.appInfo).TryGetValue (AppInfoType.Visual, out var gnameVisual))
                GnameVisual = gnameVisual as AnglrRawDrawingVisual;
            CardinalityDelimiter = name.m__cardinality_delimiter_;
            Cardinality = CardinalityDelimiter.m__cardinality_;
            _delimiter_ delimiter = CardinalityDelimiter.m__delimiter_optional_.m__delimiter_;
            if ((delimiter != null) && ((AppInfo) delimiter.m__anglr_nested_rule_.appInfo).TryGetValue (AppInfoType.Visual, out var visualObject))
                DelimiterVisual = visualObject as AnglrRawDrawingVisual;
        }

        public void Draw ()
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
            AnglrVisualizer?.AnglrLogger?.InfoLine ($"g-name, id = {Id}, position = {Position}, size = {Size}, value = {CardinalityDelimiter.parent.Emit (-1).Substring(0, 100)}");
        }

        public void OnMouseDown (object sender, MouseButtonEventArgs e, Point point, IAnglrLogger logger)
        {
            AnglrVisualizer?.AnglrLogger?.InfoLine ($"OnMouseDown (g-name, id = {Id}, value =  {CardinalityDelimiter.parent.Emit (-1)})");
        }

        public void OnMouseEnter (object sender, MouseEventArgs e, Point point, IAnglrLogger logger)
        {
            AnglrVisualizer?.AnglrLogger?.InfoLine ($"OnMouseEnter (g-name, id = {Id}, value =  {CardinalityDelimiter.parent.Emit (-1)})");
        }

        public void OnMouseLeave (object sender, MouseEventArgs e, Point point, IAnglrLogger logger)
        {
            AnglrVisualizer?.AnglrLogger?.InfoLine ($"OnMouseLeave (g-name, id = {Id}, value =  {CardinalityDelimiter.parent.Emit (-1)})");
        }

        public void OnMouseLeftButtonDown (object sender, MouseButtonEventArgs e, Point point, IAnglrLogger logger)
        {
            AnglrVisualizer?.AnglrLogger?.InfoLine ($"OnMouseLeftButtonDown (g-name, id = {Id}, value =  {CardinalityDelimiter.parent.Emit (-1)})");
        }

        public void OnMouseLeftButtonUp (object sender, MouseButtonEventArgs e, Point point, IAnglrLogger logger)
        {
            AnglrVisualizer?.AnglrLogger?.InfoLine ($"OnMouseLeftButtonUp (g-name, id = {Id}, value =  {CardinalityDelimiter.parent.Emit (-1)})");
        }

        public void OnMouseMove (object sender, MouseEventArgs e, Point point, IAnglrLogger logger)
        {
            AnglrVisualizer?.AnglrLogger?.InfoLine ($"OnMouseMove (g-name, id = {Id}, value =  {CardinalityDelimiter.parent.Emit (-1)})");
        }

        public void OnMouseRightButtonDown (object sender, MouseButtonEventArgs e, Point point, IAnglrLogger logger)
        {
            AnglrVisualizer?.AnglrLogger?.InfoLine ($"OnMouseRightButtonDown (g-name, id = {Id}, value =  {CardinalityDelimiter.parent.Emit (-1)})");
        }

        public void OnMouseRightButtonUp (object sender, MouseButtonEventArgs e, Point point, IAnglrLogger logger)
        {
            AnglrVisualizer?.AnglrLogger?.InfoLine ($"OnMouseRightButtonUp (g-name, id = {Id}, value =  {CardinalityDelimiter.parent.Emit (-1)})");
        }

        public void OnMouseUp (object sender, MouseButtonEventArgs e, Point point, IAnglrLogger logger)
        {
            AnglrVisualizer?.AnglrLogger?.InfoLine ($"OnMouseUp (g-name, id = {Id}, value =  {CardinalityDelimiter.parent.Emit (-1)})");
        }
        public void OnMouseWheel (object sender, MouseWheelEventArgs e, Point point, IAnglrLogger logger) { }
    }

    public class AnglrNameListVisual : AnglrRawDrawingVisual, IAnglrEventHandler
    {
        public _name_list_ NameList { get; private set; }
        public AnglrNameListVisual (AnglrVisualizer anglrVisualizer, _name_list_ nameList) : base (anglrVisualizer)
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

        public void Draw ()
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
            AnglrVisualizer?.AnglrLogger?.InfoLine ($"name list, id = {Id}, position = {Position}, size = {Size}, value = {NameList.Emit (-1)}");
        }

        public void OnMouseDown (object sender, MouseButtonEventArgs e, Point point, IAnglrLogger logger)
        {
            AnglrVisualizer?.AnglrLogger?.InfoLine ($"OnMouseDown (name list, id = {Id}, value =  {NameList.Emit (-1)})");
        }

        public void OnMouseEnter (object sender, MouseEventArgs e, Point point, IAnglrLogger logger)
        {
            AnglrVisualizer?.AnglrLogger?.InfoLine ($"OnMouseEnter (name list, id = {Id}, value =  {NameList.Emit (-1)})");
        }

        public void OnMouseLeave (object sender, MouseEventArgs e, Point point, IAnglrLogger logger)
        {
            AnglrVisualizer?.AnglrLogger?.InfoLine ($"OnMouseLeave (name list, id = {Id}, value =  {NameList.Emit (-1)})");
        }

        public void OnMouseLeftButtonDown (object sender, MouseButtonEventArgs e, Point point, IAnglrLogger logger)
        {
            AnglrVisualizer?.AnglrLogger?.InfoLine ($"OnMouseLeftButtonDown (name list, id = {Id}, value =  {NameList.Emit (-1)})");
        }

        public void OnMouseLeftButtonUp (object sender, MouseButtonEventArgs e, Point point, IAnglrLogger logger)
        {
            AnglrVisualizer?.AnglrLogger?.InfoLine ($"OnMouseLeftButtonUp (name list, id = {Id}, value =  {NameList.Emit (-1)})");
        }

        public void OnMouseMove (object sender, MouseEventArgs e, Point point, IAnglrLogger logger)
        {
            AnglrVisualizer?.AnglrLogger?.InfoLine ($"OnMouseMove (name list, id = {Id}, value =  {NameList.Emit (-1)})");
        }

        public void OnMouseRightButtonDown (object sender, MouseButtonEventArgs e, Point point, IAnglrLogger logger)
        {
            AnglrVisualizer?.AnglrLogger?.InfoLine ($"OnMouseRightButtonDown (name list, id = {Id}, value =  {NameList.Emit (-1)})");
        }

        public void OnMouseRightButtonUp (object sender, MouseButtonEventArgs e, Point point, IAnglrLogger logger)
        {
            AnglrVisualizer?.AnglrLogger?.InfoLine ($"OnMouseRightButtonUp (name list, id = {Id}, value =  {NameList.Emit (-1)})");
        }

        public void OnMouseUp (object sender, MouseButtonEventArgs e, Point point, IAnglrLogger logger)
        {
            AnglrVisualizer?.AnglrLogger?.InfoLine ($"OnMouseUp (name list, id = {Id}, value =  {NameList.Emit (-1)})");
        }
        public void OnMouseWheel (object sender, MouseWheelEventArgs e, Point point, IAnglrLogger logger) { }
    }

    public class AnglrNestedRuleVisual : AnglrRawDrawingVisual, IAnglrEventHandler
    {
        public _anglr_nested_rule_ NestedRule { get; private set; }
        public AnglrRawDrawingVisual SyntaxRuleNameVisual { get; private set; }
        public AnglrNestedRuleVisual (AnglrVisualizer anglrVisualizer, _anglr_nested_rule_ nestedRule) : base (anglrVisualizer)
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
        public void Draw ()
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
            AnglrVisualizer?.AnglrLogger?.InfoLine ($"nested syntax rule, id = {Id}, position = {Position}, size = {Size}, value = {NestedRule.Emit (-1)}");
        }

        public void OnMouseDown (object sender, MouseButtonEventArgs e, Point point, IAnglrLogger logger)
        {
            AnglrVisualizer?.AnglrLogger?.InfoLine ($"OnMouseDown (nested syntax rule, id = {Id}, value =  {NestedRule.Emit (-1)})");
        }

        public void OnMouseEnter (object sender, MouseEventArgs e, Point point, IAnglrLogger logger)
        {
            AnglrVisualizer?.AnglrLogger?.InfoLine ($"OnMouseEnter (nested syntax rule, id = {Id}, value =  {NestedRule.Emit (-1)})");
        }

        public void OnMouseLeave (object sender, MouseEventArgs e, Point point, IAnglrLogger logger)
        {
            AnglrVisualizer?.AnglrLogger?.InfoLine ($"OnMouseLeave (nested syntax rule, id = {Id}, value =  {NestedRule.Emit (-1)})");
        }

        public void OnMouseLeftButtonDown (object sender, MouseButtonEventArgs e, Point point, IAnglrLogger logger)
        {
            AnglrVisualizer?.AnglrLogger?.InfoLine ($"OnMouseLeftButtonDown (nested syntax rule, id = {Id}, value =  {NestedRule.Emit (-1)})");
        }

        public void OnMouseLeftButtonUp (object sender, MouseButtonEventArgs e, Point point, IAnglrLogger logger)
        {
            AnglrVisualizer?.AnglrLogger?.InfoLine ($"OnMouseLeftButtonUp (nested syntax rule, id = {Id}, value =  {NestedRule.Emit (-1)})");
        }

        public void OnMouseMove (object sender, MouseEventArgs e, Point point, IAnglrLogger logger)
        {
            AnglrVisualizer?.AnglrLogger?.InfoLine ($"OnMouseMove (nested syntax rule, id = {Id}, value =  {NestedRule.Emit (-1)})");
        }

        public void OnMouseRightButtonDown (object sender, MouseButtonEventArgs e, Point point, IAnglrLogger logger)
        {
            AnglrVisualizer?.AnglrLogger?.InfoLine ($"OnMouseRightButtonDown (nested syntax rule, id = {Id}, value =  {NestedRule.Emit (-1)})");
        }

        public void OnMouseRightButtonUp (object sender, MouseButtonEventArgs e, Point point, IAnglrLogger logger)
        {
            AnglrVisualizer?.AnglrLogger?.InfoLine ($"OnMouseRightButtonUp (nested syntax rule, id = {Id}, value =  {NestedRule.Emit (-1)})");
        }

        public void OnMouseUp (object sender, MouseButtonEventArgs e, Point point, IAnglrLogger logger)
        {
            AnglrVisualizer?.AnglrLogger?.InfoLine ($"OnMouseUp (nested syntax rule, id = {Id}, value =  {NestedRule.Emit (-1)})");
        }
        public void OnMouseWheel (object sender, MouseWheelEventArgs e, Point point, IAnglrLogger logger) { }
    }

    public class AnglrSyntaxRuleVisual : AnglrRawDrawingVisual, IAnglrEventHandler
    {
        public _anglr_syntax_rule_ SyntaxRule { get; private set; }
        public AnglrRawDrawingVisual SyntaxRuleNameVisual { get; private set; }
        public AnglrSyntaxRuleVisual (AnglrVisualizer anglrVisualizer, _anglr_syntax_rule_ syntaxRule) : base (anglrVisualizer)
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

        public void Draw ()
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
            AnglrVisualizer?.AnglrLogger?.InfoLine ($"syntax rule, id = {Id}, position = {Position}, size = {Size}, value =  {SyntaxRule.Emit (-1)}");
        }

        public void OnMouseDown (object sender, MouseButtonEventArgs e, Point point, IAnglrLogger logger)
        {
            AnglrVisualizer?.AnglrLogger?.InfoLine ($"OnMouseDown (syntax rule, id = {Id}, value =  {SyntaxRule.Emit (-1)})");
        }

        public void OnMouseEnter (object sender, MouseEventArgs e, Point point, IAnglrLogger logger)
        {
            AnglrVisualizer?.AnglrLogger?.InfoLine ($"OnMouseEnter (syntax rule, id = {Id}, value =  {SyntaxRule.Emit (-1)})");
        }

        public void OnMouseLeave (object sender, MouseEventArgs e, Point point, IAnglrLogger logger)
        {
            AnglrVisualizer?.AnglrLogger?.InfoLine ($"OnMouseLeave (syntax rule, id = {Id}, value =  {SyntaxRule.Emit (-1)})");
        }

        public void OnMouseLeftButtonDown (object sender, MouseButtonEventArgs e, Point point, IAnglrLogger logger)
        {
            AnglrVisualizer?.AnglrLogger?.InfoLine ($"OnMouseLeftButtonDown (syntax rule, id = {Id}, value =  {SyntaxRule.Emit (-1)})");
        }

        public void OnMouseLeftButtonUp (object sender, MouseButtonEventArgs e, Point point, IAnglrLogger logger)
        {
            AnglrVisualizer?.AnglrLogger?.InfoLine ($"OnMouseLeftButtonUp (syntax rule, id = {Id}, value =  {SyntaxRule.Emit (-1)})");
        }

        public void OnMouseMove (object sender, MouseEventArgs e, Point point, IAnglrLogger logger)
        {
            AnglrVisualizer?.AnglrLogger?.InfoLine ($"OnMouseMove (syntax rule, id = {Id}, value =  {SyntaxRule.Emit (-1)})");
        }

        public void OnMouseRightButtonDown (object sender, MouseButtonEventArgs e, Point point, IAnglrLogger logger)
        {
            AnglrVisualizer?.AnglrLogger?.InfoLine ($"OnMouseRightButtonDown (syntax rule, id = {Id}, value =  {SyntaxRule.Emit (-1)})");
        }

        public void OnMouseRightButtonUp (object sender, MouseButtonEventArgs e, Point point, IAnglrLogger logger)
        {
            AnglrVisualizer?.AnglrLogger?.InfoLine ($"OnMouseRightButtonUp (syntax rule, id = {Id}, value =  {SyntaxRule.Emit (-1)})");
        }

        public void OnMouseUp (object sender, MouseButtonEventArgs e, Point point, IAnglrLogger logger)
        {
            AnglrVisualizer?.AnglrLogger?.InfoLine ($"OnMouseUp (syntax rule, id = {Id}, value =  {SyntaxRule.Emit (-1)})");
        }
        public void OnMouseWheel (object sender, MouseWheelEventArgs e, Point point, IAnglrLogger logger) { }
    }

    public class AnglrSyntaxGroupVisual : AnglrRawDrawingVisual, IAnglrEventHandler
    {
        public _anglr_syntax_rule_ SyntaxGroup { get; private set; }
        public AnglrRawDrawingVisual SyntaxGroupNameVisual { get; private set; }
        public AnglrSyntaxGroupVisual (AnglrVisualizer anglrVisualizer, _anglr_syntax_rule_ syntaxGroup) : base (anglrVisualizer)
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
        public void Draw ()
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
            AnglrVisualizer?.AnglrLogger?.InfoLine ($"syntax group, id = {Id}, position = {Position}, size = {Size}, value = {SyntaxGroup.Emit (-1)}");
        }

        public void OnMouseDown (object sender, MouseButtonEventArgs e, Point point, IAnglrLogger logger)
        {
            AnglrVisualizer?.AnglrLogger?.InfoLine ($"OnMouseDown (syntax group, id = {Id}, value =  {SyntaxGroup.Emit (-1)})");
        }

        public void OnMouseEnter (object sender, MouseEventArgs e, Point point, IAnglrLogger logger)
        {
            AnglrVisualizer?.AnglrLogger?.InfoLine ($"OnMouseEnter (syntax group, id = {Id}, value =  {SyntaxGroup.Emit (-1)})");
        }

        public void OnMouseLeave (object sender, MouseEventArgs e, Point point, IAnglrLogger logger)
        {
            AnglrVisualizer?.AnglrLogger?.InfoLine ($"OnMouseLeave (syntax group, id = {Id}, value =  {SyntaxGroup.Emit (-1)})");
        }

        public void OnMouseLeftButtonDown (object sender, MouseButtonEventArgs e, Point point, IAnglrLogger logger)
        {
            AnglrVisualizer?.AnglrLogger?.InfoLine ($"OnMouseLeftButtonDown (syntax group, id = {Id}, value =  {SyntaxGroup.Emit (-1)})");
        }

        public void OnMouseLeftButtonUp (object sender, MouseButtonEventArgs e, Point point, IAnglrLogger logger)
        {
            AnglrVisualizer?.AnglrLogger?.InfoLine ($"OnMouseLeftButtonUp (syntax group, id = {Id}, value =  {SyntaxGroup.Emit (-1)})");
        }

        public void OnMouseMove (object sender, MouseEventArgs e, Point point, IAnglrLogger logger)
        {
            AnglrVisualizer?.AnglrLogger?.InfoLine ($"OnMouseMove (syntax group, id = {Id}, value =  {SyntaxGroup.Emit (-1)})");
        }

        public void OnMouseRightButtonDown (object sender, MouseButtonEventArgs e, Point point, IAnglrLogger logger)
        {
            AnglrVisualizer?.AnglrLogger?.InfoLine ($"OnMouseRightButtonDown (syntax group, id = {Id}, value =  {SyntaxGroup.Emit (-1)})");
        }

        public void OnMouseRightButtonUp (object sender, MouseButtonEventArgs e, Point point, IAnglrLogger logger)
        {
            AnglrVisualizer?.AnglrLogger?.InfoLine ($"OnMouseRightButtonUp (syntax group, id = {Id}, value =  {SyntaxGroup.Emit (-1)})");
        }

        public void OnMouseUp (object sender, MouseButtonEventArgs e, Point point, IAnglrLogger logger)
        {
            AnglrVisualizer?.AnglrLogger?.InfoLine ($"OnMouseUp (syntax group, id = {Id}, value =  {SyntaxGroup.Emit (-1)})");
        }
        public void OnMouseWheel (object sender, MouseWheelEventArgs e, Point point, IAnglrLogger logger) { }
    }

    public class AnglrParserPartVisual : AnglrRawDrawingVisual, IAnglrEventHandler
    {
        public _parser_part_ ParserPart { get; private set; }
        public AnglrParserPartVisual (AnglrVisualizer anglrVisualizer, _parser_part_ parserPart) : base (anglrVisualizer)
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

        public void Draw ()
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
            AnglrVisualizer?.AnglrLogger?.InfoLine ($"parser part, id = {Id}, position = {Position}, size = {Size}, value =  {ParserPart.Emit (-1)}");
        }

        public void OnMouseDown (object sender, MouseButtonEventArgs e, Point point, IAnglrLogger logger)
        {
            AnglrVisualizer?.AnglrLogger?.InfoLine ($"OnMouseDown (parser part, id = {Id}, value =  {ParserPart.Emit (-1)})");
        }

        public void OnMouseEnter (object sender, MouseEventArgs e, Point point, IAnglrLogger logger)
        {
            AnglrVisualizer?.AnglrLogger?.InfoLine ($"OnMouseEnter (parser part, id = {Id}, value =  {ParserPart.Emit (-1)})");
        }

        public void OnMouseLeave (object sender, MouseEventArgs e, Point point, IAnglrLogger logger)
        {
            AnglrVisualizer?.AnglrLogger?.InfoLine ($"OnMouseLeave (parser part, id = {Id}, value =  {ParserPart.Emit (-1)})");
        }

        public void OnMouseLeftButtonDown (object sender, MouseButtonEventArgs e, Point point, IAnglrLogger logger)
        {
            AnglrVisualizer?.AnglrLogger?.InfoLine ($"OnMouseLeftButtonDown (parser part, id = {Id}, value =  {ParserPart.Emit (-1)})");
        }

        public void OnMouseLeftButtonUp (object sender, MouseButtonEventArgs e, Point point, IAnglrLogger logger)
        {
            AnglrVisualizer?.AnglrLogger?.InfoLine ($"OnMouseLeftButtonUp (parser part, id = {Id}, value =  {ParserPart.Emit (-1)})");
        }

        public void OnMouseMove (object sender, MouseEventArgs e, Point point, IAnglrLogger logger)
        {
            AnglrVisualizer?.AnglrLogger?.InfoLine ($"OnMouseMove (parser part, id = {Id}, value =  {ParserPart.Emit (-1)})");
        }

        public void OnMouseRightButtonDown (object sender, MouseButtonEventArgs e, Point point, IAnglrLogger logger)
        {
            AnglrVisualizer?.AnglrLogger?.InfoLine ($"OnMouseRightButtonDown (parser part, id = {Id}, value =  {ParserPart.Emit (-1)})");
        }

        public void OnMouseRightButtonUp (object sender, MouseButtonEventArgs e, Point point, IAnglrLogger logger)
        {
            AnglrVisualizer?.AnglrLogger?.InfoLine ($"OnMouseRightButtonUp (parser part, id = {Id}, value =  {ParserPart.Emit (-1)})");
        }

        public void OnMouseUp (object sender, MouseButtonEventArgs e, Point point, IAnglrLogger logger)
        {
            AnglrVisualizer?.AnglrLogger?.InfoLine ($"OnMouseUp (parser part, id = {Id}, value =  {ParserPart.Emit (-1)})");
        }
        public void OnMouseWheel (object sender, MouseWheelEventArgs e, Point point, IAnglrLogger logger) { }
    }

    public class AnglrFilePartListVisual : AnglrRawDrawingVisual, IAnglrEventHandler
    {
        public _anglr_file_part_list_ FilePartList { get; private set; }
        public AnglrFilePartListVisual (AnglrVisualizer anglrVisualizer, _anglr_file_part_list_ filePartList) : base (anglrVisualizer)
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

        public void Draw ()
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
        }

        public override void Display ()
        {
            AnglrVisualizer?.AnglrLogger?.InfoLine ($"file part list, id = {Id}, position = {Position}, size = {Size}, value =  {FilePartList.Emit (-1)}");
        }

        public void OnMouseDown (object sender, MouseButtonEventArgs e, Point point, IAnglrLogger logger)
        {
            AnglrVisualizer?.AnglrLogger?.InfoLine ($"OnMouseDown (file part list, id = {Id}, value =  {FilePartList.Emit (-1)})");
        }

        public void OnMouseEnter (object sender, MouseEventArgs e, Point point, IAnglrLogger logger)
        {
            AnglrVisualizer?.AnglrLogger?.InfoLine ($"OnMouseEner (file part list, id = {Id}, value =  {FilePartList.Emit (-1)})");
        }

        public void OnMouseLeave (object sender, MouseEventArgs e, Point point, IAnglrLogger logger)
        {
            AnglrVisualizer?.AnglrLogger?.InfoLine ($"OnMouseLeave (file part list, id = {Id}, value =  {FilePartList.Emit (-1)})");
        }

        public void OnMouseLeftButtonDown (object sender, MouseButtonEventArgs e, Point point, IAnglrLogger logger)
        {
            AnglrVisualizer?.AnglrLogger?.InfoLine ($"OnMouseLeftButtonDown (file part list, id = {Id}, value =  {FilePartList.Emit (-1)})");
        }

        public void OnMouseLeftButtonUp (object sender, MouseButtonEventArgs e, Point point, IAnglrLogger logger)
        {
            AnglrVisualizer?.AnglrLogger?.InfoLine ($"OnMouseLeftButtonUp (file part list, id = {Id}, value =  {FilePartList.Emit (-1)})");
        }

        public void OnMouseMove (object sender, MouseEventArgs e, Point point, IAnglrLogger logger)
        {
            AnglrVisualizer?.AnglrLogger?.InfoLine ($"OnMouseMove (file part list, id = {Id}, value =  {FilePartList.Emit (-1)})");
        }

        public void OnMouseRightButtonDown (object sender, MouseButtonEventArgs e, Point point, IAnglrLogger logger)
        {
            AnglrVisualizer?.AnglrLogger?.InfoLine ($"OnMouseRightButtonDown (file part list, id = {Id}, value =  {FilePartList.Emit (-1)})");
        }

        public void OnMouseRightButtonUp (object sender, MouseButtonEventArgs e, Point point, IAnglrLogger logger)
        {
            AnglrVisualizer?.AnglrLogger?.InfoLine ($"OnMouseRightButtonUp (file part list, id = {Id}, value =  {FilePartList.Emit (-1)})");
        }

        public void OnMouseUp (object sender, MouseButtonEventArgs e, Point point, IAnglrLogger logger)
        {
            AnglrVisualizer?.AnglrLogger?.InfoLine ($"OnMouseUp (file part list, id = {Id}, value =  {FilePartList.Emit (-1)})");
        }
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

        public void OnMouseDown (object sender, MouseButtonEventArgs e, Point point, IAnglrLogger logger) { logger?.InfoLine ($"mouse down in container nr. {Index} at ({point})"); }
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

    public class AnglrDrawingDictionary : Dictionary<int, AnglrContainerSymbolVisual> { }

    public static class AnglrSyntaxRuleDrawingBuilder
    {
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

        public static AnglrRawDrawingVisual DrawRawTerminalSymbol (AnglrVisualizer anglrVisualizer, SimpleSymbolToken symbolToken)
        {
            AnglrRawTerminalSymbolVisual visual = new AnglrRawTerminalSymbolVisual (anglrVisualizer, symbolToken);
            visual.Draw ();
            return visual;
        }

        public static AnglrRawDrawingVisual DrawRawConstantSymbol (AnglrVisualizer anglrVisualizer, SimpleSymbolToken symbolToken)
        {
            AnglrRawConstantSymbolVisual visual = new AnglrRawConstantSymbolVisual (anglrVisualizer, symbolToken);
            visual.Draw ();
            return visual;
        }

        public static AnglrRawDrawingVisual DrawRawNonTerminalSymbol (AnglrVisualizer anglrVisualizer, SimpleSymbolToken symbolToken)
        {
            AnglrRawNonTerminalSymbolVisual visual = new AnglrRawNonTerminalSymbolVisual (anglrVisualizer, symbolToken);
            visual.Draw ();
            return visual;
        }

        public static AnglrRawDrawingVisual DrawGeneralizedSymbol (AnglrVisualizer anglrVisualizer, _g_name_ name)
        {
            AnglrGeneralizedNameVisual visual = new AnglrGeneralizedNameVisual (anglrVisualizer, name);
            visual.Draw ();
            return visual;
        }

        public static AnglrRawDrawingVisual DrawNameList (AnglrVisualizer anglrVisualizer, _name_list_ nameList)
        {
            AnglrNameListVisual visual = new AnglrNameListVisual (anglrVisualizer, nameList);
            visual.Draw ();
            return visual;
        }

        public static AnglrRawDrawingVisual DrawNestedRule (AnglrVisualizer anglrVisualizer, _anglr_nested_rule_ nestedRule)
        {
            AnglrNestedRuleVisual visual = new AnglrNestedRuleVisual (anglrVisualizer, nestedRule);
            visual.Draw ();
            return visual;
        }

        public static AnglrRawDrawingVisual DrawSyntaxRuleName (AnglrVisualizer anglrVisualizer, SyntaxTreeToken ruleName)
        {
            AnglrSyntaxRuleNameVisual visual = new AnglrSyntaxRuleNameVisual (anglrVisualizer, ruleName);
            visual.Draw ();
            return visual;
        }

        public static AnglrRawDrawingVisual DrawSyntaxRule (AnglrVisualizer anglrVisualizer, _anglr_syntax_rule_ syntaxRule)
        {
            AnglrSyntaxRuleVisual visual = new AnglrSyntaxRuleVisual (anglrVisualizer, syntaxRule);
            visual.Draw ();
            return visual;
        }

        public static AnglrRawDrawingVisual DrawSyntaxGroupName (AnglrVisualizer anglrVisualizer, SyntaxTreeToken groupName)
        {
            AnglrSyntaxGroupNameVisual visual = new AnglrSyntaxGroupNameVisual (anglrVisualizer, groupName);
            visual.Draw ();
            return visual;
        }

        public static AnglrRawDrawingVisual DrawSyntaxGroup (AnglrVisualizer anglrVisualizer, _anglr_syntax_rule_ syntaxRule)
        {
            AnglrSyntaxGroupVisual visual = new AnglrSyntaxGroupVisual (anglrVisualizer, syntaxRule);
            visual.Draw ();
            return visual;
        }

        public static AnglrRawDrawingVisual DrawParserPart (AnglrVisualizer anglrVisualizer, _parser_part_ parserPart)
        {
            AnglrParserPartVisual visual = new AnglrParserPartVisual (anglrVisualizer, parserPart);
            visual.Draw ();
            return visual;
        }

        public static AnglrRawDrawingVisual DrawAnglrFilePartList (AnglrVisualizer anglrVisualizer, _anglr_file_part_list_ filePartList)
        {
            AnglrFilePartListVisual visual = new AnglrFilePartListVisual (anglrVisualizer, filePartList);
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
                    AnglrVisualizer anglrVisualizer = new AnglrVisualizer (logger);
                    anglrVisualizer.Traverse (anglrFileFragment);
                    _anglr_file_ anglrFile = anglrFileFragment.m__anglr_file_;
                    if ((anglrFile == null) || (anglrFile.appInfo == null) || !((AppInfo) anglrFile.appInfo).TryGetValue (AppInfoType.Visual, out var visual))
                        return null;
                    anglrVisualizer.ComputeVisualBounds (visual as AnglrRawDrawingVisual);
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

    public class AnglrVisualizer : SyntaxTreeWalker
    {
        _anglr_file_fragment_ Fragment { get; set; }
        public IAnglrLogger AnglrLogger { get; private set; }
        public AnglrVisualizer (IAnglrLogger logger)
        {
            AnglrLogger = logger;
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

        public void ComputeVisualBounds (AnglrRawDrawingVisual drawingVisual)
        {
            if (drawingVisual == null)
                return;
            _ComputeVisualBounds (new AnglrVisualPosition (), drawingVisual);
        }

        public void _ComputeVisualBounds (AnglrVisualPosition visualPosition, AnglrRawDrawingVisual drawingVisual)
        {
            drawingVisual.Position = visualPosition.PushPosition (drawingVisual.Position);
            foreach (var child in drawingVisual.AnglrVisualChildren)
            {
                _ComputeVisualBounds (visualPosition, child);
            }
            visualPosition.PopPosition ();
        }

        public HitList HitTest (object sender, MouseEventArgs e, AnglrRawDrawingVisual drawingVisual, Point point, AnglrMouseEventKind mouseEventKind)
        {
            if (drawingVisual == null)
                return null;
            try
            {
                HitList hitVisuals = new HitList ();
                _HitTest (hitVisuals, drawingVisual, point);
                foreach (var visual in hitVisuals.Values)
                {
                    switch (mouseEventKind)
                    {
                        case AnglrMouseEventKind.None:
                            break;
                        case AnglrMouseEventKind.MouseDown:
                            (visual as IAnglrEventHandler)?.OnMouseDown (sender, e as MouseButtonEventArgs, point, AnglrLogger);
                            break;
                        case AnglrMouseEventKind.MouseEnter:
                            (visual as IAnglrEventHandler)?.OnMouseEnter (sender, e, point, AnglrLogger);
                            break;
                        case AnglrMouseEventKind.MouseLeave:
                            (visual as IAnglrEventHandler)?.OnMouseLeave (sender, e, point, AnglrLogger);
                            break;
                        case AnglrMouseEventKind.MouseLeftButttonDown:
                            (visual as IAnglrEventHandler)?.OnMouseLeftButtonDown (sender, e as MouseButtonEventArgs, point, AnglrLogger);
                            break;
                        case AnglrMouseEventKind.MouseLeftButttonUp:
                            (visual as IAnglrEventHandler)?.OnMouseLeftButtonUp (sender, e as MouseButtonEventArgs, point, AnglrLogger);
                            break;
                        case AnglrMouseEventKind.MouseMove:
                            (visual as IAnglrEventHandler)?.OnMouseMove (sender, e, point, AnglrLogger);
                            break;
                        case AnglrMouseEventKind.MouseRightButttonDown:
                            (visual as IAnglrEventHandler)?.OnMouseRightButtonDown (sender, e as MouseButtonEventArgs, point, AnglrLogger);
                            break;
                        case AnglrMouseEventKind.MouseRightButttonUp:
                            (visual as IAnglrEventHandler)?.OnMouseRightButtonUp (sender, e as MouseButtonEventArgs, point, AnglrLogger);
                            break;
                        case AnglrMouseEventKind.MouseUp:
                            (visual as IAnglrEventHandler)?.OnMouseUp (sender, e as MouseButtonEventArgs, point, AnglrLogger);
                            break;
                        case AnglrMouseEventKind.MouseWheel:
                            (visual as IAnglrEventHandler)?.OnMouseWheel (sender, e as MouseWheelEventArgs, point, AnglrLogger);
                            break;
                    }
                }
                return hitVisuals;
            }
            catch (Exception ex)
            {
                AnglrLogger?.ErrorLine (ex, "Hit test failed");
                return null;
            }
        }

        public bool _HitTest (HitList hitVisuals, AnglrRawDrawingVisual drawingVisual, Point point)
        {
            if (!drawingVisual.Bounds.Contains (point))
                return false;
            if (!hitVisuals.TryGetValue (drawingVisual.Id, out _))
                hitVisuals [drawingVisual.Id] = drawingVisual;
            foreach (var child in drawingVisual.AnglrVisualChildren)
            {
                if (_HitTest (hitVisuals, child, point))
                    break;
            }
            return true;
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
                            AnglrSyntaxRuleDrawingBuilder.DrawAnglrFilePartList (this, filePartList);
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
                            AnglrSyntaxRuleDrawingBuilder.DrawParserPart (this, p__parser_part_);
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
                                    AnglrSyntaxRuleDrawingBuilder.DrawSyntaxRuleName (this, p__anglr_syntax_rule_.m__identifier_);
                                ((AppInfo) p__anglr_syntax_rule_.appInfo) [AppInfoType.Visual] =
                                    AnglrSyntaxRuleDrawingBuilder.DrawSyntaxRule (this, p__anglr_syntax_rule_);
                            }
                            break;
                            case _anglr_syntax_rule_.production_kind.g__anglr_syntax_rule__2:
                            {
                                ((AppInfo) p__anglr_syntax_rule_.m__identifier_.appInfo) [AppInfoType.Visual] =
                                    AnglrSyntaxRuleDrawingBuilder.DrawSyntaxGroupName (this, p__anglr_syntax_rule_.m__identifier_);
                                ((AppInfo) p__anglr_syntax_rule_.appInfo) [AppInfoType.Visual] =
                                    AnglrSyntaxRuleDrawingBuilder.DrawSyntaxGroup (this, p__anglr_syntax_rule_);
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
                            AnglrSyntaxRuleDrawingBuilder.DrawNestedRule (this, p__anglr_nested_rule_);
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
                        AnglrSyntaxRuleDrawingBuilder.DrawSyntaxRuleName (this, p__anglr_syntax_production_list_name_.m__identifier_);
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
                                    AnglrSyntaxRuleDrawingBuilder.DrawNameList (this, p__anglr_syntax_production_.m__name_list_);
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
                                    AnglrSyntaxRuleDrawingBuilder.DrawGeneralizedSymbol (this, p__g_name_);
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
                                        visual = AnglrSyntaxRuleDrawingBuilder.DrawRawConstantSymbol (this, p_SymbolToken);
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
                                            visual = AnglrSyntaxRuleDrawingBuilder.DrawRawTerminalSymbol (this, p_SymbolToken);
                                        }
                                        else
                                        {
                                            AnglrLogger?.DebugLine ($"non-terminal symbol name = {p_SymbolToken.Name}");
                                            visual = AnglrSyntaxRuleDrawingBuilder.DrawRawNonTerminalSymbol (this, p_SymbolToken);
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
