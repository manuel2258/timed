using src.elements.effectors;

namespace src.time {
    
    /// <summary>
    /// Represents a event that changes a effector at a given time point
    /// </summary>
    public class TimedEffectorEvent {
        
        public float ExecutionTime { set; get; }
        private bool _alreadyExecuted;

        private readonly TriggerEffectorEvent _effectorEvent;

        public TimedEffectorEvent(float executionTime, TriggerEffectorEvent effectorEvent) {
            ExecutionTime = executionTime;
            _effectorEvent = effectorEvent;
        }
        
        public bool wasAlreadyExecuted() {
            return _alreadyExecuted;
        }

        public void reset() {
            _alreadyExecuted = false;
        }

        public void execute() {
            _effectorEvent.Invoke();
            _alreadyExecuted = true;
        }
        
    }
}