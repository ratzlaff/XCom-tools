using System;
using System.Collections.ObjectModel;
using System.IO;
using System.Collections;
//using SDLDotNet;
using XCom.GameFiles.Map;

namespace XCom
{
	public class McdFile
		:
		ReadOnlyCollection<XCTile>
	{
		internal McdFile(XCTile[] tiles)
			:
			base(tiles)
		{}
	}
}
