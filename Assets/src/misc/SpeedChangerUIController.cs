using src.time.time_managers;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;

namespace src.misc {
    public class SpeedChangerUIController : UnitySingleton<SpeedChangerUIController>, IPointerDownHandler {

        public RectTransform shaft;
        public TMP_Text speedText;
        
        private RectTransform _rectTransform;

        private bool _active;

        private void Start() {
            _rectTransform = transform as RectTransform;
        }

        private void Update() {
            if ((Input.touchCount > 0 || Input.GetMouseButton(0)) && _active) {
                RectTransformUtility.ScreenPointToLocalPointInRectangle(_rectTransform, Input.mousePosition,
                    CameraManager.Camera, out var localPoint);
                localPoint += _rectTransform.sizeDelta / 2;
                var angle = Mathf.Atan2(localPoint.x, localPoint.y) * Mathf.Rad2Deg;
                angle = Mathf.Clamp(angle, 0, 90);
                shaft.rotation = Quaternion.Euler(0,0, -angle);
                angle -= 45;
                var sign = angle > 0 ? -1 : 1;
                var speed = 0.00247f * angle * angle * sign;
                speedText.text =  $"{speed:N1}x";
                ReplayTimeManager.Instance.TimeMultiplier = speed;
            } else {
                _active = false;
            }
        }

        public void OnPointerDown(PointerEventData eventData) {
            _active = true;
        }
    }
}