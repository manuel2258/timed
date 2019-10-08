using System;
using System.Collections.Generic;
using System.Xml;
using src.level.parsing;

namespace src.translation {
    public static class TranslationXmlParser {

        /// <summary>
        /// Parses WordContainers from a XmlString
        /// </summary>
        /// <param name="xml">The to parse from XmlString</param>
        /// <returns>A list of WordContainers</returns>
        /// <exception cref="Exception">If the languageString of a Translation could not be parsed</exception>
        public static List<WordContainer> parseWordsFromXmlString(string xml) {
            var wordContainers = new List<WordContainer>();
            
            var document = new XmlDocument();
            document.LoadXml(xml);
            var words = document.SelectNodes("/TranslationPack/Translations");

            foreach (XmlNode word in words) {
                var tag = ParseHelper.getAttributeValueByName(word, "tag");
                var currentContainer = new WordContainer(tag);
                var translations = word.SelectNodes("Translation");
                foreach (XmlNode translation in translations) {
                    var languageString = ParseHelper.getAttributeValueByName(translation, "language");
                    if (!Enum.TryParse(languageString, out Language language)) {
                        throw new Exception($"Could not parse {languageString} of {translation} to a Language");
                    }
                    currentContainer.addWord(language, translation.InnerText);
                }
                wordContainers.Add(currentContainer);
            }

            return wordContainers;
        }
    }
}