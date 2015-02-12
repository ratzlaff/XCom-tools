using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Forms;
using DSShared.Windows;
using XCom.Interfaces.Base;
using XCom;

namespace MapView
{
    public class Map_Observer_Form : Form, IMap_Observer, IMenuItem
	{
        private IMap_Base map;
		private RegistryInfo registryInfo;
        private readonly Settings _settings;
		private readonly Dictionary<string, IMap_Observer> _moreObservers;

		public Map_Observer_Form()
		{
			_moreObservers = new Dictionary<string, IMap_Observer>();
			_settings = new Settings();
			Load += Map_Observer_Form_Load;
        }

		private void Map_Observer_Form_Load(object sender, EventArgs e)
		{
        }

		public virtual void LoadDefaultSettings(   ){}

		public Settings Settings
		{
			get { return _settings; }
		}

		public Dictionary<string, IMap_Observer> MoreObservers
		{
			get { return _moreObservers; }
		}

        public MenuItem MenuItem { get; set; }

        public RegistryInfo RegistryInfo
		{
			get { return registryInfo; }
			set
			{
				registryInfo = value;
				value.Loading+= (sender, e) => OnRISettingsLoad(e);
				value.Saving += (sender, e) => OnRISettingsSave(e);
			}
		}

		protected virtual void OnRISettingsSave(RegistrySaveLoadEventArgs e) { }
		protected virtual void OnRISettingsLoad(RegistrySaveLoadEventArgs e) { }

		public virtual IMap_Base Map
		{
			get { return map; }
			set { map = value; Refresh(); }
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
