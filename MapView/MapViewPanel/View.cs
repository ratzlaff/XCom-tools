using System;
using System.Windows.Forms;
using System.Drawing;
using System.Drawing.Drawing2D;
using XCom;
using XCom.Interfaces;
using System.Collections;
using XCom.Interfaces.Base;
using MapView.TopViewForm;
using MapLib.Base;
using MapLib;

namespace MapView
{
	public class View : ViewLib.Base.Map_Observer_Control
	{
		private Point origin = new Point(100, 0);
		private int currentImage;

		private MapView.Cursor cursor;
		private int offX = 0, offY = 0;

		private Size viewable;
		private bool newLeft;
		private Point topLeft;

		private static int hWid = 16, hHeight = 8;

		private bool drawAll = true;
		private bool[] draw = { false, false, false, false };
		private bool[] vis = { false, false, false, false };
		private bool flipLock = false, flipLock2 = false;

		private bool mDown;
		private MapLocation selected, mouseOver;
		private MapLocation startDrag, endDrag;
		private Pen dashPen;
		private bool selectGrayscale = true;

		private GraphicsPath underGrid;
		private Brush transBrush;
		private Color gridColor;
		private bool useGrid = true;
		private MapTile[,] copied;

//		public event EventHandler DragChanged;

		public View()
		{
			map = null;
			currentImage = 0;
			mouseOver = new MapLocation(-1, -1);
			startDrag = new MapLocation(-1, -1);
			endDrag = new MapLocation(-1, -1);
			selected = new MapLocation(-1, -1);
			topLeft = new Point(0, 0);

			newLeft = true;

			gridColor = Color.FromArgb(175, 69, 100, 129);
			transBrush = new SolidBrush(gridColor);

			dashPen = new Pen(Brushes.Black, 1);

			MapControl.HeightChanged += mapHeight;
			MapControl.SelectedTileChanged += tileChange;
		}

		public void Paste()
		{
			if (copied != null) {
				//row  col
				//y    x

				for (int r = startDrag.Row; r < map.Size.Rows && (r - startDrag.Row) < copied.GetLength(0); r++)
					for (int c = startDrag.Col; c < map.Size.Cols && (c - startDrag.Col) < copied.GetLength(1); c++) {
						XCMapTile tile = map[r, c] as XCMapTile;
						XCMapTile copyTile = copied[r - startDrag.Row, c - startDrag.Col] as XCMapTile;
						tile.Ground = copyTile.Ground;
						tile.Content = copyTile.Content;
						tile.West = copyTile.West;
						tile.North = copyTile.North;
					}

				Globals.MapChanged = true;
				Refresh();
			}
		}

		public void ClearSelection()
		{
			MapLocation s = new MapLocation(0, 0);
			MapLocation e = new MapLocation(0, 0);

			s.Row = Math.Min(startDrag.Row, endDrag.Row);
			s.Col = Math.Min(startDrag.Col, endDrag.Col);

			e.Row = Math.Max(startDrag.Row, endDrag.Row);
			e.Col = Math.Max(startDrag.Col, endDrag.Col);

			for (int c = s.Col; c <= e.Col; c++)
				for (int r = s.Row; r <= e.Row; r++)
					map[r, c] = XCMapTile.BlankTile;
			Globals.MapChanged = true;
			Refresh();
		}

		public void Copy()
		{
			MapLocation s = new MapLocation(0, 0);
			MapLocation e = new MapLocation(0, 0);

			s.Row = Math.Min(startDrag.Row, endDrag.Row);
			s.Col = Math.Min(startDrag.Col, endDrag.Col);

			e.Row = Math.Max(startDrag.Row, endDrag.Row);
			e.Col = Math.Max(startDrag.Col, endDrag.Col);

			//row  col
			//y    x
			copied = new XCMapTile[e.Row - s.Row + 1, e.Col - s.Col + 1];

			for (int c = s.Col; c <= e.Col; c++)
				for (int r = s.Row; r <= e.Row; r++)
					copied[r - s.Row, c - s.Col] = map[r, c];
		}

		#region Settings
		public bool SelectGrayscale
		{
			get { return selectGrayscale; }
			set { selectGrayscale = value; Refresh(); }
		}

		public Color GridColor
		{
			get { return gridColor; }
			set { gridColor = value; transBrush = new SolidBrush(value); Refresh(); }
		}

		public Color GridLineColor
		{
			get { return dashPen.Color; }
			set { dashPen.Color = value; Refresh(); }
		}

		public int GridLineWidth
		{
			get { return (int)dashPen.Width; }
			set { dashPen.Width = value; Refresh(); }
		}

		public bool UseGrid
		{
			get { return useGrid; }
			set { useGrid = value; Refresh(); }
		}

		public override void SetupDefaultSettings(ViewLib.Base.Map_Observer_Form sender)
		{
			base.SetupDefaultSettings(sender);

			sender.Settings.AddSetting("UseGrid", "If true, a grid will show up at the current level of editing", "MapView", "UseGrid", this);
			sender.Settings.AddSetting("GridColor", "Color of the grid in (a,r,g,b) format", "MapView", "GridColor", this);
			sender.Settings.AddSetting("GridLineColor", "Color of the lines that make up the grid", "MapView", "GridLineColor", this);
			sender.Settings.AddSetting("GridLineWidth", "Width of the grid lines in pixels", "MapView", "GridLineWidth", this);
			sender.Settings.AddSetting("SelectGrayscale", "If true, the selection area will show up in gray", "MapView", "SelectGrayscale", this);
		}
		#endregion

		public new MapView.Cursor Cursor
		{
			get { return cursor; }
			set { cursor = value; Refresh(); }
		}

		public bool DrawAll
		{
			get { return drawAll; }
			set { drawAll = value; Refresh(); }
		}

		public bool[] Vis
		{
			get { return vis; }
		}

		public bool[] Draw
		{
			get { return draw; }
		}

		public new void Resize()
		{
			OnResize(null);
		}

		protected override void OnMouseDown(MouseEventArgs e)
		{
			if (map != null) {
				mDown = true;
				selected = convertCoordsDiamond(e.X, e.Y, map.CurrentHeight);
				StartDrag = EndDrag = selected;
				flipLock2 = true;
				if (!drawAll && !flipLock)
					map[mouseOver.Row, mouseOver.Col].DrawAbove = !map[mouseOver.Row, mouseOver.Col].DrawAbove;

				map.SelectedTile = new MapLocation(mouseOver.Row, mouseOver.Col, map.CurrentHeight);

				Focus();
				Refresh();
				flipLock2 = false;
			}
		}

		protected override void OnMouseUp(MouseEventArgs e)
		{
			mDown = false;
			Refresh();
		}

		protected override void OnMouseMove(MouseEventArgs e)
		{
			if (map != null) {
				MapLocation temp = convertCoordsDiamond(e.X, e.Y, map.CurrentHeight);

				if (temp.Row != mouseOver.Row || temp.Col != mouseOver.Col) {
					mouseOver = temp;

					if (mDown)
						MapControl.EndDrag = mouseOver;

					Refresh();
				}
			}
		}

		public MapLocation StartDrag
		{
			get { return startDrag; }
			set
			{
				startDrag = value;
				if (startDrag.Row < 0)
					startDrag.Row = 0;
				else if (startDrag.Row >= map.Size.Rows)
					startDrag.Row = map.Size.Rows - 1;

				if (startDrag.Col < 0)
					startDrag.Col = 0;
				else if (startDrag.Col >= map.Size.Cols)
					startDrag.Col = map.Size.Cols - 1;
			}
		}

		public MapLocation EndDrag
		{
			get { return endDrag; }
			set
			{
				endDrag = value;
				if (endDrag.Row < 0)
					endDrag.Row = 0;
				else if (endDrag.Row >= map.Size.Rows)
					endDrag.Row = map.Size.Rows - 1;

				if (endDrag.Col < 0)
					endDrag.Col = 0;
				else if (endDrag.Col >= map.Size.Cols)
					endDrag.Col = map.Size.Cols - 1;
			}
		}

		protected override void mapChanged(MapChangedEventArgs e)
		{
			base.mapChanged(e);

			if (map != null) {
				origin = new Point((map.Size.Rows - 1) * hWid * Globals.PckImageScale, 0);
				Width = (map.Size.Rows + map.Size.Cols) * hWid * Globals.PckImageScale;
				Height = map.Size.Height * 25 * Globals.PckImageScale + (map.Size.Rows + map.Size.Cols) * hHeight * Globals.PckImageScale;
			}
		}

		public int CurrentImage
		{
			set { currentImage = value; }
		}

		public Size Viewable
		{
			get { return viewable; }
			set { viewable = value; }
		}

		public bool NewLeft
		{
			set { newLeft = value; }
		}

		private void tileChange(Map mapFile, MapLib.SelectedTileChangedEventArgs e)// MapLocation newCoords)
		{
			selected = e.MapLocation;

			StartDrag = MapControl.StartDrag;
			EndDrag = MapControl.EndDrag;

			flipLock = true;
			if (!drawAll && !flipLock2)
				map[selected.Row, selected.Col, map.CurrentHeight].DrawAbove = !map[selected.Row, selected.Col, map.CurrentHeight].DrawAbove;

			flipLock = false;
		}

		private void mapHeight(Map mapFile, MapLib.HeightChangedEventArgs e)
		{
			Refresh();
		}

		private int topx, topy, wid, hei, bottomx, bottomy;

		protected override void OnPaint(PaintEventArgs e)
		{
			base.OnPaint(e);
			if (map != null) {
				Graphics g = e.Graphics;

				if (newLeft) {
					topx = -(Location.X + PckImage.Width);
					topy = -(Location.Y + PckImage.Height);
					wid = (-Location.X) + viewable.Width;
					hei = (-Location.Y) + viewable.Height + PckImage.Height;
					bottomx = topx + wid + PckImage.Width;
					bottomy = topy + hei + PckImage.Height;

					newLeft = false;
				}

				for (int h = map.Size.Height - 1; h >= 0; h--) {
					bool val = true;

					if (!drawAll)
						val = true;
					else {
						if (h >= map.CurrentHeight)
							val = true;
						else
							val = false;
					}

					if ((!drawAll && vis[h]) || !val)
						continue;

					if (h == map.CurrentHeight && useGrid) {
						underGrid = new GraphicsPath();
						Point pt0 = new Point(origin.X + hWid, origin.Y + (map.CurrentHeight + 1) * 24);
						Point pt1 = new Point(origin.X + map.Size.Cols * hWid + hWid, origin.Y + map.Size.Cols * hHeight + (map.CurrentHeight + 1) * 24);
						Point pt2 = new Point(origin.X + hWid + (map.Size.Cols - map.Size.Rows) * hWid, origin.Y + (map.Size.Rows + map.Size.Cols) * hHeight + (map.CurrentHeight + 1) * 24);
						Point pt3 = new Point(origin.X - map.Size.Rows * hWid + hWid, origin.Y + map.Size.Rows * hHeight + (map.CurrentHeight + 1) * 24);
						underGrid.AddLine(pt0, pt1);
						underGrid.AddLine(pt1, pt2);
						underGrid.AddLine(pt2, pt3);
						underGrid.CloseFigure();

						g.FillPath(transBrush, underGrid);

						for (int i = 0; i <= map.Size.Rows; i++)
							g.DrawLine(dashPen, origin.X - i * hWid + hWid, origin.Y + i * hHeight + (map.CurrentHeight + 1) * 24,
								origin.X + ((map.Size.Cols - i) * hWid) + hWid, origin.Y + (map.CurrentHeight + 1) * 24 + ((i + map.Size.Cols) * hHeight));
						for (int i = 0; i <= map.Size.Cols; i++)
							g.DrawLine(dashPen, origin.X + i * hWid + hWid, origin.Y + i * hHeight + (map.CurrentHeight + 1) * 24,
								(origin.X + i * hWid + hWid) - map.Size.Rows * hWid, (origin.Y + i * hHeight) + map.Size.Rows * hHeight + (map.CurrentHeight + 1) * 24);
					}

					for (int row = 0, startX = origin.X, startY = origin.Y + (24 * h * Globals.PckImageScale); row < map.Size.Rows; row++, startX -= hWid * Globals.PckImageScale, startY += hHeight * Globals.PckImageScale) {
						for (int col = 0, x = startX, y = startY; col < map.Size.Cols; col++, x += hWid * Globals.PckImageScale, y += hHeight * Globals.PckImageScale) {
							if (x > bottomx || y > bottomy)
								break;

							bool here = false;
							if (row == mouseOver.Row && col == mouseOver.Col || row == selected.Row && col == selected.Col) {
								if (cursor != null)
									cursor.DrawHigh(g, x, y, MapViewPanel.Current, false, map.CurrentHeight == h);
								here = true;
							}

							if (x > topx && x < wid && y > topy && y < hei) {
								if (!drawAll && draw[h]) {
									if (map[row, col, h].DrawAbove) {
										if (!selectGrayscale)
											drawTile(g, (XCMapTile)map[row, col, h], x, y);
										else if ((here && Globals.UseGray) || (((row >= startDrag.Row && row <= endDrag.Row) || (row >= startDrag.Row && row <= endDrag.Row)) &&
											((col >= startDrag.Col && col <= endDrag.Col) || (col >= startDrag.Col && col <= endDrag.Col))))
											drawTileGray(g, (XCMapTile)map[row, col, h], x, y);
										else
											drawTile(g, (XCMapTile)map[row, col, h], x, y);
									}
								} else if (h == map.CurrentHeight || map[row, col, h].DrawAbove) {
									if (!selectGrayscale)
										drawTile(g, (XCMapTile)map[row, col, h], x, y);
									else if ((here && Globals.UseGray))
										drawTileGray(g, (XCMapTile)map[row, col, h], x, y);
									else if (startDrag.Col >= endDrag.Col && col <= startDrag.Col && col >= endDrag.Col) {
										if (startDrag.Row >= endDrag.Row && row <= startDrag.Row && row >= endDrag.Row)
											drawTileGray(g, (XCMapTile)map[row, col, h], x, y);
										else if (startDrag.Row <= endDrag.Row && row >= startDrag.Row && row <= endDrag.Row)
											drawTileGray(g, (XCMapTile)map[row, col, h], x, y);
										else
											drawTile(g, (XCMapTile)map[row, col, h], x, y);

									} else if (startDrag.Col <= endDrag.Col && col >= startDrag.Col && col <= endDrag.Col) {
										if (startDrag.Row >= endDrag.Row && row <= startDrag.Row && row >= endDrag.Row)
											drawTileGray(g, (XCMapTile)map[row, col, h], x, y);
										else if (startDrag.Row <= endDrag.Row && row >= startDrag.Row && row <= endDrag.Row)
											drawTileGray(g, (XCMapTile)map[row, col, h], x, y);
										else
											drawTile(g, (XCMapTile)map[row, col, h], x, y);
									} else
										drawTile(g, (XCMapTile)map[row, col, h], x, y);
								}
							}

							if (here && cursor != null)
								cursor.DrawLow(g, x, y, MapViewPanel.Current, false, map.CurrentHeight == h);
						}
					}

				}
			}
		}

		public Point Origin
		{
			get { return origin; }
		}

		private void drawTile(Graphics g, XCMapTile mt, int x, int y)
		{
			if (mt.Ground != null && TopView.Instance.GroundVisible)
				g.DrawImage(mt.Ground[MapViewPanel.Current].Image, x, y - mt.Ground.YOffset);

			if (mt.North != null && TopView.Instance.NorthVisible)
				g.DrawImage(mt.North[MapViewPanel.Current].Image, x, y - mt.North.YOffset);

			if (mt.West != null && TopView.Instance.WestVisible)
				g.DrawImage(mt.West[MapViewPanel.Current].Image, x, y - mt.West.YOffset);

			if (mt.Content != null && TopView.Instance.ContentVisible)
				g.DrawImage(mt.Content[MapViewPanel.Current].Image, x, y - mt.Content.YOffset);
		}

		private void drawTileGray(Graphics g, XCMapTile mt, int x, int y)
		{
			if (mt.Ground != null && TopView.Instance.GroundVisible)
				g.DrawImage(mt.Ground[MapViewPanel.Current].Gray, x, y - mt.Ground.YOffset);

			if (mt.North != null && TopView.Instance.NorthVisible)
				g.DrawImage(mt.North[MapViewPanel.Current].Gray, x, y - mt.North.YOffset);

			if (mt.West != null && TopView.Instance.WestVisible)
				g.DrawImage(mt.West[MapViewPanel.Current].Gray, x, y - mt.West.YOffset);

			if (mt.Content != null && TopView.Instance.ContentVisible)
				g.DrawImage(mt.Content[MapViewPanel.Current].Gray, x, y - mt.Content.YOffset);
		}

		/// <summary>
		/// convert from screen coordinates to tile coordinates
		/// </summary>
		/// <param name="xp"></param>
		/// <param name="yp"></param>
		/// <param name="level"></param>
		/// <returns></returns>
		private MapLocation convertCoordsDiamond(int xp, int yp, int level)
		{
			int x = xp - (origin.X + offX) - (hWid * Globals.PckImageScale); //16 is half the width of the diamond
			int y = yp - (origin.Y + offY) - (24 * Globals.PckImageScale) * (level + 1); //24 is the distance from the top of the diamond to the very top of the image

			double x1 = (x * 1.0 / (2 * (hWid * Globals.PckImageScale))) + (y * 1.0 / (2 * (hHeight * Globals.PckImageScale)));
			double x2 = -(x * 1.0 - 2 * y * 1.0) / (2 * (hWid * Globals.PckImageScale));

			return new MapLocation((int)Math.Floor(x2), (int)Math.Floor(x1));
		}

		/// <summary>
		/// convert from map coordinates to rectangular coordinates
		/// </summary>
		/// <param name="p">the map coordinates in (column,row) form</param>
		/// <returns>(x,y) screen coordinates relative to this panel</returns>
		private Point ConvertCoordsRect(Point p)
		{
			int x = p.X;
			int y = p.Y;

			return new Point(origin.X + offX + ((PckImage.Width / 2) * (x - y)), origin.Y + offY + (x + y));
		}
	}
}
