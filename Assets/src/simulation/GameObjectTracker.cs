using System;
using System.Collections.Generic;
using System.Linq;
using src.element.collider_body;
using src.misc;
using UnityEngine;

namespace src.simulation {
    
    /// <summary>
    /// A tracker that saves meta data about a gameObject
    /// </summary>
    public class GameObjectTracker {

        private readonly Dictionary<decimal, Vector3> _positions = new Dictionary<decimal, Vector3>();
        private readonly Dictionary<decimal, Quaternion> _rotations = new Dictionary<decimal, Quaternion>();
        private readonly Dictionary<decimal, Vector2> _velocities = new Dictionary<decimal, Vector2>();
        
        public Dictionary<decimal, Vector3> Positions => new Dictionary<decimal, Vector3>(_positions);
        public Dictionary<decimal, Quaternion> Rotations => new Dictionary<decimal, Quaternion>(_rotations);
        
        private readonly ColliderBody _colliderBody;

        public GameObjectTracker(GameObject target) {
            _colliderBody = target.GetComponent<ColliderBody>();
        }

        /// <summary>
        /// Tracks the current meta data of the target and saves it under the currentTime
        /// </summary>
        /// <param name="currentTime">The to save at time</param>
        public void track(decimal currentTime) {
            _positions.Add(currentTime, _colliderBody.transform.position);
            _rotations.Add(currentTime, _colliderBody.transform.rotation);
            _velocities.Add(currentTime, _colliderBody.Rigidbody.velocity);
        }

        /// <summary>
        /// Sets the meta data of the target for the given timestamp
        /// </summary>
        /// <param name="timestamp">The to set timestamp</param>
        public void replayTimestamp(decimal timestamp) {
            Vector3 currentPosition;
            Quaternion currentRotation;
            Vector2 currentVelocity;
            if (!_positions.ContainsKey(timestamp)) {
                var index = (int)Math.Floor(timestamp / SimulationManager.SIMULATION_STEPS);
                if (index >= _positions.Count) {
                    index = _positions.Count - 1;
                }

                var firstPosition = _positions.ElementAt(index).Value;
                var secondPosition = _positions.Count > index + 1 ? _positions.ElementAt(index + 1).Value : firstPosition;
                var firstRotation = _rotations.ElementAt(index).Value;
                var secondRotation = _rotations.Count > index + 1 ? _rotations.ElementAt(index + 1).Value : firstRotation;
                var firstVelocity = _velocities.ElementAt(index).Value;
                var secondVelocity = _velocities.Count > index + 1 ? _velocities.ElementAt(index + 1).Value : firstVelocity;
                
                var interpolationValue = timestamp - SimulationManager.SIMULATION_STEPS * index;
                var lerpWeight = MathHelper.mapValue((float)interpolationValue, 0, 
                    (float) SimulationManager.SIMULATION_STEPS, 0, 1);

                currentPosition = Vector3.Lerp(firstPosition, secondPosition, lerpWeight);
                currentRotation = Quaternion.Lerp(firstRotation, secondRotation, lerpWeight);
                currentVelocity = Vector2.Lerp(firstVelocity, secondVelocity, lerpWeight);
            } else {
                currentPosition = _positions[timestamp];
                currentRotation = _rotations[timestamp];
                currentVelocity = _velocities[timestamp];
            }
            
            _colliderBody.setVisualPosition(currentPosition, currentRotation, currentVelocity);
        }

        
    }
}