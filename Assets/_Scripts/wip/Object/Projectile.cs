using DG.Tweening;
using UnityEngine;

public class Projectile : MonoBehaviour
{
    private Weapon _source;
    private float _shootForce;
    private Vector3 _originalSize;
    Rigidbody _rb;
    TrailRenderer _tr;

    private bool _isBeingDestroyed;

    public virtual void Initialize(Weapon source, Vector3 direction, float shootForce)
    {
        _source = source;
        transform.forward = direction;
        _shootForce = shootForce;
    }

    void Awake()
    {
        _originalSize = transform.localScale;
        _rb = GetComponent<Rigidbody>();
        _tr = GetComponent<TrailRenderer>();
    }

    public void Activate()
    {
        transform.localScale = _originalSize;
        gameObject.SetActive(true);
        _tr.Clear();
        _rb.AddForce(transform.forward * _shootForce, ForceMode.Impulse);
        Invoke(nameof(Destroy), 4f);
    }

    private void Destroy()
    {
        _isBeingDestroyed = true;
        CancelInvoke();
        transform.DOScale(new Vector3(0.1f, 0.1f, 0.1f), .5f).SetEase(Ease.InElastic).OnComplete(ReturnToPool);
    }

    private void ReturnToPool()
    {
        _rb.linearVelocity = Vector3.zero;
        _rb.angularVelocity = Vector3.zero;
        ObjectPoolManager.ReturnObjectToPool(this.gameObject);
        _isBeingDestroyed = false;
    }

    void OnCollisionEnter(Collision collision)
    {
        var tag = collision.gameObject.tag;
        if (tag != "Projectile" && tag != "Player") //When not hitting another projectile or the player
        {
            if (!_isBeingDestroyed)
            {
                Destroy();
                Debug.Log("Destroying bullet");
            }
        }

    }
}
