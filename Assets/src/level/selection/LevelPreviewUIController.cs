using System.Linq;
using src.level.finish;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace src.level.selection {
    public class LevelPreviewUIController : MonoBehaviour {

        public TMP_Text name;
        public DifficultyUIController difficulty;
        public DifficultyUIController score;
        public GameObject notFinished;

        public Button startButton;
        
        public void setup(SelectableLevel level) {
            name.text = level.LevelHeader.Name;
            notFinished.SetActive(level.Score == 0);
            score.gameObject.SetActive(level.Score != 0);
            var scores = level.LevelHeader.Scores;
            score.displayDifficulty(1 + scores.Count(levelHeaderScore => level.Score > levelHeaderScore));
            
            difficulty.displayDifficulty(level.LevelHeader.Difficulty);

            startButton.onClick.AddListener(() => LevelSelectionManager.Instance.loadFromSelectable(level));
        }
    }
}