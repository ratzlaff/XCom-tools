using System;
using System.IO;
using XCom.Interfaces;
using XCom;
using DSShared;

namespace PckView
{
	public class xcSPE : IXCImageFile
	{
		public xcSPE() : this(0, 0) { }
		public xcSPE(int wid, int hei)
			: base(wid, hei)
		{
			ext = ".spe";
			author = "Ben Ratzlaff";
			desc = "Options for opening Abuse SPE files";
			expDesc = "SPE File";

			fileOptions.Init(true, false, true, false);
		}

		protected override XCom.XCImageCollection LoadFileOverride(string directory, string file, int imgWid, int imgHei, Palette pal)
		{
			SpecCollection sc = new SpecCollection(File.OpenRead(directory + "\\" + file));
			
			xConsole.AddLine("File: " + directory + "\\" + file);

			SPECollection spe = new SPECollection(file, sc.EntryList);

			imageSize.Height = imageSize.Width = 0;
			if (spe.Count > 0)
				defPal = spe[0].Palette;
			foreach (XCImage img in spe) {
				imageSize.Height = Math.Max(ImageSize.Height, img.Image.Height);
				imageSize.Width = Math.Max(ImageSize.Width, img.Image.Width);
			}

			return spe;
		}
	}
}