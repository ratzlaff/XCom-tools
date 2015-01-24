using System;
using System.Collections.Generic;
using System.Drawing;
using System.Collections;
using XCom.Interfaces;
using XCom.Interfaces.Base;

namespace XCom
{
    public class XCMapTile : MapTileBase
    {
        public enum MapQuadrant
        {
            Ground,
            West,
            North,
            Content
        };

        private RmpEntry _rmpInfo;
        private TileBase _ground;
        private TileBase _north;
        private TileBase _west;
        private TileBase _content;
        private bool _blank;

        internal XCMapTile(TileBase ground, TileBase west, TileBase north, TileBase content)
        {
            _ground = ground;
            _north = north;
            _west = west;
            _content = content;

            DrawAbove = true;
            _blank = false;
        }

        public static XCMapTile BlankTile
        {
            get { return CreateBlankTile(); }
        }

        public bool Blank
        {
            get { return _blank; }
            set { _blank = value; }
        }

        public TileBase this[MapQuadrant quad]
        {
            get { return GetQuadrantTile(quad); }
            set { ChangeMapQuadrant(quad, value); }
        }

        public TileBase North
        {
            get { return _north; }
            set { ChangeMapQuadrant(MapQuadrant.North, value); } 
        }

        public TileBase Content
        {
            get { return _content; }
            set { ChangeMapQuadrant(MapQuadrant.Content, value); }
        }

        public TileBase Ground
        {
            get { return _ground; }
            set { ChangeMapQuadrant(MapQuadrant.Ground, value); }
        }

        public TileBase West
        {
            get { return _west; }
            set { ChangeMapQuadrant(MapQuadrant.West, value); }
        }

        public RmpEntry Rmp
        {
            get { return _rmpInfo; }
            set { _rmpInfo = value; }
        }

        public override TileBase[] UsedTiles
        {
            get
            {
                var list = new List<TileBase>();
                if (Ground != null) list.Add(Ground);
                if (West != null) list.Add(West);
                if (North != null) list.Add(North);
                if (Content != null) list.Add(Content);
                return list.ToArray();
            }
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
                    _ground = value;
                    break;
                case MapQuadrant.Content:
                    _content = value;
                    break;
                case MapQuadrant.North:
                    _north = value;
                    break;
                case MapQuadrant.West:
                    _west = value;
                    break;
            }
        }

        private static XCMapTile CreateBlankTile()
        {
            var mt = new XCMapTile(null, null, null, null);
            mt._blank = true;
            return mt;
        }
    }
}
