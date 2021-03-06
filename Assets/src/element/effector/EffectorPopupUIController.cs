using System;
using src.element.effector.effectors;
using src.element.info;
using src.misc;
using src.translation;
using src.tutorial.check_events;
using UnityEngine;
using UnityEngine.UI;

namespace src.element.effector {
    
    /// <summary>
    /// A Singleton UI Controller that manages the Effector Event Selection
    /// </summary>
    public class EffectorPopupUIController : UnitySingleton<EffectorPopupUIController>, ICheckAbleEvent {

        private Canvas _canvas;

        /// <summary>
        /// The parent RectTransform that hosts the event children
        /// </summary>
        public RectTransform contentParent;
        
        /// <summary>
        /// The to Instantiate event children
        /// </summary>
        public GameObject contentPrefab;
        
        public TranslateAbleTMPText effectorNameText;

        public Button closeButton;

        public Button helpButton;
        
        private readonly CheckEventManager _checkEventManager = new CheckEventManager();
        public void registerEvent(string eventName, Action onEventChecked) {
            _checkEventManager.registerEvent(eventName, onEventChecked);
        }

        private void Start() {
            _canvas = GetComponent<Canvas>();
            _canvas.enabled = false;
            closeButton.onClick.AddListener(() => {
                _canvas.enabled = false;
                ElementHighlighter.Instance.deleteAllPositions();
                _checkEventManager.checkEvent("ClosedWindow");
            });
            
        }
        
        /// <summary>
        /// Shows the name and the events of the effector
        /// </summary>
        /// <param name="effector">The to show Effector</param>
        public void showEffector(BaseEffector effector) {
            _canvas.enabled = true;
            helpButton.onClick.RemoveAllListeners();
            helpButton.onClick.AddListener(() => {
                EffectorInfoUIController.Instance.toggleEffectorInfo(effector.elementInfo);
                _checkEventManager.checkEvent("ToggledInfo");
            });
 
            for (int i = 0; i < contentParent.transform.childCount; i++) {
                Destroy(contentParent.transform.GetChild(i).gameObject);
            }

            var effectorEvents = effector.getEffectorEvents();
            foreach (var effectorEvent in effectorEvents) {
                var newContentObject = Instantiate(contentPrefab, contentParent.transform);
                newContentObject.GetComponent<EffectorEventContentUIController>().setup(effectorEvent);
            }

            var contentParentAnchoredPosition = contentParent.anchoredPosition;
            contentParentAnchoredPosition.y = -contentParent.sizeDelta.y / 2;
            contentParent.anchoredPosition = contentParentAnchoredPosition;

            effectorNameText.translationTag = effector.elementInfo.elementName;
            effectorNameText.translateText();
        }
    }
}