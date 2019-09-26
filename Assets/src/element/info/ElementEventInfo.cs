using UnityEngine;

namespace src.element.info {
    
    /// <summary>
    /// A Scriptable Object that provides information about an EffectorEvent
    /// </summary>
    public class ElementEventInfo : ScriptableObject {

        public Sprite icon;
        public string searchTag;
        public string eventName;
        
        [TextArea]
        public string helpText;

    }
}