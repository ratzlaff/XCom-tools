using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Forms;
using System.Drawing;
using System.ComponentModel;
using UtilLib;
using MapLib;
using MapLib.Base;
using WeifenLuo.WinFormsUI.Docking;

namespace ViewLib
{
	public class Map_Observer_Form : DockContent
	{
		protected Settings settings;
		protected MenuItem menuItem;
		protected Map map;

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

			if (!IsDesignMode) {
				MapControl.MapChanged += mapChanged;
				MapControl.HeightChanged += HeightChanged;
				MapControl.SelectedTileChanged += SelectedTileChanged;
				Closing += formClosing;
			}
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
			if (map == null)
				MenuItem.PerformClick();

			this.map = e.Map;
			Refresh();
			OnResize(e);
		}

		public virtual void HeightChanged(Map sender, HeightChangedEventArgs e) { Refresh(); }
		public virtual void SelectedTileChanged(Map sender, SelectedTileChangedEventArgs e) { Refresh(); }
		public virtual void SetupDefaultSettings() { }

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
