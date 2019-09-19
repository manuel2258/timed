using System;
using UnityEngine;

namespace src.level {
    public class TestLevelInitializer : MonoBehaviour {

        public string filename;
        
        private void Start() {
            LevelXmlPayloadFactory.generateFromFile(filename);
        }
    }
}