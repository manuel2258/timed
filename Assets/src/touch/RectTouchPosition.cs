using src.misc;
using UnityEngine;
using UnityEngine.EventSystems;

namespace src.touch {
    public class TouchableRect<T> : UnitySingleton<T> where T : MonoBehaviour {

        protected RectTransform rectTransform;
        private bool _active;
        
        protected Vector2 RectPosition {
            private set;
            get;
        }

        protected bool hasPosition;

        protected virtual void Start() {
            rectTransform = transform as RectTransform;
        }

        protected virtual void Update() {
            if ((Input.touchCount > 0 || Input.GetMouseButton(0)) && _active) {
                RectTransformUtility.ScreenPointToLocalPointInRectangle(rectTransform, Input.mousePosition,
                    CameraManager.Camera, out var newPosition);
                RectPosition = newPosition;
                hasPosition = true;
            } else {
                hasPosition = false;
                _active = false;
            }
        }
        
        public void OnPointerDown(PointerEventData eventData) {
            _active = true;
        }
    }
}