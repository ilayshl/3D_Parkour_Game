using UnityEngine;

/// <summary>
/// Follows the target object. Defaults to the player via the prefab.
/// </summary>
public class CameraFollow : MonoBehaviour
{
    [SerializeField] Transform target;

    void Update()
    {
        transform.position = target.position;
        transform.rotation = target.rotation;
    }
}
