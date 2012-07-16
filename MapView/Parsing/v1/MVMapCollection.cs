using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Windows.Forms.Design;
using System.Drawing.Design;
using System.Text;

using UtilLib.Parser;
using MapLib.Base.Parsing;

namespace MapView.Parsing.v1
{
	public class MVMapCollection : MapCollection
	{
		protected string rootPath, rmpPath, palette;

		public MVMapCollection(string inName)
			: base(inName)
		{
		}

		[Editor(typeof(FolderNameEditor), typeof(UITypeEditor))]
		public string RootPath
		{
			get { return rootPath; }
			set { rootPath = value; }
		}

		[Editor(typeof(FolderNameEditor), typeof(UITypeEditor))]
		public string RmpPath
		{
			get { return rmpPath; }
			set { rmpPath = value; }
		}

		protected override void ProcessVar(VarCollection vars, KeyVal current)
		{
			switch (current.Keyword.ToLower()) {
				case "type":
					break;
				case "palette":
					palette = current.Rest;
					break;
				case "rootpath":
					rootPath = current.Rest;
					break;
				case "rmppath":
					rmpPath = current.Rest;
					break;
				case "blankpath":
					break;
				case "files": {
						MVTileset t = new MVTileset(this, current.Rest);
						t.Parse(vars);
						mCollection.Add(t);
					}
					break;
				default:
					base.ProcessVar(vars, current);
					break;
			}
		}
	}
}
