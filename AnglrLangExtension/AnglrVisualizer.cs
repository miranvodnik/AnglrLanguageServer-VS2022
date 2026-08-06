using Anglr.Parser;
using Anglr.Parser.Core;
using Anglr.Parser.SyntaxTree;
using AnglrJsonRpcMethods;
using AnglrLibrary;
using AnglrLogLibrary;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Diagnostics.SymbolStore;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;

namespace AnglrLangExtension
{

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

    public class AnglrRawDrawingVisual : DrawingVisual
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
        public static int ConnectorRadius { get; set; } = 6;
        public static int ConnectorLength { get; set; } = 20;

        //
        // object properties
        //

        public double ConnectorOffset { get; set; }
        public double Width { get; set; }
        public double Height { get; set; }
    }

    public interface IAnglrVisualCloneable
    {
        AnglrDrawingVisual Clone ();
    }

    public interface IAnglrRawVisualCloneable
    {
        AnglrRawDrawingVisual Clone ();
    }

    public interface IAnglrEventHandler
    {
        void OnMouseDown (object sender, MouseButtonEventArgs e, Point point, IAnglrLogger logger);
        void OnMouseEnter (object sender, MouseButtonEventArgs e, Point point, IAnglrLogger logger);
        void OnMouseLeave (object sender, MouseButtonEventArgs e, Point point, IAnglrLogger logger);
        void OnMouseLeftButtonDown (object sender, MouseButtonEventArgs e, Point point, IAnglrLogger logger);
        void OnMouseLeftButtonUp (object sender, MouseButtonEventArgs e, Point point, IAnglrLogger logger);
        void OnMouseMove (object sender, MouseButtonEventArgs e, Point point, IAnglrLogger logger);
        void OnMouseRightButtonDown (object sender, MouseButtonEventArgs e, Point point, IAnglrLogger logger);
        void OnMouseRightButtonUp (object sender, MouseButtonEventArgs e, Point point, IAnglrLogger logger);
        void OnMouseUp (object sender, MouseButtonEventArgs e, Point point, IAnglrLogger logger);
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
        public void OnMouseEnter (object sender, MouseButtonEventArgs e, Point point, IAnglrLogger logger) { }
        public void OnMouseLeave (object sender, MouseButtonEventArgs e, Point point, IAnglrLogger logger) { }
        public void OnMouseLeftButtonDown (object sender, MouseButtonEventArgs e, Point point, IAnglrLogger logger) { }
        public void OnMouseLeftButtonUp (object sender, MouseButtonEventArgs e, Point point, IAnglrLogger logger) { }
        public void OnMouseMove (object sender, MouseButtonEventArgs e, Point point, IAnglrLogger logger) { }
        public void OnMouseRightButtonDown (object sender, MouseButtonEventArgs e, Point point, IAnglrLogger logger) { }
        public void OnMouseRightButtonUp (object sender, MouseButtonEventArgs e, Point point, IAnglrLogger logger) { }
        public void OnMouseUp (object sender, MouseButtonEventArgs e, Point point, IAnglrLogger logger) { }
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
        public void OnMouseEnter (object sender, MouseButtonEventArgs e, Point point, IAnglrLogger logger) { }
        public void OnMouseLeave (object sender, MouseButtonEventArgs e, Point point, IAnglrLogger logger) { }
        public void OnMouseLeftButtonDown (object sender, MouseButtonEventArgs e, Point point, IAnglrLogger logger) { }
        public void OnMouseLeftButtonUp (object sender, MouseButtonEventArgs e, Point point, IAnglrLogger logger) { }
        public void OnMouseMove (object sender, MouseButtonEventArgs e, Point point, IAnglrLogger logger) { }
        public void OnMouseRightButtonDown (object sender, MouseButtonEventArgs e, Point point, IAnglrLogger logger) { }
        public void OnMouseRightButtonUp (object sender, MouseButtonEventArgs e, Point point, IAnglrLogger logger) { }
        public void OnMouseUp (object sender, MouseButtonEventArgs e, Point point, IAnglrLogger logger) { }
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
        public void OnMouseEnter (object sender, MouseButtonEventArgs e, Point point, IAnglrLogger logger) { }
        public void OnMouseLeave (object sender, MouseButtonEventArgs e, Point point, IAnglrLogger logger) { }
        public void OnMouseLeftButtonDown (object sender, MouseButtonEventArgs e, Point point, IAnglrLogger logger) { }
        public void OnMouseLeftButtonUp (object sender, MouseButtonEventArgs e, Point point, IAnglrLogger logger) { }
        public void OnMouseMove (object sender, MouseButtonEventArgs e, Point point, IAnglrLogger logger) { }
        public void OnMouseRightButtonDown (object sender, MouseButtonEventArgs e, Point point, IAnglrLogger logger) { }
        public void OnMouseRightButtonUp (object sender, MouseButtonEventArgs e, Point point, IAnglrLogger logger) { }
        public void OnMouseUp (object sender, MouseButtonEventArgs e, Point point, IAnglrLogger logger) { }
    }

    public class AnglrRawTerminalSymbolVisual : AnglrRawDrawingVisual, IAnglrRawVisualCloneable
    {
        public SimpleSymbolToken SymbolToken { get; private set; }
        public AnglrRawTerminalSymbolVisual (SimpleSymbolToken symbolToken)
        {
            SymbolToken = symbolToken;
        }
        public AnglrRawTerminalSymbolVisual (AnglrRawTerminalSymbolVisual terminalSymbol)
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

        public AnglrRawDrawingVisual Clone ()
        {
            AnglrRawTerminalSymbolVisual cloned = new AnglrRawTerminalSymbolVisual (this);
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
    }

    public class AnglrRawConstantSymbolVisual : AnglrRawDrawingVisual, IAnglrRawVisualCloneable
    {
        public SimpleSymbolToken SymbolToken { get; private set; }
        public AnglrRawConstantSymbolVisual (SimpleSymbolToken symbolToken)
        {
            SymbolToken = symbolToken;
        }
        public AnglrRawConstantSymbolVisual (AnglrRawConstantSymbolVisual terminalSymbol)
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

        public AnglrRawDrawingVisual Clone ()
        {
            AnglrRawConstantSymbolVisual cloned = new AnglrRawConstantSymbolVisual (this);
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
    }

    public class AnglrRawNonTerminalSymbolVisual : AnglrRawDrawingVisual, IAnglrRawVisualCloneable
    {
        public SimpleSymbolToken SymbolToken { get; private set; }
        public AnglrRawNonTerminalSymbolVisual (SimpleSymbolToken symbolToken)
        {
            SymbolToken = symbolToken;
        }
        public AnglrRawNonTerminalSymbolVisual (AnglrRawNonTerminalSymbolVisual terminalSymbol)
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

        public AnglrRawDrawingVisual Clone ()
        {
            AnglrRawNonTerminalSymbolVisual cloned = new AnglrRawNonTerminalSymbolVisual (this);
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
    }

    public class AnglrGeneralizedNameVisual : AnglrRawDrawingVisual, IAnglrRawVisualCloneable
    {
        public AnglrRawDrawingVisual GnameVisual { get; private set; }
        public _cardinality_ Cardinality { get; private set; }
        public AnglrRawDrawingVisual DelimiterVisual { get; private set; }
        public _cardinality_delimiter_ CardinalityDelimiter { get; private set; }

        public AnglrGeneralizedNameVisual (AnglrRawDrawingVisual gnameVisual, _cardinality_delimiter_ cardinalityDelimiter)
        {
            GnameVisual = gnameVisual;
            CardinalityDelimiter = cardinalityDelimiter;
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
                dc.PushTransform (new TranslateTransform (2 * Margin, 2 * Margin));
                dc.DrawDrawing (GnameVisual.Drawing);
                dc.Pop ();
            }
        }

        private void DrawRepeat ()
        {
            ConnectorOffset = GnameVisual.ConnectorOffset;
            if (DelimiterVisual != null)
            {
                double gwidth = 2 * Margin;
                double dwidth = 2 * Margin;
                double gheight = ConnectorOffset;
                double dheight = ConnectorOffset + 2 * Margin + DelimiterVisual.ConnectorOffset;

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
                    dc.PushTransform (new TranslateTransform (gwidth, 0));
                    dc.DrawDrawing (GnameVisual.Drawing);
                    dc.Pop ();
                    dc.PushTransform (new TranslateTransform (dwidth, GnameVisual.Height + 2 * Margin));
                    dc.DrawDrawing (DelimiterVisual.Drawing);
                    dc.Pop ();
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
                    dc.PushTransform (new TranslateTransform (2 * Margin, 0));
                    dc.DrawDrawing (GnameVisual.Drawing);
                    dc.Pop ();
                }
            }
        }

        private void DrawOptionalRepeat ()
        {
            ConnectorOffset = 2 * Margin + GnameVisual.ConnectorOffset;
            if (DelimiterVisual != null)
            {
                double gwidth = 4 * Margin;
                double dwidth = 4 * Margin;
                double gheight = ConnectorOffset;
                double dheight = ConnectorOffset + 2 * Margin + DelimiterVisual.ConnectorOffset;

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
                    dc.PushTransform (new TranslateTransform (gwidth, 2 * Margin));
                    dc.DrawDrawing (GnameVisual.Drawing);
                    dc.Pop ();
                    dc.PushTransform (new TranslateTransform (dwidth, GnameVisual.Height + 4 * Margin));
                    dc.DrawDrawing (DelimiterVisual.Drawing);
                    dc.Pop ();
                }
            }
            else
            {
                double gwidth = 4 * Margin;
                double dwidth = 4 * Margin;
                double gheight = ConnectorOffset;
                double dheight = ConnectorOffset + 2 * Margin;

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
                    dc.PushTransform (new TranslateTransform (4 * Margin, 2 * Margin));
                    dc.DrawDrawing (GnameVisual.Drawing);
                    dc.Pop ();
                }
            }
        }

        private void DrawInternal ()
        {
            ConnectorOffset = GnameVisual.ConnectorOffset;
            Width = GnameVisual.Width;
            Height = GnameVisual.Height;
            using (var dc = RenderOpen ())
            {
                dc.DrawDrawing (GnameVisual.Drawing);
            }
        }

        public AnglrRawDrawingVisual Clone ()
        {
            throw new NotImplementedException ();
        }
    }

    public class AnglrNameListVisual : AnglrRawDrawingVisual, IAnglrRawVisualCloneable
    {
        public _name_list_ NameList { get; private set; }
        public AnglrNameListVisual (_name_list_ nameList)
        {
            NameList = nameList;
            NameList.Iterate
            (
                null,
                (list, appData) =>
                {
                    _g_name_ name = list.m__g_name_;
                    if ((name == null) || (name.appInfo == null) || !((AppInfo) name.appInfo).TryGetValue (AppInfoType.Visual, out var visual))
                        return null;
                    AnglrRawDrawingVisual drawingVisual = visual as AnglrRawDrawingVisual;
                    if (drawingVisual == null)
                        return null;
                    if (ConnectorOffset < drawingVisual.ConnectorOffset)
                        ConnectorOffset = drawingVisual.ConnectorOffset;
                    return null;
                }
            );
            NameList.Iterate
            (
                null,
                (list, appData) =>
                {
                    _g_name_ name = list.m__g_name_;
                    if ((name == null) || (name.appInfo == null) || !((AppInfo) name.appInfo).TryGetValue (AppInfoType.Visual, out var visual))
                        return null;
                    AnglrRawDrawingVisual drawingVisual = visual as AnglrRawDrawingVisual;
                    if (drawingVisual == null)
                        return null;
                    double diff = drawingVisual.Height - drawingVisual.ConnectorOffset;
                    if (diff > Height)
                        Height = diff;
                    Width += drawingVisual.Width + 2 * Margin;
                    return null;
                }
            );
            Height += ConnectorOffset;
            Width -= 2 * Margin;
        }

        public void Draw ()
        {
            int counter = 0;
            double width = 0;
            using (var dc = RenderOpen ())
            {
                NameList.Iterate
                (
                    null,
                    (list, appData) =>
                    {
                        _g_name_ name = list.m__g_name_;
                        if ((name == null) || (name.appInfo == null) || !((AppInfo) name.appInfo).TryGetValue (AppInfoType.Visual, out var visual))
                            return null;
                        AnglrRawDrawingVisual drawingVisual = visual as AnglrRawDrawingVisual;
                        if (drawingVisual == null)
                            return null;
                        dc.PushTransform (new TranslateTransform (width, 0));
                        if (counter > 0)
                        {
                            dc.DrawLine (Pen, new Point (0, ConnectorOffset), new Point (2 * Margin, ConnectorOffset));
                            dc.PushTransform (new TranslateTransform (2 * Margin, 0));
                        }
                        dc.PushTransform (new TranslateTransform (0, ConnectorOffset - drawingVisual.ConnectorOffset));
                        dc.DrawDrawing (drawingVisual.Drawing);
                        dc.Pop ();
                        width += drawingVisual.Width;
                        if (counter > 0)
                        {
                            width += 2 * Margin;
                            dc.Pop ();
                        }
                        dc.Pop ();
                        ++counter;
                        return null;
                    }
                );
            }
        }

        public AnglrRawDrawingVisual Clone ()
        {
            throw new NotImplementedException ();
        }
    }

    public class AnglrNestedRuleVisual : AnglrRawDrawingVisual, IAnglrRawVisualCloneable
    {
        public _anglr_nested_rule_ NestedRule { get; private set; }
        public AnglrNestedRuleVisual (_anglr_nested_rule_ nestedRule)
        {
            NestedRule = nestedRule;
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
                    Height += drawingVisual.Height + 2 * Margin;
                    if (counter == 0)
                        ConnectorOffset = drawingVisual.ConnectorOffset;
                    else
                        Height += 2 * Margin;
                    return counter + 1;
                }
            );
            Width += 4 * Margin;
        }
        public void Draw ()
        {
            double height = 0;
            double connectionOffset = 0;
            using (var dc = RenderOpen ())
            {
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
                        dc.PushTransform (new TranslateTransform (2 * Margin, height));
                        dc.DrawDrawing (drawingVisual.Drawing);
                        dc.Pop ();
                        dc.DrawLine (Pen, new Point (drawingVisual.Width + 2 * Margin, height + drawingVisual.ConnectorOffset), new Point (Width, height + drawingVisual.ConnectorOffset));
                        height += drawingVisual.Height + 2 * Margin;
                        return counter + 1;
                    }
                );
                dc.DrawLine (Pen, new Point (0, ConnectorOffset), new Point (0, connectionOffset));
                dc.DrawLine (Pen, new Point (Width, ConnectorOffset), new Point (Width, connectionOffset));
            }
        }
        public AnglrRawDrawingVisual Clone ()
        {
            throw new NotImplementedException ();
        }
    }

    public class AnglrSyntaxRuleVisual : AnglrRawDrawingVisual, IAnglrRawVisualCloneable
    {
        public _anglr_syntax_rule_ SyntaxRule { get; private set; }
        public AnglrSyntaxRuleVisual (_anglr_syntax_rule_ syntaxRule)
        {
            SyntaxRule = syntaxRule;
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
                    Height += drawingVisual.Height + 2 * Margin;
                    if (counter == 0)
                        ConnectorOffset = drawingVisual.ConnectorOffset;
                    else
                        Height += 2 * Margin;
                    return counter + 1;
                }
            );
            Width += 4 * Margin;
        }
        public void Draw ()
        {
            double height = 0;
            double connectionOffset = 0;
            using (var dc = RenderOpen ())
            {
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
                        dc.PushTransform (new TranslateTransform (2 * Margin, height));
                        dc.DrawDrawing (drawingVisual.Drawing);
                        dc.Pop ();
                        dc.DrawLine (Pen, new Point (drawingVisual.Width + 2 * Margin, height + drawingVisual.ConnectorOffset), new Point (Width, height + drawingVisual.ConnectorOffset));
                        height += drawingVisual.Height + 2 * Margin;
                        return counter + 1;
                    }
                );
                dc.DrawLine (Pen, new Point (0, ConnectorOffset), new Point (0, connectionOffset));
                dc.DrawLine (Pen, new Point (Width, ConnectorOffset), new Point (Width, connectionOffset));
            }
        }
        public AnglrRawDrawingVisual Clone ()
        {
            throw new NotImplementedException ();
        }
    }

    public class AnglrSyntaxGroupVisual : AnglrRawDrawingVisual, IAnglrRawVisualCloneable
    {
        public _anglr_syntax_rule_ SyntaxGroup { get; private set; }
        public AnglrSyntaxGroupVisual (_anglr_syntax_rule_ syntaxGroup)
        {
            SyntaxGroup = syntaxGroup;
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
                        Height += drawingVisual.Height + 2 * Margin;
                        if (counter == 0)
                            ConnectorOffset = drawingVisual.ConnectorOffset;
                        else
                            Height += 2 * Margin;
                        return counter + 1;
                    }
                );
            }
        }
        public void Draw ()
        {
            double height = 0;
            double connectionOffset = 0;
            using (var dc = RenderOpen ())
            {
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
                            connectionOffset = height + drawingVisual.ConnectorOffset;
                            dc.PushTransform (new TranslateTransform (0, height));
                            dc.DrawDrawing (drawingVisual.Drawing);
                            dc.Pop ();
                            height += drawingVisual.Height + 2 * Margin;
                            return counter + 1;
                        }
                    );
                }
            }
        }
        public AnglrRawDrawingVisual Clone ()
        {
            throw new NotImplementedException ();
        }
    }

    public class AnglrParserPartVisual : AnglrRawDrawingVisual, IAnglrRawVisualCloneable
    {
        public _parser_part_ ParserPart { get; private set; }
        public AnglrParserPartVisual (_parser_part_ parserPart)
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
                        else
                            Height += 2 * Margin;
                        return counter + 1;
                    }
                );
            }
        }

        public void Draw ()
        {
            double height = 0;
            double connectionOffset = 0;
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
                            connectionOffset = height + drawingVisual.ConnectorOffset;
                            dc.PushTransform (new TranslateTransform (0, height));
                            dc.DrawDrawing (drawingVisual.Drawing);
                            dc.Pop ();
                            height += drawingVisual.Height + 2 * Margin;
                            return counter + 1;
                        }
                    );
                }
            }
        }

        public AnglrRawDrawingVisual Clone ()
        {
            throw new NotImplementedException ();
        }
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
        public void OnMouseEnter (object sender, MouseButtonEventArgs e, Point point, IAnglrLogger logger) { }
        public void OnMouseLeave (object sender, MouseButtonEventArgs e, Point point, IAnglrLogger logger) { }
        public void OnMouseLeftButtonDown (object sender, MouseButtonEventArgs e, Point point, IAnglrLogger logger) { }
        public void OnMouseLeftButtonUp (object sender, MouseButtonEventArgs e, Point point, IAnglrLogger logger) { }
        public void OnMouseMove (object sender, MouseButtonEventArgs e, Point point, IAnglrLogger logger) { }
        public void OnMouseRightButtonDown (object sender, MouseButtonEventArgs e, Point point, IAnglrLogger logger) { }
        public void OnMouseRightButtonUp (object sender, MouseButtonEventArgs e, Point point, IAnglrLogger logger) { }
        public void OnMouseUp (object sender, MouseButtonEventArgs e, Point point, IAnglrLogger logger) { }
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

        public static AnglrRawDrawingVisual DrawRawTerminalSymbol (SimpleSymbolToken symbolToken)
        {
            AnglrRawTerminalSymbolVisual visual = new AnglrRawTerminalSymbolVisual (symbolToken);
            visual.Draw ();
            return visual;
        }

        public static AnglrRawDrawingVisual DrawRawConstantSymbol (SimpleSymbolToken symbolToken)
        {
            AnglrRawConstantSymbolVisual visual = new AnglrRawConstantSymbolVisual (symbolToken);
            visual.Draw ();
            return visual;
        }

        public static AnglrRawDrawingVisual DrawRawNonTerminalSymbol (SimpleSymbolToken symbolToken)
        {
            AnglrRawNonTerminalSymbolVisual visual = new AnglrRawNonTerminalSymbolVisual (symbolToken);
            visual.Draw ();
            return visual;
        }

        public static AnglrRawDrawingVisual DrawGeneralizedSymbol (AnglrRawDrawingVisual generalizedDrawing, _cardinality_delimiter_ cardinalityDelimiter)
        {
            AnglrGeneralizedNameVisual visual = new AnglrGeneralizedNameVisual (generalizedDrawing, cardinalityDelimiter);
            visual.Draw ();
            return visual;
        }

        public static AnglrRawDrawingVisual DrawNameList (_name_list_ nameList)
        {
            AnglrNameListVisual visual = new AnglrNameListVisual (nameList);
            visual.Draw ();
            return visual;
        }

        public static AnglrRawDrawingVisual DrawNestedRule (_anglr_nested_rule_ nestedRule)
        {
            AnglrNestedRuleVisual visual = new AnglrNestedRuleVisual (nestedRule);
            visual.Draw ();
            return visual;
        }

        public static AnglrRawDrawingVisual DrawSyntaxRule (_anglr_syntax_rule_ syntaxRule)
        {
            AnglrSyntaxRuleVisual visual = new AnglrSyntaxRuleVisual (syntaxRule);
            visual.Draw ();
            return visual;
        }

        public static AnglrRawDrawingVisual DrawSyntaxGroup (_anglr_syntax_rule_ syntaxRule)
        {
            AnglrSyntaxGroupVisual visual = new AnglrSyntaxGroupVisual (syntaxRule);
            visual.Draw ();
            return visual;
        }

        public static AnglrRawDrawingVisual DrawParserPart (_parser_part_ parserPart)
        {
            AnglrParserPartVisual visual = new AnglrParserPartVisual (parserPart);
            visual.Draw ();
            return visual;
        }

        public static AnglrDrawingDictionary BuildCanonicalSyntaxRulesDrawings (AnglrGetParserSyntaxRulesResult syntaxRulesResult, AnglrGetSyntaxTreeResult syntaxTreeResult, IAnglrLogger logger)
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
                }
                else
                    logger?.ErrorLine ($"null anglr fragment conversion");
            }
            catch (Exception e)
            {
                logger?.ErrorLine (e, $"visualizer failure");
            }
            return SyntaxRuleVisuals;
        }
    }

    internal class AnglrVisualizer : SyntaxTreeWalker
    {
        public IAnglrLogger AnglrLogger { get; private set; }
        public AnglrVisualizer (IAnglrLogger logger)
        {
            AnglrLogger = logger;
            _parser_part__Event += AnglrVisualizer__parser_part__Event;
            _anglr_syntax_rule_list__Event += AnglrVisualizer__anglr_syntax_rule_list__Event;
            _anglr_syntax_rule__Event += AnglrVisualizer__anglr_syntax_rule__Event;
            _anglr_nested_rule__Event += AnglrVisualizer__anglr_nested_rule__Event;
            _anglr_syntax_production_list_name__Event += AnglrVisualizer__anglr_syntax_production_list_name__Event;
            _anglr_syntax_production_list__Event += AnglrVisualizer__anglr_syntax_production_list__Event;
            _anglr_syntax_production__Event += AnglrVisualizer__anglr_syntax_production__Event;
            _production_name__Event += AnglrVisualizer__production_name__Event;
            _priority_assoc_specification__Event += AnglrVisualizer__priority_assoc_specification__Event;
            _priority_specification__Event += AnglrVisualizer__priority_specification__Event;
            _associativity_specification__Event += AnglrVisualizer__associativity_specification__Event;
            _name_list__Event += AnglrVisualizer__name_list__Event;
            _marker_list__Event += AnglrVisualizer__marker_list__Event;
            _marker__Event += AnglrVisualizer__marker__Event;
            _g_name__Event += AnglrVisualizer__g_name__Event;
            _name__Event += AnglrVisualizer__name__Event;
            _cardinality_delimiter__Event += AnglrVisualizer__cardinality_delimiter__Event;
            _cardinality__Event += AnglrVisualizer__cardinality__Event;
            _delimiter__Event += AnglrVisualizer__delimiter__Event;
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
                        ((AppInfo) p__parser_part_.appInfo) [AppInfoType.Visual] = AnglrSyntaxRuleDrawingBuilder.DrawParserPart (p__parser_part_);
                    }
                    catch (Exception e)
                    {
                        AnglrLogger?.ErrorLine (e, $"Visualization of <anglr syntax rule> node {p__parser_part_.Emit (-1).Substring (0, 100)} failed");
                    }
                    break;
            }
            return true;
        }

        private bool AnglrVisualizer__anglr_syntax_rule_list__Event (SyntaxTreeCallbackReason reason, _anglr_syntax_rule_list_.production_kind kind, _anglr_syntax_rule_list_ p__anglr_syntax_rule_list_)
        {
            switch (reason)
            {
                case SyntaxTreeCallbackReason.TraversalPrologueCallbackReason:
                    break;
                case SyntaxTreeCallbackReason.TraversalEpilogueCallbackReason:
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
                                ((AppInfo) p__anglr_syntax_rule_.appInfo) [AppInfoType.Visual] =
                                    AnglrSyntaxRuleDrawingBuilder.DrawSyntaxRule (p__anglr_syntax_rule_);
                                break;
                            case _anglr_syntax_rule_.production_kind.g__anglr_syntax_rule__2:
                                ((AppInfo) p__anglr_syntax_rule_.appInfo) [AppInfoType.Visual] =
                                    AnglrSyntaxRuleDrawingBuilder.DrawSyntaxGroup (p__anglr_syntax_rule_);
                                break;
                        }
                    }
                    catch (Exception e)
                    {
                        AnglrLogger?.ErrorLine (e, $"Visualization of <anglr syntax rule> node {p__anglr_syntax_rule_.Emit (-1).Substring (0, 100)} failed");
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
                            AnglrSyntaxRuleDrawingBuilder.DrawNestedRule (p__anglr_nested_rule_);
                    }
                    catch (Exception e)
                    {
                        AnglrLogger?.ErrorLine (e, $"Visualization of <anglr nested rule> node {p__anglr_nested_rule_.Emit (-1).Substring (0, 100)} failed");
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
                    break;
            }
            return true;
        }

        private bool AnglrVisualizer__anglr_syntax_production_list__Event (SyntaxTreeCallbackReason reason, _anglr_syntax_production_list_.production_kind kind, _anglr_syntax_production_list_ p__anglr_syntax_production_list_)
        {
            switch (reason)
            {
                case SyntaxTreeCallbackReason.TraversalPrologueCallbackReason:
                    break;
                case SyntaxTreeCallbackReason.TraversalEpilogueCallbackReason:
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
                                ((AppInfo) p__anglr_syntax_production_.appInfo) [AppInfoType.Visual] = AnglrSyntaxRuleDrawingBuilder.DrawNameList (p__anglr_syntax_production_.m__name_list_);
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
                        AnglrLogger?.ErrorLine (e, $"Visualization of <anglr syntax production> node {p__anglr_syntax_production_.Emit (-1).Substring (0, 100)} failed");
                    }
                }
                break;
            }
            return true;
        }

        private bool AnglrVisualizer__production_name__Event (SyntaxTreeCallbackReason reason, _production_name_.production_kind kind, _production_name_ p__production_name_)
        {
            switch (reason)
            {
                case SyntaxTreeCallbackReason.TraversalPrologueCallbackReason:
                    break;
                case SyntaxTreeCallbackReason.TraversalEpilogueCallbackReason:
                    break;
            }
            return true;
        }

        private bool AnglrVisualizer__priority_assoc_specification__Event (SyntaxTreeCallbackReason reason, _priority_assoc_specification_.production_kind kind, _priority_assoc_specification_ p__priority_assoc_specification_)
        {
            switch (reason)
            {
                case SyntaxTreeCallbackReason.TraversalPrologueCallbackReason:
                    break;
                case SyntaxTreeCallbackReason.TraversalEpilogueCallbackReason:
                    break;
            }
            return true;
        }

        private bool AnglrVisualizer__priority_specification__Event (SyntaxTreeCallbackReason reason, _priority_specification_.production_kind kind, _priority_specification_ p__priority_specification_)
        {
            switch (reason)
            {
                case SyntaxTreeCallbackReason.TraversalPrologueCallbackReason:
                    break;
                case SyntaxTreeCallbackReason.TraversalEpilogueCallbackReason:
                    break;
            }
            return true;
        }

        private bool AnglrVisualizer__associativity_specification__Event (SyntaxTreeCallbackReason reason, _associativity_specification_.production_kind kind, _associativity_specification_ p__associativity_specification_)
        {
            switch (reason)
            {
                case SyntaxTreeCallbackReason.TraversalPrologueCallbackReason:
                    break;
                case SyntaxTreeCallbackReason.TraversalEpilogueCallbackReason:
                    break;
            }
            return true;
        }

        private bool AnglrVisualizer__name_list__Event (SyntaxTreeCallbackReason reason, _name_list_.production_kind kind, _name_list_ p__name_list_)
        {
            switch (reason)
            {
                case SyntaxTreeCallbackReason.TraversalPrologueCallbackReason:
                    break;
                case SyntaxTreeCallbackReason.TraversalEpilogueCallbackReason:
                    break;
            }
            return true;
        }

        private bool AnglrVisualizer__marker_list__Event (SyntaxTreeCallbackReason reason, _marker_list_.production_kind kind, _marker_list_ p__marker_list_)
        {
            switch (reason)
            {
                case SyntaxTreeCallbackReason.TraversalPrologueCallbackReason:
                    break;
                case SyntaxTreeCallbackReason.TraversalEpilogueCallbackReason:
                    break;
            }
            return true;
        }

        private bool AnglrVisualizer__marker__Event (SyntaxTreeCallbackReason reason, _marker_.production_kind kind, _marker_ p__marker_)
        {
            switch (reason)
            {
                case SyntaxTreeCallbackReason.TraversalPrologueCallbackReason:
                    break;
                case SyntaxTreeCallbackReason.TraversalEpilogueCallbackReason:
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
                                if (!((AppInfo) p__g_name_.m__cardinality_delimiter_.appInfo).TryGetValue (AppInfoType.Visual, out var cardinalityVisual))
                                    break;
                                ((AppInfo) p__g_name_.appInfo) [AppInfoType.Visual] =
                                    AnglrSyntaxRuleDrawingBuilder.DrawGeneralizedSymbol
                                    (
                                        gnameVisual as AnglrRawDrawingVisual,
                                        p__g_name_.m__cardinality_delimiter_
                                    );
                            }
                            break;
                        }
                    }
                    catch (Exception e)
                    {
                        AnglrLogger?.ErrorLine (e, $"Visualization of <g name> {p__g_name_.Emit (-1).Substring (0, 100)} failed");
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
                                        visual = AnglrSyntaxRuleDrawingBuilder.DrawRawConstantSymbol (p_SymbolToken);
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
                                            visual = AnglrSyntaxRuleDrawingBuilder.DrawRawTerminalSymbol (p_SymbolToken);
                                        }
                                        else
                                        {
                                            AnglrLogger?.DebugLine ($"non-terminal symbol name = {p_SymbolToken.Name}");
                                            visual = AnglrSyntaxRuleDrawingBuilder.DrawRawNonTerminalSymbol (p_SymbolToken);
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
                        AnglrLogger?.ErrorLine (e, $"Visualization of <name> node {p__name_.Emit (-1).Substring (0, 100)} failed");
                    }
                    break;
            }
            return true;
        }

        private bool AnglrVisualizer__cardinality_delimiter__Event (SyntaxTreeCallbackReason reason, _cardinality_delimiter_.production_kind kind, _cardinality_delimiter_ p__cardinality_delimiter_)
        {
            switch (reason)
            {
                case SyntaxTreeCallbackReason.TraversalPrologueCallbackReason:
                    break;
                case SyntaxTreeCallbackReason.TraversalEpilogueCallbackReason:
                    break;
            }
            return true;
        }

        private bool AnglrVisualizer__cardinality__Event (SyntaxTreeCallbackReason reason, _cardinality_.production_kind kind, _cardinality_ p__cardinality_)
        {
            switch (reason)
            {
                case SyntaxTreeCallbackReason.TraversalPrologueCallbackReason:
                    break;
                case SyntaxTreeCallbackReason.TraversalEpilogueCallbackReason:
                    break;
            }
            return true;
        }

        private bool AnglrVisualizer__delimiter__Event (SyntaxTreeCallbackReason reason, _delimiter_.production_kind kind, _delimiter_ p__delimiter_)
        {
            switch (reason)
            {
                case SyntaxTreeCallbackReason.TraversalPrologueCallbackReason:
                    break;
                case SyntaxTreeCallbackReason.TraversalEpilogueCallbackReason:
                    break;
            }
            return true;
        }
    }
}
