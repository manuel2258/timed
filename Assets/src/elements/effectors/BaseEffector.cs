using System;
using System.Collections.Generic;
using src.misc;
using src.time;
using UnityEngine;

namespace src.elements.effectors {
    
    /// <summary>
    /// The base of each Effector
    /// </summary>
    public abstract class BaseEffector : MonoBehaviour {
        
        public float touchHitBox = .5f;

        protected readonly List<EffectorEvent> effectorEvents = new List<EffectorEvent>();

        protected virtual void Start() {
            TimeManager.Instance.onNewTime += effectorUpdate;
        }

        private void Update() {
            if (TouchManager.Instance.isTouched(transform.position, touchHitBox)) {
                EffectorPopupUIController.Instance.showEffector(this);
            }
        }

        public ICollection<EffectorEvent> getEffectorEvents() {
            return effectorEvents;
        }

        protected abstract void effectorUpdate(float currentTime, float deltaTime);

        public abstract string getEffectorName();
    }

    public class EffectorParseException : Exception {
        public EffectorParseException(string message) : base(message) { }
    }
}