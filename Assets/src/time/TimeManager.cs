using src.misc;
using UnityEngine;

namespace src.time {
    
    /// <summary>
    /// A Singleton managing the current time and its advancement
    /// </summary>
    public class TimeManager : UnitySingleton<TimeManager> {
        
        private float _currentTime;
        public float CurrentTime => _currentTime;

        private float _timeMultiplier = 1;

        public OnNewTime onNewTime;

        private void FixedUpdate() {
            var deltaTime = Time.fixedDeltaTime * _timeMultiplier;
            _currentTime += deltaTime;
            onNewTime.Invoke(_currentTime, deltaTime);
        }
    }
    
    public delegate void OnNewTime(float newTime, float deltaTime);
}