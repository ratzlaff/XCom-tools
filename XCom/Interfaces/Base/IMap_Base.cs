using System;
using System.Collections.Generic;
using System.Drawing.Imaging;
using System.Text;
using System.Drawing;
using XCom.Services;

namespace XCom.Interfaces.Base
{
	public delegate void HeightChangedDelegate(IMap_Base sender, HeightChangedEventArgs e);
	public delegate void SelectedTileChangedDelegate(IMap_Base sender, SelectedTileChangedEventArgs e);

	/// <summary>
	/// Abstract base class definining all common functionality of an editable map
	/// </summary>
	public class IMap_Base
	{
		protected byte _currentHeight;
		protected MapLocation Selected;
		protected MapTileList MapData;

		public bool MapChanged { get; set; }

		protected IMap_Base(string name, List<TileBase> tiles)
		{
			Name = name;
			Tiles = tiles;
		}

		public string Name { get; protected set; }

		public List<TileBase> Tiles { get; protected set; }

		public virtual void Save()
		{
			throw new Exception("Save() is not yet implemented");
		}

		public event HeightChangedDelegate HeightChanged;
		public event SelectedTileChangedDelegate SelectedTileChanged;

		/// <summary>
		/// Changes the currentHeight property and fires a HeightChanged event
		/// </summary>
		public void Up()
		{
			if (_currentHeight > 0)
			{
				var e = new HeightChangedEventArgs(_currentHeight, _currentHeight - 1);
				_currentHeight--;

				if (HeightChanged != null)
					HeightChanged(this, e);
			}
		}

		/// <summary>
		/// Changes the currentHeight property and fires a HeightChanged event
		/// </summary>
		public void Down()
		{
			if (_currentHeight < MapSize.Height - 1)
			{
				_currentHeight++;
				var e = new HeightChangedEventArgs(_currentHeight, _currentHeight + 1);

				if (HeightChanged != null)
					HeightChanged(this, e);
			}
		}

		/// <summary>
		/// Gets the current height
		/// Setting the height will fire a HeightChanged event
		/// </summary>
		public byte CurrentHeight
		{
			get { return _currentHeight; }
			set
			{
				if (value >= 0 && value < MapSize.Height)
				{
					var e = new HeightChangedEventArgs(_currentHeight, value);
					_currentHeight = value;

					if (HeightChanged != null)
						HeightChanged(this, e);
				}
			}
		}

		/// <summary>
		/// Gets the current size of the map
		/// </summary>
		public MapSize MapSize { get; protected set; }

		/// <summary>
		/// gets or sets the current selected location. Setting the location will fire a SelectedTileChanged event
		/// </summary>
		public MapLocation SelectedTile
		{
			get { return Selected; }
			set
			{
				if (   value.Row >= 0 && value.Row < this.MapSize.Rows
					&& value.Col >= 0 && value.Col < this.MapSize.Cols)
				{
					Selected = value;
					var tile = this[Selected.Row, Selected.Col];
					var stc = new SelectedTileChangedEventArgs(value, tile);

					if (SelectedTileChanged != null)
						SelectedTileChanged(this, stc);
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
		public MapTileBase this[int row, int col, int height]
		{
			get
			{
				if (MapData != null)
					return MapData[row, col, height];

				return null;
			}

			set { MapData[row, col, height] = value; }
		}

		/// <summary>
		/// Get/Set a MapTile at the current height using row,col values
		/// </summary>
		/// <param name="row"></param>
		/// <param name="col"></param>
		/// <returns></returns>
		public MapTileBase this[int row, int col]
		{
			get { return this[row, col, _currentHeight]; }
			set { this[row, col, _currentHeight] = value; }
		}

		/// <summary>
		/// Get/Set a MapTile using a MapLocation
		/// </summary>
		public MapTileBase this[MapLocation position]
		{
			get { return this[position.Row, position.Col, position.Height]; }
			set { this[position.Row, position.Col, position.Height] = value; }
		}

		public virtual void ResizeTo(
								int newR,
								int newC,
								int newH,
								bool addHeightToCelling)
		{
			var mapResizeService = new MapResizeService();
			var newMap = mapResizeService.ResizeMap(
												newR,
												newC,
												newH,
												MapSize,
												MapData,
												addHeightToCelling);
			if (newMap != null)
			{
				MapData = newMap;
				MapSize = new MapSize(newR, newC, newH);
				_currentHeight = (byte)(MapSize.Height - 1);
				MapChanged = true;
			}
		}

		/// <summary>
		/// Not yet generic enough to call with custom derived classes other than XCMapFile
		/// </summary>
		/// <param name="file"></param>
		public void SaveGif(string file)
		{
			var palette = GetFirstGroundPalette();
			if (palette == null)
				throw new ApplicationException("At least 1 ground tile is required");

			var rowPlusCols = MapSize.Rows + MapSize.Cols;
			var b = Bmp.MakeBitmap(
								rowPlusCols * (PckImage.Width / 2),
								(MapSize.Height - _currentHeight) * 24 + rowPlusCols * 8,
								palette.Colors);

			var start = new Point(
								(MapSize.Rows - 1) * (PckImage.Width / 2),
								-(24 * _currentHeight));

			int curr = 0;

			if (MapData != null)
			{
				var hWid = Globals.HalfWidth;
				var hHeight = Globals.HalfHeight;
				for (int h = MapSize.Height - 1; h >= _currentHeight; h--)
				{
					for (int row = 0, startX = start.X, startY = start.Y + (24 * h);
						row < MapSize.Rows;
						row++, startX -= hWid, startY += hHeight)
					{
						for (int col = 0, x = startX, y = startY;
							col < MapSize.Cols;
							col++, x += hWid, y += hHeight, curr++)
						{
							var tiles = this[row, col, h].UsedTiles;
							foreach (var tileBase in tiles)
							{
								var t = (XCTile)tileBase;
								Bmp.Draw(t[0].Image, b, x, y - t.Info.TileOffset);
							}

							Bmp.FireLoadingEvent(curr, (MapSize.Height - _currentHeight) * MapSize.Rows * MapSize.Cols);
						}
					}
				}
			}
			try
			{
				var rect = Bmp.GetBoundsRect(b, Bmp.DefaultTransparentIndex);
				var b2 = Bmp.Crop(b, rect);
				b2.Save(file, ImageFormat.Gif);
			}
			catch
			{
				b.Save(file, ImageFormat.Gif);
			}
		}

		private Palette GetFirstGroundPalette()
		{
			for (int h = 0; h < MapSize.Height; h++)
				for (int r = 0; r < MapSize.Rows; r++)
					for (int c = 0; c < MapSize.Cols; c++)
					{
						var tile = (XCMapTile)this[r, c, h];
						if (tile.Ground != null)
							return tile.Ground[0].Palette;
					}

			return null;
		}
	}
}
