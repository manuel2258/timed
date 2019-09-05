using src.elements.effectors;

namespace src.time {
    
    /// <summary>
    /// Represents a event that changes a effector at a given time point
    /// </summary>
    public class TimedEffectorEvent {
        
        public decimal ExecutionTime { set; get; }
        private bool _alreadyExecuted;

        private readonly EffectorEvent _effectorEvent;

        public TimedEffectorEvent(decimal executionTime, EffectorEvent effectorEvent) {
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
            _effectorEvent.effectorEvent.Invoke();
            _alreadyExecuted = true;
        }

        public string getName() {
            return _effectorEvent.name;
        }
        
    }
}