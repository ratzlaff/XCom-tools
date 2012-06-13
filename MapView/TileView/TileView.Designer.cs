namespace MapView
{
	partial class TileView
	{
		/// <summary>
		/// Required designer variable.
		/// </summary>
		private System.ComponentModel.IContainer components = null;

		/// <summary>
		/// Clean up any resources being used.
		/// </summary>
		/// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
		protected override void Dispose(bool disposing)
		{
			if (disposing && (components != null)) {
				components.Dispose();
			}
			base.Dispose(disposing);
		}

		#region Windows Form Designer generated code
		/// <summary>
		/// Required method for Designer support - do not modify
		/// the contents of this method with the code editor.
		/// </summary>
		private void InitializeComponent()
		{
			this.components = new System.ComponentModel.Container();
			this.menu = new System.Windows.Forms.MainMenu(this.components);
			this.menuItem2 = new System.Windows.Forms.MenuItem();
			this.menuItem3 = new System.Windows.Forms.MenuItem();
			this.tabs = new System.Windows.Forms.TabControl();
			this.allTab = new System.Windows.Forms.TabPage();
			this.groundTab = new System.Windows.Forms.TabPage();
			this.wWallsTab = new System.Windows.Forms.TabPage();
			this.nWallsTab = new System.Windows.Forms.TabPage();
			this.objectsTab = new System.Windows.Forms.TabPage();
			this.all = new MapView.TilePanel();
			this.ground = new MapView.TilePanel();
			this.wWalls = new MapView.TilePanel();
			this.nWalls = new MapView.TilePanel();
			this.objs = new MapView.TilePanel();
			this.tabs.SuspendLayout();
			this.allTab.SuspendLayout();
			this.groundTab.SuspendLayout();
			this.wWallsTab.SuspendLayout();
			this.nWallsTab.SuspendLayout();
			this.objectsTab.SuspendLayout();
			this.SuspendLayout();
			// 
			// menu
			// 
			this.menu.MenuItems.AddRange(new System.Windows.Forms.MenuItem[] {
            this.menuItem2});
			// 
			// menuItem2
			// 
			this.menuItem2.Index = 0;
			this.menuItem2.MenuItems.AddRange(new System.Windows.Forms.MenuItem[] {
            this.menuItem3});
			this.menuItem2.Text = "Edit";
			// 
			// menuItem3
			// 
			this.menuItem3.Index = 0;
			this.menuItem3.Text = "Options";
			this.menuItem3.Click += new System.EventHandler(this.options_click);
			// 
			// tabs
			// 
			this.tabs.Controls.Add(this.allTab);
			this.tabs.Controls.Add(this.groundTab);
			this.tabs.Controls.Add(this.wWallsTab);
			this.tabs.Controls.Add(this.nWallsTab);
			this.tabs.Controls.Add(this.objectsTab);
			this.tabs.Dock = System.Windows.Forms.DockStyle.Fill;
			this.tabs.Location = new System.Drawing.Point(0, 0);
			this.tabs.Name = "tabs";
			this.tabs.SelectedIndex = 0;
			this.tabs.Size = new System.Drawing.Size(542, 289);
			this.tabs.TabIndex = 0;
			// 
			// allTab
			// 
			this.allTab.Controls.Add(this.all);
			this.allTab.Location = new System.Drawing.Point(4, 22);
			this.allTab.Name = "allTab";
			this.allTab.Size = new System.Drawing.Size(534, 263);
			this.allTab.TabIndex = 0;
			this.allTab.Text = "All";
			// 
			// groundTab
			// 
			this.groundTab.Controls.Add(this.ground);
			this.groundTab.Location = new System.Drawing.Point(4, 22);
			this.groundTab.Name = "groundTab";
			this.groundTab.Size = new System.Drawing.Size(534, 284);
			this.groundTab.TabIndex = 1;
			this.groundTab.Text = "Ground";
			// 
			// wWallsTab
			// 
			this.wWallsTab.Controls.Add(this.wWalls);
			this.wWallsTab.Location = new System.Drawing.Point(4, 22);
			this.wWallsTab.Name = "wWallsTab";
			this.wWallsTab.Size = new System.Drawing.Size(534, 284);
			this.wWallsTab.TabIndex = 2;
			this.wWallsTab.Text = "West Walls";
			// 
			// nWallsTab
			// 
			this.nWallsTab.Controls.Add(this.nWalls);
			this.nWallsTab.Location = new System.Drawing.Point(4, 22);
			this.nWallsTab.Name = "nWallsTab";
			this.nWallsTab.Size = new System.Drawing.Size(534, 284);
			this.nWallsTab.TabIndex = 4;
			this.nWallsTab.Text = "North Walls";
			// 
			// objectsTab
			// 
			this.objectsTab.Controls.Add(this.objs);
			this.objectsTab.Location = new System.Drawing.Point(4, 22);
			this.objectsTab.Name = "objectsTab";
			this.objectsTab.Size = new System.Drawing.Size(534, 284);
			this.objectsTab.TabIndex = 3;
			this.objectsTab.Text = "Objects";
			// 
			// all
			// 
			this.all.Dock = System.Windows.Forms.DockStyle.Fill;
			this.all.Location = new System.Drawing.Point(0, 0);
			this.all.Name = "all";
			this.all.SelectedTile = null;
			this.all.Size = new System.Drawing.Size(534, 263);
			this.all.StartY = 0;
			this.all.TabIndex = 0;
			this.all.TileChanged += new MapView.SelectedTileChanged(this.tileChanged);
			// 
			// ground
			// 
			this.ground.Dock = System.Windows.Forms.DockStyle.Fill;
			this.ground.Location = new System.Drawing.Point(0, 0);
			this.ground.Name = "ground";
			this.ground.SelectedTile = null;
			this.ground.Size = new System.Drawing.Size(534, 284);
			this.ground.StartY = 0;
			this.ground.TabIndex = 1;
			this.ground.TileCategory = "Ground";
			this.ground.TileChanged += new MapView.SelectedTileChanged(this.tileChanged);
			// 
			// wWalls
			// 
			this.wWalls.Dock = System.Windows.Forms.DockStyle.Fill;
			this.wWalls.Location = new System.Drawing.Point(0, 0);
			this.wWalls.Name = "wWalls";
			this.wWalls.SelectedTile = null;
			this.wWalls.Size = new System.Drawing.Size(534, 284);
			this.wWalls.StartY = 0;
			this.wWalls.TabIndex = 1;
			this.wWalls.TileCategory = "WestWall";
			this.wWalls.TileChanged += new MapView.SelectedTileChanged(this.tileChanged);
			// 
			// nWalls
			// 
			this.nWalls.Dock = System.Windows.Forms.DockStyle.Fill;
			this.nWalls.Location = new System.Drawing.Point(0, 0);
			this.nWalls.Name = "nWalls";
			this.nWalls.SelectedTile = null;
			this.nWalls.Size = new System.Drawing.Size(534, 284);
			this.nWalls.StartY = 0;
			this.nWalls.TabIndex = 1;
			this.nWalls.TileCategory = "NorthWall";
			this.nWalls.TileChanged += new MapView.SelectedTileChanged(this.tileChanged);
			// 
			// objs
			// 
			this.objs.Dock = System.Windows.Forms.DockStyle.Fill;
			this.objs.Location = new System.Drawing.Point(0, 0);
			this.objs.Name = "objs";
			this.objs.SelectedTile = null;
			this.objs.Size = new System.Drawing.Size(534, 284);
			this.objs.StartY = 0;
			this.objs.TabIndex = 1;
			this.objs.TileCategory = "Object";
			this.objs.TileChanged += new MapView.SelectedTileChanged(this.tileChanged);
			// 
			// TileView
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.ClientSize = new System.Drawing.Size(542, 289);
			this.Controls.Add(this.tabs);
			this.Menu = this.menu;
			this.Name = "TileView";
			this.Text = "TileView";
			this.tabs.ResumeLayout(false);
			this.allTab.ResumeLayout(false);
			this.groundTab.ResumeLayout(false);
			this.wWallsTab.ResumeLayout(false);
			this.nWallsTab.ResumeLayout(false);
			this.objectsTab.ResumeLayout(false);
			this.ResumeLayout(false);

		}
		#endregion

		private System.Windows.Forms.TabControl tabs;
		private System.Windows.Forms.TabPage allTab;
		private System.Windows.Forms.TabPage groundTab;
		private System.Windows.Forms.TabPage objectsTab;
		private System.Windows.Forms.TabPage nWallsTab;
		private System.Windows.Forms.TabPage wWallsTab;
		private System.Windows.Forms.MainMenu menu;
		private TilePanel all;
		private TilePanel ground;
		private TilePanel wWalls;
		private TilePanel nWalls;
		private TilePanel objs;
		private System.Windows.Forms.MenuItem menuItem2;
		private System.Windows.Forms.MenuItem menuItem3;
	}
}