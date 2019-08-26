namespace src.time {
    
    /// <summary>
    /// Represents a event that changes a effector at a given time point
    /// </summary>
    public class EffectorEvent {
        
        public float ExecutionTime { set; get; }
        private bool _alreadyExecuted;

        public EffectorEvent(float executionTime, bool alreadyExecuted) {
            ExecutionTime = executionTime;
            _alreadyExecuted = alreadyExecuted;
        }
        
        public bool wasAlreadyExecuted() {
            return _alreadyExecuted;
        }

        public void reset() {
            _alreadyExecuted = false;
        }

        public void execute() {
            _alreadyExecuted = true;
        }
    }
}