namespace PckView
{
	partial class PckViewForm
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
			if (disposing && (components != null))
			{
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
			System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(PckViewForm));
			this.mainMenu = new System.Windows.Forms.MainMenu(this.components);
			this.menuItem1 = new System.Windows.Forms.MenuItem();
			this.openItem = new System.Windows.Forms.MenuItem();
			this.miCompare = new System.Windows.Forms.MenuItem();
			this.SaveMenuItem = new System.Windows.Forms.MenuItem();
			this.saveitem = new System.Windows.Forms.MenuItem();
			this.miSaveDir = new System.Windows.Forms.MenuItem();
			this.miHq2x = new System.Windows.Forms.MenuItem();
			this.quitItem = new System.Windows.Forms.MenuItem();
			this.miPalette = new System.Windows.Forms.MenuItem();
			this.bytesMenu = new System.Windows.Forms.MenuItem();
			this.showBytes = new System.Windows.Forms.MenuItem();
			this.transItem = new System.Windows.Forms.MenuItem();
			this.transOn = new System.Windows.Forms.MenuItem();
			this.menuItem4 = new System.Windows.Forms.MenuItem();
			this.aboutItem = new System.Windows.Forms.MenuItem();
			this.helpItem = new System.Windows.Forms.MenuItem();
			this.miModList = new System.Windows.Forms.MenuItem();
			this.miConsole = new System.Windows.Forms.MenuItem();
			this.MapViewIntegrationMenuItem = new System.Windows.Forms.MenuItem();
			this.openFile = new System.Windows.Forms.OpenFileDialog();
			this.saveBmpSingle = new System.Windows.Forms.SaveFileDialog();
			this.openBMP = new System.Windows.Forms.OpenFileDialog();
			this.DrawPanel = new System.Windows.Forms.Panel();
			this.MapViewIntegrationHelpPanel = new System.Windows.Forms.Panel();
			this.GotItMapViewButton = new System.Windows.Forms.Button();
			this.label1 = new System.Windows.Forms.Label();
			this.MapViewIntegrationHelpPanel.SuspendLayout();
			this.SuspendLayout();
			//
			// mainMenu
			//
			this.mainMenu.MenuItems.AddRange(new System.Windows.Forms.MenuItem[] {
			this.menuItem1,
			this.miPalette,
			this.bytesMenu,
			this.transItem,
			this.menuItem4});
			//
			// menuItem1
			//
			this.menuItem1.Index = 0;
			this.menuItem1.MenuItems.AddRange(new System.Windows.Forms.MenuItem[] {
			this.openItem,
			this.miCompare,
			this.SaveMenuItem,
			this.saveitem,
			this.miSaveDir,
			this.miHq2x,
			this.quitItem});
			this.menuItem1.Text = "&File";
			//
			// openItem
			//
			this.openItem.Index = 0;
			this.openItem.Text = "&Open";
			this.openItem.Click += new System.EventHandler(this.openItem_Click);
			//
			// miCompare
			//
			this.miCompare.Index = 1;
			this.miCompare.Text = "Compare";
			this.miCompare.Click += new System.EventHandler(this.miCompare_Click);
			//
			// SaveMenuItem
			//
			this.SaveMenuItem.Index = 2;
			this.SaveMenuItem.Text = "Save";
			this.SaveMenuItem.Click += new System.EventHandler(this.SaveMenuItem_Click);
			//
			// saveitem
			//
			this.saveitem.Enabled = false;
			this.saveitem.Index = 3;
			this.saveitem.Text = "&Save To FIle";
			this.saveitem.Click += new System.EventHandler(this.saveAs_Click);
			//
			// miSaveDir
			//
			this.miSaveDir.Index = 4;
			this.miSaveDir.Text = "Save &Image";
			this.miSaveDir.Click += new System.EventHandler(this.miSaveDir_Click);
			//
			// miHq2x
			//
			this.miHq2x.Index = 5;
			this.miHq2x.Text = "&Hq2x";
			this.miHq2x.Click += new System.EventHandler(this.miHq2x_Click);
			//
			// quitItem
			//
			this.quitItem.Index = 6;
			this.quitItem.Text = "&Quit";
			this.quitItem.Click += new System.EventHandler(this.quitItem_Click);
			//
			// miPalette
			//
			this.miPalette.Index = 1;
			this.miPalette.Text = "&Palette";
			//
			// bytesMenu
			//
			this.bytesMenu.Enabled = false;
			this.bytesMenu.Index = 2;
			this.bytesMenu.MenuItems.AddRange(new System.Windows.Forms.MenuItem[] {
			this.showBytes});
			this.bytesMenu.Text = "&Bytes";
			//
			// showBytes
			//
			this.showBytes.Index = 0;
			this.showBytes.Text = "&Show";
			this.showBytes.Click += new System.EventHandler(this.showBytes_Click);
			//
			// transItem
			//
			this.transItem.Enabled = false;
			this.transItem.Index = 3;
			this.transItem.MenuItems.AddRange(new System.Windows.Forms.MenuItem[] {
			this.transOn});
			this.transItem.Text = "Transparency";
			//
			// transOn
			//
			this.transOn.Index = 0;
			this.transOn.Text = "On";
			this.transOn.Click += new System.EventHandler(this.transOn_Click);
			//
			// menuItem4
			//
			this.menuItem4.Index = 4;
			this.menuItem4.MenuItems.AddRange(new System.Windows.Forms.MenuItem[] {
			this.aboutItem,
			this.helpItem,
			this.miModList,
			this.miConsole,
			this.MapViewIntegrationMenuItem});
			this.menuItem4.Text = "Help";
			//
			// aboutItem
			//
			this.aboutItem.Index = 0;
			this.aboutItem.Text = "About";
			this.aboutItem.Click += new System.EventHandler(this.aboutItem_Click);
			//
			// helpItem
			//
			this.helpItem.Index = 1;
			this.helpItem.Text = "Basic Help";
			this.helpItem.Click += new System.EventHandler(this.helpItem_Click);
			//
			// miModList
			//
			this.miModList.Index = 2;
			this.miModList.Text = "Mod List";
			this.miModList.Click += new System.EventHandler(this.miModList_Click);
			//
			// miConsole
			//
			this.miConsole.Index = 3;
			this.miConsole.Text = "Console";
			this.miConsole.Click += new System.EventHandler(this.miConsole_Click);
			//
			// MapViewIntegrationMenuItem
			//
			this.MapViewIntegrationMenuItem.Index = 4;
			this.MapViewIntegrationMenuItem.Text = "MapView Integration";
			this.MapViewIntegrationMenuItem.Visible = false;
			this.MapViewIntegrationMenuItem.Click += new System.EventHandler(this.MapViewIntegrationMenuItem_Click);
			//
			// saveBmpSingle
			//
			this.saveBmpSingle.DefaultExt = "*.bmp";
			this.saveBmpSingle.Filter = "BMP Files|*.bmp";
			//
			// openBMP
			//
			this.openBMP.Filter = "8-bit 32x40 bmp|*.bmp";
			//
			// DrawPanel
			//
			this.DrawPanel.Dock = System.Windows.Forms.DockStyle.Fill;
			this.DrawPanel.Location = new System.Drawing.Point(0, 107);
			this.DrawPanel.Name = "DrawPanel";
			this.DrawPanel.Size = new System.Drawing.Size(494, 82);
			this.DrawPanel.TabIndex = 1;
			//
			// MapViewIntegrationHelpPanel
			//
			this.MapViewIntegrationHelpPanel.Controls.Add(this.GotItMapViewButton);
			this.MapViewIntegrationHelpPanel.Controls.Add(this.label1);
			this.MapViewIntegrationHelpPanel.Dock = System.Windows.Forms.DockStyle.Top;
			this.MapViewIntegrationHelpPanel.Location = new System.Drawing.Point(0, 0);
			this.MapViewIntegrationHelpPanel.Name = "MapViewIntegrationHelpPanel";
			this.MapViewIntegrationHelpPanel.Size = new System.Drawing.Size(494, 107);
			this.MapViewIntegrationHelpPanel.TabIndex = 2;
			this.MapViewIntegrationHelpPanel.Visible = false;
			//
			// GotItMapViewButton
			//
			this.GotItMapViewButton.Location = new System.Drawing.Point(23, 67);
			this.GotItMapViewButton.Name = "GotItMapViewButton";
			this.GotItMapViewButton.Size = new System.Drawing.Size(97, 34);
			this.GotItMapViewButton.TabIndex = 2;
			this.GotItMapViewButton.Text = "Got it";
			this.GotItMapViewButton.UseVisualStyleBackColor = true;
			this.GotItMapViewButton.Click += new System.EventHandler(this.GotItMapViewButton_Click);
			//
			// label1
			//
			this.label1.Dock = System.Windows.Forms.DockStyle.Top;
			this.label1.Location = new System.Drawing.Point(0, 0);
			this.label1.Name = "label1";
			this.label1.Padding = new System.Windows.Forms.Padding(3);
			this.label1.Size = new System.Drawing.Size(494, 64);
			this.label1.TabIndex = 1;
			this.label1.Text = resources.GetString("label1.Text");
			//
			// PckViewForm
			//
			this.AutoScaleBaseSize = new System.Drawing.Size(5, 13);
			this.ClientSize = new System.Drawing.Size(494, 189);
			this.Controls.Add(this.DrawPanel);
			this.Controls.Add(this.MapViewIntegrationHelpPanel);
			this.Location = new System.Drawing.Point(50, 50);
			this.Menu = this.mainMenu;
			this.Name = "PckViewForm";
			this.StartPosition = System.Windows.Forms.FormStartPosition.Manual;
			this.Text = "PckView";
			this.Shown += new System.EventHandler(this.PckViewForm_Shown);
			this.KeyDown += new System.Windows.Forms.KeyEventHandler(this.PckView_KeyDown);
			this.MapViewIntegrationHelpPanel.ResumeLayout(false);
			this.ResumeLayout(false);

		}
		#endregion

		private System.Windows.Forms.MainMenu mainMenu;
		private System.Windows.Forms.MenuItem menuItem1;
		private System.Windows.Forms.MenuItem openItem;
		private System.Windows.Forms.MenuItem quitItem;
		private System.Windows.Forms.OpenFileDialog openFile;
		private System.Windows.Forms.SaveFileDialog saveBmpSingle;
		private System.Windows.Forms.MenuItem showBytes;
		private System.Windows.Forms.MenuItem bytesMenu;
		private System.Windows.Forms.MenuItem transOn;
		private System.Windows.Forms.OpenFileDialog openBMP;
		private System.Windows.Forms.MenuItem menuItem4;
		private System.Windows.Forms.MenuItem aboutItem;
		private System.Windows.Forms.MenuItem saveitem;
		private System.Windows.Forms.MenuItem transItem;
		private System.Windows.Forms.MenuItem helpItem;
		private System.Windows.Forms.MenuItem miPalette;
		private System.Windows.Forms.MenuItem miHq2x;
		private System.Windows.Forms.MenuItem miModList;
		private System.Windows.Forms.MenuItem miSaveDir;
		private System.Windows.Forms.MenuItem miConsole;
		private System.Windows.Forms.MenuItem miCompare;
		private System.Windows.Forms.MenuItem SaveMenuItem;
		private System.Windows.Forms.Panel DrawPanel;
		private System.Windows.Forms.Panel MapViewIntegrationHelpPanel;
		private System.Windows.Forms.Label label1;
		private System.Windows.Forms.MenuItem MapViewIntegrationMenuItem;
		private System.Windows.Forms.Button GotItMapViewButton;
	}
}
