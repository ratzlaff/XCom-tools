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
using ViewLib;

namespace MapView
{
	public class MapViewPanel : SimpleMapPanel
	{
		private Point origin = new Point(100, 0);
		private int currentImage;

		private MapView.Cursor cursor;

		private Size viewable;
		private bool newLeft;
		private Point topLeft;

		private static int hdWid = 16, hdHeight = 8;

		private bool drawAll = true;
		private bool[] draw = { false, false, false, false };
		private bool[] vis = { false, false, false, false };
		private bool flipLock = false, flipLock2 = false;

		private bool mDown;
		private MapLocation selectedLoc, mouseOver;
		private MapLocation startDrag, endDrag;
		private Pen dashPen;
		private bool selectGrayscale = true;

		private GraphicsPath underGrid;
		private Brush transBrush;
		private Color gridColor;
		private bool useGrid = true;

		public MapViewPanel()
		{
			map = null;
			currentImage = 0;
			mouseOver = new MapLocation(-1, -1);
			startDrag = new MapLocation(-1, -1);
			endDrag = new MapLocation(-1, -1);
			selectedLoc = new MapLocation(-1, -1);
			topLeft = new Point(0, 0);

			newLeft = true;

			gridColor = Color.FromArgb(175, 69, 100, 129);
			transBrush = new SolidBrush(gridColor);

			dashPen = new Pen(Brushes.Black, 1);
/*
			try {
				cursor = new Cursor(GameInfo.CachePck(SharedSpace.Instance.GetString("cursorFile"), "", 4, XCPalette.TFTDBattle));
			} catch {
				try {
					cursor = new Cursor(GameInfo.CachePck(SharedSpace.Instance.GetString("cursorFile"), "", 2, XCPalette.UFOBattle));
				} catch { cursor = null; }
			}
*/
			xConsole.AddLine("Cursor loaded");
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

		public override void SetupDefaultSettings(Map_Observer_Form sender)
		{
			base.SetupDefaultSettings(sender);

			sender.Settings.AddSetting("UseGrid", "If true, a grid will show up at the current level of editing", "MapView", "UseGrid", this);
			sender.Settings.AddSetting("GridColor", "Color of the grid in (a,r,g,b) format", "MapView", "GridColor", this);
			sender.Settings.AddSetting("GridLineColor", "Color of the lines that make up the grid", "MapView", "GridLineColor", this);
			sender.Settings.AddSetting("GridLineWidth", "Width of the grid lines in pixels", "MapView", "GridLineWidth", this);
			sender.Settings.AddSetting("SelectGrayscale", "If true, the selection area will show up in gray", "MapView", "SelectGrayscale", this);
		}
		#endregion
		/*
		public new MapView.Cursor Cursor
		{
			get { return cursor; }
			set { cursor = value; Refresh(); }
		}*/

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
				selectedLoc = convertCoordsDiamond(e.X, e.Y, map.CurrentHeight);
				StartDrag = EndDrag = selectedLoc;
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
				if (map != null) {
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
				origin = new Point((map.Size.Rows - 1) * hdWid * Globals.PckImageScale, 0);
				Width = (map.Size.Rows + map.Size.Cols) * hdWid * Globals.PckImageScale;
				Height = map.Size.Height * 25 * Globals.PckImageScale + (map.Size.Rows + map.Size.Cols) * hdHeight * Globals.PckImageScale;
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


		public override void SelectedTileChanged(Map sender, SelectedTileChangedEventArgs e)
		{
			selectedLoc = e.MapLocation;

			StartDrag = MapControl.StartDrag;
			EndDrag = MapControl.EndDrag;

			flipLock = true;
			if (!drawAll && !flipLock2)
				map[selectedLoc.Row, selectedLoc.Col, map.CurrentHeight].DrawAbove = !map[selectedLoc.Row, selectedLoc.Col, map.CurrentHeight].DrawAbove;

			flipLock = false;
			base.SelectedTileChanged(sender, e);
		}

		private int topx, topy, wid, hei, bottomx, bottomy;

		protected override void OnPaint(PaintEventArgs e)
		{
			if(IsDesignMode)
				base.OnPaint(e);
			else if (map != null) {
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
						Point pt0 = new Point(origin.X + hdWid, origin.Y + (map.CurrentHeight + 1) * 24);
						Point pt1 = new Point(origin.X + map.Size.Cols * hdWid + hdWid, origin.Y + map.Size.Cols * hdHeight + (map.CurrentHeight + 1) * 24);
						Point pt2 = new Point(origin.X + hdWid + (map.Size.Cols - map.Size.Rows) * hdWid, origin.Y + (map.Size.Rows + map.Size.Cols) * hdHeight + (map.CurrentHeight + 1) * 24);
						Point pt3 = new Point(origin.X - map.Size.Rows * hdWid + hdWid, origin.Y + map.Size.Rows * hdHeight + (map.CurrentHeight + 1) * 24);
						underGrid.AddLine(pt0, pt1);
						underGrid.AddLine(pt1, pt2);
						underGrid.AddLine(pt2, pt3);
						underGrid.CloseFigure();

						g.FillPath(transBrush, underGrid);

						for (int i = 0; i <= map.Size.Rows; i++)
							g.DrawLine(dashPen, origin.X - i * hdWid + hdWid, origin.Y + i * hdHeight + (map.CurrentHeight + 1) * 24,
								origin.X + ((map.Size.Cols - i) * hdWid) + hdWid, origin.Y + (map.CurrentHeight + 1) * 24 + ((i + map.Size.Cols) * hdHeight));
						for (int i = 0; i <= map.Size.Cols; i++)
							g.DrawLine(dashPen, origin.X + i * hdWid + hdWid, origin.Y + i * hdHeight + (map.CurrentHeight + 1) * 24,
								(origin.X + i * hdWid + hdWid) - map.Size.Rows * hdWid, (origin.Y + i * hdHeight) + map.Size.Rows * hdHeight + (map.CurrentHeight + 1) * 24);
					}

					for (int row = 0, startX = origin.X, startY = origin.Y + (24 * h * Globals.PckImageScale); row < map.Size.Rows; row++, startX -= hdWid * Globals.PckImageScale, startY += hdHeight * Globals.PckImageScale) {
						for (int col = 0, x = startX, y = startY; col < map.Size.Cols; col++, x += hdWid * Globals.PckImageScale, y += hdHeight * Globals.PckImageScale) {
							if (x > bottomx || y > bottomy)
								break;

							bool here = false;
							if (row == mouseOver.Row && col == mouseOver.Col || row == selectedLoc.Row && col == selectedLoc.Col) {
								if (cursor != null)
									cursor.DrawHigh(g, x, y, MapViewScroller.Current, false, map.CurrentHeight == h);
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
								cursor.DrawLow(g, x, y, MapViewScroller.Current, false, map.CurrentHeight == h);
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
				g.DrawImage(mt.Ground[MapViewScroller.Current].Image, x, y - mt.Ground.YOffset);

			if (mt.North != null && TopView.Instance.NorthVisible)
				g.DrawImage(mt.North[MapViewScroller.Current].Image, x, y - mt.North.YOffset);

			if (mt.West != null && TopView.Instance.WestVisible)
				g.DrawImage(mt.West[MapViewScroller.Current].Image, x, y - mt.West.YOffset);

			if (mt.Content != null && TopView.Instance.ContentVisible)
				g.DrawImage(mt.Content[MapViewScroller.Current].Image, x, y - mt.Content.YOffset);
		}

		private void drawTileGray(Graphics g, XCMapTile mt, int x, int y)
		{
			if (mt.Ground != null && TopView.Instance.GroundVisible)
				g.DrawImage(mt.Ground[MapViewScroller.Current].Gray, x, y - mt.Ground.YOffset);

			if (mt.North != null && TopView.Instance.NorthVisible)
				g.DrawImage(mt.North[MapViewScroller.Current].Gray, x, y - mt.North.YOffset);

			if (mt.West != null && TopView.Instance.WestVisible)
				g.DrawImage(mt.West[MapViewScroller.Current].Gray, x, y - mt.West.YOffset);

			if (mt.Content != null && TopView.Instance.ContentVisible)
				g.DrawImage(mt.Content[MapViewScroller.Current].Gray, x, y - mt.Content.YOffset);
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
			int x = xp - origin.X - (hdWid * Globals.PckImageScale); //16 is half the width of the diamond
			int y = yp - origin.Y - (24 * Globals.PckImageScale) * (level + 1); //24 is the distance from the top of the diamond to the very top of the image

			double x1 = (x * 1.0 / (2 * (hdWid * Globals.PckImageScale))) + (y * 1.0 / (2 * (hdHeight * Globals.PckImageScale)));
			double x2 = -(x * 1.0 - 2 * y * 1.0) / (2 * (hdWid * Globals.PckImageScale));

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

			return new Point(origin.X + ((PckImage.Width / 2) * (x - y)), origin.Y + (x + y));
		}
	}
}
