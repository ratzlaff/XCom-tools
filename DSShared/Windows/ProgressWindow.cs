using System;
using System.Drawing;
using System.Collections;
using System.ComponentModel;
using System.Windows.Forms;

namespace DSShared.Windows
{
	/// <summary>
	/// </summary>
	public class ProgressWindow
		:
		System.Windows.Forms.Form
	{
		private System.Windows.Forms.ProgressBar progress;
		private System.ComponentModel.Container components = null;
		private Form parent;

		/// <summary>
		/// </summary>
		public ProgressWindow(Form parent)
		{
			InitializeComponent();

			if (parent != null)
			{
				this.parent = parent;

				Left = parent.Left + (parent.Width - Width) / 2;
				Top = parent.Top + (parent.Height - Height) / 2;
			}
		}

		/// <summary>
		/// </summary>
		public new void Show()
		{
			if (parent != null)
				parent.Enabled = false;

			base.Show();
		}

		/// <summary>
		/// </summary>
		public new void Hide()
		{
			if (parent != null)
				parent.Enabled = true;

			base.Hide();
		}

		/// <summary>
		/// </summary>
		public int Maximum
		{
			get { return progress.Maximum; }
			set { progress.Maximum = value; }
		}

		/// <summary>
		/// </summary>
		public int Value
		{
			get { return progress.Value; }
			set { progress.Value = value; }
		}

		/// <summary>
		/// </summary>
		public int Minimum
		{
			get { return progress.Minimum; }
			set { progress.Minimum = value; }
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
			this.progress = new System.Windows.Forms.ProgressBar();
			this.SuspendLayout();
			// 
			// progress
			// 
			this.progress.Dock = System.Windows.Forms.DockStyle.Fill;
			this.progress.Location = new System.Drawing.Point(0, 0);
			this.progress.Name = "progress";
			this.progress.Size = new System.Drawing.Size(320, 30);
			this.progress.TabIndex = 0;
			// 
			// ProgressWindow
			// 
			this.AutoScaleBaseSize = new System.Drawing.Size(5, 12);
			this.ClientSize = new System.Drawing.Size(320, 30);
			this.ControlBox = false;
			this.Controls.Add(this.progress);
			this.Font = new System.Drawing.Font("Verdana", 7F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
			this.Name = "ProgressWindow";
			this.ShowInTaskbar = false;
			this.StartPosition = System.Windows.Forms.FormStartPosition.Manual;
			this.Text = "Progress";
			this.ResumeLayout(false);
		}

		#endregion
	}
}
