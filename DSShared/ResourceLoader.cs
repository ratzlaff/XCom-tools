using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Reflection;
using System.IO;

namespace UtilLib
{
	public class ResourceLoader
	{
		public static Stream GetStream(Type t, string file)
		{
			Assembly a = Assembly.GetAssembly(t);
			foreach(string s in a.GetManifestResourceNames())
				if (s.EndsWith(file))
					return a.GetManifestResourceStream(s);
				
			return null;
		}
	}
}
