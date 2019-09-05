using src.simulation.reseting;
using UnityEngine;

namespace src.time.time_managers {
    
    /// <summary>
    /// A Singleton manages the time of the simulation
    /// </summary>
    public class SimulationTimeManager : BaseTimeManager<SimulationTimeManager>, IResetable {

        public void advanceTime(decimal deltaTime) {
            currentTime += deltaTime;
            onNewTime?.Invoke(currentTime, deltaTime);
        }

        public void reset() {
            currentTime = 0;
        }
    }
    
    
}