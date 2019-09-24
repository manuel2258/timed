using System;
using System.Collections.Generic;
using src.misc;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace src.level.selection {
    public class LevelSelectionManager : UnitySingleton<LevelSelectionManager> {

        [SerializeField] private List<LevelPack> levelPacks;

        public List<LevelPack> LevelPacks => new List<LevelPack>(levelPacks);

        private LevelPack _lastLoadedPack;
        private int _lastIndex;

        private void Start() {
            if (Instance != this) {
                Destroy(gameObject);
            }
            DontDestroyOnLoad(gameObject);
        }

        public void loadIndexFromPack(LevelPack levelPack, int index) {
            if (!levelPacks.Contains(levelPack)) {
                throw new Exception("Could not find the requested levelPack!");
            }

            _lastLoadedPack = levelPack;
            _lastIndex = index;
            LevelXmlPayloadFactory.generateFromString(levelPack[index]);
            SceneManager.LoadScene("MainScene");
        }

        public void loadFromString(string level) {
            LevelXmlPayloadFactory.generateFromString(level);
            SceneManager.LoadScene("MainScene");
        }

        public bool hasNextLevel() {
            if (_lastLoadedPack == null) return false;
            return _lastIndex + 2 <= _lastLoadedPack.LevelCount;
        }

        public void loadNextLevel() {
            if (!hasNextLevel()) {
                throw new Exception("Could not find a next level!");
            }
            _lastIndex++;
            loadIndexFromPack(_lastLoadedPack, _lastIndex);
        }
    }
}