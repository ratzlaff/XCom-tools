using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using DSShared.Windows;
using XCom.Interfaces.Base;

namespace XCom
{
	public partial class ConsoleForm : Form
	{
		public ConsoleForm()
		{
			InitializeComponent();

			XCom.xConsole.Init(100);
			XCom.xConsole.BufferChanged += xConsole_BufferChanged;

			new RegistryInfo(this);

			SetStyle(ControlStyles.OptimizedDoubleBuffer | ControlStyles.AllPaintingInWmPaint, true);
		}

		private void xConsole_BufferChanged(Node current)
		{
			
			string buffer = current.str+"\n";
			Node curr = current.next;

			while (current != curr)
			{
				buffer = buffer + curr.str + "\n";
				curr = curr.next;
			}

			consoleText.Text = buffer;
			Refresh();
		}

		private void miClose_Click(object sender, EventArgs e)
		{
			Close();
		}
	}
}
