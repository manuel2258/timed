using src.misc;
using UnityEngine;

namespace src.elements.effectors {
    
    /// <summary>
    /// The base of each Effector
    /// </summary>
    public class BaseEffector : MonoBehaviour {
        public float touchHitBox = 1;

        private void Update() {
            if (TouchManager.Instance.isTouched(transform.position, touchHitBox)) {
                Debug.Log("TEST");
            }
        }
    }
}