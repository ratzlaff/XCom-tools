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

		private static IconInfo extraTiles;

		public static MapLib.ImageCollection ExtraTiles
		{
			get
			{
				if (extraTiles == null)
					extraTiles = new IconInfo();
				return extraTiles.Images;
			}
		}

		private class IconInfo : MapLib.Base.Parsing.ImageInfo
		{
			PckFile mFile;
			public IconInfo()
				:	base(null, new UtilLib.Parser.KeyVal("icons", "icons"))
			{
			}

			public override MapLib.ImageCollection Images
			{
				get
				{
					if (mFile == null) {
						MemoryStream sPck = new MemoryStream(Properties.Resources.Extra_PCK);
						MemoryStream sTab = new MemoryStream(Properties.Resources.Extra_TAB);

						mFile = new PckFile(this, sPck, sTab, 2, XCPalette.TFTDBattle, 40, 32);
						mFile.Pal.SetTransparent(false);
					}
					return mFile;
				}
			}

			public override System.Collections.Generic.List<MapLib.Base.Tile> Tiles
			{
				get { throw new NotImplementedException(); }
			}
		}
	}
}
