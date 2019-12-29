using System.Collections.Generic;
using src.touch;
using src.translation;
using UnityEngine;

namespace src.setting.ui {
    public class SettingsUIController : MonoBehaviour {

        [SerializeField] private List<GroupUIController> groups;
        [SerializeField] private TranslateAbleTMPText groupNameText;
        
        private int _groupIndex;
        private RectTransform _rectTransform;
        private Canvas _canvas;

        private void Start() {
            _rectTransform = GetComponent<RectTransform>();
            _canvas = GetComponent<Canvas>();
            changeGroup(0);
        }
        
        public void changeGroup(int diff) {
            _groupIndex += diff;
            if (_groupIndex >= groups.Count) _groupIndex = 0;
            if (_groupIndex < 0) _groupIndex = groups.Count-1;
            foreach (var groupUIController in groups) {
                groupUIController.setActiveState(false);
            }

            var active = groups[_groupIndex];
            active.setActiveState(true);
            groupNameText.translationTag = active.GroupName;
            groupNameText.translateText();
        }

        public void setActiveState(bool activeState) {
            if (activeState) {
                UiMaskManager.Instance.addMask(_rectTransform);
            } else {
                UiMaskManager.Instance.removeMask(_rectTransform);
            }

            _canvas.enabled = activeState;
        }

    }
}