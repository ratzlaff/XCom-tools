using System;
using System.Drawing;

namespace MapLib.Base
{
	/// <summary>
	/// This class provides all the necessary information to draw an animated sprite
	/// </summary>
	public abstract class Tile
	{
		protected TileImage[] images;
		protected int id;
		protected TileInfo mInfo;
		protected MapLib.ImageCollection mCollection;

		public Tile(int id, MapLib.ImageCollection inCollection)
		{
			this.id = id;
			mCollection = inCollection;
		}

		/// <summary>
		/// This is the ID by which the map knows this tile by
		/// </summary>
		public abstract int MapID { get; }

		public abstract void Init(int inMapID);

		/// <summary>
		/// This is the collection this tile was made from
		/// </summary>
		public MapLib.ImageCollection Collection
		{
			get { return mCollection; }
		}

		/// <summary>
		/// This is the ID unique to this ITile after it has been loaded
		/// </summary>
		public int ID
		{
			get { return id; }
		}

		public virtual string Category
		{
			get { return "TileCategory"; }
		}

		public virtual string SpecialType
		{
			get { return "TileType"; }
		}

		public virtual bool IsDoor
		{
			get { return false; }
		}

		public virtual int YOffset
		{
			get { return 0; }
		}

		public virtual TileInfo Properties
		{
			get { return mInfo; }
		}

		public virtual void Animate(bool isOn)
		{

		}
		
		/// <summary>
		/// Gets the image array used to animate this tile
		/// </summary>
		public TileImage[] Images
		{
			get { return images; }
		}

		public TileImage this[int idx]
		{
			get { return images[idx]; }
		}

		public void Render(System.Drawing.Graphics g, System.Windows.Forms.Control inControl)
		{

		}

		public virtual void FillInfo(System.Windows.Forms.RichTextBox rtb)
		{
			
		}
	}
}
