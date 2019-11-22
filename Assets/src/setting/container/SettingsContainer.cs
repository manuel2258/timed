using System;
using System.Collections.Generic;
using src.setting.parsing;

namespace src.setting.container {
    public class SettingsContainer {
        private readonly Dictionary<SettingsType, Dictionary<string, string>> _settings = new Dictionary<SettingsType, Dictionary<string, string>>();

        public SettingsContainer() {
            _settings.Add(SettingsType.General, new Dictionary<string, string>());
        }

        public void addSetting(SettingsType settingsType, string name, string value) {
            _settings[settingsType].Add(name, value);
        }

        public string getSetting(SettingsType settingsType, string name) {
            if (!_settings[settingsType].TryGetValue(name, out var value)) {
                throw new Exception($"Could not find setting {name} in {settingsType}!");
            }

            return value;
        }
        
        
    }
}