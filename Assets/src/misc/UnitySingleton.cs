using UnityEngine;

namespace src.misc {
    
    /// <summary>
    /// Simple Singleton implementation to get static MonoBehaviour references
    /// </summary>
    /// <typeparam name="T">The reference type</typeparam>
    public abstract class UnitySingleton<T> : MonoBehaviour where T : MonoBehaviour {
        #region Singleton Implementation

        private static T _instance;

        public static T Instance {
            get {
                if (_instance == null) {
                    _instance = (T)FindObjectOfType(typeof(T));
                }

                return _instance;
            }
        }

        #endregion
    }
}