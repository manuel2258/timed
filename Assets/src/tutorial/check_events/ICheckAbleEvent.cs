using System;

namespace src.tutorial.check_events {
    public interface ICheckAbleEvent {
        void registerEvent(string eventName, Action onEventChecked);
    }
}