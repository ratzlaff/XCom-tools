using System;
using System.Drawing;
using System.Collections;
using System.ComponentModel;
using System.Windows.Forms;
using MapView.Forms.McdViewer;
using XCom;
using XCom.Interfaces.Base;

namespace MapView
{
	public delegate void SelectedTileChanged(TilePanel sender,ITile newTile);
	public partial class TileView : Map_Observer_Form
	{
		private IContainer components;
		private MainMenu menu;
		private MenuItem menuItem1;
		private MenuItem mcdInfoTab;

		private TilePanel all;
	    private TilePanel[] panels;
        private McdViewerForm MCDInfoForm;
		private TabControl tabs;
		private TabPage allTab;
		private TabPage groundTab;
		private TabPage objectsTab;
		private TabPage nWallsTab;
		private TabPage wWallsTab;
		private Settings settings;
		private Hashtable brushes;

		private static TileView myInstance;
		private TileView()
		{
		    InitializeComponent();

            tabs.Selected += tabs_Selected;

			all = new TilePanel(TileType.All);
			var ground = new TilePanel(TileType.Ground);
			var wWalls = new TilePanel(TileType.WestWall);
			var nWalls = new TilePanel(TileType.NorthWall);
			var objects = new TilePanel(TileType.Object);

			panels = new[]{all,ground,wWalls,nWalls,objects};

			addPanel(all,allTab);
			addPanel(ground,groundTab);
			addPanel(wWalls,wWallsTab);
			addPanel(nWalls,nWallsTab);
			addPanel(objects,objectsTab);

//
//			tp = new TilePanel();
//			this.Controls.Add(tp);
//			tp.TileChanged+=new SelectedTileChanged(tileChanged);

			OnResize(null);

			MenuItem edit = new MenuItem("Edit");
			edit.MenuItems.Add("Options",new EventHandler(options_click));
			menu.MenuItems.Add(edit);
		}

        private void tabs_Selected(object sender, TabControlEventArgs e)
        {
            var tile = SelectedTile;
            if (tile != null && tile.Info is McdEntry)
            {
                var info = (McdEntry)tile.Info;
                Text = "TileView: mapID:" + tile.MapID + " mcdID: " + tile.ID;
                UpdateMcdText(info);
            }
        }

		private void options_click(object sender,EventArgs e)
		{
            PropertyForm pf = new PropertyForm("tileViewOptions",settings);
			pf.Text="Tile View Settings";
			pf.Show();
		}

		private void brushChanged(object sender,string key, object val)
		{
			((SolidBrush)brushes[key]).Color=(Color)val;
			Refresh();
		}

		protected override void LoadDefaultSettings(Settings settings)
		{
			brushes = new Hashtable();

			ValueChangedDelegate bc = new ValueChangedDelegate(brushChanged);

			foreach(string s in Enum.GetNames(typeof(SpecialType)))
			{
				brushes[s]=new SolidBrush(TilePanel.tileTypes[(int)Enum.Parse(typeof(SpecialType),s)]);
				settings.AddSetting(s,((SolidBrush)brushes[s]).Color,"Color of specified tile type","TileView",bc,false,null);
			}
			TilePanel.Colors=brushes;
		}

	    private void addPanel(TilePanel panel, TabPage page)
	    {
	        panel.Dock = DockStyle.Fill;
	        page.Controls.Add(panel);
	        panel.TileChanged += tileChanged;
	    }

	    public static TileView Instance
		{
			get
			{
				if(myInstance==null)
					myInstance = new TileView();
				return myInstance;
			}
		}

	    private void tileChanged(TilePanel sender, ITile tile)
	    {
	        if (tile != null && tile.Info is McdEntry)
	        {
	            var info = (McdEntry) tile.Info;
	            Text = "TileView: mapID:" + tile.MapID + " mcdID: " + tile.ID;
	            UpdateMcdText(info);
	        }
	    }

	    private void UpdateMcdText(McdEntry info)
	    {
            if (MCDInfoForm == null) return;
	        MCDInfoForm.UpdateData(info);
	    }

	    public override IMap_Base Map
		{
			set 
			{ 
				base.Map = value;
                if (value == null)
                {
                    Tiles = null;
                }
                else
                {
                    Tiles = value.Tiles;
                }
			}
		}
        
        public System.Collections.Generic.List<ITile> Tiles
		{
			set
			{
				for(int i=0;i<panels.Length;i++)
					panels[i].Tiles = value;
				OnResize(null);
			}
		}
        
		public ITile SelectedTile
		{
			get
			{
				return panels[tabs.SelectedIndex].SelectedTile;
			}
			set
			{
				tabs.SelectedIndex=0;
				all.SelectedTile=value;
				Refresh();
			}
		}

		private void mcdInfoTab_Click(object sender, System.EventArgs e)
		{
			if(!mcdInfoTab.Checked)
			{
			    if (MCDInfoForm == null)
			    {
                    MCDInfoForm = new McdViewerForm();
			        MCDInfoForm.Size = new Size(500, 870);
			        MCDInfoForm.Closing += infoTabClosing;
			       
                    var tile = SelectedTile;
			        if (tile != null && tile.Info is McdEntry)
			        {
			            var info = (McdEntry) tile.Info;
			            Text = "TileView: mapID:" + tile.MapID + " mcdID: " + tile.ID;
			            UpdateMcdText(info);
			        }
			    }
			    MCDInfoForm.Visible=true;
				MCDInfoForm.Location = new Point(this.Location.X-MCDInfoForm.Width,this.Location.Y);
				MCDInfoForm.Show();
				mcdInfoTab.Checked=true;
			}
		}

		private void infoTabClosing(object sender, CancelEventArgs e)
		{
			e.Cancel=true;
			MCDInfoForm.Visible=false;
			mcdInfoTab.Checked=false;
		}
	}
}
