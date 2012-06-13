namespace UtilLib.Parser.Design
{
	partial class ParseBlockEditor
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

		#region Windows Form Designer generated code

		/// <summary>
		/// Required method for Designer support - do not modify
		/// the contents of this method with the code editor.
		/// </summary>
		private void InitializeComponent()
		{
			this.splitContainer1 = new System.Windows.Forms.SplitContainer();
			this.gbCaption = new System.Windows.Forms.GroupBox();
			this.items = new System.Windows.Forms.ListBox();
			this.panel1 = new System.Windows.Forms.Panel();
			this.btnDown = new System.Windows.Forms.Button();
			this.btnUp = new System.Windows.Forms.Button();
			this.panel2 = new System.Windows.Forms.Panel();
			this.btnRemove = new System.Windows.Forms.Button();
			this.btnAdd = new System.Windows.Forms.Button();
			this.propGrid = new System.Windows.Forms.PropertyGrid();
			this.splitContainer1.Panel1.SuspendLayout();
			this.splitContainer1.Panel2.SuspendLayout();
			this.splitContainer1.SuspendLayout();
			this.gbCaption.SuspendLayout();
			this.panel1.SuspendLayout();
			this.panel2.SuspendLayout();
			this.SuspendLayout();
			// 
			// splitContainer1
			// 
			this.splitContainer1.Dock = System.Windows.Forms.DockStyle.Fill;
			this.splitContainer1.Location = new System.Drawing.Point(0, 0);
			this.splitContainer1.Name = "splitContainer1";
			// 
			// splitContainer1.Panel1
			// 
			this.splitContainer1.Panel1.Controls.Add(this.gbCaption);
			// 
			// splitContainer1.Panel2
			// 
			this.splitContainer1.Panel2.Controls.Add(this.propGrid);
			this.splitContainer1.Size = new System.Drawing.Size(343, 222);
			this.splitContainer1.SplitterDistance = 129;
			this.splitContainer1.TabIndex = 0;
			// 
			// gbCaption
			// 
			this.gbCaption.Controls.Add(this.items);
			this.gbCaption.Controls.Add(this.panel1);
			this.gbCaption.Controls.Add(this.panel2);
			this.gbCaption.Dock = System.Windows.Forms.DockStyle.Fill;
			this.gbCaption.Location = new System.Drawing.Point(0, 0);
			this.gbCaption.Name = "gbCaption";
			this.gbCaption.Size = new System.Drawing.Size(129, 222);
			this.gbCaption.TabIndex = 0;
			this.gbCaption.TabStop = false;
			this.gbCaption.Text = "Object List";
			// 
			// items
			// 
			this.items.Dock = System.Windows.Forms.DockStyle.Fill;
			this.items.FormattingEnabled = true;
			this.items.Location = new System.Drawing.Point(3, 46);
			this.items.Name = "items";
			this.items.Size = new System.Drawing.Size(96, 173);
			this.items.TabIndex = 1;
			this.items.SelectedIndexChanged += new System.EventHandler(this.items_SelectedIndexChanged);
			// 
			// panel1
			// 
			this.panel1.Controls.Add(this.btnDown);
			this.panel1.Controls.Add(this.btnUp);
			this.panel1.Dock = System.Windows.Forms.DockStyle.Right;
			this.panel1.Location = new System.Drawing.Point(99, 46);
			this.panel1.Name = "panel1";
			this.panel1.Size = new System.Drawing.Size(27, 173);
			this.panel1.TabIndex = 2;
			// 
			// btnDown
			// 
			this.btnDown.Anchor = System.Windows.Forms.AnchorStyles.None;
			this.btnDown.Image = global::UtilLib.Properties.Resources.bullet_arrow_down;
			this.btnDown.Location = new System.Drawing.Point(2, 89);
			this.btnDown.Name = "btnDown";
			this.btnDown.Size = new System.Drawing.Size(22, 23);
			this.btnDown.TabIndex = 1;
			this.btnDown.UseVisualStyleBackColor = true;
			// 
			// btnUp
			// 
			this.btnUp.Anchor = System.Windows.Forms.AnchorStyles.None;
			this.btnUp.Image = global::UtilLib.Properties.Resources.bullet_arrow_up;
			this.btnUp.Location = new System.Drawing.Point(2, 60);
			this.btnUp.Name = "btnUp";
			this.btnUp.Size = new System.Drawing.Size(22, 23);
			this.btnUp.TabIndex = 0;
			this.btnUp.UseVisualStyleBackColor = true;
			// 
			// panel2
			// 
			this.panel2.Controls.Add(this.btnRemove);
			this.panel2.Controls.Add(this.btnAdd);
			this.panel2.Dock = System.Windows.Forms.DockStyle.Top;
			this.panel2.Location = new System.Drawing.Point(3, 16);
			this.panel2.Name = "panel2";
			this.panel2.Size = new System.Drawing.Size(123, 30);
			this.panel2.TabIndex = 0;
			// 
			// btnRemove
			// 
			this.btnRemove.Anchor = System.Windows.Forms.AnchorStyles.Top;
			this.btnRemove.Location = new System.Drawing.Point(64, 3);
			this.btnRemove.Name = "btnRemove";
			this.btnRemove.Size = new System.Drawing.Size(55, 23);
			this.btnRemove.TabIndex = 1;
			this.btnRemove.Text = "Remove";
			this.btnRemove.UseVisualStyleBackColor = true;
			// 
			// btnAdd
			// 
			this.btnAdd.Anchor = System.Windows.Forms.AnchorStyles.Top;
			this.btnAdd.Location = new System.Drawing.Point(3, 3);
			this.btnAdd.Name = "btnAdd";
			this.btnAdd.Size = new System.Drawing.Size(55, 23);
			this.btnAdd.TabIndex = 0;
			this.btnAdd.Text = "Add";
			this.btnAdd.UseVisualStyleBackColor = true;
			// 
			// propGrid
			// 
			this.propGrid.Dock = System.Windows.Forms.DockStyle.Fill;
			this.propGrid.Location = new System.Drawing.Point(0, 0);
			this.propGrid.Name = "propGrid";
			this.propGrid.Size = new System.Drawing.Size(210, 222);
			this.propGrid.TabIndex = 0;
			// 
			// ParseBlockEditor
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.ClientSize = new System.Drawing.Size(343, 222);
			this.Controls.Add(this.splitContainer1);
			this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.SizableToolWindow;
			this.Name = "ParseBlockEditor";
			this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
			this.Text = "Object list editor";
			this.splitContainer1.Panel1.ResumeLayout(false);
			this.splitContainer1.Panel2.ResumeLayout(false);
			this.splitContainer1.ResumeLayout(false);
			this.gbCaption.ResumeLayout(false);
			this.panel1.ResumeLayout(false);
			this.panel2.ResumeLayout(false);
			this.ResumeLayout(false);

		}

		#endregion

		private System.Windows.Forms.SplitContainer splitContainer1;
		private System.Windows.Forms.PropertyGrid propGrid;
		private System.Windows.Forms.GroupBox gbCaption;
		private System.Windows.Forms.ListBox items;
		private System.Windows.Forms.Panel panel2;
		private System.Windows.Forms.Button btnRemove;
		private System.Windows.Forms.Button btnAdd;
		private System.Windows.Forms.Panel panel1;
		private System.Windows.Forms.Button btnDown;
		private System.Windows.Forms.Button btnUp;

	}
}