using System;
using System.Drawing;
using System.Collections;
using System.ComponentModel;
using System.Windows.Forms;

namespace MapView
{
	public class ChoiceDialog
		:
		System.Windows.Forms.Form
	{
		private System.Windows.Forms.Label txt;
		private System.Windows.Forms.Button overwrite;
		private System.Windows.Forms.Button cancel;
		private System.Windows.Forms.Button exist;
		private System.ComponentModel.Container components = null;

		private Choice choice = Choice.Cancel;

		public ChoiceDialog(string file)
		{
			InitializeComponent();
			txt.Text = "The file " + file + " already exists. Do you want to overwrite the existing file, use the existing file, or cancel";
		}

		public Choice Choice
		{
			get { return choice; }
		}

		#region Windows Form Designer generated code
		
		/// <summary>
		/// Clean up any resources being used.
		/// </summary>
		protected override void Dispose( bool disposing )
		{
			if( disposing )
			{
				if(components != null)
				{
					components.Dispose();
				}
			}
			base.Dispose( disposing );
		}
		
		/// <summary>
		/// Required method for Designer support - do not modify
		/// the contents of this method with the code editor.
		/// </summary>
		private void InitializeComponent()
		{
			this.txt = new System.Windows.Forms.Label();
			this.overwrite = new System.Windows.Forms.Button();
			this.cancel = new System.Windows.Forms.Button();
			this.exist = new System.Windows.Forms.Button();
			this.SuspendLayout();
			// 
			// txt
			// 
			this.txt.Dock = System.Windows.Forms.DockStyle.Top;
			this.txt.Location = new System.Drawing.Point(0, 0);
			this.txt.Name = "txt";
			this.txt.Size = new System.Drawing.Size(292, 45);
			this.txt.TabIndex = 0;
			// 
			// overwrite
			// 
			this.overwrite.Location = new System.Drawing.Point(20, 45);
			this.overwrite.Name = "overwrite";
			this.overwrite.Size = new System.Drawing.Size(80, 30);
			this.overwrite.TabIndex = 1;
			this.overwrite.Text = "Overwrite";
			this.overwrite.Click += new System.EventHandler(this.overwrite_Click);
			// 
			// cancel
			// 
			this.cancel.Font = new System.Drawing.Font("Verdana", 7F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.cancel.Location = new System.Drawing.Point(190, 45);
			this.cancel.Name = "cancel";
			this.cancel.Size = new System.Drawing.Size(80, 30);
			this.cancel.TabIndex = 2;
			this.cancel.Text = "Cancel";
			this.cancel.Click += new System.EventHandler(this.cancel_Click);
			// 
			// exist
			// 
			this.exist.Location = new System.Drawing.Point(105, 45);
			this.exist.Name = "exist";
			this.exist.Size = new System.Drawing.Size(80, 30);
			this.exist.TabIndex = 3;
			this.exist.Text = "Existing";
			this.exist.Click += new System.EventHandler(this.exist_Click);
			// 
			// ChoiceDialog
			// 
			this.AutoScaleBaseSize = new System.Drawing.Size(5, 12);
			this.ClientSize = new System.Drawing.Size(292, 75);
			this.Controls.Add(this.exist);
			this.Controls.Add(this.cancel);
			this.Controls.Add(this.overwrite);
			this.Controls.Add(this.txt);
			this.Font = new System.Drawing.Font("Verdana", 7F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.MaximizeBox = false;
			this.MinimizeBox = false;
			this.Name = "ChoiceDialog";
			this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
			this.Text = "Decisions Decisions";
			this.ResumeLayout(false);
		}
		#endregion

		private void overwrite_Click(object sender, System.EventArgs e)
		{
			choice = Choice.Overwrite;
			Close();
		}

		private void exist_Click(object sender, System.EventArgs e)
		{
			choice = Choice.UseExisting;
			Close();
		}

		private void cancel_Click(object sender, System.EventArgs e)
		{
			choice = Choice.Cancel;
			Close();
		}
	}
}
