using System.Collections.Generic;
using System.IO;
using MapView.SettingServices;

namespace MapView.Forms.MainWindow
{
    public class SettingsManager
    {
        private readonly Dictionary<string, Settings> _settingsHash;
        private readonly SettingsService _settingsService;

        public SettingsManager()
        {
            _settingsHash = new Dictionary<string, Settings>();
            _settingsService = new SettingsService();
        }

        public void Add(string registryKey, Settings settings)
        {
            _settingsHash.Add(registryKey, settings);
        }

        public void Save()
        {
            _settingsService.Save(_settingsHash);
        }

        public void Load(string file)
        {
            using (var reader = new StreamReader(file))
            {
                ReadMapViewSettings(reader);
            }
        }

        private void ReadMapViewSettings(StreamReader sr)
        {
            var vc = new XCom.VarCollection(sr);
            var kv = vc.ReadLine();

            while (kv != null)
            {
                Settings.ReadSettings(vc, kv, _settingsHash[kv.Keyword]);
                kv = vc.ReadLine();
            }

            sr.Close();
        }

        public Settings this[string key]
        {
            get { return _settingsHash[key]; }
            set { _settingsHash[key] = value; }
        }
    }
}
