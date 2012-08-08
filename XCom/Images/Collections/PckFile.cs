using System;
using System.Collections;
using System.IO;
using System.Drawing;
using XCom.Images;
using UtilLib;
using MapLib.Base;

namespace XCom
{
	public delegate void LoadingDelegate(int curr, int total);

	public class PckFile : XCImageCollection
	{
		private PckFile mBlanks;
		private int bpp;
		public static readonly string TAB_EXT = ".tab";
		public PckFile(MapLib.Base.Parsing.ImageInfo inInfo, Palette pal, int imgHeight, int imgWidth)
			: this(inInfo, File.Open(inInfo.BasePath + inInfo.Name + ".pck", FileMode.Open), File.Open(inInfo.BasePath + inInfo.Name + ".tab", FileMode.Open), pal, imgHeight, imgWidth)
		{
		}

		public PckFile(MapLib.Base.Parsing.ImageInfo inInfo, Stream pckFile, Stream tabFile, Palette pal, int imgHeight, int imgWidth)
			: base(inInfo)
		{
			if (inInfo != null)
				Name = inInfo.Name;

			if (tabFile != null)
				tabFile.Position = 0;

			pckFile.Position = 0;

			byte[] info = new byte[pckFile.Length];
			pckFile.Read(info, 0, info.Length);
			pckFile.Close();

			Pal = pal;

			uint[] offsets;

			if (tabFile != null) {
				BinaryReader br = new BinaryReader(tabFile);
				bpp = br.ReadInt32() == 0 ? 4 : 2;
				br.BaseStream.Position = 0;

				offsets = new uint[(tabFile.Length / bpp) + 1];

				if (bpp == 2)
					for (int i = 0; i < tabFile.Length / bpp; i++)
						offsets[i] = br.ReadUInt16();
				else
					for (int i = 0; i < tabFile.Length / bpp; i++)
						offsets[i] = br.ReadUInt32();
				br.Close();
			} else {
				offsets = new uint[2];
				offsets[0] = 0;
			}

			offsets[offsets.Length - 1] = (uint)info.Length;

			for (int i = 0; i < offsets.Length - 1; i++) {
				byte[] imgDat = new byte[offsets[i + 1] - offsets[i]];
				for (int j = 0; j < imgDat.Length; j++)
					imgDat[j] = info[offsets[i] + j];

				Add(new PckImage(i, imgDat, pal, this, imgHeight, imgWidth));
			}

			pckFile.Close();
			if (tabFile != null)
				tabFile.Close();
		}

		public PckFile(MapLib.Base.Parsing.ImageInfo inInfo, Palette pal)
			: this(inInfo, pal, 40, 32)
		{ }

		public PckFile Blanks
		{
			get { return mBlanks; }
		}

		public int Bpp
		{
			get { return bpp; }
		}

		public static void Save(string directory, string file, XCImageCollection images, int bpp)
		{
			System.IO.BinaryWriter pck = new System.IO.BinaryWriter(System.IO.File.Create(directory + "\\" + file + ".pck"));
			System.IO.BinaryWriter tab = new System.IO.BinaryWriter(System.IO.File.Create(directory + "\\" + file + TAB_EXT));

			if (bpp == 2) {
				ushort count = 0;
				foreach (XCImage img in images) {
					tab.Write((ushort)count);
					ushort encLen = (ushort)PckImage.EncodePck(pck, img);
					count += encLen;
				}
			} else {
				uint count = 0;
				foreach (XCImage img in images) {
					tab.Write((uint)count);
					uint encLen = (uint)PckImage.EncodePck(pck, img);
					count += encLen;
				}
			}

			pck.Close();
			tab.Close();
		}
	}
}
