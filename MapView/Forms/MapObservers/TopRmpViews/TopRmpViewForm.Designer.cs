using MapView.Forms.MapObservers.RmpViews;
using MapView.Forms.MapObservers.TopViews;

namespace MapView.Forms.MapObservers.TileViews
{
    partial class TopRmpViewForm
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
            global::MapView.Settings settings1 = new global::MapView.Settings();
            global::MapView.Settings settings2 = new global::MapView.Settings();
            this.TopViewControl = new global::MapView.Forms.MapObservers.TopViews.TopView();
            this.RouteViewControl = new global::MapView.Forms.MapObservers.RmpViews.RmpView();
            this.tabControl1 = new System.Windows.Forms.TabControl();
            this.tabPage1 = new System.Windows.Forms.TabPage();
            this.tabPage2 = new System.Windows.Forms.TabPage();
            this.tabControl1.SuspendLayout();
            this.tabPage1.SuspendLayout();
            this.tabPage2.SuspendLayout();
            this.SuspendLayout();
            // 
            // TopViewControl
            // 
            this.TopViewControl.Dock = System.Windows.Forms.DockStyle.Fill;
            this.TopViewControl.Location = new System.Drawing.Point(3, 3);
            this.TopViewControl.Name = "TopViewControl";
            this.TopViewControl.Settings = settings1;
            this.TopViewControl.Size = new System.Drawing.Size(612, 429);
            this.TopViewControl.TabIndex = 0;
            // 
            // RouteViewControl
            // 
            this.RouteViewControl.Dock = System.Windows.Forms.DockStyle.Fill;
            this.RouteViewControl.Location = new System.Drawing.Point(3, 3);
            this.RouteViewControl.Name = "RouteViewControl";
            this.RouteViewControl.Settings = settings2;
            this.RouteViewControl.Size = new System.Drawing.Size(612, 429);
            this.RouteViewControl.TabIndex = 0;
            // 
            // tabControl1
            // 
            this.tabControl1.Controls.Add(this.tabPage1);
            this.tabControl1.Controls.Add(this.tabPage2);
            this.tabControl1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tabControl1.Location = new System.Drawing.Point(0, 0);
            this.tabControl1.Name = "tabControl1";
            this.tabControl1.SelectedIndex = 0;
            this.tabControl1.Size = new System.Drawing.Size(626, 461);
            this.tabControl1.TabIndex = 1;
            // 
            // tabPage1
            // 
            this.tabPage1.Controls.Add(this.TopViewControl);
            this.tabPage1.Location = new System.Drawing.Point(4, 22);
            this.tabPage1.Name = "tabPage1";
            this.tabPage1.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage1.Size = new System.Drawing.Size(618, 435);
            this.tabPage1.TabIndex = 0;
            this.tabPage1.Text = "Top View";
            this.tabPage1.UseVisualStyleBackColor = true;
            // 
            // tabPage2
            // 
            this.tabPage2.Controls.Add(this.RouteViewControl);
            this.tabPage2.Location = new System.Drawing.Point(4, 22);
            this.tabPage2.Name = "tabPage2";
            this.tabPage2.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage2.Size = new System.Drawing.Size(618, 435);
            this.tabPage2.TabIndex = 1;
            this.tabPage2.Text = "Route View";
            this.tabPage2.UseVisualStyleBackColor = true;
            // 
            // TopRmpViewForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(626, 461);
            this.Controls.Add(this.tabControl1);
            this.Name = "TopRmpViewForm";
            this.ShowInTaskbar = false;
            this.Text = "Top/Route View";
            this.tabControl1.ResumeLayout(false);
            this.tabPage1.ResumeLayout(false);
            this.tabPage2.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        public TopView TopViewControl;
        public RmpView RouteViewControl;
        private System.Windows.Forms.TabControl tabControl1;
        private System.Windows.Forms.TabPage tabPage1;
        private System.Windows.Forms.TabPage tabPage2;
    }
}