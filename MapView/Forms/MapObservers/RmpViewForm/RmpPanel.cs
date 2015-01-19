using System;
using System.Collections.Generic;
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

        public Dictionary<string, SolidBrush> Brushes
        {
            get { return brushes; }
            set { brushes = value; }
        }

        public Dictionary<string, Pen> Pens
        {
            get { return pens; }
            set { pens = value; }
        }

        public void Calc()
        {
            OnResize(null);
        }

        protected override void OnPaint(PaintEventArgs e)
        {
            var graphics = e.Graphics;
            try
            {
                if (map == null) return;
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
                graphics.DrawString(ex.Message, this.Font, new SolidBrush(Color.White), 8, 8);
            }

        }

        #region Draw Methods

        private void DrawInformation(Graphics g)
        {
            XCMapTile posT = GetTile(Position.X, Position.Y);
            if (posT != null)
            {
                Rectangle overlayPos = new Rectangle(Position.X + 18, Position.Y, 140,
                    (int) g.MeasureString("X", this.Font).Height + 10);
                if (posT.Rmp != null)
                {
                    overlayPos.Height += (int) g.MeasureString("X", this.Font).Height;
                }
                if (overlayPos.X + overlayPos.Width > this.ClientRectangle.Width)
                {
                    overlayPos.X = Position.X - overlayPos.Width - 8;
                }
                if (overlayPos.Y + overlayPos.Height > this.ClientRectangle.Height)
                {
                    overlayPos.Y = Position.X - overlayPos.Height;
                }

                g.FillRectangle(new SolidBrush(Color.FromArgb(192, 0, 0, 0)), overlayPos);
                g.FillRectangle(new SolidBrush(Color.FromArgb(192, 255, 255, 255)), overlayPos.X + 3,
                    overlayPos.Y + 3, overlayPos.Width - 6, overlayPos.Height - 6);
                g.DrawString("Tile " + GetTileCoordinates(Position.X, Position.Y).ToString(), this.Font,
                    System.Drawing.Brushes.Black, overlayPos.X + 5, overlayPos.Y + 5);
                if (posT.Rmp != null)
                {
                    g.DrawString("Spawns: " + RmpFile.UnitRankUFO[posT.Rmp.URank1].ToString(), this.Font,
                        System.Drawing.Brushes.Black, overlayPos.X + 5,
                        overlayPos.Y + 5 + (int) g.MeasureString("X", this.Font).Height);
                }
            }
        }

        private void DrawPoles(Graphics g)
        {
            g.DrawString("W", _font, System.Drawing.Brushes.Black, 0, 0);
            g.DrawString("N", _font, System.Drawing.Brushes.Black, Width - 30, 0);
            g.DrawString("S", _font, System.Drawing.Brushes.Black, 0, Height - _font.Height);
            g.DrawString("E", _font, System.Drawing.Brushes.Black, Width - 30, Height - _font.Height);
        }

        private void DrawGridLines(Graphics g)
        {
            for (int i = 0; i <= map.MapSize.Rows; i++)
                g.DrawLine(pens["GridLineColor"], origin.X - i * hWidth, origin.Y + i * hHeight,
                    origin.X + ((map.MapSize.Cols - i) * hWidth), origin.Y + ((i + map.MapSize.Cols) * hHeight));
            for (int i = 0; i <= map.MapSize.Cols; i++)
                g.DrawLine(pens["GridLineColor"], origin.X + i * hWidth, origin.Y + i * hHeight,
                    (origin.X + i * hWidth) - map.MapSize.Rows * hWidth,
                    (origin.Y + i * hHeight) + map.MapSize.Rows * hHeight);
        }

        private void DrawNodes(GraphicsPath upper, Graphics g)
        {
            var startX = origin.X;
            var startY = origin.Y;
            for (int row = 0; row < map.MapSize.Rows; row++)
            {
                for (int col = 0, x = startX, y = startY;
                    col < map.MapSize.Cols;
                    col++, x += hWidth, y += hHeight)
                {
                    var tile = (XCMapTile) map[row, col];
                    if (map[row, col] != null && tile.Rmp != null)
                    {
                        upper.Reset();
                        upper.AddLine(x, y, x + hWidth, y + hHeight);
                        upper.AddLine(x + hWidth, y + hHeight, x, y + 2 * hHeight);
                        upper.AddLine(x, y + 2 * hHeight, x - hWidth, y + hHeight);
                        upper.CloseFigure();

                        //if clicked on, draw Blue
                        if (row == clickPoint.Y && col == clickPoint.X)
                            g.FillPath(brushes["SelectedNodeColor"], upper);
                        else if (tile.Rmp.Usage != SpawnUsage.NoSpawn)
                            g.FillPath(brushes["SpawnNodeColor"], upper);
                        else
                            g.FillPath(brushes["UnselectedNodeColor"], upper);

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
                                        g.DrawLine(pens["UnselectedLinkColor"], x, y, x, y + hHeight * 2);
                                    }
                                    else if (map.Rmp[l.Index] != null &&
                                             map.Rmp[l.Index].Height > map.CurrentHeight)
                                    {
                                        g.DrawLine(pens["UnselectedLinkColor"], x - hWidth, y + hHeight,
                                            x + hWidth, y + hHeight);
                                    }
                                    break;
                            }
                        }
                    }
                }
                startX -= hWidth;
                startY += hHeight;
            }
        }

        private void DrawWallsAndContent(GraphicsPath lower, Graphics g)
        {
            for (int row = 0, startX = origin.X, startY = origin.Y;
                row < map.MapSize.Rows;
                row++, startX -= hWidth, startY += hHeight)
            {
                for (int col = 0, x = startX, y = startY;
                    col < map.MapSize.Cols;
                    col++, x += hWidth, y += hHeight)
                {
                    if (map[row, col] != null)
                    {
                        lower.Reset();
                        lower.AddLine(x, y + 2 * hHeight, x + hWidth, y + hHeight);
                        lower.AddLine(x + hWidth, y + hHeight, x - hWidth, y + hHeight);
                        lower.CloseFigure();
                        XCMapTile tile = (XCMapTile) map[row, col];

                        if (tile.North != null)
                            g.DrawLine(pens["WallColor"], x, y, x + hWidth, y + hHeight);

                        if (tile.West != null)
                            g.DrawLine(pens["WallColor"], x, y, x - hWidth, y + hHeight);

                        if (tile.Content != null)
                            g.FillPath(brushes["ContentTiles"], lower);
                    }
                }
            }
        }

        private void DrawUnselectedLink(GraphicsPath upper, Graphics graphics)
        {
            for (int row = 0, startX = origin.X, startY = origin.Y;
                row < map.MapSize.Rows;
                row++, startX -= hWidth, startY += hHeight)
            {
                for (int col = 0, x = startX, y = startY;
                    col < map.MapSize.Cols;
                    col++, x += hWidth, y += hHeight)
                {
                    if (map[row, col] != null && ((XCMapTile) map[row, col]).Rmp != null)
                    {
                        RmpEntry f = ((XCMapTile) map[row, col]).Rmp;
                        upper.Reset();
                        upper.AddLine(x, y, x + hWidth, y + hHeight);
                        upper.AddLine(x + hWidth, y + hHeight, x, y + 2 * hHeight);
                        upper.AddLine(x, y + 2 * hHeight, x - hWidth, y + hHeight);
                        upper.CloseFigure();

                        for (int rr = 0; rr < f.NumLinks; rr++)
                        {
                            Link l = f[rr];
                            var pen = pens["UnselectedLinkColor"];
                            switch (l.Index)
                            {
                                case Link.NotUsed:
                                    break;
                                case Link.ExitEast:
                                    graphics.DrawLine(pen, x, y + hHeight, Width, Height);
                                    break;
                                case Link.ExitNorth:
                                    graphics.DrawLine(pen, x, y + hHeight, Width, 0);
                                    break;
                                case Link.ExitSouth:
                                    graphics.DrawLine(pen, x, y + hHeight, 0, Height);
                                    break;
                                case Link.ExitWest:
                                    graphics.DrawLine(pen, x, y + hHeight, 0, 0);
                                    break;
                                default:
                                    if (map.Rmp[l.Index] != null)
                                    {
                                        if (map.Rmp[l.Index].Height == map.CurrentHeight)
                                        {
                                            int toRow = map.Rmp[l.Index].Row;
                                            int toCol = map.Rmp[l.Index].Col;
                                            graphics.DrawLine(pen, x, y + hHeight,
                                                origin.X + (toCol - toRow) * hWidth,
                                                origin.Y + (toCol + toRow + 1) * hHeight);
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
            if (((XCMapTile) map[clickPoint.Y, clickPoint.X]).Rmp == null) return;

            var pen = pens["SelectedLinkColor"];
            var r = clickPoint.Y;
            var c = clickPoint.X;
            RmpEntry f = ((XCMapTile) map[r, c]).Rmp;

            var unknownLocationX = origin.X + (c - r) * hWidth;
            var unknownLocationY = origin.Y + (c + r + 1) * hHeight;
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
                                unknownLocationY, origin.X + (toCol - toRow) * hWidth,
                                origin.Y + (toCol + toRow + 1) * hHeight);
                        }
                        break;
                }
            }
        }

        #endregion

    }
}