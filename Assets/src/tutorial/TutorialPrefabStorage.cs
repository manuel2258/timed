using System.Collections.Generic;
using Editor;
using src.misc;
using UnityEngine;

namespace src.tutorial {
    public class TutorialPrefabStorage : UnitySingleton<TutorialPrefabStorage> {
        
        [field: SerializeField, LabelOverride("TutorialPart")]
        public GameObject TutorialPart { get; private set; }

        [SerializeField] private GameObject frame;
        [SerializeField] private GameObject elementHighlight;
        [SerializeField] private GameObject text;
        
        private readonly Dictionary<HelpDisplayType, GameObject> _helpDisplayPrefabs = 
            new Dictionary<HelpDisplayType, GameObject>();

        private void Start() {
            _helpDisplayPrefabs.Add(HelpDisplayType.Frame, frame);
            _helpDisplayPrefabs.Add(HelpDisplayType.Text, text);
            _helpDisplayPrefabs.Add(HelpDisplayType.ElementHighlight, elementHighlight);
        }

        public GameObject getHelpDisplayByType(HelpDisplayType type) {
            return Instantiate(_helpDisplayPrefabs[type]);
        }
    }
}