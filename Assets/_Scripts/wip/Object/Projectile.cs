using UnityEngine;

public class Projectile : MonoBehaviour
{
    [SerializeField] protected float shootForce = 10f;
    private IWeapon _source;

    public virtual void Initialize(IWeapon source, Vector3 position, Quaternion direction)
    {
        _source = source;
        transform.position = position;
        transform.rotation = direction;
    }

    void Start()
    {
        Rigidbody rb = GetComponent<Rigidbody>();
        rb.linearVelocity = transform.forward * shootForce;
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
