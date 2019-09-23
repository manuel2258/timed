using System;
using System.Collections.Generic;
using SpriteGlow;
using src.element.collider_body;
using src.level.parsing;
using src.simulation.reseting;
using src.time.timeline;
using UnityEngine;

namespace src.element.effector {
    public class ColorChangerEffector : BaseEffector, IResetable, IVisualStateAble {

        private float _length = 4;

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

        public GameObject gate;

        public Sprite debug;
        
        public void setup(string colors, string initialColor) {
            _initialState = new ColorChangerState();

            if (!Enum.TryParse(initialColor, out _initialState.color)) {
                throw new Exception("RadialGravityEffector: Could not parse initialColor argument -> " + initialColor);
            }

            foreach (var color in ParseHelper.parseEnumListFromString<ElementColor>(colors)) {
                effectorEvents.Add(new EffectorEvent(debug,
                    () => {
                        Elements.executeVisualChange(this, () => {
                            var savedColor = color;
                            _currentState.color = savedColor;
                        });
                    }));
            }

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