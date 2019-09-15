using System.Collections.Generic;
using src.misc;
using UnityEngine;

namespace src.element.collider_body {
    
    /// <summary>
    /// Singleton that provides information's about the current ColliderBodys
    /// </summary>
    public class ColliderBodiesInfo : UnitySingleton<ColliderBodiesInfo> {

        public int ColliderBodyCount => transform.childCount;

        private List<ColliderBody> _colliderBodies;

        public List<ColliderBody> ColliderBodies {
            get {
                if (_colliderBodies != null) return _colliderBodies;
                _colliderBodies = new List<ColliderBody>();
                for (int i = 0; i < transform.childCount; i++)
                {
                    _colliderBodies.Add(transform.GetChild(i).GetComponent<ColliderBody>());
                }
                return _colliderBodies;
            }
        }
    }
}