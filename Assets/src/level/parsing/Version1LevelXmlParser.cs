using System;
using System.Collections.Generic;
using System.Xml;
using src.element;
using src.element.effector;
using src.element.triggers;
using src.level.initializing;
using UnityEngine;

namespace src.level.parsing {
    public class Version1LevelXmlParser : ILevelParser {
        
        /// <summary>
        /// Parses the provided strings into a LevelContainer
        /// </summary>
        /// <param name="levelNode">The to parse form LevelNode</param>
        /// <returns>The ready to initialize LevelContainer</returns>
        /// <exception cref="Exception">If something could not be parsed properly</exception>
        public LevelContainer parseLevelFromXmlString(XmlNode levelNode) {
            
            // Parses the levels meta data and creates a level container
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
            
            var elements = levelNode.SelectSingleNode("Elements");
            if (elements != null) {
                // Then goes through each element
                foreach (XmlNode element in elements.ChildNodes) {
                    // Parses position and rotation
                    var position = ParseHelper.parsePositionFromNode(element);
                    var angle = ParseHelper.parseRotationFromNode(element);
                    
                    // Its ElementType
                    if(!Enum.TryParse(element.Name, out ElementType elementType)) {
                        throw new Exception($"Could not parse the name ElementType {element.Name}");
                    }

                    var idString = ParseHelper.getAttributeValueByName(element, "id");
                    if (!int.TryParse(idString, out var id)) {
                        throw new Exception($"Could not parse the id {idString}");
                    }
                    
                    // And if its Wall, get its scale and create a WallInitializer
                    if (elementType == ElementType.Wall) {
                        var scale = ParseHelper.parseScaleFromNode(element);
                        level.addInitializer(new WallInitializer(scale, elementType, id, position, angle));
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
                        level.addInitializer(new ColliderBodyInitializer(parameters, elementType, id, position, angle));
                        continue;
                    }


                    if (elementType == ElementType.Effector) {
                        // And its EffectorType
                        string effectorTypeString = ParseHelper.getAttributeValueByName(element, "type");
                        if (!Enum.TryParse(effectorTypeString, out EffectorType effectorType)) {
                            throw new Exception($"Could parse {effectorTypeString} to a EffectorType");
                        }

                        // Finally add the EffectorInitializer
                        level.addInitializer(new EffectorInitializer(effectorType, parameters, elementType, id, position,
                            angle));
                    }

                    if (elementType == ElementType.Trigger) {
                        // And its TriggerType
                        string triggerTypeString = ParseHelper.getAttributeValueByName(element, "type");
                        if (!Enum.TryParse(triggerTypeString, out TriggerType triggerType)) {
                            throw new Exception($"Could parse {triggerTypeString} to a EffectorType");
                        }

                        // Finally add the EffectorInitializer
                        level.addInitializer(new TriggerInitializer(triggerType, parameters, elementType, id, position,
                            angle));
                    }
                }
            }

            return level;
        }
    }
}