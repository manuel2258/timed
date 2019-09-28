using System;
using System.Collections.Generic;
using src.misc;
using src.time.time_managers;
using UnityEngine;

namespace src.time.timeline {
    public class EventlineUIController : UnitySingleton<EventlineUIController> {

        public Transform contentParent;
        public GameObject contentPrefab;
        public Transform nextEventPointer;
        
        private readonly Dictionary<TimedEffectorEvent, TimedEffectorEventUIController> _eventUIControllers =
            new Dictionary<TimedEffectorEvent, TimedEffectorEventUIController>();

        private decimal[] _effectorEventTimes = new decimal[0];
        
        private void Start() {
            Timeline.Instance.onEffectorEventChanged += updateEffectorEvents;
            ReplayTimeManager.Instance.onNewTime += updateCurrentTimePointer;
        }

        private void updateEffectorEvents(List<TimedEffectorEvent> effectorEvents) {
            for (int i = contentParent.childCount-1; i >= 0 ; i--) {
                var currentTransform = contentParent.GetChild(i);
                if (currentTransform != nextEventPointer) {
                    Destroy(currentTransform.gameObject);
                }
            }
            
            _eventUIControllers.Clear();
            _effectorEventTimes = new decimal[effectorEvents.Count];
            for (int i = 0; i < effectorEvents.Count; i++) {
                var effectorEvent = effectorEvents[i];
                var newObject = Instantiate(contentPrefab, contentParent);
                var uiController = newObject.GetComponent<TimedEffectorEventUIController>();
                uiController.setup(effectorEvent);
                _eventUIControllers.Add(effectorEvent, uiController);
                _effectorEventTimes[i] = effectorEvent.ExecutionTime;
            }
        }

        public void highlightEvent(TimedEffectorEvent effectorEvent) {
            disHighlightAll();
            if (!_eventUIControllers.TryGetValue(effectorEvent, out var controller)) {
                throw new Exception($"Could not find a UIController for {effectorEvent}");
            }
            controller.setHighlight(true);
        }

        public void disHighlightAll() {
            foreach (var timedEffectorEventUIController in _eventUIControllers) {
                timedEffectorEventUIController.Value.setHighlight(false);
            }
        }

        private void updateCurrentTimePointer(decimal currentTime, decimal deltaTime) {
            if (_effectorEventTimes.Length <= 0) return;
            
            int index = _effectorEventTimes.Length;
            for (var i = 0; i < _effectorEventTimes.Length; i++) {
                var eventTime = _effectorEventTimes[i];
                if (currentTime < eventTime) {
                    index = i;
                    break;
                }
            }
            nextEventPointer.SetSiblingIndex(index);
        }
    }
}