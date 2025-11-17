using UnityEngine;

/// <summary>
/// Singleton that does NOT get destroyed when changing scenes.
/// </summary>
/// <typeparam name="T"></typeparam>
public class SingletonPersistent<T> : Singleton<T> where T : MonoBehaviour
{
    protected override void Awake()
    {
        base.Awake();
        DontDestroyOnLoad(gameObject);
    }
}
