namespace src.elements.effectors {
    
    /// <summary>
    /// A event container for its to execute function and its name
    /// </summary>
    public class EffectorEvent {

        public readonly string name;
        public readonly TriggerEffectorEvent effectorEvent;
        
        public EffectorEvent(string name, TriggerEffectorEvent effectorEvent) {
            this.name = name;
            this.effectorEvent = effectorEvent;
        }
        
    }

    /// <summary>
    /// The to execute effector function
    /// </summary>
    public delegate void TriggerEffectorEvent();
}