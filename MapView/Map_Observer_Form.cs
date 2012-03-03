using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Forms;
using System.Drawing;
using XCom.Interfaces.Base;
using System.ComponentModel;
using XCom;

namespace MapView
{
	public class Map_Observer_Form : Form
	{
		private DSShared.Windows.RegistryInfo registryInfo;
		private Settings settings;

		protected Dictionary<string, SolidBrush> brushes;
		protected Dictionary<string, Pen> pens;

		protected IMap_Base map;

		[Browsable(false)]
		public bool IsDesignMode
		{
			get { return DesignMode || LicenseManager.UsageMode == LicenseUsageMode.Designtime || AppDomain.CurrentDomain.FriendlyName == "DefaultDomain"; }
		}

		public Map_Observer_Form()
		{
			MoreObservers = new Dictionary<string, Map_Observer_Control>();
			settings = new Settings();
			Load += new EventHandler(formLoad);
			brushes = new Dictionary<string, SolidBrush>();
			pens = new Dictionary<string, Pen>();
			if (!IsDesignMode)
				MainWindow.Instance.MapChanged += mapChanged;
		}

		private void formLoad(object sender, EventArgs e)
		{
			LoadDefaultSettings(settings);

			foreach (string key in MoreObservers.Keys) {
				MoreObservers[key].SetDrawingTools(brushes, pens);
				MoreObservers[key].LoadDefaultSettings(settings);
			}
		}

		protected virtual void LoadDefaultSettings(Settings settings) { }

		protected virtual void mapChanged(object sender, IMap_Base map)
		{
			if (map != null) {
				map.HeightChanged -= HeightChanged;
				map.SelectedTileChanged -= SelectedTileChanged;
			}

			this.map = map;

			if (map != null) {
				map.HeightChanged += HeightChanged;
				map.SelectedTileChanged += SelectedTileChanged;
			}
		}

		public virtual void HeightChanged(IMap_Base sender, HeightChangedEventArgs e) { Refresh(); }
		public virtual void SelectedTileChanged(IMap_Base sender, SelectedTileChangedEventArgs e) { Refresh(); }

		public Settings Settings
		{
			get { return settings; }
		}

		public Dictionary<string, Map_Observer_Control> MoreObservers { get; set; }

		public MenuItem MenuItem { get; set; }

		public DSShared.Windows.RegistryInfo RegistryInfo
		{
			get { return registryInfo; }
			set
			{
				registryInfo = value;
				value.Loading += delegate(object sender, DSShared.Windows.RegistrySaveLoadEventArgs e) {
					OnRISettingsLoad(e);
				};

				value.Saving += delegate(object sender, DSShared.Windows.RegistrySaveLoadEventArgs e) {
					OnRISettingsSave(e);
				};
			}
		}

		protected virtual void OnRISettingsSave(DSShared.Windows.RegistrySaveLoadEventArgs e) { }
		protected virtual void OnRISettingsLoad(DSShared.Windows.RegistrySaveLoadEventArgs e) { }

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
