using System.IO;

namespace XCom
{
	public class RmpEntry
	{
		#region rmprec

/*		typedef struct rmprec
		{
			unsigned char row;
			unsigned char col;
			unsigned char lvl;
			unsigned char zero03;				// Always 0
			LatticeLink   latlink[MaxLinks];	// 5 3-byte entries (shown above)
			unsigned char type1;				// Uses RRT_xxxx: 0,1,2,4
		observed
			unsigned char type2;				// Uses RRR_xxxx: 0,1,2,3,4,5, ,7,
		observed
			unsigned char type3;				// Uses RRR_xxxx: 0,1,2,3,4,5,6,7,8
		observed
			unsigned char type4;				// Almost always 0
			unsigned char type5;				// 0=Don't use 1=Use most of the time...2+
		= Use less and less often, 0 thru A observed
		} RmpRec; */

		#endregion

		private readonly byte _row;
		private readonly byte _col;
		private int _height;
		private readonly Link[] _links;

		public RmpEntry(byte idx, byte[] data)
		{
//			this.data = data;
			Index = idx;
			_row = data[0];
			_col = data[1];
			_height = data[2];

			_links = new Link[5];

			int x = 4;
			for (int i = 0; i < 5; i++)
			{
				_links[i] = new Link(
									data[x],
									data[x + 1],
									data[x + 2]);
				x += 3;
			}

			UType = (UnitType)data[19];
			URank1 = data[20];
			NodeImportance = (NodeImportance) data[21];
			BaseModuleAttack = (BaseModuleAttack)data[22];
			Spawn = (SpawnUsage)data[23];
		}

		public RmpEntry(byte idx, byte row, byte col, byte height)
		{
			Index = idx;
			_row = row;
			_col = col;
			_height = height;
			_links = new Link[5];
			for (int i = 0; i < 5; i++)
			{
				_links[i] = new Link(Link.NOT_USED, 0, 0);
			}
			UType = 0;
			URank1 = 0;
			NodeImportance = 0;
			BaseModuleAttack = 0;
			Spawn = 0;
		}

		public override bool Equals(object o)
		{
			var entry = o as RmpEntry;
			if (entry != null)
			{
				return Index == entry.Index;
			}
			return false;
		}

		public override int GetHashCode()
		{
			return Index;
		}

		public void Save(FileStream fs)
		{
			//fs.Write(data,0,data.Length);

			fs.WriteByte(_row);
			fs.WriteByte(_col);
			fs.WriteByte((byte)_height);
			fs.WriteByte(0);
			for (int i = 0; i < 5; i++)
			{
				fs.WriteByte(_links[i].Index);
				fs.WriteByte(_links[i].Distance);
				fs.WriteByte((byte) _links[i].UType);
			}
			fs.WriteByte((byte) UType);
			fs.WriteByte((byte) URank1);
			fs.WriteByte((byte) NodeImportance);
			fs.WriteByte((byte)BaseModuleAttack);
			fs.WriteByte((byte) Spawn);
		}

		public override string ToString()
		{
			string res = "";
			res += "r:" + _row + " c:" + _col + " h:" + _height;
			return res;
		}

		public byte Row
		{
			get { return _row; }
		}

		public byte Col
		{
			get { return _col; }
		}

		public int Height
		{
			get { return _height; }
			set { _height = value ; }
		}

		public UnitType UType
		{ get; set; }

		public byte URank1
		{ get; set; }

		public NodeImportance NodeImportance
		{ get; set; }

		public BaseModuleAttack BaseModuleAttack
		{ get; set; }

		public SpawnUsage Spawn
		{ get; set; }

		public int NumLinks
		{
			get { return _links.Length; }
		}

		public Link this[int i]
		{
			get { return _links[i]; }
		}

		/// <summary>
		/// gets the index of this RmpEntry
		/// </summary>
		public byte Index { get; set; }
	}
}
