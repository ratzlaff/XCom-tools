using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;

using UtilLib.Parser;

namespace MapLib.Base.Parsing
{
	public abstract class ImageInfo : ParseBlock<Images_dat>
	{
		protected string basePath;

		public ImageInfo(Images_dat inParent, KeyVal inData)
			: base(inParent, inData.Keyword)
		{
			basePath = inData.Rest;
		}

		public string BasePath
		{
			get { return basePath; }
		}

		[Browsable(false)]
		public abstract ImageCollection Images
		{
			get;
		}

		[Browsable(false)]
		public abstract List<Tile> Tiles
		{
			get;
		}
	}
}
