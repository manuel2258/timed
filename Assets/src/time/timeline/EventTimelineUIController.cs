using System;
using System.Collections.Generic;
using src.misc;
using src.simulation;
using src.time.time_managers;
using UnityEngine;
using UnityEngine.UI;

namespace src.time.timeline {
    
    /// <summary>
    /// A Singleton UI Controller that visualize the CurrentTime and its advancement
    /// </summary>
    public class EventTimelineUIController : UnitySingleton<EventTimelineUIController> {

        public Sprite inActiveEventPointer;
        public Sprite activeEventPointer;

        public GameObject pointerPrefab;

        private readonly Dictionary<TimedEffectorEvent, RectTransform> _eventPointers = new Dictionary<TimedEffectorEvent, RectTransform>();

        private RectTransform _rectTransform;

        private bool _activeEdit;
        private RectTransform _activePointer;

        private OnNewPickedTime _newTimeCallback;

        private void Start() {
            _rectTransform = transform as RectTransform;
            Timeline.Instance.onEffectorEventChanged += events => {
                List<TimedEffectorEvent> oldEvents = new List<TimedEffectorEvent>(_eventPointers.Keys);
                foreach (var effectorEvent in events) {
                    if (!_eventPointers.TryGetValue(effectorEvent, out var pointerTransform)) {
                        var newPointer = Instantiate(pointerPrefab, transform);
                        pointerTransform = newPointer.transform as RectTransform;
                        _eventPointers.Add(effectorEvent, pointerTransform);
                    } 
                    setPointerPosition(effectorEvent.ExecutionTime, pointerTransform);
                    oldEvents.Remove(effectorEvent);
                }
                
                foreach (var timedEffectorEvent in oldEvents) {
                    if(!_eventPointers.TryGetValue(timedEffectorEvent, out var pointerTransform)) {
                        throw new Exception("Could not find transform of removed timedEffectorEvent!");
                    }
                    _eventPointers.Remove(timedEffectorEvent);
                    Destroy(pointerTransform.gameObject);
                }
            };

            ReplayTimeManager.Instance.onNewTime += (currentTime, _) => {
                if (_activeEdit) {
                    setPointerPosition(currentTime, _activePointer);
                    _newTimeCallback.Invoke(currentTime);
                }
            };
        }

        private void setPointerPosition(decimal time, RectTransform pointerTransform) {
            var newPositionX = MathHelper.mapValue((float)time, 0, (float)SimulationManager.SIMULATION_LENGTH, -_rectTransform.sizeDelta.x / 2,
                _rectTransform.sizeDelta.x / 2);
            pointerTransform.transform.localPosition = new Vector3(newPositionX, pointerTransform.localPosition.y);
        }

        /// <summary>
        /// Activates the timePickerMode
        /// </summary>
        /// <param name="callback">The to call callback</param>
        /// <param name="effectorEvent">The to change effector</param>
        public void setTimePickerMode(OnNewPickedTime callback, TimedEffectorEvent effectorEvent) {
            _activeEdit = true;
            _newTimeCallback = callback;

            if (!_eventPointers.TryGetValue(effectorEvent, out _activePointer)) {
                throw new Exception("Could not find a listed EffectorEvent!");
            }

            _activePointer.GetComponent<Image>().sprite = activeEventPointer;
            
            setPointerPosition(effectorEvent.ExecutionTime, _activePointer);
        }

        public void exitTimePickerMode() {
            _activeEdit = false;
            if (_activePointer != null) {
                _activePointer.GetComponent<Image>().sprite = inActiveEventPointer;
            }
        }
    }
    
    public delegate void OnNewPickedTime(decimal newTime);
}