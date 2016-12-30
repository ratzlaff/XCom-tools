using System;
using System.Windows.Forms;
using System.Drawing;
using System.Drawing.Drawing2D;
using MapView.Forms.MainWindow;
using XCom;
using XCom.Interfaces;
using System.Collections;
using XCom.Interfaces.Base;

namespace MapView
{
	public class MapView
		:
		Panel
	{
		private IMap_Base map;
		private Point _origin = new Point(100, 0);

		private CursorSprite cursor;

		private Size viewable;

		private const int H_WIDTH  = 16;
		private const int H_HEIGHT = 8;

		private Point _dragStart;
		private Point _dragEnd;
		private Pen dashPen;
		private bool selectGrayscale = true;

		private GraphicsPath underGrid;
		private Brush transBrush;
		private Color gridColor;
		private bool useGrid = true;
		private MapTileBase[,] copied;
		private bool _drawSelectionBox;

		public bool DrawSelectionBox
		{
			get { return _drawSelectionBox; }
			set
			{
				_drawSelectionBox = value;
				Refresh();
			}
		}

		public event EventHandler DragChanged;

		public MapView()
		{
			map = null;

			_dragStart =
			_dragEnd   = new Point(-1, -1);

			SetStyle(
					ControlStyles.DoubleBuffer | ControlStyles.UserPaint | ControlStyles.AllPaintingInWmPaint,
					true);

			gridColor = Color.FromArgb(175, 69, 100, 129);
			transBrush = new SolidBrush(gridColor);

			dashPen = new Pen(Brushes.Black, 1);
		}

		public void Paste()
		{
			if (map != null && copied != null)
			{
				// row col
				// y   x
				var dragStart = DragStart;
				for (int r = dragStart.Y; r < map.MapSize.Rows && (r - dragStart.Y) < copied.GetLength(0); r++)
					for (int c = dragStart.X; c < map.MapSize.Cols && (c - dragStart.X) < copied.GetLength(1); c++)
					{
						var tile = map[r, c] as XCMapTile;
						if (tile != null)
						{
							var copyTile = copied[r - dragStart.Y, c - dragStart.X] as XCMapTile;
							if (copyTile != null)
							{
								tile.Ground		= copyTile.Ground;
								tile.Content	= copyTile.Content;
								tile.West		= copyTile.West;
								tile.North		= copyTile.North;
							}
						}
					}

				map.MapChanged = true;
				Refresh();
			}
		}

		public bool SelectGrayscale
		{
			get { return selectGrayscale; }
			set
			{
				selectGrayscale = value;
				Refresh();
			}
		}

		public void ClearSelection()
		{
			if (map != null)
			{
				var start = GetDragStart();
				var end = GetDragEnd();
	
				for (int c = start.X; c <= end.X; c++)
					for (int r = start.Y; r <= end.Y; r++)
						map[r, c] = XCMapTile.BlankTile;

				map.MapChanged = true;
				Refresh();
			}
		}

		public void Copy()
		{
			if (map != null)
			{
				var start = GetDragStart();
				var end = GetDragEnd();

				// row col
				// y   x
				copied = new MapTileBase[end.Y - start.Y + 1, end.X - start.X + 1];
	
				for (int c = start.X; c <= end.X; c++)
					for (int r = start.Y; r <= end.Y; r++)
						copied[r - start.Y, c - start.X] = map[r, c];
			}
		}

		public Color GridColor
		{
			get { return gridColor; }
			set
			{
				gridColor = value;
				transBrush = new SolidBrush(value);
				Refresh();
			}
		}

		public Color GridLineColor
		{
			get { return dashPen.Color; }
			set
			{
				dashPen.Color = value;
				Refresh();
			}
		}

		public int GridLineWidth
		{
			get { return (int)dashPen.Width; }
			set
			{
				dashPen.Width = value;
				Refresh();
			}
		}

		public bool UseGrid
		{
			get { return useGrid; }
			set
			{
				useGrid = value;
				Refresh();
			}
		}

		public global::MapView.CursorSprite CursorSprite
		{
			get { return cursor; }
			set
			{
				cursor = value;
				Refresh();
			}
		}

		protected override void OnMouseDown(MouseEventArgs e)
		{
			if (map != null)
			{
				var dragStart = ConvertCoordsDiamond(
												e.X, e.Y,
												map.CurrentHeight);
				var dragEnd = ConvertCoordsDiamond(
												e.X, e.Y,
												map.CurrentHeight);
				SetDrag(dragStart, dragEnd);

				map.SelectedTile = new MapLocation(
												DragEnd.Y, DragEnd.X,
												map.CurrentHeight);

				Focus();
				Refresh();
			}
		}

		protected override void OnMouseWheel(MouseEventArgs e)
		{
			if (e.Delta < 0)
				map.Up();
			else if (e.Delta > 0)
				map.Down();
		}

		protected override void OnMouseUp(MouseEventArgs e)
		{
			Refresh();
		}

		protected override void OnMouseMove(MouseEventArgs e)
		{
			if (map != null)
			{
				Point temp = ConvertCoordsDiamond(
											e.X, e.Y,
											map.CurrentHeight);

				if (temp.X != DragEnd.X || temp.Y != DragEnd.Y)
				{
					if (e.Button != MouseButtons.None)
						SetDrag(DragStart, temp);

					Refresh();
				}
			}
		}

		public Point DragStart
		{
			get { return _dragStart; }
			private set
			{
				_dragStart = value;
				if (_dragStart.Y < 0)
					_dragStart.Y = 0;
				else if (_dragStart.Y >= map.MapSize.Rows)
					_dragStart.Y = map.MapSize.Rows - 1;

				if (_dragStart.X < 0)
					_dragStart.X = 0;
				else if (_dragStart.X >= map.MapSize.Cols)
					_dragStart.X = map.MapSize.Cols - 1;
			}
		}

		public Point DragEnd
		{
			get { return _dragEnd; }
			private set
			{
				_dragEnd = value;
				if (_dragEnd.Y < 0)
					_dragEnd.Y = 0;
				else if (_dragEnd.Y >= map.MapSize.Rows)
					_dragEnd.Y = map.MapSize.Rows - 1;

				if (_dragEnd.X < 0)
					_dragEnd.X = 0;
				else if (_dragEnd.X >= map.MapSize.Cols)
					_dragEnd.X = map.MapSize.Cols - 1;
			}
		}

		public void SetDrag(Point dragStart, Point dragEnd)
		{
			if (DragEnd != dragEnd || DragStart != dragStart)
			{
				DragStart = dragStart;
				DragEnd = dragEnd;
	
				if (DragChanged != null)
					DragChanged(this, EventArgs.Empty);

				Refresh();
			}
		}

		public IMap_Base Map
		{
			get { return map; }
			set
			{
				if (map != null)
				{
					map.HeightChanged -= MapHeight;
					map.SelectedTileChanged -= TileChange;
				}

				if ((map = value) != null)
				{
					map.HeightChanged += MapHeight;
					map.SelectedTileChanged += TileChange;
					SetupMapSize();

					DragStart = DragStart; // Calculate drag
					DragEnd = DragEnd;
				}
			}
		}

		public void SetupMapSize()
		{
			if (map != null)
			{
				var size = GetMapSize(Globals.PckImageScale);
				Width = size.Width;
				Height = size.Height;
			}
		}

		public Size GetMapSize(double pckImageScale)
		{
			if (map != null)
			{
				var halfWidth  = (int)(H_WIDTH  * pckImageScale);
				var halfHeight = (int)(H_HEIGHT * pckImageScale);

				_origin = new Point((map.MapSize.Rows - 1) * halfWidth, 0);

				var width = (map.MapSize.Rows + map.MapSize.Cols) * halfWidth;
				var height = map.MapSize.Height * (halfHeight * 3) +
							(map.MapSize.Rows + map.MapSize.Cols) * halfHeight;
				return new Size(width, height);
			}
			return Size.Empty;
		}

		public Size Viewable
		{
			get { return viewable; }
			set { viewable = value; }
		}

		private void TileChange(IMap_Base mapFile, SelectedTileChangedEventArgs e) // MapLocation newCoords)
		{
			MapLocation newCoords = e.MapPosition;
			var dragStart = new Point(newCoords.Col, newCoords.Row);
			SetDrag(dragStart, DragEnd);
		}

		private void MapHeight(IMap_Base mapFile, HeightChangedEventArgs e)
		{
			Refresh();
		}

		protected override void OnPaint(PaintEventArgs e)
		{
			base.OnPaint(e);

			if (map != null)
			{
				var g = e.Graphics;

				var dragMin = new Point(Math.Min(DragStart.X, DragEnd.X), Math.Min(DragStart.Y, DragEnd.Y));
				var dragMax = new Point(Math.Max(DragStart.X, DragEnd.X), Math.Max(DragStart.Y, DragEnd.Y));

				var dragRect = new Rectangle(dragMin, new Size(Point.Subtract(dragMax, new Size(dragMin))));
				dragRect.Width  += 1;
				dragRect.Height += 1;

				var insideDragRect = dragRect;
				insideDragRect.X += 1;
				insideDragRect.Y += 1;
				insideDragRect.Width  -= 2;
				insideDragRect.Height -= 2;

				var halfWidth  = (int)(H_WIDTH  * Globals.PckImageScale);
				var halfHeight = (int)(H_HEIGHT * Globals.PckImageScale);
				for (var h = map.MapSize.Height - 1; h > -1; h--)
				{
					if (h >= map.CurrentHeight)
					{
						DrawGrid(h, g);
	
						var startY = _origin.Y + (halfHeight * 3 * h);
						var startX = _origin.X;
						for (var row = 0; row < map.MapSize.Rows; row++)
						{
							var x = startX;
							var y = startY;
							for (var col = 0; col < map.MapSize.Cols; col++)
							{
								var isClickedLocation = IsDragEndOrStart(row, col);
								var tileRect = new Rectangle(col, row, 1, 1);
	
								if (isClickedLocation)
									DrawCursor(g, x, y, h);
	
								if (h == map.CurrentHeight || map[row, col, h].DrawAbove)
								{
									var tile = (XCMapTile) map[row, col, h];
									if (!selectGrayscale)
									{
										DrawTile(g, tile, x, y);
									}
									else if (isClickedLocation)
									{
										DrawTileGray(g, tile, x, y);
									}
									else if (dragRect.IntersectsWith(tileRect))
									{
										DrawTileGray(g, tile, x, y);
									}
									else
									{
										DrawTile(g, tile, x, y);
									}
								}
	
								if (isClickedLocation && cursor != null)
									cursor.DrawLow(g, x, y, MapViewPanel.Current, false, map.CurrentHeight == h);
	
								x += halfWidth;
								y += halfHeight;
							}
	
							startY += halfHeight;
							startX -= halfWidth;
						}
					}
				}

				if (DrawSelectionBox)
					DrawSelection(g, map.CurrentHeight, dragRect);
			}
		}

		private void DrawCursor(Graphics g, int x, int y, int h)
		{
			if (cursor != null)
				cursor.DrawHigh(g, x, y, false, map.CurrentHeight == h);
		}

		private bool IsDragEndOrStart(int row, int col)
		{
			return (row == DragEnd.Y   && col == DragEnd.X)
				|| (row == DragStart.Y && col == DragStart.X);
		}

		private void DrawGrid(int h, Graphics g)
		{
			if (h == map.CurrentHeight && useGrid)
			{
				var hWidth  = (int)(H_WIDTH  * Globals.PckImageScale);
				var hHeight = (int)(H_HEIGHT * Globals.PckImageScale);

				var x = hWidth + _origin.X;
				var y = ((map.CurrentHeight + 1) * (hHeight * 3)) + _origin.Y;

				var xMax = map.MapSize.Rows * hWidth;
				var yMax = map.MapSize.Rows * hHeight;

				underGrid = new GraphicsPath();

				var pt0 = new Point(x, y);
				var pt1 = new Point(
								x + map.MapSize.Cols * hWidth,
								y + map.MapSize.Cols * hHeight);
				var pt2 = new Point(
								x + (map.MapSize.Cols - map.MapSize.Rows) * hWidth,
								y + (map.MapSize.Rows + map.MapSize.Cols) * hHeight);
				var pt3 = new Point(x - xMax, yMax + y);

				underGrid.AddLine(pt0, pt1);
				underGrid.AddLine(pt1, pt2);
				underGrid.AddLine(pt2, pt3);
				underGrid.CloseFigure();

				g.FillPath(transBrush, underGrid);

				for (var i = 0; i <= map.MapSize.Rows; i++)
					g.DrawLine(
							dashPen,
							x - (i * hWidth),
							y + (i * hHeight),
							x + ((map.MapSize.Cols - i) * hWidth),
							y + ((map.MapSize.Cols + i) * hHeight));

				for (int i = 0; i <= map.MapSize.Cols; i++)
					g.DrawLine(
							dashPen,
							x + i * hWidth,
							y + i * hHeight,
							(i * hWidth)  - xMax + x,
							(i * hHeight) + yMax + y);
			}
		}

		private static void DrawTile(Graphics g, XCMapTile mt, int x, int y)
		{
			var topView = MainWindowsManager.TopView.TopViewControl;
			if (mt.Ground != null && topView.GroundVisible)
				DrawTile(g, x, y, mt.Ground);

			if (mt.North != null && topView.NorthVisible)
				DrawTile(g, x, y, mt.North);

			if (mt.West != null && topView.WestVisible)
				DrawTile(g, x, y, mt.West);

			if (mt.Content != null && topView.ContentVisible)
				DrawTile(g, x, y, mt.Content);
		}

		private void DrawTileGray(Graphics g, XCMapTile mt, int x, int y)
		{
			var topView = MainWindowsManager.TopView.TopViewControl;
			if (mt.Ground != null && topView.GroundVisible)
				DrawTileGray(g, x, y, mt.Ground);

			if (mt.North != null && topView.NorthVisible)
				DrawTileGray(g, x, y, mt.North);

			if (mt.West != null && topView.WestVisible)
				DrawTileGray(g, x, y, mt.West);

			if (mt.Content != null && topView.ContentVisible)
				DrawTileGray(g, x, y, mt.Content);
		}

		private static void DrawTile(Graphics g, int x, int y, TileBase tile)
		{
			Bitmap image = tile[MapViewPanel.Current].Image;
			DrawTile(g, x, y, tile, image);
		}

		private static void DrawTileGray(Graphics g, int x, int y, TileBase tile)
		{
			Bitmap image = tile[MapViewPanel.Current].Gray;
			DrawTile(g, x, y, tile, image);
		}

		private static void DrawTile(Graphics g, int x, int y, TileBase tile, Bitmap image)
		{
			g.DrawImage(
					image,
					x,
					y - tile.Info.TileOffset,
					(int)(image.Width  * Globals.PckImageScale),
					(int)(image.Height * Globals.PckImageScale));
		}


		private void DrawSelection(Graphics g, int h, Rectangle dragRect)
		{
			var hWidth = (int)(H_WIDTH * Globals.PckImageScale);

			var top		= ConvertCoordsRect(new Point(dragRect.X,     dragRect.Y), h + 1);
			var right	= ConvertCoordsRect(new Point(dragRect.Right, dragRect.Y), h + 1);
			var bottom	= ConvertCoordsRect(new Point(dragRect.Right, dragRect.Bottom), h + 1);
			var left	= ConvertCoordsRect(new Point(dragRect.Left,  dragRect.Bottom), h + 1);

			top.X    += hWidth;
			right.X  += hWidth;
			bottom.X += hWidth;
			left.X   += hWidth;

			var pen = new Pen(Color.FromArgb(70, Color.Red));
			pen.Width = 3;

			g.DrawLine(pen, top, right);
			g.DrawLine(pen, right, bottom);
			g.DrawLine(pen, bottom, left);
			g.DrawLine(pen, left, top);
		}

		/// <summary>
		/// convert from screen coordinates to tile coordinates
		/// </summary>
		/// <param name="xp"></param>
		/// <param name="yp"></param>
		/// <param name="level"></param>
		/// <returns></returns>
		private Point ConvertCoordsDiamond(int xp, int yp, int level)
		{
			// 16 is half the width of the diamond
			// 24 is the distance from the top of the diamond to the very top of the image
			var halfWidth  = H_WIDTH  * Globals.PckImageScale;
			var halfHeight = H_HEIGHT * Globals.PckImageScale;
			var x = (int)(xp - (_origin.X) - halfWidth);
			var y = (int)(yp - (_origin.Y) - (halfHeight * 3) * (level + 1));

			var x1 = (x * 1.0 / (2 * halfWidth)) +
					 (y * 1.0 / (2 * halfHeight));
			var x2 = -(x * 1.0 - 2 * y * 1.0) / (2 * halfWidth);

			return new Point(
						(int)Math.Floor(x1),
						(int)Math.Floor(x2));
		}

		private Point ConvertCoordsRect(Point p, int h)
		{
			var hWidth  = (int)(H_WIDTH  * Globals.PckImageScale);
			var hHeight = (int)(H_HEIGHT * Globals.PckImageScale);
			int x = p.X;
			int y = p.Y;
			var heightAdjust = (hHeight * 3 * h);
			return new Point(
						 _origin.X + (hWidth  * (x - y)),
						(_origin.Y + (hHeight * (x + y))) + heightAdjust);
		}

		private Point GetDragEnd()
		{
			var end = new Point();
			end.X = Math.Max(DragStart.X, DragEnd.X);
			end.Y = Math.Max(DragStart.Y, DragEnd.Y);
			return end;
		}

		private Point GetDragStart()
		{
			var start = new Point();
			start.X = Math.Max(Math.Min(DragStart.X, DragEnd.X), 0);
			start.Y = Math.Max(Math.Min(DragStart.Y, DragEnd.Y), 0);
			return start;
		}
	}
}
