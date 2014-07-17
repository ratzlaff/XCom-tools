using System.IO;

namespace XCom.GameFiles.Map
{
     public class XcTileFactory
     {
         private readonly  string _basename;
         private readonly string _directory;
         private readonly PckFile _pckFile;

         public XcTileFactory(string basename, string directory, PckFile pckFile)
         {
             _basename = basename;
             _directory = directory;
             _pckFile = pckFile;
         }

         public XCTile[] CreateTiles(McdFile mcdFile)
         {
             var file = new BufferedStream(File.OpenRead(_directory + _basename + ".MCD"));
             int diff = 0;
             if (_basename == "XBASES05")
                 diff = 3;
             const int TILE_SIZE = 62;
             var tiles = new XCTile[(((int) file.Length) / TILE_SIZE) - diff];
             var factory = new McdEntryFactory();

             for (var i = 0; i < tiles.Length; i++)
             {
                 var info = new byte[TILE_SIZE];
                 file.Read(info, 0, TILE_SIZE);
                 var mcdEntry = factory.Create(info);
                 tiles[i] = new XCTile(i, _pckFile, mcdEntry, mcdFile);
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
