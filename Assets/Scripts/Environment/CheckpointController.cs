using Assets.Scripts.ServiceLocator;
using UnityEngine;

public class CheckpointController : MonoBehaviour
{
    public Vector3 SpawnPoint { get; set; } = Vector3.zero;

    private Checkpoint[] _checkpoints;

    private void Awake()
    {
        _checkpoints = gameObject.GetComponentsInChildren<Checkpoint>();
        if (_checkpoints.Length <= 0) Debug.Log("No checkpoints found");
        ServiceLocator.Register(this);
    }

    public void DeactivateAllCheckpoints()
    {
        foreach (Checkpoint item in _checkpoints)
        {
            item.Deactivate();
        }
    }
}
