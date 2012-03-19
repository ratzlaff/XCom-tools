using System;
using System.ComponentModel;
using System.Collections.Generic;
using System.Text;
using System.Drawing;
using MapLib.Base;
using UtilLib;
using MapLib;

namespace ViewLib
{
	public class Map_Observer_Control : DoubleBufferControl
	{
		protected Map map;
		protected Dictionary<string, SolidBrush> brushes;
		protected Dictionary<string, Pen> pens;

		public Map_Observer_Control()
		{
			if (!IsDesignMode) {
				MapControl.MapChanged += mapChanged;
				MapControl.HeightChanged += HeightChanged;
				MapControl.SelectedTileChanged += SelectedTileChanged;
				MapControl.Refresh += Refresh;
			}
		}

		protected virtual void mapChanged(MapChangedEventArgs e)
		{
			this.map = e.Map;
			Refresh();
			OnResize(null);
		}

		public virtual void HeightChanged(Map sender, HeightChangedEventArgs e) { Refresh(); }
		public virtual void SelectedTileChanged(Map sender, SelectedTileChangedEventArgs e) { Refresh(); }

		public virtual void SetupDefaultSettings(Map_Observer_Form sender)
		{
			if (sender != null) {
				brushes = sender.FillBrushes;
				pens = sender.DrawPens;
			}
		}

		protected void addPenSetting(Pen pen, string name, string category, string colorTxt, string widthTxt, Settings settings)
		{
			pens.Add(name + "Color", pen);
			settings.AddSetting(name + "Color", pen.Color, colorTxt, category, penColorChanged);

			pens.Add(name + "Width", pen);
			settings.AddSetting(name + "Width", pen.Width, widthTxt, category, penWidthChanged);
		}

		protected void addBrushSetting(SolidBrush brush, string name, string category, string colorTxt, Settings settings)
		{
			brushes.Add(name + "Color", brush);
			settings.AddSetting(name + "Color", brush.Color, colorTxt, category, brushChanged);
		}

		protected virtual void brushChanged(object sender, string key, object val)
		{
			brushes[key].Color = (Color)val;
			Refresh();
		}

		protected virtual void penColorChanged(object sender, string key, object val)
		{
			pens[key].Color = (Color)val;
			Refresh();
		}

		protected virtual void penWidthChanged(object sender, string key, object val)
		{
			pens[key].Width = (float)val;
			Refresh();
		}
	}
}
