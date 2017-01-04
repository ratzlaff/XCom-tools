using System;
using System.Collections.Generic;
using System.Windows.Forms;
using MapView.Forms.MapObservers.RmpViews;
using MapView.Forms.MapObservers.TopViews;
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

		private Settings _settings;

		private bool _isDisposed;

		public WindowMenuManager(MenuItem showMenu, MenuItem miHelp)
		{
			_showMenu = showMenu;
			_miHelp = miHelp;
		}

		public void SetMenus(ConsoleForm consoleWindow, Settings settings)
		{
			_settings = settings;

			RegisterForm(
						MainWindowsManager.TileView,
						"Tile View",
						_showMenu,
						"TileView");

			_showMenu.MenuItems.Add(new MenuItem("-"));

			RegisterForm(
						MainWindowsManager.TopView,
						"Top View",
						_showMenu,
						"TopView");
			RegisterForm(
						MainWindowsManager.RmpView,
						"Route View",
						_showMenu,
						"RmpView");
			RegisterForm(
						MainWindowsManager.TopRmpView,
						"Top & Route View",
						_showMenu);

			_showMenu.MenuItems.Add(new MenuItem("-"));

			RegisterForm(
						consoleWindow,
						"Console",
						_showMenu);

			RegisterForm(
						MainWindowsManager.HelpScreen,
						"Quick Help",
						_miHelp);
			RegisterForm(
						MainWindowsManager.AboutWindow,
						"About",
						_miHelp);

			RegisterWindowMenuItemValue();
		}

		public void LoadState()
		{
			foreach (MenuItem mi in _showMenu.MenuItems)
			{
				var settingName = GetWindowSettingName(mi);
				if (_settings[settingName].ValueBool)
				{
					mi.PerformClick();
				}
				else
				{
					mi.PerformClick();
					mi.PerformClick();
				}
			}
			_showMenu.Enabled = true;
/*			foreach (MenuItem mi in _showMenu.MenuItems)	// NOTE: Don't do this. Go figure.
			{												// All the View-Panels will load ...
				mi.PerformClick();							// regardless of their saved settings.

				var settingName = GetWindowSettingName(mi);
				if (!(_settings[settingName].ValueBool))
					mi.PerformClick();
			}
			_showMenu.Enabled = true; */
		}

		public IMainWindowsShowAllManager CreateShowAll()
		{
			return new MainWindowsShowAllManager(_allForms, _allMenuItems);
		}

		private void RegisterWindowMenuItemValue()
		{
			foreach (MenuItem mi in _showMenu.MenuItems)
			{
				var settingName = GetWindowSettingName(mi);

				var defaultVal = true;
				if (   (mi.Tag is TopViewForm)
					|| (mi.Tag is RmpViewForm))
				{
					defaultVal = false;
				}

				_settings.AddSetting(
								settingName,
								defaultVal,
								"Default display window - " + mi.Text,
								"Windows",
								null,
								false,
								null);

				var form = mi.Tag as Form;
				if (form != null)
				{
					form.VisibleChanged += (sender, a) =>
					{
						if (_isDisposed)
							return;

						var senderForm = sender as Form;
						if (senderForm == null)
							return;

						_settings[settingName].Value = senderForm.Visible;
					};
				}
			}
		}

		private void RegisterForm(
								Form form,
								string title,
								MenuItem parent,
								string registryKey = null)
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

		private static string GetWindowSettingName(MenuItem mi)
		{
			return ("Window-" + mi.Text);
		}

		public void Dispose()
		{
			_isDisposed = true;
		}
	}
}
