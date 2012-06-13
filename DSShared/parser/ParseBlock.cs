using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel;

namespace UtilLib.Parser
{
	public interface IParseBlock
	{
		[Category("Node")]
		string Name
		{
			get;
			set;
		}

		void Parse(VarCollection vars);
	}

	public class ParseBlock<P> : IParseBlock
	{
		protected string mName;
		protected P mParent;

		public ParseBlock(P inParent, string inName)
		{
			mName = inName;
			mParent = inParent;
		}

		public string Name
		{
			get { return mName; }
			set { mName = value; }
		}

		[Browsable(false)]
		public P Parent
		{
			get { return mParent; }
			set { mParent = value; }
		}

		public override string ToString() { return Name; }

		public void Parse(VarCollection vars)
		{
			KeyVal kv = null;

			while (vars.ReadLine(out kv)) {
				switch (kv.Keyword.ToLower()) {
					case "end":
						return;
					default:
						ProcessVar(vars, kv);
						break;
				}
			}
		}

		protected virtual void ProcessVar(VarCollection vars, KeyVal current)
		{
			Console.WriteLine("Unhandled var: {0}:{1}", current.Keyword, current.Rest);
		}
	}
}
