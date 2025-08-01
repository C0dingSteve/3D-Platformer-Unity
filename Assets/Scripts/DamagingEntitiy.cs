using Assets.Scripts.Utility;
using UnityEngine;

public class DamagingEntity : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            DamageableEntity obj = other.GetComponent<DamageableEntity>();
            if (obj.IsNull() != null) obj.ApplyDamage(1);
            obj.LogNullStatus();
        }
    }
}
