using System;
using UnityEngine;

namespace src.level {
    public class TestLevelInitializer : MonoBehaviour {
        private void Start() {
            LevelXmlPayloadFactory.generateFromFile("sample_level");
        }
    }
}