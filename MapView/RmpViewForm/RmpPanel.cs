using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;
using System.Windows.Forms;
using System.Drawing.Drawing2D;
using XCom;

namespace MapView.RmpViewForm
{
	public class RmpPanel : MapPanel
	{
		private Font myFont = new Font("Arial", 12, FontStyle.Bold);

		public RmpPanel() { }

		public void Calc()
		{
			OnResize(null);
		}

		protected override void OnPaint(PaintEventArgs e)
		{
			if (map != null) {
				Graphics g = e.Graphics;
				GraphicsPath lower = new GraphicsPath();
				GraphicsPath upper = new GraphicsPath();

				for (int row = 0, startX = origin.X, startY = origin.Y; row < map.MapSize.Rows; row++, startX -= hWidth, startY += hHeight) {
					for (int col = 0, x = startX, y = startY; col < map.MapSize.Cols; col++, x += hWidth, y += hHeight) {
						if (map[row, col] != null) {
							lower.Reset();
							lower.AddLine(x, y + 2 * hHeight, x + hWidth, y + hHeight);
							lower.AddLine(x + hWidth, y + hHeight, x - hWidth, y + hHeight);
							lower.CloseFigure();
							XCMapTile tile = (XCMapTile)map[row, col];

							if (tile.North != null)
								g.DrawLine(Pens["WallColor"], x, y, x + hWidth, y + hHeight);

							if (tile.West != null)
								g.DrawLine(Pens["WallColor"], x, y, x - hWidth, y + hHeight);

							if (tile.Content != null)
								g.FillPath(Brushes["ContentTiles"], lower);
						}
					}
				}

				for (int row = 0, startX = origin.X, startY = origin.Y; row < map.MapSize.Rows; row++, startX -= hWidth, startY += hHeight) {
					for (int col = 0, x = startX, y = startY; col < map.MapSize.Cols; col++, x += hWidth, y += hHeight) {
						if (map[row, col] != null && ((XCMapTile)map[row, col]).Rmp != null) {
							RmpEntry f = ((XCMapTile)map[row, col]).Rmp;
							upper.Reset();
							upper.AddLine(x, y, x + hWidth, y + hHeight);
							upper.AddLine(x + hWidth, y + hHeight, x, y + 2 * hHeight);
							upper.AddLine(x, y + 2 * hHeight, x - hWidth, y + hHeight);
							upper.CloseFigure();

							for (int rr = 0; rr < f.NumLinks; rr++) {
								Link l = f[rr];
								switch (l.Index) {
									case Link.NotUsed:
										break;
									case Link.ExitEast:
										g.DrawLine(Pens["UnselectedLinkColor"], x, y + hHeight, Width, Height);
										break;
									case Link.ExitNorth:
										g.DrawLine(Pens["UnselectedLinkColor"], x, y + hHeight, Width, 0);
										break;
									case Link.ExitSouth:
										g.DrawLine(Pens["UnselectedLinkColor"], x, y + hHeight, 0, Height);
										break;
									case Link.ExitWest:
										g.DrawLine(Pens["UnselectedLinkColor"], x, y + hHeight, 0, 0);
										break;
									default:
										if (map.Rmp.Length > l.Index) {
											if (map.Rmp[l.Index] != null) {
												if (map.Rmp[l.Index].Height == map.CurrentHeight) {
													int toRow = map.Rmp[l.Index].Row;
													int toCol = map.Rmp[l.Index].Col;
													g.DrawLine(Pens["UnselectedLinkColor"], x, y + hHeight, origin.X + (toCol - toRow) * hWidth, origin.Y + (toCol + toRow + 1) * hHeight);
												}
											}
										}
										break;
								}
							}
						}
					}
				}

				if (((XCMapTile)map[clickPoint.Y, clickPoint.X]).Rmp != null) {
					int r = clickPoint.Y;
					int c = clickPoint.X;
					RmpEntry f = ((XCMapTile)map[r, c]).Rmp;

					for (int rr = 0; rr < f.NumLinks; rr++) {
						Link l = f[rr];
						switch (l.Index) {
							case Link.NotUsed:
								break;
							case Link.ExitEast:
								g.DrawLine(Pens["SelectedLinkColor"], origin.X + (c - r) * hWidth, origin.Y + (c + r + 1) * hHeight, Width, Height);
								break;
							case Link.ExitNorth:
								g.DrawLine(Pens["SelectedLinkColor"], origin.X + (c - r) * hWidth, origin.Y + (c + r + 1) * hHeight, Width, 0);
								break;
							case Link.ExitSouth:
								g.DrawLine(Pens["SelectedLinkColor"], origin.X + (c - r) * hWidth, origin.Y + (c + r + 1) * hHeight, 0, Height);
								break;
							case Link.ExitWest:
								g.DrawLine(Pens["SelectedLinkColor"], origin.X + (c - r) * hWidth, origin.Y + (c + r + 1) * hHeight, 0, 0);
								break;
							default:
								if (map.Rmp[l.Index] != null && map.Rmp[l.Index].Height == map.CurrentHeight) {
									int toRow = map.Rmp[l.Index].Row;
									int toCol = map.Rmp[l.Index].Col;
									g.DrawLine(Pens["SelectedLinkColor"], origin.X + (c - r) * hWidth, origin.Y + (c + r + 1) * hHeight, origin.X + (toCol - toRow) * hWidth, origin.Y + (toCol + toRow + 1) * hHeight);
								}
								break;
						}
					}
				}

				for (int row = 0, startX = origin.X, startY = origin.Y; row < map.MapSize.Rows; row++, startX -= hWidth, startY += hHeight) {
					for (int col = 0, x = startX, y = startY; col < map.MapSize.Cols; col++, x += hWidth, y += hHeight) {
						XCMapTile tile = (XCMapTile)map[row, col];
						if (map[row, col] != null && tile.Rmp != null) {
							upper.Reset();
							upper.AddLine(x, y, x + hWidth, y + hHeight);
							upper.AddLine(x + hWidth, y + hHeight, x, y + 2 * hHeight);
							upper.AddLine(x, y + 2 * hHeight, x - hWidth, y + hHeight);
							upper.CloseFigure();

							//if clicked on, draw Blue
							if (row == clickPoint.Y && col == clickPoint.X)
								g.FillPath(Brushes["SelectedNodeColor"], upper);
							else if (tile.Rmp.Usage != SpawnUsage.NoSpawn)
								g.FillPath(Brushes["SpawnNodeColor"], upper);
							else
								g.FillPath(Brushes["UnselectedNodeColor"], upper);

							for (int rr = 0; rr < tile.Rmp.NumLinks; rr++) {
								Link l = tile.Rmp[rr];
								switch (l.Index) {
									case Link.NotUsed:
										break;
									case Link.ExitEast:
										break;
									case Link.ExitNorth:
										break;
									case Link.ExitSouth:
										break;
									case Link.ExitWest:
										break;
									default:
										if (map.Rmp.Length > l.Index) {
											if (map.Rmp[l.Index] != null && map.Rmp[l.Index].Height < map.CurrentHeight) {
												g.DrawLine(Pens["UnselectedLinkColor"], x, y, x, y + hHeight * 2);
											} else if (map.Rmp[l.Index] != null && map.Rmp[l.Index].Height > map.CurrentHeight) {
												g.DrawLine(Pens["UnselectedLinkColor"], x - hWidth, y + hHeight, x + hWidth, y + hHeight);
											}
										}
										break;
								}
							}
						}
					}
				}

				for (int i = 0; i <= map.MapSize.Rows; i++)
					g.DrawLine(Pens["GridLineColor"], origin.X - i * hWidth, origin.Y + i * hHeight, origin.X + ((map.MapSize.Cols - i) * hWidth), origin.Y + ((i + map.MapSize.Cols) * hHeight));
				for (int i = 0; i <= map.MapSize.Cols; i++)
					g.DrawLine(Pens["GridLineColor"], origin.X + i * hWidth, origin.Y + i * hHeight, (origin.X + i * hWidth) - map.MapSize.Rows * hWidth, (origin.Y + i * hHeight) + map.MapSize.Rows * hHeight);

				g.DrawString("W", myFont, System.Drawing.Brushes.Black, 0, 0);
				g.DrawString("N", myFont, System.Drawing.Brushes.Black, Width - 30, 0);
				g.DrawString("S", myFont, System.Drawing.Brushes.Black, 0, Height - myFont.Height);
				g.DrawString("E", myFont, System.Drawing.Brushes.Black, Width - 30, Height - myFont.Height);
			}
		}
	}
}
