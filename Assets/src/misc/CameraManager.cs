using src.touch;
using UnityEngine;

namespace src.misc {
    public class CameraManager : UnitySingleton<CameraManager> {

        public float panningMultiplier;
        public float zoomMultiplier;
        
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
            var currentFrame = TouchManager.Instance.getFrameAtPosition(0);
            var lastFrame = TouchManager.Instance.getFrameAtPosition(1);
            if (currentFrame.WasTouched) {
                if (currentFrame.touchAmount() == 1 && lastFrame.touchAmount() == 1) {
                    var currentTouch = currentFrame.Touches[0];
                    var lastTouch = lastFrame.Touches[0];
                    if (currentTouch.inCameraArea && lastTouch.inCameraArea) {
                        var diff = currentTouch.screenPosition - lastTouch.screenPosition;
                        var newPosition = transform.position - Time.deltaTime * panningMultiplier * 
                                          Camera.orthographicSize / 10 * (Vector3) diff;
                        newPosition.x = Mathf.Clamp(newPosition.x, -35, 35);
                        newPosition.y = Mathf.Clamp(newPosition.y, -20, 20);
                        transform.position = newPosition;
                    }
                }
            }

            if (Input.touchSupported) {
                if (currentFrame.touchAmount() == 2 && lastFrame.touchAmount() == 2) {
                    var currentDiff = currentFrame.Touches[0].screenPosition - currentFrame.Touches[1].screenPosition;
                    var lastDiff = lastFrame.Touches[0].screenPosition - lastFrame.Touches[1].screenPosition;
                    var lengthChange = currentDiff.magnitude - lastDiff.magnitude;
                    Camera.orthographicSize += Time.deltaTime * zoomMultiplier * lengthChange;
                }
            } else {
                var scroll = Input.GetAxis("Mouse ScrollWheel");
                Camera.orthographicSize += Time.deltaTime * zoomMultiplier * -scroll * Camera.orthographicSize * 70;
            }

            Camera.orthographicSize = Mathf.Clamp(Camera.orthographicSize, 2, 30);
        }
    }
}