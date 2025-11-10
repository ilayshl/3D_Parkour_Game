using System.Collections;
using UnityEngine;

/// <summary>
/// Responsible for the rotation of the swinging hand towards the hitpoint of Grappable.
/// </summary>
public class SwingingHandRotation : MonoBehaviour
{
    private const int X_ROTATION_LIMIT = 20;
    private const int Y_ROTATION_LIMIT = 30;
    [SerializeField] private float rotationSpeed = 15;
    private Vector3 _wallPos;
    private Coroutine _activeRotation;
    
    public void SetTarget(Vector3 position)
    {
        _wallPos = position;
        StopAllCoroutines();
        _activeRotation = StartCoroutine(nameof(RotateTowards));
    }

    public void ResetTarget()
    {
        _wallPos = Vector3.zero;
    }

    private IEnumerator RotateTowards()
    {
        while (_wallPos != Vector3.zero)
        {
            transform.localRotation = Quaternion.Lerp(transform.localRotation, LookAtNoRoll(), rotationSpeed * Time.deltaTime);
            yield return null;
        }

        _activeRotation = StartCoroutine(nameof(ResetRotation));
    }

    private IEnumerator ResetRotation()
    {
        while (transform.rotation != transform.parent.rotation)
        {
            transform.rotation = Quaternion.Lerp(transform.rotation, transform.parent.rotation, rotationSpeed * Time.deltaTime);
            yield return null;
        }

        _activeRotation = null;
    }

    private Quaternion LookAtNoRoll()
    {
        Vector3 dir = _wallPos - transform.position;
        Vector3 localDir = transform.parent.InverseTransformDirection(dir); //Get vector locally
        Quaternion desiredLocalRot = Quaternion.LookRotation(localDir, Vector3.up);
        Vector3 euler = desiredLocalRot.eulerAngles; //Convert to euler to clamp axes
        euler.x = Utils.NormalizeAngle(euler.x);
        euler.y = Utils.NormalizeAngle(euler.y);
        euler.x = Mathf.Clamp(euler.x, -X_ROTATION_LIMIT, X_ROTATION_LIMIT);
        euler.y = Mathf.Clamp(euler.y, -Y_ROTATION_LIMIT, Y_ROTATION_LIMIT);
        euler.z = 0f; //Reset roll
        return Quaternion.Euler(euler);
    }
}
