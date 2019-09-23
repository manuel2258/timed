using System;
using System.Collections.Generic;
using src.element.triggers;
using src.misc;
using src.simulation.reseting;
using UnityEngine.SceneManagement;

namespace src.level {
    
    /// <summary>
    /// Manages the goal completion of the level
    /// </summary>
    public class LevelFinishManager : UnitySingleton<LevelFinishManager>, IResetable {

        public OnLevelFinished onLevelFinished;
        
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
                onLevelFinished.Invoke();
            }
        }

        public void reset() {
            _currentUnfinishedGoals = new List<GoalTrigger>(_allGoalTriggers);
        }

        public void goToMainMenu() {
            SceneManager.LoadScene("MainMenu");
        }
    }

    public delegate void OnLevelFinished();
}