using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NPCSaveSpawner : MonoBehaviour
{
    private void Start()
    {
        ServiceLocator.GetService<Bootstrap>().OnLoad += Load;
    }

    private void OnDestroy()
    {
        ServiceLocator.GetService<Bootstrap>().OnLoad -= Load;
    }

    public void Load()
    {
        var saveData = ServiceLocator.GetService<SaveData>();
        var itemSpawner = ServiceLocator.GetService<ItemSpawner>();

        var npc = new List<SaveStorage>();
        foreach (var saveStorage in saveData.SaveStorages)
        {
            var isSaveNpc = saveStorage.SaveConfiguration.IsNPC;
            var helper = saveStorage.SaveConfiguration.NPCSpawnSaveHelper;
            if (isSaveNpc)
            {
                npc.Add(saveStorage);
                var npcType = itemSpawner.SpawnNewDameableItem(helper.DamageableType);
                npcType.GetComponent<SaveHelper>().LoadFromKey(saveStorage.Key);
            }
        }

        foreach (var nps in npc)
        {
            saveData.SaveStorages.Remove(nps);
        }
    }
}