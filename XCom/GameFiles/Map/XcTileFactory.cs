using System.IO;

namespace XCom.GameFiles.Map
{
     public class XcTileFactory
     {  
         public XCTile[] CreateTiles(string basename, string directory, PckFile pckFile)
         {
             var file = new BufferedStream(File.OpenRead(directory + basename + ".MCD"));
             int diff = 0;
             if (basename == "XBASES05")
                 diff = 3;
             const int TILE_SIZE = 62;
             var tiles = new XCTile[(((int) file.Length) / TILE_SIZE) - diff];
             var factory = new McdEntryFactory();

             for (var i = 0; i < tiles.Length; i++)
             {
                 var info = new byte[TILE_SIZE];
                 file.Read(info, 0, TILE_SIZE);
                 var mcdEntry = factory.Create(info);
                 tiles[i] = new XCTile(i, pckFile, mcdEntry);
             }

             foreach (var t in tiles)
             {
                 t.Tiles = tiles;
             }
             file.Close();

             return tiles;
         }
     }
}
