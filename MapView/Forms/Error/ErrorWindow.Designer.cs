namespace MapView.Forms.Error
{
	partial class ErrorWindow
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
			this.label1 = new System.Windows.Forms.Label();
			this.panel1 = new System.Windows.Forms.Panel();
			this.ErrorDetailsPanel = new System.Windows.Forms.GroupBox();
			this.DetailsLabel = new System.Windows.Forms.TextBox();
			this.CloseButton = new System.Windows.Forms.Button();
			this.panel1.SuspendLayout();
			this.ErrorDetailsPanel.SuspendLayout();
			this.SuspendLayout();
			// 
			// label1
			// 
			this.label1.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
									| System.Windows.Forms.AnchorStyles.Right)));
			this.label1.Font = new System.Drawing.Font("Comic Sans MS", 14F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.label1.Location = new System.Drawing.Point(8, 0);
			this.label1.Name = "label1";
			this.label1.Size = new System.Drawing.Size(472, 148);
			this.label1.TabIndex = 0;
			this.label1.Text = "Fuck You!\r\nFixyourdamncode!!!\r\n\r\noh sry.";
			this.label1.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
			// 
			// panel1
			// 
			this.panel1.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
									| System.Windows.Forms.AnchorStyles.Left) 
									| System.Windows.Forms.AnchorStyles.Right)));
			this.panel1.Controls.Add(this.ErrorDetailsPanel);
			this.panel1.Location = new System.Drawing.Point(12, 148);
			this.panel1.Name = "panel1";
			this.panel1.Size = new System.Drawing.Size(465, 133);
			this.panel1.TabIndex = 1;
			// 
			// ErrorDetailsPanel
			// 
			this.ErrorDetailsPanel.Controls.Add(this.DetailsLabel);
			this.ErrorDetailsPanel.Dock = System.Windows.Forms.DockStyle.Top;
			this.ErrorDetailsPanel.Font = new System.Drawing.Font("Verdana", 7F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.ErrorDetailsPanel.Location = new System.Drawing.Point(0, 0);
			this.ErrorDetailsPanel.Margin = new System.Windows.Forms.Padding(0);
			this.ErrorDetailsPanel.Name = "ErrorDetailsPanel";
			this.ErrorDetailsPanel.Size = new System.Drawing.Size(465, 133);
			this.ErrorDetailsPanel.TabIndex = 0;
			this.ErrorDetailsPanel.TabStop = false;
			this.ErrorDetailsPanel.Text = "Error Details";
			// 
			// DetailsLabel
			// 
			this.DetailsLabel.Dock = System.Windows.Forms.DockStyle.Fill;
			this.DetailsLabel.Font = new System.Drawing.Font("Verdana", 7F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.DetailsLabel.Location = new System.Drawing.Point(3, 14);
			this.DetailsLabel.Multiline = true;
			this.DetailsLabel.Name = "DetailsLabel";
			this.DetailsLabel.ReadOnly = true;
			this.DetailsLabel.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
			this.DetailsLabel.Size = new System.Drawing.Size(459, 117);
			this.DetailsLabel.TabIndex = 1;
			this.DetailsLabel.Text = "Error Details go here";
			// 
			// CloseButton
			// 
			this.CloseButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
			this.CloseButton.DialogResult = System.Windows.Forms.DialogResult.OK;
			this.CloseButton.Font = new System.Drawing.Font("Verdana", 7F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.CloseButton.Location = new System.Drawing.Point(328, 293);
			this.CloseButton.Name = "CloseButton";
			this.CloseButton.Size = new System.Drawing.Size(152, 40);
			this.CloseButton.TabIndex = 2;
			this.CloseButton.Text = "Close";
			this.CloseButton.UseVisualStyleBackColor = true;
			// 
			// ErrorWindow
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.ClientSize = new System.Drawing.Size(489, 337);
			this.Controls.Add(this.CloseButton);
			this.Controls.Add(this.panel1);
			this.Controls.Add(this.label1);
			this.Font = new System.Drawing.Font("Verdana", 7F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.Name = "ErrorWindow";
			this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
			this.Text = "Opps something went wrong!!";
			this.Load += new System.EventHandler(this.ErrorWindow_Load);
			this.panel1.ResumeLayout(false);
			this.ErrorDetailsPanel.ResumeLayout(false);
			this.ErrorDetailsPanel.PerformLayout();
			this.ResumeLayout(false);
		}

		#endregion

		private System.Windows.Forms.Label label1;
		private System.Windows.Forms.Panel panel1;
		private System.Windows.Forms.GroupBox ErrorDetailsPanel;
		private System.Windows.Forms.TextBox DetailsLabel;
		private System.Windows.Forms.Button CloseButton;
	}
}
