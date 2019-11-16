using System;
using System.Collections.Generic;
using System.Linq;
using src.level.selection;
using src.misc;
using src.simulation.reseting;
using src.touch;
using src.tutorial.check_events;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace src.level.finish {
    public class LevelFinishUIController : UnitySingleton<LevelFinishUIController>, IResetable, IStackAbleWindow, ICheckAbleEvent {

        public Canvas endLevelScreen;
        public Button endLevelButton;

        public TMP_Text levelName;
        public Canvas finishedCanvas;

        public Button nextLevelButton;

        public Sprite notFinished;
        public Sprite finished;

        public List<RectTransform> uiMask;

        public DifficultyUIController difficultyUIController;
        public DifficultyUIController scoreUIController;
        public TMP_Text scoreText;
        

        private readonly CheckEventManager _checkEventManager = new CheckEventManager();
        public void registerEvent(string eventName, Action onEventChecked) {
            _checkEventManager.registerEvent(eventName, onEventChecked);
        }

        private void Start() {
            LevelFinishManager.Instance.onLevelFinished += (score, real) => {
                var highScore = LevelProgressManager.Instance.HighScore;
                if (real) {
                    endLevelButton.image.sprite = finished;
                    scoreText.text = highScore != -1 ? $"{highScore} -> {score}" : score.ToString();
                } else {
                    scoreText.text = score.ToString();
                }

                finishedCanvas.enabled = true;
                
                var levelHeader = LevelManager.Instance.CurrentLevel.LevelHeader;

                if (score < highScore) {
                    score = highScore;
                }
                int stars = 1 + levelHeader.Scores.Count(levelHeaderScore => score > levelHeaderScore);
                scoreUIController.displayDifficulty(stars);
            };
            endLevelButton.onClick.AddListener(() => UIWindowStack.Instance.toggleWindow(typeof(LevelFinishUIController)));
            
            var currentLevel = LevelManager.Instance.CurrentLevel;
            levelName.text = currentLevel.LevelHeader.Name;
            difficultyUIController.displayDifficulty(currentLevel.LevelHeader.Difficulty);
            GravityScaleUIController.Instance.displayGravity(currentLevel.GravityScale);

            if (LevelSelectionManager.Instance != null) {
                if (LevelSelectionManager.Instance.hasNextLevel()) {
                    nextLevelButton.gameObject.SetActive(true);
                    nextLevelButton.onClick.AddListener(() => LevelSelectionManager.Instance.loadNextLevel());
                }
            }
        }

        public void setActive(bool newState) {
            endLevelScreen.enabled = newState;
            var stateString = newState? "Active" : "NonActive";
            _checkEventManager.checkEvent($"Set{stateString}");
            if (newState) {
                UiMaskManager.Instance.addMasks(uiMask);
            } else {
                UiMaskManager.Instance.removeMasks(uiMask);
            }
        }

        public bool isActive() {
            return endLevelScreen.enabled;
        }

        public void reset() {
            endLevelButton.image.sprite = notFinished;
            //finishedCanvas.enabled = false;
        }
    }
}