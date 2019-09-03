using src.misc;
using TMPro;
using UnityEngine.UI;

namespace src.time.timeline {
    public class TimedEffectorEventPopupUIController : UnitySingleton<TimedEffectorEventPopupUIController> {

        public TMP_Text effectorEventName;
        public TMP_Text effectorEventTime;

        public Button removeButton;
        
        public void showTimedEffectorEvent(TimedEffectorEvent effectorEvent) {
            effectorEventTime.text = effectorEvent.ExecutionTime.ToString();
            removeButton.onClick.RemoveAllListeners();
            removeButton.onClick.AddListener(() => {
                Timeline.Instance.removeEffectorEvent(effectorEvent);
            });
        }
    }
}