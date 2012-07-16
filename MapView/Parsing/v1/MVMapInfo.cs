using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using UtilLib.Parser;
using MapLib.Base.Parsing;

namespace MapView.Parsing.v1
{
	public class MVMapInfo : MapLib.Base.Parsing.MapInfo
	{
		public MVMapInfo(MVTileset inParent, KeyVal inData)
			: base(inParent, inData)
		{
		}

		public override MapLib.Base.Map Map
		{
			get
			{
				Console.WriteLine("Load map here: " + Tileset.MapCollection.Name + ":" + Tileset.Name + ":" + Name);
				return null;
			}
		}
	}
}
