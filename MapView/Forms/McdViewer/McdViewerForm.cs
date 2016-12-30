using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using XCom;

namespace MapView.Forms.McdViewer
{
	public partial class McdViewerForm
		:
		Form
	{
		public McdViewerForm()
		{
			InitializeComponent();

			rtb.WordWrap = false;
			rtb.ReadOnly = true;
		}

		public void UpdateData(McdEntry info)
		{
			InfoBs.DataSource = info;

			rtb.Text = "";
			rtb.SelectionColor = Color.DarkGray;
			rtb.AppendText(info.Reference0To30);
			rtb.AppendText("\n");
			rtb.SelectionColor = Color.DarkGray;
			rtb.AppendText(info.Reference30To62);
			rtb.AppendText("\n\n");
			rtb.SelectionColor = Color.Black;

			rtb.AppendText(string.Format(
									"Images: {0} {1} {2} {3} {4} {5} {6} {7}\n",
									info.Image1,
									info.Image2,
									info.Image3,
									info.Image4,
									info.Image5,
									info.Image6,
									info.Image7,
									info.Image8));
			rtb.AppendText(info.LoftReference);
			rtb.AppendText(info.ScanGReference);
//			short int ScanG; // A reference into the GEODATA\SCANG.DAT
			//rtb.AppendText(string.Format("Unknown data: {0}\n",info[22])); // unsigned char u23;
			//rtb.AppendText(string.Format("Unknown data: {0}\n",info[23])); // unsigned char u24;
			//rtb.AppendText(string.Format("Unknown data: {0}\n",info[24])); // unsigned char u25;
			//rtb.AppendText(string.Format("Unknown data: {0}\n",info[25])); // unsigned char u26;
			//rtb.AppendText(string.Format("Unknown data: {0}\n",info[26])); // unsigned char u27;
			//rtb.AppendText(string.Format("Unknown data: {0}\n",info[27])); // unsigned char u28;
			//rtb.AppendText(string.Format("Unknown data: {0}\n",info[28])); // unsigned char u29;
			//rtb.AppendText(string.Format("Unknown data: {0}\n",info[29])); // unsigned char u30;
			rtb.AppendText(string.Format("UFO Door: {0}\n", info.UFODoor));
//			unsigned char UFO_Door;	// If it's a UFO door it uses only Frame[0] until it is walked through, then
									// it animates once and becomes Alt_MCD. It changes back at the end of the turn
			rtb.AppendText(string.Format("SeeThru: {0}\n", info.StopLOS));
//			unsigned char Stop_LOS; // You cannot see through this tile.
			rtb.AppendText(string.Format("No Floor: {0}\n", info.NoGround));
//			unsigned char No_Floor; // If 1, then a non-flying unit can't stand here
			rtb.AppendText(string.Format("Big Wall: {0}\n", info.BigWall));
//			unsigned char Big_Wall; // It's an object (tile type 3), but it acts like a wall
			rtb.AppendText(string.Format("Grav Lift: {0}\n", info.GravLift));
//			unsigned char Gravlift;
			rtb.AppendText(string.Format("People Door: {0}\n", info.HumanDoor));
//			unsigned char Door; // It's a human style door--you walk through it and it changes to Alt_MCD
			rtb.AppendText(string.Format("Block fire: {0}\n", info.BlockFire));
//			unsigned char Block_Fire; // If 1, fire won't go through the tile
			rtb.AppendText(string.Format("Block Smoke: {0}\n", info.BlockSmoke));
//			unsigned char Block_Smoke; // If 1, smoke won't go through the tile
			//rtb.AppendText(string.Format("Unknown data: {0}\n",info[38]));
//			unsigned char u39;
			rtb.AppendText(string.Format("TU Walk: {0}\n", info.TU_Walk));
//			unsigned char TU_Walk; // The number of TUs required to pass the tile while walking. 0xFF (255) means it's unpassable.
			rtb.AppendText(string.Format("TU Fly: {0}\n", info.TU_Fly));
//			unsigned char TU_Fly; // remember, 0xFF means it's impassable!
			rtb.AppendText(string.Format("TU Slide: {0}\n", info.TU_Slide));
//			unsigned char TU_Slide; // sliding things include snakemen and silacoids
			rtb.AppendText(string.Format("Armor: {0}\n", info.Armor));
//			unsigned char Armour; // The higher this is the less likely it is that a weapon will destroy this tile when it's hit.
			rtb.AppendText(string.Format("HE Block: {0}\n", info.HE_Block));
//			unsigned char HE_Block; // How much of an explosion this tile will block
			rtb.SelectionColor = Color.Red;
			rtb.AppendText(string.Format("Death tile: {0}\n", info.DieTile));
//			unsigned char Die_MCD; // If the terrain is destroyed, it is set to 0 and a tile of type Die_MCD is added
			rtb.AppendText(string.Format("Flammable: {0}\n", info.Flammable));
//			unsigned char Flammable; // How flammable it is (the higher the harder it is to set aflame)
			rtb.SelectionColor = Color.Red;
			rtb.AppendText(string.Format("Door open tile: {0}\n", info.Alt_MCD));
//			unsigned char Alt_MCD; // If "Door" or "UFO_Door" is on, then when a unit walks through it the door is set to 0 and a tile type Alt_MCD is added.
			//rtb.AppendText(string.Format("Unknown data: {0}\n",info[47]));// unsigned char u48;
			rtb.AppendText(string.Format("Unit y offset: {0}\n", info.StandOffset));
//			signed char T_Level; // When a unit or object is standing on the tile, the unit is shifted by this amount
			rtb.AppendText(string.Format("tile y offset: {0}\n", info.TileOffset));
//			unsigned char P_Level; // When the tile is drawn, this amount is subtracted from its y (so y position-P_Level is where it's drawn)
			//rtb.AppendText(string.Format("Unknown data: {0}\n",info[50]));// unsigned char u51;
			rtb.AppendText(string.Format("block light[0-10]: {0}\n", info.LightBlock));
//			unsigned char Light_Block; // The amount of light it blocks, from 0 to 10
			rtb.AppendText(string.Format("footstep sound effect: {0}\n", info.Footstep));
//			unsigned char Footstep; // The Sound Effect set to choose from when footsteps are on the tile
			rtb.AppendText(string.Format("tile type: {0}:{1}\n", (sbyte)info.TileType, info.TileType + ""));
//			info.TileType==0?"floor":info.TileType==1?"west wall":info.TileType==2?"north wall":info.TileType==3?"object":"Unknown"));
//			unsigned char Tile_Type;	// This is the type of tile it is meant to be -- 0=floor, 1=west wall, 2=north wall, 3=object.
										// When this type of tile is in the Die_As or Open_As flags, this value is added to the tile
										// coordinate to determine the byte in which the tile type should be written.
			rtb.AppendText(string.Format(
									"high explosive type: {0}:{1}\n",
									info.HE_Type,
									info.HE_Type == 0 ? "HE" : info.HE_Type == 1 ? "smoke" : "unknown"));
//			unsigned char HE_Type; // 0=HE 1=Smoke
			rtb.AppendText(string.Format("HE Strength: {0}\n", info.HE_Strength));
//			unsigned char HE_Strength; // The strength of the explosion caused when it's destroyed. 0 means no explosion.
			rtb.AppendText(string.Format("smoke blockage: {0}\n", info.SmokeBlockage));
//			unsigned char Smoke_Blockage; // ? Not sure about this
			rtb.AppendText(string.Format("turns to burn: {0}\n", info.Fuel));
//			unsigned char Fuel; // The number of turns the tile will burn when set aflame
			rtb.AppendText(string.Format("light produced: {0}\n", info.LightSource));
//			unsigned char Light_Source; // The amount of light this tile produces
			rtb.AppendText(string.Format("special: {0}\n", info.TargetType));
//			unsigned char Target_Type; // The special properties of the tile
			//rtb.AppendText(string.Format("Unknown data: {0}\n",info[60]));
//			unsigned char u61;
			//rtb.AppendText(string.Format("Unknown data: {0}\n",info[61]));
//			unsigned char u62;
		}
	}
}
