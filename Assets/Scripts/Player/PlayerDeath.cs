using UnityEngine;
using Assets.Scripts.Utility;

[RequireComponent(typeof(HealthManager))]
public class PlayerDeath : HealthEventSubscriber
{
    [SerializeField] private GameObject _playerDeathFX;
    private PlayerInvincibleEffect _invincibilityEffect;

    protected override void Awake()
    {
        base.Awake();
        _invincibilityEffect = GetComponent<PlayerInvincibleEffect>();
        _invincibilityEffect.LogNullStatus();
    }

    void OnTriggerEnter(Collider coll)
    {
        if (coll.gameObject.CompareTag("KillZone"))
        {
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
