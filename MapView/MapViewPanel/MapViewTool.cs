using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using ViewLib;

namespace MapView
{
	public partial class MapViewTool : Map_Observer_Form
	{
		private static MapViewTool instance;

		private MapViewTool()
		{
			InitializeComponent();
		}

		public static MapViewTool Instance
		{
			get
			{
				if (instance == null)
					instance = new MapViewTool();
				return instance;
			}
		}

		public override void SetupDefaultSettings()
		{
			base.SetupDefaultSettings();
			/*
			bottom.SetupDefaultSettings(this);
			topViewPanel.SetupDefaultSettings(this);

			visibleHash[topViewPanel.Ground] = settings.AddSetting("GroundVisible", true, "Show ground portion", "Tile", visibleSetting);
			visibleHash[topViewPanel.West] = settings.AddSetting("WestVisible", true, "Show west portion", "Tile", visibleSetting);
			visibleHash[topViewPanel.North] = settings.AddSetting("NorthVisible", true, "Show north portion", "Tile", visibleSetting);
			visibleHash[topViewPanel.Content] = settings.AddSetting("ContentVisible", true, "Show content portion", "Tile", visibleSetting);*/
		}
	}
}
