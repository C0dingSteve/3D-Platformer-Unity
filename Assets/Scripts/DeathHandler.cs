using UnityEngine;
using Assets.Scripts.Utility;

[RequireComponent(typeof(HealthManager))]
public class DeathHandler : HealthEventSubscriber
{
    [SerializeField] private GameObject _playerDeathFX;
    private InvincibilityEffect _invincibilityEffect;
    private HealthManager _healthManager;

    private void Awake()
    {
        _invincibilityEffect = GetComponent<InvincibilityEffect>();
        _invincibilityEffect.LogNullStatus();

        _healthManager = GetComponent<HealthManager>();
        _healthManager.LogNullStatus();
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
