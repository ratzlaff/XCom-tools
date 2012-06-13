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
			this.menuStrip1 = new System.Windows.Forms.MenuStrip();
			this.visibleToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.miGround = new System.Windows.Forms.ToolStripMenuItem();
			this.miWest = new System.Windows.Forms.ToolStripMenuItem();
			this.miNorth = new System.Windows.Forms.ToolStripMenuItem();
			this.miContent = new System.Windows.Forms.ToolStripMenuItem();
			this.editToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.miOptions = new System.Windows.Forms.ToolStripMenuItem();
			this.miFill = new System.Windows.Forms.ToolStripMenuItem();
			this.toolStrip1 = new System.Windows.Forms.ToolStrip();
			this.tsbUp = new System.Windows.Forms.ToolStripButton();
			this.tsbDown = new System.Windows.Forms.ToolStripButton();
			this.tsbCut = new System.Windows.Forms.ToolStripButton();
			this.tsbCopy = new System.Windows.Forms.ToolStripButton();
			this.tsbPaste = new System.Windows.Forms.ToolStripButton();
			this.topViewPanel = new MapView.TopViewForm.TopViewPanel();
			this.bottom = new MapView.TopViewForm.BottomPanel();
			this.toolStripContainer2.ContentPanel.SuspendLayout();
			this.toolStripContainer2.LeftToolStripPanel.SuspendLayout();
			this.toolStripContainer2.SuspendLayout();
			this.center.SuspendLayout();
			this.menuStrip1.SuspendLayout();
			this.toolStrip1.SuspendLayout();
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
			this.toolStripContainer2.ContentPanel.Size = new System.Drawing.Size(574, 384);
			this.toolStripContainer2.Dock = System.Windows.Forms.DockStyle.Fill;
			// 
			// toolStripContainer2.LeftToolStripPanel
			// 
			this.toolStripContainer2.LeftToolStripPanel.Controls.Add(this.toolStrip1);
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
			this.center.Size = new System.Drawing.Size(574, 384);
			this.center.TabIndex = 2;
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
			// miGround
			// 
			this.miGround.Name = "miGround";
			this.miGround.ShortcutKeys = System.Windows.Forms.Keys.F1;
			this.miGround.Size = new System.Drawing.Size(136, 22);
			this.miGround.Text = "Ground";
			this.miGround.Click += new System.EventHandler(this.visibleClick);
			// 
			// miWest
			// 
			this.miWest.Name = "miWest";
			this.miWest.ShortcutKeys = System.Windows.Forms.Keys.F2;
			this.miWest.Size = new System.Drawing.Size(136, 22);
			this.miWest.Text = "West";
			this.miWest.Click += new System.EventHandler(this.visibleClick);
			// 
			// miNorth
			// 
			this.miNorth.Name = "miNorth";
			this.miNorth.ShortcutKeys = System.Windows.Forms.Keys.F3;
			this.miNorth.Size = new System.Drawing.Size(136, 22);
			this.miNorth.Text = "North";
			this.miNorth.Click += new System.EventHandler(this.visibleClick);
			// 
			// miContent
			// 
			this.miContent.Name = "miContent";
			this.miContent.ShortcutKeys = System.Windows.Forms.Keys.F4;
			this.miContent.Size = new System.Drawing.Size(136, 22);
			this.miContent.Text = "Content";
			this.miContent.Click += new System.EventHandler(this.visibleClick);
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
			// miOptions
			// 
			this.miOptions.Name = "miOptions";
			this.miOptions.Size = new System.Drawing.Size(116, 22);
			this.miOptions.Text = "Options";
			this.miOptions.Click += new System.EventHandler(this.options_click);
			// 
			// miFill
			// 
			this.miFill.Name = "miFill";
			this.miFill.Size = new System.Drawing.Size(116, 22);
			this.miFill.Text = "Fill";
			this.miFill.Click += new System.EventHandler(this.fill_click);
			// 
			// toolStrip1
			// 
			this.toolStrip1.Dock = System.Windows.Forms.DockStyle.None;
			this.toolStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.tsbUp,
            this.tsbDown,
            this.tsbCut,
            this.tsbCopy,
            this.tsbPaste});
			this.toolStrip1.Location = new System.Drawing.Point(0, 3);
			this.toolStrip1.Name = "toolStrip1";
			this.toolStrip1.Size = new System.Drawing.Size(24, 126);
			this.toolStrip1.TabIndex = 0;
			// 
			// tsbUp
			// 
			this.tsbUp.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
			this.tsbUp.Image = global::MapView.Properties.Resources.bullet_arrow_up;
			this.tsbUp.ImageTransparentColor = System.Drawing.Color.Magenta;
			this.tsbUp.Name = "tsbUp";
			this.tsbUp.Size = new System.Drawing.Size(22, 20);
			this.tsbUp.Text = "Up";
			// 
			// tsbDown
			// 
			this.tsbDown.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
			this.tsbDown.Image = global::MapView.Properties.Resources.bullet_arrow_down;
			this.tsbDown.ImageTransparentColor = System.Drawing.Color.Magenta;
			this.tsbDown.Name = "tsbDown";
			this.tsbDown.Size = new System.Drawing.Size(22, 20);
			this.tsbDown.Text = "Down";
			// 
			// tsbCut
			// 
			this.tsbCut.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
			this.tsbCut.Image = global::MapView.Properties.Resources.cut;
			this.tsbCut.ImageTransparentColor = System.Drawing.Color.Magenta;
			this.tsbCut.Name = "tsbCut";
			this.tsbCut.Size = new System.Drawing.Size(22, 20);
			this.tsbCut.Text = "Cut";
			// 
			// tsbCopy
			// 
			this.tsbCopy.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
			this.tsbCopy.Image = global::MapView.Properties.Resources.page_white_copy;
			this.tsbCopy.ImageTransparentColor = System.Drawing.Color.Magenta;
			this.tsbCopy.Name = "tsbCopy";
			this.tsbCopy.Size = new System.Drawing.Size(22, 20);
			this.tsbCopy.Text = "Copy";
			// 
			// tsbPaste
			// 
			this.tsbPaste.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
			this.tsbPaste.Image = global::MapView.Properties.Resources.page_white_paste;
			this.tsbPaste.ImageTransparentColor = System.Drawing.Color.Magenta;
			this.tsbPaste.Name = "tsbPaste";
			this.tsbPaste.Size = new System.Drawing.Size(22, 20);
			this.tsbPaste.Text = "Paste";
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
			// TopView
			// 
			this.AutoScaleBaseSize = new System.Drawing.Size(5, 13);
			this.ClientSize = new System.Drawing.Size(598, 479);
			this.Controls.Add(this.toolStripContainer2);
			this.Controls.Add(this.bottom);
			this.Controls.Add(this.menuStrip1);
			this.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.MainMenuStrip = this.menuStrip1;
			this.Name = "TopView";
			this.Text = "TopView";
			this.toolStripContainer2.ContentPanel.ResumeLayout(false);
			this.toolStripContainer2.LeftToolStripPanel.ResumeLayout(false);
			this.toolStripContainer2.LeftToolStripPanel.PerformLayout();
			this.toolStripContainer2.ResumeLayout(false);
			this.toolStripContainer2.PerformLayout();
			this.center.ResumeLayout(false);
			this.menuStrip1.ResumeLayout(false);
			this.menuStrip1.PerformLayout();
			this.toolStrip1.ResumeLayout(false);
			this.toolStrip1.PerformLayout();
			this.ResumeLayout(false);
			this.PerformLayout();

		}
		#endregion

		private BottomPanel bottom;
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
		private System.Windows.Forms.ToolStrip toolStrip1;
		private System.Windows.Forms.ToolStripButton tsbUp;
		private System.Windows.Forms.ToolStripButton tsbDown;
		private System.Windows.Forms.ToolStripButton tsbCut;
		private System.Windows.Forms.ToolStripButton tsbCopy;
		private System.Windows.Forms.ToolStripButton tsbPaste;
	}
}