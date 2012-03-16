using System;
using System.Collections.Generic;
using System.Text;

namespace UtilLib
{
	public interface IFilter<T>
	{
		bool FilterObj(T o);
	}
}
