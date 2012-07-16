using System;
using System.Collections.Generic;
using System.IO;
using System.Windows.Forms;
using System.Drawing;
using XCom.Interfaces;
using System.Reflection;

using UtilLib;
using UtilLib.Parser;

using MapLib.Base;
using MapLib;
#if NO
namespace XCom
{
	public class GameInfo
	{
		private static Palette currentPalette = XCPalette.TFTDBattle;

		private static Images_dat imageInfo;
		private static MapEdit_dat tileInfo;

		private static Dictionary<Palette, Dictionary<string, PckFile>> pckHash;

		public static event ParseLineDelegate ParseLine;
#if NOPE
		public static void Init(Palette p, PathInfo paths)
		{
			currentPalette = p;

			VarCollection vars = new VarCollection(new StreamReader(File.OpenRead(paths.ToString())));

			Directory.SetCurrentDirectory(paths.Path);

			xConsole.Init(20);
			KeyVal kv = null;

			while ((kv = vars.ReadLine()) != null) {
				switch (kv.Keyword) {
					/* mapedit */
					case "mapdata":
						tileInfo = new MapSetCollection(kv.Rest, vars);
						break;
					/* mapedit */
					case "images":
						imageInfo = new ImageInfo(kv.Rest, vars);
						break;
					case "useBlanks":
						Globals.UseBlanks = bool.Parse(kv.Rest);
						break;
					default:
						if (ParseLine != null)
							ParseLine(kv, vars);
						else
							xConsole.AddLine("Error in paths file: " + kv);
						break;
				}
			}

			vars.BaseStream.Close();
		}
#endif
#if NOPE
		public static ImageInfo ImageInfo
		{
			get { return imageInfo; }
		}

		public static MapSetCollection TilesetInfo
		{
			get { return tileInfo; }
		}

		public static Palette DefaultPalette
		{
			get { return currentPalette; }
			set { currentPalette = value; }
		}

		public static PckFile GetPckFile(string imageSet, Palette p)
		{
			return imageInfo.Images[imageSet].GetPckFile(p);
		}

		public static PckFile GetPckFile(string imageSet)
		{
			return GetPckFile(imageSet, currentPalette);
		}

		public static McdFile GetMcdFile(string imageSet)
		{
			return imageInfo.Images[imageSet].GetMcdFile(currentPalette);
		}

		public static McdFile GetMcdFile(string imageSet, Palette p)
		{
			return imageInfo.Images[imageSet].GetMcdFile(p);
		}

		public static Map GetMap(string tileset, string file)
		{
			return tileInfo.MapSets[tileset].Maps[file].Map;
		}

		public static PckFile CachePck(string basePath, string basename, int bpp, Palette p)
		{
			if (pckHash == null)
				pckHash = new Dictionary<Palette, Dictionary<string, PckFile>>();

			if (!pckHash.ContainsKey(p))
				pckHash.Add(p, new Dictionary<string, PckFile>());

			if (!pckHash[p].ContainsKey(basePath + basename))
				pckHash[p].Add(basePath + basename, new PckFile(File.OpenRead(basePath + basename + ".PCK"), File.OpenRead(basePath + basename + ".TAB"), basename, bpp, p));

			return pckHash[p][basePath + basename];
		}
#endif
	}
}
#endif
