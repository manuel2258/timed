using System.Collections.Generic;
using src.misc;
using UnityEngine;

namespace src.touch {
    
    /// <summary>
    /// Manages the areas where the Ui Elements should react to touches
    /// </summary>
    public class UiMaskManager : UnitySingleton<UiMaskManager> {

        private readonly HashSet<RectTransform> _masks = new HashSet<RectTransform>();
        private readonly HashSet<RectTransform> _persistentMasks = new HashSet<RectTransform>();
        public bool HasMask => _masks.Count > 0;

        /// <summary>
        /// Adds a mask that will filter touches if there are non persistent masks, otherwise will just pass them through
        /// </summary>
        /// <param name="mask"></param>
        public void addPersistentMask(RectTransform mask) {
            _persistentMasks.Add(mask);
        }
        
        public void addMask(RectTransform mask) {
            _masks.Add(mask);
        }

        public void addMasks(IEnumerable<RectTransform> masks) {
            foreach (var mask in masks) {
                _masks.Add(mask);
            }
        }

        public void removeMask(RectTransform mask) {
            _masks.Remove(mask);
        }
        
        public void removeMasks(IEnumerable<RectTransform> masks) {
            foreach (var mask in masks) {
                _masks.Remove(mask);
            }
        }

        /// <summary>
        /// Checks whether the screen position is in any UiMask
        /// </summary>
        /// <param name="position">The to check screen position</param>
        /// <returns>True if the touch is at least in one mask, otherwise false</returns>
        public bool touchIsInMask(Vector2 position) {
            if (_masks.Count == 0) return true;
            
            foreach (var mask in _persistentMasks) {
                if (RectTransformUtility.RectangleContainsScreenPoint(mask, position,
                    CameraManager.Camera)) {
                    return true;
                }
            }
            
            foreach (var mask in _masks) {
                if (RectTransformUtility.RectangleContainsScreenPoint(mask, position,
                    CameraManager.Camera)) {
                    return true;
                }
            }

            return false;
        }

    }
}