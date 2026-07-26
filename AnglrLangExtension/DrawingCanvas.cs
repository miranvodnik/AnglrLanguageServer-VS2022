using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows.Media;

namespace AnglrLangExtension
{
    internal class DrawingCanvas : Canvas
    {
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
    }
}
