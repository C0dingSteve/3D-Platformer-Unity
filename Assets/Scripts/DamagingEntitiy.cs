using UnityEngine;

public class DamagingEntity : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            DamageableEntity obj = other.GetComponent<DamageableEntity>();
            NullChecker.Check(obj);
            obj?.ApplyDamage(1);
        }
    }
}
