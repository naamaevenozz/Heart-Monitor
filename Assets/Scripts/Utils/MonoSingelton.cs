using UnityEngine;

namespace Utils
{
    /// <summary>
    /// A generic Singleton class for MonoBehaviours.
    /// Example usage: public class GameManager : MonoSingleton<GameManager>
    /// </summary>
    public class MonoSingleton<T> : MonoBehaviour where T : MonoBehaviour
    {
        private static T _instance;

        public static T Instance
        {
            get
            {
                if (_instance != null)
                    return _instance;

                _instance = FindFirstObjectByType<T>();
                if (_instance == null)
                {
                    var singletonObject = new GameObject(typeof(T).Name);
                    _instance = singletonObject.AddComponent<T>();
                }

                return _instance;
            }
        }

        // Ensure no other instances can be created by having the constructor as protected
        protected MonoSingleton() {}
    }
}