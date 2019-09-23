using src.level.parsing;
using UnityEngine;

namespace src.level.selection {
    public class RandomGeneratorPreview : MonoBehaviour {

        private string _generatedLevel;

        private void Start() {
            generateAndPreviewLevel();
        }

        public void generateAndPreviewLevel() {
            LevelXmlPayloadFactory.generateFromFile("levels/basic_levels/sample_level");
            _generatedLevel = LevelXmlPayload.Instance.levelXml;
            LevelXmlParser.Instance.parseLevelFromXmlString(_generatedLevel).initializeLevel();
            LevelXmlParser.Instance.LevelRoot.transform.localScale /= 3;
        }

        public void loadLevel() {
            Destroy(LevelXmlPayload.Instance.gameObject);
            LevelSelectionManager.Instance.loadFromString(_generatedLevel);
        }
    }
}