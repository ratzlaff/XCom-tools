using System;
using System.Drawing;
using System.Collections.Generic;
using System.ComponentModel;
using System.Windows.Forms;
using System.IO;
using System.Reflection;
using XCom;
using UtilLib;

namespace MapView
{
	public partial class InstallWindow : Form
	{
		private XCom.VarCollection vars;
		private List<string> ufoDirs, tftdDirs;

		public InstallWindow()
		{
			//runPath = Directory.GetCurrentDirectory();
			InitializeComponent();

			DialogResult = DialogResult.Cancel;
			vars = new XCom.VarCollection();

			ufoDirs = new List<string>();
			ufoDirs.Add(@"C:\ufo");
			ufoDirs.Add(@"C:\ufo enemy unknown");
			ufoDirs.Add(@"C:\mps\ufo");
			ufoDirs.Add(@"C:\mps\ufo enemy unknown");
			ufoDirs.Add(@"d:\xcompro\mapedit\ufo");
			ufoDirs.Add(@"c:\program files\ufo enemy unknown");
			ufoDirs.Add(@"C:\Documents and Settings\Ben\Desktop\XCFiles\ufo");

			tftdDirs = new List<string>();
			tftdDirs.Add(@"C:\tftd");
			tftdDirs.Add(@"C:\mps\tftd");
			tftdDirs.Add(@"C:\terror");
			tftdDirs.Add(@"C:\mps\tftd");
			tftdDirs.Add(@"d:\xcompro\mapedit\tftd");
			tftdDirs.Add(@"c:\program files\Terror From the Deep");
			tftdDirs.Add(@"C:\Documents and Settings\Ben\Desktop\XCFiles\tftd");

			foreach (string path in ufoDirs)
				if (Directory.Exists(path)) {
					txtUFO.Text = path;
					break;
				}

			foreach (string path in tftdDirs)
				if (Directory.Exists(path)) {
					txtTFTD.Text = path;
					break;
				}
		}

		private void btnFindUFO_Click(object sender, System.EventArgs e)
		{
			folderSelector.Description = "Select UFO directory";

			if (folderSelector.ShowDialog(this) == DialogResult.OK) {
				txtUFO.Text = @folderSelector.SelectedPath;

				if (folderSelector.SelectedPath.EndsWith("\\"))
					txtUFO.Text = txtUFO.Text.Substring(0, txtUFO.Text.Length - 1);
			}
		}

		private void btnFindTFTD_Click(object sender, System.EventArgs e)
		{
			folderSelector.Description = "Select TFTD directory";

			if (folderSelector.ShowDialog(this) == DialogResult.OK) {
				txtTFTD.Text = folderSelector.SelectedPath;

				if (folderSelector.SelectedPath.EndsWith("\\"))
					txtTFTD.Text = txtTFTD.Text.Substring(0, txtTFTD.Text.Length - 1);
			}
		}

		private void okButton_Click(object sender, System.EventArgs e)
		{
			PathInfo pathsFile = (PathInfo)SharedSpace.Instance["MV_PathsFile"];
			pathsFile.EnsureDirectoryExists();
			((PathInfo)SharedSpace.Instance["MV_MapEditFile"]).EnsureDirectoryExists();
			((PathInfo)SharedSpace.Instance["MV_ImagesFile"]).EnsureDirectoryExists();

			StreamWriter sw = new StreamWriter(new FileStream(pathsFile.ToString(), FileMode.Create));

			if (txtTFTD.Text != "")
				sw.WriteLine("${tftd}:" + txtTFTD.Text);

			if (txtUFO.Text != "")
				sw.WriteLine("${ufo}:" + txtUFO.Text);

			string mapFile = SharedSpace.Instance["MV_MapEditFile"].ToString();
			string imageFile = SharedSpace.Instance["MV_ImagesFile"].ToString();
			string runPath = SharedSpace.Instance.GetString("AppDir");

			sw.WriteLine("mapdata:" + @mapFile);
			sw.WriteLine("images:" + @imageFile);
			sw.WriteLine("useBlanks:false");

			if (txtTFTD.Text != "")
				sw.WriteLine("cursor:${tftd}\\UFOGRAPH");
			else if (txtUFO.Text != "")
				sw.WriteLine("cursor:${ufo}\\UFOGRAPH");

			sw.Flush();
			sw.Close();

			vars["##RunPath##"] = runPath;

			//create files
			FileStream fs = new FileStream(@imageFile, FileMode.Create);
			fs.Close();
			fs = new FileStream(@mapFile, FileMode.Create);
			fs.Close();

			//write TFTD
			if (txtTFTD.Text != "") {
				appendFile(imageFile, "ImagesTFTD.dat");
				appendFile(mapFile, "MapEditTFTD.dat");
			}

			//write UFO
			if (txtUFO.Text != "") {
				appendFile(imageFile, "ImagesUFO.dat");
				appendFile(mapFile, "MapEditUFO.dat");
			}

			this.DialogResult = DialogResult.OK;
			Close();
		}

		private void appendFile(string file, string resourceFile)
		{
			StreamReader sr = new StreamReader(ResourceLoader.GetStream(this.GetType(), resourceFile));
			StreamWriter sw = new StreamWriter(new FileStream(file, FileMode.Append));

			while (sr.Peek() != -1) {
				string line = sr.ReadLine();

				if (line.IndexOf('#') > 0)
					foreach (string s in vars.Variables)
						line = line.Replace(s, vars[s]);

				sw.WriteLine(@line);
			}

			sw.WriteLine();
			sw.Flush();
			sw.Close();
			sr.Close();
		}
	}
}
