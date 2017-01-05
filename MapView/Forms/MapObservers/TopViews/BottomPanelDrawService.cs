using System.Collections.Generic;
using System.Drawing;
using MapView.Forms.MainWindow;
using XCom;

namespace MapView.Forms.MapObservers.TopViews
{
	internal class BottomPanelDrawService
	{
		public SolidBrush Brush
		{ get; set; }

		public Dictionary<string, SolidBrush> Brushes
		{ get; set; }

		public Dictionary<string, Pen> Pens
		{ get; set; }

		public Font Font
		{ get; set; }

		public const int TOTAL_QUADRAN_SPACE = tileWidth + 2 * space;
		public const int tileWidth = 32;
		public const int tileHeight = 40;
		public const int space = 2;
		public const int startX = 5, startY = 0;

		public void Draw(Graphics g, XCMapTile mapTile, XCMapTile.MapQuadrant selectedQuadrant)
		{
			// Draw selection
			if (selectedQuadrant == XCMapTile.MapQuadrant.Ground)
				g.FillRectangle(
							Brush,
							startX,
							startY,
							tileWidth + 1,
							tileHeight + 2);
			else if (selectedQuadrant == XCMapTile.MapQuadrant.West)
				g.FillRectangle(
							Brush,
							startX + (TOTAL_QUADRAN_SPACE),
							startY,
							tileWidth + 1,
							tileHeight + 2);
			else if (selectedQuadrant == XCMapTile.MapQuadrant.North)
				g.FillRectangle(
							Brush,
							startX + (2 * TOTAL_QUADRAN_SPACE),
							startY,
							tileWidth + 1,
							tileHeight + 2);
			else if (selectedQuadrant == XCMapTile.MapQuadrant.Content)
				g.FillRectangle(
							Brush,
							startX + (3 * TOTAL_QUADRAN_SPACE),
							startY,
							tileWidth + 1,
							tileHeight + 2);

			var topView = MainWindowsManager.TopView.TopViewControl;
		 
			// Ground not visible
			if (!topView.GroundVisible)
				g.FillRectangle(
							System.Drawing.Brushes.DarkGray,
							startX,
							startY,
							tileWidth + 1,
							tileHeight + 2);

			if (mapTile != null && mapTile.Ground != null)
			{
				g.DrawImage(
							mapTile.Ground[MapViewPanel.Current].Image,
							startX,
							startY - mapTile.Ground.Info.TileOffset);

				if (mapTile.Ground.Info.HumanDoor || mapTile.Ground.Info.UFODoor)
					g.DrawString(
							"Door",
							Font,
							System.Drawing.Brushes.Black,
							startX,
							startY + PckImage.Height - Font.Height);
			}
			else
				g.DrawImage(
							Globals.ExtraTiles[3].Image,
							startX,
							startY);

			if (!topView.WestVisible)
				g.FillRectangle(
							System.Drawing.Brushes.DarkGray,
							startX + (TOTAL_QUADRAN_SPACE),
							startY,
							tileWidth + 1,
							tileHeight + 2);

			if (mapTile != null && mapTile.West != null)
			{
				g.DrawImage(
							mapTile.West[MapViewPanel.Current].Image,
							startX + (TOTAL_QUADRAN_SPACE),
							startY - mapTile.West.Info.TileOffset);

				if (mapTile.West.Info.HumanDoor || mapTile.West.Info.UFODoor)
					g.DrawString(
							"Door",
							Font,
							System.Drawing.Brushes.Black,
							startX + (TOTAL_QUADRAN_SPACE),
							startY + PckImage.Height - Font.Height);
			}
			else
				g.DrawImage(
							Globals.ExtraTiles[1].Image,
							startX + (TOTAL_QUADRAN_SPACE),
							startY);

			if (!topView.NorthVisible)
				g.FillRectangle(
							System.Drawing.Brushes.DarkGray,
							startX + (2 * TOTAL_QUADRAN_SPACE),
							startY,
							tileWidth + 1,
							tileHeight + 2);

			if (mapTile != null && mapTile.North != null)
			{
				g.DrawImage(
							mapTile.North[MapViewPanel.Current].Image,
							startX + (2 * TOTAL_QUADRAN_SPACE),
							startY - mapTile.North.Info.TileOffset);

				if (mapTile.North.Info.HumanDoor || mapTile.North.Info.UFODoor)
					g.DrawString(
							"Door",
							Font,
							System.Drawing.Brushes.Black,
							startX + (2 * TOTAL_QUADRAN_SPACE),
							startY + PckImage.Height - Font.Height);
			}
			else
				g.DrawImage(
							Globals.ExtraTiles[2].Image,
							startX + (2 * TOTAL_QUADRAN_SPACE),
							startY);

			if (!topView.ContentVisible)
				g.FillRectangle(
							System.Drawing.Brushes.DarkGray,
							startX + (3 * TOTAL_QUADRAN_SPACE),
							startY,
							tileWidth + 1,
							tileHeight + 2);

			if (mapTile != null && mapTile.Content != null)
			{
				g.DrawImage(
							mapTile.Content[MapViewPanel.Current].Image,
							startX + (3 * TOTAL_QUADRAN_SPACE),
							startY - mapTile.Content.Info.TileOffset);

				if (mapTile.Content.Info.HumanDoor || mapTile.Content.Info.UFODoor)
					g.DrawString(
							"Door",
							Font,
							System.Drawing.Brushes.Black,
							startX + (3 * TOTAL_QUADRAN_SPACE),
							startY + PckImage.Height - Font.Height);
			}
			else
				g.DrawImage(
							Globals.ExtraTiles[4].Image,
							startX + (3 * TOTAL_QUADRAN_SPACE),
							startY);

			DrawGroundAndContent(g);

			g.DrawString(
					"Gnd",
					Font,
					System.Drawing.Brushes.Black,
					new RectangleF(
								startX,
								startY + tileHeight + space,
								tileWidth,
								50));

			g.DrawString(
					"West",
					Font,
					System.Drawing.Brushes.Black,
					new RectangleF(
								startX + (TOTAL_QUADRAN_SPACE),
								startY + tileHeight + space,
								tileWidth,
								50));

			g.DrawString(
					"North",
					Font,
					System.Drawing.Brushes.Black,
					new RectangleF(
								startX + (2 * TOTAL_QUADRAN_SPACE),
								startY + tileHeight + space,
								tileWidth,
								50));

			g.DrawString(
					"Object",
					Font,
					System.Drawing.Brushes.Black,
					new RectangleF(
								startX + (3 * TOTAL_QUADRAN_SPACE),
								startY + tileHeight + space,
								tileWidth + 50,
								50));

			for (int i = 0; i < 4; i++)
				g.DrawRectangle(
							System.Drawing.Pens.Black,
							startX - 1 + (i * TOTAL_QUADRAN_SPACE),
							startY,
							tileWidth + 2,
							tileHeight + 2);
		}

		private void DrawGroundAndContent(Graphics g)
		{
			if (Brushes != null && Pens != null)
			{
				g.FillRectangle(
							Brushes["GroundColor"],
							new RectangleF(
										startX,
										startY + tileHeight + space + Font.Height,
										tileWidth,
										3));

				g.FillRectangle(
							new SolidBrush(Pens["NorthColor"].Color),
							new RectangleF(
										startX + (TOTAL_QUADRAN_SPACE),
										startY + tileHeight + space + Font.Height,
										tileWidth,
										3));

				g.FillRectangle(
							new SolidBrush(Pens["WestColor"].Color),
							new RectangleF(
										startX + (2 * TOTAL_QUADRAN_SPACE),
										startY + tileHeight + space + Font.Height,
										tileWidth,
										3));

				g.FillRectangle(
							Brushes["ContentColor"],
							new RectangleF(
										startX + (3 * TOTAL_QUADRAN_SPACE),
										startY + tileHeight + space + Font.Height,
										tileWidth,
										3));
			}
		}
	}
}
