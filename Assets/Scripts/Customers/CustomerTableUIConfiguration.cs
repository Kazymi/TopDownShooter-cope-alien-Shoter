using System;
using UnityEngine;
using UnityEngine.UI;

[Serializable]
public class CustomerTableUIConfiguration
{
    [field: SerializeField] public Image Image { get; private set; }
    [field: SerializeField] public GameObject CheckMark { get; private set; }
}