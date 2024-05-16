using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;

[RequireComponent(typeof(ItemDropper))]
public class CampFire : InterplayObject
{
    [SerializeField] private ParticleSystem particleSystem;
    [SerializeField] private Transform dropItemPosition;

    private ItemDropper itemDropper;
    private bool isFire = true;
    
    public override void IterplayObject(CharacterInvetory characterInvetory)
    {
        if (isFire)
        {
            var getItem = characterInvetory.GetItemByType(ItemType.Meat);
            if (getItem != null)
            {
                StartCoroutine(MeatRaw(getItem));
                return;
            }

            var fullBucket = characterInvetory.GetHandItemByType(HandItemType.FullBucket);
            if (fullBucket != null)
            {
                isFire = false;
                particleSystem.Stop();
                fullBucket.ReturnToPool();
                var emptyBucket = ItemSpawner.Instance.GetHandItemByType(HandItemType.Bucket);
                characterInvetory.AddNewHandItem(emptyBucket);
            }
        }
        else
        {
            if (characterInvetory.IsHandEmpty())
            {
                particleSystem.Play();
                isFire = true;
            }
        }

    }

    private IEnumerator MeatRaw(Item item)
    {
        item.IsSelectable = false;
        item.ItemSelected();
        item.transform.DOKill();
        item.transform.DOJump(dropItemPosition.position, 1.6f, 1, 0.7f);
        item.transform.DORotateQuaternion(dropItemPosition.rotation, 0.7f);
        yield return new WaitForSeconds(3f);
        item.ReturnToPool();
        itemDropper ??= GetComponent<ItemDropper>();
        itemDropper.DropItem();
    }
}