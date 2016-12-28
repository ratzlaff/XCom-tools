using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;
using MapView.Forms.MainWindow;
using Microsoft.Win32;
using XCom;

namespace MapView.Forms.MapObservers.TopViews
{
	public partial class TopView
		:
		MapObserverControl
	{
		private readonly Dictionary<ToolStripMenuItem, int> _visibleHash;
		private Dictionary<string, SolidBrush> _brushes;
		private Dictionary<string, Pen> _pens;

		private readonly TopViewPanel _topViewPanel;
		private MainToolStripButtonsFactory _mainToolStripButtonsFactory;

		public event EventHandler VisibleTileChanged;

		public TopView()
		{
			InitializeComponent();

			SuspendLayout();
			_topViewPanel = new TopViewPanel();
			_topViewPanel.Width  = 100;
			_topViewPanel.Height = 100;

			center.AutoScroll = true;
			center.Controls.Add(_topViewPanel);

			center.Resize += (sender, e) => _topViewPanel.ParentSize(center.Width, center.Height);

			_visibleHash = new Dictionary<ToolStripMenuItem, int>();

			var visItems = VisibleToolStripButton.DropDown.Items;
			var ground = new ToolStripMenuItem("Ground");
			visItems.Add(ground);
			_topViewPanel.Ground = ground ;
			_visibleHash[_topViewPanel.Ground] = 0;
			_topViewPanel.Ground.ShortcutKeys = Keys.F1;

			var west = new ToolStripMenuItem("West");
			visItems.Add(west);
			_topViewPanel.West = west;
			_visibleHash[_topViewPanel.West] = 1;
			_topViewPanel.West.ShortcutKeys = Keys.F2;

			var north = new ToolStripMenuItem("North");
			visItems.Add(north);
			_topViewPanel.North = north;
			_visibleHash[_topViewPanel.North] = 2;
			_topViewPanel.West.ShortcutKeys = Keys.F3;

			var content = new ToolStripMenuItem("Content");
			visItems.Add(content);
			_topViewPanel.Content = content;
			_visibleHash[_topViewPanel.Content] = 3;
			_topViewPanel.Content.ShortcutKeys = Keys.F4;

			foreach (ToolStripItem visItem in visItems)
				visItem.Click += VisibleClick;

			_topViewPanel.BottomPanel = bottom;

			MoreObservers.Add("BottomPanel", bottom);
			MoreObservers.Add("TopViewPanel", _topViewPanel);

			ResumeLayout();
		}

		public void Initialize(MainToolStripButtonsFactory mainToolStripButtonsFactory)
		{
			_mainToolStripButtonsFactory = mainToolStripButtonsFactory;
			_mainToolStripButtonsFactory.MakeToolstrip(toolStrip);
		}

		public BottomPanel BottomPanel
		{
			get { return bottom; }
		}

		private void VisibleClick(object sender, EventArgs e)
		{
			((ToolStripMenuItem)sender).Checked = !((ToolStripMenuItem)sender).Checked;

			if (VisibleTileChanged != null)
				VisibleTileChanged(this, new EventArgs());

			MapViewPanel.Instance.Refresh();
			Refresh();
		}

		private void DiamondHeight(object sender, string keyword, object val)
		{
			_topViewPanel.MinHeight = (int)val;
		}

		protected override void OnRISettingsLoad(DSShared.Windows.RegistrySaveLoadEventArgs e)
		{
			bottom.Height = 74;
			RegistryKey riKey = e.OpenKey;

			foreach (var mi in _visibleHash.Keys)
				mi.Checked = bool.Parse((string)riKey.GetValue("vis" + _visibleHash[mi], "true"));
		}

		protected override void OnRISettingsSave(DSShared.Windows.RegistrySaveLoadEventArgs e)
		{
			RegistryKey riKey = e.OpenKey;

			foreach (var mi in _visibleHash.Keys)
				riKey.SetValue("vis" + _visibleHash[mi], mi.Checked);
		}

		public override void LoadDefaultSettings()
		{
			var settings = Settings;

			_brushes = new Dictionary<string, SolidBrush>();
			_pens = new Dictionary<string, Pen>();

			_brushes.Add("GroundColor", new SolidBrush(Color.Orange));
			_brushes.Add("ContentColor", new SolidBrush(Color.Green));
			_brushes.Add("SelectTileColor", bottom.SelectColor);

			var northPen = new Pen(new SolidBrush(Color.Red), 4);
			_pens.Add("NorthColor", northPen);
			_pens.Add("NorthWidth", northPen);

			var westPen = new Pen(new SolidBrush(Color.Red), 4);
			_pens.Add("WestColor", westPen);
			_pens.Add("WestWidth", westPen);

			var selPen = new Pen(new SolidBrush(Color.Black), 2);
			_pens.Add("SelectColor", selPen);
			_pens.Add("SelectWidth", selPen);

			var gridPen = new Pen(new SolidBrush(Color.Black), 1);
			_pens.Add("GridColor", gridPen);
			_pens.Add("GridWidth", gridPen);

			var mousePen = new Pen(new SolidBrush(Color.Blue), 2);
			_pens.Add("MouseColor", mousePen);
			_pens.Add("MouseWidth", mousePen);

			ValueChangedDelegate bc = BrushChanged;
			ValueChangedDelegate pc = PenColorChanged;
			ValueChangedDelegate pw = PenWidthChanged;
			ValueChangedDelegate dh = DiamondHeight;

			settings.AddSetting("GroundColor",		Color.Orange,				"Color of the ground tile indicator",			"Tile",		bc, false, null);
			settings.AddSetting("NorthColor",		Color.Red,					"Color of the north tile indicator",			"Tile",		pc, false, null);
			settings.AddSetting("WestColor",		Color.Red,					"Color of the west tile indicator",				"Tile",		pc, false, null);
			settings.AddSetting("ContentColor",		Color.Green,				"Color of the content tile indicator",			"Tile",		bc, false, null);
			settings.AddSetting("NorthWidth",		4,							"Width of the north tile indicator in pixels",	"Tile",		pw, false, null);
			settings.AddSetting("WestWidth",		4,							"Width of the west tile indicator in pixels",	"Tile",		pw, false, null);
			settings.AddSetting("SelectColor",		Color.Black,				"Color of the selection line",					"Select",	pc, false, null);
			settings.AddSetting("SelectWidth",		2,							"Width of the selection line in pixels",		"Select",	pw, false, null);
			settings.AddSetting("GridColor",		Color.Black,				"Color of the grid lines",						"Grid",		pc, false, null);
			settings.AddSetting("GridWidth",		1,							"Width of the grid lines",						"Grid",		pw, false, null);
			settings.AddSetting("MouseWidth",		2,							"Width of the mouse-over indicator",			"Grid",		pw, false, null);
			settings.AddSetting("MouseColor",		Color.Blue,					"Color of the mouse-over indicator",			"Grid",		pc, false, null);
			settings.AddSetting("SelectTileColor",	Color.LightBlue,			"Background color of the selected tile part",	"Other",	bc, false, null);
			settings.AddSetting("DiamondMinHeight",	_topViewPanel.MinHeight,	"Minimum height of the grid tiles",				"Tile",		dh, false, null);

			_topViewPanel.Brushes = _brushes;
			_topViewPanel.Pens = _pens;

			bottom.Brushes = _brushes;
			bottom.Pens = _pens;
			Invalidate();
		}

		public void Fill_Click(object sender, EventArgs evt)
		{
			var map = MapViewPanel.Instance.MapView.Map;

			if (map != null)
			{
				var s = new Point(0, 0);
				var e = new Point(0, 0);

				s.X = Math.Min(MapViewPanel.Instance.MapView.DragStart.X, MapViewPanel.Instance.MapView.DragEnd.X);
				s.Y = Math.Min(MapViewPanel.Instance.MapView.DragStart.Y, MapViewPanel.Instance.MapView.DragEnd.Y);
	
				e.X = Math.Max(MapViewPanel.Instance.MapView.DragStart.X, MapViewPanel.Instance.MapView.DragEnd.X);
				e.Y = Math.Max(MapViewPanel.Instance.MapView.DragStart.Y, MapViewPanel.Instance.MapView.DragEnd.Y);

				// row col
				// y   x
				var tileView = MainWindowsManager.TileView.TileViewControl;
				for (int c = s.X; c <= e.X; c++)
					for (int r = s.Y; r <= e.Y; r++)
						((XCMapTile) map[r, c])[bottom.SelectedQuadrant] = tileView.SelectedTile;

				map.MapChanged = true;
				MapViewPanel.Instance.Refresh();
				Refresh();
			}
		}

		private void options_click(object sender, EventArgs e)
		{
			var pf = new PropertyForm("TopViewType", Settings);

			pf.Text = "TopView Options";
			pf.Show();
		}

		private void BrushChanged(object sender, string key, object val)
		{
			_brushes[key].Color = (Color)val;

			if (key == "SelectTileColor")
				bottom.SelectColor = _brushes[key];

			Refresh();
		}

		private void PenColorChanged(object sender, string key, object val)
		{
			_pens[key].Color = (Color)val;
			Refresh();
		}

		private void PenWidthChanged(object sender, string key, object val)
		{
			_pens[key].Width = (int)val;
			Refresh();
		}

		public bool GroundVisible
		{
			get { return _topViewPanel.Ground.Checked; }
		}

		public bool NorthVisible
		{
			get { return _topViewPanel.North.Checked; }
		}

		public bool WestVisible
		{
			get { return _topViewPanel.West.Checked; }
		}

		public bool ContentVisible
		{
			get { return _topViewPanel.Content.Checked; }
		}
		 
		private void TopView_KeyDown(object sender, KeyEventArgs e)
		{
			if (e.Control
				&& e.KeyCode == Keys.S
				&& Map != null)
			{
				Map.Save();
				e.Handled = true;
			}
		}

		public void SetSelectedQuadrantFrom(TileType tileType)
		{
			switch (tileType)
			{
				case TileType.Ground:
					BottomPanel.SelectedQuadrant = XCMapTile.MapQuadrant.Ground;
					break;

				case TileType.WestWall:
					BottomPanel.SelectedQuadrant = XCMapTile.MapQuadrant.West;
					break;

				case TileType.NorthWall:
					BottomPanel.SelectedQuadrant = XCMapTile.MapQuadrant.North;
					break;

				case TileType.Object:
					BottomPanel.SelectedQuadrant = XCMapTile.MapQuadrant.Content;
					break;
			}
		}
	}
}
