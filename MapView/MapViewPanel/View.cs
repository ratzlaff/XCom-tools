using System;
using System.Windows.Forms;
using System.Drawing;
using System.Drawing.Drawing2D;
using MapView.Forms.MainWindow;
using XCom;
using XCom.Interfaces;
using System.Collections;
using XCom.Interfaces.Base;
using MapView.TopViewForm;

namespace MapView
{
    public class View : Panel
    {
        private IMap_Base map;
        private Point _origin = new Point(100, 0);

        private Cursor cursor;

        private Size viewable;

        private const int H_WIDTH = 16;
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

        public event MouseEventHandler DragChanged;

        public View()
        {
            map = null;
            _dragStart = _dragEnd = new Point(-1, -1);

            SetStyle(ControlStyles.DoubleBuffer | ControlStyles.UserPaint | ControlStyles.AllPaintingInWmPaint,true);

            gridColor = Color.FromArgb(175, 69, 100, 129);
            transBrush = new SolidBrush(gridColor);

            dashPen = new Pen(Brushes.Black, 1);
        }

        public void Paste()
        {
            if (map == null) return;
            if (copied != null)
            {
                // row  col 
                //  y    x  
                var dragStart = DragStart;
                for (int r = dragStart.Y; r < map.MapSize.Rows && (r - dragStart.Y) < copied.GetLength(0); r++)
                    for (int c = dragStart.X; c < map.MapSize.Cols && (c - dragStart.X) < copied.GetLength(1); c++)
                    {
                        var tile = map[r, c] as XCMapTile;
                        if (tile == null) continue;
                        var copyTile = copied[r - dragStart.Y, c - dragStart.X] as XCMapTile;
                        if (copyTile == null) continue;
                        tile.Ground = copyTile.Ground;
                        tile.Content = copyTile.Content;
                        tile.West = copyTile.West;
                        tile.North = copyTile.North;
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
            if (map == null) return;
            var start = GetDragStart();
            var end = GetDragEnd();
             
            for (int c = start.X; c <= end.X; c++)
                for (int r = start.Y; r <= end.Y; r++)
                    map[r, c] = XCMapTile.BlankTile;
            map.MapChanged = true;
            Refresh();
        }

        public void Copy()
        {
            if (map == null) return;
            var start = GetDragStart();
            var end = GetDragEnd();

            // row  col 
            // y     x  

            copied = new MapTileBase[end.Y - start.Y + 1, end.X - start.X + 1];

            for (int c = start.X; c <= end.X; c++)
                for (int r = start.Y; r <= end.Y; r++)
                    copied[r - start.Y, c - start.X] = map[r, c];
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
            get { return (int) dashPen.Width; }
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

        public MapView.Cursor Cursor
        {
            get { return cursor; }
            set
            {
                cursor = value;
                Refresh();
            }
        }

        public void OnResize()
        {
            OnResize(null);
        }

        protected override void OnMouseDown(MouseEventArgs e)
        {
            if (map != null)
            {
                DragStart = DragEnd = ConvertCoordsDiamond(e.X, e.Y, map.CurrentHeight);
             
                if (DragChanged != null)
                    DragChanged(null, null);

                map.SelectedTile = new MapLocation(DragEnd.Y, DragEnd.X, map.CurrentHeight);
                
                Focus();
                Refresh();
            }
        }

        protected override void OnMouseWheel(MouseEventArgs e)
        {
            if (e.Delta > 0)
                map.Up();
            else if (e.Delta < 0)
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
                Point temp = ConvertCoordsDiamond(e.X, e.Y, map.CurrentHeight);

                if (temp.X != DragEnd.X || temp.Y != DragEnd.Y)
                {
                    if (e.Button != MouseButtons.None)
                    {
                        DragEnd = temp;
                        if (DragChanged != null)
                            DragChanged(this, e);
                    }

                    Refresh();
                }
            }
        }

        public Point DragStart
        {
            get { return _dragStart; }
            set
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
            set
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

                map = value;
                if (map != null)
                {
                    _origin = new Point((map.MapSize.Rows - 1) * H_WIDTH * Globals.PckImageScale, 0);
                    map.HeightChanged += MapHeight;
                    map.SelectedTileChanged += TileChange;
                    Width = (map.MapSize.Rows + map.MapSize.Cols) * H_WIDTH * Globals.PckImageScale;
                    Height = map.MapSize.Height * 25 * Globals.PckImageScale +
                             (map.MapSize.Rows + map.MapSize.Cols) * H_HEIGHT * Globals.PckImageScale;
                    DragStart = DragStart;
                    DragEnd = DragEnd;
                }
            }
        }

        public Size Viewable
        {
            get { return viewable; }
            set { viewable = value; }
        }

        private void TileChange(IMap_Base mapFile, SelectedTileChangedEventArgs e) // MapLocation newCoords)
        {
            MapLocation newCoords = e.MapPosition;
            DragStart = new Point(newCoords.Col, newCoords.Row);
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
                dragRect.Width += 1;
                dragRect.Height += 1;

                var insideDragRect = dragRect;
                insideDragRect.X += 1;
                insideDragRect.Y += 1;
                insideDragRect.Width -= 2;
                insideDragRect.Height -= 2;

                for (var h = map.MapSize.Height - 1; h >= 0; h--)
                {
                    if (h < map.CurrentHeight) continue;

                    DrawGrid(h, g);

                    for (int row = 0, startX = _origin.X, startY = _origin.Y + (24 * h * Globals.PckImageScale);
                        row < map.MapSize.Rows;
                        row++, startX -= H_WIDTH * Globals.PckImageScale, startY += H_HEIGHT * Globals.PckImageScale)
                    {
                        for (int col = 0, x = startX, y = startY;
                            col < map.MapSize.Cols;
                            col++, x += H_WIDTH * Globals.PckImageScale, y += H_HEIGHT * Globals.PckImageScale)
                        {
                            var isClickedLocation = IsDragEndOrStart(row, col);
                            var tileRect = new Rectangle(col, row, 1, 1);

                            if (isClickedLocation)
                            {
                                DrawCursor(g, x, y, h);
                            }

                            if (h == map.CurrentHeight || map[row, col, h].DrawAbove)
                            {
                                var tile = (XCMapTile) map[row, col, h];
                                if (!selectGrayscale)
                                {
                                    drawTile(g, tile, x, y);
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
                                    drawTile(g, tile, x, y);
                                }
                            }

                            if (isClickedLocation && cursor != null)
                                cursor.DrawLow(g, x, y, MapViewPanel.Current, false, map.CurrentHeight == h);
                        }
                    }
                }
                DrawSelection(g, map.CurrentHeight, dragRect);
            }
        }

        private void DrawCursor( Graphics g, int x, int y, int h)
        {
            if (cursor != null)
            {
                cursor.DrawHigh(g, x, y, false, map.CurrentHeight == h);
            }
        }

        private bool IsDragEndOrStart(int row, int col)
        {
            bool here = (
                row == DragEnd.Y && col == DragEnd.X ||
                row == DragStart.Y && col == DragStart.X);
            return here;
        }

        private void DrawGrid(int h, Graphics g)
        {
            if (h == map.CurrentHeight && useGrid)
            {
                underGrid = new GraphicsPath();
                var pt0 = new Point(_origin.X + H_WIDTH, _origin.Y + (map.CurrentHeight + 1) * 24);
                var pt1 = new Point(_origin.X + map.MapSize.Cols * H_WIDTH + H_WIDTH,
                    _origin.Y + map.MapSize.Cols * H_HEIGHT + (map.CurrentHeight + 1) * 24);
                var pt2 = new Point(_origin.X + H_WIDTH + (map.MapSize.Cols - map.MapSize.Rows) * H_WIDTH,
                    _origin.Y + (map.MapSize.Rows + map.MapSize.Cols) * H_HEIGHT + (map.CurrentHeight + 1) * 24);
                var pt3 = new Point(_origin.X - map.MapSize.Rows * H_WIDTH + H_WIDTH,
                    _origin.Y + map.MapSize.Rows * H_HEIGHT + (map.CurrentHeight + 1) * 24);
                underGrid.AddLine(pt0, pt1);
                underGrid.AddLine(pt1, pt2);
                underGrid.AddLine(pt2, pt3);
                underGrid.CloseFigure();

                g.FillPath(transBrush, underGrid);

                for (int i = 0; i <= map.MapSize.Rows; i++)
                    g.DrawLine(dashPen, _origin.X - i * H_WIDTH + H_WIDTH,
                        _origin.Y + i * H_HEIGHT + (map.CurrentHeight + 1) * 24,
                        _origin.X + ((map.MapSize.Cols - i) * H_WIDTH) + H_WIDTH,
                        _origin.Y + (map.CurrentHeight + 1) * 24 + ((i + map.MapSize.Cols) * H_HEIGHT));
                for (int i = 0; i <= map.MapSize.Cols; i++)
                    g.DrawLine(dashPen, _origin.X + i * H_WIDTH + H_WIDTH,
                        _origin.Y + i * H_HEIGHT + (map.CurrentHeight + 1) * 24,
                        (_origin.X + i * H_WIDTH + H_WIDTH) - map.MapSize.Rows * H_WIDTH,
                        (_origin.Y + i * H_HEIGHT) + map.MapSize.Rows * H_HEIGHT + (map.CurrentHeight + 1) * 24);
            }
        } 

        private void drawTile(Graphics g, XCMapTile mt, int x, int y)
        {
            var topView = MainWindowsManager.TopView;
            if (mt.Ground != null && topView.GroundVisible)
                g.DrawImage(mt.Ground[MapViewPanel.Current].Image, x, y - mt.Ground.Info.TileOffset);

            if (mt.North != null && topView.NorthVisible)
                g.DrawImage(mt.North[MapViewPanel.Current].Image, x, y - mt.North.Info.TileOffset);

            if (mt.West != null && topView.WestVisible)
                g.DrawImage(mt.West[MapViewPanel.Current].Image, x, y - mt.West.Info.TileOffset);

            if (mt.Content != null && topView.ContentVisible)
                g.DrawImage(mt.Content[MapViewPanel.Current].Image, x, y - mt.Content.Info.TileOffset);
        }

        private void DrawTileGray(Graphics g, XCMapTile mt, int x, int y)
        {
            var topView = MainWindowsManager.TopView;
            if (mt.Ground != null && topView.GroundVisible)
                g.DrawImage(mt.Ground[MapViewPanel.Current].Gray, x, y - mt.Ground.Info.TileOffset);

            if (mt.North != null && topView.NorthVisible)
                g.DrawImage(mt.North[MapViewPanel.Current].Gray, x, y - mt.North.Info.TileOffset);

            if (mt.West != null && topView.WestVisible)
                g.DrawImage(mt.West[MapViewPanel.Current].Gray, x, y - mt.West.Info.TileOffset);

            if (mt.Content != null && topView.ContentVisible)
                g.DrawImage(mt.Content[MapViewPanel.Current].Gray, x, y - mt.Content.Info.TileOffset);
        }

        private void DrawSelection(Graphics g, int h, Rectangle dragRect)
        {
            var top = ConvertCoordsRect(new Point(dragRect.X, dragRect.Y),h+1);
            var right = ConvertCoordsRect(new Point(dragRect.Right, dragRect.Y),h+1);
            var bottom = ConvertCoordsRect(new Point(dragRect.Right, dragRect.Bottom),h+1);
            var left = ConvertCoordsRect(new Point(dragRect.Left, dragRect.Bottom),h+1);
            top.X += H_WIDTH;
            right.X += H_WIDTH;
            bottom.X += H_WIDTH;
            left.X += H_WIDTH;
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
            int x = xp - (_origin.X) - (H_WIDTH * Globals.PckImageScale); //16 is half the width of the diamond
            int y = yp - (_origin.Y) - (24 * Globals.PckImageScale) * (level + 1);
                //24 is the distance from the top of the diamond to the very top of the image

            double x1 = (x * 1.0 / (2 * (H_WIDTH * Globals.PckImageScale))) +
                        (y * 1.0 / (2 * (H_HEIGHT * Globals.PckImageScale)));
            double x2 = -(x * 1.0 - 2 * y * 1.0) / (2 * (H_WIDTH * Globals.PckImageScale));

            return new Point((int) Math.Floor(x1), (int) Math.Floor(x2));
        }

        private Point ConvertCoordsRect(Point p, int h)
        {
            int x = p.X;
            int y = p.Y;
            var heightAdjust = (H_HEIGHT * 3 * h);
            return new Point(
                _origin.X + (H_WIDTH * (x - y)),
                (_origin.Y + (H_HEIGHT * (x + y))) + heightAdjust);
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
