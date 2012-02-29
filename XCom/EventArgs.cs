using System;
using System.Collections.Generic;
using System.Text;
using XCom.Interfaces.Base;

namespace XCom
{
	/// <summary>
	/// EventArgs class that holds a MapLocation and MapTile for when a SelectedTileChanged event fires
	/// </summary>
	public class SelectedTileChangedEventArgs : EventArgs
	{
		private MapLocation newSelected;
		private IMapTile selectedTile;

		public SelectedTileChangedEventArgs(MapLocation newSelected, IMapTile selectedTile)
		{
			this.newSelected = newSelected;
			this.selectedTile = selectedTile;
		}

		public MapLocation MapLocation
		{
			get { return newSelected; }
		}

		public IMapTile SelectedTile
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
}
