using src.time.time_managers;
using src.time.timeline;
using UnityEngine;
using UnityEngine.UI;

namespace src.element.effector {
    
    /// <summary>
    /// A UI Controller representing a effectors event
    /// </summary>
    public class EffectorEventContentUIController : MonoBehaviour {
        
        public Button addEventButton;

        /// <summary>
        /// Sets up the text and button for the given event
        /// </summary>
        /// <param name="effectorEvent"></param>
        public void setup(EffectorEvent effectorEvent) {
            addEventButton.image.sprite = effectorEvent.icon;
            addEventButton.onClick.AddListener(() => {
                TimedEffectorEventEditManager.Instance.exitEditing();
                Timeline.Instance.addEffectorEvent(new TimedEffectorEvent(ReplayTimeManager.Instance.CurrentTime, 
                    effectorEvent));
            });
        }

    }
}