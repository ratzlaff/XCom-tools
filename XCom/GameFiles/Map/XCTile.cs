using System;
using System.Drawing;
using XCom.Interfaces;

namespace XCom
{
	public class XCTile : XCom.Interfaces.Base.ITile
	{
		private McdFile mcdFile;
		private PckFile myFile;
		private XCTile[] tiles;
		private McdEntry entry;
		private const int numImages = 8;

		public XCTile(int id, PckFile file, McdEntry info, McdFile mFile)
			: base(id, info)
		{
			this.entry = info;
			myFile = file;
			mcdFile = mFile;

			image = new PckImage[numImages];

			if (!info.UFODoor && !info.HumanDoor)
				MakeAnimate();
			else
				StopAnimate();

			Dead = null;
			Alternate = null;
		}

		public void MakeAnimate()
		{
			image[0] = myFile[entry.Image1];
			image[1] = myFile[entry.Image2];
			image[2] = myFile[entry.Image3];
			image[3] = myFile[entry.Image4];
			image[4] = myFile[entry.Image5];
			image[5] = myFile[entry.Image6];
			image[6] = myFile[entry.Image7];
			image[7] = myFile[entry.Image8];
		}

		public void StopAnimate()
		{
			image[0] = myFile[entry.Image1];
			image[1] = myFile[entry.Image1];
			image[2] = myFile[entry.Image1];
			image[3] = myFile[entry.Image1];
			image[4] = myFile[entry.Image1];
			image[5] = myFile[entry.Image1];
			image[6] = myFile[entry.Image1];
			image[7] = myFile[entry.Image1];
		}

		public XCTile[] Tiles
		{
			get { return tiles; }
			set
			{
				tiles = value;
				try {
					if (entry.DieTile != 0)
						Dead = tiles[entry.DieTile];
				} catch {
					if (MapID == 102 || MapID == 0)
						Dead = tiles[7];
					else
						Console.WriteLine("Error, could not set dead tile: {0} mapID:{1}", entry.DieTile, MapID);
				}

				if (entry.UFODoor || entry.HumanDoor || entry.Alt_MCD != 0)
					Alternate = tiles[entry.Alt_MCD];
			}
		}

		public XCTile Dead { get; set; }

		public XCTile Alternate { get; set; }
	}
}
