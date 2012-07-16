using System;
using System.Drawing;
using System.Collections.Generic;
using System.ComponentModel;
using System.Windows.Forms;
using UtilLib;
using XCom.Images;

//http://www.vbdotnetheaven.com/Code/May2004/MultiColListViewMCB.asp

namespace PckView
{
	public partial class ModForm : System.Windows.Forms.Form
	{
		public ModForm()
		{
			InitializeComponent();

			UtilLib.Windows.RegistryInfo ri = new UtilLib.Windows.RegistryInfo(this);
		}

		public SharedSpace SharedSpace
		{
			set
			{
				List<xcImageFile> files = (List<xcImageFile>)SharedSpace.Instance["ImageMods"];

				foreach (xcImageFile xcf in files) {
					if (xcf.FileExtension == ".bad" && xcf.Author == "Author" && xcf.Description == "Description")
						modList.Items.Add(new ListViewItem(new string[] { xcf.FileExtension, xcf.Author, xcf.GetType().ToString() }));
					else
						modList.Items.Add(new ListViewItem(new string[] { xcf.FileExtension, xcf.Author, xcf.Description }));
				}
			}
		}
	}
}
