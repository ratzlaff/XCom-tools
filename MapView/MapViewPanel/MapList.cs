using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using ViewLib;
using XCom.Interfaces.Base;
using XCom;
using MapLib;

namespace MapView
{
	public partial class MapList : Map_Observer_Form
	{
		private static MapList instance;

		private MapList()
		{
			InitializeComponent();

			tvMapList.TreeViewNodeSorter = new System.Collections.CaseInsensitiveComparer();
			tvMapList.Nodes.Clear();
			foreach (string key in GameInfo.TilesetInfo.Tilesets.Keys)
				AddTileset(GameInfo.TilesetInfo.Tilesets[key]);
			xConsole.AddLine("Map list created");
		}

		private class SortableTreeNode : TreeNode, IComparable
		{
			public SortableTreeNode(string text) : base(text) { }

			public int CompareTo(object other)
			{
				if (other is SortableTreeNode)
					return Text.CompareTo(((SortableTreeNode)other).Text);
				return -1;
			}
		}

		private void addMaps(TreeNode tn, Dictionary<string, IMapDesc> maps)
		{
			foreach (string key in maps.Keys) {
				SortableTreeNode mapNode = new SortableTreeNode(key);
				mapNode.Tag = maps[key];
				tn.Nodes.Add(mapNode);
			}
		}

		public void AddTileset(ITileset tSet)
		{
			SortableTreeNode tSetNode = new SortableTreeNode(tSet.Name);
			tSetNode.Tag = tSet;
			tvMapList.Nodes.Add(tSetNode);

			foreach (string tSetMapGroup in tSet.Subsets.Keys) {
				SortableTreeNode tsGroup = new SortableTreeNode(tSetMapGroup);
				tsGroup.Tag = tSet.Subsets[tSetMapGroup];
				tSetNode.Nodes.Add(tsGroup);

				addMaps(tsGroup, tSet.Subsets[tSetMapGroup]);
			}
		}

		private void mapList_AfterSelect(object sender, System.Windows.Forms.TreeViewEventArgs e)
		{
//			if (NotifySave() == DialogResult.Cancel)
//				return;

			if (tvMapList.SelectedNode.Tag is IMapDesc) {
				IMapDesc imd = (IMapDesc)tvMapList.SelectedNode.Tag;
				MapControl.Current = imd.GetMapFile();
			}
/*
				statusMapName.Text = "Map:" + imd.Name;
				tsMapSize.Text = "Size: " + MapControl.Current.Size.ToString();

				//turn off door animations
				if (miDoors.Checked) {
					miDoors.Checked = false;
					miDoors_Click(null, null);
				}

				miExport.Enabled = true;
				showMenu.Enabled = true;

				// notify everyone that there is a new map
				MapControl.RequestRefresh();
 */
//			} else
//				miExport.Enabled = false;
		}

		public static MapList Instance
		{
			get
			{
				if (instance == null)
					instance = new MapList();
				return instance;
			}
		}
	}
}
