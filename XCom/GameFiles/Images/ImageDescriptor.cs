using System;
using System.Collections;
using System.IO;
using XCom.GameFiles.Map;

namespace XCom
{
	/// <summary>
	/// Describes information about imagesets: the path to the pck, tab and mcd files
	/// </summary>
	public class ImageDescriptor:IComparable
	{
		//private PckFile myPck;
		//private McdFile myMcd;
		private readonly Hashtable _mcdTab;
		 
		public ImageDescriptor(string baseName, string path)
		{
			BaseName = baseName;
			BasePath = path;
			_mcdTab = new Hashtable(3);
		}

		public PckFile GetPckFile(Palette p)
		{
			return GameInfo.CachePck(BasePath, BaseName, 2, p);
			//new PckFile(File.OpenRead(basePath+basename+".PCK"),File.OpenRead(basePath+basename+".TAB"),2,p,screen);
			//GameInfo.GetPckFile(basename,basePath,p,2,screen);
		}

		public PckFile GetPckFile()
		{
			return GetPckFile(GameInfo.DefaultPalette);
		}

		public McdFile GetMcdFile(Palette p, XcTileFactory _xcTileFactory)
		{
			if (_mcdTab[p] == null)
			{
				var tiles = _xcTileFactory.CreateTiles(BaseName, BasePath, GetPckFile(p));
				_mcdTab[p] = new McdFile(tiles);
			}
			return (McdFile)_mcdTab[p];
		}

		public McdFile GetMcdFile()
		{
			return GetMcdFile(GameInfo.DefaultPalette, new XcTileFactory() );
		}

		public override string ToString()
		{
			return BaseName;
		}

		public int CompareTo(object other)
		{
			return BaseName.CompareTo(other.ToString());
		}

		public string BaseName { get; private set; }

		public string BasePath{get; set; }

		public void ClearMcd()
		{
			_mcdTab.Clear();
		}
	}
}
