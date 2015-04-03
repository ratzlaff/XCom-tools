using System.Collections.Generic;
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
        public MainWindowsShowAllManager(IEnumerable<Form> allForms, IEnumerable<MenuItem> allMenuItems)
        {
            _allForms = allForms;
            _allMenuItems = allMenuItems;
        }

        private readonly IEnumerable<Form> _allForms;
        private readonly IEnumerable<MenuItem> _allMenuItems;
        private List<Form> _formList;
        private List<MenuItem> _menuItems;

        public void HideAll()
        {
            _menuItems = new List<MenuItem>();
            foreach (var menu in _allMenuItems)
            {
                if (!menu.Checked) continue;
                _menuItems.Add(menu);
            }
            _formList = new List<Form>();
            foreach (var form in _allForms)
            {
                if (!form.Visible) continue;
                form.Close();
                _formList.Add(form);
            }
        }

        public void RestoreAll()
        {
            foreach (var form in _formList)
            {
                form.Show();
                form.WindowState = FormWindowState.Normal;
            }
            foreach (var menuItem in _menuItems)
            {
                menuItem.Checked = true;
            }
        }
    } 
}