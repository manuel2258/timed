using System;
using src.misc;
using src.simulation;
using src.time.time_managers;
using src.tutorial.check_events;

namespace src.time.timeline {
    
    /// <summary>
    /// A Singleton that manages the editing of a TimedEffectorEvents
    /// </summary>
    public class TimedEffectorEventEditManager : UnitySingleton<TimedEffectorEventEditManager>, ICheckAbleEvent {

        public OnEffectorEventTimeChanged onEffectorEventTimeChanged;
        private TimedEffectorEvent _currentEffectorEvent;

        private bool _timePickerBlocked;
        
        private readonly CheckEventManager _checkEventManager = new CheckEventManager();
        public void registerEvent(string eventName, Action onEventChecked) {
            _checkEventManager.registerEvent(eventName, onEventChecked);
        }

        private void Start() {
            SimulationManager.Instance.onCalculationStarted += wasSide => _timePickerBlocked = true;
            SimulationManager.Instance.onCalculationFinished += (trackers, wasSide) => _timePickerBlocked = false;
        }

        /// <summary>
        /// Enables the editing of the pressed timedEffect / saves it when pressed again
        /// </summary>
        /// <param name="effectorEvent">The pressed timedEvent</param>
        public void onTimedEffectorEventButtonPressed(TimedEffectorEvent effectorEvent) {
            exitEditing();
            if (_currentEffectorEvent != effectorEvent) {
                _currentEffectorEvent = effectorEvent;
                TimedEffectorEventPopupUIController.Instance.showTimedEffectorEvent(effectorEvent);
                if (effectorEvent.IsActive) {
                    effectorEvent.IsActive = false;
                    Timeline.Instance.effectorTimeChanged();
                }
                EventTimelineUIController.Instance.setTimePickerMode(newTime => {
                    if (_timePickerBlocked) return;
                    effectorEvent.ExecutionTime = newTime;
                    onEffectorEventTimeChanged?.Invoke(newTime);
                    _checkEventManager.checkEvent("ChangedEventTime");
                    _checkEventManager.checkEvent($"ChangedEventTimeTo{newTime:N1}");
                }, effectorEvent);
                ReplayManager.Instance.disableActive();
                ReplayTimeManager.Instance.setCurrentTime(effectorEvent.ExecutionTime);
                ReplayUIController.Instance.setActiveChangeButtonState(false);
                EventlineUIController.Instance.highlightEvent(effectorEvent);
                _checkEventManager.checkEvent("EditOpened");
            } else {
                _currentEffectorEvent = null;
                EventlineUIController.Instance.disHighlightAll();
            }
        }

        public void exitEditing() {
            if (_currentEffectorEvent == null) return;
            
            TimedEffectorEventPopupUIController.Instance.closePopup();    
            EventTimelineUIController.Instance.exitTimePickerMode();
            _currentEffectorEvent.IsActive = true;
            Timeline.Instance.effectorTimeChanged();
            ReplayManager.Instance.restoreActive();
            ReplayUIController.Instance.setActiveChangeButtonState(true);
            _checkEventManager.checkEvent("EditClosed");
        }

        public void removeTimedEffectorEvent(TimedEffectorEvent effectorEvent) {
            Timeline.Instance.removeEffectorEvent(effectorEvent);
            onTimedEffectorEventButtonPressed(effectorEvent);
        }
    }

    public delegate void OnEffectorEventTimeChanged(decimal newTime);
}