using System.Collections.Generic;
using src.element.effector;
using src.element.triggers;
using src.misc;
using UnityEngine;

namespace src.level.initializing {
    
    /// <summary>
    /// A storage to link ElementPrefabs
    /// </summary>
    public class ElementPrefabStorage : UnitySingleton<ElementPrefabStorage> {

        public GameObject wall;
        public GameObject colliderBody;
        
        public GameObject radialGravity;
        public GameObject colorChanger;
        public GameObject teleporter;

        public GameObject goal;

        private readonly Dictionary<EffectorType, GameObject> _effectorTypePrefabMap = 
            new Dictionary<EffectorType, GameObject>();
        
        private readonly Dictionary<TriggerType, GameObject> _triggerTypePrefabMap = 
            new Dictionary<TriggerType, GameObject>();

        private void Start() {
            _effectorTypePrefabMap.Add(EffectorType.RadialGravityEffector, radialGravity);
            _effectorTypePrefabMap.Add(EffectorType.ColorChangerEffector, colorChanger);
            _effectorTypePrefabMap.Add(EffectorType.TeleporterEffector, teleporter);
            
            _triggerTypePrefabMap.Add(TriggerType.Goal, goal);
        }

        /// <summary>
        /// Returns a copy of GameObject mapped to the EffectorType value
        /// </summary>
        /// <param name="type">The to search for type</param>
        /// <returns>A newly initiated GameObject</returns>
        public GameObject getEffectorByType(EffectorType type) {
            _effectorTypePrefabMap.TryGetValue(type, out var returnPrefab);
            return Instantiate(returnPrefab);
        }
        
        /// <summary>
        /// Returns a copy of GameObject mapped to the TriggerType value
        /// </summary>
        /// <param name="type">The to search for type</param>
        /// <returns>A newly initiated GameObject</returns>
        public GameObject getTriggerByType(TriggerType type) {
            _triggerTypePrefabMap.TryGetValue(type, out var returnPrefab);
            return Instantiate(returnPrefab);
        }

        /// <summary>
        /// Returns a copy of the Wall GameObject
        /// </summary>
        /// <returns></returns>
        public GameObject getWall() {
            return Instantiate(wall);
        }

        /// <summary>
        /// Return a copy of the ColliderBody GameObject
        /// </summary>
        /// <returns></returns>
        public GameObject getColliderBody() {
            return Instantiate(colliderBody);
        }
    }
}