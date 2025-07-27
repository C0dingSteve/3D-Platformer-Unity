using UnityEngine;

[RequireComponent(typeof(HealthManager))]
public class DamageableEntity : MonoBehaviour
{
    [SerializeField] private InvincibilityEffect _invincibilityEffect;

    private void Awake()
    {
        if (!NullChecker.Check(_invincibilityEffect))
            _invincibilityEffect = GetComponent<InvincibilityEffect>();
    }

    public void ApplyDamage(int amount)
    {
        if (!NullChecker.Check(_invincibilityEffect) && _invincibilityEffect.IsCurrentlyInvincible)
            return;

        GetComponent<HealthManager>().TakeDamage(amount);
    }
}
