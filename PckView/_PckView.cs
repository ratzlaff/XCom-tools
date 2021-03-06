using System;
using System.Drawing;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;
using System.Windows.Forms;
using System.Data;
using System.IO;
using System.Drawing.Imaging;
using System.Reflection;
using DSShared.FileSystems;
using PckView.Args;
using PckView.Forms.ImageBytes;
using PckView.Properties;
using XCom;
using XCom.GameFiles.Images.xcFiles;
using XCom.Interfaces;
using DSShared;
using DSShared.Windows;
using DSShared.Loadable;

//.net file security
// http://www.c-sharpcorner.com/Code/2002/April/DotNetSecurity.asp

namespace PckView
{
	public partial class PckViewForm
		:
		System.Windows.Forms.Form
	{
		private TotalViewPck _totalViewPck;
		private Palette currPal;
		private Editor editor;
		private MenuItem editImage, replaceImage, saveImage, deleteImage, addMany;
		private Dictionary<Palette, MenuItem> palMI;
		private SharedSpace sharedSpace;
		private LoadOfType<IXCImageFile> loadedTypes;
		private ConsoleForm console;
		private OpenSaveFilter osFilter;
		private xcCustom xcCustom;
		private TabControl tabs;
		private Dictionary<int, IXCImageFile> openDictionary;
		private Dictionary<int, IXCImageFile> saveDictionary;
		private string _currentFilePath;
		private int _currentFileBpp;
		private readonly IFileBackupManager _fileBackupManager = new FileBackupManager();

		public bool SavedFile { get; private set; }

		public PckViewForm()
		{
			InitializeComponent();

			#region shared space information

			var consoleSharedSpace = new ConsoleSharedSpace(new SharedSpace());
			console = consoleSharedSpace.GetNewConsole();
			console.FormClosing += delegate(object sender, FormClosingEventArgs e)
			{
				e.Cancel = true;
				console.Hide();
			};
			FormClosed += (sender, e) => console.Close();

			sharedSpace = SharedSpace.Instance;
			sharedSpace.GetObj("PckView", this);
			sharedSpace.GetObj("AppDir", Environment.CurrentDirectory);
			sharedSpace.GetObj("CustomDir", Environment.CurrentDirectory + "/custom");
			sharedSpace.GetObj("SettingsDir", Environment.CurrentDirectory + "/settings");	
		
			xConsole.AddLine("Current directory: " + sharedSpace["AppDir"]);
			xConsole.AddLine("Custom directory: " + sharedSpace["CustomDir"].ToString());
			#endregion

			_totalViewPck = new TotalViewPck();
			_totalViewPck.Dock = DockStyle.Fill;
			DrawPanel.Controls.Add(_totalViewPck);

			_totalViewPck.View.DoubleClick += new EventHandler(doubleClick);
			_totalViewPck.ViewClicked += new PckViewMouseClicked(viewClicked);
			_totalViewPck.XCImageCollectionSet += new XCImageCollectionHandler(v_XCImageCollectionSet);
			_totalViewPck.ContextMenu = makeContextMenu();

			SaveMenuItem.Visible = false ;

			sharedSpace["Palettes"] = new Dictionary<string, Palette>();
			palMI = new Dictionary<Palette, MenuItem>();

			AddPalette(Palette.UFOBattle,		miPalette);
			AddPalette(Palette.UFOGeo,			miPalette);
			AddPalette(Palette.UFOGraph,		miPalette);
			AddPalette(Palette.UFOResearch,		miPalette);
			AddPalette(Palette.TFTDBattle,		miPalette);
			AddPalette(Palette.TFTDGeo,			miPalette);
			AddPalette(Palette.TFTDGraph,		miPalette);
			AddPalette(Palette.TFTDResearch,	miPalette);

			currPal = Palette.UFOBattle;
//			currPal = Palette.TFTDBattle;

			palMI[currPal].Checked = true;	// kL_ufoPalette
			_totalViewPck.Pal = currPal;	// kL_ufoPalette

			editor = new Editor(null);
			editor.Closing += new CancelEventHandler(editorClosing);

			if (editor != null)				// kL_ufoPalette
				editor.Palette = currPal;	// kL_ufoPalette


			RegistryInfo ri = new RegistryInfo(this, "PckView");
			ri.AddProperty("FilterIndex");
			ri.AddProperty("SelectedPalette");

			if (!File.Exists("hq2xa.dll"))
				miHq2x.Visible = false;

			loadedTypes = new LoadOfType<IXCImageFile>();
			loadedTypes.OnLoad += new LoadOfType<IXCImageFile>.TypeLoadDelegate(loadedTypes_OnLoad);
			sharedSpace["ImageMods"] = loadedTypes.AllLoaded;

//			loadedTypes.OnLoad += new LoadOfType<IXCFile>.TypeLoadDelegate(sortLoaded);

			loadedTypes.LoadFrom(Assembly.GetExecutingAssembly());
			loadedTypes.LoadFrom(Assembly.GetAssembly(typeof(XCom.Interfaces.IXCImageFile)));

			if (Directory.Exists(sharedSpace["CustomDir"].ToString()))
			{
				//Console.WriteLine("Custom directory exists: " + sharedSpace["CustomDir"].ToString());
				xConsole.AddLine("Custom directory exists: " + sharedSpace["CustomDir"].ToString());
				foreach (string s in Directory.GetFiles(sharedSpace["CustomDir"].ToString()))
				{
					if (s.EndsWith(".dll"))
					{
						xConsole.AddLine("Loading dll: " + s);
						loadedTypes.LoadFrom(Assembly.LoadFrom(s));
					}
					else if (s.EndsWith(xcProfile.PROFILE_EXT))
					{
						foreach (xcProfile ip in ImgProfile.LoadFile(s))
							loadedTypes.Add(ip);
					}
				}
			}

			osFilter = new OpenSaveFilter();
			osFilter.SetFilter(IXCImageFile.Filter.Open);

			openDictionary = new Dictionary<int, IXCImageFile>();
			saveDictionary = new Dictionary<int, IXCImageFile>();

			osFilter.SetFilter(IXCImageFile.Filter.Open);
			string filter = loadedTypes.CreateFilter(osFilter, openDictionary);
			openFile.Filter = filter;
		}

		private void v_XCImageCollectionSet(object sender, XCImageCollectionSetEventArgs e)
		{
			var enabled = e.Collection != null;
			saveitem.Enabled = enabled;
			transItem.Enabled = enabled;
			bytesMenu.Enabled = enabled;
			miPalette.Enabled = enabled;

			if (e.Collection != null)
			{
				bytesMenu.Enabled =
				miPalette.Enabled =
				transItem.Enabled = e.Collection.IXCFile.FileOptions.BitDepth == 8;

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
			openFile.Filter = loadedTypes.CreateFilter(osFilter, openDictionary);
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
//			editImage.Visible = false;

			return cm;
		}

		void sb_Click(object sender, EventArgs e)
		{
			TotalViewPck totalViewPck = _totalViewPck;

			if (tabs != null)
				foreach (object o in tabs.SelectedTab.Controls)
					if (o is TotalViewPck)
						totalViewPck = (TotalViewPck)o;

			if (totalViewPck != null)
			{
				if (totalViewPck.SelectedItems.Count != 1)
				{
					MessageBox.Show(
								"Must select 1 item only",
								Text,
								MessageBoxButtons.OK,
								MessageBoxIcon.Exclamation);
				}
				else
				{
					Form f = new Form();
					RichTextBox rtb = new RichTextBox();
					rtb.Dock = DockStyle.Fill;
					f.Controls.Add(rtb);

					foreach (byte b in totalViewPck.SelectedItems[0].Image.Bytes)
						rtb.Text += string.Format("{0:x} ", b);

					f.Text = "Bytes: " + totalViewPck.SelectedItems[0].Image.Bytes.Length;
					f.Show();
				}
			}
		}

		void addMany_Click(object sender, EventArgs e)
		{
			if (_totalViewPck.Collection != null)
			{
				openBMP.Title = "Hold shift to select multiple files";
				openBMP.Multiselect = true;

				if (openBMP.ShowDialog() == DialogResult.OK)
				{
					foreach (string s in openBMP.FileNames)
					{
						Bitmap b = new Bitmap(s);
						_totalViewPck.Collection.Add(Bmp.LoadTile(
															b,
															0,
															_totalViewPck.Pal,
															0, 0,
															_totalViewPck.Collection.IXCFile.ImageSize.Width,
															_totalViewPck.Collection.IXCFile.ImageSize.Height));
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
//			palMI[mi2] = p;
			palMI[p] = mi2;

			((Dictionary<string, Palette>)sharedSpace["Palettes"])[p.Name] = p;
			return mi2;
		}

		private void palClick(object sender, EventArgs e)
		{
			if (currPal != null)
				palMI[currPal].Checked = false;

			currPal = (Palette)((MenuItem)sender).Tag;
			palMI[currPal].Checked = true;

			_totalViewPck.Pal = currPal;

			if (editor != null)
				editor.Palette = currPal;

			_totalViewPck.Refresh();
		}

		private void viewClicked(object sender, PckViewMouseClickArgs e)
		{
			if (_totalViewPck.SelectedItems.Count > 0)
			{
				editImage.Enabled = true;
				saveImage.Enabled = true;
				deleteImage.Enabled = true;
				var selected = _totalViewPck.SelectedItems[_totalViewPck.SelectedItems.Count - 1];
				BytesFormHelper.ReloadBytes(selected);
			}
			else // selected is null
			{
				BytesFormHelper.ReloadBytes(null);

				editImage.Enabled = false;
				saveImage.Enabled = false;
				deleteImage.Enabled = false;
			}
		}

		private void quitItem_Click(object sender, System.EventArgs e)
		{
			Close();
		}

		private void openItem_Click(object sender, System.EventArgs e)
		{
			if (openFile.ShowDialog() == DialogResult.OK)
			{
				OnResize(null);

				string fName = openFile.FileName.Substring(openFile.FileName.LastIndexOf("/") + 1).ToLower();

				string ext = fName.Substring(fName.LastIndexOf("."));
				string file = fName.Substring(0, fName.LastIndexOf("."));
				string path = openFile.FileName.Substring(0, openFile.FileName.LastIndexOf("/") + 1);

				XCom.XCImageCollection toLoad = null;
				bool recover = false;

				// remove saving - there are too many formats and stuff,
				// I will implement only one type of direct saving.
				_currentFilePath = null;
				SaveMenuItem.Visible = false;

				//Console.WriteLine(openFile.FilterIndex+" -> " + filterIndex[openFile.FilterIndex].GetType());
#if !DEBUG
				try
				{
#endif
					IXCImageFile filterIdx = openDictionary[openFile.FilterIndex]; //filterIndex[openFile.FilterIndex];
					if (filterIdx.GetType() == typeof(xcForceCustom)) // special case
					{
						toLoad = filterIdx.LoadFile(path, fName);
						recover = true;
					}
					else if (filterIdx.GetType() == typeof (xcCustom)) // for *.* files, try singles and then extensions
					{
						// try singles
						foreach (XCom.Interfaces.IXCImageFile ixf in loadedTypes.AllLoaded)
							if (ixf.SingleFileName != null && ixf.SingleFileName.ToLower() == fName.ToLower())
							{
								try
								{
									toLoad = ixf.LoadFile(path, fName);
									break;
								}
								catch
								{}
							}

						if (toLoad == null) // singles not loaded, try non singles
						{
							foreach (XCom.Interfaces.IXCImageFile ixf in loadedTypes.AllLoaded)
								if (ixf.SingleFileName == null && ixf.FileExtension.ToLower() == ext.ToLower())
								{
									try
									{
										toLoad = ixf.LoadFile(path, fName);
										break;
									}
									catch
									{}
								}

							if (toLoad == null) // nothing loaded, force the custom dialog
								toLoad = xcCustom.LoadFile(path, fName, 0, 0);
						}
					}
					else
					{
						toLoad = LoadImageCollection(filterIdx, path, fName);
					}
#if !DEBUG
				}
				catch (Exception ex)
				{
					if (MessageBox.Show(
									this,
									"Error loading file: " + fName + "\nPath: " + openFile.FileName
										+ "\nError loading file, do you wish to try and recover?\n\nError Message: "
										+ ex + ":" + ex.Message,
									"Error loading file",
									MessageBoxButtons.YesNo,
									MessageBoxIcon.Error) == DialogResult.Yes)
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

		public void LoadPckFile(string filePath, int bpp)
		{
			_currentFilePath = filePath;
			_currentFileBpp = bpp;

			SaveMenuItem.Visible = true;
			IXCImageFile filterIdx = openDictionary[7];

			var fileName = Path.GetFileName(filePath);
			var path = Path.GetDirectoryName(filePath);

			if (fileName != null)
			{
				var images = LoadImageCollection(filterIdx, path, fileName.ToLower());
				SetImages(images);
				UpdateText();

				MapViewIntegrationMenuItem.Visible = true;
				if (Settings.Default.MapViewIntegrationHelpShown < 2)
					MapViewIntegrationHelpPanel.Visible = true;
			}
		}

		private static XCImageCollection LoadImageCollection(
														IXCImageFile filterIdx,
														string path,
														string fName)
		{
			return filterIdx.LoadFile( // load file based on its filterIndex
									path,
									fName,
									filterIdx.ImageSize.Width,
									filterIdx.ImageSize.Height);
		}

		public void SetImages(XCImageCollection toLoad)
		{
			palClick(((MenuItem)palMI[toLoad.IXCFile.DefaultPalette]), null);
			_totalViewPck.Collection = toLoad;
		}

		public TotalViewPck MainPanel
		{
			get { return _totalViewPck; }
		}

		private void saveAs_Click(object sender, System.EventArgs e)
		{
			SaveFileDialog saveFile = new SaveFileDialog();

			osFilter.SetFilter(IXCImageFile.Filter.Save);
			saveDictionary.Clear();
			saveFile.Filter = loadedTypes.CreateFilter(osFilter, saveDictionary);

			if (saveFile.ShowDialog() == DialogResult.OK)
			{
				string dir = saveFile.FileName.Substring(0, saveFile.FileName.LastIndexOf("/"));
				saveDictionary[saveFile.FilterIndex].SaveCollection(
																dir,
																Path.GetFileNameWithoutExtension(saveFile.FileName),
																_totalViewPck.Collection);
			}
		}

		private void viewClick(object sender, EventArgs e)
		{
			if (_totalViewPck.SelectedItems.Count == 0) return;
			var selected = _totalViewPck.SelectedItems[_totalViewPck.SelectedItems.Count - 1];
			if (_totalViewPck.Collection != null)
			{
				if (_totalViewPck.Collection.IXCFile.SingleFileName != null)
				{
					string fName = _totalViewPck.Collection.Name.Substring(0, _totalViewPck.Collection.Name.IndexOf("."));
					string ext = _totalViewPck.Collection.Name.Substring(_totalViewPck.Collection.Name.IndexOf(".") + 1);
					saveBmpSingle.FileName = fName + selected.Image.FileNum;
				}
				else
					saveBmpSingle.FileName = _totalViewPck.Collection.Name + selected.Image.FileNum;

				if (saveBmpSingle.ShowDialog() == DialogResult.OK)
					Bmp.Save(saveBmpSingle.FileName, selected.Image.Image);
			}
		}

		private void replaceClick(object sender, EventArgs e)
		{
			if (_totalViewPck.SelectedItems.Count != 1)
			{
				MessageBox.Show(
							"Must select 1 item only",
							Text,
							MessageBoxButtons.OK,
							MessageBoxIcon.Exclamation);
			}
			else if (_totalViewPck.Collection != null )
			{
				var title = string.Empty ;
				foreach (var selectedIndex in _totalViewPck.SelectedItems)
				{
					if (title != string.Empty) title += ", ";
					title += selectedIndex.Item.Index ;
				}

				openBMP.Title = "Selected number: " + title;
				openBMP.Multiselect = false;
				if (openBMP.ShowDialog() == DialogResult.OK)
				{
					var b = new Bitmap(openBMP.FileName);
					var image = Bmp.Load(
										b,
										_totalViewPck.Pal,
										_totalViewPck.Collection.IXCFile.ImageSize.Width,
										_totalViewPck.Collection.IXCFile.ImageSize.Height,
										1)[0];
					_totalViewPck.ChangeItem(_totalViewPck.SelectedItems[0].Item.Index, image);
					Refresh();
				}
				UpdateText();
			}
		}

		private void UpdateText()
		{
			Text = _totalViewPck.Collection.Name + ":" + _totalViewPck.Collection.Count;
		}

		private void removeClick(object sender, EventArgs e)
		{
			_totalViewPck.RemoveSelected();
			UpdateText();
			Refresh();
		}

		private void showBytes_Click(object sender, EventArgs e)
		{
			if (_totalViewPck.SelectedItems.Count != 0)
			{
				showBytes.Checked = true;

				var selected = _totalViewPck.SelectedItems[_totalViewPck.SelectedItems.Count - 1];
				BytesFormHelper.ShowBytes(selected, bClosing, new Point(this.Right, this.Top));
			}
		}

		private void bClosing( )
		{
			showBytes.Checked = false;
		}

		private void transOn_Click(object sender, System.EventArgs e)
		{
			transOn.Checked = !transOn.Checked;

			currPal.SetTransparent(transOn.Checked);
			_totalViewPck.Collection.Pal = currPal;
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
			if (_totalViewPck.SelectedItems.Count != 0)
			{
				var selected = _totalViewPck.SelectedItems[_totalViewPck.SelectedItems.Count - 1];
				if (selected != null)
				{
					editor.CurrImage = (XCImage)selected.Image.Clone();

					if (editor.Visible)
						editor.BringToFront();
					else
					{
						editor.Left = Right;
						editor.Top = Top;
						editor.Palette = currPal;
						editor.Show();
					}
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

			_totalViewPck.Hq2x();

			OnResize(null);
			Refresh();
		}

		private void PckView_KeyDown(object sender, KeyEventArgs e)
		{
			if (e.KeyCode == Keys.Delete && deleteImage.Enabled)
				removeClick(null, null);
		}

		private void miModList_Click(object sender, EventArgs e)
		{
			ModForm mf = new ModForm();
			mf.SharedSpace = sharedSpace;
			mf.ShowDialog();
		}

		private void miSaveDir_Click(object sender, EventArgs e)
		{
			if (_totalViewPck.Collection != null)
			{
				string fNameStart = "";
				string extStart = "";

				if (_totalViewPck.Collection.Name.IndexOf(".") > 0)
				{
					fNameStart = _totalViewPck.Collection.Name.Substring(0, _totalViewPck.Collection.Name.IndexOf("."));
					extStart = _totalViewPck.Collection.Name.Substring(_totalViewPck.Collection.Name.IndexOf(".") + 1);
				}

				saveBmpSingle.FileName = fNameStart;

				saveBmpSingle.Title = "Select directory to save images in";

				if (saveBmpSingle.ShowDialog() == DialogResult.OK)
				{
					string path = saveBmpSingle.FileName.Substring(0, saveBmpSingle.FileName.LastIndexOf(@"/"));
					string file = saveBmpSingle.FileName.Substring(saveBmpSingle.FileName.LastIndexOf(@"/") + 1);
					string fName = file.Substring(0, file.LastIndexOf("."));
					string ext = file.Substring(file.LastIndexOf(".") + 1);

//					int countNum = 0;
//					int charPos = fName.Length - 1;
//					int tens = 1;
//					while (charPos >= 0 && Char.IsDigit(fName[charPos]))
//					{
//						int digit = int.Parse(fName[charPos].ToString());
//						countNum += digit*tens;
//						tens *= 10;
//						fName = fName.Substring(0, charPos--);
//					}

					string zeros = "";
					int tens = _totalViewPck.Collection.Count;
					while (tens > 0)
					{
						zeros += "0";
						tens /= 10;
					}

					ProgressWindow pw = new ProgressWindow(this);
					pw.Minimum = 0;
					pw.Maximum = _totalViewPck.Collection.Count;
					pw.Width = 300;
					pw.Height = 50;

					pw.Show();
					foreach (XCImage xc in _totalViewPck.Collection)
					{
						//Console.WriteLine("Save to: " + path + @"\" + fName + (xc.FileNum + countNum) + "." + ext);
						//Console.WriteLine("Save: " + path + @"\" + fName + string.Format("{0:" + zeros + "}", xc.FileNum) + "." + ext);
						Bmp.Save(
								path + @"/" + fName + string.Format(
																"{0:" + zeros + "}",
																xc.FileNum) + "." + ext,
								xc.Image);
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
			XCImageCollection original = _totalViewPck.Collection;
			openItem_Click(null, null);
			XCImageCollection newCollection = _totalViewPck.Collection;
			_totalViewPck.Collection = original;

			if (Controls.Contains(_totalViewPck))
			{
				Controls.Remove(_totalViewPck);

				tabs = new TabControl();
				tabs.Dock = DockStyle.Fill;
				DrawPanel.Controls.Add(tabs);

				TabPage tp = new TabPage();
				tp.Controls.Add(_totalViewPck);
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

		private void SaveMenuItem_Click(object sender, EventArgs e)
		{
			osFilter.SetFilter(IXCImageFile.Filter.Save);
			saveDictionary.Clear();
			loadedTypes.CreateFilter(osFilter, saveDictionary);
			var dir = Path.GetDirectoryName(_currentFilePath);
			var fileWithoutExt = Path.GetFileNameWithoutExtension(_currentFilePath);

			// Backup
			_fileBackupManager.Backup(_currentFilePath);

			// Save
			PckFile.Save(
						dir,
						fileWithoutExt,
						_totalViewPck.Collection,
						_currentFileBpp);
			SavedFile = true;
		}

		private void MapViewIntegrationMenuItem_Click(object sender, EventArgs e)
		{
			MapViewIntegrationHelpPanel.Visible = !MapViewIntegrationHelpPanel.Visible;
			MapViewIntegrationMenuItem.Checked = MapViewIntegrationHelpPanel.Visible;
		}

		private void GotItMapViewButton_Click(object sender, EventArgs e)
		{
			MapViewIntegrationHelpPanel.Visible = false;
			MapViewIntegrationMenuItem.Checked = false;
			Settings.Default.MapViewIntegrationHelpShown++;
			Settings.Default.Save();
		}

		private void PckViewForm_Shown(object sender, EventArgs e)
		{
			console.Show();
		}
	}
}
