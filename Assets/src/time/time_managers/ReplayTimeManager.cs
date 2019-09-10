using System;
using src.simulation;
using UnityEngine;

namespace src.time.time_managers {
    
    /// <summary>
    /// The Singleton TimeManager that is used when the tracked positions are played back in realtime
    /// </summary>
    public class ReplayTimeManager : BaseTimeManager<ReplayTimeManager> {

        public float TimeMultiplier { get; set; } = 1;

        public bool Active { get; set; }
        
        public void setCurrentTime(decimal newTime) {
            if (!Active) {
                currentTime = newTime;
                onNewTime?.Invoke(currentTime, 0);
            }
        }

        private void FixedUpdate() {
            if (Active) {
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
}