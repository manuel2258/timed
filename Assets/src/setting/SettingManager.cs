using System.IO;
using System.Xml;
using src.misc;
using src.setting.parsing;
using src.setting.settings;
using UnityEngine;

namespace src.setting {
    public class SettingManager : UnitySingleton<SettingManager> {
        public GeneralSettings GeneralSettings { get; private set; }

        public void Start() {
            if (Instance != this) {
                Destroy(gameObject);
            } else {
                DontDestroyOnLoad(gameObject);
            }
            var settingsContainer = new SettingsXMLParser().parseSettingsFromXmlString(getSettingsXml());
            GeneralSettings = new GeneralSettings(settingsContainer);
        }

        private XmlDocument getSettingsXml() {
            var path = Application.persistentDataPath + "/settings.xml";

            XmlDocument xmlDocument = new XmlDocument();
            if (!File.Exists(path)) {
                var defaultSettings = Resources.Load<TextAsset>("settings/settings");
                xmlDocument.LoadXml(defaultSettings.text);
                xmlDocument.Save(path);
                Debug.Log("Did not find settingsXml, creating new at: " + path);
            } else {
                xmlDocument.Load(path);
            }

            return xmlDocument;
        }
        
    }
}