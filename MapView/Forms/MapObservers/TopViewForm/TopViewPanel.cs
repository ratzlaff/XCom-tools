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

        private SolidPenBrush _northColor;
        private SolidPenBrush _westColor;
        private SolidPenBrush _contentColor;

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
		            DrawContentService.DrawFloor(g, Brushes["GroundColor"],x,y);

		        if (_northColor == null)
		            _northColor = new SolidPenBrush(Pens["NorthColor"] );
		        if (mapTile.North != null && North.Checked)
                    DrawContentService.DrawContent(g, _northColor, x, y, mapTile.North);

		        if (_westColor == null)
		            _westColor = new SolidPenBrush(Pens["WestColor"] );
		        if (mapTile.West != null && West.Checked)
                    DrawContentService.DrawContent(g, _westColor, x, y, mapTile.West);

		        if (_contentColor == null)
                    _contentColor = new SolidPenBrush(Brushes["ContentColor"], _northColor.Pen.Width);
		        if (mapTile.Content != null && Content.Checked)
                    DrawContentService.DrawContent(g, _contentColor, x, y, mapTile.Content);
		    }
		    else
		    {
		        if (!mapTile.DrawAbove)
		        {
                    DrawContentService.DrawFloor(g, Brushes["GroundColor"], x, y);
                    DrawContentService.DrawContent(g, _contentColor, x, y, mapTile.Content);
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

