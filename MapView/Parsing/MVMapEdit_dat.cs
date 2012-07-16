using System;
using System.IO;
using MapLib.Base.Parsing;
using UtilLib.Parser;

namespace MapView.Parsing
{
	public class MVMapEdit_dat : MapEdit_dat
	{
		public MVMapEdit_dat(string inName)
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
					case "tileset":
						MapCollection mapColl = new v1.MVMapCollection(kv.Rest);
						mapColl.Parse(vars);
						items.Add(mapColl.Name, mapColl);
						break;
					default:
						base.Parse(v);
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