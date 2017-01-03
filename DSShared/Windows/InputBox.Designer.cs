using System;
using System.Collections.Generic;
using System.Text;

namespace DSShared.Windows
{			
	partial class InputBox
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
			this.btnOk = new System.Windows.Forms.Button();
			this.btnCancel = new System.Windows.Forms.Button();
			this.lblCaption = new System.Windows.Forms.Label();
			this.txtInput = new System.Windows.Forms.TextBox();
			this.panel1 = new System.Windows.Forms.Panel();
			this.panelMid = new System.Windows.Forms.Panel();
			this.panel1.SuspendLayout();
			this.panelMid.SuspendLayout();
			this.SuspendLayout();
			// 
			// btnOk
			// 
			this.btnOk.Anchor = System.Windows.Forms.AnchorStyles.Top;
			this.btnOk.DialogResult = System.Windows.Forms.DialogResult.OK;
			this.btnOk.Location = new System.Drawing.Point(75, 0);
			this.btnOk.Name = "btnOk";
			this.btnOk.Size = new System.Drawing.Size(80, 25);
			this.btnOk.TabIndex = 0;
			this.btnOk.Text = "Ok";
			this.btnOk.Click += new System.EventHandler(this.buttonClick);
			// 
			// btnCancel
			// 
			this.btnCancel.Anchor = System.Windows.Forms.AnchorStyles.Top;
			this.btnCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
			this.btnCancel.Location = new System.Drawing.Point(162, 0);
			this.btnCancel.Name = "btnCancel";
			this.btnCancel.Size = new System.Drawing.Size(80, 25);
			this.btnCancel.TabIndex = 1;
			this.btnCancel.Text = "Cancel";
			this.btnCancel.Click += new System.EventHandler(this.buttonClick);
			// 
			// lblCaption
			// 
			this.lblCaption.Dock = System.Windows.Forms.DockStyle.Top;
			this.lblCaption.Location = new System.Drawing.Point(0, 0);
			this.lblCaption.Name = "lblCaption";
			this.lblCaption.Size = new System.Drawing.Size(317, 25);
			this.lblCaption.TabIndex = 2;
			// 
			// txtInput
			// 
			this.txtInput.Dock = System.Windows.Forms.DockStyle.Top;
			this.txtInput.Location = new System.Drawing.Point(0, 25);
			this.txtInput.Name = "txtInput";
			this.txtInput.Size = new System.Drawing.Size(317, 19);
			this.txtInput.TabIndex = 3;
			this.txtInput.KeyDown += new System.Windows.Forms.KeyEventHandler(this.txtInput_KeyDown);
			// 
			// panel1
			// 
			this.panel1.Controls.Add(this.btnOk);
			this.panel1.Controls.Add(this.btnCancel);
			this.panel1.Dock = System.Windows.Forms.DockStyle.Bottom;
			this.panel1.Location = new System.Drawing.Point(0, 50);
			this.panel1.Name = "panel1";
			this.panel1.Size = new System.Drawing.Size(317, 30);
			this.panel1.TabIndex = 4;
			// 
			// panelMid
			// 
			this.panelMid.Controls.Add(this.txtInput);
			this.panelMid.Controls.Add(this.lblCaption);
			this.panelMid.Dock = System.Windows.Forms.DockStyle.Fill;
			this.panelMid.Location = new System.Drawing.Point(0, 0);
			this.panelMid.Name = "panelMid";
			this.panelMid.Size = new System.Drawing.Size(317, 50);
			this.panelMid.TabIndex = 5;
			// 
			// InputBox
			// 
			this.AutoScaleBaseSize = new System.Drawing.Size(5, 12);
			this.ClientSize = new System.Drawing.Size(317, 80);
			this.Controls.Add(this.panelMid);
			this.Controls.Add(this.panel1);
			this.Font = new System.Drawing.Font("Verdana", 7F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.MaximizeBox = false;
			this.MinimizeBox = false;
			this.Name = "InputBox";
			this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
			this.panel1.ResumeLayout(false);
			this.panelMid.ResumeLayout(false);
			this.panelMid.PerformLayout();
			this.ResumeLayout(false);
		}
		#endregion

		private System.Windows.Forms.Button btnOk;
		private System.Windows.Forms.Button btnCancel;
		private System.Windows.Forms.Label lblCaption;
		private System.Windows.Forms.TextBox txtInput;
		private System.Windows.Forms.Panel panel1;
		private System.Windows.Forms.Panel panelMid;
	}		
}
