using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Documents;
using System.Windows.Media;
using System.Windows;

namespace J_JHealthSolutions.Model
{
    public class ErrorAdorner : Adorner
    {
        public ErrorAdorner(UIElement adornedElement) : base(adornedElement)
        {
            this.IsHitTestVisible = false;
        }

        protected override void OnRender(DrawingContext drawingContext)
        {
            base.OnRender(drawingContext);

            var adornedElementRect = new Rect(this.AdornedElement.RenderSize);

            SolidColorBrush renderBrush = Brushes.Red;
            double renderThickness = 2.0;
            Pen renderPen = new Pen(renderBrush, renderThickness);

            // Draw a rectangle around the control
            drawingContext.DrawRectangle(null, renderPen, adornedElementRect);
        }
    }
}
