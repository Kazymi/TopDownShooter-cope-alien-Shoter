using System.Linq;
using UnityEngine;

public class ItemSpriteStorage : MonoBehaviour
{
    [SerializeField] private ItemSpriteConfiguration[] spriteConfigurations;

    public static ItemSpriteStorage Instance;

    private void Awake()
    {
        Instance = this;
    }

    public Sprite GetSpriteByType(ItemType itemType)
    {
        return spriteConfigurations.Where(t => t.ItemType == itemType).ToList()[0].Sprite;
    }
}