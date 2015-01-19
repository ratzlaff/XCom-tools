using System.Collections.Generic;
using System.Windows.Forms;
using XCom;
using XCom.Interfaces.Base;

namespace MapView.Forms.MainWindow
{
    public interface IMainWindowsShowAllManager
    {
        void HideAll();
        void RestoreAll();
    }

    public class MainWindowsShowAllManager : IMainWindowsShowAllManager
    {
        public MainWindowsShowAllManager(ConsoleSharedSpace consoleSharedSpace)
        {
            _consoleSharedSpace = consoleSharedSpace;
        }

        private readonly ConsoleSharedSpace _consoleSharedSpace;
        private List<Form> _formList;

        public void HideAll()
        {
            var tempFormList = new List<Form>()
            {
                MainWindowsManager.TopView,
                MainWindowsManager.TileView,
                MainWindowsManager.RmpView,
            };
            var console =_consoleSharedSpace.GetConsole();
            if (console != null) tempFormList.Add(console);

            _formList = new List<Form>();
            foreach (var form in tempFormList)
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
                var mapObserverForm = form as IMenuItem;
                if (mapObserverForm != null)
                {
                    mapObserverForm.MenuItem.Checked = true;
                }
            } 
        }
    } 
}