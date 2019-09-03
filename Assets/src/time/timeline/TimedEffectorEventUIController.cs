using UnityEngine;
using UnityEngine.UI;

namespace src.time.timeline {
    public class TimedEffectorEventUIController : MonoBehaviour {

        public Button openEffectorEventPopup;
        
        public void setup(TimedEffectorEvent effectorEvent) {
            openEffectorEventPopup.onClick.AddListener(() => {
                TimedEffectorEventPopupUIController.Instance.showTimedEffectorEvent(effectorEvent);
            });
        }
    }
}