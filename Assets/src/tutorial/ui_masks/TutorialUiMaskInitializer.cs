using System;
using System.Collections.Generic;
using src.level.initializing;
using UnityEngine;

namespace src.tutorial.ui_masks {
    public class TutorialUiMaskInitializer : IPartElement {
        
        private readonly Dictionary<string, string> _parameters;
        private GameObject _gameObject;
        
        public TutorialUiMaskInitializer(Dictionary<string, string> parameters) {
            _parameters = parameters;
        }

        public void initialize(Transform rectParent, Transform worldParent) {
            _gameObject = TutorialPrefabStorage.Instance.getUiMask();
            _gameObject.transform.SetParent(rectParent);

            InitializeHelper.initializeObject(_gameObject.GetComponent<ISetupAble>(), _parameters);
        }

        public PartElementType getElementType() {
            return PartElementType.TutorialUiMask;
        }

        public GameObject getGameObject() {
            if (_gameObject == null) {
                throw new Exception("Called getGameObject before initializing it!");
            }
            return _gameObject;
        }
    }
}