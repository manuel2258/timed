using Editor;
using UnityEngine;

namespace src.misc {
    public class GlobalGameState : UnitySingleton<GlobalGameState> {
        [field: SerializeField, LabelOverride("IsInGame")]
        public bool IsInGame { get; private set; }
    }
}