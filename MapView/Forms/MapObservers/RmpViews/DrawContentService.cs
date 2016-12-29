using System.Drawing;
using System.Drawing.Drawing2D;
using MapView.Forms.MapObservers.TopViews;
using XCom.Interfaces.Base;

namespace MapView.Forms.MapObservers.RmpViews
{
	public class DrawContentService
	{
		public DrawContentService()
		{
			_content = new GraphicsPath();
			_floor = new GraphicsPath();
		}

		public int HWidth  = 8;
		public int HHeight = 4;

		private readonly GraphicsPath _floor;
		private readonly GraphicsPath _content;
		private readonly ContentTypeService _contentTypeService = new ContentTypeService();

		public void DrawFloor(
							Graphics g,
							SolidBrush brush,
							int x, int y)
		{
			g.FillPath(brush, GetFloorPath(x, y));
		}

		public void DrawContent(
							Graphics g,
							SolidPenBrush color,
							int x, int y,
							TileBase content)
		{
			var contentType = _contentTypeService.GetContentType(content);
			var isDoor = _contentTypeService.IsDoor(content);

			const int TO_WALL_MARGIN = 4;

			var topCorner		= new Point(
										x,
										y + TO_WALL_MARGIN);
			var bottomCorner	= new Point(
										x,
										y + (HHeight * 2) - TO_WALL_MARGIN);
			var leftCorner		= new Point(
										x - HWidth + (TO_WALL_MARGIN * 2),
										y + HHeight);
			var rightCorner		= new Point(
										x + HWidth - (TO_WALL_MARGIN * 2),
										y + HHeight);

			switch (contentType)
			{
				case ContentTypes.Content:
					SetGroundPath(x, y);
					g.FillPath(
							color.Brush,
							_content);
					break;

				case ContentTypes.Ground:
					SetGroundPath(x, y);
					g.FillPath(
							color.LightBrush,
							_content);
					break;

				case ContentTypes.NorthFence:
					g.DrawLine(
							color.LightPen,
							topCorner,
							rightCorner);
					break;

				case ContentTypes.NorthWall:
					g.DrawLine(
							color.Pen,
							topCorner,
							rightCorner);

					if (isDoor)
						g.DrawLine(
								color.Pen,
								topCorner,
								Point.Add(rightCorner, new Size(-10, 4)));
					break;

				case ContentTypes.WestFence:
					g.DrawLine(
							color.LightPen,
							topCorner,
							leftCorner);
					break;

				case ContentTypes.WestWall:
					g.DrawLine(
							color.Pen,
							topCorner,
							leftCorner);

					if (isDoor)
						g.DrawLine(
								color.Pen,
								Point.Add(topCorner, new Size(6, 8)),
								leftCorner);
					break;

				case ContentTypes.NorthWallWithWindow:
					DrawWindow(
							g,
							color,
							topCorner,
							rightCorner);
					break;

				case ContentTypes.WestWallWithWindow:
					DrawWindow(
							g,
							color,
							topCorner,
							leftCorner);
					break;

				case ContentTypes.SouthWall:
					g.DrawLine(
							color.Pen,
							leftCorner,
							bottomCorner);
					break;

				case ContentTypes.EastWall:
					g.DrawLine(
							color.Pen,
							bottomCorner,
							rightCorner);
					break;

				case ContentTypes.NW_To_SE:
					g.DrawLine(
							color.Pen,
							topCorner,
							bottomCorner);
					break;

				case ContentTypes.NE_To_SW:
					g.DrawLine(
							color.Pen,
							leftCorner,
							rightCorner);
					break;

				case ContentTypes.NorthWestCorner:
					g.DrawLine(
							color.Pen,
							Point.Add(topCorner, new Size(-4, 0)),
							Point.Add(topCorner, new Size( 4, 0)));
					break;

				case ContentTypes.NorthEastCorner:
					g.DrawLine(
							color.Pen,
							Point.Add(rightCorner, new Size(0, -4)),
							Point.Add(rightCorner, new Size(0,  4)));
					break;

				case ContentTypes.SouthEastCorner:
					g.DrawLine(
							color.Pen,
							Point.Add(bottomCorner, new Size(-4, 0)),
							Point.Add(bottomCorner, new Size( 4, 0)));
					break;

				case ContentTypes.SouthWestCorner:
					g.DrawLine(
							color.Pen,
							Point.Add(leftCorner, new Size(0, -4)),
							Point.Add(leftCorner, new Size(0,  4)));
					break;
			}
		}

		private void SetGroundPath(int x, int y)
		{
			var w = HWidth  / 2;
			var h = HHeight / 2;

			y += h;

			_content.Reset();
			_content.AddLine(
							x, y,
							x + w, y + h);
			_content.AddLine(
							x + w, y + h,
							x, y + h * 2);
			_content.AddLine(
							x, y + h * 2,
							x - w, y + h);
			_content.CloseFigure();
		}

		private static void DrawWindow(
									Graphics g,
									SolidPenBrush color,
									Point start, Point end)
		{
			var dist = Point.Subtract(end, new Size(start));
			var size = new Size(dist.X / 3, dist.Y / 3);
			var point2 = Point.Add(start, Size.Add(size, size));

			g.DrawLine(color.Pen, start, Point.Add(start, size));
			g.DrawLine(color.LightPen, Point.Add(start, size), point2);
			g.DrawLine(color.Pen, point2, end);
		}

		private GraphicsPath GetFloorPath(int x, int y)
		{
			_floor.Reset();
			_floor.AddLine(
						x, y,
						x + HWidth, y + HHeight);
			_floor.AddLine(
						x + HWidth, y + HHeight,
						x, y + HHeight * 2);
			_floor.AddLine(
						x, y + HHeight * 2,
						x - HWidth, y + HHeight);
			_floor.CloseFigure();

			return _floor;
		}
	}
}
