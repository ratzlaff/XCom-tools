using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;
using MapView.Forms.MainWindow;
using XCom;

namespace MapView.Forms.MapObservers.RmpViews
{
	public delegate void MapPanelClickDelegate(object sender, MapPanelClickEventArgs e);

	public class MapPanel
		:
		UserControl
	{
		private XCMapFile _map;
		protected Point Origin;

		protected int DrawAreaWidth  = 8;
		protected int DrawAreaHeight = 4;

		public Point ClickPoint;

		public event MapPanelClickDelegate MapPanelClicked;

		public Dictionary<string, Pen> MapPens;
		public Dictionary<string, SolidBrush> MapBrushes;

		public MapPanel()
		{
			MapPens = new Dictionary<string, Pen>();
			MapBrushes = new Dictionary<string, SolidBrush>();
			SetStyle(ControlStyles.AllPaintingInWmPaint | ControlStyles.DoubleBuffer | ControlStyles.UserPaint, true);
		}

		public XCMapFile Map
		{
			get { return _map; }
			set
			{
				_map = value;
				OnResize(null);
			}
		}

		/// <summary>
		/// Get the tile contained at (x,y) in local screen coordinates
		/// </summary>
		/// <param name="x"></param>
		/// <param name="y"></param>
		/// <returns>null if (x,y) is an invalid location for a tile</returns>
		public XCMapTile GetTile(int x, int y)
		{
			if (_map != null)
			{
				Point p = ConvertCoordsDiamond(x, y);
				if (   p.Y >= 0 && p.Y < _map.MapSize.Rows
					&& p.X >= 0 && p.X < _map.MapSize.Cols)
				{
					return (XCMapTile)_map[p.Y, p.X];
				}
			}
			return null;
		}

		public Point GetTileCoordinates(int x, int y)
		{
			Point p = ConvertCoordsDiamond(x, y);
			if (   p.Y >= 0 && p.Y < _map.MapSize.Rows
				&& p.X >= 0 && p.X < _map.MapSize.Cols)
			{
				return p;
			}
			return new Point(-1, -1);
		}

		public void ClearSelected()
		{
			ClickPoint = new Point(-1, -1);
		}

		protected override void OnMouseDown(MouseEventArgs e)
		{
			if (_map != null)
			{
				if (MapPanelClicked != null)
				{
					var p = ConvertCoordsDiamond(e.X, e.Y);
					if (   p.Y >= 0 && p.Y < _map.MapSize.Rows
						&& p.X >= 0 && p.X < _map.MapSize.Cols)
					{
						var tile = _map[p.Y, p.X];
						if (tile != null)
						{
							ClickPoint = p;

							_map.SelectedTile = new MapLocation(
															ClickPoint.Y,
															ClickPoint.X,
															_map.CurrentHeight);

							MapViewPanel.Instance.MapView.SetDrag(p, p);

							var mpe = new MapPanelClickEventArgs();
							mpe.ClickTile = tile;
							mpe.MouseEventArgs = e;
							mpe.ClickLocation = new MapLocation(
															ClickPoint.Y,
															ClickPoint.X,
															_map.CurrentHeight);
							MapPanelClicked(this, mpe);
						}
						else
							return;
					}
				}
				Refresh();
			}
		}

		protected override void OnResize(EventArgs e)
		{
			if (_map != null)
			{
				if (Height > Width / 2) // use width
				{
					DrawAreaWidth = Width / (_map.MapSize.Rows + _map.MapSize.Cols + 1);

					if (DrawAreaWidth % 2 != 0)
						DrawAreaWidth--;

					DrawAreaHeight = DrawAreaWidth / 2;
				}
				else // use height
				{
					DrawAreaHeight = Height / (_map.MapSize.Rows + _map.MapSize.Cols);
					DrawAreaWidth  = DrawAreaHeight * 2;
				}

				Origin = new Point(_map.MapSize.Rows * DrawAreaWidth, 0);
				Refresh();
			}
		}

		private Point ConvertCoordsDiamond(int ptX, int ptY)
		{
			int x = ptX - Origin.X;
			int y = ptY - Origin.Y;

			double x1 = ((double)x / (DrawAreaWidth  * 2))
					  + ((double)y / (DrawAreaHeight * 2));
			double x2 = -((double)x - (double)y * 2) / (DrawAreaWidth * 2);

			return new Point(
						(int)Math.Floor(x1),
						(int)Math.Floor(x2));
		}
	}
}
