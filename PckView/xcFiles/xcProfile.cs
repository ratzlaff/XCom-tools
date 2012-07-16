using System;
using XCom;
using XCom.Images;
using System.Collections.Generic;
using UtilLib;
using MapLib.Base;

namespace PckView
{
	public class xcProfile : xcImageFile
	{
		private xcImageFile codec;
		public static readonly string PROFILE_EXT = ".pvp";

		public xcProfile()
			: base(0, 0)
		{
			fileOptions.Init(false, false, false, false);
			//fileOptions.BmpDialog = false;
			//fileOptions.OpenDialog = false;
			//fileOptions.SaveDialog = false;
			//fileOptions.CustomDialog = false;

			ext = PROFILE_EXT;
			author = "Ben Ratzlaff";
			desc = "Provides profile support";
		}

		public xcProfile(ImgProfile profile)
			: base(0, 0)
		{
			imageSize = new System.Drawing.Size(profile.ImgWid, profile.ImgHei);
			codec = profile.ImgType;
			expDesc = profile.Description;
			ext = profile.Extension;
			author = "Profile";
			desc = profile.Description;

			if (profile.OpenSingle != "")
				singleFile = profile.OpenSingle;

			//fileOptions.BmpDialog = true;
			//fileOptions.OpenDialog = true;

			//since we are loading off of an already generic implementation
			//we should let that implementation determine how this format be saved
			//fileOptions.SaveDialog = false;
			//fileOptions.CustomDialog = false;

			fileOptions.Init(false, true, true, false);

			xConsole.AddLine("Profile created: " + desc);

			try {
				defPal = Palette.LoadedPalettes[profile.Palette];
				if (defPal == null)
					defPal = XCPalette.TFTDBattle;
			} catch {
				defPal = XCPalette.TFTDBattle;
			}
		}

		public xcImageFile Codec { get { return codec; } set { codec = value; } }

		protected override XCImageCollection LoadFileOverride(string directory, string file, int imgWid, int imgHei, MapLib.Base.Palette pal)
		{
			return codec.LoadFile(directory, file, imgWid, imgHei, pal);
		}

		public override void SaveCollection(string directory, string file, XCImageCollection images)
		{
			codec.SaveCollection(directory, file, images);
		}
	}
}
