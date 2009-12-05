using System;
using System.Collections.Generic;
using System.Drawing;
using XCom.Interfaces;
using DSShared;

namespace XCom
{
	public class XCImageCollection : List<XCImage>
	{
		protected string name, path;
		private Palette mPalette;
//		protected int mScale = 1;
		private XCom.Interfaces.IXCImageFile ixcFile;

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

		public IXCImageFile IXCFile
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
		}

		public void Remove(int i)
		{
			RemoveAt(i);
		}
	}
}
