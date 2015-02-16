using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Text;
using MapView.Forms.MapObservers.TopViewForm;
using MapView.TopViewForm;
using XCom.Interfaces.Base;

namespace MapView.Forms.MapObservers.RmpViewForm
{
    public class DrawContentService
    {
        public DrawContentService()
        {
            _content = new GraphicsPath();
            _floor = new GraphicsPath();
        }

        public int HWidth = 8;
        public int HHeight = 4;

        private readonly GraphicsPath _floor;
        private readonly GraphicsPath _content;
        private readonly ContentTypeService _contentTypeService = new ContentTypeService();

        public void DrawFloor(Graphics g, SolidBrush brush, int x, int y)
        {
            g.FillPath(brush, GetFloorPath(x, y));
        }

        public void DrawContent(Graphics g, SolidPenBrush color, int x, int y, TileBase content)
        {
            var contentType = _contentTypeService.GetContentType(content);
            var isDoor = _contentTypeService.IsDoor(content);
            var toWallMargin = 4;
            var topCorner = new Point(x, y + toWallMargin);
            var bottomCorner = new Point(x, y + (HHeight * 2) - toWallMargin);
            var leftCorner = new Point(x - HWidth + (toWallMargin * 2), y + HHeight);
            var rightCorner = new Point(x + HWidth - (toWallMargin * 2), y + HHeight);

            if (contentType == ContentTypes.Content)
            {
                var w = HWidth / 2;
                var h = HHeight / 2;
                y += h;
                _content.Reset();
                _content.AddLine(x, y, x + w, y + h);
                _content.AddLine(x + w, y + h, x, y + h * 2);
                _content.AddLine(x, y + h * 2, x - w, y + h);
                _content.CloseFigure();
                g.FillPath(color.Brush, _content);
            }
            else if (contentType == ContentTypes.NorthWall)
            {
                g.DrawLine(color.Pen, topCorner, rightCorner);
                if (isDoor)
                {
                    g.DrawLine(color.Pen, topCorner, Point.Add(rightCorner, new Size(-10, 4)));
                }
            }
            else if (contentType == ContentTypes.WestWall)
            {
                g.DrawLine(color.Pen, topCorner, leftCorner);
                if (isDoor)
                {
                    g.DrawLine(color.Pen, Point.Add(topCorner, new Size(6, 8)), leftCorner);
                }
            }
            else if (contentType == ContentTypes.NorthWallWithWindow)
            {
                DrawWindow(g, color, topCorner, rightCorner);
            }
            else if (contentType == ContentTypes.WestWallWithWindow)
            {
                DrawWindow(g, color, topCorner, leftCorner);
            }
            else if (contentType == ContentTypes.SouthWall)
            {
                g.DrawLine(color.Pen, leftCorner, bottomCorner);
            }
            else if (contentType == ContentTypes.EastWall)
            {
                g.DrawLine(color.Pen, bottomCorner, rightCorner);
            }
            else if (contentType == ContentTypes.NW_To_SE)
            {
                g.DrawLine(color.Pen, topCorner, bottomCorner);
            }
            else if (contentType == ContentTypes.NorthWestCorner)
            {
                g.DrawLine(color.Pen,
                    Point.Add(topCorner, new Size(-4, 0)),
                    Point.Add(topCorner, new Size(4, 0)));
            }
            else if (contentType == ContentTypes.NE_To_SW)
            {
                g.DrawLine(color.Pen, leftCorner, rightCorner);
            }

        }

        private static void DrawWindow(Graphics g, SolidPenBrush color, Point start, Point end)
        {
            var dist = Point.Subtract(end, new Size(start));
            var size = new Size(dist.X / 3, dist.Y / 3);
            var point2 = Point.Add(start, Size.Add(size, size));
            g.DrawLine(color.Pen, start, Point.Add(start, size));
            var c = color.Pen.Color;
            color.Pen.Color = Color.FromArgb(50, c);
            g.DrawLine(color.Pen, Point.Add(start, size), point2);
            color.Pen.Color = c;
            g.DrawLine(color.Pen, point2, end);
        }

        private GraphicsPath GetFloorPath(int x, int y)
        {

            _floor.Reset();
            _floor.AddLine(x, y, x + HWidth, y + HHeight);
            _floor.AddLine(x + HWidth, y + HHeight, x, y + HHeight * 2);
            _floor.AddLine(x, y + HHeight * 2, x - HWidth, y + HHeight);
            _floor.CloseFigure();
            return _floor;
        }


    }
}
