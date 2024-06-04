using System;
using UnityEngine;

[Serializable]
public class UpgradeConfiguration
{
    [field: SerializeField] public int Cost { get; private set; }
    [field: SerializeField] public int MaxLevel { get; private set; }
    [field: SerializeField] public float CostFactor { get; private set; }

    [field: SerializeField] public float LevelProgressFactor { get; private set; }
    [field: SerializeField] public UpgradeType UpgradeType { get; private set; }
}