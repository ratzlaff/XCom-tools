using MapLib.Base.Parsing;
using UtilLib.Parser;

using MapView.Parsing.v1;

namespace MapView.Parsing.v1
{
	public class MVTileset : Tileset
	{
		private MVMapCollection mMapCollectionParent;
		public MVTileset(MVMapCollection inParent, string inName)
			: base(inParent, inName)
		{
			mMapCollectionParent = inParent;
		}

		public new MVMapCollection MapCollection
		{
			get { return mMapCollectionParent; }
		}

		protected override void ProcessVar(VarCollection vars, KeyVal current)
		{
			MVMapInfo mi = new MVMapInfo(this, current);
			mCollection.Add(mi);
		}
	}
}