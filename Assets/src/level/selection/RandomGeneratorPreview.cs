using src.level.generator.levels;
using src.level.parsing;
using TMPro;
using UnityEngine;

namespace src.level.selection {
    public class RandomGeneratorPreview : MonoBehaviour {

        private string _generatedLevel;
        private int _difficulty;

        [SerializeField] private TMP_Text difficultyDisplay;

        private void Start() {
            generateAndPreviewLevel();
            changeDifficulty(0);
        }

        public void generateAndPreviewLevel() {
            LevelXmlParser.Instance.LevelRoot.transform.localScale = Vector3.one;
            _generatedLevel = new LevelGenerator().CreateLevel(Random.Range(int.MinValue, int.MaxValue), _difficulty);
            LevelXmlParser.Instance.parseLevelFromXmlString(_generatedLevel).initializeLevel();
            LevelXmlParser.Instance.LevelRoot.transform.localScale /= 5;
        }

        public void loadLevel() {
            if (LevelXmlPayload.Instance != null) {
                Destroy(LevelXmlPayload.Instance.gameObject);
            }

            LevelSelectionManager.Instance.loadFromString(_generatedLevel);
        }

        public void changeDifficulty(int diff) {
            _difficulty += diff;
            _difficulty = Mathf.Clamp(_difficulty, 1, 5);
            difficultyDisplay.text = _difficulty.ToString();
        }
    }
}