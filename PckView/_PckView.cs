using System;
using System.Drawing;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Windows.Forms;
using System.Data;
using System.IO;
using System.Drawing.Imaging;
using System.Reflection;
using XCom;
using XCom.Interfaces;
using DSShared;
using DSShared.Windows;
using DSShared.Loadable;
using MapLib.Base;
//.net file security
//http://www.c-sharpcorner.com/Code/2002/April/DotNetSecurity.asp

namespace PckView
{
	public partial class PckViewForm : System.Windows.Forms.Form
	{
		private Palette currPal;
		private Form bytesFrame;
		private RichTextBox bytesText;
		private int selected;
		private Editor editor;
		private MenuItem editImage, replaceImage, saveImage, deleteImage, addMany;
		private Dictionary<Palette, MenuItem> palMI;
		private SharedSpace sharedSpace;
		private MenuItem selectedMI;
		private LoadOfType<IXCImageFile> loadedTypes;
		private ConsoleForm console;
		private OpenSaveFilter osFilter;
		private xcCustom xcCustom;
		private TabControl tabs;

		private Dictionary<int, IXCImageFile> openDictionary;
		private Dictionary<int, IXCImageFile> saveDictionary;

		public PckViewForm()
		{
			InitializeComponent();

			#region shared space information
			sharedSpace = SharedSpace.Instance;

			if (sharedSpace.GetObj("xConsole") == null)
			{
				console = (ConsoleForm)sharedSpace.GetObj("xConsole", new ConsoleForm());
				console.FormClosing += delegate(object sender, FormClosingEventArgs e)
				{
					e.Cancel = true;
					console.Hide();
				};
			}
			console.Show();

			sharedSpace.GetObj("PckView",this);
			sharedSpace.GetObj("AppDir", Environment.CurrentDirectory);
			sharedSpace.GetObj("CustomDir", Environment.CurrentDirectory + "\\custom");
			sharedSpace.GetObj("SettingsDir", Environment.CurrentDirectory + "\\settings");	

			xConsole.AddLine("Current directory: " + sharedSpace["AppDir"]);
			xConsole.AddLine("Custom directory: " + sharedSpace["CustomDir"].ToString());
			#endregion

			if (!Directory.Exists(XCom.SharedSpace.Instance["CustomDir"].ToString())) {
				Directory.CreateDirectory(XCom.SharedSpace.Instance["CustomDir"].ToString());

				try {
					StreamReader sr = new StreamReader(Assembly.GetExecutingAssembly().GetManifestResourceStream("PckView._Embedded.scr.pvp"));
					StreamWriter sw = new StreamWriter(XCom.SharedSpace.Instance["CustomDir"].ToString() + "/scr.pvp");

					while (sr.Peek() != -1)
						sw.WriteLine(sr.ReadLine());

					sr.Close();
					sw.Close();
				} catch (Exception) {
					xConsole.AddLine("Error writing out scr profile");
				}
			}

			TotalViewPck v = TotalViewPck.Instance;
			v.Dock = DockStyle.Fill;
			this.Controls.Add(v);

			v.View.DoubleClick += new EventHandler(doubleClick);
			v.ViewClicked += new PckViewMouseClicked(viewClicked);
			v.XCImageCollectionSet += new XCImageCollectionHandler(v_XCImageCollectionSet);
			v.ContextMenu = makeContextMenu();

			palMI = new Dictionary<Palette, MenuItem>();
			selectedMI = AddPalette(XCPalette.TFTDBattle, miPalette);
			AddPalette(XCPalette.TFTDGeo, miPalette);
			AddPalette(XCPalette.TFTDGraph, miPalette);
			AddPalette(XCPalette.TFTDResearch, miPalette);
			AddPalette(XCPalette.UFOBattle, miPalette);
			AddPalette(XCPalette.UFOGeo, miPalette);
			AddPalette(XCPalette.UFOGraph, miPalette);
			AddPalette(XCPalette.UFOResearch, miPalette);

			Palette.PaletteLoaded += new PaletteLoadedDelegate(newPal);

			editor = new Editor(null);
			editor.Closing += new CancelEventHandler(editorClosing);

			currPal = XCPalette.TFTDBattle;

			RegistryInfo ri = new RegistryInfo(this, "PckView");
			ri.AddProperty("FilterIndex");
			ri.AddProperty("SelectedPalette");

			if (!File.Exists("hq2xa.dll"))
				miHq2x.Enabled = false;

			loadedTypes = new LoadOfType<IXCImageFile>();
			loadedTypes.OnLoad += new LoadOfType<IXCImageFile>.TypeLoadDelegate(loadedTypes_OnLoad);
			sharedSpace["ImageMods"] = loadedTypes.AllLoaded;

			//loadedTypes.OnLoad += new LoadOfType<IXCFile>.TypeLoadDelegate(sortLoaded);

			loadedTypes.LoadFrom(Assembly.GetExecutingAssembly());
			loadedTypes.LoadFrom(Assembly.GetAssembly(typeof(XCom.Interfaces.IXCImageFile)));			

			if (Directory.Exists(sharedSpace["CustomDir"].ToString()))
			{
				//Console.WriteLine("Custom directory exists: "+sharedSpace["CustomDir"].ToString());
				xConsole.AddLine("Custom directory exists: " + sharedSpace["CustomDir"].ToString());
				foreach (string s in Directory.GetFiles(sharedSpace["CustomDir"].ToString()))
					if (s.EndsWith(".dll"))
					{
						xConsole.AddLine("Loading dll: " + s);
						loadedTypes.LoadFrom(Assembly.LoadFrom(s));
					}
					else if (s.EndsWith(xcProfile.PROFILE_EXT))
						foreach (xcProfile ip in ImgProfile.LoadFile(s))
							loadedTypes.Add(ip);
			}

			osFilter = new OpenSaveFilter();
			osFilter.SetFilter(IXCImageFile.Filter.Open);

			openDictionary = new Dictionary<int, IXCImageFile>();
			saveDictionary = new Dictionary<int, IXCImageFile>();

			osFilter.SetFilter(IXCImageFile.Filter.Open);
			string filter = loadedTypes.CreateFilter(osFilter, openDictionary);
			openFile.Filter = filter;
		}

		void newPal(Palette newPalette)
		{
			AddPalette(newPalette, miPalette);
		}

		void v_XCImageCollectionSet(object sender, XCImageCollectionSetEventArgs e)
		{
			saveitem.Enabled = transItem.Enabled = bytesMenu.Enabled = miPalette.Enabled = e.Collection != null;

			if (e.Collection != null)
			{
				bytesMenu.Enabled = miPalette.Enabled = transItem.Enabled = e.Collection.IXCFile.FileOptions.BitDepth == 8;
				xConsole.AddLine("bpp is: " + e.Collection.IXCFile.FileOptions.BitDepth);
			}
		}

		void loadedTypes_OnLoad(object sender, LoadOfType<IXCImageFile>.TypeLoadArgs e)
		{
			if (e.LoadedObj is xcCustom)
				xcCustom = (xcCustom)e.LoadedObj;
		}

		public void LoadProfile(string s)
		{
			foreach (xcProfile ip in ImgProfile.LoadFile(s))
				loadedTypes.Add(ip);

			osFilter.SetFilter(IXCImageFile.Filter.Open);
			openDictionary.Clear();
			openFile.Filter = loadedTypes.CreateFilter(osFilter,openDictionary);
		}

		private ContextMenu makeContextMenu()
		{
			ContextMenu cm = new ContextMenu();
			saveImage = new MenuItem("Save as BMP");
			cm.MenuItems.Add(saveImage);
			saveImage.Click += new EventHandler(viewClick);

			replaceImage = new MenuItem("Replace with BMP");
			cm.MenuItems.Add(replaceImage);
			replaceImage.Click += new EventHandler(replaceClick);

			addMany = new MenuItem("Add Bmp");
			cm.MenuItems.Add(addMany);
			addMany.Click += new EventHandler(addMany_Click);

			MenuItem sb = new MenuItem("Show Bytes");
			cm.MenuItems.Add(sb);
			sb.Click += new EventHandler(sb_Click);

			deleteImage = new MenuItem("Delete\tDel");
			cm.MenuItems.Add(deleteImage);
			deleteImage.Click += new EventHandler(removeClick);

			editImage = new MenuItem("Edit");
			cm.MenuItems.Add(editImage);
			editImage.Click += new EventHandler(editClick);
			editImage.Enabled = false;
			//editImage.Visible=false;
			return cm;
		}

		void sb_Click(object sender, EventArgs e)
		{
			TotalViewPck v = null;

			if (tabs == null)
				v = TotalViewPck.Instance;
			else
			{
				foreach (object o in tabs.SelectedTab.Controls)
					if (o is TotalViewPck)
						v = (TotalViewPck)o;
			}

			if (v.Selected.Bytes != null) {
				Form f = new Form();
				f.FormBorderStyle = FormBorderStyle.SizableToolWindow;
				f.Text = "Bytes";
				RichTextBox rtb = new RichTextBox();
				rtb.Dock = DockStyle.Fill;
				f.Controls.Add(rtb);

				foreach (byte b in v.Selected.Bytes)
					rtb.Text += string.Format("{0:x} ", b);
				f.Text = "Bytes: " + v.Selected.Bytes.Length;

				f.Show();	
			}
		}

		void addMany_Click(object sender, EventArgs e)
		{
			if (TotalViewPck.Instance.Collection != null)
			{
				openBMP.Title = "Hold shift to select multiple files";
				openBMP.Multiselect = true;
				if (openBMP.ShowDialog() == DialogResult.OK)
				{
					foreach (string s in openBMP.FileNames)
					{
						Bitmap b = new Bitmap(s);
						TotalViewPck.Instance.Collection.Add(XCom.Bmp.LoadTile(b, 0, TotalViewPck.Instance.Pal, 0, 0, TotalViewPck.Instance.Collection.IXCFile.ImageSize.Width, TotalViewPck.Instance.Collection.IXCFile.ImageSize.Height));
					}

					Refresh();
				}

				UpdateText();
			}
		}

		public string SelectedPalette
		{
			get { return currPal.Name; }
			set
			{
				foreach (Palette p in palMI.Keys)
					if (p.Name.Equals(value))
						palClick(palMI[p], null);
			}
		}

		public int FilterIndex
		{
			get { return openFile.FilterIndex; }
			set { openFile.FilterIndex = value; }
		}

		private void doubleClick(object sender, EventArgs e)
		{
			if (editor.Visible)
				editClick(sender, e);
		}

		public MenuItem AddPalette(Palette p, MenuItem mi)
		{
			MenuItem mi2 = new MenuItem(p.Name);
			mi2.Tag = p;
			mi.MenuItems.Add(mi2);
			mi2.Click += new EventHandler(palClick);
			//palMI[mi2]=p;
			palMI[p] = mi2;
			return mi2;
		}

		private void palClick(object sender, EventArgs e)
		{
			if (currPal != null)
				palMI[currPal].Checked = false;

			currPal = (Palette)((MenuItem)sender).Tag;
			palMI[currPal].Checked = true;

			TotalViewPck.Instance.Pal = currPal;

			if (editor != null)
				editor.Palette = currPal;

			TotalViewPck.Instance.Refresh();
		}

		private void viewClicked(int click)
		{
			selected = click;
			if (TotalViewPck.Instance.Selected != null)
			{
				editImage.Enabled = true;
				saveImage.Enabled = true;
				deleteImage.Enabled = true;
				if (bytesText != null)
				{
					bytesText.Text = TotalViewPck.Instance.Selected.ToString();
					bytesFrame.Text = "Length: " + TotalViewPck.Instance.Selected.Bytes.Length;
				}
			}
			else //selected is null
			{
				if (bytesText != null)
					bytesText.Text = "";

				editImage.Enabled = false;
				saveImage.Enabled = false;
				deleteImage.Enabled = false;
			}
		}

		private void quitItem_Click(object sender, System.EventArgs e)
		{
			Environment.Exit(0);
		}

		private void openItem_Click(object sender, System.EventArgs e)
		{
			if (openFile.ShowDialog() == DialogResult.OK)
			{
				OnResize(null);

				string fName = openFile.FileName.Substring(openFile.FileName.LastIndexOf("\\") + 1).ToLower();

				string ext = fName.Substring(fName.LastIndexOf("."));
				string file = fName.Substring(0, fName.LastIndexOf("."));
				string path = openFile.FileName.Substring(0, openFile.FileName.LastIndexOf("\\") + 1);

				XCom.XCImageCollection toLoad = null;
				bool recover = false;

				//Console.WriteLine(openFile.FilterIndex+" -> "+filterIndex[openFile.FilterIndex].GetType());
#if !DEBUG
				try
				{
#endif
					IXCImageFile filterIdx = openDictionary[openFile.FilterIndex];//filterIndex[openFile.FilterIndex];
					if (filterIdx.GetType() == typeof(xcForceCustom)) //special case
					{
						toLoad = filterIdx.LoadFile(path, fName);
						recover = true;
					}
					else if (filterIdx.GetType() == typeof(xcCustom)) //for *.* files, try singles and then extensions
					{
						//try singles
						foreach (XCom.Interfaces.IXCImageFile ixf in loadedTypes.AllLoaded)
						{
							if (ixf.SingleFileName != null && ixf.SingleFileName.ToLower() == fName.ToLower())
								try { toLoad = ixf.LoadFile(path, fName); break; }
								catch { }
						}

						if (toLoad == null)
						{
							//singles not loaded, try non singles
							foreach (XCom.Interfaces.IXCImageFile ixf in loadedTypes.AllLoaded)
							{								
								if (ixf.SingleFileName == null && ixf.FileExtension.ToLower() == ext.ToLower())
									try { toLoad = ixf.LoadFile(path, fName); break; }
									catch { }
							}

							//nothing loaded, force the custom dialog
							if(toLoad == null)
								toLoad = xcCustom.LoadFile(path, fName, 0, 0);
						}
					}
					else //load file based on its filterIndex
						toLoad = filterIdx.LoadFile(path, fName, filterIdx.ImageSize.Width, filterIdx.ImageSize.Height);
#if !DEBUG	
				}
				catch (Exception ex)
				{
					if (MessageBox.Show(this, "Error loading file: " + fName + "\nPath: " + openFile.FileName + "\nError loading file, do you wish to try and recover?\n\nError Message: " + ex + ":" + ex.Message, "Error loading file", MessageBoxButtons.YesNo, MessageBoxIcon.Error) == DialogResult.Yes)
					{
						toLoad = xcCustom.LoadFile(path, fName, 0, 0);
						recover = true;
					}
				}
#endif

				if (!recover && toLoad != null)
				{
					SetImages(toLoad);

					UpdateText();
				}
			}
		}

		public void SetImages(XCImageCollection toLoad)
		{
			palClick(((MenuItem)palMI[toLoad.IXCFile.DefaultPalette]), null);

			TotalViewPck.Instance.Collection = toLoad;
		}

		private void saveAs_Click(object sender, System.EventArgs e)
		{
			SaveFileDialog saveFile = new SaveFileDialog();

			osFilter.SetFilter(IXCImageFile.Filter.Save);
			saveDictionary.Clear();
			saveFile.Filter = loadedTypes.CreateFilter(osFilter, saveDictionary);

			if (saveFile.ShowDialog() == DialogResult.OK)
			{
				string dir = saveFile.FileName.Substring(0, saveFile.FileName.LastIndexOf("\\"));
				saveDictionary[saveFile.FilterIndex].SaveCollection(dir, Path.GetFileNameWithoutExtension(saveFile.FileName), TotalViewPck.Instance.Collection);
			}
		}

		private void viewClick(object sender, EventArgs e)
		{
			if (TotalViewPck.Instance.Collection != null)
			{
				if (TotalViewPck.Instance.Collection.IXCFile.SingleFileName != null)
				{
					string fName = TotalViewPck.Instance.Collection.Name.Substring(0, TotalViewPck.Instance.Collection.Name.IndexOf("."));
					string ext = TotalViewPck.Instance.Collection.Name.Substring(TotalViewPck.Instance.Collection.Name.IndexOf(".") + 1);
					saveBmpSingle.FileName = fName + TotalViewPck.Instance.Selected.FileNum;
				}
				else
					saveBmpSingle.FileName = TotalViewPck.Instance.Collection.Name + TotalViewPck.Instance.Selected.FileNum;

				if (saveBmpSingle.ShowDialog() == DialogResult.OK)
					DSShared.Bmp.Save(saveBmpSingle.FileName, TotalViewPck.Instance.Selected.Image);
			}
		}

		private void replaceClick(object sender, EventArgs e)
		{
			if (TotalViewPck.Instance.Collection != null)
			{
				openBMP.Title = "Selected number: " + selected;
				openBMP.Multiselect = false;
				if (openBMP.ShowDialog() == DialogResult.OK)
				{
					Bitmap b = new Bitmap(openBMP.FileName);

					TotalViewPck.Instance.Selected = XCom.Bmp.Load(b, TotalViewPck.Instance.Pal, TotalViewPck.Instance.Collection.IXCFile.ImageSize.Width, TotalViewPck.Instance.Collection.IXCFile.ImageSize.Height, 1)[0];
					Refresh();
				}

				UpdateText();
			}
		}

		private void UpdateText()
		{
			Text = TotalViewPck.Instance.Collection.Name + ":" + TotalViewPck.Instance.Collection.Count;
		}

		private void removeClick(object sender, EventArgs e)
		{
			TotalViewPck.Instance.Collection.Remove(selected);
			UpdateText();
			Refresh();
		}

		private void showBytes_Click(object sender, System.EventArgs e)
		{
			if (showBytes.Checked)
				bytesFrame.BringToFront();
			else if (TotalViewPck.Instance.Selected != null)
			{
				bytesFrame = new Form();
				bytesText = new RichTextBox();
				bytesText.Dock = DockStyle.Fill;
				bytesFrame.Controls.Add(bytesText);

				if (TotalViewPck.Instance.Selected.Bytes != null)
					foreach (byte b in TotalViewPck.Instance.Selected.Bytes)
						bytesText.Text += b + " ";

				bytesFrame.Closing += new CancelEventHandler(bClosing);
				bytesFrame.Location = new Point(this.Right, this.Top);
				bytesFrame.Text = "Length: " + TotalViewPck.Instance.Selected.Bytes.Length;
				bytesFrame.Show();
				showBytes.Checked = true;
			}
		}

		private void bClosing(object sender, CancelEventArgs e)
		{
			showBytes.Checked = false;
		}

		private void transOn_Click(object sender, System.EventArgs e)
		{
			transOn.Checked = !transOn.Checked;

			currPal.SetTransparent(transOn.Checked);
			TotalViewPck.Instance.Collection.Pal = currPal;
			Refresh();
		}

		private void aboutItem_Click(object sender, System.EventArgs e)
		{
			new About().ShowDialog(this);
		}

		private void helpItem_Click(object sender, System.EventArgs e)
		{
			new HelpForm().ShowDialog(this);
		}

		private void editClick(object sender, EventArgs e)
		{
			if (TotalViewPck.Instance.Selected != null)
			{
				editor.CurrImage = (XCImage)TotalViewPck.Instance.Selected.Clone();

				if (editor.Visible)
					editor.BringToFront();
				else
				{
//					editor.Left = Right;
//					editor.Top = Top;
					editor.Palette = currPal;
					editor.Show();
				}
			}
		}

		private void editorClosing(object sender, CancelEventArgs e)
		{
			e.Cancel = true;
			editor.Hide();
		}

		private void miHq2x_Click(object sender, System.EventArgs e)
		{
			miPalette.Enabled = false;
			bytesMenu.Enabled = false;

			TotalViewPck.Instance.Hq2x();

			OnResize(null);
			Refresh();
		}

		private void PckView_KeyDown(object sender, System.Windows.Forms.KeyEventArgs e)
		{
			if (e.KeyCode == Keys.Delete && deleteImage.Enabled && selected < TotalViewPck.Instance.Collection.Count)
				removeClick(null, null);
		}

		private void miModList_Click(object sender, System.EventArgs e)
		{
			ModForm mf = new ModForm();
			mf.SharedSpace = sharedSpace;
			mf.ShowDialog();
		}

		private void miSaveDir_Click(object sender, System.EventArgs e)
		{
			if (TotalViewPck.Instance.Collection != null)
			{
				string fNameStart = "";
				string extStart = "";

				if (TotalViewPck.Instance.Collection.Name.IndexOf(".") > 0)
				{
					fNameStart = TotalViewPck.Instance.Collection.Name.Substring(0, TotalViewPck.Instance.Collection.Name.IndexOf("."));
					extStart = TotalViewPck.Instance.Collection.Name.Substring(TotalViewPck.Instance.Collection.Name.IndexOf(".") + 1);
				}

				saveBmpSingle.FileName = fNameStart;

				saveBmpSingle.Title = "Select directory to save images in";

				if (saveBmpSingle.ShowDialog() == DialogResult.OK)
				{
					string path = saveBmpSingle.FileName.Substring(0, saveBmpSingle.FileName.LastIndexOf(@"\"));
					string file = saveBmpSingle.FileName.Substring(saveBmpSingle.FileName.LastIndexOf(@"\") + 1);
					string fName = file.Substring(0, file.LastIndexOf("."));
					string ext = file.Substring(file.LastIndexOf(".") + 1);

					//					int countNum=0;
					//					int charPos=fName.Length-1;
					//					int tens=1;
					//					while(charPos>=0 && Char.IsDigit(fName[charPos]))
					//					{
					//						int digit = int.Parse(fName[charPos].ToString());
					//						countNum += digit*tens;
					//						tens*=10;
					//						fName = fName.Substring(0,charPos--);
					//					}

					string zeros = "";
					int tens = TotalViewPck.Instance.Collection.Count;
					while (tens > 0)
					{
						zeros += "0";
						tens /= 10;
					}

					ProgressWindow pw = new ProgressWindow();
					pw.ParentWindow = this;
					pw.Minimum = 0;
					pw.Maximum = TotalViewPck.Instance.Collection.Count;
					pw.Width = 300;
					pw.Height = 50;

					pw.Show();
					foreach (XCImage xc in TotalViewPck.Instance.Collection)
					{
						//Console.WriteLine("Save to: "+path+@"\"+fName+(xc.FileNum+countNum)+"."+ext);
						//Console.WriteLine("Save: " + path + @"\" + fName + string.Format("{0:" + zeros + "}", xc.FileNum) + "." + ext);
						DSShared.Bmp.Save(path + @"\" + fName + string.Format("{0:" + zeros + "}", xc.FileNum) + "." + ext, xc.Image);
						//Console.WriteLine("---");
						pw.Value = xc.FileNum;
					}
					pw.Hide();
				}
			}
		}

		private void miConsole_Click(object sender, EventArgs e)
		{
			if (console.Visible)
				console.BringToFront();
			else
				console.Show();
		}

		private void miCompare_Click(object sender, EventArgs e)
		{
			XCImageCollection original = TotalViewPck.Instance.Collection;
			openItem_Click(null, null);
			XCImageCollection newCollection = TotalViewPck.Instance.Collection;
			TotalViewPck.Instance.Collection = original;

			if (Controls.Contains(TotalViewPck.Instance))
			{
				Controls.Remove(TotalViewPck.Instance);

				tabs = new TabControl();
				tabs.Dock = DockStyle.Fill;
				Controls.Add(tabs);

				TabPage tp = new TabPage();
				tp.Controls.Add(TotalViewPck.Instance);
				tp.Text = "Original";
				tabs.TabPages.Add(tp);

				tp = new TabPage();
				TotalViewPck tvNew = new TotalViewPck();
				tvNew.ContextMenu = makeContextMenu();
				tvNew.Dock = DockStyle.Fill;
				tvNew.Collection = newCollection;
				tp.Controls.Add(tvNew);
				tp.Text = "New";
				tabs.TabPages.Add(tp);
			}
		}
	}
}
