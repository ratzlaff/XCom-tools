using System;
using System.IO;
using System.Collections.Generic;
using MapLib.Base;

namespace XCom
{
	public class McdFile//:IEnumerable
	{
		private List<Tile> mTiles;

		public McdFile(MapLib.ImageCollection inImages)
		{
			BufferedStream file = new BufferedStream(File.OpenRead(inImages.Info.BasePath + inImages.Info.Name + ".MCD"));
			int diff = 0;
			if (inImages.Info.Name == "XBASES05")
				diff = 3;
			mTiles = new List<Tile>();

			int numTiles = (((int)file.Length) / 62) - diff;

			for (int i = 0; i < numTiles; i++) {
				byte[] info = new byte[62];
				file.Read(info, 0, 62);
				mTiles.Add(new XCTile(i, inImages, new McdEntry(info, this)));
			}
			file.Close();

			foreach (XCTile t in mTiles)
				mTiles.Add(t);

			foreach (XCTile t in mTiles)
				t.Init();
		}

		public List<Tile> Tiles
		{
			get { return mTiles; }
		}
	}
}
