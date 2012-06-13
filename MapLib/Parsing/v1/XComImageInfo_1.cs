using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using UtilLib.Parser;

namespace MapLib.Parsing
{
	public class XComImageInfo_1 : MapLib.Base.Parsing.ImageInfo
	{
		public XComImageInfo_1(Images_dat inParent, KeyVal val)
			: base(inParent, val)
		{
		}

		public override ImageCollection Images
		{
			get { return null; }
		}
	}
}
