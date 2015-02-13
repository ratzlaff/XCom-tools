using System;
using System.Drawing;
using System.Collections.Generic;
using System.Windows.Forms;
using MapView.Forms.MainWindow;
using XCom;
using Microsoft.Win32;

namespace MapView.TopViewForm
{
	public partial class TopView : Map_Observer_Form
    {
		private readonly Dictionary<MenuItem, int> _visibleHash;
		private Dictionary<string, SolidBrush> _brushes;
		private Dictionary<string, Pen> _pens;

		private readonly TopViewPanel _topViewPanel;
	    private readonly MainToolStripButtonsFactory _mainToolStripButtonsFactory;

		public event EventHandler VisibleTileChanged;
         
        public TopView(MainToolStripButtonsFactory mainToolStripButtonsFactory)
		{
			InitializeComponent();

            _mainToolStripButtonsFactory = mainToolStripButtonsFactory;

            Load += TopView_Load;
			SuspendLayout();
			_topViewPanel = new TopViewPanel();
			_topViewPanel.Width = 100;
			_topViewPanel.Height = 100;

			center.AutoScroll = true;
			center.Controls.Add(_topViewPanel);

			center.Resize += delegate(object sender, EventArgs e)
			{
				_topViewPanel.ParentSize(center.Width, center.Height);
			};

			Menu = new MainMenu();
			MenuItem vis = Menu.MenuItems.Add("Visible");

			_visibleHash = new Dictionary<MenuItem, int>();

			_topViewPanel.Ground = vis.MenuItems.Add("Ground", VisibleClick);
			_visibleHash[_topViewPanel.Ground] = 0;
			_topViewPanel.Ground.Shortcut = Shortcut.F1;

			_topViewPanel.West = vis.MenuItems.Add("West", VisibleClick);
			_visibleHash[_topViewPanel.West] = 1;
			_topViewPanel.West.Shortcut = Shortcut.F2;

			_topViewPanel.North = vis.MenuItems.Add("North", VisibleClick);
			_visibleHash[_topViewPanel.North] = 2;
			_topViewPanel.North.Shortcut = Shortcut.F3;

			_topViewPanel.Content = vis.MenuItems.Add("Content", VisibleClick);
			_visibleHash[_topViewPanel.Content] = 3;
			_topViewPanel.Content.Shortcut = Shortcut.F4;

			MenuItem edit = Menu.MenuItems.Add("Edit");
			edit.MenuItems.Add("Options", new EventHandler(options_click));
			edit.MenuItems.Add("Fill", Fill_Click);

			_topViewPanel.BottomPanel = bottom;

			MoreObservers.Add("BottomPanel", bottom);
			MoreObservers.Add("TopViewPanel", _topViewPanel);

			ResumeLayout();
		}

        private void TopView_Load(object sender, EventArgs e)
        {
            _mainToolStripButtonsFactory.MakeToolstrip(toolStrip);
        }

		public BottomPanel BottomPanel
		{
			get { return bottom; }
		}

		private void VisibleClick(object sender, EventArgs e)
		{
			((MenuItem)sender).Checked = !((MenuItem)sender).Checked;

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

			foreach (MenuItem mi in _visibleHash.Keys)
				mi.Checked = bool.Parse((string)riKey.GetValue("vis" + _visibleHash[mi].ToString(), "true"));
		}

		protected override void OnRISettingsSave(DSShared.Windows.RegistrySaveLoadEventArgs e)
		{
			RegistryKey riKey = e.OpenKey;
			foreach (MenuItem mi in _visibleHash.Keys)
				riKey.SetValue("vis" + _visibleHash[mi].ToString(), mi.Checked);
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

			settings.AddSetting("GroundColor", Color.Orange, "Color of the ground tile indicator", "Tile", bc, false, null);
			settings.AddSetting("NorthColor", Color.Red, "Color of the north tile indicator", "Tile", pc, false, null);
			settings.AddSetting("WestColor", Color.Red, "Color of the west tile indicator", "Tile", pc, false, null);
			settings.AddSetting("ContentColor", Color.Green, "Color of the content tile indicator", "Tile", bc, false, null);
			settings.AddSetting("NorthWidth", 4, "Width of the north tile indicator in pixels", "Tile", pw, false, null);
			settings.AddSetting("WestWidth", 4, "Width of the west tile indicator in pixels", "Tile", pw, false, null);
			settings.AddSetting("SelectColor", Color.Black, "Color of the selection line", "Select", pc, false, null);
			settings.AddSetting("SelectWidth", 2, "Width of the selection line in pixels", "Select", pw, false, null);
			settings.AddSetting("GridColor", Color.Black, "Color of the grid lines", "Grid", pc, false, null);
			settings.AddSetting("GridWidth", 1, "Width of the grid lines", "Grid", pw, false, null);
			settings.AddSetting("MouseWidth", 2, "Width of the mouse-over indicatior", "Grid", pw, false, null);
			settings.AddSetting("MouseColor", Color.Blue, "Color of the mouse-over indicator", "Grid", pc, false, null);
			settings.AddSetting("SelectTileColor", Color.LightBlue, "Background color of the selected tile piece", "Other", bc, false, null);
			settings.AddSetting("DiamondMinHeight", _topViewPanel.MinHeight, "Minimum height of the grid tiles", "Tile", dh, false, null);

			_topViewPanel.Brushes = _brushes;
			_topViewPanel.Pens = _pens;

			bottom.Brushes = _brushes;
			bottom.Pens = _pens;
		}

		public void Fill_Click(object sender, EventArgs evt)
		{ 
	        var map = MapViewPanel.Instance.View.Map;
	        if (map == null) return;
	        var s = new Point(0, 0);
	        var e = new Point(0, 0);

	        s.X = Math.Min(MapViewPanel.Instance.View.StartDrag.X, MapViewPanel.Instance.View.EndDrag.X);
	        s.Y = Math.Min(MapViewPanel.Instance.View.StartDrag.Y, MapViewPanel.Instance.View.EndDrag.Y);

	        e.X = Math.Max(MapViewPanel.Instance.View.StartDrag.X, MapViewPanel.Instance.View.EndDrag.X);
	        e.Y = Math.Max(MapViewPanel.Instance.View.StartDrag.Y, MapViewPanel.Instance.View.EndDrag.Y);

	        //row   col
	        //y     x
	        for (int c = s.X; c <= e.X; c++)
	            for (int r = s.Y; r <= e.Y; r++)
	                ((XCMapTile) map[r, c])[bottom.SelectedQuadrant] = MainWindowsManager.TileView.SelectedTile;
	       
            map.MapChanged = true;
         
	        MapViewPanel.Instance.Refresh();
	        Refresh();
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
            if (e.Control && e.KeyCode == Keys.S)
            {
                if (Map != null)
                {
                    Map.Save();
                    e.Handled = true;
                }
            }
        }
	}
}

