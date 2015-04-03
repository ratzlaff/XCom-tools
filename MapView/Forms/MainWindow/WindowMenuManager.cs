using System;
using System.Collections.Generic;
using System.Windows.Forms;
using XCom;
using XCom.Interfaces.Base;

namespace MapView.Forms.MainWindow
{
    public class WindowMenuManager
    {
        private readonly MenuItem _showMenu;
        private readonly MenuItem _miHelp;
        private readonly List<MenuItem> _allMenuItems = new List<MenuItem>();
        private readonly List<Form> _allForms = new List<Form>();

        public WindowMenuManager(MenuItem showMenu, MenuItem miHelp)
        {
            _showMenu = showMenu;
            _miHelp = miHelp;
        }

        public void SetMenus(ConsoleForm consoleWindow)
        {
            RegisterForm(MainWindowsManager.TopView, "Top View", _showMenu, "TopView");
            RegisterForm(MainWindowsManager.TileView, "Tile View", _showMenu, "TileView");
            RegisterForm(MainWindowsManager.RmpView, "Route View", _showMenu, "RmpView");
            RegisterForm(MainWindowsManager.TopRmpView, "Top & Route View", _showMenu);

            RegisterForm(consoleWindow, "Console", _showMenu);

            RegisterForm(MainWindowsManager.HelpScreen, "Quick Help", _miHelp);
            RegisterForm(MainWindowsManager.AboutWindow, "About", _miHelp);
        }


        public IMainWindowsShowAllManager CreateShowAll(   )
        { 
            return new MainWindowsShowAllManager(
                _allForms, _allMenuItems);
        }

        public void RegisterForm(Form form, string title, MenuItem parent, string registryKey = null)
        {
            form.Text = title;
            var mi = new MenuItem(title);
            mi.Tag = form;

            parent.MenuItems.Add(mi);
            mi.Click += FormMiClick;
            form.Closing += (sender, e) =>
            {
                e.Cancel = true;
                mi.Checked = false;
                form.Hide();
            };

            _allMenuItems.Add(mi);
            _allForms.Add(form);
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
    }
}
