using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Input;
using System.Windows.Media;
using AnglrLogLibrary;

namespace AnglrLangExtension
{
    internal class AnglrParserPartDrawingCanvas : Panel, IScrollInfo
    {
        public IAnglrLogger Logger { get; set; }
        private VisualCollection _visualCollection;
        public AnglrParserPartDrawingCanvas ()
        {
            _visualCollection = new VisualCollection (this);
            MouseDown += AnglrParserPartDrawingCanvas_OnMouseDown;
            MouseUp += AnglrParserPartDrawingCanvas_OnMouseUp;
            MouseEnter += AnglrParserPartDrawingCanvas_OnMouseEnter;
            MouseLeave += AnglrParserPartDrawingCanvas_OnMouseLeave;
            MouseLeftButtonDown += AnglrParserPartDrawingCanvas_OnMouseLeftButtonDown;
            MouseLeftButtonUp += AnglrParserPartDrawingCanvas_OnMouseLeftButtonUp;
            MouseRightButtonDown += AnglrParserPartDrawingCanvas_OnMouseRightButtonDown;
            MouseRightButtonUp += AnglrParserPartDrawingCanvas_OnMouseRightButtonUp;
            MouseMove += AnglrParserPartDrawingCanvas_OnMouseMove;
        }

        private void AnglrParserPartDrawingCanvas_OnMouseDown (object sender, MouseButtonEventArgs e)
        {
            int result = MouseEventDispatcher (sender, e, AnglrMouseEventKind.MouseDown, $"mouse down");
            if (result < 0)
            {

            }
        }

        private void AnglrParserPartDrawingCanvas_OnMouseUp (object sender, MouseButtonEventArgs e)
        {
            int result = MouseEventDispatcher (sender, e, AnglrMouseEventKind.MouseUp, $"mouse up");
            if (result < 0)
            {

            }
        }

        private void AnglrParserPartDrawingCanvas_OnMouseEnter (object sender, MouseEventArgs e)
        {
            int result = MouseEventDispatcher (sender, e, AnglrMouseEventKind.MouseEnter, $"mouse enter");
            if (result < 0)
            {

            }
        }

        private void AnglrParserPartDrawingCanvas_OnMouseLeave (object sender, MouseEventArgs e)
        {
            int result = MouseEventDispatcher (sender, e, AnglrMouseEventKind.MouseLeave, $"mouse leave");
            if (result < 0)
            {

            }
        }

        private void AnglrParserPartDrawingCanvas_OnMouseLeftButtonDown (object sender, MouseButtonEventArgs e)
        {
            int result = MouseEventDispatcher (sender, e, AnglrMouseEventKind.MouseLeftButttonDown, $"mouse left button down");
            if (result < 0)
            {

            }
        }

        private void AnglrParserPartDrawingCanvas_OnMouseLeftButtonUp (object sender, MouseButtonEventArgs e)
        {
            int result = MouseEventDispatcher (sender, e, AnglrMouseEventKind.MouseLeftButttonUp, $"mouse left button up");
            if (result < 0)
            {

            }
        }

        private void AnglrParserPartDrawingCanvas_OnMouseRightButtonDown (object sender, MouseButtonEventArgs e)
        {
            int result = MouseEventDispatcher (sender, e, AnglrMouseEventKind.MouseRightButttonDown, $"mouse right button down");
            if (result < 0)
            {

            }
        }

        private void AnglrParserPartDrawingCanvas_OnMouseRightButtonUp (object sender, MouseButtonEventArgs e)
        {
            int result = MouseEventDispatcher (sender, e, AnglrMouseEventKind.MouseRightButttonUp, $"mouse right button up");
            if (result < 0)
            {

            }
        }

        private void AnglrParserPartDrawingCanvas_OnMouseMove (object sender, MouseEventArgs e)
        {
            int result = MouseEventDispatcher (sender, e, AnglrMouseEventKind.MouseMove, $"mouse move");
            if (result < 0)
            {

            }
        }

        private int MouseEventDispatcher (object sender, MouseEventArgs e, AnglrMouseEventKind mouseEventKind, string debugText)
        {
            switch (mouseEventKind)
            {
                case AnglrMouseEventKind.MouseEnter:
                case AnglrMouseEventKind.MouseLeave:
                case AnglrMouseEventKind.MouseMove:
                case AnglrMouseEventKind.MouseWheel:
                    return -1;
            }
            var p = e.GetPosition (sender as IInputElement);
            if (p == null)
            {
                Logger?.InfoLine ($"*** NO ANGLR VISUAL HIT ***, kind = {mouseEventKind}");
                return -1;
            }

            List<Visual> visuals = new List<Visual> ();
            VisualTreeHelper.HitTest
            (
                sender as Visual,
                null,
                (result) =>
                {
                    if (result.VisualHit is Visual visual)
                        visuals.Add (visual);
                    return HitTestResultBehavior.Continue;
                },
                new PointHitTestParameters (p)
            );

            if (visuals.Count == 0)
            {
                Logger?.InfoLine ($"*** NO ANGLR VISUAL HIT ***, kind = {mouseEventKind}, point = {p}");
                return 0;
            }
            foreach (Visual visual in visuals)
            {
                Vector offset = new Vector (p.X, p.Y);
                for (DrawingVisual parent = visual as DrawingVisual; parent != null; parent = parent.Parent as DrawingVisual)
                {
                    Rect bounds = parent.ContentBounds;
                    bounds.Union (parent.DescendantBounds);
                    offset -= parent.Offset;
                }
                Logger?.InfoLine ($"*** {debugText} ***, is raw = {visual is AnglrRawDrawingVisual}, point = {(Point) offset}");
                (visual as AnglrRawDrawingVisual)?.HitTest (sender, e, (Point) offset, mouseEventKind);
            }
            return 1;
        }

        protected override int VisualChildrenCount
        {
            get => _visualCollection.Count;
        }

        protected override Visual GetVisualChild (int index) => _visualCollection [index];
        public void AddVisual (Visual visual) => _visualCollection.Add (visual);
        public void DeleteVisual (Visual visual) => _visualCollection.Remove (visual);
        public void Clear () => _visualCollection.Clear ();

        public bool CanVerticallyScroll { get; set; } = true;
        public bool CanHorizontallyScroll { get; set; } = true;

        public double ExtentWidth { get; private set; }

        public double ExtentHeight { get; private set; }

        public double ViewportWidth { get; private set; }

        public double ViewportHeight { get; private set; }

        public double HorizontalOffset { get; private set; }

        public double VerticalOffset { get; private set; }

        public ScrollViewer ScrollOwner { get; set; }

        public void LineDown ()
        {
            Logger?.DebugLine ($"line down");
            SetVerticalOffset (VerticalOffset + 20);
        }

        public void LineUp ()
        {
            Logger?.DebugLine ($"line up");
            SetVerticalOffset (VerticalOffset - 20);
        }

        public void LineLeft ()
        {
            Logger?.DebugLine ($"line left");
            SetHorizontalOffset (HorizontalOffset - 20);
        }

        public void LineRight ()
        {
            Logger?.DebugLine ($"line right");
            SetHorizontalOffset (HorizontalOffset + 20);
        }

        public void PageUp ()
        {
            Logger?.DebugLine ($"page up");
            SetVerticalOffset (VerticalOffset - 10 * 20);
        }

        public void PageDown ()
        {
            Logger?.DebugLine ($"page down");
            SetVerticalOffset (VerticalOffset + 10 * 20);
        }

        public void PageLeft ()
        {
            Logger?.DebugLine ($"page left");
            SetHorizontalOffset (HorizontalOffset - 10 * 20);
        }

        public void PageRight ()
        {
            Logger?.DebugLine ($"page right");
            SetHorizontalOffset (HorizontalOffset + 10 * 20);
        }

        public void MouseWheelUp ()
        {
            Logger?.DebugLine ($"mouse wheel up");
            SetHorizontalOffset (HorizontalOffset - 20);
        }

        public void MouseWheelDown ()
        {
            Logger?.DebugLine ($"mouse wheel down");
            SetHorizontalOffset (HorizontalOffset + 20);
        }

        public void MouseWheelLeft ()
        {
            Logger?.DebugLine ($"mouse wheel left");
            SetHorizontalOffset (HorizontalOffset - 20);
        }

        public void MouseWheelRight ()
        {
            Logger?.DebugLine ($"mouse wheel right");
            SetHorizontalOffset (HorizontalOffset + 20);
        }

        public void SetHorizontalOffset (double offset)
        {
            Logger?.DebugLine ($"set horizontal offset = {offset}");
            HorizontalOffset = Math.Max (0, Math.Min (offset, ExtentWidth - ViewportWidth));
            InvalidateVisual ();
            ScrollOwner?.InvalidateScrollInfo ();
        }

        public void SetVerticalOffset (double offset)
        {
            Logger?.DebugLine ($"set vertical offset = {offset}");
            VerticalOffset = Math.Max (0, Math.Min (offset, ExtentHeight - ViewportHeight));
            InvalidateVisual ();
            ScrollOwner?.InvalidateScrollInfo ();
        }

        public Rect MakeVisible (Visual visual, Rect rectangle)
        {
            Logger?.DebugLine ($"make visible rectangle = {rectangle}");
            return rectangle;
        }

        protected override Size ArrangeOverride (Size finalSize)
        {
            Logger?.DebugLine ($"arrange, size = {finalSize}");
            ViewportWidth = finalSize.Width;
            ViewportHeight = finalSize.Height;
            ExtentWidth = Width;
            ExtentHeight = Height;
            Logger?.DebugLine ($"arrange, view   = ({ViewportWidth}, {ViewportHeight})");
            Logger?.DebugLine ($"arrange, extent = ({ExtentWidth}, {ExtentHeight})");

            return finalSize;
        }

        protected override void OnRender (DrawingContext dc) { }
    }
}
