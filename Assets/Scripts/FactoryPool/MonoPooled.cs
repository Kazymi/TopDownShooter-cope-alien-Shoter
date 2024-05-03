using System;
using UnityEngine;

public class MonoPooled : MonoBehaviour, IPooled
{
    private IPool parentPool;

    public void ReturnToPool()
    {
        gameObject.SetActive(false);
        parentPool.Push(this);
        ReturnedToPool?.Invoke();
    }

    public virtual void Initialize()
    {
        gameObject.SetActive(true);
    }

    public void SetParentPool<T>(IPool parent) where T : IPooled
    {
        parentPool = parent;
    }

    public event Action ReturnedToPool;
}