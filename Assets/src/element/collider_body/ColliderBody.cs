using System;
using src.element.effector;
using src.level.parsing;
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

        public void setup(string initialColor) {
            if (!Enum.TryParse(initialColor, out ElementColor parsedColor)) {
                throw new Exception("ColliderBody: Could not parse initialColor argument -> " + initialColor);
            }
            _currentState = new ColliderBodyState {color = parsedColor};
            _initialState = new ColliderBodyState(_currentState);

            _initialPosition = transform.position;
            _initialRotation = transform.rotation;
            rigidbody2D = GetComponent<Rigidbody2D>();
            
            setVisualsByState(_currentState);
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