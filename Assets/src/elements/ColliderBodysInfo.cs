using src.misc;

namespace src.elements {
    
    /// <summary>
    /// Singleton that provides information's about the current ColliderBodys
    /// </summary>
    public class ColliderBodysInfo : UnitySingleton<ColliderBodysInfo> {

        public int ColliderBodyCount => transform.childCount;
    }
}