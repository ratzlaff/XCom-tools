using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using DSShared.Windows;

namespace MapView.SettingServices
{
	public class VolutarSettingService
	{
		private readonly Settings _settings;
		private const string VOLUTAR_MCD_EDITOR_PATH = "VolutarMcdEditorPath";

		public VolutarSettingService(Settings settings)
		{
			_settings = settings;
		}

		public string GetEditorFilePath()
		{
			var pathSetting = _settings.GetSetting(VOLUTAR_MCD_EDITOR_PATH, "");
			var path = pathSetting.Value as string ;

			// Ask for the file
			if (string.IsNullOrEmpty(path) || !File.Exists(path))
			{
				using (var input = new InputBox("Enter the Volutar MCD Editor Path"))
				{
					input.ShowDialog();
					path = input.InputValue;
					if (!string.IsNullOrEmpty(path) && File.Exists(path))
					{
						pathSetting.Value = path;
						return path;
					}
					return null;
				}
			}

			return path;
		}

		public static void LoadDefaultSettings(Settings settings)
		{
			settings.AddSetting(
							VOLUTAR_MCD_EDITOR_PATH,
							"",
							"Path to volutar MCD Editor",
							"TileView",
							null,
							false,
							null);
		}
	}
}
