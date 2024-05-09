using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Outline))]
public class Item : MonoPooled
{
    [field: SerializeField] public ItemType ItemType { get; private set; }
    [field: SerializeField] public Vector3 InventoryRotation { get; private set; }

    public bool IsSelectable = true;

    private Outline outline;

    private void Awake()
    {
        outline = GetComponent<Outline>();
        outline.enabled = false;
    }

    public void SetItemOutline(bool isOutline)
    {
        outline.enabled = isOutline;
    }

    public void ItemSelected()
    {
        gameObject.layer = 6;
        IsSelectable = false;
    }

    public void ItemDeSelected()
    {
        gameObject.layer = 0;
        IsSelectable = true;
    }
}

public enum ItemType
{
    Sushi,
    Peaches,
    Cookies,
    Clear,
    Candy,
}