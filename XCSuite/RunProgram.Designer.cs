namespace XCSuite
{
	partial class RunProgram
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

		#region Component Designer generated code

		/// <summary> 
		/// Required method for Designer support - do not modify 
		/// the contents of this method with the code editor.
		/// </summary>
		private void InitializeComponent()
		{
			this.displayIcon = new System.Windows.Forms.PictureBox();
			this.panel1 = new System.Windows.Forms.Panel();
			this.btnRun = new System.Windows.Forms.Button();
			this.midPanel = new System.Windows.Forms.Panel();
			this.txtDisplay = new System.Windows.Forms.RichTextBox();
			this.btnUpdate = new System.Windows.Forms.Button();
			((System.ComponentModel.ISupportInitialize)(this.displayIcon)).BeginInit();
			this.panel1.SuspendLayout();
			this.midPanel.SuspendLayout();
			this.SuspendLayout();
			// 
			// displayIcon
			// 
			this.displayIcon.Dock = System.Windows.Forms.DockStyle.Left;
			this.displayIcon.InitialImage = null;
			this.displayIcon.Location = new System.Drawing.Point(0, 0);
			this.displayIcon.Name = "displayIcon";
			this.displayIcon.Size = new System.Drawing.Size(40, 58);
			this.displayIcon.TabIndex = 0;
			this.displayIcon.TabStop = false;
			// 
			// panel1
			// 
			this.panel1.Controls.Add(this.btnUpdate);
			this.panel1.Controls.Add(this.btnRun);
			this.panel1.Dock = System.Windows.Forms.DockStyle.Right;
			this.panel1.Location = new System.Drawing.Point(268, 0);
			this.panel1.Name = "panel1";
			this.panel1.Size = new System.Drawing.Size(60, 58);
			this.panel1.TabIndex = 1;
			// 
			// btnRun
			// 
			this.btnRun.Anchor = System.Windows.Forms.AnchorStyles.None;
			this.btnRun.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
			this.btnRun.Location = new System.Drawing.Point(4, 3);
			this.btnRun.Name = "btnRun";
			this.btnRun.Size = new System.Drawing.Size(52, 23);
			this.btnRun.TabIndex = 0;
			this.btnRun.Text = "Run";
			this.btnRun.UseVisualStyleBackColor = true;
			this.btnRun.Click += new System.EventHandler(this.btnRun_Click);
			// 
			// midPanel
			// 
			this.midPanel.Controls.Add(this.txtDisplay);
			this.midPanel.Dock = System.Windows.Forms.DockStyle.Fill;
			this.midPanel.Location = new System.Drawing.Point(40, 0);
			this.midPanel.Name = "midPanel";
			this.midPanel.Size = new System.Drawing.Size(228, 58);
			this.midPanel.TabIndex = 2;
			// 
			// txtDisplay
			// 
			this.txtDisplay.BackColor = System.Drawing.SystemColors.Control;
			this.txtDisplay.BorderStyle = System.Windows.Forms.BorderStyle.None;
			this.txtDisplay.Dock = System.Windows.Forms.DockStyle.Fill;
			this.txtDisplay.Location = new System.Drawing.Point(0, 0);
			this.txtDisplay.Name = "txtDisplay";
			this.txtDisplay.ReadOnly = true;
			this.txtDisplay.Size = new System.Drawing.Size(228, 58);
			this.txtDisplay.TabIndex = 0;
			this.txtDisplay.Text = "";
			// 
			// btnUpdate
			// 
			this.btnUpdate.Anchor = System.Windows.Forms.AnchorStyles.None;
			this.btnUpdate.AutoSize = true;
			this.btnUpdate.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
			this.btnUpdate.Enabled = false;
			this.btnUpdate.Location = new System.Drawing.Point(4, 32);
			this.btnUpdate.Name = "btnUpdate";
			this.btnUpdate.Size = new System.Drawing.Size(52, 23);
			this.btnUpdate.TabIndex = 1;
			this.btnUpdate.Text = "Update";
			this.btnUpdate.UseVisualStyleBackColor = true;
			this.btnUpdate.Click += new System.EventHandler(this.btnUpdate_Click);
			// 
			// RunProgram
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
			this.Controls.Add(this.midPanel);
			this.Controls.Add(this.panel1);
			this.Controls.Add(this.displayIcon);
			this.ForeColor = System.Drawing.SystemColors.ControlText;
			this.Name = "RunProgram";
			this.Size = new System.Drawing.Size(328, 58);
			this.MouseEnter += new System.EventHandler(this.RunProgram_MouseEnter);
			this.MouseLeave += new System.EventHandler(this.RunProgram_MouseLeave);
			((System.ComponentModel.ISupportInitialize)(this.displayIcon)).EndInit();
			this.panel1.ResumeLayout(false);
			this.panel1.PerformLayout();
			this.midPanel.ResumeLayout(false);
			this.ResumeLayout(false);

		}

		#endregion

		private System.Windows.Forms.PictureBox displayIcon;
		private System.Windows.Forms.Panel panel1;
		private System.Windows.Forms.Panel midPanel;
		private System.Windows.Forms.Button btnRun;
		private System.Windows.Forms.RichTextBox txtDisplay;
		private System.Windows.Forms.Button btnUpdate;
	}
}
