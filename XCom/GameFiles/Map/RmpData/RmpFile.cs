using System;
using System.Collections.Generic;
using System.IO;
using System.Collections;

// when determining spawn positions the following nodes are tried
// Leader > Nav > Eng> Unknown8 > Med > Soldier > Any
// Nav > Eng > Soldier > Any
// Human is only used for xcom units
// civilians can only start on any nodes
// unknown6 is only used as an alternate choice to a medic
// unknown8 is only used by squad leaders and commanders

// unknown8 = any alien
// unknown6 = Medic,Navigator,Soldier,Terrorist

	/*
		UFO			TFTD
		Commander	Commander
		Leader		Navigator
		Engineer	Medic
		Medic		Technition
		Navigator	SquadLeader
		Soldier		Soldier
	*/

#region about the RMP file
/*
// Route Record for RMP files
//
// RMP records describe the points in a lattice and the links connecting
// them.  However, special values of rmpindex are:
//   FF = Not used
//   FE = Exit terrain to north
//   FD = Exit terrain to east
//   FC = Exit terrain to south
//   FB = Exit terrain to west

typedef struct latticelink
{
	unsigned char rmpindex; // Index of a linked point in the
lattice
	unsigned char distance; // Approximate distance from current
point
	unsigned char linktype; // Uses RRT_xxxx
} LatticeLink;

#define MaxLinks 5

typedef struct rmprec
{
	unsigned char row;
	unsigned char col;
	unsigned char lvl;
	unsigned char zero03;			// Always 0
	LatticeLink latlink[MaxLinks];	// 5 3-byte entries (shown above)
	unsigned char type1;			// Uses RRT_xxxx: 0,1,2,4
observed
	unsigned char type2;			// Uses RRR_xxxx: 0,1,2,3,4,5, ,7,
observed
	unsigned char type3;			// Uses RRR_xxxx: 0,1,2,3,4,5,6,7,8
observed
	unsigned char type4;			// Almost always 0
	unsigned char type5;			// 0=Don't use 1=Use most of the time...2+
= Use less and less often, 0 thru A observed
} RmpRec;

// Types of Units

#define RRT_Any		0
#define RRT_Flying	1
#define RRT_Small	2
#define RRT_4		4 // Unknown

// Ranks of Units

#define RRR_0			0 // Unknown
#define RRR_Human		1
#define RRR_Soldier		2
#define RRR_Navigator	3
#define RRR_Leader		4
#define RRR_Engineer	5
#define RRR_6			6 // Commander?
#define RRR_Medic		7
#define RRR_8			8 // Unknown
*/

#endregion
namespace XCom
{
	public enum UnitType
		: byte
	{
		Any = 0,
		Flying,
		Small,
		FlyingLarge,
		Large
	};

	public enum UnitRankUFO
		: byte
	{
		Civilian = 0,
		XCom,
		Soldier,
		Navigator,
		LeaderCommander,
		Engineer,
		Misc1,
		Medic,
		Misc2
	};

	public enum UnitRankTFTD
		: byte
	{
		Civilian = 0,
		XCom,
		Soldier,
		SquadLeader,
		LeaderCommander,
		Medic,
		Misc1,
		Technician,
		Misc2
	};

	public enum SpawnUsage
		: byte
	{
		NoSpawn	= 0,
		Spawn1	= 1,
		Spawn2	= 2,
		Spawn3	= 3,
		Spawn4	= 4,
		Spawn5	= 5,
		Spawn6	= 6,
		Spawn7	= 7,
		Spawn8	= 8,
		Spawn9	= 9,
		Spawn10	= 10
	};

	public enum NodeImportance
		: byte
	{
		Zero = 0,
		One,
		Two,
		Three,
		Four,
		Five,
		Six,
		Seven,
		Eight,
		Nine,
		Ten
	};

	public enum BaseModuleAttack
		: byte
	{
		Zero = 0,
		One,
		Two,
		Three,
		Four,
		Five,
		Six,
		Seven,
		Eight,
		Nine,
		Ten
	};

	public enum LinkTypes
		: byte
	{
		NotUsed		= 0xFF,
		ExitNorth	= 0xFE,
		ExitEast	= 0xFD,
		ExitSouth	= 0xFC,
		ExitWest	= 0xFB
	};


	public class RmpFile
		:
		IEnumerable<RmpEntry>
	{
		private readonly List<RmpEntry> _entries;
		private readonly string _basename;
		private readonly string _basePath;

		public static readonly object[] UnitRankUFO =
		{
			new StrEnum("0:Civ-Scout",			XCom.UnitRankUFO.Civilian),
			new StrEnum("1:XCom",				XCom.UnitRankUFO.XCom),
			new StrEnum("2:Soldier",			XCom.UnitRankUFO.Soldier),
			new StrEnum("3:Navigator",			XCom.UnitRankUFO.Navigator),
			new StrEnum("4:Leader/Commander",	XCom.UnitRankUFO.LeaderCommander),
			new StrEnum("5:Engineer",			XCom.UnitRankUFO.Engineer),
			new StrEnum("6:Misc1",				XCom.UnitRankUFO.Misc1),
			new StrEnum("7:Medic",				XCom.UnitRankUFO.Medic),
			new StrEnum("8:Misc2",				XCom.UnitRankUFO.Misc2)
		};

		public static readonly object[] UnitRankTFTD =
		{
			new StrEnum("0:Civ-Scout",			XCom.UnitRankTFTD.Civilian),
			new StrEnum("1:XCom",				XCom.UnitRankTFTD.XCom),
			new StrEnum("2:Soldier",			XCom.UnitRankTFTD.Soldier),
			new StrEnum("3:Squad Leader",		XCom.UnitRankTFTD.SquadLeader),
			new StrEnum("4:Leader/Commander",	XCom.UnitRankTFTD.LeaderCommander),
			new StrEnum("5:Medic",				XCom.UnitRankTFTD.Medic),
			new StrEnum("6:Misc1",				XCom.UnitRankTFTD.Misc1),
			new StrEnum("7:Technician",			XCom.UnitRankTFTD.Technician),
			new StrEnum("8:Misc2",				XCom.UnitRankTFTD.Misc2)
		};

		public static readonly object[] SpawnUsage =
		{
			new StrEnum("0:No Spawn",	XCom.SpawnUsage.NoSpawn),
			new StrEnum("1:Spawn",		XCom.SpawnUsage.Spawn1),
			new StrEnum("2:Spawn",		XCom.SpawnUsage.Spawn2),
			new StrEnum("3:Spawn",		XCom.SpawnUsage.Spawn3),
			new StrEnum("4:Spawn",		XCom.SpawnUsage.Spawn4),
			new StrEnum("5:Spawn",		XCom.SpawnUsage.Spawn5),
			new StrEnum("6:Spawn",		XCom.SpawnUsage.Spawn6),
			new StrEnum("7:Spawn",		XCom.SpawnUsage.Spawn7),
			new StrEnum("8:Spawn",		XCom.SpawnUsage.Spawn8),
			new StrEnum("9:Spawn",		XCom.SpawnUsage.Spawn9),
			new StrEnum("10:Spawn",		XCom.SpawnUsage.Spawn10)
		};

		internal RmpFile(string basename, string basePath)
		{
			_basename = basename;
			_basePath = basePath;
			_entries = new List<RmpEntry>();

			if (File.Exists(basePath + basename + ".RMP"))
			{
				var bs = new BufferedStream(File.OpenRead(basePath + basename + ".RMP"));

				for (byte i = 0; i < bs.Length / 24; i++)
				{
					var data = new byte[24];
					bs.Read(data, 0, 24);
					_entries.Add(new RmpEntry(i, data));
				}
				bs.Close();
			}
		}

		public void Save()
		{
			Save(File.Create(_basePath + _basename + ".RMP"));
		}

		public void Save(FileStream fs)
		{
			for (int i = 0; i < _entries.Count; i++)
				_entries[i].Save(fs);

			fs.Close();
		}

		IEnumerator<RmpEntry> IEnumerable<RmpEntry>.GetEnumerator()
		{
			return _entries.GetEnumerator();
		}

		public IEnumerator GetEnumerator()
		{
			return _entries.GetEnumerator();
		}

		public RmpEntry this[int i]
		{
			get
			{
				if (i > -1 && i < _entries.Count)
					return _entries[i];

				return null;
			}
		}

		public int Length
		{
			get { return _entries.Count; }
		}

		public byte ExtraHeight
		{ get; set; }

		public void RemoveEntry(RmpEntry r)
		{
			int oldIdx = r.Index;

			_entries.Remove(r);

			foreach (var rr in _entries)
			{
				if (rr.Index > oldIdx)
					rr.Index--;

				for (int i = 0; i < 5; i++)
				{
					var l = rr[i];

					if (l.Index == oldIdx)
					{
						l.Index = Link.NOT_USED;
					}
					else if (l.Index > oldIdx && l.Index < 0xFB)
					{
						l.Index--;
					}
				}
			}
		}

		public RmpEntry AddEntry(byte row, byte col, byte height)
		{
			var re = new RmpEntry((byte)_entries.Count, row, col, height);
			_entries.Add(re);
			return re;
		}

		public void CheckRouteEntries(int newC, int newR, int newH)
		{
			var toDelete = new List<RmpEntry>();

			foreach (var entry in _entries)
				if (IsOutsideMap(entry, newC, newR, newH))
					toDelete.Add(entry);

			foreach (var entry in toDelete)
				RemoveEntry(entry);
		}

		public static bool IsOutsideMap(
									RmpEntry entry,
									int cols,
									int rows,
									int height)
		{
			return entry.Col < 0    || entry.Col >= cols
				|| entry.Row < 0    || entry.Row >= rows
				|| entry.Height < 0 || entry.Height >= height;
		}

//		public RmpEntry GetEntryAtHeight(byte currentHeight)
//		{
//			foreach (var route in _entries)
//				if (route.Height == currentHeight)
//					return route;
//
//			return null;
//		}
	}
}
