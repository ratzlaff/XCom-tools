using System;
using System.Collections.Generic;
using System.IO;
using XCom.Interfaces.Base;

namespace XCom
{
	public class XCMapDesc:IMapDesc
	{
		protected Palette myPal;
		protected bool isStatic;
		protected string[] dependencies;
		protected string basename, basePath, rmpPath, blankPath;
		//protected string tileset;

		public XCMapDesc(
			string basename,
			string basePath,
			string blankPath,
			//string tileset,
			string rmpPath,
			string[] dependencies,
			Palette myPal):base(basename)
		{
			this.myPal = myPal;
			this.basename = basename;
			this.basePath = basePath;
			this.rmpPath = rmpPath;
			this.blankPath = blankPath;
			//this.tileset = tileset;
			this.dependencies = dependencies;
			isStatic = false;
		}

		public override IMap_Base GetMapFile() 
		{
            var filePath = basePath + basename + ".MAP";
		    if (!File.Exists(filePath)) return null;
			ImageInfo images = GameInfo.ImageInfo;

			var tiles = new List<ITile>();

			foreach (string dependency in dependencies)
			{
			    var image = images[dependency];
				if (image != null)
				{
					McdFile mcd = image.GetMcdFile(myPal);
					foreach (XCTile t in mcd)
						tiles.Add(t);
				}
			}

            XCMapFile map = new XCMapFile(basename, basePath, blankPath, tiles, dependencies);
			map.Rmp = new RmpFile(basename, rmpPath);
			return map;
		}

		public string[] Dependencies { get { return dependencies; } set { dependencies = value; } }
		public bool IsStatic { get { return isStatic; } set { isStatic = value; } }
		public int CompareTo(object other)
		{
			if (other is XCMapDesc)
			{
				return basename.CompareTo(((XCMapDesc)other).basename);
			}
			return 1;
		}
	}
}
