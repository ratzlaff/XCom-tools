namespace XCom
{
	public class Link
	{
		public const byte NOT_USED		= 0xFF;
		public const byte EXIT_NORTH	= 0xFE;
		public const byte EXIT_EAST		= 0xFD;
		public const byte EXIT_SOUTH	= 0xFC;
		public const byte EXIT_WEST		= 0xFB;

		public Link(byte index, byte distance, byte type)
		{
			Index = index;
			Distance = distance;
			UType = (UnitType) type;
		}

		/// <summary>
		/// Gets or sets the index of the destination node
		/// </summary>
		public byte Index
		{ get; set; }

		/// <summary>
		/// gets or sets the distance to the destination node
		/// </summary>
		public byte Distance
		{ get; set; }

		/// <summary>
		/// gets or sets the unit type that can use this link
		/// </summary>
		public UnitType UType
		{ get; set; }
	}
}
