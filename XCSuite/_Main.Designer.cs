namespace XCSuite
{
	partial class _Main
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
			this.contentPane = new System.Windows.Forms.Panel();
			this.statusStrip1 = new System.Windows.Forms.StatusStrip();
			this.toolStripStatusLabel1 = new System.Windows.Forms.ToolStripStatusLabel();
			this.tsProgress = new System.Windows.Forms.ToolStripProgressBar();
			this.statusStrip1.SuspendLayout();
			this.SuspendLayout();
			// 
			// contentPane
			// 
			this.contentPane.AutoScroll = true;
			this.contentPane.Dock = System.Windows.Forms.DockStyle.Fill;
			this.contentPane.Location = new System.Drawing.Point(0, 0);
			this.contentPane.Name = "contentPane";
			this.contentPane.Size = new System.Drawing.Size(322, 420);
			this.contentPane.TabIndex = 0;
			// 
			// statusStrip1
			// 
			this.statusStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.toolStripStatusLabel1,
            this.tsProgress});
			this.statusStrip1.Location = new System.Drawing.Point(0, 420);
			this.statusStrip1.Name = "statusStrip1";
			this.statusStrip1.Size = new System.Drawing.Size(322, 22);
			this.statusStrip1.TabIndex = 0;
			this.statusStrip1.Text = "statusStrip1";
			// 
			// toolStripStatusLabel1
			// 
			this.toolStripStatusLabel1.AutoSize = false;
			this.toolStripStatusLabel1.Name = "toolStripStatusLabel1";
			this.toolStripStatusLabel1.Size = new System.Drawing.Size(150, 17);
			// 
			// tsProgress
			// 
			this.tsProgress.Name = "tsProgress";
			this.tsProgress.Size = new System.Drawing.Size(150, 16);
			// 
			// _Main
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.ClientSize = new System.Drawing.Size(322, 442);
			this.Controls.Add(this.contentPane);
			this.Controls.Add(this.statusStrip1);
			this.MinimumSize = new System.Drawing.Size(330, 0);
			this.Name = "_Main";
			this.Text = "Form1";
			this.statusStrip1.ResumeLayout(false);
			this.statusStrip1.PerformLayout();
			this.ResumeLayout(false);
			this.PerformLayout();

		}

		#endregion

		private System.Windows.Forms.Panel contentPane;
		private System.Windows.Forms.StatusStrip statusStrip1;
		private System.Windows.Forms.ToolStripStatusLabel toolStripStatusLabel1;
		private System.Windows.Forms.ToolStripProgressBar tsProgress;
	}
}

