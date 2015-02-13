using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Forms;
using XCom.Interfaces.Base;
using System.Drawing.Drawing2D;
using System.Drawing;
using XCom;
using System.ComponentModel;

namespace MapView.TopViewForm
{
	public class SimpleMapPanel : Map_Observer_Control
	{
		//private DSShared.Windows.RegistryInfo registryInfo;

		private int _offX = 0;
	    private int _offY = 0;
	    protected int HWidth = 8;
	    protected int HHeight = 4;
	    protected int MinimunHeight = 4;

		private readonly GraphicsPath _upper;
		private readonly GraphicsPath _lower;
		private readonly GraphicsPath _cell;
		private readonly GraphicsPath _copyArea;
		private readonly GraphicsPath _selected;

	    private Point _sel1;
	    private Point _sel2;
	    private Point _sel3;
	    private Point _sel4;

	    private int _mR;
	    private int _mC;

	    public SimpleMapPanel()
		{
			_upper = new GraphicsPath();
			_lower = new GraphicsPath();
			_cell = new GraphicsPath();
			_selected = new GraphicsPath();
			_copyArea = new GraphicsPath();

			_sel1 = new Point(0, 0);
			_sel2 = new Point(0, 0);
			_sel3 = new Point(0, 0);
			_sel4 = new Point(0, 0);
		}

		[Browsable(false)]
		[DefaultValue(4)]
		public int HalfHeight
		{
			get { return HHeight; }
			set { HHeight = value; HWidth = 2 * value; }
		}

		public void ParentSize(int width, int height)
		{
			if (map != null)
			{
				int oldWid = HWidth;

			    if (map.MapSize.Rows > 0 || map.MapSize.Cols > 0)
			    {
			        if (height > width / 2)
			        {
			            //use width
			            HWidth = width / (map.MapSize.Rows + map.MapSize.Cols);

			            if (HWidth % 2 != 0)
			                HWidth--;

			            HHeight = HWidth / 2;
			        }
			        else
			        {
			            //use height
			            HHeight = height / (map.MapSize.Rows + map.MapSize.Cols);
			            HWidth = HHeight * 2;
			        }
			    }

			    if (HHeight < MinimunHeight)
				{
					HWidth = MinimunHeight * 2;
					HHeight = MinimunHeight;
				}

				_offX = 4 + map.MapSize.Rows * HWidth;
				_offY = 4;

				if (oldWid != HWidth)
				{
					Width = 8 + (map.MapSize.Rows + map.MapSize.Cols) * HWidth;
					Height = 8 + (map.MapSize.Rows + map.MapSize.Cols) * HHeight;
					Refresh();
				}
			}
		}

		[Browsable(false)]
		[DefaultValue(null)]
		public override IMap_Base Map
		{
			set
			{
				map = value;
				HWidth = 7;
				ParentSize(Parent.Width, Parent.Height);
				Refresh();
			}
		}

        protected void ViewDrag(object sender, MouseEventArgs ex)
		{
            var s = GetDragStart();
            var e = GetDragEnd();

            //                 col hei
			_sel1.X = _offX + (s.X - s.Y) * HWidth;
			_sel1.Y = _offY + (s.X + s.Y) * HHeight;

			_sel2.X = _offX + (e.X - s.Y) * HWidth + HWidth;
			_sel2.Y = _offY + (e.X + s.Y) * HHeight + HHeight;

			_sel3.X = _offX + (e.X - e.Y) * HWidth;
			_sel3.Y = _offY + (e.X + e.Y) * HHeight + HHeight + HHeight;

			_sel4.X = _offX + (s.X - e.Y) * HWidth - HWidth;
			_sel4.Y = _offY + (s.X + e.Y) * HHeight + HHeight;

			_copyArea.Reset();
			_copyArea.AddLine(_sel1, _sel2);
			_copyArea.AddLine(_sel2, _sel3);
			_copyArea.AddLine(_sel3, _sel4);
			_copyArea.CloseFigure();

			Refresh();
		}

	    private static Point GetDragEnd()
	    {
	        var e = new Point(0, 0);
	        e.X = Math.Max(MapViewPanel.Instance.View.StartDrag.X, MapViewPanel.Instance.View.EndDrag.X);
	        e.Y = Math.Max(MapViewPanel.Instance.View.StartDrag.Y, MapViewPanel.Instance.View.EndDrag.Y);
	        return e;
	    }

	    private static Point GetDragStart()
	    {
	        var s = new Point(0, 0);
	        s.X = Math.Min(MapViewPanel.Instance.View.StartDrag.X, MapViewPanel.Instance.View.EndDrag.X);
	        s.Y = Math.Min(MapViewPanel.Instance.View.StartDrag.Y, MapViewPanel.Instance.View.EndDrag.Y);
	        return s;
	    }

	    [Browsable(false), DefaultValue(null)]
	    public Dictionary<string, SolidBrush> Brushes { get; set; }

	    [Browsable(false), DefaultValue(null)]
	    public Dictionary<string, Pen> Pens { get; set; }

	    public override void SelectedTileChanged(IMap_Base sender, SelectedTileChangedEventArgs e)
		{
			MapLocation pt = e.MapLocation;

			Text = "r: " + pt.Row + " c: " + pt.Col;

			int xc = (pt.Col - pt.Row) * HWidth;
			int yc = (pt.Col + pt.Row) * HHeight;

			_selected.Reset();
			_selected.AddLine(xc, yc, xc + HWidth, yc + HHeight);
			_selected.AddLine(xc + HWidth, yc + HHeight, xc, yc + 2 * HHeight);
			_selected.AddLine(xc, yc + 2 * HHeight, xc - HWidth, yc + HHeight);
			_selected.CloseFigure();

			ViewDrag(null, null);
			Refresh();
		}

		protected override void OnMouseWheel(MouseEventArgs e)
		{
			base.OnMouseWheel(e);
			if (e.Delta > 0)
				map.Up();
			else
				map.Down();
		}

		private void convertCoordsDiamond(int x, int y, out int row, out int col)
		{
			//int x = xp - offX; //16 is half the width of the diamond
			//int y = yp - offY; //24 is the distance from the top of the diamond to the very top of the image

			double x1 = (x * 1.0 / (2 * HWidth)) + (y * 1.0 / (2 * HHeight));
			double x2 = -(x * 1.0 - 2 * y * 1.0) / (2 * HWidth);

			row = (int)Math.Floor(x2);
			col = (int)Math.Floor(x1);
			//return new Point((int)Math.Floor(x1), (int)Math.Floor(x2));
		}

		protected virtual void RenderCell(MapTileBase tile, Graphics g, int x, int y) { }

		protected GraphicsPath UpperPath(int x, int y)
		{
			_upper.Reset();
			_upper.AddLine(x, y, x + HWidth, y + HHeight);
			_upper.AddLine(x + HWidth, y + HHeight, x - HWidth, y + HHeight);
			_upper.CloseFigure();
			return _upper;
		}

		protected GraphicsPath LowerPath(int x, int y)
		{
			_lower.Reset();
			_lower.AddLine(x, y + 2 * HHeight, x + HWidth, y + HHeight);
			_lower.AddLine(x + HWidth, y + HHeight, x - HWidth, y + HHeight);
			_lower.CloseFigure();
			return _lower;
		}

		protected GraphicsPath CellPath(int xc, int yc)
		{
			_cell.Reset();
			_cell.AddLine(xc, yc, xc + HWidth, yc + HHeight);
			_cell.AddLine(xc + HWidth, yc + HHeight, xc, yc + 2 * HHeight);
			_cell.AddLine(xc, yc + 2 * HHeight, xc - HWidth, yc + HHeight);
			_cell.CloseFigure();
			return _cell;
		}

		protected override void Render(Graphics g)
		{
			g.FillRectangle(SystemBrushes.Control, ClientRectangle);

			if (map != null)
			{
				for (int row = 0, startX = _offX, startY = _offY; row < map.MapSize.Rows; row++, startX -= HWidth, startY += HHeight)
				{
					for (int col = 0, x = startX, y = startY; col < map.MapSize.Cols; col++, x += HWidth, y += HHeight)
					{
						MapTileBase mapTile = map[row, col];

						if (mapTile != null)
							RenderCell(mapTile, g, x, y);
					}
				}

				for (int i = 0; i <= map.MapSize.Rows; i++)
					g.DrawLine(Pens["GridColor"], _offX - i * HWidth, _offY + i * HHeight, ((map.MapSize.Cols - i) * HWidth) + _offX, ((i + map.MapSize.Cols) * HHeight) + _offY);
				for (int i = 0; i <= map.MapSize.Cols; i++)
					g.DrawLine(Pens["GridColor"], _offX + i * HWidth, _offY + i * HHeight, (i * HWidth) - map.MapSize.Rows * HWidth + _offX, (i * HHeight) + map.MapSize.Rows * HHeight + _offY);

				if (_copyArea != null)
					g.DrawPath(Pens["SelectColor"], _copyArea);

				//				if(selected!=null) //clicked on
				//					g.DrawPath(new Pen(Brushes.Blue,2),selected);

				if (_mR < map.MapSize.Rows && _mC < map.MapSize.Cols && _mR >= 0 && _mC >= 0)
				{
					int xc = (_mC - _mR) * HWidth + _offX;
					int yc = (_mC + _mR) * HHeight + _offY;

					GraphicsPath selPath = CellPath(xc, yc);
					g.DrawPath(Pens["MouseColor"], selPath);
				}
			}
		}

		protected override void OnMouseDown(MouseEventArgs e)
		{
			int row, col;
		    if (map == null) return;
			convertCoordsDiamond(e.X - _offX, e.Y - _offY,out row, out col);
			map.SelectedTile = new MapLocation(row,col, map.CurrentHeight);
			mDown = true;

			Point p = new Point(col, row);

			MapViewPanel.Instance.View.StartDrag = p;
			MapViewPanel.Instance.View.EndDrag = p;
		}

		private bool mDown = false;
		protected override void OnMouseUp(MouseEventArgs e)
		{
			mDown = false;
			MapViewPanel.Instance.View.Refresh();
			Refresh();
		}

		protected override void OnMouseMove(MouseEventArgs e)
		{
			int row, col;
			convertCoordsDiamond(e.X - _offX, e.Y - _offY,out row, out col);
			if (row != _mR || col != _mC)
			{
				_mR = row;
				_mC = col;

				if (mDown)
				{
					MapViewPanel.Instance.View.EndDrag = new Point(col,row);
					MapViewPanel.Instance.View.Refresh();
				    if (e.Button == MouseButtons.Left)
				    {
				        ViewDrag(null, e);
				    }
				}
				Refresh();
			}
		}
	}
}
