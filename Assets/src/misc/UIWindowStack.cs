using System;
using System.Collections.Generic;
using System.Linq;
using src.level;
using src.touch;
using UnityEngine;

namespace src.misc {
    public class UIWindowStack : UnitySingleton<UIWindowStack> {
        readonly Dictionary<Type, IStackAbleWindow> _typeWindowMap = new Dictionary<Type, IStackAbleWindow>();
        
        private void Start() {
            var allGameObjects = FindObjectsOfType<GameObject>();
            var stackAbles = (from a in allGameObjects where a.GetComponent<IStackAbleWindow>() != null 
                select a.GetComponent<IStackAbleWindow>()).ToList();
                
            foreach (var stackAbleWindow in stackAbles) {
                _typeWindowMap.Add(stackAbleWindow.GetType(), stackAbleWindow);
            }
        }

        public void toggleWindow(Type windowType) {
            if (!_typeWindowMap.TryGetValue(windowType, out var window)) {
                throw new Exception($"Could not find a stack able window of the type {windowType}");
            }

            foreach (var stackAbleWindow in _typeWindowMap) {
                if(stackAbleWindow.Value == window) continue;
                stackAbleWindow.Value.setActive(false);
            }
            window.setActive(!window.isActive());
            TouchManager.Instance.blocked = window.isActive();
        }

    }

    public interface IStackAbleWindow {
        void setActive(bool newActive);
        bool isActive();
    }
}