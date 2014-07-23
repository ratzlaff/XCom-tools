using System;
using System.IO;
using System.Collections;
//using SDLDotNet;
using XCom.GameFiles.Map;

namespace XCom
{
	public class McdFile//:IEnumerable
	{
		private readonly XCTile[] _tiles;

        internal McdFile(XCTile[] tiles)
        {
            _tiles = tiles;
        }

	    public IEnumerator GetEnumerator()
		{
			return _tiles.GetEnumerator();
		}

		public XCTile this[int i]
		{
			get{return _tiles[i];}
		}

		public int Length
		{
			get{return _tiles.Length;}
		}
	}
}
