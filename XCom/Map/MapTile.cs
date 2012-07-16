using System;
using System.Drawing;
using System.Collections;
using XCom.Interfaces;
using MapLib.Base;

namespace XCom
{
	public class XCMapTile : MapTile
	{
		public enum MapQuadrant { Ground, West, North, Content };
		private RmpEntry rmpInfo;
		private Tile ground, north, west, content;
		private bool blank;

		//private IUnit unit;

		//		private int maxSub = 0;		
		private int standOffset = 0;

		internal XCMapTile(Tile ground, Tile west, Tile north, Tile content)
		{
			this.ground = ground;
			this.north = north;
			this.west = west;
			this.content = content;

			calcTiles();
			drawAbove = true;
			blank = false;
			//unit = null;
		}

		public static XCMapTile BlankTile
		{
			get
			{
				XCMapTile mt = new XCMapTile(null, null, null, null);
				mt.blank = true;
				return mt;
			}
		}

		/// <summary>
		/// If drawing a unit 
		/// </summary>
		public int StandOffset
		{
			get { return standOffset; }
		}

		private void calcTiles()
		{
			/*
						int notNull = 0;
						maxSub = -255;
						if (ground != null)
						{
							notNull++;
							maxSub = ground.Info.TileOffset;
							standOffset = ground.Info.StandOffset;
						}
						if (north != null)
						{
							notNull++;
							maxSub = Math.Max(maxSub, north.Info.TileOffset);
							standOffset = Math.Max(standOffset, north.Info.TileOffset);
						}
						if (west != null)
						{
							notNull++;
							maxSub = Math.Max(maxSub, west.Info.TileOffset);
							standOffset = Math.Max(standOffset, west.Info.TileOffset);
						}
						if (content != null)
						{
							notNull++;
							maxSub = Math.Max(maxSub, content.Info.TileOffset);
							standOffset = Math.Max(standOffset, content.Info.TileOffset);
						}
						*/
			if (ground != null)
				usedTiles.Add(ground);
			if (north != null)
				usedTiles.Add(north);
			if (west != null)
				usedTiles.Add(west);
			if (content != null)
				usedTiles.Add(content);
		}

		public bool Blank
		{
			get { return blank; }
			set { blank = value; }
		}

		//public MapLocation MapCoords
		//{
		//    get{return mapCoords;}
		//    set{mapCoords=value;}
		//}

		public Tile this[MapQuadrant quad]
		{
			get
			{
				switch (quad) {
					case MapQuadrant.Ground:
						return Ground;
					case MapQuadrant.Content:
						return Content;
					case MapQuadrant.North:
						return North;
					case MapQuadrant.West:
						return West;
					default:
						return null;
				}
			}
			set
			{
				switch (quad) {
					case MapQuadrant.Ground:
						Ground = value;
						break;
					case MapQuadrant.Content:
						Content = value;
						break;
					case MapQuadrant.North:
						North = value;
						break;
					case MapQuadrant.West:
						West = value;
						break;
				}
			}
		}

		public override void Paste(MapTile mapTile)
		{
			if (mapTile is XCMapTile) {
				XCMapTile copyTile = (XCMapTile)mapTile;
				Ground = copyTile.Ground;
				Content = copyTile.Content;
				West = copyTile.West;
				North = copyTile.North;
			} else {
				throw new InvalidCastException("Incoming tile is not a XCMapTile");
			}
		}

		//public IUnit Unit
		//{
		//	get { return unit; }
		//	set { unit = value; }
		//}

		public Tile North
		{
			get { return north; }
			set { north = value; calcTiles(); }
		}

		public Tile Content
		{
			get { return content; }
			set { content = value; calcTiles(); }
		}

		public Tile Ground
		{
			get { return ground; }
			set { ground = value; calcTiles(); }
		}

		public Tile West
		{
			get { return west; }
			set { west = value; calcTiles(); }
		}

		public RmpEntry Rmp
		{
			get { return rmpInfo; }
			set { rmpInfo = value; }
		}
	}
}
