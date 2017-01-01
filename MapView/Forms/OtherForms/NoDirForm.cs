using System;
using System.Drawing;
using System.Collections;
using System.ComponentModel;
using System.Windows.Forms;
using System.IO;

namespace MapView
{
	/// <summary>
	/// Summary description for NoDirForm.
	/// </summary>
	public class NoDirForm
		:
		System.Windows.Forms.Form
	{
		private System.Windows.Forms.Label label1;
		private System.Windows.Forms.Button button1;
		private System.Windows.Forms.Button button2;
		/// <summary>
		/// Required designer variable.
		/// </summary>
		private System.ComponentModel.Container components = null;

		private static readonly NoDirForm f = new NoDirForm();
		private static DialogResult result = DialogResult.Cancel;

		private static string dir;

		public NoDirForm()
		{
			InitializeComponent();
		}

		/// <summary>
		/// Clean up any resources being used.
		/// </summary>
		protected override void Dispose( bool disposing )
		{
			if (disposing && components != null)
				components.Dispose();

			base.Dispose( disposing );
		}

		public static DialogResult Show(string directory)
		{
			dir = directory;
			f.label1.Text = "Directory " + dir + " not found";
			f.ShowDialog();
			return result;
		}

		#region Windows Form Designer generated code
		/// <summary>
		/// Required method for Designer support - do not modify
		/// the contents of this method with the code editor.
		/// </summary>
		private void InitializeComponent()
		{
			this.label1 = new System.Windows.Forms.Label();
			this.button1 = new System.Windows.Forms.Button();
			this.button2 = new System.Windows.Forms.Button();
			this.SuspendLayout();
			// 
			// label1
			// 
			this.label1.Dock = System.Windows.Forms.DockStyle.Top;
			this.label1.Location = new System.Drawing.Point(0, 0);
			this.label1.Name = "label1";
			this.label1.Size = new System.Drawing.Size(444, 25);
			this.label1.TabIndex = 0;
			// 
			// button1
			// 
			this.button1.Location = new System.Drawing.Point(125, 25);
			this.button1.Name = "button1";
			this.button1.Size = new System.Drawing.Size(90, 25);
			this.button1.TabIndex = 1;
			this.button1.Text = "Create";
			this.button1.Click += new System.EventHandler(this.button1_Click);
			// 
			// button2
			// 
			this.button2.Location = new System.Drawing.Point(230, 25);
			this.button2.Name = "button2";
			this.button2.Size = new System.Drawing.Size(85, 25);
			this.button2.TabIndex = 2;
			this.button2.Text = "Cancel";
			this.button2.Click += new System.EventHandler(this.button2_Click);
			// 
			// NoDirForm
			// 
			this.AutoScaleBaseSize = new System.Drawing.Size(5, 12);
			this.ClientSize = new System.Drawing.Size(444, 52);
			this.Controls.Add(this.button2);
			this.Controls.Add(this.button1);
			this.Controls.Add(this.label1);
			this.Font = new System.Drawing.Font("Verdana", 7F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
			this.Name = "NoDirForm";
			this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
			this.Text = "Directory not found";
			this.ResumeLayout(false);
		}
		#endregion

		private void button1_Click(object sender, System.EventArgs e)
		{
			Directory.CreateDirectory(dir);
			result = DialogResult.OK;
			this.Close();
		}

		private void button2_Click(object sender, System.EventArgs e)
		{
			result = DialogResult.Cancel;
			Close();
		}
	}
}
