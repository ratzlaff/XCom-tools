using System;
using System.Drawing;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Windows.Forms;
using System.Reflection;
using System.IO;
using XCom;
using XCom.Interfaces;
using XCom.Interfaces.Base;

namespace MapView
{
	public delegate void SelectedTileChanged(TilePanel sender, ITile newTile);

	public partial class TileView : Map_Observer_Form
	{
		private RichTextBox rtb;

		private TilePanel all, ground, wWalls, nWalls, objects;
		private TilePanel[] panels;
		private Form MCDInfo;


		private static TileView myInstance;
		private TileView()
		{
			InitializeComponent();

			all = new TilePanel(TileType.All);
			ground = new TilePanel(TileType.Ground);
			wWalls = new TilePanel(TileType.WestWall);
			nWalls = new TilePanel(TileType.NorthWall);
			objects = new TilePanel(TileType.Object);

			panels = new TilePanel[] { all, ground, wWalls, nWalls, objects };

			addPanel(all, allTab);
			addPanel(ground, groundTab);
			addPanel(wWalls, wWallsTab);
			addPanel(nWalls, nWallsTab);
			addPanel(objects, objectsTab);

			OnResize(null);

			MenuItem edit = new MenuItem("Edit");
			edit.MenuItems.Add("Options", new EventHandler(options_click));
			menu.MenuItems.Add(edit);
		}

		#region Settings
		private void options_click(object sender, EventArgs e)
		{
			PropertyForm pf = new PropertyForm("tileViewOptions", Settings);
			pf.Text = "Tile View Settings";
			pf.Show();
		}

		protected virtual void brushChanged(object sender, string key, object val)
		{
			brushes[key].Color = (Color)val;
			Refresh();
		}

		protected override void LoadDefaultSettings(Settings settings)
		{
			foreach (string s in Enum.GetNames(typeof(SpecialType))) {
				brushes[s] = new SolidBrush(TilePanel.tileTypes[(int)Enum.Parse(typeof(SpecialType), s)]);
				settings.AddSetting(s, ((SolidBrush)brushes[s]).Color, "Color of specified tile type", "TileView", brushChanged);
			}
			TilePanel.Colors = brushes;
		}
		#endregion

		private void addPanel(TilePanel panel, TabPage page)
		{
			panel.Dock = DockStyle.Fill;
			page.Controls.Add(panel);
			panel.TileChanged += new SelectedTileChanged(tileChanged);
		}

		public static TileView Instance
		{
			get
			{
				if (myInstance == null)
					myInstance = new TileView();
				return myInstance;
			}
		}

		private void tileChanged(TilePanel sender, ITile t)
		{
			if (t != null && t.Info is McdEntry) {
				McdEntry info = (McdEntry)t.Info;
				Text = "TileView: mapID:" + t.MapID + " mcdID: " + t.ID;
				if (MCDInfo == null)
					return;
				rtb.Text = "";
				rtb.SelectionColor = Color.Gray;
				for (int i = 0; i < 30; i++)
					rtb.AppendText(info[i] + " ");
				rtb.AppendText("\n");
				rtb.SelectionColor = Color.Gray;
				for (int i = 30; i < 62; i++)
					rtb.AppendText(info[i] + " ");
				rtb.AppendText("\n\n");
				rtb.SelectionColor = Color.Black;

				rtb.AppendText(string.Format("Images: {0} {1} {2} {3} {4} {5} {6} {7}\n", info.Image1, info.Image2, info.Image3, info.Image4, info.Image5, info.Image6, info.Image7, info.Image8));// unsigned char Frame[8];      //Each frame is an index into the ____.TAB file; it rotates between the frames constantly.
				rtb.AppendText(string.Format("loft references: {0} {1} {2} {3} {4} {5} {6} {7} {8} {9} {10} {11}\n", info[8], info[9], info[10], info[11], info[12], info[13], info[14], info[15], info[16], info[17], info[18], info[19]));// unsigned char LOFT[12];      //The 12 levels of references into GEODATA\LOFTEMPS.DAT
				rtb.AppendText(string.Format("scang reference: {0} {1} -> {2}\n", info[20], info[21], info[20] * 256 + info[21]));// short int ScanG;      //A reference into the GEODATA\SCANG.DAT
				//rtb.AppendText(string.Format("Unknown data: {0}\n",info[22]));// unsigned char u23;
				//rtb.AppendText(string.Format("Unknown data: {0}\n",info[23]));// unsigned char u24;
				//rtb.AppendText(string.Format("Unknown data: {0}\n",info[24]));// unsigned char u25;
				//rtb.AppendText(string.Format("Unknown data: {0}\n",info[25]));// unsigned char u26;
				//rtb.AppendText(string.Format("Unknown data: {0}\n",info[26]));// unsigned char u27;
				//rtb.AppendText(string.Format("Unknown data: {0}\n",info[27]));// unsigned char u28;
				//rtb.AppendText(string.Format("Unknown data: {0}\n",info[28]));// unsigned char u29;
				//rtb.AppendText(string.Format("Unknown data: {0}\n",info[29]));// unsigned char u30;
				rtb.AppendText(string.Format("UFO Door?: {0}\n", info.UFODoor));// unsigned char UFO_Door;      //If it's a UFO door it uses only Frame[0] until it is walked through, then it animates once and becomes Alt_MCD.  It changes back at the end of the turn
				rtb.AppendText(string.Format("SeeThru?: {0}\n", info.StopLOS));// unsigned char Stop_LOS;      //You cannot see through this tile.
				rtb.AppendText(string.Format("No Floor?: {0}\n", info.NoGround));// unsigned char No_Floor;      //If 1, then a non-flying unit can't stand here
				rtb.AppendText(string.Format("Big Wall?: {0}\n", info.BigWall));// unsigned char Big_Wall;      //It's an object (tile type 3), but it acts like a wall
				rtb.AppendText(string.Format("Grav Lift?: {0}\n", info.GravLift));// unsigned char Gravlift;
				rtb.AppendText(string.Format("People Door?: {0}\n", info.HumanDoor));// unsigned char Door;      //It's a human style door--you walk through it and it changes to Alt_MCD
				rtb.AppendText(string.Format("Block fire?: {0}\n", info.BlockFire));// unsigned char Block_Fire;       //If 1, fire won't go through the tile
				rtb.AppendText(string.Format("Block Smoke?: {0}\n", info.BlockSmoke));// unsigned char Block_Smoke;      //If 1, smoke won't go through the tile
				//rtb.AppendText(string.Format("Unknown data: {0}\n",info[38]));// unsigned char u39;
				rtb.AppendText(string.Format("TU Walk: {0}\n", info.TU_Walk));// unsigned char TU_Walk;       //The number of TUs require to pass the tile while walking.  An 0xFF (255) means it's unpassable.
				rtb.AppendText(string.Format("TU Fly: {0}\n", info.TU_Fly));// unsigned char TU_Fly;        // remember, 0xFF means it's impassable!
				rtb.AppendText(string.Format("TU Slide: {0}\n", info.TU_Slide));// unsigned char TU_Slide;      // sliding things include snakemen and silacoids
				rtb.AppendText(string.Format("Armor: {0}\n", info.Armor));// unsigned char Armour;        //The higher this is the less likely it is that a weapon will destroy this tile when it's hit.
				rtb.AppendText(string.Format("HE Block: {0}\n", info.HE_Block));// unsigned char HE_Block;      //How much of an explosion this tile will block
				rtb.SelectionColor = Color.Red;
				rtb.AppendText(string.Format("Death tile: {0}\n", info.DieTile));// unsigned char Die_MCD;       //If the terrain is destroyed, it is set to 0 and a tile of type Die_MCD is added
				rtb.AppendText(string.Format("Flammable: {0}\n", info.Flammable));// unsigned char Flammable;     //How flammable it is (the higher the harder it is to set aflame)
				rtb.SelectionColor = Color.Red;
				rtb.AppendText(string.Format("Door open tile: {0}\n", info.Alt_MCD));// unsigned char Alt_MCD;       //If "Door" or "UFO_Door" is on, then when a unit walks through it the door is set to 0 and a tile type Alt_MCD is added.
				//rtb.AppendText(string.Format("Unknown data: {0}\n",info[47]));// unsigned char u48;
				rtb.AppendText(string.Format("Unit y offset: {0}\n", info.StandOffset));// signed char T_Level;      //When a unit or object is standing on the tile, the unit is shifted by this amount
				rtb.AppendText(string.Format("tile y offset: {0}\n", info.TileOffset));// unsigned char P_Level;      //When the tile is drawn, this amount is subtracted from its y (so y position-P_Level is where it's drawn)
				//rtb.AppendText(string.Format("Unknown data: {0}\n",info[50]));// unsigned char u51;
				rtb.AppendText(string.Format("block light[0-10]: {0}\n", info.LightBlock));// unsigned char Light_Block;     //The amount of light it blocks, from 0 to 10
				rtb.AppendText(string.Format("footstep sound effect: {0}\n", info.Footstep));// unsigned char Footstep;         //The Sound Effect set to choose from when footsteps are on the tile
				rtb.AppendText(string.Format("tile type: {0}:{1}\n", (sbyte)info.TileType, info.TileType + ""));//info.TileType==0?"floor":info.TileType==1?"west wall":info.TileType==2?"north wall":info.TileType==3?"object":"Unknown"));// unsigned char Tile_Type;       //This is the type of tile it is meant to be -- 0=floor, 1=west wall, 2=north wall, 3=object .  When this type of tile is in the Die_As or Open_As flags, this value is added to the tile coordinate to determine the byte in which the tile type should be written.
				rtb.AppendText(string.Format("high explosive type: {0}:{1}\n", info.HE_Type, info.HE_Type == 0 ? "HE" : info.HE_Type == 1 ? "smoke" : "unknown"));// unsigned char HE_Type;         //0=HE  1=Smoke
				rtb.AppendText(string.Format("HE Strength: {0}\n", info.HE_Strength));// unsigned char HE_Strength;     //The strength of the explosion caused when it's destroyed.  0 means no explosion.
				rtb.AppendText(string.Format("smoke blockage: {0}\n", info.SmokeBlockage));// unsigned char Smoke_Blockage;      //? Not sure about this ...
				rtb.AppendText(string.Format("# turns to burn: {0}\n", info.Fuel));// unsigned char Fuel;      //The number of turns the tile will burn when set aflame
				rtb.AppendText(string.Format("amount of light produced: {0}\n", info.LightSource));// unsigned char Light_Source;      //The amount of light this tile produces
				rtb.AppendText(string.Format("special properties: {0}\n", info.TargetType));// unsigned char Target_Type;       //The special properties of the tile
				//rtb.AppendText(string.Format("Unknown data: {0}\n",info[60]));// unsigned char u61;
				//rtb.AppendText(string.Format("Unknown data: {0}\n",info[61]));// unsigned char u62;
			}
		}

		protected override void mapChanged(object sender, IMap_Base map)
		{
			base.map = map;
			Tiles = map.Tiles;
		}

		[Browsable(false)]
		public System.Collections.Generic.List<ITile> Tiles
		{
			set
			{
				for (int i = 0; i < panels.Length; i++)
					panels[i].Tiles = value;
				OnResize(null);
			}
		}

		[Browsable(false)]
		public ITile SelectedTile
		{
			get
			{
				return panels[tabs.SelectedIndex].SelectedTile;
			}
			set
			{
				tabs.SelectedIndex = 0;
				all.SelectedTile = value;
				Refresh();
			}
		}

		private void mcdInfoTab_Click(object sender, System.EventArgs e)
		{
			if (!mcdInfoTab.Checked) {
				if (MCDInfo == null) {
					MCDInfo = new Form();
					MCDInfo.Text = "MCD Info";
					rtb = new RichTextBox();
					rtb.WordWrap = false;
					rtb.ReadOnly = true;
					MCDInfo.Controls.Add(rtb);
					rtb.Dock = DockStyle.Fill;
					MCDInfo.Size = new Size(200, 470);
					MCDInfo.Closing += new CancelEventHandler(infoTabClosing);
				}
				MCDInfo.Visible = true;
				MCDInfo.Location = new Point(this.Location.X - MCDInfo.Width, this.Location.Y);
				MCDInfo.Show();
				mcdInfoTab.Checked = true;
			}
		}

		private void infoTabClosing(object sender, CancelEventArgs e)
		{
			e.Cancel = true;
			MCDInfo.Visible = false;
			mcdInfoTab.Checked = false;
		}
	}
}
