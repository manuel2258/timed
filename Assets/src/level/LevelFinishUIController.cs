using src.misc;
using src.simulation.reseting;
using UnityEngine;
using UnityEngine.UI;

namespace src.level {
    public class LevelFinishUIController : UnitySingleton<LevelFinishUIController>, IResetable, IStackAbleWindow {

        public Canvas endLevelScreen;
        public Button endLevelButton;

        private void Start() {
            LevelFinishManager.Instance.onLevelFinished += () => endLevelButton.gameObject.SetActive(true);
            endLevelButton.onClick.AddListener(() => UIWindowStack.Instance.toggleWindow(typeof(LevelFinishUIController)));

            endLevelScreen.enabled = false;
        }

        public void setActive(bool newState) {
            endLevelScreen.enabled = newState;
        }

        public bool isActive() {
            return endLevelScreen.enabled;
        }

        public void reset() {
            endLevelButton.gameObject.SetActive(false);
            endLevelScreen.enabled = false;
        }
    }
}