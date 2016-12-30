using System;
using System.Collections.Generic;
using System.Text;

namespace DSShared
{
	/// <summary>
	/// </summary>
	public interface IFilter<T>
	{
		/// <summary>
		/// </summary>
		bool FilterObj(T o);
	}
}
