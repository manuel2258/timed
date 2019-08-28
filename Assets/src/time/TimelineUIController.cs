using src.misc;
using UnityEngine;

namespace src.time {
    
    /// <summary>
    /// A Singleton UI Controller that visualize the CurrentTime and its advancement
    /// </summary>
    public class TimelineUIController : UnitySingleton<TimelineUIController> {

        private RectTransform _rectTransform;

        public RectTransform pointer;

        public float TotalTime { get; set; } = 30;

        private void Start() {
            TimeManager.Instance.onNewTime += onNewTime;
            _rectTransform = GetComponent<RectTransform>();
        }

        private void onNewTime(float newTime, float _) {
            var newPositionX = MathHelper.mapValue(newTime, 0, TotalTime, -_rectTransform.sizeDelta.x / 2,
                _rectTransform.sizeDelta.x / 2);
            pointer.transform.localPosition = new Vector3(newPositionX, pointer.localPosition.y);
        }
    }
}