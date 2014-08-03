using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;
using System.Windows.Forms;
using XCom.Interfaces.Base;

namespace MapView.Forms.MainWindow
{
    public interface IMainWindowsMenuItemManager
    {
        void Register();
        void CloseAll();
    }

    public class MainWindowsMenuItemManager : IMainWindowsMenuItemManager
    {
        private readonly Dictionary<string, Form> _registeredForms;
        private readonly Dictionary<string, Settings> _settingsHash;
        private readonly MenuItem _showMenu;  
        private readonly MenuItem _miHelp;
        private readonly MapViewPanel _mapView;

        public MainWindowsMenuItemManager(
            MenuItem showMenu, MenuItem miHelp, MapViewPanel mapView, Dictionary<string, Settings> settingsHash)
        {
            _settingsHash = settingsHash;
            _registeredForms = new Dictionary<string, Form>();
            _showMenu = showMenu;
            _miHelp = miHelp;
            _mapView = mapView;
        }

        public void Register()
        {
            RegisterForm(MainWindowsManager.TopView, "TopView", _showMenu);
            RegisterForm(MainWindowsManager.TileView, "TileView", _showMenu);
            RegisterForm(MainWindowsManager.RmpView, "RmpView", _showMenu);

            if (XCom.Globals.UseBlanks)
                RegisterForm(_mapView.BlankForm, "Blank Info", _showMenu);

            RegisterForm(MainWindowsManager.HelpScreen, "Quick Help", _miHelp);
            RegisterForm(MainWindowsManager.AboutWindow, "About", _miHelp);
        }

        private void RegisterForm(Form f, string title, MenuItem parent)
        {
            f.Closing += FormClosing;

            f.Text = title;
            MenuItem mi = new MenuItem(title);
            mi.Tag = f;

            if (f is Map_Observer_Form)
            {
                ((Map_Observer_Form)f).MenuItem = mi;
                ((Map_Observer_Form)f).RegistryInfo = new DSShared.Windows.RegistryInfo(f, title);
                _settingsHash.Add(title, ((Map_Observer_Form)f).Settings);
            }

            f.ShowInTaskbar = false;
            f.FormBorderStyle = FormBorderStyle.SizableToolWindow;

            parent.MenuItems.Add(mi);
            mi.Click += FormMiClick;
            _registeredForms.Add(title, f);
        }

        private static void FormMiClick(object sender, EventArgs e)
        {
            MenuItem mi = (MenuItem)sender;

            if (!mi.Checked)
            {
                ((Form)mi.Tag).Show();
                ((Form)mi.Tag).WindowState = FormWindowState.Normal;
                mi.Checked = true;
            }
            else
            {
                ((Form)mi.Tag).Close();
                mi.Checked = false;
            }
        }

        public void ResetMenuItemValueFor(Map_Observer_Form form)
        {
            
        }

        private static void FormClosing(object sender, CancelEventArgs e)
        {
            e.Cancel = true;
            if (sender is Map_Observer_Form)
                ((Map_Observer_Form)sender).MenuItem.Checked = false;
            ((Form)sender).Hide();
        }

        public void CloseAll()
        {
			foreach (string key in _registeredForms.Keys)
			{
				Form form = _registeredForms[key];
				form.WindowState = FormWindowState.Normal;
				form.Close();
			}
        } 
    }
}
