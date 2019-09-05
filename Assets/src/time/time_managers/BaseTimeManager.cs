using System;
using src.misc;
using UnityEngine;

namespace src.time.time_managers {
    
    /// <summary>
    /// Singleton based base-class for timeManagers
    /// </summary>
    /// <typeparam name="T">The to use singleton type</typeparam>
    public abstract class BaseTimeManager<T> : UnitySingleton<T> where T : MonoBehaviour {

        protected decimal currentTime;

        public float debugTime;

        private void Update() {
            debugTime = (float)currentTime;
        }

        public OnNewTime onNewTime;
        public decimal CurrentTime => currentTime;
    }
    
    public delegate void OnNewTime(decimal newTime, decimal deltaTime);
}