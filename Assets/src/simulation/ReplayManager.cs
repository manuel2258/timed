using System.Collections.Generic;
using src.misc;
using src.time.time_managers;
using UnityEngine;

namespace src.simulation {
    
    /// <summary>
    /// Singleton that manages the positional replay and its flow 
    /// </summary>
    public class ReplayManager : UnitySingleton<ReplayManager> {
        
        private List<GameObjectTracker> _currentTrackers = new List<GameObjectTracker>();

        private readonly List<ParticleSystem> _particleSystems = new List<ParticleSystem>();

        public OnActiveStatusChanged onActiveStatusChanged;

        private bool _active;
        public bool Active {
            get => _active;
            set {
                _active = value;
                ReplayTimeManager.Instance.Active = _active;
                setParticleSimulationSpeed(_active? 1 : 0);
                onActiveStatusChanged?.Invoke(_active);
            }
        }

        private int _currentPriority;

        private void Start() {
            ReplayTimeManager.Instance.onNewTime += onNewTime;
            SimulationManager.Instance.onCalculationFinished += newTrackers => {
                _currentTrackers = newTrackers;
                onNewTime(ReplayTimeManager.Instance.CurrentTime, 0);
            };
            SimulationManager.Instance.onCalculationStarted += () => ReplayTimeManager.Instance.Active = false;
            foreach (var system in FindObjectsOfType<ParticleSystem>()) {
                _particleSystems.Add(system);
            }
            setParticleSimulationSpeed(0);
        }

        private void onNewTime(decimal currentTime, decimal deltaTime) {
            foreach (var tracker in _currentTrackers) {
                tracker.replayTimestamp(currentTime);
            }
        }

        public void toggleActive() {
            Active = !Active;
        }

        private void setParticleSimulationSpeed(float speed) {
            foreach (var system in _particleSystems) {
                var main = system.main;
                main.simulationSpeed = speed;
            }
        }
    }

    public delegate void OnActiveStatusChanged(bool newState);
}