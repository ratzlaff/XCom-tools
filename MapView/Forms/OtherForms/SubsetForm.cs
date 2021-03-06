using System;
using System.Drawing;
using System.Collections;
using System.ComponentModel;
using System.Windows.Forms;

namespace MapView
{
	public class SubsetForm
		:
		System.Windows.Forms.Form
	{
		private System.Windows.Forms.Label label1;
		private System.Windows.Forms.TextBox txtName;
		private System.Windows.Forms.Button btnOk;
		private System.ComponentModel.Container components = null;

		private string name;

		public SubsetForm()
		{
			InitializeComponent();
			name = null;
		}

		public string SubsetName
		{
			get { return name; }
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
			this.label1 = new System.Windows.Forms.Label();
			this.txtName = new System.Windows.Forms.TextBox();
			this.btnOk = new System.Windows.Forms.Button();
			this.SuspendLayout();
			// 
			// label1
			// 
			this.label1.Location = new System.Drawing.Point(5, 5);
			this.label1.Name = "label1";
			this.label1.Size = new System.Drawing.Size(80, 15);
			this.label1.TabIndex = 0;
			this.label1.Text = "Subset Name";
			// 
			// txtName
			// 
			this.txtName.Location = new System.Drawing.Point(0, 20);
			this.txtName.Name = "txtName";
			this.txtName.Size = new System.Drawing.Size(145, 19);
			this.txtName.TabIndex = 1;
			// 
			// btnOk
			// 
			this.btnOk.Location = new System.Drawing.Point(30, 45);
			this.btnOk.Name = "btnOk";
			this.btnOk.Size = new System.Drawing.Size(80, 25);
			this.btnOk.TabIndex = 2;
			this.btnOk.Text = "Ok";
			this.btnOk.Click += new System.EventHandler(this.btnOk_Click);
			// 
			// SubsetForm
			// 
			this.AutoScaleBaseSize = new System.Drawing.Size(5, 12);
			this.ClientSize = new System.Drawing.Size(144, 72);
			this.Controls.Add(this.btnOk);
			this.Controls.Add(this.txtName);
			this.Controls.Add(this.label1);
			this.Font = new System.Drawing.Font("Verdana", 7F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
			this.MaximizeBox = false;
			this.MinimizeBox = false;
			this.Name = "SubsetForm";
			this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
			this.Text = "SubsetForm";
			this.ResumeLayout(false);
			this.PerformLayout();
		}
		#endregion

		private void btnOk_Click(object sender, System.EventArgs e)
		{
			name = txtName.Text;
			Close();
		}
	}
}
