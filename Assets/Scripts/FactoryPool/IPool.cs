using System;

public interface IPool
{
    void Push(IPooled pooled);
}