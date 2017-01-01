namespace MapView.Forms.MapObservers.TileViews
{
	partial class TileView
	{
		#region Windows Form Designer generated code
		/// <summary>
		/// Required method for Designer support - do not modify
		/// the contents of this method with the code editor.
		/// </summary>
		private void InitializeComponent()
		{
			System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(TileView));
			this.mcdInfoTab = new System.Windows.Forms.MenuItem();
			this.tabs = new System.Windows.Forms.TabControl();
			this.allTab = new System.Windows.Forms.TabPage();
			this.groundTab = new System.Windows.Forms.TabPage();
			this.wWallsTab = new System.Windows.Forms.TabPage();
			this.nWallsTab = new System.Windows.Forms.TabPage();
			this.objectsTab = new System.Windows.Forms.TabPage();
			this.ViewToolStrip = new System.Windows.Forms.ToolStrip();
			this.toolStripButton1 = new System.Windows.Forms.ToolStripDropDownButton();
			this.mCDInformationToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.volutarMCDEditToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.toolStripButton2 = new System.Windows.Forms.ToolStripDropDownButton();
			this.editPCKToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.toolStripDropDownButton1 = new System.Windows.Forms.ToolStripDropDownButton();
			this.optionsToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.tabs.SuspendLayout();
			this.ViewToolStrip.SuspendLayout();
			this.SuspendLayout();
			// 
			// mcdInfoTab
			// 
			this.mcdInfoTab.Index = -1;
			this.mcdInfoTab.Text = "";
			// 
			// tabs
			// 
			this.tabs.Controls.Add(this.allTab);
			this.tabs.Controls.Add(this.groundTab);
			this.tabs.Controls.Add(this.wWallsTab);
			this.tabs.Controls.Add(this.nWallsTab);
			this.tabs.Controls.Add(this.objectsTab);
			this.tabs.Dock = System.Windows.Forms.DockStyle.Fill;
			this.tabs.Font = new System.Drawing.Font("Verdana", 7F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.tabs.Location = new System.Drawing.Point(0, 25);
			this.tabs.Name = "tabs";
			this.tabs.SelectedIndex = 0;
			this.tabs.Size = new System.Drawing.Size(554, 345);
			this.tabs.TabIndex = 0;
			// 
			// allTab
			// 
			this.allTab.Location = new System.Drawing.Point(4, 21);
			this.allTab.Name = "allTab";
			this.allTab.Size = new System.Drawing.Size(546, 320);
			this.allTab.TabIndex = 0;
			this.allTab.Text = "All";
			// 
			// groundTab
			// 
			this.groundTab.Location = new System.Drawing.Point(4, 21);
			this.groundTab.Name = "groundTab";
			this.groundTab.Size = new System.Drawing.Size(546, 320);
			this.groundTab.TabIndex = 1;
			this.groundTab.Text = "Ground";
			// 
			// wWallsTab
			// 
			this.wWallsTab.Location = new System.Drawing.Point(4, 21);
			this.wWallsTab.Name = "wWallsTab";
			this.wWallsTab.Size = new System.Drawing.Size(546, 320);
			this.wWallsTab.TabIndex = 2;
			this.wWallsTab.Text = "West Walls";
			// 
			// nWallsTab
			// 
			this.nWallsTab.Location = new System.Drawing.Point(4, 21);
			this.nWallsTab.Name = "nWallsTab";
			this.nWallsTab.Size = new System.Drawing.Size(546, 320);
			this.nWallsTab.TabIndex = 4;
			this.nWallsTab.Text = "North Walls";
			// 
			// objectsTab
			// 
			this.objectsTab.Location = new System.Drawing.Point(4, 21);
			this.objectsTab.Name = "objectsTab";
			this.objectsTab.Size = new System.Drawing.Size(546, 320);
			this.objectsTab.TabIndex = 3;
			this.objectsTab.Text = "Objects";
			// 
			// ViewToolStrip
			// 
			this.ViewToolStrip.Font = new System.Drawing.Font("Verdana", 7F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.ViewToolStrip.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
									this.toolStripButton1,
									this.toolStripButton2,
									this.toolStripDropDownButton1});
			this.ViewToolStrip.Location = new System.Drawing.Point(0, 0);
			this.ViewToolStrip.Name = "ViewToolStrip";
			this.ViewToolStrip.Size = new System.Drawing.Size(554, 25);
			this.ViewToolStrip.TabIndex = 1;
			this.ViewToolStrip.Text = "toolStrip1";
			// 
			// toolStripButton1
			// 
			this.toolStripButton1.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
			this.toolStripButton1.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
									this.mCDInformationToolStripMenuItem,
									this.volutarMCDEditToolStripMenuItem});
			this.toolStripButton1.Font = new System.Drawing.Font("Verdana", 7F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.toolStripButton1.Image = ((System.Drawing.Image)(resources.GetObject("toolStripButton1.Image")));
			this.toolStripButton1.ImageTransparentColor = System.Drawing.Color.Magenta;
			this.toolStripButton1.Name = "toolStripButton1";
			this.toolStripButton1.Size = new System.Drawing.Size(103, 22);
			this.toolStripButton1.Text = "MCD - Tile Info";
			// 
			// mCDInformationToolStripMenuItem
			// 
			this.mCDInformationToolStripMenuItem.Name = "mCDInformationToolStripMenuItem";
			this.mCDInformationToolStripMenuItem.Size = new System.Drawing.Size(165, 22);
			this.mCDInformationToolStripMenuItem.Text = "MCD Information";
			this.mCDInformationToolStripMenuItem.Click += new System.EventHandler(this.mcdInfoTab_Click);
			// 
			// volutarMCDEditToolStripMenuItem
			// 
			this.volutarMCDEditToolStripMenuItem.Name = "volutarMCDEditToolStripMenuItem";
			this.volutarMCDEditToolStripMenuItem.Size = new System.Drawing.Size(165, 22);
			this.volutarMCDEditToolStripMenuItem.Text = "Volutar MCD Edit";
			this.volutarMCDEditToolStripMenuItem.Click += new System.EventHandler(this.VolutarMcdEditMenuItem_Click);
			// 
			// toolStripButton2
			// 
			this.toolStripButton2.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
			this.toolStripButton2.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
									this.editPCKToolStripMenuItem});
			this.toolStripButton2.Font = new System.Drawing.Font("Verdana", 7F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.toolStripButton2.Image = ((System.Drawing.Image)(resources.GetObject("toolStripButton2.Image")));
			this.toolStripButton2.ImageTransparentColor = System.Drawing.Color.Magenta;
			this.toolStripButton2.Name = "toolStripButton2";
			this.toolStripButton2.Size = new System.Drawing.Size(110, 22);
			this.toolStripButton2.Text = "PCK - Tile Group";
			// 
			// editPCKToolStripMenuItem
			// 
			this.editPCKToolStripMenuItem.Name = "editPCKToolStripMenuItem";
			this.editPCKToolStripMenuItem.Size = new System.Drawing.Size(152, 22);
			this.editPCKToolStripMenuItem.Text = "Edit PCK";
			this.editPCKToolStripMenuItem.Click += new System.EventHandler(this.EditPckMenuItem_Click);
			// 
			// toolStripDropDownButton1
			// 
			this.toolStripDropDownButton1.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
			this.toolStripDropDownButton1.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
									this.optionsToolStripMenuItem});
			this.toolStripDropDownButton1.Font = new System.Drawing.Font("Verdana", 7F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.toolStripDropDownButton1.Image = ((System.Drawing.Image)(resources.GetObject("toolStripDropDownButton1.Image")));
			this.toolStripDropDownButton1.ImageTransparentColor = System.Drawing.Color.Magenta;
			this.toolStripDropDownButton1.Name = "toolStripDropDownButton1";
			this.toolStripDropDownButton1.Size = new System.Drawing.Size(38, 22);
			this.toolStripDropDownButton1.Text = "Edit";
			// 
			// optionsToolStripMenuItem
			// 
			this.optionsToolStripMenuItem.Name = "optionsToolStripMenuItem";
			this.optionsToolStripMenuItem.Size = new System.Drawing.Size(152, 22);
			this.optionsToolStripMenuItem.Text = "Options";
			this.optionsToolStripMenuItem.Click += new System.EventHandler(this.options_click);
			// 
			// TileView
			// 
			this.Controls.Add(this.tabs);
			this.Controls.Add(this.ViewToolStrip);
			this.Font = new System.Drawing.Font("Verdana", 7F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.Name = "TileView";
			this.Size = new System.Drawing.Size(554, 370);
			this.tabs.ResumeLayout(false);
			this.ViewToolStrip.ResumeLayout(false);
			this.ViewToolStrip.PerformLayout();
			this.ResumeLayout(false);
			this.PerformLayout();
		}

		protected override void Dispose(bool disposing)
		{
			if (disposing)
			{
				if (components != null)
				{
					components.Dispose();
				}
			}
			base.Dispose(disposing);
		}
		#endregion
		 
		private System.Windows.Forms.ToolStrip ViewToolStrip;
		private System.Windows.Forms.ToolStripDropDownButton toolStripButton1;
		private System.Windows.Forms.ToolStripMenuItem mCDInformationToolStripMenuItem;
		private System.Windows.Forms.ToolStripDropDownButton toolStripButton2;
		private System.Windows.Forms.ToolStripMenuItem editPCKToolStripMenuItem;
		private System.Windows.Forms.ToolStripDropDownButton toolStripDropDownButton1;
		private System.Windows.Forms.ToolStripMenuItem optionsToolStripMenuItem;
		private System.Windows.Forms.ToolStripMenuItem volutarMCDEditToolStripMenuItem;
	}
}
