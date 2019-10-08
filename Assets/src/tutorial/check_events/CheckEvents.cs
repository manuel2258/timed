using src.level;
using src.level.parsing;
using UnityEngine;

namespace src.tutorial.check_events {
    public class GameObjectCheckEvent : BaseCheckEvent {

        private readonly string _gameObjectName;

        public GameObjectCheckEvent(string eventName, string gameObjectName) : base(eventName) {
            _gameObjectName = gameObjectName;
        }
        
        protected override GameObject getToCheckGameObject() {
            return GameObject.Find(_gameObjectName);
        }
    }
    
    public class ElementCheckEvent : BaseCheckEvent {

        private readonly int _id;

        public ElementCheckEvent(string eventName, int id) : base(eventName) {
            _id = id;
        }
        
        protected override GameObject getToCheckGameObject() {
            return LevelManager.Instance.CurrentLevel.getElementFromId(_id);
        }
    }
    
    public class PartElementCheckEvent : BaseCheckEvent {

        private readonly int _partId;
        private readonly int _elementId;

        public PartElementCheckEvent(string eventName, int partId, int elementId) : base(eventName) {
            _partId = partId;
            _elementId = elementId;
        }
        
        protected override GameObject getToCheckGameObject() {
            return LevelManager.Instance.CurrentTutorial.getPartElementById(_partId, _elementId);
        }
    }
}