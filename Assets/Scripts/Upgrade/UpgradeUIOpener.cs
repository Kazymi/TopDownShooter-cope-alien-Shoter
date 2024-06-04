using System.Linq;
using UnityEngine;

public class UpgradeUIOpener : MonoBehaviour
{
    [SerializeField] private PlayerTriggerZone playerTriggerZone;
    [SerializeField] private Canvas upgradeCanvas;
    [SerializeField] private UpgradePannel[] upgradePannels;

    private void OnEnable()
    {
        playerTriggerZone.OnTriggerCompleted += Open;
        playerTriggerZone.OnPlayerLeftZone += Close;
        ServiceLocator.Subscribe<UpgradeUIOpener>(this);
    }

    private void OnDisable()
    {
        playerTriggerZone.OnTriggerCompleted -= Open;
        playerTriggerZone.OnPlayerLeftZone -= Close;
        ServiceLocator.UnSubscribe<UpgradeUIOpener>();
    }

    public UpgradeConfiguration GetUpgradeConfigurationByType(UpgradeType upgradeType)
    {
        return upgradePannels.Where(t => t.UpgradeConfiguration.UpgradeType == upgradeType).ToList()[0]
            .UpgradeConfiguration;
    }

    private void Open()
    {
        upgradeCanvas.enabled = true;
        foreach (var upgradePannel in upgradePannels)
        {
            upgradePannel.UIUpdate();
        }
    }

    private void Close()
    {
        upgradeCanvas.enabled = false;
    }
}