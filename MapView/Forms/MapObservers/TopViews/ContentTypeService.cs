using System.Collections.Generic;
using XCom;
using XCom.Interfaces.Base;

namespace MapView.Forms.MapObservers.TopViews
{
	public class ContentTypeService
	{
		public ContentTypes GetContentType(TileBase content)
		{
			var mcdEntry = content.Info as McdEntry;
			if (mcdEntry != null)
			{
				var loftList = mcdEntry.GetLoftList();
				var allButGround = new List<byte>(loftList);
				allButGround.RemoveAt(0);

				if (AllLoftWith(allButGround, new[]{0}))
					return ContentTypes.Ground;

				if (AllLoftWith(loftList, new[]{24, 26}))
					return ContentTypes.EastWall;

				if (AllLoftWith(loftList, new[]{23, 25}))
					return ContentTypes.SouthWall;

				if (AllLoftWith(loftList, new[]{8, 10, 12, 14, 38})
					&& HasAnyLoftWith(loftList, new[]{38}))
				{
					return ContentTypes.NorthWallWithWindow;
				}

				if (AllLoftWith(loftList, new[]{8, 10, 12, 14, 38, 0, 39, 77})
					&& HasAnyLoftWith(loftList, new[]{0}))
				{
					return ContentTypes.NorthFence;
				}

				if (AllLoftWith(loftList, new[]{8, 10, 12, 14}))
					return ContentTypes.NorthWall;

				if (AllLoftWith(loftList, new[]{7, 9, 11, 13, 37})
					&& HasAnyLoftWith(loftList, new[]{37}))
				{
					return ContentTypes.WestWallWithWindow;
				}

				if (AllLoftWith(loftList, new[]{7, 9, 11, 13, 37,0, 39, 76})
					&& HasAnyLoftWith(loftList, new[]{0}))
				{
					return ContentTypes.WestFence;
				}

				if (AllLoftWith(loftList, new[]{7, 9, 11, 13}))
					return ContentTypes.WestWall;

				if (AllLoftWith(loftList, new[]{35}))
					return ContentTypes.NW_To_SE;

				if (AllLoftWith(loftList, new[]{36}))
					return ContentTypes.NE_To_SW;

				if (AllLoftWith(loftList, new[]{39, 40, 41, 103}))
					return ContentTypes.NorthWestCorner;

				if (AllLoftWith(loftList, new[]{100}))
					return ContentTypes.NorthEastCorner;

				if (AllLoftWith(loftList, new[]{106}))
					return ContentTypes.SouthWestCorner;

				if (AllLoftWith(loftList, new[]{109}))
					return ContentTypes.SouthEastCorner;
			}
			return ContentTypes.Content;
		}

		private static bool AllLoftWith(IEnumerable<byte> loftList, int[] numbers)
		{
			foreach (var loft in loftList)
			{
				var hasIt = false;
				foreach (var number in numbers)
				{
					if (loft == number)
					{
						hasIt = true;
						break;
					}
				}

				if (!hasIt)
					return false;
			}
			return true;
		}

		private static bool HasAnyLoftWith(IEnumerable<byte> loftList, int[] numbers)
		{
			foreach (var loft in loftList)
				foreach (var number in numbers)
					if (loft == number)
						return  true;

			return false;
		}

		public bool IsDoor(TileBase content)
		{
			var mcdEntry = content.Info as McdEntry;
			if (mcdEntry != null
				&& (mcdEntry.HumanDoor || mcdEntry.UFODoor))
			{
				return true;
			}
			return false;
		}
	}

	public enum ContentTypes
	{
		Content,
		EastWall,
		SouthWall,
		NorthWall,
		WestWall,
		NW_To_SE,
		NE_To_SW,
		NorthWallWithWindow,
		WestWallWithWindow,
		Ground,
		NorthFence,
		WestFence,
		NorthWestCorner,
		NorthEastCorner,
		SouthWestCorner,
		SouthEastCorner
	}
}
