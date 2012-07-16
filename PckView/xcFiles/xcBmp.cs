using System;
using XCom.Images;
using UtilLib;

namespace PckView
{
	public class xcBmp : xcImageFile
	{
		public xcBmp()
			: base(0, 0)
		{
			//fileOptions.BmpDialog=false;
			//fileOptions.SaveDialog=false; //save as bmp has its own menu item
			//fileOptions.OpenDialog=true;
			//fileOptions.CustomDialog=false;

			fileOptions.Init(true, false, true, false);

			ext = ".bmp";
			author = "Ben Ratzlaff";
			desc = "Provides 8-bit BMP support";

			expDesc = "8-bit bmp file";
		}

		protected override XCImageCollection LoadFileOverride(string directory, string file, int imgWid, int imgHei, MapLib.Base.Palette pal)
		{
			System.Drawing.Bitmap bmp = new System.Drawing.Bitmap(directory + "\\" + file);
			BmpForm bmf = new BmpForm();
			bmf.Bitmap = bmp;

			if (bmf.ShowDialog() == System.Windows.Forms.DialogResult.OK) {
				imageSize = bmf.SelectedSize;

				return XCom.Bmp.Load(bmp, pal, imageSize.Width, imageSize.Height, bmf.SelectedSpace);
			}

			return null;
		}

		public override void SaveCollection(string directory, string file, XCImageCollection images)
		{
			TotalViewPck.Instance.View.SaveBMP(directory + "//" + file + ".bmp", TotalViewPck.Instance.Pal);
		}
	}
}