using UnityEngine;
using Assets.Scripts.ServiceLocator;

public class Checkpoint : MonoBehaviour
{
    [SerializeField] private GameObject _pfx;    
    private GameManager _gameManager;

    private void Awake() => _gameManager = ServiceLocator.Get<GameManager>();
    
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
            Activate();
    }

    private void Activate()
    {
        SendMessageUpwards(nameof(CheckpointController.DeactivateAllCheckpoints), SendMessageOptions.RequireReceiver);
        _pfx?.SetActive(true); // After SendMessageUpwards, need to overwrite to activate the real checkpoint

        _gameManager.SetSpawnPoint(transform.position);
    }

    public void Deactivate() => _pfx?.SetActive(false);
}