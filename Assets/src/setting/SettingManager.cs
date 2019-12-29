using System.Collections.Generic;
using System.IO;
using System.Xml;
using src.misc;
using src.setting.parsing;
using src.setting.settings;
using UnityEngine;

namespace src.setting {
    public class SettingManager : UnitySingleton<SettingManager> {
        
        public GeneralSettings GeneralSettings { get; private set; }
        
        private readonly List<BaseSettingsGroup> _settingGroups = new List<BaseSettingsGroup>();

        public void Awake() {
            var settingsContainer = new SettingsXMLParser().parseSettingsFromXmlString(getSettingsXml());
            GeneralSettings = new GeneralSettings(settingsContainer);
            
            _settingGroups.Add(GeneralSettings);
            
            foreach (var baseSettingsGroup in _settingGroups) {
                foreach (var keyValuePair in baseSettingsGroup.getSettings()) {
                    keyValuePair.Value.addOnUpdatedCallback(saveSettingsToXml);
                }
            }
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

        private void saveSettingsToXml() {
            XmlDocument xmlDocument = new XmlDocument();
            XmlElement root = xmlDocument.CreateElement("Settings");
            xmlDocument.AppendChild(root);
            foreach (var settingGroup in _settingGroups) {
                foreach (var setting in settingGroup.getSettings()) {
                    XmlElement settingElement = xmlDocument.CreateElement("Setting");
                    var typeAttribute = xmlDocument.CreateAttribute("type");
                    typeAttribute.Value = settingGroup.getName();
                    settingElement.Attributes.Append(typeAttribute);
                    var nameAttribute = xmlDocument.CreateAttribute("name");
                    nameAttribute.Value = setting.Key;
                    settingElement.Attributes.Append(nameAttribute);
                    settingElement.InnerText = setting.Value.getValuesAsString();
                    root.AppendChild(settingElement);
                }
            }
            xmlDocument.Save(Application.persistentDataPath + "/settings.xml");
        }
        
    }
}