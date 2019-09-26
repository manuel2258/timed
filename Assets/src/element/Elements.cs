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
        /// Filters a Stream of Collider2D for ColliderBodys with the right color
        /// </summary>
        /// <param name="colliders">The to filter Collider2Ds</param>
        /// <param name="color">The to filter for ElementColor</param>
        /// <returns>The Filter ColliderBodySteam</returns>
        public static IEnumerable<ColliderBody> filterForColorFromColliders(IEnumerable<Collider2D> colliders, ElementColor color) {
            return colliders.Where(collider => collider != null)
                .Select(collider => collider.GetComponent<ColliderBody>())
                .Where(colliderBody => colliderBody != null)
                .Where(colliderBody => colliderBody.Color == color);
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