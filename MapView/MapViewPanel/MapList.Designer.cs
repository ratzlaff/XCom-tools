namespace MapView
{
	partial class MapList
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
			this.tvMapList = new System.Windows.Forms.TreeView();
			this.SuspendLayout();
			// 
			// tvMapList
			// 
			this.tvMapList.Dock = System.Windows.Forms.DockStyle.Fill;
			this.tvMapList.Location = new System.Drawing.Point(0, 0);
			this.tvMapList.Name = "tvMapList";
			this.tvMapList.Size = new System.Drawing.Size(284, 262);
			this.tvMapList.TabIndex = 0;
			this.tvMapList.AfterSelect += new System.Windows.Forms.TreeViewEventHandler(this.mapList_AfterSelect);
			// 
			// MapList
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.ClientSize = new System.Drawing.Size(284, 262);
			this.Controls.Add(this.tvMapList);
			this.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.Name = "MapList";
			this.Text = "Map List";
			this.ResumeLayout(false);

		}

		#endregion

		private System.Windows.Forms.TreeView tvMapList;
	}
}
