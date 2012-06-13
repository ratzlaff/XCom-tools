using System;
using System.IO;
using System.Collections.Generic;
using System.Text;

using UtilLib.Parser;

namespace UtilLib
{
	public abstract class FileDesc
	{
		public FileDesc(string inPath)
		{
			mPath = inPath;
		}

		public abstract void Save(string outFile);

		private string mPath;
		public string Path { get { return mPath; } }
	}

	public abstract class FileDescType<T> : FileDesc where T : IParseBlock
	{
		protected double version;
		protected Dictionary<string, T> items;

		public FileDescType(string inPath)
			:	base(inPath)
		{
			items = new Dictionary<string, T>();
			version = 1.0;
		}

		public abstract void Parse(VarCollection v);

		public ICollection<string> Descriptors
		{
			get { return items.Keys; }
		}

		public T this[string key]
		{
			get { return items[key]; }
			set { items[key] = value; }
		}

		public ICollection<T> Items
		{
			get { return items.Values; }
		}
	}
}
