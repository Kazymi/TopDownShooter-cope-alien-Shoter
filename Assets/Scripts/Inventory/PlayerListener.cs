using System.Collections.Generic;
using UnityEngine;

public class PlayerListener
{
    private readonly float radius;
    private readonly Transform center;
    private readonly Transform playerTransform;

    public InterplayObject LastObject { get; set; }
    public Item LastItem { get; private set; }
    public PlayerHandItem LastHandItem { get; private set; }

    public PlayerListener(float radius, Transform center, Transform playerTransform)
    {
        this.radius = radius;
        this.center = center;
        this.playerTransform = playerTransform;
    }

    #region HandItem

    public void UpdateClosestHandItem()
    {
        var newFoundItem = GetClosestHandItem();
        if (newFoundItem != null)
        {
            if (newFoundItem != LastHandItem)
            {
                if (LastHandItem != null)
                {
                    LastHandItem.SetItemOutline(false);
                }

                newFoundItem.SetItemOutline(true);
                LastHandItem = newFoundItem;
            }
        }
        else
        {
            if (LastHandItem != null)
            {
                LastHandItem.SetItemOutline(false);
            }

            LastHandItem = null;
        }
    }

    private PlayerHandItem GetClosestHandItem()
    {
        var foundItems = Physics.OverlapSphere(center.position, radius);
        var sortedItems = new List<PlayerHandItem>();
        foreach (var foundItem in foundItems)
        {
            var item = foundItem.GetComponent<PlayerHandItem>();
            if (item && item.IsSelectable)
            {
                sortedItems.Add(item);
            }
        }

        if (sortedItems.Count != 0)
        {
            return GetClosestHandItem(sortedItems);
        }
        else
        {
            return null;
        }
    }

    private PlayerHandItem GetClosestHandItem(List<PlayerHandItem> items)
    {
        var closestItem = items[0];
        var lastFoundClosestDistance = float.MaxValue;

        for (int i = 0; i < items.Count; i++)
        {
            var distance = Vector3.Distance(playerTransform.position, items[i].transform.position);
            if (lastFoundClosestDistance > distance)
            {
                lastFoundClosestDistance = distance;
                closestItem = items[i];
            }
        }

        return closestItem;
    }

    #endregion

    #region Item

    public void UpdateClosestItem()
    {
        var newFoundItem = GetClosestItem();
        if (newFoundItem != null)
        {
            if (newFoundItem != LastItem)
            {
                if (LastItem != null)
                {
                    LastItem.SetItemOutline(false);
                }

                newFoundItem.SetItemOutline(true);
                LastItem = newFoundItem;
            }
        }
        else
        {
            if (LastItem != null)
            {
                LastItem.SetItemOutline(false);
            }

            LastItem = null;
        }
    }

    private Item GetClosestItem()
    {
        var foundItems = Physics.OverlapSphere(center.position, radius);
        var sortedItems = new List<Item>();
        foreach (var foundItem in foundItems)
        {
            var item = foundItem.GetComponent<Item>();
            if (item && item.IsSelectable)
            {
                sortedItems.Add(item);
            }
        }

        if (sortedItems.Count != 0)
        {
            return GetClosestItem(sortedItems);
        }
        else
        {
            return null;
        }
    }

    private Item GetClosestItem(List<Item> items)
    {
        var closestItem = items[0];
        var lastFoundClosestDistance = float.MaxValue;

        for (int i = 0; i < items.Count; i++)
        {
            var distance = Vector3.Distance(playerTransform.position, items[i].transform.position);
            if (lastFoundClosestDistance > distance)
            {
                lastFoundClosestDistance = distance;
                closestItem = items[i];
            }
        }

        return closestItem;
    }

    #endregion

    #region Interplay

    public void UpdateClosestInterplayObject()
    {
        var newFoundObject = GetClosestObject();
        if (newFoundObject != null)
        {
            if (newFoundObject != LastObject)
            {
                if (LastObject != null)
                {
                    LastObject.SetItemOutline(false);
                }

                newFoundObject.SetItemOutline(true);
                LastObject = newFoundObject;
            }
        }
        else
        {
            if (LastObject != null)
            {
                LastObject.SetItemOutline(false);
            }

            LastObject = null;
        }
    }

    private InterplayObject GetClosestObject()
    {
        var foundItems = Physics.OverlapSphere(center.position, radius);
        var sortedItems = new List<InterplayObject>();
        foreach (var foundItem in foundItems)
        {
            var item = foundItem.GetComponent<InterplayObject>();
            if (item && item.Interactable)
            {
                sortedItems.Add(item);
            }
        }

        if (sortedItems.Count != 0)
        {
            return GetClosestObject(sortedItems);
        }
        else
        {
            return null;
        }
    }

    private InterplayObject GetClosestObject(List<InterplayObject> items)
    {
        var closestItem = items[0];
        var lastFoundClosestDistance = float.MaxValue;

        for (int i = 0; i < items.Count; i++)
        {
            var distance = Vector3.Distance(playerTransform.position, items[i].transform.position);
            if (lastFoundClosestDistance > distance)
            {
                lastFoundClosestDistance = distance;
                closestItem = items[i];
            }
        }

        return closestItem;
    }

    #endregion
}