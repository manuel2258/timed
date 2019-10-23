using System;
using System.Collections.Generic;
using src.level;
using src.touch;
using src.tutorial.check_events;
using UnityEngine;
using Object = UnityEngine.Object;

namespace src.tutorial {
    public class PartContainer {
        
        private readonly Dictionary<int, IPartElement> _partElements = new Dictionary<int, IPartElement>();
        private readonly Dictionary<int, BaseCheckEvent> _checkEvents = new Dictionary<int, BaseCheckEvent>();
        private readonly Stack<BaseCheckEvent> _events = new Stack<BaseCheckEvent>();

        private readonly List<IPartElement> _masks = new List<IPartElement>();
        private readonly List<RectTransform> _uiMasks = new List<RectTransform>();
        
        public int Id { get; }
        public bool IsBlocking { get; private set; }
        public Action<int> onAllEventsChecked;

        private GameObject _rectParent;
        private GameObject _worldParent;
        private bool _currentActiveState;
        
        public PartContainer(int id) {
            Id = id;
            _currentActiveState = false;
        }

        public void addElement(IPartElement element, int id) {
            if (element.getElementType() == PartElementType.HelpDisplay) {
                _partElements.Add(id, element);
            } else if (element.getElementType() == PartElementType.CheckEvent) {
                _checkEvents.Add(id, (BaseCheckEvent)element);
            }
            else if (element.getElementType() == PartElementType.TutorialUiMask) {
                _masks.Add(element);
            }
        }

        public void initializeElements() {
            _rectParent = Object.Instantiate(TutorialPrefabStorage.Instance.TutorialPart, 
                LevelManager.Instance.TutorialRoot);
            _worldParent = new GameObject();
            _rectParent.name = $"TutorialPart {Id}";
            _worldParent.name = $"TutorialPart {Id}";
            _worldParent.transform.parent = LevelManager.Instance.LevelRoot;
            foreach (var keyValue in _partElements) {
                var partElement = keyValue.Value;
                partElement.initialize(_rectParent.transform, _worldParent.transform);
            }
            
            foreach (var blocker in _masks) {
                blocker.initialize(_rectParent.transform, _worldParent.transform);
                _uiMasks.Add(blocker.getGameObject().GetComponent<RectTransform>());
            }

            int i = _checkEvents.Count;
            while(true){
                if (_checkEvents.TryGetValue(i, out var checkEvent)) {
                    checkEvent.initialize(_rectParent.transform, _worldParent.transform);
                    _events.Push(checkEvent);
                    checkEvent.onEventChecked += checkedEvent => {
                        if (!_currentActiveState) return;
                        var stackPeek = _events.Peek();
                        if (checkedEvent != stackPeek) return;
                        _events.Pop();
                        if (_events.Count == 0) {
                            onAllEventsChecked.Invoke(Id);
                        }
                    };
                }

                i--;
                if (i < 0) {
                    break;
                }
            }
        }

        public GameObject getPartById(int id) {
            if (!_partElements.TryGetValue(id, out var part)) {
                throw new Exception($"Could not find element {id} in part {Id}!");
            }
            return part.getGameObject();
        }

        public void setActive(bool state) {
            _rectParent.SetActive(state);
            _worldParent.SetActive(state);
            _currentActiveState = state;
            if (state) {
                UiMaskManager.Instance.addMasks(_uiMasks);
            } else {
                UiMaskManager.Instance.removeMasks(_uiMasks);
            }
        }
    }

    public interface IPartElement {
        void initialize(Transform rectParent, Transform worldParent);
        PartElementType getElementType();
        GameObject getGameObject();
    }

    public enum PartElementType {
        CheckEvent,
        HelpDisplay,
        TutorialUiMask,
    }
}