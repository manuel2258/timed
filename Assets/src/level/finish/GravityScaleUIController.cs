using src.misc;
using TMPro;
using UnityEngine;

namespace src.level.finish {
    public class GravityScaleUIController : UnitySingleton<GravityScaleUIController> {

        public RectTransform direction;
        public TMP_Text scale;

        public void displayGravity(Vector2 gravity) {
            var multiplier = gravity.magnitude / 9.81f;
            scale.text = $"x {multiplier:0.0}";

            if (multiplier == 0) {
                direction.gameObject.SetActive(false);
            } else {
                var angle = Mathf.Atan2(gravity.y, gravity.x) * Mathf.Rad2Deg + 90;
                direction.rotation = Quaternion.Euler(0, 0, angle);
            }
        }
    }
}