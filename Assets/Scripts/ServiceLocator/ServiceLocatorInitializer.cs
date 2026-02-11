using UnityEngine;
using UnityEngine.SceneManagement;

namespace Assets.Scripts.ServiceLocator
{
    public static class ServiceLocatorInitializer
    {
        // Won't work for additive loading
        [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.BeforeSceneLoad)]
        private static void Init()
        {
            SceneManager.sceneUnloaded += _ =>
            {
                ServiceLocator.ClearLocalServices();
                Debug.Log($"Local Services Cleared via SceneManager event.");
            };
        }
    }
}