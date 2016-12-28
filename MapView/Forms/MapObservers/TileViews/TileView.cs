using System;
using System.Collections;
using System.ComponentModel;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Windows.Forms;
using MapView.Forms.MainWindow;
using MapView.Forms.McdViewer;
using MapView.SettingServices;
using PckView;
using XCom;
using XCom.Interfaces.Base;

namespace MapView.Forms.MapObservers.TileViews
{
	public delegate void SelectedTileTypeChanged(TileBase newTile);

	public partial class TileView
		:
		MapObserverControl
	{
		private IContainer components = null;
		private MenuItem mcdInfoTab;
//		private MenuItem menuItem1;

		private TilePanel all;
		private TilePanel[] panels;
		private McdViewerForm MCDInfoForm;
		private TabControl tabs;
		private TabPage allTab;
		private TabPage groundTab;
		private TabPage objectsTab;
		private TabPage nWallsTab;
		private TabPage wWallsTab;
		private Hashtable brushes;

		private IMainWindowsShowAllManager _mainWindowsShowAllManager;

		public event SelectedTileTypeChanged SelectedTileTypeChanged;

		private void OnSelectedTileTypeChanged(TileBase newtile)
		{
			var handler = SelectedTileTypeChanged;
			if (handler != null)
				handler(newtile);
		}

		public TileView( )
		{
			InitializeComponent();

			tabs.Selected += tabs_Selected;

			all			= new TilePanel(TileType.All);
			var ground	= new TilePanel(TileType.Ground);
			var wWalls	= new TilePanel(TileType.WestWall);
			var nWalls	= new TilePanel(TileType.NorthWall);
			var objects	= new TilePanel(TileType.Object);

			panels = new[]{all, ground, wWalls, nWalls, objects};

			AddPanel(all,		allTab);
			AddPanel(ground,	groundTab);
			AddPanel(wWalls,	wWallsTab);
			AddPanel(nWalls,	nWallsTab);
			AddPanel(objects,	objectsTab);
		}

		public void Initialize(IMainWindowsShowAllManager mainWindowsShowAllManager)
		{
			_mainWindowsShowAllManager = mainWindowsShowAllManager;
		}

		#region public events

		public event MethodInvoker MapChanged;
		private void OnMapChanged()
		{
			MethodInvoker handler = MapChanged;
			if (handler != null) handler();
		}

		#endregion

		private void tabs_Selected(object sender, TabControlEventArgs e)
		{
			var tile = SelectedTile;
			if (tile != null && tile.Info is McdEntry)
			{
				var info = (McdEntry)tile.Info;
				Text = "TileView: mapID:" + tile.MapId + " mcdID: " + tile.Id;
				UpdateMcdText(info);
			}
		}

		private void options_click(object sender,EventArgs e)
		{
			PropertyForm pf = new PropertyForm("tileViewOptions", Settings);
			pf.Text = "Tile View Settings";
			pf.Show();
		}

		private void BrushChanged(object sender,string key, object val)
		{
			((SolidBrush)brushes[key]).Color = (Color)val;
			Refresh();
		}

		public override void LoadDefaultSettings()
		{
			brushes = new Hashtable();

			ValueChangedDelegate bc = BrushChanged;
			var settings = Settings;
			foreach(string s in Enum.GetNames(typeof(SpecialType)))
			{
				brushes[s] = new SolidBrush(TilePanel.tileTypes[(int)Enum.Parse(typeof(SpecialType), s)]);
				settings.AddSetting(
								s,
								((SolidBrush)brushes[s]).Color,
								"Color of specified tile type",
								"TileView",
								bc,
								false,
								null);
			}
			VolutarSettingService.LoadDefaultSettings(settings);

			TilePanel.Colors=brushes;
		}

		private void AddPanel(TilePanel panel, TabPage page)
		{
			panel.Dock = DockStyle.Fill;
			page.Controls.Add(panel);
			panel.TileChanged += TileChanged;
		}

		private void TileChanged(TileBase tile)
		{
			if (tile != null && tile.Info is McdEntry)
			{
				var info = (McdEntry) tile.Info;
				Text = "TileView: mapID:" + tile.MapId + " mcdID: " + tile.Id + " Name: " + GetSelectedDependencyName();
				UpdateMcdText(info);
			}
			else
			{
				Text = "TileView";
			}
			OnSelectedTileTypeChanged(tile);
		}

		private void UpdateMcdText(McdEntry info)
		{
			if (MCDInfoForm != null)
				MCDInfoForm.UpdateData(info);
		}

		public override IMap_Base Map
		{
			set
			{
				base.Map = value;
				if (value == null)
				{
					Tiles = null;
				}
				else
				{
					Tiles = value.Tiles;
				}
			}
		}
		
		public System.Collections.Generic.List<TileBase> Tiles
		{
			set
			{
				for(int i=0;i<panels.Length;i++)
					panels[i].Tiles = value;
				OnResize(null);
			}
		}

		[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
		public TileBase SelectedTile
		{
			get
			{
				return panels[tabs.SelectedIndex].SelectedTile;
			}
			set
			{
				tabs.SelectedIndex=0;
				all.SelectedTile=value;
				Refresh();
			}
		}

		private void EditPckMenuItem_Click(object sender, EventArgs e)
		{
			var dependencyName = GetSelectedDependencyName();
			if (dependencyName != null)
			{
				var image = GameInfo.ImageInfo[dependencyName];
				if (image != null)
				{
					var path = image.BasePath + image.BaseName + ".PCK";
					if (!File.Exists(path))
					{
						MessageBox.Show("File does not exist: " + path);
					}
					else
					{
						_mainWindowsShowAllManager.HideAll();

						using (var editor = new PckViewForm())
						{
							var pckFile = image.GetPckFile();
							editor.SelectedPalette = pckFile.Pal.Name;
							editor.LoadPckFile(path, pckFile.Bpp);

							var parent = FindForm();

							Form owner = null;
							if (parent != null)
								owner = parent.Owner;

							if (owner == null)
								owner = parent;

							editor.ShowDialog(owner);
							if (editor.SavedFile)
							{
								GameInfo.ImageInfo.Images[dependencyName].ClearMcd();
								GameInfo.ClearPckCache(image.BasePath, image.BaseName);

								OnMapChanged();
							}
						}

						_mainWindowsShowAllManager.RestoreAll();
					}
				}
			}
		}

		private void mcdInfoTab_Click(object sender, System.EventArgs e)
		{
			if (!mcdInfoTab.Checked)
			{
				if (MCDInfoForm == null)
				{
					MCDInfoForm = new McdViewerForm();
					MCDInfoForm.Size = new Size(480, 670);
					MCDInfoForm.Closing += infoTabClosing;

					var tile = SelectedTile;
					if (tile != null && tile.Info is McdEntry)
					{
						var info = (McdEntry)tile.Info;
						Text = "TileView: mapID:" + tile.MapId + " mcdID: " + tile.Id;
						UpdateMcdText(info);
					}
				}

				MCDInfoForm.Visible = true;
				MCDInfoForm.Location = new Point(
											this.Location.X-MCDInfoForm.Width,
											this.Location.Y);
				MCDInfoForm.Show();
				mcdInfoTab.Checked = true;
			}
		}

		private void infoTabClosing(object sender, CancelEventArgs e)
		{
			e.Cancel = true;
			MCDInfoForm.Visible = false;
			mcdInfoTab.Checked = false;
		}

		private string GetSelectedDependencyName()
		{
			var selectedTile = SelectedTile;
			if (selectedTile != null)
			{
				var map = Map as XCMapFile;
				if (map != null)
					return map.GetDependecyName(selectedTile);
			}
			return null;
		}

		private void VolutarMcdEditMenuItem_Click(object sender, EventArgs e)
		{
			if ((Map as XCMapFile) != null)
			{
				var pathService = new VolutarSettingService(Settings);

				var path = pathService.GetEditorFilePath();

				if (!string.IsNullOrEmpty(path))
					Process.Start(new ProcessStartInfo(path));
			}
		}
	}
}
