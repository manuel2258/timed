using System;
using System.Collections.Generic;
using System.Xml;
using src.element;
using UnityEngine;

namespace src.level.parsing {
    public static class ParseHelper {
        
        /// <summary>
        /// Creates a List of enums contained in a string and separated by the char '|'
        /// </summary>
        /// <param name="enumListString">The to parse from string</param>
        /// <typeparam name="T">The to parse enum type</typeparam>
        /// <returns>A List of parsed enums</returns>
        /// <exception cref="Exception">When a word could not be parsed</exception>
        public static List<T> parseEnumListFromString<T>(string enumListString) where T : struct, Enum {
            List<T> parsedList = new List<T>();
            if (enumListString.Length > 0) {
                string currentWord = "";
                for (var i = 0; i < enumListString.Length; i++) {
                    var currentChar = enumListString[i];
                    if (currentChar != '|') {
                        currentWord += currentChar;
                    } else {
                        if (!Enum.TryParse(currentWord, out T parsedEnum)) {
                            throw new Exception($"Could not parse enum {currentWord} from {enumListString}");
                        }

                        parsedList.Add(parsedEnum);
                        currentWord = "";
                    }
                }

                if (!Enum.TryParse(currentWord, out T finalParsedEnum)) {
                    throw new Exception($"Could not parse enum {currentWord} from {enumListString}");
                }

                parsedList.Add(finalParsedEnum);
            }

            return parsedList;
        }

        /// <summary>
        /// Searches for a attribute in a XmlNode
        /// </summary>
        /// <param name="node">The to search through node</param>
        /// <param name="name">The to search name</param>
        /// <returns></returns>
        public static string getAttributeValueByName(XmlNode node, string name) {
            foreach (XmlAttribute attribute in node.Attributes) {
                if (attribute.Name == name) {
                    return attribute.Value;
                }
            }
            throw new Exception("Could not find Attribute " + name);
        }

        /// <summary>
        /// Parses a string to the enum type ElementColor
        /// </summary>
        /// <param name="color">The to parse String</param>
        /// <returns>The parsed ElementColor Enum</returns>
        /// <exception cref="Exception">If the string could not be parsed</exception>
        public static ElementColor getElementColorFromString(string color) {
            if(!Enum.TryParse(color, out ElementColor parsedColor)){
                throw new Exception($"Unable to parse {color} as a elementColor");
            }

            return parsedColor;
        }
        
        public static Vector2 parsePositionFromNode(XmlNode node) {
            var positionNode = node.SelectSingleNode("Position");
            float.TryParse(ParseHelper.getAttributeValueByName(positionNode, "x"), out var x);
            float.TryParse(ParseHelper.getAttributeValueByName(positionNode, "y"), out var y);
            return new Vector2(x, y);
        }
        
        public static Vector2 parseScaleFromNode(XmlNode node) {
            var positionNode = node.SelectSingleNode("Scale");
            float.TryParse(ParseHelper.getAttributeValueByName(positionNode, "x"), out var x);
            float.TryParse(ParseHelper.getAttributeValueByName(positionNode, "y"), out var y);
            return new Vector2(x, y);
        }
        
        public static float parseRotationFromNode(XmlNode node) {
            var rotationNode = node.SelectSingleNode("Rotation");
            float.TryParse(ParseHelper.getAttributeValueByName(rotationNode, "angle"), out var angle);
            return angle;
        }
    }
}