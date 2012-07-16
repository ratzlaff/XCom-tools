using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel;

using UtilLib;
using UtilLib.Parser;
using MapLib.Parsing;

namespace MapLib.Base.Parsing
{
	public abstract class MapInfo : ParseBlock<Tileset>
	{
		protected ImageDependencyList deps;
		protected List<string> depsPreLink;

		public MapInfo(Tileset inParent, KeyVal inData)
			: base(inParent, inData.Keyword)
		{
			deps = new ImageDependencyList();
			depsPreLink = new List<string>();
			foreach (string s in inData.Rest.Split(' '))
				depsPreLink.Add(s);
		}

		public MapInfo()
			:	base(null, "")
		{
			deps = new ImageDependencyList();
		}

		public Tileset Tileset
		{
			get { return Parent; }
		}

		public virtual void PostLoad()
		{
			Images_dat images = (Images_dat)SharedSpace.Instance["images"];
			foreach (string s in depsPreLink)
				deps.Add(images[s]);
		}

		[Browsable(false)]
		public abstract Base.Map Map
		{
			get;
		}

		public ParseBlockData Dependencies
		{
			get { return deps; }
		}

		protected class ImageDependencyList : ParseBlockData
		{
			protected List<ImageInfo> mData;

			public ImageDependencyList()
			{
				mData = new List<ImageInfo>();
			}

			public override int Count
			{
				get { return mData.Count; }
			}

			public override IEnumerable Data
			{
				get { return mData; }
			}

			public void Add(ImageInfo info)
			{
				mData.Add(info);
			}

			public override string Caption
			{
				get { return "Images"; }
			}

			public override string ToString()
			{
				string rval = "{";

				bool first = true;
				foreach (ImageInfo i in mData) {
					if (!first)
						rval += ", ";
					rval += i.Name;
					first = false;
				}

				return rval + "}";
			}
		}
	}
}
