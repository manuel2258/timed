using src.simulation.reseting;
using UnityEngine;

namespace src.elements {
    public class ColliderBody : MonoBehaviour, IResetable {

        private Vector2 _initialPosition;
        private Quaternion _initialRotation;
        private ColliderBodyColor _initialColor;

        public ColliderBodyColor bodyColor;

        private Rigidbody2D _rigidbody;

        private void Start() {
            _initialPosition = transform.position;
            _initialRotation = transform.rotation;
            _initialColor = bodyColor;
            _rigidbody = GetComponent<Rigidbody2D>();
        }

        public void reset() {
            transform.position = _initialPosition;
            transform.rotation = _initialRotation;
            bodyColor = _initialColor;
            _rigidbody.velocity = Vector2.zero;
        }
    }
}