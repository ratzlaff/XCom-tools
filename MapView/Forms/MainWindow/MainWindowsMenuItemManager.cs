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
	public interface IMainWindowWindowsManager
	{
		void Register();
		void CloseAll();
	}

	public class MainWindowWindowsManager : IMainWindowWindowsManager
	{
		private readonly Dictionary<string, Form> _registeredForms;
		private readonly SettingsManager _settingsManager;
		private readonly ConsoleSharedSpace _consoleSharedSpace;

		public MainWindowWindowsManager(SettingsManager settingsManager, ConsoleSharedSpace consoleSharedSpace)
		{
			_settingsManager = settingsManager;
			_consoleSharedSpace = consoleSharedSpace;
			_registeredForms = new Dictionary<string, Form>();
		}

		public void Register()
		{
			MainWindowsManager.TopRmpView.TopViewControl.Settings =
				MainWindowsManager.TopView.TopViewControl.Settings;
			MainWindowsManager.TopRmpView.RouteViewControl.Settings =
				MainWindowsManager.RmpView.RouteViewControl.Settings;

			MainWindowsManager.TopRmpView.TopViewControl.LoadDefaultSettings();
			MainWindowsManager.TopRmpView.RouteViewControl.LoadDefaultSettings();

			RegisterForm(MainWindowsManager.TopView, "Top View", "TopView");
			RegisterForm(MainWindowsManager.RmpView, "Route View", "RmpView");
			RegisterForm(MainWindowsManager.TopRmpView, "Top & Route View");
			RegisterForm(MainWindowsManager.TileView, "Tile View", "TileView");

			RegisterForm(_consoleSharedSpace.GetNewConsole(), "Console");

			RegisterForm(MainWindowsManager.HelpScreen, "Quick Help");
			RegisterForm(MainWindowsManager.AboutWindow, "About");

			MainWindowsManager.TopRmpView.TopViewControl.RegistryInfo =
				MainWindowsManager.TopView.TopViewControl.RegistryInfo;
			MainWindowsManager.TopRmpView.RouteViewControl.RegistryInfo =
				MainWindowsManager.RmpView.RouteViewControl.RegistryInfo;
		}

		private void RegisterForm(Form form, string title, string registryKey = null)
		{
			form.Text = title;

			var observerForm = form as IMapObserverFormProvider;
			if (observerForm != null)
			{
				var observer = observerForm.MapObserver;
				observer.LoadDefaultSettings();
				observer.RegistryInfo = new DSShared.Windows.RegistryInfo(form, registryKey);
				_settingsManager.Add(registryKey, observer.Settings);
			}

			form.ShowInTaskbar = false;
			form.FormBorderStyle = FormBorderStyle.SizableToolWindow;

			_registeredForms.Add(title, form);
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
