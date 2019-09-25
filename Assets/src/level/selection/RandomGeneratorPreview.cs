using src.level.generator.levels;
using src.level.parsing;
using UnityEngine;

namespace src.level.selection {
    public class RandomGeneratorPreview : MonoBehaviour {

        private string _generatedLevel;

        private void Start() {
            
            generateAndPreviewLevel();
        }

        public void generateAndPreviewLevel() {
            LevelXmlParser.Instance.LevelRoot.transform.localScale = Vector3.one;
            _generatedLevel = new LevelGenerator().CreateLevel(Random.Range(int.MinValue, int.MaxValue), 10);
            LevelXmlParser.Instance.parseLevelFromXmlString(_generatedLevel).initializeLevel();
            LevelXmlParser.Instance.LevelRoot.transform.localScale /= 5;
        }

        public void loadLevel() {
            if (LevelXmlPayload.Instance != null) {
                Destroy(LevelXmlPayload.Instance.gameObject);
            }

            LevelSelectionManager.Instance.loadFromString(_generatedLevel);
        }
    }
}