using System;
using XCom.Images;
using System.Collections.Generic;
using System.IO;
using System.Drawing;
using UtilLib;
using MapLib.Base;

namespace XCom
{
	public class SPECollection : XCImageCollection
	{
		class speImage:XCImage
		{
			public speImage(byte[] entries, int width, int height, Palette pal, int idx, string name)
				: base(entries, width, height, pal, idx)
			{
				mName = name;
			}
		}

		public SPECollection(string name, List<SpecEntry> entries)
		{
			Palette imgPalette = new Palette(name, 0, true);
			int i = 0;
			BinaryReader br;
			foreach (SpecEntry se in entries) {
				br = se.Reader;
				if (se.SpecType == SpecType.Palette) {
					ushort numColors = br.ReadUInt16();

					for (i = 0; i < (se.Size - 2) / 3; i++)
						imgPalette.Colors.Entries[i] = System.Drawing.Color.FromArgb(br.ReadByte(), br.ReadByte(), br.ReadByte());

					imgPalette.SetTransparent(true, 0);
				} else if (se.SpecType != SpecType.ColorTable) {
					ushort width = br.ReadUInt16();
					ushort height = br.ReadUInt16();
					Add(new speImage(br.ReadBytes(width * height), width, height, imgPalette, i++, se.Name));
				}
				br.Close();
			}

			Pal = imgPalette;
		}

		public static void Save(string directory, string file, string ext, XCImageCollection images)
		{
			/*
			System.IO.BinaryWriter bw = new System.IO.BinaryWriter(System.IO.File.Create(directory + "\\" + file + ext));
			foreach (XCImage tile in images)
				bw.Write(tile.Bytes);
			bw.Flush();
			bw.Close();
			 */
		}
	}
}
