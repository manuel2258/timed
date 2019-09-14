using src.element.effector;
using src.simulation.reseting;
using UnityEngine;

namespace src.element.collider_body {
    public class ColliderBody : MonoBehaviour, IResetable, IVisualStateAble {

        private Vector2 _initialPosition;
        private Quaternion _initialRotation;

        private VisualState _initialState;
        private VisualState _currentState;

        private Rigidbody2D _rigidbody;

        public SpriteRenderer colorMask;

        public void setup(ElementColor startColor) {
            _currentState = new VisualState {color = startColor};
            _initialState = new VisualState(_currentState);

            setVisualsByState(_currentState);
        }

        private void Start() {
            _initialPosition = transform.position;
            _initialRotation = transform.rotation;
            _rigidbody = GetComponent<Rigidbody2D>();
            
            setup(ElementColor.Yellow);
        }

        public void reset() {
            transform.position = _initialPosition;
            transform.rotation = _initialRotation;
            _currentState = new VisualState(_initialState);
            _rigidbody.velocity = Vector2.zero;
            _rigidbody.angularVelocity = 0;
        }

        public void setVisualsByState(VisualState state) {
            colorMask.color = ElementColors.getColorValue(state.color);
        }
    }
}