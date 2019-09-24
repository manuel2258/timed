using System.Collections.Generic;
using src.misc;
using src.time.time_managers;

namespace src.simulation {
    
    /// <summary>
    /// Singleton that manages the positional replay and its flow 
    /// </summary>
    public class ReplayManager : UnitySingleton<ReplayManager> {
        
        private List<GameObjectTracker> _currentTrackers = new List<GameObjectTracker>();

        public OnActiveStatusChanged onActiveStatusChanged;

        private bool _active;
        public bool Active {
            get => _active;
            set {
                _active = value;
                ReplayTimeManager.Instance.Active = _active;
                onActiveStatusChanged?.Invoke(_active);
            }
        }

        private int _currentPriority;

        private decimal _beforeTime;

        private bool _beforeActiveState;

        private void Start() {
            ReplayTimeManager.Instance.onNewTime += onNewTime;
            SimulationManager.Instance.onCalculationFinished += newTrackers => {
                _currentTrackers = newTrackers;
                onNewTime(ReplayTimeManager.Instance.CurrentTime, 0);
                ReplayTimeManager.Instance.setCurrentTime(_beforeTime);
                Active = _beforeActiveState;
            };
            SimulationManager.Instance.onCalculationStarted += () => {
                ReplayTimeManager.Instance.Active = false;
                _beforeTime = ReplayTimeManager.Instance.CurrentTime;
                ReplayTimeManager.Instance.setCurrentTime(0);
                _beforeActiveState = Active;
            };
            Active = false;
        }

        public void toggleActive() {
            Active = !Active;
        }

        private void onNewTime(decimal currentTime, decimal deltaTime) {
            foreach (var tracker in _currentTrackers) {
                tracker.replayTimestamp(currentTime);
            }
        }
        
        public void skipFrames(int frames) {
            ReplayTimeManager.Instance.setCurrentTime(ReplayTimeManager.Instance.CurrentTime +
                                                      SimulationManager.SIMULATION_STEPS * frames);
        }
    }

    public delegate void OnActiveStatusChanged(bool newState);
}