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
using UtilLib.Parser;
using UtilLib.Windows;
using UtilLib.Loadable;
using XCom.Interfaces.Base;
using System.Collections.Generic;
using MapView.RmpViewForm;
using MapView.TopViewForm;
using MapLib;
using WeifenLuo.WinFormsUI.Docking;
using ViewLib;

using MapLib.Base.Parsing;

namespace MapView
{
	public delegate void StringDelegate(object sender, string args);

	public partial class MainWindow : MainDockWindow
	{
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
			PathInfo layoutFile = new PathInfo(SharedSpace.Instance.GetString("SettingsDir"), "MVLayout", "xml");

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

			LoadData(pathsFile);

			xConsole.AddLine("GameInfo.Init done");

			// this logic should be reworked to use the BLANKS.PCK
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

//			MakeToolstrip(tools);
			tools.Items.Add(new ToolStripSeparator());

			xConsole.AddLine("Main view window created");

			SetupDefaultSettings();

			registeredForms = new Dictionary<string, Map_Observer_Form>();
			registeredSettings = new Dictionary<string, Settings>();
			registeredSettings["MainWindow"] = settings;

			registerForm(MapList.Instance, showMenu, DockState.DockLeft);
			registerForm(TopView.Instance, showMenu, DockState.DockBottom);
			registerForm(TileView.Instance, showMenu, DockState.DockRight);
			registerForm(PropertyView.Instance, showMenu, DockState.DockRight);
			registerForm(RmpView.Instance, showMenu, DockState.DockRightAutoHide);
			registerForm(MapViewTool.Instance, showMenu, DockState.Document);

//			if (XCom.Globals.UseBlanks)
//				registerForm(mapView.BlankForm, showMenu);

//			addWindow(xConsole.Instance,showMenu);
//			((MenuItem)windowMI[xConsole.Instance]).PerformClick();

			registerForm(new HelpScreen(), miHelp, DockState.Hidden);
			registerForm(new AboutWindow(), miHelp, DockState.Hidden);
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

			UtilLib.Parser.Design.ParseBlockCollectionEditor.DrawFont = Font;

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

		private void LoadData(PathInfo paths)
		{
			VarCollection vars = new VarCollection(new StreamReader(File.OpenRead(paths.ToString())));
			Directory.SetCurrentDirectory(paths.Path);

			KeyVal kv = null;
			MapEdit_dat mapSets = null;

			while ((kv = vars.ReadLine()) != null) {
				switch (kv.Keyword) {
					case "mapdata":
						mapSets = new MapEdit_dat(kv.Rest);
						mapSets.Parse(vars);
						SharedSpace.Instance["mapdata"] = mapSets;
						break;
					case "images":
						Images_dat imageInfo = new Images_dat(kv.Rest);
						imageInfo.Parse(vars);
						SharedSpace.Instance["images"] = imageInfo;
						break;
					case "cursor":
						if (kv.Rest.EndsWith("\\"))
							SharedSpace.Instance.GetObj("cursorFile", kv.Rest + "CURSOR");
						else
							SharedSpace.Instance.GetObj("cursorFile", kv.Rest + "\\CURSOR");
						break;
					case "logfile":
						try {
							bool lineBool = false;
							if (bool.TryParse(kv.Rest, out lineBool))
								xConsole.LogToFile("console.log");
							else
								xConsole.LogToFile(kv.Rest);
						} catch {
							Console.WriteLine("Could not parse logfile line");
						}
						break;
					default:
//						if (ParseLine != null)
//							ParseLine(kv, vars);
//						else
							Console.WriteLine("Unhandled var: {0}:{1}", kv.Keyword, kv.Rest);
						break;
				}
			}

			vars.BaseStream.Close();

			if (mapSets != null) {
				foreach (MapCollection mc in mapSets.Items)
					foreach (Tileset t in mc.Tilesets.Data)
						foreach (MapInfo mi in t.Maps.Data)
							mi.PostLoad();
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
//				case "SaveWindowPositions":
//					PathsEditor.SaveRegistry = (bool)val;
//					break;
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

		public void Cut_click(object sender, EventArgs e)
		{
			MapControl.Copy();
			MapControl.ClearSelection();
		}

		public void Copy_click(object sender, EventArgs e)
		{
			MapControl.Copy();
		}

		public void Paste_click(object sender, EventArgs e)
		{
			MapControl.Paste();
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

		private void btnUp_Click(object sender, EventArgs e)
		{
			MapControl.Current.Up();
		}

		private void btnDown_Click(object sender, EventArgs e)
		{
			MapControl.Current.Down();
		}
	}
}
