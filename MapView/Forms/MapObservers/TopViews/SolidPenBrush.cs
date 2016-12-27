using System.Drawing;

namespace MapView.Forms.MapObservers.TopViews
{
	public class SolidPenBrush
	{
		public SolidPenBrush(SolidBrush brush, float width)
		{
			Brush = brush;
			Pen = new Pen(brush.Color);
			Pen.Width = width;
			LightBrush = new SolidBrush(Color.FromArgb(50, brush.Color));
			LightPen = new Pen(Color.FromArgb(50, brush.Color), width);
		}

		public SolidPenBrush(Pen pen)
		{
			Brush = new SolidBrush(pen.Color);
			Pen = pen;
			LightBrush = new SolidBrush(Color.FromArgb(70, pen.Color));
			LightPen = new Pen(Color.FromArgb(70, pen.Color), pen.Width);
		}

		public readonly SolidBrush Brush;
		public readonly SolidBrush LightBrush;
		public readonly Pen Pen;
		public readonly Pen LightPen;
	}
}
