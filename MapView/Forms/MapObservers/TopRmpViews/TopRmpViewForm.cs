using System.Windows.Forms;
using DSShared.Windows;

namespace MapView.Forms.MapObservers.TileViews
{
    public partial class TopRmpViewForm : Form
    {
        public TopRmpViewForm()
        {
            InitializeComponent();
            RegistryInfo = new RegistryInfo(this, "TopRmpViewForm");
        }

        public RegistryInfo RegistryInfo;
    }
}
