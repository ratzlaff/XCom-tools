using System;
using System.Drawing;
using System.Collections.Generic;
using System.ComponentModel;
using System.Windows.Forms;
using XCom;
using XCom.Interfaces;
using System.Drawing.Drawing2D;
using Microsoft.Win32;
using XCom.Interfaces.Base;
using System.Reflection;

namespace MapView.TopViewForm
{
	public partial class TopView : ViewLib.Base.Map_Observer_Form
	{
		private Dictionary<ToolStripMenuItem, MVCore.Setting> visibleHash;
		public event EventHandler VisibleTileChanged;

		#region Singleton access
		private static TopView myInstance = null;
		public static TopView Instance
		{
			get
			{
				if (myInstance == null)
					myInstance = new TopView();

				return myInstance;
			}
		}
		#endregion

		private TopView()
		{
			InitializeComponent();

			topViewPanel.ScrollPanel = center;

			MainWindow.Instance.MakeToolstrip(toolStrip);

			visibleHash = new Dictionary<ToolStripMenuItem, MVCore.Setting>();

			topViewPanel.Ground = miGround;
			topViewPanel.West = miWest;
			topViewPanel.North = miNorth;
			topViewPanel.Content = miContent;

			topViewPanel.BottomPanel = bottom;
		}

		public override void SetupDefaultSettings(MVCore.Settings settings)
		{
			base.SetupDefaultSettings(settings);

			bottom.LoadDefaultSettings(this, settings);
			topViewPanel.LoadDefaultSettings(this, settings);

			visibleHash[topViewPanel.Ground] = settings.AddSetting("GroundVisible", true, "Show ground portion", "Tile", visibleSetting);
			visibleHash[topViewPanel.West] = settings.AddSetting("WestVisible", true, "Show west portion", "Tile", visibleSetting);
			visibleHash[topViewPanel.North] = settings.AddSetting("NorthVisible", true, "Show north portion", "Tile", visibleSetting);
			visibleHash[topViewPanel.Content] = settings.AddSetting("ContentVisible", true, "Show content portion", "Tile", visibleSetting);
		}

		private void visibleSetting(object sender, string key, object inValue)
		{
			switch (key) {
				case "GroundVisible":
					miGround.Checked = (bool)inValue;
					break;
				case "WestVisible":
					miWest.Checked = (bool)inValue;
					break;
				case "NorthVisible":
					miNorth.Checked = (bool)inValue;
					break;
				case "ContentVisible":
					miContent.Checked = (bool)inValue;
					break;
			}
		}

		public BottomPanel BottomPanel
		{
			get { return bottom; }
		}

		private void visibleClick(object sender, EventArgs e)
		{
			ToolStripMenuItem itm = (ToolStripMenuItem)sender;
			itm.Checked = !itm.Checked;
			visibleHash[itm].Value = itm.Checked;

			if (VisibleTileChanged != null)
				VisibleTileChanged(this, new EventArgs());

			MapViewPanel.Instance.Refresh();

			Refresh();
		}

		private void fill_click(object sender, EventArgs evt)
		{
			Point s = new Point(0, 0);
			Point e = new Point(0, 0);

			s.X = Math.Min(MapViewPanel.Instance.View.StartDrag.X, MapViewPanel.Instance.View.EndDrag.X);
			s.Y = Math.Min(MapViewPanel.Instance.View.StartDrag.Y, MapViewPanel.Instance.View.EndDrag.Y);

			e.X = Math.Max(MapViewPanel.Instance.View.StartDrag.X, MapViewPanel.Instance.View.EndDrag.X);
			e.Y = Math.Max(MapViewPanel.Instance.View.StartDrag.Y, MapViewPanel.Instance.View.EndDrag.Y);

			//row   col
			//y     x

			for (int c = s.X; c <= e.X; c++)
				for (int r = s.Y; r <= e.Y; r++)
					((XCMapTile)MapLib.Base.MapControl.Current[r, c])[bottom.SelectedQuadrant] = TileView.Instance.SelectedTile;
			Globals.MapChanged = true;
			MapViewPanel.Instance.Refresh();
			Refresh();
		}

		private void options_click(object sender, EventArgs e)
		{
			PropertyForm pf = new PropertyForm("TopViewType", Settings);
			pf.Text = "TopView Options";
			pf.Show();
		}

		public bool GroundVisible
		{
			get { return topViewPanel.Ground.Checked; }
		}

		public bool NorthVisible
		{
			get { return topViewPanel.North.Checked; }
		}

		public bool WestVisible
		{
			get { return topViewPanel.West.Checked; }
		}

		public bool ContentVisible
		{
			get { return topViewPanel.Content.Checked; }
		}

		private void btnUp_Click(object sender, EventArgs e)
		{
			map.Up();
		}

		private void btnDown_Click(object sender, EventArgs e)
		{
			map.Down();
		}
	}
}
