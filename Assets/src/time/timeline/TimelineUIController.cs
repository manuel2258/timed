using System;
using src.misc;
using src.simulation;
using src.time.time_managers;
using src.touch;
using src.tutorial.check_events;
using UnityEngine;
using UnityEngine.EventSystems;

namespace src.time.timeline {
    
    /// <summary>
    /// A Singleton UI Controller that visualize the CurrentTime and its advancement
    /// </summary>
    public class TimelineUIController : TouchableSingletonRect<TimelineUIController>, IPointerDownHandler, ICheckAbleEvent {
        
        public RectTransform pointer;
        public RectTransform fill;
        
        private RectTransform _rectTransform;

        private bool _selectingPosition;
        
        private readonly CheckEventManager _checkEventManager = new CheckEventManager();
        public void registerEvent(string eventName, Action onEventChecked) {
            _checkEventManager.registerEvent(eventName, onEventChecked);
        }

        protected override void Start() {
            base.Start();
            ReplayTimeManager.Instance.onNewTime += onNewTime;
            _rectTransform = GetComponent<RectTransform>();
        }

        protected override void Update() {
            base.Update();
            if (hasPosition && RectPosition.x > -rectTransform.sizeDelta.x / 2 &&
                RectPosition.x < rectTransform.sizeDelta.x / 2) {
                var pickedTime = (decimal) MathHelper.mapValue(RectPosition.x, -rectTransform.sizeDelta.x / 2,
                    rectTransform.sizeDelta.x / 2, 0,
                    (float) SimulationManager.SIMULATION_LENGTH);
                ReplayTimeManager.Instance.setCurrentTime(pickedTime);
                _selectingPosition = true;
                ReplayManager.Instance.Active = false;
                onNewTime(pickedTime, 0);
                _checkEventManager.checkEvent("PickedTime");
            } else {
                if (_selectingPosition) {
                    _selectingPosition = false;
                }
            } 
        }

        private void onNewTime(decimal newTime, decimal _) {
            var newPositionX = MathHelper.mapValue((float)newTime, 0, (float)SimulationManager.SIMULATION_LENGTH, -_rectTransform.sizeDelta.x / 2,
                _rectTransform.sizeDelta.x / 2);
            pointer.transform.localPosition = new Vector3(newPositionX, pointer.localPosition.y);
            var fillWidth = rectTransform.sizeDelta.x / 2 + newPositionX;
            fill.anchoredPosition = new Vector3(fillWidth/2, fill.anchoredPosition.y);
            fill.sizeDelta = new Vector2(fillWidth, fill.sizeDelta.y);
        }
    }
}