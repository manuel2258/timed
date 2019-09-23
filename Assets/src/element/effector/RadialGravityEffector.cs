using System;
using System.Collections.Generic;
using System.Linq;
using SpriteGlow;
using src.element.collider_body;
using src.level.parsing;
using src.simulation.reseting;
using src.time.timeline;
using UnityEngine;

namespace src.element.effector {
    public class RadialGravityEffector : BaseEffector, IResetable, IVisualStateAble {

        private float _radius = 4;

        class RadialGravityState : VisualState {
            public float force;
            public bool enabled;
            public ElementColor color;

            public bool colliderBodyInside;

            public RadialGravityState() { }

            public RadialGravityState(RadialGravityState that)  {
                force = that.force;
                enabled = that.enabled;
                color = that.color;
                colliderBodyInside = that.colliderBodyInside;
            }
        }

        private RadialGravityState _initialState;
        private RadialGravityState _currentState;

        private bool _invertAble = true;
        private bool _disableAble = true;

        public SpriteGlowEffect mainBody;

        public List<SpriteGlowEffect> colorChangeAbles;

        public GameObject forceSpriteParent;
        public GameObject push;
        public GameObject pull;

        public void setup(string force, string invertAble, string disableAble,
            string colors, string initialColor) {
            _initialState = new RadialGravityState {enabled = false};
            
            elementInfo.buildInfos();

            if (!float.TryParse(force, out _initialState.force)) {
                throw new Exception("RadialGravityEffector: Could not parse force argument -> " + force);
            }
            
            if (!bool.TryParse(invertAble, out _invertAble)) {
                throw new Exception("RadialGravityEffector: Could not parse invertAble argument -> " + invertAble);
            }
            
            if (!bool.TryParse(disableAble, out _disableAble)) {
                throw new Exception("RadialGravityEffector: Could not parse disableAble argument -> " + disableAble);
            }

            if (!Enum.TryParse(initialColor, out _initialState.color)) {
                throw new Exception("RadialGravityEffector: Could not parse initialColor argument -> " + initialColor);
            }

            if (_disableAble) {
                var eventInfo = elementInfo.getEventInfoBySearchTag("on_off");
                effectorEvents.Add(new EffectorEvent(eventInfo.icon,
                    () => { Elements.executeVisualChange(this, 
                        () => _currentState.enabled = !_currentState.enabled); }));
            }

            if (_invertAble) {
                var eventInfo = elementInfo.getEventInfoBySearchTag("invert");
                effectorEvents.Add(new EffectorEvent(eventInfo.icon,
                    () => {
                        var beforeState = new RadialGravityState(_currentState);
                        _currentState.force *= -1;
                        var afterState = new RadialGravityState(_currentState);
                        ReplayTimeline.Instance.addVisualEvent(this, beforeState, afterState);
                    }));
            }

            foreach (var color in ParseHelper.parseEnumListFromString<ElementColor>(colors)) {
                var eventInfo = elementInfo.getEventInfoBySearchTag("color_change_" + color.ToString().ToLower());
                effectorEvents.Add(new EffectorEvent(eventInfo.icon,
                    () => {
                        Elements.executeVisualChange(this, () => {
                            var savedColor = color;
                            _currentState.color = savedColor;
                        });
                    }));
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
            colorChangeAbles.ForEach(spriteRenderer => spriteRenderer.GlowColor = color);

            mainBody.GlowBrightness = gravityState.colliderBodyInside ? 3 : 2;
            mainBody.gameObject.transform.localScale =
                gravityState.colliderBodyInside ? new Vector3(1.5f, 1.5f) : new Vector3(1, 1);
        }

        protected override void effectorUpdate(decimal currentTime, decimal deltaTime) {
            var colliderBodyInside = false;
            var colliders = Physics2D.OverlapCircleAll(transform.position, _radius);
            foreach (var colliderBody in  Elements.filterForColor(colliders, _currentState.color)) {
                colliderBodyInside = true;
                if (!_currentState.enabled) continue;
                var diff = transform.position - colliderBody.transform.position;
                if (diff.magnitude > 0.25) {
                    var force = (-_currentState.force / 20 * diff.magnitude + _currentState.force) * (float)deltaTime;
                    colliderBody.rigidBody.AddForce(force * diff.normalized);
                }
            }

            if (colliderBodyInside != _currentState.colliderBodyInside) {
                Elements.executeVisualChange(this, () => _currentState.colliderBodyInside = colliderBodyInside);
            }
        }

        public VisualState getCurrentState() {
            return new RadialGravityState(_currentState);
        }

        public void reset() {
            _currentState = new RadialGravityState(_initialState);
        }
    }
}