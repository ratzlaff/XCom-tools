#define ORIGINAL

using System;
using System.Collections.Generic;
using System.Windows.Forms;

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
#if ORIGINAL
			Application.Run(new MainWindow());
#else
			new MainWindow().Show();
			MainMapView mmv = new MainMapView();
			mmv.RegisterForm(TopViewForm.TopView.Instance, DockState.DockRight);
			mmv.RegisterForm(TileView.Instance, DockState.DockBottom);
//			mmv.RegisterForm(new MainWindow(), DockState.DockLeft);
			Application.Run(mmv);
#endif
		}
	}
}