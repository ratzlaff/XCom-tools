using System;
using System.Drawing;
using System.Collections;
using XCom.Interfaces;
using XCom.Interfaces.Base;

namespace XCom
{
    public class XCMapTile : IMapTile
    {
        public enum MapQuadrant
        {
            Ground,
            West,
            North,
            Content
        };

        private RmpEntry rmpInfo;
        private TileBase ground;
        private TileBase north;
        private TileBase west;
        private TileBase content;
        private bool blank;

        internal XCMapTile(TileBase ground, TileBase west, TileBase north, TileBase content)
        {
            this.ground = ground;
            this.north = north;
            this.west = west;
            this.content = content;

            drawAbove = true;
            blank = false;
        }

        public static XCMapTile BlankTile
        {
            get { return CreateBlankTile(); }
        }

        public bool Blank
        {
            get { return blank; }
            set { blank = value; }
        }

        public TileBase this[MapQuadrant quad]
        {
            get { return GetQuadrantTile(quad); }
            set { ChangeMapQuadrant(quad, value); }
        }

        public TileBase North
        {
            get { return north; }
            set { ChangeMapQuadrant(MapQuadrant.North, value); } 
        }

        public TileBase Content
        {
            get { return content; }
            set { ChangeMapQuadrant(MapQuadrant.Content, value); }
        }

        public TileBase Ground
        {
            get { return ground; }
            set { ChangeMapQuadrant(MapQuadrant.Ground, value); }
        }

        public TileBase West
        {
            get { return west; }
            set { ChangeMapQuadrant(MapQuadrant.West, value); }
        }

        public RmpEntry Rmp
        {
            get { return rmpInfo; }
            set { rmpInfo = value; }
        }

        private TileBase GetQuadrantTile(MapQuadrant quad)
        {
            switch (quad)
            {
                case MapQuadrant.Ground:
                    return Ground;
                case MapQuadrant.Content:
                    return Content;
                case MapQuadrant.North:
                    return North;
                case MapQuadrant.West:
                    return West;
                default:
                    return null;
            }
        }

        private void ChangeMapQuadrant(MapQuadrant quad, TileBase value)
        {
            switch (quad)
            {
                case MapQuadrant.Ground:
                    Ground = value;
                    break;
                case MapQuadrant.Content:
                    Content = value;
                    break;
                case MapQuadrant.North:
                    North = value;
                    break;
                case MapQuadrant.West:
                    West = value;
                    break;
            }
        }

        private static XCMapTile CreateBlankTile()
        {
            var mt = new XCMapTile(null, null, null, null);
            mt.blank = true;
            return mt;
        }
    }
}
