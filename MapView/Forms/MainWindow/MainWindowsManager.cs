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
			get { return _topRmpView ?? (_topRmpView = new TopRmpViewForm()); }
		}
		public static RmpViewForm RmpView
		{
			get { return _rmpView ?? (_rmpView = new RmpViewForm()); }
		}

		public static TopViewForm TopView
		{
			get { return _topView ?? (_topView = new TopViewForm()); }
		}

		public static TileViewForm TileView
		{
			get { return _tileView ?? (_tileView = new TileViewForm()); }
		}

		public static HelpScreen HelpScreen
		{
			get { return _helpScreen ?? (_helpScreen = new HelpScreen()); }
		}

		public static AboutWindow AboutWindow
		{
			get { return _aboutWindow ?? (_aboutWindow = new AboutWindow()); }
		}

		public static void Initialize()
		{
			TopRmpView.TopViewControl.Initialize(MainToolStripButtonsFactory);
			TopView.TopViewControl.Initialize(MainToolStripButtonsFactory);
			TileView.TileViewControl.Initialize(MainWindowsShowAllManager);
			TileView.TileViewControl.SelectedTileTypeChanged += _tileView_SelectedTileTypeChanged;
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
				if (frm != null)
					SetMap(map, frm);

			MapViewPanel.Instance.MapView.Refresh();
		}

		private static void _tileView_SelectedTileTypeChanged(TileBase newTile)
		{
			if (newTile != null && newTile.Info != null)
				TopView.TopViewControl.SetSelectedQuadrantFrom(newTile.Info.TileType);
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
