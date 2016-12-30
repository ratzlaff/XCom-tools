using System.Collections.Generic;
using System.ComponentModel;
using System.Windows.Forms;
using DSShared.Windows;
using XCom.Interfaces.Base;

namespace MapView
{
	public class MapObserverControl
		:
		UserControl,
		IMap_Observer
	{
		private IMap_Base _map;

		private RegistryInfo _registryInfo;
		private readonly Dictionary<string, IMap_Observer> _moreObservers;

		public MapObserverControl()
		{
			_moreObservers = new Dictionary<string, IMap_Observer>();
			Settings = new Settings();
		}

		public virtual void LoadDefaultSettings()
		{}

		public Settings Settings
		{ get; set; }

		public Dictionary<string, IMap_Observer> MoreObservers
		{
			get { return _moreObservers; }
		}

		[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
		public RegistryInfo RegistryInfo
		{
			get { return _registryInfo; }
			set
			{
				_registryInfo = value;
				value.Loading+= (sender, e) => OnRISettingsLoad(e);
				value.Saving += (sender, e) => OnRISettingsSave(e);
			}
		}

		protected virtual void OnRISettingsSave(RegistrySaveLoadEventArgs e)
		{}

		protected virtual void OnRISettingsLoad(RegistrySaveLoadEventArgs e)
		{}

		[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
		public virtual IMap_Base Map
		{
			get { return _map; }
			set { _map = value; Refresh(); }
		}

		protected override void OnMouseWheel(MouseEventArgs e)
		{
			base.OnMouseWheel(e);
			if (e.Delta < 0)
				_map.Up();
			else
				_map.Down();
		}

		public virtual void HeightChanged(IMap_Base sender, HeightChangedEventArgs e)
		{
			Refresh();
		}

		public virtual void SelectedTileChanged(IMap_Base sender, SelectedTileChangedEventArgs e)
		{
			Refresh();
		}
	}
}
