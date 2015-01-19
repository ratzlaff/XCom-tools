using System;

namespace MapView
{	
	public class Globals
	{
		public static int PckImageScale=1;

		public static bool UseGray{get{return true;}}
		public static readonly string RegistryKey = "MapView";

		private static XCom.PckFile extraTiles = null;

		public static XCom.PckFile ExtraTiles
		{
			get { return extraTiles; }
		}

		public static void LoadExtras()
		{
			if (extraTiles == null)
			{
			    using (System.IO.Stream sPck = System.Reflection.Assembly.GetExecutingAssembly().GetManifestResourceStream("MapView._Embedded.Extra.PCK"))
			    using (System.IO.Stream sTab = System.Reflection.Assembly.GetExecutingAssembly().GetManifestResourceStream("MapView._Embedded.Extra.TAB"))
			    {
			        extraTiles = new XCom.PckFile(sPck, sTab, 2, XCom.Palette.TFTDBattle);
			    }
			}
		}
	}
}
