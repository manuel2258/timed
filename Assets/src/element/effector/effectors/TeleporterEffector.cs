using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using SpriteGlow;
using src.level.parsing;
using src.simulation.reseting;
using UnityEngine;

namespace src.element.effector.effectors {
    public class TeleporterEffector : BaseEffector, IResetable, IVisualStateAble {

        private float _length = 4;

        class TeleporterState : VisualState {
            public bool enabled;
            public ElementColor color;

            public TeleporterState() { }

            public TeleporterState(TeleporterState that)  {
                enabled = that.enabled;
                color = that.color;
            }
        }

        private TeleporterState _initialState;
        private TeleporterState _currentState;
        
        private Vector3 _difference;
        private float _differenceAngle;
        private bool _disableAble = true;

        [SerializeField] private Transform outputTransform;

        public List<SpriteGlowEffect> colorChangeAbles;

        public void setup(string disableAble, string initialEnabled, string colors, string initialColor, 
            string differenceX, string differenceY, string differenceAngle) {
            _initialState = new TeleporterState {enabled = false};
            
            elementInfo.buildInfos();
            
            if (!bool.TryParse(disableAble, out _disableAble)) {
                throw new Exception("RadialGravityEffector: Could not parse disableAble argument -> " + disableAble);
            }
            
            if (!bool.TryParse(initialEnabled, out _initialState.enabled)) {
                throw new Exception("RadialGravityEffector: Could not parse initialEnabled argument -> " + initialEnabled);
            }

            if (!Enum.TryParse(initialColor, out _initialState.color)) {
                throw new Exception("RadialGravityEffector: Could not parse initialColor argument -> " + initialColor);
            }
            
            _difference = new Vector2(0, 0);

            if (!float.TryParse(differenceX, out _difference.x)) {
                throw new Exception("RadialGravityEffector: Could not parse differenceX argument -> " + differenceX);
            }
            
            if (!float.TryParse(differenceY, out _difference.y)) {
                throw new Exception("RadialGravityEffector: Could not parse differenceY argument -> " + differenceY);
            }
            
            if (!float.TryParse(differenceAngle, out _differenceAngle)) {
                throw new Exception("RadialGravityEffector: Could not parse differenceAngle argument -> " + differenceAngle);
            }

            if (_disableAble) {
                var eventInfo = elementInfo.getEventInfoBySearchTag("on_off");
                effectorEvents.Add(new EffectorEvent(eventInfo.icon,
                    () => { 
                        Elements.executeVisualChange(this, 
                        () => _currentState.enabled = !_currentState.enabled);
                        checkEventManager.checkEvent("AddedEvent" + eventInfo.eventName);
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
                        checkEventManager.checkEvent("AddedEvent" + eventInfo.eventName);
                    }));
            }
            _currentState = new TeleporterState(_initialState);
            
            outputTransform.position = transform.position + _difference;
            outputTransform.localRotation = Quaternion.Euler(0, 0, _differenceAngle);

            //_differenceAngle += transform.eulerAngles.z;

            setVisualsByState(_currentState);
        }

        public void setVisualsByState(VisualState state) {
            var teleporterState = (TeleporterState) state;

            var color = ElementColors.getColorValue(teleporterState.color);
            colorChangeAbles.ForEach(spriteRenderer => {
                spriteRenderer.Renderer.enabled = teleporterState.enabled;
                spriteRenderer.GlowColor = color;
            });
        }

        protected override void effectorUpdate(decimal currentTime, decimal deltaTime) {
            if (!_currentState.enabled) return;
            
            var colliders = Physics2D.RaycastAll(transform.position - transform.up * _length / 2, transform.up, _length);

            foreach (var colliderBody in Elements.filterForColorFromRaycastHits(colliders, _currentState.color)) {
                var positionDifference = transform.position - colliderBody.transform.position;
                var rotatedDifference = Quaternion.Euler(0, 0, _differenceAngle + transform.eulerAngles.z) * positionDifference;

                colliderBody.transform.position += _difference;
                colliderBody.transform.position += rotatedDifference;

                colliderBody.Rigidbody.velocity = Quaternion.Euler(0, 0, _differenceAngle) * colliderBody.Rigidbody.velocity;
            }
        }

        protected override void onTouched() {
            ElementHighlighter.Instance.displayPositions(new Collection<Vector2> {transform.position, transform.position + _difference});
        }

        public VisualState getCurrentState() {
            return new TeleporterState(_currentState);
        }

        public void reset() {
            _currentState = new TeleporterState(_initialState);
        }
    }
}