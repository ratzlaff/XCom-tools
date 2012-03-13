using System;
using System.Drawing;
using DSShared;
using MapLib.Base;

namespace XCom.Interfaces
{
	public class XCImage : TileImage, ICloneable
	{
		protected byte[] idx;
		protected int fileNum;
		protected string mName = "";

		private byte transparent = 0xFE;

		//entries must not be compressed
		public XCImage(byte[] entries, int width, int height, Palette pal, int idx)
		{
			fileNum = idx;
			this.idx = entries;
			palette = pal;

			if (pal != null)
				image = DSShared.Bmp.MakeBitmap8(width, height, entries, pal.Colors);
		}

		public XCImage()
			: this(new byte[] { }, 0, 0, null, -1)
		{ }

		public XCImage(Bitmap b, int idx)
		{
			fileNum = idx;
			image = b;
			this.idx = null;
			palette = null;
		}

		public byte[] Bytes { get { return idx; } }
		public int FileNum { get { return fileNum; } set { fileNum = value; } }
		public virtual byte TransparentIndex { get { return transparent; } }

		public Color GetColor(int x, int y)
		{
			if (image.Width * y + x > idx.Length)
				return image.GetPixel(x, y);
			return palette[idx[image.Width * y + x]];
		}

		public object Clone()
		{
			if (idx != null) {
				byte[] b = new byte[idx.Length];
				for (int i = 0; i < b.Length; i++)
					b[i] = idx[i];

				return new XCImage(b, image.Width, image.Height, palette, fileNum);
			} else if (image != null)
				return new XCImage((Bitmap)image.Clone(), fileNum);
			else
				return null;
		}

		public virtual void Hq2x()
		{
			image = DSShared.Bmp.Hq2x(image);
		}

		public override string ToString()
		{
			return mName;
		}
	}
}
