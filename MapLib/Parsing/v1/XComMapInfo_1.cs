using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using UtilLib.Parser;

namespace MapLib.Parsing
{
	public class XComMapInfo_1 : MapLib.Base.Parsing.MapInfo
	{
		public XComMapInfo_1(Tileset inParent, KeyVal inData)
			: base(inParent, inData)
		{

		}

		public override Base.Map Map
		{
			get 
			{
				Console.WriteLine("Load map here: " + Tileset.MapCollection.Name + ":" + Tileset.Name + ":" + Name);
				return null; 
			}
		}
	}
}
