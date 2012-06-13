using System;
using System.Collections.Generic;
using XCom.Interfaces.Base;
using UtilLib;
using MapLib.Base;
using MapLib;
#if NOPE
namespace XCom
{
	public class XCMapDesc : MapInfo
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
			Palette myPal)
			: base(basename)
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

		public override Map  Map
		{
			get
			{
				ImageInfo images = GameInfo.ImageInfo;

				List<Tile> a = new List<Tile>();

				foreach (string s in dependencies) {
					if (images[s] != null) {
						McdFile mcd = images[s].GetMcdFile(myPal);
						foreach (XCTile t in mcd)
							a.Add(t);
					}
				}

				XCMapFile map = new XCMapFile(basename, basePath, blankPath, a, dependencies);
				map.Rmp = new RmpFile(basename, rmpPath);
				return map;
			}
		}

		public string[] Dependencies { get { return dependencies; } set { dependencies = value; } }
		public bool IsStatic { get { return isStatic; } set { isStatic = value; } }
		public int CompareTo(object other)
		{
			if (other is XCMapDesc) {
				return basename.CompareTo(((XCMapDesc)other).basename);
			}
			return 1;
		}
	}
}
#endif
