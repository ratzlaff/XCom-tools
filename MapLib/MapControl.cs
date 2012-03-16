using System;
using MapLib.Base;

namespace MapLib
{
	public static class MapControl
	{
		public static event MapChangedDelegate MapChanged;
		public static event HeightChangedDelegate HeightChanged;
		public static event TileSelectionChangedDelegate SelectedTileChanged;
		public static event RefreshDelegate Refresh;

		private static Map current;
		public static Map Current
		{
			get { return current; }
			set
			{
				current = value;
				if (MapChanged != null) {
					MapChangedEventArgs e = new MapChangedEventArgs(current);
					MapChanged(e);
				}
			}
		}

		private static MapLocation startDrag, endDrag;
		public static MapLocation StartDrag
		{
			get { return startDrag; }
			set { startDrag = value; }
		}

		public static MapLocation EndDrag
		{
			get { return endDrag; }
			set
			{
				endDrag = value;
				SelectedTileChangedEventArgs stc = new SelectedTileChangedEventArgs(value, current[endDrag.Row, endDrag.Col]);
				FireSelectedChanged(stc);
			}
		}

		public static void FireHeightChanged(HeightChangedEventArgs e)
		{
			if (HeightChanged != null)
				HeightChanged(current, e);
		}

		public static void FireSelectedChanged(SelectedTileChangedEventArgs e)
		{
			if (SelectedTileChanged != null)
				SelectedTileChanged(current, e);
		}

		public static void RequestRefresh()
		{
			if (Refresh != null)
				Refresh();
		}
	}
}
