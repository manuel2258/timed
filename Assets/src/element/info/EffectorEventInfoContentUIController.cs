using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace src.element.info {
    public class EffectorEventInfoContentUIController : MonoBehaviour {
        public TMP_Text eventName;
        public TMP_Text eventDescription;
        public Image eventIcon;
        
        public void setup(ElementEventInfo eventInfo) {
            eventName.text = eventInfo.eventName;
            eventDescription.text = eventInfo.helpText;
            eventIcon.sprite = eventInfo.icon;
        }
    }
}