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
	public partial class TopView : Map_Observer_Form
	{
		private Dictionary<MenuItem, int> visibleHash;
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

			SuspendLayout();
			this.Menu = new MainMenu();
			MenuItem vis = Menu.MenuItems.Add("Visible");

			visibleHash = new Dictionary<MenuItem, int>();

			topViewPanel.Ground = vis.MenuItems.Add("Ground", new EventHandler(visibleClick));
			visibleHash[topViewPanel.Ground] = 0;
			topViewPanel.Ground.Shortcut = Shortcut.F1;

			topViewPanel.West = vis.MenuItems.Add("West", new EventHandler(visibleClick));
			visibleHash[topViewPanel.West] = 1;
			topViewPanel.West.Shortcut = Shortcut.F2;

			topViewPanel.North = vis.MenuItems.Add("North", new EventHandler(visibleClick));
			visibleHash[topViewPanel.North] = 2;
			topViewPanel.North.Shortcut = Shortcut.F3;

			topViewPanel.Content = vis.MenuItems.Add("Content", new EventHandler(visibleClick));
			visibleHash[topViewPanel.Content] = 3;
			topViewPanel.Content.Shortcut = Shortcut.F4;

			MenuItem edit = Menu.MenuItems.Add("Edit");
			edit.MenuItems.Add("Options", new EventHandler(options_click));
			edit.MenuItems.Add("Fill", new EventHandler(fill_click));

			topViewPanel.BottomPanel = bottom;

			MoreObservers.Add("BottomPanel", bottom);
			MoreObservers.Add("TopViewPanel", topViewPanel);

			ResumeLayout();
		}

		public BottomPanel BottomPanel
		{
			get { return bottom; }
		}

		private void visibleClick(object sender, EventArgs e)
		{
			((MenuItem)sender).Checked = !((MenuItem)sender).Checked;

			if (VisibleTileChanged != null)
				VisibleTileChanged(this, new EventArgs());

			MapViewPanel.Instance.Refresh();

			Refresh();
		}

		protected override void OnRISettingsLoad(DSShared.Windows.RegistrySaveLoadEventArgs e)
		{
			bottom.Height = 74;
			RegistryKey riKey = e.OpenKey;

			foreach (MenuItem mi in visibleHash.Keys)
				mi.Checked = bool.Parse((string)riKey.GetValue("vis" + visibleHash[mi].ToString(), "true"));
		}

		protected override void OnRISettingsSave(DSShared.Windows.RegistrySaveLoadEventArgs e)
		{
			RegistryKey riKey = e.OpenKey;
			foreach (MenuItem mi in visibleHash.Keys)
				riKey.SetValue("vis" + visibleHash[mi].ToString(), mi.Checked);
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
					((XCMapTile)MapViewPanel.Instance.View.Map[r, c])[bottom.SelectedQuadrant] = TileView.Instance.SelectedTile;
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
