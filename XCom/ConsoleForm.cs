using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using UtilLib.Windows;
using ViewLib.Base;

namespace XCom
{
	public partial class ConsoleForm : ToolWindow
	{
		public ConsoleForm()
		{
			InitializeComponent();

			XCom.xConsole.Init(100);
			XCom.xConsole.BufferChanged += new BufferChangedDelegate(xConsole_BufferChanged);

			new RegistryInfo(this);

			SetStyle(ControlStyles.OptimizedDoubleBuffer | ControlStyles.AllPaintingInWmPaint, true);
		}

		void xConsole_BufferChanged(xConsole.Node current)
		{
			string buffer = current.Str + "\n";
			xConsole.Node curr = current.Next;

			while (current != curr) {
				buffer = buffer + curr.Str + "\n";
				curr = curr.Next;
			}

			consoleText.Text = buffer;
			Refresh();
		}
	}
}