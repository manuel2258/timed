using System;
using System.Collections.Generic;
using src.level.initializing;
using UnityEngine;

namespace src.tutorial {
    public class HelpDisplayInitializer : IPartElement {

        private readonly HelpDisplayType _helpDisplayType;
        private readonly Dictionary<string, string> _parameters;
        private GameObject _gameObject;
        
        public HelpDisplayInitializer(HelpDisplayType helpDisplayType, Dictionary<string, string> parameters) {
            _helpDisplayType = helpDisplayType;
            _parameters = parameters;
        }

        public void initialize(Transform rectParent, Transform worldParent) {
            _gameObject = TutorialPrefabStorage.Instance.getHelpDisplayByType(_helpDisplayType);
            _gameObject.transform.SetParent(
                _helpDisplayType == HelpDisplayType.ElementHighlight ? worldParent : rectParent);

            InitializeHelper.initializeObject(_gameObject.GetComponent<ISetupAble>(), _parameters);
        }

        public PartElementType getElementType() {
            return PartElementType.HelpDisplay;
        }

        public GameObject getGameObject() {
            if (_gameObject == null) {
                throw new Exception("Called getGameObject before initializing it!");
            }
            return _gameObject;
        }
    }

    public enum HelpDisplayType {
        Frame,
        Text,
        ElementHighlight
    }
}