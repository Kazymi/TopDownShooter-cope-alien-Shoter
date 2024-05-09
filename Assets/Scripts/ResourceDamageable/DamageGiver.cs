using System.Linq;
using UnityEngine;

public class DamageGiver : MonoBehaviour
{
    [SerializeField] private int damage;

    [Header("Attack configuration")] [SerializeField]
    private float radius;

    [SerializeField] private Transform damageCenter;

    public void GiveDamage()
    {
        var foundItem = Physics.OverlapSphere(damageCenter.position, radius)
            .Where(t => t.GetComponent<IDamageable>() != null).ToList();
        foreach (var t in foundItem)
        {
            t.GetComponent<IDamageable>().TakeDamage(damage);
        }
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.gray;
        Gizmos.DrawWireSphere(damageCenter.position, radius);
    }
}