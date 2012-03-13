namespace MapView.TopViewForm
{
	partial class TopView
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
			this.toolStripContainer2 = new System.Windows.Forms.ToolStripContainer();
			this.center = new System.Windows.Forms.Panel();
			this.toolStrip = new System.Windows.Forms.ToolStrip();
			this.topViewPanel = new MapView.TopViewForm.TopViewPanel();
			this.bottom = new MapView.TopViewForm.BottomPanel();
			this.menuStrip1 = new System.Windows.Forms.MenuStrip();
			this.visibleToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.editToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.miGround = new System.Windows.Forms.ToolStripMenuItem();
			this.miWest = new System.Windows.Forms.ToolStripMenuItem();
			this.miNorth = new System.Windows.Forms.ToolStripMenuItem();
			this.miContent = new System.Windows.Forms.ToolStripMenuItem();
			this.miOptions = new System.Windows.Forms.ToolStripMenuItem();
			this.miFill = new System.Windows.Forms.ToolStripMenuItem();
			this.toolStripContainer2.ContentPanel.SuspendLayout();
			this.toolStripContainer2.LeftToolStripPanel.SuspendLayout();
			this.toolStripContainer2.SuspendLayout();
			this.center.SuspendLayout();
			this.menuStrip1.SuspendLayout();
			this.SuspendLayout();
			// 
			// toolStripContainer2
			// 
			// 
			// toolStripContainer2.BottomToolStripPanel
			// 
			this.toolStripContainer2.BottomToolStripPanel.RenderMode = System.Windows.Forms.ToolStripRenderMode.System;
			// 
			// toolStripContainer2.ContentPanel
			// 
			this.toolStripContainer2.ContentPanel.Controls.Add(this.center);
			this.toolStripContainer2.ContentPanel.Size = new System.Drawing.Size(573, 384);
			this.toolStripContainer2.Dock = System.Windows.Forms.DockStyle.Fill;
			// 
			// toolStripContainer2.LeftToolStripPanel
			// 
			this.toolStripContainer2.LeftToolStripPanel.Controls.Add(this.toolStrip);
			this.toolStripContainer2.LeftToolStripPanel.RenderMode = System.Windows.Forms.ToolStripRenderMode.System;
			this.toolStripContainer2.Location = new System.Drawing.Point(0, 24);
			this.toolStripContainer2.Name = "toolStripContainer2";
			// 
			// toolStripContainer2.RightToolStripPanel
			// 
			this.toolStripContainer2.RightToolStripPanel.RenderMode = System.Windows.Forms.ToolStripRenderMode.System;
			this.toolStripContainer2.Size = new System.Drawing.Size(598, 384);
			this.toolStripContainer2.TabIndex = 4;
			this.toolStripContainer2.Text = "toolStripContainer2";
			// 
			// toolStripContainer2.TopToolStripPanel
			// 
			this.toolStripContainer2.TopToolStripPanel.RenderMode = System.Windows.Forms.ToolStripRenderMode.System;
			// 
			// center
			// 
			this.center.AutoScroll = true;
			this.center.Controls.Add(this.topViewPanel);
			this.center.Dock = System.Windows.Forms.DockStyle.Fill;
			this.center.Location = new System.Drawing.Point(0, 0);
			this.center.Name = "center";
			this.center.Size = new System.Drawing.Size(573, 384);
			this.center.TabIndex = 2;
			// 
			// toolStrip
			// 
			this.toolStrip.Dock = System.Windows.Forms.DockStyle.None;
			this.toolStrip.Location = new System.Drawing.Point(0, 3);
			this.toolStrip.Name = "toolStrip";
			this.toolStrip.Padding = new System.Windows.Forms.Padding(0);
			this.toolStrip.RenderMode = System.Windows.Forms.ToolStripRenderMode.System;
			this.toolStrip.Size = new System.Drawing.Size(25, 111);
			this.toolStrip.TabIndex = 1;
			this.toolStrip.Text = "toolStrip1";
			// 
			// topViewPanel
			// 
			this.topViewPanel.BottomPanel = null;
			this.topViewPanel.Content = null;
			this.topViewPanel.Ground = null;
			this.topViewPanel.Location = new System.Drawing.Point(0, 0);
			this.topViewPanel.Name = "topViewPanel";
			this.topViewPanel.North = null;
			this.topViewPanel.Size = new System.Drawing.Size(371, 191);
			this.topViewPanel.TabIndex = 0;
			this.topViewPanel.West = null;
			// 
			// bottom
			// 
			this.bottom.Dock = System.Windows.Forms.DockStyle.Bottom;
			this.bottom.Location = new System.Drawing.Point(0, 408);
			this.bottom.Name = "bottom";
			this.bottom.Size = new System.Drawing.Size(598, 71);
			this.bottom.TabIndex = 0;
			this.bottom.Tile = null;
			// 
			// menuStrip1
			// 
			this.menuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.visibleToolStripMenuItem,
            this.editToolStripMenuItem});
			this.menuStrip1.Location = new System.Drawing.Point(0, 0);
			this.menuStrip1.Name = "menuStrip1";
			this.menuStrip1.Size = new System.Drawing.Size(598, 24);
			this.menuStrip1.TabIndex = 5;
			this.menuStrip1.Text = "menuStrip1";
			// 
			// visibleToolStripMenuItem
			// 
			this.visibleToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.miGround,
            this.miWest,
            this.miNorth,
            this.miContent});
			this.visibleToolStripMenuItem.Name = "visibleToolStripMenuItem";
			this.visibleToolStripMenuItem.Size = new System.Drawing.Size(53, 20);
			this.visibleToolStripMenuItem.Text = "Visible";
			// 
			// editToolStripMenuItem
			// 
			this.editToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.miOptions,
            this.miFill});
			this.editToolStripMenuItem.Name = "editToolStripMenuItem";
			this.editToolStripMenuItem.Size = new System.Drawing.Size(39, 20);
			this.editToolStripMenuItem.Text = "Edit";
			// 
			// miGround
			// 
			this.miGround.Name = "miGround";
			this.miGround.ShortcutKeys = System.Windows.Forms.Keys.F1;
			this.miGround.Size = new System.Drawing.Size(152, 22);
			this.miGround.Text = "Ground";
			this.miGround.Click += new System.EventHandler(this.visibleClick);
			// 
			// miWest
			// 
			this.miWest.Name = "miWest";
			this.miWest.ShortcutKeys = System.Windows.Forms.Keys.F2;
			this.miWest.Size = new System.Drawing.Size(152, 22);
			this.miWest.Text = "West";
			this.miWest.Click += new System.EventHandler(this.visibleClick);
			// 
			// miNorth
			// 
			this.miNorth.Name = "miNorth";
			this.miNorth.ShortcutKeys = System.Windows.Forms.Keys.F3;
			this.miNorth.Size = new System.Drawing.Size(152, 22);
			this.miNorth.Text = "North";
			this.miNorth.Click += new System.EventHandler(this.visibleClick);
			// 
			// miContent
			// 
			this.miContent.Name = "miContent";
			this.miContent.ShortcutKeys = System.Windows.Forms.Keys.F4;
			this.miContent.Size = new System.Drawing.Size(152, 22);
			this.miContent.Text = "Content";
			this.miContent.Click += new System.EventHandler(this.visibleClick);
			// 
			// miOptions
			// 
			this.miOptions.Name = "miOptions";
			this.miOptions.Size = new System.Drawing.Size(152, 22);
			this.miOptions.Text = "Options";
			this.miOptions.Click += new System.EventHandler(this.options_click);
			// 
			// miFill
			// 
			this.miFill.Name = "miFill";
			this.miFill.Size = new System.Drawing.Size(152, 22);
			this.miFill.Text = "Fill";
			this.miFill.Click += new System.EventHandler(this.fill_click);
			// 
			// TopView
			// 
			this.AutoScaleBaseSize = new System.Drawing.Size(5, 13);
			this.ClientSize = new System.Drawing.Size(598, 479);
			this.Controls.Add(this.toolStripContainer2);
			this.Controls.Add(this.bottom);
			this.Controls.Add(this.menuStrip1);
			this.MainMenuStrip = this.menuStrip1;
			this.Name = "TopView";
			this.ShowInTaskbar = false;
			this.StartPosition = System.Windows.Forms.FormStartPosition.Manual;
			this.Text = "TopView";
			this.toolStripContainer2.ContentPanel.ResumeLayout(false);
			this.toolStripContainer2.LeftToolStripPanel.ResumeLayout(false);
			this.toolStripContainer2.LeftToolStripPanel.PerformLayout();
			this.toolStripContainer2.ResumeLayout(false);
			this.toolStripContainer2.PerformLayout();
			this.center.ResumeLayout(false);
			this.menuStrip1.ResumeLayout(false);
			this.menuStrip1.PerformLayout();
			this.ResumeLayout(false);
			this.PerformLayout();

		}
		#endregion

		private BottomPanel bottom;
		private System.Windows.Forms.ToolStrip toolStrip;
		private System.Windows.Forms.Panel center;
		private System.Windows.Forms.ToolStripContainer toolStripContainer2;
		private TopViewPanel topViewPanel;
		private System.Windows.Forms.MenuStrip menuStrip1;
		private System.Windows.Forms.ToolStripMenuItem visibleToolStripMenuItem;
		private System.Windows.Forms.ToolStripMenuItem editToolStripMenuItem;
		private System.Windows.Forms.ToolStripMenuItem miGround;
		private System.Windows.Forms.ToolStripMenuItem miWest;
		private System.Windows.Forms.ToolStripMenuItem miNorth;
		private System.Windows.Forms.ToolStripMenuItem miContent;
		private System.Windows.Forms.ToolStripMenuItem miOptions;
		private System.Windows.Forms.ToolStripMenuItem miFill;
	}
}