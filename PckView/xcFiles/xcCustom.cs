using System;
using XCom.Images;
using XCom;
using UtilLib;

namespace PckView
{
	public class xcCustom : xcImageFile
	{
		public xcCustom() : this(0, 0) { }
		public xcCustom(int wid, int hei)
			: base(wid, hei)
		{
			ext = ".*";
			author = "Ben Ratzlaff";
			desc = "Options for opening unknown files";
			expDesc = "Any File";

			fileOptions.Init(false, false, true, false);

			defPal = XCPalette.TFTDBattle;
		}

		//public override int FilterIndex
		//{
		//    get { return base.FilterIndex; }
		//    set { base.FilterIndex = value; FIDX = value; }
		//}

		protected override XCImageCollection LoadFileOverride(string directory, string file, int imgWid, int imgHei, MapLib.Base.Palette pal)
		{
			OpenCustomForm ocf = new OpenCustomForm(directory, file);
			ocf.TryClick += new TryDecodeEventHandler(tryIt);
			ocf.Show();

			return null;
		}

		private void tryIt(object sender, TryDecodeEventArgs tde)
		{
			PckViewForm pvf = (PckViewForm)SharedSpace.Instance["PckView"];

			XCImageCollection ixc = tde.XCFile.LoadFile(tde.Directory, tde.File, tde.TryWidth, tde.TryHeight);
			//ixc.IXCFile=this;
			imageSize = new System.Drawing.Size(tde.TryWidth, tde.TryHeight);

			pvf.SetImages(ixc);
		}
	}
}
