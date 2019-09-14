namespace src.element.effector {
    
    /// <summary>
    /// A event container for its to execute function and its name
    /// </summary>
    public class EffectorEvent {

        public readonly string name;
        public readonly TriggerEffectorEvent simulationEvent;

        public EffectorEvent(string name, TriggerEffectorEvent simulationEvent) {
            this.name = name;
            this.simulationEvent = simulationEvent;
        }
        
    }

    /// <summary>
    /// The to execute effector function
    /// </summary>
    public delegate void TriggerEffectorEvent();
}