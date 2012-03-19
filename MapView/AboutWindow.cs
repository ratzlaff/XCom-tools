using System;
using System.Drawing;
using System.Collections;
using System.ComponentModel;
using System.Windows.Forms;
using ViewLib;

namespace MapView
{
	/// <summary>
	/// Displays the About box 
	/// </summary>
	public partial class AboutWindow : Map_Observer_Form
	{
		public AboutWindow()
		{
			InitializeComponent();

			string ver = System.Reflection.Assembly.GetExecutingAssembly().GetName().Version.Major+"."+System.Reflection.Assembly.GetExecutingAssembly().GetName().Version.Minor+"."+System.Reflection.Assembly.GetExecutingAssembly().GetName().Version.Build;

#if DEBUG
			lblVersion.Text = "MapView version " + ver + " Debug";
#else
			lblVersion.Text = "MapView version " + ver + " Release";
#endif
		}
	}
}
