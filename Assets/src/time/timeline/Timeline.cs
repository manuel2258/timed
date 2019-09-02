using System.Collections.Generic;
using src.misc;
using src.time.time_managers;

namespace src.time.timeline {
    
    /// <summary>
    /// A Singleton representing a series of EffectorEvents
    /// </summary>
    public class Timeline : UnitySingleton<Timeline> {

        private readonly List<TimedEffectorEvent> _effectors = new List<TimedEffectorEvent>();
        private List<TimedEffectorEvent> _activeEffectors = new List<TimedEffectorEvent>();

        private void Start() {
            SimulationTimeManager.Instance.onNewTime += onNewTime;
            reset();
        }

        public void addEffector(TimedEffectorEvent effectorEvent) {
            _effectors.Add(effectorEvent);
        }

        /// <summary>
        /// Resets the effectors 
        /// </summary>
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
        private void onNewTime(float currentTime, float _) {
            for(int i = _activeEffectors.Count-1; i >= 0; i--) {
                var effector = _activeEffectors[i];
                if (!(effector.ExecutionTime < currentTime))  continue;

                effector.execute();
                _activeEffectors.Remove(effector);
            }
        }
    }
}