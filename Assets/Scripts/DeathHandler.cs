using UnityEngine;

[RequireComponent(typeof(HealthManager))]
public class DeathHandler : HealthEventSubscriber
{
    [SerializeField] private GameObject _playerDeathFX;
    private InvincibilityEffect _invincibilityEffect;
    private HealthManager _healthManager;

    private void Awake()
    {
        _invincibilityEffect = GetComponent<InvincibilityEffect>();
        NullChecker.Check(_invincibilityEffect);

        _healthManager = GetComponent<HealthManager>();
        NullChecker.Check(_healthManager);
    }

    void OnTriggerEnter(Collider coll)
    {
        if (coll.gameObject.CompareTag("KillZone"))
        {
            //HealthManager.Instance.Kill();
            GetComponent<HealthManager>().Kill();
        }
    }

    protected override void HandleDeath()
    {
        // No need to deactivate, the package script automatically does that
        Vector3 pos = new(transform.position.x, 0.1f, transform.position.z);
        Instantiate(_playerDeathFX, pos, Quaternion.identity).SetActive(true); //disabled by default

        _invincibilityEffect.StopEffect();
    }
}
