using src.misc;
using src.simulation;
using src.time.time_managers;
using src.touch;
using UnityEngine;
using UnityEngine.EventSystems;

namespace src.time.timeline {
    
    /// <summary>
    /// A Singleton UI Controller that visualize the CurrentTime and its advancement
    /// </summary>
    public class TimelineUIController : TouchableRect<TimelineUIController>, IPointerDownHandler {

        private RectTransform _rectTransform;

        public RectTransform pointer;

        private bool _timePickerMode;
        private OnNewPickedTime _timePickerCallback;

        private bool _selectingPosition;
        private bool _replayWasActiveBefore;

        protected override void Start() {
            base.Start();
            ReplayTimeManager.Instance.onNewTime += onNewTime;
            _rectTransform = GetComponent<RectTransform>();
        }

        protected override void Update() {
            base.Update();
            if (hasPosition && RectPosition.x > -rectTransform.sizeDelta.x / 2 && RectPosition.x < rectTransform.sizeDelta.x / 2) {
                var pickedTime = (decimal) MathHelper.mapValue(RectPosition.x, -rectTransform.sizeDelta.x / 2,
                    rectTransform.sizeDelta.x / 2, 0,
                    (float) SimulationManager.SIMULATION_LENGTH);
                ReplayTimeManager.Instance.setCurrentTime(pickedTime);
                if (_timePickerMode) {
                    _timePickerCallback.Invoke(pickedTime);
                } else {
                    _selectingPosition = true;
                    _replayWasActiveBefore = ReplayTimeManager.Instance.Active;
                    ReplayTimeManager.Instance.Active = false;
                }
                onNewTime(pickedTime, 0);
            } else {
                if (_selectingPosition) {
                    _selectingPosition = false;
                    ReplayTimeManager.Instance.Active = _replayWasActiveBefore;
                }
            }
        }

        private void onNewTime(decimal newTime, decimal _) {
            var newPositionX = MathHelper.mapValue((float)newTime, 0, (float)SimulationManager.SIMULATION_LENGTH, -_rectTransform.sizeDelta.x / 2,
                _rectTransform.sizeDelta.x / 2);
            pointer.transform.localPosition = new Vector3(newPositionX, pointer.localPosition.y);
        }

        public void setTimePickerMode(OnNewPickedTime callback, decimal startTime) {
            _timePickerMode = true;
            _timePickerCallback = callback;
            onNewTime(startTime, 0);
        }

        public void exitTimePickerMode() {
            _timePickerMode = false;
        }
    }
    
    public delegate void OnNewPickedTime(decimal newTime);
}