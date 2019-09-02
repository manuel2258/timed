using src.simulation;
using UnityEngine;

namespace src.elements.effectors {
    public class RadialGravityEffector : BaseEffector {

        private float _radius = 4;

        private float _force = 2500;
        private bool _enabled = true;

        private bool _invertAble = true;
        private bool _disableAble = true;

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

        private int _forceApplyCounter;
        
        protected override void effectorUpdate(float currentTime, float deltaTime) {
            if (!_enabled) return;

            Physics2D.OverlapCircleNonAlloc(transform.position, _radius, collisionBuffer);
            
            for (int i = 0; i < collisionBuffer.Length; i++) {
                var currentCollider = collisionBuffer[i];
                if(currentCollider == null) continue;
                var otherRigidBody = currentCollider.gameObject.GetComponent<Rigidbody2D>();
                if(otherRigidBody == null) continue;
                var diff = transform.position - otherRigidBody.gameObject.transform.position;
                _forceApplyCounter++;
                var force = _force * deltaTime;
                otherRigidBody.AddForce(diff.normalized / diff.magnitude * force);
            }
        }

        public override string getEffectorName() {
            return "Radial Gravity Field";
        }
    }
}