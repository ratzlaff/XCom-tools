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
using ViewLib;
using UtilLib;
using MapLib;
using MapLib.Base;

namespace MapView
{
	public delegate void SelectedTileChanged(object sender, Tile newTile);

	public partial class TileView : Map_Observer_Form
	{
		private TilePanel[] panels;
		private static TileView myInstance;

		public static event SelectedTileChanged TileChanged;

		private TileView()
		{
			InitializeComponent();
			panels = new TilePanel[] { all, ground, wWalls, nWalls, objs };
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
			FillBrushes[key].Color = (Color)val;
			Refresh();
		}

		public override void SetupDefaultSettings()
		{
			base.SetupDefaultSettings();
			foreach (string s in Enum.GetNames(typeof(SpecialType))) {
				FillBrushes[s] = new SolidBrush(TilePanel.tileTypes[(int)Enum.Parse(typeof(SpecialType), s)]);
				settings.AddSetting(s, FillBrushes[s].Color, "Color of specified tile type", "TileView", brushChanged);
			}
			TilePanel.FillBrushes = FillBrushes;
		}
		#endregion

		public static TileView Instance
		{
			get
			{
				if (myInstance == null) {
					myInstance = new TileView();
				}
				return myInstance;
			}
		}

		private void tileChanged(object sender, Tile t)
		{
			if (t != null) {
				Text = "TileView: mapID:" + t.MapID + " mcdID: " + t.ID;

				if (TileChanged != null)
					TileChanged(this, t);
			}
		}

		protected override void mapChanged(MapChangedEventArgs e)
		{
			base.mapChanged(e);
			Tiles = e.Map.Tiles;
		}

		[Browsable(false)]
		public System.Collections.Generic.List<Tile> Tiles
		{
			set
			{
				foreach (TilePanel tp in panels)
					tp.Tiles = value;
				OnResize(null);
			}
		}

		[Browsable(false)]
		public Tile SelectedTile
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
	}
}
