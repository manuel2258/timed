using System;
using src.setting.container;
using src.setting.parsing;
using src.translation;

namespace src.setting.settings {
    public class GeneralSettings : BaseSettingsGroup {
        public Setting<Language> Language { get; }

        public GeneralSettings(SettingsContainer settingsContainer) {
            Language = new Setting<Language>(settingsContainer.getSetting(SettingsType.General, "Language"), Enum.TryParse);
            settings.Add("Language", Language);
        }

        public override string getName() {
            return "General";
        }
    }
}