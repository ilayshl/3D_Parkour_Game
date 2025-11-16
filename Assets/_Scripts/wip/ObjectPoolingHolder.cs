using System.Collections.Generic;
using UnityEngine;

public class PoolHolder<T> where T : MonoBehaviour
{
    private Transform holder;
    private Queue<T> pool;
    public PoolHolder(T subject, int limit)
    {
        InitializePool(subject);
        for(int i = 0; i < limit; i++)
        {
            //Instantiate pool
        }
    }

    private void InitializePool(T subject)
    {
        var newObject = new GameObject();
        newObject.name = subject.GetType().ToString();
        holder = newObject.transform;
        pool = new();
    }

    /* public T Create<T>(T subject, Vector3 position, Quaternion direction)
    {
        
        return Instantiate()
    } */
}
