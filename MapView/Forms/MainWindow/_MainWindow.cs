using System;
using System.Drawing;
using System.ComponentModel;
using System.Windows.Forms;
using MapView.Forms.Error.WarningConsole;
using MapView.Forms.MainWindow;
using MapView.Forms.MapObservers.RmpViews;
using MapView.Forms.MapObservers.TopViews;
using MapView.SettingServices;
using XCom;
using XCom.GameFiles.Map;
using XCom.GameFiles.Map.RmpData;
using XCom.Interfaces;
using System.IO;
using Microsoft.Win32;
using DSShared;
using XCom.Interfaces.Base;
using System.Collections.Generic;

namespace MapView
{
	public delegate void MapChangedDelegate(object sender, SetMapEventArgs e);

	public delegate void StringDelegate(object sender, string args);

	public partial class MainWindow : Form
	{
		private readonly SettingsManager _settingsManager;

		private readonly MapViewPanel _mapView;
		private readonly LoadingForm _lf;
		private readonly IWarningHandler _warningHandler;
		private readonly IMainWindowWindowsManager _mainWindowWindowsManager;
		private readonly MainWindowsManager _mainWindowsManager;
		private readonly WindowMenuManager _windowMenuManager;

		public MainWindow()
		{
			/***********************/
			InitializeComponent();
			/***********************/

			_mapView = MapViewPanel.Instance;

			_settingsManager = new SettingsManager();
			_windowMenuManager = new WindowMenuManager(showMenu, miHelp);

			loadDefaults();

			Palette.TFTDBattle.SetTransparent(true);
			Palette.UFOBattle.SetTransparent(true);
			Palette.TFTDBattle.Grayscale.SetTransparent(true);
			Palette.UFOBattle.Grayscale.SetTransparent(true);

			#region Setup SharedSpace information and paths

			var sharedSpace = SharedSpace.Instance;
			var consoleSharedSpace = new ConsoleSharedSpace(sharedSpace);
			_warningHandler = new ConsoleWarningHandler(consoleSharedSpace);
			 
			MainWindowsManager.MainToolStripButtonsFactory = new MainToolStripButtonsFactory(_mapView);

			_mainWindowsManager = new MainWindowsManager();
			_mainWindowWindowsManager = new MainWindowWindowsManager(
				_settingsManager, consoleSharedSpace);

			var settings = GetSettings();
			_windowMenuManager.SetMenus(consoleSharedSpace.GetNewConsole(), settings);

			MainWindowsManager.MainWindowsShowAllManager =
				_windowMenuManager.CreateShowAll();


			MainWindowsManager.Initialize();

			sharedSpace.GetObj("MapView", this);
			sharedSpace.GetObj("AppDir", Environment.CurrentDirectory);
			sharedSpace.GetObj("CustomDir", Environment.CurrentDirectory + "\\custom");
			sharedSpace.GetObj("SettingsDir", Environment.CurrentDirectory + "\\settings");

			var pathsFile = new PathInfo(SharedSpace.Instance.GetString("SettingsDir"), "Paths", "pth");
			var settingsFile = new PathInfo(SharedSpace.Instance.GetString("SettingsDir"), "MVSettings", "dat");
			var mapeditFile = new PathInfo(SharedSpace.Instance.GetString("SettingsDir"), "MapEdit", "dat");
			var imagesFile = new PathInfo(SharedSpace.Instance.GetString("SettingsDir"), "Images", "dat");

			sharedSpace.GetObj("MV_PathsFile", pathsFile);
			sharedSpace.GetObj(SettingsService.FILE_NAME, settingsFile);
			sharedSpace.GetObj("MV_MapEditFile", mapeditFile);
			sharedSpace.GetObj("MV_ImagesFile", imagesFile);
			#endregion

			if (!pathsFile.Exists())
			{
				InstallWindow iw = new InstallWindow();

				if (iw.ShowDialog(this) != DialogResult.OK)
					Environment.Exit(-1);
			}

			GameInfo.ParseLine += parseLine;

			InitGameInfo(pathsFile);
			LogFile.Instance.WriteLine("GameInfo.Init done");

			_mainWindowWindowsManager.Register();
			 
			MainWindowsManager.TileView.TileViewControl.MapChanged += TileView_MapChanged;

			LogFile.Instance.WriteLine("Palette transparencies set");

			MapViewPanel.ImageUpdate += new EventHandler(update);

			_mapView.Dock = DockStyle.Fill;

			instance = this;

			mapList.TreeViewNodeSorter = new System.Collections.CaseInsensitiveComparer();

			toolStripContainer1.ContentPanel.Controls.Add(_mapView);
			MainWindowsManager.MainToolStripButtonsFactory.MakeToolstrip(toolStrip);
			toolStrip.Enabled = false;
			toolStrip.Items.Add(new ToolStripSeparator());

			LogFile.Instance.WriteLine("Main view window created");

			LogFile.Instance.WriteLine("Default settings loaded");

			try
			{
				_mapView.MapView.CursorSprite = new CursorSprite(GameInfo.CachePck(SharedSpace.Instance.GetString("cursorFile"), "", 4, Palette.TFTDBattle));
			}
			catch
			{
				try
				{
					_mapView.MapView.CursorSprite = new CursorSprite(GameInfo.CachePck(SharedSpace.Instance.GetString("cursorFile"), "", 2, Palette.UFOBattle));
				}
				catch { _mapView.Cursor = null; }
			}

			LogFile.Instance.WriteLine("Cursor loaded");

			initList();

			LogFile.Instance.WriteLine("Map list created");
			 
			LogFile.Instance.WriteLine("Quick help and About created");

			if (settingsFile.Exists())
			{
				_settingsManager.Load(settingsFile.ToString());
				LogFile.Instance.WriteLine("User settings loaded");
			}
			else
			{
				LogFile.Instance.WriteLine("User settings NOT loaded - no settings file to load");
			}

			OnResize(null);
			this.Closing += new CancelEventHandler(closing);

			_lf = new LoadingForm();
			Bmp.LoadingEvent += _lf.Update;

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

			LogFile.Instance.WriteLine("About to show window");
			Show();
			LogFile.Instance.Close();
		}

		private static void InitGameInfo(PathInfo pathsFile)
		{
			GameInfo.Init(Palette.TFTDBattle, pathsFile);
		}

		private static MainWindow instance;
		public static MainWindow Instance
		{
			get { return instance; }
		}
		 
		private void parseLine(XCom.KeyVal line, XCom.VarCollection vars)
		{
			switch (line.Keyword.ToLower())
			{
				case "cursor":
					if (line.Rest.EndsWith("\\"))
						SharedSpace.Instance.GetObj("cursorFile", line.Rest + "CURSOR");
					else
						SharedSpace.Instance.GetObj("cursorFile", line.Rest + "\\CURSOR");
					break;
				case "logfile":
					try
					{
						LogFile.DebugOn = bool.Parse(line.Rest);
					}
					catch
					{
						Console.WriteLine("Could not parse logfile line");
					}
					break;
			}
		}
		 
		private void ChangeSetting(object sender, string key, object val)
		{
			GetSettings()[key].Value = val;
			switch (key)
			{
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
					if (MapViewPanel.Instance.Map != null)
					{
						if ((bool)val)
							foreach (XCTile t in MapViewPanel.Instance.Map.Tiles)
							{
								if (t.Info.UFODoor || t.Info.HumanDoor)
									t.MakeAnimate();
							}
						else
							foreach (XCTile t in MapViewPanel.Instance.Map.Tiles)
								if (t.Info.UFODoor || t.Info.HumanDoor)
									t.StopAnimate();
					}
					break;
				case "SaveWindowPositions": PathsEditor.SaveRegistry = (bool)val; break;
				case "UseGrid": MapViewPanel.Instance.MapView.UseGrid = (bool)val; break;
				case "GridColor": MapViewPanel.Instance.MapView.GridColor = (Color)val; break;
				case "GridLineColor": MapViewPanel.Instance.MapView.GridLineColor = (Color)val; break;
				case "GridLineWidth": MapViewPanel.Instance.MapView.GridLineWidth = (int)val; break;
			}
		}

		private class SortableTreeNode : TreeNode,IComparable
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
			foreach (string key in maps.Keys)
			{
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

			foreach (string tSetMapGroup in tSet.Subsets.Keys)
			{
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
			if (NotifySave() == DialogResult.Cancel)
			{
				e.Cancel = true;
				return;
			}

			_windowMenuManager.Dispose();

			if (PathsEditor.SaveRegistry)
			{
				RegistryKey swKey = Registry.CurrentUser.CreateSubKey("Software");
				RegistryKey mvKey = swKey.CreateSubKey("MapView");
				RegistryKey riKey = mvKey.CreateSubKey("MainView");

				_mainWindowWindowsManager.CloseAll();

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

			_settingsManager.Save();
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

			var settings = new Settings();
			//Color.FromArgb(175,69,100,129)
			var eh = new ValueChangedDelegate(ChangeSetting);
			settings.AddSetting("Animation", MapViewPanel.Updating, "If true, the map will animate itself", "Main", eh, false, null);
			settings.AddSetting("Doors", false, "If true, the door tiles will animate themselves", "Main", eh, false, null);
			settings.AddSetting("SaveWindowPositions", PathsEditor.SaveRegistry, "If true, the window positions and sizes will be saved in the windows registry", "Main", eh, false, null);
			settings.AddSetting("UseGrid", MapViewPanel.Instance.MapView.UseGrid, "If true, a grid will show up at the current level of editing", "MapView", null, true, MapViewPanel.Instance.MapView);
			settings.AddSetting("GridColor", MapViewPanel.Instance.MapView.GridColor, "Color of the grid in (a,r,g,b) format", "MapView", null, true, MapViewPanel.Instance.MapView);
			settings.AddSetting("GridLineColor", MapViewPanel.Instance.MapView.GridLineColor, "Color of the lines that make up the grid", "MapView", null, true, MapViewPanel.Instance.MapView);
			settings.AddSetting("GridLineWidth", MapViewPanel.Instance.MapView.GridLineWidth, "Width of the grid lines in pixels", "MapView", null, true, MapViewPanel.Instance.MapView);
			settings.AddSetting("SelectGrayscale", MapViewPanel.Instance.MapView.SelectGrayscale, "If true, the selection area will show up in gray", "MapView", null, true, MapViewPanel.Instance.MapView);
			//settings.AddSetting("SaveOnExit",true,"If true, these settings will be saved on program exit","Main",null,false,null);
			SetSettings(settings);
		}

		private void update(object sender, EventArgs e)
		{
			MainWindowsManager.TopView.TopViewControl.BottomPanel.Refresh();
		}

		private static void myQuit(object sender, string command)
		{
			if (command == "OK")
				Environment.Exit(0);
		}

		private void onItem_Click(object sender, System.EventArgs e)
		{
			ChangeSetting(this, "Animation", true);
		}

		private void offItem_Click(object sender, System.EventArgs e)
		{
			ChangeSetting(this, "Animation", false);
		}

		private void saveItem_Click(object sender, System.EventArgs e)
		{
			if (_mapView.Map != null)
			{
				_mapView.Map.Save();
				xConsole.AddLine("Saved: " + _mapView.Map.Name);
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

			var pathInfo = (PathInfo) SharedSpace.Instance["MV_PathsFile"];
			InitGameInfo(pathInfo);
			initList();
		}


		private void mapList_BeforeSelect(object sender, TreeViewCancelEventArgs e)
		{
			if (NotifySave() == DialogResult.Cancel)
			{
				e.Cancel = true;
				return;
			}

			// make old selected not bold
			if (mapList.SelectedNode != null)
			{
				mapList.SelectedNode.BackColor = Color.Transparent ;
			}
		}

		private void mapList_AfterSelect(object sender, TreeViewEventArgs e)
		{
			// Make selected bold
			mapList.SelectedNode.BackColor = Color.Gold;

			LoadSelectedNodeMap();
		}

		private void TileView_MapChanged()
		{
			LoadSelectedNodeMap();
		}

		private void LoadSelectedNodeMap()
		{
			var imd = mapList.SelectedNode.Tag as IMapDesc;
			if (imd != null)
			{
				miExport.Enabled = true;

				var xcTileFactory = new XcTileFactory();
				xcTileFactory.HandleWarning += _warningHandler.HandleWarning;
				var mapService = new XcMapFileService(xcTileFactory);
				var map = mapService.Load(imd as XCMapDesc);
				_mapView.SetMap(map);
				toolStrip.Enabled = true;

				var rmpService = new RmpService();
				rmpService.ReviewRouteEntries(map);

				statusMapName.Text = "Map:" + imd.Name;
				if (map != null)
				{
					tsMapSize.Text = "Size: " + map.MapSize;
				}
				else
				{
					tsMapSize.Text = "Size: Unknown";
				}

				//turn off door animations
				if (miDoors.Checked)
				{
					miDoors.Checked = false;
					miDoors_Click(null, null);
				}

				//open all the forms in the show menu once
				if (!showMenu.Enabled)
				{
					_windowMenuManager.LoadState();
				}

				//Reset all observer events
				_mainWindowsManager.SetMap(map);
			}
			else
			{
				miExport.Enabled = false;
			}
		}

		public DialogResult NotifySave()
		{
			if (_mapView.Map != null && _mapView.Map.MapChanged)
			{
				switch (
					MessageBox.Show(this, "Map changed, do you wish to save?", "Save map?",
						MessageBoxButtons.YesNoCancel, MessageBoxIcon.Question, MessageBoxDefaultButton.Button1))
				{
					case DialogResult.No: //dont save
						break;
					case DialogResult.Yes: //save
						_mapView.Map.Save();
						break;
					case DialogResult.Cancel: //do nothing
						return DialogResult.Cancel;
				}
			}

			return DialogResult.OK;
		}

		private void miOptions_Click(object sender, System.EventArgs e)
		{
			var pf = new PropertyForm("MainViewSettings", GetSettings());
			pf.Text = "MainWindow Options";
			pf.Show();
		}

		private void miSaveImage_Click(object sender, System.EventArgs e)
		{
			if (_mapView.Map != null)
			{
				saveFile.FileName = _mapView.Map.Name;
				if (saveFile.ShowDialog() == DialogResult.OK)
				{
					_lf.Show();
					try
					{
						_mapView.Map.SaveGif(saveFile.FileName);
					}
					finally
					{
						_lf.Hide();
					}
				}
			}
		}

		private void miHq_Click(object sender, System.EventArgs e)
		{
			var map = _mapView.Map as XCMapFile;
			if (map != null)
			{
				map.Hq2x();
				_mapView.OnResize();
			}
		}

		private void miDoors_Click(object sender, System.EventArgs e)
		{
			miDoors.Checked = !miDoors.Checked;

			foreach (XCTile t in _mapView.Map.Tiles)
				if (t.Info.UFODoor || t.Info.HumanDoor)
				{
					if (miDoors.Checked)
						t.MakeAnimate();
					else
						t.StopAnimate();
				}
		}

		private void miResize_Click(object sender, System.EventArgs e)
		{
			if (_mapView.MapView.Map == null) return;
			using (var cmf = new ChangeMapSizeForm())
			{
				cmf.Map = _mapView.MapView.Map;
				if (cmf.ShowDialog(this) == DialogResult.OK)
				{
					cmf.Map.ResizeTo(cmf.NewRows, cmf.NewCols, cmf.NewHeight, cmf.AddHeightToCelling);
					_mapView.ForceResize();
				}
			}
		}

		private bool windowFlag = false;

		private void MainWindow_Activated(object sender, System.EventArgs e)
		{
			if (!windowFlag)
			{
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
			if (_mapView.Map == null) return;
			var mif = new MapInfoForm();
			mif.Show();
			mif.Map = _mapView.Map;
		}


		private void miExport_Click(object sender, EventArgs e)
		{
			//if (mapList.SelectedNode.Parent == null)//top level node - bad
			//	throw new Exception("miExport_Click: Should not be here");

			//ExportForm ef = new ExportForm();
			//List<string> maps = new List<string>();
			//if (mapList.SelectedNode.Parent.Parent == null)//tileset
			//	foreach (TreeNode tn in mapList.SelectedNode.Nodes)
			//		maps.Add(tn.Text);
			//else //map
			//	maps.Add(mapList.SelectedNode.Text);
			//ef.Maps = maps;


			//ef.ShowDialog();
		}

		private void miOpen_Click(object sender, EventArgs e)
		{

		}

		private void SetSettings(Settings settings)
		{
			_settingsManager["MainWindow"] = settings;
		}

		private Settings GetSettings()
		{
			return _settingsManager["MainWindow"];
		}

		private void drawSelectionBoxButton_Click(object sender, EventArgs e)
		{
			_mapView.MapView.DrawSelectionBox = !_mapView.MapView.DrawSelectionBox;
			drawSelectionBoxButton.Checked = !drawSelectionBoxButton.Checked;
		}

		private void ZoomInButton_Click(object sender, EventArgs e)
		{
			if (Globals.PckImageScale < Globals.MaxPckImageScale )
			{
				Globals.PckImageScale += 0.125;
				Globals.AutoPckImageScale = false;
				AutoZoomButton.Checked = false;
				_mapView.SetupMapSize();
				Refresh();
				_mapView.OnResize();
			}
		}

		private void ZoomOutButton_Click(object sender, EventArgs e)
		{
			if (Globals.PckImageScale > Globals.MinPckImageScale)
			{
				Globals.PckImageScale -= 0.125;
				Globals.AutoPckImageScale = false;
				AutoZoomButton.Checked = false;
				_mapView.SetupMapSize();
				Refresh();
				_mapView.OnResize();
			}
		}

		private void AutoZoomButton_Click(object sender, EventArgs e)
		{
			Globals.AutoPckImageScale = !Globals.AutoPckImageScale;
			if (!Globals.AutoPckImageScale)
			{
				Globals.PckImageScale = 1;
			}
			AutoZoomButton.Checked = !AutoZoomButton.Checked;
			_mapView.SetupMapSize();
			Refresh();
			_mapView.OnResize();
		}
	}
}
