using System.Collections;
using UnityEngine;

public class GameManager : HealthEventSubscriber
{
    public static GameManager Instance;

    [SerializeField] private UIManager _uiManager;
    [SerializeField] private HealthManager _healthManger;

    [SerializeField] private int _money = 0;
    [SerializeField] private Vector3 _respawnPoint;

    private void Awake()
    {
        if (Instance == null) // Initialize if previous instance doesn't exist
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        // If an instance already exists AND it's not this one, destory this one
        else if (Instance != this)
        {
            Destroy(gameObject);
        }
    }

    void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;

        _respawnPoint = PlayerController.Instance.transform.position;

        _uiManager.UpdateUIHealth(_healthManger.MaxHealth);
        _uiManager.UpdateUIMoney(_money);
    }

    public void SetSpawnPoint(Vector3 spawnPoint)
    {
        _respawnPoint = spawnPoint;
    }

    public void AddMoney(int money)
    {
        _money += money;
        _uiManager.UpdateUIMoney(_money);
    }

    // protected override void HandleDeath()
    // {
    //     Debug.Log(gameObject.name);
    //     StartCoroutine(RespawnCo());
    // }

    public void Respawn()
    {
        StartCoroutine(RespawnCo());
    }

    public IEnumerator RespawnCo()
    {
        PlayerController.Instance.gameObject.SetActive(false);
        CameraController.Instance.CMBrain.enabled = false;

        _uiManager.ActivateScreenFade();
        yield return new WaitForSeconds(1f); // Works better with half the fade duration

        _healthManger.ResetHealth();

        PlayerController.Instance.transform.position = _respawnPoint;
        CameraController.Instance.CMBrain.enabled = true;
        PlayerController.Instance.gameObject.SetActive(true);
    }
}
