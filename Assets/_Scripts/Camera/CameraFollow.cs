using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    [SerializeField] Transform playerObject;

    void LateUpdate()
    {
        transform.position = playerObject.position;
        transform.rotation = playerObject.rotation;
    }
}
