using UnityEngine;

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
        SendMessageUpwards(nameof(CheckpointManager.DeactivateAllCheckpoints), SendMessageOptions.RequireReceiver);
        _pfx?.SetActive(true); // Must be after SendMessageUpwards

        GameManager.Instance?.SetSpawnPoint(transform.position);
    }

    public void Deactivate()
    {
        _pfx?.SetActive(false);
    }
}