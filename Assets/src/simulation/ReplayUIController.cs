using src.misc;
using UnityEngine;
using UnityEngine.UI;

namespace src.simulation {
    public class ReplayUIController : UnitySingleton<ReplayUIController> {

        public Sprite activeReplay;
        public Sprite inActiveReplay;

        public Image activeChangeImage;
        private Button _activeChangeButton;
        
        private void Start() {
            _activeChangeButton = activeChangeImage.GetComponent<Button>();
            ReplayManager.Instance.onActiveStatusChanged += newState => {
                activeChangeImage.sprite = !newState ? activeReplay : inActiveReplay;
            };
        }

        public void setActiveChangeButtonState(bool newState) {
            _activeChangeButton.interactable = newState;
        }
    }
}