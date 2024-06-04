using System.Collections;
using System.Collections.Generic;
using System.Linq;
using DG.Tweening;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class UpgradePannel : MonoBehaviour
{
    [SerializeField] private UpgradeConfiguration upgradeConfiguration;
    [SerializeField] private TMP_Text priceText;
    [SerializeField] private TMP_Text levelText;
    [SerializeField] private Image slider;
    [SerializeField] private Button upgradeButton;

    private SaveData saveData;
    private Wallet wallet;

    public UpgradeConfiguration UpgradeConfiguration => upgradeConfiguration;

    private int price;
    private int level;

    private void OnEnable()
    {
        upgradeButton.onClick.AddListener(OnClick);
    }

    private void OnDisable()
    {
        upgradeButton.onClick.RemoveListener(OnClick);
    }

    public void UIUpdate()
    {
        saveData ??= ServiceLocator.GetService<SaveData>();
        var saveHelper =
            saveData.UpgradeHelpers.Where(t => t.UpgradeType == upgradeConfiguration.UpgradeType).ToList()[0];

        var priceResult = upgradeConfiguration.Cost;
        priceResult += (int) (saveHelper.CurrentLevel * upgradeConfiguration.CostFactor * priceResult);
        price = priceResult;
        level = saveHelper.CurrentLevel + 1;
        var isLevelMax = level >= upgradeConfiguration.MaxLevel;
        levelText.text = $"level {level}";
        if (isLevelMax)
        {
            slider.fillAmount = 1;
            priceText.SetText("Max");
            upgradeButton.interactable = false;
        }
        else
        {
            var fillResult = 1f / upgradeConfiguration.MaxLevel * (level - 1);
            slider.DOKill();
            slider.DOFillAmount(fillResult, 0.6f);

            priceText.SetText(price.ToString());
            upgradeButton.interactable = saveData.Money >= price;
        }
    }

    private void OnClick()
    {
        saveData ??= ServiceLocator.GetService<SaveData>();
        wallet ??= ServiceLocator.GetService<Wallet>();

        if (saveData.Money >= price)
        {
            var saveHelper =
                saveData.UpgradeHelpers.Where(t => t.UpgradeType == upgradeConfiguration.UpgradeType).ToList()[0];

            saveHelper.CurrentLevel++;
            wallet.ReduceMoney(price);
            UIUpdate();
        }
    }
}