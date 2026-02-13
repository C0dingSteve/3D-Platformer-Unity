using UnityEngine;
using Assets.Scripts.ServiceLocator;

public class Checkpoint : MonoBehaviour
{
    [SerializeField] private GameObject _pfx;
 
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
            Activate();
    }

    private void Activate()
    {
        SendMessageUpwards(nameof(CheckpointController.DeactivateAllCheckpoints), SendMessageOptions.RequireReceiver);
        _pfx?.SetActive(true); // After SendMessageUpwards, need to overwrite to activate the real checkpoint

        ServiceLocator.Get<GameManager>().SetSpawnPoint(transform.position);
    }

    public void Deactivate() => _pfx?.SetActive(false);
}