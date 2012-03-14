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

		public Map_Observer_Form()
		{
			settings = new Settings();
			FillBrushes = new Dictionary<string, SolidBrush>();
			DrawPens = new Dictionary<string, Pen>();

			this.StartPosition = FormStartPosition.Manual;
			this.ShowInTaskbar = false;
			this.FormBorderStyle = FormBorderStyle.SizableToolWindow;

			LoadedVisible = true;
			cachedProperties = new Dictionary<System.Reflection.PropertyInfo, object>();
			this.Load += formLoad;

			if (!IsDesignMode) {
				MapControl.MapChanged += mapChanged;
				MapControl.HeightChanged += HeightChanged;
				MapControl.SelectedTileChanged += SelectedTileChanged;
				Closing += formClosing;
			}
		}

		protected virtual void formLoad(object sender, EventArgs e)
		{
			foreach (System.Reflection.PropertyInfo pi in cachedProperties.Keys)
				pi.SetValue(this, cachedProperties[pi], new object[] { });
		}

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
					menuItem.Click += formMIClick;
				}

				return menuItem;
			}
		}

		private Dictionary<System.Reflection.PropertyInfo, object> cachedProperties;

		public bool CacheProperty(System.Reflection.PropertyInfo inProp, object val)
		{
			if (inProp.Name == "Width") {
				cachedProperties.Add(inProp, val);
				return true;
			}

			if (inProp.Name == "Height") {
				cachedProperties.Add(inProp, val);
				return true;
			}

			return false;
		}

		protected virtual void formClosing(object sender, CancelEventArgs e)
		{
			e.Cancel = true;
			MenuItem.Checked = false;
			Hide();
		}

		protected virtual void formMIClick(object sender, EventArgs e)
		{
			if (!MenuItem.Checked) {
				Show();
				WindowState = FormWindowState.Normal;
				MenuItem.Checked = true;
			} else {
				Close();
				MenuItem.Checked = false;
			}
		}

		protected Map map;

		[Browsable(false)]
		public bool IsDesignMode
		{
			get { return DesignMode || LicenseManager.UsageMode == LicenseUsageMode.Designtime || AppDomain.CurrentDomain.FriendlyName == "DefaultDomain"; }
		}

		public Settings Settings
		{
			get { return settings; }
		}

		protected virtual void mapChanged(MapChangedEventArgs e)
		{
			if (map == null) {
				if (loadedVisible)
					MenuItem.PerformClick();
			}

			this.map = e.Map;
			Refresh();
		}

		public virtual void HeightChanged(Map sender, HeightChangedEventArgs e) { Refresh(); }
		public virtual void SelectedTileChanged(Map sender, SelectedTileChangedEventArgs e) { Refresh(); }

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

			s = settings.AddSetting("LoadedVisible", "Whether or not the window starts out shown", "Window", "LoadedVisible", this);
			s.IsVisible = false;
		}

		private bool loadedVisible = false;
		[Browsable(false)]
		public bool LoadedVisible
		{
			get { return Visible; }
			set { loadedVisible = value; }
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
