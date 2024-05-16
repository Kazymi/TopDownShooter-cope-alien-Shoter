using DG.Tweening;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class Wallet : MonoBehaviour
{
    [SerializeField] private Image moneyImage;
    [SerializeField] private TMP_Text moneyText;

    public static Wallet Instance;

    private void Awake()
    {
        Instance = this;
    }

    private int currentMoneyAmount;

    public void AddMoney(int addMoney)
    {
        currentMoneyAmount += addMoney;
        UpdateMoney();
    }

    private void UpdateMoney()
    {
        moneyText.SetText(currentMoneyAmount.ToString());
        moneyImage.transform.DOKill();
        moneyImage.transform.DOShakeScale(0.4f, 0.3f)
            .OnComplete(() => { moneyImage.transform.localScale = Vector3.one; });
    }
}