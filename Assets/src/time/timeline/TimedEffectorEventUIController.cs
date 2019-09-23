using UnityEngine;
using UnityEngine.UI;

namespace src.time.timeline {
    public class TimedEffectorEventUIController : MonoBehaviour {

        public Button openEffectorEventPopup;
        public Image highlight;
        
        public void setup(TimedEffectorEvent effectorEvent) {
            openEffectorEventPopup.image.sprite = effectorEvent.getIcon();
            openEffectorEventPopup.onClick.AddListener(() => {
                TimedEffectorEventEditManager.Instance.onTimedEffectorEventButtonPressed(effectorEvent);
            });
        }

        public void setHighlight(bool isActive) {
            highlight.enabled = isActive;
        }
    }
}