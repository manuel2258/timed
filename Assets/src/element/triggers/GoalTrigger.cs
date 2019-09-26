using System;
using System.Collections.Generic;
using SpriteGlow;
using src.element.collider_body;
using src.element.effector.effectors;
using src.level.finish;
using src.level.parsing;
using src.misc;
using src.simulation;
using src.simulation.reseting;
using UnityEngine;

namespace src.element.triggers {
    public class GoalTrigger : BaseTrigger, IResetable, IVisualStateAble {

        public Transform triggerArea;

        public SpriteRenderer remaining;
        
        public List<SpriteGlowEffect> colorChangeAbles = new List<SpriteGlowEffect>();

        class GoalState : VisualState {
            public int remainingAmount;

            public GoalState() { }

            public GoalState(GoalState that)  {
                remainingAmount = that.remainingAmount;
            }
        }

        private ElementColor _color;

        private GoalState _currentState;
        private GoalState _initialState;

        private readonly List<ColliderBody> _alreadyCounted = new List<ColliderBody>();

        public void setup(string color, string amount) {
            onSetup();
            _color = ParseHelper.getElementColorFromString(color);
            
            _currentState = new GoalState();
            
            if (!int.TryParse(amount, out _currentState.remainingAmount)) {
                throw new Exception("GoalTrigger: Could not parse amount argument -> " + amount);
            }

            if (GlobalGameState.Instance.IsInGame) {
                LevelFinishManager.Instance.registerGoal(this);
            }

            _initialState = new GoalState(_currentState);
            setVisualsByState(_currentState);
        }
        
        protected override void triggerUpdate(decimal currentTime, decimal deltaTime) {
            if(currentTime <= SimulationManager.SIMULATION_STEPS) return;
            if(_currentState.remainingAmount <= 0) return;

            var colliders = Physics2D.OverlapBoxAll(triggerArea.position,triggerArea.localScale, 0);
            foreach (var colliderBody in Elements.filterForColorFromColliders(colliders, _color)) {
                if (_alreadyCounted.Contains(colliderBody)) continue;
                
                Elements.executeVisualChange(this, () => _currentState.remainingAmount--);
                _alreadyCounted.Add(colliderBody);
            }

            if (_currentState.remainingAmount <= 0) {
                LevelFinishManager.Instance.onGoalFinished(this);
            }
        }

        public void setVisualsByState(VisualState state) {
            var goalState = (GoalState) state;
            if (goalState.remainingAmount > 0) {
                remaining.enabled = true;

                remaining.sprite = IntToSpriteImageStorage.Instance.getSpriteByInt(goalState.remainingAmount);
                var color = ElementColors.getColorValue(_color);
                colorChangeAbles.ForEach(colorChangeAble => colorChangeAble.GlowColor = color);
            } else {
                remaining.enabled = false;
            }
        }

        public VisualState getCurrentState() {
            return new GoalState(_currentState);
        }

        public void reset() {
            _alreadyCounted.Clear();
            _currentState = new GoalState(_initialState);
        }
    }
}