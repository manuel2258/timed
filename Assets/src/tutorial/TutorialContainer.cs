using System;
using System.Collections.Generic;
using UnityEngine;

namespace src.tutorial {
    public class TutorialContainer {
        
        public bool HasTutorial { get; private set; }
        
        private readonly Dictionary<int, PartContainer> _parts = new Dictionary<int, PartContainer>();
        private int _currentId;

        public Action onTutorialFinished;

        public void addPart(PartContainer part) {
            HasTutorial = true;
            _parts.Add(part.Id, part);
            part.onAllEventsChecked += id => {
                if (id != _currentId) {
                    throw new Exception("Received a finish Event off a non active TutorialPart");
                }
                
                _parts[_currentId].setActive(false);
                _currentId++;
                if (_currentId < _parts.Count) {
                    _parts[_currentId].setActive(true);
                } else {
                    onTutorialFinished.Invoke();
                }
            };
        }

        public void initializeTutorial() {
            if (!HasTutorial) return;
            
            foreach (var keyValuePair in _parts) {
                keyValuePair.Value.initializeElements();
                keyValuePair.Value.setActive(false);
            }

            _parts[0].setActive(true);
        }

        public GameObject getPartElementById(int partId, int partElementId) {
            return _parts[partId].getPartById(partElementId);
        }
        
    }
}