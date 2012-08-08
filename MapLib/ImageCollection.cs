using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;

using MapLib.Base;

namespace MapLib
{
	public class ImageCollection : List<TileImage>
	{
		private MapLib.Base.Parsing.ImageInfo mInfo;

		public ImageCollection(MapLib.Base.Parsing.ImageInfo inInfo)
		{
			mInfo = inInfo;
		}

		public MapLib.Base.Parsing.ImageInfo Info
		{
			get { return mInfo; }
		}
	}
}
