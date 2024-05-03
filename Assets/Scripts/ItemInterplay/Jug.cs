using UnityEngine;

public class Jug : InterplayObject
{
    public override void IterplayObject(CharacterInvetory characterInvetory)
    {
        var item = characterInvetory.GetHandItemByType(HandItemType.Bucket);
        if (item)
        {
            item.ReturnToPool();
            var newFullBucket = ItemSpawner.Instance.GetHandItemByType(HandItemType.FullBucket);
            characterInvetory.AddNewHandItem(newFullBucket);
        }
    }
}