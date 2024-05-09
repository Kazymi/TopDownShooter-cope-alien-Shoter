using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;

public class CandyTree : Damageable
{
    [SerializeField] private Ease ease;
    [SerializeField] private Transform spawnPosition;
    [SerializeField] private Transform floorPosition;
    [SerializeField] private Vector2 size;

    private List<float> damagePercent = new List<float>();

    protected override void OnAwake()
    {
        damagePercent.Add(35f);
        damagePercent.Add(68f);
        damagePercent.Add(0f);
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
                SpawnCandy();
                damagePercent.Remove(damagePercent[i]);
                if (damagePercent.Count != 0)
                {
                    UpdatePercent(currentPercent);
                }

                break;
            }
        }
    }

    private void SpawnCandy()
    {
        var randomX = Random.Range(-size.x / 2f, size.x / 2f);
        var randomZ = Random.Range(-size.y / 2f, size.y / 2f);
        var newPosition = floorPosition.position + new Vector3(randomX, 0, randomZ);
        var newCandy = ItemSpawner.Instance.GetItemByType(ItemType.Candy);
        newCandy.transform.position = spawnPosition.position;
        newCandy.transform.rotation = Random.rotation;
        var sequence = DOTween.Sequence();
        newCandy.IsSelectable = false;
        sequence.Append(newCandy.transform.DORotate(Vector3.zero, 0.9f));
        sequence.Join(newCandy.transform.DOJump(newPosition, 1.8f, 1, 0.9f).SetEase(ease));
        sequence.Append(newCandy.transform.DOShakeScale(0.4f, 0.3f));
        sequence.Append(DOVirtual.DelayedCall(0.2f, () => { newCandy.IsSelectable = true; }));

    }

    private void OnDrawGizmos()
    {
        if (floorPosition == null) return;
        Gizmos.color = Color.blue;
        Gizmos.DrawWireCube(floorPosition.position, new Vector3(size.x, 0.2f, size.y));
    }
}