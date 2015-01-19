using System;
using System.IO;
using XCom.Interfaces;

namespace XCom.GameFiles.Map
{
     public class XcTileFactory: IWarningNotifier
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

                 var dead = GetDeadValue(basename, i, mcdEntry, tiles);
                 var alternate = GetAlternate(basename, i, mcdEntry, tiles);
                 var tile = new XCTile(i, pckFile, mcdEntry, tiles);
                 tile.Dead = dead;
                 tile.Alternate = alternate;
                 tiles[i] = tile;
             }
              
             file.Close();

             return tiles;
         }

         private XCTile GetAlternate(string basename, int index, McdEntry info, XCTile[] tiles)
         {
             if (info.UFODoor || info.HumanDoor || info.Alt_MCD != 0)
             {
                 if (tiles.Length < info.Alt_MCD)
                 {
                     OnHandleWarning(string.Format(
                         "In the MCD file {3}, the tile entry {0} have an invalid alternative tile (# {1} of {2} tiles)",
                         index, info.Alt_MCD, tiles.Length, basename));
                     return null;
                 }
                 return tiles[info.Alt_MCD];
             }
             return null;
         }

         private XCTile GetDeadValue(string basename, int index, McdEntry info, XCTile[] tiles)
         {
             try
             {
                 if (info.DieTile != 0)
                 {
                     return tiles[(info).DieTile];
                 }
             }
             catch
             {
                 OnHandleWarning(string.Format(
                     "In the MCD file {3}, the tile entry {0} have an invalid dead tile (# {1} of {2} tiles)",
                     index, info.Alt_MCD, tiles.Length, basename));
             }
             return null;
         }

         public event Action<string> HandleWarning;
         protected virtual void OnHandleWarning(string message)
         {
             Action<string> handler = HandleWarning;
             if (handler != null)
             {
                 handler(message);
             }
             else
             {
                 throw new ApplicationException(message);
             }
         }
     }
}
