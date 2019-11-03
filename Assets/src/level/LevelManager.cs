using System;
using System.Xml;
using Editor;
using src.level.parsing;
using src.misc;
using src.tutorial;
using UnityEngine;

namespace src.level {
    public class LevelManager : UnitySingleton<LevelManager> {
        [field: SerializeField, LabelOverride("LevelRoot")]
        public Transform LevelRoot { get; private set; }
        
        [field: SerializeField, LabelOverride("TutorialRoot")]
        public RectTransform TutorialRoot { get; private set; }
        
        public LevelContainer CurrentLevel { get; private set; }
        public TutorialContainer CurrentTutorial { get; private set; }

        [SerializeField] private bool overrideHashCheck;

        private void Start() {
            if (GlobalGameState.Instance.IsInGame) {
                loadXmlFromPayload();
            }
        }

        public void loadXmlFromPayload() {
            // Creates a XmlDocument from the 
            XmlDocument xmlDocument = new XmlDocument();
            xmlDocument.LoadXml(LevelXmlPayload.Instance.levelXml);
            var levelFile = xmlDocument.SelectSingleNode("LevelFile");
            var hashMatching = SecurityChecker.validateXmlLevel(levelFile);
            if (!hashMatching) {
                Debug.LogError("Level hashes are not matching!");
            }
            if (!(hashMatching || overrideHashCheck)) return;

            var versionString = ParseHelper.getAttributeValueByName(levelFile, "version");
            if (!int.TryParse(versionString, out var version)) {
                throw new Exception("Could not parse a valid Versionnumber from: " + versionString);
            }

            var levelXml = levelFile.SelectSingleNode("Level");
            CurrentLevel = ParserFactory.getLevelParserByVersion(version).parseLevelFromXmlString(levelXml);
            CurrentTutorial = ParserFactory.getTutorialParserByVersion(version).parseTutorialFromXmlString(levelXml);
            CurrentLevel.initializeLevel();
            CurrentTutorial.initializeTutorial();
        }

        public void clearAllLevelChildren() {
            for (int i = 0; i < LevelRoot.childCount; i++) {
                Destroy(LevelRoot.GetChild(i).gameObject);
            }
            
            for (int i = 0; i < TutorialRoot.childCount; i++) {
                Destroy(TutorialRoot.GetChild(i).gameObject);
            }
        }
    }
}