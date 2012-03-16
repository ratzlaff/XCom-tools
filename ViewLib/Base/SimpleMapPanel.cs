using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Forms;
using System.Drawing.Drawing2D;
using System.Drawing;
using System.ComponentModel;
using MapLib.Base;
using MapLib;
using UtilLib;

namespace ViewLib.Base
{
	public class SimpleMapPanel : Map_Observer_Control
	{
		protected int offX = 0, offY = 0;
		protected int hWidth = 8, hHeight = 4;
		protected int minHeight = 4;

		protected GraphicsPath upper, lower, cell, copyArea, selected;

		protected Point sel1, sel2, sel3, sel4;
		protected int mR, mC, selR, selC;

		protected UtilLib.ValueChangedDelegate diamondHeightChanged;

		protected Rectangle borderRect;

		protected Panel scrollPanel;

		public SimpleMapPanel()
		{
			upper = new GraphicsPath();
			lower = new GraphicsPath();
			cell = new GraphicsPath();
			selected = new GraphicsPath();
			copyArea = new GraphicsPath();

			sel1 = new Point(0, 0);
			sel2 = new Point(0, 0);
			sel3 = new Point(0, 0);
			sel4 = new Point(0, 0);

			diamondHeightChanged = diamondHeight;
		}

		[Browsable(false)]
		[DefaultValue(null)]
		public Panel ScrollPanel
		{
			get { return scrollPanel; }
			set
			{
				scrollPanel = value;
				scrollPanel.SizeChanged += new EventHandler(scrollPanel_SizeChanged);
			}
		}

		private void calcSize(int width, int height)
		{
			int rows = 10, cols = 10;

			if (map != null) {
				rows = map.Size.Rows;
				cols = map.Size.Cols;
			}

			int oldWid = hWidth, oldHei = hHeight;

			if (height > width / 2) {
				//use width
				hWidth = width / (rows + cols);

				if (hWidth % 2 != 0)
					hWidth--;

				hHeight = hWidth / 2;
			} else { //use height
				hHeight = height / (rows + cols);
				hWidth = hHeight * 2;
			}

			if (hHeight < minHeight) {
				hWidth = minHeight * 2;
				hHeight = minHeight;
			}

			if (!IsDesignMode) {
				if (oldWid != hWidth || oldHei != hHeight) {
					Width = 8 + (rows + cols) * hWidth;
					Height = 8 + (rows + cols) * hHeight;
					Refresh();
				}

				offX = 4 + rows * hWidth;
				offY = 4;

				Location = new Point((width - Width) / 2, (height - Height) / 2);
			} else {
				offX = Width / 2;
				offY = (Height - ((rows + cols) * hHeight)) / 2;
			}
		}

		void scrollPanel_SizeChanged(object sender, EventArgs e)
		{
			calcSize(scrollPanel.Width, scrollPanel.Height);
		}

		[DefaultValue(4)]
		public int MinHeight
		{
			get { return minHeight; }
			set
			{
				minHeight = value;
				OnResize(null);
			}
		}

		protected override void OnResize(EventArgs e)
		{
			base.OnResize(e);
			if (scrollPanel != null)
				calcSize(scrollPanel.Width, scrollPanel.Height);

			sizeSelection();

			if (IsDesignMode) {
				calcSize(ClientSize.Width, ClientSize.Height);
				borderRect = ClientRectangle;
				borderRect.Inflate(-1, -1);
			}
		}

		private void sizeSelection()
		{
			Point s = new Point(0, 0);
			Point e = new Point(0, 0);

			s.X = Math.Min(MapControl.StartDrag.Col, MapControl.EndDrag.Col);
			s.Y = Math.Min(MapControl.StartDrag.Row, MapControl.EndDrag.Row);

			e.X = Math.Max(MapControl.StartDrag.Col, MapControl.EndDrag.Col);
			e.Y = Math.Max(MapControl.StartDrag.Row, MapControl.EndDrag.Row);

			//                 col hei
			sel1.X = offX + (s.X - s.Y) * hWidth;
			sel1.Y = offY + (s.X + s.Y) * hHeight;

			sel2.X = offX + (e.X - s.Y) * hWidth + hWidth;
			sel2.Y = offY + (e.X + s.Y) * hHeight + hHeight;

			sel3.X = offX + (e.X - e.Y) * hWidth;
			sel3.Y = offY + (e.X + e.Y) * hHeight + hHeight + hHeight;

			sel4.X = offX + (s.X - e.Y) * hWidth - hWidth;
			sel4.Y = offY + (s.X + e.Y) * hHeight + hHeight;

			copyArea.Reset();
			copyArea.AddLine(sel1, sel2);
			copyArea.AddLine(sel2, sel3);
			copyArea.AddLine(sel3, sel4);
			copyArea.CloseFigure();
		}

		protected void viewDrag(object sender, EventArgs ex)
		{
			sizeSelection();
			Refresh();
		}

		public override void SelectedTileChanged(Map sender, SelectedTileChangedEventArgs e)
		{
			MapLocation pt = e.MapLocation;

			Text = "r: " + pt.Row + " c: " + pt.Col;

			int xc = (pt.Col - pt.Row) * hWidth;
			int yc = (pt.Col + pt.Row) * hHeight;

			selected.Reset();
			selected.AddLine(xc, yc, xc + hWidth, yc + hHeight);
			selected.AddLine(xc + hWidth, yc + hHeight, xc, yc + 2 * hHeight);
			selected.AddLine(xc, yc + 2 * hHeight, xc - hWidth, yc + hHeight);
			selected.CloseFigure();

			viewDrag(null, null);
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

			double x1 = (x * 1.0 / (2 * hWidth)) + (y * 1.0 / (2 * hHeight));
			double x2 = -(x * 1.0 - 2 * y * 1.0) / (2 * hWidth);

			row = (int)Math.Floor(x2);
			col = (int)Math.Floor(x1);
			//return new Point((int)Math.Floor(x1), (int)Math.Floor(x2));
		}

		/// <summary>
		/// Get the tile contained at (x,y) in local screen coordinates
		/// </summary>
		/// <param name="x"></param>
		/// <param name="y"></param>
		/// <returns>null if (x,y) is an invalid location for a tile</returns>
		public MapTile GetTile(int x, int y)
		{
			int row, col;
			convertCoordsDiamond(x, y, out row, out col);
			if (row >= 0 && row < map.Size.Rows &&
				col >= 0 && col < map.Size.Cols)
				return map[row, col];
			return null;
		}

		protected virtual void RenderCell(System.Drawing.Graphics g, int x, int y, int row, int col) { }

		protected GraphicsPath UpperPath(int x, int y)
		{
			upper.Reset();
			upper.AddLine(x, y, x + hWidth, y + hHeight);
			upper.AddLine(x + hWidth, y + hHeight, x - hWidth, y + hHeight);
			upper.CloseFigure();
			return upper;
		}

		protected GraphicsPath LowerPath(int x, int y)
		{
			lower.Reset();
			lower.AddLine(x, y + 2 * hHeight, x + hWidth, y + hHeight);
			lower.AddLine(x + hWidth, y + hHeight, x - hWidth, y + hHeight);
			lower.CloseFigure();
			return lower;
		}

		protected GraphicsPath CellPath(int x, int y)
		{
			cell.Reset();
			cell.AddLine(x, y, x + hWidth, y + hHeight);
			cell.AddLine(x + hWidth, y + hHeight, x, y + 2 * hHeight);
			cell.AddLine(x, y + 2 * hHeight, x - hWidth, y + hHeight);
			cell.CloseFigure();
			return cell;
		}

		protected override void OnPaint(PaintEventArgs e)
		{
			base.OnPaint(e);
			Graphics g = e.Graphics;

			if (IsDesignMode) {
				for (int i = 0; i <= 10; i++)
					g.DrawLine(System.Drawing.Pens.Black, offX - i * hWidth, offY + i * hHeight, ((10 - i) * hWidth) + offX, ((i + 10) * hHeight) + offY);
				for (int i = 0; i <= 10; i++)
					g.DrawLine(System.Drawing.Pens.Black, offX + i * hWidth, offY + i * hHeight, (i * hWidth) - 10 * hWidth + offX, (i * hHeight) + 10 * hHeight + offY);

				//				g.DrawLine(System.Drawing.Pens.Black, offX - (10 * hWidth), offY, offX + (10 * hWidth), offY);
				//				g.DrawLine(System.Drawing.Pens.Black, offX - (10 * hWidth), offY + (20 * hHeight), offX + (10 * hWidth), offY + (20 * hHeight));

				ControlPaint.DrawBorder3D(g, borderRect, Border3DStyle.Etched);
			} else {
				if (map != null) {
					for (int row = 0, startX = offX, startY = offY; row < map.Size.Rows; row++, startX -= hWidth, startY += hHeight)
						for (int col = 0, x = startX, y = startY; col < map.Size.Cols; col++, x += hWidth, y += hHeight)
							RenderCell(g, x, y, row, col);

					for (int i = 0; i <= map.Size.Rows; i++)
						g.DrawLine(pens["GridColor"], offX - i * hWidth, offY + i * hHeight, ((map.Size.Cols - i) * hWidth) + offX, ((i + map.Size.Cols) * hHeight) + offY);
					for (int i = 0; i <= map.Size.Cols; i++)
						g.DrawLine(pens["GridColor"], offX + i * hWidth, offY + i * hHeight, (i * hWidth) - map.Size.Rows * hWidth + offX, (i * hHeight) + map.Size.Rows * hHeight + offY);

					if (copyArea != null)
						g.DrawPath(pens["SelectColor"], copyArea);

					if (mR < map.Size.Rows && mC < map.Size.Cols && mR >= 0 && mC >= 0) {
						int xc = (mC - mR) * hWidth + offX;
						int yc = (mC + mR) * hHeight + offY;

						GraphicsPath selPath = CellPath(xc, yc);
						g.DrawPath(pens["MouseColor"], selPath);
					}
				}
			}
		}

		protected override void OnMouseDown(MouseEventArgs e)
		{
			convertCoordsDiamond(e.X - offX, e.Y - offY, out selR, out selC);
			map.SelectedTile = new MapLocation(selR, selC, map.CurrentHeight);
			mDown = true;
		}

		private bool mDown = false;
		protected override void OnMouseUp(MouseEventArgs e)
		{
			mDown = false;
			MapControl.RequestRefresh();
		}

		protected override void OnMouseMove(MouseEventArgs e)
		{
			int row, col;
			convertCoordsDiamond(e.X - offX, e.Y - offY, out row, out col);
			if (row != mR || col != mC) {
				mR = row;
				mC = col;

				if (mDown) {
					MapControl.EndDrag = new MapLocation(row, col, map.CurrentHeight);
					viewDrag(null, null);
					MapControl.RequestRefresh();
				} else
					Refresh();
			}
		}

		#region Settings control
		private void diamondHeight(object sender, string keyword, object val)
		{
			MinHeight = (int)val;
		}

		public override void SetupDefaultSettings(Map_Observer_Form sender)
		{
			base.SetupDefaultSettings(sender);
			// GridColor, GridWidth
			addPenSetting(new Pen(new SolidBrush(Color.Black), 1), "Grid", "Grid", "Color of the grid lines", "Width of the grid lines", sender.Settings);

			//SelectColor, SelectWidth
			addPenSetting(new Pen(new SolidBrush(Color.Black), 2), "Select", "Select", "Color of the selection line", "Width of the selection line in pixels", sender.Settings);

			// MouseColor, MouseWidth
			addPenSetting(new Pen(new SolidBrush(Color.Blue), 2), "Mouse", "Grid", "Color of the mouse-over indicator", "Width of the mouse-over indicatior", sender.Settings);
		}
		#endregion
	}
}
