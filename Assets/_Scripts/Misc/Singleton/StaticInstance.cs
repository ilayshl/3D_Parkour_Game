using UnityEngine;

/// <summary>
/// A simple static instance of an object.
/// This is used for the Singleton and SingletonPersistent scripts.
/// </summary>
/// <typeparam name="T"></typeparam>
public class StaticInstance<T> : MonoBehaviour where T : MonoBehaviour
{
    public static T Instance { get; private set; }
    protected virtual void Awake() => Instance = this as T;

    protected virtual void OnApplicationQuit()
    {
        Instance = null;
        Destroy(gameObject);
    }
}
