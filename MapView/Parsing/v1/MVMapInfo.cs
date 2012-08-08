using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;
using System.IO;

using UtilLib.Parser;
using MapLib.Base.Parsing;
using MapLib;
using MapLib.Base;

namespace MapView.Parsing.v1
{
	public class MVMapInfo : MapLib.Base.Parsing.MapInfo
	{
		private XCom.XCMapFile mMap;

		public MVMapInfo(MVTileset inParent, KeyVal inData)
			: base(inParent, inData)
		{
		}

		protected override void ProcessVar(VarCollection vars, KeyVal current)
		{
			base.ProcessVar(vars, current);
		}

		public override MapLib.Base.Map Map
		{
			get
			{
				if (mMap == null)
					mMap = new XCom.XCMapFile(this);

				return mMap;
			}
		}
	}
}
