using src.misc;
using UnityEngine;

namespace src.time.time_managers {
    
    /// <summary>
    /// Singleton based base-class for timeManagers
    /// </summary>
    /// <typeparam name="T">The to use singleton type</typeparam>
    public abstract class BaseTimeManager<T> : UnitySingleton<T> where T : MonoBehaviour {

        protected float currentTime;

        public OnNewTime onNewTime;
        public float CurrentTime => currentTime;
    }
    
    public delegate void OnNewTime(float newTime, float deltaTime);
}