using System;
using System.Collections;
using System.Collections.Generic;

[Serializable]
public class SaveData
{
    public int Money = 600;
    public bool IsFirstLaunch;

    public List<UpgradeHelper> UpgradeHelpers = new List<UpgradeHelper>()
    {
        new UpgradeHelper() {UpgradeType = UpgradeType.Speed, CurrentLevel = 0},
        new UpgradeHelper() {UpgradeType = UpgradeType.AttackDamage, CurrentLevel = 0},
    };

    public List<SaveStorage> SaveStorages = new List<SaveStorage>();

    public SaveStorage GetStorageByKey(string key)
    {
        foreach (var saveStorage in SaveStorages)
        {
            if (saveStorage.Key == key) return saveStorage;
        }

        return null;
    }
}

[Serializable]
public class SaveStorage
{
    public string Key;
    public GameSaveConfiguration SaveConfiguration;
}

[Serializable]
public class GameSaveConfiguration
{
    public bool IsNPC;
    public SavePositionConfiguration SavePositionConfiguration;
    public InventorySaveHelper InventorySaveHelper;
    public SaveHealthHelper SaveHealthHelper;
    public NPCSpawnSaveHelper NPCSpawnSaveHelper;
}

[Serializable]
public class UpgradeHelper
{
    public UpgradeType UpgradeType;
    public int CurrentLevel;
}