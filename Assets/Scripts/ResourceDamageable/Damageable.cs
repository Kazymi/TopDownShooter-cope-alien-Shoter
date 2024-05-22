using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Damageable : MonoPooled, IDamageable
{
    [SerializeField] private DamageableType damageableType;
    [SerializeField] protected int maxHealth;

    protected int currentHealth;

    public int CurrentHealth => currentHealth;

    private void Awake()
    {
        currentHealth = maxHealth;
        OnAwake();
        ReturnedToPool += () =>
        {
            ItemSpawner.Instance.SpawnNewDamageableItem(damageableType, transform.position, transform.rotation);
        };
    }

    protected virtual void OnAwake()
    {
    }

    public override void Initialize()
    {
        currentHealth = maxHealth;
        gameObject.SetActive(true);
    }

    public virtual void TakeDamage(int damage)
    {
        currentHealth -= damage;
        HealthUpdated();
    }

    public void UpdateCurrentHealth(int health)
    {
        currentHealth = health;
    }
    protected virtual void HealthUpdated()
    {
        if (currentHealth > 0)
        {
            return;
        }

        ReturnToPool();
    }
}

public enum DamageableType
{
    CandyTree,
    Pudu,
}