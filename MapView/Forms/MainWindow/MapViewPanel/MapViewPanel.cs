using System;
using System.Drawing;
using System.Collections;
using System.ComponentModel;
using System.Windows.Forms;
using System.Data;
using XCom;
using XCom.Interfaces;
using XCom.Interfaces.Base;


namespace MapView
{
	public class MapViewPanel : Panel
	{
		private View _view;
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

	        SetView(new View());
	    }

	    public void SetView(View v)
		{
			if (_view != null)
			{
				v.Map = _view.Map;
				Controls.Remove(_view);
			}

			_view = v;

			_view.Location = new Point(0, 0);
			_view.BorderStyle = BorderStyle.Fixed3D;

			_vert.Minimum = 0;
			_vert.Value = _vert.Minimum;

			_view.Width = ClientSize.Width - _vert.Width - 1;

			Controls.Add(_view);
		}

		public void Cut_click(object sender, EventArgs e)
		{
			_view.Copy();
			_view.ClearSelection();
		}

		public void Copy_click(object sender, EventArgs e)
		{
			_view.Copy();
		}

		public void Paste_click(object sender, EventArgs e)
		{
			_view.Paste();
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
			get { return _view.Map; }
		}

		private void update(object sender, EventArgs e)
		{
			_view.Refresh();
		}

		protected override void OnResize(EventArgs e)
		{
			base.OnResize(e);
			_vert.Value = _vert.Minimum;
			_horiz.Value = _horiz.Minimum;
			vert_Scroll(null, null);
			horiz_Scroll(null, null);

			int h = 0, w = 0;

		    _vert.Visible = (_view.Height > ClientSize.Height);
            if (_vert.Visible)
			{
				_vert.Maximum = _view.Height - ClientSize.Height + _horiz.Height;
				w = _vert.Width;
			}
			else
				_horiz.Width = ClientSize.Width;

		    _horiz.Visible = (_view.Width > ClientSize.Width);
            if (_horiz.Visible)
			{
				_horiz.Maximum = Math.Max((_view.Width - ClientSize.Width + _vert.Width), _horiz.Minimum);
				h = _horiz.Height;
			}
			else
				_vert.Height = ClientSize.Height;

			_view.Viewable = new Size(Width - w, Height - h);
			_view.Refresh();
		}

		private void vert_Scroll(object sender, ScrollEventArgs e)
		{
			_view.Location = new Point(_view.Left, -(_vert.Value) + 1);
			_view.Refresh();
		}

		private void horiz_Scroll(object sender, ScrollEventArgs e)
		{
			_view.Location = new Point(-(_horiz.Value), _view.Top);
			_view.Refresh();
		}
        
		public void SetMap(IMap_Base map)
		{
			_view.Map = map;
			_view.Focus();
			OnResize(null);
		}
		public void SetupMapSize(   )
		{
			_view.SetupMapSize ();
		}

		public void ForceResize()
		{
			OnResize(null);
		}

		public View View
		{
			get { return _view; }
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
