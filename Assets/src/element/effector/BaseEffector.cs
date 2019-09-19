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

        protected virtual void Awake() {
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

        protected abstract void effectorUpdate(decimal currentTime, decimal deltaTime);

        public abstract string getEffectorName();
    }

    public interface IVisualStateAble {
        void setVisualsByState(VisualState state);

        VisualState getCurrentState();
    }

    public class VisualState {
    }
}