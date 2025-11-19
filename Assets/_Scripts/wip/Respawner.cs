using UnityEngine;

public class Respawner : MonoBehaviour
{
    void OnCollisionEnter(Collision collision)
    {
        collision.transform.position = new Vector3(0, 1.5f, 0);
    }
}
