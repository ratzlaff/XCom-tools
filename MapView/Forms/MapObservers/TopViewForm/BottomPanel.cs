using System;
using System.ComponentModel;
using System.Collections.Generic;
using System.Windows.Forms;
using System.Drawing;
using System.Reflection;
using MapView.Forms.MainWindow;
using MapView.Forms.MapObservers.TopViewForm;
using XCom;
using XCom.Interfaces.Base;

namespace MapView.TopViewForm
{
	public class BottomPanel : Map_Observer_Control
	{
		private XCMapTile mapTile;
		private MapLocation lastLoc;

	    private readonly BottomPanelDrawService _drawService;
		public event EventHandler PanelClicked;

		public BottomPanel()
		{
			mapTile = null;
			SetStyle(ControlStyles.AllPaintingInWmPaint | ControlStyles.DoubleBuffer | ControlStyles.UserPaint, true);
			SelectedQuadrant = XCMapTile.MapQuadrant.Ground;
            
			Globals.LoadExtras();

            _drawService = new BottomPanelDrawService();
            _drawService.Brush = new SolidBrush(Color.FromArgb(204, 204, 255));
            _drawService.Font = new Font("Arial", 8);
		}

		[Browsable(false)]
		public Dictionary<string, SolidBrush> Brushes
		{
            get { return _drawService.Brushes; }
            set { _drawService.Brushes= value; }
		}

		[Browsable(false)]
		public Dictionary<string, Pen> Pens
		{
            get { return _drawService.Pens; }
            set { _drawService .Pens= value; }
		}

		[Browsable(false)]
		public SolidBrush SelectColor
		{
			get { return _drawService.Brush; }
            set { _drawService.Brush = value; Refresh(); }
		}

		[Browsable(false)]
		public XCMapTile Tile
		{
			get { return mapTile; }
			set { mapTile = value; Refresh(); }
		}

		public XCMapTile.MapQuadrant SelectedQuadrant { get; private set; }

		public void SetSelected(MouseButtons btn, int clicks)
		{
			if (btn == MouseButtons.Right && mapTile != null)
			{
				if (clicks == 1)
                    mapTile[SelectedQuadrant] = MainWindowsManager.TileView.SelectedTile;
				else if (clicks == 2)
                    mapTile[SelectedQuadrant] = null;
				Globals.MapChanged = true;
			}
			else if (btn == MouseButtons.Left && mapTile != null)
			{
				if (clicks == 2)
                    MainWindowsManager.TileView.SelectedTile = mapTile[SelectedQuadrant];
			}
		}

		public override void HeightChanged(IMap_Base sender, HeightChangedEventArgs e)
		{
			lastLoc.Height = e.NewHeight;
			mapTile = (XCMapTile)map[lastLoc.Row, lastLoc.Col];
			Refresh();
		}

		public override void SelectedTileChanged(IMap_Base sender, SelectedTileChangedEventArgs e)
		{
			mapTile = (XCMapTile)e.SelectedTile;
			lastLoc = e.MapLocation;
			Refresh();
		}

		private void visChanged(object sender, EventArgs e)
		{
			Refresh();
		}

		protected override void OnMouseDown(MouseEventArgs e)
		{
            SelectedQuadrant = (XCMapTile.MapQuadrant)((e.X - BottomPanelDrawService.startX) /
                BottomPanelDrawService.TOTAL_QUADRAN_SPACE);

			SetSelected(e.Button, e.Clicks);

			if (PanelClicked != null)
				PanelClicked(this, new EventArgs());

			Refresh();
		}

		protected override void Render(Graphics g)
		{
            _drawService.Draw(g, mapTile, SelectedQuadrant);
		}
	}
}
