using System.Collections;
using System.Collections.Generic;
using System.Linq;
using DG.Tweening;
using UnityEngine;

public class CharacterInvetory : MonoBehaviour
{
    [SerializeField] private InputController inputController;
    [SerializeField] private PlayerHandInventory playerHandInventory;
    [SerializeField] private InterplayListener interplayListener;

    [Header("Item searcher settings")] [SerializeField]
    private float radius;

    [SerializeField] private Transform searchCenter;

    [Header("Hand item settings")] [SerializeField]
    private Transform handTransform;

    [SerializeField] private CharacterStateMachine characterStateMachine;

    [Header("Inventory setting")] [SerializeField]
    private Transform startInventoryPosition;

    [SerializeField] private Vector3 nextItemInterval;

    private PlayerListener playerListener;
    private PlayerHandItem currentHandItem;

    private List<Item> inventoryStorage = new List<Item>();

    private void Awake()
    {
        playerListener = new PlayerListener(radius, searchCenter, transform);
        StartCoroutine(UpdateClosestItem());
        interplayListener.Initialize(playerListener);
        playerHandInventory.Initialize(playerListener);
    }

    private void Update()
    {
        if (inputController.InInteractItem)
        {
            AddNearestItem();
            playerListener.LastObject?.IterplayObject(this);
            AddNearestHandItem();
        }

        if (inputController.IsDropHandItem)
        {
            DropHandItem();
        }
    }

    private void DropHandItem()
    {
        if (currentHandItem != null)
        {
            currentHandItem.IsSelectable = true;
            currentHandItem.ItemDeSelected();
            currentHandItem.transform.SetParent(null);
            characterStateMachine.CharacterAnimationController.SetBool(currentHandItem.CharacterAnimation, false);
            currentHandItem = null;
        }
    }

    public bool IsHandEmpty()
    {
        return currentHandItem == null;
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

    private void AddNearestHandItem()
    {
        if (currentHandItem == null && playerListener.LastHandItem)
        {
            var handItem = playerListener.LastHandItem;
            handItem.IsSelectable = false;
            handItem.transform.SetParent(handTransform);
            handItem.transform.DOKill();
            handItem.transform.DOLocalJump(handItem.AttachPosition, 0.8f, 1, 0.5f);
            handItem.transform.DOLocalRotate(handItem.AttachRotation, 0.5f);
            characterStateMachine.CharacterAnimationController.SetBool(handItem.CharacterAnimation, true);
            handItem.ItemSelected();
            currentHandItem = handItem;
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

    public void AddNewHandItem(PlayerHandItem playerHandItem)
    {
        var handItem = playerHandItem;
        handItem.IsSelectable = false;
        handItem.transform.SetParent(handTransform);
        handItem.transform.DOKill();
        handItem.transform.localPosition = handItem.AttachPosition;
        handItem.transform.localRotation = Quaternion.Euler(handItem.AttachRotation);
        handItem.transform.DOShakeScale(0.4f, 0.3f);
        characterStateMachine.CharacterAnimationController.SetBool(handItem.CharacterAnimation, true);
        handItem.ItemSelected();
        currentHandItem = handItem;
    }

    public PlayerHandItem GetHandItem()
    {
        var returnValue = currentHandItem;
        DropHandItem();

        return returnValue;
    }

    public PlayerHandItem GetHandItemByType(HandItemType handItemType)
    {
        if (currentHandItem == null || currentHandItem.HandItemType != handItemType)
        {
            return null;
        }

        var returnValue = currentHandItem;
        DropHandItem();

        return returnValue;
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
        var lastItem = playerListener.LastItem;
        if (lastItem)
        {
            lastItem.ItemSelected();
            lastItem.transform.SetParent(startInventoryPosition);
            lastItem.transform.DOKill();
            lastItem.transform.DOLocalJump(nextItemInterval * inventoryStorage.Count, 1.4f, 1, 0.7f);
            lastItem.transform.DOLocalRotate(lastItem.InventoryRotation, 0.7f);
            inventoryStorage.Add(lastItem);
            playerListener.UpdateClosestItem();
        }
    }

    #region SearchRegion

    private IEnumerator UpdateClosestItem()
    {
        while (true)
        {
            if (playerListener.LastItem != null)
            {
                yield return new WaitForSeconds(0.3f);
                playerListener.UpdateClosestItem();
            }
            else
            {
                yield return new WaitForSeconds(0.1f);
                playerListener.UpdateClosestItem();
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