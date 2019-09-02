using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using UnityEngine;

namespace src.simulation {
    
    /// <summary>
    /// A tracker that saves meta data about a gameObject
    /// </summary>
    public class GameObjectTracker {

        private readonly Dictionary<float, Vector2> _positions = new Dictionary<float, Vector2>();
        private readonly Dictionary<float, Quaternion> _rotations = new Dictionary<float, Quaternion>();

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
        public void track(float currentTime) {
            _positions.Add(currentTime, _transform.position);
            _rotations.Add(currentTime, _transform.rotation);
        }

        /// <summary>
        /// Sets the meta data of the target for the given timestamp
        /// </summary>
        /// <param name="timestamp">The to set timestamp</param>
        public void replayTimestamp(float timestamp) {
            if (!_positions.ContainsKey(timestamp)) {
                timestamp = _positions.Keys.Aggregate((x, y) =>
                    Math.Abs(x - timestamp) < Math.Abs(y - timestamp) ? x : y);
            }

            if (!_positions.TryGetValue(timestamp, out var currentPosition)) {
                throw new TrackedTimestampNotCalculateAbleException("Could not find a fitting timestamp for: " + timestamp);
            }
            _transform.position = currentPosition;
            
            if (!_rotations.TryGetValue(timestamp, out var currentRotation)) {
                throw new TrackedTimestampNotCalculateAbleException("Could not find a fitting timestamp for: " + timestamp);
            }
            _transform.rotation = currentRotation;
        }
    }

    public class TrackedTimestampNotCalculateAbleException : Exception {
        public TrackedTimestampNotCalculateAbleException(string message) : base(message) { }
    }
}