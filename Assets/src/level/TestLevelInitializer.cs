using UnityEngine;

namespace src.level {
    public class TestLevelInitializer : MonoBehaviour {

        public string filename;
        
        private void Start() {
            if (LevelXmlPayload.Instance != null) return;
            
            LevelXmlPayloadFactory.generateFromFile(filename);
        }
    }
}