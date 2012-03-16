using System;
using System.ComponentModel;
using System.Collections.Generic;
using System.Windows.Forms;
using System.Drawing;
using System.Reflection;
using XCom;
using XCom.Interfaces.Base;
using ViewLib.Base;
using UtilLib;
using MapLib;
using MapLib.Base;

namespace MapView.TopViewForm
{
	public class BottomPanel : Map_Observer_Control
	{
		private XCMapTile mapTile;
		private MapLocation lastLoc;
		private Font font;

		private const int tileWidth = 32;
		private const int tileHeight = 40;
		private int space = 2;
		private int startX = 5, startY = 5;

		private XCMapTile.MapQuadrant selected;
		public event EventHandler PanelClicked;

		public BottomPanel()
		{
			mapTile = null;
			font = new Font("Arial", 8);
			SetStyle(ControlStyles.AllPaintingInWmPaint | ControlStyles.DoubleBuffer | ControlStyles.UserPaint | ControlStyles.ResizeRedraw, true);
			selected = XCMapTile.MapQuadrant.Ground;

			if (IsDesignMode) {
				brushes = new Dictionary<string,SolidBrush>();
				brushes["GroundColor"] = new SolidBrush(Color.Orange);
				brushes["NorthColor"] = new SolidBrush(Color.Red);
				brushes["WestColor"] = new SolidBrush(Color.Red);
				brushes["ContentColor"] = new SolidBrush(Color.Green);
			}
		}

		[Browsable(false)]
		public XCMapTile Tile
		{
			get { return mapTile; }
			set { mapTile = value; Refresh(); }
		}

		public XCMapTile.MapQuadrant SelectedQuadrant
		{
			get { return selected; }
		}

		public void SetSelected(MouseButtons btn, int clicks)
		{
			if (btn == MouseButtons.Right && mapTile != null) {
				if (clicks == 1)
					mapTile[selected] = TileView.Instance.SelectedTile;
				else if (clicks == 2)
					mapTile[selected] = null;
				Globals.MapChanged = true;
			} else if (btn == MouseButtons.Left && mapTile != null) {
				if (clicks == 2)
					TileView.Instance.SelectedTile = mapTile[selected];
			}
		}

		public override void HeightChanged(Map sender, HeightChangedEventArgs e)
		{
			lastLoc.Height = e.NewHeight;
			mapTile = (XCMapTile)map[lastLoc.Row, lastLoc.Col];
			Refresh();
		}

		public override void SelectedTileChanged(Map sender, SelectedTileChangedEventArgs e)
		{
			mapTile = (XCMapTile)e.SelectedTile;
			lastLoc = e.MapLocation;
			Refresh();
		}

		private void visChanged(object sender, EventArgs e)
		{
			Refresh();
		}

		protected override void OnMouseDown(MouseEventArgs e)
		{
			selected = (XCMapTile.MapQuadrant)((e.X - startX) / (tileWidth + 2 * space));

			SetSelected(e.Button, e.Clicks);

			if (PanelClicked != null)
				PanelClicked(this, new EventArgs());

			Refresh();
		}

		protected override void OnPaint(PaintEventArgs e)
		{
			Graphics g = e.Graphics;
			if (!DesignMode) {
				if (selected == XCMapTile.MapQuadrant.Ground)
					g.FillRectangle(brushes["SelectTileColor"], startX, startY, tileWidth + 1, tileHeight + 2);
				else if (selected == XCMapTile.MapQuadrant.West)
					g.FillRectangle(brushes["SelectTileColor"], startX + ((tileWidth + 2 * space)), startY, tileWidth + 1, tileHeight + 2);
				else if (selected == XCMapTile.MapQuadrant.North)
					g.FillRectangle(brushes["SelectTileColor"], startX + (2 * (tileWidth + 2 * space)), startY, tileWidth + 1, tileHeight + 2);
				else if (selected == XCMapTile.MapQuadrant.Content)
					g.FillRectangle(brushes["SelectTileColor"], startX + (3 * (tileWidth + 2 * space)), startY, tileWidth + 1, tileHeight + 2);

				if (!TopView.Instance.GroundVisible)
					g.FillRectangle(System.Drawing.Brushes.DarkGray, startX, startY, tileWidth + 1, tileHeight + 2);

				if (!TopView.Instance.WestVisible)
					g.FillRectangle(System.Drawing.Brushes.DarkGray, startX + ((tileWidth + 2 * space)), startY, tileWidth + 1, tileHeight + 2);

				if (!TopView.Instance.NorthVisible)
					g.FillRectangle(System.Drawing.Brushes.DarkGray, startX + (2 * (tileWidth + 2 * space)), startY, tileWidth + 1, tileHeight + 2);

				if (!TopView.Instance.ContentVisible)
					g.FillRectangle(System.Drawing.Brushes.DarkGray, startX + (3 * (tileWidth + 2 * space)), startY, tileWidth + 1, tileHeight + 2);
			}

			if (mapTile != null && mapTile.Ground != null) {
				g.DrawImage(mapTile.Ground[MapViewPanel.Current].Image, startX, startY - mapTile.Ground.YOffset);
				if (mapTile.Ground.IsDoor)
					g.DrawString("Door", Font, System.Drawing.Brushes.Black, startX, startY + PckImage.Height - Font.Height);
			} else
				g.DrawImage(Globals.ExtraTiles[3].Image, startX, startY);

			if (mapTile != null && mapTile.West != null) {
				g.DrawImage(mapTile.West[MapViewPanel.Current].Image, startX + ((tileWidth + 2 * space)), startY - mapTile.West.YOffset);
				if (mapTile.West.IsDoor)
					g.DrawString("Door", Font, System.Drawing.Brushes.Black, startX + ((tileWidth + 2 * space)), startY + PckImage.Height - Font.Height);
			} else
				g.DrawImage(Globals.ExtraTiles[1].Image, startX + ((tileWidth + 2 * space)), startY);

			if (mapTile != null && mapTile.North != null) {
				g.DrawImage(mapTile.North[MapViewPanel.Current].Image, startX + (2 * (tileWidth + 2 * space)), startY - mapTile.North.YOffset);
				if (mapTile.North.IsDoor)
					g.DrawString("Door", Font, System.Drawing.Brushes.Black, startX + (2 * (tileWidth + 2 * space)), startY + PckImage.Height - Font.Height);
			} else
				g.DrawImage(Globals.ExtraTiles[2].Image, startX + (2 * (tileWidth + 2 * space)), startY);

			if (mapTile != null && mapTile.Content != null) {
				g.DrawImage(mapTile.Content[MapViewPanel.Current].Image, startX + (3 * (tileWidth + 2 * space)), startY - mapTile.Content.YOffset);
				if (mapTile.Content.IsDoor)
					g.DrawString("Door", Font, System.Drawing.Brushes.Black, startX + (3 * (tileWidth + 2 * space)), startY + PckImage.Height - Font.Height);
			} else
				g.DrawImage(Globals.ExtraTiles[4].Image, startX + (3 * (tileWidth + 2 * space)), startY);

			g.FillRectangle(brushes["GroundColor"], new RectangleF(startX, startY + tileHeight + space + font.Height, tileWidth, 3));
			g.FillRectangle(brushes["NorthColor"], new RectangleF(startX + ((tileWidth + 2 * space)), startY + tileHeight + space + font.Height, tileWidth, 3));
			g.FillRectangle(brushes["WestColor"], new RectangleF(startX + (2 * (tileWidth + 2 * space)), startY + tileHeight + space + font.Height, tileWidth, 3));
			g.FillRectangle(brushes["ContentColor"], new RectangleF(startX + (3 * (tileWidth + 2 * space)), startY + tileHeight + space + font.Height, tileWidth, 3));

			g.DrawString("Gnd", font, System.Drawing.Brushes.Black, new RectangleF(startX, startY + tileHeight + space, tileWidth, 50));
			g.DrawString("West", font, System.Drawing.Brushes.Black, new RectangleF(startX + ((tileWidth + 2 * space)), startY + tileHeight + space, tileWidth, 50));
			g.DrawString("North", font, System.Drawing.Brushes.Black, new RectangleF(startX + (2 * (tileWidth + 2 * space)), startY + tileHeight + space, tileWidth, 50));
			g.DrawString("Content", font, System.Drawing.Brushes.Black, new RectangleF(startX + (3 * (tileWidth + 2 * space)), startY + tileHeight + space, tileWidth + 50, 50));

			for (int i = 0; i < 4; i++)
				g.DrawRectangle(System.Drawing.Pens.Black, startX - 1 + (i * (tileWidth + 2 * space)), startY, tileWidth + 2, tileHeight + 2);
		}

		protected override void penColorChanged(object sender, string key, object val)
		{
			if (key == "NorthColor")
				brushChanged(sender, key, val);
			else if (key == "WestColor")
				brushChanged(sender, key, val);

			base.penColorChanged(sender, key, val);
		}

		public override void SetupDefaultSettings(Map_Observer_Form sender)
		{
			base.SetupDefaultSettings(sender);
			// SelectTileColor
			addBrushSetting(new SolidBrush(Color.FromArgb(204, 204, 255)), "SelectTile", "Other", "Background color of the selected tile piece", sender.Settings);
		}

		protected override void OnResize(EventArgs e)
		{
			base.OnResize(e);

			startX = (Width - (4 * (tileWidth + 2 * space))) / 2;
		}
	}
}
