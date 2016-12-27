namespace XCom.Interfaces.Base
{
	public class MapTileList
	{
		private readonly MapTileBase[] _mapData;
		private readonly  MapPosition _mapPosition;

		public MapTileList(int rows, int cols, int height)
		{
			_mapData = new MapTileBase[rows * cols * height];
			_mapPosition = new MapPosition();
			_mapPosition.MaxR = rows;
			_mapPosition.MaxC = cols;
			_mapPosition.MaxH = height;
		}

		public MapTileBase this[int row, int col, int height]
		{
			get
			{
				if (_mapPosition.MaxR < row ||
					_mapPosition.MaxC < col ||
					_mapPosition.MaxH < height) return null;
				var index = GetIndex(row, col, height);
				if (index >= _mapData.Length) return null;
				return _mapData[index];
			}
			set
			{
				var index = GetIndex(row, col, height);
				_mapData[index] = value;
			}
		}

		private int GetIndex(int row, int col, int height)
		{
			_mapPosition.R = row;
			_mapPosition.C = col;
			_mapPosition.H = height;
			return _mapPosition.GetIntLocation();
		}
	}
}
