using System;
using System.Collections;
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

        public const decimal SIMULATION_LENGTH = 10;
        public const decimal SIMULATION_STEPS = 0.025M;

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
            var trackers = simulate();
            onCalculationFinished?.Invoke(trackers);
        }

        private List<GameObjectTracker> simulate() {
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
                simulationTimeManger.advanceTime(SIMULATION_STEPS);
                Physics2D.Simulate((float)SIMULATION_STEPS);
                foreach (var tracker in trackers) {
                    tracker.track(simulationTimeManger.CurrentTime);
                }
            }

            return trackers;
        }

    }

    public delegate void OnCalculationStarted();
    public delegate void OnCalculationFinished(List<GameObjectTracker> trackers);
}