using src.element.effector;
using src.simulation.reseting;
using UnityEngine;

namespace src.element.collider_body {
    public class ColliderBody : MonoBehaviour, IResetable, IVisualStateAble {
        
        class ColliderBodyState : VisualState {
            public ElementColor color;

            public ColliderBodyState() { }

            public ColliderBodyState(ColliderBodyState that)  {
                color = that.color;
            }
        }

        private Vector2 _initialPosition;
        private Quaternion _initialRotation;

        private ColliderBodyState _initialState;
        private ColliderBodyState _currentState;

        public Rigidbody2D rigidbody2D;

        public SpriteRenderer colorMask;

        public ElementColor Color {
            get => _currentState.color;
            set {
                Elements.executeVisualChange(this, () => _currentState.color = value);
            }
        }

        public void setup(ElementColor startColor) {
            _currentState = new ColliderBodyState {color = startColor};
            _initialState = new ColliderBodyState(_currentState);

            setVisualsByState(_currentState);
        }

        private void Start() {
            _initialPosition = transform.position;
            _initialRotation = transform.rotation;
            rigidbody2D = GetComponent<Rigidbody2D>();
            
            setup(ElementColor.Yellow);
        }

        public void reset() {
            transform.position = _initialPosition;
            transform.rotation = _initialRotation;
            _currentState = new ColliderBodyState(_initialState);
            rigidbody2D.velocity = Vector2.zero;
            rigidbody2D.angularVelocity = 0;
        }

        public void setVisualsByState(VisualState state) {
            colorMask.color = ElementColors.getColorValue(((ColliderBodyState)state).color);
        }

        public VisualState getCurrentState() {
            return new ColliderBodyState(_currentState);
        }
    }
}