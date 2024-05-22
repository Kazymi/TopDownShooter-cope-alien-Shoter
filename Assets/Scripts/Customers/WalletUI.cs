using DG.Tweening;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class WalletUI : MonoBehaviour
{
    [SerializeField] private Image moneyImage;
    [SerializeField] private TMP_Text moneyText;

    private SaveData saveData;

    private void OnEnable()
    {
        ServiceLocator.Subscribe<WalletUI>(this);
    }

    private void OnDisable()
    {
        ServiceLocator.UnSubscribe<WalletUI>();
    }

    private void Start()
    {
        UpdateMoney();
    }

    public void UpdateMoney()
    {
        saveData ??= ServiceLocator.GetService<SaveData>();
        moneyText.SetText(saveData.Money.ToString());
        moneyImage.transform.DOKill();
        moneyImage.transform.DOShakeScale(0.4f, 0.3f)
            .OnComplete(() => { moneyImage.transform.localScale = Vector3.one; });
    }
}

public class Wallet
{
    private readonly SaveData saveData;
    private WalletUI walletUi;
    private Bootstrap bootstrap;

    public Wallet(SaveData saveData)
    {
        this.saveData = saveData;
    }

    public void AddMoney(int money)
    {
        walletUi ??= ServiceLocator.GetService<WalletUI>();
        bootstrap ??= ServiceLocator.GetService<Bootstrap>();
        saveData.Money += money;
        walletUi.UpdateMoney();
        bootstrap.Save();
    }

    public void ReduceMoney(int money)
    {
        walletUi ??= ServiceLocator.GetService<WalletUI>();
        bootstrap ??= ServiceLocator.GetService<Bootstrap>();
        saveData.Money -= money;
        if (saveData.Money < 0)
        {
            saveData.Money = 0;
        }

        walletUi.UpdateMoney();
        bootstrap.Save();
    }
}