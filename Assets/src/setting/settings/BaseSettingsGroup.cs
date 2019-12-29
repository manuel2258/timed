using System.Collections.Generic;

namespace src.setting.settings {
    public abstract class BaseSettingsGroup {
        
        protected readonly Dictionary<string, ISetting> settings = new Dictionary<string, ISetting>();

        public Dictionary<string, ISetting> getSettings() {
            return new Dictionary<string, ISetting>(settings);
        }

        public abstract string getName();
    }
}