using System;
using src.time.time_managers;
using src.touch;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace src.misc {
    public class SpeedChangerUIController : TouchableRect<SpeedChangerUIController>, IPointerDownHandler {

        public RectTransform shaft;
        public Image fill;
        public TMP_Text speedText;
        
        protected override void Update() {
            base.Update();
            if (!hasPosition) return;

            var localPoint = RectPosition + rectTransform.sizeDelta / 2;
            var angle = Mathf.Atan2(localPoint.x, localPoint.y) * Mathf.Rad2Deg;
            angle = Mathf.Clamp(angle, 0, 90);
            shaft.rotation = Quaternion.Euler(0, 0, -angle+45);
            
            angle -= 45;
            var sign = angle > 0 ? -1 : 1;
            var speed = 0.00247f * angle * angle * sign;
            speedText.text = $"{speed:N1}x";

            fill.fillClockwise = sign < 0;

            fill.fillAmount = MathHelper.mapValue(Math.Abs(angle), 0, 45, 0, 0.125f);
            
            ReplayTimeManager.Instance.TimeMultiplier = speed;
        }
    }
}