using System.Collections;
using System.Collections.Generic;
using System.Linq;
using DG.Tweening;
using UnityEngine;

public class CharacterInvetory : MonoBehaviour
{
    [SerializeField] private InputController inputController;
    [SerializeField] private InterplayListener interplayListener;

    [Header("Item searcher settings")] [SerializeField]
    private float radius;

    [SerializeField] private Transform searchCenter;

    [Header("Inventory setting")] [SerializeField]
    private Transform startInventoryPosition;

    [SerializeField] private Vector3 nextItemInterval;

    private PlayerListener itemSearcher;
    private List<Item> inventoryStorage = new List<Item>();

    private void Awake()
    {
        itemSearcher = new PlayerListener(radius, searchCenter, transform);
        StartCoroutine(UpdateClosestItem());
        interplayListener.Initialize(itemSearcher);
    }

    private void Update()
    {
        if (inputController.InInteractItem)
        {
            AddNearestItem();
            itemSearcher.LastObject?.IterplayObject(this);
        }
    }

    public Item GetItemByType(ItemType itemType)
    {
        if (inventoryStorage.Count == 0) return null;
        for (int i = inventoryStorage.Count - 1; i >= 0; i--)
        {
            var currentItem = inventoryStorage[i];
            if (currentItem.ItemType == itemType)
            {
                currentItem.transform.parent = null;
                inventoryStorage.Remove(currentItem);
                currentItem.ItemDeSelected();
                SortStorage();
                return currentItem;
            }
        }

        return null;
    }

    private void SortStorage()
    {
        for (int i = 0; i < inventoryStorage.Count; i++)
        {
            inventoryStorage[i].transform.DOKill();
            inventoryStorage[i].transform.DOLocalMove(nextItemInterval * i, 0.5f);
            inventoryStorage[i].transform.DOLocalRotate(inventoryStorage[i].InventoryRotation, 0.4f);
        }
    }

    public Item GetLastItem()
    {
        if (inventoryStorage.Count > 0)
        {
            var last = inventoryStorage.Last();
            last.transform.parent = null;
            inventoryStorage.Remove(last);
            last.ItemDeSelected();
            return last;
        }

        return null;
    }

    public void AddNearestItem()
    {
        var lastItem = itemSearcher.LastItem;
        if (lastItem)
        {
            lastItem.ItemSelected();
            lastItem.transform.SetParent(startInventoryPosition);
            lastItem.transform.DOKill();
            lastItem.transform.DOLocalJump(nextItemInterval * inventoryStorage.Count, 1.4f, 1, 0.7f);
            lastItem.transform.DOLocalRotate(lastItem.InventoryRotation, 0.7f);
            inventoryStorage.Add(lastItem);
            itemSearcher.UpdateClosestItem();
        }
    }

    #region SearchRegion

    private IEnumerator UpdateClosestItem()
    {
        while (true)
        {
            if (itemSearcher.LastItem != null)
            {
                yield return new WaitForSeconds(0.3f);
                itemSearcher.UpdateClosestItem();
            }
            else
            {
                yield return new WaitForSeconds(0.1f);
                itemSearcher.UpdateClosestItem();
            }
        }
    }

    private void OnDrawGizmos()
    {
        if (searchCenter == null) return;
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(searchCenter.position, radius);
    }

    #endregion
}