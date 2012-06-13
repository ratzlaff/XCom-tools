using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using ViewLib;
using MapLib.Base;

namespace MapView
{
	public partial class PropertyView : Map_Observer_Form
	{
		private static PropertyView sInstance;

		public PropertyView()
		{
			InitializeComponent();
			TileView.TileChanged += new SelectedTileChanged(TileView_TileChanged);
			MapList.NodeSelected += new NodeSelectedDelegate(MapList_NodeSelected);
		}

		void MapList_NodeSelected(object sender, TreeNode node)
		{
			propGrid.SelectedObject = node.Tag;
		}

		void TileView_TileChanged(object sender, MapLib.Base.Tile newTile)
		{
			propGrid.SelectedObject = newTile;
		}

		public static PropertyView Instance
		{
			get
			{
				if (sInstance == null)
					sInstance = new PropertyView();
				
				return sInstance;
			}
		}

		public override void SelectedTileChanged(Map sender, MapLib.SelectedTileChangedEventArgs e)
		{
			propGrid.SelectedObject = e.SelectedTile;
		}
	}
}
