using System.Collections.Generic;
using System.Collections.ObjectModel;
using src.element.info;
using src.misc;
using src.simulation;
using src.time.time_managers;
using src.touch;
using UnityEngine;

namespace src.element.effector.effectors {
    
    /// <summary>
    /// The base of each Effector
    /// </summary>
    public abstract class BaseEffector : MonoBehaviour {
        
        public float touchHitBox = .5f;

        public ElementInfo elementInfo;

        protected readonly List<EffectorEvent> effectorEvents = new List<EffectorEvent>();

        protected virtual void Awake() {
            if (GlobalGameState.Instance.IsInGame) {
                SimulationTimeManager.Instance.onNewTime += effectorUpdate;
                SimulationManager.Instance.onCalculationStarted += onCalculationStarted;
            }
        }
        
        private void Update() {
            if (TouchManager.Instance.isTouched(transform.position, touchHitBox)) {
                EffectorPopupUIController.Instance.showEffector(this);
                onTouched();
            }
        }

        protected virtual void onCalculationStarted() { }

        protected virtual void onTouched() {
            ElementHighlighter.Instance.displayPositions(new Collection<Vector2> {transform.position});
        }

        public ICollection<EffectorEvent> getEffectorEvents() {
            return effectorEvents;
        }

        protected abstract void effectorUpdate(decimal currentTime, decimal deltaTime);
    }

    public interface IVisualStateAble {
        void setVisualsByState(VisualState state);

        VisualState getCurrentState();
    }

    public class VisualState {
    }
}