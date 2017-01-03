using System;
using System.IO;
using System.Collections;

namespace DSShared.old
{
	/// <summary>
	/// </summary>
	public class Variable
	{
		private static int count = 0;

		private string varName;
		private string varValue;
		private ArrayList list;

		/// <summary>
		/// </summary>
		public Variable(string prefix, string post)
		{
			varName = "${var" + (count++) + "}";
			varValue = post;
			list = new ArrayList();
			list.Add(prefix);
		}

		/// <summary>
		/// </summary>
		public string Name
		{
			get { return varName; }
		}

		/// <summary>
		/// </summary>
		public string Value
		{
			get { return varValue; }
		}

		/// <summary>
		/// </summary>
		public Variable(string baseVar, string prefix, string post)
		{
			varName = "${var" + baseVar + (count++) + "}";
			varValue = post;
			list = new ArrayList();
			list.Add(prefix);
		}

		/// <summary>
		/// </summary>
		public void Inc(string prefix)
		{
			list.Add(prefix);
		}

		/// <summary>
		/// </summary>
		public void Write(StreamWriter sw)
		{
			Write(sw, "");
		}

		/// <summary>
		/// </summary>
		public void Write(StreamWriter sw, string pref)
		{
			if (list.Count > 1)
			{
				sw.WriteLine(pref + varName + VarCollection.Separator + varValue);
				foreach (string pre in list)
					sw.WriteLine(pref + pre + varName);
			}
			else
				sw.WriteLine(pref + (string)list[0] + varValue);
		}
	}
}
