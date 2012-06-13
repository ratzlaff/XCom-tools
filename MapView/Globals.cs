using System;
using System.IO;
using XCom;


namespace MapView
{
	public class Globals
	{
		public static int PckImageScale = 1;

		public static bool MapChanged = false;
		public static bool UseGray { get { return true; } }

		private static XCom.PckFile extraTiles;
		public static XCom.PckFile ExtraTiles
		{
			get
			{
				if (extraTiles == null) {
					MemoryStream sPck = new MemoryStream(Properties.Resources.Extra_PCK);
					MemoryStream sTab = new MemoryStream(Properties.Resources.Extra_TAB);

					extraTiles = new PckFile(sPck, sTab, "icons", 2, XCPalette.TFTDBattle);
					extraTiles.Pal.SetTransparent(false);
				}
				return extraTiles;
			}
		}
	}
}
