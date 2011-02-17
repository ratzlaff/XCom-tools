namespace XCom
{
	partial class ConsoleForm
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
			this.consoleText = new System.Windows.Forms.RichTextBox();
			this.SuspendLayout();
			// 
			// consoleText
			// 
			this.consoleText.Dock = System.Windows.Forms.DockStyle.Fill;
			this.consoleText.Location = new System.Drawing.Point(0, 0);
			this.consoleText.Name = "consoleText";
			this.consoleText.Size = new System.Drawing.Size(292, 266);
			this.consoleText.TabIndex = 0;
			this.consoleText.Text = "";
			// 
			// ConsoleForm
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.ClientSize = new System.Drawing.Size(292, 266);
			this.Controls.Add(this.consoleText);
			this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.SizableToolWindow;
			this.Name = "ConsoleForm";
			this.ShowInTaskbar = false;
			this.Text = "Output Console";
			this.ResumeLayout(false);

		}

		#endregion

		private System.Windows.Forms.RichTextBox consoleText;
	}
}