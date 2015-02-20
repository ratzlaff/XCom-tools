using System;
using System.Diagnostics;
using System.Drawing;
using System.Collections.Generic;
using System.Windows.Forms;
using MapView.Forms.MapObservers.RmpViewForm;
using XCom;
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

    public partial class RmpView : Map_Observer_Form
    {
        private readonly RmpPanel _rmpPanel;

        private XCMapFile _map;
        private RmpEntry _currEntry;
        private Panel _contentPane;

        private bool _loadingGui;
        private bool _loadingMap; 
       
        private readonly List<object> _byteList = new List<object>();

        private readonly object[] _items2 =
        {
            LinkTypes.ExitEast, LinkTypes.ExitNorth, LinkTypes.ExitSouth, LinkTypes.ExitWest,
            LinkTypes.NotUsed
        };

        public RmpView()
        {
            InitializeComponent();

            _rmpPanel = new RmpPanel();
            _contentPane.Controls.Add(_rmpPanel);
            _rmpPanel.MapPanelClicked += RmpPanel_PanelClick;
            _rmpPanel.MouseMove += RmpPanel_MouseMove;
            _rmpPanel.Dock = DockStyle.Fill;

            var uTypeItms = new object[]
            {
                UnitType.Any, UnitType.Flying, UnitType.FlyingLarge, UnitType.Large, UnitType.Small
            };
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
            
            ClearSelected();

            base.Text = @"Waypoint View";
        }

        private void options_click(object sender, EventArgs e)
        {
            var pf = new PropertyForm("rmpViewOptions", Settings);
            pf.Text = @"Waypoint Settings";
            pf.Show();
        }

        private void BrushChanged(object sender, string key, object val)
        {
            _rmpPanel.MapBrushes[key].Color = (Color) val;
            Refresh();
        }

        private void PenColorChanged(object sender, string key, object val)
        {
            _rmpPanel.MapPens[key].Color = (Color) val;
            Refresh();
        }

        private void PenWidthChanged(object sender, string key, object val)
        {
            _rmpPanel.MapPens[key].Width = (int) val;
            Refresh();
        }
        
        private void RmpPanel_MouseMove(object sender, MouseEventArgs e)
        {
            XCMapTile t = _rmpPanel.GetTile(e.X, e.Y);
            if (t != null && t.Rmp != null)
            {
                lblMouseOver.Text = @"Mouse Over: " + t.Rmp.Index;
            }
            else
            {
                lblMouseOver.Text = @"";
            }
            _rmpPanel.Position.X = e.X;
            _rmpPanel.Position.Y = e.Y;
            _rmpPanel.Refresh();
        }

        private void RmpPanel_PanelClick(object sender, MapPanelClickEventArgs e)
        {
            idxLabel.Text = Text;
            try
            { 
                if (_currEntry != null && e.MouseEventArgs.Button == MouseButtons.Right)
                {
                    RmpEntry selEntry = ((XCMapTile) e.ClickTile).Rmp;
                    if (selEntry != null)
                    { 
                        ConnectNodes(selEntry);
                         
                        _currEntry = selEntry;
                        FillGui();
                        Refresh();

                        _map.MapChanged = true;
                        return;
                    }
                }

                var prevEntry = _currEntry;

                _currEntry = ((XCMapTile) e.ClickTile).Rmp;
                if (_currEntry == null && e.MouseEventArgs.Button == MouseButtons.Right)
                {
                    _currEntry = _map.AddRmp(e.ClickLocation);
                    ConnectNewNode(prevEntry);
                    _map.MapChanged = true;
                }
            }
            catch
            {
                return;
            }

            FillGui();
        }
         
        private void ConnectNodes(RmpEntry selEntry)
        {
            var connectType = GetConnectNodeTypes();
            if (connectType == ConnectNodeTypes.DontConnect) return;
            if (_currEntry.Equals(selEntry)) return;
            {
                var spaceAt = CompareLinks(_currEntry, selEntry.Index);
                if (spaceAt.HasValue)
                {
                    _currEntry[spaceAt.Value].Index = selEntry.Index;
                    _currEntry[spaceAt.Value].Distance = calcLinkDistance(_currEntry, selEntry, null);
                }
            }

            if (connectType == ConnectNodeTypes.ConnectTwoWays)
            {
                var spaceAt = CompareLinks(selEntry, _currEntry.Index);
                if (spaceAt.HasValue)
                {
                    selEntry[spaceAt.Value].Index = _currEntry.Index;
                    selEntry[spaceAt.Value].Distance = calcLinkDistance(selEntry, _currEntry, null);
                }
            }
        }

        private void ConnectNewNode(RmpEntry prevEntry)
        {
            if (prevEntry == null) return;
            var connectType = GetConnectNodeTypes();
            if (connectType == ConnectNodeTypes.DontConnect) return;

            var prevEntryLink = GetNextAvailableLink(prevEntry);
            if (prevEntryLink != null)
            {
                prevEntryLink.Index = (byte)(_map.Rmp.Length - 1);
                prevEntryLink.Distance = calcLinkDistance(prevEntry, _currEntry, null);
            }

            if (connectType == ConnectNodeTypes.ConnectTwoWays)
            {
                var firstLink = _currEntry[0];
                firstLink.Index = prevEntry.Index;
                firstLink.Distance = calcLinkDistance(_currEntry, prevEntry, txtDist1);
            }
        } 

        private static Link GetNextAvailableLink(RmpEntry prevEntry)
        {
            for (int pI = 0; pI < prevEntry.NumLinks; pI++)
            {
                if (prevEntry[pI].Index == 0xFF)
                {
                    return prevEntry[pI];
                }
            }
            return null;
        }

        private int? CompareLinks(RmpEntry list, int index)
        {
            if (list == null) return null;
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
            if (existingLink || !spaceAvailable) return null;
            return spaceAt;
        }

        private void FillGui()
        {
            if (_currEntry == null)
            {
                gbNodeInfo.Enabled = false;
                LinkGroupBox.Enabled = false;
                return;
            }
            gbNodeInfo.Enabled = true;
            gbNodeInfo.SuspendLayout();
            LinkGroupBox.Enabled = true;
            LinkGroupBox.SuspendLayout();

            _loadingGui = true;

            _byteList.Clear();

            cbLink1.Items.Clear();
            cbLink2.Items.Clear();
            cbLink3.Items.Clear();
            cbLink4.Items.Clear();
            cbLink5.Items.Clear();

            for (byte i = 0; i < _map.Rmp.Length; i++)
            {
                if (i == _currEntry.Index)
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

            cbType.SelectedItem = _currEntry.UType;

            if (_map.Tiles[0][0].Palette == Palette.UFOBattle)
                cbRank1.SelectedItem = RmpFile.UnitRankUFO[_currEntry.URank1];
            else
                cbRank1.SelectedItem = RmpFile.UnitRankTFTD[_currEntry.URank1];

            cbRank2.SelectedItem = _currEntry.URank2;
            tbZero.Text = _currEntry.Zero1 + "";
            cbUsage.SelectedItem = RmpFile.SpawnUsage[(byte) _currEntry.Usage];

            idxLabel2.Text = "Index: " + _currEntry.Index;

            if (_currEntry[0].Index < 0xFB)
                cbLink1.SelectedItem = _currEntry[0].Index;
            else
                cbLink1.SelectedItem = (LinkTypes) _currEntry[0].Index;

            if (_currEntry[1].Index < 0xFB)
                cbLink2.SelectedItem = _currEntry[1].Index;
            else
                cbLink2.SelectedItem = (LinkTypes) _currEntry[1].Index;

            if (_currEntry[2].Index < 0xFB)
                cbLink3.SelectedItem = _currEntry[2].Index;
            else
                cbLink3.SelectedItem = (LinkTypes) _currEntry[2].Index;

            if (_currEntry[3].Index < 0xFB)
                cbLink4.SelectedItem = _currEntry[3].Index;
            else
                cbLink4.SelectedItem = (LinkTypes) _currEntry[3].Index;

            if (_currEntry[4].Index < 0xFB)
                cbLink5.SelectedItem = _currEntry[4].Index;
            else
                cbLink5.SelectedItem = (LinkTypes) _currEntry[4].Index;

            cbUse1.SelectedItem = _currEntry[0].UType;
            cbUse2.SelectedItem = _currEntry[1].UType;
            cbUse3.SelectedItem = _currEntry[2].UType;
            cbUse4.SelectedItem = _currEntry[3].UType;
            cbUse5.SelectedItem = _currEntry[4].UType;

            txtDist1.Text = _currEntry[0].Distance + "";
            txtDist2.Text = _currEntry[1].Distance + "";
            txtDist3.Text = _currEntry[2].Distance + "";
            txtDist4.Text = _currEntry[3].Distance + "";
            txtDist5.Text = _currEntry[4].Distance + "";

            gbNodeInfo.ResumeLayout();
            LinkGroupBox.ResumeLayout();

            _loadingGui = false;
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
                _map = (XCMapFile)value;

                _loadingMap = true;
                try
                {
                    HeightDifTextbox.Text = _map.Rmp.ExtraHeight.ToString();

                    _currEntry = null;
                    ClearSelected();

                    _rmpPanel.Map = _map;
                    if (_rmpPanel.Map != null)
                    {
                        FillGui();
                        cbRank1.Items.Clear();

                        if (_map.Tiles[0][0].Palette == Palette.UFOBattle)
                            cbRank1.Items.AddRange(RmpFile.UnitRankUFO);
                        else
                            cbRank1.Items.AddRange(RmpFile.UnitRankTFTD);
                    }
                }
                finally
                {
                    _loadingMap = false;
                }
            }
        }

        public override void SelectedTileChanged(IMap_Base sender, SelectedTileChangedEventArgs e)
        {
            Text = string.Format("Waypoint view: r:{0} c:{1} ", e.MapPosition.Row, e.MapPosition.Col);
        }

        public override void HeightChanged(IMap_Base sender, HeightChangedEventArgs e)
        {
            ClearSelected();
            FillGui();
            Refresh();
        }

        private void cbType_SelectedIndexChanged(object sender, EventArgs e)
        {
            _currEntry.UType = (UnitType) cbType.SelectedItem;
        }

        private void cbRank1_SelectedIndexChanged(object sender, EventArgs e)
        {
            _currEntry.URank1 = (byte) ((StrEnum) cbRank1.SelectedItem).Enum;
        }

        private void cbRank2_SelectedIndexChanged(object sender, EventArgs e)
        {
            _currEntry.URank2 = (UnitRankNum) cbRank2.SelectedItem;
        }

        private void tbZero_Leave(object sender, EventArgs e)
        {
            try
            {
                _currEntry.Zero1 = byte.Parse(tbZero.Text);
            }
            catch
            {
                tbZero.Text = _currEntry.Zero1 + "";
            }
        }

        private void tbZero_KeyDown(object sender, System.Windows.Forms.KeyEventArgs e)
        {
            switch (e.KeyCode)
            {
                case Keys.Enter:
                    try
                    {
                        _currEntry.Zero1 = byte.Parse(tbZero.Text);
                    }
                    catch
                    {
                        tbZero.Text = _currEntry.Zero1 + "";
                    }
                    break;
            }
        }

        private byte calcLinkDistance(RmpEntry from, RmpEntry to, TextBox result)
        {
            var dist = ((int) Math.Sqrt(Math.Pow(from.Row - to.Row, 2) + Math.Pow(from.Col - to.Col, 2) +
                              Math.Pow(from.Height - to.Height, 2)));
            if (result != null)
            {
                result.Text = dist.ToString();
            }
            return (byte) dist;
        }

        private void cbLink_SelectedIndexChanged(ComboBox sender, int senderIndex, TextBox senderOut)
        {
            if (_loadingGui) return;

            var selIdx = sender.SelectedItem as byte?;
            if (!selIdx.HasValue)
            {
                selIdx = (byte?) (sender.SelectedItem as LinkTypes?);
            }

            if (!selIdx.HasValue)
            {
                MessageBox.Show(@"Error: Determining SelectedIndex value failed");
                return;
            }

            try
            {
                _currEntry[senderIndex].Index = selIdx.Value;
                if (_currEntry[senderIndex].Index < 0xFB)
                {
                    RmpEntry connected = _map.Rmp[_currEntry[senderIndex].Index];
                    _currEntry[senderIndex].Distance = calcLinkDistance(_currEntry, connected, senderOut);
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
            if (_loadingGui) return;
            var connectType = GetConnectNodeTypes();
            if (connectType == ConnectNodeTypes.DontConnect) return;
            if (connectType == ConnectNodeTypes.ConnectOneWay) return;

            if (sender.SelectedItem != null)
            {
                RmpEntry connected = _map.Rmp[_currEntry[senderIndex].Index]; 
                var spaceAt = CompareLinks(connected, (byte)sender.SelectedItem);
                if (spaceAt.HasValue)
                {
                    connected[spaceAt.Value].Index = _currEntry.Index;
                    connected[spaceAt.Value].Distance = calcLinkDistance(connected, _currEntry, null);
                }
                Refresh();
            }
        }

        private void cbLink1_SelectedIndexChanged(object sender, EventArgs e)
        {
            cbLink_SelectedIndexChanged(cbLink1, 0, txtDist1);
        }

        private void cbLink1_Leave(object sender, EventArgs e)
        {
            cbLink_Leave(cbLink1, 0);
        }

        private void cbLink2_SelectedIndexChanged(object sender, EventArgs e)
        {
            cbLink_SelectedIndexChanged(cbLink2, 1, txtDist2);
        }

        private void cbLink2_Leave(object sender, EventArgs e)
        {
            cbLink_Leave(cbLink2, 1);
        }

        private void cbLink3_SelectedIndexChanged(object sender, EventArgs e)
        {
            cbLink_SelectedIndexChanged(cbLink3, 2, txtDist3);
        }

        private void cbLink3_Leave(object sender, EventArgs e)
        {
            cbLink_Leave(cbLink3, 2);
        }

        private void cbLink4_SelectedIndexChanged(object sender, EventArgs e)
        {
            cbLink_SelectedIndexChanged(cbLink4, 3, txtDist4);
        }

        private void cbLink4_Leave(object sender, EventArgs e)
        {
            cbLink_Leave(cbLink4, 3);
        }

        private void cbLink5_SelectedIndexChanged(object sender, EventArgs e)
        {
            cbLink_SelectedIndexChanged(cbLink5, 4, txtDist5);
        }

        private void cbLink5_Leave(object sender, EventArgs e)
        {
            cbLink_Leave(cbLink5, 4);
        }

        private void btnRemove_Click(object sender, EventArgs e)
        {
            RemoveSelected();
        }

        private void cbUse1_SelectedIndexChanged(object sender, EventArgs e)
        {

            _currEntry[0].UType = (UnitType) cbUse1.SelectedItem;
            Refresh();
        }

        private void cbUse2_SelectedIndexChanged(object sender, EventArgs e)
        {
            _currEntry[1].UType = (UnitType) cbUse2.SelectedItem;
            Refresh();
        }

        private void cbUse3_SelectedIndexChanged(object sender, EventArgs e)
        {
            _currEntry[2].UType = (UnitType) cbUse3.SelectedItem;
            Refresh();
        }

        private void cbUse4_SelectedIndexChanged(object sender, EventArgs e)
        {
            _currEntry[3].UType = (UnitType) cbUse4.SelectedItem;
            Refresh();
        }

        private void cbUse5_SelectedIndexChanged(object sender, EventArgs e)
        {
            _currEntry[4].UType = (UnitType) cbUse5.SelectedItem;
            Refresh();
        }

        private void txtDist1_KeyDown(object sender, KeyEventArgs e)
        {
            switch (e.KeyCode)
            {
                case Keys.Enter:
                    try
                    {
                        _currEntry[0].Distance = byte.Parse(txtDist1.Text);
                    }
                    catch
                    {
                        txtDist1.Text = _currEntry[0].Distance + "";
                    }
                    break;
            }
        }

        private void txtDist1_Leave(object sender, EventArgs e)
        {
            try
            {
                _currEntry[0].Distance = byte.Parse(txtDist1.Text);
            }
            catch
            {
                txtDist1.Text = _currEntry[0].Distance + "";
            }
        }

        private void txtDist2_Leave(object sender, EventArgs e)
        {
            try
            {
                _currEntry[1].Distance = byte.Parse(txtDist2.Text);
            }
            catch
            {
                txtDist2.Text = _currEntry[1].Distance + "";
            }
        }

        private void txtDist2_KeyDown(object sender, KeyEventArgs e)
        {
            switch (e.KeyCode)
            {
                case Keys.Enter:
                    try
                    {
                        _currEntry[1].Distance = byte.Parse(txtDist2.Text);
                    }
                    catch
                    {
                        txtDist2.Text = _currEntry[1].Distance + "";
                    }
                    break;
            }
        }

        private void txtDist3_Leave(object sender, EventArgs e)
        {
            try
            {
                _currEntry[2].Distance = byte.Parse(txtDist3.Text);
            }
            catch
            {
                txtDist3.Text = _currEntry[2].Distance + "";
            }
        }

        private void txtDist3_KeyDown(object sender, KeyEventArgs e)
        {
            switch (e.KeyCode)
            {
                case Keys.Enter:
                    try
                    {
                        _currEntry[2].Distance = byte.Parse(txtDist3.Text);
                    }
                    catch
                    {
                        txtDist3.Text = _currEntry[2].Distance + "";
                    }
                    break;
            }
        }

        private void txtDist4_Leave(object sender, EventArgs e)
        {
            try
            {
                _currEntry[3].Distance = byte.Parse(txtDist4.Text);
            }
            catch
            {
                txtDist4.Text = _currEntry[3].Distance + "";
            }
        }

        private void txtDist4_KeyDown(object sender, KeyEventArgs e)
        {
            switch (e.KeyCode)
            {
                case Keys.Enter:
                    try
                    {
                        _currEntry[3].Distance = byte.Parse(txtDist4.Text);
                    }
                    catch
                    {
                        txtDist4.Text = _currEntry[3].Distance + "";
                    }
                    break;
            }
        }

        private void txtDist5_Leave(object sender, EventArgs e)
        {
            try
            {
                _currEntry[4].Distance = byte.Parse(txtDist5.Text);
            }
            catch
            {
                txtDist5.Text = _currEntry[4].Distance + "";
            }
        }

        private void txtDist5_KeyDown(object sender, KeyEventArgs e)
        {
            switch (e.KeyCode)
            {
                case Keys.Enter:
                    try
                    {
                        _currEntry[4].Distance = byte.Parse(txtDist5.Text);
                    }
                    catch
                    {
                        txtDist5.Text = _currEntry[4].Distance + "";
                    }
                    break;
            }
        }

        private void cbUsage_SelectedIndexChanged(object sender, EventArgs e)
        {
            _currEntry.Usage = (SpawnUsage) ((StrEnum) cbUsage.SelectedItem).Enum;
            Refresh();
        }

        public override void LoadDefaultSettings( )
        {
            var brushes = _rmpPanel.MapBrushes;
            var pens = _rmpPanel.MapPens;
             
            var bc = new ValueChangedDelegate(BrushChanged);
            var pc = new ValueChangedDelegate(PenColorChanged);
            var pw = new ValueChangedDelegate(PenWidthChanged);

            var settings = Settings;
            var redPen = new Pen(new SolidBrush(Color.Red), 2);
            pens["UnselectedLinkColor"] = redPen;
            pens["UnselectedLinkWidth"] = redPen;
            settings.AddSetting("UnselectedLinkColor", redPen.Color, "Color of unselected link lines", "Links", pc,
                false, null);
            settings.AddSetting("UnselectedLinkWidth", 2, "Width of unselected link lines", "Links", pw, false, null);

            var bluePen = new Pen(new SolidBrush(Color.Blue), 2);
            pens["SelectedLinkColor"] = bluePen;
            pens["SelectedLinkWidth"] = bluePen;
            settings.AddSetting("SelectedLinkColor", bluePen.Color, "Color of selected link lines", "Links", pc, false,
                null);
            settings.AddSetting("SelectedLinkWidth", 2, "Width of selected link lines", "Links", pw, false, null);

            var wallPen = new Pen(new SolidBrush(Color.Black), 4);
            pens["WallColor"] = wallPen;
            pens["WallWidth"] = wallPen;
            settings.AddSetting("WallColor", wallPen.Color, "Color of wall indicators", "View", pc, false, null);
            settings.AddSetting("WallWidth", 4, "Width of wall indicators", "View", pw, false, null);

            var gridPen = new Pen(new SolidBrush(Color.Black), 1);
            pens["GridLineColor"] = gridPen;
            pens["GridLineWidth"] = gridPen;
            settings.AddSetting("GridLineColor", gridPen.Color, "Color of grid lines", "View", pc, false, null);
            settings.AddSetting("GridLineWidth", 1, "Width of grid lines", "View", pw, false, null);

            var selBrush = new SolidBrush(Color.Blue);
            brushes["SelectedNodeColor"] = selBrush;
            settings.AddSetting("SelectedNodeColor", selBrush.Color, "Color of selected nodes", "Nodes", bc, false, null);

            var spawnBrush = new SolidBrush(Color.GreenYellow);
            brushes["SpawnNodeColor"] = spawnBrush;
            settings.AddSetting("SpawnNodeColor", spawnBrush.Color, "Color of spawn nodes", "Nodes", bc, false, null);

            var nodeBrush = new SolidBrush(Color.Green);
            brushes["UnselectedNodeColor"] = nodeBrush;
            settings.AddSetting("UnselectedNodeColor", nodeBrush.Color, "Color of unselected nodes", "Nodes", bc, false,
                null);

            var contentBrush = new SolidBrush(Color.DarkGray);
            brushes["ContentTiles"] = contentBrush;
            settings.AddSetting("ContentTiles", contentBrush.Color, "Color of map tiles with a content tile", "Other",
                bc, false, null);

            connectNodesYoolStripMenuItem.SelectedIndex = 2;
        }

        private void copyNode_Click(object sender, EventArgs e)
        {
            var nodeText = string.Format("MVNode|{0}|{1}|{2}|{3}|{4}",
                cbType.SelectedIndex, cbRank1.SelectedIndex, cbRank2.SelectedIndex, 
                tbZero.Text, cbUsage.SelectedIndex);
            Clipboard.SetText(nodeText);
        }

        private void pasteNode_Click(object sender, EventArgs e)
        {
            var nodeData = Clipboard.GetText().Split('|');
            if (nodeData[0] == "MVNode")
            {
                cbType.SelectedIndex = Int32.Parse(nodeData[1]);
                cbRank1.SelectedIndex = Int32.Parse(nodeData[2]);
                cbRank2.SelectedIndex = Int32.Parse(nodeData[3]);
                tbZero.Text = nodeData[4];
                cbUsage.SelectedIndex = Int32.Parse(nodeData[5]);
            }
        }

        private ConnectNodeTypes GetConnectNodeTypes()
        {
            if (connectNodesYoolStripMenuItem.Text == @"Connect One way")
                return ConnectNodeTypes.ConnectOneWay;
            if (connectNodesYoolStripMenuItem.Text == @"Connect Two ways")
                return ConnectNodeTypes.ConnectTwoWays;
            return ConnectNodeTypes.DontConnect;
        }

        private void ClearSelected()
        {
            _currEntry = null;
            _rmpPanel.ClearSelected();
        }
         
        private void RemoveSelected()
        {
            if (_currEntry == null) return;
            _map.Rmp.RemoveEntry(_currEntry);
            ((XCMapTile)_map[_currEntry.Row, _currEntry.Col, _currEntry.Height]).Rmp = null;
            _map.MapChanged = true;
            ClearSelected();
            gbNodeInfo.Enabled = false;
            LinkGroupBox.Enabled = false;
            Refresh();
        }

        private void RmpView_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Control && e.KeyCode == Keys.S)
            {
                if (_map != null)
                {
                    _map.Save();
                    e.Handled = true;
                }
            }
        }

        private void HeightDifTextbox_TextChanged(object sender, EventArgs e)
        {
            byte byt;
            if (byte.TryParse(HeightDifTextbox.Text, out byt))
            {
                _map.Rmp.ExtraHeight = byt;
            }
            else
            {
                _map.Rmp.ExtraHeight = 0;
                HeightDifTextbox.Text = _map.Rmp.ExtraHeight.ToString();
            }
            if (!_loadingMap) _map.MapChanged = true;
        }
    }
}
