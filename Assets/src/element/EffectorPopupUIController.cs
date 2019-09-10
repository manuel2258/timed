using src.element.effector;
using src.misc;
using src.touch;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace src.element {
    
    /// <summary>
    /// A Singleton UI Controller that manages the Effector Event Selection
    /// </summary>
    public class EffectorPopupUIController : UnitySingleton<EffectorPopupUIController> {

        private Canvas _canvas;

        /// <summary>
        /// The parent RectTransform that hosts the event children
        /// </summary>
        public RectTransform contentParent;
        
        /// <summary>
        /// The to Instantiate event children
        /// </summary>
        public GameObject contentPrefab;
        
        public TMP_Text effectorNameText;

        public Button closeButton;

        private void Start() {
            _canvas = GetComponent<Canvas>();
            _canvas.enabled = false;
            closeButton.onClick.AddListener(() => _canvas.enabled = false);
        }
        

        /// <summary>
        /// Shows the name and the events of the effector
        /// </summary>
        /// <param name="effector">The to show Effector</param>
        public void showEffector(BaseEffector effector) {
            _canvas.enabled = true;

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

            effectorNameText.text = effector.getEffectorName();
        }
    }
}