using System.Collections.Generic;
using Editor;
using UnityEngine;

namespace src.setting.ui {
    public class GroupUIController : MonoBehaviour {
        
        public List<ISettingsUIController> groupControllers;
        
        [field: SerializeField, LabelOverride("GroupTranslationTag")]
        public string GroupName { get; private set; }

        [SerializeField] private string groupName;
        
        private void Start() {
            foreach (var settingsUIController in groupControllers) {
                settingsUIController.setup(groupName);
            }
        }

        public void setActiveState(bool state) {
            gameObject.SetActive(state);
        }
    }
}
