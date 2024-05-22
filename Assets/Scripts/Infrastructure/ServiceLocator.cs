using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class ServiceLocator
{
    private static Dictionary<Type, object> storage = new Dictionary<Type, object>();

    public static void Subscribe<T>(object value)
    {
        storage.Add(typeof(T), value);
    }

    public static void UnSubscribe<T>()
    {
        if (storage.ContainsKey(typeof(T)))
        {
            storage.Remove(typeof(T));
        }
    }

    public static T GetService<T>()
    {
        if (storage.ContainsKey(typeof(T)))
        {
            return (T) storage[typeof(T)];
        }
        else
        {
            throw new Exception($"{typeof(T)} not found ");
        }
    }
}