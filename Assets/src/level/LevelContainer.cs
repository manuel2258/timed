using System.Collections.Generic;
using src.level.initializing;
using UnityEngine;

namespace src.level {
    
    /// <summary>
    /// A Container to hold the levels meta data and its initializers
    /// </summary>
    public class LevelContainer {
        
        public LevelHeader LevelHeader { get; }
        public Vector2 GravityScale { get; }

        private readonly List<ElementInitializer> _worldInitializers = new List<ElementInitializer>();
        private readonly Dictionary<int, GameObject> _elementIds = new Dictionary<int, GameObject>();
        private bool _alreadyFired;

        public LevelContainer(LevelHeader levelHeader, Vector2 gravityScale) {
            LevelHeader = levelHeader;
            GravityScale = gravityScale;
        }

        /// <summary>
        /// Ads a new initializer
        /// </summary>
        /// <param name="initializer">The to add Initializer</param>
        public void addInitializer(ElementInitializer initializer) {
            _worldInitializers.Add(initializer);
        }

        /// <summary>
        /// Initializes the level
        /// </summary>
        /// <remarks>
        /// This will spawn all the components of the level, make sure to call this in a level Scene 
        /// </remarks>
        public void initializeLevel() {
            if(_alreadyFired) return;
            
            Physics2D.gravity = GravityScale;
            LevelManager.Instance.clearAllLevelChildren();
            _worldInitializers.ForEach(initializer => {
                var gameObject = initializer.initialize();
                _elementIds.Add(initializer.Id, gameObject);
            });
            _alreadyFired = true;
        }

        public GameObject getElementFromId(int id) {
            return _elementIds[id];
        }
    }
}