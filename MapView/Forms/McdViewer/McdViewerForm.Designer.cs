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
            this.splitContainer1 = new System.Windows.Forms.SplitContainer();
            this.rtb = new System.Windows.Forms.RichTextBox();
            this.LoftNumEditor12 = new System.Windows.Forms.NumericUpDown();
            this.InfoBs = new System.Windows.Forms.BindingSource(this.components);
            this.LoftNumEditor11 = new System.Windows.Forms.NumericUpDown();
            this.LoftNumEditor6 = new System.Windows.Forms.NumericUpDown();
            this.LoftNumEditor10 = new System.Windows.Forms.NumericUpDown();
            this.LoftNumEditor5 = new System.Windows.Forms.NumericUpDown();
            this.LoftNumEditor9 = new System.Windows.Forms.NumericUpDown();
            this.LoftNumEditor4 = new System.Windows.Forms.NumericUpDown();
            this.LoftNumEditor8 = new System.Windows.Forms.NumericUpDown();
            this.LoftNumEditor3 = new System.Windows.Forms.NumericUpDown();
            this.LoftNumEditor7 = new System.Windows.Forms.NumericUpDown();
            this.LoftNumEditor2 = new System.Windows.Forms.NumericUpDown();
            this.LoftNumEditor1 = new System.Windows.Forms.NumericUpDown();
            this.label1 = new System.Windows.Forms.Label();
            this.splitContainer1.Panel1.SuspendLayout();
            this.splitContainer1.Panel2.SuspendLayout();
            this.splitContainer1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.LoftNumEditor12)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.InfoBs)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.LoftNumEditor11)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.LoftNumEditor6)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.LoftNumEditor10)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.LoftNumEditor5)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.LoftNumEditor9)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.LoftNumEditor4)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.LoftNumEditor8)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.LoftNumEditor3)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.LoftNumEditor7)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.LoftNumEditor2)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.LoftNumEditor1)).BeginInit();
            this.SuspendLayout();
            // 
            // splitContainer1
            // 
            this.splitContainer1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.splitContainer1.Location = new System.Drawing.Point(0, 0);
            this.splitContainer1.Name = "splitContainer1";
            this.splitContainer1.Orientation = System.Windows.Forms.Orientation.Horizontal;
            // 
            // splitContainer1.Panel1
            // 
            this.splitContainer1.Panel1.Controls.Add(this.rtb);
            // 
            // splitContainer1.Panel2
            // 
            this.splitContainer1.Panel2.Controls.Add(this.LoftNumEditor12);
            this.splitContainer1.Panel2.Controls.Add(this.LoftNumEditor11);
            this.splitContainer1.Panel2.Controls.Add(this.LoftNumEditor6);
            this.splitContainer1.Panel2.Controls.Add(this.LoftNumEditor10);
            this.splitContainer1.Panel2.Controls.Add(this.LoftNumEditor5);
            this.splitContainer1.Panel2.Controls.Add(this.LoftNumEditor9);
            this.splitContainer1.Panel2.Controls.Add(this.LoftNumEditor4);
            this.splitContainer1.Panel2.Controls.Add(this.LoftNumEditor8);
            this.splitContainer1.Panel2.Controls.Add(this.LoftNumEditor3);
            this.splitContainer1.Panel2.Controls.Add(this.LoftNumEditor7);
            this.splitContainer1.Panel2.Controls.Add(this.LoftNumEditor2);
            this.splitContainer1.Panel2.Controls.Add(this.LoftNumEditor1);
            this.splitContainer1.Panel2.Controls.Add(this.label1);
            this.splitContainer1.Size = new System.Drawing.Size(375, 592);
            this.splitContainer1.SplitterDistance = 349;
            this.splitContainer1.TabIndex = 0;
            // 
            // rtb
            // 
            this.rtb.Dock = System.Windows.Forms.DockStyle.Fill;
            this.rtb.Location = new System.Drawing.Point(0, 0);
            this.rtb.Name = "rtb";
            this.rtb.Size = new System.Drawing.Size(375, 349);
            this.rtb.TabIndex = 0;
            this.rtb.Text = "";
            // 
            // LoftNumEditor12
            // 
            this.LoftNumEditor12.DataBindings.Add(new System.Windows.Forms.Binding("Value", this.InfoBs, "Loft12", true));
            this.LoftNumEditor12.Location = new System.Drawing.Point(283, 38);
            this.LoftNumEditor12.Maximum = new decimal(new int[] {
            255,
            0,
            0,
            0});
            this.LoftNumEditor12.Name = "LoftNumEditor12";
            this.LoftNumEditor12.Size = new System.Drawing.Size(42, 20);
            this.LoftNumEditor12.TabIndex = 1;
            this.LoftNumEditor12.Value = new decimal(new int[] {
            100,
            0,
            0,
            0});
            // 
            // InfoBs
            // 
            this.InfoBs.DataSource = typeof(XCom.McdEntry);
            // 
            // LoftNumEditor11
            // 
            this.LoftNumEditor11.DataBindings.Add(new System.Windows.Forms.Binding("Value", this.InfoBs, "Loft11", true));
            this.LoftNumEditor11.Location = new System.Drawing.Point(235, 38);
            this.LoftNumEditor11.Maximum = new decimal(new int[] {
            255,
            0,
            0,
            0});
            this.LoftNumEditor11.Name = "LoftNumEditor11";
            this.LoftNumEditor11.Size = new System.Drawing.Size(42, 20);
            this.LoftNumEditor11.TabIndex = 1;
            this.LoftNumEditor11.Value = new decimal(new int[] {
            100,
            0,
            0,
            0});
            // 
            // LoftNumEditor6
            // 
            this.LoftNumEditor6.DataBindings.Add(new System.Windows.Forms.Binding("Value", this.InfoBs, "Loft6", true));
            this.LoftNumEditor6.Location = new System.Drawing.Point(283, 12);
            this.LoftNumEditor6.Maximum = new decimal(new int[] {
            255,
            0,
            0,
            0});
            this.LoftNumEditor6.Name = "LoftNumEditor6";
            this.LoftNumEditor6.Size = new System.Drawing.Size(42, 20);
            this.LoftNumEditor6.TabIndex = 1;
            this.LoftNumEditor6.Value = new decimal(new int[] {
            100,
            0,
            0,
            0});
            // 
            // LoftNumEditor10
            // 
            this.LoftNumEditor10.DataBindings.Add(new System.Windows.Forms.Binding("Value", this.InfoBs, "Loft10", true));
            this.LoftNumEditor10.Location = new System.Drawing.Point(187, 38);
            this.LoftNumEditor10.Maximum = new decimal(new int[] {
            255,
            0,
            0,
            0});
            this.LoftNumEditor10.Name = "LoftNumEditor10";
            this.LoftNumEditor10.Size = new System.Drawing.Size(42, 20);
            this.LoftNumEditor10.TabIndex = 1;
            this.LoftNumEditor10.Value = new decimal(new int[] {
            100,
            0,
            0,
            0});
            // 
            // LoftNumEditor5
            // 
            this.LoftNumEditor5.DataBindings.Add(new System.Windows.Forms.Binding("Value", this.InfoBs, "Loft5", true));
            this.LoftNumEditor5.Location = new System.Drawing.Point(235, 12);
            this.LoftNumEditor5.Maximum = new decimal(new int[] {
            255,
            0,
            0,
            0});
            this.LoftNumEditor5.Name = "LoftNumEditor5";
            this.LoftNumEditor5.Size = new System.Drawing.Size(42, 20);
            this.LoftNumEditor5.TabIndex = 1;
            this.LoftNumEditor5.Value = new decimal(new int[] {
            100,
            0,
            0,
            0});
            // 
            // LoftNumEditor9
            // 
            this.LoftNumEditor9.DataBindings.Add(new System.Windows.Forms.Binding("Value", this.InfoBs, "Loft9", true));
            this.LoftNumEditor9.Location = new System.Drawing.Point(139, 38);
            this.LoftNumEditor9.Maximum = new decimal(new int[] {
            255,
            0,
            0,
            0});
            this.LoftNumEditor9.Name = "LoftNumEditor9";
            this.LoftNumEditor9.Size = new System.Drawing.Size(42, 20);
            this.LoftNumEditor9.TabIndex = 1;
            this.LoftNumEditor9.Value = new decimal(new int[] {
            100,
            0,
            0,
            0});
            // 
            // LoftNumEditor4
            // 
            this.LoftNumEditor4.DataBindings.Add(new System.Windows.Forms.Binding("Value", this.InfoBs, "Loft4", true));
            this.LoftNumEditor4.Location = new System.Drawing.Point(187, 12);
            this.LoftNumEditor4.Maximum = new decimal(new int[] {
            255,
            0,
            0,
            0});
            this.LoftNumEditor4.Name = "LoftNumEditor4";
            this.LoftNumEditor4.Size = new System.Drawing.Size(42, 20);
            this.LoftNumEditor4.TabIndex = 1;
            this.LoftNumEditor4.Value = new decimal(new int[] {
            100,
            0,
            0,
            0});
            // 
            // LoftNumEditor8
            // 
            this.LoftNumEditor8.DataBindings.Add(new System.Windows.Forms.Binding("Value", this.InfoBs, "Loft8", true));
            this.LoftNumEditor8.Location = new System.Drawing.Point(91, 38);
            this.LoftNumEditor8.Maximum = new decimal(new int[] {
            255,
            0,
            0,
            0});
            this.LoftNumEditor8.Name = "LoftNumEditor8";
            this.LoftNumEditor8.Size = new System.Drawing.Size(42, 20);
            this.LoftNumEditor8.TabIndex = 1;
            this.LoftNumEditor8.Value = new decimal(new int[] {
            100,
            0,
            0,
            0});
            // 
            // LoftNumEditor3
            // 
            this.LoftNumEditor3.DataBindings.Add(new System.Windows.Forms.Binding("Value", this.InfoBs, "Loft3", true));
            this.LoftNumEditor3.Location = new System.Drawing.Point(139, 12);
            this.LoftNumEditor3.Maximum = new decimal(new int[] {
            255,
            0,
            0,
            0});
            this.LoftNumEditor3.Name = "LoftNumEditor3";
            this.LoftNumEditor3.Size = new System.Drawing.Size(42, 20);
            this.LoftNumEditor3.TabIndex = 1;
            this.LoftNumEditor3.Value = new decimal(new int[] {
            100,
            0,
            0,
            0});
            // 
            // LoftNumEditor7
            // 
            this.LoftNumEditor7.DataBindings.Add(new System.Windows.Forms.Binding("Value", this.InfoBs, "Loft7", true));
            this.LoftNumEditor7.Location = new System.Drawing.Point(43, 38);
            this.LoftNumEditor7.Maximum = new decimal(new int[] {
            255,
            0,
            0,
            0});
            this.LoftNumEditor7.Name = "LoftNumEditor7";
            this.LoftNumEditor7.Size = new System.Drawing.Size(42, 20);
            this.LoftNumEditor7.TabIndex = 1;
            this.LoftNumEditor7.Value = new decimal(new int[] {
            100,
            0,
            0,
            0});
            // 
            // LoftNumEditor2
            // 
            this.LoftNumEditor2.DataBindings.Add(new System.Windows.Forms.Binding("Value", this.InfoBs, "Loft2", true));
            this.LoftNumEditor2.Location = new System.Drawing.Point(91, 12);
            this.LoftNumEditor2.Maximum = new decimal(new int[] {
            255,
            0,
            0,
            0});
            this.LoftNumEditor2.Name = "LoftNumEditor2";
            this.LoftNumEditor2.Size = new System.Drawing.Size(42, 20);
            this.LoftNumEditor2.TabIndex = 1;
            this.LoftNumEditor2.Value = new decimal(new int[] {
            100,
            0,
            0,
            0});
            // 
            // LoftNumEditor1
            // 
            this.LoftNumEditor1.DataBindings.Add(new System.Windows.Forms.Binding("Value", this.InfoBs, "Loft1", true));
            this.LoftNumEditor1.Location = new System.Drawing.Point(43, 12);
            this.LoftNumEditor1.Maximum = new decimal(new int[] {
            255,
            0,
            0,
            0});
            this.LoftNumEditor1.Name = "LoftNumEditor1";
            this.LoftNumEditor1.Size = new System.Drawing.Size(42, 20);
            this.LoftNumEditor1.TabIndex = 1;
            this.LoftNumEditor1.Value = new decimal(new int[] {
            100,
            0,
            0,
            0});
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(12, 14);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(25, 13);
            this.label1.TabIndex = 0;
            this.label1.Text = "Loft";
            // 
            // McdViewerForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(375, 592);
            this.Controls.Add(this.splitContainer1);
            this.Name = "McdViewerForm";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Yea. A good MCD viewer";
            this.splitContainer1.Panel1.ResumeLayout(false);
            this.splitContainer1.Panel2.ResumeLayout(false);
            this.splitContainer1.Panel2.PerformLayout();
            this.splitContainer1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.LoftNumEditor12)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.InfoBs)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.LoftNumEditor11)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.LoftNumEditor6)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.LoftNumEditor10)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.LoftNumEditor5)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.LoftNumEditor9)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.LoftNumEditor4)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.LoftNumEditor8)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.LoftNumEditor3)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.LoftNumEditor7)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.LoftNumEditor2)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.LoftNumEditor1)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.SplitContainer splitContainer1;
        private System.Windows.Forms.RichTextBox rtb;
        private System.Windows.Forms.NumericUpDown LoftNumEditor12;
        private System.Windows.Forms.NumericUpDown LoftNumEditor11;
        private System.Windows.Forms.NumericUpDown LoftNumEditor6;
        private System.Windows.Forms.NumericUpDown LoftNumEditor10;
        private System.Windows.Forms.NumericUpDown LoftNumEditor5;
        private System.Windows.Forms.NumericUpDown LoftNumEditor9;
        private System.Windows.Forms.NumericUpDown LoftNumEditor4;
        private System.Windows.Forms.NumericUpDown LoftNumEditor8;
        private System.Windows.Forms.NumericUpDown LoftNumEditor3;
        private System.Windows.Forms.NumericUpDown LoftNumEditor7;
        private System.Windows.Forms.NumericUpDown LoftNumEditor2;
        private System.Windows.Forms.NumericUpDown LoftNumEditor1;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.BindingSource InfoBs;
    }
}