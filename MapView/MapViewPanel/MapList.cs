using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using ViewLib;
using MapLib;

using UtilLib;
using MapLib.Base.Parsing;

namespace MapView
{
	public delegate void NodeSelectedDelegate(object sender, TreeNode node);

	public partial class MapList : Map_Observer_Form
	{
		private static MapList instance;

		public static event NodeSelectedDelegate NodeSelected;

		private MapList()
		{
			InitializeComponent();

			tvMapList.TreeViewNodeSorter = new System.Collections.CaseInsensitiveComparer();
			tvMapList.Nodes.Clear();
			MapEdit_dat mapedit = (MapEdit_dat)SharedSpace.Instance["mapdata"];
			foreach (MapCollection mc in mapedit.Items)
				AddCollection(mc);
//			xConsole.AddLine("Map list created");
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

		public void AddCollection(MapCollection mc)
		{
			SortableTreeNode tSetNode = new SortableTreeNode(mc.Name);
			tSetNode.Tag = mc;
			tvMapList.Nodes.Add(tSetNode);

			foreach (Tileset t in mc.Tilesets) {
				SortableTreeNode tsGroup = new SortableTreeNode(t.Name);
				tsGroup.Tag = t;
				tSetNode.Nodes.Add(tsGroup);
			
				addMaps(tsGroup, t);
			}
		}

		private void addMaps(TreeNode tn, Tileset tileset)
		{
			foreach (MapInfo mi in tileset.Maps) {
				SortableTreeNode mapNode = new SortableTreeNode(mi.Name);
				mapNode.Tag = mi;
				tn.Nodes.Add(mapNode);
			}
		}

		private void mapList_AfterSelect(object sender, System.Windows.Forms.TreeViewEventArgs e)
		{
//			if (NotifySave() == DialogResult.Cancel)
//				return;

			if (tvMapList.SelectedNode.Tag is MapInfo) {
				MapInfo imd = (MapInfo)tvMapList.SelectedNode.Tag;
				MapLib.Base.Map m = imd.Map;
				if (m != null)
					MapControl.Current = m;				
			}

			if (NodeSelected != null) {
				NodeSelected(this, tvMapList.SelectedNode);
			}
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
