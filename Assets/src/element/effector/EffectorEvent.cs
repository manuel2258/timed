using UnityEngine;

namespace src.element.effector {
    
    /// <summary>
    /// A event container for its to execute function and its name
    /// </summary>
    public class EffectorEvent {

        public readonly Sprite icon;
        public readonly TriggerEffectorEvent simulationEvent;

        public EffectorEvent(Sprite icon, TriggerEffectorEvent simulationEvent) {
            this.icon = icon;
            this.simulationEvent = simulationEvent;
        }
        
    }

    /// <summary>
    /// The to execute effector function
    /// </summary>
    public delegate void TriggerEffectorEvent();
}