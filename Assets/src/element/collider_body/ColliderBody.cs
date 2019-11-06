using System;
using SpriteGlow;
using src.element.effector;
using src.element.effector.effectors;
using src.level.initializing;
using src.misc;
using src.simulation;
using src.simulation.reseting;
using src.time.time_managers;
using UnityEngine;

namespace src.element.collider_body {
    
    /// <summary>
    /// Main Mono Class for Initializing and Managing a single ColliderBody
    /// </summary>
    public class ColliderBody : MonoBehaviour, IResetable, IVisualStateAble, ISetupAble {

        /// <summary>
        /// Simple constant that caps the max possible velocity
        /// </summary>
        private const float MAX_VELOCITY = 35;
        
        /// <summary>
        /// The visual state of the ColliderBody
        /// </summary>
        class ColliderBodyState : VisualState {
            public ElementColor color;

            public ColliderBodyState() { }

            public ColliderBodyState(ColliderBodyState that)  {
                color = that.color;
            }
        }

        /// <summary>
        /// The start position
        /// </summary>
        private Vector2 _initialPosition;
        
        /// <summary>
        /// The start rotation
        /// </summary>
        private Quaternion _initialRotation;

        /// <summary>
        /// The state the ColliderBody has after the setup function
        /// </summary>
        private ColliderBodyState _initialState;
        
        /// <summary>
        /// The current state used while Simulating
        /// </summary>
        private ColliderBodyState _currentState;
        
        public Rigidbody2D Rigidbody { get; private set; }

        [Tooltip("The SpriteGlowEffect of the velocity arrow")]
        [SerializeField] private SpriteGlowEffect velocityGlow;
        [Tooltip("The RotationGameObject of the velocity arrow")]
        [SerializeField] private GameObject velocityRotation;
        [Tooltip("The ColliderBodys body")]
        [SerializeField] private SpriteGlowEffect bodyGlow;

        public ElementColor Color {
            get => _currentState.color;
            set {
                Elements.executeVisualChange(this, () => _currentState.color = value);
            }
        }

        /// <summary>
        /// Setup function called via Reflection
        /// </summary>
        /// <param name="initialColor">The start color</param>
        /// <exception cref="Exception"></exception>
        public void setup(string initialColor) {
            var argumentParser = new ArgumentParser("ColliderBody");
            _currentState = new ColliderBodyState {color = argumentParser.TryParse<ElementColor>(initialColor, Enum.TryParse)};
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

        /// <summary>
        /// Sets the visual state (position/rotation/velocity) of the ColliderBody
        /// </summary>
        /// <param name="position">The to display position</param>
        /// <param name="rotation">The to display rotation</param>
        /// <param name="velocity">The to display velocity</param>
        public void setVisualPosition(Vector3 position, Quaternion rotation, Vector2 velocity) {
            transform.rotation = _initialRotation;
            transform.position = position;
            bodyGlow.transform.rotation = rotation;
            velocityGlow.GlowBrightness = velocity.magnitude / 5;
            velocityRotation.transform.localScale = Vector3.one * velocity.magnitude / 10;
            var angle = Mathf.Atan2(velocity.y, velocity.x) * Mathf.Rad2Deg;
            velocityRotation.transform.rotation = Quaternion.Euler(0, 0, angle);
        }

        /// <summary>
        /// Resets the ColliderBody to its initial state
        /// </summary>
        public void reset() {
            transform.position = _initialPosition;
            transform.rotation = _initialRotation;
            _currentState = new ColliderBodyState(_initialState);
            Rigidbody.velocity = Vector2.zero;
            Rigidbody.angularVelocity = 0;
        }

        /// <summary>
        /// Displays the current ColliderBodys state
        /// </summary>
        /// <param name="state"></param>
        public void setVisualsByState(VisualState state) {
            var color = ElementColors.getColorValue(((ColliderBodyState) state).color);
            bodyGlow.GlowColor = color;
            velocityGlow.GlowColor = color;
        }

        public VisualState getCurrentState() {
            return new ColliderBodyState(_currentState);
        }
    }
}