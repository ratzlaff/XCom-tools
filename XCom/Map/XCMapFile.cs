using System;
using System.IO;
using System.Collections;
using System.Drawing;
using System.Windows.Forms;
using System.Collections.Generic;
using MapLib.Base;
using MapLib;
using MapLib.Base.Parsing;

namespace XCom
{
	public class XCMapFile : Map
	{
		private PckFile mCursor;
		private RmpFile rmpFile;
		private List<Tile> mTiles;

		public XCMapFile(MapInfo inInfo)
			: base(inInfo)
		{
			string basename = inInfo.Name;
			string basePath = inInfo.Tileset.MapCollection.RootPath;
			string blankPath = basePath;

			mTiles = new List<Tile>();
			foreach (ImageInfo img in inInfo.Dependencies.Data)
				if (img.Tiles != null)
					foreach (Tile t in img.Tiles)
						mTiles.Add(t);

			for (int i = 0; i < mTiles.Count; i++)
				mTiles[i].Init(i);

			readMap(File.OpenRead(basePath + basename + ".MAP"));

			if (File.Exists(basePath + "..\\UFOGRAPH\\CURSOR.TAB")) {
				Stream tabStream = File.OpenRead(basePath + "..\\UFOGRAPH\\CURSOR.TAB");
				Stream fileStream = File.OpenRead(basePath + "..\\UFOGRAPH\\CURSOR.PCK");
				mCursor = new PckFile(null, fileStream, tabStream, XCom.XCPalette.UFOBattle, 40, 32);
			}

			if (File.Exists(blankPath + basename + BlankFile.Extension)) {
				try {
					BlankFile.LoadBlanks(basename, blankPath, this);
				} catch {
					for (int h = 0; h < mapSize.Height; h++)
						for (int r = 0; r < mapSize.Rows; r++)
							for (int c = 0; c < mapSize.Cols; c++)
								((XCMapTile)this[r, c, h]).DrawAbove = true;
				}
			} else if (blankPath != "" && Globals.UseBlanks) {
				CalcDrawAbove();
				SaveBlanks();
			}
		}

		public override ImageCollection Cursor
		{
			get { return mCursor; }
		}
		/*
				public void Hq2x()
				{
					//instead, i would want to make an image of the whole map, and run that through hq2x
					foreach (string s in dependencies)
						foreach (PckImage pi in GameInfo.GetPckFile(s))
							pi.Hq2x();

					PckImage.Width *= 2;
					PckImage.Height *= 2;
				}
		*/
		public override List<Tile> Tiles
		{
			get { return mTiles; }
		}

		public RmpEntry AddRmp(MapLocation loc)
		{
			RmpEntry re = Rmp.AddEntry((byte)loc.Row, (byte)loc.Col, (byte)loc.Height);
			((XCMapTile)this[re.Row, re.Col, re.Height]).Rmp = re;
			return re;
		}

		public override MapTile BlankTile()
		{
			return XCMapTile.BlankTile;
		}

		public void SaveBlanks()
		{
			throw new NotImplementedException();
//			BlankFile.SaveBlanks(basename, blankPath, this);
		}

		public void CalcDrawAbove()
		{
			for (int h = mapSize.Height - 1; h >= 0; h--)
				for (int row = 0; row < mapSize.Rows; row++) {
					for (int col = 0; col < mapSize.Cols; col++) {
						if (this[row, col, h] == null)
							continue;

						try {
							if (((XCMapTile)this[row, col, h - 1]).Ground != null && //top
								((XCMapTile)this[row + 1, col, h - 1]).Ground != null && //south
								((XCMapTile)this[row + 2, col, h - 1]).Ground != null &&
								((XCMapTile)this[row + 1, col + 1, h - 1]).Ground != null && //southeast
								((XCMapTile)this[row + 2, col + 1, h - 1]).Ground != null &&
								((XCMapTile)this[row + 2, col + 2, h - 1]).Ground != null &&
								((XCMapTile)this[row, col + 1, h - 1]).Ground != null && //east
								((XCMapTile)this[row, col + 2, h - 1]).Ground != null &&
								((XCMapTile)this[row + 1, col + 2, h - 1]).Ground != null)
								((XCMapTile)this[row, col, h]).DrawAbove = false;
						} catch { }
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
			throw new NotImplementedException();
//			Save(File.Create(basePath + basename + ".MAP"));
		}

		public override void Save(FileStream s)
		{
			rmpFile.Save();
			s.WriteByte((byte)mapSize.Rows);
			s.WriteByte((byte)mapSize.Cols);
			s.WriteByte((byte)mapSize.Height);

			for (int h = 0; h < mapSize.Height; h++)
				for (int r = 0; r < mapSize.Rows; r++)
					for (int c = 0; c < mapSize.Cols; c++) {
						XCMapTile xcmt = (XCMapTile)this[r, c, h];
						if (xcmt.Ground == null)
							s.WriteByte(0);
						else
							s.WriteByte((byte)(((XCTile)xcmt.Ground).MapID + 2));

						if (xcmt.West == null)
							s.WriteByte(0);
						else
							s.WriteByte((byte)(((XCTile)xcmt.West).MapID + 2));

						if (xcmt.North == null)
							s.WriteByte(0);
						else
							s.WriteByte((byte)(((XCTile)xcmt.North).MapID + 2));

						if (xcmt.Content == null)
							s.WriteByte(0);
						else
							s.WriteByte((byte)(((XCTile)xcmt.Content).MapID + 2));

					}
			s.Close();
		}

		public RmpFile Rmp
		{
			get { return rmpFile; }
			set
			{
				rmpFile = value;
				foreach (RmpEntry re in rmpFile) {
					XCMapTile tile = ((XCMapTile)this[re.Row, re.Col, re.Height]);
					if (tile != null) {
						xConsole.AddLine("Could not set rmp entry! [" + re.ToString() + "]");
						tile.Rmp = re;
					}
				}
			}
		}

		private void readMap(Stream s)
		{
			BufferedStream input = new BufferedStream(s);
			int rows = input.ReadByte();
			int cols = input.ReadByte();
			int height = input.ReadByte();

			mapSize = new MapSize(rows, cols, height);

			//map = new MapTile[rows,cols,height];
			mapData = new XCMapTile[rows * cols * height];

			for (int h = 0; h < height; h++)
				for (int r = 0; r < rows; r++)
					for (int c = 0; c < cols; c++) {
						int q1 = input.ReadByte();
						int q2 = input.ReadByte();
						int q3 = input.ReadByte();
						int q4 = input.ReadByte();

						this[r, c, h] = createTile(q1, q2, q3, q4);
					}
			input.Close();
		}

		private XCMapTile createTile(int q1, int q2, int q3, int q4)
		{
			try {
				Tile a, b, c, d;
				a = b = c = d = null;
				if (q1 != 0 && q1 != 1)
					a = mTiles[q1 - 2];
				if (q2 != 0 && q2 != 1)
					b = mTiles[q2 - 2];
				if (q3 != 0 && q3 != 1)
					c = mTiles[q3 - 2];
				if (q4 != 0 && q4 != 1)
					d = mTiles[q4 - 2];

				return new XCMapTile(a, b, c, d);
			} catch {
				//Console.WriteLine("Error in Map::createTile, indexes: {0},{1},{2},{3} length: {4}",q1,q2,q3,q4,tiles.Length);
				return XCMapTile.BlankTile;
			}
		}
	}
}
