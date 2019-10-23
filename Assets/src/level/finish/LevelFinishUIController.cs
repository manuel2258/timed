using System;
using System.Collections.Generic;
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
        
        private readonly CheckEventManager _checkEventManager = new CheckEventManager();
        public void registerEvent(string eventName, Action onEventChecked) {
            _checkEventManager.registerEvent(eventName, onEventChecked);
        }

        private void Start() {
            LevelFinishManager.Instance.onLevelFinished += () => {
                endLevelButton.image.sprite = finished;
                finishedCanvas.enabled = true;
            };
            endLevelButton.onClick.AddListener(() => UIWindowStack.Instance.toggleWindow(typeof(LevelFinishUIController)));
            
            var currentLevel = LevelManager.Instance.CurrentLevel;
            levelName.text = currentLevel.Name;
            DifficultyUIController.Instance.displayDifficulty(currentLevel.Difficulty);
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
            finishedCanvas.enabled = false;
        }
    }
}