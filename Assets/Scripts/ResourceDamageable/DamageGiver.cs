using System.Linq;
using UnityEngine;

public class DamageGiver : MonoBehaviour
{
    [SerializeField] private AudioClip audioClip;
    [SerializeField] private int damage;

    [Header("Attack configuration")] [SerializeField]
    private float radius;

    private SaveData saveData;
    private UpgradeConfiguration upgradeConfiguration;

    private AudioSource gameSource;

    [SerializeField] private Transform damageCenter;

    public void GiveDamage()
    {
        saveData ??= ServiceLocator.GetService<SaveData>();
        upgradeConfiguration ??=
            ServiceLocator.GetService<UpgradeUIOpener>().GetUpgradeConfigurationByType(UpgradeType.AttackDamage);
        var saveHelper = saveData.UpgradeHelpers.Where(t => t.UpgradeType == UpgradeType.AttackDamage).ToList()[0];
        var fixDamage = saveHelper.CurrentLevel * upgradeConfiguration.LevelProgressFactor * damage;
Debug.Log(fixDamage);

        gameSource ??= GameObject.FindWithTag("GameSound").GetComponent<AudioSource>();
        var foundItem = Physics.OverlapSphere(damageCenter.position, radius)
            .Where(t => t.GetComponent<IDamageable>() != null).ToList();
        foreach (var t in foundItem)
        {
            t.GetComponent<IDamageable>().TakeDamage(damage + (int) fixDamage);
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