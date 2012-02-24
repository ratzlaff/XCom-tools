using System;
using System.Collections.Generic;
using System.Windows.Forms;
using System.Reflection;
using System.Threading;

namespace MapView
{
	public class Program : Startup<MainWindow>
	{
		[STAThread]
		public static void Main()
		{
			Application.EnableVisualStyles();
			Application.SetCompatibleTextRenderingDefault(false);
			Application.Run(new MainWindow());
		}
	}
}