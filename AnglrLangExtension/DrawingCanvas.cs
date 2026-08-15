using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Media;
using AnglrLogLibrary;

namespace AnglrLangExtension
{
    internal class DrawingCanvas : Panel, IScrollInfo
    {
        public IAnglrLogger Logger { get; set; }
        private VisualCollection _visualCollection;
        public DrawingCanvas()
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

        protected override void OnRender (DrawingContext dc)
        {
            Logger?.DebugLine ($"render");
            dc.PushTransform (new TranslateTransform (HorizontalOffset, VerticalOffset));
            dc.PushClip (new RectangleGeometry (new Rect (0, 0, ViewportWidth, ViewportHeight)));
            foreach (var visual in _visualCollection)
            {
                DrawingVisual drawingVisual = visual as DrawingVisual;
                if (drawingVisual == null)
                    continue;
                Logger?.DebugLine ($"render visual {drawingVisual.ContentBounds}");
                //dc.DrawDrawing (drawingVisual.Drawing);
            }
            dc.Pop ();
            dc.Pop ();
        }
    }
}
