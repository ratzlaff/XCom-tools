using System.Windows.Forms;

namespace MapView.Forms.MapObservers.TileViews
{
	public partial class TileViewForm
		:
		Form,
		IMapObserverFormProvider
	{
		public TileViewForm()
		{
			InitializeComponent();
		}

		public MapObserverControl MapObserver
		{
			get { return TileViewControl; }
		}
	}
}
