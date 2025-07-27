using System.Collections;
using UnityEngine;

public abstract class PickupItem : MonoBehaviour
{
    [SerializeField] private string _pickUpName = "Pickup Item";

    [SerializeField] protected GameObject _destroyFX;
    
    [SerializeField] protected GameObject _activeFX; // In case we wanna add some kind of active pfx

    [SerializeField] private bool _canSpin = false; 
    [SerializeField] private float _spinSpeed = 1f;

    private void Start()
    {
        if (_canSpin)
            StartCoroutine(SpinCo());
    }

    private IEnumerator SpinCo() 
    {
        while (_canSpin)
        {
            // Needs fixing, doesn't work with the CoinPickup
            // Likely cos of it's bigger scale
            transform.rotation = Quaternion.Euler(
                transform.rotation.eulerAngles.x,
                transform.rotation.eulerAngles.y + _spinSpeed * Time.deltaTime,
                transform.rotation.eulerAngles.z
            );
            yield return null;
        }
    }

    protected void EndEffect()
    {
        GameObject obj = Instantiate(_destroyFX, transform.position, Quaternion.identity);
        obj.transform.localScale *= 0.3f;
        Destroy(obj, 2f);

        Destroy(gameObject);
    }
}
