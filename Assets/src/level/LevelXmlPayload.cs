using src.misc;

namespace src.level {
    
    /// <summary>
    /// A cross scene payload that holds a xml String containing the level data
    /// </summary>
    public class LevelXmlPayload : UnitySingleton<LevelXmlPayload> {

        public string levelXml;
        
        private void Start() {
            DontDestroyOnLoad(gameObject);
        }
    }
}