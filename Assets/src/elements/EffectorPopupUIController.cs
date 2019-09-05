using src.elements.effectors;
using src.misc;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;

namespace src.elements {
    
    /// <summary>
    /// A Singleton UI Controller that manages the Effector Event Selection
    /// </summary>
    public class EffectorPopupUIController : UnitySingleton<EffectorPopupUIController>, IPointerEnterHandler, IPointerExitHandler {

        private Canvas _canvas;

        private bool _pointerInside;
        
        /// <summary>
        /// The parent RectTransform that hosts the event children
        /// </summary>
        public RectTransform contentParent;
        
        /// <summary>
        /// The to Instantiate event children
        /// </summary>
        public GameObject contentPrefab;
        
        public TMP_Text effectorNameText;

        private bool _wasJustEnabled;

        private void Start() {
            _canvas = GetComponent<Canvas>();
            _canvas.enabled = false;
        }

        private void Update() {
            if (_wasJustEnabled) {
                _wasJustEnabled = false;
                return;
            }
            if ((Input.touchCount > 0 || Input.GetMouseButtonDown(0)) && !_pointerInside) {
                _canvas.enabled = false;
            }
        }

        /// <summary>
        /// Shows the name and the events of the effector
        /// </summary>
        /// <param name="effector">The to show Effector</param>
        public void showEffector(BaseEffector effector) {
            _wasJustEnabled = true;
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

        public void OnPointerEnter(PointerEventData eventData) {
            _pointerInside = true;
        }

        public void OnPointerExit(PointerEventData eventData) {
            _pointerInside = false;
        }
    }
}