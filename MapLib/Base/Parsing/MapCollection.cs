using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel;
using System.ComponentModel.Design;
using System.Windows.Forms.Design;
using System.Drawing.Design;
using UtilLib.Parser;
using UtilLib;
using MapLib.Base;

namespace MapLib.Base.Parsing
{
	public class MapCollection : ParseBlock<MapEdit_dat>
	{
		protected ParseBlockCollection<Tileset, MapCollection> mCollection;

		public MapCollection(string inName)
			:	base(null, inName)
		{
			mCollection = new ParseBlockCollection<Tileset, MapCollection>(this, "Tilesets");
		}

//		[Editor(typeof(CollectionEditor), typeof(UITypeEditor))]
		public ParseBlockCollection<Tileset, MapCollection> Tilesets
		{
			get { return mCollection; }
		}
	}
}
