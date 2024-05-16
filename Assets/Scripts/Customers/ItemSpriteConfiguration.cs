using System;
using UnityEngine;

[Serializable]
public class ItemSpriteConfiguration
{
    [field: SerializeField] public Sprite Sprite { get; private set; }
    [field: SerializeField] public ItemType ItemType { get; private set; }
}