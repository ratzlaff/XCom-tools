using System;
using System.Text;

namespace XCom
{
	public struct MapSize
	{
		public int Rows { get; set; }
		public int Cols { get; set; }
		public int Height { get; set; }

		public MapSize(int rows, int cols, int height)
			: this()
		{
			Rows = rows;
			Cols = cols;
			Height = height;
		}

		public override string ToString()
		{
			return Rows + "," + Cols + "," + Height;
		}
	}
}
