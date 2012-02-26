using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using WeifenLuo.WinFormsUI.Docking;

namespace ViewLib.Base
{
	public partial class ToolWindow : DockContent
	{
		public ToolWindow()
		{
			InitializeComponent();
		}

		public static bool IsDesignMode
		{
			get { return LicenseManager.UsageMode == LicenseUsageMode.Designtime || AppDomain.CurrentDomain.FriendlyName == "DefaultDomain"; }
		}
	}
}
