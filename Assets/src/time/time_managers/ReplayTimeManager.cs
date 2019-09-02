using src.simulation;
using UnityEngine;

namespace src.time.time_managers {
    
    /// <summary>
    /// The Singleton TimeManager that is used when the tracked positions are played back in realtime
    /// </summary>
    public class ReplayTimeManager : BaseTimeManager<ReplayTimeManager> {
        private float _timeMultiplier = 1f;

        public bool Active { get; set; }

        private void Start() {
            SimulationManager.Instance.onCalculationFinished += _ => Active = true;
        }

        public void toggleActive() {
            Active = !Active;
        }

        private void FixedUpdate() {
            if (Active) {
                var deltaTime = Time.fixedDeltaTime * _timeMultiplier;
                currentTime += deltaTime;
                onNewTime?.Invoke(currentTime, deltaTime);

                if (currentTime > SimulationManager.SIMULATION_LENGTH) {
                    currentTime = 0;
                    Active = false;
                }
            }
        }
    }
}