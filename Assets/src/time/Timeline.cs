using System.Collections.Generic;
using src.misc;
using UnityEngine;

namespace src.time {
    
    /// <summary>
    /// A Singleton representing a series of EffectorEvents
    /// </summary>
    public class Timeline : UnitySingleton<Timeline> {

        private float _currentTime;

        private readonly List<EffectorEvent> _effectors = new List<EffectorEvent>();
        private List<EffectorEvent> _activeEffectors = new List<EffectorEvent>();
        
        private void FixedUpdate() {
            advance(Time.fixedDeltaTime);
        }

        public void addEffector(EffectorEvent effector) {
            _effectors.Add(effector);
        }

        public void reset() {
            foreach (var effector in _effectors) {
                effector.reset();
            }
            _activeEffectors = new List<EffectorEvent>(_effectors);
        }

        private void advance(float deltaTime) {
            _currentTime += deltaTime;

            for(int i = _activeEffectors.Count-1; i >= 0; i--) {
                var effector = _activeEffectors[i];
                if (!(effector.ExecutionTime < _currentTime))  continue;

                effector.execute();
                _activeEffectors.Remove(effector);
            }
        }
    }
}