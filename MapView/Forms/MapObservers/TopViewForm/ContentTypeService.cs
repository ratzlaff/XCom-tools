using System;
using System.Collections.Generic;
using System.Text;
using XCom;
using XCom.Interfaces.Base;

namespace MapView.Forms.MapObservers.TopViewForm
{
    public class ContentTypeService
    {
        public ContentTypes GetContentType(TileBase content)
        {
            var mcdEntry = content.Info as McdEntry;
            if (mcdEntry == null) return ContentTypes.Content;
            var loftList = mcdEntry.GetLoftList();
            if (AllLoftWith(loftList, new[] {24, 26})) return ContentTypes.EastWall;
            if (AllLoftWith(loftList, new[] {23, 25})) return ContentTypes.SouthWall;
            if (AllLoftWith(loftList, new[] {8, 10, 12, 14, 38}) &&
                HasAnyLoftWith(loftList, new[] {38})) return ContentTypes.NorthWallWithWindow;
            if (AllLoftWith(loftList, new[] {7, 9, 11, 13, 37}) &&
                HasAnyLoftWith(loftList, new[] {37})) return ContentTypes.WestWallWithWindow;
            if (AllLoftWith(loftList, new[] {8, 10, 12, 14})) return ContentTypes.NorthWall;
            if (AllLoftWith(loftList, new[] {7, 9, 11, 13})) return ContentTypes.WestWall;
            if (AllLoftWith(loftList, new[] {35})) return ContentTypes.NW_To_SE;
            if (AllLoftWith(loftList, new[] {36})) return ContentTypes.NE_To_SW;
            if (AllLoftWith(loftList, new[] {39, 40, 41, 103})) return ContentTypes.NorthWestCorner;
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
                if (!hasIt) return false;
            }
            return true;
        }

        private static bool HasAnyLoftWith(IEnumerable<byte> loftList, int[] numbers)
        {
            foreach (var loft in loftList)
            {
                foreach (var number in numbers)
                {
                    if (loft == number)
                    {
                       return  true;
                    }
                }
            }
            return false;
        }

        public bool IsDoor(TileBase content)
        {
            var mcdEntry = content.Info as McdEntry;
            if (mcdEntry == null) return false;
            if (mcdEntry.HumanDoor) return true;
            if (mcdEntry.UFODoor) return true;
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
        NorthWestCorner
    }
}
