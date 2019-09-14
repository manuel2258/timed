using src.misc;
using src.simulation;
using src.time.time_managers;

namespace src.time.timeline {
    
    /// <summary>
    /// A Singleton that manages the editing of a TimedEffectorEvents
    /// </summary>
    public class TimedEffectorEventEditManager : UnitySingleton<TimedEffectorEventEditManager> {

        public OnEffectorEventTimeChanged onEffectorEventTimeChanged;
        private TimedEffectorEvent _currentEffectorEvent;
        
        /// <summary>
        /// Enables the editing of the pressed timedEffect / saves it when pressed again
        /// </summary>
        /// <param name="effectorEvent">The pressed timedEvent</param>
        public void onTimedEffectorEventButtonPressed(TimedEffectorEvent effectorEvent) {
            if (_currentEffectorEvent != null) {
                TimedEffectorEventPopupUIController.Instance.closePopup();
                EventTimelineUIController.Instance.exitTimePickerMode();
                _currentEffectorEvent.IsActive = true;
                Timeline.Instance.effectorTimeChanged();
                ReplayManager.Instance.Active = true;
                ReplayUIController.Instance.setActiveChangeButtonState(true);
            }
            if (_currentEffectorEvent != effectorEvent) {
                _currentEffectorEvent = effectorEvent;
                TimedEffectorEventPopupUIController.Instance.showTimedEffectorEvent(effectorEvent);
                if (effectorEvent.IsActive) {
                    effectorEvent.IsActive = false;
                    Timeline.Instance.effectorTimeChanged();
                }
                EventTimelineUIController.Instance.setTimePickerMode(newTime => {
                    effectorEvent.ExecutionTime = newTime;
                    onEffectorEventTimeChanged?.Invoke(newTime);
                }, effectorEvent);
                ReplayManager.Instance.Active = false;
                ReplayTimeManager.Instance.setCurrentTime(effectorEvent.ExecutionTime);
                ReplayUIController.Instance.setActiveChangeButtonState(false);
            } else {
                _currentEffectorEvent = null;
            }
        }

        public void removeTimedEffectorEvent(TimedEffectorEvent effectorEvent) {
            Timeline.Instance.removeEffectorEvent(effectorEvent);
            onTimedEffectorEventButtonPressed(effectorEvent);
        }
    }

    public delegate void OnEffectorEventTimeChanged(decimal newTime);
}