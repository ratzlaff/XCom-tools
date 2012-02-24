using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Forms;
using System.Drawing;
using XCom;

namespace MapView.RmpViewForm
{
	public delegate void MapPanelClickDelegate(object sender, MapPanelClickEventArgs e);

	public class MapPanel : Panel
	{
		protected XCMapFile map;
		protected Point origin;
		protected Point clickPoint;

		public Dictionary<string, SolidBrush> Brushes { get; set; }
		public Dictionary<string, Pen> Pens { get; set; }

		protected int hWidth = 8, hHeight = 4;

		public event MapPanelClickDelegate MapPanelClicked;

		public MapPanel()
		{
			Pens = new Dictionary<string, Pen>();
			Brushes = new Dictionary<string, SolidBrush>();
			SetStyle(ControlStyles.AllPaintingInWmPaint | ControlStyles.DoubleBuffer | ControlStyles.UserPaint, true);
		}

		public XCMapFile Map
		{
			get { return map; }
			set
			{
				map = value;
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
			Point p = convertCoordsDiamond(x, y);
			if (p.Y >= 0 && p.Y < map.MapSize.Rows &&
				p.X >= 0 && p.X < map.MapSize.Cols)
				return (XCMapTile)map[p.Y, p.X];
			return null;
		}

		protected override void OnMouseDown(MouseEventArgs e)
		{
			//Point pt = convertCoordsDiamond(e.X, e.Y);

			if (MapPanelClicked != null) {
				XCom.Interfaces.Base.IMapTile tile = null;

				Point p = convertCoordsDiamond(e.X, e.Y);
				if (p.Y >= 0 && p.Y < map.MapSize.Rows &&
					p.X >= 0 && p.X < map.MapSize.Cols)
					tile = map[p.Y, p.X];

				if (tile != null) {
					clickPoint.X = p.X;
					clickPoint.Y = p.Y;

					map.SelectedTile = new MapLocation(clickPoint.Y, clickPoint.X, map.CurrentHeight);
					MapPanelClickEventArgs mpe = new MapPanelClickEventArgs(e, tile, map.SelectedTile);
					MapPanelClicked(this, mpe);
				}
			}

			Refresh();
		}

		protected override void OnResize(EventArgs e)
		{
			if (map != null) {
				if (Height > Width / 2) {
					//use width
					hWidth = (Width) / (map.MapSize.Rows + map.MapSize.Cols + 1);

					if (hWidth % 2 != 0)
						hWidth--;

					hHeight = hWidth / 2;
				} else { //use height
					hHeight = (Height) / (map.MapSize.Rows + map.MapSize.Cols);
					hWidth = hHeight * 2;
				}

				origin = new Point((map.MapSize.Rows) * hWidth, 0);
				Refresh();
			}
		}

		private Point convertCoordsDiamond(int xp, int yp)
		{
			int x = xp - origin.X;
			int y = yp - origin.Y;

			double x1 = (x * 1.0 / (hWidth * 2)) + (y * 1.0 / (hHeight * 2));
			double x2 = -(x * 1.0 - 2 * y * 1.0) / (hWidth * 2);

			return new Point((int)Math.Floor(x1), (int)Math.Floor(x2));
		}
	}

	public class MapPanelClickEventArgs : EventArgs
	{
		public MapPanelClickEventArgs(MouseEventArgs e, XCom.Interfaces.Base.IMapTile tile, MapLocation inLoc)
		{
			this.MouseEventArgs = e;
			this.Tile = tile;
			this.MapLocation = inLoc;
		}

		public MapLocation MapLocation { get; set; }
		public XCom.Interfaces.Base.IMapTile Tile { get; set; }
		public MouseEventArgs MouseEventArgs { get; set; }
	}
}
