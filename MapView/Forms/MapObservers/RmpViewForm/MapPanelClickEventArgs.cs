using System;
using System.Windows.Forms;
using XCom;
using XCom.Interfaces.Base;

namespace MapView.RmpViewForm
{
    public class MapPanelClickEventArgs:EventArgs
    {
        public MapLocation ClickLocation { get; set; }

        public MapTileBase ClickTile { get; set; }

        public MouseEventArgs MouseEventArgs { get; set; }
    }
}