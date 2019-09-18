using System;
using System.Collections.Generic;
using System.Xml;
using src.element;
using src.level.initializing;
using src.misc;
using UnityEngine;

namespace src.level.parsing {
    public class LevelXmlParser : UnitySingleton<LevelXmlParser> {

        public Transform effectorRoot;
        public Transform wallRoot;
        public Transform colliderBodyRoot;
        
        private readonly Dictionary<ElementType, Transform> _elementParentTransformMap =
            new Dictionary<ElementType, Transform>();

        private void Start() {
            _elementParentTransformMap.Add(ElementType.Effector, effectorRoot);
            _elementParentTransformMap.Add(ElementType.Wall, wallRoot);
            _elementParentTransformMap.Add(ElementType.ColliderBody, colliderBodyRoot);
            parseLevelFromXmlString(LevelXmlPayload.Instance.levelXml).initializeLevel();
        }

        /// <summary>
        /// Gets the parent transform for each ElementType
        /// </summary>
        /// <param name="elementType">The to get ElementTypes Transform</param>
        /// <returns>The parent Transform</returns>
        public Transform getParentByElementType(ElementType elementType) {
            _elementParentTransformMap.TryGetValue(elementType, out var rootTransform);
            return rootTransform;
        }

        /// <summary>
        /// Parses the provided strings into ElementInitializers
        /// </summary>
        /// <param name="xmlString">The to parse XmlString</param>
        /// <returns>The parsed ElementInitializers</returns>
        /// <exception cref="Exception">If something could not be parsed properly</exception>
        private LevelContainer parseLevelFromXmlString(string xmlString) {
            
            // Creates a XmlDocument from the 
            XmlDocument xmlDocument = new XmlDocument();
            xmlDocument.LoadXml(xmlString);
            var elements = xmlDocument.SelectSingleNode("/Level/Elements");
            
            // Parses the levels meta data and creates a level container
            var levelName = ParseHelper.getAttributeValueByName(xmlDocument.SelectSingleNode("Level"), "name");
            var level = new LevelContainer(levelName);

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

                    // And its EffectorType
                    string effectorTypeString = ParseHelper.getAttributeValueByName(element, "type");
                    if (!Enum.TryParse(effectorTypeString, out EffectorType effectorType)) {
                        throw new Exception($"Could parse {effectorTypeString} to a EffectorType");
                    }
                        
                    // Finally add the EffectorInitializer
                    level.addInitializer(new EffectorInitializer(effectorType, parameters, elementType, position, angle));
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