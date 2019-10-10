using System;
using System.Collections.Generic;
using System.Linq;

namespace src.tutorial.check_events {
    public class CheckEventManager {

        private readonly HashSet<string> _registeredEventNames = new HashSet<string>();

        private struct EventRegister {
            public string name;
            public Action callback;
        }
        
        private readonly List<EventRegister> _eventRegister = new List<EventRegister>();
        
        public void registerEvent(string eventName, Action callback) {
            _eventRegister.Add(new EventRegister {
                name = eventName,
                callback = callback,
            });
            _registeredEventNames.Add(eventName);
        }

        public void checkEvent(string eventName) {
            if (!_registeredEventNames.Contains(eventName)) return;
            foreach (var checkEvent in _eventRegister) {
                if(checkEvent.name != eventName) continue;
                checkEvent.callback.Invoke();
            }
        }
    }
}