using src.element;
using UnityEngine;

namespace src.level.initializing {
    public class WallInitializer : ElementInitializer {
        
        private readonly Vector2 _scale;

        public WallInitializer(Vector2 scale, ElementType elementType, int id, Vector2 position, float angle)
            : base(elementType, id, position, angle) {
            _scale = scale;
        }

        protected override void setTransform(GameObject currentGameObject) {
            currentGameObject.transform.localScale = _scale;
            base.setTransform(currentGameObject);
        }
    }
}