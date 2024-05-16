using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;

[RequireComponent(typeof(ItemDropper))]
public class CandyTree : Damageable
{
    [SerializeField] private Ease ease;
    [SerializeField] private Transform spawnPosition;
    [SerializeField] private Transform floorPosition;
    [SerializeField] private Vector2 size;

    private List<float> damagePercent = new List<float>();
    private ItemDropper itemDropper;

    protected override void OnAwake()
    {
        damagePercent = new List<float> {35f, 68f, 0f};
    }

    public override void Initialize()
    {
        base.Initialize();
        damagePercent = new List<float> {35f, 68f, 0f};
    }

    protected override void HealthUpdated()
    {
        base.HealthUpdated();
        var currentPercent = (float) currentHealth / maxHealth;
        currentPercent *= 100f;
        UpdatePercent(currentPercent);
    }

    private void UpdatePercent(float currentPercent)
    {
        for (int i = 0; i < damagePercent.Count; i++)
        {
            if (damagePercent[i] >= currentPercent)
            {
                itemDropper??=GetComponent<ItemDropper>();
                itemDropper.DropItem();
                damagePercent.Remove(damagePercent[i]);
                if (damagePercent.Count != 0)
                {
                    UpdatePercent(currentPercent);
                }

                break;
            }
        }
    }
}