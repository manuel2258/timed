using Editor;
using src.level.parsing;
using src.misc;
using src.tutorial;
using UnityEngine;

namespace src.level {
    public class LevelManager : UnitySingleton<LevelManager> {
        [field: SerializeField, LabelOverride("LevelRoot")]
        public Transform LevelRoot { get; private set; }
        
        [field: SerializeField, LabelOverride("TutorialRoot")]
        public RectTransform TutorialRoot { get; private set; }
        
        public LevelContainer CurrentLevel { get; private set; }
        public TutorialContainer CurrentTutorial { get; private set; }

        private void Start() {
            if (GlobalGameState.Instance.IsInGame) {
                CurrentLevel = LevelXmlParser.parseLevelFromXmlString(LevelXmlPayload.Instance.levelXml);
                CurrentTutorial = TutorialXmlParser.parseTutorialFromXmlString(LevelXmlPayload.Instance.levelXml);
                CurrentLevel.initializeLevel();
                CurrentTutorial.initializeTutorial();
                CurrentTutorial.onTutorialFinished += () => Debug.Log("TutorialFinished");
            }
        }
        
        public void clearAllLevelChildren() {
            for (int i = 0; i < LevelRoot.childCount; i++) {
                Destroy(LevelRoot.GetChild(i).gameObject);
            }
            
            for (int i = 0; i < TutorialRoot.childCount; i++) {
                Destroy(TutorialRoot.GetChild(i).gameObject);
            }
        }
    }
}