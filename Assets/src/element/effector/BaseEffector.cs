using System;
using System.Collections.Generic;
using src.element.collider_body;
using src.misc;
using src.simulation;
using src.time.time_managers;
using src.touch;
using UnityEngine;

namespace src.element.effector {
    
    /// <summary>
    /// The base of each Effector
    /// </summary>
    public abstract class BaseEffector : MonoBehaviour {
        
        public float touchHitBox = .5f;

        protected readonly List<EffectorEvent> effectorEvents = new List<EffectorEvent>();
        
        protected Collider2D[] collisionBuffer;

        protected virtual void Start() {
            SimulationTimeManager.Instance.onNewTime += effectorUpdate;
            SimulationManager.Instance.onCalculationStarted += onCalculationStarted;
        }

        protected virtual void onCalculationStarted() {
            collisionBuffer = new Collider2D[ColliderBodysInfo.Instance.ColliderBodyCount];
        }

        private void Update() {
            if (TouchManager.Instance.isTouched(transform.position, touchHitBox)) {
                EffectorPopupUIController.Instance.showEffector(this);
            }
        }

        public ICollection<EffectorEvent> getEffectorEvents() {
            return effectorEvents;
        }

        protected abstract void effectorUpdate(decimal currentTime, decimal deltaTime);

        public abstract string getEffectorName();
    }

    public class EffectorParseException : Exception {
        public EffectorParseException(string message) : base(message) { }
    }
}