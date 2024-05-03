using UnityEngine;

public class FactoryMonoObject<T> : IFactory<T>
{
    private readonly GameObject prefab;
    private Transform parent;

    public FactoryMonoObject(GameObject prefab, Transform parent)
    {
        this.prefab = prefab;
        this.parent = parent;

        var newParent = new GameObject();
        newParent.transform.SetParent(parent);
        this.parent = newParent.transform;
        this.parent.name = prefab.name;
    }

    public T CreateObject()
    {
        var returnObject = GameObject.Instantiate(prefab, parent);
        returnObject.SetActive(false);
        returnObject.GetComponent<IPooled>().ReturnedToPool += () => { returnObject.transform.SetParent(parent); };
        return returnObject.GetComponent<T>();
    }
}