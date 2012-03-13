using System;

namespace MapLib
{
	/// <summary>
	/// Struct for defining map locations
	/// </summary>
	public struct MapLocation
	{
		public int Row { get; set; }
		public int Col { get; set; }
		public int Height { get; set; }

		public MapLocation(int row, int col, int height)
			: this()
		{
			Row = row;
			Col = col;
			Height = height;
		}
	}
}