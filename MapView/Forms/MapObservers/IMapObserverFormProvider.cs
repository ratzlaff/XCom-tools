using System;
using System.Collections.Generic;
using System.Text;

namespace MapView.Forms.MapObservers
{
    public interface IMapObserverFormProvider
    {
        Map_Observer_Form MapObserver { get; }
    }
}
