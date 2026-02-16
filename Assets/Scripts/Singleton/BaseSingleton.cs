using Assets.Scripts.ServiceLocator;
using UnityEngine;

// NOTE:- Only job is to ensure single instance
// Must be inherited by global services
public abstract class BaseSingleton<T>: MonoBehaviour, IGlobalService where T: MonoBehaviour
{
    protected static T _instance;

    protected virtual void Awake()
    {
        if(_instance == null)
        {
            _instance = this as T;
            DontDestroyOnLoad(gameObject);
            Initialize();
        }
        else if(_instance != this)
        {
            Destroy(gameObject);
        }
    }

    // Optional override for setup logic instead of awake
    protected virtual void Initialize() { }
}