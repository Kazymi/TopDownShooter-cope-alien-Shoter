using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;

public class ItemSpawner : MonoBehaviour
{
    [SerializeField] private HandItemSpawnConfiguration[] handItemSpawnConfigurations;
    [SerializeField] private ItemSpawnConfiguration[] itemSpawnConfigurations;
    [SerializeField] private DamageableSpawnConfiguration[] damageableSpawnConfigurations;
    [SerializeField] private EffectSpawnConfiguration[] effectSpawnConfigurations;

    private Dictionary<HandItemType, IPool<PlayerHandItem>> handItemStorage =
        new Dictionary<HandItemType, IPool<PlayerHandItem>>();

    private Dictionary<ItemType, IPool<Item>> itemStorage =
        new Dictionary<ItemType, IPool<Item>>();

    private Dictionary<DamageableType, IPool<Damageable>> damageableStorage =
        new Dictionary<DamageableType, IPool<Damageable>>();

    private Dictionary<EffectType, IPool<TemporaryMonoObject>> effectStorage =
        new Dictionary<EffectType, IPool<TemporaryMonoObject>>();

    public static ItemSpawner Instance;


    private void Awake()
    {
        Instance = this;
    }

    private void Start()
    {
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

    public void SpawnNewDamageableItem(DamageableType damageableType, Vector3 spawnPosition, Quaternion spawnRotation)
    {
        StartCoroutine(SpawnDamageableTimer(damageableType, spawnPosition, spawnRotation));
    }

    private IEnumerator SpawnDamageableTimer(DamageableType damageableType, Vector3 spawnPosition,
        Quaternion spawnRotation)
    {
        yield return new WaitForSeconds(10);
        var newDamageableObject = damageableStorage[damageableType].Pull();
        newDamageableObject.transform.SetPositionAndRotation(spawnPosition, spawnRotation);
        newDamageableObject.transform.DOShakeScale(0.4f, 0.3f).OnComplete(() => newDamageableObject.transform.localScale = Vector3.one);
        newDamageableObject.Initialize();
    }

    public void SpawnEffect(EffectType effectType, Vector3 spawnPosition)
    {
        var newEffect = effectStorage[effectType].Pull();
        newEffect.transform.position = spawnPosition;
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

        foreach (var damageable in damageableSpawnConfigurations)
        {
            var factory = new FactoryMonoObject<Damageable>(damageable.Prefab.gameObject, transform);
            damageableStorage.Add(damageable.DamageableType, new Pool<Damageable>(factory, 4));
        }

        foreach (var effectSpawnConfiguration in effectSpawnConfigurations)
        {
            var factory =
                new FactoryMonoObject<TemporaryMonoObject>(effectSpawnConfiguration.Prefab.gameObject, transform);
            effectStorage.Add(effectSpawnConfiguration.effectType, new Pool<TemporaryMonoObject>(factory, 4));
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

[Serializable]
public class DamageableSpawnConfiguration
{
    [field: SerializeField] public DamageableType DamageableType { get; private set; }
    [field: SerializeField] public Damageable Prefab { get; private set; }
}

[Serializable]
public class EffectSpawnConfiguration
{
    [field: SerializeField] public EffectType effectType { get; private set; }
    [field: SerializeField] public TemporaryMonoObject Prefab { get; private set; }
}

public enum EffectType
{
    Hit,
}