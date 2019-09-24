using src.level.parsing;
using src.level.selection;
using src.misc;
using src.simulation.reseting;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace src.level {
    public class LevelFinishUIController : UnitySingleton<LevelFinishUIController>, IResetable, IStackAbleWindow {

        public Canvas endLevelScreen;
        public Button endLevelButton;

        public TMP_Text levelName;
        public Canvas finishedCanvas;

        public Button nextLevelButton;

        public Sprite notFinished;
        public Sprite finished;

        private void Start() {
            LevelFinishManager.Instance.onLevelFinished += () => {
                endLevelButton.image.sprite = finished;
                finishedCanvas.enabled = true;
            };
            endLevelButton.onClick.AddListener(() => UIWindowStack.Instance.toggleWindow(typeof(LevelFinishUIController)));
            levelName.text = LevelXmlParser.Instance.CurrentLevel.Name;

            endLevelScreen.enabled = false;

            if (LevelSelectionManager.Instance != null) {
                if (LevelSelectionManager.Instance.hasNextLevel()) {
                    nextLevelButton.gameObject.SetActive(true);
                    nextLevelButton.onClick.AddListener(() => LevelSelectionManager.Instance.loadNextLevel());
                }
            }
        }

        public void setActive(bool newState) {
            endLevelScreen.enabled = newState;
        }

        public bool isActive() {
            return endLevelScreen.enabled;
        }

        public void reset() {
            endLevelButton.image.sprite = notFinished;
            endLevelScreen.enabled = false;
            finishedCanvas.enabled = false;
        }
    }
}