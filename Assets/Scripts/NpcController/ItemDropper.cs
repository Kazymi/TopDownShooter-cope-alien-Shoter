using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;

public class ItemDropper : MonoBehaviour
{
    [SerializeField] private Vector2 radius;
    [SerializeField] private ItemType dropItem;

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireCube(transform.position, new Vector3(radius.x, 0.1f, radius.y));
    }

    public void DropItem()
    {
        var dropPosition = transform.position;
        dropPosition += new Vector3(Random.Range(-radius.x / 2f, radius.x / 2f), 0,
            Random.Range(-radius.y / 2f, radius.y / 2f));

        var droppedItem = ItemSpawner.Instance.GetItemByType(dropItem);
        droppedItem.transform.position = transform.position;
        droppedItem.transform.rotation = Random.rotation;
        droppedItem.IsSelectable = false;
        
        var sequence = DOTween.Sequence();
        sequence.Append(droppedItem.transform.DOJump(dropPosition, 1.7f, 1, 0.9f).SetEase(Ease.Flash));
        sequence.Join(droppedItem.transform.DORotate(Vector3.zero, 0.9f));
        sequence.Append(droppedItem.transform.DOShakeScale(0.3f, 0.4f));
        sequence.OnComplete(() => { droppedItem.IsSelectable = true;});
    }
}