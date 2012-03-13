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

		[Browsable(false)]
		public Dictionary<string, SolidBrush> FillBrushes { get; set; }

		[Browsable(false)]
		public Dictionary<string, Pen> DrawPens { get; set; }

		// this is the menu item to show in a View menu
		// it gets set by the Form that owns the menu bar
		[Browsable(false)]
		public MenuItem MenuItem { get; set; }

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
			MenuItem = new MenuItem();

			this.StartPosition = FormStartPosition.Manual;

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

		public virtual void SetupDefaultSettings(Settings settings)
		{
			this.settings = settings;

			Setting s = settings.AddSetting("X", Left, "Starting X-coordinate of the window", "Window", settingChanged, "Left", this);
			s.IsVisible = false;

			s = settings.AddSetting("Y", Top, "Starting Y-coordinate of the window", "Window", settingChanged, "Top", this);
			s.IsVisible = false;

			s = settings.AddSetting("Width", Width, "Starting Width of the window", "Window", settingChanged, "Width", this);
			s.IsVisible = false;

			s = settings.AddSetting("Height", Height, "Starting Height of the window", "Window", settingChanged, "Height", this);
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
