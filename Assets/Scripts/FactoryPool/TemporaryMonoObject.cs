using System;
using System.Collections;
using UnityEngine;

public class TemporaryMonoObject : MonoBehaviour, IPooled
{
    [SerializeField] private float deadTime;
    private IPool parentPool;

    public void ReturnToPool()
    {
        ReturnedToPool?.Invoke();

        if (parentPool == null)
        {
            Destroy(gameObject);
            return;
        }

        gameObject.SetActive(false);
        parentPool.Push(this);
    }

    public virtual void Initialize()
    {
        gameObject.SetActive(true);
        StopAllCoroutines();
        StartCoroutine(DeadCoroutine());
    }

    private IEnumerator DeadCoroutine()
    {
        yield return new WaitForSeconds(deadTime);
        ReturnToPool();
    }

    public void SetParentPool<T>(IPool parent) where T : IPooled
    {
        parentPool = parent;
    }

    public event Action ReturnedToPool;
}