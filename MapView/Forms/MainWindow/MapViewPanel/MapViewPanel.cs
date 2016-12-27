using System;
using System.Drawing;
using System.Windows.Forms;
using XCom;
using XCom.Interfaces.Base;

namespace MapView
{
	public class MapViewPanel : Panel
	{
		private MapView _mapView;
		private readonly HScrollBar _horiz;
		private readonly VScrollBar _vert;

		private static MapViewPanel _myInstance;

		private MapViewPanel()
		{
			ImageUpdate += update;

			_horiz = new HScrollBar();
			_vert = new VScrollBar();

			_horiz.Scroll += horiz_Scroll;
			_horiz.Dock = DockStyle.Bottom;

			_vert.Scroll += vert_Scroll;
			_vert.Dock = DockStyle.Right;

			Controls.AddRange(new Control[] {_vert, _horiz});

			SetView(new MapView());
		}

		public void SetView(MapView v)
		{
			if (_mapView != null)
			{
				v.Map = _mapView.Map;
				Controls.Remove(_mapView);
			}

			_mapView = v;

			_mapView.Location = new Point(0, 0);
			_mapView.BorderStyle = BorderStyle.Fixed3D;

			_vert.Minimum = 0;
			_vert.Value = _vert.Minimum;

			_mapView.Width = ClientSize.Width - _vert.Width - 1;

			Controls.Add(_mapView);
		}

		public void Cut_click(object sender, EventArgs e)
		{
			_mapView.Copy();
			_mapView.ClearSelection();
		}

		public void Copy_click(object sender, EventArgs e)
		{
			_mapView.Copy();
		}

		public void Paste_click(object sender, EventArgs e)
		{
			_mapView.Paste();
		}

		public static MapViewPanel Instance
		{
			get
			{
				if (_myInstance == null)
				{
					_myInstance = new MapViewPanel();
					LogFile.Instance.WriteLine("Main view panel created");
				}
				return _myInstance;
			}
		}
		public IMap_Base Map
		{
			get { return _mapView.Map; }
		}

		private void update(object sender, EventArgs e)
		{
			_mapView.Refresh();
		}

		public void OnResize()
		{
			OnResize(EventArgs.Empty);
		}

		protected override void OnResize(EventArgs e)
		{
			base.OnResize(e);

			if (Globals.AutoPckImageScale)
			{
				SetupMapSize();
			}

			_vert.Value = _vert.Minimum;
			_horiz.Value = _horiz.Minimum;
			vert_Scroll(null, null);
			horiz_Scroll(null, null);

			int h = 0, w = 0;

			_vert.Visible = (_mapView.Height > ClientSize.Height);
			if (_vert.Visible)
			{
				_vert.Maximum = _mapView.Height - ClientSize.Height + _horiz.Height;
				w = _vert.Width;
			}
			else
				_horiz.Width = ClientSize.Width;

			_horiz.Visible = (_mapView.Width > ClientSize.Width);
			if (_horiz.Visible)
			{
				_horiz.Maximum = Math.Max((_mapView.Width - ClientSize.Width + _vert.Width), _horiz.Minimum);
				h = _horiz.Height;
			}
			else
				_vert.Height = ClientSize.Height;

			_mapView.Viewable = new Size(Width - w, Height - h);
			_mapView.Refresh();
		}

		private void vert_Scroll(object sender, ScrollEventArgs e)
		{
			_mapView.Location = new Point(_mapView.Left, -(_vert.Value) + 1);
			_mapView.Refresh();
		}

		private void horiz_Scroll(object sender, ScrollEventArgs e)
		{
			_mapView.Location = new Point(-(_horiz.Value), _mapView.Top);
			_mapView.Refresh();
		}
		
		public void SetMap(IMap_Base map)
		{
			_mapView.Map = map;
			_mapView.Focus();
			OnResize(null);
		}
		public void SetupMapSize(   )
		{
			if (Globals.AutoPckImageScale)
			{
				var size = _mapView.GetMapSize(1);
				var wP = Width / (double)size.Width;
				var hP = Height / (double)size.Height;
				if (wP > hP)
				{
					// Acommodate based on height
					Globals.PckImageScale = hP;
				}
				else
				{
					// Acommodate based on width
					Globals.PckImageScale = wP;
				}
				if (Globals.PckImageScale > Globals.MaxPckImageScale)
					Globals.PckImageScale = Globals.MaxPckImageScale;
				if (Globals.PckImageScale < Globals.MinPckImageScale )
					Globals.PckImageScale = Globals.MinPckImageScale;
			}

			_mapView.SetupMapSize ();
		}

		public void ForceResize()
		{
			OnResize(null);
		}

		public MapView MapView
		{
			get { return _mapView; }
		}

		/*** Timer stuff ***/
		private static int current;
		private static Timer timer;
		private static bool started;
		public static event EventHandler ImageUpdate;

		public static void Start()
		{
			if (timer == null)
			{
				timer = new Timer();
				timer.Interval = 100;
				timer.Tick += tick;
				timer.Start();
				started = true;
			}

			if (!started)
			{
				timer.Start();
				started = true;
			}
		}

		public static void Stop()
		{
			if (timer == null)
			{
				timer = new Timer();
				timer.Interval = 100;
				timer.Tick += tick;
				started = false;
			}

			if (started)
			{
				timer.Stop();
				started = false;
			}
		}

		public static bool Updating
		{
			get { return started; }
		}

		public static int Interval
		{
			get { return timer.Interval; }
			set { timer.Interval = value; }
		}

		private static void tick(object sender, EventArgs e)
		{
			current = (current + 1) % 8;

			if (ImageUpdate != null)
				ImageUpdate(null, null);
		}

		public static int Current
		{
			get { return current; }
			set { current = value; }
		}
	}
}
