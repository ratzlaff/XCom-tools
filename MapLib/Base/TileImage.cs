using System;
using System.Drawing;

namespace MapLib.Base
{
	public class TileImage
	{
		protected Bitmap image = null;
		protected Bitmap gray = null;
		protected Palette palette = null;

		public Bitmap Image { get { return image; } }
		public Bitmap Gray { get { return gray; } }

		public Palette Palette
		{
			get { return palette; }
			set
			{
				palette = value;

				if (image != null)
					image.Palette = palette.Colors;
			}
		}
	}
}