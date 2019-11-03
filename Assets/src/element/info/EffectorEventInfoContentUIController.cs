using src.translation;
using UnityEngine;
using UnityEngine.UI;

namespace src.element.info {
    public class EffectorEventInfoContentUIController : MonoBehaviour {
        public TranslateAbleTMPText eventName;
        public TranslateAbleTMPText eventDescription;
        public Image eventIcon;
        
        public void setup(ElementEventInfo eventInfo) {
            eventName.translationTag = eventInfo.eventName;
            eventDescription.translationTag = eventInfo.helpText;
            eventName.translateText();
            eventDescription.translateText();
            eventIcon.sprite = eventInfo.icon;
        }
    }
}