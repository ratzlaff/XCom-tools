using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;
using System.Windows.Forms;
using MapView.Forms.MapObservers;
using XCom;
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
        private readonly ConsoleSharedSpace _consoleSharedSpace;

        public MainWindowsMenuItemManager(
            MenuItem showMenu, MenuItem miHelp, MapViewPanel mapView, 
            Dictionary<string, Settings> settingsHash, ConsoleSharedSpace consoleSharedSpace)
        {
            _settingsHash = settingsHash;
            _consoleSharedSpace = consoleSharedSpace;
            _registeredForms = new Dictionary<string, Form>();
            _showMenu = showMenu;
            _miHelp = miHelp;
            _mapView = mapView;
        }

        public void Register()
        {
            MainWindowsManager.TopRmpView.TopViewControl.Settings =
                MainWindowsManager.TopView.TopViewControl.Settings;
            MainWindowsManager.TopRmpView.RouteViewControl.Settings =
                MainWindowsManager.RmpView.RouteViewControl.Settings;

            MainWindowsManager.TopRmpView.TopViewControl.LoadDefaultSettings();
            MainWindowsManager.TopRmpView.RouteViewControl.LoadDefaultSettings();

            RegisterForm(MainWindowsManager.TopView, "Top View", _showMenu, "TopView");
            RegisterForm(MainWindowsManager.TileView, "Tile View", _showMenu, "TileView");
            RegisterForm(MainWindowsManager.RmpView, "Route View", _showMenu, "RmpView");
            RegisterForm(MainWindowsManager.TopRmpView, "Top & Route View", _showMenu);

            RegisterForm(_consoleSharedSpace.GetNewConsole(), "Console", _showMenu);

            RegisterForm(MainWindowsManager.HelpScreen, "Quick Help", _miHelp);
            RegisterForm(MainWindowsManager.AboutWindow, "About", _miHelp);

            MainWindowsManager.TopRmpView.TopViewControl.RegistryInfo =
                MainWindowsManager.TopView.TopViewControl.RegistryInfo;
            MainWindowsManager.TopRmpView.RouteViewControl.RegistryInfo =
                MainWindowsManager.RmpView.RouteViewControl.RegistryInfo;
        }

        private void RegisterForm(Form form, string title, MenuItem parent, string registryKey = null)
        {
            form.Closing += FormClosing;

            form.Text = title;
            var mi = new MenuItem(title);
            mi.Tag = form;

            if (form is IMenuItem)
            {
                ((IMenuItem) form).MenuItem = mi;
            }

            var observerForm = form as IMapObserverFormProvider;
            if (observerForm != null)
            {
                var observer = observerForm.MapObserver;
                observer.LoadDefaultSettings();
                observer.RegistryInfo = new DSShared.Windows.RegistryInfo(form, registryKey);

                _settingsHash.Add(registryKey, observer.Settings);
            }

            form.ShowInTaskbar = false;
            form.FormBorderStyle = FormBorderStyle.SizableToolWindow;

            parent.MenuItems.Add(mi);
            mi.Click += FormMiClick;
            _registeredForms.Add(title, form);
        }

        private static void FormMiClick(object sender, EventArgs e)
        {
            var mi = (MenuItem)sender;

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

        private static void FormClosing(object sender, CancelEventArgs e)
        {
            e.Cancel = true;
            if (sender is IMenuItem)
                ((IMenuItem)sender).MenuItem.Checked = false;
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
