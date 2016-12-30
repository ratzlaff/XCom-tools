using System;
using System.Collections.Generic;
using System.IO;
using System.Windows.Forms;
using System.Drawing;
using XCom.Interfaces;
using System.Reflection;
using XCom.Interfaces.Base;

namespace XCom
{
	public delegate void ParseLineDelegate(KeyVal kv, VarCollection vars);

	public class GameInfo
	{
		private static Palette currentPalette = Palette.UFOBattle;
//		private static Palette currentPalette = Palette.TFTDBattle;

		private static ImageInfo imageInfo;
		private static TilesetDesc tileInfo;
//		private static IWarningHandler WarningHandler;

		private static Dictionary<Palette, Dictionary<string, PckFile>> pckHash;

		public static event ParseLineDelegate ParseLine;

		public static void Init(Palette p, DSShared.PathInfo paths)
		{
			currentPalette = p;
			pckHash = new Dictionary<Palette, Dictionary<string, PckFile>>();

			VarCollection vars = new VarCollection(new StreamReader(File.OpenRead(paths.ToString())));

			Directory.SetCurrentDirectory(paths.Path);

			xConsole.Init(20);
			KeyVal kv = null;

			while ((kv = vars.ReadLine()) != null)
			{
				switch (kv.Keyword)
				{
					case "mapdata": /* mapedit */
						tileInfo = new TilesetDesc(kv.Rest, vars);
						break;

					case "images": /* mapedit */
						imageInfo = new ImageInfo(kv.Rest, vars);
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

		public static ImageInfo ImageInfo
		{
			get { return imageInfo; }
		}

		public static TilesetDesc TilesetInfo
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

		public static PckFile CachePck(
									string basePath,
									string basename,
									int bpp,
									Palette p)
		{
			if (pckHash == null)
				pckHash = new Dictionary<Palette, Dictionary<string, PckFile>>();

			if (!pckHash.ContainsKey(p))
				pckHash.Add(p, new Dictionary<string, PckFile>());

			//if(pckHash[p][basePath+basename]==null)
			var path = basePath + basename;
			var paleteHash = pckHash[p];

			if (!paleteHash.ContainsKey(path))
			{
				using (var pckStream = File.OpenRead(path + ".PCK"))
				using (var tabStream = File.OpenRead(path + ".TAB"))
				{
					paleteHash.Add(path, new PckFile(
													pckStream,
													tabStream,
													bpp,
													p));
				}
			}

			return pckHash[p][basePath + basename];
		}

		public static void ClearPckCache(string basePath, string basename)
		{
			var path = basePath + basename;
			foreach (var paleteHash in pckHash.Values)
				if (paleteHash.ContainsKey(path))
					paleteHash.Remove(path);
		}
	}
}
