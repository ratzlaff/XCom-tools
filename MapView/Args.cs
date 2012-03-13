using System;
using XCom;

namespace MapView
{
	public enum ArgType{NewMap,MapClicked};

	public class Args:EventArgs
	{
		private ArgType type;
		private MapLib.MapLocation location;

		public Args(ArgType type)
		{
			this.type=type;
		}

		public ArgType Type
		{
			get{return type;}
		}
		
		public MapLib.MapLocation Location
		{
			get{return location;}
			set{location=value;}
		}
	}
}