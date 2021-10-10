using UnityEngine;

namespace PabloLario.Managers
{
    public class Singleton<T> : MonoBehaviour where T : Component
    {
        private static T _instance;
        public static T Instance
        {
            get
            {
                if (_instance == null)
                {
                    _instance = FindObjectOfType<T>();
                    if (_instance == null)
                    {
                        GameObject obj = new GameObject();
                        obj.name = typeof(T).Name;
                        _instance = obj.AddComponent<T>();
                    }
                }
                return _instance;
            }
        }

        /* In order to use Awake, do it the next way
        * protected override void Awake()
        * {
        *     base.Awake();
        *     //Your code goes here
        * }
        * */

        protected virtual void Awake()
        {
            if (_instance == null)
            {
                _instance = this as T;
                DontDestroyOnLoad(gameObject);
            }
            else if (_instance != this as T)
            {
                Destroy(gameObject);
            }
            else { DontDestroyOnLoad(gameObject); }
        }
    }
}