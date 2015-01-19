using System;
using System.Drawing;
using XCom.Interfaces;

namespace XCom
{
    public class XCTile : XCom.Interfaces.Base.TileBase
    {
        private readonly PckFile _myFile;
        private readonly McdEntry _info;
        private const int NUM_IMAGES = 8;

        public XCTile(int id, PckFile file, McdEntry info, XCTile[] tiles) : base(id)
        {
            this.info = info;
            _info = info;
            Tiles = tiles;
            _myFile = file;

            image = new XCImage[NUM_IMAGES];

            if (!info.UFODoor && !info.HumanDoor)
                MakeAnimate();
            else
                StopAnimate();

            Dead = null;
            Alternate = null;
        }

        public XCTile[] Tiles { get; private set; }

        public XCTile Dead { get; set; }

        public XCTile Alternate { get; set; }

        public void MakeAnimate()
        {
            image[0] = _myFile[_info.Image1];
            image[1] = _myFile[_info.Image2];
            image[2] = _myFile[_info.Image3];
            image[3] = _myFile[_info.Image4];
            image[4] = _myFile[_info.Image5];
            image[5] = _myFile[_info.Image6];
            image[6] = _myFile[_info.Image7];
            image[7] = _myFile[_info.Image8];
        }

        public void StopAnimate()
        {
            image[0] = _myFile[_info.Image1];
            image[1] = _myFile[_info.Image1];
            image[2] = _myFile[_info.Image1];
            image[3] = _myFile[_info.Image1];
            image[4] = _myFile[_info.Image1];
            image[5] = _myFile[_info.Image1];
            image[6] = _myFile[_info.Image1];
            image[7] = _myFile[_info.Image1];
        }

    }
}