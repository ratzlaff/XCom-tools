using XCom.Interfaces.Base;

namespace XCom.Services
{
    public class MapResizeService
    {
        public MapTileList ResizeMap(
            int newR, int newC, int newH,
            MapSize mapSize, MapTileList oldMapTileList, bool addHeightToCelling)
        {
            if (newR == 0 ||
                newC == 0 ||
                newH == 0) return null;
            var newMap = new MapTileList(newR , newC , newH);
             
            FillNewMap(newR, newC, newH, newMap);

            for (int h = 0; h < newH && h < mapSize.Height; h++)
                for (int r = 0; r < newR && r < mapSize.Rows; r++)
                    for (int c = 0; c < newC && c < mapSize.Cols; c++)
                    {
                        var copyH =  h ;
                        var currentH =h;
                        if (addHeightToCelling)
                        {
                            copyH = mapSize.Height - h - 1;
                            currentH = newH - h - 1;
                        }
                        newMap[r, c, currentH] = oldMapTileList[r, c, copyH];
                    }
            return newMap;
        }

        private static void FillNewMap(int newR, int newC, int newH, MapTileList newMap)
        {
            for (int h = 0; h < newH; h++)
                for (int r = 0; r < newR; r++)
                    for (int c = 0; c < newC; c++)
                    {
                        newMap[r, c, h] = XCMapTile.BlankTile;
                    }
        }
    }
}
