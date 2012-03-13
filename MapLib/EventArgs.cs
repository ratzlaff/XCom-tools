using System;
using MapLib.Base;

namespace MapLib
{
	/// <summary>
	/// EventArgs class that holds a MapLocation and MapTile for when a SelectedTileChanged event fires
	/// </summary>
	public class SelectedTileChangedEventArgs : EventArgs
	{
		private MapLocation newSelected;
		private MapTile selectedTile;

		public SelectedTileChangedEventArgs(MapLocation newSelected, MapTile selectedTile)
		{
			this.newSelected = newSelected;
			this.selectedTile = selectedTile;
		}

		public MapLocation MapLocation
		{
			get { return newSelected; }
		}

		public MapTile SelectedTile
		{
			get { return selectedTile; }
		}
	}

	/// <summary>
	/// EventArgs class for when a HeightChanged event fires. 
	/// </summary>
	public class HeightChangedEventArgs : EventArgs
	{
		private int newHeight;
		private int oldHeight;

		public HeightChangedEventArgs(int oldHeight, int newHeight)
		{
			this.newHeight = newHeight;
			this.oldHeight = oldHeight;
		}

		public int NewHeight
		{
			get { return newHeight; }
		}

		public int OldHeight
		{
			get { return oldHeight; }
		}
	}

	public class MapChangedEventArgs : EventArgs
	{
		private Map map;

		public MapChangedEventArgs(Map map)
		{
			this.map = map;
		}

		public Map Map
		{
			get { return map; }
		}
	}
}
