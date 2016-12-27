using System.Collections.Generic;
using System.IO;
using XCom.GameFiles.Map;
using XCom.Interfaces.Base;

namespace XCom
{
	public class XcMapFileService
	{
		private readonly XcTileFactory _xcTileFactory;

		public XcMapFileService(XcTileFactory xcTileFactory)
		{
			_xcTileFactory = xcTileFactory;
		}

		public IMap_Base Load(XCMapDesc imd)
		{
			if (imd == null) return null;
			if (!File.Exists(imd.FilePath)) return null;
			var filePath = imd.BasePath + imd.Basename + ".MAP";
			if (!File.Exists(filePath)) return null;
			ImageInfo images = GameInfo.ImageInfo;

			var tiles = new List<TileBase>();

			foreach (string dependency in imd.Dependencies)
			{
				var image = images[dependency];
				if (image != null)
				{
					McdFile mcd = image.GetMcdFile(imd.Palette, _xcTileFactory);
					foreach (XCTile t in mcd)
						tiles.Add(t);
				}
			}

			var rmp = new RmpFile(imd.Basename, imd.RmpPath);
			var map = new XCMapFile(imd.Basename, imd.BasePath, imd.BlankPath, tiles, imd.Dependencies, rmp);

			return map;
		}
	}
}
