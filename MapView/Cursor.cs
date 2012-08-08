using System;
using System.IO;
using System.Drawing;
using XCom;
using MapLib;

namespace MapView
{
	public enum CursorState { Select, Aim, SelectMControl, Waypoint, Throw };

	public class Cursor
	{
		private CursorState state;
		private ImageCollection cursorFile;

		public Cursor()
		{
			state = CursorState.Select;
		}

		public CursorState State
		{
			get { return state; }
			set { state = value; }
		}

		public ImageCollection Images
		{
			get { return cursorFile; }
			set { cursorFile = value; }
		}

		public void DrawHigh(Graphics g, int x, int y, int i, bool over, bool top)
		{
			if (cursorFile == null)
				return;

			if (top && state != CursorState.Aim) {
				if (over)
					g.DrawImage(cursorFile[1].Image, x, y);
				else
					g.DrawImage(cursorFile[0].Image, x, y);
			} else
				g.DrawImage(cursorFile[2].Image, x, y);
		}

		public void DrawLow(Graphics g, int x, int y, int i, bool over, bool top)
		{
			if (cursorFile == null)
				return;

			if (top && state != CursorState.Aim) {
				if (over)
					g.DrawImage(cursorFile[4].Image, x, y);
				else
					g.DrawImage(cursorFile[3].Image, x, y);
				switch (state) {
					case CursorState.SelectMControl:
						g.DrawImage(cursorFile[11 + i % 2].Image, x, y);
						break;
					case CursorState.Throw:
						g.DrawImage(cursorFile[15 + i % 2].Image, x, y);
						break;
					case CursorState.Waypoint:
						g.DrawImage(cursorFile[13 + i % 2].Image, x, y);
						break;
				}
			} else if (top) { //top and aim
				g.DrawImage(cursorFile[7 + i % 4].Image, x, y);
			} else {
				g.DrawImage(cursorFile[5].Image, x, y);
			}
		}
	}
}
