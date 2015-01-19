using System;

namespace XCom.Interfaces.Base
{
	/// <summary>
	/// Objects of this class are drawn to the screen with the MapViewPanel
	/// </summary>
	public class IMapTile
	{
		protected TileBase[] usedTiles;

		/// <summary>
		/// A list of ITiles in the correct draw order. You should iterate over this array when drawing to the screen
		/// This list does not include IUnits
		/// </summary>
		public TileBase[] UsedTiles
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
	}
}
