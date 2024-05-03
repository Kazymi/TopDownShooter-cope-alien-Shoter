using System.Collections;


public interface IPool<T> : IPool where T : IPooled
{
    T Pull();
}