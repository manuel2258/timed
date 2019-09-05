using src.misc;

namespace src.element.collider_body {
    
    /// <summary>
    /// Singleton that provides information's about the current ColliderBodys
    /// </summary>
    public class ColliderBodysInfo : UnitySingleton<ColliderBodysInfo> {

        public int ColliderBodyCount => transform.childCount;
    }
}