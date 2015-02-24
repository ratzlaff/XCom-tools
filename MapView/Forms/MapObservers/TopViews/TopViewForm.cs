using System.Windows.Forms;

namespace MapView.Forms.MapObservers.TopViews
{
    public partial class TopViewForm : Form, IMapObserverFormProvider 
    {
        public TopViewForm()
        {
            InitializeComponent();
        }

        public Map_Observer_Form MapObserver { get { return TopViewControl; } }
    }
}
