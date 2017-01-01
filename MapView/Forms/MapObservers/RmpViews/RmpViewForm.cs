using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace MapView.Forms.MapObservers.RmpViews
{
	public partial class RmpViewForm
		:
		Form,
		IMapObserverFormProvider
	{
		public RmpViewForm()
		{
			InitializeComponent();
		}

		public MapObserverControl MapObserver
		{
			get { return RouteViewControl; }
		}

//		void RmpViewFormLoad(object sender, EventArgs e)
//		{}
	}
}
