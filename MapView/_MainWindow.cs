using System;
using System.Drawing;
using System.ComponentModel;
using System.Windows.Forms;
using System.Data;
using XCom;
using XCom.Interfaces;
using System.Drawing.Drawing2D;
using System.IO;
using Microsoft.Win32;
using System.Reflection;
using UtilLib;
using UtilLib.Windows;
using UtilLib.Loadable;
using XCom.Interfaces.Base;
using System.Collections.Generic;
using MapView.RmpViewForm;
using MapView.TopViewForm;
using MapLib;
using WeifenLuo.WinFormsUI.Docking;
using ViewLib;

namespace MapView
{
	public delegate void StringDelegate(object sender, string args);

	public partial class MainWindow : MainDockWindow
	{
//		private MapView.MapViewPanel mapView;
		private LoadingForm lf;
		private Dictionary<string, Map_Observer_Form> registeredForms;
		private Dictionary<string, Settings> registeredSettings;
		private Settings settings;

		public MainWindow()
		{
			#region Setup SharedSpace information and paths
			SharedSpace sharedSpace = SharedSpace.Instance;
			sharedSpace.GetObj("MapView", this);
			sharedSpace.GetObj("AppDir", Environment.CurrentDirectory);
			sharedSpace.GetObj("CustomDir", Environment.CurrentDirectory + "\\custom");
			sharedSpace.GetObj("SettingsDir", Environment.CurrentDirectory + "\\settings");

			PathInfo pathsFile = new PathInfo(SharedSpace.Instance.GetString("SettingsDir"), "Paths", "pth");
			PathInfo settingsFile = new PathInfo(SharedSpace.Instance.GetString("SettingsDir"), "MVSettings", "dat");
			PathInfo mapeditFile = new PathInfo(SharedSpace.Instance.GetString("SettingsDir"), "MapEdit", "dat");
			PathInfo imagesFile = new PathInfo(SharedSpace.Instance.GetString("SettingsDir"), "Images", "dat");
			PathInfo layoutFile = new PathInfo(SharedSpace.Instance.GetString("SettingsDir"), "Layout", "xml");

			sharedSpace.GetObj("MV_PathsFile", pathsFile);
			sharedSpace.GetObj("MV_SettingsFile", settingsFile);
			sharedSpace.GetObj("MV_MapEditFile", mapeditFile);
			sharedSpace.GetObj("MV_ImagesFile", imagesFile);
			sharedSpace.GetObj("MV_LayoutFile", layoutFile);
			#endregion

			if (!pathsFile.Exists()) {
				InstallWindow iw = new InstallWindow();

				if (iw.ShowDialog(this) != DialogResult.OK)
					Environment.Exit(-1);
			}

			GameInfo.ParseLine += new ParseLineDelegate(parseLine);
			GameInfo.Init(XCPalette.TFTDBattle, pathsFile);
			xConsole.AddLine("GameInfo.Init done");

			XCPalette.TFTDBattle.SetTransparent(true);
			XCPalette.UFOBattle.SetTransparent(true);
			XCPalette.TFTDBattle.Grayscale.SetTransparent(true);
			XCPalette.UFOBattle.Grayscale.SetTransparent(true);

			xConsole.AddLine("Palette transparencies set");

			/***********************/
			InitializeComponent();
			/***********************/

			DockPanel = dockPanel;
			LayoutFile = SharedSpace.Instance["MV_LayoutFile"].ToString();

			MakeToolstrip(tools);
			tools.Items.Add(new ToolStripSeparator());

			xConsole.AddLine("Main view window created");

			SetupDefaultSettings();

			registeredForms = new Dictionary<string, Map_Observer_Form>();
			registeredSettings = new Dictionary<string, Settings>();
			registeredSettings["MainWindow"] = settings;

			registerForm(MapList.Instance, showMenu, DockState.DockLeft);
			registerForm(TopView.Instance, showMenu, DockState.Float);
			registerForm(TileView.Instance, showMenu, DockState.DockRight);
			registerForm(RmpView.Instance, showMenu, DockState.Float);
			registerForm(MapViewTool.Instance, showMenu, DockState.Document);

//			if (XCom.Globals.UseBlanks)
//				registerForm(mapView.BlankForm, showMenu);

//			addWindow(xConsole.Instance,showMenu);
//			((MenuItem)windowMI[xConsole.Instance]).PerformClick();

			registerForm(new HelpScreen(), miHelp, DockState.Float);
			registerForm(new AboutWindow(), miHelp, DockState.Float);
			xConsole.AddLine("Quick help and About created");

			if (settingsFile.Exists()) {
				readMapViewSettings(new StreamReader(settingsFile.ToString()));
				xConsole.AddLine("User settings loaded");
			} else
				xConsole.AddLine("User settings NOT loaded - no settings file to load");

			lf = new LoadingForm();
			XCom.Bmp.LoadingEvent += new LoadingDelegate(lf.Update);

			//I should rewrite the hq2x wrapper for .net sometime (not the code, its pretty insane)
			//if(!File.Exists("hq2xa.dll"))
			miHq.Visible = false;
			//LogFile.Instance.WriteLine("Loading user-made plugins");

			/****************************************/
			//Copied from pckview
			//loadedTypes = new LoadOfType<IMapDesc>();
			//sharedSpace["MapMods"] = loadedTypes.AllLoaded;

			//There are no currently loadable maps in this assembly so this is more for future use
			//loadedTypes.LoadFrom(Assembly.GetAssembly(typeof(XCom.Interfaces.Base.IMapDesc)));

			//if (Directory.Exists(sharedSpace["CustomDir"].ToString()))
			//{
			//	xConsole.AddLine("Custom directory exists: " + sharedSpace["CustomDir"].ToString());
			//	foreach (string s in Directory.GetFiles(sharedSpace["CustomDir"].ToString()))
			//		if (s.EndsWith(".dll"))
			//		{
			//			xConsole.AddLine("Loading dll: " + s);
			//			loadedTypes.LoadFrom(Assembly.LoadFrom(s));
			//		}
			//}
			/****************************************/

			xConsole.AddLine("About to show window");
			Show();
		}

		private void registerForm(Map_Observer_Form f, MenuItem parent, DockState initialState)
		{
			parent.MenuItems.Add(f.MenuItem);
			f.SetupDefaultSettings();

			registeredSettings.Add(f.Name, f.Settings);
			registeredForms.Add(f.Name, f);
			RegisterDockForm(f, initialState);
		}

		private void parseLine(XCom.KeyVal line, XCom.VarCollection vars)
		{
			switch (line.Keyword.ToLower()) {
				case "cursor":
					if (line.Rest.EndsWith("\\"))
						SharedSpace.Instance.GetObj("cursorFile", line.Rest + "CURSOR");
					else
						SharedSpace.Instance.GetObj("cursorFile", line.Rest + "\\CURSOR");
					break;
				case "logfile":
					try {
						bool lineBool = false;
						if (bool.TryParse(line.Rest, out lineBool))
							xConsole.LogToFile("console.log");
						else
							xConsole.LogToFile(line.Rest);
					} catch {
						Console.WriteLine("Could not parse logfile line");
					}
					break;
			}
		}

		private void readMapViewSettings(StreamReader sr)
		{
			UtilLib.Parser.VarCollection vc = new UtilLib.Parser.VarCollection(sr);
			UtilLib.Parser.KeyVal kv = null;

			while ((kv = vc.ReadLine()) != null) {
				try {
					Settings.ReadSettings(vc, kv, registeredSettings[kv.Keyword]);
				} catch { }
			}

			sr.Close();
		}

		private Dictionary<System.Reflection.PropertyInfo, object> cachedProperties;

		public bool CacheProperty(System.Reflection.PropertyInfo inProp, object val)
		{
			if (cachedProperties == null)
				cachedProperties = new Dictionary<PropertyInfo, object>();

			if (inProp.Name == "Width") {
				cachedProperties.Add(inProp, val);
				return true;
			}

			if (inProp.Name == "Height") {
				cachedProperties.Add(inProp, val);
				return true;
			}

			return false;
		}

		private void changeSetting(object sender, string key, object val)
		{
			settings[key].Value = val;
			switch (key) {
				case "Animation":
					bool animVal = (bool)val;
					onItem.Checked = animVal;
					offItem.Checked = !animVal;

					if ((bool)val)
						MapViewScroller.Start();
					else
						MapViewScroller.Stop();
					break;
				case "Doors":
					if (MapControl.Current != null)
						foreach (MapLib.Base.Tile t in MapControl.Current.Tiles)
							t.Animate((bool)val);					
					break;
				case "SaveWindowPositions":
					PathsEditor.SaveRegistry = (bool)val;
					break;
/*
				case "UseGrid":
					MapViewPanel.Instance.View.UseGrid = (bool)val;
					break;
				case "GridColor":
					MapViewPanel.Instance.View.GridColor = (Color)val;
					break;
				case "GridLineColor":
					MapViewPanel.Instance.View.GridLineColor = (Color)val;
					break;
				case "GridLineWidth":
					MapViewPanel.Instance.View.GridLineWidth = (int)val;
					break;
*/
			}
		}

		protected override void OnLoad(EventArgs e)
		{
			base.OnLoad(e);

			foreach (System.Reflection.PropertyInfo pi in cachedProperties.Keys)
				pi.SetValue(this, cachedProperties[pi], new object[] { });
		}

		protected override void OnFormClosing(FormClosingEventArgs e)
		{
			base.OnFormClosing(e);

			if (NotifySave() == DialogResult.Cancel) {
				e.Cancel = true;
			} else {
				StreamWriter sw = new StreamWriter(SharedSpace.Instance["MV_SettingsFile"].ToString());
				foreach (string s in registeredSettings.Keys)
					registeredSettings[s].Save(s, sw);

				sw.Flush();
				sw.Close();
			}
		}

		private void quititem_Click(object sender, System.EventArgs e)
		{
			OnFormClosing(new FormClosingEventArgs(CloseReason.ApplicationExitCall, false));
			Environment.Exit(0);
		}

		public void SetupDefaultSettings()
		{
			settings = new Settings();

			settings.AddSetting("Animation", MapViewScroller.Updating, "If true, the map will animate itself", "Main", changeSetting);
			settings.AddSetting("Doors", false, "If true, the door tiles will animate themselves", "Main", changeSetting);
			//settings.AddSetting("SaveWindowPositions", PathsEditor.SaveRegistry, "If true, the window positions and sizes will be saved in the windows registry", "Main", changeSetting);
			Setting s = settings.AddSetting("X", "Starting X-coordinate of the window", "Window", "Left", this);
			s.IsVisible = false;

			s = settings.AddSetting("Y", "Starting Y-coordinate of the window", "Window", "Top", this);
			s.IsVisible = false;

			s = settings.AddSetting("Width", "Starting Width of the window", "Window", "Width", this);
			s.IsVisible = false;

			s = settings.AddSetting("Height", "Starting Height of the window", "Window", "Height", this);
			s.IsVisible = false;

			xConsole.AddLine("Default settings loaded");
		}
		
		private void onItem_Click(object sender, System.EventArgs e)
		{
			changeSetting(this, "Animation", true);
		}

		private void offItem_Click(object sender, System.EventArgs e)
		{
			changeSetting(this, "Animation", false);
		}

		private void saveItem_Click(object sender, System.EventArgs e)
		{
			if (MapControl.Current != null) {
				MapControl.Current.Save();
				xConsole.AddLine("Saved: " + MapControl.Current.Name);
				Globals.MapChanged = false;
			}
		}

		private void miPaths_Click(object sender, System.EventArgs e)
		{
			PathsEditor p = new PathsEditor(SharedSpace.Instance["MV_PathsFile"].ToString());
			p.ShowDialog();

//			GameInfo.Init(XCPalette.TFTDBattle, (PathInfo)SharedSpace.Instance["MV_PathsFile"]);
//			initList();
		}

		public DialogResult NotifySave()
		{
			if (MapControl.Current != null && Globals.MapChanged)
				switch (MessageBox.Show(this, "Map changed, do you wish to save?", "Save map?", MessageBoxButtons.YesNoCancel, MessageBoxIcon.Question, MessageBoxDefaultButton.Button1)) {
					case DialogResult.No: //dont save 
						break;
					case DialogResult.Yes: //save
						MapControl.Current.Save();
						break;
					case DialogResult.Cancel://do nothing
						return DialogResult.Cancel;
				}

			Globals.MapChanged = false;
			return DialogResult.OK;
		}

		private void miOptions_Click(object sender, System.EventArgs e)
		{
			PropertyForm pf = new PropertyForm("MainViewSettings", settings);
			pf.Text = "MainWindow Options";
			pf.Show();
		}

		private void miSaveImage_Click(object sender, System.EventArgs e)
		{
			throw new NotImplementedException();
/*
			if (mapView.Map != null) {
				saveFile.FileName = mapView.Map.Name;
				if (saveFile.ShowDialog() == DialogResult.OK) {
					lf.Show();
					mapView.Map.SaveGif(saveFile.FileName);
					lf.Hide();
				}
			}
*/
		}

		private void miHq_Click(object sender, System.EventArgs e)
		{
			if (MapControl.Current is XCMapFile) {
//				((XCMapFile)MapControl.Current).Hq2x();
//				mapView.View.Resize();
			}
		}

		private void miDoors_Click(object sender, System.EventArgs e)
		{
			miDoors.Checked = !miDoors.Checked;

			foreach (MapLib.Base.Tile t in MapControl.Current.Tiles)
				t.Animate(miDoors.Checked);
		}

		private void miResize_Click(object sender, System.EventArgs e)
		{
			ChangeMapSizeForm cmf = new ChangeMapSizeForm();
			cmf.Map = MapControl.Current;
			if (cmf.ShowDialog(this) == DialogResult.OK) {
				cmf.Map.ResizeTo(cmf.NewRows, cmf.NewCols, cmf.NewHeight);
//				mapView.ForceResize();
			}
		}

		private bool windowFlag = false;
		private void MainWindow_Activated(object sender, System.EventArgs e)
		{
			if (!windowFlag) {
				windowFlag = true;
				foreach (MenuItem mi in showMenu.MenuItems)
					if (mi.Checked)
						((Form)mi.Tag).BringToFront();
				Focus();
				BringToFront();
				windowFlag = false;
			}
		}

		private void miInfo_Click(object sender, System.EventArgs e)
		{
			MapInfoForm mif = new MapInfoForm();
			mif.Show();
			mif.Map = MapControl.Current;
		}

		public static void Cut_click(object sender, EventArgs e)
		{
			MapControl.Copy();
			MapControl.ClearSelection();
		}

		public static void Copy_click(object sender, EventArgs e)
		{
			MapControl.Copy();
		}

		public static void Paste_click(object sender, EventArgs e)
		{
			MapControl.Paste();
		}

		/// <summary>
		/// Adds buttons for Up,Down,Cut,Copy and Paste to a toolstrip as well as sets some properties for the toolstrip
		/// </summary>
		/// <param name="toolStrip"></param>
		public static void MakeToolstrip(ToolStrip toolStrip)
		{
			ToolStripButton btnUp = new System.Windows.Forms.ToolStripButton();
			ToolStripButton btnDown = new System.Windows.Forms.ToolStripButton();
			ToolStripButton btnCut = new System.Windows.Forms.ToolStripButton();
			ToolStripButton btnCopy = new System.Windows.Forms.ToolStripButton();
			ToolStripButton btnPaste = new System.Windows.Forms.ToolStripButton();
			// 
			// toolStrip1
			// 
			toolStrip.Items.AddRange(new ToolStripItem[] {
				btnUp,
				btnDown,
				btnCut,
				btnCopy,
				btnPaste});
			toolStrip.Padding = new System.Windows.Forms.Padding(0);
			toolStrip.RenderMode = System.Windows.Forms.ToolStripRenderMode.System;
			toolStrip.TabIndex = 1;
			// 
			// btnUp
			// 
			btnUp.AutoSize = false;
			btnUp.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
			btnUp.ImageScaling = System.Windows.Forms.ToolStripItemImageScaling.None;
			btnUp.ImageTransparentColor = System.Drawing.Color.Magenta;
			btnUp.Name = "btnUp";
			btnUp.Size = new System.Drawing.Size(25, 25);
			btnUp.Text = "toolStripButton1";
			btnUp.ToolTipText = "Level Up";
			btnUp.Click += delegate(object sender, EventArgs e) {
				MapControl.Current.Up();
			};
			// 
			// btnDown
			// 
			btnDown.AutoSize = false;
			btnDown.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
			btnDown.ImageScaling = System.Windows.Forms.ToolStripItemImageScaling.None;
			btnDown.ImageTransparentColor = System.Drawing.Color.Magenta;
			btnDown.Name = "btnDown";
			btnDown.Size = new System.Drawing.Size(25, 25);
			btnDown.Text = "toolStripButton2";
			btnDown.ToolTipText = "Level Down";
			btnDown.Click += delegate(object sender, EventArgs e) {
				MapControl.Current.Down();
			};
			// 
			// btnCut
			// 
			btnCut.AutoSize = false;
			btnCut.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
			btnCut.ImageScaling = System.Windows.Forms.ToolStripItemImageScaling.None;
			btnCut.ImageTransparentColor = System.Drawing.Color.Magenta;
			btnCut.Name = "btnCut";
			btnCut.Size = new System.Drawing.Size(25, 25);
			btnCut.Text = "toolStripButton3";
			btnCut.ToolTipText = "Cut";
			btnCut.Click += new EventHandler(Cut_click);
			// 
			// btnCopy
			// 
			btnCopy.AutoSize = false;
			btnCopy.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
			btnCopy.ImageScaling = System.Windows.Forms.ToolStripItemImageScaling.None;
			btnCopy.ImageTransparentColor = System.Drawing.Color.Magenta;
			btnCopy.Name = "btnCopy";
			btnCopy.Size = new System.Drawing.Size(25, 25);
			btnCopy.Text = "toolStripButton4";
			btnCopy.ToolTipText = "Copy";
			btnCopy.Click += new EventHandler(Copy_click);
			// 
			// btnPaste
			// 
			btnPaste.AutoSize = false;
			btnPaste.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
			btnPaste.ImageScaling = System.Windows.Forms.ToolStripItemImageScaling.None;
			btnPaste.ImageTransparentColor = System.Drawing.Color.Magenta;
			btnPaste.Name = "btnPaste";
			btnPaste.Size = new System.Drawing.Size(25, 25);
			btnPaste.Text = "toolStripButton5";
			btnPaste.ToolTipText = "Paste";
			btnPaste.Click += new EventHandler(Paste_click);

			Assembly a = Assembly.GetExecutingAssembly();
			btnCut.Image = Bitmap.FromStream(a.GetManifestResourceStream("MapView._Embedded.cut.gif"));
			btnPaste.Image = Bitmap.FromStream(a.GetManifestResourceStream("MapView._Embedded.paste.gif"));
			btnCopy.Image = Bitmap.FromStream(a.GetManifestResourceStream("MapView._Embedded.copy.gif"));
			btnUp.Image = Bitmap.FromStream(a.GetManifestResourceStream("MapView._Embedded.up.gif"));
			btnDown.Image = Bitmap.FromStream(a.GetManifestResourceStream("MapView._Embedded.down.gif"));
		}

		private void miExport_Click(object sender, EventArgs e)
		{
			//if (mapList.SelectedNode.Parent == null)//top level node - bad
			//    throw new Exception("miExport_Click: Should not be here");

			//ExportForm ef = new ExportForm();
			//List<string> maps = new List<string>();
			//if (mapList.SelectedNode.Parent.Parent == null)//tileset
			//    foreach (TreeNode tn in mapList.SelectedNode.Nodes)
			//        maps.Add(tn.Text);
			//else //map
			//    maps.Add(mapList.SelectedNode.Text);
			//ef.Maps = maps;


			//ef.ShowDialog();
		}

		private void miOpen_Click(object sender, EventArgs e)
		{

		}
	}
}
