using System.Collections.Generic;
using src.misc;
using src.time;
using src.time.time_managers;
using src.time.timeline;

namespace src.simulation {
    
    /// <summary>
    /// Singleton that manages the physic replay 
    /// </summary>
    public class ReplayManager : UnitySingleton<ReplayManager> {
        
        private List<GameObjectTracker> _currentTrackers = new List<GameObjectTracker>();

        private void Start() {
            ReplayTimeManager.Instance.onNewTime += onNewTime;
            SimulationManager.Instance.onCalculationFinished += newTrackers => {
                _currentTrackers = newTrackers;
                onNewTime(ReplayTimeManager.Instance.CurrentTime, 0);
            };
            SimulationManager.Instance.onCalculationStarted += () => ReplayTimeManager.Instance.Active = false;
        }

        private void onNewTime(decimal currentTime, decimal deltaTime) {
            foreach (var tracker in _currentTrackers) {
                tracker.replayTimestamp(currentTime);
            }
        }
    }
}