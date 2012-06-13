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

		}

		[Browsable(false)]
		public abstract ImageCollection Images
		{
			get;
		}
	}
}
