using System;

namespace XCom.Interfaces.Base
{
	/// <summary>
	/// Objects of this class are drawn to the screen with the MapViewPanel
	/// </summary>
	public abstract class MapTileBase
	{
	    /// <summary>
	    /// A list of ITiles in the correct draw order. You should iterate over this array when drawing to the screen
	    /// This list does not include IUnits
	    /// </summary>
	    public abstract TileBase[] UsedTiles { get; }

	    /// <summary>
	    /// Flag to help optimize screen drawing, if set
	    /// </summary>
	    public bool DrawAbove { get; set; }
	}
}
