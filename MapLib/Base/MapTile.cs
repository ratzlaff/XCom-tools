using System;
using System.Collections.Generic;

namespace MapLib.Base
{
	/// <summary>
	/// Objects of this class are drawn to the screen with the MapViewPanel
	/// </summary>
	public class MapTile
	{
		protected List<Tile> usedTiles;

		public MapTile()
		{
			usedTiles = new List<Tile>();
		}

		/// <summary>
		/// A list of Tiles in the correct draw order. You should iterate over this array when drawing to the screen
		/// This list does not include IUnits
		/// </summary>
		public List<Tile> UsedTiles
		{
			get { return usedTiles; }
		}

        protected bool drawAbove;

		/// <summary>
		/// Flag to help optimize screen drawing, if set
		/// </summary>
        public bool DrawAbove
        {
            get { return drawAbove; }
            set { drawAbove = value; }
        }

		private static MapTile blankTile = new MapTile();
		public static MapTile BlankTile
		{
			get { return blankTile; }
		}
	}
}
