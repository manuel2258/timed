using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using src.misc;
using UnityEngine;
using UnityEngine.Assertions.Comparers;

namespace src.simulation {
    
    /// <summary>
    /// A tracker that saves meta data about a gameObject
    /// </summary>
    public class GameObjectTracker {

        public readonly Dictionary<decimal, Vector2> _positions = new Dictionary<decimal, Vector2>();
        private readonly Dictionary<decimal, Quaternion> _rotations = new Dictionary<decimal, Quaternion>();

        private readonly GameObject _target;
        private readonly Transform _transform;

        public GameObjectTracker(GameObject target) {
            _target = target;
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
        public void replayTimestampOld(decimal timestamp) {
            if (!_positions.ContainsKey(timestamp)) {
                timestamp = _positions.Keys.Aggregate((x, y) =>
                    Math.Abs(x - timestamp) < Math.Abs(y - timestamp) ? x : y);
            }

            if (!_positions.TryGetValue(timestamp, out var currentPosition) || 
                !_rotations.TryGetValue(timestamp, out var currentRotation)) {
                throw new Exception("Could not find a fitting timestamp for: " + timestamp);
            }
            _transform.position = currentPosition;
            _transform.rotation = currentRotation;
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
                var secondPosition = _positions.Count > index ? _positions.ElementAt(index + 1).Value : firstPosition;
                var firstRotation = _rotations.ElementAt(index).Value;
                var secondRotation = _rotations.Count > index ? _rotations.ElementAt(index + 1).Value : firstRotation;
                
                var interpolationValue = timestamp - SimulationManager.SIMULATION_STEPS * index;
                var lerpWeight = MathHelper.mapValue((float)interpolationValue, 0, 
                    (float) SimulationManager.SIMULATION_STEPS, 0, 1);
                
                Debug.Log(lerpWeight);
                
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