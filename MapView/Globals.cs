using System;
using XCom;

namespace MapView
{
	public class Globals
	{
		public static int PckImageScale = 1;

		public static bool MapChanged = false;
		public static bool UseGray { get { return true; } }
		public static readonly string RegistryKey = "MapView";

		public static XCom.PckFile ExtraTiles { get; }

		public static void LoadExtras()
		{
			if (ExtraTiles == null) {
				System.IO.Stream sPck = System.Reflection.Assembly.GetExecutingAssembly().GetManifestResourceStream("MapView._Embedded.Extra.PCK");
				System.IO.Stream sTab = System.Reflection.Assembly.GetExecutingAssembly().GetManifestResourceStream("MapView._Embedded.Extra.TAB");

				ExtraTiles = new XCom.PckFile(sPck, sTab, 2, XCPalette.TFTDBattle);
			}
		}
	}
}
