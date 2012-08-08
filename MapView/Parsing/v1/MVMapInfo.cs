using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;
using System.IO;

using UtilLib.Parser;
using MapLib.Base.Parsing;
using MapLib;
using MapLib.Base;

namespace MapView.Parsing.v1
{
	public class MVMapInfo : MapLib.Base.Parsing.MapInfo
	{
		private XCom.XCMapFile mMap;

		public MVMapInfo(MVTileset inParent, KeyVal inData)
			: base(inParent, inData)
		{
		}

		public override MapLib.Base.Map Map
		{
			get
			{
				Console.WriteLine("Load map here: " + Tileset.MapCollection.Name + ":" + Tileset.Name + ":" + Name);
				foreach (ImageInfo img in Dependencies.Data) {
					if (img.Images != null) {
						foreach (TileImage tImg in img.Images) {
							Console.WriteLine("Got image!");
						}
					}
				}
				return mMap;
			}
		}
	}
}
