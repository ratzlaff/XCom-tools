using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using WeifenLuo.WinFormsUI.Docking;
using System.IO;

namespace ViewLib.MainForms
{
	public partial class MainMapView : Base.MainDockWindow
	{
		public MainMapView()
		{
			InitializeComponent();

			RegisterForm(new ToolForms.MapList(), DockState.DockLeft);
			RegisterForm(new ToolForms.MapView(), DockState.Document);

			LayoutFile = Path.Combine(Path.GetDirectoryName(Application.UserAppDataPath), "layout.xml");
			DockPanel = dockPanel;
		}

		private void MainMapView_FormClosing(object sender, FormClosingEventArgs e)
		{

		}

		private void MainMapView_Load(object sender, EventArgs e)
		{

		}
	}
}
