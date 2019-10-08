using System;
using System.Collections.Generic;
using System.Xml;
using src.level.parsing;
using src.tutorial.check_events;


namespace src.tutorial {
    public static class TutorialXmlParser {
        public static TutorialContainer parseTutorialFromXmlString(string xmlString) {

            // Creates a XmlDocument from the 
            XmlDocument xmlDocument = new XmlDocument();
            xmlDocument.LoadXml(xmlString);
            var parts = xmlDocument.SelectSingleNode("/Level/TutorialSequence");

            var tutorialContainer = new TutorialContainer();

            if (parts != null) {
                // Then goes through each element
                foreach (XmlNode part in parts.ChildNodes) {
                    var partIdString = ParseHelper.getAttributeValueByName(part, "id");
                    if (!int.TryParse(partIdString, out var partId)) {
                        throw new Exception($"Could not parse part ID: {partIdString}");
                    }

                    var partContainer = new PartContainer(partId);
                    tutorialContainer.addPart(partContainer);

                    var checkEvents = part.SelectNodes("CheckEvent");
                    var helpDisplays = part.SelectNodes("HelpDisplay");

                    if (checkEvents != null) {
                        foreach (XmlNode checkEvent in checkEvents) {
                            var typeString = ParseHelper.getAttributeValueByName(checkEvent, "type");
                            if (!Enum.TryParse(typeString, out CheckEventType checkEventType)) {
                                throw new Exception($"Could not parse EventType: {typeString}");
                            }
                            
                            var idString = ParseHelper.getAttributeValueByName(checkEvent, "id");
                            if (!int.TryParse(idString, out var id)) {
                                throw new Exception($"Could not parse id: {idString}");
                            }

                            if (checkEventType == CheckEventType.ElementEvent || checkEventType == CheckEventType.PartElementEvent) {
                                var elementIdString = ParseHelper.getAttributeValueByName(checkEvent, "elementId");
                                if (!int.TryParse(elementIdString, out var elementId)) {
                                    throw new Exception($"Could not parse elementId: {elementIdString}");
                                }
                                if (checkEventType == CheckEventType.ElementEvent) {
                                    partContainer.addElement(new ElementCheckEvent(checkEvent.InnerText, elementId),
                                        id);
                                } else {
                                    partIdString = ParseHelper.getAttributeValueByName(checkEvent, "partId");
                                    if (!int.TryParse(partIdString, out partId)) {
                                        throw new Exception($"Could not parse partId: {partIdString}");
                                    }
                                    partContainer.addElement(new PartElementCheckEvent(checkEvent.InnerText,
                                        partId, elementId), id);
                                }
                            } else if (checkEventType == CheckEventType.GameObjectEvent) {
                                var gameObjectName = ParseHelper.getAttributeValueByName(checkEvent, "name");
                                partContainer.addElement(new GameObjectCheckEvent(checkEvent.InnerText,
                                    gameObjectName), id);
                            } 
                        }
                    }

                    if (helpDisplays != null) {
                        foreach (XmlNode helpDisplay in helpDisplays) {
                            var typeString = ParseHelper.getAttributeValueByName(helpDisplay, "type");
                            if (!Enum.TryParse(typeString, out HelpDisplayType helpDisplayType)) {
                                throw new Exception($"Could not parse EventType: {typeString}");
                            }
                            
                            var idString = ParseHelper.getAttributeValueByName(helpDisplay, "id");
                            if (!int.TryParse(idString, out var id)) {
                                throw new Exception($"Could not parse id: {idString}");
                            }

                            var parameters = new Dictionary<string, string>();
                            var parameterNodes = helpDisplay.SelectNodes("Parameter");
                            foreach (XmlNode parameterNode in parameterNodes) {
                                var attributeName = parameterNode.Attributes[0].Value;
                                parameters.Add(attributeName, parameterNode.InnerText);
                            }
                            
                            partContainer.addElement(new HelpDisplayInitializer(helpDisplayType, parameters), id);
                        }
                    }
                }
            }

            return tutorialContainer;
        }
    }
}