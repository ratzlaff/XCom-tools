using System.Drawing;

namespace MapView.TopViewForm
{
    public class SolidPenBrush
    {
        public SolidPenBrush(SolidBrush brush, float width)
        {
            Brush = brush;
            Pen = new Pen(brush.Color);
            Pen.Width = width;
        }

        public SolidPenBrush(Pen pen)
        {
            Brush = new SolidBrush(pen.Color);
            Pen = pen;
        }

        public readonly SolidBrush Brush;
        public readonly Pen Pen;
    }
}