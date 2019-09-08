using System;
using src.time.time_managers;
using src.touch;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace src.misc {
    
    /// <summary>
    /// Singleton that manages the SpeedChangerUI Element
    /// </summary>
    public class SpeedChangerUIController : TouchableRect<SpeedChangerUIController>, IPointerDownHandler {

        /// <summary>
        /// The to rotate shaft
        /// </summary>
        public RectTransform shaft;
        
        /// <summary>
        /// The to radial fall UI Image
        /// </summary>
        public Image fill;
        
        /// <summary>
        /// The to text to display the current speed
        /// </summary>
        public TMP_Text speedText;
        
        protected override void Update() {
            base.Update();
            
            // Checks whether a touch was registered inside the Rectangle
            if (!hasPosition) return;

            // If so calculates the angle relative to the bottom left corner
            var localPoint = RectPosition + rectTransform.sizeDelta / 2;
            var angle = Mathf.Atan2(localPoint.x, localPoint.y) * Mathf.Rad2Deg;
            angle = Mathf.Clamp(angle, 0, 90);
            
            // And then updates the UI
            onNewSpeed(angle);
        }

        protected override void Start() {
            base.Start();
            onNewSpeed(25);
        }

        private void onNewSpeed(float angle) {
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