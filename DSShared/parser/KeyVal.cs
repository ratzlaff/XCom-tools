using System.Collections.Generic;

namespace DSShared
{
	public class KeyVal
	{
		private string keyword, rest;
		private Dictionary<string, KeyVal> kvHash;

		public KeyVal(string keyword, string rest)
		{
			this.keyword = keyword;
			this.rest = rest;
		}

		public string Keyword
		{
			get { return keyword; }
		}

		public string Rest
		{
			get { return rest; }
		}

		public Dictionary<string, KeyVal> SubHash
		{
			get { return kvHash; }
			set { kvHash = value; }
		}

		public override string ToString()
		{
			return keyword + ':' + rest;
		}
	}
}