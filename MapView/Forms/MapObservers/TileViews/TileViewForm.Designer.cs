namespace MapView.Forms.MapObservers.TileViews
{
	partial class TileViewForm
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
//			MapView.Settings settings1 = new MapView.Settings();
			this.TileViewControl = new global::MapView.Forms.MapObservers.TileViews.TileView();
			this.SuspendLayout();
			// 
			// TileViewControl
			// 
			this.TileViewControl.Dock = System.Windows.Forms.DockStyle.Fill;
			this.TileViewControl.Font = new System.Drawing.Font("Verdana", 7F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.TileViewControl.Location = new System.Drawing.Point(0, 0);
			this.TileViewControl.Name = "TileViewControl";
//			this.TileViewControl.Settings = settings1;
			this.TileViewControl.Size = new System.Drawing.Size(546, 345);
			this.TileViewControl.TabIndex = 0;
			// 
			// TileViewForm
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.ClientSize = new System.Drawing.Size(546, 345);
			this.Controls.Add(this.TileViewControl);
			this.Font = new System.Drawing.Font("Verdana", 7F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.Name = "TileViewForm";
			this.ShowInTaskbar = false;
			this.Text = "Tile View";
			this.ResumeLayout(false);
		}

		#endregion

		public  TileView TileViewControl;
	}
}
