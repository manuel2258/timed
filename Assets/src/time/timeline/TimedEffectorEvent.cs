using System;
using src.element.effector;
using UnityEngine;

namespace src.time.timeline {
    
    /// <summary>
    /// Represents a event that changes a effector at a given time point
    /// </summary>
    public class TimedEffectorEvent {
        
        public decimal ExecutionTime { set; get; }

        private bool _alreadyExecuted;

        public bool IsActive { get; set; }

        private readonly EffectorEvent _effectorEvent;

        public TimedEffectorEvent(decimal executionTime, EffectorEvent effectorEvent) {
            ExecutionTime = executionTime;
            _effectorEvent = effectorEvent;
            IsActive = true;
        }

        public void reset() {
            _alreadyExecuted = false;
        }

        public void execute() {
            if (!_alreadyExecuted) {
                _effectorEvent.simulationEvent.Invoke();
                _alreadyExecuted = true;
            } else {
                throw new Exception("TimeEffectorEvent has been executed twice!");
            }
        }

        public Sprite getIcon() {
            return _effectorEvent.icon;
        }

        public override string ToString() {
            return $"[{ExecutionTime}, {_effectorEvent.simulationEvent.Target}]";
        }
    }
}