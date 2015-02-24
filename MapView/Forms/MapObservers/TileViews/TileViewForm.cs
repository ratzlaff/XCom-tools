using System.Windows.Forms;

namespace MapView.Forms.MapObservers.TileViews
{
    public partial class TileViewForm : Form, IMapObserverFormProvider
    {
        public TileViewForm()
        {
            InitializeComponent();
        }

        public Map_Observer_Form MapObserver { get { return TileViewControl; } }
    }
}
