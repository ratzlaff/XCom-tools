using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Windows.Forms;
using System.Drawing;
using System.Drawing.Imaging;
using PckView.Args;
using PckView.Panels;
using XCom.Interfaces;
using XCom;

namespace PckView
{

	public delegate void PckViewMouseClicked(object sender, PckViewMouseClickArgs e);
	public delegate void PckViewMouseMoved(int moveNum);

	public class ViewPck
		:
		Panel
	{
		private XCImageCollection myFile;

		private int space = 2;

		private Color goodColor = Color.FromArgb(204,204,255);
		private SolidBrush goodBrush = new SolidBrush(Color.FromArgb(204, 204, 255));

		private int _moveX;
		private int _moveY;
		private int _startY;

		private readonly List<ViewPckItem> _selectedItems;

		public event PckViewMouseClicked ViewClicked;
		public event PckViewMouseMoved ViewMoved;

		public ViewPck()
		{
//			pckFile = null;
			Paint += paint;
			SetStyle(ControlStyles.DoubleBuffer | ControlStyles.UserPaint | ControlStyles.AllPaintingInWmPaint, true);
			MouseDown += click;
			MouseMove += moving;
			_startY = 0;

			_selectedItems = new List<ViewPckItem>();
		}

		// saves a bitmap as a 8-bit image
		public void SaveBMP(string file, Palette pal)
		{
			Bmp.SaveBMP(file, myFile, pal, numAcross(), 1);
		}

		public void Hq2x()
		{
			myFile.Hq2x();
		}

		public Palette Pal
		{
			get { return myFile.Pal; }
			set
			{
				if (myFile != null)
					myFile.Pal = value;
			}
		}

		public int StartY
		{
			set
			{
				_startY = value;
				Refresh();
			}
		}

		public int PreferredHeight
		{
			get
			{
				if (myFile != null)
					return calcHeight();

				return 0;
			}
		}

		public XCImageCollection Collection
		{
			get { return myFile; }
			set
			{
				myFile = value;
				if (myFile != null)
					Height = calcHeight();

				click(null, new MouseEventArgs(MouseButtons.Left, 1, 0, 0, 0));
				moving(null, new MouseEventArgs(MouseButtons.Left, 1, 0, 0, 0));
				Refresh();
			}
		}

		public ReadOnlyCollection<ViewPckItemImage> SelectedItems
		{
			get
			{
				if (myFile != null)
				{
					var list = new List<ViewPckItemImage>();
					foreach (var selectedItem in _selectedItems)
					{
						var item = new ViewPckItemImage();
						item.Item = selectedItem;
						item.Image = myFile[GetIndexOf(selectedItem)];
						list.Add(item);
					}
					return list.AsReadOnly();
				}
				return null;
			}
		}

		public void ChangeItem(int index, XCImage image)
		{
			myFile[index] = image;
		}

		private void moving(object sender, MouseEventArgs e)
		{
			if (myFile!=null)
			{
				int x = e.X / GetSpecialWidth(myFile.IXCFile.ImageSize.Width);
				int y = (e.Y-_startY) / (myFile.IXCFile.ImageSize.Height + 2 * space);

				if (x != _moveX || y != _moveY)
				{
					_moveX = x;
					_moveY = y;

					if (_moveX >= numAcross())
						_moveX = numAcross() - 1;

					if (ViewMoved != null)
						ViewMoved(_moveY * numAcross() + _moveX);
				}
			}
		}

		private void click(object sender, MouseEventArgs e)
		{
			if (myFile != null)
			{
				var x = e.X / GetSpecialWidth(myFile.IXCFile.ImageSize.Width);
				var y = (e.Y - _startY) / (myFile.IXCFile.ImageSize.Height + 2 * space);

				if (x >= numAcross())
					x = numAcross() - 1;
	
				var index = y * numAcross() + x;

				var selected = new ViewPckItem();
				selected.X = x;
				selected.Y = y;
				selected.Index = index;

				if (index < Collection.Count)
				{
					if (ModifierKeys == Keys.Control)
					{
						ViewPckItem existingItem = null;
						foreach (var item in _selectedItems)
						{
							if (item.X == x && item.Y == y)
								existingItem = item;
						}

						if (existingItem != null)
						{
							_selectedItems.Remove(existingItem);
						}
						else
						{
							_selectedItems.Add(selected);
						}
					}
					else
					{
						_selectedItems.Clear();
						_selectedItems.Add(selected);
					}

					Refresh();

					if (ViewClicked != null)
					{
						var args = new PckViewMouseClickArgs(e, index);
						ViewClicked(this, args);
					}
				}
			}
		}

		private void paint(object sender, PaintEventArgs e)
		{
			if (myFile != null && myFile.Count > 0)
			{
				var g = e.Graphics;

				var specialWidth = GetSpecialWidth(myFile.IXCFile.ImageSize.Width);

				foreach (var selectedItem in _selectedItems)
				{
					if (myFile.IXCFile.FileOptions.BitDepth == 8 && myFile[0].Palette.Transparent.A == 0)
					{
						g.FillRectangle(
									goodBrush,
									selectedItem.X * specialWidth - space,
									_startY + selectedItem.Y * (myFile.IXCFile.ImageSize.Height + 2 * space) - space,
									specialWidth,
									myFile.IXCFile.ImageSize.Height + 2 * space);
					}
					else
					{
						g.FillRectangle(
									Brushes.Red,
									selectedItem.X * specialWidth - space,
									_startY + selectedItem.Y * (myFile.IXCFile.ImageSize.Height + 2 * space) - space,
									specialWidth,
									myFile.IXCFile.ImageSize.Height + 2 * space);
					}
				}

				for (int i = 0; i < numAcross() + 1; i++)
					g.DrawLine(
							Pens.Black,
							new Point(i * specialWidth - space,          _startY),
							new Point(i * specialWidth - space, Height - _startY));

				for (int i = 0; i < myFile.Count / numAcross() + 1; i++)
					g.DrawLine(
							Pens.Black,
							new Point(0,     _startY + i * (myFile.IXCFile.ImageSize.Height + 2 * space) - space),
							new Point(Width, _startY + i * (myFile.IXCFile.ImageSize.Height + 2 * space) - space));

				for (int i = 0; i < myFile.Count; i++)
				{
					int x = i % numAcross();
					int y = i / numAcross();
					try
					{
						g.DrawImage(
								myFile[i].Image, x * specialWidth,
								_startY + y * (myFile.IXCFile.ImageSize.Height + 2 * space));
					}
					catch (Exception)
					{}
				}
			}
		}

		public void RemoveSelected()
		{
			if (SelectedItems.Count != 0)
			{
				var lowestIndex = int.MaxValue;

				var indexList = new List<int>();
				foreach (var item in SelectedItems)
					indexList.Add(item.Item.Index);

				indexList.Sort();
				indexList.Reverse();

				foreach (var index in indexList)
				{
					if (index < lowestIndex)
						lowestIndex = index;

					Collection.Remove(index);
				}

				if (Collection.Count == lowestIndex && lowestIndex > 0)
					lowestIndex = Collection.Count - 1;

				ClearSelection(lowestIndex);
			}
		}

		private void ClearSelection(int lowestIndex)
		{
			_selectedItems.Clear();

			if (Collection.Count != 0)
			{
				var selected = new ViewPckItem();
				selected.Y = lowestIndex / numAcross();
				selected.X = lowestIndex - selected.Y;
				selected.Index = selected.Y * numAcross() + selected.X;

				_selectedItems.Add(selected);
			}
		}

		private int numAcross()
		{
			return Math.Max(
						1,
						(Width - 8) / (myFile.IXCFile.ImageSize.Width + 2 * space));
		}

		private int calcHeight()
		{
			return (myFile.Count / numAcross()) * (myFile.IXCFile.ImageSize.Height + 2 * space) + 60;
		}

		private int GetIndexOf(ViewPckItem selectedItem)
		{
			return selectedItem.Y * numAcross() + selectedItem.X;
		}

		private int GetSpecialWidth(int width)
		{
			return width + 2 * space;
		}
	}
}
