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
				if (   row		<= _mapPosition.MaxR
					&& col		<= _mapPosition.MaxC
					&& height	<= _mapPosition.MaxH)
				{
					var index = GetIndex(row, col, height);
					if (index < _mapData.Length)
						return _mapData[index];
				}
				return null;
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
