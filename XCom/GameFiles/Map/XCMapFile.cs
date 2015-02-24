using System;
using System.IO;
using System.Collections;
using System.Drawing;
using System.Windows.Forms;
using System.Collections.Generic;
//using XCom.Interfaces;
using XCom.Interfaces.Base;
using XCom.Services;

namespace XCom
{
	public class XCMapFile : IMap_Base
	{
	    private readonly string _blankPath;
	    private readonly string[] _dependencies;

		public XCMapFile(string baseName, string basePath, string blankPath, List<TileBase> tiles, string[] depList, RmpFile rmp)
			: base(baseName, tiles)
		{
			BaseName = baseName;
			BasePath = basePath;
			_blankPath = blankPath;
			_dependencies = depList;
		    Rmp = rmp;

		    var filePath = basePath + baseName + ".MAP";
		    if (!File.Exists(filePath))
		    {
		        throw new FileNotFoundException(filePath);
		    }

		    for (int i = 0; i < tiles.Count; i++)
				tiles[i].MapId = i;

            ReadMap(File.OpenRead(filePath), tiles);
		   
            SetupRoutes(rmp);

		    if (File.Exists(blankPath + baseName + BlankFile.Extension))
			{
				try
				{
					BlankFile.LoadBlanks(baseName, blankPath, this);
				}
				catch
				{
					for (int h = 0; h < MapSize.Height; h++)
						for (int r = 0; r < MapSize.Rows; r++)
							for (int c = 0; c < MapSize.Cols; c++)
								this[r, c, h].DrawAbove = true;
				}
			}
            else if (!string.IsNullOrEmpty(blankPath))
			{
				CalcDrawAbove();
				SaveBlanks();
			}
		}

	    public string BaseName { get; private set; }
        public string BasePath { get; private set; }

		public void Hq2x()
		{
			//instead, i would want to make an image of the whole map, and run that through hq2x
			foreach (string s in _dependencies)
				foreach (PckImage pi in GameInfo.GetPckFile(s))
					pi.Hq2x();

			PckImage.Width *= 2;
			PckImage.Height *= 2;
		}

		public RmpEntry AddRmp(MapLocation loc)
		{
			RmpEntry re = Rmp.AddEntry((byte)loc.Row, (byte)loc.Col, (byte)loc.Height);
			((XCMapTile)this[re.Row, re.Col, re.Height]).Rmp = re;
			return re;
		}

		public string[] Dependencies
		{
			get { return _dependencies; }
		}

		public void SaveBlanks()
		{
			BlankFile.SaveBlanks(BaseName, _blankPath, this);
		}

		public void CalcDrawAbove()
		{
            for (int h = MapSize.Height - 1; h >= 0; h--)
            {
                for (int row = 0; row < MapSize.Rows - 2; row++)
                {
                    for (int col = 0; col < MapSize.Cols -2; col++)
                    {
                        if (this[row, col, h] == null) continue;
                        if (h - 1 < 0) continue;
                        var mapTileHl1 = (XCMapTile)this[row, col, h - 1];
                        if (mapTileHl1 != null && mapTileHl1.Ground != null && //top
                            ((XCMapTile)this[row + 1, col, h - 1]).Ground != null && //south
                            ((XCMapTile)this[row + 2, col, h - 1]).Ground != null &&
                            ((XCMapTile)this[row + 1, col + 1, h - 1]).Ground != null && //southeast
                            ((XCMapTile)this[row + 2, col + 1, h - 1]).Ground != null &&
                            ((XCMapTile)this[row + 2, col + 2, h - 1]).Ground != null &&
                            ((XCMapTile)this[row, col + 1, h - 1]).Ground != null && //east
                            ((XCMapTile)this[row, col + 2, h - 1]).Ground != null &&
                            ((XCMapTile)this[row + 1, col + 2, h - 1]).Ground != null)
                            this[row, col, h].DrawAbove = false;
                    }
                }
            }
		}
         
		/// <summary>
		/// Writes a blank map to the Stream provided
		/// </summary>
		/// <param name="s"></param>
		/// <param name="rows"></param>
		/// <param name="cols"></param>
		/// <param name="height"></param>
		public static void NewMap(Stream s, byte rows, byte cols, byte height)
		{
			BinaryWriter bw = new BinaryWriter(s);
			bw.Write(rows);
			bw.Write(cols);
			bw.Write(height);
			for (int h = 0; h < height; h++)
				for (int r = 0; r < rows; r++)
					for (int c = 0; c < cols; c++)
						bw.Write((int)0);

			bw.Flush();
			bw.Close();
		}

	    public override void Save()
	    {
	        using (var s = File.Create(BasePath + BaseName + ".MAP"))
            {
                // add extraHeight to release mode 
                if (Rmp.ExtraHeight != 0)
                {
                    foreach (RmpEntry route in Rmp)
                    {
                        route.Height += Rmp.ExtraHeight;
                    }
                }

	            Rmp.Save();
	            s.WriteByte((byte) MapSize.Rows);
	            s.WriteByte((byte) MapSize.Cols);
	            s.WriteByte((byte) MapSize.Height);

	            for (int h = 0; h < MapSize.Height; h++)
	                for (int r = 0; r < MapSize.Rows; r++)
	                    for (int c = 0; c < MapSize.Cols; c++)
	                    {
	                        var xcmt = (XCMapTile) this[r, c, h];
	                        if (xcmt.Ground == null)
	                            s.WriteByte(0);
	                        else
	                            s.WriteByte((byte) (xcmt.Ground.MapId + 2));

	                        if (xcmt.West == null)
	                            s.WriteByte(0);
	                        else
	                            s.WriteByte((byte) (xcmt.West.MapId + 2));

	                        if (xcmt.North == null)
	                            s.WriteByte(0);
	                        else
	                            s.WriteByte((byte) (xcmt.North.MapId + 2));

	                        if (xcmt.Content == null)
	                            s.WriteByte(0);
	                        else
	                            s.WriteByte((byte) (xcmt.Content.MapId + 2));

	                    }
                s.WriteByte(Rmp.ExtraHeight);
	            s.Close();
	        }
	        MapChanged = false;
	    }

        private void ReadMap(Stream s, List<TileBase> tiles)
        {
            var input = new BufferedStream(s);
            var rows = input.ReadByte();
            var cols = input.ReadByte();
            var height = input.ReadByte();

            MapSize = new MapSize(rows, cols, height);

            //map = new MapTile[rows,cols,height];
            MapData = new MapTileList(rows, cols, height);

            for (int h = 0; h < height; h++)
                for (int r = 0; r < rows; r++)
                    for (int c = 0; c < cols; c++)
                    {
                        int q1 = input.ReadByte();
                        int q2 = input.ReadByte();
                        int q3 = input.ReadByte();
                        int q4 = input.ReadByte();

                        this[r, c, h] = createTile(tiles, q1, q2, q3, q4);
                    }
            if (input.Position < input.Length)
            {
                Rmp.ExtraHeight = (byte) input.ReadByte();
            }
            input.Close();
        }

	    public RmpFile Rmp { get; private set; }

	    public override void ResizeTo(int newR, int newC, int newH, bool addHeightToCelling)
        {
            var mapResizeService = new MapResizeService();
            var newMap = mapResizeService.ResizeMap(newR, newC, newH, MapSize, MapData, addHeightToCelling);
            if (newMap == null) return;

            // update Routes /
            if (addHeightToCelling && newH != MapSize.Height)
            {
                var heighDif = (newH - MapSize.Height); 
                foreach (RmpEntry rmp in Rmp)
                {
                    if (newH < MapSize.Height)
                        rmp.Height = (byte)(rmp.Height + heighDif);
                    else
                        rmp.Height += (byte)heighDif;
                }
            }

            // Remove routes outside the range 
            if (newH < MapSize.Height)
            {
                Rmp.TrimHeightTo(newH);
            }

            MapData = newMap;
            MapSize = new MapSize(newR, newC, newH);
            CurrentHeight = (byte)(MapSize.Height - 1);
            MapChanged = true;
        }

		private XCMapTile createTile(List<TileBase> tiles, int q1, int q2, int q3, int q4)
		{
			try
			{
				XCTile a, b, c, d;
				a = b = c = d = null;
				if (q1 != 0 && q1 != 1)
					a = (XCTile)tiles[q1 - 2];
				if (q2 != 0 && q2 != 1)
					b = (XCTile)tiles[q2 - 2];
				if (q3 != 0 && q3 != 1)
					c = (XCTile)tiles[q3 - 2];
				if (q4 != 0 && q4 != 1)
					d = (XCTile)tiles[q4 - 2];

				return new XCMapTile(a, b, c, d);
			}
			catch
			{
				//Console.WriteLine("Error in Map::createTile, indexes: {0},{1},{2},{3} length: {4}",q1,q2,q3,q4,tiles.Length);
				return XCMapTile.BlankTile;
			}
		}

	    public string GetDependecyName(TileBase selectedTile)
	    {
	        var dependencyId = -1;
	        foreach (var tile in Tiles)
	        {
	            if (tile.Id == 0) dependencyId ++;
	            if (tile == selectedTile) break;
	        }
	        if (dependencyId == -1 ||
	            dependencyId >= _dependencies.Length) return null;
	        return _dependencies[dependencyId];
	    }

        private void SetupRoutes(RmpFile rmp)
        {
            // remove extraHeight from edit mode 
            if (rmp.ExtraHeight != 0)
            {
                foreach (RmpEntry route in rmp)
                {
                    route.Height -= rmp.ExtraHeight;
                }
            }

            // Set tile
            foreach (RmpEntry re in rmp)
            {
                var tile = this[re.Row, re.Col, re.Height];
                if (tile == null) continue;
                ((XCMapTile)tile).Rmp = re;
            }
        }
	}
}
