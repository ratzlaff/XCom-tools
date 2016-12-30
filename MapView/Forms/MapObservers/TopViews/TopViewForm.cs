using System.Windows.Forms;

namespace MapView.Forms.MapObservers.TopViews
{
	public partial class TopViewForm
		:
		Form,
		IMapObserverFormProvider
	{
		public TopViewForm()
		{
			InitializeComponent();
		}

		public MapObserverControl MapObserver
		{
			get { return TopViewControl; }
		}
	}
}
