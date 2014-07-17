using System.Drawing;

namespace XCom
{
    public class McdEntryFactory
    {
        public McdEntry Create(byte[] info)
        {
            var entry = new McdEntry();
            
            entry.Image1 = info[0];
            entry.Image2 = info[1];
            entry.Image3 = info[2];
            entry.Image4 = info[3];
            entry.Image5 = info[4];
            entry.Image6 = info[5];
            entry.Image7 = info[6];
            entry.Image8 = info[7];

            entry.Loft1 = info[8];
            entry.Loft2 = info[9];
            entry.Loft3 = info[10];
            entry.Loft4 = info[11];
            entry.Loft5 = info[12];
            entry.Loft6 = info[13];
            entry.Loft7 = info[14];
            entry.Loft8 = info[15];
            entry.Loft9 = info[16];
            entry.Loft10 = info[17];
            entry.Loft11 = info[18];
            entry.Loft12 = info[19];

            entry.ScanG = (ushort) (info[21] * 255 + info[20]);

            entry.Unknown22 = info[22];
            entry.Unknown23 = info[23];
            entry.Unknown24 = info[24];
            entry.Unknown25 = info[25];
            entry.Unknown26 = info[26];
            entry.Unknown27 = info[27];
            entry.Unknown28 = info[28];
            entry.Unknown29 = info[29];

            entry.UFODoor = info[30] == 1;
            entry.StopLOS = info[31] != 1; //				unsigned char Stop_LOS;      //You cannot see through this tile.
            entry.NoGround = info[32] == 1; //			unsigned char No_Floor;      //If 1, then a non-flying unit can't stand here
            entry.BigWall = info[33] == 1;
            entry.GravLift = info[34] == 1; //			unsigned char Gravlift;
            entry.HumanDoor = info[35] == 1;
            entry.BlockFire = info[36] == 1; //			unsigned char Block_Fire;       //If 1, fire won't go through the tile
            entry.BlockSmoke = info[37] == 1; //			unsigned char Block_Smoke;      //If 1, smoke won't go through the tile

            entry.Unknown38 = info[38]; //				unsigned char u39;
            entry.TU_Walk = info[39];
            entry.TU_Fly = info[40]; //					unsigned char TU_Fly;        // remember, 0xFF means it's impassable!
            entry.TU_Slide = info[41]; //				unsigned char TU_Slide;      // sliding things include snakemen and silacoids
            entry.Armor = info[42];
            entry.HE_Block = info[43]; //				unsigned char HE_Block;      //How much of an explosion this tile will block
            entry.DieTile = info[44];
            entry.Flammable = info[45];
            entry.Alt_MCD = info[46];
            entry.Unknown47 = info[47]; //				unsigned char u48;
            entry.StandOffset = (sbyte) info[48];
            entry.TileOffset = (sbyte) info[49];
            entry.Unknown50 = info[50]; //				unsigned char u51;
            entry.LightBlock = (sbyte) info[51]; //		unsigned char Light_Block;     //The amount of light it blocks, from 0 to 10
            entry.Footstep = (sbyte) info[52];

            entry.TileType = (TileType) info[53];
            entry.HE_Type = (sbyte) info[54]; //			unsigned char HE_Type;         //0=HE  1=Smoke
            entry.HE_Strength = (sbyte) info[55];
            entry.SmokeBlockage = (sbyte) info[56]; //	unsigned char Smoke_Blockage;      //? Not sure about this ...
            entry.Fuel = (sbyte) info[57];
            entry.LightSource = (sbyte) info[58]; //		unsigned char Light_Source;      //The amount of light this tile produces
            entry.TargetType = (SpecialType) (sbyte) info[59];
            entry.Unknown60 = info[60]; //				unsigned char u61;
            entry.Unknown61 = info[61]; //				unsigned char u62;

            entry.ScanGReference = string.Format("scang reference: {0} {1} -> {2}\n", info[20], info[21], info[20] * 256 + info[21]);
            entry.LoftReference = string.Format("loft references: {0} {1} {2} {3} {4} {5} {6} {7} {8} {9} {10} {11}\n", info[8],
                info[9], info[10], info[11], info[12], info[13], info[14], info[15], info[16], info[17], info[18], info[19]);

            entry.Reference0To30 = string.Empty;
            for (int i = 0; i < 30; i++)
                entry.Reference0To30 += info[i] + " ";
            
            entry.Reference30To62 = string.Empty;
            for (int i = 30; i < 62; i++)
                entry.Reference30To62 += info[i] + " ";

            entry.Bounds = new Rectangle(0, entry.TileOffset, PckImage.Width, PckImage.Height - entry.TileOffset);
            entry.Width = PckImage.Width;
            entry.Height = PckImage.Height - entry.TileOffset;
            return entry;
        }
    }
}