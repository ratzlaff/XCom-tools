using System;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Windows.Forms;
using MapView.Forms.MapObservers.TopViews;
using XCom;

namespace MapView.Forms.MapObservers.RmpViews
{
	public class RmpPanel
		:
		MapPanel
	{
		public Point Position = new Point(-1, -1);

		private readonly Font _font = new Font("Verdana", 12, FontStyle.Bold);

		private readonly DrawContentService _drawContentService = new DrawContentService();

		private SolidPenBrush _wallColor;

		public void Calculate()
		{
			OnResize(null);
		}

		protected override void OnPaint(PaintEventArgs e)
		{
			var graphics = e.Graphics;

			try
			{
				if (Map != null)
				{
					var upper = new GraphicsPath();

					DrawWallsAndContent(graphics);
					DrawUnselectedLink(upper, graphics);
					DrawSelectedLink(graphics);
					DrawNodes(upper, graphics);
					DrawGridLines(graphics);
					DrawPoles(graphics);
					DrawInformation(graphics);
				}
			}
			catch (Exception ex)
			{
				graphics.FillRectangle(new SolidBrush(Color.Black), graphics.ClipBounds);
				graphics.DrawString(
								ex.Message,
								Font,
								new SolidBrush(Color.White),
								8, 8);
			}
		}

		#region Draw Methods

		private void DrawInformation(Graphics g)
		{
			var posT = GetTile(Position.X, Position.Y);

			if (posT != null)
			{
				var textHeight = (int)g.MeasureString("X", Font).Height;
				var overlayPos = new Rectangle(
											Position.X + 18, Position.Y,
											200, textHeight + 10);

				if (posT.Rmp != null)
					overlayPos.Height += textHeight * 4;

				if (overlayPos.X + overlayPos.Width > ClientRectangle.Width)
					overlayPos.X = Position.X - overlayPos.Width - 8;

				if (overlayPos.Y + overlayPos.Height > ClientRectangle.Height)
					overlayPos.Y = Position.X - overlayPos.Height;

				g.FillRectangle(new SolidBrush(Color.FromArgb(192, 0, 0, 0)), overlayPos);
				g.FillRectangle(
							new SolidBrush(Color.FromArgb(192, 255, 255, 255)),
							overlayPos.X + 3,
							overlayPos.Y + 3,
							overlayPos.Width - 6,
							overlayPos.Height - 6);

				var textLeft = overlayPos.X + 5;
				var textTop  = overlayPos.Y + 5;

				var pt = GetTileCoordinates(Position.X, Position.Y);
				g.DrawString(
							"Tile (" + pt.X + "," + pt.Y + ")",
							Font,
							Brushes.Black,
							textLeft,
							textTop);

				if (posT.Rmp != null)
				{
					g.DrawString(
								"Over: " + posT.Rmp.Index,
								Font,
								Brushes.Black,
								textLeft,
								textTop + textHeight);
					g.DrawString(
								"Priority: " + posT.Rmp.NodeImportance,
								Font,
								Brushes.Black,
								textLeft,
								textTop + textHeight * 2);
					g.DrawString(
								"Spawns: " + RmpFile.UnitRankUFO[posT.Rmp.URank1],
								Font,
								Brushes.Black,
								textLeft,
								textTop + textHeight * 3);
					g.DrawString(
								"Weight: " + posT.Rmp.Spawn,
								Font,
								Brushes.Black,
								textLeft,
								textTop + textHeight * 4);
				}
			}
		}

		private void DrawPoles(Graphics g)
		{
			g.DrawString(
						"W",
						_font,
						Brushes.Black,
						0, 0);
			g.DrawString(
						"N",
						_font,
						Brushes.Black,
						Width - 30, 0);
			g.DrawString(
						"S",
						_font,
						Brushes.Black,
						0, Height - _font.Height);
			g.DrawString(
						"E",
						_font,
						brush: Brushes.Black,
						x: Width - 30,
						y: Height - _font.Height);
		}

		private void DrawGridLines(Graphics g)
		{
			var map = Map;

			for (int i = 0; i <= map.MapSize.Rows; i++)
				g.DrawLine(
						MapPens["GridLineColor"],
						Origin.X - i * DrawAreaWidth,
						Origin.Y + i * DrawAreaHeight,
						Origin.X + ((map.MapSize.Cols - i) * DrawAreaWidth),
						Origin.Y + ((map.MapSize.Cols + i) * DrawAreaHeight));

			for (int i = 0; i <= map.MapSize.Cols; i++)
				g.DrawLine(
						MapPens["GridLineColor"],
						Origin.X + i * DrawAreaWidth,
						Origin.Y + i * DrawAreaHeight,
						(Origin.X + i * DrawAreaWidth)  - map.MapSize.Rows * DrawAreaWidth,
						(Origin.Y + i * DrawAreaHeight) + map.MapSize.Rows * DrawAreaHeight);
		}

		private void DrawNodes(GraphicsPath upper, Graphics g)
		{
			var map = Map;

			var startX = Origin.X;
			var startY = Origin.Y;

			var selectedNodeColor   = MapBrushes["SelectedNodeColor"];
			var spawnNodeColor      = MapBrushes["SpawnNodeColor"];
			var unselectedNodeColor = MapBrushes["UnselectedNodeColor"];

			selectedNodeColor.Color   = Color.FromArgb(200, selectedNodeColor.Color);
			spawnNodeColor.Color      = Color.FromArgb(200, spawnNodeColor.Color);
			unselectedNodeColor.Color = Color.FromArgb(200, unselectedNodeColor.Color);

			for (int row = 0; row < map.MapSize.Rows; row++)
			{
				for (int col = 0, x = startX, y = startY;
						col < map.MapSize.Cols;
						col++, x += DrawAreaWidth, y += DrawAreaHeight)
				{
					var tile = (XCMapTile)map[row, col];
					var rmpEntry = tile.Rmp;

					if (map[row, col] != null && rmpEntry != null)
					{
						upper.Reset();
						upper.AddLine(
									x, y,
									x + DrawAreaWidth, y + DrawAreaHeight);
						upper.AddLine(
									x + DrawAreaWidth, y + DrawAreaHeight,
									x, y + 2 * DrawAreaHeight);
						upper.AddLine(
									x, y + 2 * DrawAreaHeight,
									x - DrawAreaWidth, y + DrawAreaHeight);
						upper.CloseFigure();

						// draw Blue if clicked
						if (row == ClickPoint.Y && col == ClickPoint.X)
						{
							g.FillPath(selectedNodeColor, upper);
						}
						else if (rmpEntry.Spawn != SpawnUsage.NoSpawn)
						{
							g.FillPath(spawnNodeColor, upper);
						}
						else
						{
							g.FillPath(unselectedNodeColor, upper);
						}

						for (int rr = 0; rr < rmpEntry.NumLinks; rr++)
						{
							Link l = rmpEntry[rr];
							switch (l.Index)
							{
								case Link.NOT_USED:
								case Link.EXIT_EAST:
								case Link.EXIT_NORTH:
								case Link.EXIT_SOUTH:
								case Link.EXIT_WEST:
									break;

								default:
									if (   map.Rmp[l.Index] != null
										&& map.Rmp[l.Index].Height < map.CurrentHeight)
									{
										g.DrawLine(
												MapPens["UnselectedLinkColor"],
												x, y,
												x, y + DrawAreaHeight * 2);
									}
									else if (map.Rmp[l.Index] != null
										&&   map.Rmp[l.Index].Height > map.CurrentHeight)
									{
										g.DrawLine(
												MapPens["UnselectedLinkColor"],
												x - DrawAreaWidth, y + DrawAreaHeight,
												x + DrawAreaWidth, y + DrawAreaHeight);
									}
									break;
							}
						}

						if (DrawAreaHeight >= 10)
						{
							var boxX = x - (DrawAreaWidth  / 2);
							var boxY = y + (DrawAreaHeight / 3 * 2);

							var nodeImportance = (int)rmpEntry.NodeImportance;
							const int MAX_NODE_IMPORTANCE = 8;
							DrawBox(
									g,
									boxX, boxY,
									nodeImportance, MAX_NODE_IMPORTANCE,
									Brushes.Red);

							var spawn = (int)rmpEntry.Spawn;
							if (spawn > 0)
								spawn += 5;

							DrawBox(
									g,
									boxX + 3, boxY,
									spawn, 15,
									Brushes.DeepSkyBlue);
						}
					}
				}
				startX -= DrawAreaWidth;
				startY += DrawAreaHeight;
			}
		}

		private static void DrawBox(
								Graphics g,
								int boxX, int boxY,
								int value, int max,
								Brush color)
		{
			g.FillRectangle(
						Brushes.Gray,
						boxX, boxY,
						3, 10);
			g.DrawRectangle(
						Pens.Black,
						boxX, boxY,
						3, 10);

			if (max > 8)
			{
				value = (int)((double)value / max * 8);
				max = 8;
			}

			if (value > max) value = max;

			if (value > 0)
				g.FillRectangle(
							color,
							boxX + 1, boxY + 1 + (max - value),
							2, 1 + value);
		}

		private void DrawWallsAndContent(Graphics g)
		{
			if (_wallColor == null)
				_wallColor = new SolidPenBrush(MapPens["WallColor"] );

			_drawContentService.HWidth = DrawAreaWidth;
			_drawContentService.HHeight = DrawAreaHeight;

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
						XCMapTile tile = (XCMapTile) map[row, col];

						if (tile.North != null)
							_drawContentService.DrawContent(g, _wallColor, x, y, tile.North);

						if (tile.West != null)
							_drawContentService.DrawContent(g, _wallColor, x, y, tile.West);

						if (tile.Content != null)
							_drawContentService.DrawContent(g, _wallColor, x, y, tile.Content);
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
						RmpEntry f = ((XCMapTile)map[row, col]).Rmp;
						upper.Reset();
						upper.AddLine(
									x, y,
									x + DrawAreaWidth, y + DrawAreaHeight);
						upper.AddLine(
									x + DrawAreaWidth, y + DrawAreaHeight,
									x, y + 2 * DrawAreaHeight);
						upper.AddLine(
									x, y + 2 * DrawAreaHeight,
									x - DrawAreaWidth, y + DrawAreaHeight);
						upper.CloseFigure();

						for (int rr = 0; rr < f.NumLinks; rr++)
						{
							Link l = f[rr];
							var pen = MapPens["UnselectedLinkColor"];
							switch (l.Index)
							{
								case Link.NOT_USED:
									break;

								case Link.EXIT_EAST:
									graphics.DrawLine(
													pen,
													x, y + DrawAreaHeight,
													Width, Height);
									break;

								case Link.EXIT_NORTH:
									graphics.DrawLine(
													pen,
													x, y + DrawAreaHeight,
													Width, 0);
									break;

								case Link.EXIT_SOUTH:
									graphics.DrawLine(
													pen,
													x, y + DrawAreaHeight,
													0, Height);
									break;

								case Link.EXIT_WEST:
									graphics.DrawLine(
													pen,
													x, y + DrawAreaHeight,
													0, 0);
									break;

								default:
									if (   map.Rmp[l.Index] != null
										&& map.Rmp[l.Index].Height == map.CurrentHeight)
									{
										int toRow = map.Rmp[l.Index].Row;
										int toCol = map.Rmp[l.Index].Col;
										graphics.DrawLine(
														pen,
														x, y + DrawAreaHeight,
														Origin.X + (toCol - toRow) * DrawAreaWidth,
														Origin.Y + (toCol + toRow + 1) * DrawAreaHeight);
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
			if (ClickPoint.X > -1 && ClickPoint.Y > -1)
			{
				var map = Map;

				if (((XCMapTile)map[ClickPoint.Y, ClickPoint.X]).Rmp != null)
				{
					var pen = MapPens["SelectedLinkColor"];
					var r = ClickPoint.Y;
					var c = ClickPoint.X;
					RmpEntry f = ((XCMapTile)map[r, c]).Rmp;

					var unknownLocationX = Origin.X + (c - r) * DrawAreaWidth;
					var unknownLocationY = Origin.Y + (c + r + 1) * DrawAreaHeight;

					for (int rr = 0; rr < f.NumLinks; rr++)
					{
						Link l = f[rr];
						switch (l.Index)
						{
							case Link.NOT_USED:
								break;

							case Link.EXIT_EAST:
								g.DrawLine(
										pen,
										unknownLocationX, unknownLocationY,
										Width, Height);
								break;

							case Link.EXIT_NORTH:
								g.DrawLine(
										pen,
										unknownLocationX, unknownLocationY,
										Width, 0);
								break;

							case Link.EXIT_SOUTH:
								g.DrawLine(
										pen,
										unknownLocationX, unknownLocationY,
										0, Height);
								break;

							case Link.EXIT_WEST:
								g.DrawLine(
										pen,
										unknownLocationX, unknownLocationY,
										0, 0);
								break;

							default:
								if (   map.Rmp[l.Index] != null
									&& map.Rmp[l.Index].Height == map.CurrentHeight)
								{
									int toRow = map.Rmp[l.Index].Row;
									int toCol = map.Rmp[l.Index].Col;

									g.DrawLine(
											pen,
											unknownLocationX, unknownLocationY,
											Origin.X + (toCol - toRow) * DrawAreaWidth,
											Origin.Y + (toCol + toRow + 1) * DrawAreaHeight);
								}
								break;
						}
					}
				}
			}
		}

		#endregion
	}
}
