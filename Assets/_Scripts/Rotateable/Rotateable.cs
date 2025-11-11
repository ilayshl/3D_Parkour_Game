using UnityEngine;

/// <summary>
/// Objects that need to be identified by GetComponent<> and have rotation logic will derive from this script.
/// </summary>
public class Rotateable : MonoBehaviour
{
    public void LookTowards(Quaternion direction)
    {
        transform.rotation = direction;
    }
}
