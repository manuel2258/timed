using src.time.time_managers;
using src.touch;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;

namespace src.misc {
    public class SpeedChangerUIController : TouchableRect<SpeedChangerUIController>, IPointerDownHandler {

        public RectTransform shaft;
        public TMP_Text speedText;

        protected override void Update() {
            base.Update();
            if (!hasPosition) return;
            
            var localPoint = RectPosition + rectTransform.sizeDelta / 2;
            var angle = Mathf.Atan2(localPoint.x, localPoint.y) * Mathf.Rad2Deg;
            angle = Mathf.Clamp(angle, 0, 90);
            shaft.rotation = Quaternion.Euler(0, 0, -angle);
            angle -= 45;
            var sign = angle > 0 ? -1 : 1;
            var speed = 0.00247f * angle * angle * sign;
            speedText.text = $"{speed:N1}x";
            ReplayTimeManager.Instance.TimeMultiplier = speed;
        }
    }
}