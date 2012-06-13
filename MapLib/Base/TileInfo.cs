using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MapLib.Base
{
	public class TileInfo
	{
		protected int id;

		protected TileInfo(int id)
		{
			this.id = id;
		}

		public int ID { get { return id; } }
	}
}
