using UnityEngine;
using System.Collections;

namespace UniInventory
{
    public class Singleton<T> : MonoBehaviour where T : MonoBehaviour
    {
        private static T _instance;

        private static object _lock = new object();

        public static T Instance
        {
            get
            {
                if (applicationIsQuitting)
                {
                    Debug.LogWarning("[Singleton] Instance is already destroyed.");
                    return null;
                }

                lock(_lock)
                {
                    if (_instance == null)
                    {
                        _instance = (T)FindObjectOfType(typeof(T));
                        
                        if (FindObjectsOfType(typeof(T)).Length > 1 )
                        {
                            Debug.LogError("[Singleton] There are more than 1 singleton");
                        }

                        if (_instance == null)
                        {
                            GameObject singleton = new GameObject();
                            _instance = singleton.AddComponent<T>();
                            singleton.name = "(singleton)" + typeof(T).ToString();
                            DontDestroyOnLoad(singleton);
                            Debug.Log("[Singleton] An instance is created");
                        }
                        else
                        {
                            Debug.Log("[Singleton] instance already created");
                        }                        
                    }
                }
                return _instance;
            }
        }
        private static bool applicationIsQuitting = false;

        public void OnDestroy()
        {
            applicationIsQuitting = true;
        }
    }
}