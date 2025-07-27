using UnityEngine;

public class HealthPickup : MonoBehaviour
{
    [SerializeField] private int _healAmount;
    [SerializeField] private bool _isFullHeal;

    [SerializeField] private GameObject _pfx;
    
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            _healAmount = _isFullHeal ? HealthManager.Instance.MaxHealth : _healAmount;
            other?.GetComponent<HealthManager>().Heal(_healAmount);

            GameObject obj = Instantiate(_pfx, transform.position, Quaternion.identity);
            obj.transform.localScale *= 0.3f;
            Destroy(obj, 2f);

            Destroy(gameObject);
        }
    }
}
