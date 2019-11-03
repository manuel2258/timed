using System;
using System.Collections.Generic;
using SpriteGlow;
using src.level.parsing;
using src.simulation.reseting;
using src.time.timeline;
using UnityEngine;

namespace src.element.effector.effectors {
    public class RadialGravityEffector : BaseEffector, IResetable, IVisualStateAble {

        

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
        private float _radius;

        public SpriteGlowEffect mainBody;

        public List<SpriteGlowEffect> colorChangeAbles;
        public List<SpriteRenderer> range;

        public Sprite off;
        public Sprite pull;
        public Sprite push;

        public Transform rangeChildren;

        private readonly ArgumentParser _argumentParser = new ArgumentParser("RadialGravityEffector");

        public void setup(string force, string invertAble, string disableAble, string colors, string initialColor, string radius) {
            _initialState = new RadialGravityState {enabled = false};
            elementInfo.buildInfos();

            _initialState.force = _argumentParser.TryParse<float>(force, float.TryParse);
            _invertAble = _argumentParser.TryParse<bool>(invertAble, bool.TryParse);
            _disableAble = _argumentParser.TryParse<bool>(disableAble, bool.TryParse);
            _initialState.color = _argumentParser.TryParse<ElementColor>(initialColor, Enum.TryParse);
            _radius = _argumentParser.TryParse<float>(radius, float.TryParse);
            
            BoundaryBuilder.Instance.buildCircle(rangeChildren, _radius);
            for (int i = 0; i < rangeChildren.childCount; i++) {
                colorChangeAbles.Add(rangeChildren.GetChild(i).GetComponent<SpriteGlowEffect>());
                range.Add(rangeChildren.GetChild(i).GetComponent<SpriteRenderer>());
            }

            if (_disableAble) {
                var eventInfo = elementInfo.getEventInfoBySearchTag("on_off");
                effectorEvents.Add(new EffectorEvent(eventInfo.icon,
                    () => { Elements.executeVisualChange(this, 
                        () => _currentState.enabled = !_currentState.enabled); 
                        checkEventManager.checkEvent("AddedEventOn/Off");
                    }));
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

            var positiveSign = gravityState.force >= 0;
            if (!gravityState.enabled) {
                range.ForEach(sprite => sprite.sprite = off);
            } else {
                range.ForEach(sprite => sprite.sprite = positiveSign? pull : push);
            }

            var color = ElementColors.getColorValue(gravityState.color);
            colorChangeAbles.ForEach(spriteRenderer => spriteRenderer.GlowColor = color);

            mainBody.GlowBrightness = gravityState.colliderBodyInside ? 3 : 2;
            mainBody.gameObject.transform.localScale =
                gravityState.colliderBodyInside ? new Vector3(1.5f, 1.5f) : new Vector3(1, 1);
        }

        protected override void effectorUpdate(decimal currentTime, decimal deltaTime) {
            var colliderBodyInside = false;
            var colliders = Physics2D.OverlapCircleAll(transform.position, _radius);
            foreach (var colliderBody in Elements.filterForColorFromColliders(colliders, _currentState.color)) {
                colliderBodyInside = true;
                if (!_currentState.enabled) continue;
                var diff = transform.position - colliderBody.transform.position;
                var force = (_radius - diff.magnitude) / _radius;
                if (force <= 0f) continue;
                force = Mathf.Sqrt(force);
                force *= _currentState.force * (float) deltaTime;
                colliderBody.Rigidbody.AddForce(force * diff.normalized);
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