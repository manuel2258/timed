using src.simulation;
using src.time.time_managers;
using UnityEngine;

namespace src.element.triggers {
    
    /// <summary>
    /// The base of each Effector
    /// </summary>
    public abstract class BaseTrigger : MonoBehaviour {

        protected virtual void Awake() {
            SimulationTimeManager.Instance.onNewTime += triggerUpdate;
            SimulationManager.Instance.onCalculationStarted += onCalculationStarted;
        }

        protected virtual void onCalculationStarted() { }
        
        protected abstract void triggerUpdate(decimal currentTime, decimal deltaTime);
        
    }
}