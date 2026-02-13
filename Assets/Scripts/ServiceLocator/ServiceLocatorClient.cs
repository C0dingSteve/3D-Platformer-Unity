using UnityEngine;

namespace Assets.Scripts.ServiceLocator
{
    public class ServiceLocatorClient : MonoBehaviour
    {
        [SerializeField] private HealthManager _healthManager;
        [SerializeField] private CheckpointController _checkpointController;
        [SerializeField] private UIManager _uiManager;
        [SerializeField] private PlayerController _playerController;

        private GameManager _gameManager;

        private void Awake() => RegisterServices();

        private void RegisterServices()
        {
            ServiceLocator.Register(_healthManager);
            ServiceLocator.Register(_checkpointController);
            ServiceLocator.Register(_playerController);
            ServiceLocator.Register(_uiManager);

            _gameManager = gameObject.AddComponent<GameManager>();
            ServiceLocator.Register(_gameManager);
        }
    }
}