using System;
using System.Collections.Generic;
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

        private readonly Dictionary<EffectorType, GameObject> _typePrefabMap = 
            new Dictionary<EffectorType, GameObject>();

        private void Start() {
            _typePrefabMap.Add(EffectorType.RadialGravityEffector, radialGravity);
            _typePrefabMap.Add(EffectorType.ColorChangerEffector, colorChanger);
        }

        /// <summary>
        /// Returns a copy of GameObject mapped to the type value
        /// </summary>
        /// <param name="type">The to search for type</param>
        /// <returns>A newly initiated GameObject</returns>
        public GameObject getEffectorByType(EffectorType type) {
            _typePrefabMap.TryGetValue(type, out var returnPrefab);
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