using System;
using System.IO;
using System.Drawing;
using System.Drawing.Imaging;
using System.Reflection;
using System.Collections;
using DSShared;

namespace XCom
{
	/// <summary>
	/// A class defining a color array of 256 values
	/// </summary>
	/// 
	public class XCPalette : Palette
	{
		private static readonly char COMMENT = '#';
		private static readonly string embedPath = "XCom._Embedded.";

		private static Palette getEmbeddedPalette(string inName)
		{
			if (!LoadedPalettes.ContainsKey(inName)) {
				Assembly thisAssembly = Assembly.GetExecutingAssembly();
				LoadedPalettes.Add(inName, new XCPalette(thisAssembly.GetManifestResourceStream(embedPath + inName + ".pal")));
			}

			return LoadedPalettes[inName];
		}

		/// <summary>
		/// XCom-UFO palettes embedded in this assembly
		/// </summary>
		public static Palette UFOBattle { get { return getEmbeddedPalette("ufo-battle"); } }
		public static Palette UFOGeo { get { return getEmbeddedPalette("ufo-geo"); } }
		public static Palette UFOGraph { get { return getEmbeddedPalette("ufo-graph"); } }
		public static Palette UFOResearch { get { return getEmbeddedPalette("ufo-research"); } }

		/// <summary>
		/// XCom-TFTD palettes embedded in this assembly
		/// </summary>
		public static Palette TFTDBattle { get { return getEmbeddedPalette("tftd-battle"); } }
		public static Palette TFTDGeo { get { return getEmbeddedPalette("tftd-geo"); } }
		public static Palette TFTDGraph { get { return getEmbeddedPalette("tftd-graph"); } }
		public static Palette TFTDResearch { get { return getEmbeddedPalette("tftd-research"); } }

		// xcom palettes
		public XCPalette(Stream s)
			: base("", 0xFE)
		{
			StreamReader input = new StreamReader(s);
			string[] line = new string[0];
			mName = input.ReadLine();

			for (byte i = 0; i < 0xFF; i++) {
				string allLine = input.ReadLine().Trim();
				if (allLine[0] == COMMENT) {
					i--;
					continue;
				}
				line = allLine.Split(',');
				mPalette.Entries[i] = Color.FromArgb(int.Parse(line[0]), int.Parse(line[1]), int.Parse(line[2]));
			}

			//checkPalette();
		}
/*
		private void checkPalette()
		{
			Bitmap b = new Bitmap(1,1,PixelFormat.Format8bppIndexed);
			ColorPalette colors = b.Palette;
			b.Dispose();

			ArrayList cpList = new ArrayList(cp.Entries);
			ArrayList colorList = new ArrayList();

			for(int i=0;i<cpList.Count;i++)
			{
				if(!colorList.Contains(cpList[i]))
				{
					colorList.Add(cpList[i]);
					colors.Entries[i]=(Color)cpList[i];
				}
				else
				{
					Color c = (Color)cpList[i];
					int rc=c.R;
					int gc=c.G;
					int bc=c.B;

					if(rc==0)
						rc++;
					else
						rc--;

					if(gc==0)
						gc++;
					else
						gc--;

					if(bc==0)
						bc++;
					else
						bc--;

					colorList.Add(Color.FromArgb(rc,gc,bc));
					colors.Entries[i]=Color.FromArgb(rc,gc,bc);
				}				
			}
		}
*/
	}
}
