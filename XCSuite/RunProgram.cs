using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Text;
using System.Windows.Forms;
using System.Reflection;
using System.Resources;

namespace XCSuite
{
	public partial class RunProgram : UserControl
	{
		private string asmPath;
		private AppDomain updateDomain;
		private AssemblyUpdator asmUpdate;
		private RichTextBox mainDisplay;
		private DownloadStatus downloadStatus;

		public event EventHandler RunClick;
		public event EventHandler UpdateClick;

		public RunProgram()
		{
			InitializeComponent();
		}

		public string AsmPath
		{
			get { return asmPath; }
		}

		public Assembly Assembly
		{
			set 
			{
				displayIcon.Image = Icon.ExtractAssociatedIcon(value.Location).ToBitmap();
				txtDisplay.Text = value.GetName().Name + "\nProgram to check for XCom program updates\nVersion: " + value.GetName().Version.Major + "." + value.GetName().Version.MajorRevision;
				btnRun.Enabled = false;
				//Console.WriteLine("Version: " + value.GetName().Version.ToString());
			}
		}

		public bool LoadAssembly(string asmFile)
		{
			asmPath = asmFile;

			AppDomainSetup ads = new AppDomainSetup();
			ads.ApplicationBase = "file:///" + Environment.CurrentDirectory;
			ads.DisallowBindingRedirects = false;
			ads.DisallowCodeDownload = true;
			ads.ConfigurationFile = AppDomain.CurrentDomain.SetupInformation.ConfigurationFile;

			// Create the second AppDomain.
			updateDomain = AppDomain.CreateDomain("Assembly Inspector", null, ads);

			asmUpdate = (AssemblyUpdator)updateDomain.CreateInstanceAndUnwrap(
				Assembly.GetEntryAssembly().FullName,
				typeof(AssemblyUpdator).FullName
			);

			if (asmUpdate.ContainsUpdater(asmFile))
			{
				displayIcon.Image = Icon.ExtractAssociatedIcon(asmFile).ToBitmap();
				txtDisplay.Text = asmUpdate.DisplayName + "\n" + asmUpdate.Description + "\nVersion: " + asmUpdate.Version;// +" Build: " + asmUpdate.BuildVersion;
				btnRun.Enabled = asmUpdate.RunMe;
				return true;
			}
			else
			{
				unload();
				return false;
			}			
		}

		public void Update(object sender, EventArgs e)
		{
			mainDisplay = (RichTextBox)midPanel.Controls[0];
			midPanel.Controls.Clear();

			if (downloadStatus == null)
				downloadStatus = new DownloadStatus();

			downloadStatus.Dock = DockStyle.Fill;
			midPanel.Controls.Add(downloadStatus);

			unload();

			startDownload();
		}

		private void unload()
		{
			asmUpdate = null;
			AppDomain.Unload(updateDomain);
			updateDomain = null;
		}

		private void startDownload()
		{

		}

		private void RunProgram_MouseEnter(object sender, EventArgs e)
		{
			BackColor = Color.LightBlue;
		}

		private void RunProgram_MouseLeave(object sender, EventArgs e)
		{
			BackColor = SystemColors.Control;
		}

		private void btnRun_Click(object sender, EventArgs e)
		{
			if (RunClick != null)
				RunClick(this, e);
		}

		private void btnUpdate_Click(object sender, EventArgs e)
		{
			if (UpdateClick != null)
				UpdateClick(this, e);
		}
	}
}
