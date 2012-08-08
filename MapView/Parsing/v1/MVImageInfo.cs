using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using UtilLib.Parser;
using MapLib.Base.Parsing;

using XCom;
using MapLib;
using System.IO;
using MapLib.Base;

namespace MapView.Parsing.v1
{
	public class MVImageInfo : ImageInfo
	{
		private PckFile mPckFile;
		private McdFile mMcdFile;

		public MVImageInfo(Images_dat inParent, KeyVal val)
			: base(inParent, val)
		{
		}

		public override List<Tile> Tiles
		{
			get
			{
				if (mMcdFile == null)
					mMcdFile = new XCom.McdFile(Images);
				return mMcdFile.Tiles;
			}
		}

		public override ImageCollection Images
		{
			get
			{
				if (mPckFile == null)
					mPckFile = new XCom.PckFile(this, XCom.XCPalette.UFOBattle);

				return mPckFile;
			}
		}
	}
}
