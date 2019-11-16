using System;
using System.Collections.Generic;
using System.IO;
using System.Xml;
using Editor;
using src.level.parsing;
using UnityEngine;

namespace src.level.selection {

    /// <summary>
    /// A container for levelPacks that store each level
    /// </summary>
    /// <remarks>
    /// To access the levels use the index operator
    /// </remarks>
    public class LevelPack : ScriptableObject {

        [field: SerializeField, LabelOverride("PackName")]
        public string PackName { get; private set; }

        [field: SerializeField, LabelOverride("Difficulty")]
        public int Difficulty { get; private set; }

        [SerializeField] private List<TextAsset> levels;

        private List<SelectableLevel> _selectableLevels;

        public int LevelCount => levels.Count;

        public SelectableLevel this[int index] => _selectableLevels[index];

        public void loadLevelsFromAssets() {
            _selectableLevels = new List<SelectableLevel>();
            for (var i = 0; i < levels.Count; i++) {
                var textAsset = levels[i];
                var parser = ParserFactory.getLevelParserByVersion(1);

                var xmlDocument = new XmlDocument();
                xmlDocument.LoadXml(textAsset.text);
                var levelNode = xmlDocument.SelectSingleNode("LevelFile/Level");
                var levelHead = parser.parseLevelHeadFromXmlString(levelNode);

                var levelProgressDocument = new XmlDocument();
                var savePath = Application.persistentDataPath + $"/level_saves/{levelHead.GUID}.xml";
                if (!Directory.Exists(Application.persistentDataPath + "/level_saves/")) {
                    Debug.Log($"Can't find directory at {Application.persistentDataPath + "/level_saves/"}");
                    Directory.CreateDirectory(Application.persistentDataPath + "/level_saves/");
                }

                if (!File.Exists(savePath)) {
                    var blueprint = Resources.Load<TextAsset>("level_progress");
                    levelProgressDocument.LoadXml(blueprint.text);
                    //File.Create(savePath);
                    levelProgressDocument.Save(savePath);
                    Debug.Log($"Creating new ProgessFile at: {savePath}");
                }

                levelProgressDocument.Load(savePath);
                var levelProgress = levelProgressDocument.SelectSingleNode("LevelProgress");

                var finishedString = ParseHelper.getAttributeValueByName(levelProgress, "finished");
                if (!bool.TryParse(finishedString, out var finished)) {
                    throw new Exception("Could not parse finishedString argument: " + finishedString);
                }

                var scoreString = ParseHelper.getAttributeValueByName(levelProgress, "score");
                if (!int.TryParse(scoreString, out var score)) {
                    throw new Exception("Could not parse scoreString argument: " + scoreString);
                }

                _selectableLevels.Add(new SelectableLevel(this, i, textAsset.text, levelHead, finished, score));
            }
        }
    }
}
