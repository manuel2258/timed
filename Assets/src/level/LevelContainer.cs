using System.Collections.Generic;
using src.level.initializing;

namespace src.level {
    
    /// <summary>
    /// A Container to hold the levels meta data and its initializers
    /// </summary>
    public class LevelContainer {
        
        public string Name { private set; get; }

        private readonly List<ElementInitializer> _initializers = new List<ElementInitializer>();

        public LevelContainer(string name) {
            Name = name;
        }

        /// <summary>
        /// Ads a new initializer
        /// </summary>
        /// <param name="initializer">The to add Initializer</param>
        public void addInitializer(ElementInitializer initializer) {
            _initializers.Add(initializer);
        }

        /// <summary>
        /// Initializes the level
        /// </summary>
        /// <remarks>
        /// This will spawn all the components of the level, make sure to call this in a level Scene 
        /// </remarks>
        public void initializeLevel() {
            _initializers.ForEach(initializer => initializer.initialize());
        }
    }
}