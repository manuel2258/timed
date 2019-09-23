using System;
using System.Collections.Generic;
using src.misc;
using UnityEngine.SceneManagement;
using UnityEngine;

namespace src.level.selection {
    public class LevelSelectionManager : UnitySingleton<LevelSelectionManager> {

        [SerializeField] private List<LevelPack> levelPacks;

        public List<LevelPack> LevelPacks => new List<LevelPack>(levelPacks);

        public void loadIndexFromPack(LevelPack levelPack, int index) {
            if (!levelPacks.Contains(levelPack)) {
                throw new Exception("Could not find the requested levelPack!");
            }
            
            LevelXmlPayloadFactory.generateFromString(levelPack[index]);
            SceneManager.LoadScene("MainScene");
        }

        public void loadFromString(string level) {
            LevelXmlPayloadFactory.generateFromString(level);
            SceneManager.LoadScene("MainScene");
        }
    }
}