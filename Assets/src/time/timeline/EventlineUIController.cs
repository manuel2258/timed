using System;
using System.Collections.Generic;
using src.misc;
using UnityEngine;

namespace src.time.timeline {
    public class EventlineUIController : UnitySingleton<EventlineUIController> {

        public Transform contentParent;
        public GameObject contentPrefab;
        
        private readonly Dictionary<TimedEffectorEvent, TimedEffectorEventUIController> _eventUIControllers =
            new Dictionary<TimedEffectorEvent, TimedEffectorEventUIController>();
        
        private void Start() {
            Timeline.Instance.onEffectorEventChanged += updateEffectorEvents;
        }

        private void updateEffectorEvents(List<TimedEffectorEvent> effectorEvents) {
            for (int i = contentParent.childCount-1; i >= 0 ; i--) {
                Destroy(contentParent.GetChild(i).gameObject);
            }
            _eventUIControllers.Clear();
            foreach (var effectorEvent in effectorEvents) {
                var newObject = Instantiate(contentPrefab, contentParent);
                var uiController = newObject.GetComponent<TimedEffectorEventUIController>();
                uiController.setup(effectorEvent);
                _eventUIControllers.Add(effectorEvent, uiController);
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
    }
}