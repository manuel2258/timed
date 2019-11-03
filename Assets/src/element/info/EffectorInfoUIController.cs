using System.Collections.Generic;
using src.misc;
using src.touch;
using src.translation;
using UnityEngine;
using UnityEngine.UI;

namespace src.element.info {
    public class EffectorInfoUIController : UnitySingleton<EffectorInfoUIController>, IStackAbleWindow {

        public Canvas helpCanvas;

        public TranslateAbleTMPText elementName;
        public TranslateAbleTMPText elementDescription;
        public Image elementIcon;

        public Transform eventParent;
        public GameObject eventContentPrefab;

        [SerializeField] private List<RectTransform> uiMask;

        private void Start() {
            helpCanvas.enabled = false;
        }

        public void toggleEffectorInfo(ElementInfo elementInfo) {
            UIWindowStack.Instance.toggleWindow(typeof(EffectorInfoUIController));
            elementName.translationTag = elementInfo.elementName;
            elementDescription.translationTag = elementInfo.helpText;
            elementName.translateText();
            elementDescription.translateText();
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
            if (newState) {
                UiMaskManager.Instance.addMasks(uiMask);
            } else {
                UiMaskManager.Instance.removeMasks(uiMask);
            }
        }

        public bool isActive() {
            return helpCanvas.enabled;
        }
    }
}