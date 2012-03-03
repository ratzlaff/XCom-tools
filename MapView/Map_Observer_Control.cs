using System;
using System.ComponentModel;
using System.Collections.Generic;
using System.Text;
using System.Drawing;
using XCom;
using XCom.Interfaces.Base;

namespace MapView
{
	public class Map_Observer_Control : ViewLib.Base.DoubleBufferControl
	{
		protected IMap_Base map;
		protected Settings settings;
		protected Dictionary<string, SolidBrush> brushes;
		protected Dictionary<string, Pen> pens;

		public Map_Observer_Control()
		{
			if (!IsDesignMode)
				MainWindow.Instance.MapChanged += mapChanged;
		}

		protected virtual void mapChanged(object sender, IMap_Base map)
		{
			if (map != null) {
				map.HeightChanged -= HeightChanged;
				map.SelectedTileChanged -= SelectedTileChanged;
			}

			this.map = map;

			if (map != null) {
				map.HeightChanged += HeightChanged;
				map.SelectedTileChanged += SelectedTileChanged;
			}

			OnResize(null);
		}

		public virtual void HeightChanged(IMap_Base sender, HeightChangedEventArgs e) { Refresh(); }
		public virtual void SelectedTileChanged(IMap_Base sender, SelectedTileChangedEventArgs e) { Refresh(); }

		public virtual void SetDrawingTools(Dictionary<string, SolidBrush> brushes, Dictionary<string, Pen> pens)
		{
			this.brushes = brushes;
			this.pens = pens;
		}

		public virtual void LoadDefaultSettings(Settings settings) { this.settings = settings; }

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
			pens[key].Width = (int)val;
			Refresh();
		}
	}
}
