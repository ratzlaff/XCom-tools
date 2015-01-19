using System;
using System.Collections;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;
using XCom;
using XCom.Interfaces.Base;

namespace MapView
{
    public class TilePanel : Panel
    {
        private ITile[] tiles;

        private int space=2;
        private int height = 40,width=32;
        private SolidBrush brush = new SolidBrush(Color.FromArgb(204,204,255));
        private Pen pen = new Pen(Brushes.Red,3);
        private int startY=0;
        private int selectedNum;
        private VScrollBar vert;
        private int numAcross=1;

        public static readonly Color[] tileTypes={Color.Cornsilk,Color.Lavender,Color.DarkRed,Color.Fuchsia,Color.Aqua,
            Color.DarkOrange,Color.DeepPink,Color.LightBlue,Color.Lime,
            Color.LightGreen,Color.MediumPurple,Color.LightCoral,Color.LightCyan,
            Color.Yellow,Color.Blue};
        private TileType type;

        public event SelectedTileChanged TileChanged;
        //private static PckFile extraFile;
        private static Hashtable brushes;

        //public static PckFile ExtraFile
        //{
        //    get{return extraFile;}
        //    set{extraFile=value;}
        //}

        public static Hashtable Colors
        {
            get{return brushes;}
            set{brushes=value;}
        }

        public TilePanel(TileType type)
        {
            this.type = type;
            vert = new VScrollBar();
            vert.ValueChanged += valChange;
            vert.Location = new Point(Width - vert.Width, 0);
            
            Controls.Add(vert);
            MapViewPanel.ImageUpdate += tick;

            SetStyle(ControlStyles.AllPaintingInWmPaint | ControlStyles.DoubleBuffer | ControlStyles.UserPaint, true);
            selectedNum = 0;

            Globals.LoadExtras();
        }

        private void valChange(object sender, EventArgs e)
        {
            startY = -vert.Value;
            Refresh();
        }

        protected override void OnResize(EventArgs e)
        {
            numAcross = (Width-(vert.Visible?vert.Width:0))/(width+2*space);
            vert.Location = new Point(Width-vert.Width,0);
            vert.Height=Height;
            vert.Maximum = Math.Max((PreferredHeight-Height)+10,vert.Minimum);	
		
            if(vert.Maximum==vert.Minimum)
                vert.Visible=false;
            else
                vert.Visible=true;
            Refresh();
        }

        public int StartY
        {
            get{return startY;}
            set{startY=value;Refresh();}
        }

        public int PreferredHeight
        {
            get
            {
                if(tiles!=null && numAcross>0)
                {
                    if(tiles.Length%numAcross==0)
                        return (tiles.Length/numAcross)*(height+2*space);

                    return (1+tiles.Length/numAcross)*(height+2*space);
                }
                return 0;
            }
        }

        public System.Collections.Generic.List<ITile> Tiles
        {
            set
            {
                if (value != null)
                {
                    if (type == TileType.All)
                    {
                        //tiles=value;
                        tiles = new ITile[value.Count + 1];
                        tiles[0] = null;
                        for (int i = 0; i < value.Count; i++)
                            tiles[i + 1] = value[i];
                    }
                    else
                    {
                        var list = new List<ITile>();
                        for (int i = 0; i < value.Count; i++)
                            if (value[i].Info.TileType == type)
                                list.Add(value[i]);
                        tiles = list.ToArray();
                    }
                }
                else
                {
                    tiles = null;
                }
                OnResize(null);
            }
        }

        private int scrollAmount=20;

        protected override void OnMouseWheel(MouseEventArgs e)
        {
            var handledMouseEventArgs = e as HandledMouseEventArgs;
            if (handledMouseEventArgs != null)
            {
                handledMouseEventArgs.Handled = true;
            }
            if(e.Delta<0)
                if(vert.Value+scrollAmount<vert.Maximum)
                    vert.Value+=scrollAmount;
                else
                    vert.Value = vert.Maximum;
            else if(e.Delta>0)
                if(vert.Value-scrollAmount>vert.Minimum)
                    vert.Value-=scrollAmount;
                else
                    vert.Value = vert.Minimum;
        }

        protected override void OnMouseDown(MouseEventArgs e)
        {
            this.Focus();
            if (tiles == null) return;
            int x = e.X/(width+2*space);
            int y = (e.Y-startY)/(height+2*space);

            if(x>=numAcross)
                x=numAcross-1;

            selectedNum = y*numAcross+x;

            selectedNum = (selectedNum<tiles.Length)?selectedNum:tiles.Length-1;
            if(TileChanged!=null)
                TileChanged(this,SelectedTile);
            Refresh();
        }

        protected override void OnPaint(PaintEventArgs e)
        {
            PaintTiles(e);
        }

        private void PaintTiles(PaintEventArgs e)
        {
            if (tiles == null) return;
            Graphics g = e.Graphics;

            int i = 0, j = 0;
            foreach (ITile t in tiles)
            {
                if (t != null && (type == TileType.All || t.Info.TileType == type))
                {
                    g.FillRectangle(
                        (SolidBrush) brushes[t.Info.TargetType.ToString()], i*(width + 2*space),
                        startY + j*(height + 2*space), width + 2*space, height + 2*space);
                    g.DrawImage(t[MapViewPanel.Current].Image, i*(width + 2*space),
                        startY + j*(height + 2*space) - t.Info.TileOffset);

                    if (t.Info.HumanDoor || t.Info.UFODoor)
                        g.DrawString("Door", this.Font, Brushes.Black, i*(width + 2*space),
                            startY + j*(height + 2*space) + PckImage.Height - Font.Height);

                    i = (i + 1)%numAcross;
                    if (i == 0)
                        j++;
                }
                else if (t == null)
                {
                    g.FillRectangle(
                        Brushes.AliceBlue, i * (width + 2 * space),
                        startY + j * (height + 2 * space), width + 2 * space, height + 2 * space);
                    if (Globals.ExtraTiles != null)
                        g.DrawImage(Globals.ExtraTiles[0].Image, i*(width + 2*space), startY + j*(height + 2*space));
                    i = (i + 1)%numAcross;
                    if (i == 0)
                        j++;
                }
            }

            //g.DrawRectangle(brush,(selectedNum%numAcross)*(width+2*space),startY+(selectedNum/numAcross)*(height+2*space),width+2*space,height+2*space)				

            for (int k = 0; k <= numAcross + 1; k++)
                g.DrawLine(Pens.Black, k*(width + 2*space), startY, k*(width + 2*space), startY + PreferredHeight);
            for (int k = 0; k <= PreferredHeight; k += (height + 2*space))
                g.DrawLine(Pens.Black, 0, startY + k, numAcross*(width + 2*space), startY + k);

            g.DrawRectangle(pen, (selectedNum%numAcross)*(width + 2*space),
                startY + (selectedNum/numAcross)*(height + 2*space), width + 2*space, height + 2*space);
        }

        public ITile SelectedTile
        {
            get
            {
                return tiles[selectedNum];
            }
            set
            {
                if(value==null)
                    selectedNum=0;
                else
                {
                    selectedNum = value.MapId+1;

                    if(TileChanged!=null)
                        TileChanged(this,SelectedTile);

                    int y = startY+(selectedNum/numAcross)*(height+2*space);
                    int val = -(startY-y);

                    if(val > vert.Minimum)
                    {
                        if(val < vert.Maximum)
                            vert.Value = val;
                        else
                            vert.Value = vert.Maximum;
                    }
                    else
                        vert.Value = vert.Minimum;
                }
            }
        }

        private void tick(object sender, EventArgs e)
        {
            Refresh();
        }
    }
}