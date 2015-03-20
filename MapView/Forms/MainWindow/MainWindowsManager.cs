using MapView.Forms.MapObservers.RmpViews;
using MapView.Forms.MapObservers.TileViews;
using MapView.Forms.MapObservers.TopViews;
using XCom.Interfaces.Base;

namespace MapView.Forms.MainWindow
{
    public class MainWindowsManager   
    {
        public static IMainWindowsShowAllManager MainWindowsShowAllManager;
        public static MainToolStripButtonsFactory MainToolStripButtonsFactory;

        private static TopViewForm _topView;
        private static TileViewForm _tileView;
        private static RmpViewForm _rmpView;
        private static TopRmpViewForm _topRmpView;
        private static HelpScreen _helpScreen;
        private static AboutWindow _aboutWindow;

        public static TopRmpViewForm TopRmpView
        {
            get
            {
                if (_topRmpView == null)
                {
                    _topRmpView = new TopRmpViewForm();
                    _topRmpView.TopViewControl.Initialize(MainToolStripButtonsFactory);
                }
                return _topRmpView;
            }
        }
        public static RmpViewForm RmpView
        {
            get
            {
                if (_rmpView == null)
                    _rmpView = new RmpViewForm();
                return _rmpView;
            }
        }


        public static TopViewForm TopView
        {
            get
            {
                if (_topView == null)
                {
                    _topView = new TopViewForm();
                    _topView.TopViewControl.Initialize(MainToolStripButtonsFactory);
                }

                return _topView;
            }
        }

        public static TileViewForm TileView
        {
            get
            {
                if (_tileView == null)
                {
                    _tileView = new TileViewForm();
                    _tileView.TileViewControl.Initialize(MainWindowsShowAllManager);
                    _tileView.TileViewControl.SelectedTileTypeChanged += _tileView_SelectedTileTypeChanged;
                }
                return _tileView;
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
            var maps = new IMap_Observer[]
            {
                TopRmpView.TopViewControl, 
                TopRmpView.RouteViewControl, 
                TileView.TileViewControl, 
                RmpView.RouteViewControl,
                TopView.TopViewControl
            };
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
                TopView.TopViewControl.SetSelectedQuadrantFrom(newTile.Info.TileType);
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
