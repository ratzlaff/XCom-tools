using System;
using System.Collections.Generic;
using System.Text;

namespace MapView.Forms.MapObservers
{
	public interface IMapObserverFormProvider
	{
		MapObserverControl MapObserver
		{ get; }
	}
}
