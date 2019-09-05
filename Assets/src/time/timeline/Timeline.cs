using System;
using System.Collections.Generic;
using src.misc;
using src.simulation;
using src.simulation.reseting;
using src.time.time_managers;
using UnityEngine;

namespace src.time.timeline {
    
    /// <summary>
    /// A Singleton representing a series of EffectorEvents
    /// </summary>
    public class Timeline : UnitySingleton<Timeline>, IResetable {

        private readonly List<TimedEffectorEvent> _effectors = new List<TimedEffectorEvent>();
        private List<TimedEffectorEvent> _activeEffectors = new List<TimedEffectorEvent>();

        public OnEffectorEventChanged onEffectorEventChanged;

        private void Start() {
            SimulationTimeManager.Instance.onNewTime += onNewTime;
            SimulationManager.Instance.onCalculationStarted += reset;
            reset();
        }

        public void addEffectorEvent(TimedEffectorEvent effectorEvent) {
            _effectors.Add(effectorEvent);
            _effectors.Sort((x, y) => Math.Sign(x.ExecutionTime - y.ExecutionTime));
            onEffectorEventChanged?.Invoke(_effectors);
        }

        public void removeEffectorEvent(TimedEffectorEvent effectorEvent) {
            _effectors.Remove(effectorEvent);
            _effectors.Sort((x, y) => Math.Sign(x.ExecutionTime - y.ExecutionTime));
            onEffectorEventChanged?.Invoke(_effectors);
        }
        
        public void reset() {
            foreach (var effector in _effectors) {
                effector.reset();
            }
            _activeEffectors = new List<TimedEffectorEvent>(_effectors);
        }

        /// <summary>
        /// Executes effectorEvents when present for the currentTime
        /// </summary>
        /// <param name="currentTime">The current time in seconds</param>
        /// <param name="_">Ignored deltaTime</param>
        private void onNewTime(decimal currentTime, decimal _) {
            for(int i = _activeEffectors.Count-1; i >= 0; i--) {
                var effector = _activeEffectors[i];
                if (effector.ExecutionTime > currentTime)  continue;

                effector.execute();
                Debug.Log($"Executed: {effector.getName()} at {currentTime}");
                _activeEffectors.Remove(effector);
            }
        }
    }

    public delegate void OnEffectorEventChanged(List<TimedEffectorEvent> events);
}