using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using XCom;

namespace MapView.SettingServices
{
	public class SettingsService
	{
		public const string FILE_NAME = "MV_SettingsFile";
		 
		public void Save(Dictionary<string, Settings> settingsHash)
		{
			using (var sw = new StreamWriter(SharedSpace.Instance[FILE_NAME].ToString()))
			{
				foreach (string s in settingsHash.Keys)
					if (settingsHash.ContainsKey(s))
						settingsHash[s].Save(s, sw);

				sw.Flush();
				sw.Close();
			}
		}
	}
}
