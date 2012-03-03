using System;
using System.Collections.Generic;
using System.Text;
using System.Drawing;
using XCom.Interfaces.Base;
using XCom;
using System.Windows.Forms;

namespace MapView.TopViewForm
{
	public class TopViewPanel : SimpleMapPanel
	{
		private bool blank = false;

		public TopViewPanel()
		{
			if (!IsDesignMode)
				MapViewPanel.Instance.View.DragChanged += new EventHandler(viewDrag);
		}

		public MenuItem Ground { get; set; }
		public MenuItem North { get; set; }
		public MenuItem West { get; set; }
		public MenuItem Content { get; set; }
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

		public override void LoadDefaultSettings(Settings settings)
		{
			base.LoadDefaultSettings(settings);

			// NorthColor, NorthWidth
			addPenSetting(new Pen(new SolidBrush(Color.Red), 4), "North", "Tile", "Color of the north tile indicator", "Width of the north tile indicator in pixels", settings);
			brushes.Add("NorthColor", new SolidBrush(pens["NorthColor"].Color));

			//WestColor, WestWidth
			addPenSetting(new Pen(new SolidBrush(Color.Red), 4), "West", "Tile", "Color of the west tile indicator", "Width of the west tile indicator in pixels", settings);
			brushes.Add("WestColor", new SolidBrush(pens["WestColor"].Color));

			// GroundColor
			addBrushSetting(new SolidBrush(Color.Orange), "Ground", "Tile", "Color of the ground tile indicator", settings);

			// ContentColor
			addBrushSetting(new SolidBrush(Color.Green), "Content", "Tile", "Color of the content tile indicator", settings);

			// DiamondMinHeight
			settings.AddSetting("DiamondMinHeight", MinHeight, "Minimum height of the grid tiles", "Tile", diamondHeightChanged, false, null);
		}

		private void InitializeComponent()
		{
			this.SuspendLayout();
			// 
			// TopViewPanel
			// 
			this.Name = "TopViewPanel";
			this.Size = new System.Drawing.Size(383, 209);
			this.ResumeLayout(false);

		}
	}
}
