using System;
using src.simulation;
using src.tutorial.check_events;
using UnityEngine;

namespace src.time.time_managers {
    
    /// <summary>
    /// The Singleton TimeManager that is used when the tracked positions are played back in realtime
    /// </summary>
    public class ReplayTimeManager : BaseTimeManager<ReplayTimeManager>, ICheckAbleEvent {
        
        public float TimeMultiplier { get; set; } = 1;

        public bool Active { get; set; }
        
        private readonly CheckEventManager _checkEventManager = new CheckEventManager();
        public void registerEvent(string eventName, Action onEventChecked) {
            _checkEventManager.registerEvent(eventName, onEventChecked);
        }
        
        public void setCurrentTime(decimal newTime) {
            if (Active) return;
            if (newTime < 0 || newTime > SimulationManager.SIMULATION_LENGTH) return;

            var delta = newTime - currentTime;
            currentTime = newTime;
            onNewTime?.Invoke(currentTime, delta);
            _checkEventManager.checkEvent("SetTime");
        }
        
        private void FixedUpdate() {
            if (!Active) return;
            
            var deltaTime = (decimal)(Time.fixedDeltaTime * TimeMultiplier);
            currentTime += deltaTime;
            currentTime = currentTime < 0 ? 0 : currentTime;
            currentTime = currentTime > SimulationManager.SIMULATION_LENGTH ? SimulationManager.SIMULATION_LENGTH : currentTime;
            onNewTime?.Invoke(currentTime, deltaTime);

            if (currentTime > SimulationManager.SIMULATION_LENGTH) {
                currentTime = 0;
                Active = false;
            }
        }
    }
}