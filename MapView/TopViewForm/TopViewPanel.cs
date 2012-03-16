using System;
using System.Collections.Generic;
using System.Text;
using System.Drawing;
using XCom.Interfaces.Base;
using XCom;
using System.Windows.Forms;
using ViewLib.Base;
using UtilLib;

namespace MapView.TopViewForm
{
	public class TopViewPanel : ViewLib.Base.SimpleMapPanel
	{
		private bool blank = false;

		public TopViewPanel()
		{
		}

		public ToolStripMenuItem Ground { get; set; }
		public ToolStripMenuItem North { get; set; }
		public ToolStripMenuItem West { get; set; }
		public ToolStripMenuItem Content { get; set; }
		public BottomPanel BottomPanel { get; set; }

		protected override void RenderCell(System.Drawing.Graphics g, int x, int y, int row, int col)
		{
			XCMapTile mapTile = (XCMapTile)map[row, col];
			if (!blank) {
				if (mapTile.Ground != null && Ground.Checked)
					g.FillPath(brushes["GroundColor"], UpperPath(x, y));

				if (mapTile.North != null && North.Checked)
					g.DrawLine(pens["NorthColor"], x, y, x + hWidth, y + hHeight);

				if (mapTile.West != null && West.Checked)
					g.DrawLine(pens["WestColor"], x, y, x - hWidth, y + hHeight);

				if (mapTile.Content != null && Content.Checked)
					g.FillPath(brushes["ContentColor"], LowerPath(x, y));
			} else {
				if (!mapTile.DrawAbove) {
					g.FillPath(System.Drawing.Brushes.DarkGray, UpperPath(x, y));
					g.FillPath(System.Drawing.Brushes.DarkGray, LowerPath(x, y));
				}
			}
		}

		protected override void OnMouseDown(MouseEventArgs e)
		{
			base.OnMouseDown(e);

			if (e.Button == MouseButtons.Right)
				BottomPanel.SetSelected(e.Button, 1);
			else if (e.Button == MouseButtons.Left)
				viewDrag(null, null);
		}

		public override void SetupDefaultSettings(Map_Observer_Form sender)
		{
			base.SetupDefaultSettings(sender);

			// NorthColor, NorthWidth
			addPenSetting(new Pen(new SolidBrush(Color.Red), 4), "North", "Tile", "Color of the north tile indicator", "Width of the north tile indicator in pixels", sender.Settings);
			brushes.Add("NorthColor", new SolidBrush(pens["NorthColor"].Color));

			//WestColor, WestWidth
			addPenSetting(new Pen(new SolidBrush(Color.Red), 4), "West", "Tile", "Color of the west tile indicator", "Width of the west tile indicator in pixels", sender.Settings);
			brushes.Add("WestColor", new SolidBrush(pens["WestColor"].Color));

			// GroundColor
			addBrushSetting(new SolidBrush(Color.Orange), "Ground", "Tile", "Color of the ground tile indicator", sender.Settings);

			// ContentColor
			addBrushSetting(new SolidBrush(Color.Green), "Content", "Tile", "Color of the content tile indicator", sender.Settings);

			// DiamondMinHeight
			sender.Settings.AddSetting("DiamondMinHeight", MinHeight, "Minimum height of the grid tiles", "Tile", diamondHeightChanged);
		}
	}
}
