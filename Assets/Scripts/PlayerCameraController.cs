using Cinemachine;
using UnityEngine;

[RequireComponent(typeof(CinemachineBrain))]
public class PlayerCameraController : MonoBehaviour, ICameraService
{
    public CinemachineBrain CMBrain { get; private set; }
        
    private void Awake() => CMBrain = GetComponent<CinemachineBrain>();
}