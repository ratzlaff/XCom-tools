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
            RegisterForm(MainWindowsManager.TopView, "TopView", _showMenu);
            RegisterForm(MainWindowsManager.TileView, "TileView", _showMenu);
            RegisterForm(MainWindowsManager.RmpView, "RmpView", _showMenu);

            RegisterForm(_consoleSharedSpace.GetNewConsole(), "Console", _showMenu);

            RegisterForm(MainWindowsManager.HelpScreen, "Quick Help", _miHelp);
            RegisterForm(MainWindowsManager.AboutWindow, "About", _miHelp);
        }

        private void RegisterForm(Form form, string title, MenuItem parent)
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
                observer.RegistryInfo = new DSShared.Windows.RegistryInfo(form, title);

                _settingsHash.Add(title, observer.Settings);
            }

            form.ShowInTaskbar = false;
            form.FormBorderStyle = FormBorderStyle.SizableToolWindow;

            parent.MenuItems.Add(mi);
            mi.Click += FormMiClick;
            _registeredForms.Add(title, form);
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
