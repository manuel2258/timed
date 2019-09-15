using System;
using System.Collections.Generic;
using src.element.collider_body;
using src.simulation.reseting;
using src.time.timeline;
using UnityEngine;

namespace src.element.effector {
    public class RadialGravityEffector : BaseEffector, IResetable, IVisualStateAble {

        private float _radius = 4;

        class RadialGravityState : VisualState {
            public float force;
            public bool enabled;

            public RadialGravityState() : base(ElementColor.Yellow) { }

            public RadialGravityState(RadialGravityState that) : base(that.color) {
                force = that.force;
                enabled = that.enabled;
            }
        }

        private RadialGravityState _initialState;
        private RadialGravityState _currentState;

        private bool _invertAble = true;
        private bool _disableAble = true;
        private bool _colorChangeAble = true;

        public List<SpriteRenderer> colorChangeAbles;

        public GameObject forceSpriteParent;
        public GameObject push;
        public GameObject pull;

        protected override void Start() {
            base.Start();
            setup("2000", "true", "true", "true",
                new List<ElementColor> {ElementColor.Yellow, ElementColor.Blue}, "Yellow");
        }

        public void setup(string force, string invertAble, string disableAble, string colorChangeAble,
            List<ElementColor> changeAbleColors, string initialColor) {
            _initialState = new RadialGravityState {enabled = false};

            if (!float.TryParse(force, out _initialState.force)) {
                throw new Exception("RadialGravityEffector: Could not parse force argument -> " + force);
            }
            
            if (!bool.TryParse(invertAble, out _invertAble)) {
                throw new Exception("RadialGravityEffector: Could not parse invertAble argument -> " + invertAble);
            }
            
            if (!bool.TryParse(disableAble, out _disableAble)) {
                throw new Exception("RadialGravityEffector: Could not parse disableAble argument -> " + disableAble);
            }

            if (!bool.TryParse(colorChangeAble, out _colorChangeAble)) {
                throw new Exception("RadialGravityEffector: Could not parse colorChangeAble argument -> " + colorChangeAble);
            }

            if (!Enum.TryParse(initialColor, out _initialState.color)) {
                throw new Exception("RadialGravityEffector: Could not parse initialColor argument -> " + initialColor);
            }

            if (_disableAble) {
                effectorEvents.Add(new EffectorEvent("Enable / Disable",
                    () => { executeVisualChange(this, () => _currentState.enabled = !_currentState.enabled); }));
            }

            if (_invertAble) {
                effectorEvents.Add(new EffectorEvent("Invert Force",
                    () => {
                        var beforeState = new RadialGravityState(_currentState);
                        _currentState.force *= -1;
                        var afterState = new RadialGravityState(_currentState);
                        ReplayTimeline.Instance.addVisualEvent(this, beforeState, afterState);
                    }));
            }

            if (_colorChangeAble) {
                foreach (var color in changeAbleColors) {
                    effectorEvents.Add(new EffectorEvent($"Colorchange: {color.ToString()}",
                        () => { executeVisualChange(this, () => {
                            var savedColor = color;
                            _currentState.color = savedColor;
                        }); }));
                }
            }

            _currentState = new RadialGravityState(_initialState);
            setVisualsByState(_currentState);
        }

        public void setVisualsByState(VisualState state) {
            var gravityState = (RadialGravityState) state;
            forceSpriteParent.SetActive(gravityState.enabled);
            
            var positiveSign = gravityState.force >= 0;
            pull.SetActive(positiveSign);
            push.SetActive(!positiveSign);
            
            var color = ElementColors.getColorValue(gravityState.color);
            colorChangeAbles.ForEach(spriteRenderer => spriteRenderer.color = color);
        }

        protected override void effectorUpdate(decimal currentTime, decimal deltaTime) {
            if (!_currentState.enabled) return;

            var colliders = Physics2D.OverlapCircleAll(transform.position, _radius);

            for (int i = 0; i < colliders.Length; i++) {
                var currentCollider = colliders[i];
                if(currentCollider == null) continue;
                var otherRigidBody = currentCollider.gameObject.GetComponent<Rigidbody2D>();
                if(otherRigidBody == null) continue;

                var diff = transform.position - otherRigidBody.transform.position;
                if (diff.magnitude > 0.25) {
                    var force = (-_currentState.force / 20 * diff.magnitude + _currentState.force) * (float)deltaTime;
                    otherRigidBody.AddForce(force * diff.normalized);
                }
            }
        }

        protected override VisualState getCurrentState() {
            return new RadialGravityState(_currentState);
        }

        public override string getEffectorName() {
            return "Radial Gravity Field";
        }

        public void reset() {
            _currentState = new RadialGravityState(_initialState);
        }
    }
}