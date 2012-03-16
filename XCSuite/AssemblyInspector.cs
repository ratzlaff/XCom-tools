using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Reflection;
using UtilLib.Interfaces;
using System.Drawing;

namespace XCSuite
{
	public class AssemblyUpdator:MarshalByRefObject
	{
		private IUpdater updater;
		private Assembly a;
		public bool ContainsUpdater(string assembly)
		{
			a = Assembly.LoadFrom(assembly);

			foreach (Type t in a.GetTypes())
				if (!t.IsInterface && typeof(IUpdater).IsAssignableFrom(t))
				{
					ConstructorInfo ci = t.GetConstructor(Type.EmptyTypes);
					
					updater = (IUpdater)ci.Invoke(null);
					return true;
				}

			return false;
		}

		public string Description
		{
			get { return updater.DisplayDescription; }
		}

		public string DisplayName
		{
			get { return a.GetName().Name; }
		}

		public string UpdatePath
		{
			get { return updater.UpdatePath; }
		}

		public string Version
		{
			get { return a.GetName().Version.Major + "." + a.GetName().Version.Minor; }
		}

		public string BuildVersion
		{
			get { return a.GetName().Version.Build + "." + a.GetName().Version.MinorRevision; }
		}

		public bool RunMe
		{
			get { return a.Location.EndsWith(".exe"); }
		}
	}
}
