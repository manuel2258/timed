using System;
using System.Collections.Generic;
using System.Reflection;
using src.element;
using src.element.collider_body;
using src.element.effector;
using UnityEngine;

namespace src.level.initializing {
    
    /// <summary>
    /// Initializes a new Effector
    /// </summary>
    public class ColliderBodyInitializer : ElementInitializer {
        
        private readonly Dictionary<string, string> _parameters;
        
        public ColliderBodyInitializer(Dictionary<string, string> parameters,
            ElementType elementType, int id, Vector2 position, float angle)
            : base(elementType, id, position, angle) {
            _parameters = parameters;
        }

        protected override void callSetupScript(GameObject currentGameObject) {
            var colliderBody = currentGameObject.GetComponent<ISetupAble>();
            InitializeHelper.initializeObject(colliderBody, _parameters);
        }
    }
}