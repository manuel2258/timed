using System;
using src.element;
using src.level.parsing;
using UnityEngine;

namespace src.level.initializing {
    public class ElementInitializer {

        private readonly ElementType _elementType;
        private readonly Vector2 _position;
        private readonly float _angle;

        public readonly int Id;

        public ElementInitializer(ElementType elementType, int id, Vector2 position, float angle) {
            _elementType = elementType;
            _position = position;
            _angle = angle;
            Id = id;
        }

        public GameObject initialize() {
            var currentGameObject = getGameObject();
            setTransform(currentGameObject);
            callSetupScript(currentGameObject);
            return currentGameObject;
        }

        protected virtual GameObject getGameObject() {
            switch (_elementType) {
                case ElementType.Wall:
                    return ElementPrefabStorage.Instance.getWall();
                case ElementType.ColliderBody:
                    return ElementPrefabStorage.Instance.getColliderBody();
                default:
                    throw new Exception("ElementInitialized used for effector or trigger!");
            }
        }

        protected virtual void setTransform(GameObject currentGameObject) {
            currentGameObject.transform.position = _position;
            currentGameObject.transform.rotation = Quaternion.Euler(0,0,_angle);
            currentGameObject.transform.parent = LevelManager.Instance.LevelRoot;
        }

        protected virtual void callSetupScript(GameObject currentGameObject) { }
    }
}