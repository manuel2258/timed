using System;
using System.Xml;
using src.element.effector;
using src.level.parsing;
using src.setting.container;

namespace src.setting.parsing {
    public class SettingsXMLParser {

        public SettingsContainer parseSettingsFromXmlString(XmlNode levelNode) {
            var container = new SettingsContainer();
            var settingNodes = levelNode.SelectNodes("Settings/Setting");
            var argumentParser = new ArgumentParser("SettingsXMLParser");
            foreach (XmlNode settingNode in settingNodes) {
                var settingsType = argumentParser.TryParse<SettingsType>(
                    ParseHelper.getAttributeValueByName(settingNode, "type"), Enum.TryParse);
                var name = ParseHelper.getAttributeValueByName(settingNode, "name");
                var value = settingNode.InnerText;
                container.addSetting(settingsType, name, value);
            }

            return container;
        }
    }
}