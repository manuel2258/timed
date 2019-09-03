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

        public void toggleActive() {
            Active = !Active;
        }

        private void FixedUpdate() {
            if (Active) {
                var deltaTime = Time.fixedDeltaTime * TimeMultiplier;
                currentTime += deltaTime;
                currentTime = Mathf.Clamp(currentTime, 0, SimulationManager.SIMULATION_LENGTH);
                onNewTime?.Invoke(currentTime, deltaTime);

                if (currentTime > SimulationManager.SIMULATION_LENGTH) {
                    currentTime = 0;
                    Active = false;
                }
            }
        }
    }
}