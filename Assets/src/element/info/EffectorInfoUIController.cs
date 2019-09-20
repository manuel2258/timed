using src.misc;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace src.element.info {
    public class EffectorInfoUIController : UnitySingleton<EffectorInfoUIController>, IStackAbleWindow {

        public Canvas helpCanvas;

        public TMP_Text elementName;
        public TMP_Text elementDescription;
        public Image elementIcon;

        public Transform eventParent;
        public GameObject eventContentPrefab;

        private void Start() {
            helpCanvas.enabled = false;
        }

        public void toggleEffectorInfo(ElementInfo elementInfo) {
            UIWindowStack.Instance.toggleWindow(typeof(EffectorInfoUIController));
            elementName.text = elementInfo.elementName;
            elementDescription.text = elementInfo.helpText;
            elementIcon.sprite = elementInfo.icon;

            for (int i = 0; i < eventParent.childCount; i++) {
                Destroy(eventParent.GetChild(i).gameObject);
            }
   
            foreach (var elementInfoElementEventInfo in elementInfo.elementEventInfos) {
                var newEventContent = Instantiate(eventContentPrefab, eventParent)
                    .GetComponent<EffectorEventInfoContentUIController>();
                newEventContent.setup(elementInfoElementEventInfo);
            }
        }

        public void setActive(bool newState) {
            helpCanvas.enabled = newState;
        }

        public bool isActive() {
            return helpCanvas.enabled;
        }
    }
}