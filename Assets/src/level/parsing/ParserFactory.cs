using System;
using System.Collections.Generic;
using System.Xml;
using src.tutorial;

namespace src.level.parsing {
    public static class ParserFactory {
        
        private static readonly Dictionary<int, Type> _levelParsers = new Dictionary<int, Type>();
        private static readonly Dictionary<int, Type> _tutorialParsers = new Dictionary<int, Type>();

        static ParserFactory() {
            _levelParsers.Add(1, typeof(Version1LevelXmlParser));
            _tutorialParsers.Add(1, typeof(Version1TutorialXmlParser));
        }
        
        public static ILevelParser getLevelParserByVersion(int version) {
             return (ILevelParser)_levelParsers[version].GetConstructor(Type.EmptyTypes).Invoke(new object[0]);
        }
        
        public static ITutorialParser getTutorialParserByVersion(int version) {
            return (ITutorialParser)_tutorialParsers[version].GetConstructor(Type.EmptyTypes).Invoke(new object[0]);
        }
    }

    public interface ILevelParser {
        
        /// <summary>
        /// Parses the provided xmlNode into a LevelContainer
        /// </summary>
        /// <param name="levelNode">The to parse form LevelNode</param>
        /// <returns>The ready to initialize LevelContainer</returns>
        /// <exception cref="Exception">If something could not be parsed properly</exception>
        LevelContainer parseLevelFromXmlString(XmlNode levelNode);
        
        LevelHeader parseLevelHeadFromXmlString(XmlNode levelNode);
    }

    public interface ITutorialParser {

        /// <summary>
        /// Parses the provided xmlNode into a TutorialContainer
        /// </summary>
        /// <param name="tutorialNode">The to parse from TutorialNode</param>
        /// <returns>The ready to initialize TutorialContainer</returns>
        /// <exception cref="Exception">If something could not be parsed properly</exception>
        TutorialContainer parseTutorialFromXmlString(XmlNode tutorialNode);
    }
}