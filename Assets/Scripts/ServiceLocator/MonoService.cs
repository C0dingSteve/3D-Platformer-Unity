using UnityEngine;

namespace Assets.Scripts.ServiceLocator
{
    public abstract class MonoService<T>: MonoBehaviour where T:Component
    {
        [SerializeField] protected bool _isGlobal = false;

        protected virtual void Awake()
        {
            if(_isGlobal)
            {
                DontDestroyOnLoad(gameObject);
            }

            ServiceLocator.Register<T>(this as T, _isGlobal);
        }
    }
}