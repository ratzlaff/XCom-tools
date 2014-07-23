using System;
using System.Drawing;
using System.Collections.Generic;
using System.ComponentModel;
using System.Windows.Forms;
using MapView.Forms.MainWindow;
using XCom;
using XCom.Interfaces;
using System.Drawing.Drawing2D;
using Microsoft.Win32;
using XCom.Interfaces.Base;
using System.Reflection;

namespace MapView.TopViewForm
{
	public partial class TopView : Map_Observer_Form
	{
		private Dictionary<MenuItem, int> visibleHash;
		private Dictionary<string, SolidBrush> brushes;
		private Dictionary<string, Pen> pens;

		private TopViewPanel topViewPanel;

		public event EventHandler VisibleTileChanged;

		#region Singleton access
		private static TopView myInstance = null;
		public static TopView Instance
		{
			get
			{
				if (myInstance == null)
					myInstance = new TopView();

				return myInstance;
			}
		}
		#endregion

		private TopView()
		{
			//LogFile.Instance.WriteLine("Start TopView window creation");		

			InitializeComponent();
         
            var mainToolStripButtonsFactory = new MainToolStripButtonsFactory();
            mainToolStripButtonsFactory.MakeToolstrip(toolStrip);

            var btnFill = new ToolStripButton();
            toolStrip.Items.Add(  btnFill);
            // 
            // btnFill
            // 
            btnFill.AutoSize = false;
            btnFill.DisplayStyle = ToolStripItemDisplayStyle.Text;
            btnFill.Name = "btnFill";
            btnFill.Size = new Size(25, 25);
            btnFill.Text = "Fill";
            btnFill.ToolTipText = "Fill";
            btnFill.Click += fill_click;

			SuspendLayout();
			topViewPanel = new TopViewPanel();
			topViewPanel.Width = 100;
			topViewPanel.Height = 100;

			center.AutoScroll = true;
			center.Controls.Add(topViewPanel);

			center.Resize += delegate(object sender, EventArgs e)
			{
				topViewPanel.ParentSize(center.Width, center.Height);
			};

			//bottom.PanelClicked += new EventHandler(bottomClick);

			this.Menu = new MainMenu();
			MenuItem vis = Menu.MenuItems.Add("Visible");

			visibleHash = new Dictionary<MenuItem, int>();

			topViewPanel.Ground = vis.MenuItems.Add("Ground", new EventHandler(visibleClick));
			visibleHash[topViewPanel.Ground] = 0;
			topViewPanel.Ground.Shortcut = Shortcut.F1;

			topViewPanel.West = vis.MenuItems.Add("West", new EventHandler(visibleClick));
			visibleHash[topViewPanel.West] = 1;
			topViewPanel.West.Shortcut = Shortcut.F2;

			topViewPanel.North = vis.MenuItems.Add("North", new EventHandler(visibleClick));
			visibleHash[topViewPanel.North] = 2;
			topViewPanel.North.Shortcut = Shortcut.F3;

			topViewPanel.Content = vis.MenuItems.Add("Content", new EventHandler(visibleClick));
			visibleHash[topViewPanel.Content] = 3;
			topViewPanel.Content.Shortcut = Shortcut.F4;

			MenuItem edit = Menu.MenuItems.Add("Edit");
			edit.MenuItems.Add("Options", new EventHandler(options_click));
			edit.MenuItems.Add("Fill", new EventHandler(fill_click));

			//mapView.BlankChanged += new BoolDelegate(blankMode);

			//Controls.Add(bottom);

			topViewPanel.BottomPanel = bottom;

			MoreObservers.Add("BottomPanel", bottom);
			MoreObservers.Add("TopViewPanel", topViewPanel);

			ResumeLayout();
		}

		public BottomPanel BottomPanel
		{
			get { return bottom; }
		}

		private void visibleClick(object sender, EventArgs e)
		{
			((MenuItem)sender).Checked = !((MenuItem)sender).Checked;

			if (VisibleTileChanged != null)
				VisibleTileChanged(this, new EventArgs());

			MapViewPanel.Instance.Refresh();

			Refresh();
		}

		private void diamondHeight(object sender, string keyword, object val)
		{
			topViewPanel.MinHeight = (int)val;
		}

		protected override void OnRISettingsLoad(DSShared.Windows.RegistrySaveLoadEventArgs e)
		{
			bottom.Height = 74;
			RegistryKey riKey = e.OpenKey;

			foreach (MenuItem mi in visibleHash.Keys)
				mi.Checked = bool.Parse((string)riKey.GetValue("vis" + visibleHash[mi].ToString(), "true"));
		}

		protected override void OnRISettingsSave(DSShared.Windows.RegistrySaveLoadEventArgs e)
		{
			RegistryKey riKey = e.OpenKey;
			foreach (MenuItem mi in visibleHash.Keys)
				riKey.SetValue("vis" + visibleHash[mi].ToString(), mi.Checked);
		}

		protected override void LoadDefaultSettings(Settings settings)
		{
			//RegistryKey swKey = Registry.CurrentUser.CreateSubKey("Software");
			//RegistryKey mvKey = swKey.CreateSubKey("MapView");
			//RegistryKey riKey = mvKey.CreateSubKey("TopView");

			//foreach (MenuItem mi in visibleHash.Keys)
			//    mi.Checked = bool.Parse((string)riKey.GetValue("vis" + visibleHash[mi].ToString(), "true"));

			//riKey.Close();
			//mvKey.Close();
			//swKey.Close();

			brushes = new Dictionary<string, SolidBrush>();
			pens = new Dictionary<string, Pen>();

			brushes.Add("GroundColor", new SolidBrush(Color.Orange));
			brushes.Add("ContentColor", new SolidBrush(Color.Green));
			brushes.Add("SelectTileColor", bottom.SelectColor);

			Pen northPen = new Pen(new SolidBrush(Color.Red), 4);
			pens.Add("NorthColor", northPen);
			pens.Add("NorthWidth", northPen);

			Pen westPen = new Pen(new SolidBrush(Color.Red), 4);
			pens.Add("WestColor", westPen);
			pens.Add("WestWidth", westPen);

			Pen selPen = new Pen(new SolidBrush(Color.Black), 2);
			pens.Add("SelectColor", selPen);
			pens.Add("SelectWidth", selPen);

			Pen gridPen = new Pen(new SolidBrush(Color.Black), 1);
			pens.Add("GridColor", gridPen);
			pens.Add("GridWidth", gridPen);

			Pen mousePen = new Pen(new SolidBrush(Color.Blue), 2);
			pens.Add("MouseColor", mousePen);
			pens.Add("MouseWidth", mousePen);

			ValueChangedDelegate bc = new ValueChangedDelegate(brushChanged);
			ValueChangedDelegate pc = new ValueChangedDelegate(penColorChanged);
			ValueChangedDelegate pw = new ValueChangedDelegate(penWidthChanged);
			ValueChangedDelegate dh = new ValueChangedDelegate(diamondHeight);

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
			settings.AddSetting("SelectTileColor", Color.Lavender, "Background color of the selected tile piece", "Other", bc, false, null);
			settings.AddSetting("DiamondMinHeight", topViewPanel.MinHeight, "Minimum height of the grid tiles", "Tile", dh, false, null);

			topViewPanel.Brushes = brushes;
			topViewPanel.Pens = pens;

			bottom.Brushes = brushes;
			bottom.Pens = pens;
		}

		private void fill_click(object sender, EventArgs evt)
		{ 
	        var map = MapViewPanel.Instance.View.Map;
	        if (map == null) return;
	        Point s = new Point(0, 0);
	        Point e = new Point(0, 0);

	        s.X = Math.Min(MapViewPanel.Instance.View.StartDrag.X, MapViewPanel.Instance.View.EndDrag.X);
	        s.Y = Math.Min(MapViewPanel.Instance.View.StartDrag.Y, MapViewPanel.Instance.View.EndDrag.Y);

	        e.X = Math.Max(MapViewPanel.Instance.View.StartDrag.X, MapViewPanel.Instance.View.EndDrag.X);
	        e.Y = Math.Max(MapViewPanel.Instance.View.StartDrag.Y, MapViewPanel.Instance.View.EndDrag.Y);

	        //row   col
	        //y     x
	        for (int c = s.X; c <= e.X; c++)
	            for (int r = s.Y; r <= e.Y; r++)
	                ((XCMapTile) map[r, c])[bottom.SelectedQuadrant] = TileView.Instance.SelectedTile;
	        Globals.MapChanged = true;
	        MapViewPanel.Instance.Refresh();
	        Refresh();
	    }

	    private void options_click(object sender, EventArgs e)
		{
			PropertyForm pf = new PropertyForm("TopViewType", Settings);
			pf.Text = "TopView Options";
			pf.Show();
		}

		private void brushChanged(object sender, string key, object val)
		{
			((SolidBrush)brushes[key]).Color = (Color)val;
			if (key == "SelectTileColor")
				bottom.SelectColor = (SolidBrush)brushes[key];
			Refresh();
		}

		private void penColorChanged(object sender, string key, object val)
		{
			((Pen)pens[key]).Color = (Color)val;
			Refresh();
		}

		private void penWidthChanged(object sender, string key, object val)
		{
			((Pen)pens[key]).Width = (int)val;
			Refresh();
		}

		public bool GroundVisible
		{
			get { return topViewPanel.Ground.Checked; }
		}

		public bool NorthVisible
		{
			get { return topViewPanel.North.Checked; }
		}

		public bool WestVisible
		{
			get { return topViewPanel.West.Checked; }
		}

		public bool ContentVisible
		{
			get { return topViewPanel.Content.Checked; }
		}

		private void btnUp_Click(object sender, EventArgs e)
		{
			Map.Up();
		}

		private void btnDown_Click(object sender, EventArgs e)
		{
			Map.Down();
		}
	}
}

