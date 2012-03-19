using System;
using XCom;

namespace MapView
{
	public class Globals
	{
		public static int PckImageScale = 1;

		public static bool MapChanged = false;
		public static bool UseGray { get { return true; } }
//		public static readonly string RegistryKey = "MapView";

		private static XCom.PckFile extraTiles;
		public static XCom.PckFile ExtraTiles
		{
			get
			{
				if (extraTiles == null) {
					System.IO.Stream sPck = System.Reflection.Assembly.GetExecutingAssembly().GetManifestResourceStream("MapView._Embedded.Extra.PCK");
					System.IO.Stream sTab = System.Reflection.Assembly.GetExecutingAssembly().GetManifestResourceStream("MapView._Embedded.Extra.TAB");

					extraTiles = new XCom.PckFile(sPck, sTab, 2, XCPalette.TFTDBattle);
					extraTiles.Pal.SetTransparent(false);
				}
				return extraTiles;
			}
		}
	}
}
