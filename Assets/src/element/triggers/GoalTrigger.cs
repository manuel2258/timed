using System;
using System.Collections.Generic;
using src.element.collider_body;
using src.element.effector;
using src.level;
using src.level.parsing;
using src.simulation;
using src.simulation.reseting;
using TMPro;
using UnityEngine;

namespace src.element.triggers {
    public class GoalTrigger : BaseTrigger, IResetable, IVisualStateAble {

        public Transform triggerArea;

        public SpriteRenderer area;
        public SpriteRenderer baseColorMask;

        public TMP_Text remaining;
        
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
            _color = ParseHelper.getElementColorFromString(color);
            
            _currentState = new GoalState();
            
            if (!int.TryParse(amount, out _currentState.remainingAmount)) {
                throw new Exception("GoalTrigger: Could not parse amount argument -> " + amount);
            }
            LevelFinishManager.Instance.registerGoal(this);
            
            _initialState = new GoalState(_currentState);
            setVisualsByState(_currentState);
        }
        
        protected override void triggerUpdate(decimal currentTime, decimal deltaTime) {
            if(currentTime <= SimulationManager.SIMULATION_STEPS) return;
            if(_currentState.remainingAmount <= 0) return;

            var colliders = Physics2D.OverlapBoxAll(triggerArea.position,triggerArea.localScale, 0);
            foreach (var colliderBody in Elements.filterForColor(colliders, _color)) {
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
                area.enabled = true;

                remaining.text = goalState.remainingAmount.ToString();
                var color = ElementColors.getColorValue(_color);
                baseColorMask.color = color;
                color.a = 100;
                area.color = color;
            } else {
                remaining.enabled = false;
                area.enabled = false;
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