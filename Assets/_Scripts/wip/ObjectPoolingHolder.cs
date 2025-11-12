using System.Collections.Generic;
using UnityEngine;

public class ObjectPoolingHolder<T> where T : MonoBehaviour
{
    private Queue<T> pool;
    public ObjectPoolingHolder(T subject, int limit)
    {
        InitializePool();
        //pool.
    }

    private void InitializePool()
    {
        pool = new();
    }
}
