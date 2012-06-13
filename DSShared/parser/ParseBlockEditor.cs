using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Drawing.Design;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace UtilLib.Parser.Design
{
	public partial class ParseBlockEditor : Form
	{
		private ParseBlockData mData;

		public ParseBlockEditor()
		{
			InitializeComponent();
		}

		public ParseBlockData Data
		{
			get { return mData; }
			set
			{
				mData = value;
				gbCaption.Text = value.Caption;
				refreshList();
			}
		}

		private void refreshList()
		{
			items.Items.Clear();

			foreach (object o in mData.Data)
				items.Items.Add(o);
		}

		private void items_SelectedIndexChanged(object sender, EventArgs e)
		{
			propGrid.SelectedObject = items.SelectedItem;
		}
	}

	public class ParseBlockCollectionEditor
		: UITypeEditor
	{
		public ParseBlockCollectionEditor()
		{

		}

		private static Font sFont;
		public static Font DrawFont
		{
			set { sFont = value; }
		}

		public override UITypeEditorEditStyle GetEditStyle(ITypeDescriptorContext context)
		{
			return UITypeEditorEditStyle.Modal;
		}

		public override bool GetPaintValueSupported(ITypeDescriptorContext context)
		{
			return true;
		}

		public override object EditValue(ITypeDescriptorContext context, IServiceProvider provider, object value)
		{
			if (value is ParseBlockData) {
				ParseBlockEditor pe = new ParseBlockEditor();
				pe.Data = (ParseBlockData)value;
				pe.ShowDialog();
			}

			return base.EditValue(context, provider, value);
		}

		public override void PaintValue(PaintValueEventArgs e)
		{
			if (e.Value is ParseBlockData) {
				ParseBlockData dat = (ParseBlockData)e.Value;
				SizeF fSize = e.Graphics.MeasureString(dat.Count.ToString(), sFont);
				e.Graphics.DrawString(dat.Count.ToString(), sFont, Brushes.Black, 1 + (e.Bounds.Width - fSize.Width) / 2, 1 + (e.Bounds.Height - fSize.Height) / 2);
			} else
				base.PaintValue(e);
		}
	}
}
