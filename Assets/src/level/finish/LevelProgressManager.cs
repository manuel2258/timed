using System.Xml;
using src.element.effector;
using src.misc;
using UnityEngine;

namespace src.level.finish {
    public class LevelProgressManager : UnitySingleton<LevelProgressManager> {
        
        private string _progressPath;
        
        public int HighScore { get; private set; }
        
        private void Start() {
            _progressPath = Application.persistentDataPath + $"/level_saves/{LevelManager.Instance.CurrentLevel.LevelHeader.GUID}.xml";
            var levelProgressDocument = new XmlDocument();
            levelProgressDocument.Load(_progressPath);
            var levelProgress = levelProgressDocument.SelectSingleNode("LevelProgress");
            ArgumentParser parser = new ArgumentParser("LevelProgressManager");
            var finished = parser.TryParse<bool>(levelProgress.Attributes["finished"].Value, bool.TryParse);
            if (finished) {
                var score = parser.TryParse<int>(levelProgress.Attributes["score"].Value, int.TryParse);
                HighScore = score;
                LevelFinishManager.Instance.triggerOnLevelFinish(score);
            } else {
                HighScore = -1;
            }

            LevelFinishManager.Instance.onLevelFinished += saveProgress;
        }

        private void saveProgress(int score, bool _) {
            if (score < HighScore) return;
            var levelProgressDocument = new XmlDocument();
            levelProgressDocument.Load(_progressPath);
            var levelProgress = levelProgressDocument.SelectSingleNode("LevelProgress");
            levelProgress.Attributes["finished"].Value = "true";
            levelProgress.Attributes["score"].Value = score.ToString();
            levelProgressDocument.Save(_progressPath);
        }
    }
}