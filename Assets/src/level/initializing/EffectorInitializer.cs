using System.Collections.Generic;
using src.element;
using src.element.effector;
using src.element.effector.effectors;
using UnityEngine;

namespace src.level.initializing {
    
    /// <summary>
    /// Initializes a new Effector
    /// </summary>
    public class EffectorInitializer : ElementInitializer {

        private readonly EffectorType _effectorType;

        private readonly Dictionary<string, string> _parameters;
        
        public EffectorInitializer(EffectorType effectorType, Dictionary<string, string> parameters,
            ElementType elementType, Vector2 position, float angle)
            : base(elementType, position, angle) {
            _effectorType = effectorType;
            _parameters = parameters;
        }

        protected override GameObject getGameObject() {
            return ElementPrefabStorage.Instance.getEffectorByType(_effectorType);
        }

        protected override void callSetupScript(GameObject currentGameObject) {
            var baseEffector = currentGameObject.GetComponent<BaseEffector>();
            InitializeHelper.initializeObject(baseEffector, _parameters);
        }
    }
}