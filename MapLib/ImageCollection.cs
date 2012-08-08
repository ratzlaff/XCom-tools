using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;

using MapLib.Base;
using MapLib.Base.Parsing;

namespace MapLib
{
	public class ImageCollection : List<TileImage>
	{
		private static Dictionary<string, ImageInfo> sLoadedImages;

		private MapLib.Base.Parsing.ImageInfo mInfo;

		public ImageCollection(ImageInfo inInfo)
		{
			mInfo = inInfo;
		}

		public ImageInfo Info
		{
			get { return mInfo; }
		}

		public static Dictionary<string, ImageInfo> ImageCache
		{
			get
			{
				if (sLoadedImages == null)
					sLoadedImages = new Dictionary<string, ImageInfo>();
				return sLoadedImages;
			}
		}
	}
}
