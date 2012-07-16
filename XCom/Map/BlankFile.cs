using System;
using System.IO;

namespace XCom
{
	public class BlankFile
	{
		public static readonly string Extension=".BLK";

		public static void LoadBlanks(string basename, string blankPath,XCMapFile myFile)
		{
			BinaryReader br = new BinaryReader(File.OpenRead(blankPath+basename+Extension));

			bool flip=true;
			int curr=0;

			while(br.BaseStream.Length>br.BaseStream.Position)
			{
				UInt16 v = br.ReadUInt16();

				if(flip)
				{
					for(int i=curr;i<curr+v;i++)
					{
						int h = i / (myFile.Size.Rows * myFile.Size.Cols);
						int c = i % myFile.Size.Cols;
						int r = (i / myFile.Size.Cols) - h * myFile.Size.Rows;

						((XCMapTile)myFile[r,c,h]).DrawAbove=false;
					}						
				}
	
				curr+=v;
				flip=!flip;
			}

			br.Close();
		}

		public static void SaveBlanks(string basename,string blankPath,XCMapFile myFile)
		{
			if(!Directory.Exists(blankPath))
				Directory.CreateDirectory(blankPath);

			BinaryWriter bw = new BinaryWriter(new FileStream(blankPath+basename+Extension,FileMode.Create));

			UInt16 curr=0;
			bool flip=true;

			for (int h = 0; h < myFile.Size.Height; h++)
				for (int r = 0; r < myFile.Size.Rows; r++)
					for (int c = 0; c < myFile.Size.Cols; c++)
					{
						if(flip)
						{
							if(((XCMapTile)myFile[r,c,h]).DrawAbove)
							{
								flip=!flip;
								bw.Write(curr);
								curr=1;								
							}
							else
								curr++;
						}
						else
						{
							if(((XCMapTile)myFile[r,c,h]).DrawAbove)
								curr++;
							else
							{
								flip=!flip;
								bw.Write(curr);
								curr=1;
							}
						}						
					}

			bw.Flush();
			bw.Close();
		}
	}
}
