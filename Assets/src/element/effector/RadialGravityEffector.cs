using src.simulation.reseting;
using UnityEngine;

namespace src.element.effector {
    public class RadialGravityEffector : BaseEffector, IResetable {

        private float _radius = 4;

        private float _force ;
        private bool _enabled;

        private float _initialForce;
        private bool _initialEnabled;
        
        private bool _invertAble = true;
        private bool _disableAble = true;

        protected override void Start() {
            base.Start();
            setup("10000", "true", "true");
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
            
            _initialForce = _force;
            _initialEnabled = _enabled;
        }

        protected override void effectorUpdate(decimal currentTime, decimal deltaTime) {
            if (!_enabled) return;

            Physics2D.OverlapCircleNonAlloc(transform.position, _radius, collisionBuffer);
            
            for (int i = 0; i < collisionBuffer.Length; i++) {
                var currentCollider = collisionBuffer[i];
                if(currentCollider == null) continue;
                var otherRigidBody = currentCollider.gameObject.GetComponent<Rigidbody2D>();
                if(otherRigidBody == null) continue;
                var diff = transform.position - otherRigidBody.gameObject.transform.position;
                otherRigidBody.AddForce(diff.normalized * _force * (float)deltaTime / diff.magnitude);
            }
        }

        public override string getEffectorName() {
            return "Radial Gravity Field";
        }

        public void reset() {
            _force = _initialForce;
            _enabled = _initialEnabled;
        }
    }
}