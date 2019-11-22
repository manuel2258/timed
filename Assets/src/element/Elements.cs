using System;
using System.Collections.Generic;
using System.Linq;
using src.element.collider_body;
using src.element.effector.effectors;
using src.time.timeline;
using UnityEngine;

namespace src.element {
    public static class Elements {
        public static void executeVisualChange(IVisualStateAble that, Action changeFunction) {
            var beforeState = that.getCurrentState();
            changeFunction.Invoke();
            var afterState = that.getCurrentState();
            ReplayTimeline.Instance.addVisualEvent(that, beforeState, afterState);
        }

        /// <summary>
        /// Filters a Array of Collider2D for ColliderBodys with the right color
        /// </summary>
        /// <param name="colliders">The to filter Collider2Ds</param>
        /// <param name="color">The to filter for ElementColor</param>
        /// <returns>The Filter ColliderBody Array</returns>
        public static ColliderBody[] filterForColorFromColliders(Collider2D[] colliders, ElementColor color) {
            // Bitmask of to return ColliderBodys
            int mask = 0;
            int counter = 0;
            
            // Array of all casted ColliderBodys
            var allColliderBodys = new ColliderBody[colliders.Length];
            for (var i = 0; i < colliders.Length; i++) {
                var currentCollider = colliders[i];
                var colliderBody = currentCollider.GetComponent<ColliderBody>();
                allColliderBodys[i] = colliderBody;
                if(colliderBody == null) continue;
                if(colliderBody.Color != color) continue;
                counter++;
                
                // If colliderbody has right color enable it bits at position i
                mask |= 1 << i;
            }

            var returnColliders = new ColliderBody[counter];
            counter = 0;
            for (var i = 0; i < colliders.Length; i++) {
                // Check if bit is enabled
                if ((mask & 1 << i) == 0) continue;
                returnColliders[counter] = allColliderBodys[i];
                counter++;
            }

            return returnColliders;
        }
        
        /// <summary>
        /// Filters a Stream of RayCastHits for ColliderBodys with the right color
        /// </summary>
        /// <param name="colliders">The to filter Collider2Ds</param>
        /// <param name="color">The to filter for ElementColor</param>
        /// <returns>The Filter ColliderBodySteam</returns>
        public static IEnumerable<ColliderBody> filterForColorFromRaycastHits(IEnumerable<RaycastHit2D> colliders, ElementColor color) {
            return colliders.Where(raycast => raycast.collider != null)
                .Select(collider => collider.collider.GetComponent<ColliderBody>())
                .Where(colliderBody => colliderBody != null)
                .Where(colliderBody => colliderBody.Color == color);
        }
    }
}