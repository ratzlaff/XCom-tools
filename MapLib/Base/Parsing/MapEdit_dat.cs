using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using UtilLib;
using UtilLib.Parser;

namespace MapLib.Base.Parsing
{
	public class MapEdit_dat : FileDescType<MapCollection>
	{
		public MapEdit_dat(string inName)
			: base(inName)
		{
		}

		public override void Parse(VarCollection v)
		{
			StreamReader sr = new StreamReader(File.OpenRead(Path));
			VarCollection vars = new VarCollection(sr, v);
			KeyVal kv;

			while (vars.ReadLine(out kv)) {
				switch (kv.Keyword.ToLower()) {
					case "version":
						version = double.Parse(kv.Rest);
						break;
					default:
						Console.WriteLine("Unknown token: " + kv.Keyword);
						break;
				}
			}
			sr.Close();
		}

		public override void Save(string outFile)
		{
/*
			//iterate thru each tileset, call save on them
			VarCollection vc = new VarCollection("Path");
			StreamWriter sw = new StreamWriter(outFile);

			foreach (string s in tilesets.Keys) {
				IXCTileset ts = (IXCTileset)tilesets[s];
				if (ts == null)
					continue;
				vc.AddVar("rootPath", ts.MapPath);
				vc.AddVar("rmpPath", ts.RmpPath);
				vc.AddVar("blankPath", ts.BlankPath);
			}

			foreach (string v in vc.Variables) {
				Variable var = (Variable)vc.Vars[v];
				sw.WriteLine(var.Name + ":" + var.Value);
			}

			foreach (string s in tilesets.Keys) {
				if (tilesets[s] == null)
					continue;

				((IXCTileset)tilesets[s]).Save(sw, vc);
			}
*/
		}
	}
}
