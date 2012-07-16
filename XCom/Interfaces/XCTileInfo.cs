using System;
using MapLib.Base;

namespace XCom.Interfaces.Base
{
	public class XCTileInfo : TileInfo
	{
		protected XCTileInfo(int id)
			: base(id)
		{
		}

		public virtual sbyte TileOffset { get { return 0; } }
		public virtual sbyte StandOffset { get { return 0; } }
		public virtual TileType TileType { get { return TileType.All; } }
		public virtual SpecialType SpecialType { get { return SpecialType.Tile; } }
		public virtual bool HumanDoor { get { return false; } }
		public virtual bool UFODoor { get { return false; } }
	}
}
