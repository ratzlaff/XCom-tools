using System;
using System.Collections.Generic;
using System.IO;
using System.Drawing;
using System.Drawing.Imaging;

namespace UtilLib
{
	public enum SpecType
	{
			Invalid = 0
		,	ColorTable
		,	Palette
		,	Image
		,	Foretile
		,	Backtile
		,	Character
		,	MorphPoints8
		,	MorphPoints16
		,	GrueObjs
		,	ExternSfx
		,	DmxMus
		,	PatchedMorph
		,	NormalFile
		,	CompressedFile
		,	VectorImage
		,	LightList
		,	GrueForeground
		,	GrueBackground
		,	DataArray
		,	Character2
		,	Particle
		,	ExternalLCache
	}
	public class SpecEntry
	{
		private string mName;
		private SpecType mFileType;
		private uint mFileSize, mDataOffset;
		private byte mFlags;
		private byte[] mMasterData;

		public SpecEntry(BinaryReader br, byte[] data)
		{
			mMasterData = data;
			mFileType = (SpecType)br.ReadByte();
			byte len = br.ReadByte();
			mName = new string(br.ReadChars(len));
			if (mName[mName.Length - 1] == '\0')
				mName = mName.Substring(0, mName.Length - 1);
			mFlags = br.ReadByte();
			mFileSize = br.ReadUInt32();
			mDataOffset = br.ReadUInt32();
		}

		public string Name { get { return mName; } }
		public uint Size { get { return mFileSize; } }
		public SpecType SpecType { get { return mFileType; } }
		public uint Offset
		{
			get { return mDataOffset; }
			set { mDataOffset = value; }
		}

		public BinaryReader Reader
		{
			get { return new BinaryReader(new MemoryStream(mMasterData, (int)mDataOffset, (int)mFileSize)); }
		}
	}

	public class SpecCollection
	{
		byte[] buffer;
		char[] signature;
		Dictionary<string, SpecEntry> entries;
		List<SpecEntry> entryList;
//		int imgWidth, imgHeight;

		public SpecCollection(Stream specFile)
		{
			Bmp.DefaultTransparentIndex = 0;
			entries = new Dictionary<string, SpecEntry>();
			entryList = new List<SpecEntry>();
//			imgWidth = imgHeight = 0;

			buffer = new byte[specFile.Length];
			specFile.Read(buffer, 0, buffer.Length);
			specFile.Close();

			MemoryStream ms = new MemoryStream(buffer);
			BinaryReader br = new BinaryReader(ms);

			signature = br.ReadChars(8);

			UInt16 total = br.ReadUInt16();
			int num = 0;

			for (int i = 0; i < total; i++) {
				SpecEntry se = new SpecEntry(br, buffer);
				entryList.Add(se);
				if (entries.ContainsKey(se.Name)) {
					entries.Add(se.Name + num, se);
					num++;
				} else
					entries.Add(se.Name, se);
			}

			uint pos = (uint)br.BaseStream.Position;
			foreach (SpecEntry se in entryList) {
				se.Offset = pos;
				pos += se.Size;
			}
			br.Close();

			/*
			Palette pal = null;
			if (entries.ContainsKey("palette"))
				pal = new Palette(entries["palette"]);

			for (int i = 0; i < entryList.Count; i++) {
				Entry se = entryList[i];
				if (se.Name != "palette") {
					Image si = new Image(se, pal);
					Add(si);
					imgHeight = Math.Max(imgHeight, si.Height);
					imgWidth = Math.Max(imgWidth, si.Width);
				}
			}

			foreach (Image si in this) {
				si.Resize(imgHeight, imgWidth);
			}
			*/
		}

		public List<SpecEntry> EntryList
		{
			get { return entryList; }
		}

		public Dictionary<string, SpecEntry> Entries
		{
			get { return entries; }
		}
		/*
		public class Palette
		{
			private ColorPalette cp;

			public Palette(SpecEntry se)
			{
				Bitmap b = new Bitmap(1, 1, PixelFormat.Format8bppIndexed);
				cp = b.Palette;
				b.Dispose();

				BinaryReader br = se.Reader;
				ushort numColors = br.ReadUInt16();

				for (int i = 0; i < (se.Size - 2) / 3; i++)
					cp.Entries[i] = System.Drawing.Color.FromArgb(br.ReadByte(), br.ReadByte(), br.ReadByte());

				cp.Entries[0] = System.Drawing.Color.FromArgb(cp.Entries[0].R, cp.Entries[0].G, cp.Entries[0].B, 0);
				br.Close();
			}

			public ColorPalette Colors { get { return cp; } }
		}

		public class Image
		{
			private int width, height;
			private Bitmap image;
			private byte[] data;
			private SpecEntry se;
			private Palette pal;

			public Image(SpecEntry se, Palette pal)
			{
				this.se = se;
				this.pal = pal;

				BinaryReader br = se.Reader;
				width = br.ReadUInt16();
				height = br.ReadUInt16();

				data = br.ReadBytes(width * height);

				image = Bmp.MakeBitmap8(width, height, data, pal.Colors);
				br.Close();
			}

			public SpecEntry SpecEntry
			{
				get { return se; }
			}

			public int Width
			{
				get { return width; }
			}

			public int Height
			{
				get { return height; }
			}

			public Bitmap Bitmap
			{
				get { return image; }
			}

			public void Resize(int imgHeight, int imgWidth)
			{
				Bitmap newB = Bmp.MakeBitmap(imgWidth, imgHeight, pal.Colors);
				Bmp.Draw(image, newB, (imgWidth - width) / 2, (imgHeight - height) / 2);
				image = newB;
			}
		}
		 * */
	}
}