namespace MapView.Forms.MapObservers.RmpViews
{
    partial class RmpViewForm
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
            this.RouteViewControl = new MapView.Forms.MapObservers.RmpViews.RmpView();
            this.SuspendLayout();
            // 
            // RouteViewControl
            // 
            this.RouteViewControl.Dock = System.Windows.Forms.DockStyle.Fill;
            this.RouteViewControl.Location = new System.Drawing.Point(0, 0);
            this.RouteViewControl.Name = "RouteViewControl";
            this.RouteViewControl.Size = new System.Drawing.Size(536, 423);
            this.RouteViewControl.TabIndex = 0;
            this.RouteViewControl.Text = "Waypoint View";
            // 
            // RmpViewForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(536, 423);
            this.Controls.Add(this.RouteViewControl);
            this.KeyPreview = true;
            this.Name = "RmpViewForm";
            this.ShowInTaskbar = false;
            this.Text = "Waypoint View";
            this.ResumeLayout(false);

        }

        #endregion

        public RmpView RouteViewControl;
    }
}