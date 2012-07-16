using System;
using System.IO;
using System.Collections.Generic;
using UtilLib;
using UtilLib.Parser;
using MapLib.Base;
using MapLib.Base.Parsing;

namespace MapLib.Base.Parsing
{
	public abstract class Images_dat : FileDescType<ImageInfo>
	{
		public Images_dat(string inFile)
			: base(inFile)
		{
		}
/*
		public override void Save(string outFile)
		{
			StreamWriter sw = new StreamWriter(outFile);

			List<string> a = new List<string>(images.Keys);
			a.Sort();
			Dictionary<string, Variable> vars = new Dictionary<string, Variable>();

			foreach (string str in a)
				if (images[str] != null) {
					ImageDescriptor id = images[str];
					if (vars[id.BasePath] == null)
						vars[id.BasePath] = new Variable(id.BaseName + ":", id.BasePath);
					else
						vars[id.BasePath].Inc(id.BaseName + ":");
				}

			foreach (string basePath in vars.Keys)
				vars[basePath].Write(sw);

			sw.Flush();
			sw.Close();
		}
*/

/*
		public static ImageCollection CacheImage(string inImage)
		{
			if (pckHash == null)
				pckHash = new Dictionary<Palette, Dictionary<string, PckFile>>();

			if (!pckHash.ContainsKey(p))
				pckHash.Add(p, new Dictionary<string, PckFile>());

			if (!pckHash[p].ContainsKey(basePath + basename))
				pckHash[p].Add(basePath + basename, new PckFile(File.OpenRead(basePath + basename + ".PCK"), File.OpenRead(basePath + basename + ".TAB"), basename, bpp, p));

			return pckHash[p][basePath + basename];
		}
*/
	}
}
