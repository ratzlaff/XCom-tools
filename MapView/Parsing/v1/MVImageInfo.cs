using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using UtilLib.Parser;
using MapLib.Base.Parsing;

namespace MapView.Parsing.v1
{
	public class MVImageInfo : ImageInfo
	{
		public MVImageInfo(Images_dat inParent, KeyVal val)
			: base(inParent, val)
		{
		}

		public override MapLib.ImageCollection Images
		{
			get { return null; }
		}
	}
}
