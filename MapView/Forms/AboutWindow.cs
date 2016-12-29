using System;
using System.Drawing;
using System.Collections;
using System.ComponentModel;
using System.Windows.Forms;

namespace MapView
{
	/// <summary>
	/// Displays the About box
	/// </summary>
	public partial class AboutWindow
		:
		System.Windows.Forms.Form
	{
		public AboutWindow()
		{
			InitializeComponent();

			string ver = System.Reflection.Assembly.GetExecutingAssembly().GetName().Version.Major + "."
					   + System.Reflection.Assembly.GetExecutingAssembly().GetName().Version.Minor + "."
					   + System.Reflection.Assembly.GetExecutingAssembly().GetName().Version.Build + "."
					   + System.Reflection.Assembly.GetExecutingAssembly().GetName().Version.Revision;

#if DEBUG
			lblVersion.Text = "MapView version " + ver + " Debug";
#else
			lblVersion.Text = "MapView version " + ver + " Release";
#endif
		}

		private Point _originalLocation;
		private bool _moving;
		private double _lastPoint;

		private void MoveTimer_Tick(object sender, EventArgs e)
		{
			MoveWindow();
		}

		private void MoveWindow()
		{
			_moving = true;
			try
			{
				_lastPoint += 0.02;
				var loc = GetLocation(_lastPoint);
				Location = loc;
				MoveTimer.Interval = 30;
			}
			finally
			{
				_moving = false;
			}
		}

		private Point GetLocation(double point)
		{
			var loc = Location;
			loc.X = (int)(_originalLocation.X + (Math.Sin(point) * 50));
			loc.Y = (int)(_originalLocation.Y + (Math.Cos(point) * 50));
			return loc;
		}

		private void AboutWindow_LocationChanged(object sender, EventArgs e)
		{
			if (_moving) return;
			var locationBeforeMove = new Size(GetLocation(_lastPoint));
			var distance = new Size(Location - locationBeforeMove);
			_originalLocation += distance;

			MoveTimer.Enabled = false;
			MoveTimer.Enabled = true;
			MoveTimer.Interval = 500;
		}

		private void AboutWindow_Shown(object sender, EventArgs e)
		{
			_originalLocation = Location;
			MoveWindow();
		}
	}
}
