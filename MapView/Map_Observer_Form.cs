using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Forms;
using XCom.Interfaces.Base;
using XCom;

namespace MapView
{
	public class Map_Observer_Form:Form
	{
		protected IMap_Base map;
		private DSShared.Windows.RegistryInfo registryInfo;
		private Settings settings;

		public Map_Observer_Form()
		{
			MoreObservers = new Dictionary<string, Map_Observer_Control>();
			settings = new Settings();
			Load += new EventHandler(Map_Observer_Form_Load);			
		}

		private void Map_Observer_Form_Load(object sender, EventArgs e)
		{
			LoadDefaultSettings(settings);
		}

		protected virtual void LoadDefaultSettings(Settings settings){}

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
				value.Loading+=delegate(object sender, DSShared.Windows.RegistrySaveLoadEventArgs e)
				{
					OnRISettingsLoad(e);
				};
				value.Saving += delegate(object sender, DSShared.Windows.RegistrySaveLoadEventArgs e)
				{
					OnRISettingsSave(e);
				};
			}
		}

		protected virtual void OnRISettingsSave(DSShared.Windows.RegistrySaveLoadEventArgs e) { }
		protected virtual void OnRISettingsLoad(DSShared.Windows.RegistrySaveLoadEventArgs e) { }

		public virtual IMap_Base Map
		{
			get { return map; }
			set
			{
				if (map != null) {
					map.HeightChanged -= HeightChanged;
					map.SelectedTileChanged -= SelectedTileChanged;
				}

				map = value;

				if (map != null) {
					map.HeightChanged += HeightChanged;
					map.SelectedTileChanged += SelectedTileChanged;
				}

				foreach (string key in MoreObservers.Keys)
					MoreObservers[key].Map = value;
			}
		}

        protected override void OnMouseWheel(MouseEventArgs e)
        {
            base.OnMouseWheel(e);
            if (e.Delta > 0)
                map.Up();
            else
                map.Down();
        }

		public virtual void HeightChanged(IMap_Base sender, HeightChangedEventArgs e) { Refresh(); }
		public virtual void SelectedTileChanged(IMap_Base sender, SelectedTileChangedEventArgs e) { Refresh(); }
    }
}
