using System;
using System.Collections.Generic;
using System.Xml;
using Editor;
using src.element;
using src.element.triggers;
using src.level.initializing;
using src.misc;
using UnityEngine;

namespace src.level.parsing {
    public class LevelXmlParser : UnitySingleton<LevelXmlParser> {

        [field: SerializeField, LabelOverride("LevelRoot")]
        public Transform LevelRoot { get; private set; }
        
        public LevelContainer CurrentLevel { get; private set; }

        private void Start() {
            if (GlobalGameState.Instance.IsInGame) {
                CurrentLevel = parseLevelFromXmlString(LevelXmlPayload.Instance.levelXml);
                CurrentLevel.initializeLevel();
            }
        }
        
        public void clearAllLevelChildren() {
            for (int i = 0; i < LevelRoot.childCount; i++) {
                Destroy(LevelRoot.GetChild(i).gameObject);
            }
        }

        /// <summary>
        /// Parses the provided strings into ElementInitializers
        /// </summary>
        /// <param name="xmlString">The to parse XmlString</param>
        /// <returns>The parsed ElementInitializers</returns>
        /// <exception cref="Exception">If something could not be parsed properly</exception>
        public LevelContainer parseLevelFromXmlString(string xmlString) {
            
            // Creates a XmlDocument from the 
            XmlDocument xmlDocument = new XmlDocument();
            xmlDocument.LoadXml(xmlString);
            var elements = xmlDocument.SelectSingleNode("/Level/Elements");
            
            // Parses the levels meta data and creates a level container
            var levelNode = xmlDocument.SelectSingleNode("Level");
            
            var levelName = ParseHelper.getAttributeValueByName(levelNode, "name");
            
            var gravityStringX = ParseHelper.getAttributeValueByName(levelNode, "gravity_x");
            if (!float.TryParse(gravityStringX, out var gravityX)) {
                throw new Exception("Could not parse gravityScaleX argument: " + gravityStringX);
            }
            
            var gravityStringY = ParseHelper.getAttributeValueByName(levelNode, "gravity_y");
            if (!float.TryParse(gravityStringY, out var gravityY)) {
                throw new Exception("Could not parse gravityScaleY argument: " + gravityStringY);
            }
            
            var difficultyString = ParseHelper.getAttributeValueByName(levelNode, "difficulty");
            if (!int.TryParse(difficultyString, out var difficulty)) {
                throw new Exception("Could not parse difficulty argument: " + difficulty);
            }
            
            var level = new LevelContainer(levelName, new Vector2(gravityX, gravityY), difficulty);

            if (elements != null) {
                // Then goes through each element
                foreach (XmlNode element in elements.ChildNodes) {
                    // Parses position and rotation
                    var position = parsePositionFromNode(element);
                    var angle = parseRotationFromNode(element);
                    
                    // Its ElementType
                    if(!Enum.TryParse(element.Name, out ElementType elementType)) {
                        throw new Exception($"Could not parse the name ElementType {element.Name}");
                    }
                    
                    // And if its Wall, get its scale and create a WallInitializer
                    if (elementType == ElementType.Wall) {
                        var scale = parseScaleFromNode(element);
                        level.addInitializer(new WallInitializer(scale, elementType, position, angle));
                        continue;
                    }

                    // If it is a Effector parses the Parameters
                    var parameters = new Dictionary<string, string>();
                    var parameterNodes = element.SelectNodes("Parameter");
                    foreach (XmlNode parameterNode in parameterNodes) {
                        var attributeName = parameterNode.Attributes[0].Value;
                        parameters.Add(attributeName, parameterNode.InnerText);
                    }
                    
                    // And if its ColliderBody, create a ColliderBodyInitializer with its parameter
                    if (elementType == ElementType.ColliderBody) {
                        level.addInitializer(new ColliderBodyInitializer(parameters, elementType, position, angle));
                        continue;
                    }


                    if (elementType == ElementType.Effector) {
                        // And its EffectorType
                        string effectorTypeString = ParseHelper.getAttributeValueByName(element, "type");
                        if (!Enum.TryParse(effectorTypeString, out EffectorType effectorType)) {
                            throw new Exception($"Could parse {effectorTypeString} to a EffectorType");
                        }

                        // Finally add the EffectorInitializer
                        level.addInitializer(new EffectorInitializer(effectorType, parameters, elementType, position,
                            angle));
                    }

                    if (elementType == ElementType.Trigger) {
                        // And its TriggerType
                        string triggerTypeString = ParseHelper.getAttributeValueByName(element, "type");
                        if (!Enum.TryParse(triggerTypeString, out TriggerType triggerType)) {
                            throw new Exception($"Could parse {triggerTypeString} to a EffectorType");
                        }

                        // Finally add the EffectorInitializer
                        level.addInitializer(new TriggerInitializer(triggerType, parameters, elementType, position,
                            angle));
                    }
                }
            }

            return level;
        }

        private Vector2 parsePositionFromNode(XmlNode node) {
            var positionNode = node.SelectSingleNode("Position");
            float.TryParse(ParseHelper.getAttributeValueByName(positionNode, "x"), out var x);
            float.TryParse(ParseHelper.getAttributeValueByName(positionNode, "y"), out var y);
            return new Vector2(x, y);
        }
        
        private Vector2 parseScaleFromNode(XmlNode node) {
            var positionNode = node.SelectSingleNode("Scale");
            float.TryParse(ParseHelper.getAttributeValueByName(positionNode, "x"), out var x);
            float.TryParse(ParseHelper.getAttributeValueByName(positionNode, "y"), out var y);
            return new Vector2(x, y);
        }
        
        private float parseRotationFromNode(XmlNode node) {
            var rotationNode = node.SelectSingleNode("Rotation");
            float.TryParse(ParseHelper.getAttributeValueByName(rotationNode, "angle"), out var angle);
            return angle;
        }
    }
}