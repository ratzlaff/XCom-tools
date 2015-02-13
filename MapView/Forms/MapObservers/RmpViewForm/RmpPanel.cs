using System;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Windows.Forms;
using XCom;

namespace MapView.RmpViewForm
{
    public class RmpPanel : MapPanel
    { 
        public Point Position = new Point(-1, -1);

        private readonly Font _font = new Font("Arial", 12, FontStyle.Bold);
         
        public void Calculate()
        {
            OnResize(null);
        }

        protected override void OnPaint(PaintEventArgs e)
        {
            var graphics = e.Graphics;
            try
            {
                if (Map == null) return;
                var lower = new GraphicsPath();
                var upper = new GraphicsPath();

                DrawWallsAndContent(lower, graphics);

                DrawUnselectedLink(upper, graphics);

                DrawSelectedLink(graphics);

                DrawNodes(upper, graphics);

                DrawGridLines(graphics);

                DrawPoles(graphics);

                DrawInformation(graphics);
            }
            catch (Exception ex)
            {
                graphics.FillRectangle(new SolidBrush(Color.Black), graphics.ClipBounds);
                graphics.DrawString(ex.Message, Font, new SolidBrush(Color.White), 8, 8);
            }

        }

        #region Draw Methods

        private void DrawInformation(Graphics g)
        {
            var posT = GetTile(Position.X, Position.Y);
            if (posT != null)
            {
                var textHeight = (int)g.MeasureString("X", Font).Height;
                var overlayPos = new Rectangle(Position.X + 18, Position.Y, 140,
                    textHeight + 10);
                if (posT.Rmp != null)
                {
                    overlayPos.Height += textHeight * 2;
                }
                if (overlayPos.X + overlayPos.Width > ClientRectangle.Width)
                {
                    overlayPos.X = Position.X - overlayPos.Width - 8;
                }
                if (overlayPos.Y + overlayPos.Height > ClientRectangle.Height)
                {
                    overlayPos.Y = Position.X - overlayPos.Height;
                }

                g.FillRectangle(new SolidBrush(Color.FromArgb(192, 0, 0, 0)), overlayPos);
                g.FillRectangle(new SolidBrush(Color.FromArgb(192, 255, 255, 255)), overlayPos.X + 3,
                    overlayPos.Y + 3, overlayPos.Width - 6, overlayPos.Height - 6);

                var textLeft = overlayPos.X + 5;
                {
                    var text = "Tile " + GetTileCoordinates(Position.X, Position.Y);
                    g.DrawString(text, Font, Brushes.Black, textLeft, overlayPos.Y + 5);
                }

                if (posT.Rmp != null)
                {
                    {
                        var text = "Spawns: " + RmpFile.UnitRankUFO[posT.Rmp.URank1];
                        g.DrawString(text, Font, Brushes.Black, textLeft, overlayPos.Y + 5 + textHeight);
                    }
                    {
                        var text = "Over: " + posT.Rmp.Index;
                        g.DrawString(text, Font, Brushes.Black, textLeft, overlayPos.Y + 5 + (textHeight * 2));
                    }
                }
            }
        }

        private void DrawPoles(Graphics g)
        {
            g.DrawString("W", _font, Brushes.Black, 0, 0);
            g.DrawString("N", _font, Brushes.Black, Width - 30, 0);
            g.DrawString("S", _font, Brushes.Black, 0, Height - _font.Height);
            g.DrawString("E", _font, brush: System.Drawing.Brushes.Black, x: Width - 30, y: Height - _font.Height);
        }

        private void DrawGridLines(Graphics g)
        {
            var map = Map;
            for (int i = 0; i <= map.MapSize.Rows; i++)
                g.DrawLine(MapPens["GridLineColor"], Origin.X - i * DrawAreaWidth, Origin.Y + i * DrawAreaHeight,
                    Origin.X + ((map.MapSize.Cols - i) * DrawAreaWidth), Origin.Y + ((i + map.MapSize.Cols) * DrawAreaHeight));
            for (int i = 0; i <= map.MapSize.Cols; i++)
                g.DrawLine(MapPens["GridLineColor"], Origin.X + i * DrawAreaWidth, Origin.Y + i * DrawAreaHeight,
                    (Origin.X + i * DrawAreaWidth) - map.MapSize.Rows * DrawAreaWidth,
                    (Origin.Y + i * DrawAreaHeight) + map.MapSize.Rows * DrawAreaHeight);
        }

        private void DrawNodes(GraphicsPath upper, Graphics g)
        {
            var map = Map;
            var startX = Origin.X;
            var startY = Origin.Y;
            for (int row = 0; row < map.MapSize.Rows; row++)
            {
                for (int col = 0, x = startX, y = startY;
                    col < map.MapSize.Cols;
                    col++, x += DrawAreaWidth, y += DrawAreaHeight)
                {
                    var tile = (XCMapTile) map[row, col];
                    if (map[row, col] != null && tile.Rmp != null)
                    {
                        upper.Reset();
                        upper.AddLine(x, y, x + DrawAreaWidth, y + DrawAreaHeight);
                        upper.AddLine(x + DrawAreaWidth, y + DrawAreaHeight, x, y + 2 * DrawAreaHeight);
                        upper.AddLine(x, y + 2 * DrawAreaHeight, x - DrawAreaWidth, y + DrawAreaHeight);
                        upper.CloseFigure();

                        //if clicked on, draw Blue
                        if (row == ClickPoint.Y && col == ClickPoint.X)
                            g.FillPath(MapBrushes["SelectedNodeColor"], upper);
                        else if (tile.Rmp.Usage != SpawnUsage.NoSpawn)
                            g.FillPath(MapBrushes["SpawnNodeColor"], upper);
                        else
                            g.FillPath(MapBrushes["UnselectedNodeColor"], upper);

                        for (int rr = 0; rr < tile.Rmp.NumLinks; rr++)
                        {
                            Link l = tile.Rmp[rr];
                            switch (l.Index)
                            {
                                case Link.NotUsed:
                                case Link.ExitEast:
                                case Link.ExitNorth:
                                case Link.ExitSouth:
                                case Link.ExitWest:
                                    break;
                                default:
                                    if (map.Rmp[l.Index] != null &&
                                        map.Rmp[l.Index].Height < map.CurrentHeight)
                                    {
                                        g.DrawLine(MapPens["UnselectedLinkColor"], x, y, x, y + DrawAreaHeight * 2);
                                    }
                                    else if (map.Rmp[l.Index] != null &&
                                             map.Rmp[l.Index].Height > map.CurrentHeight)
                                    {
                                        g.DrawLine(MapPens["UnselectedLinkColor"], x - DrawAreaWidth, y + DrawAreaHeight,
                                            x + DrawAreaWidth, y + DrawAreaHeight);
                                    }
                                    break;
                            }
                        }
                    }
                }
                startX -= DrawAreaWidth;
                startY += DrawAreaHeight;
            }
        }

        private void DrawWallsAndContent(GraphicsPath lower, Graphics g)
        {
            var map = Map;
            for (int row = 0, startX = Origin.X, startY = Origin.Y;
                row < map.MapSize.Rows;
                row++, startX -= DrawAreaWidth, startY += DrawAreaHeight)
            {
                for (int col = 0, x = startX, y = startY;
                    col < map.MapSize.Cols;
                    col++, x += DrawAreaWidth, y += DrawAreaHeight)
                {
                    if (map[row, col] != null)
                    {
                        lower.Reset();
                        lower.AddLine(x, y + 2 * DrawAreaHeight, x + DrawAreaWidth, y + DrawAreaHeight);
                        lower.AddLine(x + DrawAreaWidth, y + DrawAreaHeight, x - DrawAreaWidth, y + DrawAreaHeight);
                        lower.CloseFigure();
                        XCMapTile tile = (XCMapTile) map[row, col];

                        if (tile.North != null)
                            g.DrawLine(MapPens["WallColor"], x, y, x + DrawAreaWidth, y + DrawAreaHeight);

                        if (tile.West != null)
                            g.DrawLine(MapPens["WallColor"], x, y, x - DrawAreaWidth, y + DrawAreaHeight);

                        if (tile.Content != null)
                            g.FillPath(MapBrushes["ContentTiles"], lower);
                    }
                }
            }
        }

        private void DrawUnselectedLink(GraphicsPath upper, Graphics graphics)
        {
            var map = Map;
            for (int row = 0, startX = Origin.X, startY = Origin.Y;
                row < map.MapSize.Rows;
                row++, startX -= DrawAreaWidth, startY += DrawAreaHeight)
            {
                for (int col = 0, x = startX, y = startY;
                    col < map.MapSize.Cols;
                    col++, x += DrawAreaWidth, y += DrawAreaHeight)
                {
                    if (map[row, col] != null && ((XCMapTile) map[row, col]).Rmp != null)
                    {
                        RmpEntry f = ((XCMapTile) map[row, col]).Rmp;
                        upper.Reset();
                        upper.AddLine(x, y, x + DrawAreaWidth, y + DrawAreaHeight);
                        upper.AddLine(x + DrawAreaWidth, y + DrawAreaHeight, x, y + 2 * DrawAreaHeight);
                        upper.AddLine(x, y + 2 * DrawAreaHeight, x - DrawAreaWidth, y + DrawAreaHeight);
                        upper.CloseFigure();

                        for (int rr = 0; rr < f.NumLinks; rr++)
                        {
                            Link l = f[rr];
                            var pen = MapPens["UnselectedLinkColor"];
                            switch (l.Index)
                            {
                                case Link.NotUsed:
                                    break;
                                case Link.ExitEast:
                                    graphics.DrawLine(pen, x, y + DrawAreaHeight, Width, Height);
                                    break;
                                case Link.ExitNorth:
                                    graphics.DrawLine(pen, x, y + DrawAreaHeight, Width, 0);
                                    break;
                                case Link.ExitSouth:
                                    graphics.DrawLine(pen, x, y + DrawAreaHeight, 0, Height);
                                    break;
                                case Link.ExitWest:
                                    graphics.DrawLine(pen, x, y + DrawAreaHeight, 0, 0);
                                    break;
                                default:
                                    if (map.Rmp[l.Index] != null)
                                    {
                                        if (map.Rmp[l.Index].Height == map.CurrentHeight)
                                        {
                                            int toRow = map.Rmp[l.Index].Row;
                                            int toCol = map.Rmp[l.Index].Col;
                                            graphics.DrawLine(pen, x, y + DrawAreaHeight,
                                                Origin.X + (toCol - toRow) * DrawAreaWidth,
                                                Origin.Y + (toCol + toRow + 1) * DrawAreaHeight);
                                        }
                                    }
                                    break;
                            }
                        }
                    }
                }
            }
        }

        private void DrawSelectedLink(Graphics g)
        {
            var map = Map;
            if (ClickPoint.X < 0 || ClickPoint.Y < 0) return;
            if (((XCMapTile)map[ClickPoint.Y, ClickPoint.X]).Rmp == null) return;

            var pen = MapPens["SelectedLinkColor"];
            var r = ClickPoint.Y;
            var c = ClickPoint.X;
            RmpEntry f = ((XCMapTile) map[r, c]).Rmp;

            var unknownLocationX = Origin.X + (c - r) * DrawAreaWidth;
            var unknownLocationY = Origin.Y + (c + r + 1) * DrawAreaHeight;
            for (int rr = 0; rr < f.NumLinks; rr++)
            {
                Link l = f[rr];
                switch (l.Index)
                {
                    case Link.NotUsed:
                        break;
                    case Link.ExitEast:
                        g.DrawLine(pen, unknownLocationX,
                            unknownLocationY, Width, Height);
                        break;
                    case Link.ExitNorth:
                        g.DrawLine(pen, unknownLocationX,
                            unknownLocationY, Width, 0);
                        break;
                    case Link.ExitSouth:
                        g.DrawLine(pen, unknownLocationX,
                            unknownLocationY, 0, Height);
                        break;
                    case Link.ExitWest:
                        g.DrawLine(pen, unknownLocationX,
                            unknownLocationY, 0, 0);
                        break;
                    default:
                        if (map.Rmp[l.Index] != null && map.Rmp[l.Index].Height == map.CurrentHeight)
                        {
                            int toRow = map.Rmp[l.Index].Row;
                            int toCol = map.Rmp[l.Index].Col;
                            g.DrawLine(pen, unknownLocationX,
                                unknownLocationY, Origin.X + (toCol - toRow) * DrawAreaWidth,
                                Origin.Y + (toCol + toRow + 1) * DrawAreaHeight);
                        }
                        break;
                }
            }
        }

        #endregion

    }
}