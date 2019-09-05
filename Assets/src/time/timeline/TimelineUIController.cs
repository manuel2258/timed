using src.misc;
using src.simulation;
using src.time.time_managers;
using UnityEngine;

namespace src.time.timeline {
    
    /// <summary>
    /// A Singleton UI Controller that visualize the CurrentTime and its advancement
    /// </summary>
    public class TimelineUIController : UnitySingleton<TimelineUIController> {

        private RectTransform _rectTransform;

        public RectTransform pointer;

        private void Start() {
            ReplayTimeManager.Instance.onNewTime += onNewTime;
            _rectTransform = GetComponent<RectTransform>();
        }

        private void onNewTime(decimal newTime, decimal _) {
            var newPositionX = MathHelper.mapValue((float)newTime, 0, (float)SimulationManager.SIMULATION_LENGTH, -_rectTransform.sizeDelta.x / 2,
                _rectTransform.sizeDelta.x / 2);
            pointer.transform.localPosition = new Vector3(newPositionX, pointer.localPosition.y);
        }
    }
}