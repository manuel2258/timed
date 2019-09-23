using src.touch;
using UnityEngine;

namespace src.misc {
    
    /// <summary>
    /// Manges the camera movement
    /// </summary>
    public class CameraManager : UnitySingleton<CameraManager> {
        private const int MAX_X = 20;
        private const int MAX_Y = 10;

        public float panningMultiplier;
        public float zoomMultiplier;

        public Transform parallax;
        
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

        private void Update() {
            // Gets the current touch position and the last frames one
            var currentFrame = TouchManager.Instance.getFrameAtPosition(0);
            var lastFrame = TouchManager.Instance.getFrameAtPosition(1);

            // Position Updating
            // Checks whether both frames contain one touch
            if (currentFrame.touchAmount() == 1 && lastFrame.touchAmount() == 1) {
                // If so gets the first touch
                var currentTouch = currentFrame.Touches[0];
                var lastTouch = lastFrame.Touches[0];
                
                // Then checks whether they are in the camera area
                if (currentTouch.inCameraArea && lastTouch.inCameraArea) {
                    // If so calculates the dif and subtracts it from the current position
                    var diff = currentTouch.screenPosition - lastTouch.screenPosition;
                    var newPosition = transform.position - Time.deltaTime * panningMultiplier *
                                      Camera.orthographicSize / 10 * (Vector3) diff;
                    
                    // Then clamp its position to maxValues
                    newPosition.x = Mathf.Clamp(newPosition.x, -MAX_X, MAX_X);
                    newPosition.y = Mathf.Clamp(newPosition.y, -MAX_Y, MAX_Y);

                    parallax.transform.localPosition = newPosition / 10;
                    
                    // And then applies the position
                    transform.position = newPosition;
                }
            }

            // Zoom Updating
            // Checks if on mobile or testing on PC
            if (Input.touchSupported) {
                // Then check if the current and last frame contains 2 touches
                if (currentFrame.touchAmount() == 2 && lastFrame.touchAmount() == 2) {
                    // If so gets the difference of each
                    var currentDiff = currentFrame.Touches[0].screenPosition - currentFrame.Touches[1].screenPosition;
                    var lastDiff = lastFrame.Touches[0].screenPosition - lastFrame.Touches[1].screenPosition;
                    
                    // And then the difference of lengths
                    var lengthChange = currentDiff.magnitude - lastDiff.magnitude;
                    
                    // And then adds the difference * scaling
                    Camera.orthographicSize += Time.deltaTime * zoomMultiplier * lengthChange * Camera.orthographicSize / 15;
                }
            } else {
                //If in Editor simply get the scrollWheel Axis and adds it to the size
                var scroll = Input.GetAxis("Mouse ScrollWheel");
                Camera.orthographicSize += Time.deltaTime * zoomMultiplier * -scroll * Camera.orthographicSize * 70;
            }

            // And then finally also clamp the size
            Camera.orthographicSize = Mathf.Clamp(Camera.orthographicSize, 2, 20);

            var parallaxSize = Camera.orthographicSize / 10;
            parallax.transform.localScale = new Vector3(parallaxSize, parallaxSize, parallaxSize);
        }
    }
}