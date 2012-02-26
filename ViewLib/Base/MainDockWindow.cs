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

namespace ViewLib.Base
{
	public partial class MainDockWindow : Form
	{
		private List<Base.ToolWindow> toolsToLoad;
		private Dictionary<Base.ToolWindow, WeifenLuo.WinFormsUI.Docking.DockState> toolState;

		public string LayoutFile { get; set; }
		public DockPanel DockPanel { get; set; }

		public MainDockWindow()
		{
			InitializeComponent();

			toolsToLoad = new List<ViewLib.Base.ToolWindow>();
			toolState = new Dictionary<ViewLib.Base.ToolWindow, DockState>();
		}

		public void RegisterForm(Base.ToolWindow window, DockState state)
		{
			toolsToLoad.Add(window);
			toolState.Add(window, state);
		}

		private IDockContent getWindow(string window)
		{
			Base.ToolWindow rval = null;
			foreach (Base.ToolWindow tWin in toolsToLoad) {
				if (tWin.GetType().ToString() == window)
					rval = tWin;
			}

			if (rval != null)
				toolsToLoad.Remove(rval);

			return rval;
		}

		private void MainDockWindow_Load(object sender, EventArgs e)
		{
			if (File.Exists(LayoutFile))
				DockPanel.LoadFromXml(LayoutFile, new WeifenLuo.WinFormsUI.Docking.DeserializeDockContent(getWindow));

			foreach (Base.ToolWindow tw in toolsToLoad)
				tw.Show(DockPanel, toolState[tw]);
		}

		private void MainDockWindow_FormClosing(object sender, FormClosingEventArgs e)
		{
			DockPanel.SaveAsXml(LayoutFile);
		}
	}
}
