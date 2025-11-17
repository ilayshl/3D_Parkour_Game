using UnityEngine;

/// <summary>
/// Singleton that is destroyed when changing scenes.
/// </summary>
/// <typeparam name="T"></typeparam>
public class Singleton<T> : StaticInstance<T> where T : MonoBehaviour
{
    protected override void Awake()
    {
        if (Instance != null) Destroy(Instance.gameObject);
        base.Awake();
    }
}
