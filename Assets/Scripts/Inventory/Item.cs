using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Outline))]
public class Item : MonoBehaviour
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
        IsSelectable = false;
    }

    public void ItemDeSelected()
    {
        IsSelectable = true;
    }
}

public enum ItemType
{
    Sushi,
    Peaches,
    Cookies,
    Clear,
}