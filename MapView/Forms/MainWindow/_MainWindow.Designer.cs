namespace MapView
{
	partial class MainWindow
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
			System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(MainWindow));
			this.mainMenu = new System.Windows.Forms.MainMenu(this.components);
			this.fileMenu = new System.Windows.Forms.MenuItem();
			this.miOpen = new System.Windows.Forms.MenuItem();
			this.saveItem = new System.Windows.Forms.MenuItem();
			this.miSaveImage = new System.Windows.Forms.MenuItem();
			this.miExport = new System.Windows.Forms.MenuItem();
			this.miResize = new System.Windows.Forms.MenuItem();
			this.miHq = new System.Windows.Forms.MenuItem();
			this.bar = new System.Windows.Forms.MenuItem();
			this.quititem = new System.Windows.Forms.MenuItem();
			this.miEdit = new System.Windows.Forms.MenuItem();
			this.miPaths = new System.Windows.Forms.MenuItem();
			this.miOptions = new System.Windows.Forms.MenuItem();
			this.miInfo = new System.Windows.Forms.MenuItem();
			this.miAnimation = new System.Windows.Forms.MenuItem();
			this.onItem = new System.Windows.Forms.MenuItem();
			this.offItem = new System.Windows.Forms.MenuItem();
			this.miDoors = new System.Windows.Forms.MenuItem();
			this.showMenu = new System.Windows.Forms.MenuItem();
			this.miHelp = new System.Windows.Forms.MenuItem();
			this.mapList = new System.Windows.Forms.TreeView();
			this.saveFile = new System.Windows.Forms.SaveFileDialog();
			this.statusStrip1 = new System.Windows.Forms.StatusStrip();
			this.statusMapName = new System.Windows.Forms.ToolStripStatusLabel();
			this.tsMapSize = new System.Windows.Forms.ToolStripStatusLabel();
			this.statusPosition = new System.Windows.Forms.ToolStripStatusLabel();
			this.toolStripContainer1 = new System.Windows.Forms.ToolStripContainer();
			this.toolStrip = new System.Windows.Forms.ToolStrip();
			this.drawSelectionBoxButton = new System.Windows.Forms.ToolStripButton();
			this.ZoomInButton = new System.Windows.Forms.ToolStripButton();
			this.ZoomOutButton = new System.Windows.Forms.ToolStripButton();
			this.AutoZoomButton = new System.Windows.Forms.ToolStripButton();
			this.cSplitList = new DSShared.Windows.CollapsibleSplitter();
			this.statusStrip1.SuspendLayout();
			this.toolStripContainer1.TopToolStripPanel.SuspendLayout();
			this.toolStripContainer1.SuspendLayout();
			this.toolStrip.SuspendLayout();
			this.SuspendLayout();
			// 
			// mainMenu
			// 
			this.mainMenu.MenuItems.AddRange(new System.Windows.Forms.MenuItem[] {
									this.fileMenu,
									this.miEdit,
									this.miAnimation,
									this.showMenu,
									this.miHelp});
			// 
			// fileMenu
			// 
			this.fileMenu.Index = 0;
			this.fileMenu.MenuItems.AddRange(new System.Windows.Forms.MenuItem[] {
									this.miOpen,
									this.saveItem,
									this.miSaveImage,
									this.miExport,
									this.miResize,
									this.miHq,
									this.bar,
									this.quititem});
			this.fileMenu.Text = "&File";
			// 
			// miOpen
			// 
			this.miOpen.Index = 0;
			this.miOpen.Text = "Open";
			this.miOpen.Click += new System.EventHandler(this.miOpen_Click);
			// 
			// saveItem
			// 
			this.saveItem.Index = 1;
			this.saveItem.Shortcut = System.Windows.Forms.Shortcut.CtrlS;
			this.saveItem.Text = "&Save";
			this.saveItem.Click += new System.EventHandler(this.saveItem_Click);
			// 
			// miSaveImage
			// 
			this.miSaveImage.Index = 2;
			this.miSaveImage.Text = "Save Image";
			this.miSaveImage.Click += new System.EventHandler(this.miSaveImage_Click);
			// 
			// miExport
			// 
			this.miExport.Index = 3;
			this.miExport.Text = "Export";
			this.miExport.Visible = false;
			this.miExport.Click += new System.EventHandler(this.miExport_Click);
			// 
			// miResize
			// 
			this.miResize.Index = 4;
			this.miResize.Text = "Resize map";
			this.miResize.Click += new System.EventHandler(this.miResize_Click);
			// 
			// miHq
			// 
			this.miHq.Index = 5;
			this.miHq.Text = "Hq2x";
			this.miHq.Click += new System.EventHandler(this.miHq_Click);
			// 
			// bar
			// 
			this.bar.Index = 6;
			this.bar.Text = "-";
			// 
			// quititem
			// 
			this.quititem.Index = 7;
			this.quititem.Text = "&Quit";
			this.quititem.Click += new System.EventHandler(this.quititem_Click);
			// 
			// miEdit
			// 
			this.miEdit.Index = 1;
			this.miEdit.MenuItems.AddRange(new System.Windows.Forms.MenuItem[] {
									this.miPaths,
									this.miOptions,
									this.miInfo});
			this.miEdit.Text = "Edit";
			// 
			// miPaths
			// 
			this.miPaths.Index = 0;
			this.miPaths.Text = "Paths";
			this.miPaths.Click += new System.EventHandler(this.miPaths_Click);
			// 
			// miOptions
			// 
			this.miOptions.Index = 1;
			this.miOptions.Text = "Options";
			this.miOptions.Click += new System.EventHandler(this.miOptions_Click);
			// 
			// miInfo
			// 
			this.miInfo.Index = 2;
			this.miInfo.Text = "Map Info";
			this.miInfo.Click += new System.EventHandler(this.miInfo_Click);
			// 
			// miAnimation
			// 
			this.miAnimation.Index = 2;
			this.miAnimation.MenuItems.AddRange(new System.Windows.Forms.MenuItem[] {
									this.onItem,
									this.offItem,
									this.miDoors});
			this.miAnimation.Text = "&Animation";
			this.miAnimation.Visible = false;
			// 
			// onItem
			// 
			this.onItem.Checked = true;
			this.onItem.Index = 0;
			this.onItem.Shortcut = System.Windows.Forms.Shortcut.F1;
			this.onItem.Text = "O&n";
			this.onItem.Click += new System.EventHandler(this.onItem_Click);
			// 
			// offItem
			// 
			this.offItem.Index = 1;
			this.offItem.Shortcut = System.Windows.Forms.Shortcut.F2;
			this.offItem.Text = "O&ff";
			this.offItem.Click += new System.EventHandler(this.offItem_Click);
			// 
			// miDoors
			// 
			this.miDoors.Index = 2;
			this.miDoors.Text = "Doors";
			this.miDoors.Click += new System.EventHandler(this.miDoors_Click);
			// 
			// showMenu
			// 
			this.showMenu.Enabled = false;
			this.showMenu.Index = 3;
			this.showMenu.Text = "&View";
			// 
			// miHelp
			// 
			this.miHelp.Index = 4;
			this.miHelp.Text = "Help";
			// 
			// mapList
			// 
			this.mapList.Dock = System.Windows.Forms.DockStyle.Left;
			this.mapList.Location = new System.Drawing.Point(0, 0);
			this.mapList.Name = "mapList";
			this.mapList.Size = new System.Drawing.Size(249, 470);
			this.mapList.TabIndex = 0;
			this.mapList.BeforeSelect += new System.Windows.Forms.TreeViewCancelEventHandler(this.mapList_BeforeSelect);
			this.mapList.AfterSelect += new System.Windows.Forms.TreeViewEventHandler(this.mapList_AfterSelect);
			// 
			// saveFile
			// 
			this.saveFile.DefaultExt = "gif";
			this.saveFile.Filter = "gif files|*.gif";
			this.saveFile.RestoreDirectory = true;
			// 
			// statusStrip1
			// 
			this.statusStrip1.Font = new System.Drawing.Font("Verdana", 7F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.statusStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
									this.statusMapName,
									this.tsMapSize,
									this.statusPosition});
			this.statusStrip1.Location = new System.Drawing.Point(257, 448);
			this.statusStrip1.Name = "statusStrip1";
			this.statusStrip1.Size = new System.Drawing.Size(335, 22);
			this.statusStrip1.TabIndex = 2;
			this.statusStrip1.Text = "statusStrip1";
			// 
			// statusMapName
			// 
			this.statusMapName.AutoSize = false;
			this.statusMapName.BorderSides = ((System.Windows.Forms.ToolStripStatusLabelBorderSides)((((System.Windows.Forms.ToolStripStatusLabelBorderSides.Left | System.Windows.Forms.ToolStripStatusLabelBorderSides.Top) 
									| System.Windows.Forms.ToolStripStatusLabelBorderSides.Right) 
									| System.Windows.Forms.ToolStripStatusLabelBorderSides.Bottom)));
			this.statusMapName.Font = new System.Drawing.Font("Verdana", 7F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.statusMapName.Name = "statusMapName";
			this.statusMapName.Size = new System.Drawing.Size(150, 17);
			// 
			// tsMapSize
			// 
			this.tsMapSize.AutoSize = false;
			this.tsMapSize.BorderSides = ((System.Windows.Forms.ToolStripStatusLabelBorderSides)((((System.Windows.Forms.ToolStripStatusLabelBorderSides.Left | System.Windows.Forms.ToolStripStatusLabelBorderSides.Top) 
									| System.Windows.Forms.ToolStripStatusLabelBorderSides.Right) 
									| System.Windows.Forms.ToolStripStatusLabelBorderSides.Bottom)));
			this.tsMapSize.Name = "tsMapSize";
			this.tsMapSize.Size = new System.Drawing.Size(80, 17);
			// 
			// statusPosition
			// 
			this.statusPosition.AutoSize = false;
			this.statusPosition.BorderSides = ((System.Windows.Forms.ToolStripStatusLabelBorderSides)((((System.Windows.Forms.ToolStripStatusLabelBorderSides.Left | System.Windows.Forms.ToolStripStatusLabelBorderSides.Top) 
									| System.Windows.Forms.ToolStripStatusLabelBorderSides.Right) 
									| System.Windows.Forms.ToolStripStatusLabelBorderSides.Bottom)));
			this.statusPosition.Name = "statusPosition";
			this.statusPosition.Size = new System.Drawing.Size(80, 17);
			// 
			// toolStripContainer1
			// 
			// 
			// toolStripContainer1.BottomToolStripPanel
			// 
			this.toolStripContainer1.BottomToolStripPanel.RenderMode = System.Windows.Forms.ToolStripRenderMode.System;
			// 
			// toolStripContainer1.ContentPanel
			// 
			this.toolStripContainer1.ContentPanel.Size = new System.Drawing.Size(335, 423);
			this.toolStripContainer1.Dock = System.Windows.Forms.DockStyle.Fill;
			// 
			// toolStripContainer1.LeftToolStripPanel
			// 
			this.toolStripContainer1.LeftToolStripPanel.RenderMode = System.Windows.Forms.ToolStripRenderMode.System;
			this.toolStripContainer1.Location = new System.Drawing.Point(257, 0);
			this.toolStripContainer1.Name = "toolStripContainer1";
			// 
			// toolStripContainer1.RightToolStripPanel
			// 
			this.toolStripContainer1.RightToolStripPanel.RenderMode = System.Windows.Forms.ToolStripRenderMode.System;
			this.toolStripContainer1.Size = new System.Drawing.Size(335, 448);
			this.toolStripContainer1.TabIndex = 3;
			this.toolStripContainer1.Text = "toolStripContainer1";
			// 
			// toolStripContainer1.TopToolStripPanel
			// 
			this.toolStripContainer1.TopToolStripPanel.Controls.Add(this.toolStrip);
			this.toolStripContainer1.TopToolStripPanel.RenderMode = System.Windows.Forms.ToolStripRenderMode.System;
			// 
			// toolStrip
			// 
			this.toolStrip.Dock = System.Windows.Forms.DockStyle.None;
			this.toolStrip.Font = new System.Drawing.Font("Verdana", 7F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.toolStrip.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
									this.drawSelectionBoxButton,
									this.ZoomInButton,
									this.ZoomOutButton,
									this.AutoZoomButton});
			this.toolStrip.Location = new System.Drawing.Point(3, 0);
			this.toolStrip.Name = "toolStrip";
			this.toolStrip.Size = new System.Drawing.Size(163, 25);
			this.toolStrip.TabIndex = 0;
			// 
			// drawSelectionBoxButton
			// 
			this.drawSelectionBoxButton.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
			this.drawSelectionBoxButton.Font = new System.Drawing.Font("Verdana", 7F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.drawSelectionBoxButton.Image = ((System.Drawing.Image)(resources.GetObject("drawSelectionBoxButton.Image")));
			this.drawSelectionBoxButton.ImageTransparentColor = System.Drawing.Color.Magenta;
			this.drawSelectionBoxButton.Name = "drawSelectionBoxButton";
			this.drawSelectionBoxButton.Size = new System.Drawing.Size(84, 22);
			this.drawSelectionBoxButton.Text = "Selection Box";
			this.drawSelectionBoxButton.ToolTipText = "Draws a selection box in the floor";
			this.drawSelectionBoxButton.Click += new System.EventHandler(this.drawSelectionBoxButton_Click);
			// 
			// ZoomInButton
			// 
			this.ZoomInButton.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
			this.ZoomInButton.Image = global::MapView.Properties.Resources._12_Zoom_in_16;
			this.ZoomInButton.ImageTransparentColor = System.Drawing.Color.Magenta;
			this.ZoomInButton.Name = "ZoomInButton";
			this.ZoomInButton.Size = new System.Drawing.Size(23, 22);
			this.ZoomInButton.Text = "Zoom In";
			this.ZoomInButton.Click += new System.EventHandler(this.ZoomInButton_Click);
			// 
			// ZoomOutButton
			// 
			this.ZoomOutButton.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
			this.ZoomOutButton.Image = global::MapView.Properties.Resources._13_Zoom_out_16;
			this.ZoomOutButton.ImageTransparentColor = System.Drawing.Color.Magenta;
			this.ZoomOutButton.Name = "ZoomOutButton";
			this.ZoomOutButton.Size = new System.Drawing.Size(23, 22);
			this.ZoomOutButton.Text = "Zoom Out";
			this.ZoomOutButton.Click += new System.EventHandler(this.ZoomOutButton_Click);
			// 
			// AutoZoomButton
			// 
			this.AutoZoomButton.Checked = true;
			this.AutoZoomButton.CheckState = System.Windows.Forms.CheckState.Checked;
			this.AutoZoomButton.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
			this.AutoZoomButton.Font = new System.Drawing.Font("Verdana", 7F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.AutoZoomButton.Image = global::MapView.Properties.Resources._11_Search_16;
			this.AutoZoomButton.ImageTransparentColor = System.Drawing.Color.Magenta;
			this.AutoZoomButton.Name = "AutoZoomButton";
			this.AutoZoomButton.Size = new System.Drawing.Size(23, 22);
			this.AutoZoomButton.Text = "Auto Zoom";
			this.AutoZoomButton.Click += new System.EventHandler(this.AutoZoomButton_Click);
			// 
			// cSplitList
			// 
			this.cSplitList.BorderStyle3D = System.Windows.Forms.Border3DStyle.Flat;
			this.cSplitList.ControlToHide = this.mapList;
			this.cSplitList.Location = new System.Drawing.Point(249, 0);
			this.cSplitList.MinimumSize = new System.Drawing.Size(5, 5);
			this.cSplitList.Name = "cSplitList";
			this.cSplitList.Size = new System.Drawing.Size(8, 470);
			this.cSplitList.TabIndex = 1;
			this.cSplitList.TabStop = false;
			// 
			// MainWindow
			// 
			this.AutoScaleBaseSize = new System.Drawing.Size(5, 12);
			this.BackColor = System.Drawing.SystemColors.Control;
			this.ClientSize = new System.Drawing.Size(592, 470);
			this.Controls.Add(this.toolStripContainer1);
			this.Controls.Add(this.statusStrip1);
			this.Controls.Add(this.cSplitList);
			this.Controls.Add(this.mapList);
			this.Font = new System.Drawing.Font("Verdana", 7F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
			this.Menu = this.mainMenu;
			this.Name = "MainWindow";
			this.StartPosition = System.Windows.Forms.FormStartPosition.Manual;
			this.Text = "Map Editor";
			this.Activated += new System.EventHandler(this.MainWindow_Activated);
			this.statusStrip1.ResumeLayout(false);
			this.statusStrip1.PerformLayout();
			this.toolStripContainer1.TopToolStripPanel.ResumeLayout(false);
			this.toolStripContainer1.TopToolStripPanel.PerformLayout();
			this.toolStripContainer1.ResumeLayout(false);
			this.toolStripContainer1.PerformLayout();
			this.toolStrip.ResumeLayout(false);
			this.toolStrip.PerformLayout();
			this.ResumeLayout(false);
			this.PerformLayout();
		}

		private System.Windows.Forms.MenuItem fileMenu;
		private System.Windows.Forms.MenuItem quititem;
		private System.Windows.Forms.MenuItem showMenu;
		private System.Windows.Forms.MenuItem saveItem;
		private System.Windows.Forms.MainMenu mainMenu;
		private System.Windows.Forms.MenuItem bar;
		private System.Windows.Forms.MenuItem onItem;
		private System.Windows.Forms.MenuItem offItem;
		private System.Windows.Forms.MenuItem miAnimation;
		private System.Windows.Forms.MenuItem miHelp;
		private System.Windows.Forms.MenuItem miEdit;
		private System.Windows.Forms.TreeView mapList;
		private System.Windows.Forms.MenuItem miPaths;
		private System.Windows.Forms.MenuItem miOptions;
		private System.Windows.Forms.MenuItem miSaveImage;
		private System.Windows.Forms.SaveFileDialog saveFile;
		private System.Windows.Forms.MenuItem miHq;
		private System.Windows.Forms.MenuItem miDoors;
		private System.Windows.Forms.MenuItem miResize;
		private System.Windows.Forms.MenuItem miInfo;
		private System.Windows.Forms.MenuItem miExport;
		private DSShared.Windows.CollapsibleSplitter cSplitList;
		private System.Windows.Forms.StatusStrip statusStrip1;
		private System.Windows.Forms.ToolStripStatusLabel statusMapName;
		private System.Windows.Forms.ToolStripStatusLabel tsMapSize;
		private System.Windows.Forms.ToolStripStatusLabel statusPosition;
		private System.Windows.Forms.ToolStripContainer toolStripContainer1;
		private System.Windows.Forms.ToolStrip toolStrip;
		private System.Windows.Forms.MenuItem miOpen;
		private System.Windows.Forms.ToolStripButton drawSelectionBoxButton;
		private System.Windows.Forms.ToolStripButton ZoomInButton;
		private System.Windows.Forms.ToolStripButton ZoomOutButton;
		private System.Windows.Forms.ToolStripButton AutoZoomButton;

		#endregion
	}
}
