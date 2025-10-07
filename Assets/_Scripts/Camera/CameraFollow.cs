using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    [SerializeField] Transform playerObject;

    void Update()
    {
        transform.position = playerObject.position;
    }
}
