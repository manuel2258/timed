using src.misc;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace src.time.timeline {
    
    /// <summary>
    /// A Singleton representing the meta data of a timedEvent
    /// </summary>
    public class TimedEffectorEventPopupUIController : UnitySingleton<TimedEffectorEventPopupUIController> {

        public Image effectorEventIcon;
        public TMP_Text effectorEventTime;

        public Button removeButton;
        private Canvas _canvas;

        private void Start() {
            _canvas = GetComponent<Canvas>();
            _canvas.enabled = false;
            TimedEffectorEventEditManager.Instance.onEffectorEventTimeChanged += newTime => {
                effectorEventTime.text = $"{newTime:N2}s";
            };
        }

        /// <summary>
        /// Displays the meta data of a timedEvent
        /// </summary>
        /// <param name="effectorEvent"></param>
        public void showTimedEffectorEvent(TimedEffectorEvent effectorEvent) {
            _canvas.enabled = true;
            effectorEventTime.text = $"{effectorEvent.ExecutionTime:N2}";
            effectorEventIcon.sprite = effectorEvent.getIcon();
            removeButton.onClick.RemoveAllListeners();
            removeButton.onClick.AddListener(() => {
                TimedEffectorEventEditManager.Instance.removeTimedEffectorEvent(effectorEvent);
                _canvas.enabled = false;
            });
        }

        public void closePopup() {
            _canvas.enabled = false;
        }
    }
}