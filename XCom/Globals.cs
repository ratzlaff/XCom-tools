using System;

namespace XCom
{
//	public enum XcomShips{Triton, Hammerhead, Leviathon};
//	public enum ImageType{Bitmap, Surface};

	public enum SaveMode
	{
		MapEdit
	};

	public static class Globals
	{
		public static int HalfWidth  = 16;
		public static int HalfHeight = 8;

		public static string RegistryKey
		{
			get { return "ViewSuite"; }
		}

		/// <summary>
		/// Method to convert from screen(x,y) coordinates to map(col,row) coordinates
		/// </summary>
		/// <param name="x"></param>
		/// <param name="y"></param>
		/// <param name="row"></param>
		/// <param name="col"></param>
/*		public static void ConvertCoordsDiamond(
											int x,
											int y,
											int hWidth,
											int hHeight,
											out int row,
											out int col)
		{
//			int x = xp - offX; // 16 is half the width of the diamond
//			int y = yp - offY; // 24 is the distance from the top of the diamond to the very top of the image

			double x1 = (x * 1.0 / (2 * hWidth)) + (y * 1.0 / (2 * hHeight));
			double x2 = -(x * 1.0 - 2 * y * 1.0) / (2 * hWidth);

			row = (int)Math.Floor(x2);
			col = (int)Math.Floor(x1);
//			return new Point((int)Math.Floor(x1), (int)Math.Floor(x2));
		} */
		// NOTE: how many ConvertCoordsDiamond() functions do you need.
	}
}
