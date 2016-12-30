using System;
using System.Collections;
using System.IO;

namespace DSShared.old
{
	/// <summary>
	/// </summary>
	public class VarCollection
	{
		private Hashtable vars;
		private VarCollection other;
		private string baseVar;
		private StreamReader sr;

		/// <summary>
		/// </summary>
		public static readonly char Separator = ':';

		/// <summary>
		/// </summary>
		public VarCollection()
		{
			vars = new Hashtable();
			other = null;
			baseVar = "";
		}

		/// <summary>
		/// </summary>
		public VarCollection(StreamReader sr)
		{
			this.sr=sr;
			vars = new Hashtable();
			other=null;
		}

		/// <summary>
		/// </summary>
		public VarCollection(string baseVar)
			:
			this()
		{
			this.baseVar = baseVar;
		}

		/// <summary>
		/// </summary>
		public VarCollection(VarCollection other)
			:
			this()
		{
			this.other = other;
		}

		/// <summary>
		/// </summary>
		public void AddVar(string flag, string val)
		{
			if (vars[val] == null)
				vars[val] = new Variable(baseVar, flag + Separator, val);
			else
				((Variable)vars[val]).Inc(flag + Separator);
		}

		/// <summary>
		/// </summary>
		public Hashtable Vars
		{
			get { return vars; }
		}

		/// <summary>
		/// </summary>
		public StreamReader BaseStream
		{
			get { return sr; }
		}

		/// <summary>
		/// </summary>
		public string ParseVar(string line)
		{
			foreach(string s in vars.Keys)
				line = line.Replace(s, (string)vars[s]);

			if (other != null)
				return other.ParseVar(line);

			return line;
		}

		/// <summary>
		/// </summary>
		public ICollection Variables
		{
			get { return vars.Keys; }
		}

		/// <summary>
		/// </summary>
		public string this[string var]
		{
			get
			{
				if (other == null || vars[var] != null)
					return (string)vars[var];

				return other[var];
			}

			set { vars[var] = value; }
		}

		/// <summary>
		/// </summary>
		public bool ReadLine(out KeyVal output)
		{
			return (output = ReadLine()) == null;
		}

		/// <summary>
		/// </summary>
		public KeyVal ReadLine()
		{
			string line = ReadLine(sr, this);
			if (line != null)
			{
				int idx = line.IndexOf(Separator);
				if (idx > 0)
					return new KeyVal(line.Substring(0, idx), line.Substring(idx + 1));
				else
					return new KeyVal(line, "");
			}
			return null;
		}

		/// <summary>
		/// </summary>
		public string ReadLine(StreamReader sr)
		{
			return ReadLine(sr, this);
		}

		/// <summary>
		/// </summary>
		public static string ReadLine(StreamReader sr, VarCollection vars)
		{
			string line = "";

			while (true)
			{
				do // get a good line - not a comment or empty string
				{
					if (sr.Peek() != -1)
						line = sr.ReadLine().Trim();
					else
						return null;
				} while (line.Length == 0 || line[0] == '#');

				if (line[0] == '$') // cache variable, get another line
				{
					int idx = line.IndexOf(Separator);
					string var = line.Substring(0, idx);
					string val = line.Substring(idx + 1);
					vars[var] = val;
				}
				else // got a line
					break;
			}

			if (line.IndexOf("$") > 0) // replace any variables the line might have
				line = vars.ParseVar(line);

			return line;
		}
	}

	/// <summary>
	/// </summary>
	public class KeyVal
	{
		private string keyword,rest;

		/// <summary>
		/// </summary>
		public KeyVal(string keyword, string rest)
		{
			this.keyword = keyword;
			this.rest = rest;
		}

		/// <summary>
		/// </summary>
		public string Keyword
		{
			get { return keyword; }
		}

		/// <summary>
		/// </summary>
		public string Rest
		{
			get { return rest; }
		}

		/// <summary>
		/// </summary>
		public override string ToString()
		{
			return keyword + ':' + rest;
		}
	}
}
