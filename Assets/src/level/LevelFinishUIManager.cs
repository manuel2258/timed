using src.simulation.reseting;
using UnityEngine;
using UnityEngine.UI;

namespace src.level {
    public class LevelFinishUIManager : MonoBehaviour, IResetable {

        public Canvas endLevelScreen;
        public Button endLevelButton;

        private void Start() {
            LevelFinishManager.Instance.onLevelFinished += () => endLevelButton.gameObject.SetActive(true);
            endLevelButton.onClick.AddListener(() => endLevelScreen.enabled = !endLevelScreen.enabled);

            endLevelScreen.enabled = false;
        }

        public void reset() {
            endLevelButton.gameObject.SetActive(false);
            endLevelScreen.enabled = false;
        }
    }
}