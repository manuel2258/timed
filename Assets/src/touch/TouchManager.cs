using System;
using System.Collections.Generic;
using System.Linq;
using src.misc;
using TMPro;
using UnityEngine;

namespace src.touch {
    
    /// <summary>
    /// A Singleton that captures touch inputs 
    /// </summary>
    public class TouchManager : UnitySingleton<TouchManager> {

        private const int MAX_FRAMES = 5;

        public bool overrideTouchSupported;

        public RectTransform cameraMovementArea;

        public bool blocked;

        /// <summary>
        /// The last frames touch inputs
        /// </summary>
        private readonly List<Frame> _currentTouches = new List<Frame>();

        private void Start() {
            for (int i = 0; i < MAX_FRAMES; i++) {
                _currentTouches.Add(new Frame());
            }
        }

        private void Update() {
            var frame = new Frame();
            if (!blocked) {
                if (Input.touchSupported || overrideTouchSupported) {
                    for (int i = 0; i < Input.touchCount; i++) {
                        var currentTouch = Input.GetTouch(i);
                        frame.addTouch(new Touch {
                            touchSize = currentTouch.radius,
                            worldPosition = CameraManager.Camera.ScreenToWorldPoint(currentTouch.position),
                            screenPosition = currentTouch.position,
                            inCameraArea = isPositionInsideCameraMovementArea(currentTouch.position),
                        });
                    }
                } else {
                    if (Input.GetMouseButton(0)) {
                        frame.addTouch(new Touch {
                            touchSize = 0,
                            worldPosition = CameraManager.Camera.ScreenToWorldPoint(Input.mousePosition),
                            screenPosition = Input.mousePosition,
                            inCameraArea = isPositionInsideCameraMovementArea(Input.mousePosition),
                        });
                    }
                }
            }

            _currentTouches.Insert(0, frame);

            if (_currentTouches.Count > MAX_FRAMES) {
                _currentTouches.RemoveAt(_currentTouches.Count - 1);
            }
        }

        public Frame getFrameAtPosition(int position) {
            return _currentTouches[position];
        }

        /// <summary>
        /// Checks whether the given position + size was touched
        /// </summary>
        /// <param name="position">The to check at position</param>
        /// <param name="size">The radius around the position</param>
        /// <returns>True when the position was touched, False otherwise</returns>
        public bool isTouched(Vector2 position, float size) {
            if (_currentTouches[4].WasTouched) return false;
            foreach (var touch in _currentTouches[0].Touches) {
                var distance = (position - touch.worldPosition).magnitude;
                if (distance <= size + touch.touchSize) {
                    return true;
                }
            }

            return false;
        }

        private bool isPositionInsideCameraMovementArea(Vector2 position) {
            return RectTransformUtility.RectangleContainsScreenPoint(cameraMovementArea, position,
                CameraManager.Camera);
        }
    }

    /// <summary>
    /// Contains the recorded Touches of a frame
    /// </summary>
    public class Frame {
        private bool _wasTouched;
        public bool WasTouched => _wasTouched;
        
        private readonly List<Touch> _touches = new List<Touch>();
        public List<Touch> Touches => _touches;

        public Frame() {
            _wasTouched = false;
        }

        public void addTouch(Touch touch) {
            _touches.Add(touch);
            _wasTouched = true;
        }

        public int touchAmount() {
            return _touches.Count;
        }
    }

    /// <summary>
    /// A touch at its world position and its touchSize
    /// </summary>
    public struct Touch {
        public Vector2 worldPosition;
        public Vector2 screenPosition;
        public float touchSize;
        public bool inCameraArea;
    }
}