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
    }

    public void Deactivate()
    {
        gameObject.SetActive(false);
    }

    public void Activate()
    {
        gameObject.SetActive(true);
    }
}
