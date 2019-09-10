using System.Collections.Generic;
using src.misc;
using UnityEngine;

namespace src.touch {
    
    /// <summary>
    /// A Singleton that captures touch inputs 
    /// </summary>
    public class TouchManager : UnitySingleton<TouchManager> {

        public bool overrideTouchSupported;

        /// <summary>
        /// The last frames touch inputs
        /// </summary>
        private readonly List<Touch> _currentTouches = new List<Touch>();
        
        private void Update() {
            _currentTouches.Clear();
            if (Input.touchSupported || overrideTouchSupported) {
                for (int i = 0; i < Input.touchCount; i++) {
                    var currentTouch = Input.GetTouch(i);
                    _currentTouches.Add(new Touch {
                        touchSize = currentTouch.radius,
                        worldPosition = CameraManager.Camera.ScreenToWorldPoint(currentTouch.position)
                    });
                }
            } else {
                if (Input.GetMouseButton(0)) {
                    _currentTouches.Add(new Touch {
                        touchSize = 0,
                        worldPosition = CameraManager.Camera.ScreenToWorldPoint(Input.mousePosition)
                    });
                }
            }
        }

        public bool isScreenTouched() {
            return _currentTouches.Count > 0;
        }

        /// <summary>
        /// Checks whether the given position + size was touched
        /// </summary>
        /// <param name="position">The to check at position</param>
        /// <param name="size">The radius around the position</param>
        /// <returns>True when the position was touched, False otherwise</returns>
        public bool isTouched(Vector2 position, float size) {
            foreach (var touch in _currentTouches) {
                var distance = (position - touch.worldPosition).magnitude;
                if (distance <= size + touch.touchSize) {
                    return true;
                }
            }

            return false;
        }
    }

    /// <summary>
    /// A touch at its world position and its touchSize
    /// </summary>
    internal struct Touch {
        public Vector2 worldPosition;
        public float touchSize;
    }
}