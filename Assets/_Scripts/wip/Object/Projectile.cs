using DG.Tweening;
using UnityEngine;

public class Projectile : MonoBehaviour
{
    private float _shootForce;
    private Weapon _source;

    public virtual void Initialize(Weapon source, Vector3 position, Quaternion direction, float shootForce)
    {
        _source = source;
        transform.position = position;
        transform.rotation = direction;
        _shootForce = shootForce;
    }

    void Start()
    {
        Rigidbody rb = GetComponent<Rigidbody>();
        rb.linearVelocity = transform.forward * _shootForce;
        Invoke(nameof(Destroy), 5f);
    }

    public void Activate()
    {
        gameObject.SetActive(true);
    }

    private void Destroy()
    {
        transform.DOScale(Vector3.zero, .2f).SetEase(Ease.InElastic).OnComplete(ReturnToPool);
    }
    private void ReturnToPool()
    {
        ObjectPoolManager.ReturnObjectToPool(this.gameObject);
    }
}
