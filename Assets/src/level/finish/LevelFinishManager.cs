using System;
using System.Collections.Generic;
using src.element.triggers;
using src.misc;
using src.simulation;
using src.simulation.reseting;
using src.time.time_managers;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace src.level.finish {
    
    /// <summary>
    /// Manages the goal completion of the level
    /// </summary>
    public class LevelFinishManager : UnitySingleton<LevelFinishManager>, IResetable {

        public Action<int, bool> onLevelFinished;
        
        private readonly List<GoalTrigger> _allGoalTriggers = new List<GoalTrigger>();
        private List<GoalTrigger> _currentUnfinishedGoals = new List<GoalTrigger>();

        /// <summary>
        /// All GoalTriggers must call this function on initial setup
        /// </summary>
        /// <param name="goalTrigger">The to add goalTrigger</param>
        public void registerGoal(GoalTrigger goalTrigger) {
            _allGoalTriggers.Add(goalTrigger);
            _currentUnfinishedGoals = new List<GoalTrigger>(_allGoalTriggers);
        }

        /// <summary>
        /// To be called by a GoalTrigger once its goal is full filled
        /// </summary>
        /// <param name="finishedGoal">The finished GoalTrigger</param>
        public void onGoalFinished(GoalTrigger finishedGoal) {
            if (!_currentUnfinishedGoals.Remove(finishedGoal)) {
                throw new Exception("Got finished signal from a non tracker or already finished goal");
            }
            if (_currentUnfinishedGoals.Count <= 0) {
                float currentTime = (float)SimulationTimeManager.Instance.CurrentTime;
                var currentScore = (int)((Mathf.Cos(currentTime * Mathf.PI / (float) SimulationManager.SIMULATION_LENGTH) / 2 +
                                   0.5) * 1000);
                onLevelFinished.Invoke(currentScore, true);
            }
        }

        public void triggerOnLevelFinish(int score) {
            onLevelFinished?.Invoke(score, false);
        }

        public void reset() {
            _currentUnfinishedGoals = new List<GoalTrigger>(_allGoalTriggers);
        }

        public void goToMainMenu() {
            SceneManager.LoadScene("MainMenu");
        }
        
        public void resetLevel() {
            SceneManager.LoadScene("MainScene");
        }
    }

    public delegate void OnLevelFinished();
}