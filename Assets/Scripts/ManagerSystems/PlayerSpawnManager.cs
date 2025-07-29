using UnityEngine;

public class PlayerSpawnManager : MonoBehaviour
{
    public Vector3 SpawnPoint { get; set; } = Vector3.zero;

    private Checkpoint[] _checkpoints;

    private void Awake()
    {
        _checkpoints = gameObject.GetComponentsInChildren<Checkpoint>();
        if (_checkpoints.Length <= 0) Debug.Log("No checkpoints found");
    }

    public void DeactivateAllCheckpoints()
    {
        foreach (Checkpoint item in _checkpoints)
        {
            item.Deactivate();
        }
    }
}
