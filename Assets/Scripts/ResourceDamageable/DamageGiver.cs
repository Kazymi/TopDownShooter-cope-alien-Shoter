using System.Linq;
using UnityEngine;

public class DamageGiver : MonoBehaviour
{
    [SerializeField] private AudioClip audioClip;
    [SerializeField] private int damage;

    [Header("Attack configuration")] [SerializeField]
    private float radius;

    private AudioSource gameSource;
    
    [SerializeField] private Transform damageCenter;

    public void GiveDamage()
    {
        gameSource ??= GameObject.FindWithTag("GameSound").GetComponent<AudioSource>();
        var foundItem = Physics.OverlapSphere(damageCenter.position, radius)
            .Where(t => t.GetComponent<IDamageable>() != null).ToList();
        foreach (var t in foundItem)
        {
            t.GetComponent<IDamageable>().TakeDamage(damage);
        }

        if (foundItem.Count != 0)
        {
         gameSource.PlayOneShot(audioClip);   
        }
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.gray;
        Gizmos.DrawWireSphere(damageCenter.position, radius);
    }
}