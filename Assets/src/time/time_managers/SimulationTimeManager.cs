using System;
using src.simulation;
using src.simulation.reseting;
using UnityEngine;

namespace src.time.time_managers {
    
    /// <summary>
    /// A Singleton manages the time of the simulation
    /// </summary>
    public class SimulationTimeManager : BaseTimeManager<SimulationTimeManager>, IResetable {

        private float _currentFixedTime;

        public OnNewTime onNewFixedTime;

        public void advanceTime(float deltaTime) {
            currentTime += deltaTime;
            _currentFixedTime += deltaTime;
            if (_currentFixedTime > Time.fixedDeltaTime) {
                onNewFixedTime?.Invoke(currentTime, Time.fixedDeltaTime);
            }
            onNewTime?.Invoke(currentTime, deltaTime);
        }

        public void reset() {
            currentTime = 0;
            _currentFixedTime = 0;
        }
    }
    
    
}