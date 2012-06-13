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

namespace ViewLib
{
	public partial class MainDockWindow : Form
	{
		private List<DockContent> toolsToLoad;
		private Dictionary<DockContent, DockState> initialState;

		public string LayoutFile { get; set; }
		public DockPanel DockPanel { get; set; }

		public MainDockWindow()
		{
			InitializeComponent();
			toolsToLoad = new List<DockContent>();
			initialState = new Dictionary<DockContent, DockState>();
		}

		public void RegisterDockForm(DockContent window, DockState state)
		{
			toolsToLoad.Add(window);
			initialState[window] = state;
//			window.DockAreas = DockAreas.DockBottom | DockAreas.DockLeft | DockAreas.DockRight | DockAreas.DockTop | DockAreas.Float;
		}

		private IDockContent getWindow(string window)
		{
			DockContent rval = null;
			foreach (DockContent tWin in toolsToLoad) {
				if (tWin.GetType().ToString() == window)
					rval = tWin;
			}

			if (rval != null)
				toolsToLoad.Remove(rval);

			return rval;
		}

		protected override void OnLoad(EventArgs e)
		{
			base.OnLoad(e);

			if (File.Exists(LayoutFile))
				try {
					DockPanel.LoadFromXml(LayoutFile, getWindow);
				} catch (Exception ex) {
					Console.WriteLine("Could not load layout file: " + ex.ToString());
				}

			// show new forms that dont exist in the LayoutFile
			foreach (DockContent tw in toolsToLoad)
				tw.Show(DockPanel, initialState[tw]);
		}

		protected override void OnClosing(CancelEventArgs e)
		{
			base.OnClosing(e);

			if (DockPanel != null)
				DockPanel.SaveAsXml(LayoutFile);
		}
	}
}
