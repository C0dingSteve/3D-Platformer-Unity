using Assets.Scripts.ServiceLocator;
using UnityEngine;

public class GlobalServiceStarter : MonoBehaviour
{
    private void Awake()
    {
        ServiceLocator.Get<AudioManager>();
    }
}
