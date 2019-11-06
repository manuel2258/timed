using System.Collections.Generic;
using Editor;
using UnityEngine;

namespace src.level.selection {
    
    /// <summary>
    /// A container for levelPacks that store each level
    /// </summary>
    /// <remarks>
    /// To access the levels use the index operator
    /// </remarks>
    public class LevelPack : ScriptableObject {
        
        [field: SerializeField, LabelOverride("PackName")]
        public string PackName { get; private set; }
        
        [field: SerializeField, LabelOverride("Difficulty")]
        public int Difficulty { get; private set; }

        [SerializeField] private List<TextAsset> levels;
        
        public int LevelCount => levels.Count;

        public string this[int index] => levels[index].text;
    }
}