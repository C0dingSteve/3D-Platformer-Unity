using UnityEngine;

public class CheckpointManager : MonoBehaviour
{
    private Checkpoint[] _checkpoints;
    void Start()
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
