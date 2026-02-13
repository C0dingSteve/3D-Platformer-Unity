using System.Collections;
using Assets.Scripts.ServiceLocator;
using UnityEngine;

public class GameManager: MonoBehaviour
{
    public int Money {get; private set;} = 0;

    private PlayerController _playerController;
    private HealthManager _healthManger;
    private CheckpointController _checkpointController;
    private UIManager _uiManager;

    private void Awake()
    {
        _playerController = ServiceLocator.Get<PlayerController>();  
        _healthManger = ServiceLocator.Get<HealthManager>();
        _checkpointController = ServiceLocator.Get<CheckpointController>();
        _uiManager = ServiceLocator.Get<UIManager>();
    }

    void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;

        _checkpointController.SpawnPoint = _playerController.transform.position;
        _uiManager.InitializeHUD();
    }

    public IEnumerator RespawnCo()
    {
        _playerController.gameObject.SetActive(false);
        CameraController.Instance.CMBrain.enabled = false;

        _uiManager.FadeScreen();
        yield return new WaitForSeconds(1f); // Works better with half the fade duration

        _healthManger.ResetHealth();

        _playerController.transform.position = _checkpointController.SpawnPoint;
        CameraController.Instance.CMBrain.enabled = true;
        _playerController.gameObject.SetActive(true);
    }

    public void SetSpawnPoint(Vector3 spawnPoint) => _checkpointController.SpawnPoint = spawnPoint;

    public void AddMoney(int money) => Money += money;

    public void Respawn() => StartCoroutine(RespawnCo());
}
