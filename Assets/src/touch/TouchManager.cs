using System.Collections.Generic;
using src.misc;
using UnityEngine;

namespace src.touch {
    
    /// <summary>
    /// A Singleton that captures touch inputs 
    /// </summary>
    public class TouchManager : UnitySingleton<TouchManager> {

        /// <summary>
        /// The amount of saved backward frames
        /// </summary>
        private const int MAX_FRAMES = 5;

        public bool overrideTouchSupported;

        /// <summary>
        /// A RectTransform that acts
        /// </summary>
        public RectTransform cameraMovementArea;

        /// <summary>
        /// Blocks the capturing of touches, therefor will result in empty frames
        /// </summary>
        public bool blocked;

        /// <summary>
        /// The last frames touch inputs
        /// </summary>
        /// <remarks>
        /// This list is backwards!
        /// 0 Representing the current frame, 1 the lastFrame ... MAX_FRAMES the last saved frame
        /// </remarks>
        private readonly List<Frame> _currentTouches = new List<Frame>();

        private void Start() {
            for (int i = 0; i < MAX_FRAMES; i++) {
                _currentTouches.Add(new Frame());
            }

            if (cameraMovementArea != null) {
                UiMaskManager.Instance.addPersistentMask(cameraMovementArea);
            }
        }

        public void update() {
            var frame = new Frame();
            // Checks if on mobile or editor
            if (Input.touchSupported || overrideTouchSupported) {
                // If mobile go through each registered touch
                for (int i = 0; i < Input.touchCount; i++) {
                    var currentTouch = Input.GetTouch(i);
                    // And simply adds it to the frame
                    frame.addTouch(new Touch {
                        worldPosition = CameraManager.Camera.ScreenToWorldPoint(currentTouch.position),
                        screenPosition = currentTouch.position,
                        inCameraArea = isPositionInsideCameraMovementArea(currentTouch.position),
                        inputType = InputType.Touch,
                        originalTouch = currentTouch,
                    });
                }
            } else {
                // If in the Editor simply check for mouseInput and then adds it as well
                if (Input.GetMouseButton(0) || Input.GetMouseButtonDown(0) || Input.GetMouseButtonUp(0)) {
                    frame.addTouch(new Touch {
                        worldPosition = CameraManager.Camera.ScreenToWorldPoint(Input.mousePosition),
                        screenPosition = Input.mousePosition,
                        inCameraArea = isPositionInsideCameraMovementArea(Input.mousePosition),
                        inputType = InputType.Mouse,
                    });
                }
            }


            // Inserts the frame into the front
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
                if (distance <= size) {
                    return true;
                }
            }

            return false;
        }

        private bool isPositionInsideCameraMovementArea(Vector2 position) {
            if (blocked) return false;
            return RectTransformUtility.RectangleContainsScreenPoint(cameraMovementArea, position,
                CameraManager.Camera);
        }

        public bool currentTouchIsInsideUiMask(int id) {
            return !UiMaskManager.Instance.HasMask || 
                   UiMaskManager.Instance.touchIsInMask(_currentTouches[0].Touches[id].screenPosition);
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
        // Its positions
        public Vector2 worldPosition;
        public Vector2 screenPosition;

        // If the touch was in the CameraMoveArea
        public bool inCameraArea;

        public InputType inputType;
        
        public UnityEngine.Touch originalTouch;
    }

    public enum InputType {
        Touch,
        Mouse
    }
}