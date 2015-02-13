using System;
using System.Collections.Generic;
using System.Text;
using System.Drawing;
using XCom.Interfaces.Base;
using XCom;
using System.Windows.Forms;

namespace MapView.TopViewForm
{
	public class TopViewPanel : SimpleMapPanel
	{
	    private const bool BLANK = false;

	    public TopViewPanel()
	    {
	        MapViewPanel.Instance.View.DragChanged += ViewDrag;
	    }

	    public MenuItem Ground { get; set; }

	    public MenuItem North { get; set; }

	    public MenuItem West { get; set; }

	    public MenuItem Content { get; set; }

	    public BottomPanel BottomPanel { get; set; }

	    public int MinHeight
		{
			get { return MinimunHeight; }
			set { MinimunHeight = value; ParentSize(Width, Height); }
		}

		protected override void RenderCell(MapTileBase tile, Graphics g, int x, int y)
		{
			XCMapTile mapTile = (XCMapTile)tile;
			if (!BLANK)
			{
				if (mapTile.Ground != null && this.Ground.Checked)
					g.FillPath(Brushes["GroundColor"], UpperPath(x,y));

				if (mapTile.North != null && North.Checked)
					g.DrawLine(Pens["NorthColor"], x, y, x + HWidth, y + HHeight);

				if (mapTile.West != null && West.Checked)
					g.DrawLine(Pens["WestColor"], x, y, x - HWidth, y + HHeight);

				if (mapTile.Content != null && Content.Checked)
					g.FillPath(Brushes["ContentColor"], LowerPath(x,y));
			}
			else
			{
				if (!mapTile.DrawAbove)
				{
					g.FillPath(System.Drawing.Brushes.DarkGray, UpperPath(x, y));
					g.FillPath(System.Drawing.Brushes.DarkGray, LowerPath(x, y));
				}
			}
		}

		protected override void OnPaint(PaintEventArgs e)
		{
			base.OnPaint(e);

			ControlPaint.DrawBorder3D(e.Graphics, ClientRectangle,Border3DStyle.Etched);
		}

		protected override void OnMouseDown(MouseEventArgs e)
		{
			base.OnMouseDown(e);

            ViewDrag(null, e);
            if (e.Button == MouseButtons.Right)
		    {
		        BottomPanel.SetSelected(e.Button, 1);
            }
		}
	}
}
