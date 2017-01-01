using System;
using System.Drawing;
using System.Collections;
using System.ComponentModel;
using System.Windows.Forms;

namespace MapView
{
	public class NewMapForm
		:
		System.Windows.Forms.Form
	{
		private System.Windows.Forms.TextBox txtRows;
		private System.Windows.Forms.Label label1;
		private System.Windows.Forms.Label label2;
		private System.Windows.Forms.TextBox txtCols;
		private System.Windows.Forms.Label label3;
		private System.Windows.Forms.TextBox txtHeight;
		private System.Windows.Forms.Label label4;
		private System.Windows.Forms.TextBox txtMapName;
		private System.Windows.Forms.GroupBox groupBox1;
		private System.Windows.Forms.Button btnOk;
		private System.ComponentModel.Container components = null;

		private string name;
		private byte r,c,h;
		private System.Windows.Forms.Label label5;

		public NewMapForm()
		{
			InitializeComponent();
			name = null;
		}

		public string MapName
		{
			get { return name; }
		}

		public byte MapRows
		{
			get { return r; }
		}

		public byte MapCols
		{
			get { return c; }
		}

		public byte MapHeight
		{
			get { return h; }
		}

		#region Windows Form Designer generated code
		
		/// <summary>
		/// Clean up any resources being used.
		/// </summary>
		protected override void Dispose( bool disposing )
		{
			if (disposing && components != null)
				components.Dispose();

			base.Dispose( disposing );
		}
		
		/// <summary>
		/// Required method for Designer support - do not modify
		/// the contents of this method with the code editor.
		/// </summary>
		private void InitializeComponent()
		{
			this.txtRows = new System.Windows.Forms.TextBox();
			this.label1 = new System.Windows.Forms.Label();
			this.label2 = new System.Windows.Forms.Label();
			this.txtCols = new System.Windows.Forms.TextBox();
			this.label3 = new System.Windows.Forms.Label();
			this.txtHeight = new System.Windows.Forms.TextBox();
			this.label4 = new System.Windows.Forms.Label();
			this.txtMapName = new System.Windows.Forms.TextBox();
			this.groupBox1 = new System.Windows.Forms.GroupBox();
			this.btnOk = new System.Windows.Forms.Button();
			this.label5 = new System.Windows.Forms.Label();
			this.groupBox1.SuspendLayout();
			this.SuspendLayout();
			// 
			// txtRows
			// 
			this.txtRows.Location = new System.Drawing.Point(15, 30);
			this.txtRows.Name = "txtRows";
			this.txtRows.Size = new System.Drawing.Size(60, 19);
			this.txtRows.TabIndex = 0;
			// 
			// label1
			// 
			this.label1.Location = new System.Drawing.Point(15, 15);
			this.label1.Name = "label1";
			this.label1.Size = new System.Drawing.Size(35, 15);
			this.label1.TabIndex = 1;
			this.label1.Text = "Rows";
			// 
			// label2
			// 
			this.label2.Location = new System.Drawing.Point(90, 15);
			this.label2.Name = "label2";
			this.label2.Size = new System.Drawing.Size(55, 15);
			this.label2.TabIndex = 3;
			this.label2.Text = "Columns";
			// 
			// txtCols
			// 
			this.txtCols.Location = new System.Drawing.Point(90, 30);
			this.txtCols.Name = "txtCols";
			this.txtCols.Size = new System.Drawing.Size(60, 19);
			this.txtCols.TabIndex = 2;
			// 
			// label3
			// 
			this.label3.Location = new System.Drawing.Point(165, 15);
			this.label3.Name = "label3";
			this.label3.Size = new System.Drawing.Size(45, 15);
			this.label3.TabIndex = 5;
			this.label3.Text = "Height";
			// 
			// txtHeight
			// 
			this.txtHeight.Location = new System.Drawing.Point(165, 30);
			this.txtHeight.Name = "txtHeight";
			this.txtHeight.Size = new System.Drawing.Size(60, 19);
			this.txtHeight.TabIndex = 4;
			// 
			// label4
			// 
			this.label4.Location = new System.Drawing.Point(5, 65);
			this.label4.Name = "label4";
			this.label4.Size = new System.Drawing.Size(40, 15);
			this.label4.TabIndex = 7;
			this.label4.Text = "Label";
			// 
			// txtMapName
			// 
			this.txtMapName.Location = new System.Drawing.Point(5, 80);
			this.txtMapName.Name = "txtMapName";
			this.txtMapName.Size = new System.Drawing.Size(235, 19);
			this.txtMapName.TabIndex = 6;
			// 
			// groupBox1
			// 
			this.groupBox1.Controls.Add(this.txtRows);
			this.groupBox1.Controls.Add(this.label1);
			this.groupBox1.Controls.Add(this.txtCols);
			this.groupBox1.Controls.Add(this.label2);
			this.groupBox1.Controls.Add(this.label3);
			this.groupBox1.Controls.Add(this.txtHeight);
			this.groupBox1.Location = new System.Drawing.Point(5, 100);
			this.groupBox1.Name = "groupBox1";
			this.groupBox1.Size = new System.Drawing.Size(235, 60);
			this.groupBox1.TabIndex = 8;
			this.groupBox1.TabStop = false;
			this.groupBox1.Text = "Dimensions";
			// 
			// btnOk
			// 
			this.btnOk.Location = new System.Drawing.Point(60, 165);
			this.btnOk.Name = "btnOk";
			this.btnOk.Size = new System.Drawing.Size(120, 25);
			this.btnOk.TabIndex = 9;
			this.btnOk.Text = "Ok";
			this.btnOk.Click += new System.EventHandler(this.btnOk_Click);
			// 
			// label5
			// 
			this.label5.ForeColor = System.Drawing.Color.MistyRose;
			this.label5.Location = new System.Drawing.Point(5, 5);
			this.label5.Name = "label5";
			this.label5.Size = new System.Drawing.Size(235, 50);
			this.label5.TabIndex = 10;
			this.label5.Text = "For best results make maps in 10x10x4 or 20x20x4 pieces. Maps with other dimensio" +
			"ns may or may not be accepted by the game engine.";
			// 
			// NewMapForm
			// 
			this.AutoScaleBaseSize = new System.Drawing.Size(5, 12);
			this.ClientSize = new System.Drawing.Size(244, 197);
			this.Controls.Add(this.label5);
			this.Controls.Add(this.btnOk);
			this.Controls.Add(this.groupBox1);
			this.Controls.Add(this.label4);
			this.Controls.Add(this.txtMapName);
			this.Font = new System.Drawing.Font("Verdana", 7F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
			this.MaximizeBox = false;
			this.MinimizeBox = false;
			this.Name = "NewMapForm";
			this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
			this.Text = "New Map";
			this.groupBox1.ResumeLayout(false);
			this.groupBox1.PerformLayout();
			this.ResumeLayout(false);
			this.PerformLayout();
		}
		#endregion

		private void btnOk_Click(object sender, System.EventArgs e)
		{
			try
			{
				r = byte.Parse(txtRows.Text);
				c = byte.Parse(txtCols.Text);
				h = byte.Parse(txtHeight.Text);
				name = txtMapName.Text;
				Close();
			}
			catch{}
		}
	}
}
