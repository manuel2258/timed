using System;
using System.Collections.Generic;
using src.element.collider_body;
using src.simulation;
using src.time.time_managers;
using src.time.timeline;
using src.touch;
using UnityEngine;

namespace src.element.effector {
    
    /// <summary>
    /// The base of each Effector
    /// </summary>
    public abstract class BaseEffector : MonoBehaviour {
        
        public float touchHitBox = .5f;

        protected readonly List<EffectorEvent> effectorEvents = new List<EffectorEvent>();

        protected virtual void Start() {
            SimulationTimeManager.Instance.onNewTime += effectorUpdate;
            SimulationManager.Instance.onCalculationStarted += onCalculationStarted;
        }
        
        private void Update() {
            if (TouchManager.Instance.isTouched(transform.position, touchHitBox)) {
                EffectorPopupUIController.Instance.showEffector(this);
            }
        }

        protected virtual void onCalculationStarted() { }
        
        public ICollection<EffectorEvent> getEffectorEvents() {
            return effectorEvents;
        }

        protected void executeVisualChange(IVisualStateAble that, Action changeFunction) {
            var beforeState = getCurrentState();
            changeFunction.Invoke();
            var afterState = getCurrentState();
            ReplayTimeline.Instance.addVisualEvent(that, beforeState, afterState);
        }

        protected abstract void effectorUpdate(decimal currentTime, decimal deltaTime);

        protected abstract VisualState getCurrentState();

        public abstract string getEffectorName();
    }

    public interface IVisualStateAble {
        void setVisualsByState(VisualState state);
    }

    public class VisualState {
        public ElementColor color;

        public VisualState(ElementColor color) {
            this.color = color;
        }

        public VisualState() { }

        public VisualState(VisualState that) {
            color = that.color;
        }
    }
}