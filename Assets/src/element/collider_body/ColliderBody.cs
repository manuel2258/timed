using System;
using SpriteGlow;
using src.element.effector.effectors;
using src.level.initializing;
using src.misc;
using src.simulation;
using src.simulation.reseting;
using src.time.time_managers;
using UnityEngine;

namespace src.element.collider_body {
    public class ColliderBody : MonoBehaviour, IResetable, IVisualStateAble, ISetupAble {

        private const float MAX_VELOCITY = 35;
        
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

        public Rigidbody2D Rigidbody { get; private set; }

        [SerializeField] private SpriteGlowEffect velocityGlow;
        [SerializeField] private GameObject velocityRotation;

        public SpriteGlowEffect colorMask;

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
            Rigidbody = GetComponent<Rigidbody2D>();

            if (!GlobalGameState.Instance.IsInGame) {
                Rigidbody.simulated = false;
            }

            setVisualsByState(_currentState);

            if (SimulationManager.Instance != null) {
                SimulationTimeManager.Instance.onNewTime += (currentTime, deltaTime) => {
                    if (Rigidbody.velocity.magnitude > MAX_VELOCITY) {
                        Rigidbody.velocity = Rigidbody.velocity.normalized * MAX_VELOCITY;
                    }
                };
            }
        }

        public void setVisualPosition(Vector3 position, Quaternion rotation, Vector2 velocity) {
            transform.rotation = _initialRotation;
            transform.position = position;
            colorMask.transform.rotation = rotation;
            velocityGlow.GlowBrightness = velocity.magnitude / 5;
            velocityRotation.transform.localScale = Vector3.one * velocity.magnitude / 10;
            var angle = Mathf.Atan2(velocity.y, velocity.x) * Mathf.Rad2Deg;
            velocityRotation.transform.rotation = Quaternion.Euler(0, 0, angle);
        }

        public void reset() {
            transform.position = _initialPosition;
            transform.rotation = _initialRotation;
            _currentState = new ColliderBodyState(_initialState);
            Rigidbody.velocity = Vector2.zero;
            Rigidbody.angularVelocity = 0;
        }

        public void setVisualsByState(VisualState state) {
            var color = ElementColors.getColorValue(((ColliderBodyState) state).color);
            colorMask.GlowColor = color;
            velocityGlow.GlowColor = color;
        }

        public VisualState getCurrentState() {
            return new ColliderBodyState(_currentState);
        }
    }
}