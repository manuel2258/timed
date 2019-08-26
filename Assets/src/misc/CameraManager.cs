using UnityEngine;

namespace src.misc {
    public class CameraManager : UnitySingleton<CameraManager> {
        #region Singleton Implementation

        private static Camera _instance;

        public static Camera Camera {
            get {
                if (_instance == null) {
                    _instance = FindObjectOfType<Camera>();
                }

                return _instance;
            }
        }

        #endregion
    }
}