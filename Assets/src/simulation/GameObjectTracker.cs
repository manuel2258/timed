using System;
using System.Collections.Generic;
using System.Linq;
using src.misc;
using UnityEngine;

namespace src.simulation {
    
    /// <summary>
    /// A tracker that saves meta data about a gameObject
    /// </summary>
    public class GameObjectTracker {

        private readonly Dictionary<decimal, Vector3> _positions = new Dictionary<decimal, Vector3>();
        private readonly Dictionary<decimal, Quaternion> _rotations = new Dictionary<decimal, Quaternion>();
        
        public Dictionary<decimal, Vector3> Positions => new Dictionary<decimal, Vector3>(_positions);
        public Dictionary<decimal, Quaternion> Rotations => new Dictionary<decimal, Quaternion>(_rotations);
        
        private readonly Transform _transform;

        public GameObjectTracker(GameObject target) {
            _transform = target.transform;
        }

        /// <summary>
        /// Tracks the current meta data of the target and saves it under the currentTime
        /// </summary>
        /// <param name="currentTime">The to save at time</param>
        public void track(decimal currentTime) {
            _positions.Add(currentTime, _transform.position);
            _rotations.Add(currentTime, _transform.rotation);
        }

        /// <summary>
        /// Sets the meta data of the target for the given timestamp
        /// </summary>
        /// <param name="timestamp">The to set timestamp</param>
        public void replayTimestamp(decimal timestamp) {
            Vector2 currentPosition;
            Quaternion currentRotation;
            if (!_positions.ContainsKey(timestamp)) {
                var index = (int)Math.Floor(timestamp / SimulationManager.SIMULATION_STEPS);

                var firstPosition = _positions.ElementAt(index).Value;
                var secondPosition = _positions.Count > index + 1 ? _positions.ElementAt(index + 1).Value : firstPosition;
                var firstRotation = _rotations.ElementAt(index).Value;
                var secondRotation = _rotations.Count > index + 1 ? _rotations.ElementAt(index + 1).Value : firstRotation;
                
                var interpolationValue = timestamp - SimulationManager.SIMULATION_STEPS * index;
                var lerpWeight = MathHelper.mapValue((float)interpolationValue, 0, 
                    (float) SimulationManager.SIMULATION_STEPS, 0, 1);

                currentPosition = Vector2.Lerp(firstPosition, secondPosition, lerpWeight);
                currentRotation = Quaternion.Lerp(firstRotation, secondRotation, lerpWeight);
            } else {
                currentPosition = _positions[timestamp];
                currentRotation = _rotations[timestamp];
            }
            
            _transform.position = currentPosition;
            _transform.rotation = currentRotation;
        }

        
    }
}