using System;
using System.Drawing;
using XCom.Interfaces;

namespace XCom
{
	public class XCTile : MapLib.Base.Tile
	{
		private McdFile mcdFile;
		private PckFile myFile;
		private XCTile[] tiles;
		private McdEntry entry;
		private const int numImages = 8;

		public XCTile(int id, PckFile file, McdEntry info, McdFile mFile)
			: base(id/*, info*/)
		{
			this.entry = info;
			myFile = file;
			mcdFile = mFile;

			images = new XCImage[numImages];

			if (!info.UFODoor && !info.HumanDoor)
				MakeAnimate();
			else
				StopAnimate();

			Dead = null;
			Alternate = null;
		}

		public override string Category
		{
			get { return entry.TileType.ToString(); }
		}

		public override string SpecialType
		{
			get { return entry.SpecialType.ToString(); }
		}

		public override bool IsDoor
		{
			get { return entry.HumanDoor || entry.UFODoor; }
		}

		public override int YOffset
		{
			get { return entry.TileOffset; }
		}

		public override void Animate(bool isOn)
		{
			if (isOn)
				MakeAnimate();
			else
				StopAnimate();
		}

		public override void FillInfo(System.Windows.Forms.RichTextBox rtb)
		{
			if (entry == null)
				return;

			rtb.Text = "";
			rtb.SelectionColor = Color.Gray;
			for (int i = 0; i < 30; i++)
				rtb.AppendText(entry[i] + " ");
			rtb.AppendText("\n");
			rtb.SelectionColor = Color.Gray;
			for (int i = 30; i < 62; i++)
				rtb.AppendText(entry[i] + " ");
			rtb.AppendText("\n\n");
			rtb.SelectionColor = Color.Black;

			rtb.AppendText(string.Format("Images: {0} {1} {2} {3} {4} {5} {6} {7}\n", entry.Image1, entry.Image2, entry.Image3, entry.Image4, entry.Image5, entry.Image6, entry.Image7, entry.Image8));// unsigned char Frame[8];      //Each frame is an index into the ____.TAB file; it rotates between the frames constantly.
			rtb.AppendText(string.Format("loft references: {0} {1} {2} {3} {4} {5} {6} {7} {8} {9} {10} {11}\n", entry[8], entry[9], entry[10], entry[11], entry[12], entry[13], entry[14], entry[15], entry[16], entry[17], entry[18], entry[19]));// unsigned char LOFT[12];      //The 12 levels of references into GEODATA\LOFTEMPS.DAT
			rtb.AppendText(string.Format("scang reference: {0} {1} -> {2}\n", entry[20], entry[21], entry[20] * 256 + entry[21]));// short int ScanG;      //A reference into the GEODATA\SCANG.DAT
			//rtb.AppendText(string.Format("Unknown data: {0}\n",entry[22]));// unsigned char u23;
			//rtb.AppendText(string.Format("Unknown data: {0}\n",entry[23]));// unsigned char u24;
			//rtb.AppendText(string.Format("Unknown data: {0}\n",entry[24]));// unsigned char u25;
			//rtb.AppendText(string.Format("Unknown data: {0}\n",entry[25]));// unsigned char u26;
			//rtb.AppendText(string.Format("Unknown data: {0}\n",entry[26]));// unsigned char u27;
			//rtb.AppendText(string.Format("Unknown data: {0}\n",entry[27]));// unsigned char u28;
			//rtb.AppendText(string.Format("Unknown data: {0}\n",entry[28]));// unsigned char u29;
			//rtb.AppendText(string.Format("Unknown data: {0}\n",entry[29]));// unsigned char u30;
			rtb.AppendText(string.Format("UFO Door?: {0}\n", entry.UFODoor));// unsigned char UFO_Door;      //If it's a UFO door it uses only Frame[0] until it is walked through, then it animates once and becomes Alt_MCD.  It changes back at the end of the turn
			rtb.AppendText(string.Format("SeeThru?: {0}\n", entry.StopLOS));// unsigned char Stop_LOS;      //You cannot see through this tile.
			rtb.AppendText(string.Format("No Floor?: {0}\n", entry.NoGround));// unsigned char No_Floor;      //If 1, then a non-flying unit can't stand here
			rtb.AppendText(string.Format("Big Wall?: {0}\n", entry.BigWall));// unsigned char Big_Wall;      //It's an object (tile type 3), but it acts like a wall
			rtb.AppendText(string.Format("Grav Lift?: {0}\n", entry.GravLift));// unsigned char Gravlift;
			rtb.AppendText(string.Format("People Door?: {0}\n", entry.HumanDoor));// unsigned char Door;      //It's a human style door--you walk through it and it changes to Alt_MCD
			rtb.AppendText(string.Format("Block fire?: {0}\n", entry.BlockFire));// unsigned char Block_Fire;       //If 1, fire won't go through the tile
			rtb.AppendText(string.Format("Block Smoke?: {0}\n", entry.BlockSmoke));// unsigned char Block_Smoke;      //If 1, smoke won't go through the tile
			//rtb.AppendText(string.Format("Unknown data: {0}\n",entry[38]));// unsigned char u39;
			rtb.AppendText(string.Format("TU Walk: {0}\n", entry.TU_Walk));// unsigned char TU_Walk;       //The number of TUs require to pass the tile while walking.  An 0xFF (255) means it's unpassable.
			rtb.AppendText(string.Format("TU Fly: {0}\n", entry.TU_Fly));// unsigned char TU_Fly;        // remember, 0xFF means it's impassable!
			rtb.AppendText(string.Format("TU Slide: {0}\n", entry.TU_Slide));// unsigned char TU_Slide;      // sliding things include snakemen and silacoids
			rtb.AppendText(string.Format("Armor: {0}\n", entry.Armor));// unsigned char Armour;        //The higher this is the less likely it is that a weapon will destroy this tile when it's hit.
			rtb.AppendText(string.Format("HE Block: {0}\n", entry.HE_Block));// unsigned char HE_Block;      //How much of an explosion this tile will block
			rtb.SelectionColor = Color.Red;
			rtb.AppendText(string.Format("Death tile: {0}\n", entry.DieTile));// unsigned char Die_MCD;       //If the terrain is destroyed, it is set to 0 and a tile of type Die_MCD is added
			rtb.AppendText(string.Format("Flammable: {0}\n", entry.Flammable));// unsigned char Flammable;     //How flammable it is (the higher the harder it is to set aflame)
			rtb.SelectionColor = Color.Red;
			rtb.AppendText(string.Format("Door open tile: {0}\n", entry.Alt_MCD));// unsigned char Alt_MCD;       //If "Door" or "UFO_Door" is on, then when a unit walks through it the door is set to 0 and a tile type Alt_MCD is added.
			//rtb.AppendText(string.Format("Unknown data: {0}\n",entry[47]));// unsigned char u48;
			rtb.AppendText(string.Format("Unit y offset: {0}\n", entry.StandOffset));// signed char T_Level;      //When a unit or object is standing on the tile, the unit is shifted by this amount
			rtb.AppendText(string.Format("tile y offset: {0}\n", entry.TileOffset));// unsigned char P_Level;      //When the tile is drawn, this amount is subtracted from its y (so y position-P_Level is where it's drawn)
			//rtb.AppendText(string.Format("Unknown data: {0}\n",entry[50]));// unsigned char u51;
			rtb.AppendText(string.Format("block light[0-10]: {0}\n", entry.LightBlock));// unsigned char Light_Block;     //The amount of light it blocks, from 0 to 10
			rtb.AppendText(string.Format("footstep sound effect: {0}\n", entry.Footstep));// unsigned char Footstep;         //The Sound Effect set to choose from when footsteps are on the tile
			rtb.AppendText(string.Format("tile type: {0}:{1}\n", (sbyte)entry.TileType, entry.TileType + ""));//entry.TileType==0?"floor":entry.TileType==1?"west wall":entry.TileType==2?"north wall":entry.TileType==3?"object":"Unknown"));// unsigned char Tile_Type;       //This is the type of tile it is meant to be -- 0=floor, 1=west wall, 2=north wall, 3=object .  When this type of tile is in the Die_As or Open_As flags, this value is added to the tile coordinate to determine the byte in which the tile type should be written.
			rtb.AppendText(string.Format("high explosive type: {0}:{1}\n", entry.HE_Type, entry.HE_Type == 0 ? "HE" : entry.HE_Type == 1 ? "smoke" : "unknown"));// unsigned char HE_Type;         //0=HE  1=Smoke
			rtb.AppendText(string.Format("HE Strength: {0}\n", entry.HE_Strength));// unsigned char HE_Strength;     //The strength of the explosion caused when it's destroyed.  0 means no explosion.
			rtb.AppendText(string.Format("smoke blockage: {0}\n", entry.SmokeBlockage));// unsigned char Smoke_Blockage;      //? Not sure about this ...
			rtb.AppendText(string.Format("# turns to burn: {0}\n", entry.Fuel));// unsigned char Fuel;      //The number of turns the tile will burn when set aflame
			rtb.AppendText(string.Format("amount of light produced: {0}\n", entry.LightSource));// unsigned char Light_Source;      //The amount of light this tile produces
			rtb.AppendText(string.Format("special properties: {0}\n", entry.SpecialType));// unsigned char Target_Type;       //The special properties of the tile
			//rtb.AppendText(string.Format("Unknown data: {0}\n",entry[60]));// unsigned char u61;
			//rtb.AppendText(string.Format("Unknown data: {0}\n",entry[61]));// unsigned char u62;
		}

		public void MakeAnimate()
		{
			images[0] = myFile[entry.Image1];
			images[1] = myFile[entry.Image2];
			images[2] = myFile[entry.Image3];
			images[3] = myFile[entry.Image4];
			images[4] = myFile[entry.Image5];
			images[5] = myFile[entry.Image6];
			images[6] = myFile[entry.Image7];
			images[7] = myFile[entry.Image8];
		}

		public void StopAnimate()
		{
			images[0] = myFile[entry.Image1];
			images[1] = myFile[entry.Image1];
			images[2] = myFile[entry.Image1];
			images[3] = myFile[entry.Image1];
			images[4] = myFile[entry.Image1];
			images[5] = myFile[entry.Image1];
			images[6] = myFile[entry.Image1];
			images[7] = myFile[entry.Image1];
		}

		public XCTile[] Tiles
		{
			get { return tiles; }
			set
			{
				tiles = value;
				try {
					if (entry.DieTile != 0)
						Dead = tiles[entry.DieTile];
				} catch {
					if (MapID == 102 || MapID == 0)
						Dead = tiles[7];
					else
						Console.WriteLine("Error, could not set dead tile: {0} mapID:{1}", entry.DieTile, MapID);
				}

				if (entry.UFODoor || entry.HumanDoor || entry.Alt_MCD != 0)
					Alternate = tiles[entry.Alt_MCD];
			}
		}

		public XCTile Dead { get; set; }

		public XCTile Alternate { get; set; }
	}
}
