using System.Collections.Generic;
using UnityEngine;

public class Pool<T> : IPool<T> where T : IPooled
{
    private readonly IFactory<T> factory;
    private List<T> storage = new List<T>();

    public Pool(IFactory<T> factory, int startAmount)
    {
        this.factory = factory;
        for (int i = 0; i < startAmount; i++)
        {
            var newObject = factory.CreateObject();
            newObject.SetParentPool<T>(this);
            storage.Add(newObject);
        }
    }

    public T Pull()
    {
        if (storage.Count == 0)
        {
            var newObject = factory.CreateObject();
            newObject.SetParentPool<T>(this);
            storage.Add(newObject);
        }

        var returnObject = storage[0];
        returnObject.Initialize();
        storage.Remove(returnObject);
        return returnObject;
    }

    public void Push(IPooled pooled)
    {
        storage.Add((T) pooled);
        
    }
}