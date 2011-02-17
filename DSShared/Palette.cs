using System;
using System.Drawing;
using System.Collections.Generic;
using System.Drawing.Imaging;

namespace DSShared
{
	public delegate void PaletteLoadedDelegate(Palette newPalette);

	//see http://support.microsoft.com/default.aspx?scid=kb;en-us;319061
	public class Palette
	{
		protected string mName;
		protected ColorPalette mPalette;
		protected int mTransIdx;

		private static Dictionary<string, Palette> loadedPalettes = new Dictionary<string, Palette>();

		public static event PaletteLoadedDelegate PaletteLoaded;

		public Palette(string inName, int inIdx) : this(inName, inIdx, true) { }
		public Palette(string inName, int inIdx, bool fireLoadingEvent)
		{
			mTransIdx = inIdx;
			mName = inName;

			Bitmap b = new Bitmap(1, 1, PixelFormat.Format8bppIndexed);
			mPalette = b.Palette;
			b.Dispose();

			if (mName.Length > 0)
				LoadedPalettes.Add(mName, this);

			if (fireLoadingEvent && PaletteLoaded != null)
				PaletteLoaded(this);
		}

		public Color Transparent
		{
			get { return mPalette.Entries[mTransIdx]; }
		}

		public Palette Grayscale
		{
			get
			{
				if (!loadedPalettes.ContainsKey(mName + "#gray")) {
					Palette p = new Palette(mName + "#gray", mTransIdx);
					for (int i = 0; i < mPalette.Entries.Length; i++) {
						int s = (int)(this[i].R * .10 + this[i].G * .50 + this[i].B * .25);
						p[i] = Color.FromArgb(s, s, s);
					}
				}
				return loadedPalettes[mName + "#gray"];
			}
		}

		public void SetTransparent(bool val, int index)
		{
			mTransIdx = index;
			Color old = mPalette.Entries[index];
			if (val)
				mPalette.Entries[index] = Color.FromArgb(0, old);
			else
				mPalette.Entries[index] = Color.FromArgb(255, old);
		}

		public void SetTransparent(bool val)
		{
			SetTransparent(val, mTransIdx);
		}

		public ColorPalette Colors
		{
			get { return mPalette; }
		}

		/// <summary>
		/// This palette's name
		/// </summary>
		public string Name
		{
			get { return mName; }
		}

		/// <summary>
		/// Indexes colors on number
		/// </summary>
		public Color this[int i]
		{
			get { return mPalette.Entries[i]; }
			set { mPalette.Entries[i] = value; }
		}

		/// <summary>
		/// tests for palette equality
		/// </summary>
		/// <param name="other">another palette</param>
		/// <returns>true if the palette names are the same</returns>
		public override bool Equals(Object other)
		{
			if (!(other is Palette))
				return false;
			return mPalette.Equals(((Palette)other).mPalette);
		}

		public override int GetHashCode()
		{
			return mPalette.GetHashCode();
		}

		public static Dictionary<string, Palette> LoadedPalettes
		{
			get { return loadedPalettes; }
		}
		/*
		public static Palette GetPalette(string name)
		{
			if (loadedPalettes[name] == null) {
				Assembly thisAssembly = Assembly.GetExecutingAssembly();
				try {
					loadedPalettes[name] = new Palette(thisAssembly.GetManifestResourceStream(embedPath + name + ".pal"));
				} catch {
					loadedPalettes[name] = null;
				}
			}
			return loadedPalettes[name];
		}
		*/
		public override string ToString()
		{
			return mName;
		}
	}
}