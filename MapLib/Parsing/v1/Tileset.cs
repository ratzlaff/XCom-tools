using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel;
using System.ComponentModel.Design;
using System.Drawing.Design;

using UtilLib;
using UtilLib.Parser;
using MapLib.Base.Parsing;
using MapLib.Parsing;

namespace MapLib
{
	public class Tileset : ParseBlock<MapCollection>
	{
		protected ParseBlockCollection<MapInfo, Tileset> mCollection;

		public Tileset(MapCollection inParent, string inName)
			: base(inParent, inName)
		{
			mCollection = new ParseBlockCollection<MapInfo, Tileset>(this, "Map List");
			mParent = inParent;
		}

		public Tileset()
			:	base(null, "")
		{
			mCollection = new ParseBlockCollection<MapInfo, Tileset>(this, "Map List");
		}

		public MapCollection MapCollection
		{
			get { return Parent; }
		}

//		[Editor(typeof(CollectionEditor), typeof(UITypeEditor))]
		public ParseBlockCollection<MapInfo, Tileset> Maps
		{
			get { return mCollection; }
		}

		protected override void ProcessVar(VarCollection vars, KeyVal current)
		{
			MapInfo mi = new XComMapInfo_1(this, current);
			mCollection.Add(mi);
		}
	}
}
