using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using System.Reflection;
using System.IO;

/*
 * $Id$ 
 */

namespace XCSuite
{
	public partial class _Main : Form
	{
		private RunProgram curr;
		public _Main()
		{
			InitializeComponent();

			foreach (string s in Directory.GetFiles(Environment.CurrentDirectory, "*.dll"))
				loadAssembly(s);

			RunProgram rpSuite = new RunProgram();

			rpSuite.Assembly = Assembly.GetExecutingAssembly();
			rpSuite.Dock = DockStyle.Top;
			rpSuite.RunClick+=new EventHandler(runClick);
			contentPane.Controls.Add(rpSuite);

			foreach (string s in Directory.GetFiles(Environment.CurrentDirectory, "*.exe"))
				loadAssembly(s);
		}

		private void loadAssembly(string asm)
		{
			if (curr == null)
				curr = new RunProgram();

			if (curr.LoadAssembly(asm))
			{
				curr.Dock = DockStyle.Top;
				contentPane.Controls.Add(curr);
				curr.RunClick += new EventHandler(runClick);

				curr = null;
			}
		}

		private void runClick(object sender, EventArgs e)
		{
			RunProgram rp = (RunProgram)sender;

			System.Diagnostics.Process.Start(rp.AsmPath);
			Console.WriteLine("Start program: " + rp.AsmPath);
			//Application.ExitThread();
		}
	}
}