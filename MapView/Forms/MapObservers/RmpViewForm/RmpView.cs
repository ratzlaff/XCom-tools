using System;
using System.Drawing;
using System.Collections.Generic;
using System.ComponentModel;
using System.Windows.Forms;
using XCom;
using XCom.Interfaces;
using XCom.Interfaces.Base;

namespace MapView.RmpViewForm
{

    /*
		TFTD ---- UFO
		Commander ---- Commander
		Navigator ---- Leader
		Medic ---- Engineer
		Technition ---- Medic
		SquadLeader ---- Navigator 
		Soldier ---- Soldier
	*/

    //public delegate void RmpClick(int row,int col,MouseButtons button);

    public partial class RmpView : Map_Observer_Form
    {
        private XCMapFile map;
        private RmpPanel rmpPanel;
        private Label locInfo;
        private Label links;
        private int clickRow, clickCol;

        private RmpEntry currEntry;
        private Panel contentPane;
         
        private bool loadingGUI = false;

        private readonly List<object> _byteList = new List<object>();

        private readonly object[] _items2 =
        {
            LinkTypes.ExitEast, LinkTypes.ExitNorth, LinkTypes.ExitSouth, LinkTypes.ExitWest,
            LinkTypes.NotUsed
        };

        public RmpView()
        {
            clickRow = clickCol = 0;
            InitializeComponent();

            rmpPanel = new RmpPanel();
            contentPane.Controls.Add(rmpPanel);
            rmpPanel.MapPanelClicked += new MapPanelClickDelegate(panelClick);
            rmpPanel.MouseMove += new MouseEventHandler(mouseMove);
            rmpPanel.Dock = DockStyle.Fill;

            locInfo = new Label();
            links = new Label();

            locInfo.Width = 100;
            locInfo.Height = ClientSize.Height - rmpPanel.Height;

            links.Height = locInfo.Height;
            links.Width = ClientSize.Width - locInfo.Width;

            locInfo.Top = rmpPanel.Bottom;
            locInfo.Left = 0;

            links.Top = locInfo.Top;
            links.Left = locInfo.Right;

            links.BorderStyle = BorderStyle.Fixed3D;

            //this.Menu = new MainMenu();
            //MenuItem f = new MenuItem("Edit");
            //Menu.MenuItems.Add(f);
            //f.MenuItems.Add("Options",new EventHandler(options_click));

            object[] uTypeItms = new object[]
            {UnitType.Any, UnitType.Flying, UnitType.FlyingLarge, UnitType.Large, UnitType.Small};
            cbType.Items.AddRange(uTypeItms);

            cbUse1.Items.AddRange(uTypeItms);
            cbUse2.Items.AddRange(uTypeItms);
            cbUse3.Items.AddRange(uTypeItms);
            cbUse4.Items.AddRange(uTypeItms);
            cbUse5.Items.AddRange(uTypeItms);

            cbType.DropDownStyle = ComboBoxStyle.DropDownList;

            cbUse1.DropDownStyle = ComboBoxStyle.DropDownList;
            cbUse2.DropDownStyle = ComboBoxStyle.DropDownList;
            cbUse3.DropDownStyle = ComboBoxStyle.DropDownList;
            cbUse4.DropDownStyle = ComboBoxStyle.DropDownList;
            cbUse5.DropDownStyle = ComboBoxStyle.DropDownList;

            //object[] itms = {UnitRank.Civilian0,UnitRank.XCom1,UnitRank.Soldier2,UnitRank.Navigator3,UnitRank.LeaderCommander4,UnitRank.Engineer5,UnitRank.Misc1,UnitRank.Medic7,UnitRank.Misc2};
            object[] itms2 =
            {
                UnitRankNum.Zero, UnitRankNum.One, UnitRankNum.Two, UnitRankNum.Three, UnitRankNum.Four,
                UnitRankNum.Five, UnitRankNum.Six, UnitRankNum.Seven, UnitRankNum.Eight
            };

            cbRank1.Items.AddRange(RmpFile.UnitRankUFO);
            cbRank1.DropDownStyle = ComboBoxStyle.DropDownList;

            cbRank2.Items.AddRange(itms2);
            cbRank2.DropDownStyle = ComboBoxStyle.DropDownList;

            cbLink1.DropDownStyle = ComboBoxStyle.DropDownList;
            cbLink2.DropDownStyle = ComboBoxStyle.DropDownList;
            cbLink3.DropDownStyle = ComboBoxStyle.DropDownList;
            cbLink4.DropDownStyle = ComboBoxStyle.DropDownList;
            cbLink5.DropDownStyle = ComboBoxStyle.DropDownList;

            cbUsage.DropDownStyle = ComboBoxStyle.DropDownList;
            cbUsage.Items.AddRange(RmpFile.SpawnUsage);

            currEntry = null;

            Text = "RmpView";
        }

        private void options_click(object sender, EventArgs e)
        {
            var pf = new PropertyForm("rmpViewOptions", Settings);
            pf.Text = "RmpView Settings";
            pf.Show();
        }

        private void brushChanged(object sender, string key, object val)
        {
            rmpPanel.Brushes[key].Color = (Color) val;
            Refresh();
        }

        private void penColorChanged(object sender, string key, object val)
        {
            rmpPanel.Pens[key].Color = (Color) val;
            Refresh();
        }

        private void penWidthChanged(object sender, string key, object val)
        {
            rmpPanel.Pens[key].Width = (int) val;
            Refresh();
        }
        
        private void mouseMove(object sender, MouseEventArgs e)
        {
            XCMapTile t = rmpPanel.GetTile(e.X, e.Y);
            if (t != null && t.Rmp != null)
            {
                lblMouseOver.Text = "Mouse Over: " + t.Rmp.Index;
            }
            else
            {
                lblMouseOver.Text = "";
            }
            rmpPanel.Position.X = e.X;
            rmpPanel.Position.Y = e.Y;
            rmpPanel.Refresh();
        }

        private void panelClick(object sender, MapPanelClickEventArgs e)
        {
            idxLabel.Text = this.Text;
            try
            {
                if (currEntry != null && e.MouseEventArgs.Button == MouseButtons.Right)
                {
                    RmpEntry selEntry = ((XCMapTile) e.ClickTile).Rmp;
                    if (selEntry != null)
                    {
                        var spaceAt = CompareLinks(currEntry, selEntry.Index);
                        if (spaceAt.HasValue)
                        {
                            currEntry[spaceAt.Value].Index = selEntry.Index;
                            currEntry[spaceAt.Value].Distance = calcLinkDistance(currEntry, selEntry, null);
                        }
                         
                        ExecuteAutoconnectNodes(selEntry);

                        FillGui();
                        Refresh();
                        return;
                    }
                }


                currEntry = ((XCMapTile) e.ClickTile).Rmp;
                if (currEntry == null && e.MouseEventArgs.Button == MouseButtons.Right)
                {
                    if (map is XCMapFile)
                    {
                        RmpEntry prevEntry = (((XCMapFile) map).Rmp.Length > 0
                            ? ((XCMapFile) map).Rmp[((XCMapFile) map).Rmp.Length - 1]
                            : null);

                        currEntry = ((XCMapFile) map).AddRmp(e.ClickLocation);
                        if (AutoconnectNodes.Checked)
                        {
                            currEntry[0].Index = (byte) (((XCMapFile) map).Rmp.Length - 2);
                            if (prevEntry != null)
                            {
                                currEntry[0].Distance = calcLinkDistance(currEntry, prevEntry, txtDist1);
                                for (int pI = 0; pI < prevEntry.NumLinks; pI++)
                                {
                                    if (prevEntry[pI].Index == 0xFF)
                                    {
                                        prevEntry[pI].Index = (byte) (((XCMapFile) map).Rmp.Length - 1);
                                        prevEntry[pI].Distance = calcLinkDistance(prevEntry, currEntry, null);
                                        break;
                                    }
                                }
                            }
                        }
                    }
                }


            }
            catch
            {
                return;
            }

            FillGui();
        }

        private void ExecuteAutoconnectNodes(RmpEntry selEntry)
        {
            if (!AutoconnectNodes.Checked) return;
            var spaceAt = CompareLinks(selEntry, currEntry.Index);
            if (spaceAt.HasValue)
            {
                selEntry[spaceAt.Value].Index = currEntry.Index;
                selEntry[spaceAt.Value].Distance = calcLinkDistance(selEntry, currEntry, null);
            }
        }

        private int? CompareLinks(RmpEntry list, int index)
        {
            var existingLink = false;
            var spaceAvailable = false;
            var spaceAt = 512;
            for (int i = 0; i < 5; i++)
            {
                if (list[i].Index == index)
                {
                    existingLink = true;
                    break;
                }
                if (list[i].Index == (byte) LinkTypes.NotUsed)
                {
                    spaceAvailable = true;
                    if (i < spaceAt)
                        spaceAt = i;
                }
            }
            if (existingLink || !spaceAvailable) return 0;
            return spaceAt;
        }

        private void FillGui()
        {
            if (currEntry == null)
            {
                gbNodeInfo.Enabled = false;
                return;
            }
            gbNodeInfo.Enabled = true;
            gbNodeInfo.SuspendLayout();

            loadingGUI = true;

            _byteList.Clear();

            cbLink1.Items.Clear();
            cbLink2.Items.Clear();
            cbLink3.Items.Clear();
            cbLink4.Items.Clear();
            cbLink5.Items.Clear();

            for (byte i = 0; i < map.Rmp.Length; i++)
            {
                if (i == currEntry.Index)
                    continue;

                _byteList.Add(i);
            }

            _byteList.AddRange(_items2);

            object[] bArr = _byteList.ToArray();

            cbLink1.Items.AddRange(bArr);
            cbLink2.Items.AddRange(bArr);
            cbLink3.Items.AddRange(bArr);
            cbLink4.Items.AddRange(bArr);
            cbLink5.Items.AddRange(bArr);

            cbType.SelectedItem = currEntry.UType;

            if (map.Tiles[0][0].Palette == Palette.UFOBattle)
                cbRank1.SelectedItem = RmpFile.UnitRankUFO[(byte) currEntry.URank1];
            else
                cbRank1.SelectedItem = RmpFile.UnitRankTFTD[(byte) currEntry.URank1];


            cbRank2.SelectedItem = currEntry.URank2;
            tbZero.Text = currEntry.Zero1 + "";
            cbUsage.SelectedItem = RmpFile.SpawnUsage[(byte) currEntry.Usage];

            idxLabel2.Text = "Index: " + currEntry.Index;

            if (currEntry[0].Index < 0xFB)
                cbLink1.SelectedItem = currEntry[0].Index;
            else
                cbLink1.SelectedItem = (LinkTypes) currEntry[0].Index;

            if (currEntry[1].Index < 0xFB)
                cbLink2.SelectedItem = currEntry[1].Index;
            else
                cbLink2.SelectedItem = (LinkTypes) currEntry[1].Index;

            if (currEntry[2].Index < 0xFB)
                cbLink3.SelectedItem = currEntry[2].Index;
            else
                cbLink3.SelectedItem = (LinkTypes) currEntry[2].Index;

            if (currEntry[3].Index < 0xFB)
                cbLink4.SelectedItem = currEntry[3].Index;
            else
                cbLink4.SelectedItem = (LinkTypes) currEntry[3].Index;

            if (currEntry[4].Index < 0xFB)
                cbLink5.SelectedItem = currEntry[4].Index;
            else
                cbLink5.SelectedItem = (LinkTypes) currEntry[4].Index;

            cbUse1.SelectedItem = currEntry[0].UType;
            cbUse2.SelectedItem = currEntry[1].UType;
            cbUse3.SelectedItem = currEntry[2].UType;
            cbUse4.SelectedItem = currEntry[3].UType;
            cbUse5.SelectedItem = currEntry[4].UType;

            txtDist1.Text = currEntry[0].Distance + "";
            txtDist2.Text = currEntry[1].Distance + "";
            txtDist3.Text = currEntry[2].Distance + "";
            txtDist4.Text = currEntry[3].Distance + "";
            txtDist5.Text = currEntry[4].Distance + "";

            gbNodeInfo.ResumeLayout();

            loadingGUI = false;
        }

        public void SetMap(object sender, SetMapEventArgs e)
        {
            Map = (XCMapFile) e.Map;
        }

        public override IMap_Base Map
        {
            set
            {
                base.Map = value;
                this.map = (XCMapFile) value;

                rmpPanel.Map = map;
                if (rmpPanel.Map != null)
                {
                    currEntry = ((XCMapTile) map[clickRow, clickCol]).Rmp;
                    FillGui();
                    cbRank1.Items.Clear();

                    if (map.Tiles[0][0].Palette == Palette.UFOBattle)
                        cbRank1.Items.AddRange(RmpFile.UnitRankUFO);
                    else
                        cbRank1.Items.AddRange(RmpFile.UnitRankTFTD);

                    //Text = string.Format("RmpView: r:{0} c:{1} h:{2}", rmpPanel.Map.MapSize.Rows, rmpPanel.Map.MapSize.Cols, rmpPanel.Map.MapSize.Height);
                    //rmpPanel.Map.HeightChanged += new HeightChangedDelegate(heightChanged);
                    //rmpPanel.Map.SelectedTileChanged += new SelectedTileChangedDelegate(tileChanged);
                }
            }
        }

        /*public IMapFile Map
		{
			set
			{
				if (map != null)
				{
					map.HeightChanged -= new HeightChangedDelegate(heightChanged);
					map.SelectedTileChanged -= new SelectedTileChangedDelegate(tileChanged);
				}

				if (value is XCMapFile)
					map = (XCMapFile)value;
				else
					return;

				rmpPanel.Map = map;
				if (rmpPanel.Map != null)
				{
					currEntry = ((XCMapTile)map[clickRow, clickCol]).Rmp;
					fillGUI();
					cbRank1.Items.Clear();

					if (map.Tiles[0][0].Palette == Palette.UFOBattle)
						cbRank1.Items.AddRange(RmpFile.UnitRankUFO);
					else
						cbRank1.Items.AddRange(RmpFile.UnitRankTFTD);

					Text = string.Format("RmpView: r:{0} c:{1} h:{2}", rmpPanel.Map.MapSize.Rows, rmpPanel.Map.MapSize.Cols, rmpPanel.Map.MapSize.Height);
					rmpPanel.Map.HeightChanged += new HeightChangedDelegate(heightChanged);
					rmpPanel.Map.SelectedTileChanged += new SelectedTileChangedDelegate(tileChanged);
				}
				OnResize(null);
			}
		}*/

        public override void SelectedTileChanged(IMap_Base sender, SelectedTileChangedEventArgs e)
        {
            this.Text = string.Format("RmpView: r:{0} c:{1} ", e.MapLocation.Row, e.MapLocation.Col);
        }

        public override void HeightChanged(IMap_Base sender, HeightChangedEventArgs e)
        {
            currEntry = ((XCMapTile) map[clickRow, clickCol]).Rmp;
            FillGui();
            Refresh();
        }

        private void cbType_SelectedIndexChanged(object sender, System.EventArgs e)
        {
            currEntry.UType = (UnitType) cbType.SelectedItem;
        }

        private void cbRank1_SelectedIndexChanged(object sender, System.EventArgs e)
        {
            currEntry.URank1 = (byte) ((StrEnum) cbRank1.SelectedItem).Enum;
        }

        private void cbRank2_SelectedIndexChanged(object sender, System.EventArgs e)
        {
            currEntry.URank2 = (UnitRankNum) cbRank2.SelectedItem;
        }

        private void tbZero_Leave(object sender, System.EventArgs e)
        {
            try
            {
                currEntry.Zero1 = byte.Parse(tbZero.Text);
            }
            catch
            {
                tbZero.Text = currEntry.Zero1 + "";
            }
        }

        private void tbZero_KeyDown(object sender, System.Windows.Forms.KeyEventArgs e)
        {
            switch (e.KeyCode)
            {
                case Keys.Enter:
                    try
                    {
                        currEntry.Zero1 = byte.Parse(tbZero.Text);
                    }
                    catch
                    {
                        tbZero.Text = currEntry.Zero1 + "";
                    }
                    break;
            }
        }

        private byte calcLinkDistance(RmpEntry from, RmpEntry to, TextBox result)
        {
            int dist =
                ((int)
                    Math.Sqrt(Math.Pow(from.Row - to.Row, 2) + Math.Pow(from.Col - to.Col, 2) +
                              Math.Pow(from.Height - to.Height, 2)));
            if (result != null)
            {
                result.Text = dist.ToString();
            }
            return (byte) dist;
        }

        private void cbLink_SelectedIndexChanged(ComboBox sender, int senderIndex, TextBox senderOut)
        {
            if (loadingGUI)
                return;

            byte? selIdx = sender.SelectedItem as byte?;
            if (!selIdx.HasValue)
            {
                selIdx = (byte?) (sender.SelectedItem as LinkTypes?);
            }

            if (!selIdx.HasValue)
            {
                MessageBox.Show("Error: Determining SelectedIndex value failed");
                return;
            }

            try
            {
                currEntry[senderIndex].Index = selIdx.Value;
                if (currEntry[senderIndex].Index < 0xFB)
                {
                    RmpEntry connected = map.Rmp[currEntry[senderIndex].Index];
                    currEntry[senderIndex].Distance = calcLinkDistance(currEntry, connected, senderOut);
                }
            }
            catch (Exception ex)
            { 
                MessageBox.Show("Error: " + ex.Message);
            }
            Refresh();
        }

        private void cbLink_Leave(ComboBox sender, int senderIndex)
        {
            if (loadingGUI)
                return;

            if (AutoconnectNodes.Checked)
            {
                RmpEntry connected = map.Rmp[currEntry[senderIndex].Index]; 
                var spaceAt = CompareLinks(connected, (byte)sender.SelectedItem);
                if (spaceAt.HasValue)
                {
                    connected[spaceAt.Value].Index = currEntry.Index;
                    connected[spaceAt.Value].Distance = calcLinkDistance(connected, currEntry, null);
                }
                Refresh();

            }
        }

        private void cbLink1_SelectedIndexChanged(object sender, System.EventArgs e)
        {
            cbLink_SelectedIndexChanged(cbLink1, 0, txtDist1);
        }

        private void cbLink1_Leave(object sender, EventArgs e)
        {
            cbLink_Leave(cbLink1, 0);
        }

        private void cbLink2_SelectedIndexChanged(object sender, System.EventArgs e)
        {
            cbLink_SelectedIndexChanged(cbLink2, 1, txtDist2);
        }

        private void cbLink2_Leave(object sender, EventArgs e)
        {
            cbLink_Leave(cbLink2, 1);
        }

        private void cbLink3_SelectedIndexChanged(object sender, System.EventArgs e)
        {
            cbLink_SelectedIndexChanged(cbLink3, 2, txtDist3);
        }

        private void cbLink3_Leave(object sender, EventArgs e)
        {
            cbLink_Leave(cbLink3, 2);
        }

        private void cbLink4_SelectedIndexChanged(object sender, System.EventArgs e)
        {
            cbLink_SelectedIndexChanged(cbLink4, 3, txtDist4);
        }

        private void cbLink4_Leave(object sender, EventArgs e)
        {
            cbLink_Leave(cbLink4, 3);
        }

        private void cbLink5_SelectedIndexChanged(object sender, System.EventArgs e)
        {
            cbLink_SelectedIndexChanged(cbLink5, 4, txtDist5);
        }

        private void cbLink5_Leave(object sender, EventArgs e)
        {
            cbLink_Leave(cbLink5, 4);
        }

        private void btnRemove_Click(object sender, System.EventArgs e)
        {
            map.Rmp.RemoveEntry(currEntry);
            ((XCMapTile) map[currEntry.Row, currEntry.Col, currEntry.Height]).Rmp = null;
            currEntry = null;
            gbNodeInfo.Enabled = false;
            Refresh();
        }

        private void cbUse1_SelectedIndexChanged(object sender, System.EventArgs e)
        {

            currEntry[0].UType = (UnitType) cbUse1.SelectedItem;
            Refresh();
        }

        private void cbUse2_SelectedIndexChanged(object sender, System.EventArgs e)
        {
            currEntry[1].UType = (UnitType) cbUse2.SelectedItem;
            Refresh();
        }

        private void cbUse3_SelectedIndexChanged(object sender, System.EventArgs e)
        {
            currEntry[2].UType = (UnitType) cbUse3.SelectedItem;
            Refresh();
        }

        private void cbUse4_SelectedIndexChanged(object sender, System.EventArgs e)
        {
            currEntry[3].UType = (UnitType) cbUse4.SelectedItem;
            Refresh();
        }

        private void cbUse5_SelectedIndexChanged(object sender, System.EventArgs e)
        {
            currEntry[4].UType = (UnitType) cbUse5.SelectedItem;
            Refresh();
        }

        private void txtDist1_KeyDown(object sender, System.Windows.Forms.KeyEventArgs e)
        {
            switch (e.KeyCode)
            {
                case Keys.Enter:
                    try
                    {
                        currEntry[0].Distance = byte.Parse(txtDist1.Text);
                    }
                    catch
                    {
                        txtDist1.Text = currEntry[0].Distance + "";
                    }
                    break;
            }
        }

        private void txtDist1_Leave(object sender, System.EventArgs e)
        {
            try
            {
                currEntry[0].Distance = byte.Parse(txtDist1.Text);
            }
            catch
            {
                txtDist1.Text = currEntry[0].Distance + "";
            }
        }

        private void txtDist2_Leave(object sender, System.EventArgs e)
        {
            try
            {
                currEntry[1].Distance = byte.Parse(txtDist2.Text);
            }
            catch
            {
                txtDist2.Text = currEntry[1].Distance + "";
            }
        }

        private void txtDist2_KeyDown(object sender, System.Windows.Forms.KeyEventArgs e)
        {
            switch (e.KeyCode)
            {
                case Keys.Enter:
                    try
                    {
                        currEntry[1].Distance = byte.Parse(txtDist2.Text);
                    }
                    catch
                    {
                        txtDist2.Text = currEntry[1].Distance + "";
                    }
                    break;
            }
        }

        private void txtDist3_Leave(object sender, System.EventArgs e)
        {
            try
            {
                currEntry[2].Distance = byte.Parse(txtDist3.Text);
            }
            catch
            {
                txtDist3.Text = currEntry[2].Distance + "";
            }
        }

        private void txtDist3_KeyDown(object sender, System.Windows.Forms.KeyEventArgs e)
        {
            switch (e.KeyCode)
            {
                case Keys.Enter:
                    try
                    {
                        currEntry[2].Distance = byte.Parse(txtDist3.Text);
                    }
                    catch
                    {
                        txtDist3.Text = currEntry[2].Distance + "";
                    }
                    break;
            }
        }

        private void txtDist4_Leave(object sender, System.EventArgs e)
        {
            try
            {
                currEntry[3].Distance = byte.Parse(txtDist4.Text);
            }
            catch
            {
                txtDist4.Text = currEntry[3].Distance + "";
            }
        }

        private void txtDist4_KeyDown(object sender, System.Windows.Forms.KeyEventArgs e)
        {
            switch (e.KeyCode)
            {
                case Keys.Enter:
                    try
                    {
                        currEntry[3].Distance = byte.Parse(txtDist4.Text);
                    }
                    catch
                    {
                        txtDist4.Text = currEntry[3].Distance + "";
                    }
                    break;
            }
        }

        private void txtDist5_Leave(object sender, System.EventArgs e)
        {
            try
            {
                currEntry[4].Distance = byte.Parse(txtDist5.Text);
            }
            catch
            {
                txtDist5.Text = currEntry[4].Distance + "";
            }
        }

        private void txtDist5_KeyDown(object sender, System.Windows.Forms.KeyEventArgs e)
        {
            switch (e.KeyCode)
            {
                case Keys.Enter:
                    try
                    {
                        currEntry[4].Distance = byte.Parse(txtDist5.Text);
                    }
                    catch
                    {
                        txtDist5.Text = currEntry[4].Distance + "";
                    }
                    break;
            }
        }

        private void cbUsage_SelectedIndexChanged(object sender, System.EventArgs e)
        {
            currEntry.Usage = (SpawnUsage) ((StrEnum) cbUsage.SelectedItem).Enum;
            Refresh();
        }

        //private void loadDefaults()
        protected override void LoadDefaultSettings(Settings settings)
        {
            Dictionary<string, SolidBrush> brushes = rmpPanel.Brushes;
            Dictionary<string, Pen> pens = rmpPanel.Pens;

            ValueChangedDelegate bc = new ValueChangedDelegate(brushChanged);
            ValueChangedDelegate pc = new ValueChangedDelegate(penColorChanged);
            ValueChangedDelegate pw = new ValueChangedDelegate(penWidthChanged);

            Pen redPen = new Pen(new SolidBrush(Color.Red), 2);
            pens["UnselectedLinkColor"] = redPen;
            pens["UnselectedLinkWidth"] = redPen;
            settings.AddSetting("UnselectedLinkColor", redPen.Color, "Color of unselected link lines", "Links", pc,
                false, null);
            settings.AddSetting("UnselectedLinkWidth", 2, "Width of unselected link lines", "Links", pw, false, null);

            Pen bluePen = new Pen(new SolidBrush(Color.Blue), 2);
            pens["SelectedLinkColor"] = bluePen;
            pens["SelectedLinkWidth"] = bluePen;
            settings.AddSetting("SelectedLinkColor", bluePen.Color, "Color of selected link lines", "Links", pc, false,
                null);
            settings.AddSetting("SelectedLinkWidth", 2, "Width of selected link lines", "Links", pw, false, null);

            Pen wallPen = new Pen(new SolidBrush(Color.Black), 4);
            pens["WallColor"] = wallPen;
            pens["WallWidth"] = wallPen;
            settings.AddSetting("WallColor", wallPen.Color, "Color of wall indicators", "View", pc, false, null);
            settings.AddSetting("WallWidth", 4, "Width of wall indicators", "View", pw, false, null);

            Pen gridPen = new Pen(new SolidBrush(Color.Black), 1);
            pens["GridLineColor"] = gridPen;
            pens["GridLineWidth"] = gridPen;
            settings.AddSetting("GridLineColor", gridPen.Color, "Color of grid lines", "View", pc, false, null);
            settings.AddSetting("GridLineWidth", 1, "Width of grid lines", "View", pw, false, null);

            SolidBrush selBrush = new SolidBrush(Color.Blue);
            brushes["SelectedNodeColor"] = selBrush;
            settings.AddSetting("SelectedNodeColor", selBrush.Color, "Color of selected nodes", "Nodes", bc, false, null);

            SolidBrush spawnBrush = new SolidBrush(Color.GreenYellow);
            brushes["SpawnNodeColor"] = spawnBrush;
            settings.AddSetting("SpawnNodeColor", spawnBrush.Color, "Color of spawn nodes", "Nodes", bc, false, null);

            SolidBrush nodeBrush = new SolidBrush(Color.Green);
            brushes["UnselectedNodeColor"] = nodeBrush;
            settings.AddSetting("UnselectedNodeColor", nodeBrush.Color, "Color of unselected nodes", "Nodes", bc, false,
                null);

            SolidBrush contentBrush = new SolidBrush(Color.DarkGray);
            brushes["ContentTiles"] = contentBrush;
            settings.AddSetting("ContentTiles", contentBrush.Color, "Color of map tiles with a content tile", "Other",
                bc, false, null);
        }

        private void copyNode_Click(object sender, EventArgs e)
        {
            string NodeText = "MVNode|" + cbType.SelectedIndex.ToString() + "|" + cbRank1.SelectedIndex.ToString() + "|" +
                              cbRank2.SelectedIndex.ToString() + "|" + tbZero.Text + "|" +
                              cbUsage.SelectedIndex.ToString();
            Clipboard.SetText(NodeText);
        }

        private void pasteNode_Click(object sender, EventArgs e)
        {
            string[] NodeData = Clipboard.GetText().Split('|');
            if (NodeData[0] == "MVNode")
            {
                cbType.SelectedIndex = Int32.Parse(NodeData[1]);
                cbRank1.SelectedIndex = Int32.Parse(NodeData[2]);
                cbRank2.SelectedIndex = Int32.Parse(NodeData[3]);
                tbZero.Text = NodeData[4];
                cbUsage.SelectedIndex = Int32.Parse(NodeData[5]);
            }
        }

    }
}
