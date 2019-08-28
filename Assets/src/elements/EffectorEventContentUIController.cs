using src.elements.effectors;
using src.time;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace src.elements {
    
    /// <summary>
    /// A UI Controller representing a effectors event
    /// </summary>
    public class EffectorEventContentUIController : MonoBehaviour {

        public TMP_Text nameText;
        public Button addEventButton;

        /// <summary>
        /// Sets up the text and button for the given event
        /// </summary>
        /// <param name="effectorEvent"></param>
        public void setup(EffectorEvent effectorEvent) {
            nameText.text = effectorEvent.name;
            addEventButton.onClick.AddListener(() => {
                Timeline.Instance.addEffector(new TimedEffectorEvent(TimeManager.Instance.CurrentTime, 
                    effectorEvent.effectorEvent));
            });
        }

    }
}