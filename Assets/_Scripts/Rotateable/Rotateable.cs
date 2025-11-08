using UnityEngine;

public class Rotateable : MonoBehaviour
{
    public void LookTowards(Quaternion direction)
    {
        transform.rotation = direction;
    }
}
