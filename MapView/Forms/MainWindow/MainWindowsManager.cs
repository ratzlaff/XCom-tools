using System;
using MapView.RmpViewForm;
using MapView.TopViewForm;
using XCom;
using XCom.Interfaces.Base;

namespace MapView.Forms.MainWindow
{
    public class MainWindowsManager
    {
        public static IMainWindowsShowAllManager MainWindowsShowAllManager;
        public static readonly MainToolStripButtonsFactory MainToolStripButtonsFactory = new MainToolStripButtonsFactory();

        private static TileView _tileView;
        private static TopView _topView;
        private static RmpView _rmpView;
        private static HelpScreen _helpScreen;
        private static AboutWindow _aboutWindow;

        public static TileView TileView
        {
            get
            {
                if (_tileView == null)
                {
                    _tileView = new TileView(MainWindowsShowAllManager);
                    _tileView.SelectedTileTypeChanged += _tileView_SelectedTileTypeChanged;
                }
                return _tileView;
            }
        }

        public static TopView TopView
        {
            get
            {
                if (_topView == null)
                    _topView = new TopView(MainToolStripButtonsFactory);

                return _topView;
            }
        }

        public static RmpView RmpView
        {
            get
            {
                if (_rmpView == null)
                    _rmpView = new RmpView();
                return _rmpView;
            }
        }

        public static HelpScreen HelpScreen
        {
            get
            {
                if (_helpScreen == null)
                    _helpScreen = new HelpScreen();
                return _helpScreen;
            }
        }

        public static AboutWindow AboutWindow
        {
            get
            {
                if (_aboutWindow == null)
                    _aboutWindow = new AboutWindow();
                return _aboutWindow;
            }
        }

        public void SetMap(IMap_Base map)
        {
            var maps = new Map_Observer_Form[] {TileView ,TopView, RmpView};
            foreach (var frm in maps)
            {
                if (frm != null)
                {
                    SetMap(map, frm);
                }
            }
            MapViewPanel.Instance.View.Refresh();
        }

        private static void _tileView_SelectedTileTypeChanged(TileBase newTile)
        {
            if (newTile != null &&  
                newTile.Info != null )
            {
                TopView.SetSelectedQuadrantFrom(newTile.Info.TileType);
            }
        }

        private void SetMap(IMap_Base newMap, IMap_Observer observer)
        {
            if (observer.Map != null)
            {
                observer.Map.HeightChanged -= observer.HeightChanged;
                observer.Map.SelectedTileChanged -= observer.SelectedTileChanged;
            }

            observer.Map = newMap;
            if (newMap != null)
            {
                newMap.HeightChanged += observer.HeightChanged;
                newMap.SelectedTileChanged += observer.SelectedTileChanged;
            }

            foreach (string key in observer.MoreObservers.Keys)
                SetMap(newMap, observer.MoreObservers[key]);
        }
    }
}
