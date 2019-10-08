using System;
using UnityEngine;

namespace src.tutorial.check_events {
    public abstract class BaseCheckEvent: IPartElement {

        public string EventName { get; }

        protected BaseCheckEvent(string eventName) {
            EventName = eventName;
        }

        public Action<string> onEventChecked;

        public void initialize(Transform rect, Transform world) {
            getToCheckGameObject().GetComponent<ICheckAbleEvent>().registerEvent(EventName, 
                () => onEventChecked.Invoke(EventName));
        }

        protected abstract GameObject getToCheckGameObject();

        public PartElementType getElementType() {
            return PartElementType.CheckEvent;
        }

        public GameObject getGameObject() {
            throw new Exception("Can't get GameObject of a CheckEvent");
        }
    }

    public enum CheckEventType {
        ElementEvent,
        GameObjectEvent,
        PartElementEvent,
    }
}