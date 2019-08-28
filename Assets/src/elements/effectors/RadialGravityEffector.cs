using System;
using System.Collections.Generic;
using UnityEngine;

namespace src.elements.effectors {
    public class RadialGravityEffector : BaseEffector {

        private float _force = 2500;
        private bool _enabled = true;

        private bool _invertAble = true;
        private bool _disableAble = true;

        private readonly List<Rigidbody2D> _inRangeGameObjects = new List<Rigidbody2D>();

        protected override void Start() {
            base.Start();
            setup("2500", "true", "true");
        }

        public void setup(string force, string invertAble, string disableAble) {
            if (!float.TryParse(force, out _force)) {
                throw new EffectorParseException("RadialGravityEffector: Could not parse force argument -> " + force);
            }
            
            if (!bool.TryParse(invertAble, out _invertAble)) {
                throw new EffectorParseException("RadialGravityEffector: Could not parse invertAble argument -> " + invertAble);
            }
            
            if (!bool.TryParse(disableAble, out _disableAble)) {
                throw new EffectorParseException("RadialGravityEffector: Could not parse disableAble argument -> " + disableAble);
            }
            
            if(_disableAble)
                effectorEvents.Add(new EffectorEvent("Enable / Disable", () => _enabled = !_enabled));
            
            if(_invertAble)
                effectorEvents.Add(new EffectorEvent("Invert Force", () => _force *= -1));
        }

        private void OnTriggerEnter2D(Collider2D other) {
            var otherRigidBody = other.GetComponent<Rigidbody2D>();
            if (!_inRangeGameObjects.Contains(otherRigidBody)) {
                _inRangeGameObjects.Add(otherRigidBody);
            }
        }

        private void OnTriggerExit2D(Collider2D other) {
            var otherRigidBody = other.GetComponent<Rigidbody2D>();
            _inRangeGameObjects.Remove(otherRigidBody);
        }

        protected override void effectorUpdate(float currentTime, float deltaTime) {
            if (!_enabled) return;
            foreach (var rigidbody in _inRangeGameObjects) {
                var diff = transform.position - rigidbody.gameObject.transform.position;
                rigidbody.AddForce(diff.normalized / diff.magnitude * _force * deltaTime);
            }
        }

        public override string getEffectorName() {
            return "Radial Gravity Field";
        }
    }
}