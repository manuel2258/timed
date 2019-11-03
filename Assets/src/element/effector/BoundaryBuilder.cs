using src.misc;
using UnityEngine;

namespace src.element.effector {
    public class BoundaryBuilder : UnitySingleton<BoundaryBuilder> {
        
        [SerializeField] private float lineWidth;
        [SerializeField] private float lineHeight;

        [SerializeField] private GameObject linePrefab;

        private void Start() {
            linePrefab.transform.localScale = new Vector3(lineHeight, lineWidth);
        }

        public void buildCircle(Transform parent, float radius) {
            var fullSize = radius * 2 * Mathf.PI;
            var lineAmount = Mathf.CeilToInt(fullSize / (lineWidth * 4));

            var angleDif = 360 / lineAmount;
            for (int i = 0; i < lineAmount; i++) {
                var position = new Vector2(Mathf.Cos(angleDif * Mathf.Deg2Rad * i), Mathf.Sin(angleDif * Mathf.Deg2Rad * i)) * radius;
                var newRange = Instantiate(linePrefab, parent);
                newRange.transform.localPosition = position;
                newRange.transform.rotation = Quaternion.Euler(0, 0, angleDif * i);
            }
        }
    }
}