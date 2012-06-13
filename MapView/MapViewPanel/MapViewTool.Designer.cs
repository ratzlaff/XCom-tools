namespace MapView
{
	partial class MapViewTool
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

		#region Component Designer generated code

		/// <summary> 
		/// Required method for Designer support - do not modify 
		/// the contents of this method with the code editor.
		/// </summary>
		private void InitializeComponent()
		{
			this.scrollPanel = new System.Windows.Forms.Panel();
			this.panel1 = new MapView.MapViewScroller();
			this.scrollPanel.SuspendLayout();
			this.SuspendLayout();
			// 
			// scrollPanel
			// 
			this.scrollPanel.AutoScroll = true;
			this.scrollPanel.Controls.Add(this.panel1);
			this.scrollPanel.Dock = System.Windows.Forms.DockStyle.Fill;
			this.scrollPanel.Location = new System.Drawing.Point(0, 0);
			this.scrollPanel.Name = "scrollPanel";
			this.scrollPanel.Size = new System.Drawing.Size(542, 325);
			this.scrollPanel.TabIndex = 0;
			// 
			// panel1
			// 
			this.panel1.Dock = System.Windows.Forms.DockStyle.Fill;
			this.panel1.Location = new System.Drawing.Point(0, 0);
			this.panel1.Name = "panel1";
			this.panel1.Size = new System.Drawing.Size(542, 325);
			this.panel1.TabIndex = 0;
			// 
			// MapViewTool
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.ClientSize = new System.Drawing.Size(542, 325);
			this.Controls.Add(this.scrollPanel);
			this.DockAreas = WeifenLuo.WinFormsUI.Docking.DockAreas.Document;
			this.Name = "MapViewTool";
			this.Text = "MapView";
			this.scrollPanel.ResumeLayout(false);
			this.ResumeLayout(false);

		}

		#endregion

		private System.Windows.Forms.Panel scrollPanel;
		private MapViewScroller panel1;

	}
}
