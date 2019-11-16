namespace src.misc {
    
    /// <summary>
    /// Simple Singleton implementation to get static references
    /// </summary>
    /// <typeparam name="T">The reference type</typeparam>
    public abstract class Singleton<T> where T : class, new() {
        private static T _instance;
        public static T Instance => _instance ?? (_instance = new T());
    }
}