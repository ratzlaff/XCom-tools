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

		public Map_Observer_Control()
		{

		}

		#region IMap_Observer Members

		[Browsable(false)]
		[DefaultValue(null)]
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
			}
		}

		public virtual void HeightChanged(IMap_Base sender, HeightChangedEventArgs e) { Refresh(); }
		public virtual void SelectedTileChanged(IMap_Base sender, SelectedTileChangedEventArgs e) { Refresh(); }

		#endregion
	}
}
