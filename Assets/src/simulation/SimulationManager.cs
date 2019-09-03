using System;
using System.Collections.Generic;
using src.misc;
using src.simulation.reseting;
using src.time.time_managers;
using src.time.timeline;
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
            calculateSimulation();
            Timeline.Instance.onEffectorEventChanged += _ => calculateSimulation();
        }
        
        private void calculateSimulation() {
            onCalculationStarted?.Invoke();
            var rigidBodys = FindObjectsOfType<Rigidbody2D>();
            var trackers = new List<GameObjectTracker>();

            foreach (var rigidBody in rigidBodys) {
                if (rigidBody.GetComponent<IResetable>() == null) {
                    throw new Exception($"Tried to track a non resetAble gameObject!: {rigidBody.name}");
                }
                trackers.Add(new GameObjectTracker(rigidBody.gameObject));
            }

            var simulationTimeManger = SimulationTimeManager.Instance;
            
            
            while (simulationTimeManger.CurrentTime < SIMULATION_LENGTH) {
                SimulationTimeManager.Instance.advanceTime(SIMULATION_STEPS);
                Physics2D.Simulate(SIMULATION_STEPS);
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