using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class GravityScaler : MonoBehaviour
{
    [SerializeField] private float gravityScale = 1.0f;

    private Rigidbody _rb;

    void OnEnable()
    {
        _rb = GetComponent<Rigidbody>();
        _rb.useGravity = false; // isable default gravity
    }

    void FixedUpdate()
    {
        //Apply a custom gravity force based on global gravity and your scale
        Vector3 gravity = Physics.gravity * gravityScale;
        _rb.AddForce(gravity, ForceMode.Acceleration);
    }
}
