using System;
using System.Collections.Generic;
using System.Collections;
using System.Linq;
using System.Text;
using System.ComponentModel;
using UtilLib.Parser.Design;
using System.Drawing.Design;

namespace UtilLib.Parser
{
	[Editor(typeof(ParseBlockCollectionEditor), typeof(UITypeEditor))]
	public abstract class ParseBlockData
	{
//		public abstract void CreateOne(string newName);
		public abstract int Count
		{
			get;
		}

		public abstract IEnumerable Data
		{
			get;
		}

		public abstract string Caption
		{
			get;
		}
	}

	public class ParseBlockCollection<T, P> : ParseBlockData, IEnumerable<T> where T : ParseBlock<P>
	{
		protected Dictionary<string, T> mCollection;
		protected List<T> mList;
		protected P mParent;
		protected string mCaption;

		public ParseBlockCollection(P inParent, string inCaption)
		{
			mCollection = new Dictionary<string, T>();
			mList = new List<T>();
			mParent = inParent;
			mCaption = inCaption;
		}

		public override string Caption
		{
			get { return mCaption; }
		}

		public void Add(T item)
		{
			mCollection.Add(item.Name, item);
			mList.Add(item);
		}
		/*
		public override void CreateOne(string newName)
		{
			T newObj = new T();
			newObj.Name = newName;
			newObj.Parent = mParent;
			Add(newObj);
		}*/

		public override int Count
		{
			get { return mCollection.Count; }
		}

		public ICollection<string> Keys
		{
			get { return mCollection.Keys; }
		}

		public T this[string name]
		{
			get
			{
				if (mCollection.Keys.Contains(name))
					return mCollection[name];
				return null;
			}
		}

		public override IEnumerable Data
		{
			get { return mList; }
		}

		public override string ToString()
		{
			string rval = "{";
			bool one = true;
			foreach (T itm in mList) {
				if (!one)
					rval += " ";
				rval += itm.Name;
				one = false;
			}
			rval += "}";
			return rval;
		}

		#region IEnumerable<T> Members

		public IEnumerator<T> GetEnumerator()
		{
			return mList.GetEnumerator();
		}

		#endregion

		#region IEnumerable Members

		IEnumerator IEnumerable.GetEnumerator()
		{
			return mList.GetEnumerator();
		}

		#endregion
	}
}
