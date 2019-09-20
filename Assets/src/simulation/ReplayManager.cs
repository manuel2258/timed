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

        private decimal _beforeTime;

        private void Start() {
            ReplayTimeManager.Instance.onNewTime += onNewTime;
            SimulationManager.Instance.onCalculationFinished += newTrackers => {
                _currentTrackers = newTrackers;
                onNewTime(ReplayTimeManager.Instance.CurrentTime, 0);
                ReplayTimeManager.Instance.setCurrentTime(_beforeTime);
            };
            SimulationManager.Instance.onCalculationStarted += () => {
                ReplayTimeManager.Instance.Active = false;
                _beforeTime = ReplayTimeManager.Instance.CurrentTime;
                ReplayTimeManager.Instance.setCurrentTime(0);
            };
            
            addAllChildParticles(GameObject.Find("Level").transform);
            
            setParticleSimulationSpeed(0);
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

        private void setParticleSimulationSpeed(float speed) {
            foreach (var system in _particleSystems) {
                var main = system.main;
                main.simulationSpeed = speed;
            }
        }

        private void addAllChildParticles(Transform parent) {
            for (int i = 0; i < parent.childCount; i++) {
                var currentChild = parent.GetChild(i);
                {
                    var particleSystem = currentChild.GetComponent<ParticleSystem>();
                    if (particleSystem != null) {
                        _particleSystems.Add(particleSystem);
                    }
                }
                addAllChildParticles(currentChild);
            }
        }

        public void skipFrames(int frames) {
            ReplayTimeManager.Instance.setCurrentTime(ReplayTimeManager.Instance.CurrentTime +
                                                      SimulationManager.SIMULATION_STEPS * frames);
        }
    }

    public delegate void OnActiveStatusChanged(bool newState);
}