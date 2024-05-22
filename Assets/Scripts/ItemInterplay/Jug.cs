using UnityEngine;

public class Jug : InterplayObject
{
    private ItemSpawner itemSpawner;
    public override void IterplayObject(CharacterInvetory characterInvetory)
    {
        itemSpawner ??= ServiceLocator.GetService<ItemSpawner>();
        var item = characterInvetory.GetHandItemByType(HandItemType.Bucket);
        if (item)
        {
            item.ReturnToPool();
            var newFullBucket = itemSpawner.GetHandItemByType(HandItemType.FullBucket);
            characterInvetory.AddNewHandItem(newFullBucket);
        }
    }
}