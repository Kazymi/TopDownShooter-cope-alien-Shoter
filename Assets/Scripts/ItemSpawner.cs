using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemSpawner : MonoBehaviour
{
    [SerializeField] private HandItemSpawnConfiguration[] handItemSpawnConfigurations;
    [SerializeField] private ItemSpawnConfiguration[] itemSpawnConfigurations;

    private Dictionary<HandItemType, IPool<PlayerHandItem>> handItemStorage =
        new Dictionary<HandItemType, IPool<PlayerHandItem>>();

    private Dictionary<ItemType, IPool<Item>> itemStorage =
        new Dictionary<ItemType, IPool<Item>>();

    public static ItemSpawner Instance;


    private void Start()
    {
        Instance = this;
        Initialize();
        var bucket = GetHandItemByType(HandItemType.Bucket);
        bucket.transform.position = transform.position;
    }

    public PlayerHandItem GetHandItemByType(HandItemType handItemType)
    {
        return handItemStorage[handItemType].Pull();
    }

    public Item GetItemByType(ItemType itemType)
    {
        return itemStorage[itemType].Pull();
    }

    private void Initialize()
    {
        foreach (var handItemSpawnConfiguration in handItemSpawnConfigurations)
        {
            var factory =
                new FactoryMonoObject<PlayerHandItem>(handItemSpawnConfiguration.Prefab.gameObject, transform);
            handItemStorage.Add(handItemSpawnConfiguration.ItemType, new Pool<PlayerHandItem>(factory, 4));
        }

        foreach (var itemSpawnConfiguration in itemSpawnConfigurations)
        {
            var factory = new FactoryMonoObject<Item>(itemSpawnConfiguration.Prefab.gameObject, transform);
            itemStorage.Add(itemSpawnConfiguration.ItemType, new Pool<Item>(factory, 4));
        }
    }
}

[Serializable]
public class HandItemSpawnConfiguration
{
    [field: SerializeField] public HandItemType ItemType { get; private set; }
    [field: SerializeField] public PlayerHandItem Prefab { get; private set; }
}

[Serializable]
public class ItemSpawnConfiguration
{
    [field: SerializeField] public ItemType ItemType { get; private set; }
    [field: SerializeField] public Item Prefab { get; private set; }
}