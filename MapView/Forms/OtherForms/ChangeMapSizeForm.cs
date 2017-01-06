using System;
using System.Drawing;
using System.Collections;
using System.ComponentModel;
using System.Windows.Forms;

using XCom;
using XCom.Interfaces;
using XCom.Interfaces.Base;

namespace MapView
{
	public class ChangeMapSizeForm
		:
		System.Windows.Forms.Form
	{
		private System.Windows.Forms.Label label1;
		private System.Windows.Forms.TextBox txtR;
		private System.Windows.Forms.Label label2;
		private System.Windows.Forms.Label label3;
		private System.Windows.Forms.Label label4;
		private System.Windows.Forms.Label label5;
		private System.Windows.Forms.TextBox txtC;
		private System.Windows.Forms.TextBox txtH;
		private System.Windows.Forms.Button btnOk;
		private System.Windows.Forms.Button btnCancel;
		private System.ComponentModel.Container components = null;
		private System.Windows.Forms.TextBox oldC;
		private System.Windows.Forms.TextBox oldR;
		private System.Windows.Forms.TextBox oldH;
		private CheckBox CeilingCheckBox;

		private IMap_Base map;

		public ChangeMapSizeForm()
		{
			InitializeComponent();

			DialogResult = DialogResult.Cancel;
		}

		public IMap_Base Map
		{
			get { return map; }
			set
			{
				map = value;
				if (map != null)
				{
					txtR.Text =
					oldR.Text = map.MapSize.Rows.ToString();
					txtC.Text =
					oldC.Text = map.MapSize.Cols.ToString();
					txtH.Text =
					oldH.Text = map.MapSize.Height.ToString();
				}
			}
		}

		public int NewRows
		{
			get { return int.Parse(txtR.Text); }
		}

		public int NewCols
		{
			get { return int.Parse(txtC.Text); }
		}

		public int NewHeight
		{
			get { return int.Parse(txtH.Text); }
		}

		public bool AddHeightToCelling
		{
			get { return CeilingCheckBox.Checked; }
		}

		#region Windows Form Designer generated code
		/// <summary>
		/// Clean up any resources being used.
		/// </summary>
		protected override void Dispose(bool disposing)
		{
			if (disposing && components != null)
				components.Dispose();

			base.Dispose(disposing);
		}

		/// <summary>
		/// Required method for Designer support - do not modify
		/// the contents of this method with the code editor.
		/// </summary>
		private void InitializeComponent()
		{
			this.oldC = new System.Windows.Forms.TextBox();
			this.oldR = new System.Windows.Forms.TextBox();
			this.oldH = new System.Windows.Forms.TextBox();
			this.label1 = new System.Windows.Forms.Label();
			this.txtR = new System.Windows.Forms.TextBox();
			this.label2 = new System.Windows.Forms.Label();
			this.label3 = new System.Windows.Forms.Label();
			this.label4 = new System.Windows.Forms.Label();
			this.label5 = new System.Windows.Forms.Label();
			this.txtC = new System.Windows.Forms.TextBox();
			this.txtH = new System.Windows.Forms.TextBox();
			this.btnOk = new System.Windows.Forms.Button();
			this.btnCancel = new System.Windows.Forms.Button();
			this.CeilingCheckBox = new System.Windows.Forms.CheckBox();
			this.SuspendLayout();
			// 
			// oldC
			// 
			this.oldC.Location = new System.Drawing.Point(55, 40);
			this.oldC.Name = "oldC";
			this.oldC.ReadOnly = true;
			this.oldC.Size = new System.Drawing.Size(45, 19);
			this.oldC.TabIndex = 7;
			// 
			// oldR
			// 
			this.oldR.Location = new System.Drawing.Point(5, 40);
			this.oldR.Name = "oldR";
			this.oldR.ReadOnly = true;
			this.oldR.Size = new System.Drawing.Size(45, 19);
			this.oldR.TabIndex = 6;
			// 
			// oldH
			// 
			this.oldH.Location = new System.Drawing.Point(105, 40);
			this.oldH.Name = "oldH";
			this.oldH.ReadOnly = true;
			this.oldH.Size = new System.Drawing.Size(45, 19);
			this.oldH.TabIndex = 8;
			// 
			// label1
			// 
			this.label1.Location = new System.Drawing.Point(5, 10);
			this.label1.Name = "label1";
			this.label1.Size = new System.Drawing.Size(55, 15);
			this.label1.TabIndex = 0;
			this.label1.Text = "Old Size";
			// 
			// txtR
			// 
			this.txtR.Location = new System.Drawing.Point(5, 90);
			this.txtR.Name = "txtR";
			this.txtR.Size = new System.Drawing.Size(45, 19);
			this.txtR.TabIndex = 1;
			// 
			// label2
			// 
			this.label2.Location = new System.Drawing.Point(5, 75);
			this.label2.Name = "label2";
			this.label2.Size = new System.Drawing.Size(55, 15);
			this.label2.TabIndex = 5;
			this.label2.Text = "New Size";
			// 
			// label3
			// 
			this.label3.Location = new System.Drawing.Point(55, 25);
			this.label3.Name = "label3";
			this.label3.Size = new System.Drawing.Size(45, 15);
			this.label3.TabIndex = 6;
			this.label3.Text = "c";
			this.label3.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
			// 
			// label4
			// 
			this.label4.Location = new System.Drawing.Point(5, 25);
			this.label4.Name = "label4";
			this.label4.Size = new System.Drawing.Size(45, 15);
			this.label4.TabIndex = 7;
			this.label4.Text = "r";
			this.label4.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
			// 
			// label5
			// 
			this.label5.Location = new System.Drawing.Point(105, 25);
			this.label5.Name = "label5";
			this.label5.Size = new System.Drawing.Size(45, 15);
			this.label5.TabIndex = 8;
			this.label5.Text = "h";
			this.label5.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
			// 
			// txtC
			// 
			this.txtC.Location = new System.Drawing.Point(55, 90);
			this.txtC.Name = "txtC";
			this.txtC.Size = new System.Drawing.Size(45, 19);
			this.txtC.TabIndex = 2;
			// 
			// txtH
			// 
			this.txtH.Location = new System.Drawing.Point(105, 90);
			this.txtH.Name = "txtH";
			this.txtH.Size = new System.Drawing.Size(45, 19);
			this.txtH.TabIndex = 3;
			this.txtH.TextChanged += new System.EventHandler(this.txtH_TextChanged);
			// 
			// btnOk
			// 
			this.btnOk.Font = new System.Drawing.Font("Verdana", 7F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.btnOk.Location = new System.Drawing.Point(105, 125);
			this.btnOk.Name = "btnOk";
			this.btnOk.Size = new System.Drawing.Size(80, 40);
			this.btnOk.TabIndex = 4;
			this.btnOk.Text = "OK";
			this.btnOk.Click += new System.EventHandler(this.btnOk_Click);
			// 
			// btnCancel
			// 
			this.btnCancel.Font = new System.Drawing.Font("Verdana", 7F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.btnCancel.Location = new System.Drawing.Point(190, 125);
			this.btnCancel.Name = "btnCancel";
			this.btnCancel.Size = new System.Drawing.Size(80, 40);
			this.btnCancel.TabIndex = 5;
			this.btnCancel.Text = "Cancel";
			this.btnCancel.Click += new System.EventHandler(this.btnCancel_Click);
			// 
			// CeilingCheckBox
			// 
			this.CeilingCheckBox.AutoSize = true;
			this.CeilingCheckBox.Checked = true;
			this.CeilingCheckBox.CheckState = System.Windows.Forms.CheckState.Checked;
			this.CeilingCheckBox.Font = new System.Drawing.Font("Verdana", 7F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.CeilingCheckBox.Location = new System.Drawing.Point(160, 90);
			this.CeilingCheckBox.Name = "CeilingCheckBox";
			this.CeilingCheckBox.Size = new System.Drawing.Size(81, 16);
			this.CeilingCheckBox.TabIndex = 9;
			this.CeilingCheckBox.Text = "wrt Ceiling";
			this.CeilingCheckBox.UseVisualStyleBackColor = true;
			this.CeilingCheckBox.Visible = false;
			// 
			// ChangeMapSizeForm
			// 
			this.AutoScaleBaseSize = new System.Drawing.Size(5, 12);
			this.ClientSize = new System.Drawing.Size(278, 172);
			this.ControlBox = false;
			this.Controls.Add(this.CeilingCheckBox);
			this.Controls.Add(this.btnCancel);
			this.Controls.Add(this.btnOk);
			this.Controls.Add(this.txtH);
			this.Controls.Add(this.txtC);
			this.Controls.Add(this.label5);
			this.Controls.Add(this.label4);
			this.Controls.Add(this.label3);
			this.Controls.Add(this.label2);
			this.Controls.Add(this.txtR);
			this.Controls.Add(this.oldC);
			this.Controls.Add(this.oldH);
			this.Controls.Add(this.oldR);
			this.Controls.Add(this.label1);
			this.Font = new System.Drawing.Font("Verdana", 7F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
			this.Name = "ChangeMapSizeForm";
			this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
			this.Text = "Change map";
			this.ResumeLayout(false);
			this.PerformLayout();
		}
		#endregion

		private void btnOk_Click(object sender, System.EventArgs e)
		{
			try
			{
				int.Parse(txtR.Text);
				int.Parse(txtC.Text);
				int.Parse(txtH.Text);

				DialogResult = DialogResult.OK;
				Close();
			}
			catch
			{
				MessageBox.Show(
							this,
							"Input must be whole numbers",
							"Error",
							MessageBoxButtons.OK,
							MessageBoxIcon.Exclamation);
			}
		}

		private void btnCancel_Click(object sender, System.EventArgs e)
		{
			Close();
		}

		private void txtH_TextChanged(object sender, EventArgs e)
		{
			int current;
			int.TryParse(txtH.Text, out current);
			CeilingCheckBox.Visible = (current != map.MapSize.Height);
		}
	}
}
