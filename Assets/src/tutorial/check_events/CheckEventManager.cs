using System;
using System.Collections.Generic;
using System.Linq;

namespace src.tutorial.check_events {
    public class CheckEventManager {

        private readonly HashSet<string> _registeredEventNames = new HashSet<string>();
        private readonly List<KeyValuePair<string, Action>> _eventRegister = new List<KeyValuePair<string, Action>>();
        
        public void registerEvent(string eventName, Action callback) {
            _eventRegister.Add(new KeyValuePair<string, Action>(eventName, callback));
            _registeredEventNames.Add(eventName);
        }

        public void checkEvent(string eventName) {
            if (!_registeredEventNames.Contains(eventName)) return;
            foreach (var checkEvent in _eventRegister) {
                if(checkEvent.Key != eventName) continue;
                checkEvent.Value.Invoke();
            }
        }
    }
}