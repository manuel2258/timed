using System.Collections.Generic;
using src.misc;
using UnityEngine;

namespace src.time.timeline {
    public class EventlineUIController : UnitySingleton<EventlineUIController> {

        public Transform contentParent;
        public GameObject contentPrefab;
        
        private void Start() {
            Timeline.Instance.onEffectorEventChanged += updateEffectorEvents;
        }

        private void updateEffectorEvents(List<TimedEffectorEvent> effectorEvents) {
            for (int i = contentParent.childCount-1; i >= 0 ; i--) {
                Destroy(contentParent.GetChild(i).gameObject);
            }
            foreach (var effectorEvent in effectorEvents) {
                var newObject = Instantiate(contentPrefab, contentParent);
                newObject.GetComponent<TimedEffectorEventUIController>().setup(effectorEvent);
            }
        }
    }
}