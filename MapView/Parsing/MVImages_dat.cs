using MapLib.Base.Parsing;
using UtilLib.Parser;

using System.IO;

namespace MapView.Parsing
{
	public class MVImages_dat : Images_dat
	{
		public MVImages_dat(string inFile)
			: base(inFile)
		{
		}

		public override void Parse(VarCollection v)
		{
			StreamReader sr = new StreamReader(File.OpenRead(Path));
			VarCollection vars = new VarCollection(sr, v);

			KeyVal kv = null;

			while (vars.ReadLine(out kv)) {
				ImageInfo img = new v1.MVImageInfo(this, kv);
				items.Add(kv.Keyword, img);
			}
			sr.Close();
		}

		public override void Save(string outFile)
		{
			throw new System.NotImplementedException();
		}
	}
}