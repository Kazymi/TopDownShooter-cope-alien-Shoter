using System;

public interface IPooled
{
    void ReturnToPool();
    void Initialize();
    void SetParentPool<T>(IPool parent) where T : IPooled;
    event Action ReturnedToPool;
}