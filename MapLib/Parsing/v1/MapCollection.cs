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

namespace MapLib
{
	public class MapCollection : ParseBlock<MapEdit_dat>
	{
		protected ParseBlockCollection<Tileset, MapCollection> mCollection;
		protected string rootPath, rmpPath, palette;

		public MapCollection(string inName)
			:	base(null, inName)
		{
			mCollection = new ParseBlockCollection<Tileset, MapCollection>(this, "Tilesets");
		}

		[Editor(typeof(FolderNameEditor), typeof(UITypeEditor))]
		public string RootPath
		{
			get { return rootPath; }
			set { rootPath = value; }
		}

		[Editor(typeof(FolderNameEditor), typeof(UITypeEditor))]
		public string RmpPath
		{
			get { return rmpPath; }
			set { rmpPath = value; }
		}

//		[Editor(typeof(CollectionEditor), typeof(UITypeEditor))]
		public ParseBlockCollection<Tileset, MapCollection> Tilesets
		{
			get { return mCollection; }
		}

		protected override void ProcessVar(VarCollection vars, KeyVal current)
		{
			switch (current.Keyword.ToLower()) {
				case "type":
					break;
				case "palette":
					palette = current.Rest;
					break;
				case "rootpath":
					rootPath = current.Rest;
					break;
				case "rmppath":
					rmpPath = current.Rest;
					break;
				case "blankpath":
					break;
				case "files": {
					Tileset t = new Tileset(this, current.Rest);
					t.Parse(vars);
					mCollection.Add(t);
				}
					break;
				default:
					base.ProcessVar(vars, current);
					break;
			}
		}
	}
}
