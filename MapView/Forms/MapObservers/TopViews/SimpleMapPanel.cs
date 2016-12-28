using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Windows.Forms;
using MapView.Forms.MapObservers.RmpViews;
using XCom;
using XCom.Interfaces.Base;

namespace MapView.Forms.MapObservers.TopViews
{
	public class SimpleMapPanel
		:
		Map_Observer_Control
	{
		private int _offX = 0;
		private int _offY = 0;

		protected int MinimunHeight = 4;

		private readonly GraphicsPath _cell;
		private readonly GraphicsPath _copyArea;
		private readonly GraphicsPath _selected;

		private int _mR;
		private int _mC;

		protected DrawContentService DrawContentService = new DrawContentService() ;

		public SimpleMapPanel()
		{
			_cell = new GraphicsPath();
			_selected = new GraphicsPath();
			_copyArea = new GraphicsPath();
		}

		public void ParentSize(int width, int height)
		{
			if (map != null)
			{
				int oldWid = DrawContentService.HWidth;
				var hWidth = DrawContentService.HWidth;
				var hHeight = DrawContentService.HHeight;
				if (map.MapSize.Rows > 0 || map.MapSize.Cols > 0)
				{
					if (height > width / 2) // use width
					{
						hWidth = width / (map.MapSize.Rows + map.MapSize.Cols);

						if (hWidth % 2 != 0)
							hWidth--;

						hHeight = hWidth / 2;
					}
					else // use height
					{
						hHeight = height / (map.MapSize.Rows + map.MapSize.Cols);
						hWidth = hHeight * 2;
					}
				}

				if (hHeight < MinimunHeight)
				{
					hWidth = MinimunHeight * 2;
					hHeight = MinimunHeight;
				}

				DrawContentService.HWidth = hWidth;
				DrawContentService.HHeight = hHeight;

				_offX = 4 + map.MapSize.Rows * hWidth;
				_offY = 4;

				if (oldWid != hWidth)
				{
					Width  = 8 + (map.MapSize.Rows + map.MapSize.Cols) * hWidth;
					Height = 8 + (map.MapSize.Rows + map.MapSize.Cols) * hHeight;
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
				DrawContentService.HWidth = 7;
				ParentSize(Parent.Width, Parent.Height);
				Refresh();
			}
		}

		protected override void OnResize(EventArgs e)
		{
			base.OnResize(e);
			SetSelectionRect();
		}

		protected void ViewDrag(object sender, EventArgs ex)
		{
			SetSelectionRect();
		}

		private void SetSelectionRect()
		{
			var s = GetDragStart();
			var e = GetDragEnd();

			var hWidth  = DrawContentService.HWidth;
			var hHeight = DrawContentService.HHeight;
			var sel1 = new Point(
							_offX + (s.X - s.Y) * hWidth,
							_offY + (s.X + s.Y) * hHeight);

			var sel2 = new Point(
							_offX + (e.X - s.Y) * hWidth + hWidth,
							_offY + (e.X + s.Y) * hHeight + hHeight);

			var sel3 = new Point(
							_offX + (e.X - e.Y) * hWidth,
							_offY + (e.X + e.Y) * hHeight + hHeight + hHeight);

			var sel4 = new Point(
							_offX + (s.X - e.Y) * hWidth - hWidth,
							_offY + (s.X + e.Y) * hHeight + hHeight);

			_copyArea.Reset();
			_copyArea.AddLine(sel1, sel2);
			_copyArea.AddLine(sel2, sel3);
			_copyArea.AddLine(sel3, sel4);
			_copyArea.CloseFigure();

			Refresh();
		}

		private static Point GetDragEnd()
		{
			var e = new Point(0, 0);
			e.X = Math.Max(
						MapViewPanel.Instance.MapView.DragStart.X,
						MapViewPanel.Instance.MapView.DragEnd.X);
			e.Y = Math.Max(
						MapViewPanel.Instance.MapView.DragStart.Y,
						MapViewPanel.Instance.MapView.DragEnd.Y);
			return e;
		}

		private static Point GetDragStart()
		{
			var s = new Point(0, 0);
			s.X = Math.Min(
						MapViewPanel.Instance.MapView.DragStart.X,
						MapViewPanel.Instance.MapView.DragEnd.X);
			s.Y = Math.Min(
						MapViewPanel.Instance.MapView.DragStart.Y,
						MapViewPanel.Instance.MapView.DragEnd.Y);
			return s;
		}

		[Browsable(false), DefaultValue(null)]
		public Dictionary<string, SolidBrush> Brushes { get; set; }

		[Browsable(false), DefaultValue(null)]
		public Dictionary<string, Pen> Pens { get; set; }

		public override void SelectedTileChanged(IMap_Base sender, SelectedTileChangedEventArgs e)
		{
			MapLocation pt = e.MapPosition;

			Text = "r: " + pt.Row + " c: " + pt.Col;

			var hWidth = DrawContentService.HWidth;
			var hHeight = DrawContentService.HHeight;

			int xc = (pt.Col - pt.Row) * hWidth;
			int yc = (pt.Col + pt.Row) * hHeight;

			_selected.Reset();
			_selected.AddLine(
							xc, yc,
							xc + hWidth, yc + hHeight);
			_selected.AddLine(
							xc + hWidth, yc + hHeight,
							xc, yc + 2 * hHeight);
			_selected.AddLine(
							xc, yc + 2 * hHeight,
							xc - hWidth, yc + hHeight);
			_selected.CloseFigure();

			ViewDrag(null, null);
			Refresh();
		}

		protected override void OnMouseWheel(MouseEventArgs e)
		{
			base.OnMouseWheel(e);
			if (e.Delta < 0)
				map.Up();
			else
				map.Down();
		}

		private Point ConvertCoordsDiamond(int x, int y)
		{
//			int x = xp - offX; // 16 is half the width of the diamond
//			int y = yp - offY; // 24 is the distance from the top of the diamond to the very top of the image

			var hWidth = DrawContentService.HWidth;
			var hHeight = DrawContentService.HHeight;

			double x1 = (x * 1.0 / (2 * hWidth)) + (y * 1.0 / (2 * hHeight));
			double x2 = -(x * 1.0 - 2 * y * 1.0) / (2 * hWidth);

			return new Point(
						(int)Math.Floor(x1),
						(int)Math.Floor(x2));
		}

		protected virtual void RenderCell(
										MapTileBase tile,
										Graphics g,
										int x, int y)
		{}

		protected GraphicsPath CellPath(int xc, int yc)
		{
			var hWidth = DrawContentService.HWidth;
			var hHeight = DrawContentService.HHeight ;

			_cell.Reset();
			_cell.AddLine(
						xc, yc,
						xc + hWidth, yc + hHeight);
			_cell.AddLine(
						xc + hWidth, yc + hHeight,
						xc, yc + 2 * hHeight);
			_cell.AddLine(
						xc, yc + 2 * hHeight,
						xc - hWidth, yc + hHeight);
			_cell.CloseFigure();
			return _cell;
		}

		protected override void Render(Graphics g)
		{
			g.FillRectangle(SystemBrushes.Control, ClientRectangle);

			var hWidth = DrawContentService.HWidth;
			var hHeight = DrawContentService.HHeight;

			if (map != null)
			{
				for (int row = 0, startX = _offX, startY = _offY;
						row < map.MapSize.Rows;
						row++, startX -= hWidth, startY += hHeight)
				{
					for (int col = 0, x = startX, y = startY;
							col < map.MapSize.Cols;
							col++, x += hWidth, y += hHeight)
					{
						MapTileBase mapTile = map[row, col];

						if (mapTile != null)
							RenderCell(mapTile, g, x, y);
					}
				}

				for (int i = 0; i <= map.MapSize.Rows; i++)
					g.DrawLine(
							Pens["GridColor"],
							_offX - i * hWidth,
							_offY + i * hHeight,
							((map.MapSize.Cols - i) * hWidth)  + _offX,
							((map.MapSize.Cols + i) * hHeight) + _offY);

				for (int i = 0; i <= map.MapSize.Cols; i++)
					g.DrawLine(
							Pens["GridColor"],
							_offX + i * hWidth,
							_offY + i * hHeight,
							(i * hWidth)  - map.MapSize.Rows * hWidth  + _offX,
							(i * hHeight) + map.MapSize.Rows * hHeight + _offY);

				if (_copyArea != null)
					g.DrawPath(Pens["SelectColor"], _copyArea);

//				if(selected!=null) //clicked on
//					g.DrawPath(new Pen(Brushes.Blue,2),selected);

				if (   _mR < map.MapSize.Rows
					&& _mC < map.MapSize.Cols
					&& _mR >= 0
					&& _mC >= 0)
				{
					int xc = (_mC - _mR) * hWidth + _offX;
					int yc = (_mC + _mR) * hHeight + _offY;

					GraphicsPath selPath = CellPath(xc, yc);
					g.DrawPath(Pens["MouseColor"], selPath);
				}
			}
		}

		protected override void OnMouseDown(MouseEventArgs e)
		{
			if (map == null) return;
			var p = ConvertCoordsDiamond(e.X - _offX, e.Y - _offY );
			map.SelectedTile = new MapLocation(p.Y, p.X, map.CurrentHeight);
			_mDown = true;

			MapViewPanel.Instance.MapView.SetDrag(p, p);
		}

		private bool _mDown = false;

		protected override void OnMouseUp(MouseEventArgs e)
		{
			_mDown = false;
			MapViewPanel.Instance.MapView.Refresh();
			Refresh();
		}

		protected override void OnMouseMove(MouseEventArgs e)
		{
			var p = ConvertCoordsDiamond(e.X - _offX, e.Y - _offY );
			if (p.Y != _mR || p.X != _mC)
			{
				_mR = p.Y;
				_mC = p.X;

				if (_mDown)
					MapViewPanel.Instance.MapView.SetDrag(
													MapViewPanel.Instance.MapView.DragStart,
													p);
				Refresh();
			}
		}
	}
}
