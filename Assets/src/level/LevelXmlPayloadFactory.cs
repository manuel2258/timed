using UnityEngine;

namespace src.level {
    
    /// <summary>
    /// A factory to generate a new LevelXmlPayload
    /// </summary>
    public static class LevelXmlPayloadFactory {

        /// <summary>
        /// Generates a new LevelXmlPayload from a path to a xml file
        /// </summary>
        /// <param name="path"></param>
        public static void generateFromFile(string path) {
            var text = (TextAsset)Resources.Load(path);
            var payload = generateNewLevelXmlPayload();
            payload.levelXml = text.text;
        }
        
        /// <summary>
        /// Generates a new LevelXmlPayload from a string
        /// </summary>
        /// <param name="level"></param>
        public static void generateFromString(string level) {
            var payload = generateNewLevelXmlPayload();
            payload.levelXml = level;
        }
        
        private static LevelXmlPayload generateNewLevelXmlPayload() {
            if (LevelXmlPayload.Instance != null) {
                Object.Destroy(LevelXmlPayload.Instance.gameObject);
            }

            var newLevelPayload = new GameObject {name = "LevelXmlPayload"};
            return newLevelPayload.AddComponent<LevelXmlPayload>();
        }
    }
}