using System;
using System.Collections.Generic;
using SpriteGlow;
using src.element.collider_body;
using src.level.parsing;
using src.simulation.reseting;
using UnityEngine;

namespace src.element.effector.effectors {
    public class ColorChangerEffector : BaseEffector, IResetable, IVisualStateAble {
        
        class ColorChangerState : VisualState {
            public ElementColor color;

            public ColorChangerState() { }

            public ColorChangerState(ColorChangerState that) {
                color = that.color;
            }
        }

        private ColorChangerState _initialState;
        private ColorChangerState _currentState;

        public List<SpriteGlowEffect> colorChangeAbles;
        private float _length;
        
        public Transform gate;

        public void setup(string colors, string initialColor, string length) {
            var argumentParser = new ArgumentParser("ColorChanger");

            _initialState = new ColorChangerState {color = argumentParser.TryParse<ElementColor>(initialColor, Enum.TryParse)};
            _length = argumentParser.TryParse<float>(length, float.TryParse);

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
            gate.localPosition = new Vector3(0, _length / 2);
            gate.localScale = new Vector3(gate.localScale.x, _length);
            
            _currentState = new ColorChangerState(_initialState);
            setVisualsByState(_currentState);
        }

        public void setVisualsByState(VisualState state) {
            var colorChangeState = (ColorChangerState) state;

            var color = ElementColors.getColorValue(colorChangeState.color);
            colorChangeAbles.ForEach(spriteRenderer => spriteRenderer.GlowColor = color);
        }

        protected override void effectorUpdate(decimal currentTime, decimal deltaTime) {
            var colliders = Physics2D.RaycastAll(transform.position, transform.up, _length);

            for (int i = 0; i < colliders.Length; i++) {
                var currentCollider = colliders[i];
                if(currentCollider.collider == null) continue;
                var colliderBody = currentCollider.collider.GetComponent<ColliderBody>();
                if(colliderBody == null) continue;
                colliderBody.Color = _currentState.color;
            }
        }

        public VisualState getCurrentState() {
            return new ColorChangerState(_currentState);
        }

        public void reset() {
            _currentState = new ColorChangerState(_initialState);
        }
    }
}