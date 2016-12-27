using System;
using System.IO;
using System.Drawing;
using XCom;

namespace MapView
{
	public enum CursorState{Select,Aim,SelectMControl,Waypoint,Throw};

	public class CursorSprite
	{
		private CursorState state;
		private PckFile cursorFile;

		public CursorSprite(PckFile cursorFile)
		{
			state = CursorState.Select;
			this.cursorFile=cursorFile;

			foreach(PckImage pi in cursorFile)
				pi.Image.MakeTransparent(cursorFile.Pal.Transparent);
		}

		public CursorState State
		{
			get{return state;}
			set{state=value;}
		}

		public PckFile PckFile
		{
			get{return cursorFile;}
		}

		public void DrawHigh(Graphics g, int x, int y, bool over, bool top)
		{
			Bitmap image;
			if (top && state != CursorState.Aim)
			{
				if (over)
					image = cursorFile[1].Image;
				else
					image = cursorFile[0].Image;
			}
			else
			{
				image = cursorFile[2].Image;
			}
			g.DrawImage(image, x, y,
				(int)(image.Width * Globals.PckImageScale), (int)(image.Height * Globals.PckImageScale));
		}

		public void DrawLow(Graphics g, int x, int y, int i,bool over,bool top)
		{
			Bitmap image;
			if (top && state != CursorState.Aim)
			{
				if(over)
					image = cursorFile[4].Image;
				else
					image = cursorFile[3].Image;
				switch(state)
				{
					case CursorState.SelectMControl:
						image = cursorFile[11 + i % 2].Image;
						break;
					case CursorState.Throw:
						image = cursorFile[15 + i % 2].Image;
						break;
					case CursorState.Waypoint:
						image = cursorFile[13 + i % 2].Image;
						break;
				}
			}
			else if(top) //top and aim
			{
				image = cursorFile[7 + i % 4].Image;
			}
			else
			{
				image = cursorFile[5].Image;
			}
			g.DrawImage(image, x, y,
				(int)(image.Width * Globals.PckImageScale), (int)(image.Height * Globals.PckImageScale));
		}
	}
}
