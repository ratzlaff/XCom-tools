using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Forms;
using System.Drawing;
using System.ComponentModel;
using MVCore;
using MapLib;
using MapLib.Base;

namespace ViewLib.Base
{
	public class Map_Observer_Form : Form
	{
		protected Settings settings;
		protected MenuItem menuItem;

		[Browsable(false)]
		public Dictionary<string, SolidBrush> FillBrushes { get; set; }

		[Browsable(false)]
		public Dictionary<string, Pen> DrawPens { get; set; }

		// this is the menu item to show in a View menu
		// it gets set by the Form that owns the menu bar
		[Browsable(false)]
		public MenuItem MenuItem
		{
			get
			{
				if (menuItem == null) {
					menuItem = new MenuItem(Text);
					menuItem.Tag = this;
				}

				return menuItem;
			}
		}

		protected Map map;

		[Browsable(false)]
		public bool IsDesignMode
		{
			get { return DesignMode || LicenseManager.UsageMode == LicenseUsageMode.Designtime || AppDomain.CurrentDomain.FriendlyName == "DefaultDomain"; }
		}

		public Map_Observer_Form()
		{
			settings = new Settings();
			FillBrushes = new Dictionary<string, SolidBrush>();
			DrawPens = new Dictionary<string, Pen>();

			this.StartPosition = FormStartPosition.Manual;
			this.ShowInTaskbar = false;
			this.FormBorderStyle = FormBorderStyle.SizableToolWindow;

			if (!IsDesignMode) {
				MapControl.MapChanged += mapChanged;
				MapControl.HeightChanged += HeightChanged;
				MapControl.SelectedTileChanged += SelectedTileChanged;
			}
		}

		public Settings Settings
		{
			get { return settings; }
		}

		protected virtual void mapChanged(MapChangedEventArgs e)
		{
			this.map = e.Map;
			Refresh();
		}

		public virtual void HeightChanged(Map sender, HeightChangedEventArgs e) { Refresh(); }
		public virtual void SelectedTileChanged(Map sender, SelectedTileChangedEventArgs e) { Refresh(); }

		private void settingChanged(Setting sender, string keyword, object val)
		{
			switch (keyword) {
				case "Left":
					this.Left = (int)val;
					break;
				case "Top":
					this.Top = (int)val;
					break;
				case "Width":
					this.Width = (int)val;
					break;
				case "Height":
					this.Height = (int)val;
					break;
			}
		}

		public virtual void SetupDefaultSettings()
		{
			Setting s = settings.AddSetting("X", "Starting X-coordinate of the window", "Window", "Left", this);
			s.IsVisible = false;

			s = settings.AddSetting("Y", "Starting Y-coordinate of the window", "Window", "Top", this);
			s.IsVisible = false;

			s = settings.AddSetting("Width", "Starting Width of the window", "Window", "Width", this);
			s.IsVisible = false;

			s = settings.AddSetting("Height", "Starting Height of the window", "Window", "Height", this);
			s.IsVisible = false;
		}

		protected override void OnMouseWheel(MouseEventArgs e)
		{
			base.OnMouseWheel(e);
			if (map != null) {
				if (e.Delta > 0)
					map.Up();
				else
					map.Down();
			}
		}
	}
}
