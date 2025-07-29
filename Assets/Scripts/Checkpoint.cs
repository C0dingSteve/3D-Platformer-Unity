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
        SendMessageUpwards(nameof(PlayerSpawnManager.DeactivateAllCheckpoints), SendMessageOptions.RequireReceiver);
        _pfx?.SetActive(true); // After SendMessageUpwards, need to overwrite to activate the real checkpoint

        GameManager.Instance?.SetSpawnPoint(transform.position);
    }

    public void Deactivate()
    {
        _pfx?.SetActive(false);
    }
}