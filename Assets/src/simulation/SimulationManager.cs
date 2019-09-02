using System.Collections.Generic;
using src.misc;
using src.time.time_managers;
using UnityEngine;

namespace src.simulation {
    
    /// <summary>
    /// Singleton that manages the physic simulation
    /// </summary>
    public class SimulationManager : UnitySingleton<SimulationManager> {

        public const float SIMULATION_LENGTH = 30f;
        public const float SIMULATION_STEPS = 0.005f;

        /// <summary>
        /// Called when the simulation starts
        /// </summary>
        public OnCalculationStarted onCalculationStarted;
        
        /// <summary>
        /// Called when the simulations ends
        /// </summary>
        public OnCalculationFinished onCalculationFinished;

        private void Start() {
            Physics2D.autoSimulation = false;
            calculateSimulation(SIMULATION_LENGTH, SIMULATION_STEPS);
        }
        
        private void calculateSimulation(float timeLength, float deltaTime) {
            onCalculationStarted?.Invoke();
            var rigidBodys = FindObjectsOfType<Rigidbody2D>();
            var trackers = new List<GameObjectTracker>();

            foreach (var rigidBody in rigidBodys) {
                trackers.Add(new GameObjectTracker(rigidBody.gameObject));
            }

            var simulationTimeManger = SimulationTimeManager.Instance;
            
            
            while (simulationTimeManger.CurrentTime < timeLength) {
                SimulationTimeManager.Instance.advanceTime(deltaTime);
                Physics2D.Simulate(deltaTime);
                foreach (var tracker in trackers) {
                    tracker.track(simulationTimeManger.CurrentTime);
                }
            }
            onCalculationFinished?.Invoke(trackers);
        }
        
    }

    public delegate void OnCalculationStarted();
    public delegate void OnCalculationFinished(List<GameObjectTracker> trackers);
}