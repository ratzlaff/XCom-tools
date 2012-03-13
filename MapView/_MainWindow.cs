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
using DSShared;
using DSShared.Windows;
using DSShared.Loadable;
using XCom.Interfaces.Base;
using System.Collections.Generic;
using MapView.RmpViewForm;
using MapView.TopViewForm;
using MVCore;
using ViewLib.Base;

namespace MapView
{
	public delegate void StringDelegate(object sender, string args);

	public partial class MainWindow : Form
	{
		private MapView.MapViewPanel mapView;
		private LoadingForm lf;
		private Dictionary<string, Form> registeredForms;
		private static Dictionary<string, Settings> settingsHash;

//		private LoadOfType<IMapDesc> loadedTypes;
		//public event StringDelegate SendMessage;

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

			sharedSpace.GetObj("MV_PathsFile", pathsFile);
			sharedSpace.GetObj("MV_SettingsFile", settingsFile);
			sharedSpace.GetObj("MV_MapEditFile", mapeditFile);
			sharedSpace.GetObj("MV_ImagesFile", imagesFile);
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

			MapViewPanel.ImageUpdate += new EventHandler(update);

			mapView = MapViewPanel.Instance;
			mapView.Dock = DockStyle.Fill;

			instance = this;

			/***********************/
			InitializeComponent();
			/***********************/

			mapList.TreeViewNodeSorter = new System.Collections.CaseInsensitiveComparer();

			toolStripContainer1.ContentPanel.Controls.Add(mapView);
			MakeToolstrip(toolStrip);
			toolStrip.Items.Add(new ToolStripSeparator());

			xConsole.AddLine("Main view window created");

			settingsHash = new Dictionary<string, Settings>();
			loadDefaults();
			xConsole.AddLine("Default settings loaded");

			try {
				mapView.View.Cursor = new Cursor(GameInfo.CachePck(SharedSpace.Instance.GetString("cursorFile"), "", 4, XCPalette.TFTDBattle));
			} catch {
				try {
					mapView.View.Cursor = new Cursor(GameInfo.CachePck(SharedSpace.Instance.GetString("cursorFile"), "", 2, XCPalette.UFOBattle));
				} catch { mapView.Cursor = null; }
			}

			xConsole.AddLine("Cursor loaded");

			initList();

			xConsole.AddLine("Map list created");

			registeredForms = new Dictionary<string, Form>();
			registerForm(TopView.Instance, "TopView", showMenu);
			registerForm(TileView.Instance, "TileView", showMenu);
			registerForm(RmpView.Instance, "RmpView", showMenu);

			if (XCom.Globals.UseBlanks)
				registerForm(mapView.BlankForm, "Blank Info", showMenu);

			//			addWindow(xConsole.Instance,"Console",showMenu);
			//			((MenuItem)windowMI[xConsole.Instance]).PerformClick();

			registerForm(new HelpScreen(), "Quick Help", miHelp);
			registerForm(new AboutWindow(), "About", miHelp);

			xConsole.AddLine("Quick help and About created");

			if (settingsFile.Exists()) {
				readMapViewSettings(new StreamReader(settingsFile.ToString()));
				xConsole.AddLine("User settings loaded");
			} else
				xConsole.AddLine("User settings NOT loaded - no settings file to load");

			OnResize(null);
			this.Closing += new CancelEventHandler(closing);

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

		private static MainWindow instance;
		public static MainWindow Instance
		{
			get { return instance; }
		}

		private void registerForm(Form f, string title, MenuItem parent)
		{
			f.Closing += new CancelEventHandler(formClosing);

			f.Text = title;
			MenuItem mi = new MenuItem(title);
			mi.Tag = f;

			if (f is Map_Observer_Form) {
				Map_Observer_Form mof = (Map_Observer_Form)f;
				mof.MenuItem = mi;

				settingsHash.Add(title, new Settings());
				mof.SetupDefaultSettings(settingsHash[title]);
				
//				((Map_Observer_Form)f).RegistryInfo = new DSShared.Windows.RegistryInfo(f, title);
			}

			f.ShowInTaskbar = false;
			f.FormBorderStyle = FormBorderStyle.SizableToolWindow;

			parent.MenuItems.Add(mi);
			mi.Click += new EventHandler(formMIClick);
			registeredForms.Add(title, f);
		}

		private void formMIClick(object sender, EventArgs e)
		{
			MenuItem mi = (MenuItem)sender;

			if (!mi.Checked) {
				((Form)mi.Tag).Show();
				((Form)mi.Tag).WindowState = FormWindowState.Normal;
				mi.Checked = true;
			} else {
				((Form)mi.Tag).Close();
				mi.Checked = false;
			}
		}

		private void formClosing(object sender, CancelEventArgs e)
		{
			e.Cancel = true;
			if (sender is Map_Observer_Form)
				((Map_Observer_Form)sender).MenuItem.Checked = false;
			((Form)sender).Hide();
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
			MVCore.Parser.VarCollection vc = new MVCore.Parser.VarCollection(sr);
			MVCore.Parser.KeyVal kv = null;

			while ((kv = vc.ReadLine()) != null) {
				try {
					Settings.ReadSettings(vc, kv, settingsHash[kv.Keyword]);
				} catch { }
			}

			sr.Close();
		}

		private void changeSetting(object sender, string key, object val)
		{
			settingsHash["MainWindow"][key].Value = val;
			switch (key) {
				case "Animation":
					bool animVal = (bool)val;
					onItem.Checked = animVal;
					offItem.Checked = !animVal;

					if ((bool)val)
						MapViewPanel.Start();
					else
						MapViewPanel.Stop();
					break;
				case "Doors":
					if (MapLib.Base.MapControl.Current != null)
						foreach (MapLib.Base.Tile t in MapLib.Base.MapControl.Current.Tiles)
							t.Animate((bool)val);					
					break;
				case "SaveWindowPositions":
					PathsEditor.SaveRegistry = (bool)val;
					break;
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
			}
		}

		private class SortableTreeNode : TreeNode, IComparable
		{
			public SortableTreeNode(string text) : base(text) { }

			public int CompareTo(object other)
			{
				if (other is SortableTreeNode)
					return Text.CompareTo(((SortableTreeNode)other).Text);
				return -1;
			}
		}

		private void addMaps(TreeNode tn, Dictionary<string, IMapDesc> maps)
		{
			foreach (string key in maps.Keys) {
				SortableTreeNode mapNode = new SortableTreeNode(key);
				mapNode.Tag = maps[key];
				tn.Nodes.Add(mapNode);
			}
		}

		public void AddTileset(ITileset tSet)
		{
			SortableTreeNode tSetNode = new SortableTreeNode(tSet.Name);
			tSetNode.Tag = tSet;
			mapList.Nodes.Add(tSetNode);

			foreach (string tSetMapGroup in tSet.Subsets.Keys) {
				SortableTreeNode tsGroup = new SortableTreeNode(tSetMapGroup);
				tsGroup.Tag = tSet.Subsets[tSetMapGroup];
				tSetNode.Nodes.Add(tsGroup);

				addMaps(tsGroup, tSet.Subsets[tSetMapGroup]);
			}
		}

		private void initList()
		{
			mapList.Nodes.Clear();
			foreach (string key in GameInfo.TilesetInfo.Tilesets.Keys)
				AddTileset(GameInfo.TilesetInfo.Tilesets[key]);
		}

		private void closing(object sender, CancelEventArgs e)
		{
			if (NotifySave() == DialogResult.Cancel) {
				e.Cancel = true;
				return;
			}

			if (PathsEditor.SaveRegistry) {
				RegistryKey swKey = Registry.CurrentUser.CreateSubKey("Software");
				RegistryKey mvKey = swKey.CreateSubKey("MapView");
				RegistryKey riKey = mvKey.CreateSubKey("MainView");

				foreach (string key in registeredForms.Keys) {
					Form form = registeredForms[key];
					form.WindowState = FormWindowState.Normal;
					form.Close();
				}

				WindowState = FormWindowState.Normal;
				riKey.SetValue("Left", Left);
				riKey.SetValue("Top", Top);
				riKey.SetValue("Width", Width);
				riKey.SetValue("Height", Height - 19);

				//				riKey.SetValue("Animation",onItem.Checked.ToString());
				//				riKey.SetValue("Doors",miDoors.Checked.ToString());

				riKey.Close();
				mvKey.Close();
				swKey.Close();
			}

			StreamWriter sw = new StreamWriter(SharedSpace.Instance["MV_SettingsFile"].ToString());
			foreach (string s in settingsHash.Keys)
				if (settingsHash[s] != null)
					settingsHash[s].Save(s, sw);
			sw.Flush();
			sw.Close();
		}

		private void loadDefaults()
		{
			RegistryKey swKey = Registry.CurrentUser.CreateSubKey("Software");
			RegistryKey mvKey = swKey.CreateSubKey("MapView");
			RegistryKey riKey = mvKey.CreateSubKey("MainView");

			Left = (int)riKey.GetValue("Left", Left);
			Top = (int)riKey.GetValue("Top", Top);
			Width = (int)riKey.GetValue("Width", Width);
			Height = (int)riKey.GetValue("Height", Height);

			riKey.Close();
			mvKey.Close();
			swKey.Close();

			Settings settings = new Settings();
			//Color.FromArgb(175,69,100,129)
			settings.AddSetting("Animation", MapViewPanel.Updating, "If true, the map will animate itself", "Main", changeSetting);
			settings.AddSetting("Doors", false, "If true, the door tiles will animate themselves", "Main", changeSetting);
			settings.AddSetting("SaveWindowPositions", PathsEditor.SaveRegistry, "If true, the window positions and sizes will be saved in the windows registry", "Main", changeSetting);
			MapViewPanel.Instance.View.LoadDefaultSettings(null, settings);
			//settings.AddSetting("SaveOnExit",true,"If true, these settings will be saved on program exit","Main",null,false,null);
			settingsHash["MainWindow"] = settings;
		}

		private void update(object sender, EventArgs e)
		{
			TopView.Instance.BottomPanel.Refresh();
		}

		private static void myQuit(object sender, string command)
		{
			if (command == "OK")
				Environment.Exit(0);
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
			if (MapLib.Base.MapControl.Current != null) {
				MapLib.Base.MapControl.Current.Save();
				xConsole.AddLine("Saved: " + MapLib.Base.MapControl.Current.Name);
				Globals.MapChanged = false;
			}
		}

		private void quititem_Click(object sender, System.EventArgs e)
		{
			closing(null, new CancelEventArgs(true));
			Environment.Exit(0);
		}

		private void miPaths_Click(object sender, System.EventArgs e)
		{
			PathsEditor p = new PathsEditor(SharedSpace.Instance["MV_PathsFile"].ToString());
			p.ShowDialog();

			GameInfo.Init(XCPalette.TFTDBattle, (PathInfo)SharedSpace.Instance["MV_PathsFile"]);
			initList();
		}

		private void mapList_AfterSelect(object sender, System.Windows.Forms.TreeViewEventArgs e)
		{
			if (NotifySave() == DialogResult.Cancel)
				return;

			if (mapList.SelectedNode.Tag is IMapDesc) {
				IMapDesc imd = (IMapDesc)mapList.SelectedNode.Tag;
				miExport.Enabled = true;
				MapLib.Base.MapControl.Current = imd.GetMapFile();

				statusMapName.Text = "Map:" + imd.Name;
				tsMapSize.Text = "Size: " + MapLib.Base.MapControl.Current.Size.ToString();

				//turn off door animations
				if (miDoors.Checked) {
					miDoors.Checked = false;
					miDoors_Click(null, null);
				}

				//open all the forms in the show menu once
				if (!showMenu.Enabled) {
					foreach (MenuItem mi in showMenu.MenuItems)
						mi.PerformClick();

					showMenu.Enabled = true;
				}

				// notify everyone that there is a new map
				MapViewPanel.Instance.View.Refresh();
			} else
				miExport.Enabled = false;
		}

		public DialogResult NotifySave()
		{
			if (MapLib.Base.MapControl.Current != null && Globals.MapChanged)
				switch (MessageBox.Show(this, "Map changed, do you wish to save?", "Save map?", MessageBoxButtons.YesNoCancel, MessageBoxIcon.Question, MessageBoxDefaultButton.Button1)) {
					case DialogResult.No: //dont save 
						break;
					case DialogResult.Yes: //save
						MapLib.Base.MapControl.Current.Save();
						break;
					case DialogResult.Cancel://do nothing
						return DialogResult.Cancel;
				}

			Globals.MapChanged = false;
			return DialogResult.OK;
		}

		private void miOptions_Click(object sender, System.EventArgs e)
		{
			PropertyForm pf = new PropertyForm("MainViewSettings", settingsHash["MainWindow"]);
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
			if (MapLib.Base.MapControl.Current is XCMapFile) {
				((XCMapFile)MapLib.Base.MapControl.Current).Hq2x();
				mapView.View.Resize();
			}
		}

		private void miDoors_Click(object sender, System.EventArgs e)
		{
			miDoors.Checked = !miDoors.Checked;

			foreach (MapLib.Base.Tile t in MapLib.Base.MapControl.Current.Tiles)
				t.Animate(miDoors.Checked);
		}

		private void miResize_Click(object sender, System.EventArgs e)
		{
			ChangeMapSizeForm cmf = new ChangeMapSizeForm();
			cmf.Map = MapLib.Base.MapControl.Current;
			if (cmf.ShowDialog(this) == DialogResult.OK) {
				cmf.Map.ResizeTo(cmf.NewRows, cmf.NewCols, cmf.NewHeight);
				//mapView.View.Map.ResizeTo(cmf.NewRows, cmf.NewCols, cmf.NewHeight);
				mapView.ForceResize();
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
			mif.Map = MapLib.Base.MapControl.Current;
		}

		/// <summary>
		/// Adds buttons for Up,Down,Cut,Copy and Paste to a toolstrip as well as sets some properties for the toolstrip
		/// </summary>
		/// <param name="toolStrip"></param>
		public void MakeToolstrip(ToolStrip toolStrip)
		{
			System.Windows.Forms.ToolStripButton btnUp = new System.Windows.Forms.ToolStripButton();
			System.Windows.Forms.ToolStripButton btnDown = new System.Windows.Forms.ToolStripButton();
			System.Windows.Forms.ToolStripButton btnCut = new System.Windows.Forms.ToolStripButton();
			System.Windows.Forms.ToolStripButton btnCopy = new System.Windows.Forms.ToolStripButton();
			System.Windows.Forms.ToolStripButton btnPaste = new System.Windows.Forms.ToolStripButton();
			// 
			// toolStrip1
			// 
			//toolStrip.Dock = System.Windows.Forms.DockStyle.None;
			//toolStrip.GripMargin = new System.Windows.Forms.Padding(0);
			//toolStrip.GripStyle = System.Windows.Forms.ToolStripGripStyle.Hidden;
			toolStrip.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            btnUp,
            btnDown,
            btnCut,
            btnCopy,
            btnPaste});
			//toolStrip1.LayoutStyle = System.Windows.Forms.ToolStripLayoutStyle.VerticalStackWithOverflow;
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
				MapLib.Base.MapControl.Current.Up();
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
				MapLib.Base.MapControl.Current.Down();
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
			btnCut.Click += new EventHandler(MapViewPanel.Instance.Cut_click);
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
			btnCopy.Click += new EventHandler(MapViewPanel.Instance.Copy_click);
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
			btnPaste.Click += new EventHandler(MapViewPanel.Instance.Paste_click);

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
