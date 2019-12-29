using src.translation;
using UnityEngine;

namespace src.setting.ui {
    public abstract class BaseUIController<TSettingsType> : ISettingsUIController {
        
        [SerializeField] protected bool menuOnly;
        [SerializeField] protected TranslateAbleTMPText nameText;
        
        [SerializeField] private string settingTranslateTag;
        [SerializeField] private string settingName;

        private string groupName;

        protected Setting<TSettingsType> setting;

        public override void setup(string groupName) {
            this.groupName = groupName;
            nameText.translationTag = settingTranslateTag;
            nameText.translateText();
            setting = getSettingFromManager();
        }

        protected Setting<TSettingsType> getSettingFromManager() {
            var groupField = SettingManager.Instance.GetType().GetProperty(groupName);
            var groupInstance = groupField.GetValue(SettingManager.Instance);
            return (Setting<TSettingsType>)groupInstance.GetType().GetProperty(settingName).GetValue(groupInstance);
        }
        
    }
}
