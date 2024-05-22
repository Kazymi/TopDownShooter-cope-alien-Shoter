using DG.Tweening;
using UnityEngine;

public class InterplayItemSpawner : InterplayObject
{
    [SerializeField] private Transform spawnPosition;
    [SerializeField] private Vector2 spawnRadius;
    [SerializeField] private ItemType spawnItem;

    private ItemSpawner itemSpawner;
    
    public override void IterplayObject(CharacterInvetory characterInvetory)
    {
        itemSpawner ??= ServiceLocator.GetService<ItemSpawner>();
        var newSpawnPosition = spawnPosition.position;
        var randomX = Random.Range(-spawnRadius.x / 2f, spawnRadius.x / 2f);
        var randomZ = Random.Range(-spawnRadius.y / 2f, spawnRadius.y / 2f);
        newSpawnPosition += new Vector3(randomX, 0, randomZ);
        var newItem =itemSpawner.GetItemByType(spawnItem);
        newItem.transform.position = newSpawnPosition;
        newItem.transform.DOShakeScale(0.3f, 0.4f);
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.white;
        Gizmos.DrawWireCube(spawnPosition.position, new Vector3(spawnRadius.x, 0.1f, spawnRadius.y));
    }
}