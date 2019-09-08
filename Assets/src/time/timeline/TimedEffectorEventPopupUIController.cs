using src.misc;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace src.time.timeline {
    
    /// <summary>
    /// A Singleton representing the meta data of a timedEvent
    /// </summary>
    public class TimedEffectorEventPopupUIController : UnitySingleton<TimedEffectorEventPopupUIController> {

        public TMP_Text effectorEventName;
        public TMP_Text effectorEventTime;

        public Button removeButton;
        private Canvas _canvas;

        private void Start() {
            _canvas = GetComponent<Canvas>();
            _canvas.enabled = false;
            TimedEffectorEventEditManager.Instance.onEffectorEventTimeChanged += newTime => {
                effectorEventTime.text = $"{newTime:N2}";
            };
        }

        /// <summary>
        /// Displays the meta data of a timedEvent
        /// </summary>
        /// <param name="effectorEvent"></param>
        public void showTimedEffectorEvent(TimedEffectorEvent effectorEvent) {
            _canvas.enabled = true;
            effectorEventTime.text = $"{effectorEvent.ExecutionTime:N2}";
            effectorEventName.text = effectorEvent.getName();
            removeButton.onClick.RemoveAllListeners();
            removeButton.onClick.AddListener(() => {
                Timeline.Instance.removeEffectorEvent(effectorEvent);
                _canvas.enabled = false;
            });
        }

        public void closePopup() {
            _canvas.enabled = false;
        }
    }
}