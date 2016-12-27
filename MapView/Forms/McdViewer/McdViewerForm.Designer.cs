using XCom;

namespace MapView.Forms.McdViewer
{
	partial class McdViewerForm
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
			this.rtb = new System.Windows.Forms.RichTextBox();
			this.InfoBs = new System.Windows.Forms.BindingSource(this.components);
			((System.ComponentModel.ISupportInitialize)(this.InfoBs)).BeginInit();
			this.SuspendLayout();
			//
			// rtb
			//
			this.rtb.Dock = System.Windows.Forms.DockStyle.Fill;
			this.rtb.Location = new System.Drawing.Point(0, 0);
			this.rtb.Name = "rtb";
			this.rtb.Size = new System.Drawing.Size(359, 420);
			this.rtb.TabIndex = 0;
			this.rtb.Text = "";
			//
			// InfoBs
			//
			this.InfoBs.DataSource = typeof(XCom.McdEntry);
			//
			// McdViewerForm
			//
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.ClientSize = new System.Drawing.Size(359, 420);
			this.Controls.Add(this.rtb);
			this.Name = "McdViewerForm";
			this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
			this.Text = "MCD viewer";
			((System.ComponentModel.ISupportInitialize)(this.InfoBs)).EndInit();
			this.ResumeLayout(false);

		}

		#endregion

		private System.Windows.Forms.RichTextBox rtb;
		private System.Windows.Forms.BindingSource InfoBs;
	}
}
