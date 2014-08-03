using System.Windows.Forms;

namespace MapView.Forms.MainWindow
{
    public interface IMainWindowsShowAllManager
    {
        void HideAll();
        void RestoreAll();
    }

    public class MainWindowsShowAllManager : IMainWindowsShowAllManager
    {
        private bool _topViewVisible;
        private bool _tileViewVisible;
        private bool _rmpViewVisible;
         
        public void HideAll()
        {
            _topViewVisible = MainWindowsManager.TopView.Visible;
            _tileViewVisible = MainWindowsManager.TileView.Visible;
            _rmpViewVisible = MainWindowsManager.RmpView.Visible;

            if (_topViewVisible) MainWindowsManager.TopView.Close();
            if (_tileViewVisible) MainWindowsManager.TileView.Close();
            if (_rmpViewVisible) MainWindowsManager.RmpView.Close();
        }

        public void RestoreAll()
        {
            if (_topViewVisible)
            {
                MainWindowsManager.TopView.Show();
                MainWindowsManager.TopView.WindowState = FormWindowState.Normal;
                MainWindowsManager.TopView.MenuItem.Checked = true;
            }
            if (_tileViewVisible)
            {
                MainWindowsManager.TileView.Show();
                MainWindowsManager.TileView.WindowState = FormWindowState.Normal;
                MainWindowsManager.TileView.MenuItem.Checked = true;
            }
            if (_rmpViewVisible)
            {
                MainWindowsManager.RmpView.Show();
                MainWindowsManager.RmpView.WindowState = FormWindowState.Normal;
                MainWindowsManager.RmpView.MenuItem.Checked = true;
            }
        }
    } 
}