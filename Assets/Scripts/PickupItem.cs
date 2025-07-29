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
            transform.Rotate(Vector3.up * Time.deltaTime * _spinSpeed);
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
