using System;
using System.ComponentModel;
using System.Collections.Generic;
using System.Text;
using XCom.Interfaces.Base;

namespace MapView
{
	public class Map_Observer_Control : ViewLib.Base.DoubleBufferControl, XCom.Interfaces.Base.IMap_Observer
	{
		protected IMap_Base map;
		private DSShared.Windows.RegistryInfo registryInfo;

		public Map_Observer_Control()
		{

		}

		#region IMap_Observer Members

		[Browsable(false)]
		[DefaultValue(null)]
		public virtual XCom.Interfaces.Base.IMap_Base Map
		{
			get { return map; }
			set { map = value; Refresh(); }
		}

		public virtual void HeightChanged(IMap_Base sender, HeightChangedEventArgs e) { Refresh(); }

		public virtual void SelectedTileChanged(IMap_Base sender, SelectedTileChangedEventArgs e) { Refresh(); }

		[Browsable(false)]
		[DefaultValue(null)]
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

		#endregion
	}
}
