namespace XCSuite
{
	partial class DownloadStatus
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
			this.progress = new System.Windows.Forms.ProgressBar();
			this.txtStatus = new System.Windows.Forms.RichTextBox();
			this.SuspendLayout();
			// 
			// progress
			// 
			this.progress.Dock = System.Windows.Forms.DockStyle.Bottom;
			this.progress.Location = new System.Drawing.Point(0, 127);
			this.progress.Name = "progress";
			this.progress.Size = new System.Drawing.Size(150, 23);
			this.progress.TabIndex = 0;
			// 
			// txtStatus
			// 
			this.txtStatus.BorderStyle = System.Windows.Forms.BorderStyle.None;
			this.txtStatus.Dock = System.Windows.Forms.DockStyle.Fill;
			this.txtStatus.Location = new System.Drawing.Point(0, 0);
			this.txtStatus.Name = "txtStatus";
			this.txtStatus.ReadOnly = true;
			this.txtStatus.Size = new System.Drawing.Size(150, 127);
			this.txtStatus.TabIndex = 1;
			this.txtStatus.Text = "";
			// 
			// DownloadStatus
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.Controls.Add(this.txtStatus);
			this.Controls.Add(this.progress);
			this.Name = "DownloadStatus";
			this.ResumeLayout(false);

		}

		#endregion

		private System.Windows.Forms.ProgressBar progress;
		private System.Windows.Forms.RichTextBox txtStatus;
	}
}
