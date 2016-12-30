using System;
using System.Collections;
using System.Collections.Generic;
using System.Drawing;
using XCom.Interfaces;

#region About the mcdEntry
// http://ufo2k-allegro.lxnt.info/srcdocs/terrapck_8h-source.html
/* struct MCD
{
	-unsigned char Frame[8];		// Each frame is an index into the ____.TAB file; it rotates between the frames constantly.
	-unsigned char LOFT[12];		// The 12 levels of references into GEODATA\LOFTEMPS.DAT
	-short int ScanG;				// A reference into the GEODATA\SCANG.DAT
	unsigned char u23;	22
	unsigned char u24;	23
	unsigned char u25;	24
	unsigned char u26;	25
	unsigned char u27;	26
	unsigned char u28;	27
	unsigned char u29;	28
	unsigned char u30;	29
	unsigned char UFO_Door;			// If it's a UFO door it uses only Frame[0] until it is walked through, then it animates once and becomes Alt_MCD. It changes back at the end of the turn.
	unsigned char Stop_LOS;			// You cannot see through this tile.
	unsigned char No_Floor;			// If 1, then a non-flying unit can't stand here
	unsigned char Big_Wall;			// It's an object (tile type 3), but it acts like a wall
	unsigned char Gravlift;
	unsigned char Door;				// It's a human style door--you walk through it and it changes to Alt_MCD
	unsigned char Block_Fire;		// If 1, fire won't go through the tile
	unsigned char Block_Smoke;		// If 1, smoke won't go through the tile
	unsigned char u39;
	unsigned char TU_Walk;			// The number of TUs require to pass the tile while walking. 0xFF (255) means it's unpassable.
	unsigned char TU_Fly;			// remember, 0xFF means it's impassable!
	unsigned char TU_Slide;			// sliding things include snakemen and silacoids
	unsigned char Armour;			// The higher this is the less likely it is that a weapon will destroy this tile when it's hit.
	unsigned char HE_Block;			// How much of an explosion this tile will block
	unsigned char Die_MCD;			// If the terrain is destroyed, it is set to 0 and a tile of type Die_MCD is added
	unsigned char Flammable;		// How flammable it is (the higher the harder it is to set aflame)
	unsigned char Alt_MCD;			// If "Door" or "UFO_Door" is on, then when a unit walks through it the door is set to 0 and a tile type Alt_MCD is added.
	unsigned char u48;
	signed char T_Level;			// When a unit or object is standing on the tile, the unit is shifted by this amount
	unsigned char P_Level;			// When the tile is drawn, this amount is subtracted from its y (so y position-P_Level is where it's drawn)
	unsigned char u51;
	unsigned char Light_Block;		// The amount of light it blocks, from 0 to 10
	unsigned char Footstep;			// The Sound Effect set to choose from when footsteps are on the tile
	unsigned char Tile_Type;		// This is the type of tile it is meant to be -- 0=floor, 1=west wall, 2=north wall, 3=object. When this type of tile is in the Die_As or Open_As flags, this value is added to the tile coordinate to determine the byte in which the tile type should be written.
	unsigned char HE_Type;			// 0=HE  1=Smoke
	unsigned char HE_Strength;		// The strength of the explosion caused when it's destroyed.  0 means no explosion.
	unsigned char Smoke_Blockage;	// ? Not sure about this ...
	unsigned char Fuel;				// The number of turns the tile will burn when set aflame
	unsigned char Light_Source;		// The amount of light this tile produces
	unsigned char Target_Type;		// The special properties of the tile
	unsigned char u61;
	unsigned char u62;
}; */
#endregion

namespace XCom
{
	public enum TileType
	{
		Ground		=  0,
		WestWall	=  1,
		NorthWall	=  2,
		Object		=  3,
		All			= -1
	};

	public class McdEntry:XCom.Interfaces.Base.IInfo
	{
		private static int _globalStaticId = 0;

		internal McdEntry()
		{
			ID = _globalStaticId++;
		}

		public int ID { get; private set; }

		public Rectangle Bounds { get; set; }

		public int Width  { get; set; }
		public int Height { get; set; }

		public byte Image1 { get; set; }
		public byte Image2 { get; set; }
		public byte Image3 { get; set; }
		public byte Image4 { get; set; }
		public byte Image5 { get; set; }
		public byte Image6 { get; set; }
		public byte Image7 { get; set; }
		public byte Image8 { get; set; }

		public byte Loft1  { get; set; }
		public byte Loft2  { get; set; }
		public byte Loft3  { get; set; }
		public byte Loft4  { get; set; }
		public byte Loft5  { get; set; }
		public byte Loft6  { get; set; }
		public byte Loft7  { get; set; }
		public byte Loft8  { get; set; }
		public byte Loft9  { get; set; }
		public byte Loft10 { get; set; }
		public byte Loft11 { get; set; }
		public byte Loft12 { get; set; }

		public ushort ScanG { get; set; }

		public byte Unknown22 { get; set; }													// unsigned char u62;
		public byte Unknown23 { get; set; }													// unsigned char u62;
		public byte Unknown24 { get; set; }													// unsigned char u62;
		public byte Unknown25 { get; set; }													// unsigned char u62;
		public byte Unknown26 { get; set; }													// unsigned char u62;
		public byte Unknown27 { get; set; }													// unsigned char u62;
		public byte Unknown28 { get; set; }													// unsigned char u62;
		public byte Unknown29 { get; set; }													// unsigned char u62;

		public bool UFODoor		{ get; set; }			// info[30]==1;}}														// If it's a UFO door it uses only Frame[0] until it is walked through, then it animates once and becomes Alt_MCD. It changes back at the end of the turn.
		public bool StopLOS		{ get; set; }			// info[31]!=1;}}					// unsigned char Stop_LOS;			// You cannot see through this tile.
		public bool NoGround	{ get; set; }			// info[32]==1;}}					// unsigned char No_Floor;			// If 1, then a non-flying unit can't stand here
		public bool BigWall		{ get; set; }			// info[33]==1;}}					// unsigned char Big_Wall;			// It's an object (tile type 3), but it acts like a wall
		public bool GravLift	{ get; set; }			// info[34]==1;}}					// unsigned char Gravlift;
		public bool HumanDoor	{ get; set; }			// info[35]==1;}}					// unsigned char Door;				// It's a human style door--you walk through it and it changes to Alt_MCD - does not change back at end of turn
		public bool BlockFire	{ get; set; }			// info[36]==1;}}					// unsigned char Block_Fire;		// If 1, fire won't go through the tile
		public bool BlockSmoke	{ get; set; }			// info[37]==1;}}					// unsigned char Block_Smoke;		// If 1, smoke won't go through the tile

		public byte Unknown38		{ get; set; }		// info[38];}}						// unsigned char u39;
		public byte TU_Walk			{ get; set; }		// info[39];}}						// unsigned char TU_Walk;			// The number of TUs require to pass the tile while walking. 0xFF (255) means it's unpassable.
		public byte TU_Fly			{ get; set; }		// info[40];}}						// unsigned char TU_Fly;			// remember, 0xFF means it's impassable!
		public byte TU_Slide		{ get; set; }		// info[41];}}						// unsigned char TU_Slide;			// sliding things include snakemen and silacoids
		public byte Armor			{ get; set; }		// info[42];}}						// unsigned char Armour;			// The higher this is the less likely it is that a weapon will destroy this tile when it's hit.
		public byte HE_Block		{ get; set; }		// info[43];}}						// unsigned char HE_Block;			// How much of an explosion this tile will block
		public byte DieTile			{ get; set; }		// info[44];}}						// unsigned char Die_MCD;			// If the terrain is destroyed, it is set to 0 and a tile of type Die_MCD is added
		public byte Flammable		{ get; set; }		// info[45];}}						// unsigned char Flammable;			// How flammable it is (the higher the harder it is to set aflame)
		public byte Alt_MCD			{ get; set; }		// info[46];}}						// unsigned char Alt_MCD;			// If "Door" or "UFO_Door" is on, then when a unit walks through it the door is set to 0 and a tile type Alt_MCD is added.
		public byte Unknown47		{ get; set; }		// info[47];}}						// unsigned char u48;
		public sbyte StandOffset	{ get; set; }		// (sbyte)info[48];}}				// signed char T_Level;				// When a unit or object is standing on the tile, the unit is shifted by this amount
		public sbyte TileOffset		{ get; set; }		// (sbyte)info[49];}}				// unsigned char P_Level;			/ /When the tile is drawn, this amount is subtracted from its y (so y position-P_Level is where it's drawn)
		public byte Unknown50		{ get; set; }		// info[50];}}						// unsigned char u51;
		public sbyte LightBlock		{ get; set; }		// (sbyte)info[51];}}				// unsigned char Light_Block;		// The amount of light it blocks, from 0 to 10
		public sbyte Footstep		{ get; set; }		// (sbyte)info[52];}}				// unsigned char Footstep;			// The Sound Effect set to choose from when footsteps are on the tile

		public TileType TileType		{ get; set; }	// (TileType)info[53];}}			// unsigned char Tile_Type;			// This is the type of tile it is meant to be -- 0=floor, 1=west wall, 2=north wall, 3=object. When this type of tile is in the Die_As or Open_As flags, this value is added to the tile coordinate to determine the byte in which the tile type should be written.
		public sbyte HE_Type			{ get; set; }	// (sbyte)info[54];}}				// unsigned char HE_Type;			// 0=HE 1=Smoke
		public sbyte HE_Strength		{ get; set; }	// (sbyte)info[55];}}				// unsigned char HE_Strength;		// The strength of the explosion caused when it's destroyed.  0 means no explosion.
		public sbyte SmokeBlockage		{ get; set; }	// (sbyte)info[56];}}				// unsigned char Smoke_Blockage;	// ? Not sure about this
		public sbyte Fuel				{ get; set; }	// (sbyte)info[57];}}				// unsigned char Fuel;				// The number of turns the tile will burn when set aflame
		public sbyte LightSource		{ get; set; }	// (sbyte)info[58];}}				// unsigned char Light_Source;		// The amount of light this tile produces
		public SpecialType TargetType	{ get; set; }	// (SpecialType)(sbyte)info[59];}}	// unsigned char Target_Type;		// The special properties of the tile
		public byte Unknown60			{ get; set; }	// info[60];}}						// unsigned char u61;
		public byte Unknown61			{ get; set; }	// info[61];}}						// unsigned char u62;

//		unsigned char Frame[8]; // Each frame is an index into the ____.TAB file; it rotates between the frames constantly.

		public string ScanGReference { get; set; }

//		unsigned char LOFT[12]; // The 12 levels of references into GEODATA\LOFTEMPS.DAT
		public string LoftReference { get; set; }

		public string Reference0To30  { get; set; }
		public string Reference30To62 { get; set; }

		public List<byte> GetLoftList()
		{
			var list = new List<byte>();

			list.Add(Loft1);
			list.Add(Loft2);
			list.Add(Loft3);
			list.Add(Loft4);
			list.Add(Loft5);
			list.Add(Loft6);
			list.Add(Loft7);
			list.Add(Loft8);
			list.Add(Loft9);
			list.Add(Loft10);
			list.Add(Loft11);
			list.Add(Loft12);

			return list;
		}
	}
}
