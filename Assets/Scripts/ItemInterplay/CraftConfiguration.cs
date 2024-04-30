using System;
using UnityEngine;

[Serializable]
public class CraftConfiguration
{
    [field: SerializeField] public Transform ObjectPosition { get; private set; }
    [field: SerializeField] public ItemType ItemType { get; private set; }

    [HideInInspector] public Item AttachItem;
}