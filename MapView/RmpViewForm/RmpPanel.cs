using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Drawing;
using System.Windows.Forms;
using System.Drawing.Drawing2D;
using XCom;
using XCom.Interfaces.Base;
using ViewLib.Base;
using UtilLib;
using MapLib;

namespace MapView.RmpViewForm
{
	public class RmpPanel : SimpleMapPanel
	{
		protected new XCMapFile map;
		private Font myFont = new Font("Arial", 12, FontStyle.Bold);

		public RmpPanel() { }

		protected override void mapChanged(MapChangedEventArgs e)
		{
			this.map = (XCMapFile)e.Map;
			base.mapChanged(e);
		}

		protected override void OnPaint(PaintEventArgs e)
		{
			base.OnPaint(e);

			if (map != null) {
				Graphics g = e.Graphics;

				for (int row = 0, startX = offX, startY = offY; row < map.Size.Rows; row++, startX -= hWidth, startY += hHeight)
					for (int col = 0, x = startX, y = startY; col < map.Size.Cols; col++, x += hWidth, y += hHeight)
						if (map[row, col] != null) {
							lower.Reset();
							lower.AddLine(x, y + 2 * hHeight, x + hWidth, y + hHeight);
							lower.AddLine(x + hWidth, y + hHeight, x - hWidth, y + hHeight);
							lower.CloseFigure();
							XCMapTile tile = (XCMapTile)map[row, col];

							if (tile.North != null)
								g.DrawLine(pens["WallColor"], x, y, x + hWidth, y + hHeight);

							if (tile.West != null)
								g.DrawLine(pens["WallColor"], x, y, x - hWidth, y + hHeight);

							if (tile.Content != null)
								g.FillPath(brushes["ContentColor"], lower);
						}

				for (int row = 0, startX = offX, startY = offY; row < map.Size.Rows; row++, startX -= hWidth, startY += hHeight)
					for (int col = 0, x = startX, y = startY; col < map.Size.Cols; col++, x += hWidth, y += hHeight)
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
										g.DrawLine(pens["UnselectedLinkColor"], x, y + hHeight, Width, Height);
										break;
									case Link.ExitNorth:
										g.DrawLine(pens["UnselectedLinkColor"], x, y + hHeight, Width, 0);
										break;
									case Link.ExitSouth:
										g.DrawLine(pens["UnselectedLinkColor"], x, y + hHeight, 0, Height);
										break;
									case Link.ExitWest:
										g.DrawLine(pens["UnselectedLinkColor"], x, y + hHeight, 0, 0);
										break;
									default:
										if (map.Rmp.Length > l.Index) {
											if (map.Rmp[l.Index] != null) {
												if (map.Rmp[l.Index].Height == map.CurrentHeight) {
													int toRow = map.Rmp[l.Index].Row;
													int toCol = map.Rmp[l.Index].Col;
													g.DrawLine(pens["UnselectedLinkColor"], x, y + hHeight, offX + (toCol - toRow) * hWidth, offY + (toCol + toRow + 1) * hHeight);
												}
											}
										}
										break;
								}
							}
						}

				if (((XCMapTile)map[selR, selC]).Rmp != null) {
					int r = selR;
					int c = selC;
					RmpEntry f = ((XCMapTile)map[r, c]).Rmp;

					for (int rr = 0; rr < f.NumLinks; rr++) {
						Link l = f[rr];
						switch (l.Index) {
							case Link.NotUsed:
								break;
							case Link.ExitEast:
								g.DrawLine(pens["SelectedLinkColor"], offX + (c - r) * hWidth, offY + (c + r + 1) * hHeight, Width, Height);
								break;
							case Link.ExitNorth:
								g.DrawLine(pens["SelectedLinkColor"], offX + (c - r) * hWidth, offY + (c + r + 1) * hHeight, Width, 0);
								break;
							case Link.ExitSouth:
								g.DrawLine(pens["SelectedLinkColor"], offX + (c - r) * hWidth, offY + (c + r + 1) * hHeight, 0, Height);
								break;
							case Link.ExitWest:
								g.DrawLine(pens["SelectedLinkColor"], offX + (c - r) * hWidth, offY + (c + r + 1) * hHeight, 0, 0);
								break;
							default:
								if (map.Rmp[l.Index] != null && map.Rmp[l.Index].Height == map.CurrentHeight) {
									int toRow = map.Rmp[l.Index].Row;
									int toCol = map.Rmp[l.Index].Col;
									g.DrawLine(pens["SelectedLinkColor"], offX + (c - r) * hWidth, offY + (c + r + 1) * hHeight, offX + (toCol - toRow) * hWidth, offY + (toCol + toRow + 1) * hHeight);
								}
								break;
						}
					}
				}

				for (int row = 0, startX = offX, startY = offY; row < map.Size.Rows; row++, startX -= hWidth, startY += hHeight) {
					for (int col = 0, x = startX, y = startY; col < map.Size.Cols; col++, x += hWidth, y += hHeight) {
						XCMapTile tile = (XCMapTile)map[row, col];
						if (map[row, col] != null && tile.Rmp != null) {
							upper.Reset();
							upper.AddLine(x, y, x + hWidth, y + hHeight);
							upper.AddLine(x + hWidth, y + hHeight, x, y + 2 * hHeight);
							upper.AddLine(x, y + 2 * hHeight, x - hWidth, y + hHeight);
							upper.CloseFigure();

							//if clicked on, draw Blue
							if (row == selR && col == selC)
								g.FillPath(brushes["SelectedNodeColor"], upper);
							else if (tile.Rmp.Usage != SpawnUsage.NoSpawn)
								g.FillPath(brushes["SpawnNodeColor"], upper);
							else
								g.FillPath(brushes["UnselectedNodeColor"], upper);

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
												g.DrawLine(pens["UnselectedLinkColor"], x, y, x, y + hHeight * 2);
											} else if (map.Rmp[l.Index] != null && map.Rmp[l.Index].Height > map.CurrentHeight) {
												g.DrawLine(pens["UnselectedLinkColor"], x - hWidth, y + hHeight, x + hWidth, y + hHeight);
											}
										}
										break;
								}
							}
						}
					}
				}

				for (int i = 0; i <= map.Size.Rows; i++)
					g.DrawLine(pens["GridColor"], offX - i * hWidth, offY + i * hHeight, offX + ((map.Size.Cols - i) * hWidth), offY + ((i + map.Size.Cols) * hHeight));
				for (int i = 0; i <= map.Size.Cols; i++)
					g.DrawLine(pens["GridColor"], offX + i * hWidth, offY + i * hHeight, (offX + i * hWidth) - map.Size.Rows * hWidth, (offY + i * hHeight) + map.Size.Rows * hHeight);

				g.DrawString("W", myFont, System.Drawing.Brushes.Black, 0, 0);
				g.DrawString("N", myFont, System.Drawing.Brushes.Black, Width - 30, 0);
				g.DrawString("S", myFont, System.Drawing.Brushes.Black, 0, Height - myFont.Height);
				g.DrawString("E", myFont, System.Drawing.Brushes.Black, Width - 30, Height - myFont.Height);
			}
		}

		public override void SetupDefaultSettings(Map_Observer_Form sender)
		{
			base.SetupDefaultSettings(sender);

			// UnselectedLinkColor, UnselectedLinkWidth
			addPenSetting(new Pen(new SolidBrush(Color.Red), 2), "UnselectedLink", "Links", "Color of unselected link lines", "Width of unselected link lines", sender.Settings);

			// SelectedLinkColor, SelectedLinkWidth
			addPenSetting(new Pen(new SolidBrush(Color.Blue), 2), "SelectedLink", "Links", "Color of selected link lines", "Width of selected link lines", sender.Settings);

			// WallColor, WallWidth
			addPenSetting(new Pen(new SolidBrush(Color.Black), 4), "Wall", "View", "Color of wall indicators", "Width of wall indicators", sender.Settings);

			// SelectedNodeColor
			addBrushSetting(new SolidBrush(Color.Blue), "SelectedNode", "Nodes", "Color of selected nodes", sender.Settings);

			// UnselectedNodeColor
			addBrushSetting(new SolidBrush(Color.Green), "UnselectedNode", "Nodes", "Color of unselected nodes", sender.Settings);

			// SpawnNodeColor
			addBrushSetting(new SolidBrush(Color.GreenYellow), "SpawnNode", "Nodes", "Color of spawn nodes", sender.Settings);

			// ContentColor
			addBrushSetting(new SolidBrush(Color.DarkGray), "Content", "Tile", "Color of map tiles with a content tile", sender.Settings);
		}
	}
}
