using System;
using src.element.effector;

namespace src.time.timeline {
    
    /// <summary>
    /// Represents a event that changes a effector at a given time point
    /// </summary>
    public class TimedEffectorEvent {

        private decimal _executionTime;

        public decimal ExecutionTime {
            set {
                _executionTime = value;
                IsDirty = true;
            }
            get => _executionTime;
        }

        private bool _alreadyExecuted;

        public bool IsDirty {
            private set;
            get;
        }
        
        public bool IsActive { get; set; }

        private readonly EffectorEvent _effectorEvent;

        public TimedEffectorEvent(decimal executionTime, EffectorEvent effectorEvent) {
            ExecutionTime = executionTime;
            _effectorEvent = effectorEvent;
            IsActive = true;
        }

        public void reset() {
            _alreadyExecuted = false;
            IsDirty = false;
        }

        public void execute() {
            if(!IsActive) return;
            if (!_alreadyExecuted) {
                _effectorEvent.effectorEvent.Invoke();
                _alreadyExecuted = true;
            } else {
                throw new Exception("TimeEffectorEvent has executed twice!");
            }
        }

        public string getName() {
            return _effectorEvent.name;
        }
        
    }
}