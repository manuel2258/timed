using System;
using src.setting.container;
using src.setting.parsing;
using src.translation;

namespace src.setting.settings {
    public class GeneralSettings {
        public Setting<Language> Language { get; }

        public GeneralSettings(SettingsContainer settingsContainer) {
            Language = new Setting<Language>(settingsContainer.getSetting(SettingsType.General, "Language"), Enum.TryParse);
        }
    }
}