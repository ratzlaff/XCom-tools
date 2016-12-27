using System;

namespace XCom.Interfaces.Base
{
	/// <summary>
	/// This class provides all the necessary information to draw an animated sprite
	/// </summary>
	public class TileBase
	{
		protected XCImage[] image;
		protected IInfo info;

		public TileBase(int id) { this.Id = id; MapId = -1; info = null; }

		/// <summary>
		/// This is the ID unique to this TileBase after it has been loaded
		/// </summary>
		public int Id { get; protected set; }

		/// <summary>
		/// This is the ID by which the map knows this tile by
		/// </summary>
		public int MapId { get; set; }

		/// <summary>
		/// Gets an image at the specified animation frame
		/// </summary>
		/// <param name="i"></param>
		/// <returns></returns>
		public XCImage this[int i]
		{
			get { return image[i]; }
			set { image[i] = value; }
		}

		/// <summary>
		/// Gets the image array used to animate this tile
		/// </summary>
		public XCImage[] Images
		{
			get { return image; }
		}

		/// <summary>
		/// The Info object that has additional flags and information about this tile
		/// </summary>
		public IInfo Info
		{
			get { return info; }
		}
	}
}
