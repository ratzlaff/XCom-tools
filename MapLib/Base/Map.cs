using System;
using System.Collections.Generic;
using System.Text;
using System.Drawing;

namespace MapLib.Base
{
	public delegate void MapChangedDelegate(MapChangedEventArgs e);
	public delegate void HeightChangedDelegate(Map sender, HeightChangedEventArgs e);
	public delegate void TileSelectionChangedDelegate(Map sender, SelectedTileChangedEventArgs e);
	public delegate void RefreshDelegate();

	/// <summary>
	/// Abstract base class definining all common functionality of an editable map
	/// </summary>
	public class Map
	{
		protected int currentHeight = 0;
		protected MapLocation selected;
		protected MapSize mapSize;
		protected MapTile[] mapData;
		protected List<Tile> tiles;
		protected string name;

		protected Map(string name, List<Tile> tiles)
		{
			this.name = name;
			this.tiles = tiles;
		}

		public string Name
		{
			get { return name; }
		}

		public List<Tile> Tiles
		{
			get { return tiles; }
		}

		public virtual void Save() { throw new Exception("Save() is not yet implemented"); }
		public virtual void Save(System.IO.FileStream s) { throw new Exception("Save(Filestream s) is not yet implemented"); }

		/// <summary>
		/// Changes the currentHeight property and fires a HeightChanged event
		/// </summary>
		public void Up()
		{
			if (currentHeight > 0) {
				HeightChangedEventArgs e = new HeightChangedEventArgs(currentHeight, currentHeight - 1);
				currentHeight--;
				MapControl.FireHeightChanged(e);
			}
		}

		/// <summary>
		/// Changes the currentHeight property and fires a HeightChanged event
		/// </summary>
		public void Down()
		{
			if (currentHeight < mapSize.Height - 1) {
				HeightChangedEventArgs e = new HeightChangedEventArgs(currentHeight, currentHeight + 1);
				currentHeight++;
				MapControl.FireHeightChanged(e);
			}
		}

		/// <summary>
		/// Gets the current height
		/// Setting the height will fire a HeightChanged event
		/// </summary>
		public int CurrentHeight
		{
			get { return currentHeight; }
			set
			{
				if (value >= 0 && value < mapSize.Height) {
					HeightChangedEventArgs e = new HeightChangedEventArgs(currentHeight, value);
					currentHeight = value;
					MapControl.FireHeightChanged(e);
				}
			}
		}

		/// <summary>
		/// Gets the current size of the map
		/// </summary>
		public MapSize Size
		{
			get { return mapSize; }
		}

		/// <summary>
		/// gets or sets the current selected location. Setting the location will fire a SelectedTileChanged event
		/// </summary>
		public MapLocation SelectedTile
		{
			get { return selected; }
			set
			{
				if (value.Row >= 0 && value.Row < this.mapSize.Rows &&
					value.Col >= 0 && value.Col < this.mapSize.Cols) {
					selected = value;

					MapControl.StartDrag = selected;
					MapControl.EndDrag = selected;

					SelectedTileChangedEventArgs stc = new SelectedTileChangedEventArgs(value, this[selected.Row, selected.Col]);
					MapControl.FireSelectedChanged(stc);
					MapControl.RequestRefresh();
				}
			}
		}

		/// <summary>
		/// Get/Set a MapTile using row,col,height values. No error checking is done to ensure that the location is valid
		/// </summary>
		/// <param name="row"></param>
		/// <param name="col"></param>
		/// <param name="height"></param>
		/// <returns></returns>
		public MapTile this[int row, int col, int height]
		{
			get
			{
				int idx = (mapSize.Rows * mapSize.Cols * height) + (row * mapSize.Cols) + col;
				MapTile tile = null;
				if (idx < mapData.Length)
					tile = mapData[idx];
				return tile;
			}
			set
			{
				int idx = (mapSize.Rows * mapSize.Cols * height) + (row * mapSize.Cols) + col;
				if (idx < mapData.Length)
					mapData[idx] = value;
			}
		}

		/// <summary>
		/// Get/Set a MapTile at the current height using row,col values
		/// </summary>
		/// <param name="row"></param>
		/// <param name="col"></param>
		/// <returns></returns>
		public MapTile this[int row, int col]
		{
			get { return this[row, col, currentHeight]; }
			set { this[row, col, currentHeight] = value; }
		}

		/// <summary>
		/// Get/Set a MapTile using a MapLocation 
		/// </summary>
		/// <param name="location"></param>
		/// <returns></returns>
		public MapTile this[MapLocation location]
		{
			get { return this[location.Row, location.Col, location.Height]; }
			set { this[location.Row, location.Col, location.Height] = value; }
		}

		public void ResizeTo(int r, int c, int h)
		{
			MapTile[] newMap = new MapTile[r * c * h];

			for (int hc = 0; hc < h; hc++)
				for (int rc = 0; rc < r; rc++)
					for (int cc = 0; cc < c; cc++)
						newMap[(r * c * hc) + (rc * c) + cc] = MapTile.BlankTile;

			for (int hc = 0; hc < h && hc < mapSize.Height; hc++)
				for (int rc = 0; rc < r && rc < mapSize.Rows; rc++)
					for (int cc = 0; cc < c && cc < mapSize.Cols; cc++)
						newMap[(r * c * (h - hc - 1)) + (rc * c) + cc] = this[rc, cc, mapSize.Height - hc - 1];

			mapData = newMap;
			mapSize = new MapSize(r, c, h);
			currentHeight = (byte)(mapSize.Height - 1);
		}

#if NO
//		public static int HalfWidth = 16, HalfHeight = 8;
		private static int hWid = 16, hHeight = 8;
		/// <summary>
		/// Not yet generic enough to call with custom derived classes other than XCMapFile
		/// </summary>
		/// <param name="file"></param>
		public void SaveGif(string file)
		{
			DSShared.Palette curPal = null;

			for (int h = 0; h < mapSize.Height; h++)
				for (int r = 0; r < mapSize.Rows; r++)
					for (int c = 0; c < mapSize.Cols; c++)
						if (((XCMapTile)this[r, c, h]).Ground != null) {
							curPal = ((XCMapTile)this[r, c, h]).Ground[0].Palette;
							goto outLoop;
						}
		outLoop:

			Bitmap b = DSShared.Bmp.MakeBitmap((mapSize.Rows + mapSize.Cols) * (PckImage.Width / 2), (mapSize.Height - currentHeight) * 24 + (mapSize.Rows + mapSize.Cols) * 8, curPal.Colors);

			Point start = new Point((mapSize.Rows - 1) * (PckImage.Width / 2), -(24 * currentHeight));

			int curr = 0;

			if (mapData != null)
				for (int h = mapSize.Height - 1; h >= currentHeight; h--)
					for (int row = 0, startX = start.X, startY = start.Y + (24 * h); row < mapSize.Rows; row++, startX -= hWid, startY += hHeight)
						for (int col = 0, x = startX, y = startY; col < mapSize.Cols; col++, x += hWid, y += hHeight, curr++) {
							foreach (XCTile t in this[row, col, h].UsedTiles)
								DSShared.Bmp.Draw(t[0].Image, b, x, y - t.Info.TileOffset);

							Bmp.FireLoadingEvent(curr, (mapSize.Height - currentHeight) * mapSize.Rows * mapSize.Cols);
						}
			try {
				Rectangle rect = DSShared.Bmp.GetBoundsRect(b, DSShared.Bmp.DefaultTransparentIndex);
				Bitmap b2 = DSShared.Bmp.Crop(b, rect);
				b2.Save(file, System.Drawing.Imaging.ImageFormat.Gif);
			} catch {
				b.Save(file, System.Drawing.Imaging.ImageFormat.Gif);
			}
		}
#endif
	}

	public static class MapControl
	{
		public static event MapChangedDelegate MapChanged;
		public static event HeightChangedDelegate HeightChanged;
		public static event TileSelectionChangedDelegate SelectedTileChanged;
		public static event RefreshDelegate Refresh;

		private static Map current;
		public static Map Current
		{
			get { return current; }
			set
			{
				current = value;
				if (MapChanged != null) {
					MapChangedEventArgs e = new MapChangedEventArgs(current);
					MapChanged(e);
				}
			}
		}

		private static MapLocation startDrag, endDrag;
		public static MapLocation StartDrag
		{
			get { return startDrag; }
			set { startDrag = value; }
		}

		public static MapLocation EndDrag
		{
			get { return endDrag; }
			set { 
				endDrag = value;
				SelectedTileChangedEventArgs stc = new SelectedTileChangedEventArgs(value, current[endDrag.Row, endDrag.Col]);
				FireSelectedChanged(stc);
			}
		}

		public static void FireHeightChanged(HeightChangedEventArgs e)
		{
			if (HeightChanged != null)
				HeightChanged(current,e);
		}

		public static void FireSelectedChanged(SelectedTileChangedEventArgs e)
		{
			if (SelectedTileChanged != null)
				SelectedTileChanged(current, e);
		}

		public static void RequestRefresh()
		{
			if (Refresh != null)
				Refresh();
		}
	}
}
