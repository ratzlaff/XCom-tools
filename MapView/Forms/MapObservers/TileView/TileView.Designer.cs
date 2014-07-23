using System;
using System.Collections.Generic;
using System.Text;

namespace MapView
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
            this.components = new System.ComponentModel.Container();
            this.menu = new System.Windows.Forms.MainMenu(this.components);
            this.menuItem1 = new System.Windows.Forms.MenuItem();
            this.mcdInfoTab = new System.Windows.Forms.MenuItem();
            this.tabs = new System.Windows.Forms.TabControl();
            this.allTab = new System.Windows.Forms.TabPage();
            this.groundTab = new System.Windows.Forms.TabPage();
            this.wWallsTab = new System.Windows.Forms.TabPage();
            this.nWallsTab = new System.Windows.Forms.TabPage();
            this.objectsTab = new System.Windows.Forms.TabPage();
            this.menuItem2 = new System.Windows.Forms.MenuItem();
            this.EditPckMenuItem = new System.Windows.Forms.MenuItem();
            this.tabs.SuspendLayout();
            this.SuspendLayout();
            // 
            // menu
            // 
            this.menu.MenuItems.AddRange(new System.Windows.Forms.MenuItem[] {
            this.menuItem1,
            this.menuItem2});
            // 
            // menuItem1
            // 
            this.menuItem1.Index = 0;
            this.menuItem1.MenuItems.AddRange(new System.Windows.Forms.MenuItem[] {
            this.mcdInfoTab});
            this.menuItem1.Text = "MCD";
            // 
            // mcdInfoTab
            // 
            this.mcdInfoTab.Index = 0;
            this.mcdInfoTab.Text = "MCD Info";
            this.mcdInfoTab.Click += new System.EventHandler(this.mcdInfoTab_Click);
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
            this.tabs.Size = new System.Drawing.Size(320, 273);
            this.tabs.TabIndex = 0;
            // 
            // allTab
            // 
            this.allTab.Location = new System.Drawing.Point(4, 22);
            this.allTab.Name = "allTab";
            this.allTab.Size = new System.Drawing.Size(312, 247);
            this.allTab.TabIndex = 0;
            this.allTab.Text = "All";
            // 
            // groundTab
            // 
            this.groundTab.Location = new System.Drawing.Point(4, 22);
            this.groundTab.Name = "groundTab";
            this.groundTab.Size = new System.Drawing.Size(312, 247);
            this.groundTab.TabIndex = 1;
            this.groundTab.Text = "Ground";
            // 
            // wWallsTab
            // 
            this.wWallsTab.Location = new System.Drawing.Point(4, 22);
            this.wWallsTab.Name = "wWallsTab";
            this.wWallsTab.Size = new System.Drawing.Size(312, 247);
            this.wWallsTab.TabIndex = 2;
            this.wWallsTab.Text = "West Walls";
            // 
            // nWallsTab
            // 
            this.nWallsTab.Location = new System.Drawing.Point(4, 22);
            this.nWallsTab.Name = "nWallsTab";
            this.nWallsTab.Size = new System.Drawing.Size(312, 247);
            this.nWallsTab.TabIndex = 4;
            this.nWallsTab.Text = "North Walls";
            // 
            // objectsTab
            // 
            this.objectsTab.Location = new System.Drawing.Point(4, 22);
            this.objectsTab.Name = "objectsTab";
            this.objectsTab.Size = new System.Drawing.Size(312, 247);
            this.objectsTab.TabIndex = 3;
            this.objectsTab.Text = "Objects";
            // 
            // menuItem2
            // 
            this.menuItem2.Index = 1;
            this.menuItem2.MenuItems.AddRange(new System.Windows.Forms.MenuItem[] {
            this.EditPckMenuItem});
            this.menuItem2.Text = "PCK";
            // 
            // EditPckMenuItem
            // 
            this.EditPckMenuItem.Index = 0;
            this.EditPckMenuItem.Text = "Edit PCK";
            this.EditPckMenuItem.Click += new System.EventHandler(this.EditPckMenuItem_Click);
            // 
            // TileView
            // 
            this.AutoScaleBaseSize = new System.Drawing.Size(5, 13);
            this.ClientSize = new System.Drawing.Size(320, 273);
            this.Controls.Add(this.tabs);
            this.Menu = this.menu;
            this.Name = "TileView";
            this.ShowInTaskbar = false;
            this.Text = "TileView";
            this.tabs.ResumeLayout(false);
            this.ResumeLayout(false);

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

        private System.Windows.Forms.MenuItem menuItem2;
        private System.Windows.Forms.MenuItem EditPckMenuItem;

    }
}
