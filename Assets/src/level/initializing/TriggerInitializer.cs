using System.Collections.Generic;
using src.element;
using src.element.triggers;
using UnityEngine;

namespace src.level.initializing {
    
    /// <summary>
    /// Initializes a new Trigger
    /// </summary>
    public class TriggerInitializer : ElementInitializer {

        private readonly TriggerType _triggerType;

        private readonly Dictionary<string, string> _parameters;
        
        public TriggerInitializer(TriggerType triggerType, Dictionary<string, string> parameters,
            ElementType elementType, Vector2 position, float angle)
            : base(elementType, position, angle) {
            _triggerType = triggerType;
            _parameters = parameters;
        }

        protected override GameObject getGameObject() {
            return ElementPrefabStorage.Instance.getTriggerByType(_triggerType);
        }

        protected override void callSetupScript(GameObject currentGameObject) {
            var baseEffector = currentGameObject.GetComponent<BaseTrigger>();
            InitializeHelper.initializeObject(baseEffector, _parameters);
        }
    }
}