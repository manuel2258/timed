using src.misc;
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
            if (_currentEffectorEvent == effectorEvent) {
                _currentEffectorEvent = null;
                TimedEffectorEventPopupUIController.Instance.closePopup();
                TimelineUIController.Instance.exitTimePickerMode();
                if (effectorEvent.IsDirty) {
                    effectorEvent.IsActive = true;
                    Timeline.Instance.effectorTimeChanged();
                }
                ReplayTimeManager.Instance.OverrideActive = false;
            } else {
                _currentEffectorEvent = effectorEvent;
                TimedEffectorEventPopupUIController.Instance.showTimedEffectorEvent(effectorEvent);
                TimelineUIController.Instance.setTimePickerMode(newTime => {
                    effectorEvent.ExecutionTime = newTime;
                    onEffectorEventTimeChanged?.Invoke(newTime);
                    if (effectorEvent.IsActive) {
                        effectorEvent.IsActive = false;
                        Timeline.Instance.effectorTimeChanged();
                    }
                }, effectorEvent.ExecutionTime);
                ReplayTimeManager.Instance.OverrideActive = true;
            }
        }
    }

    public delegate void OnEffectorEventTimeChanged(decimal newTime);
}