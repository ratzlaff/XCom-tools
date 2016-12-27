namespace MapView.Forms.MapObservers.TopViews
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
			System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(TopView));
			this.toolStripContainer2 = new System.Windows.Forms.ToolStripContainer();
			this.center = new System.Windows.Forms.Panel();
			this.toolStrip = new System.Windows.Forms.ToolStrip();
			this.bottom = new global::MapView.Forms.MapObservers.TopViews.BottomPanel();
			this.MainToolStrip = new System.Windows.Forms.ToolStrip();
			this.VisibleToolStripButton = new System.Windows.Forms.ToolStripDropDownButton();
			this.toolStripDropDownButton1 = new System.Windows.Forms.ToolStripDropDownButton();
			this.optionsToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.toolStripContainer2.ContentPanel.SuspendLayout();
			this.toolStripContainer2.LeftToolStripPanel.SuspendLayout();
			this.toolStripContainer2.SuspendLayout();
			this.MainToolStrip.SuspendLayout();
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
			this.toolStripContainer2.ContentPanel.Size = new System.Drawing.Size(484, 213);
			this.toolStripContainer2.Dock = System.Windows.Forms.DockStyle.Fill;
			//
			// toolStripContainer2.LeftToolStripPanel
			//
			this.toolStripContainer2.LeftToolStripPanel.Controls.Add(this.toolStrip);
			this.toolStripContainer2.LeftToolStripPanel.RenderMode = System.Windows.Forms.ToolStripRenderMode.System;
			this.toolStripContainer2.Location = new System.Drawing.Point(0, 25);
			this.toolStripContainer2.Name = "toolStripContainer2";
			//
			// toolStripContainer2.RightToolStripPanel
			//
			this.toolStripContainer2.RightToolStripPanel.RenderMode = System.Windows.Forms.ToolStripRenderMode.System;
			this.toolStripContainer2.Size = new System.Drawing.Size(509, 238);
			this.toolStripContainer2.TabIndex = 4;
			this.toolStripContainer2.Text = "toolStripContainer2";
			//
			// toolStripContainer2.TopToolStripPanel
			//
			this.toolStripContainer2.TopToolStripPanel.RenderMode = System.Windows.Forms.ToolStripRenderMode.System;
			//
			// center
			//
			this.center.Dock = System.Windows.Forms.DockStyle.Fill;
			this.center.Location = new System.Drawing.Point(0, 0);
			this.center.Name = "center";
			this.center.Size = new System.Drawing.Size(484, 213);
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
			// bottom
			//
			this.bottom.Brushes = null;
			this.bottom.Dock = System.Windows.Forms.DockStyle.Bottom;
			this.bottom.Location = new System.Drawing.Point(0, 263);
			this.bottom.Name = "bottom";
			this.bottom.Pens = null;
			this.bottom.SelectedQuadrant = XCom.XCMapTile.MapQuadrant.Ground;
			this.bottom.Size = new System.Drawing.Size(509, 71);
			this.bottom.TabIndex = 0;
			this.bottom.Text = "bottom";
			//
			// MainToolStrip
			//
			this.MainToolStrip.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
			this.VisibleToolStripButton,
			this.toolStripDropDownButton1});
			this.MainToolStrip.Location = new System.Drawing.Point(0, 0);
			this.MainToolStrip.Name = "MainToolStrip";
			this.MainToolStrip.Size = new System.Drawing.Size(509, 25);
			this.MainToolStrip.TabIndex = 0;
			this.MainToolStrip.Text = "toolStrip1";
			//
			// VisibleToolStripButton
			//
			this.VisibleToolStripButton.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
			this.VisibleToolStripButton.Image = ((System.Drawing.Image)(resources.GetObject("VisibleToolStripButton.Image")));
			this.VisibleToolStripButton.ImageTransparentColor = System.Drawing.Color.Magenta;
			this.VisibleToolStripButton.Name = "VisibleToolStripButton";
			this.VisibleToolStripButton.Size = new System.Drawing.Size(54, 22);
			this.VisibleToolStripButton.Text = "Visible";
			//
			// toolStripDropDownButton1
			//
			this.toolStripDropDownButton1.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
			this.toolStripDropDownButton1.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
			this.optionsToolStripMenuItem});
			this.toolStripDropDownButton1.Image = ((System.Drawing.Image)(resources.GetObject("toolStripDropDownButton1.Image")));
			this.toolStripDropDownButton1.ImageTransparentColor = System.Drawing.Color.Magenta;
			this.toolStripDropDownButton1.Name = "toolStripDropDownButton1";
			this.toolStripDropDownButton1.Size = new System.Drawing.Size(40, 22);
			this.toolStripDropDownButton1.Text = "Edit";
			//
			// optionsToolStripMenuItem
			//
			this.optionsToolStripMenuItem.Name = "optionsToolStripMenuItem";
			this.optionsToolStripMenuItem.Size = new System.Drawing.Size(152, 22);
			this.optionsToolStripMenuItem.Text = "Options";
			this.optionsToolStripMenuItem.Click += new System.EventHandler(this.options_click);
			//
			// TopView
			//
			this.Controls.Add(this.toolStripContainer2);
			this.Controls.Add(this.bottom);
			this.Controls.Add(this.MainToolStrip);
			this.Name = "TopView";
			this.Size = new System.Drawing.Size(509, 334);
			this.KeyDown += new System.Windows.Forms.KeyEventHandler(this.TopView_KeyDown);
			this.toolStripContainer2.ContentPanel.ResumeLayout(false);
			this.toolStripContainer2.LeftToolStripPanel.ResumeLayout(false);
			this.toolStripContainer2.LeftToolStripPanel.PerformLayout();
			this.toolStripContainer2.ResumeLayout(false);
			this.toolStripContainer2.PerformLayout();
			this.MainToolStrip.ResumeLayout(false);
			this.MainToolStrip.PerformLayout();
			this.ResumeLayout(false);
			this.PerformLayout();

		}
		#endregion

		private BottomPanel bottom;
		private System.Windows.Forms.ToolStrip toolStrip;
		private System.Windows.Forms.Panel center;
		private System.Windows.Forms.ToolStripContainer toolStripContainer2;
		private System.Windows.Forms.ToolStrip MainToolStrip;
		private System.Windows.Forms.ToolStripDropDownButton VisibleToolStripButton;
		private System.Windows.Forms.ToolStripDropDownButton toolStripDropDownButton1;
		private System.Windows.Forms.ToolStripMenuItem optionsToolStripMenuItem;
	}
}
