using src.element.effector.effectors;

namespace src.time.timeline {
    
    /// <summary>
    /// Represents a event that changes a effector at a given time point
    /// </summary>
    public class TimedVisualEvent {
        
        public decimal ExecutionTime { get; }

        private readonly IVisualStateAble _stateChanger;
        private readonly VisualState _beforeState;
        private readonly VisualState _afterState;

        public TimedVisualEvent(decimal executionTime, IVisualStateAble stateChanger, VisualState before, VisualState after) {
            ExecutionTime = executionTime;
            _stateChanger = stateChanger;
            _beforeState = before;
            _afterState = after;
        }
        
        public void executeForward() {
            _stateChanger.setVisualsByState(_afterState);
        }
        
        public void executeBackwards() {
            _stateChanger.setVisualsByState(_beforeState);
        }
    }
}