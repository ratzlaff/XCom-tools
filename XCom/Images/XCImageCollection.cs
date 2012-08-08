using System;
using System.Collections.Generic;
using System.Drawing;
using XCom.Images;
using UtilLib;
using MapLib.Base;
using MapLib;

namespace XCom.Images
{
	public class XCImageCollection : ImageCollection
	{
		protected string name, path;
		private Palette mPalette;
//		protected int mScale = 1;
		private xcImageFile ixcFile;

		public XCImageCollection(MapLib.Base.Parsing.ImageInfo inInfo)
			: base(inInfo)
		{
		}

		public string Name
		{
			get { return name; }
			set { name = value; }
		}

		public string Path
		{
			get { return path; }
			set { path = value; }
		}

		public xcImageFile IXCFile
		{
			get { return ixcFile; }
			set { ixcFile = value; }
		}

		public void Hq2x()
		{
			foreach (XCImage i in this)
				i.Hq2x();

			ixcFile.SetImageSize(new Size(ixcFile.ImageSize.Width * 2, ixcFile.ImageSize.Height*2));
		}

		public virtual Palette Pal
		{
			get { return mPalette; }
			set
			{
				foreach (XCImage i in this)
					i.Palette = value;

				mPalette = value;
			}
		}
		/*
		public new XCImage this[int i]
		{
			get { return (i < Count && i >= 0 ? base[i] : null); }
			set
			{
				if (i < Count && i >= 0)
					base[i] = value;
				else {
					value.FileNum = Count;
					Add(value);
				}
			}
		}*/

		public void Remove(int i)
		{
			RemoveAt(i);
		}
	}
}
