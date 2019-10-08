using System;
using System.Collections.Generic;
using src.level;
using src.tutorial.check_events;
using UnityEngine;
using Object = UnityEngine.Object;

namespace src.tutorial {
    public class PartContainer {
        
        private readonly Dictionary<int, IPartElement> _partElements = new Dictionary<int, IPartElement>();
        private readonly List<IPartElement> _checkEvents = new List<IPartElement> ();
        private readonly List<string> _eventNames = new List<string>();
        
        public int Id { get; }
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
            } else {
                _checkEvents.Add(element);
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

            foreach (var partElement in _checkEvents) {
                partElement.initialize(_rectParent.transform, _worldParent.transform);
                var checkEvent = (BaseCheckEvent) partElement;
                _eventNames.Add(checkEvent.EventName);
                checkEvent.onEventChecked += eventName => {
                    if (!_currentActiveState) return;

                    _eventNames.Remove(eventName);
                    if (_eventNames.Count == 0) {
                        onAllEventsChecked.Invoke(Id);
                    }
                };
            }
        }

        public GameObject getPartById(int id) {
            return _partElements[id].getGameObject();
        }

        public void setActive(bool state) {
            _rectParent.SetActive(state);
            _worldParent.SetActive(state);
            _currentActiveState = state;
        }
    }

    public interface IPartElement {
        void initialize(Transform rectParent, Transform worldParent);
        PartElementType getElementType();
        GameObject getGameObject();
    }

    public enum PartElementType {
        CheckEvent,
        HelpDisplay
    }
}