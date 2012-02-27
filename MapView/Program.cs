using System;
using System.Collections.Generic;
using System.Windows.Forms;
using ViewLib.MainForms;

namespace MapView
{
	static class Program
	{
		/// <summary>
		/// The main entry point for the application.
		/// </summary>
		[STAThread]
		static void Main()
		{
			Application.EnableVisualStyles();
			Application.SetCompatibleTextRenderingDefault(false);
			//Application.Run(new MainWindow());
			new MainWindow().Show();
			Application.Run(new MainMapView());
		}
	}
}